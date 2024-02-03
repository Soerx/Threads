using Prism.Mvvm;
using System;

namespace Threads.Stores
{
    internal class NavStore
    {
        private BindableBase _currentVM;

        public event Action OnViewModelUpdated;

        public BindableBase CurrentVM
        {
            get => _currentVM;
            set
            {
                _currentVM = value ?? throw new ArgumentNullException(nameof(CurrentVM), "Current ViewModel can not be null.");
                OnViewModelUpdated?.Invoke();
            }
        }
    }
}
