using System.Diagnostics;
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
                NavigationService.Navigate(new ViewPage());
            }
        }

        private void NavigationWindow_Drop(object sender, System.Windows.DragEventArgs e)
        {
            Debug.WriteLine("AHOOOO");
        }
    }
}
