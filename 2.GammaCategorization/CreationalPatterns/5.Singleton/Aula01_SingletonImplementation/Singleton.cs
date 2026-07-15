using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

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
    // UMA instancia de `SingletonDatabase` durante toda a execucao do
    // programa, e fornece um ponto de acesso global a ela via `Instance`.
    public class SingletonDatabase : IDatabase
    {
        // ===== Campos =====
        // Estrutura em memoria: nome da capital -> populacao.
        private Dictionary<string, int> capitals;

        // ===== Construtores =====
        // Le o arquivo "capitals.txt" e monta o dicionario de capitais.
        private SingletonDatabase() 
        {
            Console.WriteLine("Initializing Database . . .");

            // [FIXED] Single-line path solution using [CallerFilePath] via an inline argument
            string currentCodeFile = GetCurrentFilePath();
            string sourceFolder = Path.GetDirectoryName(currentCodeFile) ?? "";
            string filePath = Path.Combine(sourceFolder, "capitals.txt");

            capitals = File.ReadAllLines(filePath)
                .Chunk(2)
                .ToDictionary(
                    list => list.ElementAt(0).Trim(),
                    list => int.Parse(list.ElementAt(1))
                );
        }

        // Helper method needed to capture the compile-time file path attribute string safely
        private static string GetCurrentFilePath([CallerFilePath] string path = "") => path;

        // ===== Metodos =====
        // Consulta a populacao de uma capital ja carregada em memoria.
        public int GetPopulation(string name)
        {
            return capitals[name];
        }

        //======= Singleton Forma Otimizada (Lazy) ===================
        private static Lazy<SingletonDatabase> instance = 
          new Lazy<SingletonDatabase>(() => new SingletonDatabase()); // Inicializa Singleton (Lazy)

        // Ponto de acesso global exigido pelo pattern Singleton.
        public static SingletonDatabase Instance => instance.Value; // Expõe Singleton

        //====== Singleton Padrão ============
        // private static SingletonDatabase instance = new SingletonDatabase();
        // public static SIngletonDatabase Instance => instance;
    }

    public static class Program
    {
        // ===== Metodos =====
        static void Main(string[] args)
        {
            // Primeira chamada a `Instance` aciona a criacao (lazy) da
            // unica instancia de `SingletonDatabase` e, junto com ela,
            // a leitura de "capitals.txt".
            var db = SingletonDatabase.Instance;
            var city = "Tokyo";
            Console.WriteLine($"{city} has population {db.GetPopulation(city)}");
        }
    }
}
