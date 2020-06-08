using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandLineInterpreter
{
    class Command
    {
        public string Name { get; }
        public string ShortHelp { get; }
        public string FullHelp { get; }
        Action<string[]> Action;
        public Command(string name, Action<string[]> action, string shortHelp, string fullHelp)
        {
            Name = name;
            Action = action;
            ShortHelp = shortHelp;
            FullHelp = fullHelp;
        }

        public void Run(string[] args) 
        {
            Action.Invoke(args);
        }
    }
}
