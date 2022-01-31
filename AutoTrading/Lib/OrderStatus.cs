using System.Collections.Generic;
using IBApi;

namespace AutoTrading.Lib
{
    public class OrderStatus
    {
        //public int orderId;
        public Order order;
        public List<Execution> executions;
        public Strategy strategy;
        public decimal contractsFilled = 0;
        public double avgFillPrice = 0;
        public double total = 0;
        public Stock stock;
        public string TWSstatus = "WaitingSubmit";
        public int status = 0;

        public OrderStatus(Order order,Strategy strategy,Stock stock,int status)
        {
            this.order = order;
            this.strategy = strategy;
            this.stock = stock;
            this.status = status;
            executions = new List<Execution>();
        }

        public void Add(Execution execution)
        {
            executions.Add(execution);
            contractsFilled += execution.Shares;
            total += execution.Price * (double)execution.Shares;
            avgFillPrice = total / (double)contractsFilled;
            if (contractsFilled == order.TotalQuantity)
            {
                //strategy.Settle(contractsFilled, 0, avgFillPrice);
                //strategy.Reset();
                //strategy.status = 0;
                //strategy.PeriodLow = double.MaxValue;
            }
            stock.TradeRecord.WriteT(execution.Time + " " + execution.Shares + " " + execution.Price + " | " + contractsFilled + " " + avgFillPrice);
        }

        public void reset()
        {
            executions.Clear();
        }
        public void Update(Order order)
        {
            if(order.Transmit == false)
                if(this.status >= 100)
                {
                    stock.cancelOrderWH.Set();
                }
                else
                    stock.CancelOrder();

        }

        public void Update(string status, decimal filled, decimal remaining, double avgFillPrice)
        {
            TWSstatus = status;
            contractsFilled = filled;
            this.avgFillPrice = avgFillPrice;
            if (this.status != 0)
            {
                switch (status)
                {
                    case "PreSubmitted":
                        break;
                    case "PendingSubmit":
                        if (this.status >= 100)
                            stock.cancelOrderWH.Set();
                        break;
                    case "Submitted":
                        if (this.status >= 100)
                            stock.cancelOrderWH.Set();
                        break;
                    case "Filled":
                        if (this.status >= 100)
                            stock.cancelOrderWH.Set();
                        strategy.Settle(filled, remaining, avgFillPrice);
                        this.status = 0;
                        break;
                    case "PendingCancel":
                        if (this.status >= 100)
                            stock.cancelOrderWH.Set();
                        break;
                    case "Cancelled":
                        if (this.status >= 100)
                        {
                            stock.cancelOrderWH.Set();
                        }
                        strategy.Settle(filled, remaining, avgFillPrice);
                        this.status = 0;
                        break;
                }
            }
        }
    }
}
