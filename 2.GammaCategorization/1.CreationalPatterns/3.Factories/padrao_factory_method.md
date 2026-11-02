<a id="inicio"></a>
# Design Pattern: Factory Method

## Sumário
[↑ Voltar ao início](#inicio)

- [1. O que o Factory Method representa?](#sec-001)
  - [O Problema que ele resolve:](#sec-002)
- [2. Em que situações se usa?](#sec-003)
- [3. Instruções passo a passo de como implementar (em Java)](#sec-004)
  - [Exemplo básico de implementação:](#sec-005)
- [4. Explicação detalhada do Exemplo Prático (Portal Acadêmico e Notificações)](#sec-006)
  - [A Interface Produto: Notificador](#sec-007)
  - [Os Produtos Concretos: EmailNotificador, SmsNotificador, PushNotificador](#sec-008)
  - [A Classe Criadora (Creator): NotificacaoService](#sec-009)
  - [Os Criadores Concretos (Concrete Creators): EmailService, SmsService, PushService](#sec-010)
  - [A Execução: Main](#sec-011)
- [5. Leitura dos exemplos C# do projeto](#sec-012)
- [6. O que observar ao ler o exemplo local](#sec-013)
- [Decisão prática: fábrica, Factory Method ou container de DI?](#sec-014)
- [Variações e aprofundamentos](#sec-015)
- [20. Overview](#sec-016)
  - [20.1 O que a ideia de Factory representa](#sec-017)
  - [20.2 O problema que ela resolve](#sec-018)
  - [20.3 Quando um desenvolvedor tende a preferir Factory em vez de Builder](#sec-019)
  - [20.3.1 Comparação direta entre Builder e Factory](#sec-020)
  - [20.3.2 Exemplos mentais bem concretos](#sec-021)
  - [20.3.3 Critério de decisão para não misturar os dois](#sec-022)
  - [20.4 O que muda ao longo da trilha](#sec-023)
- [21. Point Example](#sec-024)
  - [21.1 Qual é o problema real deste exemplo](#sec-025)
  - [21.2 Onde a dor aparece para quem usa a classe](#sec-026)
  - [21.3 Por que esse construtor fica sobrecarregado](#sec-027)
  - [21.4 O que a factory melhora aqui](#sec-028)
  - [21.5 Leitura prática do arquivo local](#sec-029)
- [22. Factory Method](#sec-030)
  - [22.1 O que ele enfatiza](#sec-031)
  - [22.2 O ganho mais imediato](#sec-032)
  - [22.3 O que muda concretamente no exemplo do Point](#sec-033)
  - [22.4 Leitura prática do arquivo local](#sec-034)
  - [22.5 O que ele não faz sozinho](#sec-035)
- [23. Asynchronous Factory Method](#sec-036)
  - [23.1 Qual problema ele resolve](#sec-037)
  - [23.2 O que muda em relação ao Factory Method da aula anterior](#sec-038)
  - [23.3 A ideia central](#sec-039)
  - [23.4 Leitura prática do arquivo local](#sec-040)
  - [23.5 A leitura correta](#sec-041)
- [24. Factory](#sec-042)
  - [24.1 O que muda aqui em relação ao Factory Method](#sec-043)
  - [24.2 O que o arquivo local ensina na prática](#sec-044)
  - [24.3 Quando isso faz sentido](#sec-045)
  - [24.4 Trade-off](#sec-046)
- [25. Object Tracking and Bulk Replacement](#sec-047)
  - [25.1 Por que esta aula existe](#sec-048)
  - [25.2 A intuição principal](#sec-049)
  - [25.3 O que aprender com essa aula](#sec-050)
  - [25.4 Leitura prática do arquivo local](#sec-051)
  - [25.5 Quem é quem no exemplo](#sec-052)
  - [25.6 Primeiro bloco: Object Tracking](#sec-053)
  - [25.7 Segundo bloco: Bulk Replacement](#sec-054)
  - [25.8 Comparação direta entre os dois cenários](#sec-055)
  - [25.9 Benefícios e trade-offs](#sec-056)
  - [25.10 O ganho real da aula](#sec-057)
- [26. Inner Factory](#sec-058)
  - [26.1 O que é](#sec-059)
  - [26.2 O que ela tenta equilibrar](#sec-060)
  - [26.2.1 Comparação direta com a factory externa](#sec-061)
  - [26.3 Quando faz sentido](#sec-062)
  - [26.4 A nuance mais importante](#sec-063)


**Família GoF:** criacional. **Escopo predominante:** classe; subclasses redefinem o produto criado.

<a id="sec-001"></a>
## 1. O que o Factory Method representa?

[↑ Voltar ao início](#inicio)
O padrão **Factory Method** é um padrão de projeto criacional que fornece uma interface para criar objetos em uma superclasse, mas permite que as subclasses alterem o tipo de objetos que serão criados.

<a id="sec-002"></a>
### O Problema que ele resolve:

[↑ Voltar ao início](#inicio)
Em sistemas onde precisamos gerenciar a criação de objetos que podem variar (como diferentes tipos de transporte, notificadores ou parsers), instanciar diretamente usando `new` gera forte acoplamento e código rígido:
- **Uso excessivo de condicionais:** Espalhar blocos `if/else` ou `switch` por todo o código para decidir qual classe instanciar.
- **Alto acoplamento:** O código cliente fica fortemente acoplado às implementações concretas e muda constantemente.
- **Dificuldade de extensão:** Adicionar um novo tipo de objeto exige modificar código já testado e em funcionamento, violando o Princípio Aberto/Fechado (OCP).

---

<a id="sec-003"></a>
## 2. Em que situações se usa?

[↑ Voltar ao início](#inicio)
Use o Factory Method quando:
1. **Desconhecimento de tipos concretos:** Uma classe não pode antecipar quais classes concretas de objetos precisará criar.
2. **Extensibilidade sem modificação:** Você quer dar aos usuários de uma biblioteca ou framework uma maneira de estender seus componentes internos.
3. **Centralização e Variação:** A criação de objetos varia de acordo com o contexto (ambiente dev/prod, tipo de entrada, regras de negócio, região/tenant).
4. **Isolamento de lógica:** O fluxo principal da aplicação deve permanecer genérico, delegando a responsabilidade de instanciação para subclasses especializadas.

---

<a id="sec-004"></a>
## 3. Instruções passo a passo de como implementar (em Java)

[↑ Voltar ao início](#inicio)

1. **Definir a Interface do Produto:** Criar uma interface comum (ex: `Notificador`) que declare as operações que todos os produtos concretos devem executar.
2. **Criar Produtos Concretos:** Implementar a interface em classes específicas (ex: `EmailNotificador`, `SmsNotificador`).
3. **Criar a Classe Criadora (Creator):** Definir uma classe abstrata (ex: `NotificacaoService`) que contém a lógica de negócio principal e declara o método fábrica abstrato (`criarNotificador()`).
4. **Criar Criadores Concretos:** Estender a classe criadora em subclasses (ex: `EmailService`, `SmsService`) que sobrescrevem o método fábrica para retornar o produto correspondente.

<a id="sec-005"></a>
### Exemplo básico de implementação:

[↑ Voltar ao início](#inicio)
```java
public interface Notificador {
    void enviar(String destinatario, String mensagem);
}

public abstract class NotificacaoService {
    protected abstract Notificador criarNotificador();

    public void notificar(String destinatario, String mensagem) {
        Notificador notificador = criarNotificador();
        notificador.enviar(destinatario, mensagem);
    }
}
```

---

<a id="sec-006"></a>
## 4. Explicação detalhada do Exemplo Prático (Portal Acadêmico e Notificações)

[↑ Voltar ao início](#inicio)

O código fornecido demonstra o padrão **Factory Method** aplicado em um portal acadêmico, onde diferentes canais de notificação (E-mail, SMS, Push) são instanciados por subclasses criadoras especializadas, mantendo o fluxo principal desacoplado.

<a id="sec-007"></a>
### A Interface Produto: `Notificador`

[↑ Voltar ao início](#inicio)
Define o contrato padrão que todas as implementações de canais de comunicação devem seguir.

```java
package notificacoes;

public interface Notificador {
    void enviar(String destinatario, String mensagem);
}
```

---

<a id="sec-008"></a>
### Os Produtos Concretos: `EmailNotificador`, `SmsNotificador`, `PushNotificador`

[↑ Voltar ao início](#inicio)
Implementam a interface `Notificador` com mensagens direcionadas para cada canal específico.

```java
package notificacoes;

public class EmailNotificador implements Notificador {
    @Override
    public void enviar(String destinatario, String mensagem) {
        System.out.println("[EMAIL] Enviando para " + destinatario + " -> " + mensagem);
    }
}
```

```java
package notificacoes;

public class SmsNotificador implements Notificador {
    @Override
    public void enviar(String destinatario, String mensagem) {
        System.out.println("[SMS] Enviando para " + destinatario + " -> " + mensagem);
    }
}
```

```java
package notificacoes;

public class PushNotificador implements Notificador {
    @Override
    public void enviar(String destinatario, String mensagem) {
        System.out.println("[PUSH] Enviando para o dispositivo " + destinatario + " -> " + mensagem);
    }
}
```

---

<a id="sec-009"></a>
### A Classe Criadora (Creator): `NotificacaoService`

[↑ Voltar ao início](#inicio)
Classe abstrata que centraliza o fluxo principal de notificação (montagem da mensagem padrão, chamada do método fábrica, envio e registro de log), delegando a criação do objeto concreto para as subclasses.

```java
package notificacoes;

public abstract class NotificacaoService {

    // Método fábrica abstrato que as subclasses concretas devem implementar
    protected abstract Notificador criarNotificador();

    // Fluxo comum compartilhado por todos os tipos de notificação
    public void notificar(String destinatario, String mensagem) {
        System.out.println("\n----------------------------------------");

        // 1. Montar a mensagem padrão do portal
        String mensagemPronta = montarMensagem(mensagem);

        // 2. Chamar o método fábrica para instanciar o canal correto
        Notificador notificador = criarNotificador();

        // 3. Enviar a mensagem pelo canal escolhido
        notificador.enviar(destinatario, mensagemPronta);

        // 4. Registrar no log do sistema
        registrarLog(destinatario, notificador.getClass().getSimpleName());
    }

    private String montarMensagem(String mensagemOriginal) {
        return "[Portal Acadêmico] " + mensagemOriginal;
    }

    private void registrarLog(String destinatario, String canal) {
        System.out.println("[LOG] Sucesso! Canal utilizado: " + canal + " | Destinatário: " + destinatario);
    }
}
```

---

<a id="sec-010"></a>
### Os Criadores Concretos (Concrete Creators): `EmailService`, `SmsService`, `PushService`

[↑ Voltar ao início](#inicio)
Estendem `NotificacaoService` e implementam o método fábrica retornando o produto concreto correspondente.

```java
package notificacoes;

public class EmailService extends NotificacaoService {
    @Override
    protected Notificador criarNotificador() {
        return new EmailNotificador();
    }
}
```

```java
package notificacoes;

public class SmsService extends NotificacaoService {
    @Override
    protected Notificador criarNotificador() {
        return new SmsNotificador();
    }
}
```

```java
package notificacoes;

public class PushService extends NotificacaoService {
    @Override
    protected Notificador criarNotificador() {
        return new PushNotificador();
    }
}
```

---

<a id="sec-011"></a>
### A Execução: `Main`

[↑ Voltar ao início](#inicio)
A classe principal demonstra o uso do serviço de notificações através dos diferentes criadores concretos criados para cada canal.

```java
package notificacoes;

public class Main {
    public static void main(String[] args) {
        String destinatario = "aluno.ciencia@universidade.edu";
        String aviso = "Suas notas do semestre já estão disponíveis.";

        System.out.println("=== DEMONSTRAÇÃO DO FACTORY METHOD - PORTAL ACADÊMICO ===");

        // 1. Notificação via Email
        NotificacaoService emailService = new EmailService();
        emailService.notificar(destinatario, aviso);

        // 2. Notificação via SMS
        NotificacaoService smsService = new SmsService();
        smsService.notificar("(11) 98888-7777", aviso);

        // 3. Notificação via Push Notification
        NotificacaoService pushService = new PushService();
        pushService.notificar("Device-Token-XYZ987", aviso);
    }
}

/*
 * ========================================================================================
 * EXPLICAÇÃO: COMO ADICIONAR UM NOVO CANAL (EX: WHATSAPP) SEM ALTERAR O FLUXO EXISTENTE
 * ========================================================================================
 * De acordo com o Princípio Aberto/Fechado (OCP), para incluir um novo canal (WhatsApp)
 * nós não modificamos nenhuma classe existente. Precisamos apenas de dois novos passos:
 *
 * 1. Criar o Produto Concreto implementando a interface 'Notificador':
 *    public class WhatsappNotificador implements Notificador {
 *        public void enviar(String destinatario, String mensagem) {
 *            System.out.println("[WHATSAPP] Enviando para " + destinatario + " -> " + mensagem);
 *        }
 *    }
 *
 * 2. Criar o Criador Concreto estendendo 'NotificacaoService':
 *    public class WhatsappService extends NotificacaoService {
 *        protected Notificador criarNotificador() {
 *            return new WhatsappNotificador();
 *        }
 *    }
 *
 * Com isso, o método 'notificar()' da classe abstrata continua intacto, reutilizando
 * perfeitamente a lógica de montagem de mensagem e registro de log para o novo canal.
 */
```


<a id="sec-012"></a>
## 5. Leitura dos exemplos C# do projeto

[↑ Voltar ao início](#inicio)

Os exemplos de factory na pasta [3.Factories](.) abrangem mais de uma técnica; nem todo método chamado factory implementa o Factory Method GoF:

- [FactoryMethod.cs](Aula02_FactoryMethod/FactoryMethod.cs) é a demonstração de criação por uma operação de fábrica polimórfica.
- [Aula01_PointExample.cs](Aula01_PointExample/Aula01_PointExample.cs), [Factory.cs](Aula04_Factory/Factory.cs) e [InnerFactory.cs](Aula06_InnerFactory/InnerFactory.cs) mostram métodos e objetos-fábrica nomeados/encapsulados. São alternativas úteis de API, embora não sejam automaticamente o GoF canônico.
- [AsyncFactoryMethod.cs](Aula03_AsyncFactoryMethod/AsyncFactoryMethod.cs) mostra criação assíncrona de objetos; async descreve o fluxo da plataforma, não um padrão GoF separado.
- [OBTeBR.cs](Aula05_ObjectTrackingandBulkReplacement/OBTeBR.cs) combina criação centralizada com rastreamento e substituição em massa de objetos.
- [factoryExcercise.cs](Aula09_FactoryExcercise/factoryExcercise.cs) contém o exercício de criação.

Leia o método de criação perguntando: quem escolhe a classe concreta? No Factory Method clássico, um Creator declara a operação e criadores concretos a redefinem para produzir produtos compatíveis. Um método estático que apenas dá um nome semântico ao construtor centraliza criação, mas não apresenta essa extensão polimórfica.

Abstract Factory, que cria famílias de produtos compatíveis, está documentado em [padrao_abstract_factory.md](padrao_abstract_factory.md).


<a id="sec-013"></a>
## 6. O que observar ao ler o exemplo local

[↑ Voltar ao início](#inicio)

A implementação de [FactoryMethod.cs](Aula02_FactoryMethod/FactoryMethod.cs) usa Point como produto e oferece dois caminhos nomeados: NewCartesianPoint(x, y) e NewPolarPoint(rho, theta). O construtor é privado, então o chamador precisa declarar sua intenção pela escolha do método. Para coordenadas polares, a conversão para x/y acontece antes da instância ser entregue.

Esse exemplo ensina uma ideia útil de fábrica: centralizar a criação e deixar claro o significado dos argumentos. Ele não é a estrutura canônica do Factory Method GoF, porque não existem subclasses Creator sobrescrevendo uma operação de criação. A versão canônica, descrita no exemplo de notificações desta página, coloca a criação polimórfica no Creator e deixa criadores concretos escolherem o produto.

Essa distinção importa para não classificar pelo nome do método. Um método estático pode ser uma fábrica nomeada excelente sem usar polimorfismo. Para quem está aprendendo, pergunte: o que o cliente decide? No exemplo Point ele decide a representação de entrada; no Factory Method canônico uma subclasse do Creator escolhe o tipo concreto que completa o fluxo.


<a id="sec-014"></a>
## Decisão prática: fábrica, Factory Method ou container de DI?

[↑ Voltar ao início](#inicio)

Há três decisões parecidas que não devem ser confundidas. Um método-fábrica nomeado melhora a intenção do construtor, como NewPolarPoint. Uma fábrica simples centraliza uma escolha por enum/configuração. Factory Method GoF usa polimorfismo: um Creator define a operação e subclasses redefinem qual produto ela cria durante um fluxo maior.

Em aplicações .NET atuais, um container de DI frequentemente compõe dependências e pode tornar desnecessário criar subclasses apenas para selecionar uma implementação. Ainda assim, Factory Method é útil quando a criação faz parte de um algoritmo que subclasses realmente variam, ou quando construir o produto requer uma sequência especializada. Não introduza uma hierarquia de Creator se uma função ou registro explícito resolver o problema com menos indireção.

Ao analisar um exemplo, siga o tipo declarado e o tipo retornado: o cliente deveria depender de uma abstração estável? Onde a escolha concreta acontece? Quem precisa mudar se um novo produto é adicionado? Essas perguntas são mais confiáveis do que o nome Factory no arquivo.

<a id="sec-015"></a>
## Variações e aprofundamentos

[↑ Voltar ao início](#inicio)

As seções abaixo reúnem variações, exemplos e decisões que complementam a explicação principal deste padrão. Elas preservam a linguagem e o contexto do material didático original.

<a id="sec-016"></a>
## 20. Overview

[↑ Voltar ao início](#inicio)


<a id="sec-017"></a>
### 20.1 O que a ideia de Factory representa

[↑ Voltar ao início](#inicio)

No sentido mais amplo, `Factory` é um jeito de tirar do cliente a responsabilidade de decidir detalhes de instanciação.

Leitura cirúrgica do padrão:

- na `Factory`, o cliente não monta o objeto por dentro;
- ele escolhe **como o objeto deve nascer**;
- o foco não é a montagem passo a passo, e sim **o caminho de criação**.

Em vez de espalhar `new` pelo sistema inteiro, a criação passa a acontecer por caminhos mais controlados.

Isso pode significar:

- um método estático com nome melhor do que um construtor;
- uma classe separada especializada em criar objetos;
- uma fábrica interna ao próprio tipo;
- uma abstração que produz famílias inteiras de objetos relacionados.

<a id="sec-018"></a>
### 20.2 O problema que ela resolve

[↑ Voltar ao início](#inicio)

Nem todo `new` é um problema. O problema começa quando o nascimento do objeto carrega informações demais, escolhas demais ou efeitos colaterais demais.

Regra mental curta:

- se a conversa do cliente parece **"crie deste jeito"**, o problema tem cara de `Factory`.

Sinais comuns:

- construtores com intenção ambígua;
- vários modos válidos de criar o mesmo tipo;
- regras de criação espalhadas;
- necessidade de trocar implementações concretas sem reescrever o cliente;
- necessidade de registrar, contar, rastrear ou substituir objetos criados.

Outro sinal clássico aparece quando o **construtor deixa de ser descritivo o suficiente**.

Isso acontece por vários motivos:

- o nome do construtor é forçado pelo próprio nome do tipo;
- ele não consegue expressar bem intenções diferentes com nomes diferentes;
- você não pode ter sobrecargas com a mesma assinatura mudando apenas a semântica do nome;
- a solução pode degenerar em um tipo de "inferno de parâmetros opcionais".

<a id="sec-019"></a>
### 20.3 Quando um desenvolvedor tende a preferir Factory em vez de Builder

[↑ Voltar ao início](#inicio)

Essa comparação é importante porque `Factory` e `Builder` pertencem à mesma família criacional, mas não resolvem a mesma dor principal.

`Builder` começa a perder naturalidade quando:

- a criação não precisa ser **piecewise**;
- o objeto deveria nascer pronto em uma única operação clara;
- o cliente precisa escolher entre **vários caminhos de criação** com nomes semânticos diferentes;
- a intenção da chamada importa mais do que a montagem em etapas;
- a lógica de criação ficou convoluta demais para continuar dentro de construtores ou de um builder artificial.

Em muitos desses casos, insistir em `Builder` só adiciona cerimônia:

- cria uma conversa passo a passo onde não existe conversa real no domínio;
- força o cliente a "montar" algo que, conceitualmente, deveria apenas **nascer por um caminho específico**;
- não resolve tão bem problemas como nomeação de fábricas, rastreamento, cache, substituição de implementação ou criação assíncrona.

<a id="sec-020"></a>
### 20.3.1 Comparação direta entre Builder e Factory

[↑ Voltar ao início](#inicio)

Para um iniciante, a diferença fica muito mais clara quando a comparação é feita lado a lado:

| Pergunta | Builder | Factory |
| --- | --- | --- |
| O cliente faz o quê? | descreve partes e etapas | pede o objeto por um caminho de criação |
| O objeto nasce quando? | normalmente no `Build()` final | no retorno da factory |
| Dor principal | montagem gradual | escolha, nomeação e controle da criação |
| Chamada típica | `WithX(...).WithY(...).Build()` | `CreateX(...)`, `NewY(...)`, `FromZ(...)` |
| Melhor cenário | muitas partes, opções e ordem | múltiplos caminhos de criação e construtor ambíguo |

Leitura curta:

- `Builder` organiza a **construção do conteúdo** do objeto;
- `Factory` organiza o **nascimento** do objeto.

Se você quiser guardar em uma única linha:

- **Builder:** o cliente ajuda a montar.
- **Factory:** o cliente pede para nascer do jeito certo.

<a id="sec-021"></a>
### 20.3.2 Exemplos mentais bem concretos

[↑ Voltar ao início](#inicio)

Use `Builder` quando a conversa do cliente faz sentido assim:

```csharp
pedidoBuilder
    .Para("Rua A")
    .ComItem("Teclado", 1)
    .ComItem("Mouse", 2)
    .EntregaExpressa()
    .Build();
```

Aqui existe montagem real.

Use `Factory` quando a conversa faz sentido assim:

```csharp
var pedido = PedidoFactory.CreateExpressOrder(cliente, itens);
```

Aqui a intenção não é montar passo a passo. A intenção é pedir um objeto que já deve nascer pronto por uma política conhecida.

Outro contraste útil:

- `Builder`: "quero decidir as partes";
- `Factory`: "quero decidir o tipo de nascimento".

<a id="sec-022"></a>
### 20.3.3 Critério de decisão para não misturar os dois

[↑ Voltar ao início](#inicio)

Se a sua dúvida for prática, use estas perguntas:

1. O cliente precisa participar da construção em várias etapas?
   Se sim, o problema puxa para `Builder`.
2. O objeto deveria sair pronto após uma única chamada com nome semântico claro?
   Se sim, o problema puxa para `Factory`.
3. A dificuldade está em montar o conteúdo interno?
   Isso aponta para `Builder`.
4. A dificuldade está em escolher entre vários modos de criação?
   Isso aponta para `Factory`.

Uma forma curta de guardar a motivação é esta:

- `Builder`: quando o problema é **montagem gradual**;
- `Factory`: quando o problema é **escolha, nomeação e controle da criação**.

<a id="sec-023"></a>
### 20.4 O que muda ao longo da trilha

[↑ Voltar ao início](#inicio)

Neste capítulo, o nome `Factories` cobre várias técnicas aparentadas.

O que muda de uma aula para outra é o grau de controle sobre a criação:

- primeiro, nomear melhor caminhos de criação;
- depois, separar a criação em métodos ou classes especializadas;
- depois, lidar com criação assíncrona, rastreamento e composição interna;
- por fim, produzir famílias inteiras de objetos com `Abstract Factory`.

---

<a id="sec-024"></a>
## 21. Point Example

[↑ Voltar ao início](#inicio)


Esta parte corresponde ao arquivo `CreationalPatterns/3.Factories/Aula01_PointExample/Aula01_PointExample.cs`.

<a id="sec-025"></a>
### 21.1 Qual é o problema real deste exemplo

[↑ Voltar ao início](#inicio)

O `Point Example` existe para mostrar uma dor bem específica de design:

- o mesmo objeto `Point` pode nascer por mais de um modelo mental válido;
- em coordenadas cartesianas, os números significam `x` e `y`;
- em coordenadas polares, os números significam `rho` e `theta`;
- quando tudo isso é empurrado para um único construtor, a assinatura perde significado estável.

No arquivo da aula, isso aparece assim:

```csharp
public Point(double a, double b,
    CoordinateSystem system = CoordinateSystem.Cartesian)
```

O problema não é só ter "dois jeitos de criar". O problema é que:

- `a` pode significar `x` ou `rho`;
- `b` pode significar `y` ou `theta`;
- o enum `system` vira uma chave para decidir como interpretar os mesmos parâmetros;
- o construtor passa a concentrar criação e conversão ao mesmo tempo.

<a id="sec-026"></a>
### 21.2 Onde a dor aparece para quem usa a classe

[↑ Voltar ao início](#inicio)

A chamada abaixo compila e parece simples:

```csharp
new Point(3, 4)
```

Mas ela não explica a intenção. Para entendê-la, o leitor precisa conhecer uma regra escondida: como o parâmetro `system` tem default cartesiano, `new Point(3, 4)` será interpretado como `x = 3` e `y = 4`.

Isso cria uma dor importante:

- se a intenção era criar um ponto polar;
- e o desenvolvedor esquecer de passar `CoordinateSystem.Polar`;
- o código continua compilando;
- mas produz um ponto válido com significado errado.

Ou seja, não é só uma questão de leitura. Existe risco de erro silencioso.

<a id="sec-027"></a>
### 21.3 Por que esse construtor fica sobrecarregado

[↑ Voltar ao início](#inicio)

Esse construtor faz mais de uma coisa ao mesmo tempo:

- recebe os dados do objeto;
- decide qual convenção de entrada o cliente está usando;
- converte coordenadas polares para a representação interna em `x` e `y`.

Para um exemplo pequeno isso pode parecer aceitável, mas em código real esse tipo de assinatura costuma piorar:

- legibilidade da chamada;
- descoberta correta da API;
- manutenção futura da lógica de criação.

<a id="sec-028"></a>
### 21.4 O que a factory melhora aqui

[↑ Voltar ao início](#inicio)

A factory resolve isso dando nome explícito ao caminho de criação:

```csharp
Point.NewCartesian(3, 4)
Point.NewPolar(3, 4)
```

Agora a chamada comunica:

- qual conjunto de coordenadas está entrando;
- como aqueles números devem ser interpretados;
- qual variação de criação o cliente quer usar.

Em vez de o cliente decorar "quais doubles significam o quê", a API passa a explicar isso pelo nome do método.

<a id="sec-029"></a>
### 21.5 Leitura prática do arquivo local

[↑ Voltar ao início](#inicio)

No `Aula01_PointExample.cs`, o construtor atual é útil justamente porque expõe a dor inicial com clareza.

Ele mostra por que factories não são apenas "um jeito diferente de usar `new`". Elas entram quando:

- existem múltiplos modos legítimos de criar o mesmo tipo;
- a assinatura do construtor começa a esconder intenção;
- o cliente passa a depender de flags, enums ou convenções para criar corretamente.

---

<a id="sec-030"></a>
## 22. Factory Method

[↑ Voltar ao início](#inicio)


Esta parte corresponde ao arquivo `CreationalPatterns/3.Factories/Aula02_FactoryMethod/FactoryMethod.cs`.

<a id="sec-031"></a>
### 22.1 O que ele enfatiza

[↑ Voltar ao início](#inicio)

O arquivo local usa o termo *factory method* no sentido amplo de **método-fábrica nomeado**: um método estático encapsula a criação e possui um nome mais semântico que o construtor. Essa é uma técnica válida de API, mas **não é, sozinha, o padrão `Factory Method` canônico do GoF**.

No GoF, um `Creator` declara uma operação de fábrica — muitas vezes abstrata ou redefinível — e subclasses decidem qual `ConcreteProduct` será instanciado. O ponto de variação está no polimorfismo entre criadores, não no simples fato de o método retornar um objeto.

Portanto:

- `Point.NewPolarPoint(...)` é um **static/named factory method**;
- um método de criação sobrescrito por subclasses de `Creator` representa a estrutura canônica do **Factory Method**;
- os dois recursos podem coexistir, mas não devem ser tratados como sinônimos.

<a id="sec-032"></a>
### 22.2 O ganho mais imediato

[↑ Voltar ao início](#inicio)

O ganho mais fácil de sentir é este:

- o construtor costuma dizer apenas "crie";
- o factory method pode dizer "crie deste jeito".

Na sequência da aula anterior, isso resolve exatamente a dor do `Point Example`.

Antes, o cliente precisava lidar com algo parecido com:

```csharp
new Point(a, b, CoordinateSystem.Polar)
```

Agora, a intenção aparece no próprio nome da chamada:

Exemplo conceitual:

```csharp
Point.NewCartesianPoint(x, y)
Point.NewPolarPoint(rho, theta)
```

O código cliente deixa de depender da memória do desenvolvedor sobre o significado posicional dos parâmetros.

<a id="sec-033"></a>
### 22.3 O que muda concretamente no exemplo do `Point`

[↑ Voltar ao início](#inicio)

No `FactoryMethod.cs`, a ideia principal é esta:

- o construtor passa a ser `private`;
- a criação pública acontece só por métodos estáticos nomeados;
- cada método representa um caminho semântico de criação;
- a conversão de coordenadas polares fica encapsulada dentro da fábrica, e não no cliente.

Isso melhora a API por dois motivos ao mesmo tempo:

- quem lê a chamada entende a intenção sem olhar documentação externa;
- a classe impede que o cliente volte para uma criação genérica e ambígua.

<a id="sec-034"></a>
### 22.4 Leitura prática do arquivo local

[↑ Voltar ao início](#inicio)

No arquivo `FactoryMethod.cs`, o fluxo está organizado assim:

- `NewCartesianPoint(x, y)` recebe valores já em coordenadas cartesianas;
- `NewPolarPoint(rho, theta)` recebe a entrada polar e converte para `x` e `y` antes de instanciar;
- `Point(double x, double y)` fica privado para garantir que o objeto só nasça pelos caminhos nomeados;
- o `Demo` mostra o cliente usando a intenção do domínio, e não a mecânica de conversão.

Repare que isso já é uma evolução importante em relação à aula 21:

- na aula 21, o construtor aceitava múltiplos significados;
- na aula 22, a API separa esses significados em métodos distintos.

<a id="sec-035"></a>
### 22.5 O que ele não faz sozinho

[↑ Voltar ao início](#inicio)

O método-fábrica nomeado melhora intenção e encapsulamento da criação, mas não resolve tudo automaticamente.

Ele não garante por si só:

- famílias inteiras de objetos relacionados;
- política global de rastreamento;
- isolamento total entre cliente e implementação concreta.

Para essas dores, a trilha avança para outros idiomas de fábrica e para o padrão canônico `Abstract Factory`.

---

<a id="sec-036"></a>
## 23. Asynchronous Factory Method

[↑ Voltar ao início](#inicio)


Esta parte corresponde ao arquivo `CreationalPatterns/3.Factories/Aula03_AsyncFactoryMethod/AsyncFactoryMethod.cs`.

Aqui, **Asynchronous Factory Method** nomeia um idioma de C#/.NET, não um padrão adicional do catálogo GoF. O método de criação retorna `Task<T>` para que a inicialização assíncrona termine antes de a instância ser entregue. Ele só corresponde ao `Factory Method` canônico se também houver a estrutura polimórfica de `Creator` e `ConcreteCreator`.

<a id="sec-037"></a>
### 23.1 Qual problema ele resolve

[↑ Voltar ao início](#inicio)

Construtores em C# não podem ser `async`.

Então, quando um objeto precisa nascer depois de:

- I/O;
- consulta remota;
- leitura de arquivo;
- handshake de rede;
- inicialização assíncrona pesada;

o construtor tradicional deixa de ser um bom ponto de entrada.

<a id="sec-038"></a>
### 23.2 O que muda em relação ao Factory Method da aula anterior

[↑ Voltar ao início](#inicio)

Na aula 22, o factory method resolvia principalmente um problema de intenção e clareza.

Ele dizia:

- "crie um ponto cartesiano";
- "crie um ponto polar".

Aqui, além de nomear a criação, a fábrica passa a resolver um problema de tempo:

- o objeto ainda não está pronto no momento do `new`;
- existe trabalho assíncrono obrigatório antes de entregar a instância ao cliente.

<a id="sec-039"></a>
### 23.3 A ideia central

[↑ Voltar ao início](#inicio)

Nesses casos, a criação pode sair do construtor e migrar para um método de fábrica assíncrono.

Exemplo conceitual:

```csharp
var service = await MeuServico.CreateAsync(...);
```

Essa abordagem ajuda a evitar objetos "meio prontos".

<a id="sec-040"></a>
### 23.4 Leitura prática do arquivo local

[↑ Voltar ao início](#inicio)

No `AsyncFactoryMethod.cs`, o fluxo está organizado assim:

- o construtor `Foo()` fica `private`, impedindo que o cliente crie a instância diretamente;
- `InitializeAsync()` representa o trabalho assíncrono necessário para o objeto terminar de nascer;
- `CreateAsync()` centraliza `new`, `await` da inicialização e retorno do objeto pronto;
- o `Demo` só recebe `Foo` depois de `await Foo.CreateAsync()`.

Esse é o ponto mais importante da aula:

- o cliente não administra etapas manuais de inicialização;
- a própria fábrica garante que a instância só sai quando estiver consistente.

<a id="sec-041"></a>
### 23.5 A leitura correta

[↑ Voltar ao início](#inicio)

O objetivo não é apenas "usar async por usar".

O objetivo é proteger uma invariável importante:

**o cliente só recebe o objeto quando ele realmente terminou de nascer.**

---

<a id="sec-042"></a>
## 24. Factory

[↑ Voltar ao início](#inicio)


Esta parte corresponde ao arquivo `CreationalPatterns/3.Factories/Aula04_Factory/Factory.cs`.

A `PointFactory` desta aula é um **objeto/classe-fábrica dedicada**, frequentemente chamado de *Simple Factory*. É um idioma útil, mas não uma terceira entrada do catálogo GoF além de `Factory Method` e `Abstract Factory`.

<a id="sec-043"></a>
### 24.1 O que muda aqui em relação ao Factory Method

[↑ Voltar ao início](#inicio)

Aqui a criação deixa de ficar apenas em métodos internos ao próprio tipo e pode migrar para uma classe separada.

Exemplo conceitual:

```csharp
var p = PointFactory.NewCartesianPoint(3, 4);
```

Na aula 22, o cliente ainda conversava com o próprio produto:

```csharp
Point.NewPolarPoint(rho, theta)
```

Agora, o cliente conversa com um componente dedicado à criação:

```csharp
PointFactory.NewPolarPoint(rho, theta)
```

Essa mudança parece pequena na sintaxe, mas é importante no design:

- o produto deixa de concentrar também a API de nascimento;
- a lógica de criação passa a morar em um tipo próprio;
- a entidade principal pode ficar mais focada no seu estado e comportamento.

<a id="sec-044"></a>
### 24.2 O que o arquivo local ensina na prática

[↑ Voltar ao início](#inicio)

No `Factory.cs`, a progressão fica assim:

- `PointFactory` concentra os métodos `NewCartesianPoint(...)` e `NewPolarPoint(...)`;
- `Point` volta a ser um produto mais simples, recebendo apenas `x` e `y`;
- a conversão de coordenadas polares acontece dentro da factory separada;
- o `Demo` deixa claro que o cliente agora pede o objeto para a fábrica, e não mais para o próprio `Point`.

Esse é o ganho conceitual principal da aula:

- na aula 22, a criação nomeada estava dentro do produto;
- na aula 24, a criação nomeada vira uma responsabilidade separada.

<a id="sec-045"></a>
### 24.3 Quando isso faz sentido

[↑ Voltar ao início](#inicio)

Uma classe de fábrica separada pode ajudar quando:

- você quer tirar lógica de criação de dentro da entidade principal;
- a criação cresceu demais;
- existem dependências auxiliares na criação;
- você quer centralizar políticas sem inflar o produto.

<a id="sec-046"></a>
### 24.4 Trade-off

[↑ Voltar ao início](#inicio)

O custo é introduzir mais um tipo no design.

Então a pergunta prática vira:

**o ganho de clareza e controle compensa a classe extra?**

Se sim, a factory separada faz sentido. Se não, um factory method simples pode bastar.

---

<a id="sec-047"></a>
## 25. Object Tracking and Bulk Replacement

[↑ Voltar ao início](#inicio)


<a id="sec-048"></a>
### 25.1 Por que esta aula existe

[↑ Voltar ao início](#inicio)

Esta parte mostra um ganho menos óbvio de centralizar a criação.

`Object Tracking` e `Bulk Replacement` são capacidades acrescentadas às factories do exemplo, não padrões criacionais autônomos do GoF nem consequências obrigatórias de usar uma factory.

No começo, muita gente enxerga factory apenas como "um lugar mais elegante para esconder `new`". Essa leitura é incompleta. Quando toda criação passa por uma factory, ela vira um ponto de controle do sistema.

Esse ponto central pode:

- contar objetos criados;
- registrar referências;
- aplicar configurações padrão;
- trocar em massa a implementação ativa;
- substituir tipos concretos sem reescrever todos os clientes.

Em outras palavras: a factory não serve apenas para melhorar a chamada do cliente. Ela também pode carregar política, observação e coordenação.

Mas aqui existe uma distinção importante: isso não quer dizer que `Object Tracking` e `Bulk Replacement` sejam consequências automáticas de toda factory.

O que a centralização da criação faz é abrir essa possibilidade.

Ou seja:

- toda factory centraliza algum aspecto da criação;
- algumas factories param por aí;
- outras também assumem responsabilidades extras, como rastrear objetos ou substituir implementações em massa.

<a id="sec-049"></a>
### 25.2 A intuição principal

[↑ Voltar ao início](#inicio)

Se o sistema inteiro faz `new` diretamente, cada chamada vira um ponto isolado e difícil de coordenar.

Se toda criação passa por uma factory, aparece um "gargalo controlado". Tudo nasce no mesmo lugar. Isso dá ao design um ponto único para responder perguntas como:

- o que foi criado?
- quando foi criado?
- quem ainda está usando?
- é possível trocar o objeto entregue sem editar cada cliente?

Esse tipo de pergunta quase nunca fica fácil quando o `new` está espalhado pela aplicação.

<a id="sec-050"></a>
### 25.3 O que aprender com essa aula

[↑ Voltar ao início](#inicio)

Factory não serve apenas para "ficar bonito".

Ela também pode ser uma ferramenta de governança da criação. Nesta aula, o objetivo pedagógico é mostrar dois cenários diferentes:

- no primeiro, a factory quer observar os objetos que já nasceram;
- no segundo, a factory quer trocar em massa o objeto ativo que os clientes enxergam.

Os dois cenários nascem da mesma decisão arquitetural: a criação foi centralizada.

Então, a forma mais precisa de enxergar esta aula é esta:

- `Object Tracking` e `Bulk Replacement` não são "o próximo passo obrigatório" de qualquer factory;
- eles são estilos ou capacidades opcionais que uma factory pode incorporar;
- a escolha depende do problema real do sistema.

<a id="sec-051"></a>
### 25.4 Leitura prática do arquivo local

[↑ Voltar ao início](#inicio)

Esta parte corresponde ao arquivo `CreationalPatterns/3.Factories/Aula05_ObjectTrackingandBulkReplacement/OBTeBR.cs`.

O exemplo usa um domínio propositalmente simples para deixar o foco no mecanismo da factory:

- `ITheme` representa o contrato que o cliente consome;
- `LightTheme` e `DarkTheme` representam os produtos concretos;
- `TrackingThemeFactory` mostra o cenário de **object tracking**;
- `ReplaceableThemeFactory` mostra o cenário de **bulk replacement**;
- `Ref<ITheme>` funciona como um handle mutável entre o cliente e o tema atual.

O ponto importante é que o cliente não escolhe diretamente qual classe concreta instanciar. Ele pede um tema para a factory, e a factory passa a controlar esse nascimento.

Isso é importante porque a aula não está tentando ensinar "tema claro" e "tema escuro". O tema é só um domínio neutro para deixar visível a mecânica do padrão.

<a id="sec-052"></a>
### 25.5 Quem é quem no exemplo

[↑ Voltar ao início](#inicio)

Lendo o arquivo com a terminologia de padrões:

- `ITheme` faz o papel de **Product Abstraction**, o contrato que expõe cores de texto e de fundo;
- `LightTheme` e `DarkTheme` fazem o papel de **Concrete Products**, as implementações concretas que a factory pode devolver;
- `TrackingThemeFactory` e `ReplaceableThemeFactory` fazem o papel de **Factories**, isto é, os pontos centralizados de criação e política;
- `Ref<ITheme>` não é um produto novo do GoF; ele é um recurso auxiliar do exemplo para permitir substituir o tema por dentro sem trocar a referência que o cliente segura;
- `Demo` faz o papel de **Client**, porque consome os produtos por meio da factory.

Essa identificação dos participantes ajuda a evitar uma confusão comum: achar que "factory" é só um método estático com nome bonito. Nesta aula, a factory também carrega política de observação e substituição.

O arquivo também usa a palavra **handle**.

Aqui, `Ref<ITheme>` funciona como um handle no sentido de software design:

- ele não é o tema final em si;
- ele é o objeto intermediário que o cliente segura para chegar ao tema atual;
- `Value` é o lugar onde o tema atual fica guardado;
- como a factory consegue trocar `Value` por dentro, o cliente continua com o mesmo handle externo, mas passa a enxergar outro tema internamente.

Se você quiser uma imagem mental simples, pense assim:

- `Ref<ITheme>` é uma caixa;
- o cliente segura a caixa;
- o tema atual está dentro da caixa, em `Value`;
- a factory pode abrir a caixa e trocar o conteúdo;
- o cliente continua com a mesma caixa na mão.

Em forma de leitura curta:

`cliente -> Ref<ITheme> -> Value -> tema atual`

Logo, neste exemplo:

- o **handle** é `Ref<ITheme>`;
- o **tema** não é o handle;
- o **alvo atual do handle** é o objeto que está em `Value`.

Importante: esse uso de "handle" aqui não é o mesmo que `SafeHandle` do .NET para recursos de sistema operacional. Nesta aula, "handle" quer dizer apenas uma referência indireta, mutável e controlada para outro objeto.

<a id="sec-053"></a>
### 25.6 Primeiro bloco: Object Tracking

[↑ Voltar ao início](#inicio)

No trecho de `TrackingThemeFactory`, a ideia principal é simples:

- a factory cria o tema;
- a factory registra uma referência para esse tema;
- depois ela consegue consultar o que já passou por ela.

O detalhe técnico importante é o uso de `WeakReference<ITheme>`.

Isso significa que a factory consegue rastrear os temas sem impedir que o Garbage Collector os libere quando não houver mais uso real no resto da aplicação.

Também vale nomear corretamente a estrutura do exemplo:

- `List<WeakReference<ITheme>>` não é "uma lista de temas" no sentido direto;
- é uma lista de referências fracas para temas;
- em outras palavras, é um registro das instâncias que a factory quer observar.

O termo **weak reference** significa exatamente isso: uma referência que permite tentar reencontrar o objeto mais tarde, mas que não força o runtime a mantê-lo vivo.

Comparação mental útil:

- uma referência forte diz "este objeto ainda está sendo segurado por alguém";
- uma weak reference diz "eu só quero saber se ele ainda existe, sem ser a razão de ele continuar existindo".

#### 25.6.1 Fluxo mental do tracking

Se você estiver começando, leia o fluxo assim:

1. O cliente chama `CreateTheme(...)`.
2. A factory decide se vai criar `DarkTheme` ou `LightTheme`.
3. A factory devolve o tema ao cliente.
4. Antes de terminar, ela guarda uma `WeakReference` para esse mesmo tema em sua lista.
5. Quando alguém acessa `Info`, a factory percorre a lista e tenta reencontrar cada objeto.

Se a referência ainda encontrar o tema, o relatório mostra suas cores. Se não encontrar, o relatório mostra `collected`, isto é, aquele objeto já foi coletado pelo GC.

#### 25.6.2 O que `Info` realmente faz

A propriedade `Info` é um detalhe bom para iniciantes observarem.

Ela não devolve um texto previamente salvo em um campo. Ela gera um relatório novo toda vez que o `get` é executado. Por isso o código:

- cria um `StringBuilder`;
- percorre a lista de referências;
- usa `TryGetTarget(...)` para perguntar se o objeto ainda existe;
- monta uma string final com o estado atual do tracking.

Esse detalhe é importante porque mostra uma ideia comum em C#: uma propriedade `get` pode calcular o valor no momento da leitura, e não apenas expor um campo simples.

<a id="sec-054"></a>
### 25.7 Segundo bloco: Bulk Replacement

[↑ Voltar ao início](#inicio)

O segundo cenário ensina uma ideia diferente.

Aqui a factory não quer apenas saber o que foi criado. Ela quer conseguir trocar em massa a implementação ativa dos temas já distribuídos.

Se o cliente recebesse `ITheme` diretamente, haveria um problema:

- cada parte do sistema guardaria sua própria referência;
- a factory até poderia criar novos temas dali para frente;
- mas não conseguiria atualizar retroativamente quem já recebeu um tema antigo.

É por isso que o exemplo introduz `Ref<ITheme>`.

Em vez de o cliente guardar o tema diretamente, ele guarda uma caixa mutável que aponta para o tema atual. Assim, quando `ReplaceableThemeFactory.ReplaceTheme(...)` percorre todos os handles vivos e troca `Value`, os clientes passam a enxergar o novo tema sem reescrever o próprio código cliente.

Em linguagem mais curta:

- `ITheme` é o objeto real de domínio;
- `Ref<ITheme>` é o handle;
- `Value` é o ponto em que o handle alcança o objeto atual.

#### 25.7.1 Fluxo mental do bulk replacement

Leia o segundo bloco em sequência:

1. O cliente chama `CreateTheme(...)` na `ReplaceableThemeFactory`.
2. A factory cria o tema real.
3. A factory coloca esse tema dentro de um `Ref<ITheme>`.
4. O cliente recebe o `Ref<ITheme>`, e não o tema cru.
5. A factory guarda uma `WeakReference<Ref<ITheme>>` para esse handle.
6. Mais tarde, `ReplaceTheme(...)` percorre os handles ainda vivos e troca o `Value` de cada um.

O ponto decisivo é este: o cliente continua segurando o mesmo handle, mas o objeto dentro dele muda.

#### 25.7.2 O que realmente muda e o que não muda

Isso costuma gerar confusão no início, então vale ser muito explícito.

Quando a troca em massa acontece:

- o handle `Ref<ITheme>` continua sendo o mesmo objeto externo;
- o que muda é o `Value` dentro dele;
- depois da troca, novas leituras de `headerTheme.Value` ou `footerTheme.Value` passam a enxergar outro tema.

Mas existe uma nuance importante:

- se alguém guardou o handle, essa pessoa passa a ver o tema novo;
- se alguém copiou o tema antigo para outra variável antes da troca, essa cópia antiga não muda magicamente.

Ou seja: o bulk replacement troca o alvo do handle, não reescreve referências antigas já copiadas para fora dele.

#### 25.7.3 Por que também existe `WeakReference` aqui

Aqui aparece uma segunda lista importante:

- `List<WeakReference<Ref<ITheme>>>`;
- ela não guarda os temas diretamente;
- ela guarda referências fracas para os handles mutáveis entregues aos clientes.

Isso permite a factory localizar os handles ainda vivos sem ser a responsável por mantê-los vivos para sempre. A ideia continua sendo observação, não posse.

#### 25.7.4 O que significa "troca em massa" na prática?

O termo pode soar abstrato no início, então vale deixar literal.

Imagine que três partes da aplicação receberam um tema por meio da factory:

- a tela principal;
- o menu lateral;
- uma janela de configuração.

Cada uma delas não recebeu `ITheme` diretamente. Cada uma recebeu um `Ref<ITheme>`.

Se todas essas partes estiverem apontando para `DarkTheme`, uma única chamada:

```csharp
replaceableFactory.ReplaceTheme(dark: false);
```

faz a factory percorrer todos os handles vivos e trocar o `Value` de cada um para um `LightTheme`.

Então, "troca em massa" significa exatamente isto:

- não atualizar cliente por cliente manualmente;
- não pedir para cada tela recriar seu próprio tema;
- fazer uma única operação centralizada trocar o objeto ativo visto por vários clientes ao mesmo tempo.

#### 25.7.5 Bulk Replacement é continuação do Object Tracking?

Pedagogicamente, sim: o segundo bloco reaproveita a ideia de que a factory pode manter um registro interno do que ela entregou.

Conceitualmente, não: eles resolvem problemas diferentes.

- **Object Tracking** responde: "o que já foi criado?"
- **Bulk Replacement** responde: "como atualizo vários clientes sem ir um por um?"

Existe uma nuance importante:

- no primeiro bloco, a factory rastreia **temas** para observação;
- no segundo bloco, a factory rastreia **handles** para substituição.

Então, o bulk replacement usa uma forma de rastreamento internamente, mas não é "a mesma coisa" que o primeiro exemplo.

Em um sistema real, três arranjos são possíveis:

- usar apenas **object tracking**, se o objetivo for observação, diagnóstico ou telemetria;
- usar **bulk replacement**, se o objetivo for trocar rapidamente o objeto ativo visto por vários clientes;
- combinar os dois, se fizer sentido observar e também atualizar em massa.

O ponto principal é: você não é obrigado a ter os dois. A escolha depende do problema do sistema.

<a id="sec-055"></a>
### 25.8 Comparação direta entre os dois cenários

[↑ Voltar ao início](#inicio)

Uma forma rápida de consolidar a aula é comparar os dois blocos:

- no **object tracking**, a factory rastreia os **temas criados**;
- no **bulk replacement**, a factory rastreia os **handles entregues**;
- no **object tracking**, o foco é **inspecionar**;
- no **bulk replacement**, o foco é **atualizar em massa**;
- no primeiro bloco, o cliente recebe `ITheme`;
- no segundo bloco, o cliente recebe `Ref<ITheme>`.

Perceba que a técnica muda porque a necessidade muda. O design não está sendo sofisticado "por estilo"; ele está sendo ajustado ao problema que se quer resolver.

<a id="sec-056"></a>
### 25.9 Benefícios e trade-offs

[↑ Voltar ao início](#inicio)

O exemplo ensina ideias poderosas, mas também mostra custos reais.

Ganhos:

- a criação fica centralizada;
- a observação do ciclo de vida fica mais fácil;
- mudanças de política ficam concentradas na factory;
- o cliente fica menos acoplado a tipos concretos.

Custos:

- o design fica mais sofisticado do que um `new` direto;
- `WeakReference` exige boa clareza conceitual;
- `Ref<ITheme>` adiciona um nível extra de indireção;
- esse tipo de solução só vale a pena quando o problema realmente existe.

Essa última observação é importante para iniciantes: nem toda factory precisa rastrear objetos, e nem todo sistema precisa de bulk replacement. O valor da aula está em mostrar o que se torna possível quando a criação foi bem centralizada.

<a id="sec-057"></a>
### 25.10 O ganho real da aula

[↑ Voltar ao início](#inicio)

Esta aula mostra um ganho mais avançado da centralização da criação:

- a factory pode observar o ciclo de vida dos objetos criados;
- a factory pode concentrar política de substituição;
- a aplicação ganha um ponto único para alterar comportamento sem caçar `new` espalhado pelo código.

Em outras palavras:

- sem factory central, a criação fica difusa;
- com factory central, a criação vira um ponto de coordenação.

---

<a id="sec-058"></a>
## 26. Inner Factory

[↑ Voltar ao início](#inicio)


<a id="sec-059"></a>
### 26.1 O que é

[↑ Voltar ao início](#inicio)

`Inner Factory` é uma fábrica aninhada dentro do próprio tipo, normalmente como classe interna.

Vale uma precisão importante aqui:

- isso **não aparece no catálogo GoF como um padrão separado**;
- aqui estamos falando de uma **forma de organizar uma factory**;
- a intenção continua sendo a mesma da aula anterior: controlar a criação do objeto;
- o que muda é **onde a fábrica mora**.

Exemplo conceitual:

```csharp
var p = Point.InnerFactory.NewCartesianPoint(3, 4);
```

Essa sintaxe quer comunicar uma ideia intermediária:

- a criação não fica exposta como vários métodos estáticos diretamente em `Point`;
- mas também não foi empurrada para uma classe totalmente externa como `PointFactory`.

<a id="sec-060"></a>
### 26.2 O que ela tenta equilibrar

[↑ Voltar ao início](#inicio)

Ela fica no meio do caminho entre:

- deixar factory methods soltos diretamente no tipo;
- ou criar uma fábrica totalmente externa.

O ganho pode ser este:

- a fábrica continua claramente associada ao produto;
- mas a lógica de criação não precisa poluir tanto a superfície principal do tipo;
- o leitor da API enxerga imediatamente que aquela fábrica pertence a `Point`.

<a id="sec-061"></a>
### 26.2.1 Comparação direta com a factory externa

[↑ Voltar ao início](#inicio)

Aqui está a comparação mais objetiva:

| Forma | Exemplo | Ideia principal |
| --- | --- | --- |
| **Factory externa** | `PointFactory.NewPolarPoint(...)` | a factory vira um tipo separado, fora do produto |
| **Inner Factory** | `Point.InnerFactory.NewPolarPoint(...)` | a factory continua separada da API principal, mas fica aninhada no produto |

Em outras palavras:

- as duas resolvem o **mesmo problema**: centralizar a criação e dar nomes claros aos caminhos de nascimento;
- a diferença principal não é de intenção;
- a diferença principal é de **organização e encapsulamento**.

Leitura curta:

- **Factory externa:** "a fábrica mora fora do produto";
- **Inner Factory:** "a fábrica mora dentro do produto".

Na prática, isso costuma significar:

- **Factory externa:** mais separação estrutural;
- **Inner Factory:** mais proximidade semântica com o produto.

<a id="sec-062"></a>
### 26.3 Quando faz sentido

[↑ Voltar ao início](#inicio)

Ela pode ser útil quando:

- a criação é relevante demais para ficar perdida fora do tipo;
- mas a API principal ficaria inchada se tudo virasse método estático no produto;
- você quer deixar visualmente claro que a fábrica pertence àquele produto específico.

Em C#, existe ainda um detalhe técnico bem prático:

- um tipo aninhado pode acessar membros privados do tipo recipiente;
- por isso, `Point.InnerFactory` pode chamar um construtor `private Point(...)`;
- isso permite manter o produto mais fechado para criação direta e, ao mesmo tempo, expor um caminho de criação controlado.

Esse ponto ajuda a diferenciar de forma bem concreta `Inner Factory` da aula `Factory`:

- na **factory externa**, muitas vezes o construtor precisa ficar pelo menos `internal`;
- `internal` permite criar o objeto de qualquer lugar da **mesma assembly**, mas bloqueia criação direta por código de fora dela;
- isso resolve o problema de a factory externa conseguir instanciar o produto, mas deixa a porta um pouco mais aberta dentro do projeto;
- na **inner factory**, o construtor pode continuar `private`;
- `private` permite criação apenas dentro do **próprio tipo**;
- como a factory aninhada vive dentro desse escopo, ela continua podendo instanciar o produto, enquanto o resto do código fica mais bloqueado.

Regra mental rápida:

- prefira **factory externa** quando a criação merece um componente mais independente;
- prefira **inner factory** quando a criação pertence claramente àquele produto e você quer manter essa proximidade na API.

<a id="sec-063"></a>
### 26.4 A nuance mais importante

[↑ Voltar ao início](#inicio)

O principal cuidado didático aqui é não enxergar `Inner Factory` como "um novo pattern com nova intenção".

Não é isso.

O que existe aqui é:

- a **mesma ideia geral de factory** da aula anterior;
- organizada de outra forma;
- com trade-offs diferentes de proximidade, encapsulamento e legibilidade.

Em linguagem bem direta:

- se a dúvida é "qual problema ela resolve?", a resposta continua sendo **controle e clareza da criação**;
- se a dúvida é "o que muda de verdade?", a resposta é **a localização da fábrica e o grau de acoplamento com o produto**.

---


<a id="sec-glossario"></a>
## Glossário
[↑ Voltar ao início](#inicio)

- **Creator:** classe que declara ou usa uma operação de fábrica.
- **Factory Method:** método que delega a criação do produto a uma implementação.
- **Produto concreto:** implementação específica do contrato de produto.
- **Inversão de controle:** o fluxo principal delega a criação a um componente especializado.
