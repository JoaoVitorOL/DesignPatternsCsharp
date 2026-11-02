<a id="inicio"></a>
# Design Pattern: Abstract Factory

## Sumário
[↑ Voltar ao início](#inicio)

- [1. O que o Abstract Factory representa?](#sec-001)
  - [O Problema que ele resolve:](#sec-002)
- [2. Em que situações se usa?](#sec-003)
- [3. Instruções passo a passo de como implementar (em Java)](#sec-004)
  - [Exemplo básico de implementação:](#sec-005)
- [4. Explicação detalhada do Exemplo Prático (Configuração de Mobília de Sala)](#sec-006)
  - [As Interfaces e Classes de Produtos: Cadeira, Sofa, MesaDeCentro](#sec-007)
  - [A Interface e as Fábricas Concretas: FabricaMobilia](#sec-008)
  - [A Classe Cliente: ConfiguradorDeSala](#sec-009)
  - [A Execução: Main](#sec-010)
- [5. Implementação nos exemplos C# deste repositório](#sec-011)
- [6. Percurso da criação no exemplo das bebidas](#sec-012)
- [Quando uma família de produtos justifica Abstract Factory](#sec-013)
- [Variações e aprofundamentos](#sec-014)
- [27. Abstract Factory](#sec-015)
  - [27.1 O que ela enfatiza](#sec-016)
  - [27.2 O que o arquivo local ensina na prática](#sec-017)
  - [27.3 Quem é quem no exemplo](#sec-018)
  - [27.4 Por que IHotDrink e não Tea ou Coffee?](#sec-019)
  - [27.5 Exemplo mental clássico](#sec-020)
  - [27.6 Estrutura mental](#sec-021)
  - [27.7 A nuance importante do exemplo atual](#sec-022)
  - [27.8 O ganho principal](#sec-023)
- [28. Abstract Factory and OCP](#sec-024)
  - [28.1 O que exatamente estava errado em Aula07](#sec-025)
  - [28.2 O que precisou ser corrigido em Aula08](#sec-026)
  - [28.3 Entendendo a transição do enum para extensão por factory](#sec-027)
  - [28.4 O que cada trecho menos óbvio está fazendo](#sec-028)
  - [28.5 Por que a Aula08 é mais extensível neste cenário](#sec-029)
  - [28.6 Honestidade técnica importante](#sec-030)
  - [28.7 O que é Reflection neste contexto](#sec-031)
  - [28.8 O que é Dependency Injection](#sec-032)
  - [28.9 DI não é a mesma coisa que container](#sec-033)
  - [28.10 Por que o instrutor usou Reflection na aula](#sec-034)
  - [28.11 Quando Reflection e DI costumam ser preferidos](#sec-035)
  - [28.12 Comparação curta e cirúrgica](#sec-036)
- [Coding Exercise 2: Factory Coding Exercise](#sec-037)
  - [O que o exercício quer de verdade](#sec-038)
  - [Como ler o uso esperado](#sec-039)
  - [O que é o produto neste exercício](#sec-040)
  - [Participantes do padrão no exercício](#sec-041)
  - [O que faz este exercício ser uma factory](#sec-042)
  - [Onde o controle de estado aparece](#sec-043)
  - [Por que a factory não é estática](#sec-044)
  - [Leitura guiada da solução](#sec-045)
  - [O que o aluno precisa enxergar para conseguir reproduzir sozinho](#sec-046)
  - [O que costuma confundir aqui](#sec-047)
  - [Como validar mentalmente se a solução está correta](#sec-048)


**Família GoF:** criacional. **Escopo predominante:** objetos; fábricas concretas produzem famílias compatíveis.

<a id="sec-001"></a>
## 1. O que o Abstract Factory representa?

[↑ Voltar ao início](#inicio)
O padrão **Abstract Factory** é um padrão de projeto criacional que permite a criação de **famílias de objetos relacionados ou dependentes** sem que suas classes concretas sejam especificadas diretamente pelo código cliente.

<a id="sec-002"></a>
### O Problema que ele resolve:

[↑ Voltar ao início](#inicio)
Em sistemas que precisam suportar múltiplos temas, ambientes ou plataformas (como interfaces visuais Windows vs. macOS ou diferentes drivers de banco de dados), a criação direta de objetos acopla o código e gera problemas críticos:
- **Alto acoplamento:** O código fica cheio de chamadas `new` para classes concretas específicas de um ambiente.
- **Mistura indevida de famílias:** Falta de garantia estrutural, permitindo acidentalmente combinar componentes incompatíveis no mesmo contexto (ex: um botão do Windows com um checkbox do macOS).
- **Baixa extensibilidade:** Adicionar uma nova família ou plataforma exige modificar múltiplos pontos do sistema.

---

<a id="sec-003"></a>
## 2. Em que situações se usa?

[↑ Voltar ao início](#inicio)
Use o Abstract Factory quando:
1. **Famílias de objetos compatíveis:** O sistema precisa criar conjuntos de objetos que devem trabalhar juntos de forma harmoniosa (ex: móveis de um mesmo estilo ou componentes de interface de um mesmo tema).
2. **Independência de plataforma/ambiente:** Você deseja isolar a criação de objetos para permitir a troca fácil de temas, drivers, provedores de nuvem ou motores de renderização sem alterar o código cliente.
3. **Restrição de consistência:** É necessário garantir que o cliente utilize apenas produtos pertencentes à mesma família selecionada.

---

<a id="sec-004"></a>
## 3. Instruções passo a passo de como implementar (em Java)

[↑ Voltar ao início](#inicio)

1. **Definir as Interfaces de Produto:** Criar uma interface para cada tipo de produto distinto que compõe a família (ex: `Cadeira`, `Sofa`, `MesaDeCentro`).
2. **Criar Produtos Concretos:** Implementar as interfaces para cada família específica (ex: `CadeiraModerna`, `CadeiraVitoriana`).
3. **Definir a Interface da Fábrica Abstrata:** Criar uma interface que declare um método de criação para cada produto da família (ex: `criarCadeira()`, `criarSofa()`).
4. **Criar Fábricas Concretas:** Implementar a fábrica abstrata para cada família/estilo, retornando os produtos concretos correspondentes e compatíveis.

<a id="sec-005"></a>
### Exemplo básico de implementação:

[↑ Voltar ao início](#inicio)
```java
public interface FabricaMobilia {
    Cadeira criarCadeira();
    Sofa criarSofa();
}

public class FabricaMobiliaModerna implements FabricaMobilia {
    @Override
    public Cadeira criarCadeira() { return new CadeiraModerna(); }
    @Override
    public Sofa criarSofa() { return new SofaModerno(); }
}
```

---

<a id="sec-006"></a>
## 4. Explicação detalhada do Exemplo Prático (Configuração de Mobília de Sala)

[↑ Voltar ao início](#inicio)

O código fornecido demonstra o padrão **Abstract Factory** aplicado na montagem de conjuntos de móveis para sala de estar, garantindo que estilos diferentes (Moderno e Vitoriano) não sejam misturados de forma incorreta.

<a id="sec-007"></a>
### As Interfaces e Classes de Produtos: `Cadeira`, `Sofa`, `MesaDeCentro`

[↑ Voltar ao início](#inicio)
Definem os contratos e as implementações concretas para cada estilo de mobília.

```java
package moveis;

public interface Cadeira {
    void assentar();
}

public class CadeiraModerna implements Cadeira {
    @Override
    public void assentar() {
        System.out.println("[Moderna] Sentando em uma cadeira Moderna (design minimalista e limpo).");
    }
}

public class CadeiraVitoriana implements Cadeira {
    @Override
    public void assentar() {
        System.out.println("[Vitoriana] Sentando em uma cadeira Vitoriana (entalhes clássicos em madeira nobre).");
    }
}
```

```java
package moveis;

public interface Sofa {
    void deitar();
}

public class SofaModerno implements Sofa {
    @Override
    public void deitar() {
        System.out.println("[Moderna] Deitando em um sofá Moderno (linhas retas e tecido confortável).");
    }
}

public class SofaVitoriano implements Sofa {
    @Override
    public void deitar() {
        System.out.println("[Vitoriana] Deitando em um sofá Vitoriano (estofado capitonê luxuoso).");
    }
}
```

```java
package moveis;

public interface MesaDeCentro {
    void apoiar();
}

public class MesaDeCentroModerna implements MesaDeCentro {
    @Override
    public void apoiar() {
        System.out.println("[Moderna] Apoiando objetos em uma mesa de centro Moderna (vidro e metal).");
    }
}

public class MesaDeCentroVitoriana implements MesaDeCentro {
    @Override
    public void apoiar() {
        System.out.println("[Vitoriana] Apoiando objetos em uma mesa de centro Vitoriana (acabamento clássico detalhado).");
    }
}
```

---

<a id="sec-008"></a>
### A Interface e as Fábricas Concretas: `FabricaMobilia`

[↑ Voltar ao início](#inicio)
A interface `FabricaMobilia` define os métodos de criação da família, enquanto as classes concretas encapsulam a instanciação de cada conjunto estilístico.

```java
package moveis;

public interface FabricaMobilia {
    Cadeira criarCadeira();
    Sofa criarSofa();
    MesaDeCentro criarMesaDeCentro();
}
```

```java
package moveis;

public class FabricaMobiliaModerna implements FabricaMobilia {
    @Override
    public Cadeira criarCadeira() { return new CadeiraModerna(); }

    @Override
    public Sofa criarSofa() { return new SofaModerno(); }

    @Override
    public MesaDeCentro criarMesaDeCentro() { return new MesaDeCentroModerna(); }
}
```

```java
package moveis;

public class FabricaMobiliaVitoriana implements FabricaMobilia {
    @Override
    public Cadeira criarCadeira() { return new CadeiraVitoriana(); }

    @Override
    public Sofa criarSofa() { return new SofaVitoriano(); }

    @Override
    public MesaDeCentro criarMesaDeCentro() { return new MesaDeCentroVitoriana(); }
}
```

---

<a id="sec-009"></a>
### A Classe Cliente: `ConfiguradorDeSala`

[↑ Voltar ao início](#inicio)
Recebe a fábrica abstrata em seu construtor e inicializa os móveis necessários de forma totalmente desacoplada das classes concretas.

```java
package moveis;

public class ConfiguradorDeSala {
    private final Cadeira cadeira;
    private final Sofa sofa;
    private final MesaDeCentro mesaDeCentro;

    // Recebe a fábrica abstrata no construtor
    public ConfiguradorDeSala(FabricaMobilia fabrica) {
        this.cadeira = fabrica.criarCadeira();
        this.sofa = fabrica.criarSofa();
        this.mesaDeCentro = fabrica.criarMesaDeCentro();
    }

    public void exibir() {
        System.out.println("--- Configuração da Sala de Estar ---");
        cadeira.assentar();
        sofa.deitar();
        mesaDeCentro.apoiar();
    }
}
```

---

<a id="sec-010"></a>
### A Execução: `Main`

[↑ Voltar ao início](#inicio)
Demonstra a troca de famílias de objetos alterando apenas a implementação da fábrica injetada no cliente.

```java
package moveis;

public class Main {
    public static void main(String[] args) {
        System.out.println("=== 1. MONTANDO SALA COM O ESTILO MODERNO ===");
        FabricaMobilia fabricaModerna = new FabricaMobiliaModerna();
        ConfiguradorDeSala salaModerna = new ConfiguradorDeSala(fabricaModerna);
        salaModerna.exibir();

        System.out.println("\n=== 2. TROCANDO PARA O ESTILO VITORIANO (Com uma única mudança na inicialização) ===");
        FabricaMobilia fabricaVitoriana = new FabricaMobiliaVitoriana();
        ConfiguradorDeSala salaVitoriana = new ConfiguradorDeSala(fabricaVitoriana);
        salaVitoriana.exibir();
    }
}

/*
 * ========================================================================================
 * EXPLICAÇÃO: O QUE ACONTECERIA SE ALGUÉM TENTASSE MISTURAR MÓVEIS E POR QUE O ABSTRACT FACTORY EVITA ISSO
 * ========================================================================================
 * Se a criação de móveis fosse feita espalhada pelo código usando o operador 'new' diretamente
 * (ex: new CadeiraModerna(), new SofaVitoriano()), o sistema estaria vulnerável a inconsistências de design,
 * permitindo que peças de estilos completamente diferentes (como uma cadeira moderna minimalista combinada
 * com um sofá vitoriano cheio de entalhes clássicos) fossem agrupadas em um mesmo kit de sala de estar.
 *
 * O padrão Abstract Factory blinda a aplicação contra esse problema ao isolar a criação dos objetos por famílias.
 * Como a classe cliente ('ConfiguradorDeSala') interage exclusivamente com a interface 'FabricaMobilia' e nunca
 * instancia classes concretas diretamente, torna-se estruturalmente impossível misturar produtos de famílias diferentes.
 * A própria fábrica garante de forma consistente que todos os móveis gerados pertençam estritamente ao mesmo estilo selecionado.
 */
```


<a id="sec-011"></a>
## 5. Implementação nos exemplos C# deste repositório

[↑ Voltar ao início](#inicio)

[Aula07_AbstractFactory.cs](Aula07_AbstractFactory/Aula07_AbstractFactory.cs) define IHotDrink para o produto, IHotDrinkFactory para criar/preparar bebidas, TeaFactory e CoffeeFactory como factories concretas, e HotDrinkMachine como entrada usada pelo cliente. O cliente recebe IHotDrink e chama Consume() sem depender da classe concreta.

Esta primeira máquina usa AvailableDrink e um dicionário de factories. Adicionar uma bebida exige alterar o catálogo central, limitação didática relacionada ao OCP. [AbstractFactoryOCP.cs](Aula08_AbstractFactoryOCP/AbstractFactoryOCP.cs) remove o enum e descobre implementações concretas de IHotDrinkFactory no assembly, permitindo adicionar factories sem editar aquele catálogo.

Reflection torna a descoberta automática, mas não é obrigatória nem sempre a melhor escolha de produção. Registro explícito ou injeção de dependência podem tornar seleção, validação e erros mais previsíveis. O exemplo é uma versão compacta de Abstract Factory: prepara variantes de bebida; uma fábrica clássica pode criar vários produtos relacionados como uma família.


<a id="sec-012"></a>
## 6. Percurso da criação no exemplo das bebidas

[↑ Voltar ao início](#inicio)

Em [Aula07_AbstractFactory.cs](Aula07_AbstractFactory/Aula07_AbstractFactory.cs), o cliente quer uma bebida pronta, não precisa saber como ferver água ou qual classe concreta será criada. IHotDrink descreve o produto consumível. Tea e Coffee são produtos concretos. IHotDrinkFactory descreve o preparo, e TeaFactory/CoffeeFactory encapsulam as receitas. HotDrinkMachine seleciona uma fábrica e retorna IHotDrink.

O caminho completo é: usuário escolhe uma bebida → máquina localiza a fábrica → fábrica executa seu preparo e cria o produto → cliente usa Consume pelo contrato comum. A vantagem prática é que a seleção e os detalhes de construção deixam de ficar espalhados pelos clientes.

A primeira máquina usa enum e catálogo fechado; isso torna a alteração central necessária ao adicionar bebida. Em [AbstractFactoryOCP.cs](Aula08_AbstractFactoryOCP/AbstractFactoryOCP.cs), as factories são descobertas por reflection e a máquina trabalha com a coleção de IHotDrinkFactory. Essa solução demonstra extensão sem editar o enum, mas reflection traz acoplamento a nomes e descoberta em runtime. Em sistemas reais, registro explícito ou injeção de dependências costumam tornar o conjunto de produtos e os erros mais claros.

O exemplo prepara uma bebida por fábrica. Uma família clássica pode ter várias operações de criação relacionadas, como criar botão e caixa de seleção do mesmo tema; o ponto didático continua sendo manter a compatibilidade dos produtos da família.


<a id="sec-013"></a>
## Quando uma família de produtos justifica Abstract Factory

[↑ Voltar ao início](#inicio)

A necessidade principal não é esconder cada new. É garantir que produtos relacionados sejam criados de maneira compatível. Por exemplo, uma família DarkTheme pode criar Button e Checkbox que compartilham visual e comportamento; uma família WindowsTheme fornece os dois produtos correspondentes. O cliente recebe as abstrações e usa a família sem misturar produtos incompatíveis.

No exercício de bebidas, há uma interface de fábrica por bebida e um produto retornado. Isso ensina o desacoplamento, mas é uma forma reduzida do padrão: uma Abstract Factory clássica costuma oferecer vários métodos de criação de produtos relacionados. Se só há uma escolha simples e nenhum conjunto de produtos precisa permanecer coerente, uma fábrica simples costuma ser suficiente.

A máquina OCP usa reflection para descobrir factories. Isso reduz alterações no catálogo, mas transfere erros para runtime e depende de convenções de nome/construtor. Em produção, um registro explícito ou container de DI costuma facilitar diagnósticos e seleção de famílias.

<a id="sec-014"></a>
## Variações e aprofundamentos

[↑ Voltar ao início](#inicio)

As seções abaixo reúnem variações, exemplos e decisões que complementam a explicação principal deste padrão. Elas preservam a linguagem e o contexto do material didático original.

<a id="sec-015"></a>
## 27. Abstract Factory

[↑ Voltar ao início](#inicio)


Esta parte corresponde ao arquivo `CreationalPatterns/3.Factories/Aula07_AbstractFactory/Aula07_AbstractFactory.cs`.

<a id="sec-016"></a>
### 27.1 O que ela enfatiza

[↑ Voltar ao início](#inicio)

`Abstract Factory` não cria apenas um objeto isolado.

Ela cria **famílias de objetos relacionados**.

Essa é a virada conceitual mais importante desta parte do capítulo.

No sentido mais útil para iniciantes, a pergunta aqui deixa de ser:

- "como eu crio este objeto?"

e passa a ser:

- **"como eu peço uma família coerente de objetos sem me acoplar às classes concretas?"**

<a id="sec-017"></a>
### 27.2 O que o arquivo local ensina na prática

[↑ Voltar ao início](#inicio)

No arquivo `Aula07_AbstractFactory.cs`, o domínio escolhido é o de bebidas quentes.

O cliente não quer depender diretamente de:

- `Tea`;
- `Coffee`;
- `TeaFactory`;
- `CoffeeFactory`.

Ele quer pedir uma bebida por meio de abstrações:

- `IHotDrink`, do lado do produto;
- `IHotDrinkFactory`, do lado da criação.

O `HotDrinkMachine` funciona como ponto central de acesso às factories concretas. Assim, o cliente escolhe a bebida desejada, mas continua recebendo um contrato abstrato (`IHotDrink`) em vez de trabalhar acoplado a cada classe concreta.

Há, porém, um limite conceitual decisivo: cada `TeaFactory` ou `CoffeeFactory` cria somente um tipo de produto. O arquivo ilustra **fábricas polimórficas e produtos abstratos**, mas não realiza por completo a intenção canônica de criar uma família com vários produtos relacionados. O exemplo de `IGuiFactory`, apresentado adiante, é a representação fiel de `Abstract Factory`.

#### 27.2.1 Limite didático importante deste arquivo

Apesar de ensinar bem a ideia de abstração, o arquivo `Aula07_AbstractFactory.cs` deixa uma violação intencional de `OCP` para ser comparada com a correção da aula seguinte.

O problema não está em `IHotDrink` nem em `IHotDrinkFactory`.

O problema está em concentrar as opções num `enum` (`AvailableDrink`) e fazer a `HotDrinkMachine` depender desse catálogo fechado.

Na prática, para adicionar `Chocolate`, não bastaria criar `Chocolate` e `ChocolateFactory`.

Também seria preciso:

- editar o `enum`;
- reabrir a `HotDrinkMachine`;
- alterar código já testado só para cadastrar a nova bebida.

Isso é exatamente o tipo de mudança que o `Open-Closed Principle` tenta evitar.

A versão corrigida dessa ideia aparece no arquivo `CreationalPatterns/3.Factories/Aula08_AbstractFactoryOCP/AbstractFactoryOCP.cs`.

<a id="sec-018"></a>
### 27.3 Quem é quem no exemplo

[↑ Voltar ao início](#inicio)

Mapeando os participantes do padrão no arquivo local:

- `IHotDrink` funciona como **Product Abstraction**;
- `Tea` e `Coffee` funcionam como **Concrete Products**;
- `IHotDrinkFactory` funciona como uma **abstração de fábrica**;
- `TeaFactory` e `CoffeeFactory` funcionam como **fábricas concretas de um único produto**;
- `HotDrinkMachine` funciona como uma camada de orquestração e seleção das factories;
- `Program` funciona como **Client**.

Essa leitura ajuda a evitar duas confusões comuns:

- achar que `Abstract Factory` é só "uma factory com interface";
- achar que o centro do exemplo é a reflexão do `Activator`.

Não é isso.

O centro da aula é este:

- o cliente depende de contratos;
- a criação concreta fica atrás das factories;
- e a escolha da variante não obriga o cliente a trabalhar diretamente com as classes concretas.

<a id="sec-019"></a>
### 27.4 Por que `IHotDrink` e não `Tea` ou `Coffee`?

[↑ Voltar ao início](#inicio)

Esse ponto merece atenção especial porque ele ensina uma ideia muito importante para a carreira profissional:

**o cliente quase nunca deveria depender de mais detalhe do que realmente precisa.**

No exemplo da máquina, o cliente quer uma bebida quente que possa ser consumida.

Ele não precisa necessariamente saber:

- se a classe concreta é `Tea`;
- se a classe concreta é `Coffee`;
- como aquela bebida foi preparada por dentro;
- qual factory concreta foi usada.

Por isso, o método da máquina devolve o tipo:

```csharp
IHotDrink
```

e não:

```csharp
Tea
```

ou

```csharp
Coffee
```

#### 27.4.1 O ganho prático dessa decisão

Quando o retorno é `IHotDrink`, vários ganhos aparecem ao mesmo tempo.

##### 1. O método pode ter um retorno estável

`MakeDrink(...)` sempre devolve o mesmo contrato externo:

- hoje pode nascer `Tea`;
- amanhã pode nascer `Coffee`;
- depois pode nascer `Chocolate`;
- mas o cliente continua lidando com o tipo `IHotDrink`.

Se o método tivesse que devolver `Tea` ou `Coffee` diretamente, a API ficaria muito mais rígida. Em algum momento, você acabaria caindo em alternativas piores, como:

- vários métodos concretos diferentes;
- retorno como `object`;
- `if` e cast no cliente;
- ou acoplamento direto a tipos concretos.

##### 2. O cliente depende do que importa, não do detalhe interno

Aqui, o que importa para o cliente é:

- existe um método `Consume()`;
- a bebida está pronta para uso.

O nome concreto da classe é detalhe de implementação.

Essa é uma aplicação muito direta da ideia de abstração descrita nas docs da Microsoft: interfaces definem um contrato comum que pode ser implementado por múltiplos tipos.

##### 3. O polimorfismo entra de forma natural

Com `IHotDrink`, o cliente pode escrever:

```csharp
IHotDrink drink = machine.MakeDrink(...);
drink.Consume();
```

sem precisar tratar `Tea` e `Coffee` separadamente.

Isso conversa diretamente com a ideia de polimorfismo da documentação oficial: objetos diferentes podem ser usados por meio do mesmo contrato quando o cliente depende da abstração correta.

##### 4. A evolução do sistema fica menos dolorosa

Se uma nova bebida entrar depois, a parte cliente que só quer consumir uma bebida quente tende a sofrer menos.

Isso não elimina toda mudança do sistema, mas reduz o acoplamento onde ele não precisa existir.

##### 5. Testes e substituições ficam mais simples

Quando o cliente depende de `IHotDrink`, fica mais fácil:

- usar um fake;
- usar um mock;
- criar uma implementação temporária para teste;
- trocar a implementação concreta sem reescrever todo consumidor.

#### 27.4.2 O que daria errado se o cliente dependesse de `Tea` e `Coffee`

Se o código cliente fosse escrito em cima de tipos concretos, alguns problemas começariam a aparecer:

- o cliente precisaria saber mais sobre as classes concretas;
- a máquina deixaria de esconder tão bem a criação;
- qualquer nova variante tenderia a vazar para mais lugares do sistema;
- o cliente poderia começar a acumular `if`, cast e ramificações por tipo concreto.

Em linguagem bem direta:

- com `IHotDrink`, o cliente pensa em **capacidade**;
- com `Tea` e `Coffee`, o cliente pensa em **classe concreta**.

Para design profissional, quase sempre é melhor depender da capacidade quando ela já resolve o problema.

#### 27.4.3 A nuance importante

Isso não quer dizer que abstração deva ser usada sem critério.

Se o cliente realmente precisa de algo específico de `Tea` que não existe em `IHotDrink`, então existem duas opções honestas:

- o contrato `IHotDrink` talvez esteja pobre demais para o caso real;
- ou aquele cliente específico realmente precisa conhecer o tipo concreto.

O ponto não é "sempre usar interface por religião".

O ponto é:

- use a abstração quando ela cobre o que o cliente realmente precisa;
- não exponha a classe concreta quando esse detalhe não agrega valor ao consumidor.

<a id="sec-020"></a>
### 27.5 Exemplo mental clássico

[↑ Voltar ao início](#inicio)

Imagine uma aplicação que precisa trabalhar com famílias consistentes, como:

- `WindowsButton` + `WindowsCheckbox`;
- `MacButton` + `MacCheckbox`.

O cliente não quer escolher cada classe concreta manualmente. Ele quer pedir uma família coerente.

<a id="sec-021"></a>
### 27.6 Estrutura mental

[↑ Voltar ao início](#inicio)

Em geral aparece algo assim:

- uma interface de fábrica abstrata;
- várias fábricas concretas;
- vários produtos abstratos;
- várias implementações concretas desses produtos.

Leitura correta:

- o cliente depende de abstrações;
- a fábrica concreta decide qual família inteira será entregue.

<a id="sec-022"></a>
### 27.7 A nuance importante do exemplo atual

[↑ Voltar ao início](#inicio)

Vale uma honestidade técnica aqui: o exemplo local de bebidas quentes é uma versão **simplificada** da ideia clássica.

No exemplo mais tradicional de `Abstract Factory`, uma mesma factory concreta costuma criar **vários produtos relacionados** da mesma família, por exemplo:

- `CreateButton()`;
- `CreateCheckbox()`;
- `CreateTextbox()`.

O ponto decisivo aqui não é apenas "criar três coisas".

O ponto decisivo é este:

- esses três produtos pertencem à **mesma família visual ou tecnológica**;
- eles foram pensados para funcionar juntos;
- e a factory concreta garante que o conjunto inteiro venha da mesma linha de compatibilidade.

Uma imagem mental boa é esta:

```csharp
public interface IGuiFactory
{
    IButton CreateButton();
    ICheckbox CreateCheckbox();
    ITextbox CreateTextbox();
}
```

Depois aparecem factories concretas como:

- `WindowsGuiFactory`;
- `MacGuiFactory`.

E produtos concretos como:

- `WindowsButton`, `WindowsCheckbox`, `WindowsTextbox`;
- `MacButton`, `MacCheckbox`, `MacTextbox`.

O ganho fica muito mais claro quando você pensa no que a factory evita.

Sem `Abstract Factory`, o cliente poderia acabar fazendo misturas como:

- `new WindowsButton()`;
- `new MacCheckbox()`;
- `new WindowsTextbox()`.

Esse tipo de mistura pode parecer pequeno no exemplo, mas em um sistema real costuma significar:

- inconsistência visual;
- comportamento desigual entre componentes;
- aumento de `if` para tratar combinações;
- acoplamento do cliente a muitas classes concretas.

Com `Abstract Factory`, a conversa muda para algo como:

```csharp
var factory = new WindowsGuiFactory();
var button = factory.CreateButton();
var checkbox = factory.CreateCheckbox();
var textbox = factory.CreateTextbox();
```

Aqui o cliente não decide mais cada classe concreta isoladamente.
Ele decide apenas a **família**.
Depois disso, a própria factory garante que o restante do conjunto seja coerente.

Essa é a diferença mais importante em relação a uma factory comum:

- uma factory comum muitas vezes responde "qual objeto eu crio agora?";
- a `Abstract Factory` responde "qual conjunto coerente de objetos eu entrego a partir desta família?".

No arquivo `Aula07_AbstractFactory.cs`, cada factory concreta cria apenas **um tipo principal de produto** (`IHotDrink`), variando entre `Tea` e `Coffee`. Por isso, o exemplo se aproxima mais de fábricas polimórficas de um produto do que de uma implementação completa de `Abstract Factory`.

Mesmo assim, a aula continua útil porque deixa visíveis elementos preparatórios:

- existe uma abstração de criação;
- existem factories concretas por variante;
- o cliente conversa com contratos, e não com as classes concretas.

Para caracterizar o padrão canônico, falta uma fábrica concreta criar **mais de um produto relacionado** e garantir a compatibilidade da família inteira.

<a id="sec-023"></a>
### 27.8 O ganho principal

[↑ Voltar ao início](#inicio)

O grande ganho aqui não é apenas "tirar um `new`".

O ganho é garantir compatibilidade entre objetos que devem nascer juntos como um conjunto coerente.

Quando o exemplo cresce para famílias maiores, esse ganho fica ainda mais claro:

- uma `WindowsFactory` devolve componentes compatíveis entre si;
- uma `MacFactory` devolve outra família coerente;
- o cliente continua dependendo apenas das abstrações.

---

<a id="sec-024"></a>
## 28. Abstract Factory and OCP

[↑ Voltar ao início](#inicio)


Esta parte corresponde ao arquivo `CreationalPatterns/3.Factories/Aula08_AbstractFactoryOCP/AbstractFactoryOCP.cs`.

<a id="sec-025"></a>
### 28.1 O que exatamente estava errado em Aula07

[↑ Voltar ao início](#inicio)

Em `Aula07_AbstractFactory.cs`, a quebra de `OCP` não acontece simplesmente porque existem classes concretas.

Ela acontece porque a máquina central precisa ser modificada sempre que o sistema ganha uma nova bebida.

O ponto mais claro disso é o `enum AvailableDrink`.

Quando o sistema usa um enum como catálogo central fechado, adicionar um novo item exige reabrir código existente.

Trecho representativo da versão problemática:

```csharp
public enum AvailableDrink
{
    Coffee, Tea
}

public IHotDrink MakeDrink(AvailableDrink drink, int amount)
{
    return factories[drink].Prepare(amount);
}
```

Leitura direta desse trecho:

- a máquina depende de um conjunto fechado de opções;
- o cliente escolhe por esse catálogo central;
- se surgir uma nova bebida, esse ponto precisa ser reaberto.

No exemplo, para entrar `Chocolate`, você teria que:

- criar `Chocolate`;
- criar `ChocolateFactory`;
- editar o `enum AvailableDrink`;
- manter a `HotDrinkMachine` alinhada com essa nova opção.

Ou seja: a extensão não para na criação de novas classes. Ela obriga manutenção dentro da classe que deveria apenas orquestrar.

<a id="sec-026"></a>
### 28.2 O que precisou ser corrigido em Aula08

[↑ Voltar ao início](#inicio)

O arquivo `AbstractFactoryOCP.cs` ataca exatamente esse ponto.

A transição principal foi esta:

- sair de um catálogo fechado por `enum`;
- ir para um catálogo aberto baseado em classes que implementam `IHotDrinkFactory`.

Em termos práticos, a regra mudou de:

- "para existir uma nova bebida, primeiro edite o enum"

para:

- "para existir uma nova bebida, crie uma nova factory concreta que implemente o contrato esperado"

Essa mudança é o coração da correção de `OCP`.

Uma versão defensiva do trecho central — complementando o arquivo local com filtros para tipos abstratos e para o construtor exigido — fica assim:

```csharp
foreach (var type in typeof(HotDrinkMachine).Assembly.GetTypes())
{
    if (typeof(IHotDrinkFactory).IsAssignableFrom(type)
        && !type.IsInterface
        && !type.IsAbstract
        && type.GetConstructor(Type.EmptyTypes) is not null)
    {
        factories.Add(Tuple.Create(
            type.Name.Replace("Factory", string.Empty),
            (IHotDrinkFactory)Activator.CreateInstance(type)));
    }
}
```

Aqui a pergunta deixa de ser:

- "quais opções eu listei no enum?"

e passa a ser:

- "quais classes concretas implementam `IHotDrinkFactory` neste assembly?"

<a id="sec-027"></a>
### 28.3 Entendendo a transição do enum para extensão por factory

[↑ Voltar ao início](#inicio)

Na versão antiga, a máquina sabia antecipadamente quais bebidas existiam.

Na versão nova, a máquina passa a descobrir quais factories concretas estão disponíveis no assembly.

Isso muda o eixo da evolução:

- antes, a lista de opções morava dentro da máquina;
- agora, a lista de opções nasce das classes concretas que respeitam `IHotDrinkFactory`.

Por isso, se você adicionar `ChocolateFactory`, a ideia da aula é que a máquina possa incorporá-la sem ganhar novo `enum`, novo `if` ou novo `switch`.

Esse é o critério mais importante aqui:

- a classe de orquestração cresce por extensão das implementações ao redor;
- ela deixa de depender de reabertura constante do próprio código.

Em forma de contraste, a transição é esta:

```csharp
// Aula07: catálogo fechado
public enum AvailableDrink
{
    Coffee, Tea
}
```

```csharp
// Aula08: catálogo aberto por implementações
if (typeof(IHotDrinkFactory).IsAssignableFrom(type) && !type.IsInterface)
{
    factories.Add(...);
}
```

<a id="sec-028"></a>
### 28.4 O que cada trecho menos óbvio está fazendo

[↑ Voltar ao início](#inicio)

No `HotDrinkMachine` da Aula08, alguns trechos costumam soar estranhos para iniciantes. A versão abaixo repete as duas proteções adicionais mostradas na seção 28.2; o trecho literal do arquivo, exibido na seção 28.3, filtra apenas interfaces.

Trecho comentado com as proteções adicionais:

```csharp
foreach (var type in typeof(HotDrinkMachine).Assembly.GetTypes())
{
    if (typeof(IHotDrinkFactory).IsAssignableFrom(type)
        && !type.IsInterface
        && !type.IsAbstract
        && type.GetConstructor(Type.EmptyTypes) is not null)
    {
        factories.Add(Tuple.Create(
            type.Name.Replace("Factory", string.Empty),
            (IHotDrinkFactory)Activator.CreateInstance(type)));
    }
}
```

- `typeof(HotDrinkMachine).Assembly.GetTypes()`: percorre os tipos compilados no mesmo assembly da aula;
- `typeof(IHotDrinkFactory).IsAssignableFrom(tipo)`: verifica se aquele tipo implementa `IHotDrinkFactory`;
- `!tipo.IsInterface` e `!tipo.IsAbstract`: evitam tentar instanciar tipos que não podem produzir objetos;
- `GetConstructor(Type.EmptyTypes)`: confirma a existência de construtor público sem parâmetros;
- `Activator.CreateInstance(tipo)`: cria uma instância da factory encontrada em tempo de execução;
- `Tuple<string, IHotDrinkFactory>`: guarda juntos o nome exibido ao usuário e a factory concreta correspondente.

A ideia dessas linhas não é transformar reflection no assunto principal.

A ideia é mostrar um jeito de remover da máquina a obrigação de ser editada a cada nova bebida.

Também vale olhar o novo formato da operação principal:

```csharp
public IHotDrink MakeDrink()
{
    WriteLine("Available drinks:");

    for (var index = 0; index < factories.Count; ++index)
    {
        var tuple = factories[index];
        WriteLine($"{index}: {tuple.Item1}");
    }

    return factories[i].Item2.Prepare(amount);
}
```

O ponto importante aqui não é a interação de console em si.

O ponto importante é que:

- a máquina não recebe mais um `AvailableDrink` fixo;
- ela trabalha sobre a coleção de factories que encontrou;
- a criação concreta continua delegada para `IHotDrinkFactory`.

<a id="sec-029"></a>
### 28.5 Por que a Aula08 é mais extensível neste cenário

[↑ Voltar ao início](#inicio)

`Aula07` ainda é útil para aprender:

- produto abstrato;
- factory abstrata;
- factories concretas;
- retorno por interface (`IHotDrink`).

Mas, do ponto de vista de `OCP`, ele foi deixado incompleto de propósito.

`Aula08` melhora justamente esse eixo: a máquina deixa de depender de um conjunto fechado definido por `enum` e passa a trabalhar em cima de factories que seguem o mesmo contrato.

Então a comparação cirúrgica é esta:

- `Aula07`: para adicionar bebida nova, você reabre a máquina;
- `Aula08`: para adicionar bebida nova, você tende a adicionar nova factory.

<a id="sec-030"></a>
### 28.6 Honestidade técnica importante

[↑ Voltar ao início](#inicio)

A solução da aula usa reflection para descobrir factories automaticamente.

Isso ajuda muito a visualizar `OCP`, mas reflection não é a única forma correta de modelar essa extensão.

O código original filtra apenas interfaces. Uma implementação robusta também precisa rejeitar classes abstratas, verificar se existe um construtor acessível compatível e tratar falhas de carregamento ou instanciação. Descoberta por convenção ainda pode incluir tipos não desejados e não garante uma ordem estável de apresentação; esses aspectos precisam de uma política explícita em produção.

Em código profissional, também é comum usar:

- injeção de dependências;
- registro explícito de factories;
- composição em um bootstrapper.

O ponto realmente essencial não é "usar reflection".

O ponto essencial é este:

- evitar que a classe central precise ser editada sempre que uma nova variante aparece.

<a id="sec-031"></a>
### 28.7 O que é Reflection neste contexto

[↑ Voltar ao início](#inicio)

`Reflection` é a capacidade de o programa inspecionar tipos em tempo de execução.

Em linguagem bem direta:

- o código consegue perguntar "quais classes existem aqui?";
- "essa classe implementa tal interface?";
- "consigo criar uma instância desse tipo agora?".

É exatamente isso que aparece em `AbstractFactoryOCP.cs` quando a máquina percorre os tipos do assembly e procura implementações de `IHotDrinkFactory`.

No exemplo, a sequência mental é:

- descobrir os tipos carregados;
- filtrar os que implementam `IHotDrinkFactory`;
- criar instâncias dessas factories;
- montar a lista de opções sem depender de um `enum` fixo.

Por isso a reflection funciona muito bem como ferramenta didática aqui: ela deixa visível, de forma quase "manual", como a máquina passa de um catálogo fechado para um catálogo aberto por extensão.

<a id="sec-032"></a>
### 28.8 O que é Dependency Injection

[↑ Voltar ao início](#inicio)

`Dependency Injection` (`DI`) é uma forma de entregar a uma classe as dependências de que ela precisa, em vez de obrigá-la a criar tudo sozinha com `new`.

No caso desta aula, uma dependência da `HotDrinkMachine` poderia ser justamente a coleção de factories disponíveis.

Em vez de a máquina:

- varrer assembly;
- descobrir tipos sozinha;
- instanciar cada factory por reflection;

ela poderia simplesmente receber algo como:

```csharp
IEnumerable<IHotDrinkFactory>
```

e trabalhar em cima disso.

Essa é a ideia central de DI:

- a classe declara do que precisa;
- outra parte do sistema fornece isso para ela.

<a id="sec-033"></a>
### 28.9 DI não é a mesma coisa que container

[↑ Voltar ao início](#inicio)

Esse ponto é importante para não misturar os conceitos.

- `Dependency Injection` é uma técnica/padrão arquitetural de fornecimento de dependências, mas não faz parte dos 23 padrões do catálogo original do GoF;
- o **DI container** é a ferramenta que automatiza registro, criação e entrega dessas dependências.

Em .NET profissional, é muito comum registrar serviços em um container e depois deixar o framework injetar tudo por construtor.

Então, quando o instrutor diz que "tipicamente se usaria um dependency injection container", ele está falando de usar uma infraestrutura mais comum de aplicação real para entregar as factories à máquina.

Nuance importante:

- `DI` e `reflection` não são inimigos obrigatórios;
- um container pode até usar reflection internamente;
- mas, para o código da aplicação, a experiência continua sendo "recebo dependências prontas", e não "descubro tipos manualmente".

<a id="sec-034"></a>
### 28.10 Por que o instrutor usou Reflection na aula

[↑ Voltar ao início](#inicio)

Porque reflection resolve o problema e, ao mesmo tempo, expõe melhor o mecanismo.

Se a aula pulasse direto para container, um iniciante talvez visse só a mágica:

- registrou no container;
- injetou no construtor;
- funcionou.

Mas ficaria menos claro **o que a máquina realmente precisa** e **como novas factories entram no sistema**.

Com reflection, o aluno enxerga explicitamente:

- de onde as factories estão vindo;
- como elas são localizadas;
- como a máquina monta seu catálogo;
- por que o `enum` deixou de ser necessário.

Então a escolha da aula é pedagógica, não necessariamente a mais comum para produção.

<a id="sec-035"></a>
### 28.11 Quando Reflection e DI costumam ser preferidos

[↑ Voltar ao início](#inicio)

Use a leitura mental abaixo:

- `Reflection`: boa quando você quer descoberta dinâmica de tipos, plugins, metadados, atributos ou um mecanismo didático/infraestrutural que trabalha com tipos desconhecidos em compile-time.
- `Dependency Injection`: boa quando as dependências do sistema já são conhecidas pela composição da aplicação e você quer código mais explícito, testável, previsível e fácil de manter.

Em aplicações profissionais, `DI container` costuma ser mais comum para montar objetos de negócio e serviços.

Já reflection costuma aparecer mais em:

- frameworks;
- bibliotecas;
- plugin systems;
- serialização;
- ORMs;
- tooling;
- infraestrutura genérica.

<a id="sec-036"></a>
### 28.12 Comparação curta e cirúrgica

[↑ Voltar ao início](#inicio)

Para este exemplo da aula:

- com `reflection`, a máquina descobre sozinha quais factories existem;
- com `DI container`, a máquina receberia de fora as factories já registradas.

Ou seja:

- `reflection` enfatiza **descoberta dinâmica**;
- `dependency injection` enfatiza **fornecimento explícito de dependências**.

As duas abordagens podem respeitar `OCP`.

O critério decisivo não é "usar reflection" ou "usar DI" por si só.

O critério decisivo é:

- evitar que a `HotDrinkMachine` precise ser alterada sempre que uma nova factory concreta for adicionada.

---

<a id="sec-037"></a>
## Coding Exercise 2: Factory Coding Exercise

[↑ Voltar ao início](#inicio)


Esta parte corresponde ao arquivo `CreationalPatterns/3.Factories/Aula09_FactoryExcercise/factoryExcercise.cs`.

Este exercício é importante porque pega a ideia central do capítulo de `Factories` e transforma em uma implementação pequena, concreta e fácil de inspecionar.

<a id="sec-038"></a>
### O que o exercício quer de verdade

[↑ Voltar ao início](#inicio)

O enunciado não está pedindo uma `Abstract Factory`, nem uma fábrica genérica capaz de criar qualquer tipo de objeto.

Ele está pedindo algo mais específico:

- criar objetos `Person`;
- receber apenas o nome da pessoa;
- atribuir o `Id` automaticamente;
- manter uma sequência de `Id` começando em zero;
- centralizar essa regra dentro de uma `PersonFactory` não estática.

Em outras palavras, o aluno não precisa resolver "criação de objetos" em geral. O aluno precisa resolver:

**"como encapsular a criação de `Person` em uma factory que controla automaticamente o próximo `Id`?"**

<a id="sec-039"></a>
### Como ler o uso esperado

[↑ Voltar ao início](#inicio)

O uso esperado é este:

```csharp
var factory = new PersonFactory();

var person1 = factory.CreatePerson("Alice");
var person2 = factory.CreatePerson("Bob");
var person3 = factory.CreatePerson("Charlie");
```

Essa leitura pode ser feita em etapas:

1. `new PersonFactory()` cria uma fábrica concreta com seu próprio contador interno.
2. `CreatePerson("Alice")` cria a primeira pessoa e atribui `Id = 0`.
3. `CreatePerson("Bob")` cria a segunda pessoa e atribui `Id = 1`.
4. `CreatePerson("Charlie")` cria a terceira pessoa e atribui `Id = 2`.

Ou seja, o cliente não está controlando manualmente:

- qual é o próximo `Id`;
- quando o contador deve aumentar;
- como o objeto `Person` deve ser inicializado;
- qual construtor deve ser chamado.

Esse trabalho foi centralizado na factory.

<a id="sec-040"></a>
### O que é o produto neste exercício

[↑ Voltar ao início](#inicio)

No exemplo atual, o produto é direto:

- **produto criado:** o objeto `Person`;
- **dados do produto:** `Id` e `Name`;
- **regra de criação:** o `Name` vem do cliente, mas o `Id` vem da factory.

Isso ajuda a enxergar a diferença entre dado informado e dado calculado.

O cliente sabe o nome da pessoa que quer criar. Mas o cliente não deveria precisar saber qual é o próximo `Id` disponível. Essa decisão pertence à factory.

<a id="sec-041"></a>
### Participantes do padrão no exercício

[↑ Voltar ao início](#inicio)

Mapeando o exercício para a linguagem da família `Factory`:

- `Person`: é o **Product**, ou seja, o objeto criado.
- `PersonFactory`: é a **Factory**, ou seja, a classe responsável por criar `Person`.
- `CreatePerson(...)`: é o **método de criação da factory**; ele não constitui o `Factory Method` canônico do GoF porque não é redefinido por subclasses de um `Creator`.
- `counter`: é o estado interno que permite gerar `Id` sequencial.
- `Demo`/cliente: é quem pede a criação e usa os objetos criados.

Repare que este exercício não usa interface, herança ou famílias de produtos. Isso é proposital: ele isola a ideia mais básica de factory antes de complicar o desenho.

<a id="sec-042"></a>
### O que faz este exercício ser uma factory

[↑ Voltar ao início](#inicio)

Este ponto vale atenção porque, à primeira vista, alguém pode pensar:

**"isso não é só trocar `new Person(...)` por `CreatePerson(...)`?"**

Não. A troca só vale a pena porque existe uma regra de criação sendo protegida.

O que faz a solução ser uma **Factory** é a estrutura da responsabilidade:

- existe um objeto especializado em criação;
- ele guarda o estado necessário para criar corretamente;
- ele expõe um método com nome claro para o cliente;
- o cliente informa apenas o que ele realmente sabe;
- a factory completa o restante da criação.

Se o cliente precisasse escrever diretamente:

- o próximo `Id`;
- o incremento do contador;
- o `new Person(...)`;
- a regra de sequência;

então estaríamos mais perto de criação manual do que de factory.

<a id="sec-043"></a>
### Onde o controle de estado aparece

[↑ Voltar ao início](#inicio)

Este exercício também mostra uma diferença importante entre uma factory sem estado e uma factory com estado.

Aqui, a `PersonFactory` precisa lembrar quantas pessoas já criou:

```csharp
private int counter = 0;
```

Esse contador começa em zero porque o enunciado pede uma sequência **0-based**.

Depois, a criação usa `counter++`:

```csharp
return new Person(counter++, name);
```

Esse operador é um pós-incremento:

- primeiro usa o valor atual de `counter`;
- depois incrementa `counter` para a próxima criação.

Por isso a primeira pessoa recebe `Id = 0`. Se a solução usasse `++counter`, a primeira pessoa receberia `Id = 1`, quebrando o requisito do exercício.

<a id="sec-044"></a>
### Por que a factory não é estática

[↑ Voltar ao início](#inicio)

O enunciado pede explicitamente uma `PersonFactory` não estática.

Isso é importante porque o contador pertence à instância da factory:

```csharp
var factory = new PersonFactory();
```

Cada factory criada tem sua própria sequência. Isso torna o exemplo mais controlado, mais testável e mais fácil de entender.

Se tudo fosse estático, o contador tenderia a virar estado global. Para um exercício didático, isso esconderia uma ideia importante: a factory pode ser um objeto com memória própria.

<a id="sec-045"></a>
### Leitura guiada da solução

[↑ Voltar ao início](#inicio)

A classe `Person` representa o produto:

```csharp
public class Person
{
    public int Id { get; set; }
    public string Name { get; set; }

    internal Person(int id, string name)
    {
        Id = id;
        Name = name;
    }
}
```

`Person` tem duas propriedades:

- `Id`, que identifica a pessoa;
- `Name`, que guarda o nome informado pelo cliente.

O construtor recebe os dois valores prontos. Na solução do arquivo, ele aparece como `internal`, reforçando que o caminho preferido de criação é a factory dentro do mesmo assembly.

Depois vem a factory:

```csharp
public class PersonFactory
{
    private int counter = 0;

    public Person CreatePerson(string name)
    {
        return new Person(counter++, name);
    }
}
```

`PersonFactory` tem uma responsabilidade muito clara: criar `Person` corretamente.

Ela guarda o contador em `counter`, recebe o nome em `CreatePerson(...)` e constrói a pessoa com o próximo `Id`.

Por fim, o cliente usa a factory:

```csharp
var factory = new PersonFactory();
var person1 = factory.CreatePerson("Alice");
var person2 = factory.CreatePerson("Bob");
var person3 = factory.CreatePerson("Charlie");
```

A saída esperada conceitualmente é:

| Pessoa | Nome | Id esperado |
| --- | --- | ---: |
| `person1` | `Alice` | `0` |
| `person2` | `Bob` | `1` |
| `person3` | `Charlie` | `2` |

<a id="sec-046"></a>
### O que o aluno precisa enxergar para conseguir reproduzir sozinho

[↑ Voltar ao início](#inicio)

Se você quisesse construir outra factory parecida depois, o raciocínio seria este:

1. descubra **qual é o produto** que será criado;
2. descubra **quais dados o cliente deve informar**;
3. descubra **quais dados a factory deve calcular ou controlar**;
4. crie uma factory que **guarde o estado necessário para a criação**;
5. exponha um método de criação com **nome claro**;
6. faça o cliente depender da factory, não dos detalhes de instanciação.

No exercício atual:

1. o produto é `Person`;
2. o cliente informa apenas o `name`;
3. a factory calcula o `Id`;
4. a factory guarda o contador em `counter`;
5. `CreatePerson(...)` é o método de criação;
6. o cliente chama `factory.CreatePerson(...)` sem informar `Id`.

<a id="sec-047"></a>
### O que costuma confundir aqui

[↑ Voltar ao início](#inicio)

As confusões mais comuns nesta aula costumam ser estas:

- achar que toda factory precisa ser estática;
- achar que toda factory precisa criar vários tipos diferentes;
- esquecer que o `Id` começa em `0`, não em `1`;
- usar `++counter` em vez de `counter++`;
- colocar o contador no cliente, espalhando a regra de criação;
- não perceber que cada instância de `PersonFactory` tem sua própria sequência.

<a id="sec-048"></a>
### Como validar mentalmente se a solução está correta

[↑ Voltar ao início](#inicio)

Uma boa solução para este exercício deve satisfazer estas perguntas:

- o cliente consegue criar pessoas informando apenas o nome?
- a primeira pessoa criada recebe `Id = 0`?
- o `Id` aumenta a cada chamada de `CreatePerson(...)`?
- a sequência fica guardada dentro da factory?
- a factory é uma classe instanciável, e não apenas um método estático?

Se essas respostas forem "sim", então o exercício cumpriu seu objetivo pedagógico.

---


<a id="sec-glossario"></a>
## Glossário
[↑ Voltar ao início](#inicio)

- **Família de produtos:** objetos relacionados que devem funcionar juntos.
- **Abstract Factory:** contrato que cria produtos de uma família sem expor classes concretas.
- **Fábrica concreta:** implementação que seleciona uma família de produtos.
- **Compatibilidade:** garantia de que produtos combinados formam uma configuração válida.
