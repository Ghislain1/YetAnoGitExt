using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfCommon.Command
{
    public interface ICommandCanExecuteChangedRaiser
    {
        public void RaiseCanExecuteChanged();
    }

    public interface ICommand<T> : ICommand, ICommandCanExecuteChangedRaiser
    {
    }

    public interface IAsyncCommand : ICommand, ICommandCanExecuteChangedRaiser
    {
        public Task ExecuteAsync(object parameter);

        public bool IsExecuting { get; }
    }

    public interface IAsyncCommandWithoutArgs : IAsyncCommand
    {
        public Task ExecuteAsync();
        public bool CanExecutable { get; }
    }
}
