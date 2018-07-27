using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace VisualizzatoreWPF.ViewModels
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
                    if (IsBusyChanged != null)
                    {
                        SetProperty(ref isBusy, value);
                        IsBusyChanged.Invoke(this, value);
                    }
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
                    if (IsLoggedChanged != null)
                    {
                        IsLoggedChanged.Invoke(this, value);
                    }
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
                    if (IsConnectedChanged != null)
                    {
                        IsConnectedChanged.Invoke(this, value);
                    }
                }
            }
        }

        public event EventHandler<bool> IsBusyChanged;
        public static event EventHandler<bool> IsLoggedChanged;
        public static event EventHandler<bool> IsConnectedChanged;

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
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public BaseViewModel()
        {
            IsConnected = true;
        }

    }
}
