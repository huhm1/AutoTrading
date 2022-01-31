using System;
using System.Collections.Generic;

namespace AutoTrading.Lib
{
    public class HLZLine : Indicator
    {
        public List<Point> HPoints;
        public List<Point> LPoints;
        public List<double> Average;
        public List<double> Sigma;
        
        private double rate = 0.05;
        private double Length = 200;

        private Point ph, phl, h;
        private Point pl, plh, l;

        private enum direction { UP, DOWN, NONE }

        private direction tend = direction.NONE;

        public override string Name
        {
            get { return "HLZLine"; }
        }
        public HLZLine(double rate, double Length)
        {
            this.rate = rate;
            this.Length = 1.0 / Length;
            HPoints = new List<Point>();
            LPoints = new List<Point>();

            Average = new List<double>();
            Sigma = new List<double>();

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
                        //
                        int n = HPoints.Count;
                        if (n > 1)
                        {
                            HPoints[n-1].GetBarSpan(HPoints[n - 2]);
                        }
                        //
                        if (n > 2)
                            CurveFit.PolyFit(ref HPoints, 2, out HPoints[n - 1].Line.a);
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
                        //
                        int n = LPoints.Count;
                        if (n > 1)
                        {
                            LPoints[n-1].GetBarSpan(LPoints[n - 2]);
                        }
                        //
                        if (n > 2)
                            CurveFit.PolyFit(ref LPoints, 2, out LPoints[n - 1].Line.a);
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

        }

        private void Statistics()
        {
            double difference = HPoints[HPoints.Count - 1].Price - LPoints[LPoints.Count - 1].Price;
            double am = Average.Count == 0 ? difference : Average[Average.Count - 1] * (1 - Length) + difference * Length;
            Average.Add(am);
            double sg = Math.Pow(Math.Abs(difference - am), 2);
            double asg = Sigma.Count == 0 ? sg : Sigma[Sigma.Count - 1] * (1 - Length) + sg * Length;
            TimeSpan ts = HPoints[HPoints.Count - 1].Time - LPoints[LPoints.Count - 1].Time;
            Sigma.Add(asg);
        }

        public override void Initialize()
        {
            throw new NotImplementedException();
        }
    }
}
