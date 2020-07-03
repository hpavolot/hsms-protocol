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

            this.Activated += (sender, args) =>
            {
                var viewModel = (VmConfiguration)DataContext;
                viewModel.Closing += (s, ea) => Close();
            };

        }
    }
}
