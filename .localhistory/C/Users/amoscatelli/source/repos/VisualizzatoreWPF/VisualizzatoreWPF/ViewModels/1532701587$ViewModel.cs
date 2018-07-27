using Org.Visiontech.Compute;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace VisualizzatoreWPF.ViewModels
{
    public class ViewModel : BaseViewModel
    {

        protected readonly ComputeSoapClient credentialSoapClient = VisiontechCommons.Container.ServiceProvider.GetService(typeof(ComputeSoapClient)) as ComputeSoapClient;

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

            analyzeLensRequestDTO analyzeLensRequestDTO = new analyzeLensRequestDTO();

            switch (Path.GetExtension(tuple.Item2[0]))
            {
                case ".txt":
                case ".xyz":

                    using (StreamReader streamReader = new StreamReader(tuple.Item2[0]))
                    {

                        string line;

                        while ((line = streamReader.ReadLine()) != null)
                        {
                            line.Split(' ');
                        }

                            

                        analyzeLensRequestDTO.

                        Debug.WriteLine("0");

                        credentialSoapClient.an

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
