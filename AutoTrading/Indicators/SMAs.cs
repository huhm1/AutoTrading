using System;
using System.Collections.Generic;

namespace AutoTrading.Lib
{
    class SMAs:Indicator
    {
        //public new string Name = "A";
        //public List<double> R;
        //public List<double> UR;
        public Logs IndicatorRecord;

        public List<int> UD;///Line up down value;
        public List<double> MASD;///MAs Standard Deviation
        public List<int> MAsDM;///a number of MAs;
        //public List<double> MASDp;///MAs Standard Deviation divide by by average 
        public List<double> MASDMA;///Standard Deviation move average.
        public Dictionary<int,List<double >> MAs;
        public Dictionary<int, List<double>> MAsChange;///Change of each MA.
                                                       ///
        public double ur = 0;
        //public double dr = 0;
        public double lr = 100.0;
        public double sr = 0.5;
        public List<int> n ;

        public string Trade = "";

        private int SDMALong = 6;
        private  double r ,dr;
        private double mar;
        //private int ud;
        public List<double> pk;
        //private int count = 0;
        private double[] lastMA;
        private bool p = false;
        public override string Name
        {
            get { return "SMAs"; }
        }
        public SMAs(string name,params int[] list)
        {
            MAs = new Dictionary<int, List<double>>();
            MAsChange = new Dictionary<int, List<double>>();
            MASD = new List<double>();
            MAsDM = new List<int>();
            //MASDp = new List<double>();
            MASDMA = new List<double>();
            n = new List<int>();
            pk = new List<double>();
            UD = new List<int>();
            foreach (int i in list)
            {
                n.Add(i);
                pk.Add(1.0/i);
                MAs.Add(i, new List<double>());
                MAsChange.Add(i, new List<double>());
            }

            lastMA = new double[n.Count];
            //MA = new List<double>();

            IndicatorRecord = new Logs(name + ".csv", false);

        }

        override public void Update()
        {
            int drM = 0;
            List<double> MA;
            int last = barList.Count-1;
            for(int i=0;i<n.Count;i++)
            {
                int p = n[i];
                MA = MAs[p];
                if (last > 0)
                {
                    r = (MA[last - 1] * (p - 1) + barList[last].C) / p;
                    dr = r - MA[last - 1];
                }
                else
                {
                    r = barList[last].C;
                    dr = 0;
                }
                if (MA.Count - 1 < last)
                {
                    MA.Add(r);
                    MAsChange[p].Add(dr);
                }
                else
                {
                    MA[last] = r;
                    MAsChange[p][last] = dr;
                }
                lastMA[i] = r;

                if(dr>0)
                    drM += 1 << i;
            }


            int ud = 0;
            double ma;
            double sd;
            double sdp;
            double u2 = 0, u = 0;
            string lastMAstring = null;
            for (int i = 0; i < n.Count; i++)
            {
                ma = lastMA[i];
                u += ma;
                u2 += ma*ma;
                lastMAstring += ma+", ";
                for (int j = i+1; j < n.Count; j++)
                {
                    if (ma > lastMA[j])
                        ud++;
                }
            }

            sd = Math.Sqrt((u2 - u*u/n.Count)/n.Count);

            if (last > 0)
            {
                mar = (MASDMA[last - 1] * (SDMALong - 1) + sd) / SDMALong;
            }
            else
                mar = sd;


            sdp = sd/u;
            if (UD.Count - 1 < last)
            {
                UD.Add(ud);
                MASD.Add(sd);
                MASDMA.Add(mar);
                //MASDp.Add(sdp);
                MAsDM.Add(drM);
                p = true;
            }
            else
            {
                UD[last] = ud;
                MASD[last] = sd;
                MASDMA[last] = mar;
                //MASDp.Add(sdp);
                MAsDM[last] = drM;

            }

                if (last > 0)
                {
                    r = (MASDMA[last - 1] * (SDMALong - 1) + sd) / SDMALong;
                }
                else
                    r = sd;


            foreach ( KeyValuePair<int, List<double>> pair in MAsChange)
            {
                if (pair.Value[last] > 0)
                    lastMAstring += "+, ";
                else if (pair.Value[last] < 0)
                    lastMAstring += "-, ";
                else
                    lastMAstring += "0, ";

            }
            if (p)
            {
                IndicatorRecord.Write(barList[last].D + ", " + lastMAstring + drM + ", " + ud + ", " + sd + ", " + mar +
                                      ", " + barList[last].C + ", " + Trade);
                Trade = "";
                p = false;
            }
        }

        public override void Initialize()
        {
            throw new NotImplementedException();
        }
    }
}

