using System.Text;

namespace Brainfuck.Parsing;

public class Interpreter : Command.IVisitor<object?>
{
    private List<byte> _cells = new() { 0 };
    private int _pointer = 0;

    private int _count;

    private List<Command> _commands;
    private Dictionary<int, int> _bracketMap;

    public void Interpret(List<Command> commands, Dictionary<int, int> bracketMap)
    {
        _commands = commands;
        _bracketMap = bracketMap;
        
        for (_count = 0; _count < _commands.Count; _count++)
        {
            _commands[_count].Accept(this);
        }
    }

    public object? VisitInputCommand(Command.Input command)
    {
        _cells[_pointer] = Encoding.Default.GetBytes(Console.ReadKey().KeyChar.ToString())[0];

        return null;
    }

    public object? VisitOutputCommand(Command.Output command)
    {
        Console.Write((char)_cells[_pointer]);

        return null;
    }

    public object? VisitLeftCommand(Command.Left command)
    {
        _pointer -= (byte)command.Count;
        if (_pointer < 0) throw new Exception("Pointer out of bound");

        return null;
    }

    public object? VisitRightCommand(Command.Right command)
    {
        _pointer += (byte)command.Count;

        var originalCount = _cells.Count;
        for (var i = 0; i <= _pointer - originalCount; i++)
            _cells.Add(0);

        return null;
    }

    public object? VisitIncrementCommand(Command.Increment command)
    {
        _cells[_pointer] += (byte)command.Count;

        return null;
    }

    public object? VisitDecrementCommand(Command.Decrement command)
    {
        _cells[_pointer] -= (byte)command.Count;

        return null;
    }

    public object? VisitLeftParenCommand(Command.LeftParen command)
    {
        if (_cells[_pointer] == 0)
            _count = _bracketMap[_count];

        return null;
    }

    public object? VisitRightParenCommand(Command.RightParen command)
    {
        if (_cells[_pointer] != 0)
            _count = _bracketMap[_count];

        return null;
    }

    public object? VisitToZeroCommand()
    {
        _cells[_pointer] = 0;

        return null;
    }

    public object? VisitEofCommand(Command.Eof command)
    {
        return null;
    }
}