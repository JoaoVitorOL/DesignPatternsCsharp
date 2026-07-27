using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

// ============================================================================
// Aula02 - Neural Networks (Padrao Composite)
// ----------------------------------------------------------------------------
// O QUE E: esta aula mostra uma variacao do Composite em que um objeto individual
//          (Neuron) e um grupo de objetos (NeuronLayer) podem ser tratados de
//          forma uniforme como sequencias de neuronios.
//
// CENARIO:
// * Um neuronio individual precisa poder se conectar a outro neuronio.
// * Um neuronio individual tambem precisa poder se conectar a uma camada inteira.
// * Uma camada inteira precisa poder se conectar a outra camada inteira.
//
// A SOLUCAO (Composite):
// * Neuron implementa IEnumerable<Neuron> e se apresenta como uma colecao de um
//   unico item: ele mesmo.
// * NeuronLayer herda de Collection<Neuron>, portanto ja e naturalmente uma
//   colecao de neuronios.
// * O metodo de extensao ConnectTo trabalha apenas com IEnumerable<Neuron>.
//   Assim, ele nao precisa saber se recebeu uma folha ou um grupo.
// ============================================================================
namespace DotNetDesignPatternDemos.Structural.Composite.NeuralNetworks
{
  // Nao usamos uma classe-base comum para "neuronio ou camada".
  // Em vez disso, usamos o contrato IEnumerable<Neuron> como ponto de unificacao.

  // ========================================================================
  // ==== OPERACAO COMUM DO COMPOSITE ====
  // Esta classe guarda metodos de extensao para qualquer objeto que consiga
  // ser visto como uma sequencia de Neuron.
  // ========================================================================
  public static class ExtensionMethods
  {
    // Conecta todos os neuronios de "self" a todos os neuronios de "other".
    // A assinatura e o ponto central da aula:
    // - um Neuron funciona porque implementa IEnumerable<Neuron>;
    // - um NeuronLayer funciona porque herda de Collection<Neuron>.
    public static void ConnectTo(this IEnumerable<Neuron> self, IEnumerable<Neuron> other)
    {
      // Evita conectar uma sequencia nela mesma.
      // Em uma rede neural, isso impediria auto-conexoes neste exemplo.
      if (ReferenceEquals(self, other)) return;

      // Produto cartesiano entre as duas sequencias:
      // cada neuronio de origem recebe uma conexao para cada neuronio de destino.
      foreach (var from in self)
        foreach (var to in other)
        {
          // Registra a conexao de saida no neuronio de origem.
          from.Out.Add(to);

          // Registra a conexao de entrada no neuronio de destino.
          to.In.Add(from);
        }
    }
  }

  // ========================================================================
  // ==== LEAF ====
  // Neuron e a folha conceitual do Composite, mas ele tambem se comporta como
  // uma colecao de um elemento para caber na mesma API de NeuronLayer.
  // ========================================================================
  public class Neuron : IEnumerable<Neuron>
  {
    // Valor numerico associado ao neuronio.
    public float Value;

    // Conexoes recebidas (In) e emitidas (Out).
    // As listas ja nascem inicializadas para que ConnectTo possa adicionar
    // conexoes sem causar NullReferenceException.
    public List<Neuron> In = new List<Neuron>();
    public List<Neuron> Out = new List<Neuron>();

    // Permite iterar sobre um unico Neuron como se ele fosse uma colecao.
    // O "yield return this" entrega o proprio objeto como unico item da sequencia.
    public IEnumerator<Neuron> GetEnumerator()
    {
      yield return this;
    }

    // Implementacao exigida pela interface IEnumerable nao generica.
    // Mantem compatibilidade com APIs antigas que trabalham com IEnumerable puro.
    IEnumerator IEnumerable.GetEnumerator()
    {
      yield return this;
    }
  }

  // ========================================================================
  // ==== COMPOSITE ====
  // NeuronLayer representa um grupo de neuronios.
  // Como herda de Collection<Neuron>, ela ja fornece Add, Count e enumeracao.
  // ========================================================================
  public class NeuronLayer : Collection<Neuron>
  {

  }

  // ========================================================================
  // ==== CLIENT ====
  // Demonstra que o cliente usa sempre ConnectTo, sem criar metodos separados
  // para neuronio-neuronio, neuronio-camada ou camada-camada.
  // ========================================================================
  public class Demo
  {
    static void Main(string[] args)
    {
      // Folhas individuais.
      var neuron1 = new Neuron();
      var neuron2 = new Neuron();

      // Grupos de neuronios.
      var layer1 = new NeuronLayer();
      var layer2 = new NeuronLayer();

      // Neuron -> Neuron:
      // funciona porque cada Neuron se enumera como uma sequencia de um item.
      neuron1.ConnectTo(neuron2);

      // Neuron -> NeuronLayer:
      // a folha e o grupo compartilham o contrato IEnumerable<Neuron>.
      neuron1.ConnectTo(layer1);

      // NeuronLayer -> NeuronLayer:
      // o mesmo metodo percorre todos os neuronios das duas camadas.
      layer1.ConnectTo(layer2);
    }
  }
}
