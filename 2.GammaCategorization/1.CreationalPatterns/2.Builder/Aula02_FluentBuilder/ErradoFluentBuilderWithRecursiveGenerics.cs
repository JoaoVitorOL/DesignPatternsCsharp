// Exemplo da aula:
// um fluent builder para montar uma `Person` com nome e cargo.
// Este arquivo mostra a tentativa errada com heranca simples:
// a chamada fluent quebra porque o metodo da base retorna o tipo da base.
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

        // ===== Metodos =====
        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}, {nameof(Position)}: {Position}";
        }
    }

    // ===== Builder =====
    public class PersonInfoBuilder
    {
        // ===== Campos =====
        protected Person person = new Person();

        // Aqui nasce o problema:
        // `Called()` retorna `PersonInfoBuilder`.
        // Se a cadeia comecou em `PersonJobBuilder`, esse retorno
        // rebaixa a expressao para o tipo base.

        // ===== Metodos =====
        public PersonInfoBuilder Called(string name)
        {
            person.Name = name;
            return this;
        }
    }

    // Herdar da classe base reaproveita os metodos dela,
    // mas nao muda o tipo de retorno que ela ja definiu.

    // ===== Builder =====
    public class PersonJobBuilder : PersonInfoBuilder
    {
        // ===== Metodos =====
        public PersonJobBuilder workAsA(string position)
        {
            person.Position = position;
            return this;
        }
    }

    // ===== Classe =====
    internal class Program
    {
        // ===== Metodos =====
        public static void Main(string[] args)
        {
            var builder = new PersonJobBuilder();

            // Depois de `Called()`, o tipo da cadeia vira `PersonInfoBuilder`.
            // Por isso `workAsA()` deixa de estar disponivel aqui.
            builder.Called("Dmitri")
                .workAsA("Professor");
        }
    }
}
