using Brainfuck.Parsing;
using Brainfuck.CodeGeneration;

namespace Brainfuck.CodeGeneration;

public class CSharpCodeGenerator : BaseCodeGenerator, Command.IVisitor<object?>
{
    public override string Generate(List<Command> commands)
    {
        Commands = commands;

        ResultBuilder.AppendLine("""
            using System;

            class Program
            {
                private static int[] cells = new int[30000];
                private static int pointer = 0;
                
                static void Read()
                {
                    cells[pointer] = System.Text.Encoding.Default.GetBytes(Console.ReadKey().KeyChar.ToString())[0];
                }
        
                static void Write()
                {
                    Console.Write((char)cells[pointer]);
                }
        
                static void Main(string[] args)
                {
            """);
        Indentations = 2;

        foreach (var command in Commands)
        {
            command.Accept(this);
        }

        return ResultBuilder.ToString();
    }

    public object? VisitInputCommand(Command.Input command)
    {
        Add("Read();");

        return null;
    }

    public object? VisitOutputCommand(Command.Output command)
    {
        Add("Write();");

        return null;
    }

    public object? VisitLeftCommand(Command.Left command)
    {
        Add($"pointer -= {command.Count};");

        return null;
    }

    public object? VisitRightCommand(Command.Right command)
    {
        Add($"pointer += {command.Count};");
        
        return null;
    }

    public object? VisitIncrementCommand(Command.Increment command)
    {
        Add($"cells[pointer] += {command.Count};");

        return null;
    }

    public object? VisitDecrementCommand(Command.Decrement command)
    {
        Add($"cells[pointer] -= {command.Count};");

        return null;
    }

    public object? VisitLoopCommand(Command.Loop loop)
    {
        Add("while (cells[pointer] != 0) {");
        Indentations++;

        foreach (var command in loop.Commands)
            command.Accept(this);

        Indentations--;
        Add("}");

        return null;
    }

    public object? VisitToZeroCommand()
    {
        Add("cells[pointer] = 0;");

        return null;
    }

    public object? VisitEofCommand(Command.Eof command)
    {
        Indentations--;
        Add("}");
        Indentations--;
        Add("}");

        return null;
    }
}

