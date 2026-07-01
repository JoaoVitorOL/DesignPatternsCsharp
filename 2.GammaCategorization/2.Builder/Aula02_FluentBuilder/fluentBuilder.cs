// Exemplo da aula:
// o mesmo gerador de HTML da versao anterior.
// Aqui o foco e mostrar quando o builder passa a ser fluent builder:
// o metodo retorna o proprio builder e permite encadear a construcao.
using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;




namespace Aula02_FluentBuilder
{
    // Fluent Builder:
    // gera o mesmo HTML do builder tradicional.
    // A diferenca principal esta na API:
    // o metodo de construcao retorna o proprio builder com "this" e permite encadeamento.

    // ===== Classe =====
    public class HtmlElement
    {
        // Produto final igual ao exemplo anterior.

        // ===== Campos =====
        public string Name = string.Empty, Text = string.Empty;
        public List<HtmlElement> Elements = new List<HtmlElement>();

        // Apenas formata a saida do gerador de HTML.
        private const int indentSize = 2;

        // ===== Construtores =====
        public HtmlElement()
        {
        }

        public HtmlElement(string name, string text)
        {
            if (name == null)
            {
                throw new ArgumentNullException(paramName: nameof(name));
            }

            if (text == null)
            {
                throw new ArgumentNullException(paramName: nameof(text));
            }

            Name = name;
            Text = text;
        }

        // ===== Metodos =====
        private string ToStringImpl(int indent)
        {
            var sb = new StringBuilder();
            var i = new string(' ', indentSize * indent);

            sb.AppendLine($"{i}<{Name}>");

            if (!string.IsNullOrWhiteSpace(Text))
            {
                sb.Append(new string(' ', indentSize * (indent + 1)));
                sb.AppendLine(Text);
            }

            foreach (var e in Elements)
            {
                sb.Append(e.ToStringImpl(indent + 1));
            }

            sb.AppendLine($"{i}</{Name}>");
            return sb.ToString();
        }

        public override string ToString()
        {
            return ToStringImpl(0);
        }
    }

    // ===== Builder =====
    public class HtmlBuilder
    {
        // ===== Campos =====
        private readonly string rootName;
        private HtmlElement root = new HtmlElement();

        // ===== Construtores =====
        public HtmlBuilder(string rootName)
        {
            this.rootName = rootName;
            root.Name = rootName;
        }

        // Este trecho ainda e builder normal:
        // constroi o filho, mas retorna `void`.

        // ===== Metodos =====
        public void AddChild(string childName, string childText)
        {
            var e = new HtmlElement(childName, childText);
            root.Elements.Add(e);
        }

        // Aqui comeca a diferenca que transforma a API em fluent builder:
        // 1. o metodo retorna `HtmlBuilder`, nao `void`
        // 2. isso permite devolver o proprio builder ao final
        public HtmlBuilder AddChildFluent(string childName, string childText)
        {
            var e = new HtmlElement(childName, childText);
            root.Elements.Add(e);

            // Este `return this` devolve a instancia atual.
            // Com isso, a proxima chamada pode continuar na mesma expressao.
            return this;
        }

        public void Clear()
        {
            root = new HtmlElement { Name = rootName };
        }

        public override string ToString()
        {
            return root.ToString();
        }
    }

    // ===== Classe =====
    public class Demo
    {
        // ===== Metodos =====
        static void Main(string[] args)
        {
            var builder = new HtmlBuilder("ul");

            // Como o metodo devolve o proprio builder, a construcao passa a encadear.
            builder
                .AddChildFluent("li", "hello")
                .AddChildFluent("li", "world")
                .AddChildFluent("li", "XD");

            WriteLine(builder.ToString());
        }
    }
}
