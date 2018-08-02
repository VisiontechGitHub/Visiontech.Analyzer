using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Visiontech.Analyzer.ViewModels;
using VisiontechCommons;
using Xceed.Wpf.Toolkit;

namespace Visiontech.Analyzer.View.Abstraction
{
    public abstract class LoadingPage<T> : Page where T : BaseViewModel
    {

        public readonly T model = Container.ServiceProvider.GetService(typeof(T)) as T;

        private readonly Grid absoluteLayout;
        private readonly Grid overlay;
        private readonly Grid contentView;
        private readonly BusyIndicator busyIndicator;

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {

            base.OnPropertyChanged(e);

            if (ContentProperty.Equals(e.Property))
            {
                if (!absoluteLayout.Equals(e.NewValue) && e.NewValue is UIElement)
                {

                    Content = null;

                    contentView.Children.Clear();
                    contentView.Children.Add(e.NewValue as UIElement);

                    Content = absoluteLayout;

                }
            }

        }

        public LoadingPage()
        {   

            DataContext = model;

            model.PropertyChanged += Model_PropertyChanged;

            absoluteLayout = new Grid();

            overlay = new Grid
            {
                Background = Brushes.Black,
                Opacity = 0.5,
                Visibility = Visibility.Hidden
            };

            busyIndicator = new BusyIndicator(){
                IsBusy = true,
                Visibility = Visibility.Hidden
            };

            Panel.SetZIndex(overlay, 1000);
            Panel.SetZIndex(busyIndicator, 1500);

            contentView = new Grid();

            absoluteLayout.Children.Add(busyIndicator);
            absoluteLayout.Children.Add(overlay);
            absoluteLayout.Children.Add(contentView);

        }

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if ("IsBusy".Equals(e.PropertyName))
            {
                Dispatcher.Invoke(() =>
                    {
                        overlay.Visibility = model.IsBusy ? Visibility.Visible : Visibility.Hidden;
                        busyIndicator.Visibility = model.IsBusy ? Visibility.Visible : Visibility.Hidden;
                    }
                );
            }
        }

    }
}
