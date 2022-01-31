using System;
using System.Collections.Generic;

namespace AutoTrading.Lib
{
    public class HLRate:Indicator
    {
        //public new string Name = "HLRate";
        private static double hlrLength = 1/28.0;
        public List<double> HL;
        public List<double> HLR;
        public double hlr;
        public double hl;
        public double ahl;
        public override string Name
        {
            get { return "HLRate"; }
        }
        public HLRate()
        {
            HL = new List<double>();
            HLR = new List<double>();
        }

        public override void Update()
        {
            int lastBar = barList.Count - 1;
            hl = barList[lastBar].H - barList[lastBar].L;
            ahl = lastBar == 0 ? hl : HL[lastBar - 1] * (1 - hlrLength) + hl * hlrLength;
            hlr = lastBar == 0 ? hl / barList[lastBar].H : HLR[lastBar - 1] * (1 - hlrLength) + (hl / barList[lastBar].H) * hlrLength;
            if (HL.Count - 1 < lastBar)
            {
                HL.Add(ahl);
                HLR.Add(hlr);
            }
            else
            {
                HL[lastBar] = ahl;
                HLR[lastBar] = hlr;
            }
        }

        public override void Initialize()
        {
            throw new NotImplementedException();
        }
    }
}
