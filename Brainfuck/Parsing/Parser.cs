using System.Collections;
using Brainfuck.Tokenization;

namespace Brainfuck.Parsing
{
    public class Parser : BaseScanner<Token, Command>
    {
        public Dictionary<int, int> Brackets = new();

        public Parser(List<Token> tokens) : base(tokens) {}

        public List<Command> Parse()
        {
            while (!IsAtEnd())
            {
                Convert(Outputs);

                Move();
            }

            Outputs.Add(new Command.Eof());

            return Outputs;
        }

        private Command.Loop ParseLoop()
        {
            var commandsInLoop = new List<Command>();
            
            if (GetCurrent().Type is TokenType.LeftParen)
                Move();

            while (GetCurrent().Type is not TokenType.RightParen)
            {
                Convert(commandsInLoop);

                if (IsAtEnd())
                    throw new Exception("Missing closing bracket");
                
                Move();
            }

            return new Command.Loop(commandsInLoop);
        }

        private void Convert(List<Command> commands)
        {
            switch (GetCurrent().Type)
            {
                case TokenType.Input:
                    commands.Add(new Command.Input());
                    break;
                case TokenType.Output:
                    commands.Add(new Command.Output());
                    break;
                case TokenType.Left:
                    commands.Add(new Command.Left(GetCount(TokenType.Left)));
                    break;
                case TokenType.Right:
                    commands.Add(new Command.Right(GetCount(TokenType.Right)));
                    break;
                case TokenType.Decrement:
                    commands.Add(new Command.Decrement(GetCount(TokenType.Decrement)));
                    break;
                case TokenType.Increment:
                    commands.Add(new Command.Increment(GetCount(TokenType.Increment)));
                    break;
                case TokenType.LeftParen:
                    commands.Add(ParseLoop());
                    break;
            }
        }

        private int GetCount(TokenType type)
        {
            int count = 0;
            if (GetCurrent().Type == type)
                count++;

            while (Next().Type == type)
            {
                Move();
                count++;
            }

            return count;
        }

        private bool IsAtEnd() => GetCurrent().Type == TokenType.Eof;
    }
}