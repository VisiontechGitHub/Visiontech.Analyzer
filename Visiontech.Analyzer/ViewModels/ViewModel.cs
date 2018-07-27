using Org.Visiontech.Compute;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
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

        public event EventHandler<Tuple<Side, analyzeLensResponseDTO>> LensAnalyzed;

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

                    Task.Run(() => LensAnalyzed.Invoke(this, new Tuple<Side, analyzeLensResponseDTO>(tuple.Item1, computeSoapClient.analyzeLens(FromXYZFile(tuple.Item2[0])) as analyzeLensResponseDTO)));

                    break;
                case ".hmf":

                    Task.Run(() => LensAnalyzed.Invoke(this, new Tuple<Side, analyzeLensResponseDTO>(tuple.Item1, computeSoapClient.analyzeLens(FromHMFFile(tuple.Item2[0])) as analyzeLensResponseDTO)));

                    break;
                case ".sdf":

                    Tuple<analyzeLensRequestDTO, analyzeLensRequestDTO> requests = FromSDFFile(tuple.Item2[0]);
                    if (requests.Item1 != null)
                    {
                        Task.Run(() => LensAnalyzed.Invoke(this, new Tuple<Side, analyzeLensResponseDTO>(Side.LEFT, computeSoapClient.analyzeLens(requests.Item1) as analyzeLensResponseDTO)));
                    }
                    if (requests.Item2 != null)
                    {
                        Task.Run(() => LensAnalyzed.Invoke(this, new Tuple<Side, analyzeLensResponseDTO>(Side.RIGHT, computeSoapClient.analyzeLens(requests.Item2) as analyzeLensResponseDTO)));
                    }

                    break;
            }

        }

        private analyzeLensRequestDTO FromXYZFile(string file)
        {

            analyzeLensRequestDTO analyzeLensRequestDTO = new analyzeLensRequestDTO();
            ICollection<threeDimensionalPointDTO> points = new Collection<threeDimensionalPointDTO>();

            using (StreamReader streamReader = new StreamReader(file))
            {

                string line;

                while ((line = streamReader.ReadLine()) != null)
                {
                    string[] coordinates = line.Split(' ');
                    if (coordinates.Length == 3)
                    {
                        points.Add(new threeDimensionalPointDTO()
                        {
                            x = double.Parse(coordinates[0], CultureInfo.InvariantCulture),
                            xSpecified = true,
                            y = double.Parse(coordinates[1], CultureInfo.InvariantCulture),
                            ySpecified = true,
                            z = double.Parse(coordinates[2], CultureInfo.InvariantCulture),
                            zSpecified = true
                        });
                    }
                }

            }

            analyzeLensRequestDTO.points = points.ToArray();

            return analyzeLensRequestDTO;

        }

        private analyzeLensRequestDTO FromHMFFile(string file)
        {

            analyzeLensRequestDTO analyzeLensRequestDTO = null;
            Collection<Collection<threeDimensionalPointDTO>> rows = new Collection<Collection<threeDimensionalPointDTO>>();

            int Count = 0;
            double Interval = 0;

            using (StreamReader streamReader = new StreamReader(file))
            {

                string line;

                while ((line = streamReader.ReadLine()) != null)
                {
                    if (line.StartsWith("Count")) {
                        Count = int.Parse(line.Split('=')[1], CultureInfo.InvariantCulture);
                    } else if (line.StartsWith("Interval"))
                    {
                        Interval = double.Parse(line.Split('=')[1], CultureInfo.InvariantCulture);
                    } else if ("[Data]".Equals(line))
                    {
                        while ((line = streamReader.ReadLine()) != null)
                        {
                            Collection<threeDimensionalPointDTO> row = new Collection<threeDimensionalPointDTO>();
                            foreach (string z in line.Split(','))
                            {
                                row.Add(
                                    new threeDimensionalPointDTO(){
                                        z = double.Parse(z, CultureInfo.InvariantCulture),
                                        zSpecified = true
                                    }
                                );
                            }
                            rows.Add(row);
                        }
                    }
                }

            }

            if (Count > 0 && Interval > 0 && Count == rows.Count) {
                analyzeLensRequestDTO = new analyzeLensRequestDTO
                {
                    points = ComputeXY(rows, Count, Interval).ToArray()
                };
            }

            return analyzeLensRequestDTO;

        }



        private Tuple<analyzeLensRequestDTO, analyzeLensRequestDTO> FromSDFFile(string file)
        {

            analyzeLensRequestDTO leftAnalyzeLensRequestDTO = null;
            analyzeLensRequestDTO rightAnalyzeLensRequestDTO = null;
            Collection<Collection<threeDimensionalPointDTO>> leftRows = null;
            Collection<Collection<threeDimensionalPointDTO>> rightRows = null;
            Collection<Collection<threeDimensionalPointDTO>> currentRows = null;

            int leftCount = 0;
            int rightCount = 0;
            double leftInterval = 0;
            double rightInterval = 0;

            using (StreamReader streamReader = new StreamReader(file))
            {

                string line;

                while ((line = streamReader.ReadLine()) != null)
                {
                    if (line.StartsWith("SURFMT"))
                    {
                        string[] parameters = line.Split('=')[1].Split(';');

                        currentRows = new Collection<Collection<threeDimensionalPointDTO>>();

                        if ("R".Equals(parameters[1]))
                        {
                            rightInterval = double.Parse(parameters[0], CultureInfo.InvariantCulture);
                            rightCount = int.Parse(parameters[3], CultureInfo.InvariantCulture);
                            rightRows = currentRows;
                        } else
                        {
                            leftInterval = double.Parse(parameters[0], CultureInfo.InvariantCulture);
                            leftCount = int.Parse(parameters[3], CultureInfo.InvariantCulture);
                            leftRows = currentRows;
                        }

                    } else if (line.StartsWith("ZZ") && currentRows != null)
                    {
                        Collection<threeDimensionalPointDTO> row = new Collection<threeDimensionalPointDTO>();
                        foreach (string z in line.Split('=')[1].Split(';'))
                        {
                            row.Add(
                                new threeDimensionalPointDTO()
                                {
                                    z = double.Parse(z, CultureInfo.InvariantCulture),
                                    zSpecified = true
                                }
                            );
                        }
                        currentRows.Add(row);
                    }
                }

            }

            if (leftCount > 0 && leftInterval > 0 && leftCount == leftRows.Count)
            {
                leftAnalyzeLensRequestDTO = new analyzeLensRequestDTO
                {
                    points = ComputeXY(leftRows, leftCount, leftInterval).ToArray()
                };
            }

            if (rightCount > 0 && rightInterval > 0 && rightCount == rightRows.Count)
            {
                rightAnalyzeLensRequestDTO = new analyzeLensRequestDTO
                {
                    points = ComputeXY(rightRows, rightCount, rightInterval).ToArray()
                };
            }

            return new Tuple<analyzeLensRequestDTO, analyzeLensRequestDTO>(leftAnalyzeLensRequestDTO, rightAnalyzeLensRequestDTO);

        }

        private Collection<threeDimensionalPointDTO> ComputeXY(Collection<Collection<threeDimensionalPointDTO>> rows, int Count, double Interval) {


            Collection<threeDimensionalPointDTO> points = new Collection<threeDimensionalPointDTO>();

            foreach (Collection<threeDimensionalPointDTO> row in rows)
            {
                foreach (threeDimensionalPointDTO point in row)
                {
                    point.x = Interval * row.IndexOf(point) - (Count - 1) / 2.0 * Interval;
                    point.xSpecified = true;
                    point.y = (Count - 1) / 2.0 * Interval - Interval * rows.IndexOf(row);
                    point.ySpecified = true;

                    points.Add(point);
                }
            }

            return points;

        }

    }
}
