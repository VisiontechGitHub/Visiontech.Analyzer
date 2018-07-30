using System.Windows;
using System.Windows.Controls;
using Visiontech.Analyzer.Views.Abstraction;
using VisiontechCommons;
using VisualizzatoreWPF.ViewModels;

namespace Visiontech.Analyzer.Views
{
    
    public partial class LoginPage : LoadingPage
    {

        protected readonly LoginModel model = Container.ServiceProvider.GetService(typeof(LoginModel)) as LoginModel;

        public LoginPage()
        {
            InitializeComponent();
            DataContext = model;
        }

        private void Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            model.Password = (e.OriginalSource as PasswordBox).SecurePassword;
        }
    }
}
