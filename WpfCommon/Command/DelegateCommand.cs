using System;
using System.Diagnostics;

namespace WpfCommon.Command
{
    public class DelegateCommand<T> : ICommand<T>
    {
        private readonly Action<T> _execute;
        private readonly Func<T, bool> _canExecute;
        private static NLog.Logger s_logger_cache = null;
        private static NLog.Logger s_logger => s_logger_cache ??= NLog.LogManager.GetCurrentClassLogger();

        public event EventHandler CanExecuteChanged;

        public DelegateCommand(
            Action<T> execute,
            Func<T, bool> canExecute = null)
        {
            Debug.Assert(execute is not null);
            _execute = execute;
            _canExecute = canExecute ?? (_ => true);
        }

        public bool CanExecute(object parameter)
        {
            try
            {
                if (parameter is T tValue)
                {
                    return _canExecute(tValue);
                }
                else
                {
                    return _canExecute(default);
                }
            }
            catch (Exception e)
            {
                s_logger.Error(e);
                return default;
            }

        }

        public void Execute(object parameter)
        {
            try
            {
                if (parameter is T tValue)
                {
                    _execute(tValue);
                }
                else
                {
                    _execute(default);
                }
            }
            catch (Exception e)
            {
                s_logger.Error(e);
            }
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public class DelegateCommand : DelegateCommand<object>
    {
        public DelegateCommand(Action action, Func<bool> canExecute = null)
            : base(_ => action(), _ => canExecute?.Invoke() ?? true) { }

        public void Execute()
        {
            Execute(null);
        }

        public bool CanExecute()
        {
            return CanExecute(null);
        }
    }
}


