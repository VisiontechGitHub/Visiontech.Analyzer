using Org.Visiontech.Commons;
using Org.Visiontech.Credential;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Security;
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

        private SecureString password = new SecureString();

        public SecureString Password
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
            CanLogin = !string.IsNullOrWhiteSpace(Username) && Password.Length>0 && IsConnected;
        }

        public ICommand LoginCommand { get; }

        public LoginModel()
        {
            LoginCommand = new Command((parameter) => LoginAsync(), (parameter) => CanLogin);
        }

        private async void LoginAsync()
        {

            IsBusy = true;

            if (IsConnected && CanLogin)
            {

                string token = await tokenService.GetToken(Username, new System.Net.NetworkCredential(string.Empty, Password).Password);

                if (!string.IsNullOrWhiteSpace(token))
                {

                    authenticatingMessageInspector.Bearer = token;

                    credentialDTO credentialDTO = credentialSoapClient.my();

                    if (!(credentialDTO is null) && !string.IsNullOrWhiteSpace(credentialDTO.id))
                    {

                        IsLogged = true;

                    }

                }

            }


            IsBusy = false;

        }

    }
}
