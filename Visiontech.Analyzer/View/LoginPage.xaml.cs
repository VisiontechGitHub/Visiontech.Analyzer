using System.Windows;
using System.Windows.Controls;
using Visiontech.Analyzer.View.Abstraction;
using VisiontechCommons;
using Visiontech.Analyzer.ViewModels;
using System.Diagnostics;

namespace Visiontech.Analyzer.View
{
    
    public partial class LoginPage : LoadingPage<LoginModel>
    {

        public LoginPage()
        {
            InitializeComponent();

            model.PropertyChanged += Model_PropertyChanged;

        }

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

            if (model.IsLogged)
            {
                NavigationService.Navigate(new ViewPage());
            }

        }

        private void Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            model.Password = (e.OriginalSource as PasswordBox).SecurePassword;
        }
    }
}
