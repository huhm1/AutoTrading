using System;

namespace AutoTrading.Lib
{
    public class Bar
    {
		public DateTime D;
		public double O,H,L,C;
		public decimal V;
		public Bar(DateTime d,double o,double h,double l, double c, decimal v)
		{
			D=d;
			O=o;
			H=h;
			L=l;
			C=c;
			V=v;
		}

        public Bar(double price, DateTime dateTime)
        {
            D = dateTime;
            O = H = L = C = price;
            V = 0;
        }
        public void UpdatePrice(double price)
        {
            C = price;
            if(price > H)
                H = price;
            if(price < L)
                L = price;
        }
        public void UpdateVolume(decimal volume)
        {
            V = volume;
        }


        public static string BarSizeToString(TimeSpan interval)
        {
            string barSize;

            switch ((int)interval.TotalSeconds)
            {
                case 1:
                    barSize = "1 secs";
                    break;
                case 5:
                    barSize = "5 secs";
                    break;
                case 15:
                    barSize = "15 secs";
                    break;
                case 30:
                    barSize = "30 secs";
                    break;
                case 60:
                    barSize = "1 min";
                    break;
                case 120:
                    barSize = "2 mins";
                    break;
                case 180:
                    barSize = "3 mins";
                    break;
                case 300:
                    barSize = "5 mins";
                    break;
                case 900:
                    barSize = "15 mins";
                    break;
                case 1800:
                    barSize = "30 mins";
                    break;
                case 3600:
                    barSize = "1 hour";
                    break;
                case 86400:  //24 hours
                    barSize = "1 day";
                    break;
                case 604800:  //7 days
                    barSize = "1 week";
                    break;
                case 2592000: //30 days
                    barSize = "1 month";
                    break;
                default:
                    barSize = "1 mins";
                    break;
            }

            return barSize;
        }
    }
}
