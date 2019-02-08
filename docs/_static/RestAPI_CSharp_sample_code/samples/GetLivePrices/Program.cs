using System;
using System.Collections.Specialized;
using System.Configuration;
using FXCMRest;

namespace GetLivePrices
{
    class Program
    {
        static int count = 0;
        static void Main(string[] args)
        {
            Session session = null;
            try
            {
                SampleParams sampleParams = new SampleParams(ConfigurationManager.AppSettings);
                PrintSampleParams("GetLivePrices", sampleParams);

                session = new Session(sampleParams.AccessToken, sampleParams.Url);
                Console.WriteLine("Session status: Connecting");
                session.Connect();
                Console.WriteLine("Session status: Connected");

                session.SubscribeSymbol(sampleParams.Instrument);
                session.PriceUpdate += Session_PriceUpdate;
                Console.ReadLine();

                session.PriceUpdate -= Session_PriceUpdate;
                session.UnsubscribeSymbol(sampleParams.Instrument);
                session.Close();
                Console.WriteLine("Session status: Closed");
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
            }
        }
        static void Session_PriceUpdate(PriceUpdate priceUpdate)
        {
            Console.WriteLine(priceUpdate);
            if (++count % 5 == 0)
                Console.WriteLine("Press enter key to exit");
        }

        /// <summary>
        /// Print process name and sample parameters
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="loginPrm"></param>
        private static void PrintSampleParams(string procName, SampleParams sampleParams)
        {
            Console.WriteLine("{0}:\nAccessToken='{1}'\nUrl='{2}'\nInstrument='{3}'\n", procName, sampleParams.AccessToken, sampleParams.Url, sampleParams.Instrument);
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
            }
            /// <summary>
            /// Gets the Access token to use.
            /// </summary>
            public string AccessToken { get; }

            /// <summary>
            /// Gets the server RestAPI URL.
            /// </summary>
            public string Url { get; }

            /// <summary>
            /// Gets the Instrument.
            /// </summary>
            public string Instrument { get; }

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
        }
    }
}
