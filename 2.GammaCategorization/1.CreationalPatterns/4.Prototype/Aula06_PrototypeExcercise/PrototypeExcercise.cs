using System;

//  Exercicio:
//  implemente o padrao Prototype para criar uma copia profunda de uma Line.
//
//  Importante:
//  - o objetivo nao e apenas copiar os valores de forma rasa
//  - o objetivo e garantir que cada Point da nova Line seja uma instancia independente
//  - a copia deve preservar os dados, mas sem compartilhar o mesmo estado interno
//
//  Em outras palavras, o Prototype deve:
//  1. receber um objeto ja configurado como prototipo
//  2. criar uma nova instancia de Line
//  3. copiar os Points de forma independente
//  4. garantir que alteracoes na copia nao afetem o prototipo original
//
//  A API esperada para o uso do Prototype e esta:
//
//  var line = new Line(new Point(1, 2), new Point(3, 4));
//  var copy = line.DeepCopy();
//
//  O resultado esperado e:
//
//  copy.Start.X == 1
//  copy.End.Y == 4
//
//  Observe os detalhes pedidos no enunciado:
//  - Point.DeepCopy() deve criar uma nova instancia de Point
//  - Line.DeepCopy() deve criar uma nova Line com novos Points
//  - a copia deve ser profunda, nao rasa

using System;

namespace Coding.Exercise
{
    // 1o Defina o contrato de clonagem que o Prototype vai usar.

    // ===== Interface do Prototype =====
    public interface IPrototype<T>
    {
        T DeepCopy();
    }


    // 2o Defina o produto simples que sera copiado.

    // ===== Classe do produto =====
    public class Point : IPrototype<Point>
    {
      // ===== Campos =====
      public int X, Y;

      // ===== Construtores =====

      public Point()
      {
      }

      public Point(int x, int y)
      {
          X = x;
          Y = y;
      }

      // ===== Metodo de clonagem =====
      public Point DeepCopy()
      {
          // 3o Cada copia de Point precisa criar uma nova instancia, nao reaproveitar a antiga.
          return new Point(X, Y);
      }
    }

    // 4o Defina o objeto composto que usa os pontos como parte do estado.

    // ===== Classe composta =====
    public class Line : IPrototype<Line>
    {
      // ===== Campos =====
      public Point Start = new Point();
      public Point End = new Point();

      // ===== Construtores =====

      public Line()
      {
      }

      public Line(Point start, Point end)
      {
          if (start == null)
          {
              throw new ArgumentNullException(paramName: nameof(start));
          }
          if (end == null)
          {
              throw new ArgumentNullException(paramName: nameof(end));
          }

          Start = start.DeepCopy();
          End = end.DeepCopy();
          // o objetivo do DeeoCopy() no construtor
          // é garantir que a cópia de uma linha seja 100% independente da linha original.

      }

      // ===== Metodo de clonagem =====
      public Line DeepCopy()
      {
          // 5o Antes de clonar, a Line precisa ter os dois pontos configurados.
          if (Start == null || End == null)
          {
              throw new InvalidOperationException("Line must have both points configured before cloning.");
          }

          // 6o A copia profunda cria uma nova Line e tambem duplica cada Point.
          return new Line(Start.DeepCopy(), End.DeepCopy());
      }
    }

    // ===== Classe =====
    public static class Program
    {
        // ===== Metodos =====
        public static void Main()
        {
            // 7o O cliente cria um prototipo ja configurado.
            var line = new Line(new Point(1, 2), new Point(3, 4));

            // 8o O cliente pede uma copia profunda do prototipo.
            var copy = line.DeepCopy();

            // 9o Cada ponto da copia deve ser uma nova instancia e nao o mesmo objeto.
            Console.WriteLine(copy.Start.X);
            Console.WriteLine(copy.End.Y);
        }
    }
  }
