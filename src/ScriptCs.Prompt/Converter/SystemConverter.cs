using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptCs.Prompt
{
    internal class SystemConverter<T> : IConverter<T>
    {
        TypeConverter _converter;
        internal SystemConverter()
        {
            _converter = TypeDescriptor.GetConverter(typeof(T));
        }

        T IConverter<T>.ConvertFromString(string input)
        {
            if (String.IsNullOrWhiteSpace(input))
                throw new ArgumentException("Input cannot be null or whitespace");

            return (T)_converter.ConvertFromString(input);
        }


        bool IConverter<T>.CanConvertFrom(Type sourceType)
        {
            return _converter.CanConvertFrom(sourceType);
        }
    }
}
