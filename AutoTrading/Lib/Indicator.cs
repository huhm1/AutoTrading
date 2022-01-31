
using System.Collections.Generic;

namespace AutoTrading.Lib
{
    abstract public class Indicator
    {
        public List<Bar> barList;
        abstract public string Name
        {
            get;
        }
        //abstract public void Calculate(List<Bar> barList);
        abstract public void Update();
        abstract public void Initialize();

    }
}

