using hsms.wpf.ViewModels;
using System.Windows;

namespace hsms.wpf.Views
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
                var viewModel = (ConfigurationViewModel)DataContext;
                viewModel.Closing += (s, ea) => Close();
            };

        }


    }
}
