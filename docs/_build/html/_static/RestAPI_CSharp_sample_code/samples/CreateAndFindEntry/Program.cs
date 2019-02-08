using System;
using System.Collections.Specialized;
using System.Configuration;
using FXCMRest;
using FXCMRest.Models;
using System.Collections.Generic;
using System.Linq;

namespace CreateAndFindEntry
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                SampleParams sampleParams = new SampleParams(ConfigurationManager.AppSettings);
                PrintSampleParams("CreateAndFindEntry", sampleParams);

                var session = new Session(sampleParams.AccessToken, sampleParams.Url);
                Console.WriteLine("Session status: Connecting");
                session.Connect();
                Console.WriteLine("Session status: Connected\n");

                IList<Offer> offers = session.GetOffers();
                Offer offer = offers.FirstOrDefault(o => o.Currency == sampleParams.Instrument);

                if (offer == null)
                {
                    throw new Exception(string.Format("The instrument '{0}' is not valid", sampleParams.Instrument));
                }

                double rate = CalculateRate(sampleParams.OrderType, sampleParams.BuySell, offer.Buy, offer.Sell, offer.Pip);
                string orderid = CreateEntryOrder(session, sampleParams, rate);

                Console.WriteLine("You have successfully created an entry order for instrument {0}", sampleParams.Instrument);
                Console.WriteLine("Your order ID is {0}\n", orderid);

                Order order = FindOrder(session, orderid);
                PrintOrder(order);

                session.Close();
                Console.WriteLine("\nSession status: Closed");
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
            }
        }

        private static void PrintOrder(Order order)
        {
            Console.WriteLine("Information for OrderID = {0}", order.OrderId);
            Console.WriteLine("Account: {0}", order.AccountId);
            Console.WriteLine("Amount: {0}", order.AmountK);
            Console.WriteLine("Rate: {0}", order.IsBuy ? order.Buy : order.Sell);
            Console.WriteLine("Type: {0}", order.Type);
            Console.WriteLine("Buy/Sell: {0}", order.IsBuy ? "B" : "S");
            Console.WriteLine("Status: {0}", order.Status);
        }

        private static Order FindOrder(Session session, string orderid)
        {
            IList<Order> orders = session.GetOrders();
            return orders.FirstOrDefault(o => string.Equals(o.OrderId, orderid));
        }

        /// <summary>
        /// For the purpose of this example we will place entry order 10 pips from the current market price
        /// </summary>
        /// <param name="sOrderType"></param>
        /// <param name="sBuySell"></param>
        /// <param name="dAsk"></param>
        /// <param name="dBid"></param>
        /// <param name="dPointSize"></param>
        /// <returns>rate</returns>
        private static double CalculateRate(string sOrderType, string sBuySell, double dAsk, double dBid, double dPointSize)
        {
            double dRate = 0D;
            if (sOrderType.Equals("LE"))
            {
                if (sBuySell.Equals("B"))
                {
                    dRate = dAsk - 10 * dPointSize;
                }
                else
                {
                    dRate = dBid + 10 * dPointSize;
                }
            }
            else
            {
                if (sBuySell.Equals("B"))
                {
                    dRate = dAsk + 10 * dPointSize;
                }
                else
                {
                    dRate = dBid - 10 * dPointSize;
                }
            }
            return dRate;
        }

        /// <summary>
        /// Create entry order using sample params 
        /// </summary>
        /// <param name="sampleParams"></param>
        /// <param name="rate"></param>
        private static string CreateEntryOrder(Session session, SampleParams sampleParams, double rate)
        {
            CreateEntryOrderParams createEntryOrderParams = new CreateEntryOrderParams();

            if (!string.IsNullOrEmpty(sampleParams.Account))
            {
                createEntryOrderParams.AccountId = sampleParams.Account;
            }
            else
            {
                var accounts = session.GetAccounts();
                foreach (var account in accounts)
                {
                    if (!string.IsNullOrEmpty(account.AccountId))
                    {
                        createEntryOrderParams.AccountId = account.AccountId;
                        break;
                    }
                }
            }

            if (sampleParams.Lots != null)
                createEntryOrderParams.Amount = sampleParams.Lots.Value;
            else
                createEntryOrderParams.Amount = 1;

            createEntryOrderParams.Rate = rate;
            createEntryOrderParams.Symbol = sampleParams.Instrument;
            createEntryOrderParams.IsBuy = sampleParams.BuySell == "B" ? true : false;
            createEntryOrderParams.OrderType = "Entry";
            createEntryOrderParams.TimeInForce = "GTC";

            return session.CreateEntryOrder(createEntryOrderParams);
        }

        /// <summary>
        /// Print process name and sample parameters
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="sampleParams"></param>
        private static void PrintSampleParams(string procName, SampleParams sampleParams)
        {
            Console.WriteLine("{0}:\nAccessToken='{1}'\nUrl='{2}'\nInstrument='{3}'\nBuySell='{4}'\nOrderType='{5}'\nLots='{6}'\n", 
                procName, sampleParams.AccessToken, sampleParams.Url, sampleParams.Instrument, sampleParams.BuySell,
                sampleParams.OrderType, sampleParams.Lots);
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
                Account = GetArgument(args, "Account") ?? ""; ;
                OrderType = GetArgument(args, "OrderType") ?? "LE";
                Lots = GetLots(args);
            }

            /// <summary>
            /// Gets lots param from config file
            /// </summary>
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
            public string AccessToken { get; }

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
            public string Account { get; }

            /// <summary>
            /// Gets the OrderType.
            /// </summary>
            public string OrderType { get; }

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
                if (string.IsNullOrEmpty(sArgument) || sArgument.IndexOfAny(new char[] { '{', '}' }) >= 0)
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
                    sArgument = null;
                }

                return sArgument;
            }
        }
    }
}
