using IBApi;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Threading;

namespace AutoTrading.Lib
{
    public class Stock
    {
        static object locker = new object();
        public Strategy strategy = null;
        public int LastOrderID;
        public EventWaitHandle cancelOrderWH = new AutoResetEvent(false);
        public static EventWaitHandle LoadDataWH = new AutoResetEvent(true);

        static DateTime DataReqTime = DateTime.MinValue;

        public bool DataFromFile = false;
        public bool RealTime = false;


        public Logs TradeRecord;
        public Logs HistoryCVS;
        public DateTime PriceDateTime = DateTime.MinValue;

        public delegate void PriceChangeHandler();
        public event PriceChangeHandler OnPriceChange;
        public delegate void VolumeChangeHandler();
        public event VolumeChangeHandler OnVolumeChange;

        //public delegate void HistoricalDataHandler(DateTime dateTime, double open, double high, double low, double close, int volume);
        public delegate void HistoricalDataHandler(DateTime dateTime);
        public event HistoricalDataHandler OnHistoricalData;


        public delegate void AfterPriceUpdateHandler();
        public event AfterPriceUpdateHandler AfterPriceUpdate;

        public static Dictionary<int, Stock> StockList = new Dictionary<int, Stock>();
        public Dictionary<TimeSpan, BarHistory> BarHistoryList;
        //public List<string> names = new List<string>();
        public static Dictionary<int, OrderStatus> OrderList = new Dictionary<int, OrderStatus>();

        public Contract contract;
        public int TickerID;
        public double open = 0, high = 0, low = double.MaxValue, last = 0, bid = 0, ask = double.MaxValue;
        public decimal volume, lastSize, bidSize, askSize;

        public TimeSpan marketStartTime, marketEndTime;
        public TimeSpan marketTimeSpan;
        public DateTime historyStratDate;
        public TimeSpan ReqHistoricalInterval;
        public bool HistoricalDataReqCompleted = true;

        public bool ReqHistoricalError = false;
        public int ErrorNumber = 0;

        public EventWaitHandle HistoricalDataWH;
        public EventWaitHandle MarketDataWH;

        private DateTime PeriodTimeStamp = DateTime.MinValue;


        public Stock(Strategy strategy, int reqId, Contract contract, TimeSpan historySpanDays, TimeSpan start, TimeSpan end)
        {
            this.strategy = strategy;
            TradeRecord = new Logs(contract.Symbol + ".txt", false);
            HistoricalDataWH = new AutoResetEvent(false);
            MarketDataWH = new AutoResetEvent(false);
            TickerID = reqId;
            this.contract = contract;
            historyStratDate = DateTime.Now.Date - historySpanDays;
            while (historyStratDate.DayOfWeek == DayOfWeek.Saturday || historyStratDate.DayOfWeek == DayOfWeek.Sunday)
                historyStratDate += TimeSpan.FromDays(1);
            marketStartTime = start;
            marketEndTime = end;
            marketTimeSpan = end - start;
            BarHistoryList = new Dictionary<TimeSpan, BarHistory>();
            StockList.Add(reqId, this);
        }

        public void ReqMarketData()
        {
            if (Client.Socket.IsConnected())
            {
                Client.Socket.reqMktData(TickerID, contract, null, false, false, null);
                RealTime = true;
            }
            //http://www.interactivebrokers.com/en/software/apiReleaseNotes/api9.php?ib_entity=uk#9_ticks
        }


        public void ReqData(TimeSpan interval)
        {
            ReqHistoricalData(interval);
#if !Test
            strategy.Ready = true;
            ReqMarketData();
#endif
        }

        public void ReqHistoricalData(TimeSpan interval)
        {
            TimeSpan sleepTime;
            DateTime reqEndDateTime = historyStratDate;
            DataFromFile = false;

            ReqHistoricalInterval = interval;
            string fileNameStr = contract.Symbol + "_" + interval.TotalSeconds + ".csv";

            if (File.Exists(Client.FilePath + fileNameStr))
            {
                DataFromFile = true;
                LoadHistoricalData(fileNameStr);
                reqEndDateTime = PriceDateTime + ReqHistoricalInterval;
                if (reqEndDateTime < historyStratDate)///mabe need roll back
                    reqEndDateTime = historyStratDate;
            }

            if (!Client.Connected)
                return;
            HistoryCVS = new Logs(fileNameStr, true);
            HistroyDuration historyDuration = new HistroyDuration(interval, marketStartTime, marketEndTime);
            TimeSpan duration = historyDuration.Duration;
            string durationStr = historyDuration.DurationStr;
            if (duration <= TimeSpan.FromDays(1))
            {
                TimeSpan span = duration;
                while (marketTimeSpan < span)
                {
                    reqEndDateTime += TimeSpan.FromDays(1);
                    span -= marketTimeSpan;
                    while (reqEndDateTime.DayOfWeek == DayOfWeek.Saturday ||
                           reqEndDateTime.DayOfWeek == DayOfWeek.Sunday)
                        reqEndDateTime += TimeSpan.FromDays(1);
                }
                if (DataFromFile)
                    reqEndDateTime += span;
                else
                    reqEndDateTime = reqEndDateTime.Date + marketStartTime + span;
                if (reqEndDateTime.TimeOfDay > marketEndTime)
                {
                    reqEndDateTime = reqEndDateTime.Date + TimeSpan.FromDays(1) + reqEndDateTime.TimeOfDay -
                                     marketTimeSpan;
                    while (reqEndDateTime.DayOfWeek == DayOfWeek.Saturday ||
                           reqEndDateTime.DayOfWeek == DayOfWeek.Sunday)
                        reqEndDateTime += TimeSpan.FromDays(1);
                }
            }
            else
            {
                reqEndDateTime = reqEndDateTime.Date + marketEndTime + duration;
            }

            do
            {
                HistoricalDataReqCompleted = false;
                ReqHistoricalError = false;
                ErrorNumber = 0;
                if (reqEndDateTime > DateTime.Now + TimeSpan.FromHours(1))
                    reqEndDateTime = DateTime.Now + TimeSpan.FromHours(1);

                sleepTime = (TimeSpan.FromSeconds(10) - (DateTime.Now - DataReqTime)); ///
                while (sleepTime.Ticks > 0)
                {
                    Thread.Sleep(sleepTime);
                    sleepTime = (TimeSpan.FromSeconds(10) - (DateTime.Now - DataReqTime));
                }
                lock (locker)
                    DataReqTime = DateTime.Now;
                lock (locker)
                {
                    Client.Logs.WriteT(TickerID + " " + contract + " " + reqEndDateTime.ToString("yyyyMMdd HH:mm:ss") +
                                       " " + durationStr + " " + historyDuration.BarSizeStr + " " + DataReqTime);
                }
                //Client.Socket.reqHistoricalData(TickerID, contract, reqEndDateTime.ToString("yyyyMMdd HH:mm:ss"), durationStr, historyDuration.BarSizeStr, "TRADES", 1, 1, true, null);

                Client.Socket.reqHistoricalData(TickerID, contract, "", durationStr, historyDuration.BarSizeStr, "TRADES", 1, 1, true, null);
                if (ReqHistoricalError)
                {
                    switch (ErrorNumber)
                    {
                        case 162:
                            Thread.Sleep(TimeSpan.FromSeconds(5));
                            break;
                        case 165:
                            break;
                    }

                    continue;
                }

                if (reqEndDateTime >= DateTime.Now.Date + marketEndTime || reqEndDateTime >= DateTime.Now)
                {
                    HistoricalDataReqCompleted = true;
                    break;
                }
                else
                {
                    if (duration <= TimeSpan.FromDays(1))
                    {
                        TimeSpan span = duration;
                        while (marketTimeSpan < span)
                        {
                            reqEndDateTime += TimeSpan.FromDays(1);
                            span -= marketTimeSpan;
                            while (reqEndDateTime.DayOfWeek == DayOfWeek.Saturday ||
                                   reqEndDateTime.DayOfWeek == DayOfWeek.Sunday)
                                reqEndDateTime += TimeSpan.FromDays(1);
                        }
                        reqEndDateTime += span;
                        if (reqEndDateTime.TimeOfDay > marketEndTime)
                        {
                            reqEndDateTime = reqEndDateTime.Date + TimeSpan.FromDays(1) + reqEndDateTime.TimeOfDay -
                                             marketTimeSpan;
                            while (reqEndDateTime.DayOfWeek == DayOfWeek.Saturday ||
                                   reqEndDateTime.DayOfWeek == DayOfWeek.Sunday)
                                reqEndDateTime += TimeSpan.FromDays(1);
                        }
                    }
                    else
                    {
                        reqEndDateTime = reqEndDateTime.Date + marketEndTime + duration;
                    }
                }
            } while (true);

            /*
                        marketDataWH.Set();
            */
        }

        private void LoadHistoricalData(string fileNameStr)
        {
            LoadDataWH.WaitOne();
            OleDbConnection ExcelConnection = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Client.FilePath + "; Extended Properties='Text;HDR=No;FMT=Delimited'");
            OleDbCommand ExcelCommand = new OleDbCommand(@"SELECT * FROM " + fileNameStr, ExcelConnection);
            OleDbDataReader reader;

            try
            {
                ExcelConnection.Open();
                reader = ExcelCommand.ExecuteReader();
                while (reader.Read())
                {
                    //DateTime dt = Convert.ToDateTime(reader[0]);
                    //ew.historicalData(tickerID, Convert.ToDateTime(reader[0]).ToString("yyyyMMdd  HH:mm:ss"), Convert.ToDouble(reader[1]), Convert.ToDouble(reader[2]), Convert.ToDouble(reader[3]), Convert.ToDouble(reader[4]), Convert.ToInt32(reader[5]), Convert.ToInt32(reader[6]), Convert.ToDouble(reader[7]), Convert.ToBoolean(reader[8]));
                    HistoricalData((DateTime)reader[0], (Double)reader[1], (Double)reader[2], (Double)reader[3], (Double)reader[4], (Int32)reader[5]);
                }

            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
                ExcelConnection.Close();
            }
            ExcelConnection.Close();
            LoadDataWH.Set();

        }


        public List<Bar> AddBarHistory(TimeSpan interval)
        {
            BarHistory barHistory = new BarHistory(this, interval);
            BarHistoryList.Add(interval, barHistory);
            OnPriceChange += barHistory.PriceUpdate;
            OnVolumeChange += barHistory.VolumeUpdate;
            OnHistoricalData += barHistory.HistoryUpdate;
            return barHistory.BarList;
        }

        public Contract Contract(string symbol)
        {
            throw new NotImplementedException();
        }

        public static Stock GetStock(int reqId)
        {
            return StockList[reqId];
        }

        public void HistoricalData(DateTime date, double open, double high, double low, double close, decimal volume)
        {
            PriceDateTime = date - ReqHistoricalInterval;
            //Skip untrusted spike
            if (high - low > open * 0.008)
                return;
            if (AfterPriceUpdate != null)
            {
                if (open > close)
                {
                    UpdatePrice(open);
                    OnHistoricalData(PriceDateTime);
                    AfterPriceUpdate();
                    UpdatePrice(high);
                    OnHistoricalData(PriceDateTime);
                    UpdatePrice(low);
                    OnHistoricalData(PriceDateTime);
                    UpdatePrice(close);
                    OnHistoricalData(PriceDateTime);
                    AfterPriceUpdate();
                }
                else
                {
                    UpdatePrice(open);
                    OnHistoricalData(PriceDateTime);
                    AfterPriceUpdate();
                    UpdatePrice(low);
                    OnHistoricalData(PriceDateTime);
                    UpdatePrice(high);
                    OnHistoricalData(PriceDateTime);
                    UpdatePrice(close);
                    OnHistoricalData(PriceDateTime);
                    AfterPriceUpdate();
                }
            }
        }

        public void UpdatePrice(double price)
        {
            last = price;
            if (PeriodTimeStamp != PriceDateTime.Date)
            {
                high = 0;
                low = double.MaxValue;
                PeriodTimeStamp = PriceDateTime.Date;
            }

            if (last > high) high = last;
            if (last < low) low = last;
        }

        public static void UpdatePrice(int reqId, int tickType, double price, TickAttrib attribs)
        {
            //PriceDateTime = DateTime.Now;
            //Skip untrusted price
            if (price <= 0.9)
                return;
            Stock stock = StockList[reqId];
            stock.PriceDateTime = DateTime.Now;
            switch (tickType)
            {
                case TickType.OPEN:
                    stock.open = price;
                    return;
                case TickType.HIGH:
                    stock.high = price;
                    return;
                case TickType.LOW:
                    stock.low = price;
                    return;
                case TickType.LAST:
                    stock.last = price;
                    if (stock.OnPriceChange != null)
                    {
                        stock.OnPriceChange();
                    }
                    if (stock.AfterPriceUpdate != null)
                    {
                        stock.AfterPriceUpdate();
                    }
                    return;
                case TickType.BID:
                    if (price > stock.ask)
                        stock.bid = stock.last;
                    else
                        stock.bid = price;
                    return;
                case TickType.ASK:
                    if (price < stock.bid)
                        stock.ask = stock.last;
                    else
                        stock.ask = price;
                    return;
                default:
                    break;
            }

        }
        public static void UpdateSize(int reqId, int tickType, decimal size)
        {
            Stock stock = StockList[reqId];
            switch (tickType)
            {
                case TickType.VOLUME:
                    stock.volume = size;
                    if (stock.OnVolumeChange != null)
                    {
                        stock.OnVolumeChange();
                    }
                    return;
                case TickType.LAST_SIZE:
                    stock.lastSize = size;
                    return;
                case TickType.BID_SIZE:
                    stock.bidSize = size;
                    return;
                case TickType.ASK_SIZE:
                    stock.askSize = size;
                    return;
                default:
                    break;
            }


        }

        public void Buy(decimal size, double price, string type, double total, Strategy strategy)
        {
            if (RealTime)
            {
                Order order = new Order
                {
                    Action = "BUY",
                    TotalQuantity = size,
                    OutsideRth = false,
                    LmtPrice = price,
                    OrderType = type
                };
                strategy.status = 1;
                PlaceOrder(order, " Buy " + size + "," + price + ", " + total, strategy, 1);

            }
            else
            {
                strategy.status = 1;
                TradeRecord.Write(PriceDateTime.ToString("yyyy-MM-dd HH:mm:ss") + ", Bought, " + size + ", " + price + ", " + total);
                strategy.Settle(size, 0, price);
                strategy.ResetHighLow();
                strategy.status = 0;
            }

        }


        public void Sell(decimal size, double price, string type, double total, Strategy strategy)
        {
            if (RealTime)
            {
                Order order = new Order
                {
                    Action = "SELL",
                    TotalQuantity = size,
                    OutsideRth = false,
                    LmtPrice = price,
                    OrderType = type
                };
                strategy.status = 2;
                PlaceOrder(order, " Sell " + size + "," + price + ", " + total, strategy, 2);

            }
            else
            {
                strategy.status = 2;
                TradeRecord.Write(PriceDateTime.ToString("yyyy-MM-dd HH:mm:ss") + ", Sold, " + size + ", " + price + ", " + total);
                strategy.Settle(size, 0, price);
                strategy.ResetHighLow();
                strategy.status = 0;

            }
        }
        private void PlaceOrder(Order order, string message, Strategy strategy, int action)
        {
            LastOrderID = Client.OrderID;
            Client.Socket.placeOrder(LastOrderID, contract, order);
            strategy.currentOrderID = LastOrderID;
            OrderList.Add(LastOrderID, new OrderStatus(order, strategy, this, action));
            String msg = "Placed order " + LastOrderID + message;
            TradeRecord.WriteT(msg);
        }

        public void CancelOrder()
        {
            OrderStatus orderStatus = OrderList[LastOrderID];
            if (orderStatus.TWSstatus == "Submitted" || orderStatus.TWSstatus == "PendingSubmit")
            {
                Client.Socket.cancelOrder(LastOrderID);
                TradeRecord.WriteT("Cancel order1: " + LastOrderID);
            }
            else
            {
                if (orderStatus.TWSstatus == "PendingCancel" || orderStatus.TWSstatus == "Cancelled")
                    return;
                orderStatus.status += 100;
                Client.Socket.reqAllOpenOrders();
                cancelOrderWH.WaitOne();
                orderStatus.status -= 50;
                Client.Socket.cancelOrder(LastOrderID);
                TradeRecord.WriteT("Cancel order2: " + LastOrderID);
            }


        }
    }
}
