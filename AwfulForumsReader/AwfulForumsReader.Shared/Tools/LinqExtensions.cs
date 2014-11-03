using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace AwfulForumsReader.Tools
{
    public static class LinqExtensions
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> _LinqResult)
        {
            return new ObservableCollection<T>(_LinqResult);
        }
    }
}
