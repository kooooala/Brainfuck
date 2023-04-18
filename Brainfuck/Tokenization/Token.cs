namespace Brainfuck.Tokenization;

public class Token
{
    public readonly TokenType Type;
    public readonly int Line;
    public readonly int Character;

    public Token(TokenType type, int line, int character)
    {
        Type = type;
        Line = line;
        Character = character;
    }

    public override string ToString() => $"{Type} at ({Line}, {Character}";
}