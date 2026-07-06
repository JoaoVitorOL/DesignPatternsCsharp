//  Exercicio:
//  implemente uma factory nao estatica para criar objetos Person.
//
//  Importante:
//  - o objetivo nao e apenas chamar `new Person(...)`
//  - o objetivo e centralizar a regra de criacao dentro de uma factory
//  - cada Person criada deve receber um Id automatico
//  - o Id deve seguir uma sequencia baseada em zero
//
//  Em outras palavras, a factory deve:
//  1. receber apenas o nome da pessoa
//  2. criar uma nova instancia de Person
//  3. atribuir o Id automaticamente
//  4. incrementar o contador para a proxima pessoa criada
//
//  A API esperada para o uso da factory e esta:
//
//  var factory = new PersonFactory();
//  var person1 = factory.CreatePerson("Alice");
//  var person2 = factory.CreatePerson("Bob");
//
//  O resultado esperado e:
//
//  person1.Id == 0
//  person2.Id == 1
//
//  Observe os detalhes pedidos no enunciado:
//  - PersonFactory nao deve ser estatica
//  - CreatePerson deve receber o nome da pessoa
//  - o cliente nao deve informar o Id manualmente
//  - a propria factory deve controlar a sequencia dos Ids


using System;

namespace aula09_FactoryExcercise
{
    // 1o Defina o produto que sera criado pela factory.

    // ===== Classe do produto =====
    public class Person
    {
        // ===== Propriedades =====
        public int Id { get; set; }
        public string Name { get; set; }

        // ===== Construtores =====
        internal Person(int id, string name)
        {
            // 2o No produto, receba os dados ja decididos pela factory.
            Id = id;
            Name = name;
        }
    }

    // 3o Crie a factory que vai centralizar a regra de criacao.

    // ===== Factory =====
    public class PersonFactory
    {
        // 4o Toda factory com criacao sequencial precisa guardar seu estado interno.

        // ===== Campos =====
        private int counter = 0;

        // ===== Factory Method =====
        public Person CreatePerson(string name)
        {
            // 5o O cliente informa apenas o nome; a factory decide o Id.

            // 6o `counter++` usa o valor atual como Id e depois incrementa para a proxima criacao.
            return new Person(counter++, name);
        }
    }

    // ===== Classe =====
    public class Demo
    {
        // ===== Metodos =====
        public static void Main(string[] args)
        {
            // 7o O cliente cria uma factory concreta para controlar esta sequencia de pessoas.
            var factory = new PersonFactory();

            // 8o O cliente pede pessoas pelo nome, sem informar Id manualmente.
            var person1 = factory.CreatePerson("Alice");
            var person2 = factory.CreatePerson("Bob");
            var person3 = factory.CreatePerson("Charlie");

            // 9o Cada pessoa criada pela mesma factory recebe o proximo Id da sequencia.
            Console.WriteLine($"Person 1: Id={person1.Id}, Name={person1.Name}");
            Console.WriteLine($"Person 2: Id={person2.Id}, Name={person2.Name}");
            Console.WriteLine($"Person 3: Id={person3.Id}, Name={person3.Name}");
        }
    }
}
