using Brainfuck.Parsing;

namespace Brainfuck.CodeGeneration;

public class PythonCodeGenerator : BaseCodeGenerator, Command.IVisitor<object?>
{
    public override string Generate(List<Command> commands)
    {
        ResultBuilder.AppendLine("""
            import sys

            def readInput():
                return sys.stdin.buffer.read(1)[0]

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
        if (command.Offset != 0)
        {
            Add($"cells[pointer + {command.Offset}] = to_byte(cells[pointer + {command.Offset}])");
            return null;
        }
        
        Add($"cells[pointer] = to_byte(cells[pointer] + {command.Count})");

        return null;
    }

    public object? VisitDecrementCommand(Command.Decrement command)
    {
        if (command.Offset != 0)
        {
            Add($"cells[pointer + {command.Offset}] = to_byte(cells[pointer + {command.Offset}] - {command.Count})");
        }
        
        Add($"cells[pointer] = to_byte(cells[pointer] - {command.Count})");

        return null;
    }

    public object? VisitLoopCommand(Command.Loop loop)
    {
        Add("while (cells[pointer] != 0):");
        Indentations++;
        
        foreach (var command in loop.Commands)
        {
            command.Accept(this);
        }

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

    public object? VisitMultiplyCommand(Command.Multiply command)
    {
        Add($"cells[pointer + {command.Offset}] += cells[pointer] * {command.Count}");

        return null;
    }
}