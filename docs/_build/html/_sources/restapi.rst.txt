=======================
REST API Specifications
=======================

Our **REST API** is a web-based API using a Websocket connection and was developed with algorithmic trading in mind. 

Developers and investors can create custom trading applications, integrate into our platform, back test strategies and build robot trading. Calls can be made in any language that supports a standard HTTP. 

We utilize the new OAuth 2.0 specification for authentication via token. This allows for a more secure authorization to access your application and can easily be integrated with web applications, mobile devices, and desktop platforms

With the use of the socket.io library, the API has streaming capability and will push data notifications in a JSON format. Your application will have access to our real-time streaming market data, subscribe in real time access to trading tables and place live trades.

**FXCM Trading hours**

FXCM's trading hours vary by product. For forex, trading opens on Sundays between 5:00 PM ET and 5:15 PM ET and closes on Fridays around 4:55 PM ET. For CFDs, please check the `CFD Product Guide <http://docs.fxcorporate.com/user-guide/ug-cfd-product-guide-ltd-en.pdf>`_.

Getting Started
===============

1. Quick start guide

   * `Python <https://github.com/fxcm/RestAPI/blob/master/Rest_quick_start_guide_python.docx/>`_
   * `Node.js <https://github.com/fxcm/RestAPI/blob/master/Rest_quick_start_guide_nodejs.docx/>`_
   * `Java <https://github.com/fxcm/RestAPI/blob/master/FXCM%20JAVA%20REST%20API%20QuickStart.pdf/>`_

2. Sample code


   * `Node.js sample code <https://github.com/fxcm/RestAPI/tree/master/fxcm-api-rest-nodejs-example/>`_
   * :download:`Java sample code </_downloads/JavaRestClient.zip>`
   * :download:`C# sample code </_downloads/RestAPI_CSharp_sample_code.zip>`

3. Account setup

   * Apply for a `demo account <https://www.fxcm.com/uk/forex-trading-demo/>`_. 
   * Generate access token. You can generate one from `Trading Station web <https://tradingstation.fxcm.com/>`_. Click on User Account > Token Management on the upper right hand of the website. 
   * For live account, please send your username to api@fxcm.com, we will need to enable Rest API access. For demo account, Rest API access is enabled by default.
   
4. Download :download:`Rest API </_downloads/Socket REST API Specs.pdf>` document.

5. Access REST API documentation using `Swagger <https://fxcm.github.io/rest-api-docs/#/>`_.

6. Start coding. You will need to reference the `socket.io library <https://socket.io/docs/client-api/>`_ in your code.

   * `Socket.io for Javascript <https://www.npmjs.com/package/socket.io/>`_
   * `Socket.io for Python <https://pypi.python.org/pypi/socketIO-client/>`_


Connect
=======

Clients should establish a persistent WebSocket connection using socket.io library. All non-solicited updates will be sent over this connection. Client requests are to be sent via normal HTTP messages. Every HTTP message must contain following parameters:

.. tabularcolumns:: |p{10cm}|p{10cm}|p{8cm}|p{5cm}|
	
.. csv-table:: HTTP Message
   :file: _files/httpmessage.csv
   :header-rows: 1
   :class: longtable
   :widths: 1 1 1 1
   :align: center

Sample Request:
::

   GET/socket.io/?access_token=cj5wedhq3007v61fe935ihqed&EIO=3&transport=polling&t=Lsd_lZY&b64=1
   HTTP/1.1 
   User-Agent: node-XMLHttpRequest 
   Accept: */* 
   Host: api.fxcm.com 
   Connection: close

What 't' means::

"t" is the table id: 

.. tabularcolumns:: |p{2cm}|p{8cm}|
	
.. csv-table:: TableID
   :file: _files/tableID.csv
   :header-rows: 1
   :class: longtable
   :widths: 1 1
   :align: center

Subscribe vs Snapshot
=====================

FXCM Rest API provides two ways to deliever data. susbcribe vs snapshot.

After susbcribe, data will be pushed to your socket whenever there is an update. You can susbcribe Market data stream /susbcribe or live table update /trading/susbcribe. You can also unsubscribe.
You can request a snapshot of trading tables via /trading/get_model. 

::

      Model choices: 'Offer', 'OpenPosition', 'ClosedPosition', 'Order', 'Summary', 'LeverageProfile', 'Account', 'Properties'.   

OrderID vs TradeID
==================

OrderID and TradeID are different.
In Market order, an order id is created straightaway and it is in callback immediately. 

::

      {"response":{"executed":true},"data":{"type":0,"orderId":81712802}}

A trade id is not generated until after order is executed. You have to subscribe the order table and listing the live update and look up the trade id. You will not get a trade id in snapshot as the information disappears when you submit the request. 

::

      Examples:
      Subscribing for Orders table:
      POST /trading/subscribe
      models=Order

      Placing Market order:
      POST /trading/open_trade
      account_id=1537581&symbol=EUR%2FUSD&is_buy=false&rate=0&amount=5&order_type=AtMarket&time_in_force=GTC

      Response from server:
      {"executed":true}{"type":0,"orderId":390285837}

      Received Order record from /trading/subscribe with order_id and trade_id:
      {"t":3,"ratePrecision":5,"orderId":"390285837","tradeId":"170162801","time":"04252018120716391","accountName":"01537581","accountId":"1537581","timeInForce":"GTC","expireDate":"","currency":"EUR/USD","isBuy":false,"buy":0,"sell":1.21818,"type":"OM","status":2,"amountK":5,"currencyPoint":0.5,"stopMove":0,"stop":0,"stopRate":0,"limit":0,"limitRate":0,"isEntryOrder":false,"ocoBulkId":0,"isNetQuantity":false,"isLimitOrder":false,"isStopOrder":false,"isELSOrder":false,"stopPegBaseType":-1,"limitPegBaseType":-1,"range":0,"action":"I"}


Furthermore, a single market order can have many TradeIDs, if they are partial fills or closing of other orders. In this case, it's more approriate to provide the OrderID which ties back to that spcific market order request, from there you can join this OrderID to any associated order.

In an entry order, an order ID is in callback function. You can also see it on an order table sanpshot. but you will not get a TradeID until order been executed. 

Candle Request Limit
====================
.. tabularcolumns:: |p{1cm}|p{8cm}|p{6cm}|
	
.. csv-table:: Candle download limit
   :file: _files/candledownloadlimit.csv
   :header-rows: 1
   :class: longtable
   :widths: 1 1 1
   :align: center

Placing Trailing Stops
======================

The fixed trailing stop should be 10 or above, for dynamic trailing stop = 1, number between 2-9 will be rejected. Parameter is trailing_stop_step.
      
::

      Example Entry order with trailing stop of 10 pips:
      POST /trading/create_entry_order account_id=1537581&symbol=EUR%2FUSD&is_buy=true&rate=1.1655&amount=3&order_type=Entry&time_in_force=GTC&stop=-50&trailing_stop_step=10&is_in_pips=true

Account Name vs Account ID
==========================

There is a difference between account name and account id. Usually removing the heading zeros are account ID. You need to pass the account_id when placing orders. You can retrieve this information from /trading/get_model/accounts.

::

      Wrong:
      {"is_buy":false,"account_id":"00654061","symbol":"EUR/USD","rate":1.15,"amount":11,"stop":-40,"is_in_pips":true,"order_type":"AtMarket","time_in_force":"GTC"}

      ERR noExec: /trading/create_entry_order
      {"code":3,"message":"Amount should be divisible by 10","parameters":["10"]}
 
      Correct:
      {"is_buy":false,"account_id":"654061","symbol":"EUR/USD","rate":1.15,"amount":11,"stop":-40,"is_in_pips":true,"order_type":"AtMarket","time_in_force":"GTC"}
      
      request # 2  has been executed: {
      "response": {"executed": true}, "data": {"type": 0,"orderId": 194963057}}

Offer Ids
=========
.. tabularcolumns:: |p{1cm}|p{8cm}|
	
.. csv-table:: Offer Ids 
   :file: _files/offer_ids.csv
   :header-rows: 1
   :class: longtable
   :widths: 1 1
   :align: center
   
.. note::

	This is for personal use and abides by our `EULA <https://www.fxcm.com/uk/forms/eula/>`_.
	For more information, you may contact us at api@fxcm.com

**Disclaimer**:

CFDs are complex instruments and come with a high risk of losing money rapidly due to leverage.
73.62% of retail investor accounts lose money when trading CFDs with this provider.
You should consider whether you understand how CFDs work and whether you can afford to take the high risk of losing your money.
High Risk Investment Notice: Trading Forex/CFD's on margin carries a high level of risk and may not be suitable for all investors as you could sustain losses in excess of deposits. The products are intended for retail, professional and eligible counterparty clients. For clients who maintain account(s) with Forex Capital Markets Limited (“FXCM LTD”), retail clients could sustain a total loss of deposited funds but are not subject to subsequent payment obligations beyond the deposited funds and professional clients could sustain losses in excess of deposits. Prior to trading any products offered by FXCM LTD, inclusive of all EU branches, FXCM Australia Pty. Limited, FXCM South Africa (PTY) Ltd, any affiliates of aforementioned firms, or other firms within the FXCM group of companies [collectively the "FXCM Group"], carefully consider your financial situation and experience level. If you decide to trade products offered by FXCM Australia Pty. Limited ("FXCM AU") (AFSL 309763), you must read and understand the Financial Services Guide, Product Disclosure Statement and Terms of Business. Our Forex/CFD prices are set by FXCM, are not made on an Exchange and are not governed under the Financial Advisory and Intermediary Services Act. The FXCM Group may provide general commentary which is not intended as investment advice and must not be construed as such. Seek advice from a separate financial advisor. The FXCM Group assumes no liability for errors, inaccuracies or omissions; does not warrant the accuracy, completeness of information, text, graphics, links or other items contained within these materials. Read and understand the Terms and Conditions on the FXCM Group’s websites prior to taking further action.”
