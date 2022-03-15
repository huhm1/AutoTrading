using AutoTrading.Lib;
using System;
using System.Collections.Generic;

namespace AutoTrading.Indicators
{
    class SMA:Indicator
    {
        //public new string Name = "A";
        //public List<double> R;
        //public List<double> UR;
        //public List<double> DR;
        public List<double> MA;
        public double r = 50.0;
        public double ur = 0;
        public double dr = 0;
        public double hr = 0;
        public double lr = 100.0;
        public double sr = 0.5;
        public int n = 15;

        public double pk;

        public override string Name
        {
            get { return "SMA"; }
        }
        public SMA(int period)
        {
            n = period;
            pk = 1.0/period;
            MA = new List<double>();
        }

        override public void Update()
        {
            int last = barList.Count-1;
            if (last >=1)
            {
                r = r*(1 - pk) + barList[last].C*pk;
            }
            else
                r = barList[last].C;

            if (MA.Count - 1 < last)
                MA.Add(r);
            else
                MA[last] = r;

        }

        public override void Initialize()
        {
            throw new NotImplementedException();
        }
    }
}

