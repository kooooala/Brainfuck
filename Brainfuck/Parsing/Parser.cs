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
            var bracketStack = new Stack<int>();
            
            while (!IsAtEnd())
            {
                switch (GetCurrent().Type)
                {
                    case TokenType.Input:
                        Outputs.Add(new Command.Input());
                        break;
                    case TokenType.Output:
                        Outputs.Add(new Command.Output());
                        break;
                    case TokenType.Left:
                        Outputs.Add(new Command.Left(GetCount(TokenType.Left)));
                        break;
                    case TokenType.Right:
                        Outputs.Add(new Command.Right(GetCount(TokenType.Right)));
                        break;
                    case TokenType.Decrement:
                        Outputs.Add(new Command.Decrement(GetCount(TokenType.Decrement)));
                        break;
                    case TokenType.Increment:
                        Outputs.Add(new Command.Increment(GetCount(TokenType.Increment)));
                        break;
                    case TokenType.LeftParen:
                        // check for "[-]" which would set the cell to zero for optimisation  
                        if (Next().Type == TokenType.Decrement && Next(2).Type == TokenType.RightParen)
                        {
                            Move(2);
                            Outputs.Add(new Command.ToZero());
                            break;
                        }

                        Outputs.Add(new Command.LeftParen());

                        bracketStack.Push(Outputs.Count);
                        break;
                    case TokenType.RightParen:
                        Outputs.Add(new Command.RightParen());

                        int target;
                        var isSuccessful = bracketStack.TryPop(out target);
                        if (!isSuccessful) throw new Exception("Missing closing bracket.");

                        Brackets.Add(Outputs.Count - 1, target - 1);
                        Brackets.Add(target - 1, Outputs.Count - 1);
                        
                        break;
                }

                Move();
            }

            Outputs.Add(new Command.Eof());

            return Outputs;
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