<a id="inicio"></a>
## Sumário
[↑ Voltar ao início](#inicio)

- [1. O que o Composite representa?](#sec-001)
  - [O Problema que ele resolve:](#sec-002)
- [2. Em que situações se usa?](#sec-003)
- [3. Instruções passo a passo de como implementar (em Java)](#sec-004)
- [4. Explicação detalhada do Exemplo Prático (Cardápio e Combos de Restaurante)](#sec-005)
  - [A Interface Componente: ItemMenu](#sec-006)
  - [A Classe Folha (Leaf): Prato](#sec-007)
  - [A Classe Composta (Composite): Combo](#sec-008)
  - [A Execução: Main](#sec-009)
- [5. Implementação real do repositório: valores simples e agrupados](#sec-010)
- [Outros exemplos de Composite na pasta](#sec-011)
- [6. Por que a soma não pergunta o tipo de cada item](#sec-012)
- [Como reconhecer um Composite real](#sec-013)
- [Variações e aprofundamentos](#sec-014)
- [55. Geometric Shapes](#sec-015)
  - [55.1 O problema do exemplo](#sec-016)
  - [55.2 Estrutura do código](#sec-017)
  - [55.3 Participantes do padrão neste arquivo](#sec-018)
  - [55.4 Como a árvore é montada](#sec-019)
  - [55.5 Como a impressão recursiva funciona](#sec-020)
  - [55.6 Variante transparente vs. variante segura](#sec-021)
  - [55.7 O papel de Lazy<List<GraphicObject>>](#sec-022)
  - [55.8 O que este exemplo ensina](#sec-023)
- [56. Neural Networks](#sec-024)
  - [56.1 A ideia central: tudo vira sequência de neurônios](#sec-025)
  - [56.2 O método ConnectTo](#sec-026)
  - [56.3 Simulação visual do foreach duplo](#sec-027)
  - [56.4 Como isso aparece nas chamadas do Main](#sec-028)
- [57. Composite Specification](#sec-029)
  - [57.1 O modelo usado na aula](#sec-030)
  - [57.2 O contrato ISpecification<T>](#sec-031)
  - [57.3 As specifications simples são folhas](#sec-032)
  - [57.4 Onde o Composite aparece](#sec-033)
  - [57.5 O operador &](#sec-034)
  - [57.6 Como o filtro consome a regra composta](#sec-035)
  - [57.7 Simulação visual da avaliação](#sec-036)
  - [57.8 O que este exemplo ensina](#sec-037)
- [Coding Exercise 7: Composite Coding Exercise](#sec-038)
  - [O que o exercício quer de verdade](#sec-039)
  - [A folha: SingleValue](#sec-040)
  - [Por que existem dois GetEnumerator()](#sec-041)
  - [IEnumerable<int> vs. ICollection<T>](#sec-042)
  - [O composto: ManyValues](#sec-043)
  - [Como Sum() usa o contrato comum](#sec-044)
  - [O que costuma confundir aqui](#sec-045)
  - [Como validar mentalmente se a solução está correta](#sec-046)



**Família GoF:** estrutural. **Escopo predominante:** objetos organizados em hierarquias parte-todo.
<a id="sec-001"></a>
## 1. O que o Composite representa?

[↑ Voltar ao início](#inicio)

O **Composite** é um padrão de projeto estrutural que permite compor objetos em **estruturas de árvore** para representar hierarquias do tipo parte-todo. A ideia central é que o código cliente trate objetos individuais (folhas) e composições de objetos (compostos) de maneira uniforme, utilizando exatamente a mesma interface.

<a id="sec-002"></a>
### O Problema que ele resolve:

[↑ Voltar ao início](#inicio)

Em sistemas com estruturas recursivas (como sistemas de arquivos, menus ou cardápios de restaurantes), tratar cada tipo de nó de forma diferente gera graves problemas:

* O código cliente precisa usar checagens de tipos (`instanceof`) para distinguir elementos simples de recipientes.
* A lógica de percorrimento e cálculo recursivo fica espalhada e duplicada pelo cliente.
* Listas genéricas sem tipagem segura (como `List<Object>`) tornam o código vulnerável a erros.
* Adicionar novos tipos de nós exige alterar múltiplos pontos do sistema.

---

<a id="sec-003"></a>
## 2. Em que situações se usa?

[↑ Voltar ao início](#inicio)

* Há uma estrutura recursiva do tipo parte-todo (itens que contêm outros itens).
* O cliente precisa tratar itens individuais e grupos de itens exatamente da mesma forma (uniformidade).
* Você quer processar hierarquias de forma genérica e recursiva.
* Deseja adicionar novos tipos de nós sem alterar o código que percorre a estrutura.

---

<a id="sec-004"></a>
## 3. Instruções passo a passo de como implementar (em Java)

[↑ Voltar ao início](#inicio)

1. **Identifique a operação comum** que faz sentido para todos os nós da árvore (ex: obter preço, calcular tamanho).
2. **Defina a interface `Component**` contendo essa operação.
3. **Implemente a folha (`Leaf`)**, que representa o nó final e resolve a operação de forma isolada.
4. **Implemente o composto (`Composite`)**, que armazena uma lista de filhos do tipo `Component` e delega ou combina os resultados recursivamente.
5. **Decida onde gerenciar os filhos** (métodos como adicionar/remover): na interface (abordagem transparente) ou apenas no composto (abordagem segura).

---

<a id="sec-005"></a>
## 4. Explicação detalhada do Exemplo Prático (Cardápio e Combos de Restaurante)

[↑ Voltar ao início](#inicio)

O código fornecido demonstra o padrão Composite aplicado na gestão de um cardápio, onde pratos individuais (`Prato`) e combinações complexas aninhadas (`Combo`) são tratados de forma totalmente uniforme através da interface `ItemMenu`.

<a id="sec-006"></a>
### A Interface Componente: `ItemMenu`

[↑ Voltar ao início](#inicio)

Define o contrato padrão (`getNome()` e `getPreco()`) que tanto os itens individuais quanto os combos devem implementar.

```java
package restaurante;

public interface ItemMenu {
    String getNome();
    double getPreco();
}

```

---

<a id="sec-007"></a>
### A Classe Folha (Leaf): `Prato`

[↑ Voltar ao início](#inicio)

Representa os itens simples do cardápio que não possuem filhos. Implementam as operações diretamente.

```java
package restaurante;

public class Prato implements ItemMenu {
    private final String nome;
    private final double preco;

    public Prato(String nome, double preco) {
        this.nome = nome;
        this.preco = preco;
    }

    @Override
    public String getNome() {
        return nome;
    }

    @Override
    public double getPreco() {
        return preco;
    }
}

```

---

<a id="sec-008"></a>
### A Classe Composta (Composite): `Combo`

[↑ Voltar ao início](#inicio)

Representa os recipientes que podem conter pratos simples ou outros combos, formando a estrutura de árvore recursiva. O método `getPreco()` realiza a soma recursiva dos preços de todos os seus filhos.

```java
package restaurante;

import java.util.ArrayList;
import java.util.List;

public class Combo implements ItemMenu {
    private final String nome;
    private final List<ItemMenu> itens = new ArrayList<>();

    public Combo(String nome) {
        this.nome = nome;
    }

    public void adicionar(ItemMenu item) {
        itens.add(item);
    }

    public void remover(ItemMenu item) {
        itens.remove(item);
    }

    @Override
    public String getNome() {
        return nome;
    }

    @Override
    public double getPreco() {
        // Soma recursiva do preço de todos os filhos (seja prato ou outro combo)
        double total = 0.0;
        for (ItemMenu item : itens) {
            total += item.getPreco();
        }
        return total;
    }
}

```

---

<a id="sec-009"></a>
### A Execução: `Main`

[↑ Voltar ao início](#inicio)

Demonstra a criação de pratos, a composição de combos simples, combos aninhados dentro de outros combos (recursão) e o tratamento perfeitamente uniforme pelo método auxiliar `imprimirDetalhesItem`.

```java
package restaurante;

public class Main {
    public static void main(String[] args) {
        System.out.println("=== 1. MONTANDO PRATOS INDIVIDUAIS (FOLHAS) ===");
        ItemMenu hamburguer = new Prato("Hambúrguer Artesanal", 25.00);
        ItemMenu batataFrita = new Prato("Batata Frita Média", 12.50);
        ItemMenu refrigerante = new Prato("Refrigerante Lata", 7.00);

        System.out.println(hamburguer.getNome() + " - R$ " + hamburguer.getPreco());
        System.out.println(batataFrita.getNome() + " - R$ " + batataFrita.getPreco());
        System.out.println(refrigerante.getNome() + " - R$ " + refrigerante.getPreco());

        System.out.println("\n=== 2. MONTANDO O COMBO DUPLO ===");
        Combo comboDuplo = new Combo("Combo Duplo");
        comboDuplo.adicionar(hamburguer);
        comboDuplo.adicionar(hamburguer);
        comboDuplo.adicionar(refrigerante);

        System.out.println("Preço do " + comboDuplo.getNome() + ": R$ " + comboDuplo.getPreco());

        System.out.println("\n=== 3. MONTANDO O COMBO FAMÍLIA (COM RECURSÃO) ===");
        Combo comboFamilia = new Combo("Combo Família");
        comboFamilia.adicionar(comboDuplo); // Inclui o Combo Duplo dentro dele!
        comboFamilia.adicionar(batataFrita);
        comboFamilia.adicionar(refrigerante);

        System.out.println("Preço do " + comboFamilia.getNome() + ": R$ " + comboFamilia.getPreco());

        System.out.println("\n=== 4. DEMONSTRANDO A UNIFORMIDADE DA INTERFACE ===");
        imprimirDetalhesItem(hamburguer);
        imprimirDetalhesItem(comboDuplo);
        imprimirDetalhesItem(comboFamilia);
    }

    private static void imprimirDetalhesItem(ItemMenu item) {
        System.out.println("[Tratamento Uniforme] Item: " + item.getNome() + " | Preço Total: R$ " + item.getPreco());
    }
}

```

> **Vantagem prática:** O código cliente (`Main` e o método `imprimirDetalhesItem`) não precisa saber se está lidando com um prato isolado ou com um combo complexo contendo subcombos, pois ambos compartilham exatamente a mesma interface `ItemMenu`.

---

Tem alguma dúvida sobre o padrão Composite ou gostaria de explorar outro padrão de projeto?

<a id="sec-010"></a>
## 5. Implementação real do repositório: valores simples e agrupados

[↑ Voltar ao início](#inicio)

O exercício em [`CompositeExcercise.cs`](Aula04_CompositeExcercise/CompositeExcercise.cs) unifica um inteiro isolado e uma coleção de inteiros sob `IValueContainer : IEnumerable<int>`. `SingleValue` é a folha: seu enumerador produz um único valor. `ManyValues` é o composto: herda a coleção de vários valores. O cliente soma sem testar qual classe concreta recebeu.

```csharp
public interface IValueContainer : IEnumerable<int> { }
public class SingleValue : IValueContainer
{
    public int Value;
    public IEnumerator<int> GetEnumerator() { yield return Value; }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
public class ManyValues : List<int>, IValueContainer { }
```

O `Sum()` percorre primeiro cada container e depois cada inteiro que ele expõe. Essa uniformidade é a essência do padrão: folhas e grupos respondem ao mesmo contrato. As aulas `GeometricShapes` e `NeuralNetworks` exploram outras composições recursivas; `CompositeSpecification` aplica a mesma composição a regras.

Neste exercício, `ManyValues` herda de `List<int>` por simplicidade didática. Em código de produção, normalmente é melhor compor uma coleção e expor apenas as operações necessárias, para não herdar toda a API mutável de `List<T>`.


<a id="sec-011"></a>
## Outros exemplos de Composite na pasta

[↑ Voltar ao início](#inicio)

- [geometricShapes.cs](Aula01_GeometricShapes/geometricShapes.cs) compõe figuras para aplicar operações uniformes a grupos.
- [neuralNetworks.cs](Aula02_NeuralNetworks/neuralNetworks.cs) combina neurônios e conexões em estruturas compostas.
- [CompositeSpecification.cs](Aula03_CompositeSpecification/CompositeSpecification.cs) combina especificações lógicas, como AND/OR, sob um contrato comum.
- [CompositeExcercise.cs](Aula04_CompositeExcercise/CompositeExcercise.cs) faz SingleValue e ManyValues serem percorríveis como IValueContainer e permite somar sem distinguir os tipos concretos.

No exercício, folha e composto não formam necessariamente uma árvore recursiva completa como os exemplos de formas; a lição central é o tratamento uniforme pelo contrato. Compare as estruturas para notar que Composite é uma intenção, não uma única forma obrigatória de classe.


<a id="sec-012"></a>
## 6. Por que a soma não pergunta o tipo de cada item

[↑ Voltar ao início](#inicio)

No exercício [CompositeExcercise.cs](Aula04_CompositeExcercise/CompositeExcercise.cs), Sum recebe uma lista de IValueContainer e usa dois foreach: o primeiro visita containers; o segundo percorre os inteiros de cada container. Sum não pergunta “isto é SingleValue ou ManyValues?” nem contém uma ramificação para cada classe.

SingleValue é uma folha: seu enumerador expõe exatamente um inteiro com yield return. ManyValues representa um grupo e reutiliza a coleção List<int>. Como ambas implementam IValueContainer, o cliente tem uma operação uniforme. A ideia central é deixar cada nó expor seu conteúdo pelo contrato comum, em vez de transferir ao chamador a responsabilidade de distinguir estruturas.

As aulas de formas, redes neurais e especificações mostram árvores mais recursivas. Composite é especialmente útil quando folhas e grupos devem responder à mesma operação, como desenhar, calcular custo ou buscar. Nem toda coleção é Composite: faz sentido quando o grupo é parte significativa do modelo e compartilha operações com os itens.

No exercício, ManyValues herda de List<int> para simplificar a aula. Em produção, composição geralmente limita a API mutável exposta e protege melhor os invariantes.


<a id="sec-013"></a>
## Como reconhecer um Composite real

[↑ Voltar ao início](#inicio)

Pergunte se o domínio contém uma estrutura parte-todo, normalmente uma árvore, e se o cliente deve executar a mesma operação em uma folha ou num grupo. Pasta/arquivo, grupo de formas e combinação de especificações são casos naturais. Uma lista genérica não vira Composite só por conter muitos itens: o grupo deve participar do modelo e responder ao mesmo contrato.

No exercício de IValueContainer, a uniformidade permite Somar qualquer container por enumeração; ele ensina o contrato comum. O componente ManyValues não contém outros IValueContainer, então esse exercício é menos recursivo que uma árvore de formas. A diferença é útil para não achar que todo exemplo Composite precisa ter a mesma estrutura.

Uma API pode ser transparente (o contrato comum inclui operações de adicionar/remover, mesmo para folhas) ou segura (só composites expõem mutação). Prefira a versão segura quando chamar Add numa folha não fizer sentido; prefira transparência quando tratar folha e grupo com exatamente a mesma API for mais valioso.

<a id="sec-014"></a>
## Variações e aprofundamentos

[↑ Voltar ao início](#inicio)

As seções abaixo reúnem variações, exemplos e decisões que complementam a explicação principal deste padrão. Elas preservam a linguagem e o contexto do material didático original.

<a id="sec-015"></a>
## 55. Geometric Shapes

[↑ Voltar ao início](#inicio)


O arquivo `StructuralPatterns/8.Composite/Aula01_GeometricShapes/geometricShapes.cs` demonstra o Composite com um exemplo pequeno, mas conceitualmente completo: um desenho pode conter formas individuais e também grupos de formas. O ponto central é que o cliente manipula tudo como `GraphicObject`, sem precisar criar uma ramificação do tipo "se for grupo, percorra os filhos; se for forma, desenhe direto".

Essa é exatamente a intenção do padrão: transformar uma hierarquia parte-todo em uma estrutura de árvore onde folhas e composições compartilham uma mesma abstração.

---

<a id="sec-016"></a>
### 55.1 O problema do exemplo

[↑ Voltar ao início](#inicio)

Imagine que queremos representar um desenho geométrico formado por:

- um quadrado vermelho;
- um círculo amarelo;
- um grupo contendo um círculo azul e um quadrado azul.

Sem Composite, o cliente tenderia a separar manualmente os casos:

- uma lista para formas simples;
- outra estrutura para grupos;
- condicionais para descobrir se o item possui filhos;
- loops diferentes para imprimir folhas e grupos.

Esse tipo de código espalha conhecimento estrutural pelo cliente. Toda vez que o desenho ganhasse um novo nível de agrupamento, mais lógica de navegação teria que ser lembrada e repetida.

Com Composite, o cliente monta uma árvore de `GraphicObject` e chama uma única operação na raiz.

---

<a id="sec-017"></a>
### 55.2 Estrutura do código

[↑ Voltar ao início](#inicio)

A classe principal do exemplo é `GraphicObject`:

```csharp
public class GraphicObject
{
  public virtual string Name { get; set; } = "Group";
  public string Color;
  private Lazy<List<GraphicObject>> children = new Lazy<List<GraphicObject>>();
  public List<GraphicObject> Children => children.Value;

  private void Print(StringBuilder sb, int depth)
  {
    sb.Append(new string('*', depth))
      .Append(string.IsNullOrWhiteSpace(Color) ? string.Empty : $"{Color} ")
      .AppendLine($"{Name}");

    foreach (var child in Children)
      child.Print(sb, depth + 1);
  }

  public override string ToString()
  {
    var sb = new StringBuilder();
    Print(sb, 0);
    return sb.ToString();
  }
}
```

Essa classe concentra três ideias importantes:

- `Name` descreve o tipo visual do objeto. Por padrão, um `GraphicObject` é chamado de `"Group"`.
- `Children` é uma coleção de outros `GraphicObject`, o que permite que um objeto contenha folhas ou outros grupos.
- `Print` executa a operação comum e delega a mesma operação para cada filho, aumentando a profundidade.

O detalhe mais importante está na assinatura da coleção:

```csharp
public List<GraphicObject> Children => children.Value;
```

Como cada filho também é um `GraphicObject`, a estrutura pode crescer recursivamente. Um grupo pode conter uma forma, outro grupo, outro grupo dentro dele, e assim por diante.

---

<a id="sec-018"></a>
### 55.3 Participantes do padrão neste arquivo

[↑ Voltar ao início](#inicio)

Mapeando o código para os participantes clássicos do GoF:

- **Component:** `GraphicObject`. É o tipo comum que define a operação compartilhada, neste caso a representação textual via `ToString()` e a impressão interna via `Print(...)`.
- **Composite:** também `GraphicObject`, quando usado como grupo. Ele possui `Children` e delega a operação para seus filhos.
- **Leaf:** `Circle` e `Square`. São objetos finais no uso demonstrado e mudam apenas o `Name`.
- **Client:** `Demo.Main`. Monta a árvore e imprime a raiz sem distinguir manualmente folhas e grupos.

As folhas são pequenas de propósito:

```csharp
public class Circle : GraphicObject
{
  public override string Name => "Circle";
}

public class Square : GraphicObject
{
  public override string Name => "Square";
}
```

Elas não precisam reimplementar `ToString()` nem `Print(...)`, porque a lógica comum já está na classe-base. Esse é um ganho direto do padrão: o comportamento aplicável à árvore inteira fica centralizado no componente comum.

---

<a id="sec-019"></a>
### 55.4 Como a árvore é montada

[↑ Voltar ao início](#inicio)

No `Main`, o primeiro objeto criado representa a raiz do desenho:

```csharp
var drawing = new GraphicObject {Name = "My Drawing"};
```

Depois, duas folhas são adicionadas diretamente nessa raiz:

```csharp
drawing.Children.Add(new Square {Color = "Red"});
drawing.Children.Add(new Circle{Color="Yellow"});
```

Em seguida, o código cria outro `GraphicObject`. Como o nome padrão é `"Group"`, ele funciona como um agrupamento:

```csharp
var group = new GraphicObject();
group.Children.Add(new Circle{Color="Blue"});
group.Children.Add(new Square{Color="Blue"});
```

Por fim, esse grupo é colocado dentro do desenho principal:

```csharp
drawing.Children.Add(group);
```

A estrutura resultante pode ser lida assim:

```text
My Drawing
  Red Square
  Yellow Circle
  Group
    Blue Circle
    Blue Square
```

Essa árvore tem dois níveis abaixo da raiz, mas nada no modelo impede que grupos sejam aninhados em profundidades maiores.

---

<a id="sec-020"></a>
### 55.5 Como a impressão recursiva funciona

[↑ Voltar ao início](#inicio)

O método `ToString()` apenas prepara o `StringBuilder` e inicia a operação pela raiz:

```csharp
public override string ToString()
{
  var sb = new StringBuilder();
  Print(sb, 0);
  return sb.ToString();
}
```

A recursão acontece dentro de `Print(...)`:

```csharp
foreach (var child in Children)
  child.Print(sb, depth + 1);
```

Cada objeto imprime a si mesmo e, depois, pede que seus filhos façam a mesma coisa. A variável `depth` registra o nível da árvore. Quanto mais profundo o nó, mais asteriscos são adicionados antes do texto:

```csharp
sb.Append(new string('*', depth))
```

Visualmente, o processo interno pode ser imaginado assim:

```text
Entrada do cliente:
WriteLine(drawing)
  -> chama drawing.ToString()
     -> cria sb = ""
     -> chama Print(drawing, depth: 0)

Estado inicial da árvore:
My Drawing
  Red Square
  Yellow Circle
  Group
    Blue Circle
    Blue Square

------------------------------------------------------------
Chamada 1: Print("My Drawing", depth: 0)
------------------------------------------------------------
new string('*', 0) => ""
linha adicionada ao sb => "My Drawing\n"

sb agora:
My Drawing

Children de "My Drawing":
  [0] Red Square
  [1] Yellow Circle
  [2] Group

foreach entra no filho [0]
  -> chama Print("Red Square", depth: 1)

------------------------------------------------------------
Chamada 2: Print("Red Square", depth: 1)
------------------------------------------------------------
new string('*', 1) => "*"
linha adicionada ao sb => "*Red Square\n"

sb agora:
My Drawing
*Red Square

Children de "Red Square":
  lista vazia

Nao ha filhos para percorrer.
Retorna para a Chamada 1, que continua o foreach.

------------------------------------------------------------
Chamada 1 continua: proximo filho [1]
------------------------------------------------------------
  -> chama Print("Yellow Circle", depth: 1)

------------------------------------------------------------
Chamada 3: Print("Yellow Circle", depth: 1)
------------------------------------------------------------
new string('*', 1) => "*"
linha adicionada ao sb => "*Yellow Circle\n"

sb agora:
My Drawing
*Red Square
*Yellow Circle

Children de "Yellow Circle":
  lista vazia

Retorna para a Chamada 1, que continua o foreach.

------------------------------------------------------------
Chamada 1 continua: proximo filho [2]
------------------------------------------------------------
  -> chama Print("Group", depth: 1)

------------------------------------------------------------
Chamada 4: Print("Group", depth: 1)
------------------------------------------------------------
new string('*', 1) => "*"
linha adicionada ao sb => "*Group\n"

sb agora:
My Drawing
*Red Square
*Yellow Circle
*Group

Children de "Group":
  [0] Blue Circle
  [1] Blue Square

foreach do grupo entra no filho [0]
  -> chama Print("Blue Circle", depth: 2)

------------------------------------------------------------
Chamada 5: Print("Blue Circle", depth: 2)
------------------------------------------------------------
new string('*', 2) => "**"
linha adicionada ao sb => "**Blue Circle\n"

sb agora:
My Drawing
*Red Square
*Yellow Circle
*Group
**Blue Circle

Children de "Blue Circle":
  lista vazia

Retorna para a Chamada 4, que continua o foreach do grupo.

------------------------------------------------------------
Chamada 4 continua: proximo filho [1]
------------------------------------------------------------
  -> chama Print("Blue Square", depth: 2)

------------------------------------------------------------
Chamada 6: Print("Blue Square", depth: 2)
------------------------------------------------------------
new string('*', 2) => "**"
linha adicionada ao sb => "**Blue Square\n"

sb agora:
My Drawing
*Red Square
*Yellow Circle
*Group
**Blue Circle
**Blue Square

Children de "Blue Square":
  lista vazia

Retorna para a Chamada 4.
Chamada 4 terminou todos os filhos do grupo.
Retorna para a Chamada 1.

Chamada 1 terminou todos os filhos de "My Drawing".
ToString() retorna sb.ToString().
WriteLine imprime o texto final.
```

O detalhe mais importante dessa simulação é que o `foreach` da raiz não "some". Ele fica aguardando cada chamada filha terminar. Quando a chamada filha retorna, a execução volta exatamente para o próximo item do `foreach` anterior.

A saída esperada do exemplo é:

```text
My Drawing
*Red Square
*Yellow Circle
*Group
**Blue Circle
**Blue Square
```

O cliente não percorre a árvore manualmente. Ele chama:

```csharp
WriteLine(drawing);
```

O `WriteLine` usa `drawing.ToString()`, e o restante da árvore aparece porque a própria estrutura sabe propagar a operação.

---

<a id="sec-021"></a>
### 55.6 Variante transparente vs. variante segura

[↑ Voltar ao início](#inicio)

Este exemplo usa uma variante chamada **transparente** do Composite. Nela, todo `GraphicObject` expõe `Children`, inclusive `Circle` e `Square`.

O benefício é simplicidade:

- o cliente usa uma única API;
- qualquer objeto pode ser tratado como possível nó da árvore;
- não é necessário separar interface de folha e interface de grupo.

O custo é que as folhas não são estruturalmente protegidas contra filhos. Nada impede o seguinte código:

```csharp
var circle = new Circle();
circle.Children.Add(new Square());
```

Do ponto de vista geométrico, isso é estranho: um círculo normalmente não deveria conter um quadrado. Mas, do ponto de vista do exemplo didático, essa escolha deixa o Composite mais fácil de enxergar.

Uma variante **segura** separaria os papéis:

- `GraphicObject` ou `IGraphicObject` teria apenas operações comuns, como imprimir ou renderizar;
- uma classe `Group` teria a coleção de filhos;
- `Circle` e `Square` não exporiam `Children`.

Essa alternativa protege melhor o domínio, mas torna o cliente um pouco mais consciente da diferença entre folha e composição quando precisa montar a árvore.

---

<a id="sec-022"></a>
### 55.7 O papel de `Lazy<List<GraphicObject>>`

[↑ Voltar ao início](#inicio)

O campo de filhos é inicializado com `Lazy<T>`:

```csharp
private Lazy<List<GraphicObject>> children = new Lazy<List<GraphicObject>>();
public List<GraphicObject> Children => children.Value;
```

A intenção aparente é adiar a criação da lista até que ela seja realmente acessada. Isso combina com a ideia de que muitas folhas talvez nunca precisem armazenar filhos.

Mas existe uma nuance importante neste arquivo: `Print(...)` sempre acessa `Children` para fazer o `foreach`, mesmo nos objetos folha.

```csharp
foreach (var child in Children)
  child.Print(sb, depth + 1);
```

Portanto, quando uma folha é impressa, `Children` é acessado e a lista vazia acaba sendo criada. Nesse exemplo específico, `Lazy<List<GraphicObject>>` é mais um detalhe didático do que uma otimização efetiva de memória.

---

<a id="sec-023"></a>
### 55.8 O que este exemplo ensina

[↑ Voltar ao início](#inicio)

Este arquivo é pequeno, mas captura bem a essência do Composite:

- uma árvore é formada porque cada nó pode apontar para outros nós do mesmo tipo-base;
- folhas e grupos compartilham a mesma operação de apresentação;
- o cliente trabalha com a raiz e não precisa conhecer a estrutura interna completa;
- a propagação da operação é explícita: o composto percorre seus filhos e delega a chamada;
- a simplicidade da variante transparente vem com o custo de permitir filhos em objetos que conceitualmente seriam folhas.

A regra mental para guardar é:

> Se o domínio tem objetos individuais e agrupamentos dos mesmos objetos, e o cliente precisa executar a mesma operação sobre ambos, Composite pode transformar condicionais e loops espalhados em uma árvore com comportamento uniforme.

---


<a id="sec-024"></a>
## 56. Neural Networks

[↑ Voltar ao início](#inicio)


O arquivo `StructuralPatterns/8.Composite/Aula02_NeuralNetworks/neuralNetworks.cs` mostra outra forma de aplicar a ideia do Composite. Aqui, a estrutura não é uma árvore visual como no exemplo das formas geométricas. A ideia é tratar um objeto individual (`Neuron`) e um grupo de objetos (`NeuronLayer`) por meio do mesmo contrato: `IEnumerable<Neuron>`.

O problema da aula é simples de formular:

- um neurônio precisa se conectar a outro neurônio;
- um neurônio precisa se conectar a uma camada inteira;
- uma camada inteira precisa se conectar a outra camada inteira.

Sem Composite, a tendência seria criar métodos separados:

```csharp
Connect(Neuron from, Neuron to)
Connect(Neuron from, NeuronLayer to)
Connect(NeuronLayer from, NeuronLayer to)
```

Isso cresce mal. Cada nova combinação entre elemento individual e agrupamento exige mais uma sobrecarga ou mais condicionais.

---

<a id="sec-025"></a>
### 56.1 A ideia central: tudo vira sequência de neurônios

[↑ Voltar ao início](#inicio)

A solução do arquivo é fazer com que tanto a folha quanto o grupo possam ser vistos como `IEnumerable<Neuron>`.

`NeuronLayer` já é naturalmente uma coleção, porque herda de `Collection<Neuron>`:

```csharp
public class NeuronLayer : Collection<Neuron>
{

}
```

Já `Neuron`, que é um objeto individual, implementa `IEnumerable<Neuron>` e se enumera como uma sequência de um único item:

```csharp
public class Neuron : IEnumerable<Neuron>
{
  public IEnumerator<Neuron> GetEnumerator()
  {
    yield return this;
  }

  IEnumerator IEnumerable.GetEnumerator()
  {
    yield return this;
  }
}
```

O ponto mais importante é o `yield return this`. Ele diz: "quando alguém tentar percorrer este neurônio como uma coleção, entregue o próprio neurônio como único elemento".

Visualmente:

```text
neuron1 como objeto individual:
Neuron

neuron1 quando visto como IEnumerable<Neuron>:
[
  neuron1
]

layer1 quando visto como IEnumerable<Neuron>:
[
  neuronA,
  neuronB,
  neuronC
]
```

Com isso, o cliente não precisa saber se está lidando com uma folha ou com um grupo. Ambos podem entrar no mesmo método.

---

<a id="sec-026"></a>
### 56.2 O método `ConnectTo`

[↑ Voltar ao início](#inicio)

O método de extensão recebe duas sequências de neurônios:

```csharp
public static void ConnectTo(this IEnumerable<Neuron> self, IEnumerable<Neuron> other)
```

Essa assinatura é o coração da aula. O método não pergunta se `self` é um `Neuron` ou uma `NeuronLayer`. Ele apenas sabe que pode percorrer `self`. O mesmo vale para `other`.

O trecho que realiza as conexões é este:

```csharp
foreach (var from in self)
  foreach (var to in other)
  {
    // Registra a conexao de saida no neuronio de origem.
    from.Out.Add(to);

    // Registra a conexao de entrada no neuronio de destino.
    to.In.Add(from);
  }
```

Antes da simulação, uma ressalva prática: `In` e `Out` precisam ser listas reais, não apenas campos declarados. Por isso, no arquivo da aula, cada neurônio já inicializa suas coleções de conexões:

```csharp
public List<Neuron> In = new List<Neuron>();
public List<Neuron> Out = new List<Neuron>();
```

Sem essa inicialização, a primeira chamada a `from.Out.Add(to)` tentaria adicionar um item em uma referência `null`, causando `NullReferenceException`.

---

<a id="sec-027"></a>
### 56.3 Simulação visual do `foreach` duplo

[↑ Voltar ao início](#inicio)

Para enxergar o processo interno da máquina, imagine esta situação:

```text
self  = layer1
other = layer2

layer1:
  [0] A
  [1] B

layer2:
  [0] X
  [1] Y
  [2] Z

Estado inicial das conexoes:
  A.Out = []
  B.Out = []

  X.In = []
  Y.In = []
  Z.In = []
```

Agora a máquina entra no `foreach` externo:

```text
foreach externo:
  from = A
```

Para `from = A`, o `foreach` interno percorre todos os neurônios de `other`:

```text
------------------------------------------------------------
Iteracao 1
------------------------------------------------------------
from = A
to   = X

Executa:
  from.Out.Add(to)
  => A.Out.Add(X)

  to.In.Add(from)
  => X.In.Add(A)

Estado agora:
  A.Out = [X]
  X.In  = [A]

------------------------------------------------------------
Iteracao 2
------------------------------------------------------------
from = A
to   = Y

Executa:
  A.Out.Add(Y)
  Y.In.Add(A)

Estado agora:
  A.Out = [X, Y]
  Y.In  = [A]

------------------------------------------------------------
Iteracao 3
------------------------------------------------------------
from = A
to   = Z

Executa:
  A.Out.Add(Z)
  Z.In.Add(A)

Estado agora:
  A.Out = [X, Y, Z]
  Z.In  = [A]
```

Quando o `foreach` interno termina todos os destinos para `A`, a máquina volta para o `foreach` externo e pega o próximo neurônio de origem:

```text
foreach externo:
  from = B
```

Agora o processo se repete para `B`:

```text
------------------------------------------------------------
Iteracao 4
------------------------------------------------------------
from = B
to   = X

Executa:
  B.Out.Add(X)
  X.In.Add(B)

Estado agora:
  B.Out = [X]
  X.In  = [A, B]

------------------------------------------------------------
Iteracao 5
------------------------------------------------------------
from = B
to   = Y

Executa:
  B.Out.Add(Y)
  Y.In.Add(B)

Estado agora:
  B.Out = [X, Y]
  Y.In  = [A, B]

------------------------------------------------------------
Iteracao 6
------------------------------------------------------------
from = B
to   = Z

Executa:
  B.Out.Add(Z)
  Z.In.Add(B)

Estado agora:
  B.Out = [X, Y, Z]
  Z.In  = [A, B]
```

Resultado final:

```text
Saidas:
  A.Out = [X, Y, Z]
  B.Out = [X, Y, Z]

Entradas:
  X.In = [A, B]
  Y.In = [A, B]
  Z.In = [A, B]

Interpretacao:
  todos os neuronios de layer1 foram conectados
  a todos os neuronios de layer2.
```

Essa combinação completa entre todos os elementos de `self` e todos os elementos de `other` é um **produto cartesiano**. Se `self` tem 2 neurônios e `other` tem 3 neurônios, o método cria `2 * 3 = 6` conexões.

---

<a id="sec-028"></a>
### 56.4 Como isso aparece nas chamadas do `Main`

[↑ Voltar ao início](#inicio)

No cliente, as chamadas ficam uniformes:

```csharp
neuron1.ConnectTo(neuron2);
neuron1.ConnectTo(layer1);
layer1.ConnectTo(layer2);
```

O mesmo método cobre os três casos porque todos os participantes podem ser vistos como `IEnumerable<Neuron>`.

Visualmente:

```text
neuron1.ConnectTo(neuron2)
  self  = [neuron1]
  other = [neuron2]
  conexoes criadas = 1 * 1 = 1

neuron1.ConnectTo(layer1)
  self  = [neuron1]
  other = [todos os neuronios de layer1]
  conexoes criadas = 1 * quantidade_de_neuronios_em_layer1

layer1.ConnectTo(layer2)
  self  = [todos os neuronios de layer1]
  other = [todos os neuronios de layer2]
  conexoes criadas = quantidade_layer1 * quantidade_layer2
```

Aqui está o ganho do Composite nesta aula: o cliente não precisa escolher um método diferente para folha ou grupo. A diferença estrutural fica escondida atrás do contrato comum de enumeração.


---


<a id="sec-029"></a>
## 57. Composite Specification

[↑ Voltar ao início](#inicio)


O arquivo `StructuralPatterns/8.Composite/Aula03_CompositeSpecification/CompositeSpecification.cs` mistura duas ideias que já apareceram no curso:

- o **Specification Pattern**, usado para transformar regras de filtragem em objetos;
- o **Composite Pattern**, usado para fazer uma regra composta tratar regras simples de maneira uniforme.

A pergunta da aula é:

> Como combinar regras pequenas, como "produto verde" e "produto grande", sem criar um método especial para cada combinação possível?

Sem esse desenho, o filtro tenderia a crescer assim:

```csharp
FilterByColor(...)
FilterBySize(...)
FilterByColorAndSize(...)
FilterByColorAndSizeAndWeight(...)
```

Isso viola a ideia do Open/Closed Principle: toda regra nova ou combinação nova empurra mais código para dentro do filtro. A solução é fazer o filtro depender apenas de uma abstração de regra.

---

<a id="sec-030"></a>
### 57.1 O modelo usado na aula

[↑ Voltar ao início](#inicio)

O arquivo cria um modelo pequeno de produto:

```csharp
public class Product
{
    public string Name { get; }
    public Color Color { get; }
    public Size Size { get; }
}
```

As propriedades `Color` e `Size` são os dados que as specifications vão avaliar.

No `Main`, os produtos usados no exemplo são:

```csharp
var apple = new Product("Apple", Color.Green, Size.Small);
var tree = new Product("Tree", Color.Green, Size.Large);
var house = new Product("House", Color.Blue, Size.Large);
```

Visualmente:

```text
products:
  [0] Apple
      Color = Green
      Size  = Small

  [1] Tree
      Color = Green
      Size  = Large

  [2] House
      Color = Blue
      Size  = Large
```

O objetivo final será encontrar produtos que sejam **verdes e grandes**. Nesse conjunto, apenas `Tree` deve passar.

---

<a id="sec-031"></a>
### 57.2 O contrato `ISpecification<T>`

[↑ Voltar ao início](#inicio)

A abstração central é:

```csharp
public abstract class ISpecification<T>
{
    public abstract bool IsSatisfied(T item);

    public static ISpecification<T> operator &(ISpecification<T> first,
        ISpecification<T> second)
    {
        if (first == null)
        {
            throw new ArgumentNullException(nameof(first));
        }

        if (second == null)
        {
            throw new ArgumentNullException(nameof(second));
        }

        return new AndSpecification<T>(first, second);
    }
}
```

Apesar do prefixo `I`, neste arquivo `ISpecification<T>` é uma **classe abstrata**, não uma interface. A escolha é didática e técnica: o exemplo quer declarar o operador `&` diretamente na abstração para permitir a sintaxe:

```csharp
var greenAndLarge = green & large;
```

O trecho abaixo é uma **sobrecarga de operador**:

```csharp
public static ISpecification<T> operator &(ISpecification<T> first,
    ISpecification<T> second)
```

Em C#, alguns operadores podem receber significado próprio dentro de uma classe ou struct. Aqui, o operador `&` está sendo ensinado a combinar duas `ISpecification<T>`.

Lendo a assinatura por partes:

- `public`: o operador pode ser usado por código externo.
- `static`: operadores sobrecarregados em C# precisam ser estáticos; eles pertencem ao tipo, não a uma instância específica.
- `ISpecification<T>` antes de `operator`: é o tipo de retorno. O resultado da combinação também será uma specification.
- `operator &`: indica qual operador está sendo redefinido para esse tipo.
- `first` e `second`: são os dois lados da expressão.

Quando o cliente escreve:

```csharp
var greenAndLarge = green & large;
```

o compilador interpreta conceitualmente como uma chamada ao método especial do operador:

```text
green & large
  => chama a sobrecarga do operador &
  => passa green como first
  => passa large como second
  => retorna new AndSpecification<Product>(green, large)
```

Essa forma acima é apenas uma maneira mental de enxergar o processo. No código C# normal, você usa a sintaxe com `&`; não precisa chamar o método do operador diretamente.

Os blocos de validação protegem a composição contra regras inexistentes:

```csharp
if (first == null)
{
    throw new ArgumentNullException(nameof(first));
}

if (second == null)
{
    throw new ArgumentNullException(nameof(second));
}
```

Isso evita montar uma `AndSpecification<T>` incompleta. Se uma das regras for `null`, o erro aparece no momento da composição, e não mais tarde durante o filtro.

Por fim, esta linha cria a regra composta:

```csharp
return new AndSpecification<T>(first, second);
```

Visualmente, o operador faz esta transformação:

```text
Expressao escrita pelo cliente:
  green & large

Lado esquerdo:
  first = green
  tipo real = ColorSpecification

Lado direito:
  second = large
  tipo real = SizeSpecification

Validacao:
  first == null?  nao
  second == null? nao

Objeto criado:
  new AndSpecification<Product>(green, large)

Resultado:
  greenAndLarge
    tipo visto pelo cliente = ISpecification<Product>
    tipo real em runtime    = AndSpecification<Product>
    items[0] = green
    items[1] = large
```

O ganho de design é que `green & large` continua parecendo uma expressão de regra, não uma montagem manual de objeto. O cliente lê a linha quase como português: "verde e grande". Internamente, porém, o sistema criou um Composite que contém duas specifications-filhas.

Uma dúvida comum aqui é: se uma `AndSpecification` pode receber três ou mais regras por causa do `params`, por que o operador `&` recebe apenas `first` e `second`?

A resposta é que `&` é um **operador binário**. Isso significa que ele sempre trabalha com dois lados por vez:

```text
green & large

lado esquerdo  = green
lado direito   = large
```

Por isso a assinatura do operador é:

```csharp
public static ISpecification<T> operator &(ISpecification<T> first,
    ISpecification<T> second)
```

O operador não está tentando receber todas as regras possíveis. Ele só define o que significa combinar **uma specification da esquerda** com **uma specification da direita**.

Quando você escreve três ou mais regras encadeadas:

```csharp
var spec = green & large & anotherRule;
```

o C# resolve a expressão em etapas, sempre de dois em dois:

```text
Expressao original:
  green & large & anotherRule

Agrupamento conceitual:
  (green & large) & anotherRule

Primeira chamada do operador:
  first  = green
  second = large

Retorno da primeira chamada:
  AndSpecification(green, large)

Segunda chamada do operador:
  first  = AndSpecification(green, large)
  second = anotherRule

Retorno final:
  AndSpecification(
    AndSpecification(green, large),
    anotherRule
  )
```

Isso funciona porque o retorno do operador também é `ISpecification<T>`. Então uma regra composta pode entrar novamente no lado esquerdo ou direito de outra composição.

Em outras palavras:

```text
ColorSpecification
  e uma ISpecification<Product>

SizeSpecification
  e uma ISpecification<Product>

AndSpecification
  tambem e uma ISpecification<Product>

Por isso:
  regra simples & regra simples      funciona
  regra composta & regra simples    funciona
  regra composta & regra composta   tambem funciona
```

Conceitualmente, `ISpecification<T>` diz:

> "Eu sou uma regra capaz de responder se um item do tipo `T` satisfaz ou não uma condição."

O método `IsSatisfied(...)` sempre devolve um `bool`:

- `true`: o item passou pela regra;
- `false`: o item falhou na regra.

---

<a id="sec-032"></a>
### 57.3 As specifications simples são folhas

[↑ Voltar ao início](#inicio)

As regras simples são as folhas do Composite. Elas avaliam uma única condição e não possuem outras specifications dentro delas.

`ColorSpecification` responde: "o produto tem esta cor?"

```csharp
public class ColorSpecification : ISpecification<Product>
{
    private readonly Color color;

    public override bool IsSatisfied(Product product)
    {
        return product.Color == color;
    }
}
```

`SizeSpecification` responde: "o produto tem este tamanho?"

```csharp
public class SizeSpecification : ISpecification<Product>
{
    private readonly Size size;

    public override bool IsSatisfied(Product product)
    {
        return product.Size == size;
    }
}
```

Essas classes são **Leafs** porque representam regras finais. Elas não delegam para outras regras.

---

<a id="sec-033"></a>
### 57.4 Onde o Composite aparece

[↑ Voltar ao início](#inicio)

A base do composto é `CompositeSpecification<T>`:

```csharp
public abstract class CompositeSpecification<T> : ISpecification<T>
{
    protected readonly ISpecification<T>[] items;

    protected CompositeSpecification(params ISpecification<T>[] items)
    {
        if (items == null)
        {
            throw new ArgumentNullException(nameof(items));
        }

        this.items = items;
    }
}
```

Essa classe guarda um array de `ISpecification<T>`. Esse é o ponto estrutural mais importante: uma specification composta contém outras specifications, mas todas continuam sendo tratadas pelo mesmo contrato.

O detalhe `params` merece atenção:

```csharp
params ISpecification<T>[] items
```

Em C#, `params` permite que o chamador passe vários argumentos do mesmo tipo sem montar manualmente um array. Como o parâmetro real é um array de `ISpecification<T>`, estas duas chamadas são equivalentes:

```csharp
new AndSpecification<Product>(green, large);
```

```csharp
new AndSpecification<Product>(
    new ISpecification<Product>[] { green, large }
);
```

Ou seja, `params` melhora a ergonomia da API. Ele deixa a criação de uma specification composta parecer uma lista natural de regras:

```text
Chamada escrita:
  new AndSpecification<Product>(green, large)

O compilador entende:
  items = [
    green,
    large
  ]

Dentro do construtor:
  this.items = items

Estado final do composite:
  AndSpecification
    items[0] = ColorSpecification(Green)
    items[1] = SizeSpecification(Large)
```

O `if (items == null)` é uma validação defensiva. Ele protege a classe caso alguém force uma chamada passando `null` no lugar do array de regras. Em uso comum com `params`, como `new AndSpecification<Product>(green, large)`, o compilador cria o array automaticamente.

Esse mesmo mecanismo funciona para três ou mais specifications de uma vez:

```csharp
var greenAndLargeAndAnotherRule =
    new AndSpecification<Product>(green, large, anotherRule);
```

Visualmente:

```text
Chamada escrita:
  new AndSpecification<Product>(green, large, anotherRule)

Por causa do params, o compilador monta:
  items = [
    green,
    large,
    anotherRule
  ]

Depois, o All percorre todas:
  items[0].IsSatisfied(product)
  items[1].IsSatisfied(product)
  items[2].IsSatisfied(product)

Resultado:
  o produto so passa se todas as regras retornarem true
```

Também é possível encadear o operador `&`:

```csharp
var greenAndLargeAndAnotherRule = green & large & anotherRule;
```

Nesse caso, a composição fica aninhada, porque o operador `&` combina duas specifications por vez:

```text
Expressao:
  green & large & anotherRule

Primeiro passo:
  green & large
  => AndSpecification(green, large)

Segundo passo:
  (green & large) & anotherRule
  => AndSpecification(
       AndSpecification(green, large),
       anotherRule
     )

Estrutura final:
  AndSpecification
    items[0] = AndSpecification
      items[0] = green
      items[1] = large
    items[1] = anotherRule
```

Mesmo aninhado, o resultado funciona porque `AndSpecification<T>` também é uma `ISpecification<T>`. Quando o filtro chama `IsSatisfied(...)`, a specification externa delega para suas filhas; uma dessas filhas pode ser outra `AndSpecification`, que por sua vez delega para as próprias filhas.

Há dois cuidados importantes:

- `new AndSpecification<Product>()`, sem nenhuma regra, faz `All(...)` retornar `true`, porque em C# `All` sobre uma sequência vazia é verdadeiro;
- se alguma regra dentro de `items` for `null`, a avaliação quebrará quando o código tentar chamar `specification.IsSatisfied(product)`.

O composto concreto da aula é `AndSpecification<T>`:

```csharp
public class AndSpecification<T> : CompositeSpecification<T>
{
    public override bool IsSatisfied(T item)
    {
        return items.All(specification => specification.IsSatisfied(item));
    }
}
```

`AndSpecification<T>` é uma specification, mas também contém várias specifications. Por isso ela é o Composite: por fora, parece uma regra comum; por dentro, delega o trabalho para regras-filhas.

Mapeando para o GoF:

- **Component:** `ISpecification<T>`.
- **Leaf:** `ColorSpecification`, `SizeSpecification`.
- **Composite:** `CompositeSpecification<T>` como base e `AndSpecification<T>` como composto concreto.
- **Client:** `BetterFilter` e `Demo.Main`.

---

<a id="sec-034"></a>
### 57.5 O operador `&`

[↑ Voltar ao início](#inicio)

O operador `&` transforma duas regras simples em uma regra composta:

```csharp
var green = new ColorSpecification(Color.Green);
var large = new SizeSpecification(Size.Large);

var greenAndLarge = green & large;
```

Internamente, isso equivale a:

```csharp
var greenAndLarge = new AndSpecification<Product>(green, large);
```

Visualmente:

```text
Antes:
  green
    regra: Product.Color == Green

  large
    regra: Product.Size == Large

Expressao:
  green & large

Depois:
  AndSpecification
    items[0] = green
    items[1] = large
```

O cliente passa a enxergar `greenAndLarge` como uma única specification, mesmo que ela seja formada por duas regras internas.

---

<a id="sec-035"></a>
### 57.6 Como o filtro consome a regra composta

[↑ Voltar ao início](#inicio)

O filtro não sabe se recebeu uma regra simples ou composta:

```csharp
public IEnumerable<Product> Filter(IEnumerable<Product> items, ISpecification<Product> spec)
{
    foreach (var item in items)
    {
        if (spec.IsSatisfied(item))
        {
            yield return item;
        }
    }
}
```

Ele apenas chama `spec.IsSatisfied(item)`. Se `spec` for uma `ColorSpecification`, uma única comparação acontece. Se `spec` for uma `AndSpecification`, ela avalia suas regras internas.

Esse é o mesmo ganho visto nas aulas anteriores de Composite: o cliente usa a abstração comum e não precisa perguntar qual tipo concreto recebeu.

---

<a id="sec-036"></a>
### 57.7 Simulação visual da avaliação

[↑ Voltar ao início](#inicio)

Agora vamos simular a execução de:

```csharp
foreach (var product in filter.Filter(products, greenAndLarge))
{
    WriteLine($" - {product.Name}");
}
```

Estado inicial:

```text
products:
  [0] Apple  | Green | Small
  [1] Tree   | Green | Large
  [2] House  | Blue  | Large

spec recebida pelo filtro:
  greenAndLarge = AndSpecification

greenAndLarge.items:
  [0] ColorSpecification(Green)
  [1] SizeSpecification(Large)
```

Primeira passagem do `foreach` do filtro:

```text
------------------------------------------------------------
Iteracao 1 do BetterFilter
------------------------------------------------------------
item = Apple

Filtro pergunta:
  greenAndLarge.IsSatisfied(Apple)

Como greenAndLarge e uma AndSpecification:
  items.All(specification => specification.IsSatisfied(Apple))

Avaliacoes internas:
  items[0] = ColorSpecification(Green)
    Apple.Color == Green
    Green == Green
    resultado: true

  items[1] = SizeSpecification(Large)
    Apple.Size == Large
    Small == Large
    resultado: false

Resultado do All:
  true E false => false

BetterFilter:
  if (false) nao entra
  Apple nao e retornado pelo yield return
```

Segunda passagem:

```text
------------------------------------------------------------
Iteracao 2 do BetterFilter
------------------------------------------------------------
item = Tree

Filtro pergunta:
  greenAndLarge.IsSatisfied(Tree)

Avaliacoes internas:
  items[0] = ColorSpecification(Green)
    Tree.Color == Green
    Green == Green
    resultado: true

  items[1] = SizeSpecification(Large)
    Tree.Size == Large
    Large == Large
    resultado: true

Resultado do All:
  true E true => true

BetterFilter:
  if (true) entra
  yield return Tree

Console:
   - Tree
```

Terceira passagem:

```text
------------------------------------------------------------
Iteracao 3 do BetterFilter
------------------------------------------------------------
item = House

Filtro pergunta:
  greenAndLarge.IsSatisfied(House)

Avaliacoes internas:
  items[0] = ColorSpecification(Green)
    House.Color == Green
    Blue == Green
    resultado: false

Como All usa E logico, uma regra falsa ja torna o resultado final falso.
A avaliacao pode parar aqui sem precisar aprovar as demais regras.

Resultado do All:
  false

BetterFilter:
  if (false) nao entra
  House nao e retornado pelo yield return
```

Resultado final:

```text
Produtos verdes e grandes:
 - Tree
```

O detalhe mental mais importante é que `BetterFilter` nunca muda. A regra cresce por composição: `AndSpecification` junta folhas pequenas e se apresenta para o filtro como se fosse apenas mais uma `ISpecification<Product>`.

---

<a id="sec-037"></a>
### 57.8 O que este exemplo ensina

[↑ Voltar ao início](#inicio)

Composite Specification é útil quando regras pequenas precisam ser combinadas em regras maiores.

Neste arquivo:

- `ColorSpecification` e `SizeSpecification` são regras simples;
- `AndSpecification` é uma regra composta;
- `CompositeSpecification<T>` guarda as regras-filhas;
- `BetterFilter` depende apenas da abstração `ISpecification<Product>`;
- o operador `&` torna a composição mais natural para o cliente.

A regra mental para guardar é:

> Quando uma regra composta puder ser tratada como uma regra comum, Composite permite montar expressões maiores sem alterar o código que consome essas regras.




---


<a id="sec-038"></a>
## Coding Exercise 7: Composite Coding Exercise

[↑ Voltar ao início](#inicio)


Este exercício consolida a ideia do Composite em uma forma bem pequena: somar valores sem se importar se cada item contém **um único valor** ou **vários valores**.

O enunciado diz que `Sum()` recebe uma lista de `IValueContainer`:

```csharp
public static int Sum(this List<IValueContainer> containers)
```

Essa lista pode conter:

- `SingleValue`, que guarda apenas um `int`;
- `ManyValues`, que guarda vários `int`.

O desafio é fazer o método abaixo funcionar sem `if`, sem `switch` e sem checar tipo concreto:

```csharp
foreach (var c in containers)
foreach (var i in c)
  result += i;
```

O ponto decisivo é que `c` precisa ser percorrível com `foreach`. Como `c` é do tipo `IValueContainer`, a interface precisa prometer que todo container de valor expõe uma sequência de inteiros.

---

<a id="sec-039"></a>
### O que o exercício quer de verdade

[↑ Voltar ao início](#inicio)

A solução não é fazer `Sum()` conhecer `SingleValue` e `ManyValues`. A solução é fazer os dois tipos parecerem iguais para o cliente.

Esse é o Composite no exercício:

```text
IValueContainer
  contrato comum:
    "eu posso ser percorrido como uma sequencia de int"

SingleValue
  folha:
    contem um unico int

ManyValues
  composite:
    contem varios int

Sum()
  cliente:
    soma qualquer IValueContainer sem saber o tipo concreto
```

Por isso a interface fica assim:

```csharp
public interface IValueContainer : IEnumerable<int>
{
}
```

Ela não precisa declarar nenhum método novo. Ao herdar de `IEnumerable<int>`, ela já diz o essencial:

> Todo `IValueContainer` pode ser usado em um `foreach` que entrega valores `int`.

---

<a id="sec-040"></a>
### A folha: `SingleValue`

[↑ Voltar ao início](#inicio)

`SingleValue` tem apenas um campo:

```csharp
public int Value;
```

Mas para participar do Composite, ele precisa se comportar como uma sequência de inteiros. Por isso implementa `IValueContainer` e fornece um enumerador:

```csharp
public class SingleValue : IValueContainer
{
  public int Value;

  public IEnumerator<int> GetEnumerator()
  {
    yield return Value;
  }

  IEnumerator IEnumerable.GetEnumerator()
  {
    return GetEnumerator();
  }
}
```

O trecho mais importante é:

```csharp
yield return Value;
```

Ele faz um objeto individual se apresentar como uma coleção de um único item.

Visualmente:

```text
Objeto:
  SingleValue
    Value = 11

Quando o foreach percorre esse objeto:
  entrega 11
  termina

Como sequencia:
  [11]
```

Essa é a mesma ideia vista em `Neuron`: um objeto único passa a ser tratável como uma sequência de um elemento.

---

<a id="sec-041"></a>
### Por que existem dois `GetEnumerator()`

[↑ Voltar ao início](#inicio)

No `SingleValue`, aparecem dois métodos parecidos:

```csharp
public IEnumerator<int> GetEnumerator()
{
  yield return Value;
}

IEnumerator IEnumerable.GetEnumerator()
{
  return GetEnumerator();
}
```

O primeiro é o enumerador **genérico**:

```csharp
public IEnumerator<int> GetEnumerator()
```

Ele é exigido por `IEnumerable<int>` e diz que, ao percorrer o objeto, os itens entregues serão `int`.

Visualmente:

```text
SingleValue { Value = 7 }

foreach (var i in singleValue)
  i = 7

tipo de i:
  int
```

O segundo é o enumerador **não genérico**:

```csharp
IEnumerator IEnumerable.GetEnumerator()
```

Ele vem da interface antiga `IEnumerable`, sem `<int>`. Como `IEnumerable<int>` herda de `IEnumerable`, a classe também precisa atender esse contrato. Em vez de duplicar a lógica, o método não genérico delega para o genérico:

```csharp
return GetEnumerator();
```

Mentalmente:

```text
Alguem pediu enumeracao antiga:
  IEnumerable.GetEnumerator()

O codigo responde:
  use a mesma enumeracao generica que ja entrega int
```

Por isso o método não genérico não faz outro `yield return`. Ele reaproveita o enumerador principal.

---

<a id="sec-042"></a>
### `IEnumerable<int>` vs. `ICollection<T>`

[↑ Voltar ao início](#inicio)

Neste exercício, a interface escolhida é:

```csharp
public interface IValueContainer : IEnumerable<int>
{
}
```

`IEnumerable<int>` significa apenas:

> "Este objeto pode ser percorrido e entrega valores `int`."

Ele permite:

```csharp
foreach (var i in c)
```

Mas não promete que o cliente pode adicionar, remover ou contar itens diretamente pela interface.

Visualmente:

```text
IEnumerable<int>
  consigo percorrer
  consigo receber int por int

Nao promete:
  Add(...)
  Remove(...)
  Clear()
  Count
```

`ICollection<T>` é um contrato mais forte. Ele representa uma coleção manipulável:

```text
ICollection<T>
  tambem pode ser percorrido
  tem Count
  tem Add(...)
  tem Remove(...)
  tem Clear()
  tem Contains(...)
```

Se a interface fosse:

```csharp
public interface IValueContainer : ICollection<int>
{
}
```

então `SingleValue` teria que implementar operações que não combinam bem com a ideia de "um único valor":

```text
SingleValue teria que responder:
  Add(10) faz o que?
  Remove(7) deixa o objeto vazio?
  Clear() apaga o valor?
  Count sempre e 1?
```

Isso complicaria o modelo sem necessidade.

O método `Sum()` só precisa ler os valores:

```csharp
foreach (var i in c)
  result += i;
```

Ele não precisa modificar os containers. Portanto, `IEnumerable<int>` é suficiente e mais adequado.

A regra mental é:

```text
Use IEnumerable<T>
  quando o cliente so precisa percorrer os itens.

Use ICollection<T>
  quando o cliente tambem precisa manipular a colecao
  com Add, Remove, Clear, Count, etc.
```

No Composite deste exercício, o contrato mínimo é melhor: tanto a folha (`SingleValue`) quanto o composto (`ManyValues`) só precisam provar que conseguem entregar uma sequência de `int`.

---

<a id="sec-043"></a>
### O composto: `ManyValues`

[↑ Voltar ao início](#inicio)

`ManyValues` representa vários inteiros:

```csharp
public class ManyValues : List<int>, IValueContainer
{
}
```

Como `List<int>` já implementa `IEnumerable<int>`, `ManyValues` já sabe ser percorrido com `foreach`.

Visualmente:

```text
Objeto:
  ManyValues
    [22, 33, 44]

Quando o foreach percorre esse objeto:
  entrega 22
  entrega 33
  entrega 44
  termina
```

O exercício fica simples porque `ManyValues` reaproveita o comportamento de `List<int>`. Ele só precisa declarar que também é um `IValueContainer`.

---

<a id="sec-044"></a>
### Como `Sum()` usa o contrato comum

[↑ Voltar ao início](#inicio)

O método final não precisa saber se está somando folhas ou compostos:

```csharp
public static int Sum(this List<IValueContainer> containers)
{
  int result = 0;

  foreach (var c in containers)
    foreach (var i in c)
      result += i;

  return result;
}
```

O primeiro `foreach` percorre os containers:

```text
containers:
  [0] SingleValue(11)
  [1] ManyValues(22, 33)
```

O segundo `foreach` percorre os inteiros dentro de cada container, sem se importar com o tipo concreto.

Simulação visual:

```text
Estado inicial:
  result = 0

containers:
  [0] SingleValue { Value = 11 }
  [1] ManyValues  { 22, 33 }

------------------------------------------------------------
Iteracao 1 do foreach externo
------------------------------------------------------------
c = SingleValue { Value = 11 }

foreach interno:
  SingleValue se comporta como [11]

  i = 11
  result += i
  result = 0 + 11
  result = 11

------------------------------------------------------------
Iteracao 2 do foreach externo
------------------------------------------------------------
c = ManyValues { 22, 33 }

foreach interno:
  ManyValues se comporta como [22, 33]

  i = 22
  result += i
  result = 11 + 22
  result = 33

  i = 33
  result += i
  result = 33 + 33
  result = 66

Fim:
  return 66
```

Essa simulação mostra o ganho central: `Sum()` trabalha apenas com `IValueContainer`, mas consegue somar tanto um valor único quanto vários valores.

---

<a id="sec-045"></a>
### O que costuma confundir aqui

[↑ Voltar ao início](#inicio)

O erro mais comum é fazer `SingleValue` implementar `IEnumerable<SingleValue>`. Isso parece natural no começo, mas quebra a intenção do exercício.

O método `Sum()` faz:

```csharp
result += i;
```

Como `result` é `int`, o `i` também precisa ser `int`. Portanto, o container deve entregar inteiros:

```text
Correto:
  IValueContainer : IEnumerable<int>

  foreach (var i in c)
    i e int

Incorreto:
  IEnumerable<SingleValue>

  foreach (var i in c)
    i seria SingleValue
```

Outro erro comum é fazer `ManyValues` herdar de `Collection<SingleValue>` ou `List<SingleValue>`. O teste espera poder adicionar inteiros diretamente em `ManyValues`, como:

```csharp
var manyValues = new ManyValues { 22, 33 };
```

Então `ManyValues` precisa ser uma coleção de `int`, não de `SingleValue`.

---

<a id="sec-046"></a>
### Como validar mentalmente se a solução está correta

[↑ Voltar ao início](#inicio)

Antes de rodar o teste, faça estas perguntas:

- `IValueContainer` herda de `IEnumerable<int>`?
- `SingleValue` implementa `IValueContainer`?
- `SingleValue.GetEnumerator()` entrega `Value` com `yield return Value`?
- `ManyValues` herda de `List<int>` e implementa `IValueContainer`?
- O `Sum()` consegue usar `foreach (var i in c)` sem saber se `c` é folha ou composto?

Se todas as respostas forem sim, o exercício está alinhado com Composite.

A regra mental final:

> Quando um item individual e um grupo de itens precisam ser consumidos pelo mesmo código, faça ambos obedecerem ao mesmo contrato de coleção.


---


<a id="sec-glossario"></a>
## Glossário
[↑ Voltar ao início](#inicio)

- **Componente:** contrato comum para objetos individuais e grupos.
- **Folha (Leaf):** elemento individual que não contém outros componentes.
- **Composto (Composite):** elemento que contém filhos e usa o mesmo contrato.
- **Árvore:** hierarquia formada por compostos e folhas.
