using System.Windows.Navigation;
using VisiontechCommons;
using VisualizzatoreWPF.ViewModels;

namespace VisualizzatoreWPF
{
    public partial class MainWindow : NavigationWindow
    {

        protected readonly MainModel model = Container.ServiceProvider.GetService(typeof(MainModel)) as MainModel;

        public MainWindow()
        {
            InitializeComponent();

            BaseViewModel.IsLoggedChanged += IsLoggedChanged;
        }

        private void IsLoggedChanged(object sender, bool e)
        {
            if (model.IsLogged)
            {

            }
        }

    }
}
