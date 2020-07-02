#region Usings
using System.Collections.Generic;
using System.Linq;
#endregion

namespace Semi.Hsms.Messages
{
	/// <summary>
	/// 
	/// </summary>
	public class DataMessage : Message
	{
		#region Class members
		/// <summary>
		/// 
		/// </summary>
		protected bool _isReplyRequired;
		/// <summary>
		/// 
		/// </summary>
		private List<DataItem> _items;
		#endregion

		#region Class properties
		/// <summary>
		/// 
		/// </summary>
		public byte Function { get; protected set; }
		/// <summary>
		/// 
		/// </summary>
		public byte Stream { get; protected set; }
		/// <summary>
		/// 
		/// </summary>
		public override MessageType Type => MessageType.DataMessage;
		/// <summary>
		/// 
		/// </summary>
		public IEnumerable<DataItem> Items => _items.ToList();
		/// <summary>
		/// 
		/// </summary>
		public override bool IsReplyRequired => IsPrimary;
		/// <summary>
		/// 
		/// </summary>
		public override bool IsPrimary
		{
			get { return ( 0 != ( Function % 2 ) ); }
		}
		/// <summary>
		/// 
		/// </summary>
		public static DataMessageBuilder Builder => new DataMessageBuilder();
		#endregion

		#region Class initialization
		/// <summary>
		/// 
		/// </summary>
		/// <param name="device"></param>
		/// <param name="context"></param>
		private DataMessage( ushort device, uint context )
				: base( device, context )
		{
		}
		#endregion

		#region Class internal structs
		/// <summary>
		/// 
		/// </summary>
		public class DataMessageBuilder
		{
			#region Class members
			/// <summary>
			/// 
			/// </summary>
			private byte _function;
			/// <summary>
			/// 
			/// </summary>
			/// <returns></returns>
			private byte _stream;
			/// <summary>
			/// 
			/// </summary>
			private ushort _device;
			/// <summary>
			/// 
			/// </summary>
			private uint _context;
			/// <summary>
			/// 
			/// </summary>
			private List<DataItem> _items = new List<DataItem>();
			#endregion

			#region Class public methods
			/// <summary>
			/// 
			/// </summary>
			/// <param name="function"></param>
			/// <returns></returns>
			public DataMessageBuilder Function( byte function )
			{
				_function = function;
				return this;
			}
			/// <summary>
			/// 
			/// </summary>
			/// <param name="stream"></param>
			/// <returns></returns>
			public DataMessageBuilder Stream( byte stream )
			{
				_stream = stream;
				return this;
			}
			/// <summary>
			/// 
			/// </summary>
			/// <param name="device"></param>
			/// <returns></returns>
			public DataMessageBuilder Device( ushort device )
			{
				_device = device;
				return this;
			}
			/// <summary>
			/// 
			/// </summary>
			/// <param name="context"></param>
			/// <returns></returns>
			public DataMessageBuilder Context( uint context )
			{
				_context = context;
				return this;
			}
			/// <summary>
			/// 
			/// </summary>
			/// <param name="context"></param>
			/// <returns></returns>
			public DataMessageBuilder NewContext()
			{
				_context = Message.NextContext;
				return this;
			}
			/// <summary>
			/// 
			/// </summary>
			/// <param name="dataItem"></param>
			/// <returns></returns>
			public DataMessageBuilder Items( params DataItem [] dataItems )
			{
				foreach( var item in dataItems )
				{
					_items.Add( item );
				}

				return this;
			}
			/// <summary>
			/// 
			/// </summary>
			/// <param name="s"></param>
			/// <returns></returns>
			public DataMessageBuilder Items( IEnumerable<DataItem> items )
			{
				_items = ( null == items ) ? new List<DataItem>() : items.ToList();

				return this;
			}
			/// <summary>
			/// 
			/// </summary>
			/// <returns></returns>
			public DataMessage Build()
			{
				var dataMessage = new DataMessage( _device, _context )
				{
					Function = _function,
					Stream = _stream,
					_items = _items,
				};

				return dataMessage;
			}
			#endregion
		}
		#endregion

		#region Class public methods
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return $"Data.msg";
		}
		#endregion
	}
}