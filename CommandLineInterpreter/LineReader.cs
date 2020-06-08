using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CommandLineInterpreter
{
    class LineReader
    {
        private StringBuilder Line = new StringBuilder();

        private int StartPosition { get; set; }
        private int IndexInWord { get; set; }

        private List<string> Dictionary = new List<string>();
        private int IndexInDictionary { get; set; }
        
        private List<string> AutoComplete = new List<string>();
        private int IndexInAutoComplete { get; set; }

        public string ReadLine()
        {
            Line.Clear();
            StartPosition = Console.CursorLeft;
            IndexInWord = 0;
            ConsoleKeyInfo ch;
            IndexInAutoComplete = 0;
            AutoComplete.Clear();
            foreach (string i in Directory.GetFileSystemEntries(Directory.GetCurrentDirectory()))
            {
                
                AutoComplete.Add(i.Substring(i.LastIndexOf('\\') + 1));
            }

            while ((ch = Console.ReadKey(true)).Key != ConsoleKey.Enter)
            {
                switch (ch.Key)
                {
                    case ConsoleKey.Backspace:
                        Backspace();
                        break;
                    case ConsoleKey.Delete:
                        Delete();
                        break;
                    case ConsoleKey.Tab:
                        Tab();
                        break;
                    case ConsoleKey.LeftArrow:
                        MoveToLeft();
                        break;
                    case ConsoleKey.RightArrow:
                        MoveToRight();
                        break;
                    case ConsoleKey.UpArrow:
                        NextWord();
                        break;
                    case ConsoleKey.DownArrow:
                        PreviewWord();
                        break;
                    default:
                        AddToString(ch.KeyChar);
                        break;
                }
            }
            Dictionary.Add(Line.ToString());
            IndexInDictionary = Dictionary.Count == 0 ? 0: Dictionary.Count;
            Console.WriteLine();
            return Line.ToString() ;
        }
        private void NextWord()
        {
            IndexInAutoComplete = 0;
            if (IndexInDictionary > 0 && Dictionary.Count != 0)
            {
                IndexInDictionary--;
                Console.CursorLeft = StartPosition;
                Console.Write(new string(' ', Line.Length));
                Console.CursorLeft = StartPosition;
                Line.Clear();
                Line.Append(Dictionary[IndexInDictionary]);
                IndexInWord = Line.ToString().Length;
                Console.Write(Line.ToString());
                
            }
        }
        private void PreviewWord()
        {
            IndexInAutoComplete = 0;
            if (IndexInDictionary < Dictionary.Count - 1)
            {
                IndexInDictionary++;
                Console.CursorLeft = StartPosition;
                Console.Write(new string(' ', Line.Length));
                Console.CursorLeft = StartPosition;
                Line.Clear();
                Line.Append(Dictionary[IndexInDictionary]);
                IndexInWord = Line.ToString().Length;
                Console.Write(Line.ToString());
            }
        }
        private void MoveToLeft()
        {
            IndexInAutoComplete = 0;
            if (IndexInWord != 0)
            {
                IndexInWord--;
                Console.CursorLeft--;
            }
        }
        private void MoveToRight()
        {
            IndexInAutoComplete = 0;
            if (IndexInWord != Line.Length)
            {
                IndexInWord++;
                Console.CursorLeft++;
            }
        }
        private void AddToString(char ch)
        {
            IndexInAutoComplete = 0;
            Line.Insert(IndexInWord, ch);
            Console.Write(Line.ToString().Substring(IndexInWord));
            IndexInWord++;
            Console.CursorLeft = StartPosition + IndexInWord;
        }
        private void Delete()
        {
            IndexInAutoComplete = 0;
            if (IndexInWord != Line.Length)
            {
                Line.Remove(IndexInWord, 1);
                Console.Write(Line.ToString().Substring(IndexInWord));
                Console.Write(" ");
                Console.CursorLeft = StartPosition + IndexInWord;
            }
        }
        private void Backspace()
        {
            IndexInAutoComplete = 0;
            if (IndexInWord != 0)
            {
                IndexInWord--;
                Line.Remove(IndexInWord, 1);
                Console.CursorLeft = StartPosition + IndexInWord;
                Console.Write(Line.ToString().Substring(IndexInWord));
                Console.Write(" ");
                Console.CursorLeft = StartPosition + IndexInWord;
            }
        }
        private void Tab()
        {
            if(IndexInAutoComplete < AutoComplete.Count)
            {
                if(IndexInWord == Line.Length)
                {
                    Line.Replace(AutoComplete[IndexInAutoComplete == 0 ? AutoComplete.Count - 1 : IndexInAutoComplete - 1], "");
                    IndexInWord = Line.Length;
                    Line.Append(AutoComplete[IndexInAutoComplete]);
                    IndexInWord = Line.Length;
                    Console.CursorLeft = StartPosition;
                    Console.Write(new string(' ', AutoComplete[IndexInAutoComplete == 0 ? AutoComplete.Count - 1 : IndexInAutoComplete - 1].Length + 10));
                    Console.CursorLeft = StartPosition;
                    Console.Write(Line);
                    IndexInAutoComplete = (IndexInAutoComplete + 1) % AutoComplete.Count;
                }
            }
        }
    }
}
