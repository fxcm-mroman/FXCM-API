=======================
JAVA API Specifications
=======================

Java trading SDK, a wrapper SDK of FIX API, provides clients with a fully functioning programmable API into the FXCM FX trading platform. The API’s main features are streaming executable FX trading prices, the ability to open/close positions and entry orders as well as set/update/delete stops ands limits. The API Object model is based on the FIX specification for FX. It is scalable, light and robust and is compatible on any Java-compliant operating system.

Getting Started
===============

* Open a demo `Trading Station II <https://www.fxcm.com/uk/algorithmic-trading/api-trading/>`_ account.	
* Download the package `here <https://apiwiki.fxcorporate.com/api/java/trading_sdk.zip/>`_.	
* Documents are in the package at ``trading_sdk\fxcm-api\javadoc``.	
* Sample code at ``trading_sdk\fxcm-api\src\QATest.java``	

Running QATest.java
===================

To run the program, it needs to be passed as below arguments:

	``loginid`` ``loginpwd`` ``connection_name`` ``hostUrl`` ``test_command``
	   	     	
	   * loginid: your Trading Station username
	   * loginpwd: your Trading station password
	   * connection_name: ``Demo`` or ``Real`` 
	   * hostUrl: http://www.fxcorporate.com/Hosts.jsp
	
 	``test_command`` is one of the following::
	
	   LISTEN:    Just listen for message, do not do anything
	   CMO:       createMarketOrder (previously quoted)
	   SSLMO:     set Stop/Limit on an open position
	   USLMO:     update Stop/Limit price on a positon 
	   DSLMO:     delete Stop/Limit from a position
	   CEO:       create entry order 
	   SSLEO:     set Stop/Limit on an entry order
	   USLEO:     update Stop/Limit on an entry order
	   DSLEO:     remove Stop/Limit on an entry order
	   DEO:       remove Entry Order
	   CLOSEMO:   close positon
	   UREO:      Update rate on an entry order
	   MDH:	   Retrieve Marke data history
	   RECONNECT: Reconnect the session

Login
=====

.. code-block:: java

    private void setup(IGenericMessageListener aGenericListener, boolean aPrintStatus) {
        try {
		// step 1: get an instance of IGateway from the GatewayFactory
            if (mFxcmGateway == null) {
                mFxcmGateway = GatewayFactory.createGateway();
            }
            /*
                step 2: register a generic message listener with the gateway, this
                listener in particular gets all messages that are related to the trading
                platform Quote,OrderSingle,ExecutionReport, etc...
            */
            mFxcmGateway.registerGenericMessageListener(aGenericListener);
            mStatusListener = new DefaultStatusListener(aPrintStatus);
            mFxcmGateway.registerStatusMessageListener(mStatusListener);
            if (!mFxcmGateway.isConnected()) {
                System.out.println("client: login");
                FXCMLoginProperties properties = new FXCMLoginProperties(mUsername, mPassword, mStation, mServer, mConfigFile);
                /*
                    step 3: call login on the gateway, this method takes an instance of FXCMLoginProperties
                    which takes 4 parameters: username,password,terminal and server or path to a Hosts.xml
                    file which it uses for resolving servers. As soon as the login  method executes your listeners begin
                    receiving asynch messages from the FXCM servers.
                */
                mFxcmGateway.login(properties);
            }
            //after login you must retrieve your trading session status and get accounts to receive messages
            mFxcmGateway.requestTradingSessionStatus();
            mAccountMassID = mFxcmGateway.requestAccounts();
        } catch (Exception e) {
            e.printStackTrace();
        }
    }


Rollover
========

With Java API you can get the current rollover for each symbol, it can be done with the functions ``getFXCMSymInterestBuy()`` and ``getFXCMSymInterestSell()`` from ``TradingSecurity Class``,  for Long and Short positions:
::

	For example:
	getFXCMSymInterestBuy() = 0.12     you will get $0.12 for 10k
	getFXCMSymInterestSell() = -0.39    you will pay $0.39  for 10k

	The 10k in this example is the server default base unit size, it can be 
	found with FXCMParamValue where FXCMParamName = “BASE_UNIT_SIZE”
	
Sample Codes
============
	
	1. :download:`RSI signal and back testing </_downloads/FXCM_Java_API_Tutorial_RsiSignal_Strategy.zip>` strategy
	
	2. :download:`CCI Oscillator </_downloads/CCIOscillatorStrategy-2.zip>` strategy
	
	3. :download:`Breakout </_downloads/BreakOutStrategy_JavaAPI.zip>` strategy
 
	4. :download:`Range Stochastic </_downloads/RangeStochasticStrategy.zip>` strategy

	5. :download:`Mean Reversion </_downloads/MeanReversionStrategy.zip>`

.. note::

	This is for personal use and abides by our `EULA <https://www.fxcm.com/uk/forms/eula/>`_

	For more information, you may contact us at api@fxcm.com

**Release Notes**:

	Build.number = 260: Roll up of all previous builds, plus fixes for range entry order with Good Til Date semantics;

**Disclaimer**:

CFDs are complex instruments and come with a high risk of losing money rapidly due to leverage.
73.62% of retail investor accounts lose money when trading CFDs with this provider.
You should consider whether you understand how CFDs work and whether you can afford to take the high risk of losing your money.
High Risk Investment Notice: Trading Forex/CFD's on margin carries a high level of risk and may not be suitable for all investors as you could sustain losses in excess of deposits. The products are intended for retail, professional and eligible counterparty clients. For clients who maintain account(s) with Forex Capital Markets Limited (“FXCM LTD”), retail clients could sustain a total loss of deposited funds but are not subject to subsequent payment obligations beyond the deposited funds and professional clients could sustain losses in excess of deposits. Prior to trading any products offered by FXCM LTD, inclusive of all EU branches, FXCM Australia Pty. Limited, FXCM South Africa (PTY) Ltd, any affiliates of aforementioned firms, or other firms within the FXCM group of companies [collectively the "FXCM Group"], carefully consider your financial situation and experience level. If you decide to trade products offered by FXCM Australia Pty. Limited ("FXCM AU") (AFSL 309763), you must read and understand the Financial Services Guide, Product Disclosure Statement and Terms of Business. Our Forex/CFD prices are set by FXCM, are not made on an Exchange and are not governed under the Financial Advisory and Intermediary Services Act. The FXCM Group may provide general commentary which is not intended as investment advice and must not be construed as such. Seek advice from a separate financial advisor. The FXCM Group assumes no liability for errors, inaccuracies or omissions; does not warrant the accuracy, completeness of information, text, graphics, links or other items contained within these materials. Read and understand the Terms and Conditions on the FXCM Group’s websites prior to taking further action.