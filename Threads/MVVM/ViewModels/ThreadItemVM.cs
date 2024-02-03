using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Threads.MVVM.Models;
using Threads.Stores;

namespace Threads.MVVM.ViewModels
{
    internal class ThreadItemVM : BindableBase
    {
        private NavStore _navStore;

        public Thread Thread { get; }

        public RelayCommand SaveCommand { get; }
        public RelayCommand CancelCommand { get; }

        public ObservableCollection<ThreadType> ThreadTypes { get; }

        public ThreadItemVM(ThreadType threadType, bool isInternal, ObservableCollection<ThreadType> threadTypes, NavStore navStore) : this(new Thread() { Id = Guid.NewGuid(), IsInternal = isInternal, Type = threadType }, threadTypes, navStore) => _navStore = navStore;

        public ThreadItemVM(Thread thread, ObservableCollection<ThreadType> threadTypes, NavStore navStore)
        {
            Thread = thread;
            _navStore = navStore;
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
            ThreadTypes = threadTypes;
        }

        private void Save(object parameter)
        {
            var existingThread = App.CurrentInstance.Context.Threads.FirstOrDefault(t => t.Id == Thread.Id);

            if (existingThread is null)
            {
                App.CurrentInstance.Context.Threads.Add(Thread);
            }
            else
            {
                App.CurrentInstance.Context.Threads.Update(Thread);
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
