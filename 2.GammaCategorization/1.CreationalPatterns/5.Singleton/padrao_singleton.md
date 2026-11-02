<a id="inicio"></a>
# Design Pattern: Singleton

## Sumário
[↑ Voltar ao início](#inicio)

- [1. O que o Singleton representa?](#sec-001)
  - [O Problema que ele resolve:](#sec-002)
- [2. Em que situações se usa?](#sec-003)
- [3. Instruções passo a passo de como implementar (em Java)](#sec-004)
  - [Exemplo básico de implementação:](#sec-005)
- [4. Explicação detalhada do Exemplo Prático (Central de Alertas)](#sec-006)
  - [A Classe Singleton: CentralDeAlertas](#sec-007)
  - [As Classes de Acesso (POLICIA, BOMBEIRO, SAMU)](#sec-008)
  - [A Execução: Main](#sec-009)
- [5. Implementações e variações encontradas no projeto C](#sec-010)
- [6. A finalidade fica clara no exercício de alertas](#sec-011)
- [Singleton no código moderno: identidade, concorrência e ciclo de vida](#sec-012)
- [Variações e aprofundamentos](#sec-013)
- [39. Testability Issues](#sec-014)
  - [39.1 O problema prático: análise de SingletonRecordFinder](#sec-015)
  - [39.2 Anatomia do construtor de SingletonDatabase](#sec-016)
  - [39.3 Por que o teste unitário quebra ou falha em isolar a lógica?](#sec-017)
  - [39.4 Entendendo o problema com analogias do mundo real](#sec-018)
  - [39.5 Resumo dos sintomas de falta de testabilidade](#sec-019)
- [40. Singleton in Dependency Injection](#sec-020)
  - [40.1 O que é Dependency Injection](#sec-021)
  - [40.2 O ponto central: retirar o acesso global da classe](#sec-022)
  - [40.3 E o compartilhamento em produção? Ele desaparece?](#sec-023)
  - [40.4 Como um container de DI funciona, por dentro](#sec-024)
  - [40.5 Por que isso é evolução, não desistência](#sec-025)
  - [40.6 Quem é quem no exemplo](#sec-026)
  - [40.7 Duas formas de injetar, nenhuma toca no Singleton](#sec-027)
  - [40.8 O que costuma confundir aqui](#sec-028)
  - [40.9 Resumo mental](#sec-029)
- [41. Monostate](#sec-030)
  - [41.1 O que é Monostate](#sec-031)
  - [41.2 Singleton vs. Monostate: a diferença estrutural](#sec-032)
  - [41.3 O comportamento observado no Main](#sec-033)
  - [41.4 Vantagens e desvantagens](#sec-034)
  - [41.5 Caso de uso: quando Monostate faz sentido na prática](#sec-035)
  - [41.6 Comparativo de decisão: Singleton clássico vs. Monostate](#sec-036)
  - [41.7 O que costuma confundir aqui](#sec-037)
  - [41.8 Resumo mental](#sec-038)
- [42. Per-Thread Singleton](#sec-039)
  - [42.1 O que é Per-Thread Singleton](#sec-040)
  - [42.2 Singleton comum vs. Per-Thread Singleton: a diferença estrutural](#sec-041)
  - [42.3 O comportamento observado no Main](#sec-042)
  - [42.4 Vantagens e desvantagens](#sec-043)
  - [42.5 Caso de uso: quando Per-Thread Singleton faz sentido na prática](#sec-044)
  - [42.6 Comparativo de decisão: Singleton comum vs. Per-Thread Singleton](#sec-045)
  - [42.7 O que costuma confundir aqui](#sec-046)
  - [42.8 Resumo mental](#sec-047)
- [43. Ambient Context](#sec-048)
  - [43.1 Conexão com os capítulos anteriores](#sec-049)
  - [43.2 Por que usar?](#sec-050)
  - [43.3 Quando usar?](#sec-051)
  - [43.4 Quando evitar?](#sec-052)
  - [43.5 Diferença essencial: Singleton vs. Ambient Context](#sec-053)
  - [43.6 O que é a struct utilizada no código e quando usá-la?](#sec-054)
  - [43.7 Como isso aparece na estrutura do código](#sec-055)
  - [43.8 Como o código cliente consome o padrão](#sec-056)
  - [43.9 Onde isso é usado no mundo real?](#sec-057)
- [Coding Exercise 4: Singleton Coding Exercise](#sec-058)
  - [O objetivo do exercício](#sec-059)
  - [Recapitulação: o passo a passo da construção de um Singleton](#sec-060)
  - [O grande problema do Singleton tradicional: testabilidade](#sec-061)


**Família GoF:** criacional. **Escopo predominante:** classe e ciclo de vida da instância.

<a id="sec-001"></a>
## 1. O que o Singleton representa?

[↑ Voltar ao início](#inicio)
O padrão **Singleton** é um padrão de projeto criacional que tem como objetivo **garantir que uma classe tenha apenas uma única instância** em toda a aplicação, fornecendo ao mesmo tempo um **ponto global de acesso** a essa instância.

<a id="sec-002"></a>
### O Problema que ele resolve:

[↑ Voltar ao início](#inicio)
Em sistemas complexos, existem recursos que devem ser únicos na aplicação inteira, tais como:
- Arquivos de configuração central.
- Gerenciadores de conexão com o banco de dados.
- Serviços de log ou centrais de alerta.

Se cada parte do sistema puder instanciar esses recursos livremente usando `new`, surgem problemas como **inconsistência de dados**, **uso excessivo de recursos** (estouro de conexões) e **dificuldade de coordenação**.

---

<a id="sec-003"></a>
## 2. Em que situações se usa?

[↑ Voltar ao início](#inicio)
Use o Singleton quando:
1. **Exigência de unicidade:** Deve existir apenas uma instância de uma classe específica compartilhada por todo o sistema (ex: Gerenciador de Configurações, Pool de Conexões).
2. **Ponto global de acesso:** Quando você precisa acessar esse recurso compartilhado de diferentes lugares da aplicação (ex: preferências do usuário, cache em memória).
3. **Controle de inicialização tardia (Lazy Initialization):** Para criar o objeto apenas quando ele for realmente necessário, economizando recursos na inicialização do sistema.

---

<a id="sec-004"></a>
## 3. Instruções passo a passo de como implementar (em Java)

[↑ Voltar ao início](#inicio)

1. **Tornar o construtor privado:** Impede que outras classes criem instâncias diretamente usando `new`.
2. **Criar um atributo estático privado:** Armazena a única instância da classe.
3. **Criar um método estático público (`getInstancia`):** Controla a criação e o retorno do objeto único.

<a id="sec-005"></a>
### Exemplo básico de implementação:

[↑ Voltar ao início](#inicio)
```java
public class ConfiguracaoSistema {
    private static ConfiguracaoSistema instancia;

    // 1. Construtor privado
    private ConfiguracaoSistema() {
        // Inicialização de recursos
    }

    // 3. Método estático de acesso
    public static ConfiguracaoSistema getInstancia() {
        if (instancia == null) {
            instancia = new ConfiguracaoSistema();
        }
        return instancia;
    }
}
```

---

<a id="sec-006"></a>
## 4. Explicação detalhada do Exemplo Prático (Central de Alertas)

[↑ Voltar ao início](#inicio)

O código fornecido exemplifica o padrão **Singleton** aplicado em um cenário de **Central de Alertas de Emergência** (inspirado na analogia de serviços como Polícia, Bombeiros e SAMU).

<a id="sec-007"></a>
### A Classe Singleton: `CentralDeAlertas`

[↑ Voltar ao início](#inicio)
Esta classe centraliza o gerenciamento de alertas utilizando uma abordagem **Thread-Safe** (segura para ambientes concorrentes com múltiplas threads) através de *Double-Checked Locking* e a palavra-chave `volatile`:
- **Atributo `instancia` (`volatile`):** Garante a visibilidade correta da instância única entre diferentes threads.
- **Construtor privado:** Impede que qualquer classe crie uma nova central com `new CentralDeAlertas()`. Utiliza um `CopyOnWriteArrayList` para armazenar as mensagens com segurança em ambientes concorrentes.
- **Método `getInstancia()`:** Verifica se a instância já existe. Caso contrário, sincroniza a criação para evitar que duas threads criem instâncias simultâneas.

```java
import java.util.ArrayList;
import java.util.Collections;
import java.util.List;
import java.util.concurrent.CopyOnWriteArrayList;

public class CentralDeAlertas {
    private final List<String> mensagens;
    private static volatile CentralDeAlertas instancia;

    private CentralDeAlertas() {
        mensagens = new CopyOnWriteArrayList<>();
    }

    public static CentralDeAlertas getInstancia() {
        if (instancia == null) {
            synchronized (CentralDeAlertas.class) {
                if (instancia == null) {
                    instancia = new CentralDeAlertas();
                }
            }
        }
        return instancia;
    }

    public void enviarAlerta(String orgao, String mensagem) {
        String alertaFormatado = "[" + orgao + "] " + mensagem;
        mensagens.add(alertaFormatado);
    }

    public List<String> getAlertas() {
        return Collections.unmodifiableList(new ArrayList<>(mensagens));
    }
}
```

---

<a id="sec-008"></a>
### As Classes de Acesso (`POLICIA`, `BOMBEIRO`, `SAMU`)

[↑ Voltar ao início](#inicio)
Estes órgãos funcionam como os "diferentes telefones" da nossa analogia. Cada um possui métodos para enviar alertas e listar o histórico, delegando sempre as operações para a única instância de `CentralDeAlertas` obtida via `CentralDeAlertas.getInstancia()`.

Exemplo (`POLICIA`):
```java
import java.util.List;

public class POLICIA {
    public void enviarAlerta(String mensagem) {
        CentralDeAlertas.getInstancia().enviarAlerta("POLICIA", mensagem);
    }

    public List<String> listarAlertas() {
        return CentralDeAlertas.getInstancia().getAlertas();
    }
}
```
*(O mesmo comportamento ocorre nas classes `BOMBEIRO` e `SAMU`, identificando seus respectivos órgãos).*

---

<a id="sec-009"></a>
### A Execução: `Main`

[↑ Voltar ao início](#inicio)
A classe principal demonstra o funcionamento prático e a comprovação da unicidade:
1. **Comprovação de Instância Única (`hashCode()`):** É impresso o hash code obtido a partir de chamadas feitas pela Polícia, Bombeiros e SAMU. Todos exibem exatamente o **mesmo hash code**, provando que se trata do mesmo objeto em memória.
2. **Envio de Alertas:** Cada órgão envia sua respectiva ocorrência. Como compartilham a mesma lista na `CentralDeAlertas`, todas as mensagens são centralizadas.
3. **Compartilhamento do Histórico:** Ao listar os alertas a partir da Polícia ou do SAMU, o histórico completo e unificado é exibido, provando que o estado é compartilhado globalmente.

```java
public class Main {
    public static void main(String[] args) {
        POLICIA policia = new POLICIA();
        BOMBEIRO bombeiros = new BOMBEIRO();
        SAMU samu = new SAMU();

        System.out.println("=== COMPROVAÇÃO DE INSTÂNCIA ÚNICA (HASHCODE) ===");
        System.out.println("Hash via Polícia:   " + CentralDeAlertas.getInstancia().hashCode());
        System.out.println("Hash via Bombeiros: " + CentralDeAlertas.getInstancia().hashCode());
        System.out.println("Hash via SAMU:      " + CentralDeAlertas.getInstancia().hashCode());
        System.out.println("===============================================
");

        policia.enviarAlerta("Ocorrência de roubo na Zona Sul.");
        bombeiros.enviarAlerta("Princípio de incêndio em edificação comercial.");
        samu.enviarAlerta("Acidente de trânsito grave na Rodovia Principal.");

        System.out.println("--- Histórico visualizado pela POLÍCIA ---");
        policia.listarAlertas().forEach(System.out::println);

        System.out.println("\n--- Histórico visualizado pelo SAMU ---");
        samu.listarAlertas().forEach(System.out::println);
    }
}
```


<a id="sec-010"></a>
## 5. Implementações e variações encontradas no projeto C#

[↑ Voltar ao início](#inicio)

[Aula01_SingletonImplementation/Singleton.cs](Aula01_SingletonImplementation/Singleton.cs) mostra uma instância única acessada por propriedade estática. [Aula02_TestabilityIssues/TestabilityIssues.cs](Aula02_TestabilityIssues/TestabilityIssues.cs) discute como acesso global dificulta substituir dependências em testes. [Aula03_SingletonDepencyInjection/SingletonDepencyInjection.cs](Aula03_SingletonDepencyInjection/SingletonDepencyInjection.cs) compara o conceito com lifetime singleton de um container: o container controla compartilhamento, mas as classes consumidoras ainda podem depender de abstrações.

As aulas seguintes apresentam conceitos próximos, que não devem ser confundidos com a intenção exata do Singleton:

- [Monostate.cs](Aula04_Monostate/Monostate.cs) compartilha campos estáticos entre instâncias, mantendo construtor público.
- [ThreadSingleton.cs](Aula05_PerThreadSingleton/ThreadSingleton.cs) cria uma instância por thread, não uma instância global única.
- [AmbientContext.cs](Aula06_AmbientContext/AmbientContext.cs) demonstra contexto acessível de forma ambiental; isso reduz parâmetros, mas cria dependência implícita.
- [SingletonExcercise.cs](Aula07_SingletonExcercise/SingletonExcercise.cs) propõe identificar se uma factory retorna a mesma referência em chamadas repetidas. O exercício de alertas em [Aula_08_SinlgetonExercise2](Aula07_SingletonExcercise/Aula_08_SinlgetonExercise2/C%23/Main.cs) compartilha CentralDeAlertas entre serviços.

Singleton deve ser uma decisão consciente sobre identidade e ciclo de vida, não apenas um modo conveniente de obter qualquer dependência. Para serviços de aplicação, DI costuma deixar o escopo compartilhado mais explícito e testável.


<a id="sec-011"></a>
## 6. A finalidade fica clara no exercício de alertas

[↑ Voltar ao início](#inicio)

O exercício de [CentralDeAlertas](Aula07_SingletonExcercise/Aula_08_SinlgetonExercise2/C%23/Main.cs) representa um serviço compartilhado por Polícia, Bombeiro e SAMU. O requisito didático é que todos usem o mesmo ponto central de alertas, para não manterem cópias locais divergentes. Leia CentralDeAlertas.cs junto com Main.cs e os serviços para acompanhar onde a instância é obtida e como mensagens de diferentes participantes chegam ao mesmo destino.

A implementação de Singleton tradicional cria uma instância única e controla como ela é acessada; o ganho é identidade compartilhada e uma política de ciclo de vida simples. O custo é que o acesso global esconde dependências: uma classe pode usar o serviço sem recebê-lo explicitamente, o que torna substituição e testes mais difíceis.

Por isso, a aula de DI compara Singleton como padrão da classe com lifetime singleton de um container. Com DI, consumidores ainda declaram sua dependência por construtor e o container decide compartilhar a instância. Monostate também não é o mesmo mecanismo: várias instâncias compartilham campos estáticos. Per-thread singleton mantém uma instância por thread, não uma única instância para todo o processo.

Use Singleton apenas quando a unicidade de identidade fizer parte do requisito. Compartilhar um serviço por conveniência não basta, por si só, para justificar estado global.


<a id="sec-012"></a>
## Singleton no código moderno: identidade, concorrência e ciclo de vida

[↑ Voltar ao início](#inicio)

Singleton combina duas decisões: a classe controla a criação e oferece um acesso global, e o sistema compartilha uma única instância. A segunda pode ser útil para um recurso realmente único; a primeira costuma introduzir acoplamento global. Separe as perguntas: precisa haver um único objeto? Quem deve controlar essa instância?

Em C#, uma inicialização estática é thread-safe para a criação básica. Para construção preguiçosa, Lazy<T> torna a política explícita. Mesmo com inicialização segura, os métodos e o estado interno do objeto não se tornam thread-safe automaticamente. Uma instância compartilhada mutável ainda precisa sincronizar operações concorrentes.

Em aplicações com DI, registre o serviço com lifetime Singleton quando o requisito for compartilhar uma instância no escopo do container, mas injete a interface nos consumidores. Isso preserva a possibilidade de testes e substituição. Singleton global é inadequado para dados por usuário/request e pode fazer testes influenciarem uns aos outros. O exercício da CentralDeAlertas demonstra compartilhamento; numa aplicação real, considere se um serviço singleton injetado atende melhor ao requisito.

<a id="sec-013"></a>
## Variações e aprofundamentos

[↑ Voltar ao início](#inicio)

As seções abaixo reúnem variações, exemplos e decisões que complementam a explicação principal deste padrão. Elas preservam a linguagem e o contexto do material didático original.

<a id="sec-014"></a>
## 39. Testability Issues

[↑ Voltar ao início](#inicio)


O maior perigo do padrão Singleton não está na sua implementação, mas no impacto devastador que ele causa na **testabilidade** da aplicação. Quando uma classe consome um Singleton acessando-o diretamente por meio de seu ponto global estático (`Classe.Instance`), ela introduz um **acoplamento oculto e rígido** com uma dependência real.

Nos testes unitários, o objetivo principal é isolar a unidade sob teste de fatores externos (como arquivos de disco, redes ou bancos de dados). No entanto, o Singleton tradicional impede esse isolamento, forçando os testes de unidade a se tornarem testes de integração lentos, frágeis e dependentes de estado global.

---

<a id="sec-015"></a>
### 39.1 O problema prático: análise de `SingletonRecordFinder`

[↑ Voltar ao início](#inicio)

No arquivo prático da aula (`TestabilityIssues.cs`), temos a classe `SingletonRecordFinder`:

```csharp
public class SingletonRecordFinder
{
    public int GetTotalPopulation(IEnumerable<string> names)
    {
        int result = 0;

        foreach(var name in names)
        {
            result += SingletonDatabase.Instance.GetPopulation(name);
        }

        return result;
    }
}

```

A responsabilidade dessa classe é simples: receber uma lista de nomes de cidades e somar a população total delas. Contudo, veja o que acontece na linha central:

```csharp
result += SingletonDatabase.Instance.GetPopulation(name);

```

Aqui, `SingletonRecordFinder` faz uma chamada direta para a propriedade estática `SingletonDatabase.Instance`. Isso significa que a classe está acoplada à implementação concreta de produção do banco de dados.

---

<a id="sec-016"></a>
### 39.2 Anatomia do construtor de `SingletonDatabase`

[↑ Voltar ao início](#inicio)

Para entender por que esse acoplamento é problemático nos testes, precisamos examinar o que acontece no construtor privado de `SingletonDatabase`:

```csharp
private SingletonDatabase()
{
    instanceCount++;
    Console.WriteLine("Initializing Database . . .");

    string currentCodeFile = GetCurrentFilePath();
    string sourceFolder = Path.GetDirectoryName(currentCodeFile) ?? "";
    string filePath = Path.Combine(sourceFolder, "capitals.txt");

    capitals = File.ReadAllLines(filePath)
        .Chunk(2)
        .ToDictionary(
            list => list.ElementAt(0).Trim(),
            list => int.Parse(list.ElementAt(1))
        );
}

```

#### Explicação didática dos termos e métodos

- **`File.ReadAllLines(filePath)`**: Método nativo do .NET que abre o arquivo físico `capitals.txt` no disco rígido, lê todas as suas linhas e as carrega na memória como um array de strings. É uma operação real de **I/O (Input/Output)**, inerentemente lenta e propensa a erros (o arquivo pode não existir, estar corrompido ou bloqueado).
- **`.Chunk(2)`**: Um método de extensão do LINQ que divide uma coleção em pequenos "pedaços" (blocos) de tamanho fixo. Como o arquivo texto alterna uma linha com o nome da cidade e a linha seguinte com a população, o `Chunk(2)` agrupa essas linhas de duas em duas (ex: `["Seoul", "175000"]`).
- **`.ToDictionary(...)`**: Transforma os pedaços gerados pelo `Chunk` em uma estrutura de chave e valor (`Dictionary<string, int>`), mapeando o nome limpado (`.Trim()`) da cidade como a **Chave** e convertendo o texto numérico para inteiro (`int.Parse`) como o **Valor**.

---

<a id="sec-017"></a>
### 39.3 Por que o teste unitário quebra ou falha em isolar a lógica?

[↑ Voltar ao início](#inicio)

Ao tentar testar o método `GetTotalPopulation`, o desenvolvedor escreve o seguinte teste de unidade (que na verdade falha no quesito isolamento):

```csharp
[Test]
public void SingletonPopulationTest()
{
    var rf = new SingletonRecordFinder();
    var names = new[] {"Seoul", "Mexico"};

    int tp = rf.GetTotalPopulation(names);
    Assert.That(tp, Is.EqualTo(175000 + 174000));
}

```

Este teste possui problemas graves de design arquitetural:

1. **Dependência de Dados Reais e Voláteis**: O teste é obrigado a consultar as populações reais de `Seoul` e `Mexico` contidas no arquivo `capitals.txt` de produção. Se o arquivo de produção for atualizado amanhã com dados novos (ex: a população de Seoul mudar para `180000`), o teste unitário irá **quebrar**, mesmo que a lógica do algoritmo em `SingletonRecordFinder` continue perfeitamente correta.
2. **Ausência de um ponto convencional de substituição:** Como o consumidor busca `SingletonDatabase.Instance` diretamente, ele não permite injetar um *mock* ou *stub* por construtor. Interceptar a chamada estática exigiria ferramentas especiais de *shimming* ou *profiling*; o desenho comum com DI não consegue substituí-la.
3. **Poluição de estado entre testes:** O campo estático mantém uma referência para a instância enquanto o contexto de carregamento correspondente permanecer vivo. Se um teste alterar estado mutável global, testes subsequentes podem observar esse efeito e perder isolamento.

---

<a id="sec-018"></a>
### 39.4 Entendendo o problema com analogias do mundo real

[↑ Voltar ao início](#inicio)

Para que os impactos negativos do Singleton em testes fiquem mais claros, podemos visualizar o cenário por meio de duas analogias. Elas ilustram por que estado global mutável e acoplamento rígido reduzem a previsibilidade do código.

---

#### Analogia 1: o banheiro único do prédio

Imagine que você trabalha em um prédio comercial de 20 andares, mas as regras de construção do edifício (o padrão Singleton) determinam que só pode existir **um único banheiro funcional em todo o prédio**. Todos os funcionários de todos os departamentos precisam usar exatamente a mesma cabine.

- **No mundo do desenvolvimento (Produção):** Enquanto o fluxo de pessoas for baixo e ordenado, o sistema funciona. Cada um entra, usa e sai.
- **No mundo dos testes unitários:** Imagine que cada teste unitário é um funcionário diferente tentando validar sua rotina diária de forma isolada, rápida e em paralelo.
- O **Teste A** entra no banheiro e deixa as luzes apagadas e a torneira aberta.
- O **Teste B** entra logo em seguida esperando encontrar o banheiro limpo e com as luzes acesas. Ele falha instantaneamente porque o teste anterior alterou o estado do ambiente e não limpou a própria bagunça.
- Se tentarmos rodar 5 testes ao mesmo tempo (testes concorrentes), o sistema colapsa em filas e comportamentos imprevisíveis.

**Moral da história:** Quando os seus testes compartilham uma única instância global de um recurso (`SingletonDatabase.Instance`), o que um teste faz afeta diretamente o resultado do próximo. O isolamento deixa de existir.

---

#### Analogia 2: o restaurante com fornecedor monopolista

Imagine que você é dono de uma pizzaria (`SingletonRecordFinder`) e quer testar uma nova receita de molho de tomate. No entanto, por causa de um contrato de exclusividade inflexível escrito na própria estrutura da cozinha, você é obrigado a usar apenas os tomates do "Fazendeiro João" (`SingletonDatabase`). Você não consegue mudar de fornecedor porque a tubulação de molho vem lacrada direto da fazenda dele para a sua panela.

- **O problema no teste:** Para testar puramente se a sua proporção de sal e temperos está correta (teste de unidade), você não precisaria colher tomates de verdade da fazenda do João. Você só precisaria de tomates de plástico ou uma simulação rápida (um *Mock* ou *Stub*) para validar a sua técnica de cozinha.
- **O impacto real:** Como o contrato é rígido, toda vez que você vai testar o molho, você é obrigado a acionar o caminhão do João, esperar a entrega, pagar o preço de mercado e torcer para que a plantação dele não tenha sofrido com a chuva. Se a fazenda do João fechar para balanço (o arquivo `capitals.txt` sumir ou for modificado), a sua pizzaria não consegue sequer testar a colher de sal.

**Moral da história:** O Singleton age como esse contrato de exclusividade destrutivo. Como o método chama diretamente a classe concreta, você perde a liberdade de colocar um "fornecedor de testes" (uma implementação simulada) no lugar, tornando o seu código refém de dependências externas e pesadas.

---

<a id="sec-019"></a>
### 39.5 Resumo dos sintomas de falta de testabilidade

[↑ Voltar ao início](#inicio)

| Conceito | Comportamento no código real | Impacto no Teste Unitário |
| --- | --- | --- |
| **Acesso estático** | `SingletonDatabase.Instance` é chamado diretamente pelo consumidor. | Contorna a injeção de dependência convencional e dificulta substituir o colaborador por um dublê de teste. |
| **I/O Inerente** | O construtor lê de forma síncrona do arquivo físico `capitals.txt`. | Transforma o teste unitário em teste de integração; o teste falha se o arquivo sumir ou mudar no ambiente de CI/CD. |
| **Referência estática duradoura** | O campo estático retém a instância no contexto de carregamento em que o tipo está ativo. | Estado mutável compartilhado pode criar acoplamento temporal entre testes, especialmente quando executados em paralelo. |

Este cenário desenha o paradoxo do Singleton tradicional: embora exista `IDatabase`, a invocação direta de `SingletonDatabase.Instance` impede que o consumidor receba outra implementação pela forma convencional. Uma solução comum é injetar `IDatabase` e deixar o *composition root* — manualmente ou com um container — controlar o ciclo de vida.

---

<a id="sec-020"></a>
## 40. Singleton in Dependency Injection

[↑ Voltar ao início](#inicio)


A seção anterior (**39. Testability Issues**) chegou a um diagnóstico: o problema não é o Singleton existir, é a forma como ele é **consumido**. Quando uma classe chama `SingletonDatabase.Instance` diretamente dentro de um método, ela esconde essa dependência do mundo exterior — e o que está escondido não pode ser substituído por um dublê de teste.

Esta seção lê `CreationalPatterns/5.Singleton/Aula03_SingletonDepencyInjection/SingletonDepencyInjection.cs`, que continua a ideia da Aula02. O arquivo local mantém trechos comentados e trechos de transição da aula; a explicação abaixo foca no desenho pretendido da solução com **Dependency Injection**.

---

<a id="sec-021"></a>
### 40.1 O que é Dependency Injection

[↑ Voltar ao início](#inicio)

**Dependency Injection (DI)** é uma técnica em que um objeto **recebe** as dependências de que precisa, em vez de **criá-las** (ou buscá-las) sozinho por dentro do próprio código.

- **Sem DI:** a classe conhece a implementação concreta de que depende e a acessa ela mesma. É o caso de `SingletonRecordFinder` (seção 39), que chama `SingletonDatabase.Instance` dentro de `GetTotalPopulation`. Essa classe acumula duas responsabilidades: sua lógica de negócio (somar populações) **e** a decisão de qual banco de dados usar.
- **Com DI:** a classe declara *do que* precisa — normalmente via interface — e recebe uma implementação já pronta, montada por quem a criou. Ela deixa de saber (e de se importar) *quem* fornece essa implementação ou como foi construída.

Essa inversão — "eu não busco minha dependência, alguém me entrega ela pronta" — é um caso concreto de um princípio mais amplo, **Inversion of Control (IoC)**. DI é a forma mais comum de aplicar IoC no dia a dia, e é também o mecanismo prático por trás do **Dependency Inversion Principle** (o "D" do SOLID, já visto em `1.SOLID/Aula05_DependencyInversion`): módulos de alto nível não devem depender de módulos de baixo nível, e sim de abstrações. É por causa da DI que uma classe consegue depender de `IDatabase` em vez de uma implementação concreta.

| Forma | Como funciona | Usada neste exemplo? |
| --- | --- | --- |
| **Constructor Injection** | Dependência passada como parâmetro do construtor, guardada em campo | **Sim** — `ConfigurableRecordFinder` |
| **Property/Setter Injection** | Dependência atribuída depois de o objeto já existir, via propriedade pública | Não |
| **Method Injection** | Dependência passada só como parâmetro do método que precisa dela | Não |

Constructor Injection é a forma mais recomendada na maioria dos cenários porque garante que o objeto nunca existe em estado incompleto — sem a dependência obrigatória, ele simplesmente não é criado.

---

<a id="sec-022"></a>
### 40.2 O ponto central: retirar o acesso global da classe

[↑ Voltar ao início](#inicio)

Esta é a distinção que evita o erro de leitura mais comum do capítulo:

> A solução retira da classe o construtor privado e o acesso estático global. O container pode manter um *lifetime* `Singleton`, mas essa é uma política externa de compartilhamento por container, não uma garantia global embutida no tipo.

| | Antes (Aulas 38/39) | Depois (Aula 40) |
| --- | --- | --- |
| Implementação em **produção** | `SingletonDatabase.Instance` | `OrdinaryDatabase` |
| Implementação em **teste** | A mesma `SingletonDatabase.Instance` — impossível trocar | `DummyDatabase` |
| `SingletonDatabase` é usada na versão corrigida? | Sim, em todo lugar | **Não, em lugar nenhum** — fica no arquivo só como referência de comparação |
| Quem decide a implementação | Ninguém — hard-coded dentro de `SingletonRecordFinder` | Quem monta o objeto: o teste ou o container de DI |

O que muda entre produção e teste não é "qual classe é o Singleton" — é simplesmente qual `IDatabase` é injetado em `ConfigurableRecordFinder`.

---

<a id="sec-023"></a>
### 40.3 E o compartilhamento em produção? Ele desaparece?

[↑ Voltar ao início](#inicio)

Não. A garantia de "uma instância só" continua existindo — só que muda de dono:

- **Antes:** a própria classe `SingletonDatabase` se responsabilizava por isso (construtor `private`, campo estático `Lazy<T>`, propriedade `Instance`). A regra de unicidade morava **dentro** da classe.
- **Depois:** o **container de DI** reutiliza a instância no escopo do container por meio de `.SingleInstance()` no registro de `OrdinaryDatabase`.

A mesma necessidade de negócio que motivava o Singleton — evitar reler `capitals.txt` várias vezes, compartilhar o mesmo `Dictionary` em memória — continua sendo atendida. O que muda é a ferramenta: agora "fazer a lógica de negócio" e "controlar o ciclo de vida do objeto" são responsabilidades separadas, em vez de misturadas na mesma classe.

**Analogia:** `SingletonDatabase` é uma pessoa que só sabe abrir a própria porta — a fechadura está embutida nela. `OrdinaryDatabase` + container é uma casa sem fechadura própria, mas com um porteiro que decide "só deixo uma pessoa por vez entrar, sempre a mesma". A regra de unicidade existe do mesmo jeito — só que está no porteiro, não na porta.

---

<a id="sec-024"></a>
### 40.4 Como um container de DI funciona, por dentro

[↑ Voltar ao início](#inicio)

Um **container de DI** (ou *IoC Container*) é uma biblioteca cuja única responsabilidade é montar objetos e suas dependências para você, em vez de você escrever `new` manualmente em todo o sistema. Ele resolve o problema em duas etapas:

1. **Registro:** você diz ao container, uma vez, no início da aplicação — "quando alguém pedir um `IDatabase`, entregue um `OrdinaryDatabase`". É um cadastro; nada é criado ainda.
2. **Resolução:** em qualquer outro ponto do sistema, em vez de `new OrdinaryDatabase()`, você pede ao container "me dá um `IDatabase`". Ele consulta o registro e entrega a instância certa — criando-a na hora ou reaproveitando uma existente, conforme a regra de ciclo de vida configurada.

Sem container, esse trabalho ainda pode ser centralizado manualmente em um *composition root*. Um container automatiza a resolução e os ciclos de vida nesse ponto de configuração; o restante do sistema continua declarando as abstrações de que precisa.

`Autofac` (usado neste exemplo por meio de `ContainerBuilder`) é uma implementação concreta desse conceito. O ecossistema .NET também oferece `Microsoft.Extensions.DependencyInjection`; o conceito de container de DI não pertence a uma biblioteca específica.

```csharp
var cb = new ContainerBuilder();
cb.RegisterType<OrdinaryDatabase>()
   .As<IDatabase>()
   .SingleInstance();
cb.RegisterType<ConfigurableRecordFinder>();

using var c = cb.Build();
var rf = c.Resolve<ConfigurableRecordFinder>();
```

| Trecho | O que faz |
| --- | --- |
| `RegisterType<OrdinaryDatabase>()` | Cadastra o tipo que o container sabe construir |
| `.As<IDatabase>()` | Expõe `OrdinaryDatabase` pelo contrato `IDatabase`; o DIP depende de os módulos consumidores realmente dependerem dessa abstração |
| `.SingleInstance()` | Define o *lifetime*: uma instância do componente por container Autofac, reutilizada pelos escopos descendentes desse container |
| `cb.Build()` | Fecha os registros e gera o container pronto para uso |
| `c.Resolve<ConfigurableRecordFinder>()` | Container percebe que o construtor pede `IDatabase`, consulta o registro, cria (ou reaproveita) `OrdinaryDatabase` e injeta automaticamente — sem você escrever `new ConfigurableRecordFinder(new OrdinaryDatabase())` |

Em sistemas com muitos grafos de dependências e ciclos de vida distintos, essa resolução automática pode justificar o uso do container em vez de uma montagem manual extensa.

---

<a id="sec-025"></a>
### 40.5 Por que isso é evolução, não desistência

[↑ Voltar ao início](#inicio)

Na Aula 38, `SingletonDatabase` acumulava a lógica de dados e o controle do próprio ciclo de vida. Isso aumenta as razões para a classe mudar e pode ser interpretado como uma tensão com o **SRP — Single Responsibility Principle**; não significa que toda implementação de Singleton viole SRP por definição.

Na Aula 40, essas responsabilidades são separadas:

- `OrdinaryDatabase` cuida só da lógica de negócio.
- O container de DI (ou, na versão manual do teste, o próprio código que chama `new`) cuida só do ciclo de vida.

É essa separação que permite trocar `OrdinaryDatabase` por `DummyDatabase` em teste sem tocar em nenhuma regra de instanciação.

---

<a id="sec-026"></a>
### 40.6 Quem é quem no exemplo

[↑ Voltar ao início](#inicio)

| Elemento | Papel | Papel na solução com DI |
| --- | --- | --- |
| `IDatabase` | Abstração comum a todas as implementações | Contrato central |
| `SingletonDatabase` | Padrão Singleton clássico, mantida só para comparação | Exemplo do acoplamento a evitar |
| `SingletonRecordFinder` | Consumidor acoplado ao acesso estático — o "jeito errado" | Exemplo do consumidor difícil de testar |
| `OrdinaryDatabase` | Mesma lógica de `SingletonDatabase`, sem mecanismo de instância única embutido | Implementação real pretendida |
| `ConfigurableRecordFinder` | Recebe `IDatabase` via construtor | Consumidor corrigido |
| `DummyDatabase` | Implementação falsa, sem I/O | Dublê de teste |

#### `OrdinaryDatabase`

```csharp
public class OrdinaryDatabase : IDatabase
{
    private Dictionary<string, int> capitals;

    private OrdinaryDatabase()
    {
        Console.WriteLine("Initializing Database . . .");
        string currentCodeFile = GetCurrentFilePath();
        string sourceFolder = Path.GetDirectoryName(currentCodeFile) ?? "";
        string filePath = Path.Combine(sourceFolder, "capitals.txt");

        capitals = File.ReadAllLines(filePath)
            .Chunk(2)
            .ToDictionary(
                list => list.ElementAt(0).Trim(),
                list => int.Parse(list.ElementAt(1))
            );
    }

    public int GetPopulation(string name) => capitals[name];
}
```

A intenção é que `OrdinaryDatabase` conserve a lógica de dados, mas não o campo estático nem a propriedade `Instance`; o container passa a controlar seu lifetime. O arquivo local, contudo, ainda contém resíduos da cópia de `SingletonDatabase` e **não compila como está**.

**⚠️ Três detalhes que impedem este arquivo de compilar ou de ser resolvido pelo container como está**:

1. `instanceCount++` no construtor — esse campo é `private static int instanceCount`, declarado **dentro de `SingletonDatabase`**, invisível para qualquer outra classe do arquivo. O compilador acusaria `CS0103`. Conceitualmente também não faz sentido: contar instâncias só importa para uma classe que promete ter no máximo uma — `OrdinaryDatabase` não faz essa promessa, então essa linha deve ser **removida**, não corrigida.
2. `GetCurrentFilePath()` — mesmo problema: esse método auxiliar existe só dentro de `SingletonDatabase`, e C# não permite chamar um membro `private` de outra classe não relacionada por herança. `OrdinaryDatabase` precisa da **sua própria cópia** desse método.
3. o construtor de `OrdinaryDatabase` está `private`; nem o cliente nem o Autofac podem chamá-lo nessa configuração. Para o desenho pretendido, ele deve ser `public` ou o registro deve fornecer uma factory autorizada.

#### `ConfigurableRecordFinder`

```csharp
public class ConfigurableRecordFinder
{
    private IDatabase database;

    public ConfigurableRecordFinder(IDatabase database)
    {
        if (database == null)
            throw new ArgumentNullException(paramName: nameof(database));
        this.database = database;
    }

    public int GetTotalPopulation(IEnumerable<string> names)
    {
        int result = 0;
        foreach (var name in names)
            result += database.GetPopulation(name);
        return result;
    }
}
```

| | `SingletonRecordFinder` (seção 39 — descontinuada) | `ConfigurableRecordFinder` (a que roda de verdade) |
| --- | --- | --- |
| Como obtém o banco de dados | Chama `SingletonDatabase.Instance` dentro do método | Recebe `IDatabase` de fora, pelo construtor |
| Tipo da dependência | Classe concreta | Abstração (`IDatabase`) |
| Substituível por dublê de teste? | Não | Sim |
| Dependência visível na assinatura pública? | Não — escondida no corpo do método | Sim — `ConfigurableRecordFinder(IDatabase database)` |

O ponto mais importante: basta olhar a assinatura do construtor para saber exatamente do que a classe depende. A guarda `if (database == null)` não é acidental — como agora *qualquer* `IDatabase` pode ser passado, a classe perde a garantia implícita que o Singleton dava (`Instance` nunca era nula), e a validação recupera parte dessa segurança de forma explícita.

#### `DummyDatabase`

```csharp
public class DummyDatabase : IDatabase
{
    public int GetPopulation(string name) => new Dictionary<string, int>
    {
        ["alpha"] = 1,
        ["beta"] = 2,
        ["gamma"] = 3
    }[name];
}
```

Não lê arquivo nenhum — devolve valores fixos e conhecidos. Retomando a **Analogia 2** da seção 39.5 (o restaurante com fornecedor monopolista): `DummyDatabase` é o "tomate de plástico" que faltava. Antes não havia como colocar um fornecedor alternativo no lugar de `SingletonDatabase`; agora, como `ConfigurableRecordFinder` aceita qualquer `IDatabase`, `DummyDatabase` ocupa esse lugar **apenas em teste** — em produção, quem ocupa é `OrdinaryDatabase`.

---

<a id="sec-027"></a>
### 40.7 Duas formas de injetar, nenhuma toca no Singleton

[↑ Voltar ao início](#inicio)

```csharp
[Test]
public void ConfigurablePopulationTest()   // DI manual
{
    var rf = new ConfigurableRecordFinder(new DummyDatabase());
    var names = new[] { "alpha", "gamma" };
    int tp = rf.GetTotalPopulation(names);
    Assert.That(tp, Is.EqualTo(4));
}

[Test]
public void DIPopulationTest()             // DI via container
{
    var cb = new ContainerBuilder();
    cb.RegisterType<OrdinaryDatabase>().As<IDatabase>().SingleInstance();
    cb.RegisterType<ConfigurableRecordFinder>();
    using var c = cb.Build();
    var rf = c.Resolve<ConfigurableRecordFinder>();
}
```

| | DI manual | DI via container |
| --- | --- | --- |
| Quem cria o `IDatabase` | O próprio teste, com `new DummyDatabase()` | O container |
| Quem cria o `ConfigurableRecordFinder` | O próprio teste | O container, via `c.Resolve<...>()` |
| `.SingleInstance()` | Não se aplica | Reutiliza a mesma instância de `OrdinaryDatabase` dentro daquele container |
| `SingletonDatabase.Instance` aparece? | Não | Não |

`DIPopulationTest` é o que mais se aproxima do cenário real de produção: `OrdinaryDatabase` registrada como única instância no container, `ConfigurableRecordFinder` resolvido automaticamente já com a dependência encaixada — e `SingletonDatabase` não é criada nem referenciada em momento algum.

---

<a id="sec-028"></a>
### 40.8 O que costuma confundir aqui

[↑ Voltar ao início](#inicio)

- **Achar que `OrdinaryDatabase` é "a versão de teste".** Não é — é a implementação de **produção**. Quem é exclusivo de teste é `DummyDatabase`.
- **Achar que o sistema continua usando `SingletonDatabase` por baixo dos panos.** Não continua — `SingletonDatabase.Instance` não é chamada em lugar nenhum na versão corrigida; a classe fica no arquivo só como referência.
- **Achar que DI é "um framework" ou "uma biblioteca".** É uma técnica de design. Autofac automatiza a técnica, mas `ConfigurablePopulationTest` prova que DI funciona sem nenhum container, só com `new` manual.
- **Achar que remover o acesso estático significa abrir mão de compartilhamento em produção.** `.SingleInstance()` mantém uma instância por container; construir outro container pode produzir outra instância.
- **Confundir `ConfigurableRecordFinder` com `SingletonRecordFinder`** pelo corpo quase idêntico de `GetTotalPopulation`. A diferença crítica está em **de onde vem `database`** — campo recebido no construtor versus chamada estática dentro do método.
- **Achar que `DummyDatabase` "testa o banco de dados".** Não testa — existe para **eliminar** o banco real da equação, permitindo testar isoladamente a lógica de soma em `GetTotalPopulation`.

---

<a id="sec-029"></a>
### 40.9 Resumo mental

[↑ Voltar ao início](#inicio)

- **Dependency Injection** é receber uma dependência pronta de fora, em vez de criá-la ou buscá-la por dentro da própria classe — aplicação prática de IoC e do Dependency Inversion Principle;
- a seção 39 mostrou o problema: dependência buscada por dentro do método, escondida e impossível de substituir;
- **a solução elimina o acesso estático embutido no tipo:** `SingletonDatabase` deixa de ser usada em qualquer caminho, produção ou teste;
- `OrdinaryDatabase` é a nova implementação de produção, com a mesma lógica de `SingletonDatabase`, mas sem o mecanismo de instância embutido;
- `ConfigurableRecordFinder` é o consumidor corrigido: dependência explícita na assinatura do construtor — Constructor Injection;
- `DummyDatabase` é exclusiva de teste — substitui `OrdinaryDatabase` sem que `ConfigurableRecordFinder` perceba a troca;
- o compartilhamento em produção passa a ser controlado pelo container (`.SingleInstance()`), com uma instância por container;
- o ganho real não é "trocar de Singleton por outro Singleton disfarçado" — é separar a lógica de negócio do controle de ciclo de vida, o que é a definição prática de testabilidade.

---


<a id="sec-030"></a>
## 41. Monostate

[↑ Voltar ao início](#inicio)


As seções 39 e 40 melhoraram a testabilidade ao retirar o acesso estático do consumidor: a dependência passou a ser recebida via construtor (`ConfigurableRecordFinder`), e o container de DI assumiu a política de compartilhamento em produção.

O **Monostate** ataca um problema vizinho, mas diferente: como ter várias instâncias que **compartilham o mesmo estado**, sem que a classe precise de um construtor privado nem de um ponto de acesso estático (`.Instance`). Ele é um idioma relacionado, **não uma entrada dos 23 padrões originais do GoF**. A leitura desta seção usa `CreationalPatterns/5.Singleton/Aula04_Monostate/Monostate.cs`.

---

<a id="sec-031"></a>
### 41.1 O que é Monostate

[↑ Voltar ao início](#inicio)

**Monostate** é uma variação conceitualmente relacionada ao Singleton: em vez de restringir o número de instâncias, deixa a classe instanciável com `new`, mas faz o **estado** ser `static`.

Como campos estáticos pertencem à classe, e não a cada objeto individual, toda instância criada lê e escreve exatamente os mesmos valores na memória. O efeito observável — "existe um único conjunto de dados compartilhado" — é o mesmo do Singleton. O caminho até lá é que muda.

```csharp
public class CEO
{
    // Campos estáticos: pertencem à classe, não à instância.
    private static string name;
    private static int age;

    // Propriedades comuns, de instância — mas por baixo,
    // sempre leem/escrevem o mesmo campo estático.
    public string Name
    {
        get => name;
        set => name = value;
    }

    public int Age
    {
        get => age;
        set => age = value;
    }
}
```

---

<a id="sec-032"></a>
### 41.2 Singleton vs. Monostate: a diferença estrutural

[↑ Voltar ao início](#inicio)

| | Singleton (Aula 01) | Monostate (esta seção) |
| --- | --- | --- |
| Construtor | `private` — bloqueia `new` externo | Público (implícito) — `new CEO()` livre |
| Quantidade de instâncias possíveis | Uma no escopo efetivamente controlado pela classe | Quantas o código quiser |
| Onde mora o estado único | No único objeto existente | Em campos `static`, compartilhados por todas as instâncias |
| Ponto de acesso | Explícito e estático (`Classe.Instance`) | Nenhum — parece uma API de instância comum |
| API do cliente | Precisa saber que está lidando com um Singleton | Não precisa saber nada — usa `new` e propriedades normalmente |

O ponto mais importante desta tabela é a última linha: do ponto de vista de quem consome `CEO`, o código parece completamente comum. Não há nenhum sinal sintático de que `ceo` e `ceo2` vão compartilhar dados. Essa "invisibilidade" é ao mesmo tempo a vantagem e o risco do padrão — volta na seção 41.4.

---

<a id="sec-033"></a>
### 41.3 O comportamento observado no `Main`

[↑ Voltar ao início](#inicio)

```csharp
var ceo = new CEO();
ceo.Name = "Adam Smith";
ceo.Age = 55;

var ceo2 = new CEO();

Console.WriteLine(ceo2);
```

| Passo | O que acontece | Por quê |
| --- | --- | --- |
| `ceo.Name = "Adam Smith"` | Escreve no campo estático `name` | `Name` é só uma "porta de entrada" para o campo estático — não existe `name` por instância |
| `ceo.Age = 55` | Escreve no campo estático `age` | Mesmo raciocínio |
| `var ceo2 = new CEO();` | Cria uma instância nova, sem atribuir nada a ela | Nada impede a criação — `CEO` não é um Singleton clássico |
| `Console.WriteLine(ceo2)` | Imprime `Name: Adam Smith, Age: 55` | `ceo2` nunca recebeu esses valores diretamente — mas como `name`/`age` são estáticos, `ceo2` enxerga o mesmo estado que `ceo` escreveu |

Se `name` e `age` fossem campos de instância comuns (sem `static`), `ceo2` sairia com `Name: null, Age: 0`. A saída real do programa é a prova em tempo de execução de que o estado é compartilhado por trás de uma fachada de instância comum.

---

<a id="sec-034"></a>
### 41.4 Vantagens e desvantagens

[↑ Voltar ao início](#inicio)

| Vantagem | Por quê |
| --- | --- |
| API mais natural para o consumidor | `new CEO()` funciona como qualquer classe comum; não exige conhecer um padrão específico |
| API de instância preservada | Chamadores continuam usando `new CEO()` e propriedades de instância; subclasses também enxergam os campos estáticos herdados, embora isso possa tornar o estado compartilhado ainda menos explícito |
| Migração mais suave | Uma classe comum pode virar Monostate (tornando campos `static`) sem quebrar a assinatura pública nem o código que já chama `new` |

| Desvantagem | Por quê |
| --- | --- |
| **Não resolve o problema de testabilidade das seções 39/40** | O estado continua estático e global. Não existe interface (`IDatabase`-like) nem construtor que aceite uma implementação alternativa — não há como injetar um dublê de teste aqui |
| Estado invisível ao ler o código do cliente | Nada na assinatura de `CEO` avisa que instâncias compartilham dados; é fácil um desenvolvedor assumir, por engano, que `ceo2` é independente de `ceo` |
| Testes ficam acoplados por ordem de execução | Como o estado é estático, um teste que roda antes pode deixar valores que "vazam" para o teste seguinte — o mesmo tipo de problema de isolamento que a seção 39 descreveu para o Singleton |

O ponto central da tabela de desvantagens: Monostate resolve a rigidez de **instanciação** do Singleton (não precisa mais de `.Instance`, aceita `new` e herança), mas **não** resolve o problema de testabilidade que motivou as seções 39 e 40. O estado continua global e estático — só a fachada mudou.

---

<a id="sec-035"></a>
### 41.5 Caso de uso: quando Monostate faz sentido na prática

[↑ Voltar ao início](#inicio)

O cenário clássico em que Monostate é escolhido **no lugar de** um Singleton é a migração de uma classe já existente, amplamente instanciada com `new` em múltiplos pontos do código, para um comportamento de estado compartilhado — **sem poder alterar todos esses pontos de chamada**.

Exemplo concreto: imagine que `CEO` já é usada em dezenas de lugares do sistema como `var ceo = new CEO();`, antes de qualquer padrão ser aplicado. Se a decisão de negócio passa a ser "só pode existir um CEO, e todo mundo precisa enxergar os mesmos dados", duas rotas são possíveis:

| Rota | O que exige |
| --- | --- |
| **Converter `CEO` em Singleton clássico** | Reescrever **todo** `new CEO()` existente para `CEO.Instance` — um refactor que toca toda chamada no sistema, além de tornar o construtor `private` |
| **Converter `CEO` em Monostate** | Só tornar os campos internos `static` — nenhuma chamada `new CEO()` existente precisa mudar; o comportamento de estado único passa a valer automaticamente para todo código já escrito |

É esse segundo cenário — legado que não pode ser massivamente refatorado, mas que precisa passar a se comportar como se tivesse um único estado global — que justifica Monostate na prática. Outros casos de uso típicos seguem a mesma lógica: objetos de configuração ou contexto de aplicação (idioma corrente, tema visual, flags de ambiente) que historicamente são instanciados livremente pelo time, mas que devem refletir sempre o mesmo valor em qualquer ponto do sistema.

O trade-off dessa conveniência já foi descrito na seção 41.4: o ganho de não precisar tocar em código existente vem à custa de manter o problema de testabilidade das seções 39/40 sem solução.

---

<a id="sec-036"></a>
### 41.6 Comparativo de decisão: Singleton clássico vs. Monostate

[↑ Voltar ao início](#inicio)

| Critério | Escolha Singleton clássico | Escolha Monostate |
| --- | --- | --- |
| Código já chama `new` em vários lugares e não pode ser tocado | Não se aplica — exigiria reescrever cada `new` para `.Instance` | **Sim** — só os campos internos mudam, nenhuma chamada externa é afetada |
| Precisa impedir herança/subclasses descontroladas | **Sim** — construtor `private` bloqueia isso nativamente | Não impede — qualquer subclasse de `CEO` herda e continua compartilhando o mesmo estado estático |
| Testabilidade é prioridade | **Nenhum dos dois resolve sozinho** — ambos exigem migrar para DI (seção 40) para ficarem testáveis | Idem |
| Precisa deixar explícito, na leitura do código, que a classe é de instância única | **Sim** — `.Instance` sinaliza isso a quem lê | Não — a fachada de classe comum esconde o compartilhamento, o que é uma desvantagem de legibilidade (seção 41.4) |
| Cenário típico | Sistemas novos, escritos já com o padrão em mente (ex.: `SingletonDatabase`, seção 38) | Sistemas legados, ou classes de configuração/contexto que precisam parecer comuns para o time que já as usa |

A leitura correta desta tabela não é "um padrão é melhor que o outro" — é que os dois resolvem o mesmo problema de negócio (estado único) partindo de restrições de projeto diferentes: Singleton parte da liberdade de escrever o código do zero; Monostate parte da restrição de não poder tocar em código que já existe.

---

<a id="sec-037"></a>
### 41.7 O que costuma confundir aqui

[↑ Voltar ao início](#inicio)

- **Achar que Monostate "corrige" o Singleton do ponto de vista de testabilidade.** Não corrige — o campo estático ainda é um único ponto de estado global, só que sem um `.Instance` explícito para apontar o dedo. `DummyDatabase` (seção 40.9) não tem equivalente possível aqui, porque não existe abstração (`interface`) nem construtor que aceite dependência.
- **Achar que `new CEO()` cria um objeto independente.** Cria um objeto novo, sim — mas os dados que esse objeto expõe (`Name`, `Age`) não são dele; são da classe inteira.
- **Confundir Monostate com Singleton "porque no final o comportamento observável é parecido".** O comportamento observável (estado único) é parecido, mas o mecanismo — restrição de instanciação vs. compartilhamento de campos — é oposto, e as implicações práticas (herança, testabilidade, visibilidade do padrão para o consumidor) são diferentes.
- **Achar que Monostate é sempre a escolha "mais fácil".** É mais fácil de introduzir em código legado, mas custa legibilidade — quem lê `new CEO()` isoladamente não tem como saber, sem examinar a classe, que está mexendo em estado global.

---

<a id="sec-038"></a>
### 41.8 Resumo mental

[↑ Voltar ao início](#inicio)

- **Monostate** é um idioma relacionado, fora do catálogo GoF: em vez de restringir o número de instâncias, deixa o **estado** (`static`) compartilhado entre todas elas;
- a classe parece uma API de instância comum — não há `.Instance`, não há construtor `private` — mas por trás das propriedades, todo `get`/`set` opera sobre os mesmos campos estáticos;
- o `Main` prova isso na prática: `ceo2`, criada sem nenhuma atribuição, imprime os dados que `ceo` escreveu, porque ambas compartilham `name` e `age`;
- a vantagem sobre o Singleton clássico é a naturalidade da API e a compatibilidade com herança, sem exigir mecanismos extras;
- a desvantagem central é que **o problema de testabilidade das seções 39/40 continua sem solução** — o estado é global e estático, e não existe nenhum ponto de injeção (interface, construtor) para substituí-lo por um dublê em teste;
- o caso de uso típico é código legado, já instanciado com `new` em vários lugares, que precisa passar a compartilhar estado **sem** reescrever cada ponto de chamada — cenário em que Singleton exigiria um refactor amplo e Monostate não;
- na escolha entre os dois padrões, o critério decisivo não é "qual é melhor", é qual restrição de projeto está em jogo: liberdade para escrever do zero (Singleton) ou impossibilidade de tocar em código já existente (Monostate).

---


<a id="sec-039"></a>
## 42. Per-Thread Singleton

[↑ Voltar ao início](#inicio)


O Monostate (seção 41) mudou a forma de instanciação, mas manteve estado estático compartilhado. O **Per-Thread Singleton** é outra variação fora do catálogo GoF: troca o valor compartilhado por uma instância associada a cada thread física que acessar a propriedade. A leitura desta seção usa `CreationalPatterns/5.Singleton/Aula05_PerThreadSingleton/ThreadSingleton.cs`.

---

<a id="sec-040"></a>
### 42.1 O que é Per-Thread Singleton

[↑ Voltar ao início](#inicio)

Na configuração da Aula 01, um campo estático `Lazy<T>` expõe um valor compartilhado por todas as threads daquele contexto de execução — é justamente esse acesso global que dificulta o isolamento em teste.

O Per-Thread Singleton troca `Lazy<T>` por `ThreadLocal<T>`. A diferença de comportamento está inteira nesse tipo:

- **`Lazy<T>` estático:** um único valor, compartilhado por todas as threads.
- **`ThreadLocal<T>` estático:** associa um valor a cada thread. Quando uma thread acessa `.Value` pela primeira vez, a fábrica é executada para aquela thread; outra thread possui seu próprio valor.

```csharp
public sealed class PerThreadSingleton
{
    private static ThreadLocal<PerThreadSingleton> threadInstance
      = new ThreadLocal<PerThreadSingleton>(() => new PerThreadSingleton());

    public int Id;

    private PerThreadSingleton()
    {
        Id = Thread.CurrentThread.ManagedThreadId;
    }

    public static PerThreadSingleton Instance => threadInstance.Value;
}
```

O construtor `private` continua bloqueando `new` externo, exatamente como no Singleton comum — a única forma de obter uma instância é via `Instance`. A diferença não está em *como* a instância é protegida, está em *quantas* instâncias existem ao mesmo tempo: uma por thread, não uma por processo.

O `sealed` na classe impede herança, evitando que uma subclasse abra um caminho de instanciação alternativo — a mesma prática defensiva já vista em implementações de Singleton.

---

<a id="sec-041"></a>
### 42.2 Singleton comum vs. Per-Thread Singleton: a diferença estrutural

[↑ Voltar ao início](#inicio)

| | Singleton comum (Aula 01) | Per-Thread Singleton (esta seção) |
| --- | --- | --- |
| Tipo do campo estático | `Lazy<T>` | `ThreadLocal<T>` |
| Quantas instâncias ficam acessíveis por esse campo | Um valor compartilhado | Um valor por thread que acessar `Instance` |
| Escopo da garantia de unicidade | Campo estático/contexto de carregamento | Cada thread física |
| Duas threads acessando `Instance` recebem o mesmo objeto? | Sim, sempre | **Não** — cada uma recebe a sua própria instância |
| Uma mesma thread acessando `Instance` duas vezes recebe o mesmo objeto? | Sim | **Sim** — dentro da mesma thread, o comportamento é idêntico ao Singleton comum |

O ponto mais importante desta tabela é a última linha: Per-Thread Singleton não abre mão de unicidade — ele só **reposiciona o escopo** dessa unicidade, do processo para a thread.

---

<a id="sec-042"></a>
### 42.3 O comportamento observado no `Main`

[↑ Voltar ao início](#inicio)

```csharp
var t1 = Task.Factory.StartNew(() =>
{
    Console.WriteLine($"t1: " + PerThreadSingleton.Instance.Id);
});

var t2 = Task.Factory.StartNew(() =>
{
    Console.WriteLine($"t2: " + PerThreadSingleton.Instance.Id);
});

var t3 = Task.Factory.StartNew(() =>
{
    Console.WriteLine($"t3: " + PerThreadSingleton.Instance.Id);
});

Task.WaitAll(t1, t2, t3);
```

| Passo | O que acontece | Por quê |
| --- | --- | --- |
| `Task.Factory.StartNew(...)` × 3 | Três tarefas são agendadas; o agendador decide em quais threads do pool elas executarão | Uma `Task` não promete uma thread exclusiva |
| `PerThreadSingleton.Instance` dentro de cada task | O valor é obtido para a thread física que estiver executando naquele instante | Duas tarefas executadas na mesma thread podem observar a mesma instância |
| `Id` impresso por `t1`, `t2`, `t3` | Pode repetir ou variar conforme o agendamento | O exemplo ilustra afinidade por thread, mas não prova que cada tarefa possui uma thread diferente |
| `Task.WaitAll(t1, t2, t3)` | Bloqueia o `Main` até as três tasks terminarem | Garante que o processo não encerre antes de todo `Console.WriteLine` rodar |

Se o mesmo código fosse escrito com o Singleton comum (`Lazy<T>`), os três `Id` impressos seriam sempre o mesmo valor — o da primeira thread que tivesse acessado `Instance`.

---

<a id="sec-043"></a>
### 42.4 Vantagens e desvantagens

[↑ Voltar ao início](#inicio)

| Vantagem | Por quê |
| --- | --- |
| Elimina a necessidade de sincronização manual (`lock`) para estado por thread | Cada thread já enxerga sua própria cópia; não há concorrência de escrita no mesmo objeto |
| Isola o valor armazenado pelo `ThreadLocal<T>` entre threads | O isolamento vale para esse valor; objetos externos ainda podem ser compartilhados por outros caminhos |
| Mantém a API familiar do Singleton (`.Instance`) | Quem consome o código não precisa aprender uma sintaxe nova |

| Desvantagem | Por quê |
| --- | --- |
| **Ainda não resolve o problema de testabilidade das seções 39/40** | O acesso continua estático (`PerThreadSingleton.Instance`); não há interface nem construtor que aceite uma implementação alternativa para teste |
| Multiplica o custo de inicialização | Se a fábrica for cara (leitura de arquivo, conexão de rede), esse custo se repete a cada thread nova, não uma vez só |
| Comportamento pode surpreender quem espera um Singleton "de verdade" | Um desenvolvedor acostumado com Singleton comum pode assumir, por engano, que duas threads enxergam o mesmo estado — o nome "Singleton" no padrão é, nesse sentido, um pouco enganoso |

---

<a id="sec-044"></a>
### 42.5 Caso de uso: quando Per-Thread Singleton faz sentido na prática

[↑ Voltar ao início](#inicio)

O cenário típico é um recurso que **não pode ser compartilhado com segurança entre threads**, mas que também não faz sentido recriar a cada chamada dentro da mesma thread — por exemplo:

- Uma instância comum de `Random` que não foi projetada para uso concorrente. Em .NET 6 ou superior, `Random.Shared` já fornece uma instância segura para acesso simultâneo e costuma eliminar a necessidade desse exemplo.
- Um recurso nativo estritamente associado a uma thread, desde que a aplicação realmente controle essa afinidade.
- Um buffer ou cache de trabalho por thread, útil em processamento paralelo onde cada *worker* precisa de sua própria área temporária.

`DbContext` não deve ser apresentado como “contexto por thread”. O EF Core recomenda uma instância por unidade de trabalho/escopo e proíbe operações paralelas na mesma instância; em fluxos concorrentes, use instâncias separadas e aguarde imediatamente as operações assíncronas.

Nesses casos, um valor por thread pode evitar sincronização sobre aquele estado. Entretanto, código `async` pode continuar em outra thread e threads do pool são reutilizadas entre trabalhos; portanto, `ThreadLocal<T>` não representa automaticamente o escopo lógico de uma requisição ou operação assíncrona.

---

<a id="sec-045"></a>
### 42.6 Comparativo de decisão: Singleton comum vs. Per-Thread Singleton

[↑ Voltar ao início](#inicio)

| Critério | Escolha Singleton comum | Escolha Per-Thread Singleton |
| --- | --- | --- |
| O recurso é seguro para acesso concorrente (thread-safe) | **Sim** — não há motivo para isolar por thread | Não se aplica — não é necessário pagar o custo de múltiplas instâncias |
| O recurso não é thread-safe, mas precisa ser reaproveitado dentro da mesma thread | Exigiria `lock` em todo acesso, criando contenção | **Sim** — cada thread tem sua cópia isolada, sem necessidade de `lock` |
| O estado precisa ser idêntico para todas as threads do processo | **Sim** — é exatamente essa a garantia do Singleton comum | Não — aqui cada thread vê um estado diferente, por definição |
| Testabilidade é prioridade | **Nenhum dos dois resolve sozinho** — ambos exigem migrar para DI (seção 40) para ficarem testáveis | Idem |
| Cenário típico | Configuração imutável ou cache compartilhado e thread-safe | Buffers de processamento com afinidade real por thread |

---

<a id="sec-046"></a>
### 42.7 O que costuma confundir aqui

[↑ Voltar ao início](#inicio)

- **Achar que Per-Thread Singleton é "menos Singleton" por ter várias instâncias.** Dentro do escopo de cada thread, a garantia de unicidade é tão forte quanto no Singleton comum — o que muda é o tamanho do escopo (thread em vez de processo), não a existência da garantia.
- **Achar que três tarefas significam três threads.** Não significam: o `TaskScheduler` pode executar tarefas na mesma thread em momentos diferentes, e o ThreadPool reutiliza threads.
- **Achar que isso resolve os mesmos problemas que DI resolveu nas seções 39/40.** Não resolve — o acesso continua estático e global (agora "global por thread"), e nada aqui permite substituir a implementação por um dublê de teste.
- **Esquecer os `using` necessários.** `ThreadLocal<T>` e `Thread` vêm de `System.Threading`; `Task` vem de `System.Threading.Tasks`. Sem esses dois `using`, o arquivo não compila.

---

<a id="sec-047"></a>
### 42.8 Resumo mental

[↑ Voltar ao início](#inicio)

- **Per-Thread Singleton** troca o campo estático `Lazy<T>` por `ThreadLocal<T>`, associando um valor a cada thread física;
- dentro da mesma thread, o comportamento é idêntico ao Singleton comum: chamadas repetidas a `Instance` sempre devolvem a mesma instância;
- entre threads diferentes, cada uma recebe sua própria instância, criada de forma independente na primeira vez que acessa `Instance`;
- o `Main` observa os IDs das threads escolhidas pelo agendador; eles podem repetir, portanto o exemplo não comprova uma relação de uma thread por tarefa;
- a vantagem central é evitar `lock` manual para recursos que não são thread-safe, mas que valem a pena reaproveitar dentro da mesma thread;
- a desvantagem central é a mesma das seções 39–41: **o acesso estático continua impedindo testabilidade** — nada aqui foi resolvido nesse sentido, é um problema ortogonal ao que este padrão se propõe a resolver.

---


<a id="sec-048"></a>
## 43. Ambient Context

[↑ Voltar ao início](#inicio)


Esta parte corresponde ao arquivo `CreationalPatterns/5.Singleton/Aula06_AmbientContext/AmbientContext.cs`.

O **Ambient Context** (Contexto Ambiente) é uma técnica de design contextual para expor dados transversais de um fluxo sem precisar passá-los manualmente por todas as assinaturas. Ele se parece com um acesso global, mas o valor ativo depende do escopo atual.

`Ambient Context` não pertence aos 23 padrões do catálogo original do GoF. Ele aparece aqui como idioma relacionado a estado acessível pelo contexto e deve ser usado com cautela, pois também esconde dependências.

Imagine o "ar" que você respira: ele está simplesmente disponível ao seu redor (no ambiente) sem que você precise carregá-lo explicitamente em uma mochila para onde quer que vá. No código, o Ambient Context faz exatamente isso: ele coloca dados "no ar" para um determinado trecho do programa.

---

<a id="sec-049"></a>
### 43.1 Conexão com os capítulos anteriores

[↑ Voltar ao início](#inicio)

Para ler este padrão com maturidade de design, é fundamental conectá-lo ao que você estudou nas seções anteriores deste capítulo:

- **O contraste direto com o Singleton tradicional (Aulas 38 e 39):** o Singleton expõe um valor compartilhado no escopo escolhido; o Ambient Context permite substituir temporariamente o valor atual, mas também introduz dependência implícita.
- **O contraste com o Per-Thread Singleton (Aula 42):** uma pilha de contextos modela escopos aninhados, enquanto `ThreadLocal<T>` acompanha uma thread física. Em código assíncrono, `AsyncLocal<T>` faz o valor fluir com o `ExecutionContext`, mas não transforma automaticamente objetos mutáveis em cópias isoladas.
- **O contraponto ao Parameter Pollution do Builder/Factory (Capítulos 2 e 3):** Quando você usa Builders ou Fábricas para construir objetos profundos na árvore de dependências, frequentemente se depara com a dor de ter que passar parâmetros transversais (como fusos horários, tokens ou configurações de ambiente) por todos os construtores intermediários. O Ambient Context limpa essas assinaturas, deixando que os objetos busquem esses dados transversais diretamente "no ar".

---

<a id="sec-050"></a>
### 43.2 Por que usar?

[↑ Voltar ao início](#inicio)

Para entender o porquê de usar este padrão, imagine que você está construindo um sistema de engenharia para desenhar prédios. Você precisa criar centenas de objetos do tipo `Parede`. Todas as paredes de um andar costumam ter a mesma altura padrão (por exemplo, 3 metros).

Se você tentasse resolver isso de forma tradicional, teria dois grandes problemas:

1. **Poluição de Parâmetros (*Parameter Pollution*):** Você teria que passar a variável `alturaPadrao` no construtor de **todas** as classes e funções do sistema. Se uma função intermediária não usa a altura, mas precisa chamar alguém que usa, ela é obrigada a receber esse parâmetro só para repassá-lo adiante. O código fica poluído e difícil de manter.
2. **O Engessamento do Singleton:** Se você decidir criar uma classe estática global (`ConfiguracaoGlobal.AlturaParede = 3000`), você resolve a poluição, mas destrói a flexibilidade. Se em um ponto específico do prédio (como o teto ou a garagem) você precisar temporariamente de uma parede com 3,5 metros, alterar o Singleton mudará a altura do prédio inteiro, quebrando o resto do sistema.

> **O Ambient Context trata essa dor:** ele oferece um ponto de acesso ambiental com variações locais e temporárias. O custo é uma dependência implícita que precisa de escopo, restauração e concorrência bem definidos.

---

<a id="sec-051"></a>
### 43.3 Quando usar?

[↑ Voltar ao início](#inicio)

Você deve usar o Ambient Context quando uma informação ou serviço atender a **dois critérios simultâneos**:

- **É um interesse transversal (*Cross-Cutting Concern*):** A informação precisa estar disponível em praticamente qualquer lugar do fluxo daquele domínio (ex: logs, fusos horários, conexões de banco de dados, escopos de transações financeiras, ou a altura padrão de um edifício).
- **O valor muda dependendo do contexto ou momento:** O valor precisa ser alterado temporariamente para um bloco de código específico e, logo em seguida, voltar automaticamente ao que era antes.

---

<a id="sec-052"></a>
### 43.4 Quando evitar?

[↑ Voltar ao início](#inicio)

- **Regras de negócio locais:** Não esconda dados que são vitais para a lógica de uma única classe dentro de um contexto invisível. Isso gera acoplamento implícito (mágicas no código que outros desenvolvedores não vão entender de onde vêm).
- **Cenários assíncronos sem o devido cuidado:** uma pilha estática comum é compartilhada por todas as threads e não é segura para acesso concorrente. `AsyncLocal<T>` acompanha o contexto lógico de execução, mas valores mutáveis ainda podem ser compartilhados entre fluxos derivados; ele não é uma garantia universal de isolamento.

---

<a id="sec-053"></a>
### 43.5 Diferença essencial: Singleton vs. Ambient Context

[↑ Voltar ao início](#inicio)

Embora ambos permitam uma chamada global (como `Contexto.Current`), o controle de escopo é diferente:

| Característica | Singleton Tradicional | Ambient Context |
| --- | --- | --- |
| **Instância/valor** | Um objeto compartilhado no escopo controlado pela implementação; ele pode ser mutável ou imutável. | Um valor atual que pode variar entre escopos aninhados. |
| **Escopo** | Definido pela implementação ou pelo container. | Deve ser definido pela pilha, thread ou contexto lógico escolhido. |
| **Analogia** | O **número do seu CPF** (é um só e nunca muda). | O **Caps Lock** do seu teclado (você liga quando precisa e desliga para voltar ao normal). |

---

<a id="sec-054"></a>
### 43.6 O que é a `struct` utilizada no código e quando usá-la?

[↑ Voltar ao início](#inicio)

No exemplo prático abaixo, a coordenada `Point` foi definida como `struct` em vez de `class`. Isso muda sua semântica para tipo de valor; qualquer impacto de alocação e desempenho depende do modo de uso e deve ser medido.

#### 43.6.1 Tipo de valor (*Value Type*) vs. tipo de referência (*Reference Type*)

- **Classes são tipos de referência:** ao criar uma instância de classe, a variável normalmente guarda uma referência para o objeto. Se você copiar essa variável para outra, as duas passam a apontar para o mesmo objeto.
- **Structs são tipos de valor:** ao atribuir uma `struct` a outra variável ou passá-la por valor, seus dados são copiados. O local físico de armazenamento depende do contexto: uma `struct` local pode ficar na stack; uma `struct` dentro de uma classe fica armazenada inline dentro do objeto que está na heap.

#### 43.6.2 Quando faz sentido usar uma `struct`?

As *Framework Design Guidelines* da Microsoft recomendam **considerar** `struct` apenas quando o tipo atende em conjunto a estes critérios:

1. **Representa logicamente um valor único:** Deve agir como tipos primitivos (ex: um número complexo, uma cor RGB, um ponto tridimensional `X,Y,Z`, ou um intervalo de datas).
2. **É pequena:** A diretriz histórica usa tamanho de instância inferior a 16 bytes como regra prática, não como garantia universal de desempenho.
3. **É imutável:** Uma vez criada, seus valores não devem mudar. Mudar os dados de uma struct copiada por valor gera bugs difíceis de rastrear.
4. **Não precisa sofrer *Boxing* frequentemente:** Não deve ser convertida para interfaces ou para o tipo `object` a todo momento, pois isso anularia o ganho de performance jogando-a de volta para a Heap.

O `Point` do arquivo local é mutável, portanto não atende integralmente a essa diretriz. Isso não torna o exemplo inválido, mas impede usar a lista como prova automática de que `struct` é a melhor escolha para produção.

#### 43.6.3 O impacto prático no design de sistemas

No nosso sistema de engenharia (`Building`), uma parede precisa apenas delimitar onde começa e onde termina. Se criássemos muitos pontos coordenados usando classes, cada ponto poderia virar uma alocação separada, aumentando a pressão sobre o *Garbage Collector*.

Como `Point` é uma `struct`, os dois pontos ficam armazenados diretamente dentro de `Wall`. Isso não torna o sistema magicamente "zero heap", porque `Wall` ainda é uma classe, mas evita alocações separadas para cada coordenada simples.

---

<a id="sec-055"></a>
### 43.7 Como isso aparece na estrutura do código

[↑ Voltar ao início](#inicio)

Uma forma didática de criar um Ambient Context síncrono é utilizar uma **pilha** (`Stack<T>`). O `Dispose` remove o contexto atual e restaura o anterior, desde que os escopos sejam encerrados em ordem LIFO. Essa implementação estática não é segura para concorrência nem adequada, sem adaptação, a fluxos assíncronos.

```csharp
public class BuildingContext : IDisposable
{
    public int WallHeight;

    // =========================================================================
    // O COMPONENTE SINGLETON (Ponto de Acesso Global)
    // =========================================================================
    // A pilha e a propriedade estática abaixo representam a mecânica do Singleton:
    // Expõe um ponto de acesso ambiental por meio de `BuildingContext.Current`.
    private static Stack<BuildingContext> stack = new Stack<BuildingContext>();

    // Construtor estático: garante que o sistema comece com um contexto padrão (raiz)
    static BuildingContext()
    {
        stack.Push(new BuildingContext(0)); // Altura padrão zero se nada for dito
    }

    // Ponto de acesso global (Mecânica Singleton)
    // Qualquer classe no sistema pode olhar "para o ar" e ver o topo da pilha
    public static BuildingContext Current => stack.Peek();


    // =========================================================================
    // O COMPONENTE AMBIENT CONTEXT (Gerenciamento de Escopo Dinâmico)
    // =========================================================================
    // Aqui o padrão se descola do Singleton tradicional: em vez de uma instância fixa,
    // nós empilhamos e desempilhamos contextos diferentes usando o ciclo de vida do IDisposable.

    // Construtor: Entrando em um novo Contexto Ambiente (Push)
    public BuildingContext(int wallHeight)
    {
        WallHeight = wallHeight;
        stack.Push(this); // Coloca este novo contexto "no ar" (no topo da pilha)
    }

    // Dispose: Saindo do Contexto Ambiente (Pop)
    // Quando o bloco "using" terminar, remove o contexto atual e volta para o anterior
    public void Dispose()
    {
        if (stack.Count > 1)
        {
            stack.Pop(); // Tira o contexto atual do ar e restaura o anterior
        }
    }
}

```

---

<a id="sec-056"></a>
### 43.8 Como o código cliente consome o padrão

[↑ Voltar ao início](#inicio)

O grande ganho visual e ergonômico está no uso da instrução `using` do C#. Ela delimita perfeitamente onde aquele "ambiente" é válido.

```csharp
var house = new Building();

// =========================================================================
// DEFINIÇÃO DOS ESCOPOS DO AMBIENT CONTEXT
// =========================================================================

// 1. Iniciando o Contexto Ambiente "A" (Altura: 3000)
using (new BuildingContext(3000))
{
    // O construtor de Wall não pede altura por parâmetro!
    // Ele vai ler implicitamente o contexto de 3000 que está ativo no momento.
    house.Walls.Add(new Wall(new Point(0, 0), new Point(5000, 0)));
    house.Walls.Add(new Wall(new Point(0, 0), new Point(0, 4000)));

} // O Dispose() é chamado, limpa a pilha e restaura o contexto anterior.

// 2. Iniciando o Contexto Ambiente "B" (Altura: 3500)
using (new BuildingContext(3500))
{
    // Aqui, o mesmo construtor de Wall agora lerá 3500 do "ar"
    house.Walls.Add(new Wall(new Point(0, 0), new Point(6000, 0)));
}


// =========================================================================
// CONSUMO IMPLÍCITO (A Mágica do Padrão)
// =========================================================================
public class Wall
{
    public Point Start, End;
    public int Height;

    public Wall(Point start, Point end)
    {
        Start = start;
        End = end;

        // A assinatura do construtor fica totalmente limpa de Parameter Pollution.
        // A classe Wall usa a propriedade estática (Singleton) para capturar
        // o estado do escopo dinâmico atual (Ambient Context).
        Height = BuildingContext.Current.WallHeight;
    }
}

```

---

<a id="sec-057"></a>
### 43.9 Onde isso é usado no mundo real?

[↑ Voltar ao início](#inicio)

No ASP.NET Core, `IHttpContextAccessor` é um exemplo de acesso a estado ambiental, mas seu uso não é obrigatório em toda aplicação e deve ser criterioso.

Ele fornece acesso ao `HttpContext` atual, quando existe, e sua implementação se apoia em `AsyncLocal<T>`. Não funciona “exatamente” como a pilha estática do exemplo: o valor flui com o contexto assíncrono. A própria documentação da Microsoft alerta para custo em chamadas assíncronas, dependência de estado ambiental e maior dificuldade de testes; fora de uma requisição ativa, `HttpContext` pode ser `null`.

O arquivo local ainda possui duas limitações concretas: o construtor estático faz `stack.Push(new BuildingContext(0))`, mas o construtor de instância já executa `stack.Push(this)`, inserindo o contexto raiz duas vezes; além disso, `Dispose` não confirma que o objeto descartado é o topo da pilha. Esses detalhes precisam ser corrigidos antes de reutilizar a implementação em produção.

---

<a id="sec-058"></a>
## Coding Exercise 4: Singleton Coding Exercise

[↑ Voltar ao início](#inicio)


Esta parte corresponde ao arquivo `CreationalPatterns/5.Singleton/Aula07_SingletonExcercise/SingletonExcercise.cs`.

<a id="sec-059"></a>
### O objetivo do exercício

[↑ Voltar ao início](#inicio)

O objetivo deste exercício é testar o entendimento sobre o impacto de um Singleton na **testabilidade** do código (um dos tópicos mais críticos abordados no Capítulo 5).

Geralmente, o enunciado solicita que você avalie se um objeto fornecido é um Singleton real ou propõe um cenário onde um componente depende de um Singleton *hardcoded*, o que dificulta testes unitários isolados. Para verificar a reutilização da instância no mesmo processo de teste, compare identidade com `ReferenceEquals(a, b)`; endereços físicos não fazem parte do contrato gerenciado.

Se o foco for a validação da instância, a regra central do exercício é comprovar que:

$$A \equiv B$$

Ou seja, dentro do escopo testado, chamadas repetidas ao ponto de acesso devem devolver a mesma referência de objeto.

---

<a id="sec-060"></a>
### Recapitulação: o passo a passo da construção de um Singleton

[↑ Voltar ao início](#inicio)

O padrão **Singleton** tem uma intenção criacional clara: **garantir uma instância da classe no escopo controlado pela implementação e fornecer um ponto global de acesso nesse escopo.**

Para construir um Singleton robusto em C# do zero, seguimos uma evolução de etapas para garantir encapsulamento e segurança:

#### Passo 1: bloquear o construtor externo

O primeiro passo absoluto é tornar o construtor da classe `private`. Isso impede que qualquer código cliente use o operador `new`.

```csharp
public class SerasaDatabase
{
    // 1. Construtor privado impede novos acoplamentos diretos externos
    private SerasaDatabase()
    {
        // Inicialização pesada ou conexão com banco aqui
    }
}

```

#### Passo 2: criar o repositório da instância única

A classe precisa guardar a sua própria e única instância dentro de um campo estático e privado.

```csharp
private static volatile SerasaDatabase? _instance;

```

#### Passo 3: fornecer o ponto de acesso global

Criamos uma propriedade pública estática (geralmente chamada de `Instance`) que expõe o objeto. Para eficiência, costuma-se aplicar a inicialização preguiçosa (*Lazy Initialization*), onde o objeto só é criado no momento do primeiro acesso.

##### A versão básica sem thread-safety

```csharp
public static SerasaDatabase Instance
{
    get
    {
        if (_instance == null)
        {
            _instance = new SerasaDatabase();
        }
        return _instance!;
    }
}

```

> ⚠️ **O Perigo:** O código acima possui uma condição de corrida (*race condition*). Se duas threads entrarem no `if (_instance == null)` exatamente ao mesmo tempo, duas instâncias serão criadas, violando o padrão.

---

#### Passo 4: garantir segurança entre threads

Em aplicações concorrentes, precisamos proteger o bloco de criação. Existem duas formas comuns de resolver isso em C#:

##### Abordagem A: com bloqueio explícito (`lock`)

Usamos uma linha de suporte (`lock object`) para garantir que apenas uma thread inspecione e crie a instância por vez.

```csharp
public class SerasaDatabase
{
    private static volatile SerasaDatabase? _instance;
    private static readonly object _padlock = new object();

    private SerasaDatabase() { }

    public static SerasaDatabase Instance
    {
        get
        {
            // Double-check locking pattern
            if (_instance == null)
            {
                lock (_padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new SerasaDatabase();
                    }
                }
            }
            return _instance!;
        }
    }
}

```

##### Abordagem B: usando `Lazy<T>`

O .NET fornece `Lazy<T>`, que combina inicialização preguiçosa com políticas de concorrência. Na sobrecarga abaixo, o modo padrão é `ExecutionAndPublication`; “performático” não deve ser presumido sem medição.

```csharp
public class SerasaDatabase
{
    // O .NET garante por baixo dos panos que a criação é thread-safe
    private static readonly Lazy<SerasaDatabase> _lazyInstance =
        new Lazy<SerasaDatabase>(() => new SerasaDatabase());

    private SerasaDatabase() { }

    public static SerasaDatabase Instance => _lazyInstance.Value;
}

```

---

<a id="sec-061"></a>
### O grande problema do Singleton tradicional: testabilidade

[↑ Voltar ao início](#inicio)

Como mencionado nas dores levantadas no mapeamento conceitual do livro do GoF, o Singleton introduz um estado global oculto. Se a sua classe de negócio chama `SerasaDatabase.Instance` diretamente no meio de um método, fica difícil substituir esse banco de dados por um dublê em um teste unitário. O teste pode acabar acessando o banco real.

#### A solução profissional: Singleton via Injeção de Dependência (DI)

Em muitos sistemas, a classe pode ser criada normalmente e seu ciclo de vida delegado ao container de DI do .NET, com `.AddSingleton<IDatabase, SerasaDatabase>()`. O container reutiliza uma instância por `ServiceProvider`; construir providers independentes pode criar outras. O ganho de testabilidade vem de o consumidor depender de `IDatabase` por injeção, não do nome `Singleton` do lifetime.

---


<a id="sec-glossario"></a>
## Glossário
[↑ Voltar ao início](#inicio)

- **Instância única:** instância compartilhada durante determinado ciclo de vida.
- **Acesso global:** ponto conhecido para obter um serviço compartilhado.
- **Inicialização tardia:** criação apenas na primeira solicitação.
- **Injeção de dependência (DI):** entrega explícita de dependências aos consumidores.
