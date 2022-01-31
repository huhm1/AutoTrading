using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using IBApi;

namespace AutoTrading.Lib
{
    public static class Client
    {
        public static EventWaitHandle OrderIDWH = new EventWaitHandle(true, EventResetMode.ManualReset);
        public static string FilePath = @".\..\Data\";

        public static string StrategyPath = @".\..\Data\Strategy\";

        public static EClientSocket Socket;
        public static EW ew =new EW();
        public static DateTime twsTime;
        public static CultureInfo culture = new CultureInfo("en-US", true);
        public static Account account;
        public static Dictionary<string,Strategy> StrategyList = new Dictionary<string, Strategy>();
        public static Logs Logs;
        private static readonly object locker = new object();
        public static bool Connected = false;
        public static List<string> mTWS = new List<string>();
        public static List<string> mTickers = new List<string>();
        public static List<string> mErrors = new List<string>();
        public static AutoTrading Form;

        private static int nextOrderID = 0;
        private static int tickerID = 1;
        private static EReaderMonitorSignal signal = new EReaderMonitorSignal();

        public static int TickerID
        {
            get { lock (locker) return tickerID++; }
            set { lock (locker) tickerID = value; }
        }
        public static int OrderID
        {
            get
            {
                OrderIDWH.WaitOne();
                lock (locker) return nextOrderID++;
            }
            set { lock (locker) nextOrderID = value; }
        }

        public static void Start()
        {
            Logs = new Logs("ClientLog.txt", false);
            Socket = new EClientSocket(ew, signal);
            if (Conncet())
            {
                Connected = true;
                EReader reader = new EReader(Socket, signal);

                reader.Start();

                new Thread(() => { while (Socket.IsConnected()) { signal.waitForSignal(); reader.processMsgs(); } }) { IsBackground = true }.Start();

                twsTime = DateTime.ParseExact(Socket.ServerTime, "yyyyMMdd HH:mm:ss 'EST'", culture);
                account = new Account();//
                if (!Account.PaperTrade)
                {
                    DisplayTWS("Connected to Real acount.");/////

                    //Close();
                    //return;
                }
                else
                    DisplayTWS("Connected to Papeer acount.");

                RunStrategy();
                Socket.reqAutoOpenOrders(true);
                Socket.reqOpenOrders();
                //RunStrategy();
            }
            else
                RunStrategy();

            //Logs.Close();
        }

        public static bool Conncet()
        {
            Socket.eConnect("127.0.0.1", 7496, 0);
            if (Socket.IsConnected())
            {
                DisplayTWS("Connected to Tws server version " + Socket.ServerVersion + " at " + Socket.ServerTime);
                return true;
           }
            DisplayTWS("Connect is failed.");
            return false;
        }

        public static void RunStrategy()
        {
            Strategy strategy;

//            Thread newThread = new Thread(StrategyTest);
//            newThread.Start();

            strategy = new StrategySample();
            StrategyList.Add(strategy.name, strategy);


            //EW.openOrderWH.Set();
        }
        public static void Close()
        {
            Socket.Close();
        }
        public static void DisplayErrors(string str)
        {
            Form.m_errors.Text += str + "\r\n";
        }
        public static void DisplayTWS(string str)
        {
            Form.m_TWS.Text += str + "\r\n";
        }
        public static void DisplaymTickers(string str)
        {
            Form.m_tickers.Text += str + "\r\n";
        }

    }
}
