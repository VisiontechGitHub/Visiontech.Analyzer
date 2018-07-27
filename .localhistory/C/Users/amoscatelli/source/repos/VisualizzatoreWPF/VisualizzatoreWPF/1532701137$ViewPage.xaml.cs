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

            if (RightStackPanel.Equals(sender))
            {
                model.LoadFileCommand.Execute(new Tuple<ViewModel.Side, string[]>(ViewModel.Side.RIGHT, e.Data.GetData(DataFormats.FileDrop) as string[]));
            }
            else if (LeftStackPanel.Equals(sender))
            {
                model.LoadFileCommand.Execute(new Tuple<ViewModel.Side, string[]>(ViewModel.Side.LEFT, e.Data.GetData(DataFormats.FileDrop) as string[]));
            }

        }
    }
}
