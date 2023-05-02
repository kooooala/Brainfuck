namespace Brainfuck.Parsing;

public class Optimizer : BaseScanner<Command, Command>
{
    public Optimizer(List<Command> commands) : base(commands) {}

    public List<Command> Optimize()
    {
        while (!IsAtEnd())
        {
            var current = GetCurrent();
            switch (current)
            {
                case Command.Loop loop:
                    Outputs.AddRange(OptimizeLoop(loop));
                    break;
                default:
                    Outputs.Add(current);
                    break;
            }
            
            Move();
        }

        Outputs.Add(new Command.Eof());
        return Outputs;
    }

    private List<Command> OptimizeLoop(Command.Loop loop)
    {
        var result = new List<Command>();

        if (loop.Commands is [Command.Decrement]) return new List<Command> { new Command.ToZero() };
        if (IsMultiplyLoop(loop)) return OptimizeMultiplyLoop(loop);

        foreach (var command in loop.Commands)
        {
            if (command is Command.Loop loopCommand)
            {
                result.AddRange(OptimizeLoop(loopCommand));
                continue;
            }
            
            result.Add(command);
        }

        return new List<Command> { new Command.Loop(result) };
    }

    private static bool IsMultiplyLoop(Command.Loop loop)
    {
        
        var offset = 0;
        var containsMoves = false;
        var containsDecrement = false;
        
        foreach (var command in loop.Commands)
        {
            switch (command)
            {
                case Command.Left leftCommand:
                    offset += leftCommand.Count;
                    containsMoves = true;
                    break;
                case Command.Right rightCommand:
                    offset -= rightCommand.Count;
                    containsMoves = true;
                    break;
                case Command.Decrement decrementCommand:
                    if (offset == 0)
                        containsDecrement = true;
                    break;
                case Command.Increment:
                    if (offset == 0)
                        containsDecrement = false;
                    break;
                case Command.Loop:
                    return false;
            }
        }

        return offset == 0 && containsMoves && containsDecrement;
    }

    private static List<Command> OptimizeMultiplyLoop(Command.Loop loop)
    {
        var result = new List<Command>();
        var currentOffset = 0;

        var decrementCount = GetDecrementCount(loop.Commands);

        foreach (var command in loop.Commands)
        {
            switch (command)
            {
                case Command.Left leftCommand:
                    currentOffset -= leftCommand.Count;
                    break;
                case Command.Right rightCommand:
                    currentOffset += rightCommand.Count;
                    break;
                case Command.IManipulation manipulationCommand:
                    if (currentOffset != 0)
                    {
                        var count = manipulationCommand.Count / decrementCount;
                        result.Add(new Command.Multiply(currentOffset, manipulationCommand is Command.Increment ? count : -count));
                    }
                    break;
                case Command.ICommandWithOffset commandWithOffset:
                    commandWithOffset.Offset = currentOffset;
                    result.Add((Command)commandWithOffset);
                    break;
            }
        }
        
        result.Add(new Command.ToZero());
        return result;
    }

    private static int GetDecrementCount(List<Command> commands)
    {
        var currentOffset = 0;
        var total = 0;
        
        foreach (var command in commands)
        {
            switch (command)
            {
                case Command.Left leftCommand:
                    currentOffset -= leftCommand.Count;
                    break;
                case Command.Right rightCommand:
                    currentOffset += rightCommand.Count;
                    break;
                case Command.Decrement decrementCommand:
                    if (currentOffset == 0)
                        total += decrementCommand.Count;
                    break;
            }
        }

        return total;
    }

    private bool IsAtEnd() => GetCurrent() is Command.Eof;
}