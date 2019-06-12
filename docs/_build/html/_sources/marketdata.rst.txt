===========
Market Data
===========

FXCM provides several sample data for free. Historical tick, candle, order flow, sentiment and volume data.

TickData
========

Enjoy free access to our historical Tick Data.

Our repository contains Tick Data from 4 January 2015. The data is compiled by trading instrument for that trading week. The files are stored in our public directory and is updated every Monday:

::

	https://tickdata.fxcorporate.com/{instrument}/{year}/{int of week of year}.csv.gz

	Instrument: 
             AUDCAD, AUDCHF, AUDJPY, AUDNZD, CADCHF, EURAUD,
             EURCHF, EURGBP, EURJPY, EURUSD, GBPCHF, GBPJPY,
             GBPNZD, GBPUSD, NZDCAD, NZDCHF, NZDJPY, NZDUSD,
             USDCAD, USDCHF, USDJPY, AUDUSD, CADJPY, GBPCAD,
             USDTRY, EURNZD

	Year: 2015, 2016, 2017, 2018
	Week: 1 to 52/53
	To give an example, the path for extracting EURUSD data for the 1st week of 2015 would be:
		https://tickdata.fxcorporate.com/EURUSD/2015/1.csv.gz

If you are familiar with Python, we have two scripts that you may use for Python 2.7 and Python 3.4

Sample tick data code
---------------------

TickData34.py
^^^^^^^^^^^^^

.. code-block:: Python


	# -*- coding: utf-8 -*-
	"""
	Created on Thu Dec 08 15:46:59 2016

	@author: fxcm
	"""
	##from StringIO import StringIO
	from io import BytesIO
	import gzip
	import urllib
	import datetime 


	url = 'https://tickdata.fxcorporate.com/'##This is the base url 
	url_suffix = '.csv.gz' ##Extension of the file name
	symbol = 'EURUSD' ##symbol we want to get tick data for
	##Available Currencies 
	##AUDCAD,AUDCHF,AUDJPY, AUDNZD,CADCHF,EURAUD,EURCHF,EURGBP
	##EURJPY,EURUSD,GBPCHF,GBPJPY,GBPNZD,GBPUSD,GBPCHF,GBPJPY
	##GBPNZD,NZDCAD,NZDCHF.NZDJPY,NZDUSD,USDCAD,USDCHF,USDJPY


	##The tick files are stored a compressed csv.  The storage structure comes as {symbol}/{year}/{week_of_year}.csv.gz  
	##The first week of the year will be 1.csv.gz where the 
	##last week might be 52 or 53.  That will depend on the year.
	##Once we have the week of the year we will be able to pull the correct file with the data that is needed.
	start_dt =  datetime.date(2015,7,16)##random start date
	end_dt = datetime.date(2015,8,16)##random end date 


	start_wk = start_dt.isocalendar()[1]##find the week of the year for the start  
	end_wk = end_dt.isocalendar()[1] ##find the week of the year for the end 
	year = str(start_dt.isocalendar()[0]) ##pull out the year of the start

	###The URL is a combination of the currency, year, and week of the year.
	###Example URL https://tickdata.fxcorporate.com/EURUSD/2015/29.csv.gz
	###The example URL should be the first URL of this example

	##This will loop through the weeks needed, create the correct URL and print out the lenght of the file.
	for i in range(start_wk, end_wk ):
	    url_data = url + symbol+'/'+year+'/'+str(i)+url_suffix
	    print(url_data)
	    requests = urllib.request.urlopen(url_data)
	    buf = BytesIO(requests.read())
	    f = gzip.GzipFile(fileobj=buf)
	    data = f.read()
	    print(len(data))
		
TickData27.py
^^^^^^^^^^^^^

.. code-block:: Python

	# -*- coding: utf-8 -*-
	"""
	Created on Thu Dec 08 15:46:59 2016

	@author: fxcm
	"""
	from StringIO import StringIO
	import gzip
	import urllib2
	import datetime 


	url = 'https://tickdata.fxcorporate.com/'##This is the base url 
	url_suffix = '.csv.gz' ##Extension of the file name
	symbol = 'EURUSD' ##symbol we want to get tick data for
	##Available Currencies 
	##AUDCAD,AUDCHF,AUDJPY, AUDNZD,CADCHF,EURAUD,EURCHF,EURGBP
	##EURJPY,EURUSD,GBPCHF,GBPJPY,GBPNZD,GBPUSD,GBPCHF,GBPJPY
	##GBPNZD,NZDCAD,NZDCHF.NZDJPY,NZDUSD,USDCAD,USDCHF,USDJPY


	##The tick files are stored a compressed csv.  The storage structure comes as {symbol}/{year}/{week_of_year}.csv.gz  
	##The first week of the year will be 1.csv.gz where the 
	##last week might be 52 or 53.  That will depend on the year.
	##Once we have the week of the year we will be able to pull the correct file with the data that is needed.
	start_dt =  datetime.date(2015,7,16)##random start date
	end_dt = datetime.date(2015,8,16)##random end date 


	start_wk = start_dt.isocalendar()[1]##find the week of the year for the start  
	end_wk = end_dt.isocalendar()[1] ##find the week of the year for the end 
	year = str(start_dt.isocalendar()[0]) ##pull out the year of the start

	###The URL is a combination of the currency, year, and week of the year.
	###Example URL https://tickdata.fxcorporate.com/EURUSD/2015/29.csv.gz
	###The example URL should be the first URL of this example

	##This will loop through the weeks needed, create the correct URL and print out the lenght of the file.
	for i in range(start_wk, end_wk ):
	    url_data = url + symbol+'/'+year+'/'+str(i)+url_suffix
	    print(url_data)
	    request = urllib2.Request(url_data)
	    response = urllib2.urlopen(request)
	    buf = StringIO(response.read())
	    f = gzip.GzipFile(fileobj=buf)
	    data = f.read()
	    print(len(data))

.. note::

	Losses can exceed deposits.
	Past performance is not indicative of future results.
	Timestamps are in UTC.
	Data points are indicative and based on the lowest spreads available exclusively on Active Trader accounts.
	This is for personal use and abides by our EULA.
	For more information, you may contact us at api@fxcm.com.

CandleData
==========

Enjoy free access to our historical Time Series or Candle Data.

Our repository contains Candle Data from 1 January 2012. The data is compiled by trading instrument for that trading week for m1 & H1, and trading year for D1. The files are stored in our public directory and is updated every Monday for minute (m1) and hour (H1) data only:
::
	
	https://candledata.fxcorporate.com/{periodicity}/{instrument}/{year}/{int of week of year}.csv.gz

Periodicity
::

 m1, H1, D1

Instrument 
::

		AUDCAD, AUDCHF, AUDJPY, AUDNZD, CADCHF, EURAUD,
		EURCHF, EURGBP, EURJPY, EURUSD, GBPCHF, GBPJPY,	
		GBPNZD, GBPUSD, NZDCAD, NZDCHF, NZDJPY, NZDUSD,	
		USDCAD, USDCHF, USDJPY

Year
::

	2012, 2013, 2014, 2015, 2016, 2017, 2018

Week 
::

	1 to 52/53 (only applicable to m1 and H1)
	
To give an example, the path for extracting EURUSD minute-data for the 1st week of 2012 would be:
::

		https://candledata.fxcorporate.com/m1/EURUSD/2012/1.csv.gz

To give an example, the path for extracting EURUSD hourly-data for the 1st week of 2012 would be:	
::

	https://candledata.fxcorporate.com/H1/EURUSD/2012/1.csv.gz

To give an example, the path for extracting EURUSD daily-data for 2012 would be:
::

	https://candledata.fxcorporate.com/D1/EURUSD/2012.csv.gz

If you are familiar with Python, we have three scripts that you may use for Python 2.7, Python 3.4, or a ``pandas`` data frame.

Sample candle data code
-----------------------

CandleData(pandas).py
^^^^^^^^^^^^^^^^^^^^^

.. code-block:: Python

	# -*- coding: utf-8 -*-
	"""
	Created on Thu Feb 08 08:11:38 2018

	@author: fxcm
	"""
	import datetime
	import pandas as pd


	url = 'https://candledata.fxcorporate.com/'##This is the base url
	periodicity='m1' ##periodicity, can be m1, H1, D1
	url_suffix = '.csv.gz' ##Extension of the file name
	symbol = 'EURUSD' 
	##symbol we want to get tick data for
	##Available Currencies 
	##AUDCAD,AUDCHF,AUDJPY, AUDNZD,CADCHF,EURAUD,EURCHF,EURGBP
	##EURJPY,EURUSD,GBPCHF,GBPJPY,GBPNZD,GBPUSD,GBPCHF,GBPJPY
	##GBPNZD,NZDCAD,NZDCHF.NZDJPY,NZDUSD,USDCAD,USDCHF,USDJPY


	##The candle files are stored in compressed csv.  The storage structure comes as {periodicity}/{symbol}/{year}/{week_of_year}.csv.gz
	##The first week of the year will be 1.csv.gz where the 
	##last week might be 52 or 53.  That will depend on the year.
	##Once we have the week of the year we will be able to pull the correct 
	##file with the data that is needed.
	start_dt =  datetime.date(2017,7,5)##random start date
	end_dt = datetime.date(2017,12,16)##random end date


	start_wk = start_dt.isocalendar()[1]##find the week of the year for the start  
	end_wk = end_dt.isocalendar()[1] ##find the week of the year for the end 
	year = str(start_dt.isocalendar()[0]) ##pull out the year of the start

	###The URL is a combination of the currency, periodicity,  year, and week of the year.
	###Example URL https://candledata.fxcorporate.com/m1/EURUSD/2017/29.csv.gz
	###The example URL should be the first URL of this example
	data=pd.DataFrame()
	##This will loop through the weeks needed, create the correct URL and print out the lenght of the file.
	for i in range(start_wk, end_wk ):
	    url_data = url + periodicity + '/' + symbol + '/' + year + '/' + str(i) + url_suffix
	    print(url_data)
	    tempdata = pd.read_csv(url_data, compression='gzip')
	    data=pd.concat([data, tempdata])
	print(data)
	
CandleData34.py
^^^^^^^^^^^^^^^

.. code-block:: Python


	# -*- coding: utf-8 -*-
	"""
	Created on Thu Feb 08 07:35:59 2018

	@author: fxcm
	"""
	##from StringIO import StringIO
	from io import BytesIO
	import gzip
	import urllib.request as ur
	import datetime 


	url = 'https://candledata.fxcorporate.com/'##This is the base url
	periodicity='m1' ##periodicity, can be m1, H1, D1
	url_suffix = '.csv.gz' ##Extension of the file name
	symbol = 'EURUSD' ##symbol we want to get candle data for
	##Available Currencies 
	##AUDCAD,AUDCHF,AUDJPY, AUDNZD,CADCHF,EURAUD,EURCHF,EURGBP
	##EURJPY,EURUSD,GBPCHF,GBPJPY,GBPNZD,GBPUSD,GBPCHF,GBPJPY
	##GBPNZD,NZDCAD,NZDCHF.NZDJPY,NZDUSD,USDCAD,USDCHF,USDJPY


	##The candle files are stored in compressed csv.  The storage structure comes as {periodicity}/{symbol}/{year}/{week_of_year}.csv.gz
	##The first week of the year will be 1.csv.gz where the 
	##last week might be 52 or 53.  That will depend on the year.
	##Once we have the week of the year we will be able to pull the correct file with the data that is needed.
	start_dt =  datetime.date(2017,7,5)##random start date
	end_dt = datetime.date(2017,12,16)##random end date


	start_wk = start_dt.isocalendar()[1]##find the week of the year for the start  
	end_wk = end_dt.isocalendar()[1] ##find the week of the year for the end 
	year = str(start_dt.isocalendar()[0]) ##pull out the year of the start

	###The URL is a combination of the currency, periodicity, year, and week of the year.
	###Example URL https://candledata.fxcorporate.com/m1/EURUSD/2017/29.csv.gz
	###The example URL should be the first URL of this example

	##This will loop through the weeks needed, create the correct URL and print out the lenght of the file.
	for i in range(start_wk, end_wk ):
	    url_data = url + periodicity+'/'+symbol+'/'+year+'/'+str(i)+url_suffix
	    print(url_data)
	    requests = ur.urlopen(url_data)
	    buf = BytesIO(requests.read())
	    f = gzip.GzipFile(fileobj=buf)
	    data = f.read()
	    print(len(data))

CandleData27.py
^^^^^^^^^^^^^^^

.. code-block:: Python

	# -*- coding: utf-8 -*-
	"""
	Created on Thu Feb 08 07:35:59 2018

	@author: fxcm
	"""
	from StringIO import StringIO
	import gzip
	import urllib2
	import datetime 


	url = 'https://candledata.fxcorporate.com/'##This is the base url
	periodicity='m1' ##periodicity, can be m1, H1, D1
	url_suffix = '.csv.gz' ##Extension of the file name
	symbol = 'EURUSD' ##symbol we want to get tick data for
	##Available Currencies 
	##AUDCAD,AUDCHF,AUDJPY, AUDNZD,CADCHF,EURAUD,EURCHF,EURGBP
	##EURJPY,EURUSD,GBPCHF,GBPJPY,GBPNZD,GBPUSD,GBPCHF,GBPJPY
	##GBPNZD,NZDCAD,NZDCHF.NZDJPY,NZDUSD,USDCAD,USDCHF,USDJPY


	##The candle files are stored in compressed csv.  The storage structure comes as {periodicity}/{symbol}/{year}/{week_of_year}.csv.gz
	##The first week of the year will be 1.csv.gz where the 
	##last week might be 52 or 53.  That will depend on the year.
	##Once we have the week of the year we will be able to pull the correct file with the data that is needed.
	start_dt =  datetime.date(2017,7,5)##random start date
	end_dt = datetime.date(2017,12,16)##random end date


	start_wk = start_dt.isocalendar()[1]##find the week of the year for the start  
	end_wk = end_dt.isocalendar()[1] ##find the week of the year for the end 
	year = str(start_dt.isocalendar()[0]) ##pull out the year of the start

	###The URL is a combination of the currency, periodicity,  year, and week of the year.
	###Example URL https://candledata.fxcorporate.com/m1/EURUSD/2017/29.csv.gz
	###The example URL should be the first URL of this example

	##This will loop through the weeks needed, create the correct URL and print out the lenght of the file.
	for i in range(start_wk, end_wk ):
	    url_data = url + periodicity + '/' + symbol + '/' + year + '/' + str(i) + url_suffix
	    print(url_data)
	    request = urllib2.Request(url_data)
	    response = urllib2.urlopen(request)
	    buf = StringIO(response.read())
	    f = gzip.GzipFile(fileobj=buf)
	    data = f.read()
	    print(len(data))

.. note::

	Losses can exceed deposits.
	Past performance is not indicative of future results.
	Timestamps are in UTC.
	Data points are indicative and based on the lowest spreads available exclusively on Active Trader accounts.
	This is for personal use and abides by our EULA.
	For more information, you may contact us at api@fxcm.com.

Order Flow
==========

Enjoy a free one-month sample of our historical Orders Data.

https://sampledata.fxcorporate.com/orders/sample.csv.gz

Each data set would include:
    • DateTime (UTC)
    • Symbol
    • Quantity (Volume)
    • Rate (Price)

.. note::

	Losses can exceed deposits.
	Past performance is not indicative of future results.
	Timestamps are in UTC.
	Data points are indicative and based on the lowest spreads available exclusively on Active Trader accounts.
	This is for personal use and abides by our EULA.
	For more information, you may contact us at api@fxcm.com.

Sentiment
=========

Enjoy a free one-month sample of our historical Sentiment Data also known as SSI:

::

	https://sampledata.fxcorporate.com/sentiment/{instrument}.csv.gz

	Instrument: 
         	 AUDJPY, AUDUSD, CADJPY, CHFJPY, EURAUD, EURCAD, EURCHF, EURGBP,
         	 EURJPY, EURNOK, EURSEK, EURUSD, GBPCHF, GBPJPY, GBPUSD, NZDJPY,
         	 NZDUSD, USDCAD, USDCHF, USDCNH, USDJPY, USDNOK, USDSEK, FRA40,
         	 GER30, JPN225, SPX500, UK100, US30, USOil, XAGUSD, XAUUSD

	Each data set would include:
        •DateTime (EST)
        •Symbol
        •Name
        •Value
		
.. note::

	Losses can exceed deposits.
	Past performance is not indicative of future results.
	Timestamps are in UTC.
	Data points are indicative and based on the lowest spreads available exclusively on Active Trader accounts.
	This is for personal use and abides by our EULA.
	For more information, you may contact us at api@fxcm.com.

Volume
======

Enjoy a free one-month sample of our historical Volume Data:

::

	https://sampledata.fxcorporate.com/volume/{instrument}.csv.gz

	Instrument: 
         	 AUDJPY, AUDUSD, CADJPY, CHFJPY, EURAUD, EURCAD, EURCHF, EURGBP,
         	 EURJPY, EURNOK, EURSEK, EURUSD, GBPCHF, GBPJPY, GBPUSD, NZDJPY,
         	 NZDUSD, USDCAD, USDCHF, USDCNH, USDJPY, USDNOK, USDSEK, FRA40,
         	 GER30, JPN225, SPX500, UK100, US30, USOil, XAGUSD, XAUUSD

	Each data set would include:
        •DateTime (UTC)
        •Symbol
        •Name
        •Value
		
.. note::

	Losses can exceed deposits.
	Past performance is not indicative of future results.
	Timestamps are in UTC.
	Data points are indicative and based on the lowest spreads available exclusively on Active Trader accounts.
	This is for personal use and abides by our EULA.
	For more information, you may contact us at api@fxcm.com.
	
**Disclaimer**:

CFDs are complex instruments and come with a high risk of losing money rapidly due to leverage.
73.62% of retail investor accounts lose money when trading CFDs with this provider.
You should consider whether you understand how CFDs work and whether you can afford to take the high risk of losing your money.
High Risk Investment Notice: Trading Forex/CFD's on margin carries a high level of risk and may not be suitable for all investors as you could sustain losses in excess of deposits. The products are intended for retail, professional and eligible counterparty clients. For clients who maintain account(s) with Forex Capital Markets Limited (“FXCM LTD”), retail clients could sustain a total loss of deposited funds but are not subject to subsequent payment obligations beyond the deposited funds and professional clients could sustain losses in excess of deposits. Prior to trading any products offered by FXCM LTD, inclusive of all EU branches, FXCM Australia Pty. Limited, FXCM South Africa (PTY) Ltd, any affiliates of aforementioned firms, or other firms within the FXCM group of companies [collectively the "FXCM Group"], carefully consider your financial situation and experience level. If you decide to trade products offered by FXCM Australia Pty. Limited ("FXCM AU") (AFSL 309763), you must read and understand the Financial Services Guide, Product Disclosure Statement and Terms of Business. Our Forex/CFD prices are set by FXCM, are not made on an Exchange and are not governed under the Financial Advisory and Intermediary Services Act. The FXCM Group may provide general commentary which is not intended as investment advice and must not be construed as such. Seek advice from a separate financial advisor. The FXCM Group assumes no liability for errors, inaccuracies or omissions; does not warrant the accuracy, completeness of information, text, graphics, links or other items contained within these materials. Read and understand the Terms and Conditions on the FXCM Group’s websites prior to taking further action.”
