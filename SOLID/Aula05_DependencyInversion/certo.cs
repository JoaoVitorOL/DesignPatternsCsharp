using System;
using System.Collections.Generic;
using static System.Console;

namespace AulasSOLIDpatterns.Aula05_DependencyInversion
{
    // O principio de Dependency Inversion diz:
    // modulos de alto nivel nao devem depender de modulos de baixo nivel.
    // Os dois devem depender de abstracoes.
    //
    // Em termos praticos:
    // a regra de negocio nao deveria conhecer detalhes internos
    // de como os dados sao armazenados.
    //
    // O que torna este arquivo "certo" e exatamente isso:
    // a classe Research nao depende mais da classe concreta RelationShips.
    // Agora ela depende de uma abstracao.

    public enum RelationShip2
    {
        Parent,
        Child,
        Sibling,
    }

    public class Person
    {
        public string Name { get; set; } = string.Empty;
        // public dateTime DateOfBirth {get; set;}
    }

    // O que e uma abstracao aqui?
    // E um contrato que diz apenas o que o modulo de alto nivel precisa.
    // Neste caso, Research so precisa saber buscar filhos de uma pessoa.
    // Ele nao precisa saber se os dados estao em lista, banco, arquivo ou API.
    public interface IRelationshipBrowser
    {
        IEnumerable<Person> FindAllChildrenOf(string name);
    }

    // Low-level
    // Esta classe continua sendo o modulo de baixo nivel.
    // Ela ainda decide como guardar os dados internamente.
    // A diferenca e que agora ela tambem implementa a abstracao
    // que o modulo de alto nivel entende.
    public class RelationShips : IRelationshipBrowser
    {
        private readonly List<(Person, RelationShip2, Person)> relations
            = new List<(Person, RelationShip2, Person)>();

        public void AddParentAndChild(Person parent, Person child)
        {
            relations.Add((parent, RelationShip2.Parent, child));
            relations.Add((child, RelationShip2.Child, parent));
        }

        // Aqui a classe de baixo nivel transforma seus detalhes internos
        // em uma resposta mais limpa para quem esta acima.
        //
        // Repare no efeito da mudanca:
        // Research nao enxerga mais List, tupla, Item1, Item2 ou Item3.
        // Ele recebe apenas pessoas ja filtradas pela abstracao.
        public IEnumerable<Person> FindAllChildrenOf(string name)
        {
            foreach (var relation in relations)
            {
                if (relation.Item1.Name == name && relation.Item2 == RelationShip2.Parent)
                {
                    yield return relation.Item3;
                }
            }
        }
    }

    // High-level
    // Esta e a parte da regra de negocio.
    // Ela quer pesquisar informacoes, nao quer saber como os dados foram guardados.
    public class Research
    {
        // Por que certo.cs e certo?
        //
        // Porque Research recebe IRelationshipBrowser,
        // e nao a classe concreta RelationShips.
        //
        // Ou seja:
        // o modulo de alto nivel depende de uma abstracao.
        // E o modulo de baixo nivel tambem depende dessa mesma abstracao,
        // ao implementa-la.
        public Research(IRelationshipBrowser browser)
        {
            foreach (var person in browser.FindAllChildrenOf("John"))
            {
                WriteLine($"John has a child called {person.Name}");
            }
        }

        public static void RunDemo()
        {
            var parent = new Person { Name = "John" };
            var child1 = new Person { Name = "Chris" };
            var child2 = new Person { Name = "Matt" };

            var relationships = new RelationShips();
            relationships.AddParentAndChild(parent, child1);
            relationships.AddParentAndChild(parent, child2);

            new Research(relationships);
        }
    }

    // Qual foi a mudanca que tornou o codigo certo?
    //
    // 1. Antes, Research dependia diretamente de RelationShips2.
    // 2. Agora, Research depende de IRelationshipBrowser.
    // 3. RelationShips continua existindo, mas ficou escondido atras de uma abstracao.
    // 4. Research nao conhece mais o formato interno dos dados.
    // 5. Se o armazenamento mudar, Research pode continuar igual,
    //    desde que a nova implementacao continue respeitando a interface.
}
