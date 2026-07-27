using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

// ============================================================================
// Aula01 - Geometric Shapes (Padrao Composite)
// ----------------------------------------------------------------------------
// O QUE E: o padrao Composite permite representar estruturas parte-todo em
//          forma de arvore. O cliente trabalha com uma abstracao comum e pode
//          tratar objetos individuais e grupos de objetos de maneira uniforme.
//
// CENARIO:
// * Uma forma geometrica pode ser uma folha simples, como Circle ou Square.
// * Um desenho tambem pode ser um grupo contendo varias formas. []
// * Como um grupo pode conter outros grupos, a estrutura cresce como uma arvore. [[],[[],[]],[]]
//
// A SOLUCAO (Composite):
// * GraphicObject funciona como Component e, nesta versao transparente, tambem
//   como Composite, pois todos os objetos expoem a colecao Children.
// * Circle e Square sao Leafs no uso demonstrado: especializam o nome da forma,
//   mas nao precisam redefinir a logica de impressao.
// * ToString inicia uma operacao na raiz, e Print propaga essa operacao
//   recursivamente para todos os filhos.
// ============================================================================
namespace DotNetDesignPatternDemos.Structural.Composite.GeometricShapes
{
  // ========================================================================
  // ==== COMPONENT / COMPOSITE ====
  // GraphicObject e a abstracao comum usada tanto por folhas quanto por grupos.
  // A presenca de Children em todos os objetos caracteriza a variante
  // "transparente" do Composite: o cliente usa sempre a mesma API.
  // ========================================================================
  public class GraphicObject
  {
    // Nome padrao de um grupo. As folhas concretas sobrescrevem este valor.
    public virtual string Name { get; set; } = "Group";

    // Dado visual simples aplicado igualmente a folhas e grupos.
    public string Color;

    // A lista de filhos so nasce quando Children e acessado pela primeira vez.
    private Lazy<List<GraphicObject>> children = new Lazy<List<GraphicObject>>();

    // Como a colecao guarda GraphicObject, qualquer filho pode ser folha ou grupo.
    public List<GraphicObject> Children => children.Value;

    // Percorre a arvore a partir do objeto atual.
    // depth indica o nivel hierarquico e vira uma indentacao visual com '*'.
    private void Print(StringBuilder sb, int depth)
    {
      sb.Append(new string('*', depth))
        .Append(string.IsNullOrWhiteSpace(Color) ? string.Empty : $"{Color} ")
        .AppendLine($"{Name}");

      // Delegacao explicita: cada filho executa a mesma operacao de impressao.
      foreach (var child in Children)
        child.Print(sb, depth + 1); // chamada recursiva
    }

    // O cliente pede a representacao textual apenas da raiz.
    // A recursao interna cuida de incluir todos os descendentes.
    public override string ToString()
    {
      var sb = new StringBuilder();
      Print(sb, 0);
      return sb.ToString();
    }
  }
  
  // ========================================================================
  // ==== LEAFS ====
  // Folhas sao objetos finais da arvore no exemplo. Elas reutilizam o
  // comportamento herdado e mudam apenas o nome exibido.
  // ========================================================================
  public class Circle : GraphicObject
  {
    public override string Name => "Circle";
  }

  public class Square : GraphicObject
  {
    public override string Name => "Square";
  }

  // ========================================================================
  // ==== CLIENT ====
  // Monta uma arvore com uma raiz, duas folhas diretas e um grupo aninhado.
  // O cliente imprime a raiz sem precisar perguntar se cada elemento e folha
  // ou composicao.
  // ========================================================================
  public class Demo
  {
    static void Main(string[] args)
    {
      // Raiz da arvore: representa o desenho completo.
      var drawing = new GraphicObject {Name = "My Drawing"};

      // Folhas adicionadas diretamente na raiz.
      drawing.Children.Add(new Square {Color = "Red"});
      drawing.Children.Add(new Circle{Color="Yellow"});
      
      // Um grupo tambem e um GraphicObject, portanto pode entrar na mesma lista.
      var group = new GraphicObject();

      // O grupo contem suas proprias folhas, formando um segundo nivel.
      group.Children.Add(new Circle{Color="Blue"});
      group.Children.Add(new Square{Color="Blue"});
      drawing.Children.Add(group);

      // Uma unica chamada em drawing imprime toda a estrutura.
      WriteLine(drawing);
    }
  }
}
