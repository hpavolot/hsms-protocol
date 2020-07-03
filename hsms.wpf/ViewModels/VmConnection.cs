#region Usings
using hsms.wpf.Models;
using Semi.Hsms;
using Semi.Hsms.connections;
using Semi.Hsms.Messages;
using System.Windows.Input;
#endregion

namespace hsms.wpf
{
	/// <summary>
	/// 
	/// </summary>
	internal class VmConnection : BaseViewModel
	{
		#region Class members
		/// <summary>
		/// 
		/// </summary>
		private Connection _connection;
		/// <summary>
		/// 
		/// </summary>
		private readonly Logger _logger;
		/// <summary>
		/// 
		/// </summary>
		private bool _canTryToConnect = true;
		/// <summary>
		/// 
		/// </summary>
		private readonly ConfigurationViewModel _configurationViewModel;

		#endregion

		#region Class properties
		/// <summary>
		/// 
		/// </summary>
		public Logger Logger => _logger;

		/// <summary>
		/// 
		/// </summary>
		public bool CanTryToConnect
		{
			get
			{
				return _canTryToConnect;
			}
			set
			{
				_canTryToConnect = value;
				FirePropertyChanged( () => CanTryToConnect );
				FirePropertyChanged( () => CannotConnect );
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool CannotConnect => !CanTryToConnect;

		#region Commands
		/// <summary>
		/// 
		/// </summary>
		public ICommand ConnectCommand { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		public ICommand DisconnectCommand { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		public ICommand SendCommand { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		public ICommand ClearLogCommand { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		public ICommand ConfigureCommand { get; private set; }


		#endregion

		#endregion

		#region Class initialization
		/// <summary>
		/// 
		/// </summary>
		public VmConnection()
		{
			_configurationViewModel = new ConfigurationViewModel();
			_logger = new Logger();

			InitializeCommands();
		}
		/// <summary>
		/// 
		/// </summary>
		private void InitializeCommands()
		{
			ConfigureCommand = new RelayCommand( Configure );
			ConnectCommand = new RelayCommand( Start );
			DisconnectCommand = new RelayCommand( Stop );
			SendCommand = new RelayCommand( Send );
			ClearLogCommand = new RelayCommand( ClearLog );
		}
		/// <summary>
		/// 
		/// </summary>
		private void SubscribeToEvents()
		{
			_connection.Events.Connecting += ( s, ea ) => _logger.LogEvent( EventType.Connecting, ea );
			_connection.Events.Connected += ( s, ea ) => _logger.LogEvent( EventType.Connected, ea );
			_connection.Events.Disconnected += ( s, ea ) => _logger.LogEvent( EventType.Disconnected, ea );
			_connection.Events.Listening += ( s, ea ) => _logger.LogEvent( EventType.Listening, ea );
			_connection.Events.Sent += ( s, ea ) => _logger.LogEvent( EventType.Sent, ea );
			_connection.Events.Received += ( s, ea ) => _logger.LogEvent( EventType.Received, ea );
			////_connection.Events.T3Timeout += (s, ea) => LogLine = $"Message has not been delivered: {ea} {ea.Context}";

		}
		#endregion

		#region Class methods
		/// <summary>
		/// 54
		/// </summary>
		private void Start()
		{
			_connection = new Connection( _configurationViewModel.Configurator );

			_connection.Start();
			CanTryToConnect = false;

			SubscribeToEvents();
		}
		/// <summary>
		/// 
		/// </summary>
		private void Stop()
		{
			_connection.Stop();
			CanTryToConnect = true;
		}
		/// <summary>
		/// 
		/// </summary>
		private void Send()
		{
			var m = DataMessage
					.Builder
					.NewContext()
					.Device( 6 )
					.Stream( 8 )
					.Function( 101 )
					.Build();

			_connection.Send( m );
		}
		/// <summary>
		/// 
		/// </summary>
		private void ClearLog()
		{
			_logger.Clear();
		}
		/// <summary>
		/// 
		/// </summary>
		private void Configure()
		{
			ConfigurationView view = new ConfigurationView();
			view.DataContext = _configurationViewModel;
			view.ShowDialog();
		}
		#endregion
	}
}
