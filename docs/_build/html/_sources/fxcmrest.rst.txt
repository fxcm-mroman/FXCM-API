=================
REST Sample Codes
=================

Connecting
==========

1. `FXCM REST API Demonstration <https://github.com/fxcm/RestAPI/blob/master/Europe-Algo-Meetup/FXCM-REST-API-Demonstration.ipynb/>`_

2. `Quick Start.ipynb <https://github.com/fxcm/RestAPI/blob/master/fxcmpy-doc/00_quick_start.ipynb/>`_

3. `Connecting.ipynb <https://github.com/fxcm/RestAPI/blob/master/fxcmpy-doc/01_connecting.ipynb/>`_


Technical Analysis Sample Codes
===============================

1. `bt backtest <https://apiwiki.fxcorporate.com/api/StrategyRealCaseStudy/RestAPI/BT strategy on FXCM data.zip/>`_ using FXCM historical data. What is `bt <http://pmorissette.github.io/bt/>`_?

2. `QSTrader <https://apiwiki.fxcorporate.com/api/StrategyRealCaseStudy/RestAPI/QSTrader on FXCM data.zip/>`_ using FXCM data. What is `QSTrader <https://www.quantstart.com/qstrader/>`_?

3. `RSI strategy <https://apiwiki.fxcorporate.com/api/StrategyRealCaseStudy/RestAPI/RsiStrategy.zip/>`_.
	
4. `Moving Average Crossover strategy <https://apiwiki.fxcorporate.com/api/StrategyRealCaseStudy/RestAPI/Moving_Average_Crossover_Strategy.zip/>`_.
	
5. `Video demonstration <https://www.youtube.com/watch?v=m6llfznP4d4/>`_ on how to backtest strategies in Visual Studio using FXCM data on QuantConnect LEAN platform.

PHP
====

``FxcmRest`` is a library for event-driven trading with FXCM over RestAPI using ``ReactPHP``.

Requirements
------------

  PHP 7.0.2+

Installation
------------

The recommended way to install FxcmRest is through `Composer <https://getcomposer.org/>`_.

This command will install the latest stable version:

::

	$ composer require fxcm/fxcmrest


Usage
-----

As FXCM Rest API requires you to keep Socket.IO connection open through the whole time it is used, this library must be run within a php script and not as part of php generated website.

Interaction can be done either by using a console or through HTTP requests handled directly by the php script for example with ``\React\HTTP``.

Main class of the library is ``\FxcmRest\FxcmRest``. It must be instantiated with two objects:

	* ``\React\EventLoop\LoopInterface``
	* ``\FxcmRest\Config``

Configuration class ``\FxcmRest\Config`` must be instantiated with an array containing at least the two following parameters:

	* ``host``
	* ``token``

Configuration Parameters
------------------------
	* ``protocol`` - either ``\FxcmRest\Protocol::HTTPS`` (default) or ``\FxcmRest\Protocol::HTTP``
	* ``host`` - either ``api.fxcm.com`` for Real accounts or ``api-demo.fxcm.com`` for Demo accounts
 	* ``port`` - port number. ``443`` default
 	* ``token`` - 40 char hexadecimal string

Functions
---------
.. code-block:: php

	connect() : null

1. Opens a connection to the server. When connection is complete, ``connected`` signal will be emitted.

.. code-block:: php

	disconnect() : null

2. Disconnects from the server. When disconnection is complete, ``disconnected`` signal will be emitted. 

.. code-block:: php

	socketID() : string

3. If connected to the server, returns a string representing the socketID. If not connected, returns an empty string.

.. code-block:: php

	request(\FxcmRest\HttpMethod $method, string $path, array $arguments, callable $callback) : null

4. Sends a http request to the server. When request is completed, ``$callback`` will be called with two parameters:

	* ``int`` representing HTTP status code. 200 for OK
	* ``string`` representing server answer body

.. code-block:: Java

	on(string $signalName, callable $callback) : null

5. Registers a ``$callback`` for a signal of ``$signalName``. For a list of signals and parameters that are passed with them please see **Signals** section.
 
Signals
-------
1. ``connected`` - Emitted when connection sequence is complete. After this socketID is valid and requests can be sent to the server. No parameters are passed.

2. ``disconnected`` - Emitted when connection to the server is closed. No parameters are passed.

3. ``error`` - Emitted on errors. Passes error description as string.

4. ``[Offer,OpenPosition,ClosedPosition,Account,Summary,Properties]`` - Emmited on trading table changes. Passes table update contents as JSON string. Requires subscription through ``/trading/subscribe``

5. ``(EUR/USD,EUR/GBP,...)`` - Emmited on price update. Passes the price update as a JSON string. Requires subscription through ``/subscribe``.

PHP Sample Code
---------------

.. code-block:: php

    <?php
    require_once __DIR__ . '/vendor/autoload.php';

    $loop = \React\EventLoop\Factory::create();

    $config = new \FxcmRest\Config([
        'host' => 'api-demo.fxcm.com',
        'token' => 'YOUR_TOKEN',
    ]);

    $counter = 0;
    $rest = new \FxcmRest\FxcmRest($loop, $config);
    $rest->on('connected', function() use ($rest,&$counter) {
        $rest->request('POST', '/subscribe',
            ['pairs' => 'EUR/USD'],
            function($code, $data) use ($rest,&$counter) {
                if($code === 200) {
                    $rest->on('EUR/USD', function($data) use ($rest,&$counter) {
                        echo "price update: {$data}\n";
                        $counter++;
                        if($counter === 5){
                            $rest->disconnect();
                        }
                    });
                }
            }
        );
    });
    $rest->on('error', function($e) use ($loop) {
        echo "socket error: {$e}\n";
        $loop->stop();
    });
    $rest->on('disconnected', function() use ($loop) {
        echo "FxcmRest disconnected\n";
        $loop->stop();
    });
    $rest->connect();

    $loop->run();
    ?>


Python
======

.. note:: REST API Python code sample - **fxcm-api-rest-python3-example**. Clone this repository by clicking `here <https://github.com/fxcm/RestAPI/tree/master/fxcm-api-rest-python3-example/>`_.

Getting started
---------------
1. Install `Python <https://www.python.org/>`_.
2. Run: ``pip install -r requirements.txt``
3. Within the ``fxcm_rest.json`` file:

   *  Set log path via the ``logpath`` field
   *  Set ``debugLevel`` if desired
   *  Set subscription lists if desired
4. In the ``fxcm_rest_client_sample.py`` file:

   *  Set your token and environment (demo/real)

Using Jupyter Notebook
----------------------
1. Install `Python <https://www.python.org/>`_.
2. Run: ``pip install jupyter`` (if you don't have jupyter installed already)
3. Run: ``pip install -r requirements.txt``
4. In this directory run: ``jupyter notebook``
5. Start the ``RestApiNotebook.ipynb``.

Details
-------

This API exposes the methods of the REST API as a class, dealing with all of the common tasks 
involved with setting up connections and wiring callback listeners for you. In addition to that
there are a few convenience methods. 

A quick example is as follows:

.. code-block:: python

    import fxcm_rest_api_token
    import time
    trader = fxcm_rest_api_token.Trader('YOURTOKEN', 'prod')
    trader.login()

    #### Open Market Order
    # query account details and use the first account found
    accounts = trader.get_model("Account")
    account_id = accounts['accounts'][0]['accountId']
    # Open 10 lots on USD/JPY for the first account_id found.
    response = trader.open_trade(account_id, "USD/JPY", True, 10)
    if response['status']:
    # close all USD/JPY trades.
      response = trader.close_all_for_symbol("USD/JPY")

    #### Historical Data request
    basic = trader.candles("USD/JPY", "m1", 5)
    print(basic)
    date_fmt = trader.candles("USD/JPY", "m1", 5, dt_fmt="%Y/%m/%d %H:%M:%S")
    print(date_fmt)
    date_fmt_headers = trader.candles_as_dict("USD/JPY", "m1", 3, dt_fmt="%Y/%m/%d %H:%M:%S")
    print(date_fmt_headers)
    ##### Price subscriptions
    subscription_result = trader.subscribe_symbol("USD/JPY")

    # Define alternative price update handler and supply that.
    def pupdate(msg):
        print("Price update: ", msg)
    subscription_result = trader.subscribe_symbol("USD/JPY", pupdate)
    counter = 1
    while counter < 60:
        time.sleep(1)
        counter += 1 
  
Candles
-------

All calls to candles allow either ``instrument name``, or ``offerId``. They also allow the ``From`` and ``To`` to be specified as timestamp or a date/time format that will be interpreted ("2017/08/01 10:00", "Aug 1, 2017 10:00", etc.).
In addition to ``instrument_id``, ``response``, ``period_id`` and ``candles``, a ``headers`` field (not documented in the API notes) is returned, representing the candle fields.

``basic``

::


    for item in basic['candles']: 
        print item
    
    [1503694500, 109.317, 109.336, 109.336, 109.314, 109.346, 109.366, 109.373, 109.344, 72]
    [1503694560, 109.336, 109.321, 109.337, 109.317, 109.366, 109.359, 109.366, 109.354, 83]
    [1503694620, 109.321, 109.326, 109.326, 109.316, 109.359, 109.358, 109.362, 109.357, 28]
	
``date_fmt``

::

    for item in date_fmt['candles']:
        print item
    
    [1503694500, 109.317, 109.336, 109.336, 109.314, 109.346, 109.366, 109.373, 109.344, 72, '2017/08/26 05:55:00']
    [1503694560, 109.336, 109.321, 109.337, 109.317, 109.366, 109.359, 109.366, 109.354, 83, '2017/08/26 05:56:00']
    [1503694620, 109.321, 109.326, 109.326, 109.316, 109.359, 109.358, 109.362, 109.357, 28, '2017/08/26 05:57:00']
	
``date_fmt_headers``

::

    for item in date_fmt_headers['candles']: 
        print item
    
    Headers(timestamp=1503694620, bidopen=109.321, bidclose=109.326, bidhigh=109.326, bidlow=109.316, askopen=109.359, askclose=109.358, askhigh=109.362, asklow=109.357, tickqty=28, datestring='2017/08/26 05:57:00')
    Headers(timestamp=1503694680, bidopen=109.326, bidclose=109.312, bidhigh=109.326, bidlow=109.31, askopen=109.358, askclose=109.374, askhigh=109.376, asklow=109.358, tickqty=42, datestring='2017/08/26 05:58:00')
    Headers(timestamp=1503694740, bidopen=109.312, bidclose=109.312, bidhigh=109.312, bidlow=109.31, askopen=109.374, askclose=109.374, askhigh=109.374, asklow=109.372, tickqty=4, datestring='2017/08/26 05:59:00')
    
    for item in date_fmt_headers['candles']: 
        print "%s: Ask Close [%s], High Bid [%s] " % (item.datestring, item.askclose, item.bidhigh)
    
    2017/08/26 05:57:00: Ask Close [109.358], High Bid [109.326]
    2017/08/26 05:58:00: Ask Close [109.374], High Bid [109.326]
    2017/08/26 05:59:00: Ask Close [109.374], High Bid [109.312]
	
``subscribe_symbol - default``

::

    {u'Updated': 1504167080, u'Rates': [110.467, 110.488, 110.629, 110.156], u'Symbol': u'USD/JPY'}
    {u'Updated': 1504167081, u'Rates': [110.469, 110.49, 110.629, 110.156], u'Symbol': u'USD/JPY'}
	
``subscribe_symbol - overridden``

::

    Price update:  {"Updated":1504167248,"Rates":[110.446,110.468,110.629,110.156],"Symbol":"USD/JPY"}
    Price update:  {"Updated":1504167250,"Rates":[110.446,110.468,110.629,110.156],"Symbol":"USD/JPY"}
	
.. note::

	This is for personal use and abides by our `EULA <https://www.fxcm.com/uk/forms/eula/>`_.
	For more information, you may contact us at api@fxcm.com

**Disclaimer**:

Trading forex/CFDs on margin carries a high level of risk and may not be suitable for all investors as you could sustain losses in excess of deposits. Leverage can work against you. The products are intended for retail and professional clients. Due to the certain restrictions imposed by the local law and regulation, German resident retail client(s) could sustain a total loss of deposited funds but are not subject to subsequent payment obligations beyond the deposited funds. Be aware and fully understand all risks associated with the market and trading. Prior to trading any products, carefully consider your financial situation and experience level. If you decide to trade products offered by FXCM Australia Pty. Limited (“FXCM AU”) (AFSL 309763), you must read and understand the `Financial Services Guide <https://docs.fxcorporate.com/financial-services-guide-au.pdf/>`_, `Product Disclosure Statement  <https://www.fxcm.com/au/legal/product-disclosure-statements/>`_, and `Terms of Business <https://docs.fxcorporate.com/tob_au_en.pdf/>`_. Any opinions, news, research, analyses, prices, or other information is provided as general market commentary, and does not constitute investment advice. FXCM will not accept liability for any loss or damage, including without limitation to, any loss of profit, which may arise directly or indirectly from use of or reliance on such information. FXCM will not accept liability for any loss or damage, including without limitation to, any loss of profit, which may arise directly or indirectly from use of or reliance on such information.

