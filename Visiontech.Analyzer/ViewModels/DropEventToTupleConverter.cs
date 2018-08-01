using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Data;

namespace Visiontech.Analyzer.ViewModels
{
    public class DropEventToTupleConverter : IValueConverter
    {

        public static readonly DropEventToTupleConverter Default = new DropEventToTupleConverter();

        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {

            if (value is DragEventArgs && parameter is string)
            {
                return new Tuple<ViewModel.Side, string[]>((ViewModel.Side) Enum.Parse(typeof(ViewModel.Side), parameter as string) , (value as DragEventArgs).Data.GetData(DataFormats.FileDrop) as string[]);
            }

            throw new Exception("NOT SUPPORTED");
        }

        public object ConvertBack(object value, Type targetType, object parameter,
                System.Globalization.CultureInfo culture)
        {
            throw new Exception("NOT SUPPORTED");
        }


    }
}
