using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Org.Visiontech.Commons;
using Org.Visiontech.Commons.Models;
using Org.Visiontech.Commons.Services;
using Org.Visiontech.Compute;
using Org.Visiontech.Credential;
using Org.Visiontech.Group;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;
using Visiontech.Services.Utils;
using VisiontechCommons;
using Visiontech.Analyzer.ViewModels;

namespace VisualizzatoreWPF
{

    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Container.Services.AddSingleton<IProvider<HttpClientHandler>, HttpClientHandlerProvider>();
            Container.Services.AddSingleton<IProvider<HttpClient>, HttpClientProvider>();

            Container.Services.AddSingleton<IAuthenticatingMessageInspector, AuthenticatingMessageInspector>();

            Container.Services.AddSingleton<ITokenService, TokenService>(serviceProvider => new TokenService("https://cas.dev.optoplus.cloud:8543/cas/v1/tickets", "services.dev.optoplus.cloud/optoplus-services-web"));

            Container.Services.AddSingleton(serviceProvider => ClientBaseUtils.InitClientBase<CredentialSoap, CredentialSoapClient>(serviceProvider, new EndpointAddress("https://services.dev.optoplus.cloud:8443/optoplus-services-web/CredentialSoap")));
            Container.Services.AddSingleton(serviceProvider => ClientBaseUtils.InitClientBase<GroupSoap, GroupSoapClient>(serviceProvider, new EndpointAddress("https://services.dev.optoplus.cloud:8443/optoplus-services-web/GroupSoap")));
            Container.Services.AddSingleton(serviceProvider => ClientBaseUtils.InitClientBase<ComputeSoap, ComputeSoapClient>(serviceProvider, new EndpointAddress("https://services.dev.optoplus.cloud:8443/optoplus-services-web/ComputeSoap")));

            Container.Services.AddSingleton<MainModel>();
            Container.Services.AddSingleton<LoginModel>();
            Container.Services.AddSingleton<ViewModel>();
        }

    }
}
