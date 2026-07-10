using System;
using System.Reflection;
using Microsoft.VisualBasic;
using System.Runtime.CompilerServices; // [FIX] Fixes CS0246 for CallerFilePath
using NUnit.Framework;                  // [FIX] Fixes CS0246 for TestFixture, Test, and Assert


// Problemas do Sinlgeton em relação à Testabilidade

// Impossibilidade de Mocking (Acoplamento Rígido): 
// A classe SingletonRecordFinder consome diretamente a propriedade estática SingletonDatabase.
// Instance em vez de receber a interface IDatabase.
// Isso impede que o teste isole a lógica de contagem, obrigando-o a interagir sempre com a implementação real do banco de dados.


// Dependência Oculta de Infraestrutura (I/O de Disco): 
// O construtor do Singleton lê o arquivo físico "capitals.txt" diretamente do disco. ]
// Se esse arquivo for alterado durante o teste, deletado ou se o teste rodar em um ambiente sem acesso a essa pasta,
// o teste unitário falhará, tornando-o frágil e lento.

// Poluição de Estado entre Testes (Estado Global): 
// Como a Instance é estática e persistente via Lazy<SingletonDatabase>, 
// ela mantém o mesmo estado na memória durante toda a execução da suíte de testes.
//  Se um teste modificar os dados internos, os testes seguintes herdarão essa sujeira, gerando testes instáveis
// (que passam sozinhos, mas falham em lote).

// Dificuldade de Paralelização: 
// Devido ao estado global único compartilhado pela propriedade Instance,
//  rodar os testes dessa classe em paralelo pode causar condições de corrida (race conditions),
//  corrompendo os resultados.
namespace Aula02_TestabilityIssues
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

        private static int instanceCount;
        public static int Count => instanceCount;

        // ===== Construtores =====
        // Le o arquivo "capitals.txt" e monta o dicionario de capitais.
        private SingletonDatabase() 
        {
            instanceCount++;
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

        //======= Forma Otimizada (Lazy) ===================
        private static Lazy<SingletonDatabase> instance = 
          new Lazy<SingletonDatabase>(() => new SingletonDatabase()); // Inicializa Singleton (Lazy)

        // Ponto de acesso global exigido pelo pattern Singleton.
        public static SingletonDatabase Instance => instance.Value; // Expõe Singleton
    }



    public class SingletonRecordFinder
    {
        public int GetTotalPopulation(IEnumerable<string> names)
        {
            int result = 0;

            foreach(var name in names)
            {
                result += SingletonDatabase.Instance.GetPopulation(name);
            }

            return result;
        }
    }





    // ================= Seção de Testes =====================
//    [TestFixture]
//    public class SingletonTests
//    {
//        [Test]
//        public void IsSingletonTest()
//        {
//            var db = SingletonDatabase.Instance;
//            var db2 = SingletonDatabase.Instance;
//
//            Assert.That(db, Is.SameAs(db2));
//            Assert.That(SingletonDatabase.Count, Is.EqualTo(1));
//
//        }

//         [Test]
//         public void SingletonPopulationTest()
//         {
//            var rf = new SingletonRecordFinder();
//            var names = new[] {"Seoul","Mexico"};
//
//            int tp = rf.GetTotalPopulation(names);
//            Assert.That(tp, IsBoxed.EqualTo(175000 + 174000));
//
//            
//         }
//     }
//





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