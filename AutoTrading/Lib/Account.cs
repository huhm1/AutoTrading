using IBApi;
using System;
using System.Collections.Generic;
using System.Threading;

namespace AutoTrading.Lib
{
    public class Account
    {
        public static EventWaitHandle reqAccountWH = new AutoResetEvent(false);

        public static bool PaperTrade = false;
        public static Dictionary<string, Portfolio> PortfolioList = new Dictionary<string, Portfolio>();
        public static Dictionary<string ,string > Keys = new Dictionary<string, string>();
        public double CashBalance; //Account cash balance
        public string  Currency; //Currency string
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
    
        public Account()
        {
            Client.Socket.reqAccountUpdates(true, "");
            reqAccountWH.WaitOne();
        }

        public static void UpdateAccountValue(string key, string value, string currency, string name)
        {
            if (key.StartsWith("AccountCode"))
            {
                if (value.StartsWith("D"))
                    PaperTrade = true;
                reqAccountWH.Set();
            }
            Keys[name+" "+currency+" "+key] = value;

        }
        public static void UpdatePortfolio(Contract contract, decimal position, double marketPrice, double marketValue, double averageCost, double unrealizedPNL, double realizedPNL, String accountName)
        {
            string name = contract.Symbol+" "+contract.SecType;
            PortfolioList[name] = new Portfolio(contract, position, marketPrice, marketValue, averageCost, unrealizedPNL, realizedPNL, accountName);
        }

    }
}
