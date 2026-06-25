using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;

namespace Aula01_builder
{
    // LEITURA OBRIGATORIA DESTA AULA
    //
    // Este arquivo existe para mostrar a diferenca entre:
    // 1. Builder normal
    // 2. Fluent Builder
    //
    // Os DOIS continuam sendo Builder.
    // Os DOIS continuam separando:
    // - o objeto final que sera construido
    // - o processo de construcao desse objeto
    //
    // Entao onde esta a diferenca real?
    //
    // Builder normal:
    // - o metodo de construcao faz o trabalho
    // - mas retorna void
    // - entao a chamada acaba ali
    //
    // Fluent Builder:
    // - o metodo de construcao faz o trabalho
    // - e devolve o proprio builder
    // - entao a chamada pode continuar
    //
    // Comparacao visual:
    //
    // Builder normal:
    // builder.AddChild("li", "hello");
    // builder.AddChild("li", "world");
    // builder.AddChild("li", "XD");
    //
    // Fluent Builder:
    // builder.AddChildFluent("li", "hello")
    //        .AddChildFluent("li", "world")
    //        .AddChildFluent("li", "XD");
    //
    // Resumo em uma frase:
    // Fluent Builder = Builder com API encadeavel via return this.


    // ========================
    // PRODUTO FINAL
    // ========================
    public class HtmlElement
    {
        // O produto continua o mesmo da aula anterior.
        // Isso e importante:
        // a aula de Fluent Builder NAO quer mudar o HTML produzido.
        // Ela quer mudar a forma de montar esse HTML.
        public string Name = string.Empty, Text = string.Empty;

        // A estrutura continua sendo uma arvore:
        // um elemento pode ter varios filhos.
        public List<HtmlElement> Elements = new List<HtmlElement>();

        // Apenas para formatar a saida visual.
        private const int indentSize = 2;

        public HtmlElement()
        {
            // Construtor vazio:
            // util quando o builder cria a raiz primeiro
            // e preenche o restante aos poucos.
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

        private string ToStringImpl(int indent)
        {
            // Repare:
            // a renderizacao continua sendo detalhe interno do produto.
            // Fluent Builder nao muda isso.
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


    // ========================
    // BUILDER
    // ========================
    public class HtmlBuilder
    {
        // O builder continua guardando:
        // - o nome da raiz
        // - o objeto raiz em construcao
        private readonly string rootName;
        private HtmlElement root = new HtmlElement();

        public HtmlBuilder(string rootName)
        {
            this.rootName = rootName;
            root.Name = rootName;
        }

        // ========================
        // BUILDER NORMAL
        // ========================
        public void AddChild(string childName, string childText)
        {
            // ESTE E O ESTILO DA AULA ANTERIOR.
            //
            // O metodo funciona normalmente:
            // - cria o filho
            // - adiciona o filho na raiz
            //
            // MAS ele retorna void.
            // Isso significa:
            // a chamada termina aqui.
            //
            // Entao o cliente e obrigado a fazer:
            // builder.AddChild(...);
            // builder.AddChild(...);
            // builder.AddChild(...);
            //
            // Ou seja:
            // uma linha por passo.
            var e = new HtmlElement(childName, childText);
            root.Elements.Add(e);
        }

        // ========================
        // FLUENT BUILDER
        // ========================
        public HtmlBuilder AddChildFluent(string childName, string childText)
        {
            // A LOGICA INTERNA continua quase igual ao builder normal.
            //
            // Isso e o ponto mais importante desta aula:
            // o Fluent Builder NAO muda o processo de construcao em si.
            // O que ele muda e a assinatura do metodo e a experiencia de uso.
            var e = new HtmlElement(childName, childText);
            root.Elements.Add(e);

            // DIFERENCA REAL COMECA AQUI
            //
            // `this` significa:
            // "devolva a propria instancia atual do builder".
            //
            // Traduzindo:
            // em vez de encerrar a conversa com o cliente,
            // o metodo devolve o mesmo builder para que a conversa continue.
            //
            // E isso permite o encadeamento:
            // builder.AddChildFluent(...)
            //        .AddChildFluent(...)
            //        .AddChildFluent(...);
            //
            // Regra mental:
            // - se retorna void -> builder normal
            // - se retorna o proprio builder -> fluent builder
            return this;
        }

        public void Clear()
        {
            // Reinicia a construcao mantendo a mesma raiz.
            root = new HtmlElement { Name = rootName };
        }

        public override string ToString()
        {
            // No final, tanto o builder normal quanto o fluent builder
            // entregam o mesmo tipo de resultado.
            //
            // O Fluent Builder melhora a leitura da construcao.
            // Ele nao existe para mudar o ToString().
            return root.ToString();
        }
    }


    // ========================
    // DEMONSTRACAO
    // ========================
    public class Demo
    {
        static void Main(string[] args)
        {
            var builder = new HtmlBuilder("ul");

            // COMPARACAO 1: BUILDER NORMAL
            //
            // Se quisessemos usar o builder normal, seria assim:
            //
            // builder.AddChild("li", "hello");
            // builder.AddChild("li", "world");
            // builder.AddChild("li", "XD");
            //
            // O objeto final seria montado corretamente,
            // mas cada passo fica isolado em sua propria linha.

            // COMPARACAO 2: FLUENT BUILDER
            //
            // Aqui a construcao continua sendo a mesma.
            // O que muda e que o codigo agora "flui" como uma frase:
            builder
                .AddChildFluent("li", "hello")
                .AddChildFluent("li", "world")
                .AddChildFluent("li", "XD");

            // O ganho principal desta aula e:
            // - mais legibilidade na montagem
            // - menos repeticao do nome da variavel builder
            // - sensacao de sequencia unica de intencao
            //
            // O ganho NAO e:
            // - performance
            // - mudar o HTML gerado
            // - criar outro padrao diferente do Builder
            WriteLine(builder.ToString());
        }
    }
}
