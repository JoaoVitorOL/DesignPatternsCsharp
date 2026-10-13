using System;
using System.Reflection;
using Microsoft.VisualBasic;
using System.Threading;         // [FIX] Fixes CS0246 for ThreadLocal<T> e Thread
using System.Threading.Tasks;   // [FIX] Fixes CS0246 for Task

// ============================================================================
// Aula05 - Per-Thread Singleton
// ----------------------------------------------------------------------------
// Este é mais um desvio estrutural do Singleton clássico (Aula01), na mesma
// família do Monostate (Aula04) - mas por um caminho oposto.
//
// O Singleton "puro" promete UMA instância para o processo inteiro, não
// importa quantas threads a estejam acessando ao mesmo tempo. O Per-Thread
// Singleton relaxa essa promessa: garante UMA instância POR THREAD, não uma
// instância global. Ou seja, cada thread que pedir `Instance` recebe sua
// própria cópia isolada, e chamadas repetidas da MESMA thread sempre
// recebem essa mesma cópia.
//
// A peça que viabiliza isso é `ThreadLocal<T>`, do namespace
// `System.Threading`: em vez de um único slot de memória compartilhado
// (como o `Lazy<T>` estático do Singleton comum), `ThreadLocal<T>` mantém
// um slot de memória DIFERENTE para cada thread, todos acessíveis através
// da mesma variável estática `threadInstance`.
// ============================================================================
namespace Aula05_PerThreadSingleton
{
    // ===== Classe =====
    // `sealed` impede que `PerThreadSingleton` seja herdada. Não é
    // obrigatório para o padrão funcionar, mas é uma prática comum em
    // implementações de Singleton (e variações): evita que uma subclasse
    // crie um caminho alternativo de instanciação que escape do controle
    // da classe base.
    public sealed class PerThreadSingleton
    {
        // ===== Campos =====
        // `ThreadLocal<PerThreadSingleton>` é o substituto do
        // `Lazy<SingletonDatabase>` visto no Singleton comum (Aula01).
        // A diferença de comportamento está inteira no tipo:
        //   - `Lazy<T>` estático: um único valor, compartilhado por
        //     todas as threads que o acessam.
        //   - `ThreadLocal<T>` estático: por baixo dos panos, guarda um
        //     DICIONÁRIO interno de "thread -> valor". Quando uma thread
        //     acessa `.Value` pela primeira vez, a fábrica passada no
        //     construtor (`() => new PerThreadSingleton()`) é executada
        //     UMA vez PARA AQUELA thread, e o resultado fica guardado
        //     só para ela. Outras threads, ao acessarem `.Value`, disparam
        //     sua própria criação, de forma independente.
        private static ThreadLocal<PerThreadSingleton> threadInstance
          = new ThreadLocal<PerThreadSingleton>(() => new PerThreadSingleton());

        // Campo público simples, só para tornar visível, no `Console.WriteLine`
        // do `Main`, qual thread criou qual instância.
        public int Id;

        // ===== Construtores =====
        // Construtor `private`: igual ao Singleton comum, impede que
        // qualquer código externo escreva `new PerThreadSingleton()`
        // diretamente. A única forma de obter uma instância é através
        // da propriedade estática `Instance`, logo abaixo.
        //
        // `Thread.CurrentThread.ManagedThreadId` captura o identificador
        // da thread que está executando este construtor NO MOMENTO em
        // que ele roda. Como a fábrica do `ThreadLocal<T>` só executa
        // este construtor uma vez por thread (comentário no campo acima),
        // esse `Id` fica "congelado" com o identificador da thread dona
        // desta instância específica.
        private PerThreadSingleton()
        {
            Id = Thread.CurrentThread.ManagedThreadId;
        }

        // ===== Propriedades =====
        // Ponto de acesso global exigido pelo padrão Singleton - só que,
        // por trás de `threadInstance.Value`, "global" aqui significa
        // "a mesma instância para chamadas repetidas dentro da MESMA
        // thread", não "a mesma instância para o processo inteiro".
        public static PerThreadSingleton Instance => threadInstance.Value;
    }

    // ===== Classe =====
    static class Program
    {
        // ===== Métodos =====
        static void Main(string[] args)
        {
            // Três tarefas (`Task`) são disparadas com `Task.Factory.StartNew`.
            // Por padrão, o .NET executa cada `Task` em uma thread do
            // ThreadPool - normalmente threads diferentes entre si (embora
            // o pool possa, em cenários específicos, reaproveitar uma
            // thread para mais de uma Task; o ponto pedagógico aqui segue
            // válido mesmo assim, porque o que importa é a thread que
            // efetivamente executa o corpo de cada uma).
            //
            // Cada `Task` acessa `PerThreadSingleton.Instance` pela
            // primeira vez dentro de sua própria thread de execução -
            // isso dispara a fábrica do `ThreadLocal<T>` e cria uma
            // instância nova, com o `Id` daquela thread específica.
            var t1 = Task.Factory.StartNew(() =>
            {
                Console.WriteLine($"t1: " + PerThreadSingleton.Instance.Id);
            });

            var t2 = Task.Factory.StartNew(() =>
            {
                Console.WriteLine($"t2: " + PerThreadSingleton.Instance.Id);
            });

            var t3 = Task.Factory.StartNew(() =>
            {
                Console.WriteLine($"t3: " + PerThreadSingleton.Instance.Id);
            });

            // Como as três instâncias são criadas por threads diferentes
            // do ThreadPool, o `Id` impresso por `t1`, `t2` e `t3` tende
            // a ser diferente entre si - prova, em tempo de execução, de
            // que NÃO existe uma única instância compartilhada por todo
            // o processo (como haveria no Singleton comum da Aula01),
            // e sim uma instância isolada por thread.
            //
            // `Task.WaitAll` bloqueia o `Main` até que as três tarefas
            // terminem, garantindo que o processo não encerre antes de
            // todo `Console.WriteLine` ser executado.
            Task.WaitAll(t1, t2, t3);
        }
    }
}