using AutoTrading.Lib;
using System;
using System.Collections.Generic;

namespace AutoTrading.Indicators
{
    class CurveFit 
    {
        public static double PolyFit(ref List<Point> Points, int m,out double[] a)
        {
            int n = Points.Count;
            int np = 3;
            //m = 3;
            a = new double[m];
            int i, j, k;
            double p, c, g, q, d1=0, d2;
            double[] s = new double[m];
            double[] t = new double[m];
            double[] b = new double[m];
            double[] x = new double[np];
            try
            {
                for (i = 1; i < np; i++)
                {
                    x[i] = Points[i+n-np].BarSpan + x[i - 1];
                }

                /*
                            for (i = 0; i < m; i++)
                            {
                                a[i] = 0;
                            }
                */
                q = 0;
                b[0] = 1;
                d1 = np;
                p = 0;
                c = 0;

                //Points[n - 3].Span = 0;
                for (i = n - np; i < n; i++)
                {
                    //Points[i].GetSpan(Points[n-1].Time);
                    //Points[i] -= Points[n - 3];

                    p += x[i - n + np];
                    c += Points[i].Price;
                }
                c /= d1;
                p /= d1;
                a[0] = c*b[0];
                if (m > 1)
                {
                    t[1] = 1;
                    t[0] = -p;
                    c = 0;
                    g = 0;
                    d2 = 0;
                    for (i = n - np; i < n; i++)
                    {
                        q = x[i - n + np] - p;
                        d2 += q*q;
                        c += Points[i].Price*q;
                        g += x[i - n + np] * q * q;
                    }
                    c /= d2;
                    p = g/d2;
                    q = d2/d1;
                    d1 = d2;
                    a[1] = t[1]*c;
                    a[0] = c*t[0] + a[0];
                }
                for (j = 2; j < m; j++)
                {
                    s[j] = t[j - 1];
                    s[j - 1] = -p*t[j - 1] + t[j - 2];
                    if (j >= 3)
                    {
                        for (k = j - 2; k >= 1; k--)
                        {
                            s[k] = -p*t[k] + t[k - 1] - q*b[k];
                        }
                    }
                    s[0] = -p*t[0] - q*b[0];
                    d2 = 0;
                    c = 0;
                    g = 0;
                    for (i = n - np; i < n; i++)
                    {
                        q = s[j];
                        for (k = j - 1; k >= 0; k--)
                        {
                            q = q * x[i - n + np] + s[k];
                        }
                        d2 += q*q;
                        c += Points[i].Price*q;
                        g += x[i - n + np] * q * q;
                    }
                    c /= d2;
                    p = g/d2;
                    q = d2/d1;
                    d1 = d2;
                    a[j] = s[j]*c;
                    t[j] = s[j];
                    for (k = j - 1; k >= 0; k--)
                    {
                        a[k] = c*s[k] + a[k];
                        b[k] = t[k];
                        t[k] = s[k];
                    }
                }
                d1 = 0;
            }
            catch(Exception e)
            {
                Console.Write(e.ToString());

            }
            for (i = n - np; i < n; i++)
            {
                q = a[m - 1];
                for (k = m - 2; k >= 0; k--)
                {
                    q = a[k] + x[i - n + np] * q;

                }
                p = Points[i].Price - q;
                d1 += p * p;

            }

            //Points[n - 1].line.a = a;
            return d1;
        }

        public static double PolyFit(double[] x, double[] y, out double[] a, int m)
        {
            int n = x.Length;
            if (m >= n)
            {
                m = n - 1;
            }
            a = new double[m];
            int i, j, k;
            double p, c, g, q, d1, d2;
            double[] s = new double[m];
            double[] t = new double[m];
            double[] b = new double[m];

            for (i = 0; i < m; i++)
            {
                a[i] = 0;
            }
            q = 0;
            b[0] = 1;
            d1 = n;
            p = 0;
            c = 0;

            for (i = 0; i < n; i++)
            {
                p += x[i];
                c += y[i];
            }
            c /= d1;
            p /= d1;
            a[0] = c * b[0];
            if (m > 1)
            {
                t[1] = 1;
                t[0] = -p;
                c = 0;
                g = 0;
                d2 = 0;
                for (i = 0; i < n; i++)
                {
                    q = x[i] - p;
                    d2 += q * q;
                    c += y[i] * q;
                    g += x[i] * q * q;
                }
                c /= d2;
                p = g / d2;
                q = d2 / d1;
                d1 = d2;
                a[1] = t[1] * c;
                a[0] = c * t[0] + a[0];

            }
            for (j = 2; j < m; j++)
            {
                s[j] = t[j - 1];
                s[j - 1] = -p * t[j - 1] + t[j - 2];
                if (j >= 3)
                {
                    for (k = j - 2; k >= 1; k--)
                    {
                        s[k] = -p * t[k] + t[k - 1] - q * b[k];
                    }
                }
                s[0] = -p * t[0] - q * b[0];
                d2 = 0;
                c = 0;
                g = 0;
                for (i = 0; i < n; i++)
                {
                    q = s[j];
                    for (k = j - 1; k >= 0; k--)
                    {
                        q = q * x[i] + s[k];


                    }
                    d2 += q * q;
                    c += y[i] * q;
                    g += x[i] * q * q;
                }
                c /= d2;
                p = g / d2;
                q = d2 / d1;
                d1 = d2;
                a[j] = s[j] * c;
                t[j] = s[j];
                for (k = j - 1; k >= 0; k--)
                {
                    a[k] = c * s[k] + a[k];
                    b[k] = t[k];
                    t[k] = s[k];
                }
            }
            d1 = 0;
            for (i = 0; i < n; i++)
            {
                q = a[m - 1];
                for (k = m - 2; k >= 0; k--)
                {
                    q = a[k] + x[i] * q;

                }
                p = y[i] - q;
                d1 += p * p;

            }
            return d1;
        }

    }
}
