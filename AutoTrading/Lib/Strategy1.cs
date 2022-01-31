using System;
using System.Collections.Generic;
using System.Text;
//using com.ib.client;


namespace CJava.Lib
{
    class Strategy1:Strategy
    {
        public Strategy1()
        {
            CreateContract();
            stock = new Stock(tickerID, contract);
            stock.AddBarHistory(TimeSpan.FromDays(1));
            stock.AddBarHistory(TimeSpan.FromHours(1));
            stock.BarHistoryList[TimeSpan.FromDays(1)].AddIndicator("a", new IndicatorA());
            Client.clientSocket.reqMktData(TickerID, contract, "100,101,104,106,162,165,221,225");
            AddDecision();

        }
        override public void CreateContract()
        {
            contract = new Contract();
            contract.m_symbol = "qqqq";
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


        public override void PriceUpdate(object o, EventArgs e)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
