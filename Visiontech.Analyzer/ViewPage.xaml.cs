using Org.Visiontech.Compute;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using SkiaSharp.Views.WPF;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using VisiontechCommons;
using VisualizzatoreWPF.ViewModels;

namespace VisualizzatoreWPF
{
    public partial class ViewPage : Page
    {

        protected readonly ViewModel model = Container.ServiceProvider.GetService(typeof(ViewModel)) as ViewModel;

        public ViewPage()
        {
            InitializeComponent();

            model.LensAnalyzed += Model_LensAnalyzed;
        }

        private void Model_LensAnalyzed(object sender, Tuple<ViewModel.Side, analyzeLensResponseDTO> tuple)
        {
            
            switch (tuple.Item1)
            {
                case ViewModel.Side.LEFT:
                    Dispatcher.Invoke(() => {
                        LeftLens.DataContext = tuple.Item2;
                        LeftLens.InvalidateVisual();
                    });
                    break;
                case ViewModel.Side.RIGHT:
                    Dispatcher.Invoke(() => {
                        RightLens.DataContext = tuple.Item2;
                        RightLens.InvalidateVisual();
                    });
                    break;
            }

        }

        private void DropFile(object sender, DragEventArgs e)
        {
            ViewModel.Side side = RightLens.Equals(sender) ? ViewModel.Side.RIGHT : ViewModel.Side.LEFT;
            model.LoadFileCommand.Execute(new Tuple<ViewModel.Side, string[]>(side, e.Data.GetData(DataFormats.FileDrop) as string[]));
        }

        private void PaintLens(object sender, SKPaintSurfaceEventArgs args)
        {

            if ((sender as SKElement).DataContext != null && (sender as SKElement).DataContext is analyzeLensResponseDTO)
            {

                analyzeLensResponseDTO analyzeLensResponseDTO = (sender as SKElement).DataContext as analyzeLensResponseDTO;

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

                    double radius = Math.Max(analyzeLensResponseDTO.points.Select(point => Math.Abs(point.x)).Max(), analyzeLensResponseDTO.points.Select(point => Math.Abs(point.y)).Max());

                    ICollection<analyzedPointDTO> points = analyzeLensResponseDTO.points.Where(point => Math.Sqrt(Math.Pow(point.x, 2) + Math.Pow(point.y, 2)) <= radius).Select(point => point as analyzedPointDTO).ToList();

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

                            canvas.DrawBitmap(bitmap.Resize(new SKImageInfo(Convert.ToInt32(bitmapSide / 6), Convert.ToInt32(bitmapSide)), SKBitmapResizeMethod.Mitchell), new SKRect(Convert.ToSingle(info.Width - bitmapSide / 6), 0, Convert.ToSingle(info.Width), info.Height));

                        }

                        blackSKPaint.TextSize = info.Height / 32;

                        float textRange = (info.Height - blackSKPaint.TextSize * 3) / 6;

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
    }
}
