=====================
FXCMPY Python Wrapper
=====================

FXCM provides a RESTful API to interact with its trading platform. Among others, it allows the retrieval of historical data as well as of streaming data. In addition, it allows to place different types of orders and to read out account information. The overall goal is to allow the implementation automated, algortithmic trading programs. ``fxcmpy.py`` is a Python wrapper package for that API.

Demo Account
------------

To get started with the the API and the package, a demo account with FXCM is sufficient. You can open such an account under https://www.fxcm.com/uk/forex-trading-demo/.

Package Installation
--------------------

Installation happens via pip install on the command line.

.. code-block:: python
	
	pip install fxcmpy

API Token
---------

To connect to the API, you need an API token that you can create or revoke from within your (demo) account in the Trading Station https://tradingstation.fxcm.com/.

In an interactive context, you can use e.g. a variable called TOKEN to reference your unique API token.

::

	TOKEN = YOUR_FXCM_API_TOKEN
	Connecting to the server, then boils down to the following line of code.

	con = fxcmpy.fxcmpy(access_token=TOKEN, log_level='error')
	
Configuration file
------------------	

It is recommended to store the API token in a configuration file which allows for re-usability and hides the token on the GUI level. The file should contain the following lines.

::

	[FXCM]
	log_level = error
	log_file = PATH_TO_AND_NAME_OF_LOG_FILE
	access_token = YOUR_FXCM_API_TOKEN
	
It is assumed onwards that this file is in the current working directory and that its name is fxcm.cfg.

With such a configuration file in the current working directory, only the filename need to be passed as a parameter to connect to the API.

::

   con = fxcmpy.fxcmpy(config_file='fxcm.cfg')

Documentation
-------------

* The detailed documentation of this wrapper is found under:

	| https://www.fxcm.com/fxcmpy/

* The detailed documentation of the API is found under:

	| https://github.com/fxcm/RestAPI

.. note::

	This is for personal use and abides by our `EULA <https://www.fxcm.com/uk/forms/eula/>`_.
	For more information, you may contact us at api@fxcm.com
	
fxcmpy wrapper Sample Code
--------------------------

1. `EMA Crossover Strategy.ipynb  <https://github.com/fxcm/RestAPI/blob/master/Europe-Algo-Meetup/EMA%20Crossover%20Strategy%20and%20Backtesting.ipynb/>`_

2. `Real-Time SMA Crossover Strategy.ipynb  <https://github.com/fxcm/RestAPI/blob/master/Europe-Algo-Meetup/Real-Time%20SMA%20Crossover%20Strategy.ipynb/>`_

3. `EMA Crossover Strategy and Backtest-checkpoint.ipynb  <https://github.com/fxcm/RestAPI/blob/master/FXCM-Algo-Summit/.ipynb_checkpoints/EMA%20Crossover%20Strategy%20and%20Backtest-checkpoint.ipynb/>`_

4. `Hull Moving Average Strategy.ipynb  <https://github.com/fxcm/RestAPI/blob/master/FXCM-Algo-Summit/Hull%20Moving%20Average.ipynb/>`_

5. `Bollinger Band Backtest Part 1.ipynb  <https://github.com/fxcm/RestAPI/blob/master/Python-Backtest_Examples/Bollinger%20Band%20Backtest%20Part%201.ipynb/>`_

6. `Bollinger Band Backtest Part 2.ipynb   <https://github.com/fxcm/RestAPI/blob/master/Python-Backtest_Examples/Bollinger%20Band%20Backtest%20Part%202.ipynb/>`_

7. `EMA Crossover Strategy backtest.py  <https://github.com/fxcm/RestAPI/blob/master/Python-Backtest_Examples/EMA%20Crossover%20Strategy%20backtest.py/>`_

8. `Introduction to Monte Carlo Simulation.ipynb <https://github.com/fxcm/RestAPI/blob/master/Python-Backtest_Examples/Introduction%20to%20Monte%20Carlo%20Simulation%20.ipynb/>`_

9. `Stochastic Strategy Backtest.py <https://github.com/fxcm/RestAPI/blob/master/Python-Backtest_Examples/Stochastic%20Strategy%20Backtest.py/>`_

10. `BB ADX Range Strategy.py <https://github.com/fxcm/RestAPI/blob/master/Python-Live-Trading-Examples/BB%20ADX%20Range%20Strategy.py/>`_

11. `Bitcoin Breakout Strategy.py <https://github.com/fxcm/RestAPI/blob/master/Python-Live-Trading-Examples/Bitcoin%20Breakout%20Strategy.py/>`_

12. `Python Strategy Template.py <https://github.com/fxcm/RestAPI/blob/master/Python-Live-Trading-Examples/Python%20Strategy%20Template.py/>`_

13. `RSI Range Strategy with Trading Time Range.py <https://github.com/fxcm/RestAPI/blob/master/Python-Live-Trading-Examples/RSI%20Range%20Strategy%20With%20Trading%20Time%20Range.py/>`_

14. `RSI Range Strategy.py <https://github.com/fxcm/RestAPI/blob/master/Python-Live-Trading-Examples/RSI%20Range%20Strategy.py/>`_

15. `RSI with SMA Trend Filter.py <https://github.com/fxcm/RestAPI/blob/master/Python-Live-Trading-Examples/RSI%20with%20SMA%20Trend%20Filter.py>`_

16. `SMA Crossover Strategy.py <https://github.com/fxcm/RestAPI/blob/master/Python-Live-Trading-Examples/SMA%20Crossover%20Strategy.py/>`_

17. `Yves Webinar Code.ipynb <https://github.com/fxcm/RestAPI/find/master/>`_


**Disclaimer**:

Trading forex/CFDs on margin carries a high level of risk and may not be suitable for all investors as you could sustain losses in excess of deposits. Leverage can work against you. The products are intended for retail and professional clients. Due to the certain restrictions imposed by the local law and regulation, German resident retail client(s) could sustain a total loss of deposited funds but are not subject to subsequent payment obligations beyond the deposited funds. Be aware and fully understand all risks associated with the market and trading. Prior to trading any products, carefully consider your financial situation and experience level. If you decide to trade products offered by FXCM Australia Pty. Limited (“FXCM AU”) (AFSL 309763), you must read and understand the `Financial Services Guide <https://docs.fxcorporate.com/financial-services-guide-au.pdf/>`_, `Product Disclosure Statement  <https://www.fxcm.com/au/legal/product-disclosure-statements/>`_, and `Terms of Business <https://docs.fxcorporate.com/tob_au_en.pdf/>`_. Any opinions, news, research, analyses, prices, or other information is provided as general market commentary, and does not constitute investment advice. FXCM will not accept liability for any loss or damage, including without limitation to, any loss of profit, which may arise directly or indirectly from use of or reliance on such information. FXCM will not accept liability for any loss or damage, including without limitation to, any loss of profit, which may arise directly or indirectly from use of or reliance on such information.