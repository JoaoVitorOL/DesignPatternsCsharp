using System;
using static System.Console;

namespace aula04_PrototypeInhertance
{
  // ==============
  // Contexto do exemplo
  // ==============
  // Este exemplo retrata um cenário em que um objeto base, como Person,
  // é usado como protótipo para criar versões derivadas, como Employee.
  // A ideia é mostrar como a clonagem profunda pode ser aplicada em uma hierarquia
  // de classes sem perder o isolamento entre as instâncias.
  // ==============
  // Interface de cópia explícita
  // ==============
  // Define um contrato simples para a operação de clonagem profunda.
  // A ideia é deixar explícito que a cópia deve ser feita de forma independente.
  public interface IPrototype<T> where T : new()
  {
    void CopyTo(T target);
    
    public T DeepCopy()
    {
      T t = new T();
      CopyTo(t);
      return t;
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
    // Recebe os valores iniciais do endereço e os armazena no objeto.
    public Address(string streetName, int houseNumber)
    {
      StreetName = streetName;
      HouseNumber = houseNumber;
    }

    public Address()
    {
      
    }

    public override string ToString()
    {
      return $"{nameof(StreetName)}: {StreetName}, {nameof(HouseNumber)}: {HouseNumber}";
    }

    // ==============
    // Implementação da cópia profunda
    // ==============
    // Copia os dados do endereço para outra instância, sem compartilhar o mesmo estado.
    public void CopyTo(Address target)
    {
      target.StreetName = StreetName;
      target.HouseNumber = HouseNumber;
    }
  }



  // ==============
  // Classe Person
  // ==============
  // Aqui começa o Prototype na hierarquia: Person é a classe base do protótipo.
  // Ela define o estado comum e a lógica inicial de clonagem que será reutilizada.
  public class Person : IPrototype<Person>
  {
    public string[] Names;
    public Address Address;

    // ==============
    // Construtor padrão
    // ==============
    // Permite criar uma instância base antes de configurar seus valores.
    public Person()
    {
      
    }
    
    // ==============
    // Construtor principal
    // ==============
    // Recebe os dados básicos da pessoa e prepara o estado inicial do objeto.
    public Person(string[] names, Address address)
    {
      Names = names;
      Address = address;
    }

    public override string ToString()
    {
      return $"{nameof(Names)}: {string.Join(",", Names)}, {nameof(Address)}: {Address}";
    }

    // ==============
    // Implementação da cópia profunda
    // ==============
    // Copia os nomes e também cria uma cópia do endereço, preservando o isolamento do clone.
    public virtual void CopyTo(Person target)
    {
      target.Names = (string[]) Names.Clone();
      target.Address = Address.DeepCopy();
    }
  }

  // ==============
  // Classe Employee
  // ==============
  // Aqui está a herança do Prototype: Employee herda de Person e estende o comportamento
  // de cópia para incluir o estado específico da subclasse, como o salário.
  public class Employee : Person, IPrototype<Employee>
  {
    public int Salary;

    // ==============
    // Implementação da cópia na subclasse
    // ==============
    // Este é o ponto exato em que a herança do Prototype aparece:
    // a subclasse reutiliza a lógica da classe base com base.CopyTo(target)
    // e depois adiciona o que é próprio dela.
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
  // Extensões de cópia
  // ==============
  // Torna a operação de clonagem mais natural de ser chamada no código cliente.
  public static class DeepCopyExtensions
  {
    public static T DeepCopy<T>(this IPrototype<T> item) 
      where T : new()
    {
      return item.DeepCopy();
    }

    public static T DeepCopy<T>(this T person)
      where T : Person, new()
    {
      return ((IPrototype<T>) person).DeepCopy();
    }
  }
  
  // ==============
  // Demonstração
  // ==============
  // Mostra o comportamento do Prototype em uma hierarquia de classes.
  public static class Demo
  {
    static void Main()
    {
      var john = new Employee();
      john.Names = new[] {"John", "Doe"};
      john.Address = new Address {HouseNumber = 123, StreetName = "London Road"};
      john.Salary = 321000;
      var copy = john.DeepCopy();

      copy.Names[1] = "Smith";
      copy.Address.HouseNumber++;
      copy.Salary = 123000;
      
      Console.WriteLine(john);
      Console.WriteLine(copy);
    }
  }
}