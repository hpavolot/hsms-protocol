using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;

namespace hsms.wpf
{
	public class BaseViewModel : INotifyPropertyChanged
	{
		#region Class events
		/// <summary>
		/// 
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;
		/// <summary>
		/// 
		/// </summary>
		/// <param name="field"></param>
		/// <param name="value"></param>
		/// <param name="memberExpression"></param>
		protected void ChangeAndNotify<T>( ref T field, T value, Expression<Func<T>> memberExpression )
		{
			PropertyChanged.ChangeAndNotify( ref field, value, memberExpression );
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sPropName"></param>
		protected void FirePropertyChanged( string sPropName )
		{
			if( null == PropertyChanged )
				return;

			PropertyChanged( this, new PropertyChangedEventArgs( sPropName ) );
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="memberExpression"></param>
		protected void FirePropertyChanged<T>( Expression<Func<T>> memberExpression )
		{
			PropertyChanged.Notify( memberExpression );
		}
		#endregion
	}

	/// <summary>
	/// 
	/// </summary>
	public static class Ext
	{
		#region Class public methods
		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="handler"></param>
		/// <param name="field"></param>
		/// <param name="value"></param>
		/// <param name="memberExpression"></param>
		/// <returns></returns>
		public static bool ChangeAndNotify<T>( this PropertyChangedEventHandler handler,
		 ref T field, T value, Expression<Func<T>> memberExpression )
		{
			if( memberExpression == null )
				throw new ArgumentNullException( "memberExpression" );

			var body = memberExpression.Body as MemberExpression;

			if( body == null )
				throw new ArgumentException( "Lambda must return a property." );

			if( EqualityComparer<T>.Default.Equals( field, value ) )
				return false;

			var vmExpression = body.Expression as ConstantExpression;

			if( vmExpression != null )
			{
				var lambda = Expression.Lambda( vmExpression );
				var vmFunc = lambda.Compile();
				var sender = vmFunc.DynamicInvoke();

				if( handler != null )
				{
					handler( sender, new PropertyChangedEventArgs( body.Member.Name ) );
				}
			}

			field = value;

			return true;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="handler"></param>
		/// <param name="field"></param>
		/// <param name="value"></param>
		/// <param name="memberExpression"></param>
		/// <returns></returns>
		public static void Notify<T>( this PropertyChangedEventHandler handler,
			Expression<Func<T>> memberExpression )
		{
			if( memberExpression == null )
				throw new ArgumentNullException( "memberExpression" );

			var body = memberExpression.Body as MemberExpression;

			if( body == null )
				throw new ArgumentException( "Lambda must return a property." );

			//var vmExpression = body.Expression as ConstantExpression;

			//if( vmExpression != null )
			{
				var lambda = Expression.Lambda( body.Expression );
				var vmFunc = lambda.Compile();
				var sender = vmFunc.DynamicInvoke();

				if( handler != null )
				{
					handler( sender, new PropertyChangedEventArgs( body.Member.Name ) );
				}
			}
		}
		#endregion

	
	}
}
