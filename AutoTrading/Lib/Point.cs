using System;

namespace AutoTrading.Lib
{
    public class Point
    {
        public DateTime Time;
        public double Price;
        public Line Line;
        public int BarSN;
        public int BarSpan;

        public Point()
        {
        }

        public Point(double price, DateTime time, int n)
        {
            Price = price;
            Time = time;
            BarSpan = 0;
            Line = new Line();
            BarSN = n;
        }
        public Point(double price, DateTime time, int span, int n)
        {
            Price = price;
            Time = time;
            BarSpan = span;
            Line = new Line();
            BarSN = n;
        }
        public Point(double price, DateTime time, int span, Line l, int n)
        {
            Price = price;
            Time = time;
            BarSpan = span;
            Line = l;
            BarSN = n;
        }

        public void GetBarSpan(int barSN)
        {
            BarSpan = BarSN - barSN;
        }
        public void GetBarSpan(Point p)
        {
            BarSpan = BarSN - p.BarSN;
        }

        public static Point operator -(Point a, Point b)
        {
            int span;
            span = a.BarSpan - b.BarSpan;
            return new Point(a.Price, a.Time, span, a.Line, a.BarSN);
        }

    }
}
