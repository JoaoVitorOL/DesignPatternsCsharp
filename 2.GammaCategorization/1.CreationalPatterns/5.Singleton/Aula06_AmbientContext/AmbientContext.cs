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
//           configurem sozinhos, sem precisar passar parâmetros manualmente.
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
    // ==== CLASSE ====
    // [IMPLEMENTAÇÃO DO PADRÃO]
    // Esta classe gerencia o estado do ambiente e define o escopo ativo.
    // Ela assina o ==== CONTRATO / INTERFACE (IDisposable) ====
    // ========================================================================
    public class BuildingContext : IDisposable
    {
        // ==== ATRIBUTO / CAMPO ====
        public int WallHeight;
        
        // ==== ATRIBUTO / CAMPO (Mecânica estática do Singleton) ====
        private static Stack<BuildingContext> stack = new Stack<BuildingContext>();

        // ==== CONSTRUTOR ESTÁTICO ====
        static BuildingContext()
        {
            stack.Push(new BuildingContext(0)); // Contexto padrão (raiz)
        }

        // ==== CONSTRUTOR DE INSTÂNCIA (Ativação do Ambient Context) ====
        public BuildingContext(int wallHeight)
        {
            WallHeight = wallHeight;
            stack.Push(this); // Ativa o novo contexto colocando-o no topo
        }

        // ==== PROPRIEDADE (Ponto de Acesso Global do Singleton) ====
        // Permite que o código cliente consulte o "ambiente" de qualquer lugar
        public static BuildingContext Current => stack.Peek();

        // ==== FUNÇÃO / MÉTODO (Implementação do Contrato IDisposable) ====
        public void Dispose()
        {
            if (stack.Count > 1)
            {
                stack.Pop(); // Remove o contexto atual ao fechar o bloco 'using'
            }
        }
    }

    // ========================================================================
    // ==== ESTRUTURA (STRUCT) ====
    // Tipo de valor leve usado para coordenadas planas.
    // ========================================================================
    public struct Point
    {
        // ==== ATRIBUTOS / CAMPOS PRIVADOS ====
        private int x, y;
        
        // ==== CONSTRUTOR ====
        public Point(int x, int y) { this.x = x; this.y = y; }
        
        // ==== FUNÇÃO / MÉTODO (Sobrecrita de comportamento padrão) ====
        public override string ToString() => $"({x}, {y})";
    } 

    // ========================================================================
    // ==== CLASSE ====
    // ========================================================================
    public class Wall
    {
        // ==== ATRIBUTOS / CAMPOS ====
        public Point Start, End;
        public int Height;

        // ==== CONSTRUTOR ====
        public Wall(Point start, Point end)
        {
            Start = start;
            End = end;
            
            // ================================================================
            // [CONSUMO DO PADRÃO]
            // A classe descobre seu estado buscando dados diretamente "no ar"
            // através da propriedade estática da classe de contexto.
            // ================================================================
            Height = BuildingContext.Current.WallHeight; 
        }

        // ==== FUNÇÃO / MÉTODO ====
        public override string ToString() => $"Wall from {Start} to {End} with Height {Height}";
    }

    // ========================================================================
    // ==== CLASSE ====
    // ========================================================================
    public class Building
    {
        // ==== ATRIBUTO / CAMPO (Instanciado) ====
        public List<Wall> Walls = new List<Wall>();

        // ==== FUNÇÃO / MÉTODO ====
        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var wall in Walls) sb.AppendLine(wall.ToString());
            return sb.ToString();
        }
    }

    // ========================================================================
    // ==== CLASSE ====
    // ========================================================================
    public class Demo
    {
        // ==== FUNÇÃO / MÉTODO PRINCIPAL (Ponto de entrada da aplicação) ====
        public static void Main(string[] args)
        {
            var house = new Building();

            // ================================================================
            // [USO PRÁTICO DOS ESCOPOS DO AMBIENT CONTEXT]
            // O bloco 'using' delimita o tempo de vida e escopo de cada contexto
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