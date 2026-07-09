
using System;
using Microsoft.VisualBasic;

namespace Aula01_SingletonImplementation
{

    // ===== Interface =====
    // Contrato que representa o "acesso a dados de populacao".
    public interface IDatabase
    {
        int GetPopulation(string name);
    }


    // ===== Classe =====
    // Implementa o pattern Singleton: garante que exista, no maximo,
    // UMA instancia de `SingletonDatabase
    public class SingletonDatabase : IDatabase
    {

        
        private Dictionary<string, int> capitals;


        // ===== Construtores =====
        // Le o arquivo "capitals.txt" e monta o dicionario de capitais.
        // Este construtor só deve ser chamado uma vez na aplicação
        // o modificador de acesso "private" garante isso, fazendo com que
        // não tenha como chamar ele externamente usando "new" para criar uma nova instância .

        private SingletonDatabase() // Construtor privado. Assim, não tem como chamar ele externamente com new para criar uma nova instância .
        {
            Console.WriteLine("Initializing Database . . .");

            capitals = File.ReadAllLines("capitals.txt")
             .batch(2)
             .ToDictionary(
                list => list.ElementAt(0).trim(),
                list => int.Parse(list.ElementAt(1))
             );
        }

        public int GetPopulation(string name)
        {
            return capitals[name];
        }

        
    // privtae static SingletonDatabase instance = new SingletonDatabase();
    //                                                                 Inicializa o Singleton Forma tradicional
    // public static SingletonDatabase Instance => instance;
    //                                                                 Expõe Singleton  Forma tradicional
        private static Lazy<SingletonDatabase> instance = 
          new Lazy<SingletonDatabase>(() => new SingletonDatabase()); // Inicializa Singleton (Lazy)
        public static SingletonDatabase Instance => instance.Value; // Expõe Singleton


        //Apenas cria o Singleton do Databse quando alguém acessa a instância.
        // Por que? Porque a lambda é ativada só quando se faz o get do .Value


    }


    public static class Program
    {
        static void Main(string [] args)
        {
            
            var db = SingletonDatabase.Instance;
            var city = "Tokyo";
            Console.WriteLine($"{city} has population {db.GetPopulation(city)}");

        }
        
    }
}
