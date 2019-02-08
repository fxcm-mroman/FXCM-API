========
Overview
========

Java trading SDK, a wrapper SDK of FIX API, provides clients with a fully functioning programmable API into the FXCM FX trading platform. The API’s main features are streaming executable FX trading prices, the ability to open/close positions and entry orders as well as set/update/delete stops ands limits. The API Object model is based on the FIX specification for FX. It is scalable, light and robust and is compatible on any Java-compliant operating system.

Getting Started
===============

* A FXCM account. You can apply for a demo account `here <https://www.fxcm.com/uk/algorithmic-trading/api-trading/>`_.	
* Download the package `here <https://apiwiki.fxcorporate.com/api/java/trading_sdk.zip/>`_.	
* Documents are in the package at trading_sdk\fxcm-api\javadoc.	
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
	
	1. `RSI signal and back testing 	<https://apiwiki.fxcorporate.com/api/StrategyRealCaseStudy/JavaAPI/FXCM_Java_API_Tutorial_RsiSignal_Strategy.zip/>`_ strategy. 
	
	2. `CCI Oscillator <https://apiwiki.fxcorporate.com/api/StrategyRealCaseStudy/JavaAPI/CCIOscillatorStrategy-2.zip/>`_ strategy.
	
	3. `Breakout <https://apiwiki.fxcorporate.com/api/StrategyRealCaseStudy/JavaAPI/BreakOutStrategy_JavaAPI.zip/>`_ strategy.
 
	4. `Range Stochastic <https://apiwiki.fxcorporate.com/api/StrategyRealCaseStudy/JavaAPI/RangeStochasticStrategy.zip/>`_ strategy .

	5. `Mean Reversion <https://apiwiki.fxcorporate.com/api/StrategyRealCaseStudy/JavaAPI/MeanReversionStrategy.zip/>`_ strategy.

.. note::

	This is for personal use and abides by our `EULA <https://www.fxcm.com/uk/forms/eula/>`_

	For more information, you may contact us at api@fxcm.com

**Release Notes**:

	Build.number = 260: Roll up of all previous builds, plus fixes for range entry order with Good Til Date semantics;

**Disclaimer**:

Trading forex/CFDs on margin carries a high level of risk and may not be suitable for all investors as you could sustain losses in excess of deposits. Leverage can work against you. The products are intended for retail and professional clients. Due to the certain restrictions imposed by the local law and regulation, German resident retail client(s) could sustain a total loss of deposited funds but are not subject to subsequent payment obligations beyond the deposited funds. Be aware and fully understand all risks associated with the market and trading. Prior to trading any products, carefully consider your financial situation and experience level. If you decide to trade products offered by FXCM Australia Pty. Limited (“FXCM AU”) (AFSL 309763), you must read and understand the `Financial Services Guide <https://docs.fxcorporate.com/financial-services-guide-au.pdf/>`_, `Product Disclosure Statement <https://www.fxcm.com/au/legal/product-disclosure-statements/>`_, and `Terms of Business <https://docs.fxcorporate.com/tob_au_en.pdf/>`_. Any opinions, news, research, analyses, prices, or other information is provided as general market commentary, and does not constitute investment advice. FXCM will not accept liability for any loss or damage, including without limitation to, any loss of profit, which may arise directly or indirectly from use of or reliance on such information. FXCM will not accept liability for any loss or damage, including without limitation to, any loss of profit, which may arise directly or indirectly from use of or reliance on such information.