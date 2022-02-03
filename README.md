# AutoTrading
I'm refactoring my old AutoTrding project and open source it.
If you are looking for a profitable stock trading strategy. Unfortunately, you won't find it in here, because I don't have one. But we can work together to find one.
This project coded by c#, working in multi thread, monitoring and analysis real time marketing data from Interactive Brokers API. It can automatically place trade order by designed trading strategy. It also includes a back testing function.


# How to use it?
First you need create your strategy class.
Then you need to register your strategy in Client.cs as  following
```sh
        public static void RunStrategy()
        {
            Strategy strategy;

            strategy = new StrategySample();
            StrategyList.Add(strategy.name, strategy);
        }
```
You can register multiple strategy at here.

you own strategy inherit from class Strategy, in your own strategy you need create contract first, then you can add different bars and indicators to it. In Decision() method use all attached bars and indicators with real time market data to make trading decision. 