using System.Diagnostics;
using Brainfuck.Tokenization;
using Brainfuck.CodeGeneration;
using Brainfuck.Parsing;

namespace Brainfuck;

class Program
{
    // TODO: support for Javascript & C#
    // TODO: more optimised IR

    static void Main(string[] args)
    {
        Action<string[]> run;

        run = args switch
        {
            [] => RunRepl,
            [_, ..] or [_] => RunFromFile,
            _ => ShowHelp
        };

        try
        {
            run(args);
        }
        catch (FileNotFoundException exception)
        {
            Console.WriteLine($"Unable to find file {args[0]}");
            ShowHelp(args);
        }
    }

    static void RunRepl(string[] args)
    {
        Console.Write("""
                Brainfuck interactive console by kooooala
                https://github.com/kooooala/brainfuck

                Type quit to exit
                >
                """);
        var userInput = string.Empty;
        var interpreter = new Interpreter();

        do
        {
            try
            {
                userInput = Console.ReadLine();

                if (userInput == null) continue;
                
                var lexer = new Lexer(userInput);
                var parser = new Parser(lexer.Scan());
            
                interpreter.Interpret(parser.Parse(), parser.Brackets);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        } while (userInput != "quit");
        
        Environment.Exit(0);
    }

    static void RunFromFile(string[] args)
    {
        var sourceFile = args[0];
        BaseCodeGenerator generator = args.Contains("-l")
            ? args[Array.FindIndex(args, arg => arg == "-l") + 1].ToLower() switch
            {
                "c" => new CCodeGenerator(),
                "py" or "python" => new PythonCodeGenerator(),
                _ => throw new Exception("Unknown language")
            }
            : new CCodeGenerator();
        var outputFile = args.Contains("-o")
            ? args[Array.FindIndex(args, arg => arg == "-o") + 1]
            : sourceFile + generator switch
            {
                CCodeGenerator => ".c", 
                PythonCodeGenerator => ".py"
            };

        var souceCode = File.ReadAllText(sourceFile);

        var lexer = new Lexer(souceCode);
        var parser = new Parser(lexer.Scan());

        File.WriteAllText(outputFile, generator.Generate(parser.Parse()));
    }

    static void ShowHelp(string[] args)
    {
        Console.WriteLine("""
            Usage: bf [<source>] [-l <language>] [-o <file>] [-r]
            
            Options: 
                -l <language>   Specify the target language
                -o <file>       Place the output into <file>
            
            Arguments:
                <source>        Path to the source file
                <language>      Target language
                <file>          Output file

            Supported languages:
                c               C
                py | python     Python
            """);
    }
}