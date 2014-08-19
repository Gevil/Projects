using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Windows.Data;
using System.Windows.Media;

namespace WpfColourSampler.Classes
{
    /// <summary>
    /// Converts between PropertyInfo and Brushes by name
    /// </summary>
    public class PropertyInfoToBrushConverter : IValueConverter
    {
        #region IValueConverter Members

        object IValueConverter.Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                // if we have a brush
                if (value is Brush)
                {
                    // get type
                    Type brushType = (typeof(System.Windows.Media.Brushes));
                    // Get the public properties.
                    PropertyInfo[] brushPropertyInfo = brushType.GetProperties();

                    // hold value bruhs
                    SolidColorBrush valueBrush = value as SolidColorBrush;

                    // create converter
                    BrushConverter brushConverter = new BrushConverter();

                    // create brush
                    SolidColorBrush propertyBrush;

                    // itterate all property infos
                    foreach (PropertyInfo info in brushPropertyInfo)
                    {
                        // set item from colour
                        propertyBrush = (SolidColorBrush)brushConverter.ConvertFromString(info.Name);

                        // compare
                        if (propertyBrush.Color == valueBrush.Color)
                        {
                            // we have found our info! PHEW!
                            return info;
                        }
                    }

                    // just return null
                    return null;
                }
                else
                {
                    // just return null
                    return null;
                }
            }
            catch (Exception e)
            {
                // display
                Console.WriteLine("Exception caught converting brush: {0}", e.Message);

                // return null
                return null;
            }
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                // get brush from runtime property info
                if (value is PropertyInfo)
                {
                    // create converter
                    BrushConverter converter = new BrushConverter();

                    // convert neme to brush
                    Brush selectedBrush = (Brush)converter.ConvertFromString(((PropertyInfo)value).Name.ToString());

                    // get brush
                    return selectedBrush;
                }
                else
                {
                    // return black
                    return Brushes.Black;
                }
            }
            catch (Exception e)
            {
                // display
                Console.WriteLine("Exception caught converting property info: {0}", e.Message);

                // return black
                return Brushes.Black;
            }
        }

        #endregion
    }

}
