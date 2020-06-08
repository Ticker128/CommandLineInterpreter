using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandLineInterpreter
{
    class CommandManager
    {
        public Dictionary<string, Command> list = new Dictionary<string, Command>();

        public void AddCommand(string name, Action<string[]> action, string shortHelp, string fullHelp)
        {
            list.Add(name, new Command(name, action, shortHelp, fullHelp));
        }
        public void RemoveCommand(string name)
        {
            list.Remove(name);
        }
        public void RunCommand(string name,string[] args)
        {
            if (list.ContainsKey(name))
                list[name].Run(args);
            else
                Console.WriteLine("Введена неверная команда");
        }
    }
}
