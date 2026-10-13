
using System;

namespace aula01_ICloneableIsBad
{
// ==============
// Classe
// ==============
    public class Person : ICloneable
    {
        public string[] Names;
        public Address Address;

// ==============
// Constructor
// ==============
        public Person(string [] names, Address address)
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


        public override string ToString()
        {
            return $"Name: {string.Join(" ", Names)}, Address: {Address.StreetName}, {Address.HouseNumber}";
        }


        // ==============
        // Implementação de ICloneable
        // ==============
        // Este método retorna object, o que força um cast manual no código cliente.
        // Isso torna a API menos segura e menos explícita do que seria desejável.
        // Além disso, a interface não diz se a cópia deve ser rasa ou profunda.
        public object Clone()
        {
            // ==============
            // Cópia rasa
            // ==============
            // MemberwiseClone() cria uma nova instância, mas mantém as referências internas.
            // Ou seja, Names e Address podem continuar compartilhados com o objeto original.
            // Esse é um exemplo clássico do porquê ICloneable é confuso: o significado de "clone"
            // fica ambíguo e pode gerar efeitos colaterais inesperados.
            return MemberwiseClone();
        }

    }


// ==============
// Classe
// ==============
    public class Address
    {
        public string StreetName;
        public int HouseNumber;

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

    }



    static class Program
    {
        static void Main(string[] args)
        {
            var john = new Person(new string[] { "John", "Smith" },
             new Address("London Road", 123));

            // ==============
            // Exemplo de uso problemático
            // ==============
            // Esta chamada usa ICloneable, mas o contrato é fraco e não informa o tipo real de cópia.
            // O código precisa fazer cast manualmente e ainda assim não sabe se a cópia é rasa ou profunda.
            var jane = (Person)john.Clone();

            // ==============
            // Efeito colateral típico
            // ==============
            // Como a cópia é rasa, as alterações aqui podem afetar o objeto original.
            // Isso mostra por que a semântica de ICloneable é ruim para um padrão como Prototype.
            jane.Names[0] = "Jane";
            jane.Address.HouseNumber = 321;

            Console.WriteLine(john);
            Console.WriteLine(jane);
        }
    }
    
}
