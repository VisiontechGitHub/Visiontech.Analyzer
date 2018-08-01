using System.Windows;
using System.Windows.Controls;
using Visiontech.Analyzer.View.Abstraction;
using VisiontechCommons;
using Visiontech.Analyzer.ViewModels;

namespace Visiontech.Analyzer.View
{
    
    public partial class LoginPage : LoadingPage<LoginModel>
    {

        public LoginPage()
        {
            InitializeComponent();
        }

        private void Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            model.Password = (e.OriginalSource as PasswordBox).SecurePassword;
        }
    }
}
