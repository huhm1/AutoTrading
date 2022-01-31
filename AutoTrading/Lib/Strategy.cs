using IBApi;
using System;
using System.Threading;

namespace AutoTrading.Lib
{
    abstract public class Strategy
    {
        public double Cash = 100000;
        public double difference = 0.01;
        public double Commission = 0.005;
        public decimal HoldSize = 0;
        public bool ColseAtEnd = false;
        public Portfolio portfolio;
        public DateTime TradeTime = DateTime.MinValue;


        public int status = 0;
        public string name;
        public bool IsBackTest = true;
        public bool Ready = true;
        public Stock stock;
        public Contract contract;
        public int tickerID;
        public TimeSpan marketStartTime, marketEndTime;
        public TimeSpan tradeStartTime, tradeEndTime;
        public TimeSpan historySpanDays;
        public int currentOrderID = 0;
        //public DateTime historyStratDate;

        public EventWaitHandle marketDataWH;


        protected Strategy()
        {
            //throw new NotImplementedException();
        }

        public void CreateContract(string symbol, string secTye = "STK", string exchange = "SMART", string currency = "USD")
        {
            contract = new Contract
            {
                Symbol = symbol,
                SecType = secTye,
                Exchange = exchange,
                Currency = currency
            };

        }
        abstract public void ResetHighLow();
        abstract public void Close();
        abstract public void Settle(decimal filled, decimal remaining, double avgFillPrice);

        public void Decision()
        {
        }

        public void Buy(double price, decimal size, string type)
        {
            double dp = Commission + difference;
            stock.Buy(size, price + difference, type, Cash + (double)HoldSize * price - (double)(Math.Abs(HoldSize) + size) * dp, this);
        }

        public void Sell(double price, decimal size, string type)
        {
            double dp = Commission + difference;
            stock.Sell(size, price - difference, type, Cash + (double)HoldSize * price - (double)(Math.Abs(HoldSize) + size) * dp, this);
        }

        public void Buy(double price, double spend, string type)
        {
            decimal size = ((int)(spend / (price + Commission + difference) / 100.0)) * 100;//less size
            Buy(price, size, type);
        }

        public void Sell(double price, double spend, string type)
        {
            decimal size = ((int)(spend / price / 100.0)) * 100;//less size
            Sell(price, size, type);
        }

        public void Buy(double price, string type)
        {
            if (HoldSize == 0)
                Buy(price, Cash, type);
            else if (HoldSize < 0)
                Buy(price, -HoldSize, type);
        }
        public void Sell(double price, string type)
        {
            if (HoldSize == 0)
                Sell(price, Cash, type);
            else if (HoldSize > 0)
                Sell(price, HoldSize, type);
        }
        public void BuyAll(double price, string type)
        {
            Buy(price, Cash, type);
        }
        public void SellAll(double price, string type)
        {
            if (HoldSize == 0)
                Sell(price, Cash, type);
            else if (HoldSize > 0)
                Sell(price, (double)HoldSize * 2 * price + Cash, type);/// not complete?????
        }

    }
}
