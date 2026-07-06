
using System;

namespace aula02_CopyConstructors
{
    // ==============
    // Classe Person
    // ==============
    // Representa um objeto que pode ser copiado a partir de outro objeto.
    // O construtor de cópia é uma forma explícita de implementar Prototype.
    public class Person
    {
        public string[] Names;
        public Address Address;

        // ==============
        // Construtor principal
        // ==============
        // Recebe os valores iniciais e valida se eles foram informados corretamente.
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

            Names = names;
            Address = address;
        }

        // ==============
        // Construtor de cópia
        // ==============
        // Cria uma nova instância a partir de uma existente.
        // Aqui, fazemos uma cópia do array para evitar que as duas instâncias
        // compartilhem a mesma coleção de nomes.
        public Person(Person other)
        {
            Names = (string[])other.Names.Clone();
            Address = new Address(other.Address);
        }

        public override string ToString()
        {
            return $"Name: {string.Join(" ", Names)}, Address: {Address.StreetName}, {Address.HouseNumber}";
        }
    }

    // ==============
    // Classe Address
    // ==============
    // Representa um valor composto que também precisa ser copiado corretamente.
    public class Address
    {
        public string StreetName;
        public int HouseNumber;

        // ==============
        // Construtor principal
        // ==============
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
        // Construtor de cópia
        // ==============
        // Cria uma nova Address a partir de outra, preservando os dados sem compartilhar a mesma instância.
        public Address(Address otherAddress)
        {
            StreetName = otherAddress.StreetName;
            HouseNumber = otherAddress.HouseNumber;
        }
    }

    static class Program
    {
        static void Main(string[] args)
        {
            // Exemplo inicial: John é a pessoa base.
            var john = new Person(new string[] { "John", "Smith" },
                new Address("London Road", 123));

            // Cria uma cópia de John.
            var jane = new Person(john);

            // Modificações posteriores devem afetar apenas a cópia.
            jane.Names[0] = "Jane";
            jane.Address.HouseNumber = 321;

            Console.WriteLine(john);
            Console.WriteLine(jane);
        }
    }
}
