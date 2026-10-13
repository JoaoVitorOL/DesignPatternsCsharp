using System;
using System.Collections.Generic;
using System.Linq;
using static System.Console;

// ============================================================================
// Aula03 - Composite Specification (Padrao Composite + Specification)
// ----------------------------------------------------------------------------
// O QUE E: esta aula combina o padrao Specification com a ideia do Composite.
//          Uma regra simples, como "produto verde", e uma folha. Uma regra
//          composta, como "produto verde E grande", agrega outras regras.
//
// CENARIO:
// * Queremos filtrar produtos por criterios diferentes.
// * Nao queremos criar um metodo novo para cada combinacao possivel.
// * Tambem nao queremos espalhar ifs no filtro toda vez que surgir uma regra.
//
// A SOLUCAO (Composite Specification):
// * Cada specification sabe avaliar uma regra booleana.
// * Specifications simples sao folhas: ColorSpecification, SizeSpecification.
// * AndSpecification e um composite: guarda outras specifications e so aprova
//   o item quando todas as regras internas aprovam tambem.
// * O operador & cria uma composicao de regras com sintaxe curta.
// ============================================================================
namespace DotNetDesignPatternDemos.Structural.Composite.CompositeSpecification
{
    // ========================================================================
    // ==== MODELO DE DOMINIO ====
    // Produtos possuem propriedades que podem ser avaliadas por specifications.
    // ========================================================================

    // ===== Enum =====
    public enum Color
    {
        Red,
        Green,
        Blue
    }

    // ===== Enum =====
    public enum Size
    {
        Small,
        Medium,
        Large
    }

    // ===== Classe =====
    public class Product
    {
        // ===== Propriedades =====
        public string Name { get; }
        public Color Color { get; }
        public Size Size { get; }

        // ===== Construtores =====
        public Product(string name, Color color, Size size)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            Name = name;
            Color = color;
            Size = size;
        }
    }

    // ========================================================================
    // ==== CONTRATO DO FILTRO ====
    // O filtro conhece apenas uma sequencia de itens e uma regra de aprovacao.
    // Ele nao precisa saber quais criterios concretos existem.
    // ========================================================================

    // ===== Interface =====
    public interface IFilter<T>
    {
        // ===== Metodos =====
        IEnumerable<T> Filter(IEnumerable<T> items, ISpecification<T> spec);
    }

    // ===== Classe =====
    public class BetterFilter : IFilter<Product>
    {
        // ===== Metodos =====
        public IEnumerable<Product> Filter(IEnumerable<Product> items, ISpecification<Product> spec)
        {
            foreach (var item in items)
            {
                // O filtro delega a decisao para a specification recebida.
                if (spec.IsSatisfied(item))
                {
                    yield return item;
                }
            }
        }
    }

    // ========================================================================
    // ==== COMPONENT ====
    // ISpecification<T> representa a abstracao comum para folhas e composites.
    // Apesar do prefixo "I", aqui ela e uma classe abstrata para permitir o
    // operador &, que cria uma especificacao composta.
    // ========================================================================

    // ===== Classe Abstrata =====
    public abstract class ISpecification<T>
    {
        // ===== Metodos =====
        public abstract bool IsSatisfied(T item);

        // O operador & permite escrever:
        // greenSpecification & largeSpecification
        // e receber uma AndSpecification com as duas regras dentro.
        public static ISpecification<T> operator &(ISpecification<T> first,
            ISpecification<T> second)
        {
            if (first == null)
            {
                throw new ArgumentNullException(nameof(first));
            }

            if (second == null)
            {
                throw new ArgumentNullException(nameof(second));
            }

            return new AndSpecification<T>(first, second);
        }
    }

    // ========================================================================
    // ==== LEAFS ====
    // Specifications simples avaliam uma unica regra e nao possuem filhos.
    // ========================================================================

    // ===== Classe =====
    public class ColorSpecification : ISpecification<Product>
    {
        // ===== Campos =====
        private readonly Color color;

        // ===== Construtores =====
        public ColorSpecification(Color color)
        {
            this.color = color;
        }

        // ===== Metodos =====
        public override bool IsSatisfied(Product product)
        {
            return product.Color == color;
        }
    }

    // ===== Classe =====
    public class SizeSpecification : ISpecification<Product>
    {
        // ===== Campos =====
        private readonly Size size;

        // ===== Construtores =====
        public SizeSpecification(Size size)
        {
            this.size = size;
        }

        // ===== Metodos =====
        public override bool IsSatisfied(Product product)
        {
            return product.Size == size;
        }
    }

    // ========================================================================
    // ==== BASE DO COMPOSITE ====
    // Guarda as specifications filhas que serao combinadas por uma regra maior.
    // ========================================================================

    // ===== Classe Abstrata =====
    public abstract class CompositeSpecification<T> : ISpecification<T>
    {
        // ===== Campos =====
        protected readonly ISpecification<T>[] items;

        // ===== Construtores =====
        protected CompositeSpecification(params ISpecification<T>[] items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            this.items = items;
        }
    }

    // ========================================================================
    // ==== COMPOSITE CONCRETO ====
    // AndSpecification combina varias regras usando E logico.
    // Para ser aprovado, o produto precisa satisfazer todas as regras internas.
    // ========================================================================

    // ===== Classe =====
    public class AndSpecification<T> : CompositeSpecification<T>
    {
        // ===== Construtores =====
        public AndSpecification(params ISpecification<T>[] items) : base(items)
        {
        }

        // ===== Metodos =====
        public override bool IsSatisfied(T item)
        {
            // All representa o E logico entre as specifications filhas.
            // Para criar um OR, a ideia seria usar Any em uma OrSpecification.
            return items.All(specification => specification.IsSatisfied(item));
        }
    }

    // ========================================================================
    // ==== CLIENT ====
    // Demonstra que regras simples podem ser combinadas sem alterar BetterFilter.
    // ========================================================================

    // ===== Classe =====
    public class Demo
    {
        // ===== Metodos =====
        static void Main(string[] args)
        {
            var apple = new Product("Apple", Color.Green, Size.Small);
            var tree = new Product("Tree", Color.Green, Size.Large);
            var house = new Product("House", Color.Blue, Size.Large);

            Product[] products = { apple, tree, house };

            var green = new ColorSpecification(Color.Green);
            var large = new SizeSpecification(Size.Large);

            // O operador & cria uma specification composta:
            // produto precisa ser verde E grande.
            var greenAndLarge = green & large;

            var filter = new BetterFilter();

            WriteLine("Produtos verdes e grandes:");

            foreach (var product in filter.Filter(products, greenAndLarge))
            {
                WriteLine($" - {product.Name}");
            }
        }
    }
}
