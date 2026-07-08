using System;
using System.Text;
using static System.Console;

namespace aula05_PrototypeSerialization
{
  // ==============
  // Contexto do exemplo
  // ==============
  // Este exemplo continua a ideia do Prototype, mas tenta mostrar uma alternativa
  // para copiar um objeto através de uma forma de serialização binária manual.
  // A intenção é manter o conceito do padrão, mesmo sem depender do BinaryFormatter,
  // que hoje é incompatível com o runtime moderno.
  // ==============
  // Extensões de cópia
  // ==============
  // Aqui criamos uma extensão chamada DeepCopy que transforma o objeto em um fluxo
  // binário e depois reconstrói outro objeto a partir desse estado.
  // Esse padrão é útil para demonstrar o conceito de clonagem baseada em estado.
  public static class ExtensionsMethods
    {
        // ==============
        // Método principal de clonagem
        // ==============
        // O fluxo binário funciona como uma espécie de "snapshot" do objeto.
        // Em vez de criar a cópia manualmente, escrevemos o estado em bytes e depois
        // reconstruímos um novo objeto a partir dessa representação.
        public static T DeepCopy<T>(this T self)
        {
            if (self is null)
                throw new ArgumentNullException(nameof(self));

            using var stream = new MemoryStream();
            using (var writer = new BinaryWriter(stream, Encoding.UTF8, true))
            {
                WriteValue(writer, self);
            }

            stream.Position = 0;

            using (var reader = new BinaryReader(stream, Encoding.UTF8, true))
            {
                return (T)ReadValue(reader, typeof(T))!;
            }
        }

        // ==============
        // Serialização manual
        // ==============
        // Este método escreve o estado do objeto em um formato simples.
        // Ele usa marcas como "Employee", "Person" e "Address" para identificar o tipo
        // e depois grava os campos relevantes.
        private static void WriteValue(BinaryWriter writer, object value)
        {
            switch (value)
            {
                case Employee employee:
                    writer.Write("Employee");
                    writer.Write(employee.Names?.Length ?? 0);
                    foreach (var name in employee.Names ?? Array.Empty<string>())
                        writer.Write(name);

                    writer.Write(employee.Address != null);
                    if (employee.Address != null)
                        WriteValue(writer, employee.Address);

                    writer.Write(employee.Salary);
                    break;

                case Person person:
                    writer.Write("Person");
                    writer.Write(person.Names?.Length ?? 0);
                    foreach (var name in person.Names ?? Array.Empty<string>())
                        writer.Write(name);

                    writer.Write(person.Address != null);
                    if (person.Address != null)
                        WriteValue(writer, person.Address);
                    break;

                case Address address:
                    writer.Write("Address");
                    writer.Write(address.StreetName ?? string.Empty);
                    writer.Write(address.HouseNumber);
                    break;

                default:
                    throw new NotSupportedException("This demo only supports Address, Person and Employee.");
            }
        }

        // ==============
        // Desserialização manual
        // ==============
        // Aqui o fluxo é lido de volta. Cada marca indica qual tipo deve ser reconstruído,
        // e o método correspondente monta um novo objeto com o estado salvo.
        private static object ReadValue(BinaryReader reader, Type type)
        {
            var marker = reader.ReadString();

            return marker switch
            {
                "Address" => new Address(reader.ReadString(), reader.ReadInt32()),
                "Person" => ReadPerson(reader),
                "Employee" => ReadEmployee(reader),
                _ => throw new InvalidOperationException("Unsupported binary payload.")
            };
        }

        private static Person ReadPerson(BinaryReader reader)
        {
            var count = reader.ReadInt32();
            var names = new string[count];
            for (var i = 0; i < count; i++)
                names[i] = reader.ReadString();

            var hasAddress = reader.ReadBoolean();
            Address address = hasAddress
                ? ReadAddress(reader)
                : new Address();

            return new Person(names, address);
        }

        private static Employee ReadEmployee(BinaryReader reader)
        {
            var count = reader.ReadInt32();
            var names = new string[count];
            for (var i = 0; i < count; i++)
                names[i] = reader.ReadString();

            var hasAddress = reader.ReadBoolean();
            Address address = hasAddress
                ? ReadAddress(reader)
                : new Address();

            var salary = reader.ReadInt32();
            return new Employee(names, address, salary);
        }

        private static Address ReadAddress(BinaryReader reader)
        {
            var marker = reader.ReadString();
            if (marker != "Address")
                throw new InvalidOperationException("Expected an Address payload.");

            return new Address(reader.ReadString(), reader.ReadInt32());
        }
    }


[Serializable]
  // ==============
  // Classe Address
  // ==============
  // Representa um valor composto usado dentro da pessoa.
  // Aqui o objeto é simples, mas mostra o tipo de estado que pode ser copiado.
  public class Address
  {
    public string StreetName = string.Empty;
    public int HouseNumber;


    public Address(string streetName, int houseNumber)
    {
      StreetName = streetName ?? string.Empty;
      HouseNumber = houseNumber;
    }

    public Address()
    {
      
    }

    public override string ToString()
    {
      return $"{nameof(StreetName)}: {StreetName}, {nameof(HouseNumber)}: {HouseNumber}";
    }


    public void CopyTo(Address target)
    {
      target.StreetName = StreetName;
      target.HouseNumber = HouseNumber;
    }
  }



[Serializable]
  // ==============
  // Classe Person
  // ==============
  // Esta entidade possui estado próprio e também um objeto composto, o Address.
  // O importante aqui é que a cópia precisa não apenas duplicar os dados, mas
  // também garantir que a nova instância se torne independente da antiga.
  public class Person 
  {
    public string[] Names = Array.Empty<string>();
    public Address Address = new Address();


    public Person()
    {
      
    }
    
 
    public Person(string[] names, Address address)
    {
      Names = names ?? Array.Empty<string>();
      Address = address ?? new Address();
    }

    public override string ToString()
    {
      return $"{nameof(Names)}: {string.Join(",", Names)}, {nameof(Address)}: {Address}";
    }

   
    public virtual void CopyTo(Person target)
    {
      target.Names = (string[]) Names.Clone();
      target.Address = Address.DeepCopy();
    }
  }


[Serializable]
  // ==============
  // Classe Employee
  // ==============
  // Aqui a herança entra em cena. Employee herda de Person e acrescenta um valor
  // específico, que também precisa ser preservado na cópia.
  public class Employee : Person
  {
    public int Salary;

    public Employee()
    {
    }

    public Employee(string[] names, Address address, int salary) : base(names, address)
    {
      Salary = salary;
    }

    public void CopyTo(Employee target)
    {
      base.CopyTo(target);
      target.Salary = Salary;
    }

    public override string ToString()
    {
      return $"{base.ToString()}, {nameof(Salary)}: {Salary}";
    }
  }
  

  
  // ==============
  // Demonstração
  // ==============
  // O protótipo é criado com um estado já configurado e, em seguida, clonamos esse
  // objeto para gerar uma nova instância. Depois alteramos apenas a cópia para mostrar
  // que ela ficou independente do objeto original.
  public static class Demo
  {
    static void Main()
    {
      var john = new Employee();
      john.Names = new[] {"John", "Doe"};
      john.Address = new Address {HouseNumber = 123, StreetName = "London Road"};
      john.Salary = 321000;
      var jane = john.DeepCopy();

      jane.Names[0] = "Jane";
      jane.Names[1] = "Smith";
      jane.Address.HouseNumber++;
      jane.Salary = 123000;
      
      Console.WriteLine(john);
      Console.WriteLine(jane);
    }
  }
}