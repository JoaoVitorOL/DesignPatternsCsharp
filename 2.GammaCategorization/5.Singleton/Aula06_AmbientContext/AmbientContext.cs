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
// O QUE É E PARA QUE SERVE?
// Imagine que você está desenhando no Photoshop. Se você quiser pintar 10 linhas
// de vermelho, você não clica na cor vermelha para CADA linha que desenha. 
// Você seleciona a cor vermelha UMA vez e o programa entra em um "ambiente" onde 
// o pincel atual é vermelho. Qualquer linha desenhada a partir dali descobre a
// cor sozinha, olhando para o "ambiente" do programa.
//
// O Ambient Context faz exatamente isso no código: ele cria uma configuração
// "no ar" (um contexto) para que os objetos criados ali dentro se configurem
// sozinhos, sem que você precise ficar passando parâmetros manualmente toda hora.
//
// POR QUE NÃO USAR PARÂMETROS COMUNS? (O problema)
// Se você não usar este padrão, para criar uma parede você teria que escrever:
// `new Wall(pontoA, pontoB, altura)`
// Se o seu prédio tiver 500 paredes de mesma altura, você terá que passar o
// número "3000" quinhentas vezes. Se uma função chamar outra que chama outra 
// para criar a parede, todas elas precisarão receber a variável `altura` só para 
// passar adiante. Isso entope o código de parâmetros inúteis (Parameter Pollution).
//
// POR QUE NÃO USAR O SINGLETON TRADICIONAL (Aula01)?
// O Singleton cria UMA única instância para o programa inteiro e acabou. Se o 
// seu banco de dados ou a sua altura padrão for um Singleton fixo de 3000, e no 
// meio do código você precisar criar uma parede de 3500 (o teto), você quebra 
// o sistema, porque o Singleton não aceita variações locais ou temporárias facilmente.
//
// COMO ELE FUNCIONA NA PRÁTICA AQUI?
// 1. O bloco `using (new BuildingContext(3000))` funciona como ligar o "Caps Lock".
// 2. Qualquer `new Wall(...)` criada dentro desse bloco não pede altura. Ela 
//    olha "pro ar" (`BuildingContext.Current`) e descobre que a altura atual é 3000.
// 3. Quando o bloco `using` fecha, o C# limpa o ambiente (chama o `Dispose`) e 
//    desliga o "Caps Lock", voltando tudo ao normal.
// ============================================================================
namespace Aula06_AmbientContext
{
    // ===== Classe =====
    // `BuildingContext` gerencia o ciclo de vida do contexto atual do edifício.
    // Implementa `IDisposable` para que possa ser utilizado em estruturas `using`,
    // limpando o estado de forma automática e segura ao fim do escopo.
    public class BuildingContext : IDisposable
    {
        // ===== Campos =====
        public int WallHeight;

        // Uma pilha (`Stack`) estática controla o empilhamento de contextos.
        // Isso permite a criação de contextos aninhados (nested contexts), onde
        // um novo contexto temporário sobrescreve o anterior e, ao ser destruído,
        // restaura o estado que estava no topo da pilha.
        private static Stack<BuildingContext> stack = new Stack<BuildingContext>();

        // ===== Construtores =====
        // Construtor estático usado para inicializar a pilha com um contexto padrão.
        // Garante que `stack.Peek()` nunca falhe por falta de elementos na memória.
        static BuildingContext()
        {
            stack.Push(new BuildingContext(0));
        }

        // Construtor de instância que aceita uma configuração e a define como
        // o contexto ativo atual ao empilhá-la na coleção estática.
        public BuildingContext(int wallHeight)
        {
            WallHeight = wallHeight;
            stack.Push(this);
        }

        // ===== Propriedades =====
        // Ponto de acesso global e estático para ler o contexto que está ativo
        // no momento (o topo da pilha). Substitui a necessidade de passar o dado de fora.
        public static BuildingContext Current => stack.Peek();

        // ===== Métodos =====
        // O método `Dispose` é acionado ao final de um bloco `using`.
        // Ele desempilha o contexto atual, restaurando o contexto anterior.
        public void Dispose()
        {
            // Proteção para nunca remover o contexto padrão (raiz) inserido no construtor estático.
            if (stack.Count > 1)
            {
                stack.Pop();
            }
        }
    }

    // ===== Estrutura =====
    // Representação simples de coordenadas bidimensionais.
    public struct Point
    {
        // ===== Campos =====
        private int x, y;

        // ===== Construtores =====
        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        // ===== Métodos =====
        public override string ToString()
        {
            return $"({x}, {y})";
        }
    } 

    // ===== Classe =====
    // Entidade que consome implicitamente o Ambient Context.
    public class Wall
    {
        // ===== Campos =====
        public Point Start, End;
        public int Height;

        // ===== Construtores =====
        public Wall(Point start, Point end)
        {
            Start = start;
            End = end;
            
            // O cerne do padrão Ambient Context está nesta linha:
            // A parede descobre sua altura consultando de forma transparente a propriedade estática
            // `BuildingContext.Current`. A assinatura do construtor permanece limpa, sem parâmetros extras.
            Height = BuildingContext.Current.WallHeight; 
        }

        // ===== Métodos =====
        public override string ToString()
        {
            return $"Wall from {Start} to {End} with Height {Height}";
        }
    }

    // ===== Classe =====
    public class Building
    {
        // ===== Campos =====
        public List<Wall> Walls = new List<Wall>();

        // ===== Métodos =====
        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var wall in Walls)
            {
                sb.AppendLine(wall.ToString());
            }
            return sb.ToString();
        }
    }

    // ===== Classe =====
    public class Demo
    {
        // ===== Métodos =====
        public static void Main(string[] args)
        {
            var house = new Building();

            // Através dos blocos `using`, criamos escopos bem definidos de execução.
            // Qualquer `Wall` criada dentro deste primeiro escopo de 3000ms absorverá 
            // essa altura automaticamente sem que precisemos informá-la por parâmetro.
            // [FIX] Removed 'height' parameter since Ambient Context handles it implicitly
            using (new BuildingContext(3000))
            {
                house.Walls.Add(new Wall(new Point(0, 0), new Point(5000, 0)));
                house.Walls.Add(new Wall(new Point(0, 0), new Point(0, 4000)));
            } // O Dispose() é disparado aqui, removendo o contexto de 3000 da pilha.

            // Novo bloco com escopo configurado para 3500ms.
            using (new BuildingContext(3500))
            {
                house.Walls.Add(new Wall(new Point(0, 0), new Point(6000, 0)));
                house.Walls.Add(new Wall(new Point(0, 0), new Point(0, 4000)));
            } // O Dispose() é disparado aqui, removendo o contexto de 3500 da pilha.

            // Retorno temporário ao escopo de 3000ms.
            using (new BuildingContext(3000))
            {
                house.Walls.Add(new Wall(new Point(5000, 0), new Point(5000, 4000)));
            }

            
            Console.WriteLine(house);
        }
    }
}