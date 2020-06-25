using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace hsms.wpf.ViewModel.Base
{
    public class RelayCommand : ICommand
    {
        #region  Private Members
        /// <summary>
        /// 
        /// </summary>
        private readonly Action _action;

        #endregion

        #region Public Events
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler CanExecuteChanged = (sender, e) => { };

        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        public RelayCommand(Action action)
        {
            _action = action;
        }

        #endregion

        #region CommandMethods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter) => true;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            _action();
        }
        #endregion

    }
}
