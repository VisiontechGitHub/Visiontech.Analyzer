using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace VisualizzatoreWPF.ViewModels
{
    public class ActionCommand : ICommand
    {

        public delegate void ICommandOnExecute(object parameter);
        public delegate bool ICommandOnCanExecute(object parameter);

        public event EventHandler CanExecuteChanged;

        private ICommandOnExecute _execute;
        private ICommandOnCanExecute _canExecute;

        public ActionCommand(ICommandOnExecute onExecuteMethod, ICommandOnCanExecute onCanExecuteMethod))
        {
            this.action = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            action.DynamicInvoke(parameter);
        }
    }
}
