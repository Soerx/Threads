using Threads.Stores;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using Threads.MVVM.Models;
using System.Linq;
using System.Windows;
using System;

namespace Threads.MVVM.ViewModels
{
    internal class HomeVM : BindableBase
    {
        private readonly NavStore _navStore;
        private ThreadType _selectedThreadType;
        private Thread _selectedThread;
        private bool _isInternal;

        public ObservableCollection<ThreadType> ThreadTypes { get; }
        public ObservableCollection<Thread> Threads { get; }

        public bool IsDropDownThreadTypesListEnabled => _selectedThreadType != null;
        public bool IsDropDownThreadsListEnabled => _selectedThread != null;

        public bool IsInternal
        {
            get => _isInternal;
            set
            {
                _isInternal = value;
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Threads.Clear();
                    Threads.AddRange(InitializeThreads(thread => thread.IsInternal == IsInternal && thread.Type.Id == SelectedThreadType.Id));
                });
                SelectedThread = Threads.FirstOrDefault();
                RaiseCanExecuteChanged();
                RaisePropertyChanged(nameof(IsDropDownThreadTypesListEnabled));
                RaisePropertyChanged(nameof(IsDropDownThreadsListEnabled));
            }
        }

        public ThreadType SelectedThreadType
        {
            get => _selectedThreadType;
            set
            {
                _selectedThreadType = value;
                RaisePropertyChanged(nameof(SelectedThreadType));

                if (value == null)
                {
                    return;
                }

                if (Threads != null)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        Threads.Clear();
                        Threads.AddRange(InitializeThreads(thread => thread.IsInternal == IsInternal && thread.Type.Id == SelectedThreadType.Id));
                    });
                    SelectedThread = Threads.FirstOrDefault();
                    RaiseCanExecuteChanged();
                    RaisePropertyChanged(nameof(IsDropDownThreadTypesListEnabled));
                    RaisePropertyChanged(nameof(IsDropDownThreadsListEnabled));
                }
            }
        }

        public Thread SelectedThread
        {
            get => _selectedThread;
            set
            {
                _selectedThread = value;
                RaisePropertyChanged(nameof(SelectedThread));
            }
        }

        public RelayCommand CreateThreadTypeCommand { get; }
        public RelayCommand EditThreadTypeCommand { get; }
        public RelayCommand DeleteThreadTypeCommand { get; }
        public RelayCommand CreateThreadCommand { get; }
        public RelayCommand EditThreadCommand { get; }
        public RelayCommand DeleteThreadCommand { get; }

        public HomeVM(NavStore navStore)
        {
            _navStore = navStore;
            ThreadTypes = InitializeThreadTypes();
            SelectedThreadType = ThreadTypes.FirstOrDefault();
            Threads = InitializeThreads(thread => thread.IsInternal == IsInternal && thread.Type.Id == SelectedThreadType.Id);
            SelectedThread = Threads.FirstOrDefault();
            CreateThreadTypeCommand = new RelayCommand(CreateThreadType);
            EditThreadTypeCommand = new RelayCommand(EditThreadType, IsThreadTypeSelected);
            DeleteThreadTypeCommand = new RelayCommand(DeleteThreadType, IsThreadTypeSelected);
            CreateThreadCommand = new RelayCommand(CreateThread, IsThreadTypeSelected);
            EditThreadCommand = new RelayCommand(EditThread, IsThreadSelected);
            DeleteThreadCommand = new RelayCommand(DeleteThread, IsThreadSelected);
        }

        public ObservableCollection<Thread> InitializeThreads(Func<Thread, bool> predicate) => new ObservableCollection<Thread>(App.CurrentInstance.Context.Threads.Where(predicate));
        public ObservableCollection<ThreadType> InitializeThreadTypes() => new ObservableCollection<ThreadType>(App.CurrentInstance.Context.ThreadTypes);

        private void CreateThreadType(object parameter) => _navStore.CurrentVM = new ThreadTypeItemVM(_navStore);

        private void EditThreadType(object parameter) => _navStore.CurrentVM = new ThreadTypeItemVM(_selectedThreadType, _navStore);

        private void DeleteThreadType(object parameter)
        {
            if (_selectedThreadType is null)
            {
                return;
            }

            var result = MessageBox.Show($"Вы действительно хотите навсегда удалить тип резьб: {_selectedThreadType.Name}?\n" +
                $"Данное изменение нельзя будет отменить.", "Вы уверены?", MessageBoxButton.OKCancel, MessageBoxImage.Question);

            if (result is MessageBoxResult.OK)
            {
                App.CurrentInstance.Context.ThreadTypes.Remove(_selectedThreadType);
                App.CurrentInstance.Context.SaveChanges();
                Application.Current.Dispatcher.Invoke(() => ThreadTypes.Remove(_selectedThreadType));
                RaiseCanExecuteChanged();
                RaisePropertyChanged(nameof(IsDropDownThreadTypesListEnabled));
            }
        }

        private bool IsThreadTypeSelected(object parameter) => _selectedThreadType != null;

        private void CreateThread(object parameter) => _navStore.CurrentVM = new ThreadItemVM(_selectedThreadType, IsInternal, ThreadTypes, _navStore);

        private void EditThread(object parameter) => _navStore.CurrentVM = new ThreadItemVM(_selectedThread, ThreadTypes, _navStore);

        private void DeleteThread(object parameter)
        {
            if (_selectedThread == null)
            {
                return;
            }

            var result = MessageBox.Show($"Вы действительно хотите навсегда удалить резьбу: {_selectedThread.Name}?\n" +
                $"Данное изменение нельзя будет отменить.", "Вы уверены?", MessageBoxButton.OKCancel, MessageBoxImage.Question);

            if (result is MessageBoxResult.OK)
            {
                App.CurrentInstance.Context.Threads.Remove(_selectedThread);
                App.CurrentInstance.Context.SaveChanges();
                Application.Current.Dispatcher.Invoke(() => Threads.Remove(_selectedThread));
                RaiseCanExecuteChanged();
                RaisePropertyChanged(nameof(IsDropDownThreadsListEnabled));
            }
        }

        private bool IsThreadSelected(object parameter) => _selectedThread != null;

        private void RaiseCanExecuteChanged()
        {
            CreateThreadTypeCommand.RaiseCanExecuteChanged();
            EditThreadTypeCommand.RaiseCanExecuteChanged();
            DeleteThreadTypeCommand.RaiseCanExecuteChanged();
            CreateThreadCommand.RaiseCanExecuteChanged();
            EditThreadCommand.RaiseCanExecuteChanged();
            DeleteThreadCommand.RaiseCanExecuteChanged();
        }
    }
}
