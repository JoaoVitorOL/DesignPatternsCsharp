using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MoreLinq;
using NUnit.Framework;
using static System.Console;

// ============================================================================
// Aula 02 - Adapter com Caching (Padrão Adapter Otimizado)
// ----------------------------------------------------------------------------
// O QUE É: Uma evolução direta do Adapter básico. Aqui, adicionamos um mecanismo
//           de Cache temporário para evitar o recálculo redundante de dados.
//
// O PROBLEMA QUE ESTAMOS RESOLVENDO:
// * No Adapter básico, toda vez que a tela era atualizada (Frame), o construtor 
//   recalculava todos os pixels de todas as linhas do zero. 
// * Isso causava desperdício massivo de CPU e gerava milhares de objetos inúteis 
//   na memória (pressão no Garbage Collector), mesmo que o objeto estivesse parado.
//
// A SOLUÇÃO COM CACHE:
// * Guardamos os pontos gerados em um Dicionário Estático (`cache`), usando o 
//   código Hash da linha (`Line.GetHashCode()`) como chave única de busca.
// * Se o sistema pedir para adaptar uma linha que já foi calculada antes, nós 
//   simplesmente retornamos a lista de pontos já existente na memória instantaneamente.
// ============================================================================
namespace DotNetDesignPatternDemos.Structural.Adapter.WithCaching
{
  // ========================================================================
  // ==== MODELO DE DOMÍNIO ====
  // ========================================================================
  public class Point
  {
    public int X;
    public int Y;

    public Point(int x, int y)
    {
      this.X = x;
      this.Y = y;
    }

    // === COMPARANDO COMPORTAMENTO (Equivalência de Valor) ===
    // Por padrão, o C# compara objetos pela referência na memória (se são o mesmo objeto físico).
    // Sobrescrever o Equals garante que dois pontos com as mesmas coordenadas X e Y sejam considerados IGUAIS.
    protected bool Equals(Point other) // Overload de método
    { 
      return X == other.X && Y == other.Y;
    }

    public override bool Equals(object obj) // Overload de método
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != this.GetType()) return false;
      return Equals((Point) obj);
    }

    // === IMPORTANTE PARA O CACHE ===
    // O GetHashCode gera um "número de identidade digital" único para o objeto com base nos seus dados.
    // Como o Point será usado indiretamente como chave no dicionário (através do Hash da Line),
    // precisamos garantir que pontos com as mesmas coordenadas gerem EXATAMENTE o mesmo Hash.
    public override int GetHashCode()
    {
      unchecked
      {
        return (X * 397) ^ Y; // Multiplicador primo 397 reduz colisões de hash
      }
    }

    public override string ToString()
    {
      return $"({X}, {Y})";
    }
  }

  public class Line
  {
    public Point Start;
    public Point End;

    public Line(Point start, Point end)
    {
      this.Start = start;
      this.End = end;
    }

    // === COMPARANDO COMPORTAMENTO ===
    // Uma linha é considerada idêntica a outra se seus pontos de início e fim forem equivalentes.
    protected bool Equals(Line other) // méotodo overload
    {
      return Equals(Start, other.Start) && Equals(End, other.End);
    }

    public override bool Equals(object obj) // método overload
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != this.GetType()) return false;
      return Equals((Line) obj);
    }

    // === O CORAÇÃO DO CACHE ===
    // Esta função calcula o Hash exclusivo desta linha específica combinando o Hash do ponto de início
    // com o Hash do ponto final. O Dicionário de Cache usará esse inteiro retornado aqui como a CHAVE 
    // para buscar se a linha já foi convertida antes ou não.
    public override int GetHashCode()
    {
      unchecked
      {
        return ((Start != null ? Start.GetHashCode() : 0) * 397) ^ (End != null ? End.GetHashCode() : 0);
      }
    }
  }

  // ========================================================================
  // ==== MODELO VETORIAL ====
  // ========================================================================
  public abstract class VectorObject : Collection<Line>
  {
  }

  public class VectorRectangle : VectorObject
  {
    public VectorRectangle(int x, int y, int width, int height)
    {
      Add(new Line(new Point(x, y), new Point(x + width, y)));
      Add(new Line(new Point(x + width, y), new Point(x + width, y + height)));
      Add(new Line(new Point(x, y), new Point(x, y + height)));
      Add(new Line(new Point(x, y + height), new Point(x + width, y + height)));
    }
  }

  // ========================================================================
  // ==== O ADAPTADOR OTIMIZADO (ADAPTER WITH CACHING) ====
  // [MUDANÇA DE ARQUITETURA]:
  // Em vez de herdar de `Collection<Point>` (o que nos obrigaria a duplicar e carregar dados
  // na memória de cada instância), agora implementamos `IEnumerable<Point>`.
  // Isso nos permite expor uma coleção de pontos sem precisar manter uma lista física local; 
  // nós apenas "redirecionamos" quem nos lê para o cache estático centralizado!
  // ========================================================================
  public class LineToPointAdapter : IEnumerable<Point>
  {
    private static int count = 0;
    
    // O cache estático: chave (hash único da linha) -> valor (lista de pontos calculados para aquela linha)
    static Dictionary<int, List<Point>> cache = new Dictionary<int, List<Point>>();
    private int hash;

    public LineToPointAdapter(Line line)
    {
      // 1. Gera o código de identificação único da linha que foi solicitada para desenho
      hash = line.GetHashCode();
      
      // 2. [VERIFICAÇÃO DO CACHE]: Se o dicionário estático já possuir essa chave, abortamos o processo!
      // Não gastaremos processamento calculando nada. A linha já foi rasterizada no passado.
      if (cache.ContainsKey(hash)) return; 

      // 3. Se não estiver no cache, o console registrará a primeira (e única) geração dessa linha
      WriteLine($"{++count}: Generating points for line [{line.Start.X},{line.Start.Y}]-[{line.End.X},{line.End.Y}] (with caching)");

      List<Point> points = new List<Point>();

      int left = Math.Min(line.Start.X, line.End.X);
      int right = Math.Max(line.Start.X, line.End.X);
      int top = Math.Min(line.Start.Y, line.End.Y);
      int bottom = Math.Max(line.Start.Y, line.End.Y);
      int dx = right - left;
      int dy = line.End.Y - line.Start.Y;

      // Executa o cálculo matemático para preencher a lista local 'points'
      if (dx == 0)
      {
        for (int y = top; y <= bottom; ++y)
        {
          points.Add(new Point(left, y));
        }
      }
      else if (dy == 0)
      {
        for (int x = left; x <= right; ++x)
        {
          points.Add(new Point(x, top));
        }
      }

      // 4. [SALVANDO NO CACHE]: Registra os pontos gerados no dicionário estático sob a chave da linha.
      // Da próxima vez que precisarem dessa mesma linha, ela será resolvida de forma instantânea.
      cache.Add(hash, points);
    }

    // === SATISFAZENDO O CONTRATO IEnumerable ===
    // Quando o cliente tenta iterar (`foreach`) sobre o adaptador para desenhar seus pontos, 
    // nós não lemos uma lista interna do objeto. Nós vamos direto ao `cache` estático usando o 
    // `hash` do nosso objeto e retornamos o enumerador da lista de pontos persistida lá.
    public IEnumerator<Point> GetEnumerator()
    {
      return cache[hash].GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }
  
  // ========================================================================
  // ==== DEMONSTRAÇÃO DO GANHO DE PERFORMANCE ====
  // ========================================================================
  public class Demo
  {
    private static readonly List<VectorObject> vectorObjects = new List<VectorObject>
    {
      new VectorRectangle(1, 1, 10, 10),
      new VectorRectangle(3, 3, 6, 6)
    };

    public static void DrawPoint(Point p)
    {
      Write(".");
    }

    static void Main(string[] args)
    {
      // Chamamos o método Draw duas vezes seguidas para provar o cache.
      // * No primeiro Draw(), as linhas serão calculadas normalmente (logs aparecerão).
      // * No segundo Draw(), nenhuma matemática será executada! O console apenas imprimirá 
      //   os pontos "." sem disparar nenhuma mensagem "Generating points...".
      Draw();
      Draw();
    }

    private static void Draw()
    {
      foreach (var vo in vectorObjects)
      {
        foreach (var line in vo)
        {
          // Toda vez que instanciamos o adapter, ele apenas checa o cache.
          var adapter = new LineToPointAdapter(line);
          
          // O método estendido '.ForEach()' (da biblioteca MoreLinq) consome o IEnumerable
          // desenhando pixel a pixel na tela.
          adapter.ForEach(DrawPoint);
        }
      }
    }
  }
}