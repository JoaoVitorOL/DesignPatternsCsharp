using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;

namespace Aula01_builder
{
    // LEITURA OBRIGATÓRIA DESTA AULA:
    //
    // Este arquivo mostra o BUILDER NORMAL.
    //
    // Comparação com o SemBuilder.cs:
    // - lá o cliente monta o HTML manualmente como texto;
    // - aqui o cliente usa um objeto construtor para montar a estrutura.
    //
    // Comparação com o fluentBuilder.cs:
    // - aqui cada passo é chamado separadamente;
    // - os métodos de construção NÃO devolvem o próprio builder;
    // - por isso a API não encadeia.
    //
    // Builder normal:
    // builder.AddChild(...);
    // builder.AddChild(...);
    // builder.AddChild(...);
    //
    // Fluent Builder:
    // builder.AddChildFluent(...)
    //        .AddChildFluent(...)
    //        .AddChildFluent(...);
    //
    // Resumo em uma frase:
    // este arquivo mostra o Builder básico, com passos separados.


    // ========================
    // Classe para o elemento html
    // ========================
    public class HtmlElement
    {
        // Cada HtmlElement representa um nó da árvore HTML.
        // Exemplo: <ul>, <li>, etc.
        public string Name = string.Empty, Text = string.Empty;

        // Em vez de concatenar tudo direto em uma string,
        // guardamos filhos dentro de uma lista.
        // Isso transforma o HTML em uma estrutura de árvore.
        public List<HtmlElement> Elements = new List<HtmlElement>();

        // Quantidade de espaços usada para "indentação" visual.
        // Serve apenas para deixar a saída mais legível.
        private const int identSize = 2;



    // ========================
    // Construtor
    // ========================
        public HtmlElement()
        {
            // Construtor vazio:
            // útil quando queremos criar o elemento primeiro
            // e preencher suas partes depois.
        }

        public HtmlElement(string name, string text)
        {
            // Essas validações protegem o objeto de nascer em estado ruim.
            if (name == null)
            {
                throw new ArgumentNullException(paramName: nameof(name));
            }

            if (text == null)
            {
                throw new ArgumentNullException(paramName: nameof(text));
            }

            // Aqui estamos dizendo:
            // este elemento tem uma tag (Name) e um conteúdo de texto (Text).
            Name = name;
            Text = text;
            
        }


    // ========================
    // Substituto do ToString padrão
    // ========================
        private string ToStringImpl(int ident)
        {
            // Aqui usamos StringBuilder de novo,
            // mas agora ele aparece no lugar certo:
            // como detalhe interno da renderização do objeto.
            //
            // Em SemBuilder.cs, o StringBuilder era a estratégia principal do cliente.
            // Aqui, ele é apenas um detalhe técnico escondido dentro da classe.
            var sb = new StringBuilder();

            // "ident" informa em qual nível da árvore estamos.
            // Quanto mais fundo, mais espaços colocamos.
            var i = new string(' ', identSize * ident);

            // Abre a tag do elemento atual.
            sb.AppendLine($"{i}<{Name}>");

            if (!string.IsNullOrWhiteSpace(Text))
            {
                // Se este nó tiver texto, renderizamos o conteúdo interno.
                sb.Append(new string(' ', identSize * (ident + 1)));
                sb.AppendLine(Text);
            }

            // Aqui aparece a ideia de árvore claramente:
            // cada elemento sabe renderizar seus próprios filhos.
            foreach (var e in Elements)
            {
                sb.Append(e.ToStringImpl(ident + 1)); // chamadas recursivas
            }

            // Fecha a tag do elemento atual.
            sb.AppendLine($"{i}</{Name}>");

            return sb.ToString();
        }


    // ========================
    // Override do ToString padrão
    // ========================
        public override string ToString()
        {
            // O cliente só pede a representação final.
            // Toda a lógica de como renderizar ficou encapsulada aqui.
            return ToStringImpl(0);
        }


    // ========================
    // Classe do BUILDER HTML
    // ========================
        public class HtmlBuilder
        {
            // Guardamos o nome da raiz para conseguir recriar a estrutura depois.
            private readonly string rootName;

            // "root" é o objeto que está sendo montado aos poucos.
            HtmlElement root = new HtmlElement();

            public  HtmlBuilder(string rootName)
            {
                // O Builder já nasce sabendo qual é o elemento principal.
                // Exemplo: a lista raiz será um <ul>.
                this.rootName = rootName;
                root.Name = rootName;
            }

            public void AddChild(string childName, string childText)
            {
                // O cliente não precisa saber como concatenar "<li>texto</li>".
                // Ele apenas diz: adicione um filho com nome e texto.
                // A montagem concreta fica centralizada no Builder.
                var e = new HtmlElement(childName, childText);
                root.Elements.Add(e);

            }


            public override string ToString()
            {
                // O Builder delega ao objeto final a responsabilidade
                // de se transformar em texto.
                return root.ToString();
            }

            public void Clear()
            {
                // Recomeça a construção mantendo a mesma raiz.
                // Isso reforça a ideia de "objeto em construção".
                root = new HtmlElement{Name = rootName};
            }
        }


        
        public class Demo
        {
            static void Main(string[] args)
            {
                // Aqui está a diferença principal em relação ao SemBuilder.cs:
                // o cliente não monta texto manualmente.
                // Ele usa uma API mais semântica para dizer o que quer construir.
                var builder = new HtmlBuilder("ul");

                // "quero uma lista"
                // "quero um item com texto hello"
                // "quero outro item com texto world"
                builder.AddChild("li","hello");
                builder.AddChild("li","world");
                builder.AddChild("p","Ser ou não ser, eis a questão");
                // Só no final pedimos a representação em string.
                // Ou seja: primeiro construímos a estrutura, depois renderizamos.
                WriteLine(builder.ToString());

            }
        }

    }

}
