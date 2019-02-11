===============================
ForexConnect API Specifications
===============================

The ForexConnect SDK is designed to get trading data, trade, load price histories and subscribe for the most recent prices. 
It is intended to be used by FXCM clients on auto-trading robots and systems, 
chart and market analysis application, custom trading application on FXCM accounts.

ForexConnect supports ``Python``, ``C++``, ``C#``, ``Java``, ``VB``, ``VBA``, ``Windows``, ``Linux``, ``iOS`` and ``Android`` for mobile devices. And it is free.

You can use ForexConnect on Trading station account, no extra setup required.

.. note:: If using ``O2G2`` namespace, keep in mind that it is currently deprecated as it has not been updated since the beginning of 2015. It may give the users errors or not be compatible in certain cases.

Getting Started
===============

1) Open a `demo Trading Station II account <https://www.fxcm.com/uk/algorithmic-trading/api-trading/>`_.
2) Download `ForexConnect SDK <http://www.fxcodebase.com/wiki/index.php/Download/>`_
3) Examples codes and documents are included in the ForexConnectAPI packages after installation.
4) Online documents: :download:`Getting Started </_downloads/Getting_Started-fc.pdf>`
5) ForexConnect using :download:`Matlab </_downloads/FXCM-MATLAB-master.zip>`
6) ForexConnect sample code for `Android/iOS/macOS/Python/Linux/Windows <https://github.com/gehtsoft/forex-connect/tree/master/samples/>`_
7) ForexConnect using `Python <http://fxcodebase.com/code/viewforum.php?f=51/>`_

Top Development Platform IDEs
=============================

* Windows 32bit and 64bit – `Visual Studio 2005 and up <https://www.visualstudio.com/en-us/downloads/download-visual-studio-vs.aspx/>`_
* Linux 32bit and 64bit – `Eclipse <https://eclipse.org/>`_
* iOS – `Xcode <https://developer.apple.com/xcode/ide/>`_
* Android - `Android Studio <https://developer.android.com/studio/intro/index.html/>`_

Table manager vs Non-table manager
==================================

Table manager preload all tables to your local memory, it is an in-memory representation of API tables. The table manager allows you to subscribe to table change events such as updates, adding rows, or removing rows. It is important to note that the ``SummaryTable`` is only accessible through the table manager. ``Table manager`` presents a performance decrease because it is constantly recalculating fields.

``Non-table manager`` allow you to capture table updates adhoc via the use of a class that implements the ``IO2GResponseListener`` interface. It give performance advantage but you need to calculate some fields such as ``PipCost`` or ``P/L``.

Request Current Balance
=======================

You need to request the table from server. Please refer to ``NonTableManagerSamples\PrintTable`` example program:

.. code-block:: C#

   	private static O2GAccountRow GetAccount(O2GSession session)
      {
          O2GResponseReaderFactory readerFactory = session.getResponseReaderFactory();
          if (readerFactory == null)
          {
              throw new Exception("Cannot create response reader factory");
          }
          O2GLoginRules loginRules = session.getLoginRules();
          O2GResponse response = loginRules.getTableRefreshResponse(O2GTableType.Accounts);
          O2GAccountsTableResponseReader accountsResponseReader = readerFactory.createAccountsTableReader(response);
          for (int i = 0; i < accountsResponseReader.Count; i++)
          {
              O2GAccountRow accountRow = accountsResponseReader.getRow(i);
              Console.WriteLine("AccountID: {0}, Balance: {1}", accountRow.AccountID, accountRow.Balance);
          }
          return accountsResponseReader.getRow(0);
      }

Retrieve Price History
========================

For pricehistory, you need to use non-table manager. 
You can see examples under ``NonTableManagerSamples\GetHistPrices``

.. note::

	This is for personal use and abides by our `EULA <https://www.fxcm.com/uk/forms/eula/>`_.
	For more information, you may contact us at api@fxcm.com

Sample Codes
============

1. Learn how to build and backtest:

	* :download:`RSI Signals</_downloads/RsiSignals_via_ForexConnectAPI.zip>`
	* :download:`CCI Oscillator </_downloads/2.1.CCI_via_FC_API.zip>`
	* :download:`Breakout strategy </_downloads/3.1.BreakoutStrategy_via_FC_API.zip>`
	* :download:`Range Stochastic Strategy </_downloads/4.1.StochasticStrategy_via.FC.API.zip>`
	* :download:`Mean Reversion Strategy </_downloads/5.1.MeanReverionStrategy_via_FC_API.zip>`

2. Some examples like :download:`attached stop limit to position, create if-then ELS order, get rollover </_downloads/FC-examples-master-orders.zip>`

3. :download:`Historical data </_downloads/FXCMHDD-master.zip>` download

**Disclaimer**:

Trading forex/CFDs on margin carries a high level of risk and may not be suitable for all investors as you could sustain losses in excess of deposits. Leverage can work against you. The products are intended for retail and professional clients. Due to the certain restrictions imposed by the local law and regulation, German resident retail client(s) could sustain a total loss of deposited funds but are not subject to subsequent payment obligations beyond the deposited funds. Be aware and fully understand all risks associated with the market and trading. Prior to trading any products, carefully consider your financial situation and experience level. If you decide to trade products offered by FXCM Australia Pty. Limited (“FXCM AU”) (AFSL 309763), you must read and understand the `Financial Services Guide <https://docs.fxcorporate.com/financial-services-guide-au.pdf/>`_, `Product Disclosure Statement <https://www.fxcm.com/au/legal/product-disclosure-statements/>`_, and `Terms of Business <https://docs.fxcorporate.com/tob_au_en.pdf/>`_. Any opinions, news, research, analyses, prices, or other information is provided as general market commentary, and does not constitute investment advice. FXCM will not accept liability for any loss or damage, including without limitation to, any loss of profit, which may arise directly or indirectly from use of or reliance on such information. FXCM will not accept liability for any loss or damage, including without limitation to, any loss of profit, which may arise directly or indirectly from use of or reliance on such information.