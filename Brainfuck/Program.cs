using System.Diagnostics;
using Brainfuck.Tokenization;
using Brainfuck.CodeGeneration;
using Brainfuck.Parsing;

namespace Brainfuck;

class Program
{
    // TODO: optimise IR

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

    private static void RunFromFile(string[] args)
    {
        var sourceFile = args[0];

        BaseCodeGenerator generator = args.Contains("-l")
            ? args[Array.FindIndex(args, arg => arg == "-l") + 1].ToLower() switch
            {
                "c" => new CCodeGenerator(),
                "py" or "python" => new PythonCodeGenerator(),
                "c#" or "csharp" or "cs" => new CSharpCodeGenerator(),
                _ => throw new Exception("Unknown language")
            }
            : new CCodeGenerator();
        var outputFile = args.Contains("-o")
            ? args[Array.FindIndex(args, arg => arg == "-o") + 1]
            : ExtractFileName(sourceFile) + generator switch
            {
                CCodeGenerator => ".c", 
                PythonCodeGenerator => ".py",
                CSharpCodeGenerator => ".cs",
            };

        string ExtractFileName(string source)
        {
            var result = source.Split(source.Contains('/') ? '/' : '\\')[^1]; 
            return result.Split('.')[0];
        }

        var sourceCode = File.ReadAllText(sourceFile);

        var lexer = new Lexer(sourceCode);
        var parser = new Parser(lexer.Scan());

        if (args.Contains("-r"))
        {
            new Interpreter().Interpret(parser.Parse(), parser.Brackets);
            return;
        }

        File.WriteAllText(outputFile, generator.Generate(parser.Parse()));
    }

    private static void ShowHelp(string[] args)
    {
        Console.WriteLine("""
            Usage: bf [<source>] [-l <language>] [-o <file>] [-r]
            
            Options: 
                -l <language>       Specify the target language
                -o <file>           Place the output into <file>
                -r                  Run file using the built-in BF interpreter
                
            Arguments: 
                <source>            Path to the source file
                <language>          Target language
                <file>              Output file
    
            Supported languages:    
                c                   C
                py | python         Python
                c# | cs | CSharp    C#
            """);
    }
}