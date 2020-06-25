using System;
using System.Windows.Input;

namespace hsms.wpf.ViewModels.Base
{
    /// <summary>
    /// 
    /// </summary>
    public class RelayCommand : ICommand
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly Action _action;
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler CanExecuteChanged = (sender, e) => { };
        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewModel"></param>
        public RelayCommand(Action action)
        {
            _action = action;
        }
        public bool CanExecute(object parameter) => true;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            _action();
        }
    }
}
