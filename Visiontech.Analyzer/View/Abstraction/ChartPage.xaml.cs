using Org.Visiontech.Compute;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using SkiaSharp.Views.WPF;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Visiontech.Analyzer.View.Abstraction;
using Visiontech.Analyzer.ViewModels;

namespace Visiontech.Analyzer.View
{
    public partial class ChartPage : LoadingPage<ViewModel>
    {

        private SKElement selected;
        private Func<threeDimensionalPointDTO, double> mapping;
        private bool compare = false;
        private static bool mask = true;

        public ChartPage()
        {
            InitializeComponent();

            MouseWheel += ViewPage_MouseWheel;

            mapping = ToCylinderMap;

            model.PropertyChanged += Model_PropertyChanged;
        }

        private void ViewPage_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            Debug.WriteLine("FREGNA");
        }

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {



        }

        private void LensSelected(object sender, MouseButtonEventArgs e)
        {


        }

        private void PaintLens(object sender, SKPaintSurfaceEventArgs args)
        {

            

        }

        private static bool IsInside(twoDimensionalPointDTO point, double radius)
        {
            return mask ? Math.Sqrt(Math.Pow(point.x, 2) + Math.Pow(point.y, 2)) <= radius : Math.Abs(point.x) <= radius && Math.Abs(point.y) <= radius;
        }
        private static double ToZ(threeDimensionalPointDTO point)
        {
            return point.z;
        }
        private static double ToCylinderMap(threeDimensionalPointDTO point)
        {
            return (point as analyzedPointDTO).cylinderMap;
        }
        private static double ToPowerMap(threeDimensionalPointDTO point)
        {
            return (point as analyzedPointDTO).powerMap;
        }

    }
}
