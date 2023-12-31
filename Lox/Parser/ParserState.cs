﻿using Lox.Models;

namespace Lox.Parser
{
    public class ParserState
    {
        public int Cursor { get; set; }
        
        public List<Token> Tokens { get; set; }

        public ParserState(int cursor, List<Token> tokens)
        {
            Cursor = cursor;
            Tokens = tokens;
        }

        public void MoveNext() 
        {
            Cursor++;
        }

        public Token GetNext() => Tokens[++Cursor];

        public Token Peek() => Tokens[Cursor];

        public Token PeekNext() => Tokens[Cursor + 1];

        public Token Previous() => Tokens[Cursor - 1];

        public bool IsAtEnd() => Peek().Type == TokenType.EOF;

        public bool IsSameAsCurrent(TokenType type) => !IsAtEnd() && Peek().Type == type;

        public bool CurrentIs(params TokenType[] types)
        {
            if (types.Any(IsSameAsCurrent))
            {
                MoveNext();
                return true;
            }

            return false;
        }

        public Token Consume(TokenType type, string message)
        {
            if (IsSameAsCurrent(type))
            {
                MoveNext();
                return Previous();
            }
            else
            {
                throw Error(Peek(), message);
            }
        }

        public void Synchronize() 
        {
            MoveNext();

            while (!IsAtEnd()) 
            {
                if (Previous().Type == TokenType.SEMICOLON)
                    return;

                if (Peek().Type 
                     is TokenType.CLASS
                     or TokenType.FUN 
                     or TokenType.VAR 
                     or TokenType.FOR 
                     or TokenType.IF 
                     or TokenType.WHILE 
                     or TokenType.PRINT 
                     or TokenType.RETURN)
                {
                    return;
                }
                
                MoveNext();
            }
        }

        public ParseError Error(Token token, string message)
        {
            Program.Error(token, message);
            return new ParseError();
        }
    }
}
