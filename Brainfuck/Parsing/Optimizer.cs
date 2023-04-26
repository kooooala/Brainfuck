using System.ComponentModel.Design;

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
                    Outputs.Add(OptimizeLoop(loop));
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

    private Command OptimizeLoop(Command.Loop loop)
    {
        var result = new List<Command>();

        if (loop.Commands is [Command.Decrement]) return new Command.ToZero();

        if (IsCopyLoop(loop)) return OptimizeCopyLoop(loop);

        foreach (var command in loop.Commands)
        {
            if (command is Command.Loop loopCommand)
            {
                result.Add(OptimizeLoop(loopCommand));
                continue;
            }
            
            result.Add(command);
        }

        return new Command.Loop(result);
    }

    private static bool IsCopyLoop(Command.Loop loop)
    {
        int leftCount = 0, rightCount = 0;
        foreach (var command in loop.Commands)
        {
            switch (command)
            {
                case Command.Left leftCommand:
                    leftCount += leftCommand.Count;
                    break;
                case Command.Right rightCommand:
                    rightCount += rightCommand.Count;
                    break;
                case Command.Loop:
                    return false;
            }
        }

        return leftCount == rightCount && leftCount + rightCount != 0;
    }

    private static Command.Loop OptimizeCopyLoop(Command.Loop loop)
    {
        var result = new List<Command>();
        var currentOffset = 0;
        
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
                case Command.Increment incrementCommand:
                    result.Add(new Command.Increment(incrementCommand.Count, currentOffset));
                    break;
                case Command.Decrement decrementCommand:
                    result.Add(new Command.Decrement(decrementCommand.Count, currentOffset));
                    break;
                default:
                    result.Add(command);
                    break;
            }
        }

        return new Command.Loop(result);
    }

    private bool IsAtEnd() => GetCurrent() is Command.Eof;
}