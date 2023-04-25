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

    private bool IsAtEnd() => GetCurrent() is Command.Eof;
}