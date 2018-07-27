using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VisualizzatoreWPF
{
    public partial class ViewPage : Page
    {
        public ViewPage()
        {
            InitializeComponent();
        }

        private void StackPanel_Drop(object sender, DragEventArgs e)
        {


            switch (System.IO.Path.GetExtension(dlg.FileName))
            {
                case ".txt":
                case ".xyz":
                    break;
                case ".hmf":
                    break;
                case ".sdf":
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
