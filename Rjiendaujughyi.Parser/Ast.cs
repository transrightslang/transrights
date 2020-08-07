using System.Collections.Generic;

namespace rjiendaujughyi
{
    public interface IAcui { }
    public interface IAcuiExpr : IAcui { }
    public interface IAcuiLiteral : IAcuiExpr { }
    public class AcuiIdentifierLiteral : IAcuiLiteral
    {
        public string reference { get; set; }
        public override string ToString() => reference;
    }
    public class AcuiStringLiteral : IAcuiLiteral
    {
        public string value { get; set; }
        public override string ToString() => '`' + value + '`';
    }
    public class AcuiMessage : IAcuiExpr
    {
        public AcuiIdentifierLiteral target { get; set; }
        public List<(string, IAcuiExpr)> selectors { get; set; }
        public override string ToString()
        {
            return $"({target} {string.Join(" ", selectors.ConvertAll(item => $"{item.Item1}:{item.Item2}"))})";
        }
    }
}