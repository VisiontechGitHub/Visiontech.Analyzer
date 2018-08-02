using System.Diagnostics;
using System.Windows.Navigation;
using Visiontech.Analyzer.View;
using VisiontechCommons;
using Visiontech.Analyzer.ViewModels;

namespace Visiontech.Analyzer.View
{
    public partial class MainWindow : NavigationWindow
    {

        protected readonly MainModel model = Container.ServiceProvider.GetService(typeof(MainModel)) as MainModel;

        public MainWindow()
        {
            InitializeComponent();

            NavigationService.Navigate(new LoginPage());
        }

    }
}
