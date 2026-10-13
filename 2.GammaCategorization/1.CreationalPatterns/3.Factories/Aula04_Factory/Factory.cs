using System;
using static System.Console;

namespace Aula04_Factory
{
    // ===== Factory =====
    public static class PointFactory
    {
        // Diferenca para a aula anterior:
        // no Factory Method, a criacao nomeada ainda ficava dentro do proprio `Point`.
        // Aqui, esse conhecimento sai do produto e vai para um componente separado.
        //
        // Isso faz sentido quando queremos:
        // - manter a entidade principal mais enxuta;
        // - centralizar a logica de criacao fora do produto;
        // - deixar a politica de criacao evoluir sem inflar a API de `Point`.

        // ===== Metodos =====
        public static Point NewCartesianPoint(double x, double y)
        {
            // Neste caso, os dados ja chegaram no formato interno esperado pelo produto.
            return new Point(x, y);
        }

        // A intencao continua explicita pelo nome,
        // mas agora a conversao nao pertence mais ao cliente
        // nem precisa ficar dentro da classe `Point`.
        public static Point NewPolarPoint(double rho, double theta)
        {
            return new Point(rho * Math.Cos(theta), rho * Math.Sin(theta));
        }
    }

    // ===== Classe =====
    public class Point
    {
        // ===== Campos =====
        private readonly double x;
        private readonly double y;

        // Repare na simplificacao do produto:
        // `Point` ja nao decide mais se a entrada era cartesiana ou polar.
        // Ele apenas recebe coordenadas normalizadas em x/y.
        //
        // Em um design mais completo, a factory separada pode ser a porta
        // preferencial de criacao e o produto pode focar apenas no seu estado.

        // ===== Construtores =====
        internal Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        // ===== Metodos =====
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
            // Agora o cliente nem precisa conversar com `Point` diretamente
            // para descobrir como ele deve nascer.
            // A criacao fica centralizada em um tipo dedicado.
            var point = PointFactory.NewPolarPoint(1.0, Math.PI / 2);
            WriteLine(point);
        }
    }
}
