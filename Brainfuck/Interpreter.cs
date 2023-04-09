using System.Runtime.InteropServices.JavaScript;
using System.Text;

namespace Brainfuck;

public class Interpreter
{
    private List<byte> _cells { get; set; } = new() { 0 };
    private int _index = 0;

    public void Execute(string code)
    {
        for (int i = 0; i < code.Length; i++)
        {
            var c = code[i];
            switch (c)
            {
                case '>':
                    _index++;
                    if (_index >= _cells.Count) _cells.Add(0);
                    break;
                case '<':
                    if (_index - 1 < 0)
                    {
                        Error("pointer out of bound", i + 1);
                        return;
                    }
                    _index--;
                    break;
                case '+':
                    _cells[_index]++;
                    break;
                case '-':
                    _cells[_index]--;
                    break;
                case '.':
                    Console.Write((char)_cells[_index]);
                    break;
                case ',':
                    Console.Write('\n');
                    _cells[_index] = Encoding.Default.GetBytes(Console.ReadKey().KeyChar.ToString())[0];
                    Console.Write('\n');
                    break;
                case '[':
                    if (_cells[_index] == 0)
                    {
                        int loops = 1;
                        char currentChar;

                        while (loops > 0)
                        {
                            i++;
                            if (i >= code.Length)
                            {
                                Error("missing ']'", null);
                                return;
                            }
                                
                            currentChar = code[i];
                                
                            if (currentChar == '[') loops++;
                            else if (currentChar == ']') loops--;
                        }
                    }
                    
                    break;
                case ']':
                    if (_cells[_index] != 0)
                    {
                        int loops = 1;
                        char currentChar;

                        while (loops > 0)
                        {
                            i--;
                            if (i < 0)
                            {
                                Error("missing '['", null);
                                return;
                            }
                                
                            currentChar = code[i];

                            if (currentChar == '[') loops--;
                            else if (currentChar == ']') loops++;
                        }
                    }

                    break;

                default:
                    continue;
            }
        }
    }

    private static void Error(string message, int? index)
    {
        if (index is not null)
            Console.WriteLine($"Error occured at char {index}: {message}");
        else 
            Console.WriteLine($"Error occured: {message}");
    }
}