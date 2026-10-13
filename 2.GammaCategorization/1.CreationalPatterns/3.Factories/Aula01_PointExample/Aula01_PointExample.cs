using System;
using static System.Console;

namespace Aula01_PointExample
{
    // ===== Enum =====
    public enum CoordinateSystem
    {
        Cartesian,
        Polar
    }

    // ===== Classe =====
    public class Point
    {
        // ===== Campos =====
        private readonly double x;
        private readonly double y;

        // Este construtor e "ruim de proposito" para fins didaticos.
        // Ele tenta suportar dois contratos diferentes de criacao:
        // - (x, y) quando o ponto nasce em coordenadas cartesianas
        // - (rho, theta) quando o ponto nasce em coordenadas polares
        //
        // O problema e que `a` e `b` nao tem significado fixo.
        // O chamador so entende a chamada se lembrar de uma regra externa:
        // qual CoordinateSystem foi passado, ou pior, qual ficou no default.

        // ===== Construtores =====
        public Point(double a, double b,
            CoordinateSystem system = CoordinateSystem.Cartesian)
        {
            switch (system)
            {
                case CoordinateSystem.Cartesian:
                    x = a;
                    y = b;
                    break;
                case CoordinateSystem.Polar:
                    x = a * Math.Cos(b);
                    y = a * Math.Sin(b);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(system), system, null);
            }
        }

        // ===== Metodos =====
        public override string ToString()
        {
            return $"({x:0.##}, {y:0.##})";
        }
    }

    // Problemas com a abordagem acima:
    // 1. Dois doubles (a,b) nao explicam sozinhos se estamos falando de x/y ou rho/theta.
    // 2. O enum `system` vira uma chave de interpretacao, nao uma API clara.
    // 3. O default `Cartesian` permite erro silencioso: o codigo compila mesmo quando a intencao era polar ou outro sistema.
    // 4. O construtor mistura criacao do objeto com conversao de coordenadas.
    // 5. O cliente fica dependente de documentacao e memoria, quando a API deveria comunicar a intencao.
    //
    // E exatamente aqui que factories ajudam:
    // Point.NewCartesian(3, 4)
    // Point.NewPolar(3, 4)
    //
    // Os nomes dos metodos tornam explicito o caminho de criacao e evitam ambiguidade.

    // ===== Classe =====
    public class Demo
    {
        // ===== Metodos =====
        static void Main(string[] args)
        {
            var cartesian = new Point(3, 4);
            var polar = new Point(3, 4, CoordinateSystem.Polar);

            WriteLine("Mesmo formato de entrada, significados diferentes:");
            WriteLine($"new Point(3, 4) -> {cartesian}");
            WriteLine($"new Point(3, 4, CoordinateSystem.Polar) -> {polar}");
            WriteLine();
            WriteLine("Se a intencao era polar e o enum for esquecido,");
            WriteLine("o codigo continua valido, mas o ponto criado fica errado.");
        }
    }
}
