using AutoTrading.Lib;
using System;
using System.Collections.Generic;

namespace AutoTrading.Indicators
{
    class IndicatorA:Indicator
    {
        //public new string Name = "A";
        public List<double> HL;
        public double hl;
        public override string Name
        {
            get { return "A"; }
        }
        public IndicatorA()
        {
            HL = new List<double>();
        }

        override public void Update()
        {
            int last = barList.Count-1;
            hl = barList[last].H - barList[last].L;
            if (HL.Count - 1 < last)
                HL.Add(hl);
            else
                HL[last] = hl;

        }

        public override void Initialize()
        {
            throw new NotImplementedException();
        }
    }
}

