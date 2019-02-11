using Org.Visiontech.Compute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Visiontech.Analyzer.ViewModels
{
    public class ChartModel : BaseViewModel
    {

        public ICollection<threeDimensionalPointDTO> Points { get; }

        public Func<threeDimensionalPointDTO, double> Mapping { get; }

    }
}
