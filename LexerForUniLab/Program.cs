//using System;
//using System.Collections.Generic;

namespace LexerForUniLab;

public enum TokenType
{
    Number,
    Plus,
    Minus,
    Multiply,
    Divide,
    LeftParenthesis,
    RightParenthesis,
    // ReSharper disable once InconsistentNaming
    EOF // End of File
}

public class Token
{
    public TokenType Type { get; }
    public string Value { get; }

    public Token(TokenType type, string value)
    {
        Type = type;
        Value = value;
    }
}

public class Lexer
{
    private readonly string _input;
    private int _position;

    public Lexer(string input)
    {
        _input = input;
        _position = 0;
    }

    private void SkipWhitespace()
    {
        while (_position < _input.Length && char.IsWhiteSpace(_input[_position]))
        {
            _position++;
        }
    }

    public List<Token?> Tokenize()
    {
        List<Token?> tokens = new List<Token?>();

        while (_position < _input.Length)
        {
            char currentChar = _input[_position];

            if (char.IsDigit(currentChar))
            {
                string number = string.Empty;
                while (_position < _input.Length && (char.IsDigit(_input[_position]) || _input[_position] == '.'))
                {
                    number += _input[_position];
                    _position++;
                }
                tokens.Add(new Token(TokenType.Number, number));
            }
            else switch (currentChar)
            {
                case '+':
                    tokens.Add(new Token(TokenType.Plus, "+"));
                    _position++;
                    break;
                case '-':
                    tokens.Add(new Token(TokenType.Minus, "-"));
                    _position++;
                    break;
                case '*':
                    tokens.Add(new Token(TokenType.Multiply, "*"));
                    _position++;
                    break;
                case '/':
                    tokens.Add(new Token(TokenType.Divide, "/"));
                    _position++;
                    break;
                case '(':
                    tokens.Add(new Token(TokenType.LeftParenthesis, "("));
                    _position++;
                    break;
                case ')':
                    tokens.Add(new Token(TokenType.RightParenthesis, ")"));
                    _position++;
                    break;
                default:
                {
                    if (char.IsWhiteSpace(currentChar))
                    {
                        SkipWhitespace();
                    }
                    else
                    {
                        throw new InvalidOperationException($"Invalid character: {currentChar}");
                    }

                    break;
                }
            }
        }

        tokens.Add(new Token(TokenType.EOF, "")); // Add EOF token to mark the end of tokens
        return tokens;
    }
}

public class Calculator
{
    private readonly List<Token?> _tokens;
    private int _position;

    public Calculator(List<Token?> tokens)
    {
        _tokens = tokens;
        _position = 0;
    }

    private Token? Peek()
    {
        return _position < _tokens.Count ? _tokens[_position] : null;
    }

    private Token? Advance()
    {
        _position++;
        return _tokens[_position - 1];
    }

    private double Factor()
    {
        Token? currentToken = Peek();
        switch (currentToken)
        {
            case { Type: TokenType.Number }:
                Advance();
                return double.Parse(currentToken.Value);
            case { Type: TokenType.LeftParenthesis }:
            {
                Advance();
                double result = Expression();
                if (Peek()?.Type != TokenType.RightParenthesis)
                {
                    // Console.ForegroundColor = ConsoleColor.Red;
                    throw new InvalidOperationException($"Invalid expression: Missing right parenthesis after operator at position {_position}");
                }
                Advance();
                return result;
            }
            default:
                throw new InvalidOperationException($"Invalid expression: Expected number or bracket after operator at position {_position}");
        }
    }

    private double Term()
    {
        double result = Factor();

        while (Peek()?.Type == TokenType.Multiply || Peek()?.Type == TokenType.Divide)
        {
            Token? op = Advance();
            double right = Factor();
            if (op is { Type: TokenType.Multiply })
            {
                result *= right;
            }
            else
            {
                result /= right;
            }
        }

        return result;
    }

    private double Expression()
    {
        double result = Term();

        while (Peek()?.Type is TokenType.Plus or TokenType.Minus)
        {
            Token? op = Advance();
            double right = Term();
            if (op is { Type: TokenType.Plus })
            {
                result += right;
            }
            else
            {
                result -= right;
            }
        }

        return result;
    }

    public double Calculate()
    {
        double result = Expression();

        if (_position < _tokens.Count - 1 || (_position == _tokens.Count - 1 && _tokens[_position]!.Type != TokenType.EOF))
        {
          //  Console.ForegroundColor = ConsoleColor.Red;
           throw new InvalidOperationException($"Invalid expression: Unexpected token '{_tokens[_position]?.Value}' at position {_position}");
        }

        return result;
    }
}

public static class Program
{
    private static void Main()
    {
        string input = "2/1 - (10 + 5) * 2 - 6 / 0 ";
        Lexer lexer = new Lexer(input);
        List<Token?> tokens = lexer.Tokenize();

        foreach (Token? token in tokens)
        {
            if (token != null) Console.WriteLine($"Type: {token.Type}, Value: {token.Value}");
        }

        Calculator calculator = new Calculator(tokens);
        double result = calculator.Calculate();
        Console.WriteLine($"Result: {result}");
    }
}