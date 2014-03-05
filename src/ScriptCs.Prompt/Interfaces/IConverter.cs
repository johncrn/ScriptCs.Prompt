using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptCs.Prompt
{
    public interface IConverter<T>
    {
        T ConvertFromString(string input);

        bool CanConvertFrom(Type sourceType);
    }
}
