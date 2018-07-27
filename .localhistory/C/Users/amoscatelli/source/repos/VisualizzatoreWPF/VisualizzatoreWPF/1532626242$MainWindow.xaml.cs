using Org.Visiontech.Commons;
using Org.Visiontech.Commons.Models;
using Org.Visiontech.Commons.Services;
using Org.Visiontech.Credential;
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
using Visiontech.Services.Utils;
using VisiontechCommons;
using VisualizzatoreWPF.ViewModels;

namespace VisualizzatoreWPF
{
    public partial class MainWindow : Window
    {

        protected readonly LoginModel model = Container.ServiceProvider.GetService(typeof(LoginModel)) as LoginModel;

        public MainWindow()
        {
            InitializeComponent();
        }

    }
}
