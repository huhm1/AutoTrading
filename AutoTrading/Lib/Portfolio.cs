using IBApi;
using System;

namespace AutoTrading.Lib
{
    public class Portfolio
    {
        public Contract Contract;
        public decimal Position;
        public double MarketPrice;
        public double MarketValue;
        public double AverageCost;
        public double UnrealizedPNL;
        public double RealizedPNL;
        public string AccountName;
        
        public Portfolio(Contract contract, decimal position, double marketPrice,
            double marketValue, double averageCost, double unrealizedPNL, double realizedPNL,
            String accountName)
        {
            Contract = contract;
            Position = position;
            MarketPrice = marketPrice;
            MarketValue = marketValue;
            AverageCost = averageCost;
            UnrealizedPNL = unrealizedPNL;
            RealizedPNL = realizedPNL;
            AccountName = accountName;
        }
        public void Update(Contract contract, int position, double marketPrice,
            double marketValue, double averageCost, double unrealizedPNL, double realizedPNL,
            String accountName)
        {
            Contract = contract;
            Position = position;
            MarketPrice = marketPrice;
            MarketValue = marketValue;
            AverageCost = averageCost;
            UnrealizedPNL = unrealizedPNL;
            RealizedPNL = realizedPNL;
            AccountName = accountName;
        }
    }
}
