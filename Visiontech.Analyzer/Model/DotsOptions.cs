using Org.Visiontech.Compute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Urho;

namespace Visiontech.Analyzer.Model
{
    public class DotsOptions : ApplicationOptions
    {
        public ICollection<threeDimensionalPointDTO> Points { get; set; }

        public Func<threeDimensionalPointDTO, double> Mapping { get; set; }

        public DotsOptions()
        {
            Mapping = point => point.z;
        }

    }
}
