using System.Windows.Input;

namespace MediaPlayerPL
{
    public class Command : ICommand
    {
        private Action methodToExecute = null;
        private Func<bool> canMethodBeExecuted = null;

        public event EventHandler CanExecuteChanged;

        public Command(Action methodToExecute, Func<bool> canMethodBeExecuted)
        {
            this.methodToExecute = methodToExecute;
            this.canMethodBeExecuted = canMethodBeExecuted;
        }

        public void Execute(object parameter)
        {
            methodToExecute?.Invoke();
        }

        public bool CanExecute(object parameter)
        {
            if (canMethodBeExecuted == null)
            {
                return true;
            }
            else
            {
                return canMethodBeExecuted();
            }
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public class CommandWithParameter<T> : ICommand
    {
        private Action<T> methodToExecuteWithParam = null;
        private Func<T, bool> canMethodBeExecuted = null;
        private Action methodToExecute = null;

        public event EventHandler CanExecuteChanged;

        public CommandWithParameter(Action<T> methodToExecute, Func<T, bool> canMethodBeExecuted)
        {
            methodToExecuteWithParam = methodToExecute;
            this.canMethodBeExecuted = canMethodBeExecuted;
        }

        public void Execute(object parameter)
        {
            if (parameter != null && parameter is T typedParameter)
            {
                methodToExecuteWithParam?.Invoke(typedParameter);
            }
            else
            {
                methodToExecute?.Invoke();
            }
        }

        public bool CanExecute(object parameter)
        {
            if (parameter != null && parameter is T typedParameter)
            {
                return canMethodBeExecuted(typedParameter);
            }
            else
            {
                return false;
            }
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }

}
