GetLivePrices application

Brief
==================================================================================
This sample shows live quotes.
The sample performs the following actions:
1. Login. 
2. Subscribe to Market Data stream.
3. Print the received data.
4. UnSubscribe to Market Data stream.
5. Logout.

Building the application
==================================================================================
In order to build this application you will need MS Visual Studio 2015 and
.NET framework 4.5 or later.
You can download .NET framework from http://msdn.microsoft.com/en-us/netframework/

Running the application
==================================================================================
Change the App.config file by putting your information in the "appSettings" section.

Arguments
==================================================================================
{ACCESSTOKEN} - Your Access token. Mandatory argument.
{URL} - The RestAPI URL. Mandatory argument. For example, https://api-demo.fxcm.com 
{INSTRUMENT} - The instrument that you want to use in the sample.
        For example, "EUR/USD". Mandatory argument.