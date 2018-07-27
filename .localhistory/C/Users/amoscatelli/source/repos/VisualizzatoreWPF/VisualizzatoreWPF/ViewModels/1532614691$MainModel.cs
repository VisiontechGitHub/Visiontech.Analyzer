﻿using Org.Visiontech.Commons;
using Org.Visiontech.Credential;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visiontech.Services.Utils;
using VisiontechCommons;

namespace VisualizzatoreWPF.ViewModels
{
    public class MainModel
    {

        protected readonly ITokenService tokenService = Container.ServiceProvider.GetService(typeof(ITokenService)) as ITokenService;
        protected readonly CredentialSoapClient credentialSoapClient = Container.ServiceProvider.GetService(typeof(CredentialSoapClient)) as CredentialSoapClient;
        protected readonly IAuthenticatingMessageInspector authenticatingMessageInspector = Container.ServiceProvider.GetService(typeof(IAuthenticatingMessageInspector)) as IAuthenticatingMessageInspector;


    }
}
