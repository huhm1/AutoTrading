using System;
using System.Collections.Generic;
using System.Text;
using com.ib.client;

namespace CJava.Lib
{
    public class Bars
    {
        //public TimeSpan Interval;
        //public Contract Contract;
        public List<Bar> BarList;
        public Dictionary<string,IndicatorHistory> IndicatorList;

        public Bars()
        {
            BarList = new List<Bar>();
        }
        public Bars(Contract contract,TimeSpan interval)
        {
            BarList = new List<Bar>();
            Contract = contract;
            Interval = interval;
        }
        public void Add(Bar bar)
        {
            BarList.Add(bar);
        }

    }
}
