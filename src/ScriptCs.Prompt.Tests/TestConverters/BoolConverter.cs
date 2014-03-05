using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptCs.Prompt.Tests
{
    public class BoolConverter : IConverter<bool>
    {
        public bool ConvertFromString(string input)
        {
            bool output;
            if (!Boolean.TryParse(input, out output))
            {
                var massagedInput = input.Trim().ToUpperInvariant();
                switch (massagedInput)
                {
                    case "Y":
                    case "YES":
                        output = true;
                        break;
                    case "N":
                    case "NO":
                        output = false;
                        break;
                }
            }

            return output;
        }

        public bool CanConvertFrom(Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;
            else
                return false;
        }
    }
}
