using System.Windows;

namespace hsms.wpf
{
	/// <summary>
	/// Interaction logic for ConfigureConnectionView.xaml
	/// </summary>
	public partial class ConfigurationView : Window
	{
		public ConfigurationView()
		{
			InitializeComponent();
			this.Activated += ( sender, args ) =>
			{
				var viewModel = ( ConfigurationViewModel )DataContext;
				viewModel.Closing += ( s, ea ) => Close();
			};

		}

		private void RadioButton_Checked( object sender, RoutedEventArgs e )
		{

		}

		private void Button_Click( object sender, RoutedEventArgs e ) => Close();
	}
}
