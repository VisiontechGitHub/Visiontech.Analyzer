﻿using Org.Visiontech.Commons;
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

        protected readonly MainModel model = Container.ServiceProvider.GetService(typeof(MainModel)) as MainModel;

  
        public MainWindow()
        {
            InitializeComponent();
            DataContext = model;
        }

        private async void Load_File_Click(object sender, RoutedEventArgs e)
        {

            string token = await tokenService.GetToken(Username.Text, Password.Text);

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
