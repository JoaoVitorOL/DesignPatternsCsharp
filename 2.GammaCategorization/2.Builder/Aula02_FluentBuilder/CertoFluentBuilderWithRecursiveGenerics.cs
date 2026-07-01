// Exemplo da aula:
// o mesmo builder de `Person`, agora corrigido com recursive generics.
// A ideia e manter o tipo concreto do builder durante toda a cadeia fluent.
using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;

namespace Aula02_FluentBuilder
{
    // ===== Classe =====
    public class Person
    {
        // ===== Campos =====
        public string Name;
        public string Position;

        // Fecha o ciclo da hierarquia:
        // o builder concreto final desta cadeia sera `Builder`.

        // ===== Builder =====
        public class Builder : PersonJobBuilder<Builder>
        {
        }

        public static Builder New => new Builder();

        // ===== Metodos =====
        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}, {nameof(Position)}: {Position}";
        }
    }

    // ===== Builder =====
    public abstract class PersonBuilder
    {
        // ===== Campos =====
        protected Person person = new Person();

        // ===== Metodos =====
        public Person Build()
        {
            return person;
        }
    }

    // `SELF` significa: "qual e o tipo concreto do builder nesta cadeia?"
    // A restricao garante que o tipo usado realmente pertence a esta familia de builders.

    // ===== Builder =====
    public class PersonInfoBuilder<SELF> : PersonBuilder
        where SELF : PersonInfoBuilder<SELF>
    {
        // Aqui o retorno ja nao e mais a classe base fixa.
        // O metodo devolve o tipo concreto esperado pela cadeia.

        // ===== Metodos =====
        public SELF Called(string name)
        {
            person.Name = name;
            return (SELF)this;
        }
    }

    // Aqui esta a correcao principal:
    // `PersonJobBuilder<SELF>` ainda recebe o tipo concreto final como parametro.
    //
    // `: PersonInfoBuilder<PersonJobBuilder<SELF>>` diz ao builder de info:
    // "quando voce devolver SELF na sua API, devolva um `PersonJobBuilder<SELF>`".
    // Com isso, depois de `Called()`, a cadeia continua em um tipo
    // que ainda conhece `workAsA()`.
    //
    // `where SELF : PersonJobBuilder<SELF>` garante que o tipo final
    // tambem pertence a esta camada da hierarquia.
    // No nosso caso, esse `SELF` sera `Person.Builder`.

    // ===== Builder =====
    public class PersonJobBuilder<SELF>
        : PersonInfoBuilder<PersonJobBuilder<SELF>>
        where SELF : PersonJobBuilder<SELF>
    {
        // Depois de configurar o cargo, devolvemos o tipo concreto final.

        // ===== Metodos =====
        public SELF workAsA(string position)
        {
            person.Position = position;
            return (SELF)this;
        }
    }

    // ===== Classe =====
    internal class Program
    {
        // ===== Metodos =====
        public static void Main(string[] args)
        {
            // Agora `Called()` nao degrada a cadeia para a classe base.
            // O tipo continua compativel com `workAsA()` e depois com `Build()`.
            var me = Person.New
                .Called("Dmitri")
                .workAsA("Professor")
                .Build();

            Console.WriteLine(me);
        }
    }
}
