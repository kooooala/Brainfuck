// Generated on 16/04/2023 19:56:47

using System.Text;

namespace Brainfuck.Parsing; 

public abstract class Command
{
    public interface IVisitor<T> {
        public T VisitInputCommand(Input command);
        public T VisitOutputCommand(Output command);
        public T VisitLeftCommand(Left command);
        public T VisitRightCommand(Right command);
        public T VisitIncrementCommand(Increment command);
        public T VisitDecrementCommand(Decrement command);
        public T VisitLoopCommand(Loop loop);
        public T VisitEofCommand(Eof command);

        public T VisitToZeroCommand();

        public T VisitMultiplyCommand(Multiply command);
    }

    public interface ICommandWithOffset
    {
        public int Offset { get; set; }
    }
    
    public interface IManipulation
    {
        public int Count { get; }
        public int Offset { get; }
    }

    public abstract T Accept<T>(IVisitor<T> visitor);
    public class Input: Command, ICommandWithOffset {
        public override T Accept<T>(IVisitor<T> visitor) {
            return visitor.VisitInputCommand(this);
        }

        public Input()
        {
            Offset = 0;
        }
        
        public Input(int offset)
        {
            Offset = offset;
        }
        
        public int Offset { get; set; }
        
        public override string ToString() => "input() ";
    }

    public class Output: Command, ICommandWithOffset {
        public override T Accept<T>(IVisitor<T> visitor) {
            return visitor.VisitOutputCommand(this);
        }
        
        public Output()
        {
            Offset = 0;
        }
        
        public Output(int offset)
        {
            Offset = offset;
        }
        
        public int Offset { get; set; }

        public override string ToString() => "print() ";
    }

    public class Left: Command {
        public Left(int Count) {
            this.Count = Count;
        }

        public override T Accept<T>(IVisitor<T> visitor) {
            return visitor.VisitLeftCommand(this);
        }

        public override string ToString() => $"p-={Count} ";

        public readonly int Count;
    }

    public class Right: Command {
        public Right(int Count) {
            this.Count = Count;
        }

        public override T Accept<T>(IVisitor<T> visitor) {
            return visitor.VisitRightCommand(this);
        }

        public override string ToString() => $"p+={Count} ";

        public readonly int Count;
    }

    public class Increment: Command, IManipulation {
        public Increment(int count) {
            Count = count;
        }

        public Increment(int count, int offset)
        {
            Count = count;
            Offset = offset;
        }

        public override T Accept<T>(IVisitor<T> visitor) {
            return visitor.VisitIncrementCommand(this);
        }

        public override string ToString() => $"*p+={Count} ";

        public int Count { get; }
        public int Offset { get; }
    }

    public class Decrement: Command, IManipulation {
        public Decrement(int count) {
            Count = count;
        }

        public Decrement(int count, int offset)
        {
            Count = count;
            Offset = offset;
        }

        public override T Accept<T>(IVisitor<T> visitor) {
            return visitor.VisitDecrementCommand(this);
        }

        public override string ToString() => $"*p-={Count} ";

        public int Count { get; }
        public int Offset { get; }
    }

    public class Loop : Command {
        public Loop(List<Command> commands)
        {
            Commands = commands;
        }
        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitLoopCommand(this);
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            foreach (var command in Commands)
            {
                builder.Append(command);
            }

            return builder.ToString();
        }

        public readonly List<Command> Commands;
    }

    public class Eof : Command
    {
        public override T Accept<T>(IVisitor<T> visitor) {
            return visitor.VisitEofCommand(this);
        }
    }

    public class ToZero : Command {
        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitToZeroCommand();
        }

        public override string ToString() => "zero() ";
    }

    public class Multiply : Command, ICommandWithOffset
    {
        public override T Accept<T>(IVisitor<T> visitor)
        {
            return visitor.VisitMultiplyCommand(this);
        }

        public Multiply(int offset, int count)
        {
            Offset = offset;
            Count = count;
        }

        public override string ToString()
        {
            return $"mul({Offset}, {Count}) ";
        }

        public int Offset { get; set; }
        public readonly int Count;
    }
}
