#region Usings
using System.ComponentModel;
#endregion

namespace hsms.wpf.ViewModel.Base
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => {};
    }
}
