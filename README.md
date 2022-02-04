# AutoTrading
I'm refactoring my old AutoTrding project and open source it.

If you are looking for a profitable stock trading strategy. Unfortunately, you won't find it in here, because I don't have one. But we can work together to find one.

This project coded by c#, working in multi thread, monitoring and analysis real time marketing data from Interactive Brokers API. It can automatically place trade order by designed trading strategy. It also includes back testing function.

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

You own strategy manage a virtual account, this account include cash value and stock hold size, these information stored in a csv file. 

It inherit from abstract class Strategy, in your own strategy class you need create contract(the stock) first, then you can add different bars and indicators to it.

In Decision() method you can use all attached bars and indicators with real time market data, these will help you to make trading decision and place trade order. 

The Decision() method will associate with Stock.AfterPriceUpdate event, every time after the stock price be updated then it will trigger your Decision() method. 

After Decision() method placed trade order, if order status be updated, it will trigger the Settle() method to update your virtual account data.