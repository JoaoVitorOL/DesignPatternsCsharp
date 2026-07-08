
using System;
using Microsoft.VisualBasic;

namespace Aula01_SingletonImplementation
{

    public interface IDatabase
    {
        int GetPopulation(string name);
    }


    public class SingletonDatabase : IDatabase
    {
        private Dictionary<string, int> capitals;

        public SingletonDatabase()
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

        

        private static Lazy<SingletonDatabase> instance = 
          new Lazy<SingletonDatabase>(() => new SingletonDatabase());
        public static SingletonDatabase Instance => instance.Value;


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
