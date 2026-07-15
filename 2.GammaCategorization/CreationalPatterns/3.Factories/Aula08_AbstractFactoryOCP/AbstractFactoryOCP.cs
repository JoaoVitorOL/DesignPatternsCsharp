using System;
using System.Collections.Generic;
using static System.Console;



// Como corrigir a violação anterior do Open/Closed Pricniple (OCP)?


// Esta aula corrige o ponto em que a versao anterior quebrava OCP.
// A ideia principal nao e "usar reflection por moda".
// A ideia principal e remover da `HotDrinkMachine` a obrigacao de
// ser editada sempre que uma nova bebida aparecer.
//
// Em `Aula07_AbstractFactory`, adicionar uma nova bebida exigia:
// 1. criar a nova bebida concreta;
// 2. criar a nova factory concreta;
// 3. reabrir a maquina para mexer no catalogo central (`enum`).
//
// Aqui a extensao passa a acontecer pela adicao de novas classes que
// implementam `IHotDrinkFactory`.
namespace Aula08_AbstractFactoryOCP
{


    // ===== Interface =====
    public interface IHotDrink
    {
        void Consume();
    }

    // ===== Classe =====
    internal class Tea : IHotDrink
    {
        

        public void Consume()
        {
            WriteLine("This tea is nice but I'd prefer it with milk.");
        }
    }

    // ===== Classe =====
    internal class Coffee : IHotDrink
    {
        

        public void Consume()
        {
            WriteLine("This coffee is delicious.");
        }
    }

    // ===== Interface ABSTRACT FACTORY =====
    public interface IHotDrinkFactory
    {
     
        IHotDrink Prepare(int amount);
    }



    // =====  Classe CONCRETE FACTORY =====
    internal class TeaFactory : IHotDrinkFactory
    {
        

        public IHotDrink Prepare(int amount)
        {
            
            WriteLine($"Put in tea bag, boil water, pour {amount} ml, add lemon, enjoy!");
            return new Tea();
        }
    }

    // =====  Classe CONCRETE FACTORY =====
    internal class CoffeeFactory : IHotDrinkFactory
    {
       

        public IHotDrink Prepare(int amount)
        {
            WriteLine($"Grind some beans, boil water, pour {amount} ml, add cream and sugar, enjoy!");
            return new Coffee();
        }
    }


 // ===== Classe =====
    public class HotDrinkMachine
{
      

// !!!!!!!!!!!!!!!!!!!!!!!!!!
// ESQUEÇA SOBRE A ABORDAGEM COM ENUMS, isso quebra o Open/Closed Principle (OCP) e não é escalável.
// !!!!!!!!!!!!!!!!!!!!!!!!!!


//       public enum AvailableDrink
//        {
//           Coffee, Tea
//       }

        // Mapeia cada opcao de bebida para sua factory concreta.
//        private Dictionary<AvailableDrink, IHotDrinkFactory> factories
//          = new Dictionary<AvailableDrink, IHotDrinkFactory>();
//
//        // Constructor
//        public HotDrinkMachine()
//        {
//           
//            foreach (AvailableDrink drink in Enum.GetValues(typeof(AvailableDrink)))
//            {
//                var factory = (IHotDrinkFactory)Activator.CreateInstance(
//                    Type.GetType("Aula07_AbstractFactory." + Enum.GetName(typeof(AvailableDrink), drink) + "Factory"));
//                factories.Add(drink, factory);
//            }
//        }
//
//        public IHotDrink MakeDrink(AvailableDrink drink, int amount)
//        {
//            
//            return factories[drink].Prepare(amount);
//        }








    // Em vez de mapear `enum -> factory`, a maquina guarda uma lista de
    // pares:
    // - `Item1`: nome exibido ao usuario;
    // - `Item2`: factory concreta correspondente.
    //
    // Para quem esta comecando:
    // `Tuple<string, IHotDrinkFactory>` e apenas um jeito simples de
    // manter esses dois dados juntos.


    private List<Tuple<string, IHotDrinkFactory>> factories = new List<Tuple<string, IHotDrinkFactory>>();
// =================================
//    Constructor
// =================================
        public HotDrinkMachine()
        {
            // Esta linha percorre os tipos compilados no mesmo assembly
            // desta aula.
            // Em termos simples: a maquina pergunta "quais classes
            // existem aqui dentro?".
            foreach (var VARIABLE in typeof(HotDrinkMachine).Assembly.GetTypes())
            {
                // `IsAssignableFrom` verifica se o tipo atual implementa
                // `IHotDrinkFactory`.
                // `!VARIABLE.IsInterface` evita tentar instanciar a propria
                // interface, que nao pode virar objeto concreto.
                if (typeof(IHotDrinkFactory).IsAssignableFrom(VARIABLE) && !VARIABLE.IsInterface)
                {
                    // Aqui acontece a correcao conceitual de OCP:
                    // a lista de opcoes passa a nascer das factories
                    // concretas existentes, e nao de um enum fechado.
                    //
                    // `VARIABLE.Name.Replace("Factory", string.Empty)`
                    // transforma nomes como `TeaFactory` em `Tea`
                    // apenas para exibicao ao usuario.
                    //
                    // `Activator.CreateInstance(VARIABLE)` cria uma
                    // instancia da factory encontrada em tempo de execucao.
                    //
                    // Honestidade tecnica:
                    // reflection ajuda muito a visualizar a extensao,
                    // mas nao e a unica forma profissional de resolver isso.
                    // Injecao de dependencia ou registro explicito tambem
                    // sao caminhos comuns.
                    factories.Add(Tuple.Create(
                        VARIABLE.Name.Replace("Factory", string.Empty),
                        (IHotDrinkFactory)Activator.CreateInstance(VARIABLE)));
                }
            }
        }



        public IHotDrink MakeDrink()
        {
            // Diferenca conceitual em relacao a Aula07:
            // o cliente nao escolhe mais a partir de um enum fechado
            // dentro da maquina.
            // A maquina mostra as factories descobertas e trabalha em cima
            // dessa colecao aberta a extensao.
            WriteLine("Available drinks:");
            for (var index = 0; index < factories.Count; ++index)
            {
                var tuple = factories[index];
                WriteLine($"{index}: {tuple.Item1}");
            }
            
            while (true)
            {
                // `TryParse` tenta converter a entrada textual para numero
                // sem gerar excecao quando o usuario digita algo invalido.
                string s;

                if ((s = ReadLine()) != null
                    && int.TryParse(s, out int i)
                    && i >= 0
                    && i < factories.Count)
                {
                    Write("Specify amount: ");
                    s = ReadLine();
                    if (s != null && int.TryParse(s, out int amount) && amount > 0)
                    {
                        // `Item2` e a factory concreta guardada na tupla.
                        // Depois de localizar a opcao escolhida, a maquina
                        // delega o preparo para a factory correta.
                        return factories[i].Item2.Prepare(amount);
                    }
                }

                WriteLine("Incorrect input, try again.");

            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            
            var machine = new HotDrinkMachine();
            var drink =  machine.MakeDrink();

            drink.Consume();
           
        }
    }
}
