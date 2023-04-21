using System.Collections;
using Brainfuck.Tokenization;

namespace Brainfuck.Parsing
{
    public class Parser
    {
        private List<Command> _commands;
        private List<Token> _tokens;
        private int _currentTokenLocation = 0;

        public Dictionary<int, int> Brackets = new(); 

        public Parser(List<Token> tokens)
        {
            _tokens = tokens;
            _commands = new();
        }

        public List<Command> Parse()
        {
            var bracketStack = new Stack<int>();
            
            while (!IsAtEnd())
            {
                switch (GetCurrentToken().Type)
                {
                    case TokenType.Input:
                        _commands.Add(new Command.Input());
                        break;
                    case TokenType.Output:
                        _commands.Add(new Command.Output());
                        break;
                    case TokenType.Left:
                        _commands.Add(new Command.Left(GetCount(TokenType.Left)));
                        break;
                    case TokenType.Right:
                        _commands.Add(new Command.Right(GetCount(TokenType.Right)));
                        break;
                    case TokenType.Decrement:
                        _commands.Add(new Command.Decrement(GetCount(TokenType.Decrement)));
                        break;
                    case TokenType.Increment:
                        _commands.Add(new Command.Increment(GetCount(TokenType.Increment)));
                        break;
                    case TokenType.LeftParen:
                        // check for "[-]" which would set the cell to zero for optimisation  
                        if (NextToken().Type == TokenType.Decrement && NextToken(2).Type == TokenType.RightParen)
                        {
                            Move(2);
                            _commands.Add(new Command.ToZero());
                            break;
                        }

                        _commands.Add(new Command.LeftParen());

                        bracketStack.Push(_commands.Count);
                        break;
                    case TokenType.RightParen:
                        _commands.Add(new Command.RightParen());

                        int target;
                        var isSuccessful = bracketStack.TryPop(out target);
                        if (!isSuccessful) throw new Exception("Missing closing bracket.");

                        Brackets.Add(_commands.Count - 1, target - 1);
                        Brackets.Add(target - 1, _commands.Count - 1);
                        
                        break;
                }

                Move();
            }

            _commands.Add(new Command.Eof());

            return _commands;
        }

        private int GetCount(TokenType type)
        {
            int count = 0;
            if (GetCurrentToken().Type == type)
                count++;

            while (NextToken().Type == type)
            {
                Move();
                count++;
            }

            return count;
        }

        private Token GetCurrentToken() => _tokens[_currentTokenLocation];

        private void Move()
        {
            _currentTokenLocation++;
        }

        private void Move(int count)
        {
            _currentTokenLocation += count;
        }

        private bool IsAtEnd() => GetCurrentToken().Type == TokenType.Eof;

        private Token NextToken() => _tokens[_currentTokenLocation + 1];

        private Token NextToken(int count) => _tokens[_currentTokenLocation + count];
    }
}