using System;

namespace DotNetDesignPatternDemos.Structural.Decorator
{
  // ============================================================================
  // Aula03 - Multiple Inheritance with Interfaces
  // ----------------------------------------------------------------------------
  // Ideia da aula:
  // C# nao permite heranca multipla de CLASSES:
  //
  //   public class Dragon : Bird, Lizard
  //
  // Mas C# permite que uma classe implemente varias INTERFACES:
  //
  //   public class Dragon : IBird, ILizard
  //
  // Essa diferenca e a aula inteira:
  // - heranca de classe traz implementacao e estado;
  // - interface traz contrato;
  // - composicao/delegacao reaproveita implementacoes concretas sem herdar delas.
  //
  // Dragon, portanto, promete dois contratos publicos:
  // - sabe voar e possui Weight, porque implementa IBird;
  // - sabe rastejar e possui Weight, porque implementa ILizard.
  //
  // Por dentro, ele nao copia a logica de Bird nem de Lizard.
  // Ele recebe colaboradores e delega cada capacidade para o colaborador certo.
  // Para Weight, ele escolhe uma politica explicita: manter o peso de Bird e
  // Lizard sincronizado.

  // ===== Interface: contrato de voo =====
  // IBird define as partes que interessam ao exemplo:
  // - Fly(), o comportamento;
  // - Weight, um estado/propriedade que tambem faz parte do contrato.
  //
  // Importante:
  // uma interface nao fornece, neste exemplo, a implementacao do voo nem o
  // armazenamento do peso.
  // Ela apenas diz:
  // "qualquer tipo que se declare IBird precisa saber voar e expor Weight".
  public interface IBird
  {
    // ===== Metodo =====
    // Contrato minimo de quem sabe voar.
    void Fly();

    // ===== Propriedade =====
    // Tambem faz parte do contrato.
    // Se uma classe diz que e IBird, ela precisa expor um Weight.
    int Weight { get; set; }
  }

  // ===== Classe concreta que cumpre IBird =====
  // Bird e uma implementacao real do contrato IBird.
  //
  // Agora o papel de Bird ficou mais claro:
  // ela nao e uma classe base que Dragon tenta herdar.
  // Ela e uma peca concreta que Dragon pode usar internamente para realizar Fly().
  public class Bird : IBird
  {
    // ===== Propriedade =====
    // Estado simples usado pelo contrato IBird.
    public int Weight { get; set; }

    // ===== Metodo =====
    // Implementacao concreta do voo.
    public void Fly()
    {
      Console.WriteLine("Soaring in the sky");
    }
  }

  // ===== Interface: contrato de rastejar =====
  // ILizard define a segunda capacidade que Dragon tambem quer expor.
  // Assim como IBird, ele tambem exige Weight.
  //
  // O nome ILizard vem do exemplo original, mas mentalmente voce pode ler como:
  // "algo que sabe rastejar e tambem expoe Weight".
  public interface ILizard
  {
    // ===== Metodo =====
    // Contrato minimo de quem sabe rastejar.
    void Crawl();

    // ===== Propriedade =====
    // O mesmo nome aparece em ILizard.
    // Isso obriga Dragon, que implementa IBird e ILizard, a decidir como esse
    // Weight sera representado.
    int Weight { get; set; }
  }

  // ===== Classe concreta que cumpre ILizard =====
  // Lizard e uma implementacao real do contrato ILizard.
  //
  // Assim como Bird, ela nao precisa ser classe base de Dragon.
  // Ela pode ser apenas um colaborador interno.
  public class Lizard : ILizard
  {
    // ===== Propriedade =====
    // Estado simples usado pelo contrato ILizard.
    public int Weight { get; set; }

    // ===== Metodo =====
    // Implementacao concreta do rastejo.
    public void Crawl()
    {
      Console.WriteLine("Crawling through the land");
    }
  }

  // ===== Classe que combina os dois contratos =====
  // Dragon implementa IBird e ILizard.
  //
  // Isso NAO significa que Dragon herdou codigo de Bird e Lizard.
  // Ele apenas prometeu ao compilador e ao cliente:
  // - eu tenho Fly();
  // - eu tenho Crawl().
  // - eu tenho Weight.
  //
  // Fly() e Crawl() serao delegados para colaboradores.
  // Weight sera tratado por Dragon e repassado aos dois colaboradores.
  //
  // Esse arranjo substitui a heranca multipla que C# nao permite:
  //
  //   errado em C#:
  //     Dragon : Bird, Lizard
  //
  //   correto em C#:
  //     Dragon : IBird, ILizard
  //     Dragon possui objetos que implementam esses contratos
  public class Dragon : IBird, ILizard // no multiple inheritance of classes
  {
    // ===== Campos =====
    // Dragon depende de CONTRATOS, nao das classes concretas diretamente.
    //
    // Essa e a melhoria importante em relacao a usar:
    //
    //   private Bird bird;
    //   private Lizard lizard;
    //
    // Com IBird/ILizard, qualquer implementacao desses contratos poderia entrar
    // aqui. O exemplo usa Bird e Lizard concretos por conveniencia, mas Dragon
    // nao precisa saber disso.
    private readonly IBird bird;
    private readonly ILizard lizard;

    // ===== Construtor =====
    // Construtor de conveniencia para a demo.
    //
    // Ele cria as implementacoes padrao:
    // - Bird para Fly()
    // - Lizard para Crawl()
    //
    // Assim, o Main pode escrever apenas:
    //
    //   var d = new Dragon();
    //
    // Em codigo de producao ou em testes, o construtor abaixo, que recebe
    // interfaces, seria mais flexivel.
    public Dragon()
      : this(new Bird(), new Lizard())
    {
    }

    // ===== Construtor principal =====
    // Aqui esta o design mais importante:
    // Dragon recebe capacidades abstratas.
    //
    // Ele nao exige especificamente uma instancia de Bird.
    // Ele exige algo que cumpra IBird.
    //
    // Ele nao exige especificamente uma instancia de Lizard.
    // Ele exige algo que cumpra ILizard.
    public Dragon(IBird bird, ILizard lizard)
    {
      // Guard clauses:
      // impedem que Dragon nasca sem uma das capacidades necessarias.
      // Se bird fosse null, Fly() e Weight quebrariam depois.
      // Se lizard fosse null, Crawl() e Weight quebrariam depois.
      //
      // Com a validacao no construtor, o erro aparece cedo e com nome claro.
      this.bird = bird ?? throw new ArgumentNullException(paramName: nameof(bird));
      this.lizard = lizard ?? throw new ArgumentNullException(paramName: nameof(lizard));
    }

    // ===== Metodo exposto por Dragon =====
    // Como Dragon implementa ILizard, ele precisa oferecer Crawl().
    //
    // Mas ele nao precisa conhecer a implementacao concreta do rastejo.
    // Ele apenas delega para o colaborador ILizard recebido no construtor.
    //
    // Fluxo:
    // cliente -> Dragon.Crawl() -> lizard.Crawl()
    public void Crawl()
    {
      lizard.Crawl();
    }

    // ===== Metodo exposto por Dragon =====
    // Como Dragon implementa IBird, ele precisa oferecer Fly().
    //
    // Por dentro, a chamada e repassada para o colaborador IBird.
    //
    // Fluxo:
    // cliente -> Dragon.Fly() -> bird.Fly()
    public void Fly()
    {
      bird.Fly();
    }

    // ===== Propriedade compartilhada pelos contratos =====
    // IBird e ILizard exigem uma propriedade chamada Weight.
    //
    // Como Dragon implementa as duas interfaces, uma unica propriedade publica
    // chamada Weight satisfaz os dois contratos ao mesmo tempo.
    //
    // A escolha aqui e sincronizar os colaboradores internos:
    // - ao ler Weight, usamos o valor guardado no colaborador IBird;
    // - ao definir Weight, repassamos o valor para IBird e ILizard.
    //
    // Assim, d.Weight = 123 compila e mantem Bird/Lizard com o mesmo peso.
    public int Weight
    {
      get
      {
        return bird.Weight;
      }
      set
      {
        bird.Weight = value;
        lizard.Weight = value;
      }
    }
  }

  // Regra mental da aula:
  //
  // Sem interfaces:
  //   Dragon tentaria herdar de Bird e Lizard.
  //   C# nao permite heranca multipla de classes.
  //
  // Com interfaces:
  //   Dragon pode implementar IBird e ILizard.
  //   Isso combina contratos, nao implementacoes.
  //
  // Com composicao:
  //   Dragon recebe colaboradores que cumprem esses contratos.
  //   Dragon delega Fly() e Crawl() para eles.
  //   Dragon tambem coordena o Weight comum aos dois contratos.
  //
  // Resultado:
  //   Dragon parece ter as duas capacidades e um Weight unico, mas sem herdar
  //   codigo de duas classes concretas.

  // ===== Demo =====
  static class Program
  {
    static void Main(string[] args)
    {
      // Usa o construtor de conveniencia, que cria Bird e Lizard por dentro.
      var d = new Dragon();

      // O cliente ve Dragon pelos comportamentos prometidos.
      d.Fly();
      d.Crawl();

      // Weight tambem faz parte dos contratos IBird e ILizard.
      // Sem a propriedade Weight em Dragon, esta linha nao compila.
      d.Weight = 123;
    }
  }
}
