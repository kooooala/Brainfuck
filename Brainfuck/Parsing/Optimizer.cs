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
                    if (loop.Commands is [Command.Decrement])
                    {
                        Outputs.Add(new Command.ToZero());
                        break;
                    }
                    Outputs.Add(current);
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

    private bool IsAtEnd() => GetCurrent() is Command.Eof;
}