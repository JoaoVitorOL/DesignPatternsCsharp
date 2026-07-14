using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Security.AccessControl;

using static System.Console;

// ============================================================================
// Aula01 - Vector Raster Demo (Padrão Adapter)
// ----------------------------------------------------------------------------
// O QUE É: O padrão Adapter (Adaptador) converte a interface de uma classe 
//           em outra interface que o cliente espera encontrar. Ele permite 
//           que classes com interfaces incompatíveis trabalhem juntas.
//
// PROBLEMA DE NEGÓCIO / CENÁRIO DA HISTÓRIA:
// * Nós temos objetos representados de forma VETORIAL (VectorObject, compostos 
//   por linhas definidas por pontos de início e fim: 'Line' de Start até End).
// * No entanto, a nossa única ferramenta de renderização/desenho disponível 
//   (a infraestrutura existente) só sabe desenhar PONTOS/PIXELS individuais 
//   na tela, ou seja, trabalha com dados RASTERIZADOS ('DrawPoint(Point p)').
// * Como desenhar retângulos e linhas se a nossa única função desenha apenas 
//   pontos individuais? Precisamos de um tradutor no meio do caminho.
//
// A SOLUÇÃO (Adapter):
// * Criamos o 'LineToPointAdapter'. Ele recebe uma linha ('Line') e, em tempo 
//   de execução, calcula todos os pixels necessários para renderizar aquela 
//   linha, comportando-se como uma coleção de pontos ('Collection<Point>').
// * O código que desenha agora pode iterar sobre o adaptador como se ele fosse 
//   uma simples lista de pontos e passá-los individualmente para 'DrawPoint'.
// ============================================================================
namespace Aula01_VectorRasterDemo
{
  // ========================================================================
  // ==== MODELO DE DOMÍNIO (PONTOS E LINHAS) ====
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

    public override string ToString()
    {
      return $"{nameof(X)}: {X}, {nameof(Y)}: {Y}";
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
  }

  // ========================================================================
  // ==== MODELO VETORIAL COMPLEXO ====
  // Representa coleções de linhas geométricas abstratas.
  // ========================================================================
  
  // Classe base para qualquer objeto vetorial (agrupa várias linhas)
  public class VectorObject : Collection<Line>
  {
    
  }

  // Uma representação de Retângulo construída a partir de 4 linhas conectadas
  public class VectorRectangle : VectorObject
  {
    public VectorRectangle(int x, int y, int width, int height)
    {
      // Linha superior
      Add(new Line(new Point(x,y), new Point(x+width, y) ));
      // Linha lateral direita
      Add(new Line(new Point(x+width,y), new Point(x+width, y+height) ));
      // Linha lateral esquerda
      Add(new Line(new Point(x,y), new Point(x, y+height) ));
      // Linha inferior
      Add(new Line(new Point(x,y+height), new Point(x+width, y+height) ));
    }
  }

  // ========================================================================
  // ==== O ADAPTADOR (ADAPTER) ====
  // [IMPLEMENTAÇÃO DO PADRÃO]
  // Esta classe converte uma 'Line' (dados vetoriais de início/fim) em uma 
  // coleção de pontos rasterizados ('Collection<Point>').
  // Herdando de Collection<Point>, o próprio adaptador se comporta como a 
  // estrutura de dados que o cliente final precisa consumir.
  // ========================================================================
  public class LineToPointAdapter : Collection<Point>
  {
    private static int count = 0;

    public LineToPointAdapter(Line line)
    {
      // Log explicativo para evidenciar no console o momento da conversão (e o problema de performance)
      WriteLine($"{++count}: Generating points for line [{line.Start.X},{line.Start.Y}]-[{line.End.X},{line.End.Y}] (no caching)");

      // Identifica os limites matemáticos da linha para fazer a interpolação pixel a pixel
      int left = Math.Min(line.Start.X, line.End.X);
      int right = Math.Max(line.Start.X, line.End.X);
      int top = Math.Min(line.Start.Y, line.End.Y);
      int bottom = Math.Max(line.Start.Y, line.End.Y);
      
      int dx = right - left;
      int dy = line.End.Y - line.Start.Y;

      // Algoritmo simples de rasterização para preencher a coleção interna:
      
      // Se for uma linha perfeitamente vertical (sem variação no eixo X)
      if (dx == 0)
      {
        for (int y = top; y <= bottom; ++y)
        {
          Add(new Point(left, y)); // Adiciona os pontos calculados à lista interna do Adapter
        }
      } 
      // Se for uma linha perfeitamente horizontal (sem variação no eixo Y)
      else if (dy == 0)
      {
        for (int x = left; x <= right; ++x)
        {
          Add(new Point(x, top)); // Adiciona os pontos calculados à lista interna do Adapter
        }
      }
    }
  }

  // ========================================================================
  // ==== DEMONSTRAÇÃO E CONSUMO ====
  // ========================================================================
  public class Demo
  {
    // Nossa "cena" composta por dois retângulos representados vetorialmente
    private static readonly List<VectorObject> vectorObjects = new List<VectorObject>
    {
      new VectorRectangle(1, 1, 10, 10),
      new VectorRectangle(3, 3, 6, 6)
    };

    // === A interface que nós possuímos ===
    // Este método é a limitação do nosso sistema: ele não sabe desenhar retângulos,
    // ele só consegue desenhar um único caractere "." em uma coordenada cartesiana.
    public static void DrawPoint(Point p)
    {
      Write(".");
    }

    static void Main()
    {
      // Desenhamos duas vezes de propósito para ilustrar um problema crônico de
      // adaptadores sem cache: eles recalculam tudo do zero a cada execução (desperdício de CPU).
      Draw();
      Draw();
    }

    // Método encarregado de unir o modelo de dados incompatível (Vetor) 
    // com a nossa tela física (Raster/Pixels) através do Adapter.
    private static void Draw()
    {
        foreach (var vo in vectorObjects)
        {
            foreach (var line in vo)
            {
                // ============================================================
                // [USO DO ADAPTADOR]
                // Convertemos cada linha do objeto vetorial para uma coleção 
                // de pontos compreensível para o método 'DrawPoint'.
                // ============================================================
                var adapter = new LineToPointAdapter(line);

                // Como o adapter herda de 'Collection<Point>', nós podemos 
                // simplesmente iterar sobre ele consumindo seus pontos.
                foreach (var point in adapter)
                {
                    DrawPoint(point); // Renderiza o ponto compatibilizado na tela
                }
            }
        }
    }
  }
}