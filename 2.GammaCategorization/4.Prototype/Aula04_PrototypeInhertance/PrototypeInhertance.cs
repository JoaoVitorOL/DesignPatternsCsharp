using System;
using static System.Console;

namespace DotNetDesignPatternDemos.Creational.Prototype.Inheritance
{
  // ==============
  // Interface de cópia explícita
  // ==============
  // Define um contrato simples para a operação de clonagem profunda.
  // A ideia é deixar explícito que a cópia deve ser feita de forma independente.
  public interface IDeepCopyable<T> where T : new()
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
  public class Address : IDeepCopyable<Address>
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
  // Representa a base do modelo de protótipo, com estado comum a todas as subclasses.
  public class Person : IDeepCopyable<Person>
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
  // Extende a hierarquia e adiciona um estado específico, como o salário.
  public class Employee : Person, IDeepCopyable<Employee>
  {
    public int Salary;

    // ==============
    // Implementação da cópia na subclasse
    // ==============
    // Reutiliza a cópia da classe base e adiciona o estado específico da subclasse.
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
    public static T DeepCopy<T>(this IDeepCopyable<T> item) 
      where T : new()
    {
      return item.DeepCopy();
    }

    public static T DeepCopy<T>(this T person)
      where T : Person, new()
    {
      return ((IDeepCopyable<T>) person).DeepCopy();
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