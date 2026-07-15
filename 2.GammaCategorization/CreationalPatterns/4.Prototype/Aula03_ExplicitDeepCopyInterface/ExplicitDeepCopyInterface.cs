
using System;

namespace aula03_ExplicitDeepCopyInterface
{

    // ==============
    // Contexto do exemplo
    // ==============
    // Este exemplo retrata um cenário em que uma pessoa já configurada
    // serve como protótipo para criar outra instância com estado independente.
    // A ideia é mostrar como o padrão Prototype pode ser implementado com uma
    // interface explícita para cópia profunda, sem depender de ICloneable.
    // ==============
    // Interface de cópia explícita
    // ==============
    // Define um contrato claro para a operação de clonagem profunda.
    // A intenção é deixá-la explícita no código, em vez de depender de uma
    // semântica ambígua como a de ICloneable.
    public interface IPrototype<T>
    {
        T DeepCopy();
    }

    // ==============
    // Classe Person
    // ==============
    // Representa um objeto que possui estado interno e pode ser copiado
    // de forma independente para servir como protótipo.
    public class Person : IPrototype<Person>
    {
        public string[] Names;
        public Address Address;

        // ==============
        // Construtor principal
        // ==============
        // Recebe os valores iniciais e valida se eles foram informados corretamente.
        // Aqui, a cópia é feita de forma explícita para evitar compartilhamento de referências.
        public Person(string[] names, Address address)
        {
            if (names == null)
            {
                throw new ArgumentNullException(paramName: nameof(names));
            }
            if (address == null)
            {
                throw new ArgumentNullException(paramName: nameof(address));
            }

            Names = (string[])names.Clone();
            Address = address.DeepCopy();
        }


        public override string ToString()
        {
            return $"Name: {string.Join(" ", Names)}, Address: {Address.StreetName}, {Address.HouseNumber}";
        }


        // ==============
        // Implementação da cópia profunda
        // ==============
        // Este método cria uma nova Person e também duplica o objeto Address.
        // A ideia é garantir que a cópia seja totalmente independente.
        public Person DeepCopy()
        {
            return new Person(Names, Address.DeepCopy());
        }


    }

    // ==============
    // Classe Address
    // ==============
    // Representa um objeto composto que também precisa ser copiado corretamente.
    public class Address : IPrototype<Address>
    {
        public string StreetName;
        public int HouseNumber;

        // ==============
        // Construtor principal
        // ==============
        // Inicia os valores de endereço de forma simples e valida o estado.
        public Address(string streetName, int houseNumber)
        {
            if (streetName == null)
            {
                throw new ArgumentNullException(paramName: nameof(streetName));
            }
            if (houseNumber <= 0)
            {
                throw new ArgumentOutOfRangeException(paramName: nameof(houseNumber));
            }

            StreetName = streetName;
            HouseNumber = houseNumber;
        }



        // ==============
        // Implementação da cópia profunda para Address
        // ==============
        // Cria um novo endereço com os mesmos valores, sem compartilhar a mesma instância.
        public Address DeepCopy()
        {
            return new Address(StreetName, HouseNumber);
        }
    }

    static class Program
    {
        static void Main(string[] args)
        {
            // Exemplo inicial: John é a pessoa base e representa o protótipo.
            var john = new Person(new string[] { "John", "Smith" },
                new Address("London Road", 123));

            // Cria uma cópia profunda de John usando a interface explícita.
            var jane = john.DeepCopy();

            // Modificações posteriores devem afetar apenas a cópia.
            jane.Names[0] = "Jane";
            jane.Address.HouseNumber = 321;

            Console.WriteLine(john);
            Console.WriteLine(jane);
        }
    }
}
