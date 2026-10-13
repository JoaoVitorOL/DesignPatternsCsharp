using System;
using static System.Console;

namespace Aula02_FactoryMethod
{
    // ===== Classe =====
    public class Point
    {
        // ===== Campos =====
        private readonly double x;
        private readonly double y;

        // Factory Method:
        // em vez de um construtor ambiguo como no Point Example,
        // usamos um metodo nomeado para deixar explicito que
        // estes valores ja representam coordenadas cartesianas.

        // ===== Metodos =====
        public static Point NewCartesianPoint(double x, double y)
        {
            return new Point(x, y);
        }

        // Aqui a intencao tambem fica explicita pelo nome.
        // O cliente informa rho/theta e a conversao acontece
        // antes do objeto ser devolvido.
        public static Point NewPolarPoint(double rho, double theta)
        {
            return new Point(rho * Math.Cos(theta), rho * Math.Sin(theta));
        }

        // O construtor fica privado para impedir criacao "generica".
        // Assim, quem usa a classe e conduzido a escolher um caminho
        // de criacao com nome semantico.

        // ===== Construtores =====
        private Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public override string ToString()
        {
            return $"({nameof(x)}: {x}, {nameof(y)}: {y})";
        }
    }

    // ===== Classe =====
    public class Demo
    {
        // ===== Metodos =====
        static void Main(string[] args)
        {
            // O cliente nao precisa mais passar enum, flag ou parametro opcional.
            // Basta escolher o metodo que representa a intencao correta.
            var point = Point.NewPolarPoint(1.0, Math.PI / 2);
            WriteLine(point);
        }
    }
}
