CreateAndFindEntry application

Brief
==================================================================================
This sample shows how to create an entry order and find it in the orders table.
The sample performs the following actions:
1. Login.
2. Create an entry order.
3. Find the newly created order and print information about it.
4. Logout.

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
{INSTRUMENT} - An instrument, for which you want to create an order.
        For example, EUR/USD. Mandatory argument.
{BUYSELL} - The order direction. Possible values are: B - buy, S - sell. Mandatory argument.
{ORDERTYPE} - Type of an entry order. Optional argument.
        Possible values are: LE - entry limit, SE - entry stop. Default value is LE.
{LOTS} - Trade amount in lots. Optional argument.
        For example, 2.
{ACCOUNT} - Your Account ID. Optional argument.