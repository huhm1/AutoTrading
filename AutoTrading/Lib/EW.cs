using IBApi;
using System;
using System.Collections.Generic;
using System.Threading;

namespace AutoTrading.Lib
{
    public class EW : EWrapper
    {
        public static EventWaitHandle openOrderWH = new EventWaitHandle(false, EventResetMode.ManualReset);

        #region EWrapper Members

        public void accountDownloadEnd(string accountName)
        {
        }

        public void bondContractDetails(int reqId, ContractDetails contractDetails)
        {
            Contract contract = contractDetails.Contract;

            Client.mTWS.Add(" ---- Bond Contract Details begin ----");
            Client.mTWS.Add("symbol = " + contract.Symbol);
            Client.mTWS.Add("secType = " + contract.SecType);
            Client.mTWS.Add("cusip = " + contract.ConId);
            Client.mTWS.Add("exchange = " + contract.Exchange);
            Client.mTWS.Add("currency = " + contract.Currency);
            Client.mTWS.Add("tradingClass = " + contract.TradingClass);
            Client.mTWS.Add("marketName = " + contractDetails.MarketName);
            Client.mTWS.Add("minTick = " + contractDetails.MinTick);
            Client.mTWS.Add("orderTypes = " + contractDetails.OrderTypes);
            Client.mTWS.Add("validExchanges = " + contractDetails.ValidExchanges);
            Client.mTWS.Add(" ---- Bond Contract Details End ----");
        }
        public void connectionClosed()
        {
            Client.mTWS.Add("Connection Closed");
        }
        public void contractDetails(int reqId, ContractDetails contractDetails)
        {
            Contract contract = contractDetails.Contract;

            Client.mTWS.Add(" ---- Contract Details begin ----");
            Client.mTWS.Add("symbol = " + contract.Symbol);
            Client.mTWS.Add("secType = " + contract.SecType);
            //Client.mTWS.Add("expiry = " + contract.m_expiry);
            Client.mTWS.Add("strike = " + contract.Strike);
            Client.mTWS.Add("right = " + contract.Right);
            Client.mTWS.Add("exchange = " + contract.Exchange);
            Client.mTWS.Add("currency = " + contract.Currency);
            Client.mTWS.Add("localSymbol = " + contract.LocalSymbol);
            Client.mTWS.Add("tradingClass = " + contract.TradingClass);
            Client.mTWS.Add("marketName = " + contractDetails.MarketName);
            Client.mTWS.Add("minTick = " + contractDetails.MinTick);
            Client.mTWS.Add("price magnifier = " + contractDetails.PriceMagnifier);
            Client.mTWS.Add("orderTypes = " + contractDetails.OrderTypes);
            Client.mTWS.Add("validExchanges = " + contractDetails.ValidExchanges);
            Client.mTWS.Add(" ---- Contract Details End ----");
        }

        public void contractDetailsEnd(int reqId)
        {
            //            throw new Exception("The method or operation is not implemented.");
        }

        public void error(int id, int errorCode, string errorMsg)
        {
            // received error
            string err = "";
            err += id.ToString();
            err += " | ";
            err += errorCode.ToString();
            err += " | ";
            err += errorMsg;
            Client.Logs.WriteT(err);
            //((TextBox) CJ.ActiveForm.Container.Components["m_tickers"]).Text = Client.mErrors.ToString();
            //Client.Form.m_errors.Text = Client.mErrors.ToString();
            Client.DisplayErrors(err);

            /*
                        try
                        {
            */
            // Error 162 is Historical Market Data Service error message:Historical data request pacing violation.
            if (errorCode == 165)
            {
                /*
                                    Stock stock = Stock.StockList[id];
                                    stock.ReqHistoricalError = true;
                                    stock.ErrorNumber = 165;
                                    stock.HistoricalDataWH.Set();
                */
            }
            if (errorCode == 321)
            {
                /*
                                    Stock stock = Stock.StockList[id];
                                    stock.ReqHistoricalError = true;
                                    stock.ErrorNumber = 321;
                                    stock.HistoricalDataWH.Set();
                */
            }
            if (errorCode == 162)
            {
                Stock stock = Stock.StockList[id];
                stock.ReqHistoricalError = true;
                stock.ErrorNumber = 162;
                stock.HistoricalDataWH.Set();
            }




            //Client.StrategyList[id].
            //Stock.StockList[id].ReqData(Stock.StockList[id].ReqHistoricalInterval);
            //                    if (Stock.OrderList.ContainsKey(id))
            //                    {
            //                        OrderStatus orderStatus = Stock.OrderList[id];
            //                        orderStatus.strategy.Settle(0, orderStatus.order.m_totalQuantity, 0);
            //                        orderStatus.strategy.status = 0;
            //                    }

            // Error 135 is fired when Cancel Order, TWS Can't find order id.
            if (errorCode == 135)
            {
                //                    if (Stock.OrderList.ContainsKey(id))
                //                    {
                //                        OrderStatus orderStatus = Stock.OrderList[id];
                //                        orderStatus.strategy.Settle(0, orderStatus.order.m_totalQuantity, 0);
                //                        orderStatus.strategy.status = 0;
                //                    }
            }
            // Error 1101 is fired when connection between IB and TWS is restored
            // after a temporary connection loss and the data is "lost". In this case,
            // we need to resubscribe to market data.
            if (errorCode == 1101)
            {
                //Stock.resubscribeToMarketData();
            }

            // If either error 1101 or 1102 was fired, it means TWS was disconnected
            // from the IB server, so some orders could have been executed during
            // that time. In this case, we need to request executions.
            if (errorCode == 1101 || errorCode == 1102)
            {
                //Stock.resetOrders();
                //Stock.requestExecutions();
            }
            /*
                        }
                        catch(Exception e)
                        {
                            Client.Logs.WriteT(e.Message);
                        }
            */

            /*   for (int ctr = 0; ctr < faErrorCodes.length; ctr++)
               {
                   faError |= (errorCode == faErrorCodes[ctr]);
               }
               if (errorCode == MktDepthDlg.MKT_DEPTH_DATA_RESET)
               {
                   m_mktDepthDlg.reset();
               }*/
        }
        public void error(String str)
        {
            // received error
            Client.Logs.WriteT(str);
            Client.DisplayErrors(str);

        }
        public void error(Exception e)
        {
            Client.Logs.WriteT(e.Message);
            Client.DisplayErrors(e.Message);

        }
        public void execDetails(int orderId, Contract contract, Execution execution)
        {
            if (Stock.OrderList.ContainsKey(orderId))
            {
                OrderStatus orderStatus = Stock.OrderList[orderId];
                orderStatus.Add(execution);
            }
            /* Client.mTWS.Add(" ---- Execution Details begin ----");
             Client.mTWS.Add("orderId = " + orderId);
             Client.mTWS.Add("clientId = " + execution.m_clientId);

             Client.mTWS.Add("symbol = " + contract.m_symbol);
             Client.mTWS.Add("secType = " + contract.m_secType);
             Client.mTWS.Add("expiry = " + contract.m_expiry);
             Client.mTWS.Add("strike = " + contract.m_strike);
             Client.mTWS.Add("right = " + contract.m_right);
             Client.mTWS.Add("contractExchange = " + contract.m_exchange);
             Client.mTWS.Add("currency = " + contract.m_currency);
             Client.mTWS.Add("localSymbol = " + contract.m_localSymbol);

             Client.mTWS.Add("execId = " + execution.m_execId);
             Client.mTWS.Add("time = " + execution.m_time);
             Client.mTWS.Add("acctNumber = " + execution.m_acctNumber);
             Client.mTWS.Add("executionExchange = " + execution.m_exchange);
             Client.mTWS.Add("side = " + execution.m_side);
             Client.mTWS.Add("shares = " + execution.m_shares);
             Client.mTWS.Add("price = " + execution.m_price);
             Client.mTWS.Add("permId = " + execution.m_permId);
             Client.mTWS.Add("liquidation = " + execution.m_liquidation);
             Client.mTWS.Add(" ---- Execution Details end ----"); */
        }

        public virtual void execDetailsEnd(int reqId)
        {
        }

        public void historicalData(int reqId, IBApi.Bar bar)
        {
            Stock stock = Stock.StockList[reqId];
            //try
            DateTime dateTime = DateTime.ParseExact(bar.Time, "yyyyMMdd  HH:mm:ss", Client.culture);
            //if (dateTime.TimeOfDay > stock.marketStartTime && dateTime.TimeOfDay <= stock.marketEndTime)
            if (dateTime > stock.PriceDateTime + stock.ReqHistoricalInterval)
            {
                //if(!stock.DataFromFile)
                stock.HistoryCVS.Write(dateTime.ToString("yyyy-MM-dd HH:mm:ss") + "," + bar.Open + "," + bar.High + "," +
                                       bar.Low + "," + bar.Close + "," + bar.Volume + "," + bar.Count + "," + bar.WAP);
                stock.HistoricalData(dateTime, bar.Open, bar.High, bar.Low, bar.Close, bar.Volume);
            }

        }
        public void historicalDataEnd(int reqId, string start, string end)
        {
            Stock stock = Stock.StockList[reqId];
            stock.HistoricalDataReqCompleted = true;
            stock.HistoricalDataWH.Set();

        }
        public void historicalDataUpdate(int reqId, IBApi.Bar bar)
        {
        }


        public void managedAccounts(String accountsList)
        {
            //m_bIsFAAccount = true;
            //m_FAAcctCodes = accountsList;

            Client.mTWS.Add("Connected : The list of managed accounts are : [" + accountsList + "]");
        }

        public void nextValidId(int orderId)
        {
            Client.OrderID = orderId;
            Client.OrderIDWH.Set();
            // received next valid order id
            //Client.mTWS.Add("Next Valid Order ID: " + orderId.ToString());
            //m_orderDlg.setIdAtLeast(orderId);
        }
        public void openOrder(int orderId, Contract contract, Order order, OrderState orderState)
        {
            // received open order
            if (Stock.OrderList.ContainsKey(orderId))
            {
                OrderStatus orderStatus = Stock.OrderList[orderId];
                orderStatus.Update(order);
            }
            else
            {
                Client.OrderIDWH.Reset();
                Client.Socket.reqIds(orderId);

                //openOrderWH.WaitOne();
                Strategy strategy;
                foreach (KeyValuePair<string, Strategy> kvp in Client.StrategyList)
                {
                    strategy = kvp.Value;
                    if (contract.Symbol == strategy.contract.Symbol)
                    {

                        Stock stock = strategy.stock;
                        int action = order.Action == "BUY" ? 1 : 2;

                        Stock.OrderList.Add(orderId, new OrderStatus(order, kvp.Value, stock, action));
                        stock.LastOrderID = orderId;
                        strategy.status = action;
                        break;
                    }
                }
            }
            Client.Logs.WriteT("open order: orderId=" + orderId +
                            " action=" + order.Action +
                            " quantity=" + order.TotalQuantity +
                            " symbol=" + contract.Symbol +
                            " type=" + order.OrderType +
                            " lmtPrice=" + order.LmtPrice +
                            " auxPrice=" + order.AuxPrice +
                            " TIF=" + order.Tif +
                            " localSymbol=" + contract.LocalSymbol +
                            " client Id=" + order.ClientId +
                            " parent Id=" + order.ParentId +
                            " permId=" + order.PermId +
                            " hidden=" + order.Hidden +
                            " discretionaryAmt=" + order.DiscretionaryAmt +
                            " triggerMethod=" + order.TriggerMethod +
                            " goodAfterTime=" + order.GoodAfterTime +
                            " goodTillDate=" + order.GoodTillDate +
                            " account=" + order.Account +
                            " faGroup=" + order.FaGroup +
                            " faMethod=" + order.FaMethod +
                            " faPercentage=" + order.FaPercentage +
                            " faProfile=" + order.FaProfile +
                            " shortSaleSlot=" + order.ShortSaleSlot +
                            " designatedLocation=" + order.DesignatedLocation +
                            " ocaType=" + order.OcaType +
                            " rule80A=" + order.Rule80A +
                            " settlingFirm=" + order.SettlingFirm +
                            " allOrNone=" + order.AllOrNone +
                            " minQty=" + order.MinQty +
                            " percentOffset=" + order.PercentOffset +
                            " eTradeOnly=" + order.m_eTradeOnly +
                            //                   " firmQuoteOnly=" + order.m_firmQuoteOnly +
                            //                   " nbboPriceCap=" + order.m_nbboPriceCap +
                            " auctionStrategy=" + order.AuctionStrategy +
                            " startingPrice=" + order.StartingPrice +
                            " stockRefPrice=" + order.StockRefPrice +
                            " delta=" + order.Delta +
                            " stockRangeLower=" + order.StockRangeLower +
                            " stockRangeUpper=" + order.StockRangeUpper +
                            " volatility=" + order.Volatility +
                            " volatilityType=" + order.VolatilityType +
                            " deltaNeutralOrderType=" + order.DeltaNeutralOrderType +
                            " deltaNeutralAuxPrice=" + order.DeltaNeutralAuxPrice +
                            " continuousUpdate=" + order.ContinuousUpdate +
                            " referencePriceType=" + order.ReferencePriceType +
                            " trailStopPrice=" + order.TrailStopPrice);
        }

        public void openOrderEnd()
        {
            //            throw new Exception("The method or operation is not implemented.");
        }

        public virtual void receiveFA(int faDataType, String xml)
        {
            /*displayXML("FA: " + EClientSocket.faMsgTypeName(faDataType), xml);
            switch (faDataType)
            {
                case EClientSocket.GROUPS:
                    faGroupXML = xml;
                    break;
                case EClientSocket.PROFILES:
                    faProfilesXML = xml;
                    break;
                case EClientSocket.ALIASES:
                    faAliasesXML = xml;
                    break;
            }

            if (!faError &&
                !(faGroupXML == null || faProfilesXML == null || faAliasesXML == null))
            {
                FinancialAdvisorDlg dlg = new FinancialAdvisorDlg(this);
                dlg.receiveInitialXML(faGroupXML, faProfilesXML, faAliasesXML);
                dlg.show();

                if (!dlg.m_rc)
                {
                    return;
                }

                m_client.replaceFA(EClientSocket.GROUPS, dlg.groupsXML);
                m_client.replaceFA(EClientSocket.PROFILES, dlg.profilesXML);
                m_client.replaceFA(EClientSocket.ALIASES, dlg.aliasesXML);

            }*/
        }
        public virtual void scannerData(int reqId, int rank,
                                ContractDetails contractDetails, String distance,
                                String benchmark, String projection, string legsStr)
        {
            /*           Contract contract = contractDetails.m_summary;

                       m_tickers.add("id = " + reqId +
                                  " rank=" + rank +
                                  " symbol=" + contract.m_symbol +
                                  " secType=" + contract.m_secType +
                                  " expiry=" + contract.m_expiry +
                                  " strike=" + contract.m_strike +
                                  " right=" + contract.m_right +
                                  " exchange=" + contract.m_exchange +
                                  " currency=" + contract.m_currency +
                                  " localSymbol=" + contract.m_localSymbol +
                                  " marketName=" + contractDetails.m_marketName +
                                  " tradingClass=" + contractDetails.m_tradingClass +
                                  " distance=" + distance +
                                  " benchmark=" + benchmark +
                                  " projection=" + projection);*/
        }

        public virtual void scannerDataEnd(int reqId)
        {
        }

        public virtual void scannerParameters(String xml)
        {
        }

        public virtual void realtimeBar(int reqId, long time, double open, double high, double low, double close, decimal volume, decimal WAP, int count)
        {
        }

        public virtual void currentTime(long time)
        {
        }

        public void tickGeneric(int tickerId, int field, double value)
        {
            // received size tick
            //Client.DisplaymTickers( "id=" + tickerId + "  " + TickType.getField(field) + "=" + value);
        }
        public void tickOptionComputation(int tickerId, int field, int tickAttrib, double impliedVolatility, double delta, double optPrice, double pvDividend, double gamma, double vega, double theta, double undPrice)
        {
            // received price tick
            String toAdd = "id=" + tickerId + "  " + TickType.getField(field) + ": vol = " +
               ((impliedVolatility >= 0 && impliedVolatility != Double.MaxValue) ? impliedVolatility.ToString() : "N/A") + " delta = " +
               ((Math.Abs(delta) <= 1) ? delta.ToString() : "N/A");
            if (field == TickType.MODEL_OPTION)
            {
                toAdd += ": optPrice = " + ((optPrice >= 0 && optPrice != Double.MaxValue) ? optPrice.ToString() : "N/A");
                toAdd += ": pvDividend = " + ((pvDividend >= 0 && pvDividend != Double.MaxValue) ? pvDividend.ToString() : "N/A");
            }
            Client.mTickers.Add(toAdd);
        }
        public virtual void tickSnapshotEnd(int tickerId)
        {
        }

        public void tickPrice(int tickerId, int field, double price, TickAttrib attribs)
        {
            Stock.UpdatePrice(tickerId, field, price, attribs);
        }
        public void tickSize(int tickerId, int field, decimal size)
        {
            Stock.UpdateSize(tickerId, field, size);
        }
        public void tickString(int tickerId, int field, String value)
        {
            // received size tick
            //Client.DisplaymTickers( "idStr=" + tickerId + "  " + TickType.getField(field) + "=" + value + "\r\n");
        }

        public void tickEFP(int tickerId, int tickType, double basisPoints, string formattedBasisPoints,
                            double impliedFuture, int holdDays, string futureExpiry, double dividendImpact,
                            double dividendsToExpiry)
        {
            throw new NotImplementedException();
        }

        public void orderStatus(int orderId, string status, decimal filled, decimal remaining, double avgFillPrice, int permId, int parentId, double lastFillPrice, int clientId, string whyHeld, double mktCapPrice)
        {
            if (!Stock.OrderList.ContainsKey(orderId))
                return;
            OrderStatus orderStatus = Stock.OrderList[orderId];
            orderStatus.Update(status, filled, remaining, avgFillPrice);

            /*
                        if (status.Contains("Cancelled"))
                        {
                            orderStatus.strategy.Settle(filled, remaining, avgFillPrice);
                            orderStatus.strategy.status = 0;
                        }
            */

            orderStatus.stock.TradeRecord.WriteT("order status: orderId=" + orderId + " clientId=" + clientId + " permId=" + permId +
                            " status=" + status + " filled=" + filled + " remaining=" + remaining +
                            " avgFillPrice=" + avgFillPrice + " lastFillPrice=" + lastFillPrice +
                            " parent Id=" + parentId);
        }

        public void updateAccountTime(String timeStamp)
        {
            // m_acctDlg.updateAccountTime(timeStamp);
        }
        public void updateAccountValue(String key, String value,
                                       String currency, String accountName)
        {
            Account.UpdateAccountValue(key, value, currency, accountName);
        }
        public virtual void updateMktDepth(int tickerId, int position, int operation, int side, double price, decimal size)
        {
        }

        public virtual void updateMktDepthL2(int tickerId, int position, string marketMaker, int operation, int side, double price, decimal size, bool isSmartDepth)
        {
        }
        public void updateNewsBulletin(int msgId, int msgType, String message, String origExchange)
        {
            string msg = " MsgId=" + msgId + " :: MsgType=" + msgType + " :: Origin=" + origExchange + " :: Message=" + message;
            Client.DisplayTWS(msg);
        }

        public void updatePortfolio(Contract contract, decimal position, double marketPrice, double marketValue, double averageCost, double unrealizedPNL, double realizedPNL, string accountName)
        {
            Account.UpdatePortfolio(contract, position, marketPrice, marketValue, averageCost, unrealizedPNL, realizedPNL, accountName);
        }

        public void deltaNeutralValidation(int reqId, DeltaNeutralContract deltaNeutralContract)
        {
        }
        public void fundamentalData(int reqId, string data)
        {
        }

        public virtual void accountSummary(int reqId, string account, string tag, string value, string currency)
        {
        }

        public virtual void accountSummaryEnd(int reqId)
        {
        }
        public virtual void commissionReport(CommissionReport commissionReport)
        {
        }
        public virtual void accountUpdateMultiEnd(int requestId)
        {
        }


        public virtual void securityDefinitionOptionParameter(int reqId, string exchange, int underlyingConId, string tradingClass, string multiplier, HashSet<string> expirations, HashSet<double> strikes)
        {
        }

        public virtual void securityDefinitionOptionParameterEnd(int reqId)
        {
        }

        public virtual void softDollarTiers(int reqId, SoftDollarTier[] tiers)
        {
        }

        public virtual void familyCodes(FamilyCode[] familyCodes)
        {
        }

        public virtual void symbolSamples(int reqId, ContractDescription[] contractDescriptions)
        {
        }

        public virtual void mktDepthExchanges(DepthMktDataDescription[] descriptions)
        {
        }

        public virtual void tickNews(int tickerId, long timeStamp, string providerCode, string articleId, string headline, string extraData)
        {
        }

        public virtual void smartComponents(int reqId, Dictionary<int, KeyValuePair<string, char>> theMap)
        {
        }

        public virtual void tickReqParams(int tickerId, double minTick, string bboExchange, int snapshotPermissions)
        {
        }

        public virtual void newsProviders(NewsProvider[] newsProviders)
        {
        }

        public virtual void newsArticle(int requestId, int articleType, string articleText)
        {
        }

        public virtual void historicalNews(int requestId, string time, string providerCode, string articleId, string headline)
        {
        }

        public virtual void historicalNewsEnd(int requestId, bool hasMore)
        {
        }

        public virtual void headTimestamp(int reqId, string headTimestamp)
        {
        }

        public virtual void histogramData(int reqId, HistogramEntry[] data)
        {
        }

        public virtual void rerouteMktDataReq(int reqId, int conId, string exchange)
        {
        }

        public virtual void rerouteMktDepthReq(int reqId, int conId, string exchange)
        {
        }

        public virtual void marketRule(int marketRuleId, PriceIncrement[] priceIncrements)
        {
        }


        public virtual void pnl(int reqId, double dailyPnL, double unrealizedPnL, double realizedPnL)
        {

        }

        public virtual void pnlSingle(int reqId, decimal pos, double dailyPnL, double realizedPnL, double value, double unrealizedPnL)
        {

        }

        public virtual void historicalTicks(int reqId, HistoricalTick[] ticks, bool done)
        {
        }

        public virtual void historicalTicksBidAsk(int reqId, HistoricalTickBidAsk[] ticks, bool done)
        {
        }

        public virtual void historicalTicksLast(int reqId, HistoricalTickLast[] ticks, bool done)
        {
        }

        public virtual void tickByTickAllLast(int reqId, int tickType, long time, double price, decimal size, TickAttribLast tickAttribLast, string exchange, string specialConditions)
        {
        }

        public virtual void tickByTickBidAsk(int reqId, long time, double bidPrice, double askPrice, decimal bidSize, decimal askSize, TickAttribBidAsk tickAttribBidAsk)
        {
        }

        public virtual void tickByTickMidPoint(int reqId, long time, double midPoint)
        {
        }

        public virtual void orderBound(long orderId, int apiClientId, int apiOrderId)
        {
        }

        public virtual void completedOrder(Contract contract, Order order, OrderState orderState)
        {
        }

        public virtual void completedOrdersEnd()
        {
        }

        public virtual void replaceFAEnd(int reqId, string text)
        {
        }

        public virtual void wshMetaData(int reqId, string dataJson)
        {
        }

        public virtual void wshEventData(int reqId, string dataJson)
        {
        }

        public void historicalSchedule(int reqId, string startDateTime, string endDateTime, string timeZone, HistoricalSession[] sessions)
        {
        }

        public void marketDataType(int reqId, int marketDataType)
        {
        }

        public void position(string account, Contract contract, decimal pos, double avgCost)
        {
        }

        public void positionEnd()
        {
        }

        public void verifyMessageAPI(string apiData)
        {
        }

        public void verifyCompleted(bool isSuccessful, string errorText)
        {
        }

        public void verifyAndAuthMessageAPI(string apiData, string xyzChallenge)
        {
        }

        public void verifyAndAuthCompleted(bool isSuccessful, string errorText)
        {
        }

        public virtual void displayGroupList(int reqId, string groups)
        {
        }

        public virtual void displayGroupUpdated(int reqId, string contractInfo)
        {
        }

        public void connectAck()
        {
            if (Client.Socket.AsyncEConnect)
                Client.Socket.startApi();

        }

        public virtual void positionMulti(int requestId, string account, string modelCode, Contract contract, decimal pos, double avgCost)
        {
        }

        public virtual void positionMultiEnd(int requestId)
        {
        }

        public virtual void accountUpdateMulti(int requestId, string account, string modelCode, string key, string value, string currency)
        {
        }




        #endregion
    }

    public class UnderComp
    {
    }
}
