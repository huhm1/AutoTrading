using System;
using System.Collections.Generic;

namespace CJava.Lib
{
    public class HLRate:Indicator
    {
        //public new string Name = "HLRate";
        private static double hlrLength = 1/38.0;
        public List<double> HL;
        public List<double> HLR;
        public double hlr;
        public double hl;
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
            hlr = lastBar != 0 ? hlr*(1 - hlrLength) + hl/barList[lastBar].H*hlrLength : hl/barList[lastBar].H;
            if (HL.Count - 1 < lastBar)
            {
                HL.Add(hl);
                HLR.Add(hlr);
            }
            else
            {
                HL[lastBar] = hl;
                HLR[lastBar] = hlr;
            }
        }

        public override void Initialize()
        {
            throw new NotImplementedException();
        }
    }
}
