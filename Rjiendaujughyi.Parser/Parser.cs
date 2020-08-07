using System.Linq;
using Pidgin;
using static Pidgin.Parser;
using static Pidgin.Parser<char>;

namespace rjiendaujughyi
{
    public static class AcuiParser
    {
        private static readonly Pidgin.Parser<char, char> LParen = Char('(');
        private static readonly Pidgin.Parser<char, char> RParen = Char(')');
        private static readonly Pidgin.Parser<char, char> Colon = Char(':');
        private static readonly Parser<char, char> ColonWhitespace =
            Colon.Between(SkipWhitespaces);
        private static readonly Pidgin.Parser<char, char> Quote = Char('`');
        private static readonly Pidgin.Parser<char, string> String = Token(c => c != '`').ManyString().Between(Quote);
        private static readonly Pidgin.Parser<char, IAcui> AcuiIdentifier =
                Map(
                    (first, second) =>
                    {
                        IAcui ident = new AcuiIdentifierLiteral
                        {
                            reference = first + second
                        };
                        return ident;
                    },
                    Token(c => System.Char.IsLetter(c) || c == '_'),
                    Token(c => System.Char.IsLetterOrDigit(c) || c == '_').ManyString()
                );
        private static readonly Pidgin.Parser<char, (string, IAcuiExpr)> AcuiSelector =
                Map(
                    (ident, _colon, value) =>
                    {
                        return (((AcuiIdentifierLiteral)ident).reference, (IAcuiExpr)value);
                    },
                    AcuiIdentifier, ColonWhitespace, Rec(() => Expression)
                );
        private static readonly Pidgin.Parser<char, IAcui> AcuiMessage =
                Map(
                    (_lparen, ident, expr, _rparen) =>
                    {
                        IAcui message = new AcuiMessage
                        {
                            target = (AcuiIdentifierLiteral)ident,
                            selectors = expr.ToList(),
                        };
                        return message;
                    },
                    LParen, AcuiIdentifier, Rec(() => AcuiSelector.Between(SkipWhitespaces).Many()).Between(SkipWhitespaces), RParen
                );
        private static readonly Pidgin.Parser<char, IAcui> AcuiString = String.Select<IAcui>(s => new AcuiStringLiteral { value = s });
        private static readonly Pidgin.Parser<char, IAcui> Expression = OneOf(AcuiString, AcuiMessage);

        public static Pidgin.Result<char, IAcui> Parse(string input) => Expression.Between(SkipWhitespaces).Parse(input);
    }
}