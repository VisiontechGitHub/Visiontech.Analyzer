using Org.Visiontech.Commons;
using Org.Visiontech.Commons.Models;
using Org.Visiontech.Commons.Services;
using Org.Visiontech.Credential;
using Org.Visiontech.Optoplus.Dto.Entity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VisiontechCommons;

namespace VisualizzatoreWPF
{
    public partial class MainWindow : Window
    {

        protected readonly ITokenService tokenService = Container.ServiceProvider.GetService(typeof(ITokenService)) as ITokenService;
        protected readonly CredentialSoapClient credentialService = Container.ServiceProvider.GetService(typeof(CredentialSoapClient)) as CredentialSoapClient;
        protected readonly IAuthenticatingMessageInspector authenticatingMessageInspector = Container.ServiceProvider.GetService(typeof(IAuthenticatingMessageInspector)) as IAuthenticatingMessageInspector;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Load_File_Click(object sender, RoutedEventArgs e)
        {

            string token = await tokenService.GetToken(Username.Text, Password.Text);

            if (!string.IsNullOrWhiteSpace(token))
            {

                httpClientProvider.Provided.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                CredentialDTO credentialDTO = await credentialService.My();

                if (!(credentialDTO is null) && !string.IsNullOrWhiteSpace(credentialDTO.Id))
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
