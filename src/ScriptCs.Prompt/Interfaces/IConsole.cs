using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptCs.Prompt
{
    public interface IConsole
    {
        void WriteLine(string value);

        void WriteLine(string format, params object[] args);

        void WriteLine();

        void Write(string value);

        void Write(string format, params object[] args);

        string ReadLine();
    }
}
