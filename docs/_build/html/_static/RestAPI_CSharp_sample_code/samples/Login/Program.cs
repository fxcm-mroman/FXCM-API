using System;
using System.Collections.Specialized;
using System.Configuration;
using FXCMRest;

namespace Login
{
    class Program
    {
        static void Main(string[] args)
        {          
            try
            {
                LoginParams loginParams = new LoginParams(ConfigurationManager.AppSettings);
                PrintSampleParams("Login", loginParams);

                var session = new Session(loginParams.AccessToken, loginParams.Url);
                Console.WriteLine("Session status: Connecting");
                session.Connect();
                Console.WriteLine("Session status: Connected");

                PrintAccounts(session);

                session.Close();
                Console.WriteLine("Session status: Closed");
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
            }
        }

        /// <summary>
        /// Print process name and sample parameters
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="loginPrm"></param>
        private static void PrintSampleParams(string procName, LoginParams loginParams)
        {
            Console.WriteLine("{0}:\nAccessToken='{1}'\nUrl='{2}'\n", procName, loginParams.AccessToken, loginParams.Url);
        }

        /// <summary>
        /// Print accounts table
        /// </summary>
        /// <param name="session"></param>
        private static void PrintAccounts(Session session)
        {
            var accounts = session.GetAccounts();

            foreach(var account in accounts)
            {
                if (!string.IsNullOrEmpty(account.AccountId))
                    Console.WriteLine("AccountID: {0}, Balance: {1}", account.AccountId, account.Balance);
            }
        }

        /// <summary>
        ///  Login parameters.
        /// </summary>
        class LoginParams
        {
            /// <summary>
            /// Construct using AppSettings
            /// </summary>
            /// <param name="args"></param>
            public LoginParams(NameValueCollection args)
            {
                AccessToken = GetRequiredArgument(args, "AccessToken");
                Url = GetRequiredArgument(args, "URL");
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
