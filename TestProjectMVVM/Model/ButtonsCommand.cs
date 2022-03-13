using System;
using System.Windows.Input;

namespace TestProjectMVVM
{
    public class ButtonsCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private Action _action;

        public ButtonsCommand(Action action)
        {
            _action = action;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _action();
        }
    }
}
