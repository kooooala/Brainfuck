// See https://aka.ms/new-console-template for more information

namespace Brainfuck;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            var interpreter = new Interpreter();
            while (true)
            {
                Console.Write("> ");
                var code = Console.ReadLine();
                if (code == null) continue;
                
                interpreter.Execute(code);
            }
        }
        else if (args.Length == 1)
        {
            var code = File.ReadAllText(args[0]);
            var interpreter = new Interpreter();
            
            interpreter.Execute(code);
        }
        else Console.WriteLine("Usage: brainfuck [file]");
    }
}