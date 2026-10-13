using System;
using System.Collections.Generic;
using static System.Console;

// ==============================================================
// ATENCAO DIDATICA
// ==============================================================
// Este arquivo foi deixado propositalmente com uma violacao de OCP
// (Open/Closed Principle) para comparacao com a correcao da aula
// seguinte.
//
// O ponto quebrado nao e a existencia de `IHotDrink` nem de
// `IHotDrinkFactory`.
//
// O problema esta na `HotDrinkMachine` depender de um catalogo
// fechado (`AvailableDrink`).
// Se entrar uma nova bebida, como `Chocolate`, nao basta criar
// `Chocolate` e `ChocolateFactory`.
// Tambem sera preciso reabrir a maquina e editar codigo existente
// para cadastrar a nova opcao.
//
// Isso fere OCP porque a classe central precisa ser modificada
// sempre que o sistema cresce nesse eixo.
//
// A versao corrigida desta ideia esta em:
// `3.Factories/Aula08_AbstractFactoryOCP/AbstractFactoryOCP.cs`
// ==============================================================
namespace Aula07_AbstractFactory
{
    // A aula mostra uma versao simplificada de Abstract Factory.
    // O objetivo principal nao e apenas esconder `new`.
    // O objetivo principal e permitir que o cliente peca uma familia
    // coerente de produtos por meio de abstracoes.
    //
    // No exemplo classico do GoF, uma Abstract Factory costuma criar
    // VARIOS produtos relacionados, como Button + Checkbox de um mesmo tema.
    // Aqui a ideia aparece de forma mais enxuta:
    // temos uma familia de bebidas quentes e varias factories concretas
    // para preparar variantes dessa familia.
    //
    // O cliente nao quer saber de `Tea` ou `Coffee` diretamente.
    // Ele quer pedir uma `IHotDrink` por um caminho controlado.

    // ===== Interface =====
    public interface IHotDrink
    {
        // Product Abstraction:
        // contrato que o cliente recebe depois da criacao.
        //
        // Por que devolver `IHotDrink` em vez de `Tea` ou `Coffee`?
        // Porque, do ponto de vista do cliente, o detalhe relevante aqui
        // nao e "qual classe concreta nasceu", e sim "eu recebi algo que
        // pode ser consumido".
        //
        // Essa abstracao traz alguns ganhos:
        // 1. O metodo `MakeDrink(...)` pode ter um retorno estavel.
        //    Ele sempre devolve `IHotDrink`, mesmo que por baixo nasca
        //    `Tea`, `Coffee` ou outra bebida futura.
        // 2. O cliente fica menos acoplado a classes concretas.
        //    Se a implementacao concreta mudar, o cliente nao precisa mudar
        //    junto, desde que o contrato continue o mesmo.
        // 3. A API expressa melhor a intencao.
        //    O cliente quer "uma bebida quente consumivel",
        //    nao necessariamente "um objeto Tea".
        // 4. Novas variantes entram com menos atrito.
        //    Podemos adicionar outra bebida que implemente `IHotDrink`
        //    sem reescrever toda a parte que so quer consumir a bebida.
        void Consume();
    }

    // ===== Classe =====
    internal class Tea : IHotDrink
    {
        // Concrete Product: uma implementacao concreta da abstracao.

        public void Consume()
        {
            WriteLine("This tea is nice but I'd prefer it with milk.");
        }
    }

    // ===== Classe =====
    internal class Coffee : IHotDrink
    {
        // Concrete Product: outra implementacao concreta da mesma abstracao.

        public void Consume()
        {
            WriteLine("This coffee is delicious.");
        }
    }

    // ===== Interface ABSTRACT FACTORY =====
    public interface IHotDrinkFactory
    {
        // ABSTRACT FACTORY:
        // define o contrato comum para preparar qualquer bebida quente.
        IHotDrink Prepare(int amount);
    }

    // Diferenca para uma factory simples:
    // aqui nao temos apenas "um metodo de criacao".
    // Temos uma abstracao de factory (`IHotDrinkFactory`)
    // e varias factories concretas para variantes da familia.

    // =====  Classe CONCRETE FACTORY =====
    internal class TeaFactory : IHotDrinkFactory
    {
        // CONCRETE FACTORY: sabe preparar Tea.

        public IHotDrink Prepare(int amount)
        {
            // A politica de criacao dessa variante fica encapsulada aqui.
            WriteLine($"Put in tea bag, boil water, pour {amount} ml, add lemon, enjoy!");
            return new Tea();
        }
    }

    // =====  Classe CONCRETE FACTORY =====
    internal class CoffeeFactory : IHotDrinkFactory
    {
        // CONCRETE FACTORY: sabe preparar Coffee.

        public IHotDrink Prepare(int amount)
        {
            WriteLine($"Grind some beans, boil water, pour {amount} ml, add cream and sugar, enjoy!");
            return new Coffee();
        }
    }


 // ===== Classe =====
    public class HotDrinkMachine
    {
        // Esta classe funciona como um ponto de entrada para o cliente.
        // Em vez de o cliente dar new em TeaFactory ou CoffeeFactory,
        // ele conversa com uma maquina que localiza a factory correta.

        // Aqui fica a causa mais concreta da quebra de OCP:
        // a maquina centraliza as opcoes em um conjunto fechado.
        // Se surgir outra bebida, este tipo precisara ser editado.
        public enum AvailableDrink
        {
            Coffee, Tea
// Este Enum é uma das causas da quebra do Open/Closed/Principle (OCP).
// Se quisermos adicionar uma nova bebida, teremos que alterar este Enum.
        }

        // Mapeia cada opcao de bebida para sua factory concreta.
        private Dictionary<AvailableDrink, IHotDrinkFactory> factories
          = new Dictionary<AvailableDrink, IHotDrinkFactory>();

        // Constructor
        public HotDrinkMachine()
        {
            // A maquina instancia as factories por convencao de nome.
            // O ponto pedagogico aqui nao e reflection em si.
            // O ponto pedagogico e que a selecao continua acontecendo
            // por meio da abstracao `IHotDrinkFactory`.
            //
            // Repare na sutileza:
            // usar reflection aqui nao resolve o problema de OCP,
            // porque a fonte principal das opcoes continua sendo o enum.
            foreach (AvailableDrink drink in Enum.GetValues(typeof(AvailableDrink)))
            {
                var factory = (IHotDrinkFactory)Activator.CreateInstance(
                    Type.GetType("Aula07_AbstractFactory." + Enum.GetName(typeof(AvailableDrink), drink) + "Factory"));
                factories.Add(drink, factory);
            }
        }

        public IHotDrink MakeDrink(AvailableDrink drink, int amount)
        {
            // O cliente escolhe a variante desejada.
            // A maquina delega a criacao para a factory concreta correspondente.
            //
            // Repare no tipo de retorno:
            // a maquina NAO devolve `Tea` ou `Coffee`.
            // Ela devolve `IHotDrink`, porque o contrato estavel da maquina
            // e "eu entrego uma bebida quente preparada", nao
            // "eu exponho qual classe concreta nasceu por baixo".
            return factories[drink].Prepare(amount);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // O cliente conversa com a maquina e recebe o produto abstrato.
            // Ele nao depende diretamente de TeaFactory, CoffeeFactory,
            // Tea ou Coffee para consumir a bebida.
            //
            // Isso e polimorfismo na pratica:
            // `drink` e `drink2` tem o mesmo tipo visivel (`IHotDrink`),
            // mas cada variavel aponta para uma implementacao concreta diferente.
            // O cliente continua funcionando porque so depende do contrato comum.
            var machine = new HotDrinkMachine();
            var drink = machine.MakeDrink(HotDrinkMachine.AvailableDrink.Tea, 200);
            drink.Consume();

            var drink2 = machine.MakeDrink(HotDrinkMachine.AvailableDrink.Coffee, 150);
            drink2.Consume();
        }
    }
}
