using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace VisualizzatoreWPF.ViewModels
{
    public class ViewModel : BaseViewModel
    {

        public enum Side
        {
            LEFT, RIGHT
        }

        public ICommand LoadFileCommand { get; }

        public ViewModel()
        {
            LoadFileCommand = new Command((parameter) => LoadFileAsync(parameter as Tuple<Side, string[]>), (parameter) => !(parameter is null) && parameter is Tuple<Side, string[]>  && (parameter as Tuple<Side, string[]>).Item2.Length > 0);
        }

        private void LoadFileAsync(Tuple<Side, string[]> tuple)
        {

            tuple.Item2[0];

            switch (Path.GetExtension(files[0]))
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

            Debug.WriteLine("CICCIO");

        }
    }
}
