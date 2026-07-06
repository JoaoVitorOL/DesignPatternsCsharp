
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


        public object Clone()
        {
            
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
            var jane = john;

            jane.Names[0] = "Jane";
            jane.Address.HouseNumber = 321;

            Console.WriteLine(john);
            Console.WriteLine(jane);
        }
    }
    
}
