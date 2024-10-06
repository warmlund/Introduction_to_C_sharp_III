using System.Windows.Input;

namespace MediaPlayerPL
{
    /// <summary>
    /// These are two classes for handling commands between
    /// the view and the view model. Command handles passes methods without any arguments.
    /// CommandWithParameter handles methods with arguments
    /// </summary>

    public class Command : ICommand
    {
        private Action methodToExecute = null; //variable representing the command that is being executed
        private Func<bool> canMethodBeExecuted = null; //variable that represents the boolean which checks if you can execute the command or not
        public event EventHandler CanExecuteChanged;  // An event that is triggered if the state of _canExecute changes

        /// <summary>
        /// Constructor and sets the variable with the arguments
        /// </summary>
        public Command(Action methodToExecute, Func<bool> canMethodBeExecuted)
        {
            this.methodToExecute = methodToExecute;
            this.canMethodBeExecuted = canMethodBeExecuted;
        }

        /// <summary>
        /// Executes the command
        /// </summary>
        public void Execute(object parameter)
        {
            methodToExecute?.Invoke();
        }

        /// <summary>
        /// Method that determines if the command can be executed based on _canExecute
        /// </summary>
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


        /// <summary>
        /// Method that is called to raise the CanExecuteChanged
        /// </summary>
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
