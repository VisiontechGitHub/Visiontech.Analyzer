using Org.Visiontech.Compute;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System;
using System.Collections.Generic;
using System.Linq;
using Visiontech.Analyzer.View.Abstraction;
using Visiontech.Analyzer.ViewModels;

namespace Visiontech.Analyzer.View
{
    public partial class ViewPage : LoadingPage<ViewModel>
    {

        public ViewPage()
        {
            InitializeComponent();

            model.LensAnalyzed += Model_LensAnalyzed;
        }

        private void Model_LensAnalyzed(object sender, Tuple<ViewModel.Side, analyzeLensResponseDTO> tuple)
        {

            Dispatcher.Invoke(() => {
                RightLens.InvalidateVisual();
                LeftLens.InvalidateVisual();
            });

        }

        private void PaintLens(object sender, SKPaintSurfaceEventArgs args)
        {

            analyzeLensResponseDTO analyzeLensResponseDTO = RightLens.Equals(sender) ? model.RightAnalyzeLensResponse : model.LeftAnalyzeLensResponse;

            if (analyzeLensResponseDTO != null)
            {

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

                if (analyzeLensResponseDTO.points != null & analyzeLensResponseDTO.points.Any())
                {

                    ICollection<analyzedPointDTO> points = analyzeLensResponseDTO.points;

                    double maxCylinder = points.Select(point => point.cylinderMap).Max();
                    double minCylinder = points.Select(point => point.cylinderMap).Min();
                    double cylinderRange = (maxCylinder - minCylinder) / 6.0;

                    if (cylinderRange > 0)
                    {

                        double doubleCylinderRange = cylinderRange * 2;
                        double tripleCylinderRange = cylinderRange * 3;
                        double fiveCylinderRange = cylinderRange * 5;

                        double centerX = info.Width / 2;
                        double centerY = info.Height / 2;

                        int sqrt = Convert.ToInt32(Math.Sqrt(points.Count));
                        int side = (sqrt - 1) / 2;

                        double bitmapSide = Math.Min(info.Width, info.Height) / 2.0;

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

                        canvas.DrawText(string.Format("{0:N2}", minCylinder), Convert.ToSingle(info.Width - bitmapSide / 8), blackSKPaint.TextSize * 2, blackSKPaint);
                        canvas.DrawText(string.Format("{0:N2}", minCylinder + cylinderRange), Convert.ToSingle(info.Width - bitmapSide / 8), blackSKPaint.TextSize * 2 + textRange, blackSKPaint);
                        canvas.DrawText(string.Format("{0:N2}", minCylinder + cylinderRange * 2), Convert.ToSingle(info.Width - bitmapSide / 8), blackSKPaint.TextSize * 2 + textRange * 2, blackSKPaint);
                        canvas.DrawText(string.Format("{0:N2}", minCylinder + cylinderRange * 3), Convert.ToSingle(info.Width - bitmapSide / 8), blackSKPaint.TextSize * 2 + textRange * 3, blackSKPaint);
                        canvas.DrawText(string.Format("{0:N2}", minCylinder + cylinderRange * 4), Convert.ToSingle(info.Width - bitmapSide / 8), blackSKPaint.TextSize * 2 + textRange * 4, blackSKPaint);
                        canvas.DrawText(string.Format("{0:N2}", minCylinder + cylinderRange * 5), Convert.ToSingle(info.Width - bitmapSide / 8), blackSKPaint.TextSize * 2 + textRange * 5, blackSKPaint);
                        canvas.DrawText(string.Format("{0:N2}", maxCylinder), Convert.ToSingle(info.Width - bitmapSide / 8), blackSKPaint.TextSize * 2 + textRange * 6, blackSKPaint);

                        using (SKBitmap bitmap = new SKBitmap(sqrt, sqrt))
                        {

                            foreach (analyzedPointDTO point in points)
                            {

                                SKColor color = SKColors.Black;

                                double value = point.cylinderMap - minCylinder;

                                switch (Math.Floor(value / cylinderRange))
                                {

                                    case 0:
                                        color = new SKColor(Convert.ToByte(Math.Round(value / cylinderRange * 255)), Convert.ToByte(Math.Round(value / cylinderRange * 255)), 255);
                                        break;
                                    case 1:
                                        color = new SKColor(Convert.ToByte(Math.Round((doubleCylinderRange - value) / cylinderRange * 255)), 255, 255);
                                        break;
                                    case 2:
                                        color = new SKColor(0, 255, Convert.ToByte(Math.Round((tripleCylinderRange - value) / cylinderRange * 255)));
                                        break;
                                    case 3:
                                        color = new SKColor(Convert.ToByte(Math.Round((value - tripleCylinderRange) / cylinderRange * 255)), 255, 0);
                                        break;
                                    case 4:
                                        color = new SKColor(255, Convert.ToByte(Math.Round((fiveCylinderRange - value) / cylinderRange * 255)), 0);
                                        break;
                                    default:
                                        color = new SKColor(255, 0, Convert.ToByte(Math.Round((value - fiveCylinderRange) / cylinderRange * 255)));
                                        break;

                                }

                                bitmap.SetPixel(Convert.ToInt32(side - point.x), Convert.ToInt32(side - point.y), color);

                            }

                            using (SKPath path = new SKPath())
                            {
                                path.AddCircle(Convert.ToSingle(centerX), Convert.ToSingle(centerY), Convert.ToSingle(bitmapSide * 5 / 6));
                                canvas.ClipPath(path, SKClipOperation.Intersect);
                            }

                            canvas.DrawBitmap(bitmap.Resize(new SKImageInfo(Convert.ToInt32(bitmapSide * 5 / 6), Convert.ToInt32(bitmapSide * 5 / 6)), SKBitmapResizeMethod.Mitchell), new SKRect(Convert.ToSingle(centerX - bitmapSide * 5 / 6), Convert.ToSingle(centerY - bitmapSide * 5 / 6), Convert.ToSingle(centerX + bitmapSide * 5 / 6), Convert.ToSingle(centerY + bitmapSide * 5 / 6)));

                        }

                    }

                }

            }

        }

        private void ComputeChart(analyzeLensResponseDTO analyzeLensResponseDTO, Func<threeDimensionalPointDTO, double> Mapping)
        {

            NavigationService.Navigate(new ThreeDimensionalLensPage(analyzeLensResponseDTO.points, Mapping));

        }

        private void Left_Toolbar_3D_Clicked(object sender, EventArgs e)
        {
            ComputeChart(LeftLens.DataContext as analyzeLensResponseDTO, ToZ);
        }
        private void Right_Toolbar_3D_Clicked(object sender, EventArgs e)
        {
            ComputeChart(RightLens.DataContext as analyzeLensResponseDTO, ToZ);
        }
        private void Left_Toolbar_CylinderMap_Clicked(object sender, EventArgs e)
        {
            ComputeChart(LeftLens.DataContext as analyzeLensResponseDTO, ToCylinderMap);
        }
        private void Right_Toolbar_CylinderMap_Clicked(object sender, EventArgs e)
        {
            ComputeChart(RightLens.DataContext as analyzeLensResponseDTO, ToCylinderMap);
        }
        private void Left_Toolbar_PowerMap_Clicked(object sender, EventArgs e)
        {
            ComputeChart(LeftLens.DataContext as analyzeLensResponseDTO, ToPowerMap);
        }
        private void Right_Toolbar_PowerMap_Clicked(object sender, EventArgs e)
        {
            ComputeChart(RightLens.DataContext as analyzeLensResponseDTO, ToPowerMap);
        }

        private double ToZ(threeDimensionalPointDTO point)
        {
            return point.z;
        }
        private double ToCylinderMap(threeDimensionalPointDTO point)
        {
            return (point as analyzedPointDTO).cylinderMap;
        }
        private double ToPowerMap(threeDimensionalPointDTO point)
        {
            return (point as analyzedPointDTO).powerMap;
        }


    }
}
