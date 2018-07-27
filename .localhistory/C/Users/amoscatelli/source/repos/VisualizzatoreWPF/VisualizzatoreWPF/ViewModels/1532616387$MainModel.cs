using Org.Visiontech.Commons;
using Org.Visiontech.Credential;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Visiontech.Services.Utils;
using VisiontechCommons;

namespace VisualizzatoreWPF.ViewModels
{
    public class MainModel
    {

        protected readonly ITokenService tokenService = Container.ServiceProvider.GetService(typeof(ITokenService)) as ITokenService;
        protected readonly CredentialSoapClient credentialSoapClient = Container.ServiceProvider.GetService(typeof(CredentialSoapClient)) as CredentialSoapClient;
        protected readonly IAuthenticatingMessageInspector authenticatingMessageInspector = Container.ServiceProvider.GetService(typeof(IAuthenticatingMessageInspector)) as IAuthenticatingMessageInspector;

        private bool canLogin = false;
        public bool CanLogin
        {
            get { return canLogin; }
            set { SetProperty(ref canLogin, value); }
        }

        public ICommand LoginCommand { get; }

        public MainModel()
        {
            LoginCommand = new Command(Login, (parameter) => CanLogin);
        }

        private void Login()
        {

        }

    }
}
