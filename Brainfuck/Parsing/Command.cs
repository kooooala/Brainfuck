// Generated on 16/04/2023 19:56:47

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
        public T VisitLeftParenCommand(LeftParen command);
        public T VisitRightParenCommand(RightParen command);
        public T VisitEofCommand(Eof command);

        public T VisitToZeroCommand();
    }

    public abstract T Accept<T>(IVisitor<T> visitor);
    public class Input: Command {
        public override T Accept<T>(IVisitor<T> visitor) {
            return visitor.VisitInputCommand(this);
        }
    }

    public class Output: Command {
        public override T Accept<T>(IVisitor<T> visitor) {
            return visitor.VisitOutputCommand(this);
        }
    }

    public class Left: Command {
        public Left(int Count) {
            this.Count = Count;
        }

        public override T Accept<T>(IVisitor<T> visitor) {
            return visitor.VisitLeftCommand(this);
        }

        public readonly int Count;
    }

    public class Right: Command {
        public Right(int Count) {
            this.Count = Count;
        }

        public override T Accept<T>(IVisitor<T> visitor) {
            return visitor.VisitRightCommand(this);
        }

        public readonly int Count;
    }

    public class Increment: Command {
        public Increment(int Count) {
            this.Count = Count;
        }

        public override T Accept<T>(IVisitor<T> visitor) {
            return visitor.VisitIncrementCommand(this);
        }

        public readonly int Count;
    }

    public class Decrement: Command {
        public Decrement(int Count) {
            this.Count = Count;
        }

        public override T Accept<T>(IVisitor<T> visitor) {
            return visitor.VisitDecrementCommand(this);
        }

        public readonly int Count;
    }

    public class LeftParen: Command {
        public override T Accept<T>(IVisitor<T> visitor) {
            return visitor.VisitLeftParenCommand(this);
        }
    }

    public class RightParen: Command {
        public override T Accept<T>(IVisitor<T> visitor) {
            return visitor.VisitRightParenCommand(this);
        }
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
    }

}
