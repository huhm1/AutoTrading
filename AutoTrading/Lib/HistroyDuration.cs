using System;
using System.Collections.Generic;
using System.Text;

namespace AutoTrading.Lib
{
    class HistroyDuration
    {
        public string BarSizeStr;
        public TimeSpan Duration;
        public string DurationStr;

        public HistroyDuration(TimeSpan interval, TimeSpan marketStartTime, TimeSpan marketEndTime)
        {
            switch ((int)interval.TotalSeconds)
            {
                case 1:
                    BarSizeStr = "1 secs";
                    Duration = TimeSpan.FromSeconds(1*2000);
                    DurationStr = "2000 S";
                    break;
                case 5:
                    BarSizeStr = "5 secs";
                    Duration = TimeSpan.FromSeconds(5*2000);
                    DurationStr = "10000 S";
                    break;
                case 15:
                    BarSizeStr = "15 secs";
                    Duration = TimeSpan.FromSeconds(15*2000);
                    DurationStr = "30000 S";
                    break;
                case 30:
                    BarSizeStr = "30 secs";
                    Duration = TimeSpan.FromSeconds(86400);
                    DurationStr = "86400 S";
                    break;
                case 60:
                    BarSizeStr = "1 min";
                    Duration = TimeSpan.FromDays(6);
                    DurationStr = "6 D";
                    break;
                case 120:
                    BarSizeStr = "2 mins";
                    Duration = TimeSpan.FromDays(6);
                    DurationStr = "6 D";
                    break;
                case 300:
                    BarSizeStr = "5 mins";
                    Duration = TimeSpan.FromDays(7);
                    DurationStr = "6 D";
                    break;
                case 900:
                    BarSizeStr = "15 mins";
                    Duration = TimeSpan.FromDays(20);
                    DurationStr = "20 D";
                    break;
                case 1800:
                    BarSizeStr = "30 mins";
                    Duration = TimeSpan.FromDays(34);
                    DurationStr = "34 D";
                    break;
                case 3600:
                    BarSizeStr = "1 hour";
                    Duration = TimeSpan.FromDays(34);
                    DurationStr = "34 D";
                    break;
                case 86400:  //24 hours
                    BarSizeStr = "1 day";
                    Duration = TimeSpan.FromDays(364);
                    DurationStr = "52 W";
                    break;
                default:
                    BarSizeStr = "1 mins";
                    Duration = TimeSpan.FromSeconds(2000);
                    DurationStr = "2000 S";
                    break;
            }
        }
    }
}
/*
Bar size		Max duration
--------		------------
1 sec		2000 S
5 sec		10000 S
15 sec		30000 S
30 sec		86400 S
1 minute		86400 S		6 D
2 minutes		86400 S		6 D
5 minutes		86400 S		6 D
15 minutes		86400 S		20 D		2 W
30 minutes		86400 S		34 D		4 W		1 M
1 hour		86400 S		34 D		4 W		1 M
1 day		60 D		12 M		52 W		1 Y

*/