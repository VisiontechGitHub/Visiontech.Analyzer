using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using VisiontechCommons;
using VisualizzatoreWPF.ViewModels;

namespace VisualizzatoreWPF
{
    public partial class ViewPage : Page
    {

        protected readonly ViewModel model = Container.ServiceProvider.GetService(typeof(ViewModel)) as ViewModel;

        public ViewPage()
        {
            InitializeComponent();
        }

        private void StackPanel_Drop(object sender, DragEventArgs e)
        {

            ViewModel.Side side = RightStackPanel.Equals(sender) ? ViewModel.Side.RIGHT : ViewModel.Side.LEFT;
            model.LoadFileCommand.Execute(new Tuple<ViewModel.Side, string[]>(side, e.Data.GetData(DataFormats.FileDrop) as string[]));

        }
    }
}
