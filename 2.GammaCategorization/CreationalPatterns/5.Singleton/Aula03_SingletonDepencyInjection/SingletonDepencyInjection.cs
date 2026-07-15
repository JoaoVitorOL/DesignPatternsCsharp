using System;
using System.Reflection;
using Microsoft.VisualBasic;
using System.Runtime.CompilerServices; // [FIX] Fixes CS0246 for CallerFilePath
using NUnit.Framework;                  // [FIX] Fixes CS0246 for TestFixture, Test, and Assert

// ============================================================================
// Aula03 - Singleton + Dependency Injection
// ----------------------------------------------------------------------------
// Esta aula é a evolução direta da Aula02 (TestabilityIssues).
// Lá ficou provado o problema: `SingletonRecordFinder` chama
// `SingletonDatabase.Instance` diretamente dentro do método, criando um
// acoplamento rígido (hard-coded dependency) que impede a troca da base
// de dados por um double/mock em testes.
//
// Aqui aparece a solução clássica: em vez de a classe consumidora buscar
// a dependência sozinha (Singleton estático), a dependência é INJETADA
// de fora via construtor (Dependency Injection / Inversion of Control).
// Isso é o que torna a classe testável.
// ============================================================================
namespace Aula03_SingletonDependencyInjection
{
    // ===== Interface =====
    public interface IDatabase
    {
        int GetPopulation(string name);
    }

    // ===== Classe SingletonDatabase (Jeito Errado) =====
//    public class SingletonDatabase : IDatabase
//    {
//        // ===== Campos =====
//        // Estrutura em memoria: nome da capital -> populacao.
//        private Dictionary<string, int> capitals;
//
//        private static int instanceCount;
//        public static int Count => instanceCount;
//
//        // ===== Construtores =====
//        // Le o arquivo "capitals.txt" e monta o dicionario de capitais.
//        private SingletonDatabase() 
//        {
//            instanceCount++;
//            Console.WriteLine("Initializing Database . . .");
//
//         
//            string currentCodeFile = GetCurrentFilePath();
//            string sourceFolder = Path.GetDirectoryName(currentCodeFile) ?? "";
//            string filePath = Path.Combine(sourceFolder, "capitals.txt");
//
//            capitals = File.ReadAllLines(filePath)
//                .Chunk(2)
//                .ToDictionary(
//                    list => list.ElementAt(0).Trim(),
//                    list => int.Parse(list.ElementAt(1))
//                );
//        }
//
//        // Helper method needed to capture the compile-time file path attribute string safely
//        private static string GetCurrentFilePath([CallerFilePath] string path = "") => path;
//
//        // ===== Metodos =====
//        // Consulta a populacao de uma capital ja carregada em memoria.
//        public int GetPopulation(string name)
//        {
//            return capitals[name];
//        }
//
//        //======= Forma Otimizada (Lazy) ===================
//        private static Lazy<SingletonDatabase> instance = 
//          new Lazy<SingletonDatabase>(() => new SingletonDatabase()); // Inicializa Singleton (Lazy)
//
//        // Ponto de acesso global exigido pelo pattern Singleton.
//        public static SingletonDatabase Instance => instance.Value; // Expõe Singleton
//    }


    // ===== Classe (NOVA em relação à Aula02) =====
    // `OrdinaryDatabase` é a peça central da mudança: é a MESMA lógica de
    // leitura do arquivo "capitals.txt" que existia dentro do
    // `SingletonDatabase`, só que sem nenhum mecanismo de Singleton.
    //
    // Ou seja: sem `private static Lazy<...> instance`, sem `Instance`
    // estático, sem contador de instâncias. É uma classe "comum", que
    // pode ser instanciada quantas vezes for necessário com `new`.
    //
    // Isso por si só não muda nada de comportamento em produção — o ganho
    // aparece quando ela é combinada com `ConfigurableRecordFinder` logo
    // abaixo, que recebe a dependência via construtor em vez de acessá-la
    // como Singleton.
    public class OrdinaryDatabase : IDatabase
    {

        // ===== Campos =====
        // Estrutura em memoria: nome da capital -> populacao.
        private Dictionary<string, int> capitals;


        // ===== Construtores =====
        private OrdinaryDatabase() 
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

        // ===== Metodos =====
        // Consulta a populacao de uma capital ja carregada em memoria.
        // Mesma assinatura/comportamento do `SingletonDatabase.GetPopulation`,
        // exigida pelo contrato `IDatabase`.
        public int GetPopulation(string name)
        {
            return capitals[name];
        }
    }


    // ===== Classe (herdada da Aula02, mantida para comparação) =====
    // Representa o "jeito errado": depende diretamente do Singleton
    // estático `SingletonDatabase.Instance` dentro do próprio método.
    // Não há como injetar um banco de dados fake aqui para testar,
    // porque a dependência não entra por fora — ela é buscada
    // internamente. É esse acoplamento que motiva a classe seguinte.
//    public class SingletonRecordFinder
//    {
//        public int GetTotalPopulation(IEnumerable<string> names)
//        {
//            int result = 0;
//
//            foreach(var name in names)
//            {
//                result += SingletonDatabase.Instance.GetPopulation(name);
//            }
//
//            return result;
//        }
//    }


    // ===== Classe (NOVA em relação à Aula02) =====
    // `ConfigurableRecordFinder` é a correção do problema exposto pelo
    // `SingletonRecordFinder`. A diferença central:
    //
    //   - `SingletonRecordFinder` busca a dependência sozinho
    //     (`SingletonDatabase.Instance`), acoplando-se a UMA implementação
    //     concreta específica.
    //   - `ConfigurableRecordFinder` recebe a dependência de fora, via
    //     construtor, tipada pela interface `IDatabase`. Isso é
    //     Dependency Injection: quem cria o `ConfigurableRecordFinder`
    //     decide qual `IDatabase` ele vai usar (o real `OrdinaryDatabase`
    //     em produção, ou um double como `DummyDatabase` em teste).
    //
    // Com isso, a classe fica desacoplada da implementação concreta e
    // pode ser testada isoladamente, sem precisar ler `capitals.txt`
    // nem depender do estado global do Singleton.
    public class ConfigurableRecordFinder
    {
        // ===== Campos =====
        // Guarda a abstração recebida, não uma implementação concreta.
        private IDatabase database;

        // ===== Construtores =====
        // Constructor Injection: a dependência entra pronta de fora.
        public ConfigurableRecordFinder(IDatabase database)
        {
            // Guarda de nulidade: evita que a classe funcione num estado
            // inválido (sem banco de dados nenhum).
            if (database == null)
            {
                throw new ArgumentNullException(paramName: nameof(database));
            }

            this.database = database;
        }

        // ===== Metodos =====
        // Mesma lógica de soma de populações do `SingletonRecordFinder`,
        // porém usando a dependência injetada (`this.database`) em vez
        // de um Singleton estático.
        public int GetTotalPopulation(IEnumerable<string> names)
        {
            int result = 0;

            foreach(var name in names)
            {
                result += database.GetPopulation(name);
            }

            return result;
        }
    }

    // ===== Classe (NOVA em relação à Aula02) =====
    // `DummyDatabase` é um test double: uma implementação de `IDatabase`
    // que não lê arquivo nenhum, só devolve valores fixos e conhecidos
    // ("alpha", "beta", "gamma"). Só é possível existir e ser usada por
    // causa da Dependency Injection: como `ConfigurableRecordFinder`
    // recebe qualquer `IDatabase` pelo construtor, este dublê pode
    // substituir o banco real em testes, sem tocar em `capitals.txt`.
    public class DummyDatabase : IDatabase
    {
        public int GetPopulation(string name)
        {
            return new Dictionary<string, int>
            {
                ["alpha"] = 1,
                ["beta"] = 2,
                ["gamma"] = 3

            }[name];
        }
    }



    // ================= Seção de Testes =====================
    // Os testes comentados abaixo mostram exatamente a evolução do
    // capítulo: os dois primeiros (`IsSingletonTest`, `SingletonPopulationTest`)
    // são os mesmos da Aula02 e continuam sofrendo do problema de
    // testabilidade. Os dois últimos (`ConfigurablePopulationTest`,
    // `DIPopulationTest`) são NOVOS e mostram a solução:
    //   - `ConfigurablePopulationTest` injeta manualmente um `DummyDatabase`.
    //   - `DIPopulationTest` injeta via um container de DI (Autofac -
    //     `ContainerBuilder`), registrando `OrdinaryDatabase` como
    //     `IDatabase` (Singleton dentro do container, mas sem usar o
    //     pattern Singleton "cru" da linguagem) e resolvendo
    //     `ConfigurableRecordFinder` automaticamente com a dependência já
    //     encaixada.
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
//         }

//         [Test]
//         public void ConfigurablePopulationTest()
//         {
//               var rf = new ConfigurableRecordFinder(new DummyDatabase());
//               var names = new[] {"alpha","gamma"};
//               int tp = rf.GetTotalPopulation(names);
//               Assert.That(tp, Is.EqualTo(4))

//         }


//        [Test]
//        public void DIPopulationTest()
//        {
//            var cb = new ContainerBuilder();
//            cb.RegisterType<OrdinaryDatabase>()
//               .AS<IDatabase>()
//               .SingleInstance();
//            cb.RegisterType<ConfigurableRecordFinder>();
//        
//            using (var c = cb.Build())
//                {
//                   var rf = c.Resolve<ConfigurableRecordFinder>();     
//                }
//        }
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
            //
            // [SEM MUDANÇA] O `Main` continua usando o caminho antigo
            // (Singleton) só para ilustrar; a versão "correta" seria
            // trocar isso por `new ConfigurableRecordFinder(new OrdinaryDatabase())`.
            var db = SingletonDatabase.Instance;
            var city = "Tokyo";
            Console.WriteLine($"{city} has population {db.GetPopulation(city)}");
        }
    }
}