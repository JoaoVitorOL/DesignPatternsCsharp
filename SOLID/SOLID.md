# Capítulo 1 — Princípios de Design SOLID

**por Robert C. Martin**

> **Base conceitual:** *Agile Principles, Patterns, and Practices in C#*  
> **Tema central:** princípios de design orientado a objetos aplicados com exemplos reais do projeto

---

## Nota Editorial

Este capítulo foi escrito em formato de livro técnico, com linguagem progressiva, didática e detalhada. O objetivo não é apenas definir os princípios SOLID, mas mostrar como eles aparecem na prática por meio dos arquivos deste próprio material.

Diferentemente de uma explicação apenas teórica, aqui cada princípio será acompanhado de referências diretas às aulas em código da pasta `SOLID/`. Assim, o leitor poderá alternar entre a leitura conceitual e a observação do exemplo concreto.

Ao longo do capítulo, há espaços reservados para **imagens, ilustrações, diagramas e quadros comparativos**.

---

## Espaço para Imagens de Abertura

> **Imagem sugerida 1:** retrato de Robert C. Martin  
> **Imagem sugerida 2:** capa do livro *Agile Principles, Patterns, and Practices in C#*  
> **Imagem sugerida 3:** diagrama visual com as cinco letras de SOLID

---

## Prefácio

Aprender orientação a objetos não é apenas aprender sintaxe. Saber declarar classes, heranças, interfaces e métodos é o começo. O desafio verdadeiro aparece quando o sistema cresce. É nesse momento que o programador descobre que o problema central do software raramente está em “fazer funcionar uma vez”; o problema está em **fazer continuar funcionando bem enquanto muda**.

Os princípios SOLID nasceram justamente como resposta a esse tipo de dificuldade.

Em projetos reais, o software começa simples. Depois surgem novas regras, novas integrações, novos comportamentos, novos tipos, novas exceções e novas combinações de cenários. Quando o código não tem boa estrutura, cada nova mudança custa mais caro do que a anterior. Uma classe que parecia inocente se transforma em um centro de risco. Uma herança aparentemente elegante se revela enganosa. Uma interface grande demais começa a forçar implementações artificiais. Um módulo de negócio passa a conhecer detalhes demais da infraestrutura.

SOLID oferece um conjunto de princípios para enfrentar exatamente esse tipo de deterioração.

Neste capítulo, cada princípio será estudado em duas camadas:

1. **A camada conceitual**, que explica a ideia arquitetural por trás do princípio.
2. **A camada prática**, que conecta a teoria aos exemplos construídos neste projeto.

Os cinco princípios estudados são:

- **S** — *Single Responsibility Principle*
- **O** — *Open-Closed Principle*
- **L** — *Liskov Substitution Principle*
- **I** — *Interface Segregation Principle*
- **D** — *Dependency Inversion Principle*

---

## Sumário

1. [Introdução ao SOLID](#1-introdução-ao-solid)
2. [Single Responsibility Principle (SRP)](#2-single-responsibility-principle-srp)
3. [Open-Closed Principle (OCP)](#3-open-closed-principle-ocp)
4. [Liskov Substitution Principle (LSP)](#4-liskov-substitution-principle-lsp)
5. [Interface Segregation Principle (ISP)](#5-interface-segregation-principle-isp)
6. [Dependency Inversion Principle (DIP)](#6-dependency-inversion-principle-dip)
7. [Conclusão do Capítulo](#7-conclusão-do-capítulo)

---

## 1. Introdução ao SOLID

SOLID é um acrônimo formado pelas iniciais de cinco princípios de design orientado a objetos. Embora sejam frequentemente apresentados como “regras”, é mais preciso entendê-los como **critérios de qualidade estrutural**.

Um sistema pode estar correto do ponto de vista funcional e, ainda assim, estar mal desenhado. Isso acontece quando o software funciona hoje, mas foi organizado de uma forma que dificulta qualquer evolução amanhã.

Os sintomas mais comuns desse problema são:

- classes com responsabilidades demais;
- necessidade constante de alterar classes antigas para incluir novos comportamentos;
- heranças que quebram expectativas do código cliente;
- interfaces grandes e pouco específicas;
- regras de negócio acopladas a detalhes de armazenamento ou infraestrutura.

Cada letra do SOLID combate um tipo de fragilidade:

- **SRP** combate a mistura de responsabilidades.
- **OCP** combate a necessidade de modificar continuamente classes estáveis.
- **LSP** combate hierarquias que parecem corretas, mas quebram o comportamento esperado.
- **ISP** combate interfaces inchadas.
- **DIP** combate o acoplamento entre alto nível e implementação concreta.

### Espaço para ilustração

> **Ilustração sugerida:** uma tabela visual ligando cada letra de SOLID a um problema recorrente de design.

---

## 2. Single Responsibility Principle (SRP)

### 2.1 Definição

O **Single Responsibility Principle** afirma que uma classe deve ter **apenas uma razão para mudar**.

Essa formulação é importante. Dizer apenas que uma classe deve “fazer uma coisa só” pode parecer simples demais. A noção de “uma razão para mudar” é mais precisa porque nos obriga a perguntar: **que tipo de decisão afeta esta classe?**

Se a resposta inclui persistência, exibição, negócio, integração externa e formatação ao mesmo tempo, provavelmente a classe está acumulando responsabilidades demais.

### 2.2 O problema que o SRP tenta evitar

Quando uma classe reúne muitas responsabilidades:

- ela cresce rápido demais;
- o nome dela perde clareza;
- mudanças em um ponto afetam outros;
- os testes ficam mais difíceis;
- a leitura e a manutenção pioram.

Em geral, a classe deixa de representar uma ideia coesa e passa a representar um “depósito de funcionalidades”.

### 2.3 Exemplo utilizado neste projeto

Nesta pasta, o SRP foi trabalhado com a ideia de um diário, usando os arquivos:

- `SOLID/Aula01_SingleResponsibility/errado.cs`
- `SOLID/Aula01_SingleResponsibility/certo.cs`

Na versão errada, a classe `JournalErrado` não apenas manipula as entradas do diário, mas também tenta:

- salvar em arquivo com `Save`;
- carregar de arquivo com `Load`;
- carregar por `Uri`.

Trecho representativo do arquivo `errado.cs`:

```csharp
public class JournalErrado
{
    public int AddEntry(string text) { ... }
    public void RemoveEntry(int index) { ... }
    public override string ToString() { ... }

    public void Save(string filename)
    {
        File.WriteAllText(filename, ToString());
    }

    public static JournalErrado Load(string filename)
    {
        var journal = new JournalErrado();
        journal.entries.AddRange(File.ReadAllLines(filename));
        return journal;
    }
}
```

### 2.4 Por que o exemplo errado está errado

O nome `JournalErrado` sugere uma entidade de domínio: um diário. O leitor espera que essa classe lide com:

- criação de entradas;
- remoção de entradas;
- representação textual.

Mas, a partir do momento em que ela também salva e carrega dados, passa a misturar duas naturezas diferentes:

- **domínio** do diário;
- **persistência** de dados.

Essas duas responsabilidades mudam por razões diferentes. O diário pode mudar porque o formato das entradas mudou. A persistência pode mudar porque o tipo de arquivo mudou, o local de armazenamento mudou ou a estratégia de gravação mudou.

Colocar tudo isso na mesma classe cria acoplamento indevido.

### 2.5 Como o exemplo certo resolve

Na versão correta, o projeto separa as responsabilidades entre:

- `JournalCerto`, responsável pelas entradas;
- `Persistence`, responsável pela gravação em arquivo.

Trecho representativo do arquivo `certo.cs`:

```csharp
public class JournalCerto
{
    public int AddEntry(string text) { ... }
    public void RemoveEntry(int index) { ... }
    public override string ToString() { ... }
}

public class Persistence
{
    public void SavetoFile(JournalCerto journal, string filename, bool overwrite = false)
    {
        File.WriteAllText(filename, journal.ToString());
    }
}
```

### 2.6 O que mudou na prática

A mudança que torna o código melhor não é apenas “criar mais uma classe”. A mudança importante é esta:

- o diário voltou a cuidar apenas de “coisas de diário”;
- a persistência passou a cuidar apenas de “coisas de arquivo”.

Isso melhora:

- clareza semântica;
- organização;
- manutenção;
- testabilidade.

### 2.7 Espaço para imagem

> **Ilustração sugerida:** antes e depois da separação entre `JournalErrado` e a dupla `JournalCerto` + `Persistence`.

---

## 3. Open-Closed Principle (OCP)

### 3.1 Definição

O **Open-Closed Principle** afirma que entidades de software devem estar **abertas para extensão, mas fechadas para modificação**.

Isso significa que o sistema deve aceitar novos comportamentos sem exigir a reedição contínua das mesmas classes antigas.

### 3.2 O problema que o OCP tenta evitar

Quando toda nova regra exige modificar uma classe já existente:

- o risco de regressão cresce;
- o código central se torna frágil;
- a manutenção fica cada vez mais cara;
- a classe vira um ponto permanente de instabilidade.

### 3.3 Exemplo utilizado neste projeto

No projeto, o OCP aparece nos arquivos:

- `SOLID/Aula02_OpenClosed/errado.cs`
- `SOLID/Aula02_OpenClosed/certo.cs`

Na versão errada, a classe `ProductFilter` concentra vários filtros específicos:

```csharp
public class ProductFilter
{
    public static IEnumerable<Product1> FilterBySize(IEnumerable<Product1> products, Size1 size) { ... }
    public static IEnumerable<Product1> FilterByColor(IEnumerable<Product1> products, Color1 color) { ... }
    public static IEnumerable<Product1> FilterBySizeAndColor(IEnumerable<Product1> products, Size1 size, Color1 color) { ... }
}
```

### 3.4 Por que o exemplo errado está errado

Cada nova regra exige mais um método.

Hoje existe:

- `FilterBySize`
- `FilterByColor`
- `FilterBySizeAndColor`

Amanhã poderiam surgir:

- `FilterByWeight`
- `FilterByCategory`
- `FilterByColorAndWeight`
- `FilterByColorAndCategoryAndSize`

O problema não é apenas quantidade de métodos. O problema é que a própria classe precisa ser reaberta toda vez que uma regra nova aparece.

### 3.5 Como o exemplo certo resolve

Na versão correta, o projeto usa abstrações:

- `ISpecification<T>`
- `IFilter<T>`
- `BetterFilter`

E regras concretas como:

- `ColorSpecification`
- `SizeSpecification`

Trecho representativo:

```csharp
public interface ISpecification<T>
{
    bool IsSatisfied(T item);
}

public class BetterFilter : IFilter<Product>
{
    public IEnumerable<Product> Filter(IEnumerable<Product> items, ISpecification<Product> spec)
    {
        foreach (var i in items)
        {
            if (spec.IsSatisfied(i))
            {
                yield return i;
            }
        }
    }
}
```

### 3.6 O que torna a solução melhor

Agora, quando surge uma nova regra, não é necessário editar `BetterFilter`. Basta criar uma nova especificação.

Em vez de crescer por modificação interna, o sistema cresce por **extensão externa**.

Esse é o coração do OCP.

### 3.7 O que mudou na prática

A mudança concreta foi:

- sair de uma classe com filtros acumulados;
- para um mecanismo genérico que recebe qualquer especificação válida.

O efeito é:

- menos risco de quebrar regras antigas;
- mais modularidade;
- mais flexibilidade para evoluir o sistema.

### 3.8 Espaço para imagem

> **Ilustração sugerida:** uma seta mostrando `ProductFilter` crescendo por adição de métodos versus `BetterFilter` recebendo novas especificações plugáveis.

---

## 4. Liskov Substitution Principle (LSP)

### 4.1 Definição

O **Liskov Substitution Principle** afirma que uma subclasse deve poder substituir sua classe base sem quebrar o comportamento esperado pelo código cliente.

O foco aqui não é apenas herança formal, mas **contrato comportamental**.

### 4.2 O problema que o LSP tenta evitar

Uma hierarquia pode parecer correta no papel e, ainda assim, ser perigosa na prática. Isso acontece quando a subclasse herda a forma da classe base, mas não respeita a expectativa de comportamento associada a ela.

### 4.3 Exemplo utilizado neste projeto

No projeto, a Aula 3 utiliza os arquivos:

- `SOLID/Aula03_LiskovSubstituiton/errado.cs`
- `SOLID/Aula03_LiskovSubstituiton/certo.cs`

No arquivo errado, a classe `Square` herda de `Rectangle`, mas oculta `Width` e `Height` com `new`:

```csharp
public class Square : Rectangle
{
    public new int Width
    {
        set { base.Width = base.Height = value; }
    }

    public new int Height
    {
        set { base.Width = base.Height = value; }
    }
}
```

### 4.4 Por que o exemplo errado está errado

`Rectangle` sugere um contrato implícito: largura e altura podem ser tratadas separadamente.

Mas `Square` muda esse comportamento:

- ao definir `Width`, ele altera `Height`;
- ao definir `Height`, ele altera `Width`.

Além disso, o exemplo da aula enfatiza outro detalhe técnico importante: o uso de `new` não sobrescreve o membro herdado; ele **oculta** o membro.

Por isso, estas duas situações não são equivalentes:

```csharp
Square sq = new Square();
sq.Width = 4;

Rectangle sq2 = new Square();
sq2.Width = 4;
```

Com `new`, a escolha do membro depende do tipo da variável. Isso torna o comportamento menos previsível e ajuda a expor a fragilidade da substituição.

### 4.5 Como a versão correta foi construída neste material

No arquivo `certo.cs`, o material didático foca em mostrar a diferença entre:

- **ocultar** com `new`;
- **participar do mesmo contrato** com `virtual` e `override`.

Trecho representativo:

```csharp
public class Rectangle2
{
    public virtual int Width { get; set; }
    public virtual int Height { get; set; }
}

public class Square2 : Rectangle2
{
    public override int Width
    {
        set { base.Width = base.Height = value; }
    }

    public override int Height
    {
        set { base.Width = base.Height = value; }
    }
}
```

### 4.6 O que torna essa versão “certa” dentro da proposta da aula

Neste material, a versão correta foi construída para destacar o mecanismo de substituição polimórfica da linguagem.

O ponto central é:

- com `new`, a subclasse cria uma segunda versão do membro;
- com `virtual` e `override`, a subclasse participa do mesmo contrato polimórfico da classe base.

Assim, a aula usa o exemplo para mostrar que o comportamento da substituição muda quando saímos da ocultação e passamos para a sobrescrita explícita.

### 4.7 O que mudou na prática

O efeito da mudança, neste projeto, foi:

- deixar a substituição mais explícita;
- mostrar a diferença entre tipo da variável e tipo real do objeto;
- destacar o papel de `virtual` e `override` no polimorfismo.

### 4.8 Espaço para imagem

> **Ilustração sugerida:** uma comparação entre “ocultação com `new`” e “sobrescrita com `override`”, destacando em qual ponto a chamada é resolvida.

---

## 5. Interface Segregation Principle (ISP)

### 5.1 Definição

O **Interface Segregation Principle** afirma que nenhuma classe deve ser forçada a depender de métodos que não usa.

Interfaces devem ser pequenas, específicas e compatíveis com as capacidades reais de quem as implementa.

### 5.2 O problema que o ISP tenta evitar

Quando uma interface reúne responsabilidades demais:

- classes simples precisam implementar métodos irrelevantes;
- surgem métodos artificiais;
- aparecem `NotImplementedException`;
- o contrato deixa de ser honesto.

### 5.3 Exemplo utilizado neste projeto

No projeto, o ISP foi trabalhado com:

- `SOLID/Aula04_InterfaceSegregation/errado.cs`
- `SOLID/Aula04_InterfaceSegregation/certo.cs`

Na versão errada, existe uma interface única:

```csharp
public interface IMachine2
{
    void Print(Document2 d);
    void Scan(Document2 d);
    void Fax(Document2 d);
}
```

E a classe `OldFashionedPrinter` é obrigada a implementar tudo isso.

### 5.4 Por que o exemplo errado está errado

`OldFashionedPrinter` representa uma impressora antiga. Ela deveria precisar apenas de impressão. No entanto, a interface `IMachine2` a obriga a declarar também:

- `Scan`
- `Fax`

O resultado é um contrato que empurra para a classe capacidades que ela não possui.

Esse tipo de estrutura costuma produzir implementações vazias ou exceções artificiais, que são sinais clássicos de violação de ISP.

### 5.5 Como o exemplo certo resolve

Na versão correta, a interface grande foi quebrada em contratos menores:

- `IPrinter`
- `IScanner`
- `IFax`

Trecho representativo:

```csharp
public interface IPrinter
{
    void Print(Document2 d);
}

public interface IScanner
{
    void Scan(Document2 d);
}

public interface IFax
{
    void Fax(Document2 d);
}
```

### 5.6 O papel das classes do exemplo

O código da aula organiza bem essa ideia:

- `PhotoCopier` implementa apenas `IPrinter` e `IScanner`;
- `IMultiFunctionDevice` recompõe os três contratos menores;
- `MultiFunctionPrinter` implementa o dispositivo multifuncional completo.

Isso torna a modelagem mais fiel à realidade das capacidades de cada máquina.

### 5.7 O que mudou na prática

A mudança estrutural foi:

- sair de uma interface “faz tudo”;
- para interfaces menores, mais honestas e reutilizáveis.

O efeito foi:

- remover dependências indevidas;
- melhorar a clareza do contrato;
- favorecer composição;
- evitar métodos sem sentido.

### 5.8 Observação importante sobre o exemplo

No `certo.cs`, a classe `MultiFunctionPrinter` também mostra uma ideia próxima de composição, ao receber no construtor:

- um `IPrinter`;
- um `IScanner`;
- um `IFax`.

Isso reforça bem a consequência natural do ISP: quando os contratos ficam menores, compor comportamentos se torna mais fácil.

### 5.9 Espaço para imagem

> **Ilustração sugerida:** uma interface grande sendo dividida em três cartões menores, depois recombinados conforme o tipo de máquina.

---

## 6. Dependency Inversion Principle (DIP)

### 6.1 Definição

O **Dependency Inversion Principle** afirma que módulos de alto nível não devem depender de módulos de baixo nível. Ambos devem depender de abstrações.

Em termos práticos:

- a regra de negócio não deveria conhecer detalhes internos do armazenamento;
- o fluxo principal não deveria depender diretamente da implementação concreta;
- os detalhes deveriam ficar atrás de contratos estáveis.

### 6.2 Exemplo utilizado neste projeto

No projeto, o DIP aparece em:

- `SOLID/Aula05_DependencyInversion/errado.cs`
- `SOLID/Aula05_DependencyInversion/certo.cs`

Na versão errada, a classe `Research2` recebe diretamente a classe concreta `RelationShips2`:

```csharp
public Research2(RelationShips2 relationships)
{
    var relations = relationships.Relations;

    foreach (var r in relations.Where(x => x.Item1.Name == "John" && x.Item2 == RelationShip.Parent))
    {
        WriteLine($"John has a child called {r.Item3.Name}");
    }
}
```

### 6.3 Por que o exemplo errado está errado

Esse trecho concentra duas dependências problemáticas:

1. `Research2` depende diretamente da classe concreta `RelationShips2`.
2. `Research2` depende também do formato interno do dado:
   - `List`
   - tupla
   - `Item1`
   - `Item2`
   - `Item3`

Ou seja, a regra de negócio não está apenas pedindo “quero os filhos de John”. Ela está conhecendo detalhes internos demais do mecanismo de armazenamento.

Se a estrutura mudar, a regra de negócio muda junto.

### 6.4 Como o exemplo certo resolve

Na versão correta, foi criada a abstração `IRelationshipBrowser`:

```csharp
public interface IRelationshipBrowser
{
    IEnumerable<Person> FindAllChildrenOf(string name);
}
```

Agora a classe `Research` depende da abstração:

```csharp
public Research(IRelationshipBrowser browser)
{
    foreach (var person in browser.FindAllChildrenOf("John"))
    {
        WriteLine($"John has a child called {person.Name}");
    }
}
```

E a classe concreta `RelationShips` implementa esse contrato:

```csharp
public class RelationShips : IRelationshipBrowser
{
    public IEnumerable<Person> FindAllChildrenOf(string name)
    {
        foreach (var relation in relations)
        {
            if (relation.Item1.Name == name && relation.Item2 == RelationShip2.Parent)
            {
                yield return relation.Item3;
            }
        }
    }
}
```

### 6.5 O que torna a solução melhor

Agora `Research`:

- não conhece `List`;
- não conhece tuplas;
- não conhece `Item1`, `Item2`, `Item3`;
- não sabe nem precisa saber onde ou como os dados são armazenados.

Ele conhece apenas a abstração que responde à pergunta de negócio de que ele precisa.

### 6.6 O que mudou na prática

A mudança foi profunda:

- antes, o alto nível dependia diretamente do baixo nível;
- agora, ambos dependem do mesmo contrato.

Isso inverte a direção do acoplamento e protege o código de negócio contra mudanças de infraestrutura.

### 6.7 Espaço para imagem

> **Ilustração sugerida:** um diagrama com `Research2 -> RelationShips2` no exemplo errado e `Research -> IRelationshipBrowser <- RelationShips` no exemplo correto.

---

## 7. Conclusão do Capítulo

Os princípios SOLID não devem ser tratados como frases decoradas para entrevista nem como um conjunto de mandamentos rígidos. O valor deles aparece quando observamos o impacto que causam na manutenção do software.

Neste material, cada princípio foi associado a um exemplo concreto:

- **SRP:** `JournalErrado` versus `JournalCerto` + `Persistence`
- **OCP:** `ProductFilter` versus `BetterFilter` com especificações
- **LSP:** `Rectangle`/`Square` com `new` versus `Rectangle2`/`Square2` com `virtual` e `override`
- **ISP:** `IMachine2` versus `IPrinter`, `IScanner` e `IFax`
- **DIP:** `Research2` acoplado a `RelationShips2` versus `Research` dependente de `IRelationshipBrowser`

Esse vínculo entre teoria e código é uma das melhores formas de estudar design. Quando o leitor consegue olhar para uma classe real e dizer “aqui há acoplamento demais”, “aqui a interface está inchada”, “aqui a subclasse quebrou o contrato”, o princípio deixa de ser abstração acadêmica e passa a se tornar ferramenta de raciocínio.

### 7.1 O valor pedagógico dos exemplos deste projeto

Os exemplos desta pasta não são úteis apenas por ilustrar o “antes” e o “depois”. Eles também mostram uma coisa muito importante: **cada princípio costuma aparecer como resposta a um desconforto estrutural**.

Em geral, o bom design não nasce porque alguém decorou um nome sofisticado. Ele nasce porque alguém percebeu um problema recorrente e procurou uma forma melhor de organizar o código.

Essa é a atitude correta diante do SOLID:

- primeiro, enxergar a fragilidade;
- depois, entender qual princípio ajuda a reduzi-la.

### 7.2 Espaço para imagem final

> **Ilustração sugerida:** uma página-resumo com as cinco letras de SOLID, cada uma conectada ao exemplo correspondente do projeto.

---

## Resumo Executivo

### Single Responsibility Principle

No exemplo do projeto, `JournalErrado` mistura entradas do diário com persistência. A versão correta separa `JournalCerto` e `Persistence`.

### Open-Closed Principle

No exemplo do projeto, `ProductFilter` cresce por adição de métodos. A versão correta usa `BetterFilter` com `ISpecification<T>`.

### Liskov Substitution Principle

No exemplo do projeto, `Square` oculta membros com `new`, enquanto a versão didática correta mostra a substituição por `virtual` e `override`.

### Interface Segregation Principle

No exemplo do projeto, `IMachine2` força impressoras simples a implementarem métodos desnecessários. A versão correta divide o contrato em interfaces menores.

### Dependency Inversion Principle

No exemplo do projeto, `Research2` conhece diretamente o armazenamento de `RelationShips2`. A versão correta faz `Research` depender de `IRelationshipBrowser`.

---

## Espaço Editorial para Recursos Visuais

### Sugestões de materiais complementares

1. Quadro comparativo entre “classe concreta” e “abstração”.
2. Diagrama de responsabilidade para a Aula 1.
3. Diagrama de extensão por especificações para a Aula 2.
4. Quadro visual explicando `new` versus `override` para a Aula 3.
5. Mapa de interfaces pequenas e composição para a Aula 4.
6. Setas de dependência antes e depois para a Aula 5.

---

## Fechamento

Estudar SOLID sem olhar para código concreto tende a transformar o assunto em teoria vaga. Por isso, este capítulo foi construído para dialogar diretamente com os exemplos da pasta `SOLID/`.

O ideal, daqui para frente, é que o leitor use este capítulo como guia de leitura do código e use o código como laboratório de leitura do capítulo.

É nesse movimento de ida e volta entre conceito e implementação que o design realmente começa a fazer sentido.
