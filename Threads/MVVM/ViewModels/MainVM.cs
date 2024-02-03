using Threads.Stores;
using Prism.Mvvm;

namespace Threads.MVVM.ViewModels
{
    internal class MainVM : BindableBase
    {
        private readonly NavStore _navStore;

        public BindableBase CurrentVM => _navStore.CurrentVM;

        public MainVM()
        {
            _navStore = new NavStore();
            _navStore.OnViewModelUpdated += NotifyViewModelUpdated;
            _navStore.CurrentVM = new HomeVM(_navStore);
        }

        private void NotifyViewModelUpdated() => RaisePropertyChanged(nameof(CurrentVM));
    }
}
