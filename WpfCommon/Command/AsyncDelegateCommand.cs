using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace WpfCommon.Command
{
    public class AsyncDelegateCommand<T> : NotificationBase, ICommand<T>, IAsyncCommand
    {
        private readonly Func<T, Task> _action;
        private readonly Func<T, bool> _canExecute;

        private int _invokeCount = 0;
        private readonly object _lock = new();
        private static NLog.Logger s_logger_cache = null;
        private static NLog.Logger s_logger => s_logger_cache ??= NLog.LogManager.GetCurrentClassLogger();

        public bool IsExecuting
        {
            get
            {
                lock (_lock)
                {
                    return _invokeCount is not 0;
                }
            }
        }

        public AsyncDelegateCommand(Func<T, Task> action, Func<T, bool> canExecute = null)
        {
            _action = action;
            _canExecute = canExecute ?? (_ => true);
        }

        public bool CanExecute(object parameter)
        {
            ValidateCallingOnUIThread();
            if (IsExecuting)
            {
                return false;
            }

            if (parameter is T tValue)
            {
                return _canExecute(tValue);
            }
            else
            {
                return _canExecute(default);
            }
        }

        [Obsolete("This method can call from WPF systems only, you should use instead ExecuteAsync this method.")]
        public void Execute(object parameter)
        {
            _ = ExecuteAsync(parameter);
        }

        public async Task ExecuteAsync(object parameter)
        {
            ValidateCallingOnUIThread();
            lock (_lock)
            {
                Debug.Assert(_invokeCount is 0, "Detecting recursive calls");

                if (_invokeCount is not 0)
                {
                    return;
                }

                _invokeCount++;
            }
            RaiseCanExecuteChanged();

            try
            {
                if (parameter is T tValue)
                {
                    await _action(tValue);
                }
                else
                {
                    await _action(default);
                }
            }
            catch (Exception e)
            {
                s_logger.Error(e);
            }
            finally
            {
                lock (_lock)
                {
                    _invokeCount--;
                }

                RaiseCanExecuteChanged();
            }
        }

        public void RaiseCanExecuteChanged()
        {
            ValidateCallingOnUIThread();
            OnPropertyChanged(nameof(IsExecuting));
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler CanExecuteChanged;

        private void ValidateCallingOnUIThread()
        {
            Debug.Assert(Thread.CurrentThread.GetApartmentState() == ApartmentState.STA, "Should call on the UI thread");
        }
    }

    public class AsyncDelegateCommand : AsyncDelegateCommand<object>, IAsyncCommandWithoutArgs
    {
        public AsyncDelegateCommand(Func<Task> action, Func<bool> canExecute = null)
            : base(
                async (_) => await action(),
                (_) => canExecute?.Invoke() ?? true)
        {
            CanExecuteChanged += (_, _) =>
            {
                OnPropertyChanged(nameof(CanExecutable));
            };
        }

        public Task ExecuteAsync() => ExecuteAsync(default);

        public bool CanExecutable => CanExecute(default);
    }
}


