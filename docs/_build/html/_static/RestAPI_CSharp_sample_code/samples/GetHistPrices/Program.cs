using System;
using System.Collections.Specialized;
using System.Configuration;
using FXCMRest;
using FXCMRest.Models;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace GetHistPrices
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                SampleParams sampleParams = new SampleParams(ConfigurationManager.AppSettings);
                PrintSampleParams("GetHistPrices", sampleParams);

                var session = new Session(sampleParams.AccessToken, sampleParams.Url);
                Console.WriteLine("Session status: Connecting");
                session.Connect();
                Console.WriteLine("Session status: Connected\n");

                IList<Candle> candleHistory  = GetHistory(session, sampleParams);
                PrintHistory(candleHistory);

                session.Close();
                Console.WriteLine("\nSession status: Closed");
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
            }
        }

        private static IList<Candle> GetHistory(Session session, SampleParams sampleParams)
        {
            IList<Offer> offers = session.GetOffers();
            Offer offer = offers.FirstOrDefault(o => o.Currency == sampleParams.Instrument);
            if (offer == null)
            {
                throw new Exception(string.Format("The instrument '{0}' is not valid", sampleParams.Instrument));
            }

            if (sampleParams.DateFrom != DateTime.MinValue && sampleParams.DateTo == DateTime.MinValue)
            {
                throw new Exception(string.Format("Please provide DateTo in configuration file"));
            }

            return session.GetCandles(offer.OfferId, sampleParams.Timeframe, sampleParams.Count,
                sampleParams.DateFrom, sampleParams.DateTo);
        }

        private static void PrintHistory(IList<Candle> candleHistory)
        {
            foreach(Candle candle in candleHistory)
            {
                Console.WriteLine(string.Format("DateTime={0}, BidOpen={1}, BidClose={2}, AskOpen={3}, AskClose={4}",
                    candle.Timestamp, candle.BidOpen, candle.BidClose, candle.AskOpen, candle.AskClose));
            }
        }

        /// <summary>
        /// Print process name and sample parameters
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="sampleParams"></param>
        private static void PrintSampleParams(string procName, SampleParams sampleParams)
        {
            Console.WriteLine("{0}:\nAccessToken='{1}'\nUrl='{2}'\nInstrument='{3}'\nTimeframe='{4}'\nDateFrom='{5}'\nDateTo='{6}'\n",
                procName, sampleParams.AccessToken, sampleParams.Url, sampleParams.Instrument, sampleParams.Timeframe,
                sampleParams.DateFrom, sampleParams.DateTo);
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
                Instrument = GetRequiredArgument(args, "Instrument");
                Timeframe = GetArgument(args, "Timeframe") ?? "m1"; ;
                DateFrom = GetDateTime(args, "DateFrom");
                DateTo = GetDateTime(args, "DateTo");
                DateTo = DateTo == DateTime.MinValue ? DateTime.Now : DateTo;
                Count = GetCount(args);
            }

            /// <summary>
            /// Gets dateTime param from config file
            /// </summary>
            private DateTime GetDateTime(NameValueCollection args, string paramName)
            {
                string sDateFormat = "MM.dd.yyyy HH:mm:ss";
                string sDateTime = args[paramName];
                DateTime dateTime;
                if (!DateTime.TryParseExact(sDateTime, sDateFormat, CultureInfo.InvariantCulture,
                           DateTimeStyles.AssumeLocal, out dateTime))
                {
                    return DateTime.MinValue;
                }
                else
                {
                    if (DateTime.Compare(dateTime, DateTime.Now) >= 0)
                    {
                        throw new Exception(string.Format("\"{0}\" value {1} is invalid; please fix the value in the configuration file", paramName, sDateTime));
                    }
                }

                return dateTime;
            }

            /// <summary>
            /// Gets count candles param from config file
            /// </summary>
            private int GetCount(NameValueCollection args)
            {
                const int exceptValue = -1;
                string sCount = GetArgument(args, "Count") ?? "";

                if (string.IsNullOrEmpty(sCount) == true)
                {
                    return exceptValue;
                }

                int count = 0;
                if (!Int32.TryParse(sCount, out count))
                    return exceptValue;
                else
                    return count;
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
            /// Gets the Timeframe.
            /// </summary>
            public string Timeframe { get; }

            /// <summary>
            /// Gets the DateFrom.
            /// </summary>
            public DateTime DateFrom { get; }

            /// <summary>
            /// Gets the DateTo.
            /// </summary>
            public DateTime DateTo { get; }

            /// <summary>
            /// Number of candles.  If time range is specified, number of candles parameter is ignored.
            /// </summary>
            public int Count { get; }

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
