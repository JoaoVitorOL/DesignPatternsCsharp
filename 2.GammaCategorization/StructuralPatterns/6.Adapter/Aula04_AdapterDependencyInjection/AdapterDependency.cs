using System;
using System.Collections.Generic;
using Autofac;
using Autofac.Features.Metadata;

// ============================================================================
// Aula - Adapter via Injeção de Dependência (Autofac Adapters)
// ----------------------------------------------------------------------------
// O QUE É: Nas Aulas anteriores (VectorRasterDemo, AdapterCaching), o Adapter
//          era criado manualmente pelo código-cliente: era o próprio
//          desenvolvedor quem escrevia `new LineToPointAdapter(line)`.
//          Aqui a história muda: quem cria o Adapter é o CONTAINER DE
//          INJEÇÃO DE DEPENDÊNCIA (Autofac). O padrão estrutural continua
//          sendo o mesmo Adapter, mas a responsabilidade de "encaixar" o
//          objeto adaptado passa a ser delegada ao framework de DI.
//
// O PROBLEMA QUE ESTAMOS RESOLVENDO:
// * Temos um `Editor` que depende de uma coleção de `Button` (`IEnumerable<Button>`).
// * Só que o que realmente registramos no container são objetos `ICommand`
//   (`SaveCommand`, `OpenCommand`) - nenhum `Button` é registrado diretamente.
// * `Button` não é `ICommand`; `Button` ENVOLVE um `ICommand` (composição).
//   Ou seja, existe uma incompatibilidade de interface clássica entre o que
//   está registrado (`ICommand`) e o que o `Editor` espera receber (`Button`).
//
// A SOLUÇÃO (RegisterAdapter):
// * O método `RegisterAdapter<TFrom, TTo>(Func<TFrom, TTo> adapter)` instrui
//   o Autofac: "toda vez que alguém pedir um `TTo` e você só tiver um `TFrom`
//   registrado, use esta função lambda para ADAPTAR automaticamente um no
//   outro." Isso é o próprio padrão Adapter, só que aplicado a nível de
//   configuração do container, e não a nível de código manual.
// * Autofac ainda entra com um recurso extra chamado METADADOS (`Meta<T>`):
//   é possível anexar dados adicionais (`WithMetadata`) a um registro, e
//   depois recuperar `ICommand` + seus metadados juntos através do tipo
//   genérico `Meta<ICommand>`. Isso permite ao Adapter usar informações
//   externas (como o nome do botão) que não fazem parte da interface
//   `ICommand` em si.
// ============================================================================
namespace AutofacDemos
{
  // ===== Interface =====
  // Contrato mínimo que qualquer "comando" precisa cumprir: saber se
  // executar sozinho. `SaveCommand` e `OpenCommand` são adaptados abaixo
  // para se tornarem `Button`, mesmo sem terem nenhuma relação de herança
  // com essa classe.
  public interface ICommand
  {
    void Execute();
  }

  // ===== Classe =====
  // Implementação concreta de `ICommand` que representa a ação de salvar.
  public class SaveCommand : ICommand
  {
    // ===== Métodos =====
    public void Execute()
    {
      Console.WriteLine("Saving current file");
    }
  }

  // ===== Classe =====
  // Implementação concreta de `ICommand` que representa a ação de abrir
  // um arquivo.
  public class OpenCommand : ICommand
  {
    // ===== Métodos =====
    public void Execute()
    {
      Console.WriteLine("Opening a file");
    }
  }

  // ===== Classe =====
  // Este é o "TIPO ALVO" do Adapter (`TTo`). O container nunca vai
  // encontrar um `Button` pronto no seu registro; ele vai CONSTRUIR um
  // `Button` a partir de um `ICommand` (ou de um `Meta<ICommand>`) usando
  // as funções lambda passadas para `RegisterAdapter` mais abaixo.
  public class Button
  {
    // ===== Campos =====
    // Composição: o `Button` guarda uma referência a um `ICommand`, mas
    // NÃO implementa a interface `ICommand`. É exatamente essa diferença
    // de "forma"/interface entre `ICommand` e `Button` que caracteriza o
    // problema clássico que o Adapter resolve.
    private ICommand command;
    private string name;

    // ===== Construtores =====
    public Button(ICommand command, string name)
    {
      // Guard clause: impede a criação de um `Button` "quebrado", sem
      // nenhum comando associado a ele. Falha rápido (fail-fast) em vez
      // de permitir um `NullReferenceException` mais tarde, dentro de
      // `Click()`.
      if (command == null)
      {
        throw new ArgumentNullException(paramName: nameof(command));
      }
      this.command = command;
      this.name = name;
    }

    // ===== Métodos =====
    // Delega a execução para o `ICommand` interno. Do ponto de vista de
    // quem clica no botão, não importa qual comando concreto está por
    // trás - só importa que ele sabe se `Execute()`.
    public void Click()
    {
      command.Execute();
    }

    public void PrintMe()
    {
      Console.WriteLine($"I am a button called {name}");
    }
  }

  // ===== Classe =====
  // Representa a tela/janela que agrupa vários botões. Repare que o
  // `Editor` depende de `IEnumerable<Button>`, e NUNCA de `ICommand`
  // diretamente - ele não sabe (nem precisa saber) que, por trás dos
  // panos, cada `Button` foi adaptado a partir de um comando.
  public class Editor
  {
    // ===== Campos =====
    private readonly IEnumerable<Button> buttons;

    // ===== Propriedades =====
    public IEnumerable<Button> Buttons => buttons;

    // ===== Construtores =====
    // Este construtor é o ponto em que a "incompatibilidade de interface"
    // citada no cabeçalho do arquivo se torna concreta: o Autofac
    // precisa entregar aqui uma coleção de `Button`, mas só tem
    // `ICommand`s registrados. É aí que os `RegisterAdapter` do método
    // `Main_` entram em ação.
    public Editor(IEnumerable<Button> buttons)
    {
      this.buttons = buttons;
    }

    // ===== Métodos =====
    public void ClickAll()
    {
      foreach (var btn in buttons)
      {
        btn.Click();
      }
    }
  }

  // ===== Classe =====
  static class Adapters
  {
    // ===== Métodos =====
    static void Main_(string[] args)
    {
      // for each ICommand, a ToolbarButton is created to wrap it, and all
      // are passed to the editor
      var b = new ContainerBuilder();

      // Registra `OpenCommand` como implementação de `ICommand` e anexa
      // um METADADO extra ao registro: um par chave/valor ("Name" ->
      // "Open") que não faz parte da interface `ICommand`, mas fica
      // disponível para quem resolver o tipo através de `Meta<ICommand>`.
      b.RegisterType<OpenCommand>()
        .As<ICommand>()
        .WithMetadata("Name", "Open");

      // Mesma lógica para `SaveCommand`, com metadado "Name" -> "Save".
      b.RegisterType<SaveCommand>()
        .As<ICommand>()
        .WithMetadata("Name", "Save");

      //b.RegisterType<Button>();
      // ^ Comentado de propósito: registrar `Button` diretamente NÃO
      // funcionaria aqui, pois `Button` exige um `ICommand` no construtor,
      // e o container não saberia qual `ICommand` (Save ou Open) usar
      // para construir cada instância. É exatamente esse vácuo que os
      // `RegisterAdapter` abaixo preenchem.

      // ===== Adapter nº 1: ICommand -> Button (sem metadado) =====
      // Instrui o Autofac: "para cada `ICommand` registrado, se alguém
      // pedir um `Button`, construa um usando esta função". O nome do
      // botão fica vazio ("") porque, a partir de um `ICommand` puro, não
      // existe nenhuma informação de nome disponível - a interface
      // `ICommand` só expõe `Execute()`.
      b.RegisterAdapter<ICommand, Button>(cmd => new Button(cmd, ""));

      // ===== Adapter nº 2: Meta<ICommand> -> Button (com metadado) =====
      // `Meta<ICommand>` é um wrapper do Autofac que entrega, ao mesmo
      // tempo, o `.Value` (a instância do `ICommand`) e o `.Metadata`
      // (o dicionário de metadados anexado via `WithMetadata`). Aqui
      // usamos exatamente o metadado "Name" registrado acima para dar um
      // nome de verdade ao `Button` adaptado.
      b.RegisterAdapter<Meta<ICommand>, Button>(cmd =>
        new Button(cmd.Value, (string)cmd.Metadata["Name"]));

      // Registra o `Editor`. Quando o container precisar preencher o
      // parâmetro `IEnumerable<Button>` do construtor do `Editor`, ele
      // vai acionar os Adapters registrados acima para cada `ICommand`
      // disponível.
      b.RegisterType<Editor>();

      using (var c = b.Build())
      {
        var editor = c.Resolve<Editor>();
        editor.ClickAll();

        // problem: only one button
        // Eu, Claude, não estou 100% certo desta informação: o
        // comportamento exato do Autofac ao encontrar DOIS
        // `RegisterAdapter` competindo pelo mesmo tipo de destino
        // (`Button`) - um partindo de `ICommand` puro, outro partindo de
        // `Meta<ICommand>` - não está documentado de forma explícita no
        // material de origem. O comentário original do autor indica que,
        // na prática, o `Editor.Buttons` resolve apenas UM `Button`
        // (em vez de dois, um por comando), sugerindo que o Autofac não
        // combina os dois Adapters em uma lista, mas escolhe um caminho
        // de resolução por adaptação por vez. Trata-se de uma limitação
        // observada do mecanismo `RegisterAdapter` quando há mais de uma
        // rota de adaptação possível para o mesmo tipo alvo, não de um
        // erro de sintaxe no código.
        foreach (var btn in editor.Buttons)
          btn.PrintMe();
      }
    }
  }
}