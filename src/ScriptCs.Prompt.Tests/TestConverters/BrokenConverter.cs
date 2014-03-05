using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptCs.Prompt.Tests
{
    class BrokenConverter : IConverter<int>
    {
        public int ConvertFromString(string input)
        {
            return Convert.ToInt32(input);
        }

        public bool CanConvertFrom(Type sourceType)
        {
            return false;
        }
    }
}
