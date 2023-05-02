using System.Text;

namespace Brainfuck.Parsing;

public class Interpreter : Command.IVisitor<object?>
{
    private List<byte> _cells = new() { 0 };
    private int _pointer = 0;

    private int _count;

    private List<Command> _commands;

    public void Interpret(List<Command> commands)
    {
        _commands = commands;
        
        for (_count = 0; _count < _commands.Count; _count++)
        {
            _commands[_count].Accept(this);
        }
    }

    public object? VisitInputCommand(Command.Input command)
    {
        if (IsOutOfBound(command.Offset)) 
            AddCells(command.Offset);
            
        _cells[_pointer + command.Offset] = Encoding.Default.GetBytes(Console.ReadKey().KeyChar.ToString())[0];
        
        return null;
    }

    public object? VisitOutputCommand(Command.Output command)
    {
        if (IsOutOfBound(command.Offset)) 
            AddCells(command.Offset);
        
        Console.Write((char)_cells[_pointer + command.Offset]);

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

        if (_pointer - _cells.Count >= 0)
            AddCells(_pointer - _cells.Count);

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
    public object? VisitLoopCommand(Command.Loop loop)
    {
        while (_cells[_pointer] != 0)
        {
            foreach (var command in loop.Commands)
            {
                command.Accept(this);
            }
        }

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

    public object? VisitMultiplyCommand(Command.Multiply command)
    {
        if (IsOutOfBound(command.Offset)) 
            AddCells(command.Offset);

        _cells[_pointer + command.Offset] += (byte)(_cells[_pointer] * command.Count);

        return null;
    }

    private bool IsOutOfBound(int offset) => offset > 0 && _pointer + offset >= _cells.Count;
    
    private void AddCells(int offset)
    {   
        var originalCount = _cells.Count;
        for (var i = 0; i <= _pointer + offset - originalCount + 1; i++)
            _cells.Add(0);
    }
}