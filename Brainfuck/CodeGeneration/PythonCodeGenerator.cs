using Brainfuck.Parsing;

namespace Brainfuck.CodeGeneration;

public class PythonCodeGenerator : BaseCodeGenerator, Command.IVisitor<object?>
{
    public override string Generate(List<Command> commands)
    {
        ResultBuilder.AppendLine("""
            import sys

            def readInput():
                return sys.stdin.read(1)

            def to_byte(num: int):
                if (num < 0):
                    return 256 + num

                return num % 256
            
            cells = bytearray([0] * 30000)
            pointer = 0
            """);
        
        foreach (var command in commands)
        {
            command.Accept(this);
        }

        return ResultBuilder.ToString();
    }

    public object? VisitInputCommand(Command.Input command)
    {
        Add("cells[pointer] = readInput()");

        return null;
    }

    public object? VisitOutputCommand(Command.Output command)
    {
        Add("sys.stdout.write(chr(cells[pointer]))");

        return null;
    }

    public object? VisitLeftCommand(Command.Left command)
    {
        Add($"pointer -= {command.Count}");

        return null;
    }

    public object? VisitRightCommand(Command.Right command)
    {
        Add($"pointer += {command.Count}");

        return null;
    }

    public object? VisitIncrementCommand(Command.Increment command)
    {
        Add($"cells[pointer] = to_byte(cells[pointer] + {command.Count})");

        return null;
    }

    public object? VisitDecrementCommand(Command.Decrement command)
    {
        Add($"cells[pointer] = to_byte(cells[pointer] - {command.Count})");

        return null;
    }

    public object? VisitLeftParenCommand(Command.LeftParen command)
    {
        Add("while (cells[pointer] != 0):");
        Indentations++;

        return null;
    }

    public object? VisitRightParenCommand(Command.RightParen command)
    {
        Indentations--;

        return null;
    }

    public object? VisitToZeroCommand()
    {
        Add("cells[pointer] = 0");

        return null;
    }

    public object? VisitEofCommand(Command.Eof command)
    {
        return null;
    }
}