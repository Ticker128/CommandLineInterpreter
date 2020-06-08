using System;
using System.Diagnostics;
using System.IO;

namespace CommandLineInterpreter
{
    class Interpreter
    {
        private CommandManager commandManager = new CommandManager();
        private LineReader Reader = new LineReader();
        public Interpreter()
        {
            commandManager.AddCommand("cd",CD,"Переход в другой каталог", "Переход в другой каталог");
            commandManager.AddCommand("dir", DIR, "Отобразить содержимое папки", "Отобразить содержимое папки");
            commandManager.AddCommand("cls", CLEAR, "Очистка экрана", "Очистка экрана");
            commandManager.AddCommand("rename", RENAME, "Переименовать файл", "Переименовать файл");
            commandManager.AddCommand("renamedir", RENAMEDIR, "Переименовать папку", "Переименовать папку");
            commandManager.AddCommand("run", RUN, "Запуск файла", "Запуск файла или открыть папку");
            commandManager.AddCommand("help", HELP, "Вызов справки", "Вызов справки");
            commandManager.AddCommand("exit", EXIT, "Выход из программы", "Выход из программы");
        }
        public void Interpret()
        {
            string[] line = Reader.ReadLine().Trim().Split(new char[] {' '}, 2, StringSplitOptions.RemoveEmptyEntries);
            if(line.Length != 0)
                commandManager.RunCommand(line[0], line);
        }

        private void CD(string[] args)
        {
            if(args.Length <= 3)
            {
                if (args.Length == 1)
                    Console.WriteLine(Directory.GetCurrentDirectory());
                else if (args[1] == "..")
                    Directory.SetCurrentDirectory(Directory.GetParent(Directory.GetCurrentDirectory()).FullName);
                else if (args[1] == "\\")
                    Directory.SetCurrentDirectory(Directory.GetDirectoryRoot(Directory.GetCurrentDirectory()));
                else if (Directory.Exists(Directory.GetCurrentDirectory() + "\\" + args[1]))
                {
                    Directory.SetCurrentDirectory(Directory.GetCurrentDirectory() + "\\" + args[1]);
                }
                else if (args[1].StartsWith("/D"))
                {
                    string[] temp = args[1].Trim().Split(new char[]{' '}, 2, StringSplitOptions.RemoveEmptyEntries);
                    foreach (DriveInfo i in DriveInfo.GetDrives())
                    {
                        if (i.Name.ToLower()[0] == temp[1].Trim().ToLower()[0])
                        {
                            Directory.SetCurrentDirectory(i.Name);
                            break;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Путь не найден");
                }
            }
            else
            {
                Console.WriteLine("Неверно указаны параметры");
            }
        }
        private void DIR(string[] args)
        {
            DirectoryInfo directory = new DirectoryInfo(Directory.GetCurrentDirectory());
            if(args.Length == 1)
            {
                Console.WriteLine();
                Console.WriteLine("Directories:");
                foreach (DirectoryInfo i in directory.GetDirectories())
                    Console.WriteLine($"{" ", 2} {i.Name, -10}");

                Console.WriteLine();
                Console.WriteLine("Files:");
                foreach (FileInfo i in directory.GetFiles())
                    Console.WriteLine($"{" ", 2} {i.Name, -10}");
            }
        }
        private void CLEAR(string[] args)
        {
            Console.Clear();
        }
        private void RENAME(string[] args)
        {
            string[] temp = args[1].Trim().Split(new char[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);
            temp[1] = temp[1].Trim();
            if (File.Exists(temp[0]) && !File.Exists(temp[1]))
            {
                File.Move(temp[0], temp[1]);
            }
            else
            {
                Console.WriteLine("Не найден указанный файл или хз");
            }
        }
        private void RENAMEDIR(string[] args)
        {
            string[] temp = args[1].Trim().Split(new char[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);
            temp[1] = temp[1].Trim();
            if (Directory.Exists(temp[0]) && !Directory.Exists(temp[1]))
            {
                Directory.Move(temp[0], temp[1]);
            }
            else
            {
                Console.WriteLine("Не найден указанный файл или хз");
            }
        }
        private void RUN(string[] args)
        {
            if (args.Length == 2)
            {
                Process.Start(args[1]);
            }
        }
        private void EXIT(string[] args)
        {
            Environment.Exit(0);
        }
        private void HELP(string[] args)
        {
            if (args.Length == 2)
            {
                args[1] = args[1].Trim();
                foreach(string i in commandManager.list.Keys)
                {
                    if(args[1] == i)
                    {
                        Console.WriteLine(commandManager.list[i].FullHelp);
                        break;
                    }
                }
            }
            else if(args.Length == 1)
            {
                foreach (string i in commandManager.list.Keys)
                {
                   Console.WriteLine($"{i, -15} {commandManager.list[i].ShortHelp}");
                }
            }
        }
    }
}
