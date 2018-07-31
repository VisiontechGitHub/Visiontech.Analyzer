using Org.Visiontech.Compute;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Media;
using Urho;
using Urho.Extensions.Wpf;
using Visiontech.Analyzer.Model;

namespace Visiontech.Analyzer.View
{
    public class ThreeDimensionalLensPage : Page
    {

        private UrhoSurface urhoSurface = new UrhoSurface();

        public ICollection<threeDimensionalPointDTO> Points { get; }

        public Func<threeDimensionalPointDTO, double> Mapping { get; }

        public ThreeDimensionalLensPage(ICollection<threeDimensionalPointDTO> Points, Func<threeDimensionalPointDTO, double> Mapping)
        {
            Content = urhoSurface;
            this.Points = Points;
            this.Mapping = Mapping;

            urhoSurface.Show<Dots>(new DotsOptions()
            {
                Points = Points,
                Mapping = Mapping
            });
        }

    }
}
