#region S# License
/******************************************************************************************
NOTICE!!!  This program and source code is owned and licensed by
StockSharp, LLC, www.stocksharp.com
Viewing or use of this code requires your acceptance of the license
agreement found at https://github.com/StockSharp/StockSharp/blob/master/LICENSE
Removal of this comment is a violation of the license agreement.

Project: StockSharp.Algo.Storages.Binary.Algo
File: TransactionBinarySerializer.cs
Created: 2015, 12, 14, 1:43 PM

Copyright 2010 by StockSharp, LLC
*******************************************************************************************/
#endregion S# License
namespace StockSharp.Algo.Storages.Binary
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;

	using Ecng.Collections;
	using Ecng.Common;
	using Ecng.Serialization;

	using StockSharp.Messages;
	using StockSharp.Localization;

	class TransactionSerializerMetaInfo : BinaryMetaInfo<TransactionSerializerMetaInfo>
	{
		public TransactionSerializerMetaInfo(DateTime date)
			: base(date)
		{
			FirstOrderId = -1;
			FirstTransactionId = -1;
			FirstOriginalTransactionId = -1;
			FirstTradeId = -1;

			Portfolios = new List<string>();
			ClientCodes = new List<string>();
			BrokerCodes = new List<string>();
			DepoNames = new List<string>();
			UserOrderIds = new List<string>();
			Comments = new List<string>();
			SystemComments = new List<string>();
			Errors = new List<string>();
		}

		public override object LastId => LastTransactionId;

		public long FirstOrderId { get; set; }
		public long LastOrderId { get; set; }

		public long FirstTradeId { get; set; }
		public long LastTradeId { get; set; }

		public long FirstTransactionId { get; set; }
		public long LastTransactionId { get; set; }

		public long FirstOriginalTransactionId { get; set; }
		public long LastOriginalTransactionId { get; set; }

		public decimal FirstCommission { get; set; }
		public decimal LastCommission { get; set; }

		public decimal FirstPnL { get; set; }
		public decimal LastPnL { get; set; }

		public decimal FirstPosition { get; set; }
		public decimal LastPosition { get; set; }

		public decimal FirstSlippage { get; set; }
		public decimal LastSlippage { get; set; }

		public IList<string> Portfolios { get; }
		public IList<string> ClientCodes { get; }
		public IList<string> BrokerCodes { get; }
		public IList<string> DepoNames { get; }

		public IList<string> UserOrderIds { get; }

		public IList<string> Comments { get; }
		public IList<string> SystemComments { get; }

		public IList<string> Errors { get; }

		public override void Write(Stream stream)
		{
			base.Write(stream);

			stream.Write(FirstOrderId);
			stream.Write(LastOrderId);
			stream.Write(FirstTradeId);
			stream.Write(LastTradeId);
			stream.Write(FirstTransactionId);
			stream.Write(LastTransactionId);
			stream.Write(FirstOriginalTransactionId);
			stream.Write(LastOriginalTransactionId);
			stream.Write(FirstPrice);
			stream.Write(LastPrice);
			stream.Write(FirstCommission);
			stream.Write(LastCommission);
			stream.Write(FirstPnL);
			stream.Write(LastPnL);
			stream.Write(FirstPosition);
			stream.Write(LastPosition);
			stream.Write(FirstSlippage);
			stream.Write(LastSlippage);

			WriteList(stream, Portfolios);
			WriteList(stream, ClientCodes);
			WriteList(stream, BrokerCodes);
			WriteList(stream, DepoNames);
			WriteList(stream, UserOrderIds);
			WriteList(stream, Comments);
			WriteList(stream, SystemComments);
			WriteList(stream, Errors);

			WriteNonSystemPrice(stream);
			WriteFractionalVolume(stream);

			WriteLocalTime(stream, MarketDataVersions.Version47);

			if (Version < MarketDataVersions.Version51)
				return;

			stream.Write(ServerOffset);

			if (Version < MarketDataVersions.Version56)
				return;

			WriteOffsets(stream);
		}

		private static void WriteList(Stream stream, IList<string> list)
		{
			stream.Write(list.Count);

			foreach (var item in list)
				stream.Write(item);
		}

		private static void ReadList(Stream stream, IList<string> list)
		{
			var count = stream.Read<int>();

			for (var i = 0; i < count; i++)
				list.Add(stream.Read<string>());
		}

		public override void Read(Stream stream)
		{
			base.Read(stream);

			FirstOrderId = stream.Read<long>();
			LastOrderId = stream.Read<long>();
			FirstTradeId = stream.Read<long>();
			LastTradeId = stream.Read<long>();
			FirstTransactionId = stream.Read<long>();
			LastTransactionId = stream.Read<long>();
			FirstOriginalTransactionId = stream.Read<long>();
			LastOriginalTransactionId = stream.Read<long>();
			FirstPrice = stream.Read<decimal>();
			LastPrice = stream.Read<decimal>();
			FirstCommission = stream.Read<decimal>();
			LastCommission = stream.Read<decimal>();
			FirstPnL = stream.Read<decimal>();
			LastPnL = stream.Read<decimal>();
			FirstPosition = stream.Read<decimal>();
			LastPosition = stream.Read<decimal>();
			FirstSlippage = stream.Read<decimal>();
			LastSlippage = stream.Read<decimal>();

			ReadList(stream, Portfolios);
			ReadList(stream, ClientCodes);
			ReadList(stream, BrokerCodes);
			ReadList(stream, DepoNames);
			ReadList(stream, UserOrderIds);
			ReadList(stream, Comments);
			ReadList(stream, SystemComments);
			ReadList(stream, Errors);

			ReadNonSystemPrice(stream);
			ReadFractionalVolume(stream);

			ReadLocalTime(stream, MarketDataVersions.Version47);

			if (Version < MarketDataVersions.Version51)
				return;

			ServerOffset = stream.Read<TimeSpan>();

			if (Version < MarketDataVersions.Version56)
				return;

			ReadOffsets(stream);
		}

		public override void CopyFrom(TransactionSerializerMetaInfo src)
		{
			base.CopyFrom(src);

			FirstOrderId = src.FirstOrderId;
			LastOrderId = src.LastOrderId;
			FirstTradeId = src.FirstTradeId;
			LastTradeId = src.LastTradeId;
			FirstTransactionId = src.FirstTransactionId;
			LastTransactionId = src.LastTransactionId;
			FirstOriginalTransactionId = src.FirstOriginalTransactionId;
			LastOriginalTransactionId = src.LastOriginalTransactionId;
			FirstPrice = src.FirstPrice;
			LastPrice = src.LastPrice;
			FirstCommission = src.FirstCommission;
			LastCommission = src.LastCommission;
			FirstPnL = src.FirstPnL;
			LastPnL = src.LastPnL;
			FirstPosition = src.FirstPosition;
			LastPosition = src.LastPosition;
			FirstSlippage = src.FirstSlippage;
			LastSlippage = src.LastSlippage;

			Portfolios.Clear();
			Portfolios.AddRange(src.Portfolios);

			ClientCodes.Clear();
			ClientCodes.AddRange(src.ClientCodes);

			BrokerCodes.Clear();
			BrokerCodes.AddRange(src.BrokerCodes);

			DepoNames.Clear();
			DepoNames.AddRange(src.DepoNames);

			UserOrderIds.Clear();
			UserOrderIds.AddRange(src.UserOrderIds);

			Comments.Clear();
			Comments.AddRange(src.Comments);

			SystemComments.Clear();
			SystemComments.AddRange(src.Comments);

			Errors.Clear();
			Errors.AddRange(src.Errors);
		}
	}

	class TransactionBinarySerializer : BinaryMarketDataSerializer<ExecutionMessage, TransactionSerializerMetaInfo>
	{
		public TransactionBinarySerializer(SecurityId securityId)
			: base(securityId, 200, MarketDataVersions.Version58)
		{
		}

		protected override void OnSave(BitArrayWriter writer, IEnumerable<ExecutionMessage> messages, TransactionSerializerMetaInfo metaInfo)
		{
			if (metaInfo.IsEmpty())
			{
				var msg = messages.First();

				metaInfo.FirstOrderId = metaInfo.LastOrderId = msg.OrderId ?? 0;
				metaInfo.FirstTradeId = metaInfo.LastTradeId = msg.TradeId ?? 0;
				metaInfo.FirstTransactionId = metaInfo.LastTransactionId = msg.TransactionId;
				metaInfo.FirstOriginalTransactionId = metaInfo.LastOriginalTransactionId = msg.OriginalTransactionId;
				metaInfo.FirstCommission = metaInfo.LastCommission = msg.Commission ?? 0;
				metaInfo.FirstPnL = metaInfo.LastPnL = msg.PnL ?? 0;
				metaInfo.FirstPosition = metaInfo.LastPosition = msg.Position ?? 0;
				metaInfo.FirstSlippage = metaInfo.LastSlippage = msg.Slippage ?? 0;
				metaInfo.ServerOffset = msg.ServerTime.Offset;
			}

			writer.WriteInt(messages.Count());

			var allowNonOrdered = metaInfo.Version >= MarketDataVersions.Version48;
			var isUtc = metaInfo.Version >= MarketDataVersions.Version51;
			var allowDiffOffsets = metaInfo.Version >= MarketDataVersions.Version56;

			foreach (var msg in messages)
			{
				//var isTrade = msg.ExecutionType == ExecutionTypes.Trade;

				if (msg.ExecutionType != ExecutionTypes.Transaction)
					throw new ArgumentOutOfRangeException(nameof(messages), msg.ExecutionType, LocalizedStrings.Str1695Params.Put(msg.OrderId ?? msg.TradeId));

				// нулевой номер заявки возможен при сохранении в момент регистрации
				if (msg.OrderId < 0)
					throw new ArgumentOutOfRangeException(nameof(messages), msg.OrderId, LocalizedStrings.Str925);

				// нулевая цена возможна, если идет "рыночная" продажа по инструменту без планок
				if (msg.OrderPrice < 0)
					throw new ArgumentOutOfRangeException(nameof(messages), msg.OrderPrice, LocalizedStrings.Str926Params.Put(msg.OrderId == null ? msg.OrderStringId : msg.OrderId.To<string>()));

				//var volume = msg.Volume;

				//if (volume < 0)
				//	throw new ArgumentOutOfRangeException(nameof(messages), volume, LocalizedStrings.Str927Params.Put(msg.OrderId == null ? msg.OrderStringId : msg.OrderId.To<string>()));

				if (msg.HasTradeInfo())
				{
					//if ((msg.TradeId == null && msg.TradeStringId.IsEmpty()) || msg.TradeId <= 0)
					//	throw new ArgumentOutOfRangeException(nameof(messages), msg.TradeId, LocalizedStrings.Str928Params.Put(msg.TransactionId));

					if (msg.TradePrice == null || msg.TradePrice <= 0)
						throw new ArgumentOutOfRangeException(nameof(messages), msg.TradePrice, LocalizedStrings.Str929Params.Put(msg.TradeId, msg.OrderId));
				}

				//writer.WriteInt((int)msg.ExecutionType);

				metaInfo.LastTransactionId = writer.SerializeId(msg.TransactionId, metaInfo.LastTransactionId);
				metaInfo.LastOriginalTransactionId = writer.SerializeId(msg.OriginalTransactionId, metaInfo.LastOriginalTransactionId);

				writer.Write(msg.HasOrderInfo);
				writer.Write(msg.HasTradeInfo);

				if (metaInfo.Version < MarketDataVersions.Version50)
					metaInfo.LastOrderId = writer.SerializeId(msg.OrderId ?? 0, metaInfo.LastOrderId);
				else
				{
					writer.Write(msg.OrderId != null);

					if (msg.OrderId != null)
					{
						metaInfo.LastOrderId = writer.SerializeId(msg.OrderId.Value, metaInfo.LastOrderId);
					}
					else
					{
						writer.Write(!msg.OrderStringId.IsEmpty());

						if (!msg.OrderStringId.IsEmpty())
							writer.WriteString(msg.OrderStringId);
					}

					writer.Write(!msg.OrderBoardId.IsEmpty());

					if (!msg.OrderBoardId.IsEmpty())
						writer.WriteString(msg.OrderBoardId);
				}

				if (metaInfo.Version < MarketDataVersions.Version50)
					metaInfo.LastTradeId = writer.SerializeId(msg.TradeId ?? 0, metaInfo.LastTradeId);
				else
				{
					writer.Write(msg.TradeId != null);

					if (msg.TradeId != null)
					{
						metaInfo.LastTradeId = writer.SerializeId(msg.TradeId.Value, metaInfo.LastTradeId);
					}
					else
					{
						writer.Write(!msg.TradeStringId.IsEmpty());

						if (!msg.TradeStringId.IsEmpty())
							writer.WriteString(msg.TradeStringId);
					}
				}

				if (msg.OrderPrice != 0)
				{
					writer.Write(true);
					writer.WritePriceEx(msg.OrderPrice, metaInfo, SecurityId);
				}
				else
					writer.Write(false);

				if (msg.TradePrice != null)
				{
					writer.Write(true);
					writer.WritePriceEx(msg.TradePrice.Value, metaInfo, SecurityId);
				}
				else
					writer.Write(false);

				writer.Write(msg.Side == Sides.Buy);

				//if (metaInfo.Version < MarketDataVersions.Version57)
				//{
				//	writer.WriteVolume(msg.OrderVolume ?? msg.TradeVolume ?? 0, metaInfo, SecurityId);
				//}
				//else
				//{
				writer.Write(msg.OrderVolume != null);

				if (msg.OrderVolume != null)
					writer.WriteVolume(msg.OrderVolume.Value, metaInfo, SecurityId);

				writer.Write(msg.TradeVolume != null);

				if (msg.TradeVolume != null)
					writer.WriteVolume(msg.TradeVolume.Value, metaInfo, SecurityId);
				//}

				if (metaInfo.Version < MarketDataVersions.Version54)
				{
					writer.WriteVolume(msg.VisibleVolume ?? 0, metaInfo, SecurityId);
					writer.WriteVolume(msg.Balance ?? 0, metaInfo, SecurityId);
				}
				else
				{
					writer.Write(msg.VisibleVolume != null);

					if (msg.VisibleVolume != null)
						writer.WriteVolume(msg.VisibleVolume.Value, metaInfo, SecurityId);

					writer.Write(msg.Balance != null);

					if (msg.Balance != null)
						writer.WriteVolume(msg.Balance.Value, metaInfo, SecurityId);
				}

				var lastOffset = metaInfo.LastServerOffset;
				metaInfo.LastTime = writer.WriteTime(msg.ServerTime, metaInfo.LastTime, LocalizedStrings.Str930, allowNonOrdered, isUtc, metaInfo.ServerOffset, allowDiffOffsets, ref lastOffset);
				metaInfo.LastServerOffset = lastOffset;

				writer.Write(msg.OrderType != null);

				if (msg.OrderType != null)
					writer.WriteInt((int)msg.OrderType.Value);

				writer.WriteNullableInt(msg.OrderState);
				writer.WriteNullableInt(msg.OrderStatus);

				if (metaInfo.Version < MarketDataVersions.Version52)
					writer.WriteInt(msg.TradeStatus ?? 0);
				else
					writer.WriteNullableInt(msg.TradeStatus);
				
				if (metaInfo.Version < MarketDataVersions.Version53)
					writer.WriteInt((int)(msg.TimeInForce ?? TimeInForce.PutInQueue));
				else
				{
					writer.Write(msg.TimeInForce != null);

					if (msg.TimeInForce != null)
						writer.WriteInt((int)msg.TimeInForce.Value);
				}

				if (metaInfo.Version < MarketDataVersions.Version52)
					writer.Write(msg.IsSystem ?? true);
				else
				{
					writer.Write(msg.IsSystem != null);

					if (msg.IsSystem != null)
						writer.Write(msg.IsSystem.Value);
				}

				if (msg.ExpiryDate != null)
				{
					writer.Write(true);
					writer.WriteLong(msg.ExpiryDate.Value.Ticks);
					writer.WriteInt(msg.ExpiryDate.Value.Offset.Hours);
					writer.WriteInt(msg.ExpiryDate.Value.Offset.Minutes);
				}
				else
					writer.Write(false);

				metaInfo.LastCommission = Write(writer, msg.Commission, metaInfo.LastCommission);
				metaInfo.LastPnL = Write(writer, msg.PnL, metaInfo.LastPnL);
				metaInfo.LastPosition = Write(writer, msg.Position, metaInfo.LastPosition);
				metaInfo.LastSlippage = Write(writer, msg.Slippage, metaInfo.LastSlippage);

				WriteString(writer, metaInfo.Portfolios, msg.PortfolioName);
				WriteString(writer, metaInfo.ClientCodes, msg.ClientCode);
				WriteString(writer, metaInfo.BrokerCodes, msg.BrokerCode);
				WriteString(writer, metaInfo.DepoNames, msg.DepoName);
				WriteString(writer, metaInfo.UserOrderIds, msg.UserOrderId);
				WriteString(writer, metaInfo.Comments, msg.Comment);
				WriteString(writer, metaInfo.SystemComments, msg.SystemComment);
				WriteString(writer, metaInfo.Errors, msg.Error?.Message);

				if (metaInfo.Version < MarketDataVersions.Version55)
					continue;

				writer.Write(msg.Currency != null);

				if (msg.Currency != null)
					writer.WriteInt((int)msg.Currency.Value);

				writer.Write(msg.Latency != null);

				if (msg.Latency != null)
					writer.WriteLong(msg.Latency.Value.Ticks);

				writer.Write(msg.OriginSide != null);

				if (msg.OriginSide != null)
					writer.Write(msg.OriginSide.Value == Sides.Buy);
			}
		}

		public override ExecutionMessage MoveNext(MarketDataEnumerator enumerator)
		{
			var reader = enumerator.Reader;
			var metaInfo = enumerator.MetaInfo;

			//var execType = (ExecutionTypes)reader.ReadInt();
			//var isTrade = execType == ExecutionTypes.Trade;

			metaInfo.FirstTransactionId += reader.ReadLong();
			metaInfo.FirstOriginalTransactionId += reader.ReadLong();

			var hasOrderInfo = reader.Read();
			var hasTradeInfo = reader.Read();

			long? orderId = null;
			long? tradeId = null;

			string orderBoardId = null;
			string orderStringId = null;
			string tradeStringId = null;

			if (metaInfo.Version < MarketDataVersions.Version50)
			{
				orderId = reader.ReadLong();
				metaInfo.FirstOrderId += orderId.Value;
			}
			else
			{
				if (reader.Read())
				{
					orderId = reader.ReadLong();
					metaInfo.FirstOrderId += orderId.Value;
				}
				else
				{
					if (reader.Read())
						orderStringId = reader.ReadString();
				}

				if (reader.Read())
					orderBoardId = reader.ReadString();
			}

			if (metaInfo.Version < MarketDataVersions.Version50)
			{
				tradeId = reader.ReadLong();
				metaInfo.FirstTradeId += tradeId.Value;
			}
			else
			{
				if (reader.Read())
				{
					tradeId = reader.ReadLong();
					metaInfo.FirstTradeId += tradeId.Value;
				}
				else
				{
					if (reader.Read())
						tradeStringId = reader.ReadString();
				}
			}

			var orderPrice = reader.Read() ? reader.ReadPriceEx(metaInfo) : (decimal?)null;
			var tradePrice = reader.Read() ? reader.ReadPriceEx(metaInfo) : (decimal?)null;

			var side = reader.Read() ? Sides.Buy : Sides.Sell;

			var orderVolume = reader.Read() ? reader.ReadVolume(metaInfo) : (decimal?)null;
			var tradeVolume = reader.Read() ? reader.ReadVolume(metaInfo) : (decimal?)null;

			var visibleVolume = (metaInfo.Version < MarketDataVersions.Version54 || reader.Read())
				? reader.ReadVolume(metaInfo) : (decimal?)null;

			var balance = (metaInfo.Version < MarketDataVersions.Version54 || reader.Read())
				? reader.ReadVolume(metaInfo) : (decimal?)null;

			var allowNonOrdered = metaInfo.Version >= MarketDataVersions.Version48;
			var isUtc = metaInfo.Version >= MarketDataVersions.Version51;
			var allowDiffOffsets = metaInfo.Version >= MarketDataVersions.Version56;

			var prevTime = metaInfo.FirstTime;
			var lastOffset = metaInfo.FirstServerOffset;
			var serverTime = reader.ReadTime(ref prevTime, allowNonOrdered, isUtc, metaInfo.GetTimeZone(isUtc, SecurityId), allowDiffOffsets, ref lastOffset);
			metaInfo.FirstTime = prevTime;
			metaInfo.FirstServerOffset = lastOffset;

			var type = reader.Read() ? reader.ReadInt().To<OrderTypes>() : (OrderTypes?)null;

			var state = reader.ReadNullableInt<OrderStates>();
			var status = reader.ReadNullableInt<OrderStatus>();

			var tradeStatus = metaInfo.Version < MarketDataVersions.Version52
				? reader.ReadInt()
				: reader.ReadNullableInt<int>();

			var timeInForce = metaInfo.Version < MarketDataVersions.Version53
				? reader.ReadInt().To<TimeInForce>()
				: reader.Read() ? reader.ReadInt().To<TimeInForce>() : (TimeInForce?)null;

			var isSystem = metaInfo.Version < MarketDataVersions.Version52
						? reader.Read()
						: (reader.Read() ? reader.Read() : (bool?)null);

			var expDate = reader.Read() ? reader.ReadLong().To<DateTime>().ApplyTimeZone(new TimeSpan(reader.ReadInt(), reader.ReadInt(), 0)) : (DateTimeOffset?)null;

			var commission = reader.Read() ? metaInfo.FirstCommission = reader.ReadDecimal(metaInfo.FirstCommission) : (decimal?)null;
			var pnl = reader.Read() ? metaInfo.FirstPnL = reader.ReadDecimal(metaInfo.FirstPnL) : (decimal?)null;
			var position = reader.Read() ? metaInfo.FirstPosition = reader.ReadDecimal(metaInfo.FirstPosition) : (decimal?)null;
			var slippage = reader.Read() ? metaInfo.FirstSlippage = reader.ReadDecimal(metaInfo.FirstSlippage) : (decimal?)null;

			var portfolio = ReadString(reader, metaInfo.Portfolios);
			var clientCode = ReadString(reader, metaInfo.ClientCodes);
			var brokerCode = ReadString(reader, metaInfo.BrokerCodes);
			var depoName = ReadString(reader, metaInfo.DepoNames);
			var userOrderId = ReadString(reader, metaInfo.UserOrderIds);
			var comment = ReadString(reader, metaInfo.Comments);
			var sysComment = ReadString(reader, metaInfo.SystemComments);

			var msg = new ExecutionMessage
			{
				ExecutionType = ExecutionTypes.Transaction,
				SecurityId = SecurityId,

				ServerTime = serverTime,

				TransactionId = metaInfo.FirstTransactionId,
				OriginalTransactionId = metaInfo.FirstOriginalTransactionId,

				Side = side,
				OrderVolume = orderVolume,
				TradeVolume = tradeVolume,
				VisibleVolume = visibleVolume,
				Balance = balance,

				OrderType = type,
				OrderState = state,
				OrderStatus = status,
				TimeInForce = timeInForce,
				IsSystem = isSystem,
				ExpiryDate = expDate,
				Commission = commission,
				PnL = pnl,
				Position = position,
				Slippage = slippage,
				PortfolioName = portfolio,
				ClientCode = clientCode,
				BrokerCode = brokerCode,
				DepoName = depoName,
				UserOrderId = userOrderId,
				Comment = comment,
				SystemComment = sysComment,

				TradeStatus = tradeStatus,

				HasOrderInfo = hasOrderInfo,
				HasTradeInfo = hasTradeInfo,

				OrderPrice = orderPrice ?? 0,
				TradePrice = tradePrice,

				OrderId = orderId,
				TradeId = tradeId,

				OrderBoardId = orderBoardId,
				OrderStringId = orderStringId,
				TradeStringId = tradeStringId,
			};

			var error = ReadString(reader, metaInfo.Errors);

			if (!error.IsEmpty())
				msg.Error = new InvalidOperationException(error);

			if (metaInfo.Version >= MarketDataVersions.Version55)
			{
				if (reader.Read())
					msg.Currency = (CurrencyTypes)reader.ReadInt();

				if (reader.Read())
					msg.Latency = reader.ReadLong().To<TimeSpan>();

				if (reader.Read())
					msg.OriginSide = reader.Read() ? Sides.Buy : Sides.Sell;
			}

			return msg;
		}

		private static decimal Write(BitArrayWriter writer, decimal? value, decimal last)
		{
			if (value == null)
			{
				writer.Write(false);
				return last;
			}
			else
			{
				writer.Write(true);
				writer.WriteDecimal((decimal)value, last);

				return value.Value;
			}
		}

		private static void WriteString(BitArrayWriter writer, IList<string> items, string value)
		{
			if (value.IsEmpty())
				writer.Write(false);
			else
			{
				writer.Write(true);

				items.TryAdd(value);
				writer.WriteInt(items.IndexOf(value));
			}
		}

		private static string ReadString(BitArrayReader reader, IList<string> items)
		{
			if (!reader.Read())
				return null;

			return items[reader.ReadInt()];
		}
	}
}