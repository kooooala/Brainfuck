namespace Brainfuck.Tokenization;

public class Lexer
{
    private readonly string _source;
    private int _line = 0, _character = 0;
    private List<Token> _tokens;

    public Lexer(string source)
    {
        _source = source;
        _tokens = new();
    }

    public List<Token> Scan()
    {
        foreach (var c in _source)
        {
            _character++;
            switch (c)
            {
                case ',':
                    AddToken(TokenType.Input);
                    break;
                case '.':
                    AddToken(TokenType.Output);
                    break;
                case '<':
                    AddToken(TokenType.Left);
                    break;
                case '>':
                    AddToken(TokenType.Right);
                    break;
                case '+':
                    AddToken(TokenType.Increment);
                    break;
                case '-':
                    AddToken(TokenType.Decrement);
                    break;
                case '[':
                    AddToken(TokenType.LeftParen);
                    break;
                case ']':
                    AddToken(TokenType.RightParen);
                    break;
                case '\n':
                    _line++;
                    _character = 0;
                    break;
                default:
                    continue;
            }
        }
        AddToken(TokenType.Eof);

        return _tokens;
    }
    
    private void AddToken(TokenType token)
    {
        _tokens.Add(new Token(token, _line, _character));
    }
}