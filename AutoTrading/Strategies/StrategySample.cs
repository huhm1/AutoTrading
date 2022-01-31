#define Test
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Threading;
using IBApi;

namespace AutoTrading.Lib
{
    class StrategySample : Strategy
    {
        public int np = 0;
        private Double K = 0.12;

        //TimeSpan LongPeriod = new TimeSpan(3,15,0);
        TimeSpan LongPeriod = TimeSpan.FromDays(1);
        TimeSpan ShortPeriod = TimeSpan.FromMinutes(1);
        private Logs Result;
        private Logs TradeRecord;
        /// <summary>
        /// K is a base rate
        /// </summary>

        private DateTime PeriodTimeStamp = DateTime.MinValue;
        double PeriodHigh = 0, PeriodLow = double.MaxValue;
        double HighWhenOder = 0, LowWhenOder = double.MaxValue;


        public StrategySample()
        {
            name = "Test";
            CreateContract("MSFT");
            Load();
#if Test
            TradeRecord = new Logs("Test_" + contract.Symbol + "_T.txt", true);
#else
            TradeRecord = new Logs(contract.m_symbol + "_T.txt", true);
#endif

            marketStartTime = new TimeSpan(9, 30, 0); //9:30
            marketEndTime = new TimeSpan(16, 0, 0);

            tradeStartTime = new TimeSpan(9, 31, 30);
            tradeEndTime = new TimeSpan(15, 58, 30);

            historySpanDays = TimeSpan.FromDays(200); ///////////////

            tickerID = Client.TickerID;
            stock = new Stock(this, tickerID, contract, historySpanDays, marketStartTime, marketEndTime);

            stock.AddBarHistory(LongPeriod);
            stock.BarHistoryList[LongPeriod].AddIndicator(new HLRate());
            status = 0;

#if !Test
            Ready = false;
#endif
            stock.AfterPriceUpdate += Decision;
            Thread t = new Thread(delegate () { stock.ReqData(TimeSpan.FromSeconds(15)); });
            t.Start();

#if !Test
            ResetHighLow();
            if (Account.PortfolioList.TryGetValue(contract.m_symbol + " " + contract.m_secType, out portfolio))
            {
                if (HoldSize != portfolio.Position)
                {
                    Cash -= (portfolio.Position - HoldSize) * portfolio.AverageCost;
                    HoldSize = portfolio.Position;
/*
                    if (HoldSize == 0)
                    {
                        HoldSize = portfolio.Position;
                        Cash -= HoldSize*portfolio.AverageCost;
                    }
                    else
                    {
                        Cash -= (portfolio.Position-HoldSize) * portfolio.AverageCost;
                        HoldSize = portfolio.Position;
                    }
*/
                }
            }
            //status = 0;
#endif
        }

        public override void ResetHighLow()
        {
            PeriodHigh = 0;
            PeriodLow = double.MaxValue;
        }
        public override void Settle(decimal filled, decimal remaining, double avgFillPrice)
        {
            if (status == 1 || status == 101)
            {
                HoldSize += filled;
                Cash -= (double)filled * (avgFillPrice + Commission);
                TradeRecord.WriteT("Bought, " + filled + ", " + avgFillPrice + ", " + (double)filled * avgFillPrice + ", " + Cash);
            }
            if (status == 2 || status == 102)
            {
                HoldSize -= filled;
                Cash += (double)filled * (avgFillPrice - Commission);
                TradeRecord.WriteT("Sold, " + filled + ", " + avgFillPrice + ", " + (double)HoldSize * avgFillPrice + ", " + Cash);
            }
            ResetHighLow();
            status = 0;
#if !Test
            Result = new Logs(Client.StrategyPath, name + "_Strategy.csv", false);
            Result.Write(Cash.ToString("f2")+","+HoldSize+","+K+",false");
            Result.Close();
#endif
        }


        protected void Load()
        {
            string fileNameStr = name + "_Strategy.csv";
            OleDbConnection ExcelConnection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Client.StrategyPath + @"; Extended Properties='Text;HDR=No;FMT=Delimited'");
            OleDbCommand ExcelCommand = new OleDbCommand(@"SELECT * FROM " + fileNameStr, ExcelConnection);
            OleDbDataReader reader;

            try
            {
                ExcelConnection.Open();
                reader = ExcelCommand.ExecuteReader();
                if (reader.Read())
                {
                    Cash = (Double)reader[0];
                    HoldSize = (Int32)reader[1];
                    K = (Double)reader[2];
                    ColseAtEnd = Convert.ToBoolean(reader[3]);
                }

            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
                ExcelConnection.Close();
            }
            ExcelConnection.Close();

        }

        public new void Decision()
        {
            if (stock.PriceDateTime.TimeOfDay >= marketStartTime && stock.PriceDateTime.TimeOfDay <= marketEndTime && Ready)
            {
                List<Bar> LongBars = stock.BarHistoryList[LongPeriod].BarList;
                HLRate hLRate = new HLRate();
                hLRate = (HLRate)stock.BarHistoryList[LongPeriod].IndicatorLists[hLRate.Name];
                int n = LongBars.Count;
                if (n <= 1) return;
                n -= 1;
                double r = hLRate.HLR[n - 1] * K;
                double hl = hLRate.HL[n - 1] * K;

#if Test
                if (PeriodTimeStamp != LongBars[n].D.Date)
                {
                    PeriodHigh = 0;
                    PeriodLow = double.MaxValue;
                    PeriodTimeStamp = LongBars[n].D.Date;
                }
#endif
                double last = stock.last;
                if (last > PeriodHigh) PeriodHigh = last;
                if (last < PeriodLow) PeriodLow = last;
                if (status != 0)
                {
                    if (status == 1 && last > LowWhenOder + 0.05 || status == 2 && last < HighWhenOder - 0.05)
                    {
                        Thread newThread = new Thread(stock.CancelOrder);
                        newThread.Start();
                        status += 100;
                    }
                    return;
                }

                if (PeriodHigh - last >= hl && HoldSize == 0 && TradeTime != stock.PriceDateTime)
                {
                    LowWhenOder = last;
                    int size = ((int)(Cash / (last + Commission + 0.01) / 100.0)) * 100;
                    double cost = size * (last + Commission + 0.01);
                    stock.Buy(size, last + 0.01, "MKT", Cash - cost + size * last, this);//?????
                    TradeTime = stock.PriceDateTime;
                    np = n;
                    return;
                }

                if (last - PeriodLow >= hl && HoldSize != 0 && TradeTime != stock.PriceDateTime)
                {
                    HighWhenOder = last;
                    double cost = (double)HoldSize * (last - Commission - 0.01);
                    status = 2;
                    stock.Sell(HoldSize, last - 0.01, "MKT", Cash + cost, this);
                    TradeTime = stock.PriceDateTime;

                    np = n;

                }
            }
        }

        public override void Close()
        {
            throw new NotImplementedException();
        }
    }
}
