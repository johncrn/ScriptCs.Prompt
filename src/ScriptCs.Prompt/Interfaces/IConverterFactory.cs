using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptCs.Prompt
{
    public interface IConverterFactory
    {
        IConverter<T> GetConverter<T>();
    }
}
