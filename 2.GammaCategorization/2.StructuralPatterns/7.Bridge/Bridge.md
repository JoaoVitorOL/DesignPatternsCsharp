<a id="inicio"></a>
## Sumário
[↑ Voltar ao início](#inicio)

- [1. O que o Bridge representa?](#sec-001)
  - [O Problema que ele resolve:](#sec-002)
- [2. Em que situações se usa?](#sec-003)
- [3. Instruções passo a passo de como implementar (em Java)](#sec-004)
- [4. Explicação detalhada do Exemplo Prático (Veículos e Motores)](#sec-005)
  - [A Interface de Implementação: Motor](#sec-006)
  - [As Implementações Concretas: MotorGasolina e MotorEletrico](#sec-007)
  - [A Abstração: Veiculo](#sec-008)
  - [As Abstrações Refinadas: Sedan e Suv](#sec-009)
  - [A Execução: Main](#sec-010)
- [5. Implementação real do repositório: formas e renderizadores](#sec-011)
- [Outros exemplos de Bridge na pasta](#sec-012)
- [6. Entenda o problema de combinações](#sec-013)
- [Variações e aprofundamentos](#sec-014)
- [52. Bridge](#sec-015)
  - [52.1 O Problema de Negócio: A Explosão Cartesiana](#sec-016)
  - [52.2 A Solução da Ponte](#sec-017)
  - [52.3 Estrutura do Código (bridge.cs)](#sec-018)
  - [52.4 Demonstração de Uso (Client)](#sec-019)
  - [52.5 O que é composição?](#sec-020)
  - [52.6 O que acontece em : base(renderer)?](#sec-021)
  - [52.7 Principais benefícios e limites](#sec-022)
- [Coding Exercise 6: Bridge Coding Exercise](#sec-023)
  - [O que o exercício quer de verdade](#sec-024)
  - [Anatomia da Implementação Prática](#sec-025)
  - [Participantes do Padrão no Exercício](#sec-026)
  - [O ganho real da refatoração](#sec-027)
- [Papéis técnicos no exemplo](#sec-028)



**Família GoF:** estrutural. **Escopo predominante:** objetos, separando abstração e implementação por composição.
<a id="sec-001"></a>
## 1. O que o Bridge representa?

[↑ Voltar ao início](#inicio)

O **Bridge (Ponte)** é um padrão de projeto estrutural que **separa uma solução em duas partes independentes**: a abstração e a implementação.

<a id="sec-002"></a>
### O Problema que ele resolve:

[↑ Voltar ao início](#inicio)

Quando um sistema possui duas variações independentes que precisam crescer juntas (por exemplo: tipos de veículos e tipos de motores), o uso exclusivo da herança gera um cenário conhecido como **explosão combinatória de subclasses**. Os principais problemas disso são:

* O número de classes cresce rapidamente de forma descontrolada.


* Cada nova dimensão multiplica as combinações existentes (ex: $N \times M$ classes).


* A manutenção, os testes e as extensões tornam-se caros e rígidos.



---

<a id="sec-003"></a>
## 2. Em que situações se usa?

[↑ Voltar ao início](#inicio)

* Há duas dimensões independentes de variação em um problema.


* A hierarquia de classes está crescendo vertiginosamente por causa de combinações.


* Você deseja trocar implementações ou comportamentos em tempo de execução.


* Precisa reduzir o acoplamento entre a lógica de alto nível e os detalhes específicos de plataforma ou infraestrutura.



---

<a id="sec-004"></a>
## 3. Instruções passo a passo de como implementar (em Java)

[↑ Voltar ao início](#inicio)

1. **Identifique as duas dimensões ortogonais** do problema (ex: o que faz vs. como faz).


2. **Defina a interface para as implementações** de baixo nível.


3. **Defina a abstração principal** (classe de alto nível) que o cliente vai utilizar.


4. **Faça a abstração receber a implementação via construtor** (composição/ponte).


5. **Delegue operações** da abstração para o objeto de implementação sempre que fizer sentido.



---

<a id="sec-005"></a>
## 4. Explicação detalhada do Exemplo Prático (Veículos e Motores)

[↑ Voltar ao início](#inicio)

O código fornecido demonstra o padrão Bridge aplicado na relação entre **Veículos** (`Sedan`, `Suv`) e **Motores** (`MotorGasolina`, `MotorEletrico`).

<a id="sec-006"></a>
### A Interface de Implementação: `Motor`

[↑ Voltar ao início](#inicio)

Define o contrato de baixo nível que os motores devem seguir, desacoplando o comportamento mecânico/elétrico dos veículos.

```java
package veiculos;

public interface Motor {
    void ligar();
    void acelerar();
}

```

---

<a id="sec-007"></a>
### As Implementações Concretas: `MotorGasolina` e `MotorEletrico`

[↑ Voltar ao início](#inicio)

Fornecem os detalhes específicos de funcionamento de cada tipo de motor.

```java
package veiculos;

public class MotorGasolina implements Motor {
    @Override
    public void ligar() {
        System.out.println("Vrum! Motor a combustão (Gasolina) ligado.");
    }

    @Override
    public void acelerar() {
        System.out.println("Acelerando com ronco do motor e queima de combustível.");
    }
}

```

```java
package veiculos;

public class MotorEletrico implements Motor {
    @Override
    public void ligar() {
        System.out.println("Silêncio... Sistema elétrico ativado e pronto.");
    }

    @Override
    public void acelerar() {
        System.out.println("Acelerando instantaneamente de forma 100% silenciosa.");
    }
}

```

---

<a id="sec-008"></a>
### A Abstração: `Veiculo`

[↑ Voltar ao início](#inicio)

Contém a referência para a interface `Motor` (a "ponte") e delega as chamadas operacionais para ela.

```java
package veiculos;

public abstract class Veiculo {
    // Referência para a implementação (o "Bridge" que conecta as duas pontas)
    protected final Motor motor;

    public Veiculo(Motor motor) {
        this.motor = motor;
    }

    public void dirigir() {
        System.out.print("Preparando veículo -> ");
        motor.ligar();
    }

    public void acelerar() {
        motor.acelerar();
    }
}

```

---

<a id="sec-009"></a>
### As Abstrações Refinadas: `Sedan` e `Suv`

[↑ Voltar ao início](#inicio)

Especializam a classe `Veiculo` adicionando características próprias ao comportamento de dirigir e acelerar.

```java
package veiculos;

public class Sedan extends Veiculo {
    public Sedan(Motor motor) {
        super(motor);
    }

    @Override
    public void dirigir() {
        System.out.print("[Sedan Confortável]: ");
        super.dirigir();
    }

    @Override
    public void acelerar() {
        System.out.print("[Sedan Dinâmico]: ");
        super.acelerar();
    }
}

```

```java
package veiculos;

public class Suv extends Veiculo {
    public Suv(Motor motor) {
        super(motor);
    }

    @Override
    public void dirigir() {
        System.out.print("[SUV Robusta]: ");
        super.dirigir();
    }

    @Override
    public void acelerar() {
        System.out.print("[SUV Potente]: ");
        super.acelerar();
    }
}

```

---

<a id="sec-010"></a>
### A Execução: `Main`

[↑ Voltar ao início](#inicio)

Demonstra a combinação dinâmica entre diferentes tipos de veículos e motores em tempo de execução sem a necessidade de criar classes combinadas estáticas.

```java
package veiculos;

public class Main {
    public static void main(String[] args) {
        System.out.println("=== 1. SEDAN COM MOTOR A GASOLINA ===");
        Veiculo sedanGasolina = new Sedan(new MotorGasolina());
        sedanGasolina.dirigir();
        sedanGasolina.acelerar();

        System.out.println("\n=== 2. SEDAN COM MOTOR ELÉTRICO ===");
        Veiculo sedanEletrico = new Sedan(new MotorEletrico());
        sedanEletrico.dirigir();
        sedanEletrico.acelerar();

        System.out.println("\n=== 3. SUV COM MOTOR A GASOLINA ===");
        Veiculo suvGasolina = new Suv(new MotorGasolina());
        suvGasolina.dirigir();
        suvGasolina.acelerar();

        System.out.println("\n=== 4. SUV COM MOTOR ELÉTRICO ===");
        Veiculo suvEletrico = new Suv(new MotorEletrico());
        suvEletrico.dirigir();
        suvEletrico.acelerar();
    }
}

```

> **Vantagem prática:** Sem o Bridge, para 3 veículos e 3 motores, precisaríamos de 9 classes fixas. Com o Bridge, criamos apenas 3 + 3 classes independentes e as combinamos livremente via composição.
>
>

---

Tem alguma dúvida sobre este padrão ou gostaria de ver como ele se compara com outros padrões estruturais?

<a id="sec-011"></a>
## 5. Implementação real do repositório: formas e renderizadores

[↑ Voltar ao início](#inicio)

O exercício em [`BridgeExcercise.cs`](Aula02_BridgeExcercise/BridgeExcercise.cs) separa duas dimensões que variam independentemente: a forma (`Triangle`, `Square`) e a forma de desenhar (`VectorRenderer`, `RasterRenderer`). `Shape` é a abstração; `IRenderer` é a implementação. A abstração guarda uma referência à implementação, recebida pelo construtor, e delega a ela o detalhe variável.

```csharp
public abstract class Shape
{
    protected IRenderer renderer;
    protected Shape(IRenderer renderer) =>
        this.renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
    public string Name { get; set; }
    public override string ToString() => $"Drawing {Name} as {renderer.WhatToRenderAs}";
}
```

Assim, `new Triangle(new RasterRenderer())` produz “Drawing Triangle as pixels”. Sem Bridge, a herança tende a gerar `VectorTriangle`, `RasterTriangle`, `VectorSquare`, `RasterSquare` e uma classe nova para cada combinação. Composição mantém essas hierarquias separadas, permite trocar renderer e possibilita adicionar uma forma ou backend sem multiplicar subclasses.

O primeiro arquivo [`bridge.cs`](Aula01_Bridge/bridge.cs) apresenta o mesmo princípio. Use Bridge quando há eixos de variação estáveis e combináveis; se só existe uma variação simples, a abstração extra pode não compensar.


<a id="sec-012"></a>
## Outros exemplos de Bridge na pasta

[↑ Voltar ao início](#inicio)

[Aula01_Bridge/bridge.cs](Aula01_Bridge/bridge.cs) separa formas geométricas de renderização vetorial/raster por uma referência à abstração do renderizador. [Aula02_BridgeExcercise/BridgeExcercise.cs](Aula02_BridgeExcercise/BridgeExcercise.cs) é a versão corrigida do exercício: Shape recebe IRenderer; Triangle e Square recebem VectorRenderer ou RasterRenderer por composição.

Leia os exemplos identificando as duas dimensões que podem variar. Bridge é útil quando novas formas e novas implementações de desenho precisam se combinar sem produzir uma subclasse para cada par. O primeiro exemplo tem um contrato de renderização específico por forma; por isso, adicionar uma forma requer atualizar os renderizadores. O exercício usa WhatToRenderAs genérico para demonstrar combinações com menos acoplamento.


<a id="sec-013"></a>
## 6. Entenda o problema de combinações

[↑ Voltar ao início](#inicio)

O exercício [BridgeExcercise.cs](Aula02_BridgeExcercise/BridgeExcercise.cs) separa duas decisões: que forma desenhar (Triangle ou Square) e como desenhar (VectorRenderer ou RasterRenderer). Se essas decisões forem codificadas por subclasses como VectorTriangle, RasterTriangle e VectorSquare, cada tecnologia nova multiplica a quantidade de classes.

Shape mantém IRenderer por composição. Triangle e Square recebem o renderer no construtor; ToString combina o nome da forma com a representação fornecida por WhatToRenderAs. Portanto, new Triangle(new RasterRenderer()) seleciona pixels, sem criar uma classe para essa combinação. Para acrescentar renderer, implemente IRenderer; para acrescentar forma, crie outra subclasse de Shape.

O primeiro exemplo bridge.cs tem métodos específicos para formas e mostra uma limitação: adicionar uma forma exige ampliar a interface e todos renderizadores. O exercício usa uma resposta genérica para ilustrar melhor a independência dos eixos. Na prática, Bridge funciona bem quando ambos os lados mudam ao longo do tempo e precisam combinar. Se um lado nunca varia, a abstração extra pode não valer o custo.

<a id="sec-014"></a>
## Variações e aprofundamentos

[↑ Voltar ao início](#inicio)

As seções abaixo reúnem variações, exemplos e decisões que complementam a explicação principal deste padrão. Elas preservam a linguagem e o contexto do material didático original.

<a id="sec-015"></a>
## 52. Bridge

[↑ Voltar ao início](#inicio)



O padrão **Bridge** é estrutural e desacopla uma abstração de sua implementação para que ambas possam variar por hierarquias separadas. A composição introduz uma indireção e um contrato comum; ela não torna as duas partes totalmente independentes.

---

<a id="sec-016"></a>
### 52.1 O Problema de Negócio: A Explosão Cartesiana

[↑ Voltar ao início](#inicio)

Imagine um cenário onde temos formas geométricas (Abstrações como `Circle` e `Square`) e precisamos renderizá-las em diferentes mídias (Implementações como `VectorRenderer` e `RasterRenderer`).

Se tentássemos resolver esse problema por herança direta, cairíamos no problema da **explosão cartesiana de classes**:

- Teríamos que criar classes para cada combinação: `VectorCircle`, `RasterCircle`, `VectorSquare`, `RasterSquare`.

- Num desenho que materializa cada combinação por herança, $M$ formas e $N$ renderizadores podem exigir até $M \times N$ classes concretas de combinação.

---

<a id="sec-017"></a>
### 52.2 A Solução da Ponte

[↑ Voltar ao início](#inicio)

Em vez de dizer que *"um círculo vetorizado é uma classe filha de círculo"*, o padrão Bridge aplica o princípio de design de **favorecer a composição sobre a herança**. Nós dizemos: *"uma forma geométrica possui um renderizador"*.

Ao criar uma interface para as implementações (`IRenderer`) e fazer `Shape` manter uma referência para ela, o modelo idealizado troca classes por combinação por aproximadamente $M + N$ classes, além das abstrações-base. Essa redução depende de um contrato do implementador que permaneça suficientemente genérico.

---

<a id="sec-018"></a>
### 52.3 Estrutura do Código (`bridge.cs`)

[↑ Voltar ao início](#inicio)

#### 1. A Ponte (The Implementor)

A interface define o contrato técnico para as implementações de renderização de baixo nível.

```csharp
public interface IRenderer
{
    void RenderCircle(float radius);
    void RenderSquare(float side);
}

```

Esse contrato revela uma limitação do exemplo local: `IRenderer` contém um método por forma. Adicionar `Triangle` exigiria alterar a interface e todos os renderizadores, portanto as formas **não evoluem de maneira plenamente independente**. O exercício da seção seguinte usa `WhatToRenderAs`, um contrato mais genérico, e demonstra melhor a separação das dimensões.

#### 2. Implementações Concretas (Concrete Implementors)

Realizam o trabalho técnico específico de desenho de acordo com a tecnologia de saída.

```csharp
// Renderizador focado em desenho vetorial (linhas de precisão)
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

```

#### 3. A Abstração (The Abstraction)

Esta é a raiz da hierarquia de alto nível. Em vez de herdar diretamente os caminhos de desenho, ela usa **composição** apontando para a ponte (`IRenderer`).

```csharp
public abstract class Shape
{
    // A "Ponte" física no código
    protected IRenderer renderer;

    // A implementação é injetada no momento do nascimento do objeto
    protected Shape(IRenderer renderer)
    {
        this.renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
    }

    public abstract void Draw();
    public abstract void Resize(float factor);
}

```

#### 4. Abstração Refinada (Refined Abstraction)

Uma especialização da abstração que adiciona regras de negócio próprias (como o `Circle`), sem se preocupar em como o desenho técnico é de fato realizado.

```csharp
public class Circle : Shape
{
    private float radius;

    // O Círculo não sabe (e não precisa saber) qual é o renderizador concreto
    public Circle(IRenderer renderer, float radius) : base(renderer)
    {
        this.radius = radius;
    }

    // Delegamos a responsabilidade técnica do desenho para a "Ponte"
    public override void Draw()
    {
        renderer.RenderCircle(radius);
    }

    public override void Resize(float factor)
    {
        radius *= factor;
    }
}

```

---

<a id="sec-019"></a>
### 52.4 Demonstração de Uso (Client)

[↑ Voltar ao início](#inicio)

No código cliente, nós instanciamos os motores de renderização desejados e os injetamos diretamente nas abstrações geométricas de forma dinâmica.

```csharp
class Program
{
    static void Main(string[] args)
    {
        // 1. Criamos os motores de renderização (Implementações)
        var vectorRenderer = new VectorRenderer();
        var rasterRenderer = new RasterRenderer();

        // 2. Criamos as formas (Abstrações) injetando o motor desejado
        var circle1 = new Circle(vectorRenderer, 5.0f);
        Console.WriteLine("--- Desenhando Círculo Vetorial ---");
        circle1.Draw(); // Saída: Drawing a circle of radius 5 as vectors.

        var circle2 = new Circle(rasterRenderer, 10.0f);
        Console.WriteLine("\n--- Desenhando Círculo Rasterizado ---");
        circle2.Draw(); // Saída: Drawing a circle of radius 10 as pixels.

        // Modificamos a abstração sem afetar o motor gráfico
        Console.WriteLine("\n--- Redimensionando Círculo Vetorial ---");
        circle1.Resize(2.0f); // Altera o raio para 10.0
        circle1.Draw();       // Redesenha com o motor vetorizado
    }
}

```

Para compreender a implementação, vale separar o conceito de **composição** do mecanismo de construtores com **`base(renderer)`**.

---

<a id="sec-020"></a>
### 52.5 O que é composição?

[↑ Voltar ao início](#inicio)

No design de software, existem duas formas principais de estender o comportamento de uma classe:

1. **Herança (*É um*):** Você cria uma subclasse. Ex: `VectorCircle` *é um* `Circle`. Se você precisar de um círculo para pixels, cria `RasterCircle`. Isso gera a explosão de classes.
2. **Composição (*Tem um*):** Em vez de colocar o comportamento dentro de uma árvore genealógica de herança, você guarda uma **referência** (um objeto de outra classe) dentro da sua classe.

No código fornecido, a composição acontece exatamente aqui:

```csharp
public abstract class Shape
{
    // Isso é composição! A classe Shape "tem um" renderer.
    protected IRenderer renderer;
}

```

A ponte não é apenas a variável: é a relação estável entre a hierarquia de `Shape` e o contrato `IRenderer`. A referência materializa essa relação no código.

---

<a id="sec-021"></a>
### 52.6 O que acontece em `: base(renderer)`?

[↑ Voltar ao início](#inicio)

Para entender o trecho do `Circle`, precisamos olhar para a classe mãe dele, que é a classe abstrata `Shape`.

#### O Contrato da Classe Mãe (`Shape`)

A classe `Shape` possui um construtor que **obriga** qualquer um a fornecer um renderizador para ela existir:

```csharp
protected Shape(IRenderer renderer)
{
    // Guarda o renderizador recebido na variável da classe mãe
    this.renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
}

```

#### A Classe Filha (`Circle`) repassando o bastão

Quando você cria a classe `Circle`, ela herda de `Shape`. No C#, quando a classe mãe possui um construtor com parâmetros, **a classe filha é obrigada a chamar esse construtor e fornecer os dados que a mãe precisa**.

É exatamente isso que o `: base(renderer)` faz:

```csharp
// 1. O cliente passa o 'renderer' e o 'radius' para o construtor do Circle
public Circle(IRenderer renderer, float radius)
    : base(renderer) // 2. O Circle pega esse 'renderer' e chuta para cima, para o construtor do 'Shape'
{
    // 3. O Circle se preocupa apenas com o que é DELE (o raio)
    this.radius = radius;
}

```

#### O Fluxo Passo a Passo na Memória

1. No `Main`, você faz: `new Circle(vectorRenderer, 5.0f);`
2. O construtor do `Circle` recebe o `vectorRenderer`.
3. Antes de executar o código dentro das chaves `{ ... }` do `Circle`, o programa ativa o `: base(renderer)`.
4. O construtor do `Shape` (a base) é executado, pegando o `vectorRenderer` e guardando na variável protegida `protected IRenderer renderer;`.
5. O controle volta para o `Circle`, que executa `this.radius = 5.0f;`.

#### Como isso sustenta o Bridge

Graças a esse mecanismo, quando o método `Draw()` do círculo é chamado:

```csharp
public override void Draw()
{
    // Ele usa a variável que foi guardada lá na classe mãe através do 'base'
    renderer.RenderCircle(radius);
}

```

O `Circle` não faz a menor ideia se o desenho sairá em linhas vetoriais ou em pixels. Ele apenas sabe que o objeto que a classe mãe guardou (`renderer`) sabe executar o método `RenderCircle`.

---

<a id="sec-022"></a>
### 52.7 Principais benefícios e limites

[↑ Voltar ao início](#inicio)

- **Desacoplamento:** a hierarquia de formas depende do contrato `IRenderer`, não das classes concretas de renderização.

- **Extensibilidade por implementação:** um novo `OpenGLRenderer` pode ser adicionado sem alterar `Circle` ou `Square`, desde que implemente todo o contrato. Já uma nova forma exige ampliar o `IRenderer` específico deste exemplo, como explicado acima.

- **Seleção em tempo de execução:** o cliente escolhe o renderizador ao construir a forma. No arquivo local, o campo não possui setter nem método de troca; substituir o renderizador depois da construção exigiria adicionar uma operação explícita. Herança também pode selecionar comportamentos em runtime por polimorfismo, mas não com a mesma composição entre dimensões.

Há ainda um erro independente do padrão no código local: `Square.Resize(float factor)` faz `lateral_size = factor`, enquanto o comentário afirma que o lado será multiplicado. Para redimensionamento proporcional, a implementação deveria usar `lateral_size *= factor`.

---

<a id="sec-023"></a>
## Coding Exercise 6: Bridge Coding Exercise

[↑ Voltar ao início](#inicio)



<a id="sec-024"></a>
### O que o exercício quer de verdade

[↑ Voltar ao início](#inicio)

O objetivo do exercício é refatorar uma hierarquia de classes incorreta baseada em herança combinatória. Devemos introduzir a interface `IRenderer` e injetá-la na classe abstrata `Shape`, permitindo que o método `ToString()` resolva o tipo de renderização de forma dinâmica e limpa.

<a id="sec-025"></a>
### Anatomia da Implementação Prática

[↑ Voltar ao início](#inicio)

Abaixo está a estrutura de código refatorada e corrigida que soluciona o problema de duplicação:

```csharp
using System;

namespace Coding.Exercise
{
    // 1. Definição da interface de implementação (A Ponte)
    public interface IRenderer
    {
        string WhatToRenderAs { get; }
    }

    // 2. Implementadores concretos da ponte
    public class VectorRenderer : IRenderer
    {
        public string WhatToRenderAs => "lines";
    }

    public class RasterRenderer : IRenderer
    {
        public string WhatToRenderAs => "pixels";
    }

    // 3. Abstração Base refatorada para usar composição
    public abstract class Shape
    {
        protected IRenderer renderer;
        public string Name { get; set; }

        protected Shape(IRenderer renderer)
        {
            this.renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
        }

        // O comportamento do ToString() resolve dinamicamente usando a ponte
        public override string ToString()
        {
            return $"Drawing {Name} as {renderer.WhatToRenderAs}";
        }
    }

    // 4. Abstrações Refinadas livres de detalhes de renderização
    public class Triangle : Shape
    {
        public Triangle(IRenderer renderer) : base(renderer)
        {
            Name = "Triangle";
        }
    }

    public class Square : Shape
    {
        public Square(IRenderer renderer) : base(renderer)
        {
            Name = "Square";
        }
    }
}

```

<a id="sec-026"></a>
### Participantes do Padrão no Exercício

[↑ Voltar ao início](#inicio)

Mapeando os componentes do código para a terminologia oficial do GoF:

- **Abstraction (`Shape`):** Define a interface da abstração e mantém uma referência para o objeto do tipo `IRenderer`.
- **Refined Abstraction (`Triangle`, `Square`):** Estende a interface definida pela abstração base, configurando o estado específico do domínio (como o `Name`).
- **Implementor (`IRenderer`):** Define a interface para as classes de implementação. Ela não precisa corresponder exatamente à interface da abstração.
- **Concrete Implementor (`VectorRenderer`, `RasterRenderer`):** Fornece a implementação concreta e o comportamento específico da plataforma/tecnologia.

<a id="sec-027"></a>
### O ganho real da refatoração

[↑ Voltar ao início](#inicio)

Neste exercício, o contrato `IRenderer.WhatToRenderAs` é genérico o bastante para que o modelo evite uma classe por combinação: há uma classe por forma e uma por modo de renderização, além das abstrações. É uma redução aproximada de $N \times M$ para $N + M$, não uma regra universal de complexidade. As partes continuam acopladas ao contrato `IRenderer`, mas não às classes concretas uma da outra.

---

<a id="sec-028"></a>
## Papéis técnicos no exemplo

Os nomes **Context**, **Strategy**, **Concrete Strategy** e semelhantes descrevem responsabilidades no desenho. O código não precisa usar esses nomes literalmente: o mapeamento abaixo identifica quem exerce cada papel no exemplo desta página.

[↑ Voltar ao início](#inicio)

| Papel no padrão | Classe do exemplo | O que representa no código |
|---|---|---|
| Implementor | **Motor** | Contrato da dimensão variável de propulsão, separado da hierarquia de veículos. |
| Implementações concretas | **MotorGasolina**, **MotorEletrico** | Realizam as operações de motor de modos diferentes. |
| Abstração | **Veiculo** | Mantém uma referência a Motor e delega a ele parte do trabalho. |
| Abstrações refinadas | **Sedan**, **Suv** | Especializam veículos sem fixar o tipo de motor. |
| Cliente | **Main** | Combina tipos de veículo e motor em pares diferentes. |

**Outro mapeamento local:** no exemplo de renderização, **Shape** é a abstração e **IRenderer** é o implementor; Circle/Triangle/Square variam independentemente de VectorRenderer e RasterRenderer.

<a id="sec-glossario"></a>
## Glossário
[↑ Voltar ao início](#inicio)

- **Abstração:** interface de alto nível usada pelo cliente.
- **Implementor:** contrato para uma dimensão separável de implementação.
- **Abstração refinada:** especialização que pode combinar-se com diferentes implementações.
- **Composição:** combinação das hierarquias sem multiplicar subclasses.
