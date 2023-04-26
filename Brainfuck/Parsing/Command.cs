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
    }

    public abstract T Accept<T>(IVisitor<T> visitor);
    public class Input: Command {
        public override T Accept<T>(IVisitor<T> visitor) {
            return visitor.VisitInputCommand(this);
        }

        public override string ToString() => ",";
    }

    public class Output: Command {
        public override T Accept<T>(IVisitor<T> visitor) {
            return visitor.VisitOutputCommand(this);
        }

        public override string ToString() => ".";
    }

    public class Left: Command {
        public Left(int Count) {
            this.Count = Count;
        }

        public override T Accept<T>(IVisitor<T> visitor) {
            return visitor.VisitLeftCommand(this);
        }

        public override string ToString() => $"<{Count}";

        public readonly int Count;
    }

    public class Right: Command {
        public Right(int Count) {
            this.Count = Count;
        }

        public override T Accept<T>(IVisitor<T> visitor) {
            return visitor.VisitRightCommand(this);
        }

        public override string ToString() => $">{Count}";

        public readonly int Count;
    }

    public class Increment: Command {
        public Increment(int Count) {
            this.Count = Count;
        }

        public override T Accept<T>(IVisitor<T> visitor) {
            return visitor.VisitIncrementCommand(this);
        }

        public override string ToString() => $"+{Count}";

        public readonly int Count;
    }

    public class Decrement: Command {
        public Decrement(int Count) {
            this.Count = Count;
        }

        public override T Accept<T>(IVisitor<T> visitor) {
            return visitor.VisitDecrementCommand(this);
        }

        public override string ToString() => $"-{Count}";

        public readonly int Count;
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


        public override string ToString() => "0";
    }
}
