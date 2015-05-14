using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Data;

namespace AwfulForumsReader.Tools
{ 
public class NativeTypeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        // No format provided.
        if (parameter == null)
            return value;

        var parameters = parameter.ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        var format = (parameters.Length > 1 ? parameters[1] : "");
        return FormatString(value, format);
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        var parameters = parameter.ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        var type = parameters[0];

        switch (type)
        {
            case "Decimal":
                return decimal.Parse(value.ToString());
            case "Int":
                return int.Parse(value.ToString());
            case "DateTime":
                return DateTime.Parse(value.ToString());
        }

        return value;
    }

    private static string FormatString(object value, string format)
    {
        if (value != null && string.IsNullOrEmpty(format))
            return value.ToString();

        return string.Format(format, value);
    }
}

}
