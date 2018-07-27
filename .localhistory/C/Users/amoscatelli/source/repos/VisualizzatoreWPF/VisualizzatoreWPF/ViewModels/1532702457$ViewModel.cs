using Org.Visiontech.Compute;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace VisualizzatoreWPF.ViewModels
{
    public class ViewModel : BaseViewModel
    {

        protected readonly ComputeSoapClient computeSoapClient = VisiontechCommons.Container.ServiceProvider.GetService(typeof(ComputeSoapClient)) as ComputeSoapClient;

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
            ICollection<threeDimensionalPointDTO> points = new Collection<threeDimensionalPointDTO>();

            switch (Path.GetExtension(tuple.Item2[0]))
            {
                case ".txt":
                case ".xyz":

                    using (StreamReader streamReader = new StreamReader(tuple.Item2[0]))
                    {

                        string line;

                        while ((line = streamReader.ReadLine()) != null)
                        {
                            string[] coordinates = line.Split(' ');
                            if (coordinates.Length == 3) {
                                points.Add(new threeDimensionalPointDTO() {
                                    x = double.Parse(coordinates[0], CultureInfo.InvariantCulture),
                                    y = double.Parse(coordinates[1], CultureInfo.InvariantCulture),
                                    z = double.Parse(coordinates[2], CultureInfo.InvariantCulture)
                                });
                            }
                        }

                        if (points.Count > 0)
                        {

                            analyzeLensRequestDTO.points = points.ToArray();

                        }

                    }

                    break;
                case ".hmf":
                    Debug.WriteLine("1");
                    break;
                case ".sdf":
                    Debug.WriteLine("2");
                    break;
            }

            foreach (threeDimensionalPointDTO point in points)
            {
                Debug.WriteLine("CICCIO " + string.Join(" ", point.x, point.y, point.z));
            }


        }
    }
}
