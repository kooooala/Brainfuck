using System.Text;
using Brainfuck.Parsing;

namespace Brainfuck.CodeGeneration;

public abstract class BaseCodeGenerator
{
    protected readonly List<Command> Commands;
    protected StringBuilder ResultBuilder;
    protected int Indentations = 0;
    
    protected BaseCodeGenerator()
    {
        ResultBuilder = new();
    }

    public abstract string Generate(List<Command> commands);
 
    protected virtual void Add(string code)
    {
        ResultBuilder.AppendLine(Indent() + code);
    }

    protected virtual string Indent()
    {
        string result = string.Empty;
        for (int i = 0; i < Indentations; i++)
            result += "    ";
        return result;
    }
}