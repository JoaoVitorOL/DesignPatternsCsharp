using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetDesignPatternDemos.Structural.Decorator
{
  // ============================================================================
  // Aula03 - Multiple Inheritance with Interfaces
  // ----------------------------------------------------------------------------
  // Ideia da aula:
  // C# nao permite heranca multipla de CLASSES. Ou seja, uma classe nao pode
  // nascer assim:
  //
  //   public class Dragon : Bird, Lizard
  //
  // Isso compilaria em linguagens com heranca multipla de classes, mas nao em C#.
  //
  // O motivo conceitual e evitar ambiguidades e acoplamentos pesados:
  // - e se Bird e Lizard tivessem metodos com o mesmo nome?
  // - qual implementacao Dragon herdaria?
  // - como resolver conflitos de estado interno entre as duas bases?
  //
  // Entao esta aula mostra o caminho usado em C#:
  // em vez de HERDAR implementacoes de duas classes, Dragon POSSUI objetos
  // especializados e DELEGA chamadas para eles.
  //
  // Observacao importante:
  // apesar do nome do arquivo falar "with interfaces", este trecho ainda nao
  // declara interfaces como IFlyingCreature ou ICrawlingCreature. Ele mostra a
  // versao por composicao direta com classes concretas. O passo seguinte seria
  // trocar os campos Bird/Lizard por interfaces, deixando Dragon depender de
  // capacidades abstratas em vez de classes concretas.

  // ===== Classe que representa a capacidade de voar =====
  // Bird e uma implementacao concreta. Ela sabe executar Fly().
  // No exemplo didatico, o metodo esta vazio porque o foco nao e a fisica do voo;
  // o foco e mostrar como Dragon reaproveita essa capacidade sem herdar de Bird.
  public class Bird
  {
    // ===== Metodo =====
    // Comportamento especifico de uma entidade que sabe voar.
    public void Fly()
    {
      
    }
  }

  // ===== Classe que representa a capacidade de rastejar =====
  // Lizard e outra implementacao concreta. Ela sabe executar Crawl().
  // O exemplo quer combinar "voar" e "rastejar" dentro de Dragon.
  public class Lizard
  {
    // ===== Metodo =====
    // Comportamento especifico de uma entidade que sabe rastejar.
    public void Crawl()
    {
      
    }
  }

  // ===== Classe que combina capacidades =====
  // A tentacao inicial seria escrever:
  //
  //   public class Dragon : Bird, Lizard
  //
  // Mas C# nao permite isso. Dragon so pode herdar de uma classe base.
  //
  // A solucao aqui e composicao:
  // Dragon tem um Bird e tem um Lizard.
  //
  // Em termos de padroes estruturais, isso conversa com Decorator/Adapter porque
  // Dragon cria uma fachada propria por cima de objetos internos e repassa as
  // chamadas. O cliente chama Dragon.Fly(), mas quem faz o trabalho real e Bird.
  public class Dragon // no multiple inheritance
  {
    // ===== Campos =====
    // Estes campos sao as partes internas que fornecem comportamento ao Dragon.
    //
    // Importante para design:
    // como os tipos sao concretos, Dragon fica acoplado diretamente a Bird e
    // Lizard. Se quisermos deixar isso mais flexivel, podemos trocar por
    // interfaces, por exemplo IFlyer e ICrawler.
    private Bird bird;
    private Lizard lizard;

    // ===== Construtor =====
    // As dependencias entram prontas por fora.
    // Isso deixa claro que Dragon nao cria sozinho seus colaboradores; ele recebe
    // objetos capazes de realizar as partes do comportamento.
    public Dragon(Bird bird, Lizard lizard)
    {
      // Guard clauses:
      // impedem que Dragon nasca sem uma das capacidades necessarias.
      // Se bird fosse null, Fly() quebraria depois com NullReferenceException.
      // Com a validacao no construtor, o erro aparece cedo e com nome claro.
      this.bird = bird ?? throw new ArgumentNullException(paramName: nameof(bird));
      this.lizard = lizard ?? throw new ArgumentNullException(paramName: nameof(lizard));
    }

    // ===== Metodo exposto por Dragon =====
    // Para o cliente, Dragon sabe rastejar.
    //
    // Mas Dragon nao implementa a logica de rastejar diretamente.
    // Ele delega para o Lizard interno.
    //
    // Fluxo:
    // cliente -> Dragon.Crawl() -> lizard.Crawl()
    public void Crawl()
    {
      lizard.Crawl();
    }

    // ===== Metodo exposto por Dragon =====
    // Para o cliente, Dragon sabe voar.
    //
    // Por dentro, a chamada e repassada para Bird.
    //
    // Fluxo:
    // cliente -> Dragon.Fly() -> bird.Fly()
    public void Fly()
    {
      bird.Fly();
    }
  }

  // Regra mental da aula:
  //
  // Sem heranca multipla:
  //   Dragon nao pode ser Bird e Lizard ao mesmo tempo por heranca de classes.
  //
  // Com composicao:
  //   Dragon possui um Bird para voar.
  //   Dragon possui um Lizard para rastejar.
  //   Dragon expoe Fly() e Crawl() e delega cada chamada ao objeto apropriado.
  //
  // Com interfaces, que seria a evolucao natural:
  //   Dragon dependeria de contratos pequenos, como IFlyer e ICrawler.
  //   Assim, qualquer implementacao capaz de voar ou rastejar poderia ser usada,
  //   sem prender Dragon especificamente a Bird e Lizard.
}
