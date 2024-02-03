using Prism.Commands;
using System;

namespace Threads
{
    internal class RelayCommand : DelegateCommandBase
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        protected override bool CanExecute(object parameter) => _canExecute is null || _canExecute(parameter);
        protected override void Execute(object parameter) => _execute(parameter);
    }
}
