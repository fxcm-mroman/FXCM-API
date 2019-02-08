using System;
using System.Collections.Specialized;
using System.Configuration;
using FXCMRest;
using FXCMRest.Models;
using System.Threading;

namespace OpenPosition
{
    class Program
    {
        static private EventWaitHandle SyncResponseEvent = new EventWaitHandle(false, EventResetMode.AutoReset);

        static void Main(string[] args)
        {
            try
            {
                SampleParams sampleParams = new SampleParams(ConfigurationManager.AppSettings);
                PrintSampleParams("OpenPosition", sampleParams);

                var session = new Session(sampleParams.AccessToken, sampleParams.Url);
                Console.WriteLine("Session status: Connecting");
                session.Connect();
                Console.WriteLine("Session status: Connected\n");

                session.Subscribe(TradingTable.OpenPosition);
                session.Subscribe(TradingTable.Order);
                session.OpenPositionUpdate += Session_OpenPositionUpdate;
                session.OrderUpdate += Session_OrderUpdate;

                CreateMarketOrder(session, sampleParams);
                if (!SyncResponseEvent.WaitOne(30000)) //wait 30 sec
                {
                    throw new Exception("Response waiting timeout expired");
                }

                session.Unsubscribe(TradingTable.OpenPosition);
                session.Unsubscribe(TradingTable.Order);
                session.OrderUpdate -= Session_OrderUpdate;
                session.OpenPositionUpdate -= Session_OpenPositionUpdate;
                session.Close();
                Console.WriteLine("\nSession status: Closed");
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
            }
        }

        private static void Session_OpenPositionUpdate(UpdateAction action, FXCMRest.Models.OpenPosition obj)
        {
            if (action == UpdateAction.Insert)
            {
                Console.WriteLine(string.Format("{0} Trade ID: {1}; Amount: {2}; Rate: {3}", Enum.GetName(typeof(UpdateAction), action),
                    obj.TradeId, obj.AmountK, obj.Open));
                SyncResponseEvent.Set();
            }
        }
        private static void Session_OrderUpdate(UpdateAction action, Order obj)
        {
            if (action == UpdateAction.Insert || action == UpdateAction.Delete)
            {
                Console.WriteLine(string.Format("{0} OrderID: {1}", Enum.GetName(typeof(UpdateAction), action), obj.OrderId));
            }
        }

        /// <summary>
        /// Create market Order using sample params 
        /// </summary>
        /// <param name="sampleParams"></param>
        private static void CreateMarketOrder(Session session, SampleParams sampleParams)
        {
            OpenTradeParams openTradeParams = new OpenTradeParams();

            if (!string.IsNullOrEmpty(sampleParams.Account))
            {
                openTradeParams.AccountId = sampleParams.Account;
            }
            else
            {
                var accounts = session.GetAccounts();
                foreach (var account in accounts)
                {
                    if (!string.IsNullOrEmpty(account.AccountId))
                    {
                        openTradeParams.AccountId = account.AccountId;
                        break;
                    }
                }
            }

            if (sampleParams.Lots != null)
                openTradeParams.Amount = sampleParams.Lots.Value;
            else
                openTradeParams.Amount = 1;
        
            openTradeParams.Symbol = sampleParams.Instrument;
            openTradeParams.IsBuy = sampleParams.BuySell == "B" ? true : false;
            openTradeParams.OrderType = "AtMarket";
            openTradeParams.TimeInForce = "GTC";

            session.OpenTrade(openTradeParams);
        }

        /// <summary>
        /// Print process name and sample parameters
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="loginPrm"></param>
        private static void PrintSampleParams(string procName, SampleParams sampleParams)
        {
            Console.WriteLine("{0}:\nAccessToken='{1}'\nUrl='{2}'\n", procName, sampleParams.AccessToken, sampleParams.Url);
        }

        /// <summary>
        ///  Sample parameters.
        /// </summary>
        class SampleParams
        {
            /// <summary>
            /// Construct using AppSettings
            /// </summary>
            /// <param name="args"></param>
            public SampleParams(NameValueCollection args)
            {
                AccessToken = GetRequiredArgument(args, "AccessToken");
                Url = GetRequiredArgument(args, "URL");
                BuySell = GetRequiredArgument(args, "BuySell");
                Instrument = GetRequiredArgument(args, "Instrument");
                Account = GetArgument(args, "Account");
                Lots = GetLots(args);
            }

            private int GetLots(NameValueCollection args)
            {
                string sLots = GetRequiredArgument(args, "Lots");
                int lots = 0;
                if (!Int32.TryParse(sLots, out lots))
                    return 1;
                else
                    return lots;
            }

            /// <summary>
            /// Gets the Access token to use.
            /// </summary>
            public string AccessToken { get;}

            /// <summary>
            /// Gets the Instrument RestAPI URL.
            /// </summary>
            public string Url { get; }

            /// <summary>
            /// Gets the Instrument for trade.
            /// </summary>
            public string Instrument { get; }

            /// <summary>
            /// Gets the type Buy or Sell (B or S) trade.
            /// </summary>
            public string BuySell { get; }

            /// <summary>
            /// Gets the trade amount in lots.
            /// </summary>
            public int? Lots { get; }

            /// <summary>
            /// Gets the AccountId.
            /// </summary>
            public string Account{ get; }

            /// <summary>
            /// Get required argument from configuration file
            /// </summary>
            /// <param name="args">Configuration file key-value collection</param>
            /// <param name="sArgumentName">Argument name (key) from configuration file</param>
            /// <returns>Argument value</returns>
            private string GetRequiredArgument(NameValueCollection args, string sArgumentName)
            {
                string sArgument = args[sArgumentName];
                if (!string.IsNullOrEmpty(sArgument))
                {
                    sArgument = sArgument.Trim();
                }
                if (string.IsNullOrEmpty(sArgument) || sArgument.IndexOfAny(new char[] { '{', '}'}) >= 0)
                {
                    throw new Exception(string.Format("Please provide {0} in configuration file", sArgumentName));
                }
                return sArgument;
            }


            /// <summary>
            /// Get argument from configuration file
            /// </summary>
            /// <param name="args">Configuration file key-value collection</param>
            /// <param name="sArgumentName">Argument name (key) from configuration file</param>
            /// <returns>Argument value</returns>
            private string GetArgument(NameValueCollection args, string sArgumentName)
            {
                string sArgument = args[sArgumentName];
                if (string.IsNullOrEmpty(sArgument) || sArgument.IndexOfAny(new char[] { '{', '}' }) >= 0)
                {
                    sArgument = "";
                }

                return sArgument;
            }
        }
    }
}
