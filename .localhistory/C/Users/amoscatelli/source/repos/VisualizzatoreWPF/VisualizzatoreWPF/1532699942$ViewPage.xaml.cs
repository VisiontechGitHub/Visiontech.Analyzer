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
            
            string[] files = e.Data.GetData(DataFormats.FileDrop) as string[];


            switch (System.IO.Path.GetExtension(files[0]))
            {
                case ".txt":
                case ".xyz":

                    using (StreamReader streamReader = new StreamReader(files[0]))
                    {



                        Debug.WriteLine("0");
                    }

                    break;
                case ".hmf":
                    Debug.WriteLine("1");
                    break;
                case ".sdf":
                    Debug.WriteLine("2");
                    break;
            }

            if (RightStackPanel.Equals(sender))
            {
                Debug.WriteLine("DESTRO");
            } else if (LeftStackPanel.Equals(sender))
            {
                Debug.WriteLine("SINISTRO");
            }

        }
    }
}
