using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Visiontech.Analyzer.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {

        private bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                if (isBusy != value)
                {
                    SetProperty(ref isBusy, value);
                }
            }
        }

        private static bool isLogged = false;
        public bool IsLogged
        {
            get { return isLogged; }
            set
            {
                if (isLogged != value)
                {
                    SetProperty(ref isLogged, value);
                }
            }
        }

        private static bool isConnected = false;
        public bool IsConnected
        {
            get { return isConnected; }
            set
            {
                if (isConnected != value)
                {
                    SetProperty(ref isConnected, value);
                }
            }
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName]string propertyName = "",
            Action onChanged = null)
        {
            
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public BaseViewModel()
        {
            IsConnected = true;
        }

    }
}
