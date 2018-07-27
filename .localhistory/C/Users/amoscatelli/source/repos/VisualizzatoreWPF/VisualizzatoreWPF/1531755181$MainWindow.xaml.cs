using Org.Visiontech.Commons;
using Org.Visiontech.Commons.Models;
using Org.Visiontech.Commons.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        protected readonly ICredentialService credentialService = Container.ServiceProvider.GetService(typeof(ICredentialService)) as ICredentialService;
        protected readonly IProvider<HttpClient> httpClientProvider = Container.ServiceProvider.GetService(typeof(IProvider<HttpClient>)) as IProvider<HttpClient>;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Load_File_Click(object sender, RoutedEventArgs e)
        {

            string Token = await tokenService.GetToken(Username.Text, Password.Text);



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
                        Debug.WriteLine("STOCAZZO");
                        break;
                    case ".hmf":
                        Debug.WriteLine("STOCAZZO2");
                        break;
                    case ".sdf":
                        Debug.WriteLine("STOCAZZO2");
                        break;
                }

            }

        }
    }
}
