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
    public partial class ViewPage : LoadingPage<ViewModel>
    {

        private SKElement selected;
        private Func<threeDimensionalPointDTO, double> mapping;
        private bool compare = false;
        private static bool mask = true;

        public ViewPage()
        {
            InitializeComponent();

            mapping = ToCylinderMap;

            model.PropertyChanged += Model_PropertyChanged;
        }

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

            if ("RightAnalyzeLensResponse".Equals(e.PropertyName))
            {
                RightLens.InvalidateVisual();
                RightLens.MouseLeftButtonDown -= LensSelected;
                RightLens.MouseLeftButtonDown += LensSelected;
                RightLens.MouseMove += Tooltip_MouseMove;
            } else if ("LeftAnalyzeLensResponse".Equals(e.PropertyName))
            {
                LeftLens.InvalidateVisual();
                LeftLens.MouseLeftButtonDown -= LensSelected;
                LeftLens.MouseLeftButtonDown += LensSelected;
                LeftLens.MouseMove += Tooltip_MouseMove;
            }

        }

        private void LensSelected(object sender, MouseButtonEventArgs e)
        {

            if (sender is SKElement)
            {

                if (!sender.Equals(selected))
                {

                    if (!(selected is null))
                    {

                        selected.Margin = new Thickness(0);

                    }

                    selected = sender as SKElement;

                    selected.Margin = new Thickness(5);

                    ToolBar.Visibility = Visibility.Visible;

                } else {

                    if (!(selected is null))
                    {

                        selected.Margin = new Thickness(0);

                        selected = null;

                    }

                    ToolBar.Visibility = Visibility.Collapsed;
                }



            }
        }

        private void PaintLens(object sender, SKPaintSurfaceEventArgs args)
        {

            analyzeLensResponseDTO analyzeLensResponseDTO = RightLens.Equals(sender) ? model.RightAnalyzeLensResponse : model.LeftAnalyzeLensResponse;

            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            SKPaint blackSKPaint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = SKColors.Black
            };

            canvas.DrawRect(new SKRect(0, 0, info.Width, info.Height), blackSKPaint);

            if (analyzeLensResponseDTO != null && analyzeLensResponseDTO.points != null & analyzeLensResponseDTO.points.Any())
            {

                int sqrt = Convert.ToInt32(Math.Sqrt(analyzeLensResponseDTO.points.Count()));
                int side = (sqrt - 1) / 2;

                ICollection<analyzedPointDTO> points = analyzeLensResponseDTO.points.Where(p => IsInside(p, side + 1)).ToList();

                double max = compare && model.RightAnalyzeLensResponse != null && model.LeftAnalyzeLensResponse != null ? Math.Max(model.RightAnalyzeLensResponse.points.Where(p => IsInside(p, side + 1)).Select(mapping).Max(), model.LeftAnalyzeLensResponse.points.Where(p => IsInside(p, side + 1)).Select(mapping).Max()) : points.Where(p => IsInside(p, side + 1)).Select(mapping).Max();
                double min = compare && model.RightAnalyzeLensResponse != null && model.LeftAnalyzeLensResponse != null ? Math.Min(model.RightAnalyzeLensResponse.points.Where(p => IsInside(p, side + 1)).Select(mapping).Min(), model.LeftAnalyzeLensResponse.points.Where(p => IsInside(p, side + 1)).Select(mapping).Min()) : points.Where(p => IsInside(p, side + 1)).Select(mapping).Min();
                double range = (max - min) / 6.0;

                if (range > 0)
                {

                    double centerX = info.Width / 2.0;
                    double centerY = info.Height / 2.0;
                    double bitmapSide = Math.Min(centerX, centerY);
                    double radius = bitmapSide * 5 / 6;

                    double doubleRange = range * 2;
                    double tripleRange = range * 3;
                    double fiveRange = range * 5;

                    double sqrtRange = sqrt / 6.0;
                    double doubleSqrtRange = sqrtRange * 2;
                    double tripleSqrtRange = sqrtRange * 3;
                    double fiveSqrtRange = sqrtRange * 5;

                    using (SKBitmap bitmap = new SKBitmap(1, sqrt))
                    {

                        for (int i = sqrt - 1; i >= 0; i--)
                        {

                            SKColor color = SKColors.Black;

                            switch (Math.Floor(i / sqrtRange))
                            {

                                case 0:
                                    color = new SKColor(Convert.ToByte(Math.Round(i / sqrtRange * 255)), Convert.ToByte(Math.Round(i / sqrtRange * 255)), 255);
                                    break;
                                case 1:
                                    color = new SKColor(Convert.ToByte(Math.Round((doubleSqrtRange - i) / sqrtRange * 255)), 255, 255);
                                    break;
                                case 2:
                                    color = new SKColor(0, 255, Convert.ToByte(Math.Round((tripleSqrtRange - i) / sqrtRange * 255)));
                                    break;
                                case 3:
                                    color = new SKColor(Convert.ToByte(Math.Round((i - tripleSqrtRange) / sqrtRange * 255)), 255, 0);
                                    break;
                                case 4:
                                    color = new SKColor(255, Convert.ToByte(Math.Round((fiveSqrtRange - i) / sqrtRange * 255)), 0);
                                    break;
                                default:
                                    color = new SKColor(255, 0, Convert.ToByte(Math.Round((i - fiveSqrtRange) / sqrtRange * 255)));
                                    break;

                            }

                            bitmap.SetPixel(0, i, color);

                        }

                        canvas.DrawBitmap(bitmap.Resize(new SKImageInfo(Convert.ToInt32(bitmapSide / 6.0), Convert.ToInt32(bitmapSide)), SKBitmapResizeMethod.Mitchell), new SKRect(Convert.ToSingle(info.Width - bitmapSide / 6.0), 0, Convert.ToSingle(info.Width), info.Height));

                    }

                    blackSKPaint.TextSize = info.Height / 32f;

                    float textRange = (info.Height - blackSKPaint.TextSize * 3) / 6f;

                    canvas.DrawText(string.Format("{0:N2}", min), Convert.ToSingle(info.Width - bitmapSide / 8), blackSKPaint.TextSize * 2, blackSKPaint);
                    canvas.DrawText(string.Format("{0:N2}", min + range), Convert.ToSingle(info.Width - bitmapSide / 8), blackSKPaint.TextSize * 2 + textRange, blackSKPaint);
                    canvas.DrawText(string.Format("{0:N2}", min + range * 2), Convert.ToSingle(info.Width - bitmapSide / 8), blackSKPaint.TextSize * 2 + textRange * 2, blackSKPaint);
                    canvas.DrawText(string.Format("{0:N2}", min + range * 3), Convert.ToSingle(info.Width - bitmapSide / 8), blackSKPaint.TextSize * 2 + textRange * 3, blackSKPaint);
                    canvas.DrawText(string.Format("{0:N2}", min + range * 4), Convert.ToSingle(info.Width - bitmapSide / 8), blackSKPaint.TextSize * 2 + textRange * 4, blackSKPaint);
                    canvas.DrawText(string.Format("{0:N2}", min + range * 5), Convert.ToSingle(info.Width - bitmapSide / 8), blackSKPaint.TextSize * 2 + textRange * 5, blackSKPaint);
                    canvas.DrawText(string.Format("{0:N2}", max), Convert.ToSingle(info.Width - bitmapSide / 8), blackSKPaint.TextSize * 2 + textRange * 6, blackSKPaint);

                    using (SKBitmap bitmap = new SKBitmap(sqrt, sqrt))
                    {

                        foreach (analyzedPointDTO point in points)
                        {

                            SKColor color = SKColors.Black;

                            double value = mapping.Invoke(point) - min;

                            switch (Math.Floor(value / range))
                            {

                                case 0:
                                    color = new SKColor(Convert.ToByte(Math.Round(value / range * 255)), Convert.ToByte(Math.Round(value / range * 255)), 255);
                                    break;
                                case 1:
                                    color = new SKColor(Convert.ToByte(Math.Round((doubleRange - value) / range * 255)), 255, 255);
                                    break;
                                case 2:
                                    color = new SKColor(0, 255, Convert.ToByte(Math.Round((tripleRange - value) / range * 255)));
                                    break;
                                case 3:
                                    color = new SKColor(Convert.ToByte(Math.Round((value - tripleRange) / range * 255)), 255, 0);
                                    break;
                                case 4:
                                    color = new SKColor(255, Convert.ToByte(Math.Round((fiveRange - value) / range * 255)), 0);
                                    break;
                                default:
                                    color = new SKColor(255, 0, Convert.ToByte(Math.Round((value - fiveRange) / range * 255)));
                                    break;

                            }

                            bitmap.SetPixel(Convert.ToInt32(side - point.x), Convert.ToInt32(side - point.y), color);

                        }

                        if (mask)
                        {
                            using (SKPath path = new SKPath())
                            {
                                path.AddCircle(Convert.ToSingle(centerX), Convert.ToSingle(centerY), Convert.ToSingle(radius));
                                canvas.ClipPath(path, SKClipOperation.Intersect);
                            }
                        }

                        canvas.DrawBitmap(bitmap.Resize(new SKImageInfo(Convert.ToInt32(radius), Convert.ToInt32(radius)), SKBitmapResizeMethod.Mitchell), new SKRect(Convert.ToSingle(centerX - radius), Convert.ToSingle(centerY - radius), Convert.ToSingle(centerX + radius), Convert.ToSingle(centerY + radius)));

                    }

                }

            }

        }

        private void ComputeChart(Func<threeDimensionalPointDTO, double> Mapping)
        {
            if (LeftLens.Equals(selected))
            {
                NavigationService.Navigate(new ThreeDimensionalLensPage(model.LeftAnalyzeLensResponse.points, Mapping));
            } else if (RightLens.Equals(selected))
            {
                NavigationService.Navigate(new ThreeDimensionalLensPage(model.RightAnalyzeLensResponse.points, Mapping));
            }
        }


        private void Toolbar_CurveMap_Clicked(object sender, EventArgs e)
        {
            mapping = ToZ;
            RightLens.InvalidateVisual();
            LeftLens.InvalidateVisual();
        }
        private void Toolbar_CylinderMap_Clicked(object sender, EventArgs e)
        {
            mapping = ToCylinderMap;
            RightLens.InvalidateVisual();
            LeftLens.InvalidateVisual();
        }
        private void Toolbar_PowerMap_Clicked(object sender, EventArgs e)
        {
            mapping = ToPowerMap;
            RightLens.InvalidateVisual();
            LeftLens.InvalidateVisual();
        }

        private void Toolbar_3DCurveMap_Clicked(object sender, EventArgs e)
        {
            ComputeChart(ToZ);
        }
        private void Toolbar_3DCylinderMap_Clicked(object sender, EventArgs e)
        {
            ComputeChart(ToCylinderMap);
        }
        private void Toolbar_3DPowerMap_Clicked(object sender, EventArgs e)
        {
            ComputeChart(ToPowerMap);
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



        private void Tooltip_MouseMove(object sender, MouseEventArgs e)
        {

            double centerX = (sender as FrameworkElement).ActualWidth / 2.0;
            double centerY = (sender as FrameworkElement).ActualHeight / 2.0;
            double radius = Math.Min(centerX, centerY) * 5 / 6;

            Point currentPos = e.GetPosition(sender as IInputElement);

            double x = centerX - currentPos.X;
            double y = centerY - currentPos.Y;

            bool inside = IsInside(
                new twoDimensionalPointDTO() {
                    x = x,
                    y = y
                },
                radius);

            if (!floatingTip.IsOpen.Equals(inside)) {
                floatingTip.IsOpen = inside;
            }

            if (inside)
            {

                floatingTip.PlacementTarget = sender as UIElement;

                floatingTip.HorizontalOffset = currentPos.X + 20;
                floatingTip.VerticalOffset = currentPos.Y;

                analyzedPointDTO[] points = LeftLens.Equals(sender) ? model.LeftAnalyzeLensResponse.points : model.RightAnalyzeLensResponse.points;

                int sqrt = Convert.ToInt32(Math.Sqrt(points.Count()));
                int side = (sqrt - 1) / 2;

                int xPoint = Convert.ToInt32(Math.Round(x * side / radius));
                int yPoint = Convert.ToInt32(Math.Round(y * side / radius));

                int index = (side + xPoint) * sqrt + side - yPoint;

                floatingTip.DataContext = points[index];

            }

        }

        private void Toolbar_Compare_Clicked(object sender, RoutedEventArgs e)
        {
            compare = !compare;
            RightLens.InvalidateVisual();
            LeftLens.InvalidateVisual();
        }

        private void Toolbar_Mask_Clicked(object sender, RoutedEventArgs e)
        {
            mask = !mask;
            RightLens.InvalidateVisual();
            LeftLens.InvalidateVisual();
        }
    }
}
