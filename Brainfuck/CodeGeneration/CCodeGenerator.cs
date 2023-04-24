using Brainfuck.Parsing;

namespace Brainfuck.CodeGeneration;

public class CCodeGenerator : BaseCodeGenerator, Command.IVisitor<object?>
{
    public override string Generate(List<Command> commands)
    {
        Commands = commands;
        
        // initialise code
        ResultBuilder.AppendLine("""
            #include <stdint.h>
            #include <stdio.h>

            uint8_t input() {
                int key = getchar();
                return (uint8_t)(key != EOF ? key : 0);
            }

            int main() {
                uint8_t cells[30000] = { 0 };
                uint8_t * pointer = &cells[0];
            """);
        Indentations = 1;

        foreach (var command in Commands)
        {
            command.Accept(this);
        }

        return ResultBuilder.ToString();
    }

    public object? VisitInputCommand(Command.Input command)
    {
        Add("*pointer = input();");
        
        return null;
    }

    public object? VisitOutputCommand(Command.Output command)
    {
        Add("putchar(*pointer);");
        
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
        Add($"*pointer += {command.Count};");
        
        return null;
    }

    public object? VisitDecrementCommand(Command.Decrement command)
    {
        Add($"*pointer -= {command.Count};");
        
        return null;
    }

    public object? VisitLoopCommand(Command.Loop loop)
    {
        Add("while (*pointer != 0) {");
        Indentations++;

        foreach (var command in loop.Commands)
            command.Accept(this);

        Indentations--;
        Add("}");

        return null;
    }

    public object? VisitEofCommand(Command.Eof command)
    {
        Indentations = 0;
        Add("}");

        return null;
    }

    public object? VisitToZeroCommand()
    {
        Add("*pointer = 0;");

        return null;
    }
}