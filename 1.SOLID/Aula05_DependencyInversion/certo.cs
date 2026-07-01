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
    // a classe Research nao depende mais da classe concreta Relationships.
    // Agora ela depende de uma abstracao.

    // ===== Enum =====
    public enum RelationshipType2
    {
        Parent,
        Child,
        Sibling,
    }

    // ===== Classe =====
    public class Person
    {
        // ===== Propriedades =====
        public string Name { get; set; } = string.Empty;
        // public dateTime DateOfBirth {get; set;}
    }

    // O que e uma abstracao aqui?
    // E um contrato que diz apenas o que o modulo de alto nivel precisa.
    // Neste caso, Research so precisa saber buscar filhos de uma pessoa.
    // Ele nao precisa saber se os dados estao em lista, banco, arquivo ou API.

    // ===== Interface =====
    public interface IRelationshipReader
    {
        // O nome do metodo ja expressa a pergunta de negocio.
        // Quem consome este contrato nao precisa saber nada
        // sobre List, tupla, banco, SQL ou qualquer estrutura interna.

        // ===== Metodos =====
        IEnumerable<Person> FindChildrenOf(string parentName);
    }

    // Low-level
    // Esta classe continua sendo o modulo de baixo nivel.
    // Ela ainda decide como guardar os dados internamente.
    // A diferenca e que agora ela tambem implementa a abstracao
    // que o modulo de alto nivel entende.

    // ===== Classe =====
    public class Relationships : IRelationshipReader
    {
        // O formato interno continua existindo.
        // A diferenca e que agora ele fica escondido dentro do modulo de baixo nivel.

        // ===== Campos =====
        private readonly List<(Person From, RelationshipType2 Type, Person To)> relations
            = new List<(Person From, RelationshipType2 Type, Person To)>();

        // ===== Metodos =====
        public void AddParentAndChild(Person parent, Person child)
        {
            relations.Add((parent, RelationshipType2.Parent, child));
            relations.Add((child, RelationshipType2.Child, parent));
        }

        // Aqui a classe de baixo nivel transforma seus detalhes internos
        // em uma resposta mais limpa para quem esta acima.
        //
        // Repare no efeito da mudanca:
        // Research nao enxerga mais List, tupla, From, Type ou To.
        // Ele recebe apenas pessoas ja filtradas pela abstracao.
        public IEnumerable<Person> FindChildrenOf(string parentName)
        {
            foreach (var relation in relations)
            {
                if (relation.From.Name == parentName && relation.Type == RelationshipType2.Parent)
                {
                    // yield return devolve cada filho sob demanda.
                    // Para o consumidor, isso continua parecendo apenas
                    // uma sequencia de Person.
                    yield return relation.To;
                }
            }
        }
    }

    // High-level
    // Esta e a parte da regra de negocio.
    // Ela quer pesquisar informacoes, nao quer saber como os dados foram guardados.

    // ===== Classe =====
    public class Research
    {
        // Por que certo.cs e certo?
        //
        // Porque Research recebe IRelationshipReader,
        // e nao a classe concreta Relationships.
        //
        // Ou seja:
        // o modulo de alto nivel depende de uma abstracao.
        // E o modulo de baixo nivel tambem depende dessa mesma abstracao,
        // ao implementa-la.

        // ===== Construtores =====
        public Research(IRelationshipReader relationshipReader)
        {
            // Research faz a pergunta de negocio diretamente.
            // Ele nao filtra lista, nao conhece tupla e nao conhece armazenamento.
            foreach (var person in relationshipReader.FindChildrenOf("John"))
            {
                WriteLine($"John has a child called {person.Name}");
            }
        }

        // ===== Metodos =====
        public static void RunDemo()
        {
            var parent = new Person { Name = "John" };
            var child1 = new Person { Name = "Chris" };
            var child2 = new Person { Name = "Matt" };

            // Aqui a classe concreta e escolhida.
            // Mas ela entra no alto nivel pela interface, e nao pelo tipo concreto.
            var relationships = new Relationships();
            relationships.AddParentAndChild(parent, child1);
            relationships.AddParentAndChild(parent, child2);

            IRelationshipReader relationshipReader = relationships;
            new Research(relationshipReader);
        }
    }

    // Qual foi a mudanca que tornou o codigo certo?
    //
    // 1. Antes, Research dependia diretamente de Relationships2.
    // 2. Agora, Research depende de IRelationshipReader.
    // 3. Relationships continua existindo, mas ficou escondido atras de uma abstracao.
    // 4. Research nao conhece mais o formato interno dos dados.
    // 5. Se o armazenamento mudar, Research pode continuar igual,
    //    desde que a nova implementacao continue respeitando a interface.
}
