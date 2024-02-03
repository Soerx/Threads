using Prism.Mvvm;
using System;
using System.Linq;
using System.Windows;
using Threads.MVVM.Models;
using Threads.Stores;

namespace Threads.MVVM.ViewModels
{
    internal class ThreadTypeItemVM : BindableBase
    {
        private readonly NavStore _navStore;

        public ThreadType ThreadType { get; }

        public RelayCommand SaveCommand { get; }
        public RelayCommand CancelCommand { get; }

        public ThreadTypeItemVM(NavStore navStore) : this(new ThreadType() { Id = Guid.NewGuid() }, navStore) { }

        public ThreadTypeItemVM(ThreadType threadType, NavStore navStore)
        {
            ThreadType = threadType;
            _navStore = navStore;
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void Save(object parameter)
        {
            var existingType = App.CurrentInstance.Context.ThreadTypes.FirstOrDefault(t => t.Id == ThreadType.Id);

            if (existingType is null)
            {
                App.CurrentInstance.Context.ThreadTypes.Add(ThreadType);
            }
            else
            {
                App.CurrentInstance.Context.ThreadTypes.Update(ThreadType);
            }

            App.CurrentInstance.Context.SaveChanges();
            _navStore.CurrentVM = new HomeVM(_navStore);
        }

        private void Cancel(object parameter)
        {
            var result = MessageBox.Show("Несохраненные изменения будут навсегда утеряны.", "Вы уверены?", MessageBoxButton.OKCancel, MessageBoxImage.Question);

            if (result == MessageBoxResult.OK)
            {
                _navStore.CurrentVM = new HomeVM(_navStore);
            }
        }
    }
}
