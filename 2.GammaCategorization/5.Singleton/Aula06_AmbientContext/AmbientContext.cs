using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.VisualBasic;
using System.Threading;         
using System.Threading.Tasks;
using System.Text;   

// ============================================================================
// Aula06 - Ambient Context (Contexto Ambiente)
// ----------------------------------------------------------------------------
// O QUE É: Cria uma configuração "no ar" (ambiente) para que novos objetos se
//          configurem sozinhos, sem precisar passar parâmetros manualmente.
//
// EVOLUÇÃO DA HISTÓRIA (Conexão com as aulas anteriores):
// * Aula01 (Singleton): Garante uma única instância pro programa todo, mas é 
//   rígido demais. Se você precisar mudar um valor temporariamente, quebra tudo.
// * Aula03 (Dependency Injection): Resolve a rigidez do Singleton passando tudo
//   por parâmetro no construtor. Mas se você tiver 500 objetos iguais, vai poluir
//   o código repetindo o mesmo parâmetro toda hora (Parameter Pollution).
// * Aula06 (Ambient Context): O equilíbrio perfeito. Ele dá o acesso fácil e 
//   global do Singleton, mas com a flexibilidade de mudar o valor em blocos 
//   isolados (usando o escopo do 'using'), limpando tudo ao fechar o bloco.
//
// ANALOGIAS PRÁTICAS:
// * Photoshop: Você escolhe a cor vermelha UMA vez. Todas as linhas desenhadas
//   a partir dali nascem vermelhas porque olham para a cor ativa do "ambiente".
// * Caps Lock: O bloco 'using' liga o Caps Lock. Tudo ali dentro sai em maiúsculo.
//   Fechou o bloco, o Caps Lock desliga automaticamente e o ambiente volta ao normal.
// ============================================================================
namespace Aula06_AmbientContext
{
    // ========================================================================
    // [IMPLEMENTAÇÃO DO PADRÃO]
    // Esta classe gerencia o estado do ambiente e define o escopo ativo.
    // ========================================================================
    public class BuildingContext : IDisposable
    {
        public int WallHeight;
        private static Stack<BuildingContext> stack = new Stack<BuildingContext>();

        static BuildingContext()
        {
            stack.Push(new BuildingContext(0)); // Contexto padrão (raiz)
        }

        public BuildingContext(int wallHeight)
        {
            WallHeight = wallHeight;
            stack.Push(this); // Ativa o novo contexto colocando-o no topo
        }

        // Ponto de acesso global para o código cliente consultar o "ambiente"
        public static BuildingContext Current => stack.Peek();

        public void Dispose()
        {
            if (stack.Count > 1)
            {
                stack.Pop(); // Remove o contexto atual ao fechar o bloco 'using'
            }
        }
    }

    public struct Point
    {
        private int x, y;
        public Point(int x, int y) { this.x = x; this.y = y; }
        public override string ToString() => $"({x}, {y})";
    } 

    public class Wall
    {
        public Point Start, End;
        public int Height;

        public Wall(Point start, Point end)
        {
            Start = start;
            End = end;
            
            // ================================================================
            // [CONSUMO DO PADRÃO]
            // A classe descobre seu estado buscando dados diretamente "no ar"
            // ================================================================
            Height = BuildingContext.Current.WallHeight; 
        }

        public override string ToString() => $"Wall from {Start} to {End} with Height {Height}";
    }

    public class Building
    {
        public List<Wall> Walls = new List<Wall>();

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var wall in Walls) sb.AppendLine(wall.ToString());
            return sb.ToString();
        }
    }

    public class Demo
    {
        public static void Main(string[] args)
        {
            var house = new Building();

            // ================================================================
            // [USO PRÁTICO DOS ESCOPOS]
            // O bloco 'using' define o tempo de vida de cada configuração global
            // ================================================================
            
            using (new BuildingContext(3000))
            {
                house.Walls.Add(new Wall(new Point(0, 0), new Point(5000, 0)));
                house.Walls.Add(new Wall(new Point(0, 0), new Point(0, 4000)));
            } 

            using (new BuildingContext(3500))
            {
                house.Walls.Add(new Wall(new Point(0, 0), new Point(6000, 0)));
                house.Walls.Add(new Wall(new Point(0, 0), new Point(0, 4000)));
            } 

            using (new BuildingContext(3000))
            {
                house.Walls.Add(new Wall(new Point(5000, 0), new Point(5000, 4000)));
            }
            
            Console.WriteLine(house);
        }
    }
}