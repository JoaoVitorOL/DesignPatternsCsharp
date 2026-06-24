using System;
using System.Collections.Generic;
using System.Linq;
using static System.Console;

namespace AulasSOLIDpatterns.Aula05_DependencyInversion
{
    // O principio de Dependency Inversion diz:
    // modulos de alto nivel nao devem depender de modulos de baixo nivel.
    // Os dois devem depender de abstracoes.
    //
    // Em linguagem simples:
    // a regra de negocio nao deveria conhecer os detalhes internos
    // de como os dados estao guardados.
    //
    // "Abstracao" aqui normalmente significa interface ou contrato.
    // A ideia e depender de "o que eu preciso" e nao de "como isso esta implementado".

    public enum RelationshipType
    {
        Parent,
        Child,
        Sibling,
  
    }

    public class Person2
    {
        public string Name {get; set;} = string.Empty;
        // public dateTime DateOfBirth {get; set;}
    }

    // Low-level
    // Esta classe e um modulo de baixo nivel.
    // Ela cuida do armazenamento concreto dos relacionamentos.
    // Repare que ela decidiu guardar os dados em uma List de tuplas.
    public class Relationships2
    {
        // A tupla tem nomes melhores do que Item1/Item2/Item3,
        // mas continua sendo um detalhe interno de armazenamento.
        private readonly List<(Person2 From, RelationshipType Type, Person2 To)> relations
            = new List<(Person2 From, RelationshipType Type, Person2 To)>();

        // Este detalhe ja expoe um problema didatico para o DIP:
        // a classe de fora consegue enxergar a estrutura interna exata dos dados.
        // Ou seja: quem consumir essa classe passa a depender diretamente
        // da List, da tupla e dos campos From / Type / To.
        //
        // O nome RawRelations deixa a violacao bem explicita:
        // o alto nivel vai consumir dados crus, e nao um contrato de negocio.
        public List<(Person2 From, RelationshipType Type, Person2 To)> RawRelations => relations;

        public void AddParentAndChild(Person2 parent, Person2 child)
        {
            relations.Add((parent, RelationshipType.Parent, child));
            relations.Add((child, RelationshipType.Child, parent));
        }
    }

    public class Research2
    {
        // Research2 e o modulo de alto nivel.
        // Ele representa a parte mais "inteligente" da aplicacao:
        // a parte que quer pesquisar e responder perguntas sobre os dados.
        //
        // Por que errado.cs e errado?
        // Porque o modulo de alto nivel (Research2) depende DIRETAMENTE
        // da classe concreta Relationships2, que e o modulo de baixo nivel.
        // Em vez de depender de uma interface como:
        // "algo que sabe buscar filhos de uma pessoa",
        // ele depende da implementacao concreta inteira.
        public Research2(Relationships2 relationships)
        {
            // Aqui o acoplamento fica explicito:
            // Research2 nao recebe uma abstracao.
            // Ele recebe a classe concreta Relationships2.
            var rawRelations = relationships.RawRelations;

            // Aqui esta a maior violacao do DIP.
            // Research2 nao apenas depende da classe concreta,
            // como tambem depende do FORMATO INTERNO dos dados.
            //
            // Ele sabe que:
            // - existe uma lista
            // - cada item e uma tupla
            // - a pessoa de origem esta em From
            // - o tipo da relacao esta em Type
            // - a outra pessoa esta em To
            //
            // Isso significa que, se o armazenamento mudar,
            // a regra de negocio tambem precisara mudar.
            //
            // Exemplo:
            // se Relationships2 trocar List por banco de dados,
            // dicionario, API, ou outro modelo interno,
            // Research2 provavelmente quebra.
            //
            // A pergunta de negocio deveria ser:
            // "quais sao os filhos de John?"
            //
            // Mas o codigo abaixo faz outra coisa:
            // "deixa eu navegar no seu formato cru e descobrir isso manualmente."
            foreach (var relation in rawRelations.Where(x => x.From.Name == "John" && x.Type == RelationshipType.Parent))
            {
                WriteLine($"John has a child called {relation.To.Name}");
            }
        }

        static void Main(string[] args)
        {
            var parent = new Person2 { Name = "John" };
            var child1 = new Person2 { Name = "Chris" };
            var child2 = new Person2 { Name = "Matt" };

            // O problema comeca aqui: o alto nivel sera ligado
            // diretamente a uma classe concreta de armazenamento.
            var relationships = new Relationships2();
            relationships.AddParentAndChild(parent, child1);
            relationships.AddParentAndChild(parent, child2);

            new Research2(relationships);
        }
    }

    // Resumo do por que este arquivo esta errado no contexto de Dependency Inversion:
    //
    // 1. O modulo de alto nivel (Research2) depende do modulo de baixo nivel (Relationships2).
    // 2. Research2 depende da implementacao concreta, e nao de uma abstracao.
    // 3. Research2 conhece detalhes demais do armazenamento interno.
    // 4. Qualquer mudanca no jeito de guardar os dados pode obrigar a mudar a regra de negocio.
    //
    // O jeito correto seria:
    // criar uma abstracao, por exemplo uma interface,
    // e fazer Research2 depender dessa abstracao.
    // Assim, Research2 pediria apenas "me traga os filhos de John",
    // sem saber se isso veio de lista, tupla, banco ou qualquer outra implementacao.





}
