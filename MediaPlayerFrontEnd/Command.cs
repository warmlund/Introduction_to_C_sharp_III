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
}
