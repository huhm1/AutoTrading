using System;
using System.Collections.Generic;

namespace AutoTrading.Lib
{
    class RSI:Indicator
    {
        //public new string Name = "A";
        public List<double> R;
        public List<double> UR;
        public List<double> DR;
        public List<double> StochRSI;
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
            get { return "RSI"; }
        }
        public RSI(int period)
        {
            n = period;
            pk = 1.0/period;
            R = new List<double>();
            StochRSI = new List<double>();
        }

        override public void Update()
        {
            int last = barList.Count-1;
            if (last > 1)
            {
                if (barList[last - 1].C < barList[last].C)
                {
                    ur = ur * (1 - pk) + (barList[last].C - barList[last - 1].C) * pk;
                    dr = dr * (1 - pk);
                    
                }
                else
                {
                    dr = dr * (1 - pk) + (barList[last - 1].C - barList[last].C) * pk;
                    ur = ur * (1 - pk);
                   
                }

                if (ur + dr == 0)
                    r = 50;
                else
                    r = ur / (ur + dr) * 100.0;
            }
            else
                r = 50;

            if (R.Count - 1 < last)
                R.Add(r);
            else
                R[last] = r;

            if (last > 1)
            {
                hr = 0;
                lr = 100;
                int j = 0;
                if(last-n>=0)
                   j = last - n;
                for (int i = last; i > j; i--)
                {
                    if (hr < R[i]) hr = R[i];
                    if (lr > R[i]) lr = R[i];
                }
                if (hr != lr)
                    sr = (R[last] - lr) / (hr - lr);
                else
                    sr = 0.5;
            }
            else
                sr = 0.5;
            if (StochRSI.Count - 1 < last)
                StochRSI.Add(sr);
            else
                StochRSI[last] = sr;

        }

        public override void Initialize()
        {
            throw new NotImplementedException();
        }
    }
}

