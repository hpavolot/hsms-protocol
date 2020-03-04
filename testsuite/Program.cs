#region Usings

using Semi.Hsms.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

#endregion

namespace Semi.Hsms.TestSuite
{
	class Program
	{
		#region Class public methods

		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		static void Main( string [] args )
		{
			var arr = new List<DataItem>();
			arr.Add( new I2( 32 ) );
			arr.Add( new U2( 2 ) );

			arr.Clear();

			var message = DataMessage
					.Builder
					.Stream( 1 )
					.Function( 5 )
					.Context( 522555 )
					.Items( arr )
					.Build();

			var copy = message.Items as List<DataItem>;
			copy.Clear();

			var st1 = new Student() { Name = "denis" };

			var col1 = new List<Student>();
			var col2 = new List<Student>();

			col1.Add( st1.Copy() );
			col2.Add( st1.Copy() );

			st1.Name = "lena";

			col1.Clear();
		}

		#endregion
	}

	public class Student 
	{
		public string Name { get; set; }

		public Student Copy() 
		{
			return new Student()
			{
				Name = Name
			};
		}
	}
}
