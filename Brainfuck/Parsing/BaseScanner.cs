namespace Brainfuck.Parsing;

public abstract class BaseScanner<TInput, TOutput>
{
    protected List<TInput> Inputs;
    protected List<TOutput> Outputs;
    protected int CurrentLocation = 0;
    
    protected BaseScanner(List<TInput> inputs)
    {
        Inputs = inputs;
        Outputs = new();
    }
    
    protected virtual TInput GetCurrent() => Inputs[CurrentLocation];
    
    protected virtual void Move()
    {
        CurrentLocation++;
    }
    
    protected virtual void Move(int count)
    {
        CurrentLocation += count;
    }

    protected virtual TInput Next() => Inputs[CurrentLocation + 1];

    protected virtual TInput Next(int count) => Inputs[CurrentLocation + count];
}