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
    // UMA instancia de `SingletonDatabase` durante toda a execucao do
    // programa, e fornece um ponto de acesso global a ela via `Instance`.
    //
    // Motivacao do pattern: a leitura de "capitals.txt" e uma operacao
    // custosa (I/O em disco). Se cada parte do sistema pudesse criar sua
    // propria instancia, o arquivo seria lido varias vezes sem necessidade,
    // desperdicando tempo e memoria. O Singleton centraliza essa carga
    // em um unico ponto, compartilhado por todos os consumidores.
    public class SingletonDatabase : IDatabase
    {

        // ===== Campos =====
        // Estrutura em memoria: nome da capital -> populacao.
        // Preenchida uma unica vez, no construtor.
        private Dictionary<string, int> capitals;


        // ===== Construtores =====
        // Le o arquivo "capitals.txt" e monta o dicionario de capitais.
        // Este construtor só deve ser chamado uma vez na aplicação
        // o modificador de acesso "private" garante isso, fazendo com que
        // não tenha como chamar ele externamente usando "new" para criar uma nova instância .

        private SingletonDatabase() // Construtor privado. Assim, não tem como chamar ele externamente com new para criar uma nova instância .
        {
            Console.WriteLine("Initializing Database . . .");

            // Le todas as linhas do arquivo e as agrupa em pares
            // (nome da capital, populacao), convertendo o resultado
            // diretamente para um Dictionary<string, int>.
            capitals = File.ReadAllLines("capitals.txt")
             .Chunk(2)
             .ToDictionary(
                list => list.ElementAt(0).Trim(),
                list => int.Parse(list.ElementAt(1))
             );
        }

        // ===== Metodos =====
        // Consulta a populacao de uma capital ja carregada em memoria.
        // Note que nao ha novo acesso a disco aqui: todo o custo de I/O
        // foi pago uma unica vez, no construtor.
        public int GetPopulation(string name)
        {
            return capitals[name];
        }

        
    // ===== Instancia unica (Singleton) =====
    // Duas formas possiveis de expor a instancia unica. A forma tradicional
    // (eager, comentada abaixo) cria o objeto assim que a classe e carregada
    // pelo runtime, independente de o Singleton chegar a ser usado ou nao:
    
    //======= Forma Tradicional (Eager) ===================
    // private static SingletonDatabase instance = new SingletonDatabase();
    // public static SingletonDatabase Instance => instance;
    //=========================================================


   //======= Forma Otimizada (Lazy) ===================
    // A forma adotada abaixo (lazy, com `Lazy<T>`) adia a criacao ate o
    // primeiro acesso a `Instance.Value`. Vantagens em relacao a versao
    // eager comentada acima:
    //   1. O custo de leitura do arquivo so e pago se o Singleton
    //      chegar a ser usado.
    //   2. `Lazy<T>` e thread-safe por padrao (LazyThreadSafetyMode.
    //      ExecutionAndPublication): mesmo com varias threads
    //      acessando `Instance` ao mesmo tempo, o construtor de
    //      `SingletonDatabase` executa apenas uma vez.
        private static Lazy<SingletonDatabase> instance = 
          new Lazy<SingletonDatabase>(() => new SingletonDatabase()); // Inicializa Singleton (Lazy)

        // Ponto de acesso global exigido pelo pattern Singleton.
        // Todo consumidor do sistema deve obter a instancia por aqui,
        // em vez de instanciar `SingletonDatabase` diretamente — o que,
        // alias, agora e impossivel de fora da classe, ja que o
        // construtor e `private`.
        public static SingletonDatabase Instance => instance.Value; // Expõe Singleton





    }


    public static class Program
    {
        // ===== Metodos =====
        static void Main(string [] args)
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