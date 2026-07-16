using System;

// ============================================================================
// Aula01 - Bridge (Padrão Ponte)
// ----------------------------------------------------------------------------
// O QUE É: O padrão Bridge (Ponte) desacopla uma abstração de sua implementação,
//          permitindo que as duas possam variar de forma totalmente independente.
//          Ele substitui uma relação de herança pesada por uma relação de 
//          composição muito mais leve e escalável.
//
// O PROBLEMA DE NEGÓCIO / CENÁRIO DA HISTÓRIA:
// * Nós temos formas geométricas (Abstrações: `Circle`, `Square`) e precisamos 
//   renderizá-las em diferentes mídias (Implementações: `VectorRenderer`, `RasterRenderer`).
// * Se usássemos herança direta para resolver isso, cairíamos no problema da
//   EXPLOSÃO CARTESIANA de classes:
//     - Precisaríamos criar: `VectorCircle`, `RasterCircle`, `VectorSquare`, `RasterSquare`...
//     - Se adicionássemos mais 3 formas e mais 2 renderizadores, o número de 
//       classes necessárias escalaria de forma descontrolada: $M \times N$ classes!
//
// A SOLUÇÃO (Bridge):
// * Em vez de dizer "um círculo vetorizado é uma classe filha de círculo", nós
//   dizemos: "uma forma geométrica POSSUI um renderizador".
// * Criamos uma interface para a implementação (`IRenderer`) e fazemos a classe
//   base da abstração (`Shape`) segurar uma referência para essa interface.
// * Agora, novas formas e novos renderizadores podem ser criados sem que um
//   interfira na estrutura do outro. Reduzimos a complexidade de $M \times N$
//   para apenas $M + N$ classes!
// ============================================================================
namespace Aula01_Bridge
{
    // ========================================================================
    // ==== A PONTE (THE IMPLEMENTOR) ====
    // Esta interface define o contrato para as implementações concretas de
    // renderização. A abstração ("Shape") vai consumir esse contrato por
    // composição, delegando o trabalho técnico para quem o implementar.
    // ========================================================================
    public interface IRenderer
    {
        void RenderCircle(float radius);
        // Se quiséssemos adicionar suporte a quadrado, adicionaríamos aqui:
        // void RenderSquare(float side);
    }

    // ========================================================================
    // ==== IMPLEMENTAÇÕES CONCRETAS (CONCRETE IMPLEMENTORS) ====
    // Estas classes realizam o trabalho de baixo nível, cada uma focada em
    // um tipo de tecnologia ou plataforma de desenho específica.
    // ========================================================================
    
    // Renderizador focado em desenho vetorial (linhas matemáticas de precisão)
    public class VectorRenderer : IRenderer
    {
        public void RenderCircle(float radius)
        {
            Console.WriteLine($"Drawing a circle of radius {radius} as vectors.");
        }
    }

    // Renderizador focado em desenho rasterizado (geração pixel por pixel)
    public class RasterRenderer : IRenderer
    {
        public void RenderCircle(float radius)
        {
            Console.WriteLine($"Drawing a circle of radius {radius} as pixels.");
        }
    }

    // ========================================================================
    // ==== A ABSTRAÇÃO (THE ABSTRACTION) ====
    // Esta é a raiz da hierarquia de conceitos de alto nível.
    // Em vez de herdar diretamente os caminhos de desenho, ela usa COMPOSIÇÃO:
    // carrega uma referência protegida ('renderer') para a interface 'IRenderer'.
    // ========================================================================
    public abstract class Shape
    {
        // A "Ponte" física no código. A abstração se comunica com a
        // implementação exclusivamente através desta referência.
        protected IRenderer renderer;

        // O construtor exige o injetor de implementação (geralmente via DI),
        // amarrando a abstração ao motor de renderização escolhido no nascimento.
        protected Shape(IRenderer renderer)
        {
            this.renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
        }

        // Métodos abstratos que as formas concretas precisarão implementar para
        // definir suas lógicas e regras de negócio de alto nível.
        public abstract void Draw();
        public abstract void Resize(float factor);
    }

    // ========================================================================
    // ==== ABSTRAÇÃO REFINADA (REFINED ABSTRACTION) ====
    // Uma especialização da abstração que adiciona comportamento específico do
    // conceito de negócio (Círculo), sem se preocupar em COMO ele será desenhado.
    // ========================================================================
    public class Circle : Shape
    {
        private float radius;

        // Repare que o Círculo não sabe (nem quer saber) se o renderizador é
        // vetorizado ou rasterizado. Ele apenas repassa a dependência para a base.
        public Circle(IRenderer renderer, float radius) : base(renderer)
        {
            this.radius = radius;
        }

        // Implementação da lógica de desenho: delegamos a responsabilidade técnica
        // para o renderizador associado via "Ponte".
        public override void Draw()
        {
            renderer.RenderCircle(radius);
        }

        // Regra de negócio própria do domínio "Círculo"
        public override void Resize(float factor)
        {
            radius *= factor;
        }
    }

    // ========================================================================
    // ==== DEMONSTRAÇÃO DE USO (CLIENT) ====
    // ========================================================================
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("==========================================================");
            Console.WriteLine("                DEMONSTRAÇÃO DO PADRÃO BRIDGE            ");
            Console.WriteLine("==========================================================\n");
            Console.ResetColor();

            // Criamos a implementação concreta (o motor gráfico)
            var vectorRenderer = new VectorRenderer();
            var rasterRenderer = new RasterRenderer();

            // 1. Criamos a Abstração ("Circle") passando a Implementação por injeção
            var circle1 = new Circle(vectorRenderer, 5.0f);
            Console.WriteLine("--- Desenhando Círculo Vetorial ---");
            circle1.Draw(); // Saída orientada a vetores

            // 2. Criamos outra instância da Abstração, mas com motor gráfico diferente
            var circle2 = new Circle(rasterRenderer, 10.0f);
            Console.WriteLine("\n--- Desenhando Círculo Rasterizado ---");
            circle2.Draw(); // Saída orientada a pixels

            // Modificamos o estado do círculo através da abstração e renderizamos novamente
            Console.WriteLine("\n--- Redimensionando Círculo Vetorial ---");
            circle1.Resize(2.0f); // Dobra o raio de 5.0 para 10.0
            circle1.Draw();

            Console.WriteLine("\n==========================================================");
        }
    }
}