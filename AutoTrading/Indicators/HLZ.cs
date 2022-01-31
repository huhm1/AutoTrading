using System;
using System.Collections.Generic;

namespace AutoTrading.Lib
{
    public class HLZ : Indicator
    {
        private double rate = 0.05;
        private double Length = 200;
        private int AvgLength = 14;
        private int DAvgLength = 3;
        public List<Point> HPoints;
        public List<Point> LPoints;
        public List<double> Average;
        public List<double> DAverage;
        public List<double> Sigma;
        public List<double> Df;

        public Point ph,phl,h;
        public Point pl,plh,l;
        public double m;
        enum direction{UP,DOWN,NONE}

        private direction tend = direction.NONE;

        private direction HSide = direction.UP;
        private direction LSide = direction.DOWN;

        public override string Name
        {
            get { return "HLZ"; }
        }
        public HLZ(double rate, double Length)
        {
            this.rate = rate;
            this.Length = 1.0/Length;
            HPoints = new List<Point>();
            LPoints = new List<Point>();

            Average = new List<double>();
            Sigma = new List<double>();
            Df = new List<double>();
            DAverage = new List<double>();

            ph = plh = new Point();
            pl = phl = new Point();

            ph.Price = plh.Price = 0.01;//double.MinValue;
            pl.Price = phl.Price = double.MaxValue;
            
        }
        public HLZ(double rate, double Length,int Avg,int DAvg)
        {
            this.rate = rate;
            this.Length = 1.0 / Length;
            this.AvgLength = Avg;
            this.DAvgLength = DAvg;
            HPoints = new List<Point>();
            LPoints = new List<Point>();

            Average = new List<double>();
            Sigma = new List<double>();
            Df = new List<double>();
            DAverage = new List<double>();

            ph = plh = new Point();
            pl = phl = new Point();

            ph.Price = plh.Price = 0.01;//double.MinValue;
            pl.Price = phl.Price = double.MaxValue;

        }

        public override void Update()
        {
            int lastBar = barList.Count - 1;
            Bar bar = barList[lastBar];
            h = new Point(bar.H, bar.D, barList.Count);
            l = new Point(bar.L, bar.D, barList.Count);

            switch (tend)
            {
                case direction.UP:
                    if ((ph.Price - bar.L) / ph.Price > rate)
                    {
                        HPoints.Add(ph);
                        pl = l;
                        tend = direction.DOWN;
                        Statistics();
                    }
                    else if (ph.Price <= bar.H)
                        ph = h;
                    break;
                case direction.DOWN:
                    if ((bar.H - pl.Price) / bar.H > rate)
                    {
                        LPoints.Add(pl);
                        ph = h;
                        tend = direction.UP;
                        Statistics();
                    }
                    else if (pl.Price >= bar.L)
                        pl = l;
                    break;
                case direction.NONE:
                    if ((ph.Price - bar.L) / ph.Price > rate)
                    {
                        HPoints.Add(ph);
                        pl = l;
                        tend = direction.DOWN;
                        break;
                    }
                    else if (ph.Price <= bar.H)
                        ph = h;
                    if ((bar.H - pl.Price) / bar.H > rate)
                    {
                        LPoints.Add(pl);
                        ph = h;
                        tend = direction.UP;
                    }
                    else if (pl.Price >= bar.L)
                        pl = l;
                    break;

            }


/*
            if (bar.H >= ph.Price) ph = h;
            if (bar.H < phl.Price) phl = h;
            if (bar.L <= pl.Price) pl = l;
            if (bar.L > plh.Price) plh = l;


            if ((ph.Price - bar.H) / ph.Price > rate && HSide == direction.UP)
            {
                HPoints.Add(ph);
                phl = h;
                HSide = direction.DOWN;
            }
            if ((bar.H - phl.Price) / bar.H > rate && HSide == direction.DOWN)
            {
                //if h>ph
                ph = h;
                HSide = direction.UP;
            }

            if ((bar.L - pl.Price) / bar.L > rate && LSide == direction.DOWN)
            {
                LPoints.Add(pl);
                plh = l;
                LSide = direction.UP;
            }
            if ((plh.Price - bar.L) / plh.Price > rate && LSide == direction.UP)
            {
                pl = l;
                LSide = direction.DOWN;
            }
*/

        }

        private void Statistics()
        {
            double difference = HPoints[HPoints.Count - 1].Price - LPoints[LPoints.Count - 1].Price;
            Df.Add(difference);
            double am = Average.Count == 0 ? difference : Average[Average.Count - 1] * (1 - Length) + difference * Length;
            Average.Add(am);
            double sg = Math.Pow(Math.Abs(difference - am), 2);
            double asg = Sigma.Count == 0 ? sg : Sigma[Sigma.Count - 1] * (1 - Length) + sg * Length;
            TimeSpan ts= HPoints[HPoints.Count - 1].Time - LPoints[LPoints.Count - 1].Time;
            Sigma.Add(asg);
            if (Df.Count >= AvgLength)
            {
                List<double> ad = new List<double>();
                for (int i = Df.Count - 1; i >= Df.Count - AvgLength; i--)
                {
                    ad.Add(Df[i]);
                }
                ad.Sort();
                double dv = 0;
                for (int i = DAvgLength; i < ad.Count - DAvgLength; i++)
                {
                    dv += ad[i]/(AvgLength - 2*DAvgLength);
                }
                DAverage.Add(dv);
            }
        }

        public override void Initialize()
        {
            throw new NotImplementedException();
        }
    }
}
