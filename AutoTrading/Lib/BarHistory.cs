using System;
using System.Collections.Generic;

namespace AutoTrading.Lib
{
    public class BarHistory
    {
        public delegate void BarUpdateHandler();
        public event BarUpdateHandler OnIndicatorUpdate;

        static object locker = new object();
        public List<Bar> BarList;
        public Dictionary<string, Indicator> IndicatorLists;


        public decimal lastVolume = 0;
        public DateTime nextBarTime;
        public TimeSpan interval;
        public Stock stock;

        public BarHistory(Stock stock, TimeSpan interval)
        {
            this.stock = stock;
            this.interval = interval;
            nextBarTime = stock.historyStratDate + stock.marketStartTime;
            BarList = new List<Bar>();
            IndicatorLists = new Dictionary<string, Indicator>();
        }


        public void Add(Bar bar)
        {
            BarList.Add(bar);
        }

        public void AddIndicator(Indicator indicator,string name)
        {
            IndicatorLists.Add(name,indicator);
            indicator.barList = BarList;
            OnIndicatorUpdate += indicator.Update;
        }

        public void AddIndicator(Indicator indicator)
        {
            IndicatorLists.Add(indicator.Name, indicator);
            indicator.barList = BarList;
            OnIndicatorUpdate += indicator.Update;
        }
/*        public void HistoryUpdate()
        {
        }*/

        public void PriceUpdate()
        {
            {
                if (DateTime.Now >= nextBarTime)
                {
                    if (OnIndicatorUpdate != null && BarList.Count > 0)
                    {
                        OnIndicatorUpdate();
                    }
                    lock (locker)
                    {
                        BarList.Add(new Bar(stock.last, nextBarTime));
                        nextBarTime += interval; //need be locked
                        if (nextBarTime.TimeOfDay >= stock.marketEndTime)//????
                        {
                            nextBarTime = nextBarTime.Date + TimeSpan.FromDays(1) + stock.marketStartTime;
                            lastVolume = 0;
                        }
                        lastVolume = stock.volume;
                    }
                }
                else if (BarList.Count >= 1)
                {
                    BarList[BarList.Count - 1].UpdatePrice(stock.last);
                }
            }
        }
        public void VolumeUpdate()
        {
            int lastBar = BarList.Count - 1;
            if (lastBar >= 0)
                BarList[lastBar].UpdateVolume(stock.volume - lastVolume);
        }
        public void HistoryUpdate(DateTime dateTime)
        {
            if (OnIndicatorUpdate != null && BarList.Count > 0)/// Update every time.
                {
                    OnIndicatorUpdate();
                }
            if (dateTime >= nextBarTime)//????XXXXXXXXXXX
            {
/*
                if (OnIndicatorUpdate != null && BarList.Count > 0) /// Update after whole bar.
                {
                    OnIndicatorUpdate();
                }
                */

                lock (locker)
                {
                    if(dateTime.Date>nextBarTime.Date)
                        nextBarTime = dateTime.Date + stock.marketStartTime;
                    else 
                        nextBarTime = dateTime.Date + nextBarTime.TimeOfDay;
                    BarList.Add(new Bar(stock.last, nextBarTime));
                    nextBarTime += interval; //need be locked
                    if (nextBarTime.TimeOfDay >= stock.marketEndTime)
                        nextBarTime = nextBarTime.Date +TimeSpan.FromDays(1)+ stock.marketStartTime;
                }
                //lastVolume += volume;
            }
            else if (BarList.Count >= 1)
            {
                BarList[BarList.Count - 1].UpdatePrice(stock.last);
            }
        }
    }
}
