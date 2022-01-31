using System;
using System.Collections.Generic;
using System.Text;

namespace AutoTrading.Lib
{
    class SubAccount
    {
        public double CashBalance; //Account cash balance
        public string Currency; //Currency string
        public int DayTradesRemaining; //Number of day trades left
        public double EquityWithLoanValue; //Equity with Loan Value
        public double InitMarginReq; //Current initial margin requirement
        public double LongOptionValue; //Long option value
        public double MaintMarginReq; //Current maintenance margin
        public double NetLiquidation; //Net liquidation value
        public double OptionMarketValue; //Option market value
        public double ShortOptionValue; //Short option value
        public double StockMarketValue; //Stock market value
        public double UnalteredInitMarginReq; //Overnight initial margin requirement
        public double UnalteredMaintMarginReq; //Overnight maintenance margin requirement

    }
}
