using Org.Visiontech.Commons;
using Org.Visiontech.Credential;
using System.ComponentModel;
using System.Windows.Input;
using Visiontech.Services.Utils;
using VisiontechCommons;

namespace VisualizzatoreWPF.ViewModels
{
    public class MainModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        protected readonly ITokenService tokenService = Container.ServiceProvider.GetService(typeof(ITokenService)) as ITokenService;
        protected readonly CredentialSoapClient credentialSoapClient = Container.ServiceProvider.GetService(typeof(CredentialSoapClient)) as CredentialSoapClient;
        protected readonly IAuthenticatingMessageInspector authenticatingMessageInspector = Container.ServiceProvider.GetService(typeof(IAuthenticatingMessageInspector)) as IAuthenticatingMessageInspector;

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
                CheckIsLoginEnabled();
            }
        }

        private string password = string.Empty;

        public string Password
        {
            get { return password; }
            set
            {
                SetProperty(ref password, value);
                CheckIsLoginEnabled();
            }
        }

        private void CheckIsLoginEnabled()
        {
            CanLogin = !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);
        }

        public ICommand LoginCommand { get; }

        public MainModel()
        {
            LoginCommand = new Command((parameter) => Login(), (parameter) => CanLogin);
        }

        private void Login()
        {

            string token = tokenService.GetToken(Username.Text, Password.Text);

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
