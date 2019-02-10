================================
Marketdata Download Instructions
================================

FXCM provides several sample data for free. Historical tick or candle data. Order flow, sentiment and volume.

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

  	Periodicity:  m1, H1, D1

  	Instrument: 
			 AUDCAD, AUDCHF, AUDJPY, AUDNZD, CADCHF, EURAUD,
             EURCHF, EURGBP, EURJPY, EURUSD, GBPCHF, GBPJPY,
             GBPNZD, GBPUSD, NZDCAD, NZDCHF, NZDJPY, NZDUSD,
             USDCAD, USDCHF, USDJPY

  	Year: 2012, 2013, 2014, 2015, 2016, 2017, 2018

 	Week: 1 to 52/53 (only applicable to m1 and H1)
	
	To give an example, the path for extracting EURUSD minute-data for the 1st week of 2012 would be:
		https://candledata.fxcorporate.com/m1/EURUSD/2012/1.csv.gz

	To give an example, the path for extracting EURUSD hourly-data for the 1st week of 2012 would be:	
		https://candledata.fxcorporate.com/H1/EURUSD/2012/1.csv.gz

	To give an example, the path for extracting EURUSD daily-data for 2012 would be
		https://candledata.fxcorporate.com/D1/EURUSD/2012.csv.gz

If you are familiar with Python, we have three scripts that you may use for Python 2.7, Python 3.4, or a pandas data frame

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

Trading forex/CFDs on margin carries a high level of risk and may not be suitable for all investors as you could sustain losses in excess of deposits. Leverage can work against you. The products are intended for retail and professional clients. Due to the certain restrictions imposed by the local law and regulation, German resident retail client(s) could sustain a total loss of deposited funds but are not subject to subsequent payment obligations beyond the deposited funds. Be aware and fully understand all risks associated with the market and trading. Prior to trading any products, carefully consider your financial situation and experience level. If you decide to trade products offered by FXCM Australia Pty. Limited (“FXCM AU”) (AFSL 309763), you must read and understand the `Financial Services Guide <https://docs.fxcorporate.com/financial-services-guide-au.pdf/>`_, `Product Disclosure Statement  <https://www.fxcm.com/au/legal/product-disclosure-statements/>`_, and `Terms of Business <https://docs.fxcorporate.com/tob_au_en.pdf/>`_. Any opinions, news, research, analyses, prices, or other information is provided as general market commentary, and does not constitute investment advice. FXCM will not accept liability for any loss or damage, including without limitation to, any loss of profit, which may arise directly or indirectly from use of or reliance on such information. FXCM will not accept liability for any loss or damage, including without limitation to, any loss of profit, which may arise directly or indirectly from use of or reliance on such information.