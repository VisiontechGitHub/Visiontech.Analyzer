using Org.Visiontech.Commons;
using Org.Visiontech.Credential;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using Visiontech.Services.Utils;
using VisiontechCommons;

namespace VisualizzatoreWPF.ViewModels
{
    public class LoginModel : BaseViewModel
    {

        protected readonly ITokenService tokenService = VisiontechCommons.Container.ServiceProvider.GetService(typeof(ITokenService)) as ITokenService;
        protected readonly CredentialSoapClient credentialSoapClient = VisiontechCommons.Container.ServiceProvider.GetService(typeof(CredentialSoapClient)) as CredentialSoapClient;
        protected readonly IAuthenticatingMessageInspector authenticatingMessageInspector = VisiontechCommons.Container.ServiceProvider.GetService(typeof(IAuthenticatingMessageInspector)) as IAuthenticatingMessageInspector;

        private bool canLogin = false;
        public bool CanLogin
        {
            get { return canLogin; }
            set { SetProperty(ref canLogin, value); }
        }

        private string username = string.Empty;
        public string Username
        {
            get { return username; }
            set
            {
                SetProperty(ref username, value);
                CheckCanLogin();
            }
        }

        private string password = string.Empty;

        public string Password
        {
            get { return password; }
            set
            {
                SetProperty(ref password, value);
                CheckCanLogin();
            }
        }

        private void CheckCanLogin()
        {
            CanLogin = !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password) && IsConnected;
        }

        public ICommand LoginCommand { get; }

        public LoginModel()
        {
            LoginCommand = new Command((parameter) => LoginAsync(), (parameter) => CanLogin);
        }

        private async void LoginAsync()
        {

            string token = await tokenService.GetToken(Username, Password);

            if (!string.IsNullOrWhiteSpace(token))
            {

                authenticatingMessageInspector.Bearer = token;

                credentialDTO credentialDTO = credentialSoapClient.my();

                if (!(credentialDTO is null) && !string.IsNullOrWhiteSpace(credentialDTO.id))
                {

                    Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog
                    {
                        DefaultExt = ".txt",
                        Filter = "TXT Files (*.txt)|*.txt|XYZ Files (*.xyz)|*.xyz|HMF Files (*.hmf)|*.hmf|SDF Files (*.sdf)|*.sdf"
                    };

                    bool? result = dlg.ShowDialog();

                    if (result == true)
                    {

                        switch (System.IO.Path.GetExtension(dlg.FileName))
                        {
                            case ".txt":
                            case ".xyz":
                                break;
                            case ".hmf":
                                break;
                            case ".sdf":
                                break;
                        }

                    }

                }

            }

        }

    }
}
