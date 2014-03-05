using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptCs.Prompt
{
    internal class SystemConverterFactory : IConverterFactory
    {
        IConverter<T> IConverterFactory.GetConverter<T>()
        {
            return new SystemConverter<T>();
        }
    }
}
