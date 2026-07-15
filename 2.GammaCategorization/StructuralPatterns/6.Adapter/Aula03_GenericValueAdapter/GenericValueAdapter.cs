using System;
using System.Linq;

// ============================================================================
// Aula - Adapter de Tipos e Dimensões Genéricas (Metaprogramação em C#)
// ----------------------------------------------------------------------------
// O PROBLEMA QUE ESTAMOS RESOLVENDO:
// * Em linguagens como C++, é possível passar valores numéricos literais como 
//   parâmetros genéricos de template (ex: `std::array<int, 3>` cria um array de 
//   tamanho físico 3 garantido em tempo de compilação).
// * O C# nativamente NÃO suporta parâmetros genéricos de valor (value-typed 
//   generic parameters); a linguagem exige que todo parâmetro genérico `<T>` 
//   seja estritamente um Tipo (uma classe, interface, struct, etc.).
// * Devido a isso, criar estruturas matemáticas como vetores multidimensionais 
//   (Vector2, Vector3, Vector4) costuma exigir a criação de classes totalmente 
//   separadas e redundantes para cada dimensão, ou abrir mão da segurança de 
//   tipos em tempo de compilação usando arrays dinâmicos.
//
// A SOLUÇÃO (PADRÃO ADAPTER DE VALOR PARA TIPO):
// * Adaptamos um valor numérico bruto para que ele se comporte como um "Tipo" 
//   válido para o compilador do C#.
// * Para isso, definimos uma interface comum (`IInteger`) que expõe um valor 
//   inteiro e criamos classes pequenas e leves (`Two`, `Three`) para implementá-la.
// * Ao passar essas classes como parâmetro genérico (`TDimension`), o C# consegue 
//   instanciá-las dinamicamente e ler seu valor numérico interno para definir, 
//   por exemplo, o tamanho do array de dados de um vetor.
// * Isso nos permite criar uma única classe genérica de Vetor que se adapta 
//   perfeitamente para 2, 3 ou "N" dimensões, impedindo erros bizarros (como 
//   tentar somar um vetor 2D com um 3D) diretamente na compilação!
// ============================================================================
namespace DotNetDesignPatternDemos.Structural.Adapter
{
  // === O CONTRATO DO ADAPTER ===
  // Qualquer tipo que queira representar uma dimensão numérica na assinatura genérica
  // precisará implementar esta interface para expor seu valor inteiro correspondente.
  public interface IInteger
  {
    int Value { get; }
  }

  // === AS DIMENSÕES CONCRETAS (OS ADAPTADORES) ===
  // Estas classes servem exclusivamente como "metadados" para o compilador.
  // Elas encapsulam os números 2 e 3 para que possam ser passados como parâmetros genéricos `<TDimension>`.
  public static class Dimensions
  {
    public class Two : IInteger
    {
      public int Value => 2;
    }

    public class Three : IInteger
    {
      public int Value => 3;
    }
  }

  // === O VETOR GENÉRICO PAI ===
  // Aqui usamos uma técnica avançada conhecida como CRTP (Curiously Recurring Template Pattern).
  // * TDerivedClass: Garante que a classe base conheça o tipo da classe filha (útil para métodos fluídos/fábricas).
  // * TDataType: O tipo de dado interno (int, float, double, etc.).
  // * TDimension: A dimensão do vetor (deve herdar de IInteger e ter um construtor sem parâmetros).
  public class Vector<TDerivedClass, TDataType, TDimension>
    where TDimension : IInteger, new()
    where TDerivedClass : Vector<TDerivedClass, TDataType, TDimension>, new()
  {
    // O array interno que armazenará os dados físicos do vetor.
    protected TDataType[] data;

    // Construtor Padrão: Inicializa o array com o tamanho exato extraído do tipo genérico 'TDimension'.
    public Vector()
    {
      // 'new TDimension().Value' instancia o metadado (ex: Two) e extrai seu valor inteiro (ex: 2) em tempo de execução.
      data = new TDataType[new TDimension().Value];
    }

    // Construtor de Inicialização: Aceita múltiplos valores e os copia respeitando o limite da dimensão 'TDimension'.
    public Vector(params TDataType[] values)
    {
      var requiredSize = new TDimension().Value;
      data = new TDataType[requiredSize];

      var providedSize = values.Length;

      // Preenche o array interno até o menor limite entre o que foi fornecido e o que é obrigatório.
      for (int i = 0; i < Math.Min(requiredSize, providedSize); ++i)
        data[i] = values[i];
    }

    // Método Fábrica (Factory Method) estático:
    // Graças ao 'TDerivedClass' do CRTP, este método consegue criar e retornar exatamente o tipo da classe filha!
    public static TDerivedClass Create(params TDataType[] values)
    {
      var result = new TDerivedClass();
      var requiredSize = new TDimension().Value;
      result.data = new TDataType[requiredSize];

      var providedSize = values.Length;

      for (int i = 0; i < Math.Min(requiredSize, providedSize); ++i)
        result.data[i] = values[i];

      return result;
    }

    // Indexador (Indexer): Permite acessar os elementos do vetor usando colchetes (ex: vetor[0] = 5).
    public TDataType this[int index]
    {
      get => data[index];
      set => data[index] = value;
    }

    // Propriedade de Atalho: Facilita o acesso direto à primeira coordenada (X) sem precisar de índice.
    public TDataType X
    {
      get => data[0];
      set => data[0] = value;
    }

    // Formatação amigável para exibir os dados do vetor de forma legível no console.
    public override string ToString()
    {
      return $"[{string.Join(", ", data)}]";
    }
  }

  // === ESPECIALIZAÇÃO PARA FLOATS ===
  // Uma classe intermediária que amarra o tipo de dado interno como 'float'.
  public class VectorOfFloat<TDerivedClass, TDimension> 
    : Vector<TDerivedClass, float, TDimension>
    where TDimension : IInteger, new() 
    where TDerivedClass : Vector<TDerivedClass, float, TDimension>, new()
  {
  }

  // === ESPECIALIZAÇÃO PARA INTEIROS COM SUPORTE A SOMA ===
  // Esta classe intermediária amarra o tipo interno como 'int'.
  // Ela aproveita para implementar operadores matemáticos específicos que só fazem sentido para números inteiros.
  public class VectorOfInt<TDimension> : Vector<VectorOfInt<TDimension>, int, TDimension>
    where TDimension : IInteger, new()
  {
    public VectorOfInt()
    {
    }

    public VectorOfInt(params int[] values) : base(values)
    {
    }

    // Sobrecarga do Operador de Soma (+):
    // Permite somar dois vetores de forma extremamente intuitiva (vetorA + vetorB).
    // O compilador garante, via tipo genérico 'TDimension', que só podemos somar vetores de dimensões IDÊNTICAS!
    public static VectorOfInt<TDimension> operator +
      (VectorOfInt<TDimension> leftVector, VectorOfInt<TDimension> rightVector)
    {
      var result = new VectorOfInt<TDimension>();
      var dimensionValue = new TDimension().Value;
      for (int i = 0; i < dimensionValue; i++)
      {
        result[i] = leftVector[i] + rightVector[i];
      }

      return result;
    }
  }

  // === IMPLEMENTAÇÃO CONCRETA: Vetor Inteiro de 2 Dimensões ===
  // Note como passamos 'Dimensions.Two' como parâmetro genérico.
  // Isso cria um vetor que possui, por definição de tipo, o tamanho físico exato de 2 posições.
  public class Vector2i : VectorOfInt<Dimensions.Two>
  {
    public Vector2i()
    {
    }

    public Vector2i(params int[] values) : base(values)
    {
    }
  }

  // === IMPLEMENTAÇÃO CONCRETA: Vetor de Floats de 3 Dimensões ===
  // Aqui passamos a nós mesmos como 'TDerivedClass' (Vector3f) e a dimensão 'Dimensions.Three'.
  public class Vector3f 
    : VectorOfFloat<Vector3f, Dimensions.Three>
  {
    // Formatação amigável para exibir os dados do vetor de forma legível no console.
    public override string ToString()
    {
      return $"{string.Join(",", data)}";
    }
  }
  
  // === ÁREA DE TESTE (DEMONSTRAÇÃO) ===
  class Demo
  {
    public static void Main(string[] args)
    {
      Console.ForegroundColor = ConsoleColor.Cyan;
      Console.WriteLine("==========================================================");
      Console.WriteLine("   DEMONSTRAÇÃO: ADAPTER DE VALOR PARA TIPO (VETORES)     ");
      Console.WriteLine("==========================================================\n");
      Console.ResetColor();

      // 1. Criação de um vetor bidimensional de inteiros (X, Y)
      var vectorA = new Vector2i(1, 2);
      vectorA[0] = 0; // Alterando o primeiro elemento usando o indexador
      Console.WriteLine($"Vetor A criado e modificado: {vectorA}");
      
      var vectorB = new Vector2i(3, 2);
      Console.WriteLine($"Vetor B criado: {vectorB}");

      // 2. Operação matemática segura de tipo.
      // O compilador sabe que 'vectorA' e 'vectorB' são ambos 'Vector2i' e permite a soma.
      // CORREÇÃO: Usamos 'var' para inferir corretamente o retorno do operador '+' como 'VectorOfInt<Dimensions.Two>'
      var result = vectorA + vectorB;
      Console.WriteLine($"Resultado da Soma (A + B): [{string.Join(", ", result[0], result[1])}]");
      Console.WriteLine();

      // 3. Criação de um vetor tridimensional de floats usando o Factory Method estático.
      // Graças ao CRTP, o tipo retornado é exatamente 'Vector3f' e não o tipo genérico pai.
      Vector3f vector3D = Vector3f.Create(3.5f, 2.2f, 1);
      Console.WriteLine($"Vetor 3D criado via Factory (CRTP): {vector3D}");
      Console.WriteLine("\n==========================================================");
    }
  }
}