using System;
using System.Collections.Generic;
using static System.Console;

namespace AulasSOLIDpatterns.Aula02_OpenClosed
{


// ===========================================
// Enumeradores para as propriedades do produto
// ===========================================

// ===== Enum =====
public enum Color
{
    Red, Green, Blue, Black, White
}

// ===== Enum =====
public enum Size
{
    Small, Medium, Large, ExtraLarge
}


// ===========================================
//  Classe de Produto
// ===========================================

// ===== Classe =====
public class Product
{
    // ===== Propriedades =====
    public string Name { get; set; }
    public Color Color { get; set; }
    public Size Size { get; set; }

    // O construtor garante que o produto ja nasca com seus dados principais.

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

// !!!!! CLASSE DE FILTROS DE PRODUTO !!!!!!!!!!!!!!!!!!!!!!!
// Por que a classe respeita o principio Open/Closed?
// Da forma que esta, toda vez que quisermos adicionar um novo criterio de filtragem
// ou uma nova propriedade do produto, nao teremos que modificar a classe "BetterFilter".
// Isso esta de acordo com o principio Open/Closed, pois ela esta fechada para modificacoes
// e aberta para extensoes por meio de novas especificacoes.
// BetterFilter implementa o contrato IFilter<Product>.
// Ou seja: ela promete saber filtrar produtos usando qualquer regra valida.

// ===== Classe =====
public class BetterFilter : IFilter<Product>
{
    // IEnumerable<Product> significa "uma sequencia de produtos percorrivel com foreach".

    // ===== Metodos =====
    public IEnumerable<Product> Filter(IEnumerable<Product> items, ISpecification<Product> spec)
    {
        foreach (var i in items)
        {
            
            // O filtro nao decide a regra; ele delega isso para a especificacao recebida.
            if (spec.IsSatisfied(i))
            {
                // yield return devolve os itens aprovados um a um, sem montar lista manual.
                yield return i;
            }
        }
    }
}

// Interface = contrato.
// ISpecification<T> esta dizendo:
// "qualquer classe que entrar aqui precisa ter um metodo
// capaz de dizer se um item satisfaz ou nao uma regra".

// ===== Interface =====
public interface ISpecification<T>
{
    // ===== Metodos =====
    bool IsSatisfied(T item);
}

// Interface = contrato.
// IFilter<T> esta dizendo:
// "qualquer classe que entrar aqui precisa receber itens + uma regra
// e devolver so os itens aprovados por essa regra".

// ===== Interface =====
public interface IFilter<T>
{
    // ===== Metodos =====
    IEnumerable<T> Filter(IEnumerable<T> items, ISpecification<T> spec);
}

// Esta classe cumpre o contrato ISpecification<Product>.
// A regra concreta aqui e: "o produto tem este tamanho?"

// ===== Classe =====
public class SizeSpecification : ISpecification<Product>
{
    // ===== Campos =====
    private Size size;

    // ===== Construtores =====
    public SizeSpecification(Size size)
    {
        this.size = size;
    }

    // ===== Metodos =====
    public bool IsSatisfied(Product item)
    {
        return item.Size == size;
    }
}

// Esta classe tambem cumpre o contrato ISpecification<Product>.
// A regra concreta aqui e: "o produto tem esta cor?"

// ===== Classe =====
public class ColorSpecification : ISpecification<Product>
{
    // ===== Campos =====
    private Color color;

    // ===== Construtores =====
    public ColorSpecification(Color color)
    {
        this.color = color;
    }

    // ===== Metodos =====
    public bool IsSatisfied(Product item)
    {
        // Retorna true quando o produto atende a regra atual; senao, false.
        return item.Color == color;
    }
}





// ===== Classe =====
public class OpenClosedMain
{
    // ===== Metodos =====
    public static void RunDemo()
    {
        // Criamos alguns produtos para testar.
        var apple = new Product("Apple", Color.Green, Size.Small);
        var tree = new Product("Tree", Color.Green, Size.Large);
        var house = new Product("House", Color.Blue, Size.Large);

        Product[] products = { apple, tree, house };

        var bf = new BetterFilter();
        WriteLine("Produtos verdes (novo):");

        // Em vez de alterar BetterFilter para cada caso novo,
        // apenas plugamos uma nova especificacao.
        foreach (var p in bf.Filter(products, new ColorSpecification(Color.Green)))
        {
            WriteLine($" - {p.Name} é verde");
        }
    }
}

}
