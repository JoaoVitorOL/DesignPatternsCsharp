using System;
using System.Collections.Generic;
using System.Linq;
using static System.Console;

namespace Aula05_FacetedBuilder
{
    // Produto final com dados de facetas diferentes.
    // Neste exemplo, a mesma Person tem uma faceta de endereco
    // e outra faceta profissional.

    // ===== Classe =====
    public class Person
    {
        // ===== Campos =====
        public string StreetAddress = string.Empty,
            PostCode = string.Empty,
            City = string.Empty;

        public string CompanyName = string.Empty,
            Position = string.Empty;

        public int AnnualIncome;

        // ===== Metodos =====
        public override string ToString()
        {
            return $"{nameof(StreetAddress)}: {StreetAddress}, {nameof(PostCode)}: {PostCode},{nameof(City)}: {City}";
        }
    }

    // "Faceta", neste contexto, significa:
    // um recorte especializado da construcao do MESMO objeto.
    //
    // Exemplo desta aula:
    // - a faceta de endereco cuida de StreetAddress, PostCode e City
    // - a faceta profissional cuida de CompanyName, Position e AnnualIncome
    //
    // Entao uma faceta NAO e um produto separado.
    // Ela e apenas uma parte da API responsavel por uma area especifica da Person.
    //
    // Este builder raiz nao tenta expor todos os metodos de uma vez.
    // A diferenca para o Fluent Builder "comum" esta aqui:
    // a API nao cresce como uma lista unica de metodos no mesmo builder;
    // ela e quebrada em builders menores, cada um responsavel por uma faceta.
    //
    // Diferenca para o Stepwise Builder:
    // `Lives` e `Works` nao estao forçando uma ordem obrigatoria.
    // O cliente pode alternar entre facetas conforme fizer sentido.
    //
    // Diferenca para o Functional Builder:
    // aqui nao acumulamos acoes para aplicar depois.
    // Cada faceta altera a mesma Person compartilhada imediatamente.

    // ===== Builder =====
    public class PersonBuilder
    {
        // ===== Campos =====
        protected Person person = new Person();

        // Cada propriedade abre uma "porta" para uma area da construcao.
        public PersonJobBuilder Works => new PersonJobBuilder(person);
        public PersonAddressBuilder Lives => new PersonAddressBuilder(person);

        // Permite encerrar a cadeia devolvendo o mesmo produto compartilhado,
        // independentemente de qual faceta foi a ultima usada.

        // ===== Metodos =====
        public static implicit operator Person(PersonBuilder builder)
        {
            return builder.person;
        }
    }

    // ===== Builder =====
    public class PersonAddressBuilder : PersonBuilder
    {
        // Recebemos a instancia ja existente para continuar montando
        // o MESMO objeto, e nao criar uma Person nova.

        // ===== Construtores =====
        public PersonAddressBuilder(Person person)
        {
            this.person = person;
        }

        // Esta faceta sabe apenas de endereco.

        // ===== Metodos =====
        public PersonAddressBuilder At(string streetAddress)
        {
            person.StreetAddress = streetAddress;
            return this;
        }


        public PersonAddressBuilder WithPostCode(string postcode)
        {
            person.PostCode = postcode;
            return this;
        }

        public PersonAddressBuilder In(string city)
        {
            person.City = city;
            return this;
        }
    }

    // ===== Builder =====
    public class PersonJobBuilder : PersonBuilder
    {
        // O mesmo padrao da faceta de endereco:
        // reaproveitamos a mesma Person compartilhada.

        // ===== Construtores =====
        public PersonJobBuilder(Person person)
        {
            this.person = person;
        }

        // Esta faceta cuida apenas da parte profissional.

        // ===== Metodos =====
        public PersonJobBuilder At(string companyName)
        {
            person.CompanyName = companyName;
            return this;
        }

        public PersonJobBuilder AsA(string position)
        {
            person.Position = position;
            return this;
        }


        public PersonJobBuilder Earning(int amount )
        {
            person.AnnualIncome = amount;
            return this;
        }
    }


    // ===== Classe =====
    public class Demo
    {
        // ===== Metodos =====
        static void Main(string[] args)
        {
            var pb = new PersonBuilder();

            // A leitura correta desta cadeia e:
            // "estou montando uma unica Person, mas navegando entre facetas".
            Person person = pb
             .Lives.At("123 London Road")
               .In("London")
               .WithPostCode("Wexa14320afr")
             .Works.At("Fabikram")
             .AsA("Engineer")
             .Earning(12300);

            WriteLine(person);
            WriteLine("fim do codigo");
        }
    }
}
