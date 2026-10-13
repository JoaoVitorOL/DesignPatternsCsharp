// Exemplo da aula:
// um gerador de HTML que monta uma lista com itens filhos.
// Neste arquivo, o foco e mostrar o builder tradicional,
// em que a construcao acontece passo a passo, sem encadeamento.
using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;

namespace Aula01_builder
{
    // Builder tradicional:
    // monta a arvore HTML passo a passo.
    // A diferenca para o fluent builder esta na API:
    // aqui cada chamada termina em `void`, sem encadeamento.

    // ===== Classe =====
    public class HtmlElement
    {
        // Produto final do exemplo: um no da arvore HTML.

        // ===== Campos =====
        public string Name = string.Empty, Text = string.Empty;
        public List<HtmlElement> Elements = new List<HtmlElement>();

        // Apenas formata a saida do gerador de HTML.
        private const int identSize = 2;

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
        private string ToStringImpl(int ident)
        {
            var sb = new StringBuilder();
            var i = new string(' ', identSize * ident);

            sb.AppendLine($"{i}<{Name}>");

            if (!string.IsNullOrWhiteSpace(Text))
            {
                sb.Append(new string(' ', identSize * (ident + 1)));
                sb.AppendLine(Text);
            }

            foreach (var e in Elements)
            {
                sb.Append(e.ToStringImpl(ident + 1));
            }

            sb.AppendLine($"{i}</{Name}>");

            return sb.ToString();
        }

        public override string ToString()
        {
            return ToStringImpl(0);
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

            // Aqui ele continua sendo builder normal:
            // o metodo constroi, mas retorna `void`.
            // Sem retorno do proprio builder, nao existe encadeamento.

            // ===== Metodos =====
            public void AddChild(string childName, string childText)
            {
                var e = new HtmlElement(childName, childText);
                root.Elements.Add(e);
            }

            public override string ToString()
            {
                return root.ToString();
            }

            public void Clear()
            {
                root = new HtmlElement { Name = rootName };
            }
        }

        // ===== Classe =====
        public class Demo
        {
            // ===== Metodos =====
            static void Main(string[] args)
            {
                var builder = new HtmlBuilder("ul");

                // Como `AddChild` retorna `void`, cada passo precisa ficar em sua propria linha.
                builder.AddChild("li", "hello");
                builder.AddChild("li", "world");
                builder.AddChild("p", "Ser ou nao ser, eis a questao");

                WriteLine(builder.ToString());
            }
        }
    }
}
