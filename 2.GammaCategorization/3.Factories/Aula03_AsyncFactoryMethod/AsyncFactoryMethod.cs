using System;
using System.Threading.Tasks;
using static System.Console;

namespace Aula03_AsyncFactoryMethod
{
    // ===== Classe =====
    public class Foo
    {
        // ===== Campos =====
        private string status = "Ainda nao inicializado";

        // O ponto novo desta aula e o limite da aula anterior:
        // factory methods nomeiam melhor a criacao, mas continuam sincronos.
        // Quando o objeto precisa nascer depois de uma operacao async,
        // o construtor deixa de ser um ponto de entrada viavel.
        //
        // Em C#, construtores nao podem usar `await`.
        // Por isso, o objeto nasce por um metodo de fabrica assincrono.

        // ===== Construtores =====
        private Foo()
        {
            // Construtor privado
        }

        // Esta etapa representa trabalho assincrono real:
        // leitura de arquivo, chamada HTTP, handshake, token, cache remoto, etc.
        // O cliente nao deveria receber o objeto antes disso terminar.

        // ===== Metodos =====
        private async Task InitializeAsync()
        {
            await Task.Delay(1000);
            status = "Inicializacao assincrona concluida";
        }

        // A factory assincrona concentra o fluxo completo:
        // 1. cria a instancia;
        // 2. aguarda a inicializacao;
        // 3. so entao devolve o objeto pronto para uso.
        public static async Task<Foo> CreateAsync() // Task<Foo>  aqui significa: Operação Task assíncrona em andamento, vai retornar um objeto Foo quando terminar.
        {
            var foo = new Foo();
            await foo.InitializeAsync();
            return foo;
        }

        public override string ToString()
        {
            return status;
        }
    }

    // ===== Classe =====
    public class Demo
    {
        // ===== Metodos =====
        static async Task Main(string[] args)
        {
            // Diferenca para a aula anterior:
            // aqui o metodo de fabrica nao so nomeia a criacao,
            // ele tambem segura a entrega do objeto ate o fim do trabalho async.
            var foo = await Foo.CreateAsync();
            WriteLine(foo);
        }
    }
}
