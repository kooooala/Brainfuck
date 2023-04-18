namespace Brainfuck.Tokenization;

public enum TokenType
{
    // I/O
    Input, Output,
    
    // Pointer manipulation
    Left, Right,
    
    // Cell manipulation
    Increment, Decrement,
    
    // Loop
    LeftParen, RightParen,
    
    Eof
}