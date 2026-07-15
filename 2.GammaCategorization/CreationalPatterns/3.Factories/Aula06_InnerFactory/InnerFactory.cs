using System;
using static System.Console;

namespace Aula06_InnerFactory
{
    // A aula mostra uma variacao de organizacao da mesma ideia de factory:
    // centralizar a criacao em um ponto controlado e com nomes claros.
    // Importante: isso NAO significa que `InnerFactory` seja um pattern GoF separado.
    // Aqui, "inner" quer dizer apenas que a factory foi aninhada dentro do produto.
    // 1. Factory externa: a criacao vai para algo como `PointFactory`.
    // 2. Inner Factory: a criacao continua separada, mas agora mora em `Point.InnerFactory` (Mora dentro do produto).
    // Em C#, isso ainda permite a factory acessar o construtor privado do produto.


    // ===== Classe =====
    public class Point
    {
        // ===== Campos =====
        private readonly double x;
        private readonly double y;

        // Repare na simplificacao do produto:
        // `Point` nao quer mais saber se nasceu de coordenadas cartesianas
        // ou polares. Essa decisao fica na factory.
        //
        // O produto agora so recebe o formato interno final: x/y.
        // Isso reduz a responsabilidade de `Point` para "guardar estado
        // valido" em vez de tambem carregar varias regras de criacao.

        // ===== Construtores =====
        private Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        // ===== Metodos =====
        public override string ToString()
        {
            return $"({nameof(x)}: {x}, {nameof(y)}: {y})";
        }

        // =====  Inner Factory =====
        public static class InnerFactory
        {
            // Importante:
            // `InnerFactory` nao e um pattern GoF separado.
            // Aqui ele representa uma forma de ORGANIZAR a factory:
            // a fabrica continua separada da API principal de `Point`,
            // mas fica aninhada dentro do proprio tipo.
            //
            // Compare mentalmente com a aula `Factory`:
            // - `PointFactory.NewPolarPoint(...)` -> factory externa
            // - `Point.InnerFactory.NewPolarPoint(...)` -> factory interna
            //
            // A responsabilidade e praticamente a mesma nos dois casos.
            // O que muda e o lugar onde a factory mora.
            //
            // Em C#, isso traz um detalhe util: como a factory e um tipo
            // aninhado, ela pode acessar o construtor privado de `Point`.
            // Assim, o produto continua fechado para `new Point(...)`
            // vindo de fora, mas a criacao controlada continua liberada.

            // ===== Metodos =====
            public static Point NewCartesianPoint(double x, double y)
            {
                // Caminho mais simples:
                // os dados ja chegaram no formato interno esperado.
                return new Point(x, y);
            }

            // Aqui o cliente fala em rho/theta,
            // mas o produto continua recebendo apenas x/y.
            // A conversao fica encapsulada na factory.
            public static Point NewPolarPoint(double rho, double theta)
            {
                return new Point(rho * Math.Cos(theta), rho * Math.Sin(theta));
            }
        }
    }






    // ===== Classe =====
    public class Demo
    {
        // ===== Metodos =====
        static void Main(string[] args)
        {
            // O cliente continua com uma chamada semantica e explicita,
            // mas sem inflar tanto a API principal de `Point`.
            //
            // Leitura mental:
            // "quero um Point, e vou usar a fabrica que pertence a Point
            // para escolher o caminho correto de criacao".
            var point = Point.InnerFactory.NewPolarPoint(1.0, Math.PI / 2);
            WriteLine(point);


        }
    }
}
