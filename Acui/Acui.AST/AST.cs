using System.Collections.Generic;

namespace acui.AST
{
    public interface IAcui {
        string Transpile();
    }
    public interface IAcuiTopLevel : IAcui {}
    public interface IAcuiStatement : IAcui {}
    public interface IAcuiExpr : IAcuiStatement { }
    public interface IAcuiLiteral : IAcuiExpr { }

    ///
    /// Toplevels
    ///
    public class AcuiFunction : IAcuiTopLevel
    {
        public AcuiIdentifierLiteral name { get; set; }
        public AcuiIdentifierLiteral replies { get; set; }
        public List<(AcuiIdentifierLiteral,AcuiIdentifierLiteral)> arguments { get; set; }
        public List<IAcuiStatement> statements { get; set; }
        public override string ToString() {
            return $"func {name.reference} {string.Join(' ', arguments.ConvertAll(a => a.Item1.ToString() + ":" + a.Item2.ToString()))} {(replies != null ? "-> " + replies.ToString() : "")} {'{'}\n{string.Join('\n', statements.ConvertAll(s => '\t'+s.ToString()))}\n{'}'}";
        }
        public string Transpile() =>
            $"{(replies != null ? replies.Transpile() : "void")} {name.reference} ({string.Join(", ", arguments.ConvertAll(a => a.Item2.Transpile() + " " + a.Item1.Transpile()))}) {'{'}\n{string.Join("\n", statements.ConvertAll(s => '\t'+s.Transpile()+";"))}\n{'}'}";
    }
    public class AcuiImport : IAcuiTopLevel
    {
        public string import { get; set; }
        public override string ToString() => $"import {import}";
        public string Transpile() => $"#include {import}";
    }

    ///
    /// Literals
    ///
    public class AcuiIdentifierLiteral : IAcuiLiteral
    {
        public string reference { get; set; }
        public override string ToString() => reference;
        public string Transpile() => reference;
    }
    public class AcuiStringLiteral : IAcuiLiteral
    {
        public string value { get; set; }
        public override string ToString() => '`' + value + '`';
        public string Transpile() => $"\"{value}\"";
    }
    public class AcuiIntegerLiteral : IAcuiLiteral
    {
        public int value { get; set; }
        public override string ToString() => value.ToString();
        public string Transpile() => value.ToString();
    }
    ///
    /// Statements
    ///
    public class AcuiVariableDefinition : IAcuiStatement
    {
        public AcuiIdentifierLiteral name { get; set; }
        public IAcuiExpr value { get; set; }
        public override string ToString() => $"{name} := {value}";
        public string Transpile() => $"__auto_type {name.Transpile()} = {value.Transpile()}";
    }
    public class AcuiVariableAssignment : IAcuiStatement
    {
        public AcuiIdentifierLiteral name { get; set; }
        public IAcuiExpr value { get; set; }
        public override string ToString() => $"{name} = {value}";
        public string Transpile() => $"{name} = {value}";
    }
    public class AcuiReply : IAcuiStatement
    {
        public IAcuiExpr value { get; set; }
        public override string ToString() => $"reply {value}";
        public string Transpile() => $"return {value}";
    }

    ///
    /// Expressions
    ///
    public class AcuiMessage : IAcuiExpr
    {
        public AcuiIdentifierLiteral target { get; set; }
        public List<(string, IAcuiExpr)> selectors { get; set; }
        public override string ToString()
        {
            return $"({target} {string.Join(" ", selectors.ConvertAll(item => $"{(item.Item2 == null ? $".{item.Item1}" : $"{item.Item1}:{item.Item2}")}"))})";
        }
        public string Transpile() {
            var selector = new List<string>();
            var values = new List<string>();
            foreach (var item in selectors) {
                selector.Add(item.Item1);
                if (item.Item2 != null) {
                    values.Add(item.Item2.Transpile());
                }
            }
            var args = new List<string>();
            args.Add($"\":{string.Join(':', selector)}\"");
            args.Add(values.Count.ToString());
            args.AddRange(values);
            args.Insert(0, target.Transpile());
            return $"acui_sendMessage({string.Join(", ", args)})";
        }
    }
    public class AcuiFunctionCall : IAcuiExpr
    {
        public AcuiIdentifierLiteral function { get; set; }
        public List<IAcuiExpr> arguments { get; set; }
        public override string ToString() => $"{function}({string.Join(" ,", arguments.ConvertAll(i => i.ToString()))})";
        public string Transpile() => $"{function}({string.Join(" ,", arguments.ConvertAll(i => i.Transpile()))})";
    }
}