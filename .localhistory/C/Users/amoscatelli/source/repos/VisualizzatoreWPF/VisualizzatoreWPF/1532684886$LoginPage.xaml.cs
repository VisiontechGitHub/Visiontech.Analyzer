
using System;
using System.Windows;
using System.Windows.Controls;
using VisiontechCommons;
using VisualizzatoreWPF.ViewModels;

namespace VisualizzatoreWPF
{
    
    public partial class LoginPage : Page
    {

        protected readonly LoginModel model = Container.ServiceProvider.GetService(typeof(LoginModel)) as LoginModel;

        public LoginPage()
        {
            InitializeComponent();
            DataContext = model;

            BaseViewModel.IsLoggedChanged += IsLoggedChanged;
        }

        private void IsLoggedChanged(object sender, bool e)
        {
            if (model.IsLogged)
            {
                
            }
        }

        private void Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            model.Password = (e.OriginalSource as PasswordBox).SecurePassword;
        }
    }
}
