using System;
using System.Collections.Generic;
using static System.Console;

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

    // ===== Interface =====
    public interface IHotDrinkFactory
    {
        // Abstract Factory:
        // define o contrato comum para preparar qualquer bebida quente.
        IHotDrink Prepare(int amount);
    }

    // Diferenca para uma factory simples:
    // aqui nao temos apenas "um metodo de criacao".
    // Temos uma abstracao de factory (`IHotDrinkFactory`)
    // e varias factories concretas para variantes da familia.

    // ===== Classe =====
    internal class TeaFactory : IHotDrinkFactory
    {
        // Concrete Factory: sabe preparar Tea.

        public IHotDrink Prepare(int amount)
        {
            // A politica de criacao dessa variante fica encapsulada aqui.
            WriteLine($"Put in tea bag, boil water, pour {amount} ml, add lemon, enjoy!");
            return new Tea();
        }
    }

    // ===== Classe =====
    internal class CoffeeFactory : IHotDrinkFactory
    {
        // Concrete Factory: sabe preparar Coffee.

        public IHotDrink Prepare(int amount)
        {
            WriteLine($"Grind some beans, boil water, pour {amount} ml, add cream and sugar, enjoy!");
            return new Coffee();
        }
    }

    public class HotDrinkMachine
    {
        // Esta classe funciona como um ponto de entrada para o cliente.
        // Em vez de o cliente dar new em TeaFactory ou CoffeeFactory,
        // ele conversa com uma maquina que localiza a factory correta.

        public enum AvailableDrink
        {
            Coffee, Tea
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
            var machine = new HotDrinkMachine();
            var drink = machine.MakeDrink(HotDrinkMachine.AvailableDrink.Tea, 200);
            drink.Consume();

            var drink2 = machine.MakeDrink(HotDrinkMachine.AvailableDrink.Coffee, 150);
            drink2.Consume();
        }
    }
}
