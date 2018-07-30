using System.Diagnostics;
using System.Windows.Navigation;
using Visiontech.Analyzer.View;
using VisiontechCommons;
using VisualizzatoreWPF.ViewModels;

namespace Visiontech.Analyzer.View
{
    public partial class MainWindow : NavigationWindow
    {

        protected readonly MainModel model = Container.ServiceProvider.GetService(typeof(MainModel)) as MainModel;

        public MainWindow()
        {
            InitializeComponent();

            BaseViewModel.IsLoggedChanged += IsLoggedChanged;

            NavigationService.Navigate(new LoginPage());
        }

        private void IsLoggedChanged(object sender, bool e)
        {
            if (model.IsLogged)
            {
                NavigationService.Navigate(new ViewPage());
            }
        }
    }
}
