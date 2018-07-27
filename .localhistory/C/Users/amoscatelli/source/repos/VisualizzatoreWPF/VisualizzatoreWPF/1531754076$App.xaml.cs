using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace VisualizzatoreWPF
{
    /// <summary>
    /// Logica di interazione per App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Container.Services.AddSingleton<IProvider<JsonSerializer>, JsonDeserializerProvider>();
            Container.Services.AddSingleton<IProvider<HttpClientHandler>, HttpClientHandlerProvider>();
            Container.Services.AddSingleton<IProvider<HttpClient>, HttpClientProvider>();

            Container.Services.AddSingleton<ITokenService, TokenService>();
            Container.Services.AddSingleton<ICredentialService, CredentialService>();
            Container.Services.AddSingleton<IDeletableService<ProductDTO>, ProductService>();
            Container.Services.AddSingleton<IDeletableService<PersonDTO>, PersonService>();
            Container.Services.AddSingleton<IComputeLensService, ComputeLensService>();

            Container.Services.AddSingleton<MainModel>();
            Container.Services.AddSingleton<MenuModel>();
            Container.Services.AddSingleton<LoginModel>();
            Container.Services.AddSingleton<LensPreviewModel>();

            Container.Services.AddSingleton<ITranslateService, TranslateService>();

            Container.Services.AddSingleton<EntityTemplate<IdentifiableDTO>, IdentifiableTemplate>();
        }

    }
}
