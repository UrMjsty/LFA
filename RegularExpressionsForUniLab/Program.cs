using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace RegularExpressionsForUniLab
{
    public class MainClass
    {
        public static void Main()
        {
            string regex = "O(P|Q|R)+2(3|4)";
            for (int i = 0; i < 1; i++)
            {
                Console.WriteLine("Final String = " + ParseRegEx(regex));
            }
            string regex2 = "A*B(C|D|E)F(G|H|i)^2";
            for (int i = 0; i < 1; i++)
            {
                Console.WriteLine("Final String = " + ParseRegEx(regex2));
            }
            string regex3 = "J+K(L|M|N)*O?(P|Q)^3";
            for (int i = 0; i < 1; i++)
            {
                Console.WriteLine("Final String = " + ParseRegEx(regex3));
            }
        }

        public static string ParseRegEx(string regex)
        {
            // Tokenize the regular expression
            Lexer lexer = new Lexer(regex);
            List<Token> tokens = lexer.Tokenize();

            // Generate strings based on the tokens
            RegExGenerator generator = new RegExGenerator(tokens);
            return generator.GenerateStrings();
        }
    }

    public enum TokenType
    {
        Symbol,
        LBracket,
        Rbracket,
        Or,
        Power,
        QuestionMark,
        Plus,
        Star,
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

        public List<Token> Tokenize()
        {
            List<Token> tokens = new List<Token>();

            while (_position < _input.Length)
            {
                char currentChar = _input[_position];

                if (char.IsLetterOrDigit(currentChar))
                {
                    string symbol = string.Empty;
                    while (_position < _input.Length && (char.IsLetterOrDigit(_input[_position])))
                    {
                        symbol += _input[_position];
                        _position++;
                    }
                    tokens.Add(new Token(TokenType.Symbol, symbol));
                }
                else
                {
                    switch (currentChar)
                    {
                        case '(':
                            tokens.Add(new Token(TokenType.LBracket, "("));
                            _position++;
                            break;
                        case ')':
                            tokens.Add(new Token(TokenType.Rbracket, ")"));
                            _position++;
                            break;
                        case '|':
                            tokens.Add(new Token(TokenType.Or, "|"));
                            _position++;
                            break;
                        case '+':
                            tokens.Add(new Token(TokenType.Plus, "+"));
                            _position++;
                            break;
                        case '*':
                            tokens.Add(new Token(TokenType.Star, "*"));
                            _position++;
                            break;
                        case '^':
                            tokens.Add(new Token(TokenType.Power, "^"));
                            _position++;
                            break;
                        case '?':
                            tokens.Add(new Token(TokenType.QuestionMark, "?"));
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
            }

            tokens.Add(new Token(TokenType.EOF, "")); // Add EOF token to mark the end of tokens
            return tokens;
        }
    }

    public class RegExGenerator
    {
        private readonly List<Token> _tokens;
        private readonly List<string> _inBracketsTokens;
        private readonly Random _rand;
        private bool _isInBrackets;
        private bool _isPower;
        private int rand;
        public RegExGenerator(List<Token> tokens)
        {
            _isInBrackets = false;
            _isPower = false;
            _inBracketsTokens = new List<string>();
            _tokens = tokens;
            _rand = new Random();
        }

        public string GenerateStrings()
        {
            StringBuilder sb = new StringBuilder();
            var curStr = "";
            foreach (Token token in _tokens)
            {
                switch (token.Type)
                {
                    case TokenType.Symbol:
                        if(_isInBrackets)
                            _inBracketsTokens.Add(token.Value);
                        else if (_isPower)
                        {
                            var count = token.Value.ToCharArray()[0] - '0' - 1 ;
                            for (int i = 0; i < count; i++)
                            {
                                sb.Append(curStr);
                                Console.WriteLine("current string = " + sb.ToString());

                            }

                            _isPower = false;
                        }
                        else
                        {
                            curStr = token.Value;
                            sb.Append(curStr);
                            Console.WriteLine("current string = " + sb.ToString());
                        }

                        break;
                    case TokenType.Or:
                        // Do nothing for now, handle this in future implementations
                        break;
                    case TokenType.Plus:
                        rand = new Random().Next(0, 5);
                        for (int i = 0; i < rand; i++)
                        {
                            sb.Append(curStr);
                            Console.WriteLine("current string = " + sb.ToString());

                        }
                        // Do nothing for now, handle this in future implementations    
                        break;
                    case TokenType.Star:
                        sb.Length--;
                        rand = new Random().Next(0, 5);
                        for (int i = 0; i < rand; i++)
                        {
                            sb.Append(curStr);
                            Console.WriteLine("current string = " + sb.ToString());

                        }
                        // Do nothing for now, handle this in future implementations
                        break;
                    case TokenType.LBracket:
                        _isInBrackets = true;
                        break;
                    case TokenType.Rbracket: 
                        rand = new Random().Next(0, _inBracketsTokens.Count);
                        curStr = _inBracketsTokens[rand];
                        sb.Append(curStr);
                        Console.WriteLine("current string = " + sb.ToString());

                        _inBracketsTokens.Clear();
                        _isInBrackets = false;
                        break;
                    case TokenType.EOF:
                        // Ignore brackets and EOF for now
                        break;
                    case TokenType.Power:
                        _isPower = true;
                        break;
                    case TokenType.QuestionMark:
                        rand = new Random().Next(0, 2);
                        if (rand == 1)
                            sb.Length--;
                        break;
                }
               // Console.Write(curStr + " ");
            }
            return sb.ToString();
        }

    }
}
