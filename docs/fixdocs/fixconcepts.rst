=============
Core Concepts
=============

TradingSessionStatusRequest (g)
===============================

After successful authentication, it is necessary to receive an update on the status of the market (open or closed), common system parameters, and a list of securities with their characteristics. The ``TradingSessionStatus`` message is returned as a response to the ``TradingSessionStatusRequest`` and it accomplishes these things.

TradingSessionStatus Message (h)
================================

The ``TradingSessionStatus`` message is used to provide an update on the status of the market. Furthermore, this message contains useful system parameters as well as information about each trading security (embedded ``SecurityList``).

``TradingSessionStatus`` should be requested upon successful ``Logon`` and subscribed to. The contents of the ``TradingSessionStatus`` message, specifically the ``SecurityList`` and system parameters, should dictate how fields are set when sending messages to FXCM. For example, the instrument component within ``SecurityList`` indicates minimum order quantity ``9095``. Subsequent ``NewOrderSingle (D)`` messages should not violate this value when setting the ``OrderQty field (38)``.

* Requesting ``TradingSessionStatus``

::

	FIX44::TradingSessionStatusRequest request;
	request.setField(FIX::TradSesReqID(NextId())); 
	request.setField(FIX::SubscriptionRequestType(FIX::SubscriptionRequestType_SNAPSHOT_PLUS_UPDATES));
 
* Reading System Parameters

Here we demonstrate how to extract the FXCM system properties from the ``TradingSessionStatus`` message. The code below will print out the name and value of the each of the properties. The following custom fields are used:

::

	9016 - FXCMNoParam
	9017 - FXCMParamName
	9018 - FXCMParamValue
	int param_count = FIX::IntConvertor::convert(status.getField(9016));
 
	cout << "TSS - FXCM System Parameters" << endl;
	for(int i = 1; i <= param_count; i++)
	{
		 FIX::FieldMap map = status.getGroupRef(i,9016);
		 string param_name = map.getField(9017);
		 string param_value = map.getField(9018);
	 
		 cout << param_name << " - " << param_value << endl;
	}
	FIX::Session::sendToTarget(request,session_id);

CollateralInquiry (BB)
======================

CollateralInquiry is used to request the ``CollateralReport(BA)`` message from FXCM. This message contains important account related information such as the account number. With the exception of FIX sessions used solely for market data, you should include this message in your login sequence.

The login you use to connect will have access to one or more trading accounts. You will receive a ``CollateralReport`` for each of these accounts. When sending or modifying orders, you must set the ``Account(1)`` tag. This tag value must be set only to an account that you have actually received a ``CollateralReport`` for, otherwise you will see a rejection.

Requesting Market Data
======================

The ``MarketDataSnapshotFullRefresh(W)`` message contains the updates to market data. It is obtained as a response to the ``MarketDataRequest(V)`` message. FIX connections are then subscription based for the market data; meaning, you must request it to receive it.

The types of data you can receive, such as the Bid price or Offer price, are referred to as ``MDEntryTypes`` in FIX. FXCM supports the following ``MDEntryTypes`` in each message: ``Bid(0)``, ``Offer(1)``, ``High Price(7)``, and ``Low Price(8)``. Additional ``MDEntryTypes`` such as ``MDEntryDate``, ``MDEntryTime``, ``QuoteCondition``, etc., are found only once within the first repeating group of the message.

* Sending MarketDataRequest(V) Message

::

	FIX44::MarketDataRequest mdr;
 
	mdr.set(FIX::MDReqID(NextId()));
	mdr.set(FIX::SubscriptionRequestType(FIX::SubscriptionRequestType_SNAPSHOT_PLUS_UPDATES));
	mdr.set(FIX::MarketDepth(0));
	mdr.set(FIX::NoMDEntryTypes(2));
 
	FIX44::MarketDataRequest::NoMDEntryTypes types_group;
	types_group.set(FIX::MDEntryType(FIX::MDEntryType_BID));
	mdr.addGroup(types_group);
	types_group.set(FIX::MDEntryType(FIX::MDEntryType_OFFER));
	mdr.addGroup(types_group);
 
	int no_sym = FIX::IntConvertor::convert(security_list.getField(FIX::FIELD::NoRelatedSym));
	for(int i = 1; i <= no_sym; i++)
	{
	   FIX44::SecurityList::NoRelatedSym sym_group;
	   mdr.addGroup(security_list.getGroup(i,sym_group));
	}
	 
	FIX::Session::sendToTarget(mdr,session_id);
	
Getting Positions
=================

Open and closed positions are retrieved through the ``PositionReport(AP)`` message. Unlike the ``ExecutionReport(8)`` message which contains information relating to orders, the ``PositionReport`` is not automatically sent to your FIX client. You can make individual requests for ``PositionReport``, or you can subscribe to updates on this message. Sending a ``RequestForPositions(AN)`` message with ``SubscriptionRequestType(263)`` set to ``1 (SnapshotAndUpdates)`` will subscribe to updates.	

Open Position vs. Closed Position
---------------------------------

``PosReqType(724)`` is used to determine if a received ``PositionReport`` represents an open position or closed position. A value of ``0`` indicates an open position while a value of ``1`` indicates a closed position.

Open Positions
^^^^^^^^^^^^^^

When a ``PositionReport`` representing an open position is sent to you, it will contain the price at which the position was opened. This can be seen using ``SettlPrice(730)``. The close price and the P/L of the position are not present given that the position is open. Close price and P/L are real-time calculated values and are not contain in any ``PositionReport`` where ``PosReqType(724) = 0`` (Open Position).

Closed Positions
^^^^^^^^^^^^^^^^

``PositionReport`` messages representing closed positions will include the price at which the position was closed, as well as other useful information such as P/L. The following additional tags are present:

::

	Tag							Description
	FXCMPosClosePNL (9052)		Gross P/L of the position; e.g., $24.17, or €43.72
	FXCMPosInterest (9040)		Rollover interest applied to the position
	FXCMPosCommission (9053)	Commission applied to the position
	FXCMCloseSettlPrice (9043)	Close price of the position
	
Position Margin
^^^^^^^^^^^^^^^

The margin applied to each individual position can be obtained from a ``PositionReport`` representing an open position. ``FXCMUsedMargin(9038)`` will contain this margin value. Note that the total margin required for an account can be obtained by this same tag from the ``CollateralReport(BA)`` message.	

Overview of Basic Order and Time-In-Force Types
===============================================

Time-In-Force (TIF) Types
-------------------------

The ``Time-In-Force(59)`` Tag is used to indicate how long an order should remain active before it is either executed by the broker or cancelled by the client. Below are the four TIF values with descriptions.

Good Til Cancel (GTC)
^^^^^^^^^^^^^^^^^^^^^

Orders with this TIF value remain open and active until fully executed or cancelled. This means that the order remains active until the entire order amount is executed.

**When to use GTC**
	
Use GTC when your order must remain active until it can be filled
Use GTC when your entire order must get filled
	
Day
^^^

	Orders with this TIF value will remain open and active until fully executed, cancelled by the client, or when the trading day ends. Like Good Til Cancel (GTC), this means the order will remain active until the entire order amount is executed, unless the order is cancelled or the trading day ends.

**When to use Day**

Use Day when your original intention for the order becomes obsolete with time.
	
Immediate or Cancel (IOC)
^^^^^^^^^^^^^^^^^^^^^^^^^

Orders with this TIF value will immediately attempt to execute as much of your order as possible and cancel any remaining amount if necessary. As a result, this TIF value will allow partial fills.

**When to use IOC**

Use IOC when you expect execution to take place immediately
Use IOC when it is acceptable if your entire order does not get filled

Fill or Kill (FOK)
^^^^^^^^^^^^^^^^^^

Orders with this TIF value will attempt to execute the entire order amount immediately. If the entire order amount cannot be executed, the order is cancelled.

**When to use FOK**

Use FOK when you expect execution to take place immediately
Use FOK when your entire order must get filled

Order Types
-----------

Market
^^^^^^

A market order is an order to buy or sell immediately at the next available price. This means that the order is not guaranteed to fill at any specific price.

**When to Use Market**

Use market when your order being filled is more important than the price it is filled at.
	
**Supported TIF Values**

GTC, DAY, IOC, and FOK

Market Range (Stop-Limit)
^^^^^^^^^^^^^^^^^^^^^^^^^

Market range is a market order that comes with a limitation on the price at which the order can be filled. In other words, it is a market order with a protection against slippage (price deviation).

In FIX terms, you can convert a market order to a market range order by setting the ``OrdType (40)`` to ``4 (Stop-Limit)`` and by setting the ``StopPx(99)`` Tag. The StopPx tag value should be set to the worst price you would accept being filled.

**Example**

Assume you want to Buy EUR/USD now while it is trading at 1.4531 but you do not want to get filled at a price higher than 1.4535. In this case you would set the ``StopPx (99)`` tag value to 1.4535. If your order cannot be filled at 1.4535 or below, it will be cancelled.

**When to use Market Range**

Use market range when you are concerned about slippage
Use market range when it is acceptable that your order may be cancelled

**Supported TIF Values**

IOC and FOK

Limit
^^^^^

A limit order is an order to buy or sell only at a specific price (or better). In other words, the order can only be filled at the limit price or for some better price.

**When to use Limit**

Use limit when you must guarantee the price at which an order is filled

**Supported TIF Values**

GTC, Day, IOC, and FOK

**Common Applications**

The limit order can be used to achieve multiple objectives when combined with different TIF values. The two common application are:

GTC/Day Limit Order
^^^^^^^^^^^^^^^^^^^

Recall that both GTC and DAY remain active until the entire order is filled, until cancelled, or the trading day ends (for DAY orders). When you combine the limit order with these TIF values, you have an order that will remain active until the entire amount is filled at your limit price or better. This type of order is often used to close an existing position and ensure the position is closed at a specific rate.

IOC/FOK Limit Order
^^^^^^^^^^^^^^^^^^^

Recall that with IOC and FOK, your order will immediately attempt execution. In the case of IOC, part of the order will be filled if possible. In the case of FOK, the entire order must be filled. When you combine IOC/FOK with the limit order, you have an order which will attempt execution immediately but will fill only at your limit price or better. This order type is commonly used to guarantee the price at which a new order is filled while also controlling how much can be filled; IOC would allow partial fills while FOK would not.

Stop
^^^^

	A stop is an order to buy or sell some amount when the current market price reaches your stop price. In other words, a stop order is a market order which is waiting to be active until the market price reaches a certain level (your stop price). Given that a stop is effectively a type of market order, it does not guarantee any specific fill price.

**When to use Stop**

Use stop when your order being filled is more important than the price it is filled at
	
**Supported TIF Values**

GTC and DAY
^^^^^^^^^^^

Handling of Partial Fills
=========================

Order Quantity Fields
---------------------

	There are three fields which can be used to determine the quantity filled or rejected by FXCM. These fields are:

	* LastQty (32) – the quantity filled on the last successful attempt to fill the order
	* CumQty (14) – the total quantity filled
	* LeavesQty (151) – the remaining quantity to be filled
	* Importance of OrdStatus (39)
	
	It is important to consider ``OrdStatus (39)`` when using the quantity fields above. As FXCM is attempting to execute an order, the values of ``OrdStatus`` will progress from an initial value of ``New (0)`` to some final state. There are three possible final values for ``OrdStatus``:

	* OrdStatus = Filled (2)
	* OrdStatus = Rejected (8)
	* OrdStatus = Cancelled (4)
	
	When you receive an ExecutionReport ``8`` with ``OrdStatus`` set to one of these final values, you can inspect the ``CumQty (14)`` field to determine the total amount executed. If ``OrdStatus`` = ``Filled`` (2), the entire order was filled and ``CumQty`` will equal the original ``OrdQty`` value. If ``OrdStatus`` = ``Rejected (8)``, the order was partially filled and ``CumQty`` will be some value less than the original ``OrdQty``.

Example Partial Fill
--------------------

	The following ExecutionReport messages serve as an example of a partially filled order. The original OrderQty(38) was 1,000,000. In this example only 600,000 of the order was filled. The most important line here is the last, where we can see a final ``OrdStatus`` value (Rejected in this case). When this last ``ExecutionReport`` is received, we can inspect ``CumQty(14)`` to see that 600,000 was filled.
	
::

	OrderQty(38) = 1,000,000; OrdStatus(39) = New 
	6=85.558 14=0 17=59342024 31=85.558 32=0 37=31654622 38=1000000 39=0 40=4 44=85.55 854=2 59=3 99=0 150=0 151=1000000 211=0835=0 836=0 1094=0 9000=17 9041=13151786 9050=OR 9051=P 9061=0

	OrderQty(38) = 1,000,000; OrdStatus(39) = Stopped 
	6=85.558 14=0 17=59342025 31=85.558 32=0 37=31654622 38=1000000 39=7 40=4 44=85.55 854=2 59=3 99=0 150=7 151=1000000 211=0835=0 836=0 1094=0 9000=17 9041=13151786 9050=OR 9051=U 9061=0

	OrderQty(38) = 1,000,000; OrdStatus(39) = Stopped 
	6=85.488 14=0 17=59342047 31=85.488 32=0 37=31654622 38=1000000 39=7 40=4 44=85.48 854=2 9=3 99=0 150=7 151=1000000 211=0835=0 836=0 1094=0 9000=17 9041=13151786 9050=OR 9051=U 9061=0

	OrderQty(38) = 1,000,000; OrdStatus(39) = Partially Filled; LastQty(32) = 600,000; CumQty(14) = 600,000 
	6=85.488 14=600000 17=59342048 31=85.488 32=600000 37=31654622 38=1000000 39=1 40=4 44=85.488 54=259=399=0 150=F 151=400000 211=0 835=0 836=0 1094=0 9000=17 9041=13151888 9050=OR 9051=U 9061=0

	OrderQty(38) = 1,000,000; OrdStatus(39) = Rejected; CumQty(14) = 600,000 
	6=85.488 14=600000 17=59342049 31=85.488 32=0 37=31654622 38=1000000 39=8 40=4 44=85.488 54=258=Rejected 59=399=0 150=8 151=0 211=0 835=0 836=0 1094=0 9000=17 9041=13151888 9050=OR 9051=R 9061=0

Closing A Position
------------------

How you close a position depends upon the position maintenance type of the account. Some accounts support hedging while others do not. Hedging is the ability to have two positions in the same symbol but of a different side; for example, holding both Buy EUR/USD and Sell EUR/USD positions at the same time.

**Accounts with Hedging**

	Accounts that support hedging allow you to close individual positions, regardless of when they were opened relative to other positions. Clearly with these accounts, Buy and Sell orders do not offset themselves but instead form a hedge. Consequently, you must close these positions with a NewOrderSingle message that specifies the ``TicketID`` to close.

Sending A Closing Order
-----------------------

	NewOrderSingle (D) can be used to close a specific position simply by setting the ``FXCMPosID (9041)`` field. This converts a basic market order into a closing order.

* Closing Order in Code

::

		FIX44::NewOrderSingle order;
		 
		order.setField(ClOrdID(NextClOrdID())); 
		order.setField(Account(account_ID));
		order.setField(Symbol("EUR/USD")); 
		order.setField(Side(Side_BUY)); 
		order.setField(TransactTime()); 
		order.setField(OrderQty(10000));
		order.setField(OrdType(OrdType_MARKET));
		order.setField(FXCM_POS_ID/*9041*/, ”84736256”);
	 
	Session::sendToTarget(order, session_ID);
	
**Accounts without Hedging**

	For accounts without hedging, orders of the opposite Side cancel each other out; e.g., sending a ``NewOrderSingle`` with a Side of Buy will net against any existing Sell positions. This netting is done in First-In, First-Out ``FIFO`` order. As a result, a basic market order will suffice to close any open position.

Getting Account Position Maintenance
====================================

	The position maintenance type of each account can be retrieved from the Parties component of ``CollateralReport (BA)``. The ``NoPartySubIDs`` group contains a custom PartySubIDType for position maintenance. This specific ``PartySubIDType`` tag is set to a value of ``4000``. ``PartySubID`` can be checked for the value of position maintenance. ``Y`` = “Hedging Enabled,” ``N`` = “No Hedging,” and ``0`` = “Netting.” Anything other than ``Y`` implies hedging is disabled and we will not use closing orders.

Getting Position Maintenance in Code
------------------------------------

::

		int number_subID = IntConvertor::convert(group.getField(FIELD::NoPartySubIDs));
		for(int u = 1; u <= number_subID; u++){
			FIX44::CollateralReport::NoPartyIDs::NoPartySubIDs sub_group;
			group.getGroup(u, sub_group);
		 
			string sub_type  = sub_group.getField(FIELD::PartySubIDType);
			string sub_value = sub_group.getField(FIELD::PartySubID);
			if(sub_type == "4000"){
				// Check sub_value for position maintenance 
				// Y = Hedging
				// N = No Hedging
				// 0 = Netting
			}
		}

When To Reset MsgSeqNum
=======================

Reset On Logon
--------------

``MsgSeqNum`` should be reset upon each Logon. This means that every Logon message should include tags ``MsgSeqNum (34)`` set to “1” and ``ResetSeqNumFlag (141)`` set to “Yes.” It is necessary to reset upon each Logon due to the fact that connections to FXCM are load balanced against a cluster of servers. This promotes a stable trading environment for users, but it also means you should reset upon each Logon.

Example Logon Message::

	8(BeginString)=FIX.4.4 
	9(BodyLength)=114  
	35(MsgType)=A  
	34(MsgSeqNum)=1
	49(SenderCompID)=sender_client1  
	52(SendingTime)=20120927-13:15:34.754  
	56(TargetCompID)=FXCM  
	57(TargetSubID)=U100D1  
	553(Username)=some_user  
	554(Password)=some_password 
	98(EncryptMethod)=0  
	108(HeartBtInt)=30  
	141(ResetSeqNumFlag)=Y
	10(CheckSum)=146
	
Account Equity
==============

FIX API does not have a field which represents account equity. Equity is a real-time value that is dependent upon a floating price. If your application needs immediate access to equity in real-time, you would have to calculate it using market data. However, the ``CollateralReport (BA)`` does provide an equity value that corresponds with a specific time in the trading day.

StartCash(921)
==============

The ``StartCash (921)`` field from CollateralReport is the equity value of the account at 5:00pm EST (New York). This can be used as a snapshot of what the equity was at that time. This value will include the account balance and any profit or loss on open trades.

Steps To Retrieve Short Version Of Market Price
===============================================

FXCM give client an oppertunity to retrieve market price for just ``Bid/Ask``, please follow instructions at `here <https://docs.fxcorporate.com/api-message-info.pdf/>`_.

EMF
====

.. note::

	Important detail to note about order execution with FXCM is the difference between order fill notification and order finished notification. As an order is filled by a liquidity provider, client will be sent a fill confirmation in the form of an execution report that includes 35=8|39=7|150=F or, in case of a partial fill, 35=8|39=1|150=F. This confirmation is sent as soon as the LP confirms the trade. After the order is completed and every database operation associated with it is committed, the client will be sent an execution report of order being done. This execution report includes 35=8|39=2|150=F.Alternatively, if the order was filled only partially before being canceled, the final confirmation will include 35=8|39=4|150=4. You can find the remaining quantity that was not filled in tag 151. It is important to note, that the final execution report can be sent much later. When looking for fill confirmations, clients can take advantage of faster notifications than before implementing  EMF. Even if clients are not taking advantage of the EMF execution, they will always be notified of the orders being filled. The only difference would be the delivery delay.

Execution Disclaimer
====================

FXCM aggregates bid and ask prices from a pool of liquidity providers and is the final counterparty when trading forex on FXCM's dealing desk and No Dealing Desk (NDD) execution models. With NDD, FXCM's platforms display the best-available direct bid and ask prices from the liquidity providers. In addition to the spread, the trading cost with NDD is a fixed lot-based commission at the open and close of the trade. While generally NDD accounts offer spreads with no markups, in some circumstances, FXCM may add a markup to NDD spreads. This may occur due to, but not limited to, account type, such as accounts opened through a referring agent. With dealing desk execution, FXCM can act as the dealer on any or all currency pairs. Backup liquidity providers fill in when FXCM does not act as the dealer. FXCM’s dealing desk has fewer liquidity providers than NDD. There are many other factors to consider when choosing an execution model (such as conflict of interest, trading style or strategy). See Execution Risks. Note: Contractual relationships with liquidity providers are consolidated through the FXCM Group, which, in turn, provides technology and pricing to the group affiliate entities.

Commonly Asked Questions
========================

1. Error Messages received
--------------------------

a. ZDas Exception ORA-20103: Session expired
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
::

	SEVERE: onMessageRecieved::Forcereconnect from server. Update session state::19915;DAS 19915: ZDas Exception ORA-20103: Session expired.

Unordered List Item Error code ``20103`` session expired means your connection has been lost. This error message could be displayed due to a number of reasons, including network instability, a system issue or a client side program crash. If the problem is a system issue, please try to reboot.

b. ZDas Exception ORA-20143: Order price too far from market price
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
::

	Done! request.RequestID: U10D1_0F742A280DE8276EE053182B3C0A1526_02192015163720178345_XIX0-2 offer.OfferID:1 AccountID: 831293 iAmount: 10000 dRate: 1.13953 dRateLimit: 1.14153 dRateStop: 1.13803 BuySell: B OrderType: LE 19915;DAS 19915: ZDas Exception ORA-20143: Order price too far from market price :1.14219 vs 1.14162

Unordered List Item This error message is generated when the Buy Limit price is above the Bid price. For example if the Bid price was 1.13919, and your Buy limit 1.13953. If you want to place a Buy Limit above the Bid price you can do so using an OpenLimit order, which is available in API ver. 1.3.2. The fill price will be the limit price or better.

c. ZDas Exception ORA-20112: Limit price did not pass validation
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
::
	
	19915;DAS 19915: ZDas Exception ORA-20112: Limit price did not pass validation: A:308386 OF:22 SB:B

Unordered List Item This error message is generated when the Limit price does not correspond to the ask price for the order type required. If the Time in Force is IOC or FOK then the Buy limit price should be >= Ask price. For GTC or GTD the Buy Limit should be ⇐ Ask price.
	
d. ZDas Exception ORA-20113: Insufficient margin in session
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
::

	19915;DAS 19915: ZDas Exception ORA-20113: Insufficient margin in session

Unordered List Item This error message is generated when you don’t have enough margin.

e. ZDas Exception ORA-20102: Access Violation
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
::

	19915;DAS 19915: ZDas Exception ORA-20102: Access Violation: U10D1_0C73B32A0A4299C5E053182B3C0ADFC8_01122015125318715906_UGQ1 phase:4

Unordered List Item This error message is generated when a trade account is missing from the dealer account.

f. ZDas Exception ORA-20105: Order price did not pass validation
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
:: 	

	8=FIX.4.4|9=179|35=D|1=2620231783|11=FXSTAT_32951421147085|15=EUR|38=5|40=3|54=2|55=ESP35|59=1|60=20150113-11:04:45|99=9864|386=1|336=FXCM|625=EUREAL|516=0|526=OT_-2_1421147086_ESP35_ESP35_0_0_|10=080

Unordered List Item The rejected orders error message is generated when the stop price is within 12 points of ask price. The reason for rejected orders is because the Minimum Stop Distance for ESP35 is 12 points. For example, if the Ask price was 9911 and your Stop price 99=9917, you would receive this error message. In this example, the stop price should be at least 9911 + 12 = 9923.

g. ORA-20008: Failed to create order, primary validation
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

This error message is generated when Range prices are below the Ask price. For example if orders were placed on news events, and the spreads got wider, e.g. If Buy range IOC 55=CAD/JPY 44=92.55100 99=95.55100 and Ask price was 95.612	
::

	8=FIX.4.4 9=190 35=D 34=19 49=2620237129_client1 52=20150304-15:00:00 56=FXCM 57=EUREAL 1=2620237129 11=28020200337fa23 38=1000000 40=4 44=92.55100 54=1 55=CAD/JPY 59=3 60=20150304-14:57:19.363 99=95.55100 10=252
	
	DEBUG (2015-03-04 10:00:00,171) [0:Bus:2620237129_client1FXCMEUREAL] (app) - »> app message to counterparty: 8=FIX.4.4 9=558 35=8 34=18308 49=FXCM 50=EUREAL 52=20150304-15:00:00.171 56=2620237129_client1 1=2620237129 6=0 11=28020200337fa23 14=0 15=CAD 17=0 31=0 32=0 37=NONE 38=1000000 39=8 40=4 44=0 54=1 55=CAD/JPY 58=19915;DAS 19915: ZDas Exception ORA-20008: Failed to create order, primary validation
	
2. tag specified out of required order
--------------------------------------

This error message is generated when the tag order does not pass our system check. You can avoid this by setting ``ValidateFieldsOutOfOrder=N`` in your config file.

3. How can I tell what account type I have?
-------------------------------------------

Checking on Trading station: To check the type of account you have, you can login to Trading station and look in the tab “Accounts”. Scroll to the end and find column Type. ``Y = Hedging`` is allowed; ``N = Hedging`` is not allowed, ``O = Netting`` only, ``D = Day netting``, ``F = FIFO``

FIX: Using tag ``453`` and ``803 (PartySubIDType)`` ``Y = Hedging``, ``N = No Hedging``, ``0 = Netting``. This is located on page 34 of the documentation.

4. Limit order Day order vs Limit IOC/FOK
-----------------------------------------

A Limit Order is an order to buy or sell a predetermined amount at a specified price. This order will be filled only when the market price equals the specified limit price or better. Limit orders also allow a trader to limit the length of time an order can be outstanding before being canceled with the following time in force values GTC, DAY, GTD, IOC, FOK.

The Limit price for order with TIF GTC or DAY (for future execution) should be market or better than current market price, but for orders with TIF IOC or FOK (immediate execution) the Limit price should be market or worse. This is because IOC/FOK orders will be sent immediately for execution, without waiting for the market price to reach the Limit price.

If the current Bid is 100, and you place a Sell Limit IOC/FOK at 102, this order will be rejected because the Limit price has to be market or worse for IOC/FOK, (it would be like asking to get order filled immediately at 102 or better).

If your intention was to control the slippage, you can use a Limit OIC or FOK. The Limit price should be the Bid price or below. So in this example, if Limit=98, the order will be filled at 98 or better, but because it’s immediate execution, if for any reason the order can’t be filled in few attempts, the order will be canceled. Can you fix the paragraph spacing below?

5. What is the Base Unit Size (also the minimum trade size) for all FX instruments?
-----------------------------------------------------------------------------------

tag ``53`` in collateral report(35=BA) For each CFD - tag ``228`` in Trading Session Status ``35=h``

6. How can I get Positions side and quantity from open position report?
-----------------------------------------------------------------------

First you need to send position report request ``35=AN`` with ``724=0`` (open position), ``724=1`` is for closed positions. If you have open position you will get positon report ``35=AP`` for each open position. If you don’t have it you will receive “no open position” in message ``AO`` instead of ``35=AP`` message. In the positon report, you need to look at ``704 (LongQty)`` or ``705(shortQty)``. If you see ``704`` it is long order (buy order), if you see ``705`` it is short order (sell order).
::

	8=FIX.4.4|9=149|35=AN|34=5|49=d101968168_client1|52=20151111-21:01:12.396|56=FXCM|57=U100D1|1=01958448|60=20151111-21:01:12.395|263=1|581=6|710=4|715=20151111|724=0|10=085|

	8=FIX.4.4|9=565|35=AP|34=8|49=FXCM|50=U100D1|52=20151111-20:19:59.929|56=d101968168_client1|1=01958448|11=FIX.4.4:d101968168_client1->FXCM/U100D1-1437981786837-10|15=EUR|37=207486895|55=EUR/USD|60=20150727-07:23:08|325=N|336=FXCM|526=fix_example_test|581=6|625=U100D1|710=4|715=20151111|721=3684204026|724=0|727=2|728=0|730=1.10728|731=1|734=0|912=N|9000=1|9038=260|9040=-21.16|9041=80775478|9042=20150727-07:23:08|9053=0.8|453=1|448=FXCMID|447=D|452=3|802=4|523=32|803=26|523=d101968168|803=2|523=fix-test112|803=22|523=1958448|803=10|702=1|703=TQ|704=10000|753=1|707=CASH|708=0|10=137|

7. How can I get closed positions?
----------------------------------

First you need to send position request with ``724=1``
::

	8=FIX.4.4|9=177|35=AN|34=6|49=d101968168_client1|52=20151111-21:01:12.400|56=FXCM|57=U100D1|1=01958448|60=20151111-21:01:12.400|263=1|581=6|710=5|715=20151111|724=1|9012=20150311|9014=20151112|10=110|
	9043       Closing price
	730         Open price
	9052       PnL for the closed position
	9053       Commission 
	9040       Interest  fee associated whit this position
	9038       used margin

	8=FIX.4.4|9=702|35=AP|34=20|49=FXCM|50=U100D1|52=20151111-21:01:11.936|56=d101968168_client1|1=01958448|11=FIX.4.4:d101968168_client1->FXCM/U100D1-1428599035518-4|15=EUR|37=202027586|55=EUR/USD|60=20150519-03:30:43|325=N|336=FXCM|526=fix_example_test|581=6|625=U100D1|710=5|715=20151111|721=3533878441|724=1|727=13|728=0|730=1.06572|731=1|734=0|912=Y|9000=1|9040=-6.08|9041=78911063|9042=20150409-17:03:56|9043=1.12979|9044=20150519-03:30:43|9048=U100D1_16679142D2EE08ABE053142B3C0A452A_05192015032653174913_QCV-127|9049=FXTS|9052=640.7|9053=0.8|9054=204437509|453=1|448=FXCM ID|447=D|452=3|802=4|523=32|803=26|523=d101968168|803=2|523=fix-test112|803=22|523=1958448|803=10|702=1|703=TQ|704=10000|753=1|707=CASH|708=0|10=042|

8. How can I close open positions?
----------------------------------

If your account is **non-hedgng** account, you just need to send same qantity with opposite side. 

If your account is **hedging** account, get the ticket id tag ``9041`` from open position then send single market order with ticket id = 9041 and opposite side.
::

	20160404-06:18:07.432 : 8=FIX.4.4 9=193 35=D 34=7 49=D101546502001_client1 52=20160404-06:18:07.432 56=FXCM 57=U100D1 1=01537581 11=635953582874324718 38=100000 40=1 44=0.99629 54=2 55=AUD/CAD 59=1 60=20160404-06:18:07 9041=89061181 10=006 

	20160404-06:18:07.922 : 8=FIX.4.4 9=489 35=8 34=15 49=FXCM 50=U100D1 52=20160404-06:18:07.634 56=D101546502001_client1 1=01537581 6=0.99631 11=635953582874324718 14=100000 15=AUD 17=819171964 31=0.99631 32=100000 37=225010828 38=100000 39=2 40=1 44=0.99631 54=2 55=AUD/CAD 58=Executed 59=1 60=20160404-06:18:07 99=0 150=F 151=0 211=0 336=FXCM 625=U100D1 835=0 836=0 1094=0 9000=16 9041=89061181 9050=CM 9051=F 9061=0 453=1 448=FXCM ID 447=D 452=3 802=4 523=1537581 803=10 523=d101546502001 803=2 523=Halpert 803=22 523=32 803=26 10=128

**Disclaimer**

High Risk Investment Warning: Trading foreign exchange and/or contracts for differences on margin carries a high level of risk, and may not be suitable for all investors. The possibility exists that you could sustain a loss in excess of your deposited funds and therefore, you should not speculate with capital that you cannot afford to lose. Before deciding to trade the products offered by FXCM you should carefully consider your objectives, financial situation, needs and level of experience. You should be aware of all the risks associated with trading on margin. FXCM provides general advice that does not take into account your objectives, financial situation or needs. The content of this Website must not be construed as personal advice. FXCM recommends you seek advice from a separate financial advisor. 
