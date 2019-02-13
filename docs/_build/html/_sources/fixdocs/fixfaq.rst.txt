==========================
Frequently Asked Questions
==========================

Error Messages received
=======================

**ORA-20103: Session expired**

::

	SEVERE: onMessageRecieved::Forcereconnect from server. Update session state::19915;DAS 19915: ZDas Exception ORA-20103: Session expired.

Unordered List Item Error code ``20103`` session expired means your connection has been lost. This error message could be displayed due to a number of reasons, including network instability, a system issue or a client side program crash. If the problem is a system issue, please try to reboot.

**ORA-20143: Order price too far from market price**

::

	Done! request.RequestID: U10D1_0F742A280DE8276EE053182B3C0A1526_02192015163720178345_XIX0-2 offer.OfferID:1 AccountID: 831293 iAmount: 10000 dRate: 1.13953 dRateLimit: 1.14153 dRateStop: 1.13803 BuySell: B OrderType: LE 19915;DAS 19915: ZDas Exception ORA-20143: Order price too far from market price :1.14219 vs 1.14162

Unordered List Item This error message is generated when the Buy Limit price is above the Bid price. For example if the Bid price was 1.13919, and your Buy limit 1.13953. If you want to place a Buy Limit above the Bid price you can do so using an OpenLimit order, which is available in API ver. 1.3.2. The fill price will be the limit price or better.

**ORA-20112: Limit price did not pass validation**

::
	
	19915;DAS 19915: ZDas Exception ORA-20112: Limit price did not pass validation: A:308386 OF:22 SB:B

Unordered List Item This error message is generated when the Limit price does not correspond to the ask price for the order type required. If the Time in Force is IOC or FOK then the Buy limit price should be >= Ask price. For GTC or GTD the Buy Limit should be ⇐ Ask price.
	
**ORA-20113: Insufficient margin in session**

::

	19915;DAS 19915: ZDas Exception ORA-20113: Insufficient margin in session

Unordered List Item This error message is generated when you don’t have enough margin.

**ORA-20102: Access Violation**

::

	19915;DAS 19915: ZDas Exception ORA-20102: Access Violation: U10D1_0C73B32A0A4299C5E053182B3C0ADFC8_01122015125318715906_UGQ1 phase:4

Unordered List Item This error message is generated when a trade account is missing from the dealer account.

**ORA-20105: Order price did not pass validation**

:: 	

	8=FIX.4.4|9=179|35=D|1=2620231783|11=FXSTAT_32951421147085|15=EUR|38=5|40=3|54=2|55=ESP35|59=1|60=20150113-11:04:45|99=9864|386=1|336=FXCM|625=EUREAL|516=0|526=OT_-2_1421147086_ESP35_ESP35_0_0_|10=080

Unordered List Item The rejected orders error message is generated when the stop price is within 12 points of ask price. The reason for rejected orders is because the Minimum Stop Distance for ESP35 is 12 points. For example, if the Ask price was 9911 and your Stop price 99=9917, you would receive this error message. In this example, the stop price should be at least 9911 + 12 = 9923.

**ORA-20008: Failed to create order, primary validation**


This error message is generated when Range prices are below the Ask price. For example if orders were placed on news events, and the spreads got wider, e.g. If Buy range IOC 55=CAD/JPY 44=92.55100 99=95.55100 and Ask price was 95.612	
::

	8=FIX.4.4 9=190 35=D 34=19 49=2620237129_client1 52=20150304-15:00:00 56=FXCM 57=EUREAL 1=2620237129 11=28020200337fa23 38=1000000 40=4 44=92.55100 54=1 55=CAD/JPY 59=3 60=20150304-14:57:19.363 99=95.55100 10=252
	
	DEBUG (2015-03-04 10:00:00,171) [0:Bus:2620237129_client1FXCMEUREAL] (app) - »> app message to counterparty: 8=FIX.4.4 9=558 35=8 34=18308 49=FXCM 50=EUREAL 52=20150304-15:00:00.171 56=2620237129_client1 1=2620237129 6=0 11=28020200337fa23 14=0 15=CAD 17=0 31=0 32=0 37=NONE 38=1000000 39=8 40=4 44=0 54=1 55=CAD/JPY 58=19915;DAS 19915: ZDas Exception ORA-20008: Failed to create order, primary validation
	
**tag specified out of required order**

This error message is generated when the tag order does not pass our system check. You can avoid this by setting ``ValidateFieldsOutOfOrder=N`` in your config file.

**How can I tell what account type I have?**

Checking on Trading station: To check the type of account you have, you can login to Trading station and look in the tab “Accounts”. Scroll to the end and find column Type. ``Y = Hedging`` is allowed; ``N = Hedging`` is not allowed, ``O = Netting`` only, ``D = Day netting``, ``F = FIFO``

FIX: Using tag ``453`` and ``803 (PartySubIDType)`` ``Y = Hedging``, ``N = No Hedging``, ``0 = Netting``. This is located on page 34 of the documentation.

Limit order Day order vs Limit IOC/FOK
======================================

A Limit Order is an order to buy or sell a predetermined amount at a specified price. This order will be filled only when the market price equals the specified limit price or better. Limit orders also allow a trader to limit the length of time an order can be outstanding before being canceled with the following time in force values GTC, DAY, GTD, IOC, FOK.

The Limit price for order with TIF GTC or DAY (for future execution) should be market or better than current market price, but for orders with TIF IOC or FOK (immediate execution) the Limit price should be market or worse. This is because IOC/FOK orders will be sent immediately for execution, without waiting for the market price to reach the Limit price.

If the current Bid is 100, and you place a Sell Limit IOC/FOK at 102, this order will be rejected because the Limit price has to be market or worse for IOC/FOK, (it would be like asking to get order filled immediately at 102 or better).

If your intention was to control the slippage, you can use a Limit OIC or FOK. The Limit price should be the Bid price or below. So in this example, if Limit=98, the order will be filled at 98 or better, but because it’s immediate execution, if for any reason the order can’t be filled in few attempts, the order will be canceled. Can you fix the paragraph spacing below?

What is the Base Unit Size (also the minimum trade size) for all FX instruments?
================================================================================

tag ``53`` in collateral report(35=BA) For each CFD - tag ``228`` in Trading Session Status ``35=h``

How can I get Positions side and quantity from open position report?
====================================================================

First you need to send position report request ``35=AN`` with ``724=0`` (open position), ``724=1`` is for closed positions. If you have open position you will get positon report ``35=AP`` for each open position. If you don’t have it you will receive “no open position” in message ``AO`` instead of ``35=AP`` message. In the positon report, you need to look at ``704 (LongQty)`` or ``705(shortQty)``. If you see ``704`` it is long order (buy order), if you see ``705`` it is short order (sell order).
::

	8=FIX.4.4|9=149|35=AN|34=5|49=d101968168_client1|52=20151111-21:01:12.396|56=FXCM|57=U100D1|1=01958448|60=20151111-21:01:12.395|263=1|581=6|710=4|715=20151111|724=0|10=085|

	8=FIX.4.4|9=565|35=AP|34=8|49=FXCM|50=U100D1|52=20151111-20:19:59.929|56=d101968168_client1|1=01958448|11=FIX.4.4:d101968168_client1->FXCM/U100D1-1437981786837-10|15=EUR|37=207486895|55=EUR/USD|60=20150727-07:23:08|325=N|336=FXCM|526=fix_example_test|581=6|625=U100D1|710=4|715=20151111|721=3684204026|724=0|727=2|728=0|730=1.10728|731=1|734=0|912=N|9000=1|9038=260|9040=-21.16|9041=80775478|9042=20150727-07:23:08|9053=0.8|453=1|448=FXCMID|447=D|452=3|802=4|523=32|803=26|523=d101968168|803=2|523=fix-test112|803=22|523=1958448|803=10|702=1|703=TQ|704=10000|753=1|707=CASH|708=0|10=137|

How can I get closed positions?
===============================

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

How can I close open positions?
===============================

If your account is **non-hedgng** account, you just need to send same qantity with opposite side. 

If your account is **hedging** account, get the ticket id tag ``9041`` from open position then send single market order with ticket id = 9041 and opposite side.
::

	20160404-06:18:07.432 : 8=FIX.4.4 9=193 35=D 34=7 49=D101546502001_client1 52=20160404-06:18:07.432 56=FXCM 57=U100D1 1=01537581 11=635953582874324718 38=100000 40=1 44=0.99629 54=2 55=AUD/CAD 59=1 60=20160404-06:18:07 9041=89061181 10=006 

	20160404-06:18:07.922 : 8=FIX.4.4 9=489 35=8 34=15 49=FXCM 50=U100D1 52=20160404-06:18:07.634 56=D101546502001_client1 1=01537581 6=0.99631 11=635953582874324718 14=100000 15=AUD 17=819171964 31=0.99631 32=100000 37=225010828 38=100000 39=2 40=1 44=0.99631 54=2 55=AUD/CAD 58=Executed 59=1 60=20160404-06:18:07 99=0 150=F 151=0 211=0 336=FXCM 625=U100D1 835=0 836=0 1094=0 9000=16 9041=89061181 9050=CM 9051=F 9061=0 453=1 448=FXCM ID 447=D 452=3 802=4 523=1537581 803=10 523=d101546502001 803=2 523=Halpert 803=22 523=32 803=26 10=128
	
Sample Code in C++
==================


**Get FXCM System Parameters: C++**
::

	void FixApplication::onMessage(const FIX44::TradingSessionStatus& tss, const SessionID& session_ID)
	{
		int param_count = FIX::IntConvertor::convert(tss.getField(9016));
	 
		cout << "TSS - FXCM System Parameters" << endl;
		for(int i = 1; i =< param_count; i++){
			FIX::FieldMap map  = tss.getGroupRef(1,9016);
	 
			string param_name  = map.getField(9017);
			string param_value = map.getField(9018);
	 
			cout << param_name << " - " << param_value << endl;
		}
	}
	
**Get Rollover Interest: C++**

::

	void FixApplication::onMessage(const FIX44::TradingSessionStatus& tss, const SessionID& session_ID)
	{
		int symbols_count = IntConvertor::convert(tss.getField(FIELD::NoRelatedSym));
		for(int i = 1; i <= symbols_count; i++) {
			FIX44::SecurityList::NoRelatedSym symbols_group;
			tss.getGroup(i,symbols_group);
			string symbol = symbols_group.getField(FIELD::Symbol);
	 
			cout << "    Symbol -> " << symbol << endl;
			cout << "      RolloverBuy -> " << symbols_group.getField(9003) << endl;
			cout << "      RolloverSell -> " << symbols_group.getField(9004) << endl;
		}
	}
	
**Determine Hedging Status: C++**

::

	void FixApplication::onMessage(const FIX44::CollateralReport& cr, const SessionID& session_ID)
	{
		FIX44::CollateralReport::NoPartyIDs group;
		cr.getGroup(1,group); 
		cout << "  Parties -> "<< endl;
	 
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
	}	
	
**Subscribe to a Symbol: C++**

::

	string request_ID = "EUR_USD_Request_";
	FIX44::MarketDataRequest request;
	request.setField(MDReqID(request_ID));
	request.setField(SubscriptionRequestType(
		SubscriptionRequestType_SNAPSHOT_PLUS_UPDATES));
	request.setField(MarketDepth(0));
	request.setField(NoRelatedSym(1));
	 
	FIX44::MarketDataRequest::NoRelatedSym symbols_group;
	symbols_group.setField(Symbol("EUR/USD"));
	request.addGroup(symbols_group);
	 
	FIX44::MarketDataRequest::NoMDEntryTypes entry_types;
	entry_types.setField(MDEntryType(MDEntryType_BID));
	request.addGroup(entry_types);
	entry_types.setField(MDEntryType(MDEntryType_OFFER));
	request.addGroup(entry_types);
	entry_types.setField(MDEntryType(MDEntryType_TRADING_SESSION_HIGH_PRICE));
	request.addGroup(entry_types);
	entry_types.setField(MDEntryType(MDEntryType_TRADING_SESSION_LOW_PRICE));
	request.addGroup(entry_types);
	 
	Session::sendToTarget(request, sessionID);
	
**Subscribe to All Symbols: C++**

::

	void FixApplication::onMessage(const FIX44::TradingSessionStatus& tss, const SessionID& session_ID)
	{
		FIX44::MarketDataRequest request;
		request.setField(MDReqID(NextRequestID()));
		request.setField(SubscriptionRequestType(
			SubscriptionRequestType_SNAPSHOT_PLUS_UPDATES));
		request.setField(MarketDepth(0));
	 
		FIX44::MarketDataRequest::NoMDEntryTypes entry_types;
		entry_types.setField(MDEntryType(MDEntryType_BID));
		request.addGroup(entry_types);
		entry_types.setField(MDEntryType(MDEntryType_OFFER));
		request.addGroup(entry_types);
		entry_types.setField(MDEntryType(MDEntryType_TRADING_SESSION_HIGH_PRICE));
		request.addGroup(entry_types);
		entry_types.setField(MDEntryType(MDEntryType_TRADING_SESSION_LOW_PRICE));
		request.addGroup(entry_types);
	 
		int symbols_count = IntConvertor::convert(tss.getField(FIELD::NoRelatedSym));
		request.setField(NoRelatedSym(symbols_count));
		for(int i = 1; i <= symbols_count; i++){
			FIX44::SecurityList::NoRelatedSym symbols_group_SL;
			tss.getGroup(i,symbols_group_SL);
			string symbol = symbols_group_SL.getField(FIELD::Symbol);
	 
			FIX44::MarketDataRequest::NoRelatedSym symbols_group_MDR;
			symbols_group_MDR.setField(Symbol(symbol));
			request.addGroup(symbols_group_MDR);
		}
	 
		Session::sendToTarget(request, sessionID);
	}

**Create Market Order: C++**

::

	FIX44::NewOrderSingle order;
	order.setField(FIX::ClOrdID(NextClOrdID())); 
	order.setField(FIX::Account(account));
	order.setField(FIX::Symbol("EUR/USD")); 
	order.setField(FIX::Side(FIX::Side_BUY)); 
	order.setField(FIX::TransactTime(FIX::TransactTime())); 
	order.setField(FIX::OrderQty(10000));
	order.setField(FIX::OrdType(FIX::OrdType_MARKET));
	 
	FIX::Session::sendToTarget(order,session_id);
	
**Create Market Range Order: C++**


In the case of a market range order, we set the OrdType to StopLimit and we must set the ``StopPx`` tag. The ``StopPx`` tag indicates the worst price we are willing to get filled at; i.e., the stop.

::

	FIX44::NewOrderSingle order;
	order.setField(FIX::ClOrdID(NextClOrdID())); 
	order.setField(FIX::Account(account));
	order.setField(FIX::Symbol("EUR/USD")); 
	order.setField(FIX::Side(FIX::Side_BUY)); 
	order.setField(FIX::TransactTime(FIX::TransactTime())); 
	order.setField(FIX::OrderQty(10000));
	order.setField(FIX::OrdType(FIX::OrdType_STOPLIMIT));
	order.setField(FIX::StopPx(stop));
	 
	FIX::Session::sendToTarget(order,session_id);	

**Create Entry (Pending) Order: C++**

::

	FIX44::NewOrderSingle order;
	order.setField(FIX::ClOrdID(NextClOrdID())); 
	order.setField(FIX::Account(account));
	order.setField(FIX::Symbol("EUR/USD")); 
	order.setField(FIX::Side(FIX::Side_BUY)); 
	order.setField(FIX::TransactTime(FIX::TransactTime())); 
	order.setField(FIX::OrderQty(10000));
	order.setField(FIX::OrdType(FIX::OrdType_LIMIT)); 
	order.setField(FIX::Price(price));
	 
	FIX::Session::sendToTarget(order,session_id);

**Create One-Cancels-Other (OCO) Order: C++**

::

	FIX44::NewOrderList olist;
	 
	olist.setField(FIX::ListID(NextClOrdID())); 
	olist.setField(FIX::TotNoOrders(2));
	olist.setField(FIX::ContingencyType(FIX::ContingencyType_ONE_CANCELS_THE_OTHER)); 
	 
	FIX44::NewOrderList::NoOrders stop;
	stop.setField(FIX::ClOrdID(next_ClOrdID())); 
	stop.setField(FIX::ListSeqNo(0)); 
	stop.setField(FIX::ClOrdLinkID("1")); 
	stop.setField(FIX::Account(account));
	stop.setField(FIX::Symbol(symbol)); 
	stop.setField(FIX::Side(FIX::Side_SELL)); 
	stop.setField(FIX::OrderQty(20000));
	stop.setField(FIX::OrdType(FIX::OrdType_STOP)); 
	stop.setField(FIX::StopPx(stop_price));
	olist.addGroup(stop);
	 
	FIX44::NewOrderList::NoOrders limit;
	limit.setField(FIX::ClOrdID(next_ClOrdID())); 
	limit.setField(FIX::ListSeqNo(1)); 
	limit.setField(FIX::ClOrdLinkID("1")); 
	limit.setField(FIX::Account(account));
	limit.setField(FIX::Symbol(symbol)); 
	limit.setField(FIX::Side(FIX::Side_SELL));
	limit.setField(FIX::OrderQty(20000));
	limit.setField(FIX::OrdType(FIX::OrdType_LIMIT)); 
	limit.setField(FIX::Price(limit_price));
	olist.addGroup(limit);
	 
	FIX::Session::sendToTarget(olist,session_id);

**Create Entry with Limit and Stop (ELS) Order: C++**

The entry with limit and stop is a FXCM specific contingency type that allows you to associate a stop and limit with a specific position (or market order). In this case, the ContingencyType field must be set to ``101`` for the ELS contingency. When the stop or limit is executed, or when you close the position, these contingent orders will be deleted automatically. 
::

	FIX44::NewOrderList olist;
	 
	olist.setField(FIX::ListID(next_ClOrdID())); 
	olist.setField(FIX::TotNoOrders(3));
	olist.setField(FIX::FIELD::ContingencyType,"101");
	 
	FIX44::NewOrderList::NoOrders order;
	order.setField(FIX::ClOrdID(next_ClOrdID()));
	order.setField(FIX::ListSeqNo(0)); 
	order.setField(FIX::ClOrdLinkID("1"));
	order.setField(FIX::Account(account));
	order.setField(FIX::Symbol(symbol));
	order.setField(FIX::Side(FIX::Side_BUY));
	order.setField(FIX::Symbol(symbol));
	order.setField(FIX::OrderQty(10000));
	order.setField(FIX::OrdType(FIX::OrdType_MARKET)); 
	olist.addGroup(order);
	 
	FIX44::NewOrderList::NoOrders stop;
	stop.setField(FIX::ClOrdID(next_ClOrdID())); 
	stop.setField(FIX::ListSeqNo(1)); 
	stop.setField(FIX::ClOrdLinkID("2")); 
	stop.setField(FIX::Account(account));
	stop.setField(FIX::Side(FIX::Side_SELL));
	stop.setField(FIX::Symbol(symbol)); 
	stop.setField(FIX::OrderQty(10000)); 
	stop.setField(FIX::OrdType(FIX::OrdType_STOP)); 
	stop.setField(FIX::StopPx(stop_price));
	olist.addGroup(stop);
	 
	FIX44::NewOrderList::NoOrders limit;
	limit.setField(FIX::ClOrdID(next_ClOrdID())); 
	limit.setField(FIX::ListSeqNo(2)); 
	limit.setField(FIX::ClOrdLinkID("2")); 
	limit.setField(FIX::Account(account));
	limit.setField(FIX::Side(FIX::Side_SELL));
	limit.setField(FIX::Symbol(symbol)); 
	limit.setField(FIX::OrderQty(10000)); 
	limit.setField(FIX::OrdType(FIX::OrdType_LIMIT)); 
	limit.setField(FIX::Price(limit_price));
	olist.addGroup(limit);
	 
	FIX::Session::sendToTarget(olist,session_id);
	
**Create Market Order with Trailing Stop: C++**

In our example below, we use two orders with ELS contingency type (see above for details on ELS). Specifically, we send both a market order and a stop order. What makes this stop order a trailing stop is the existence of the ``FXCMPegFluctuatePts(9061)`` tag, which we have enumerated as ``FXCM_PEG_FLUCTUATE_PTS``. This field is set to``10`` which means our stop will trail the market at a rate of 10 pips. 
::

	FIX44::NewOrderList olist;
	olist.setField(FIX::ListID(next_ClOrdID())); 
	olist.setField(FIX::TotNoOrders(2)); 
	olist.setField(FIX::FIELD::ContingencyType,"101");
	 
	FIX44::NewOrderList::NoOrders order;
	order.setField(FIX::ClOrdID(next_ClOrdID()));
	order.setField(FIX::ListSeqNo(0)); 
	order.setField(FIX::ClOrdLinkID("1"));
	order.setField(FIX::Account(account)); 
	order.setField(FIX::Symbol(symbol)); 
	order.setField(FIX::Side(FIX::Side_BUY)); 
	order.setField(FIX::OrderQty(10000)); 
	order.setField(FIX::OrdType(FIX::OrdType_MARKET)); 
	olist.addGroup(order);
	 
	FIX44::NewOrderList::NoOrders stop;
	stop.setField(FIX::ClOrdID(next_ClOrdID())); 
	stop.setField(FIX::ListSeqNo(1)); 
	stop.setField(FIX::ClOrdLinkID("2"));
	stop.setField(FIX::Account(account)); 
	stop.setField(FIX::Side(FIX::Side_SELL));
	stop.setField(FIX::Symbol(symbol)); 
	stop.setField(FIX::OrderQty(10000)); 
	stop.setField(FIX::OrdType(FIX::OrdType_STOP)); 
	stop.setField(FIX::StopPx(stop_price));
	stop.setField(FXCM_PEG_FLUCTUATE_PTS, "10");
	olist.addGroup(stop);
	 
	FIX::Session::sendToTarget(olist,session_id);	
	
**Get Order Status and Executed Amount: C++**

::

	void FixApplication::onMessage(const FIX44::ExecutionReport& er, const SessionID& session_ID)
	{
		string status  = er.getField(FIELD::OrdStatus);
		string execQty = er.getField(FIELD::CumQty);
	 
		cout << "ExecutionReport ->" << endl;
		cout << "  OrderStatus: " << status << endl;
		if(status == "2" /*Filled*/ || status == "8" /*Rejected */ || status == "4" /*Cancelled*/) {
			cout << "    Executed Amount: "  << execQty << endl;
		}
	}
	
**Request All Open Positions: C++**

::

	FIX44::RequestForPositions request;
	request.setField(PosReqID(NextRequestID()));
	request.setField(PosReqType(PosReqType_POSITIONS));
	 
	request.setField(Account(account_ID)); 
	request.setField(SubscriptionRequestType(SubscriptionRequestType_SNAPSHOT_PLUS_UPDATES));
	request.setField(AccountType(
		AccountType_ACCOUNT_IS_CARRIED_ON_NON_CUSTOMER_SIDE_OF_BOOKS_AND_IS_CROSS_MARGINED));
	request.setField(TransactTime());
	request.setField(ClearingBusinessDate());
	request.setField(TradingSessionID("FXCM"));
	 
	Session::sendToTarget(request, sessionID);
	
**Request Open Positions for a Single Account: C++**

::

	FIX44::RequestForPositions request;
	request.setField(PosReqID(NextRequestID()));
	request.setField(PosReqType(PosReqType_POSITIONS));
	 
	request.setField(Account(account_ID)); 
	request.setField(SubscriptionRequestType(SubscriptionRequestType_SNAPSHOT_PLUS_UPDATES));
	request.setField(AccountType(
		AccountType_ACCOUNT_IS_CARRIED_ON_NON_CUSTOMER_SIDE_OF_BOOKS_AND_IS_CROSS_MARGINED));
	request.setField(TransactTime());
	request.setField(ClearingBusinessDate());
	request.setField(TradingSessionID("FXCM"));
	 
	request.setField(NoPartyIDs(1));
	FIX44::RequestForPositions::NoPartyIDs parties_group;
	parties_group.setField(PartyID("FXCM ID"));
	parties_group.setField(PartyIDSource('D'));
	parties_group.setField(PartyRole(3));
	parties_group.setField(NoPartySubIDs(1));
	FIX44::RequestForPositions::NoPartyIDs::NoPartySubIDs sub_parties;
	sub_parties.setField(PartySubIDType(PartySubIDType_SECURITIES_ACCOUNT_NUMBER));
	sub_parties.setField(PartySubID(account_ID));
	parties_group.addGroup(sub_parties);
	request.addGroup(parties_group);
	 
	Session::sendToTarget(request, sessionID);
	
**Get All Waiting Orders: C++**

::

	FIX44::OrderMassStatusRequest request;
	request.setField(MassStatusReqID(NextRequestID()));
	request.setField(MassStatusReqType(MassStatusReqType_STATUS_FOR_ALL_ORDERS));
	request.setField(Account(account_ID));
	Session::sendToTarget(request, sessionID);	

FXCM Custom fields
==================

From TradingSessionStatus(h)
----------------------------

``FXCMSymPrecision (9001)``

	This shows the numerical precision of the security. For example, the USD/JPY security would show a value of 3 for this field because it is quoted to 3 decimal places. AUD/USD would show a value of 5 for this field given that it is quoted to 5 decimal places.

``FXCMSymPointSize (9002)``
	
	The size of the point (pip) of the security. For example, the EUR/USD security would show a value of 0.0001 for this field. This is useful for many purposes, such as calculating the profit or loss of a position in points.

``FXCMSymInterestBuy (9003)``


``FXCMSymInterestSell (9004)``
	
	The price is in the currency of your account for the default lot size for your server. If your account is in USD and server default lot size is 10k For example for CAD/JPY 9003(FXCMSymInterestBuy) = 0.64 - you will get $0.64 for 10k 9004(FXCMSymInterestSell) = -1.48 - you will pay $1.48 for 10k The server default lot size you can get from same report from tags: 9017=BASE_UNIT_SIZE 9018=10000

	You can get it also from Trading Station in Simple Dealing Rates under columns Roll S and Roll B

``FXCMProductID (9080)``

	
	FXCMProductID distinguishes each security by its type. There are 5 types of securities: 1-Forex, 2-Index, 3-Commodity, 4-Treasury, and 5-Bullion. As an example, GBP/USD would obviously show a value of 1 for this field, but the CFD Index JPY225 would show a value of 2.

``FXCMCondDistStop (9090)``
	
	The value of this field indicates the minimum distance for stop orders on an open position. The distance referred to here is the distance between your stop order price and the current market price. For example, assume you want to place a stop order on an existing buy position. Your stop order price then must meet or exceed the minimum distance from the current bid (sell) price.

``FXCMCondDistLimit (9091)``
	
	The value of this field indicates the minimum distance for limit orders on an open position. The distance referred to here is the distance between your limit order price and the current market price. For example, assume you want to place a limit order on an existing buy position. Your limit order price then must meet or exceed the minimum distance from the current Bid (Sell) price.

``FXCMCondDistEntryStop (9092)``
	
	This field indicates the minimum distance for new stop entry (pending) orders. The distance referred to here is the distance between your stop entry order price and the current market price. For example, assume you wanted to place a stop entry order to buy. The price of this order must or exceed the minimum distance from the current Ask (Buy) price.

``FXCMCondDistEntryLimit (9093)``
	
	This field indicates the minimum distance for new limit entry (pending) orders. The distance referred to here is the distance between your limit entry order price and the current market price. For example, assume you wanted to place a limit entry order to buy. The price of this order must or exceed the minimum distance from the current Ask (Buy) price.

``FXCMMaxQuantity (9094)``

	
	This is the largest quantity for which you can place an order.

``FXCMMinQuantity (9095)``
	
	This is the smallest quantity for which you can place an order. This field only applies to CFD products. The minimum trade size for Forex must be obtained from the Quantity(53) field from CollateralReport.

``FXCMTradingStatus (9096)``
	
	This field indicates whether the trading desk is opened or closed. When trading is open, this will return “O” for Open. When trading is closed, this will return “C” for Closed. Forex securities are open throughout the entire trading week. However, CFD securities such as Indices often have daily schedules and/or daily break times. In order to determine if a CFD security is both Open and Tradeable, you must refer to the MarketDataSnapshot message. See Requesting Market Data for more on this topic.

From MarketDataSnapshotFullRefresh(W)
-------------------------------------

**Trading Status**

``Field: QuoteType(537)``
	
``Field: QuoteCondition(276)``

	To determine if a specific instrument is open and available for trading, you must refer to the QuoteType(537) and QuoteCondition(276) tags from this message. With QuoteType, a value of 0 = Indicative, and a value of 1 = Tradeable. With QuoteCondition, a value of “A” = Open, and a value of “B” = Closed.

From CollateralReport(BA)
-------------------------

**Minimum Order Qty. - Forex**
	
``Field: Quantity(53)``
	
	The value of this field represents the minimum quantity for which you can place a Forex order. This minimum quantity is specific to the trading account in the same CollateralReport. The Account(1) field can be used to obtain the AccountID. Most accounts are defaulted to 1,000 (Micro Lot).
	
FXCM System Parameters
======================

The following is a list of parameter names. You will see these returned as the values for Tag ``9017``, ``FXCMParamName``. The value of this parameter is found in Tag ``9018``, ``FXCMParamValue``.

``BASE_CRNCY``
	
	This parameter shows the currency of the account. Margin, P/L, balance, and equity will all be expressed in this currency. An example base currency would be “USD”.

``SERVER_TIME_UTC``

	The value of this parameter indicates whether or not time values sent from the server will be expressed in ``UTC``. If this is the case, the value of this parameter will be set to “UTC.” If the value of this parameter is not ``UTC``, then time values sent from the server will be expressed in the local time zone of the server, which can be checked with the ``BASE_TIME_ZONE`` parameter.

``BASE_TIME_ZONE``
	
	This parameter shows the name of the time zone of the server. For example, “America/New_York.”

``COND_DIST``
	
	This parameter shows the minimum recommended distance between the price of new stop or limit orders and the current market price. This is expressed in pips and it is generally defaulted to 0.10. It important to note that CFD securities often have their own minimum stop or limit distances, which should be checked in the SecurityList message.

``COND_DIST_ENTRY``
	
	This parameter shows the minimum recommended distance between the price of new stop entry or limit entry orders and the current market price. This is expressed in pips and it is generally defaulted to 0.10. It important to note that CFD securities often have their own minimum stop entry or limit entry distances, which should be checked in the ``SecurityList`` message.

``BASE_UNIT_SIZE``
	
	The minimum order size allowed for Forex securities. For example, 1,000 (Micro Lot) or 10,000 (Mini Lot). Note that in order to check the minimum order size for CFD securities, it is necessary to check the ``FXCMMinQuantity (9095)`` Tag from the ``SecurityList`` message. It is recommended that your application relies on this field when determining minimum order size for all securities, including Forex.

``END_TRADING_DAY``
	
	The value of this parameter contains the time when the trading day ends. The time is expressed in the format hh:mm:ss, where hh is in 24-hour format, mm is minutes, and ss is seconds. The time is always in UTC time. For example, 21:00:00.

**Disclaimer**

Trading forex/CFDs on margin carries a high level of risk and may not be suitable for all investors as you could sustain losses in excess of deposits. Leverage can work against you. The products are intended for retail and professional clients. Due to the certain restrictions imposed by the local law and regulation, German resident retail client(s) could sustain a total loss of deposited funds but are not subject to subsequent payment obligations beyond the deposited funds. Be aware and fully understand all risks associated with the market and trading. Prior to trading any products, carefully consider your financial situation and experience level. If you decide to trade products offered by FXCM Australia Pty. Limited (“FXCM AU”) (AFSL 309763), you must read and understand the Financial Services Guide, Product Disclosure Statement, and Terms of Business. Any opinions, news, research, analyses, prices, or other information is provided as general market commentary, and does not constitute investment advice. FXCM will not accept liability for any loss or damage, including without limitation to, any loss of profit, which may arise directly or indirectly from use of or reliance on such information. FXCM will not accept liability for any loss or damage, including without limitation to, any loss of profit, which may arise directly or indirectly from use of or reliance on such information.



