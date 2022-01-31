using System;
using System.Threading;
using com.ib.client;

namespace CJava.Lib
{
    class MyStrategy:Strategy
    {
        
        public MyStrategy()
        {
            name = "My";
            //marketDataWH = new AutoResetEvent(false);
            marketStartTime = new TimeSpan(9, 30, 0);//9:30
            marketEndTime = new TimeSpan(16, 0, 0);

            tradeStartTime = new TimeSpan(9, 31, 0);
            tradeEndTime = new TimeSpan(15, 59, 0);

            //tradeStartTime = TimeSpan.FromHours(9.5) + TimeSpan.FromMinutes(1);
            //tradeEndTime = TimeSpan.FromHours(16) - TimeSpan.FromMinutes(1);
            historyDurationDays = TimeSpan.FromDays(1);
            historyStratTime = DateTime.Now.Date - historyDurationDays;

            CreateContract();
            tickerID = Client.TickerID;
            stock = new Stock(tickerID, contract, historyStratTime, marketStartTime, marketEndTime);
            //List<Bar> barList = stock.AddBarHistory(DateTime.Now, TimeSpan.FromMinutes(1));
            stock.AddBarHistory(TimeSpan.FromMinutes(1));
            stock.BarHistoryList[TimeSpan.FromMinutes(1)].AddIndicator(new IndicatorA());
            
            stock.AddBarHistory(TimeSpan.FromMinutes(10));
            stock.BarHistoryList[TimeSpan.FromMinutes(10)].AddIndicator(new HLRate());
           // stock.BarHistoryList[TimeSpan.FromMinutes(1)].AddIndicator(new HLRate());
           
            stock.AfterPriceUpdate += Decision;

            //stock.ReqHistoricalData(TimeSpan.FromSeconds(1));
            stock.ReqHistoricalData(TimeSpan.FromMinutes(1));

            stock.ReqMarketData();
            //Client.clientSocket.reqMktData(tickerID, contract, "100,101,104,106,162,165,221,225");
            //Client.clientSocket.reqMktData(1, contract, "100,101,104,106,162,165,221,225");
            //AddDecision();

        }
        override public void CreateContract()
        {
            contract = new Contract();
            contract.m_symbol = "YHOO";
            contract.m_secType = "STK";
            contract.m_expiry = "";
            contract.m_strike = 0;
            contract.m_exchange = "SMART";
            //Client.clientSocket.reqMktData(TickerID, m_contract, "100,101,104,106,162,165,221,225");
            //m_client.reqHistoricalData(5, m_contract, "20061220 16:00:00", "1 D", "1 day", "BID_ASK", 1, 1);

        }
        public new void Decision()
        {
            base.Decision();
        }
    }
}
