# Capítulo 1 - SOLID

**por Robert C. Martin**

> **Base conceitual:** *Agile Principles, Patterns, and Practices in C#*  
> **Objetivo deste capítulo:** explicar os princípios SOLID com profundidade conceitual e leitura guiada dos exemplos desta pasta

---

## Prefácio

[⬆️ Voltar ao Sumário](#sumário)

Muita gente aprende orientação a objetos por camadas superficiais. Primeiro aprende a criar classes. Depois aprende herança. Em seguida descobre interfaces, abstrações, polimorfismo e alguns padrões. O problema é que, sem um critério de organização, todo esse conhecimento vira apenas um conjunto de ferramentas soltas.

É exatamente nesse ponto que SOLID se torna importante.

SOLID não é "mágica arquitetural", nem uma receita pronta para qualquer projeto. Também não é um conjunto de regras rígidas que devem ser obedecidas cegamente. SOLID é melhor entendido como um conjunto de princípios que ajudam o engenheiro de software a responder perguntas como:

- Esta classe está fazendo coisas demais?
- Estou quebrando código antigo sempre que adiciono uma regra nova?
- Minha herança realmente representa um contrato seguro?
- Esta interface está obrigando classes a mentirem sobre o que sabem fazer?
- Minha regra de negócio depende demais de detalhes concretos?

Quando essas perguntas não são feitas, o software até pode funcionar, mas começa a envelhecer mal. Mudanças pequenas passam a causar regressões. O código fica acoplado. A leitura fica cansativa. Os testes ficam mais caros. A evolução do sistema se torna tensa.

Este capítulo foi escrito para evitar esse estudo "decoreba". A proposta aqui é tratar SOLID como um assunto de engenharia, e não apenas de sintaxe.

---

## Como ler este capítulo

[⬆️ Voltar ao Sumário](#sumário)

Este material foi escrito em duas camadas ao mesmo tempo:

1. **Camada conceitual:** explica a ideia arquitetural por trás de cada princípio.
2. **Camada prática:** conecta essa ideia aos arquivos reais da pasta `SOLID/`.

Se você está começando, a melhor leitura é sequencial. Se você já tem alguma experiência, pode usar o texto como manual de consulta.

Ao longo do capítulo, sempre que aparecer um recurso de C# que pode não ser familiar, a explicação será feita no próprio fluxo da leitura. O objetivo é que o leitor não precise "saber tudo antes" para entender o design.

---

## Sumário

1. [O que é SOLID e por que ele existe](#1-o-que-é-solid-e-por-que-ele-existe)
2. [Conceitos de C# que ajudam a ler os exemplos](#2-conceitos-de-c-que-ajudam-a-ler-os-exemplos)
3. [Single Responsibility Principle (SRP)](#3-single-responsibility-principle-srp)
4. [Open-Closed Principle (OCP)](#4-open-closed-principle-ocp)
5. [Liskov Substitution Principle (LSP)](#5-liskov-substitution-principle-lsp)
6. [Interface Segregation Principle (ISP)](#6-interface-segregation-principle-isp)
7. [Dependency Inversion Principle (DIP)](#7-dependency-inversion-principle-dip)
8. [Como os princípios se conectam entre si](#8-como-os-princípios-se-conectam-entre-si)
9. [Erros comuns ao estudar SOLID](#9-erros-comuns-ao-estudar-solid)
10. [Conclusão](#10-conclusão)

---

## 1. O que é SOLID e por que ele existe

[⬆️ Voltar ao Sumário](#sumário)

SOLID é um acrônimo formado pelas iniciais de cinco princípios de design orientado a objetos:

- **S** - *Single Responsibility Principle*
- **O** - *Open-Closed Principle*
- **L** - *Liskov Substitution Principle*
- **I** - *Interface Segregation Principle*
- **D** - *Dependency Inversion Principle*

Esses princípios surgiram como resposta a um problema recorrente no desenvolvimento de software: sistemas que começam pequenos, funcionam bem no início, mas se tornam frágeis conforme evoluem.

É importante entender uma distinção fundamental:

- **funcionar** não é a mesma coisa que **ter bom design**;
- **compilar** não é a mesma coisa que **ser fácil de manter**;
- **ter herança e interface** não significa automaticamente **ter orientação a objetos bem aplicada**.

SOLID existe para ajudar a organizar o software de um jeito que reduza certos tipos de deterioração estrutural.

### 1.1 O tipo de problema que SOLID combate

[⬆️ Voltar ao Sumário](#sumário)

Sem bons princípios de design, começam a surgir sintomas como estes:

- classes longas e confusas;
- acúmulo de regras não relacionadas no mesmo lugar;
- necessidade constante de mexer em código antigo e estável;
- hierarquias de herança que parecem bonitas, mas se comportam mal;
- interfaces grandes demais;
- regras de negócio acopladas a armazenamento, framework ou infraestrutura.

Esses sintomas aumentam:

- o custo de manutenção;
- o risco de regressão;
- a dificuldade de teste;
- a dependência de conhecimento tácito da equipe;
- o tempo de onboarding de novos desenvolvedores.

### 1.2 SOLID não é uma religião

[⬆️ Voltar ao Sumário](#sumário)

Um erro comum é tratar SOLID como se cada princípio fosse uma lei absoluta. Não é assim que software real funciona.

Na prática:

- às vezes uma classe pequena pode concentrar duas tarefas sem causar dano real;
- às vezes abstrair cedo demais piora o projeto;
- às vezes criar interfaces demais só adiciona burocracia;
- às vezes a simplicidade atual vale mais do que uma arquitetura "preparada para tudo".

Portanto, a pergunta correta não é "estou obedecendo SOLID perfeitamente?", e sim:

**"Estou reduzindo acoplamento desnecessário e tornando o sistema mais fácil de evoluir?"**

Esse é o espírito certo.

---

## 2. Conceitos de C# que ajudam a ler os exemplos

[⬆️ Voltar ao Sumário](#sumário)

Antes de entrar nas cinco letras, vale alinhar alguns conceitos de linguagem que aparecem bastante nas aulas.

### 2.1 Classe

[⬆️ Voltar ao Sumário](#sumário)

Uma **classe** é um tipo que agrupa dados e comportamento.

Exemplo mental:

- `JournalCerto` representa um diário;
- `Product` representa um produto;
- `Research` representa uma regra de consulta;
- `Persistence` representa uma peça voltada a persistência.

Quando você vê:

```csharp
public class Product
{
    public string Name { get; set; }
}
```

isso significa que existe um tipo chamado `Product`, e que objetos desse tipo terão uma propriedade `Name`.

### 2.2 Interface

[⬆️ Voltar ao Sumário](#sumário)

Uma **interface** é um contrato.

Ela diz:

"qualquer classe que implementar isso precisa oferecer estes membros".

Exemplo:

```csharp
public interface IPrinter
{
    void Print(Document2 d);
}
```

Isso não diz **como** imprimir. Só diz que existe uma capacidade chamada `Print`.

Esse detalhe é central em SOLID, porque vários princípios dependem da ideia de separar:

- **o contrato**;
- **a implementação concreta**.

### 2.3 Herança

[⬆️ Voltar ao Sumário](#sumário)

Herança significa que uma classe deriva de outra.

Exemplo:

```csharp
public class Square : Rectangle
```

Aqui `Square` herda de `Rectangle`.

Mas herdar não significa apenas "reaproveitar código". Em orientação a objetos, herdar também implica uma promessa comportamental:

**a classe filha precisa continuar sendo utilizável como a classe pai, sem quebrar expectativas importantes.**

Essa ideia é a base do LSP.

### 2.4 `virtual`, `override` e `new`

[⬆️ Voltar ao Sumário](#sumário)

Essas três palavras aparecem no capítulo de LSP e podem confundir bastante.

- `virtual` diz que um membro da classe base pode ser redefinido pela classe filha.
- `override` redefine esse membro virtual na subclasse.
- `new`, nesse contexto, não cria objeto; ele **oculta** um membro herdado.

Exemplo conceitual:

```csharp
public virtual int Width { get; set; }
public override int Width { set { ... } }
public new int Width { set { ... } }
```

`override` participa do mesmo contrato polimórfico.
`new` cria uma segunda versão visível dependendo do tipo da variável.

Essa diferença é importante porque muita gente acha que `new` e `override` produzem o mesmo efeito. Não produzem.

### 2.5 `IEnumerable<T>`

[⬆️ Voltar ao Sumário](#sumário)

`IEnumerable<T>` significa, em termos simples:

"uma sequência de itens do tipo `T` que pode ser percorrida com `foreach`".

Exemplo:

```csharp
IEnumerable<Product> produtos
```

Isso não diz se a sequência é:

- lista;
- array;
- resultado de filtro;
- consulta lazy;
- dados vindos de algum outro lugar.

Só diz que você consegue iterá-la.

### 2.6 `yield return`

[⬆️ Voltar ao Sumário](#sumário)

`yield return` aparece nos exemplos de OCP e DIP.

Quando você vê:

```csharp
yield return i;
```

isso significa que o método não precisa montar uma lista inteira antes de devolver os resultados. Ele vai "entregando" os itens um por um à medida que são encontrados.

Isso é útil para:

- deixar o código mais expressivo;
- evitar alocação manual de lista em exemplos simples;
- representar uma sequência filtrada de forma elegante.

### 2.7 Tuplas e `Item1`, `Item2`, `Item3`

[⬆️ Voltar ao Sumário](#sumário)

No exemplo errado de DIP aparece algo assim:

```csharp
List<(Person2, RelationShip, Person2)>
```

Isso é uma lista de **tuplas**.

Cada tupla guarda três valores:

1. uma pessoa;
2. um tipo de relação;
3. outra pessoa.

Quando o código usa `Item1`, `Item2` e `Item3`, ele está acessando essas posições.

Didaticamente, esse exemplo é ótimo para mostrar um problema: o módulo de alto nível passa a conhecer detalhes demais da estrutura interna.

### 2.8 Valores padrão em parâmetros

[⬆️ Voltar ao Sumário](#sumário)

No exemplo de SRP aparece:

```csharp
bool overwrite = false
```

Isso significa que o parâmetro é opcional. Se quem chamar o método não passar esse argumento, o valor usado será `false`.

### 2.9 `readonly`

[⬆️ Voltar ao Sumário](#sumário)

Quando aparece:

```csharp
private readonly List<string> entries = new List<string>();
```

o `readonly` não torna a lista imutável. O que ele impede é a troca da referência após a construção do objeto.

Ou seja:

- você ainda pode adicionar itens na lista;
- mas não pode fazer `entries = outraLista` depois.

Esse é um detalhe que frequentemente confunde quem está começando.

---

<img width="1160" height="600" alt="image" src="https://github.com/user-attachments/assets/21077cbc-d481-4ce4-bbe3-43bed9ad6a2a" />


## 3. Single Responsibility Principle (SRP)

[⬆️ Voltar ao Sumário](#sumário)

### 3.1 Definição

[⬆️ Voltar ao Sumário](#sumário)

O **Single Responsibility Principle** afirma que uma classe deve ter **uma única razão para mudar**.

Essa formulação é melhor do que dizer "uma classe deve fazer uma coisa só", porque "uma coisa só" pode ser interpretado de maneira vaga demais.

A pergunta realmente útil é:

**"quais decisões de mudança impactam esta classe?"**

Se a resposta envolver múltiplos eixos diferentes, provavelmente a classe acumulou responsabilidades demais.

### 3.2 O que é uma "razão para mudar"

[⬆️ Voltar ao Sumário](#sumário)

Uma responsabilidade não é apenas uma função visível do código. Ela está ligada ao tipo de decisão que pode exigir alteração futura.

Exemplo:

- uma regra de negócio muda por causa do domínio;
- persistência muda por causa da forma de armazenamento;
- exibição muda por causa de interface ou formatação;
- integração muda por causa de protocolo externo.

Se uma mesma classe responde a todas essas pressões, ela virou um ponto de acoplamento estrutural.

### 3.3 Leitura guiada do exemplo errado

[⬆️ Voltar ao Sumário](#sumário)

Arquivos da aula:

- `SOLID/Aula01_SingleResponsibility/errado.cs`
- `SOLID/Aula01_SingleResponsibility/certo.cs`

No exemplo errado, a classe `JournalErrado` concentra:

- criação de entradas;
- remoção de entradas;
- representação textual;
- gravação em arquivo;
- carregamento de arquivo;
- indício de carregamento por `Uri`.

Trecho representativo:

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

### 3.4 Por que isso é uma violação de SRP

[⬆️ Voltar ao Sumário](#sumário)

O nome `JournalErrado` sugere um objeto de domínio: um diário.

Quando o leitor vê esse nome, ele espera que a classe cuide de:

- entradas do diário;
- organização dessas entradas;
- alguma forma de representar o diário.

Mas a classe também passa a cuidar de:

- caminho de arquivo;
- operação de leitura/escrita;
- mecanismo de persistência.

Aqui temos duas naturezas diferentes:

- **responsabilidade de domínio**;
- **responsabilidade de infraestrutura**.

Essas duas coisas mudam por razões diferentes. Se amanhã a persistência mudar para banco de dados, JSON, API ou armazenamento em nuvem, o diário em si não deveria precisar mudar.

### 3.5 Pontos de C# que o leitor talvez não conheça

[⬆️ Voltar ao Sumário](#sumário)

#### `override string ToString()`

[⬆️ Voltar ao Sumário](#sumário)

Toda classe em C# herda de `object`, e `object` possui um método `ToString()`.

Quando a classe faz:

```csharp
public override string ToString()
```

ela está dizendo:

"quero fornecer uma representação textual mais útil para este objeto".

#### `File.WriteAllText(...)`

[⬆️ Voltar ao Sumário](#sumário)

`File.WriteAllText` é uma chamada da biblioteca padrão do .NET que grava texto em um arquivo.

O detalhe didático importante aqui é que isso já é uma operação de infraestrutura. Ou seja: já é um forte sinal de que a classe saiu do campo do domínio e entrou em outro tipo de responsabilidade.

#### Método `static`

[⬆️ Voltar ao Sumário](#sumário)

No exemplo:

```csharp
public static JournalErrado Load(string filename)
```

`static` significa que o método pertence à classe, não a uma instância específica.

Você chama algo assim diretamente pela classe:

```csharp
JournalErrado.Load("arquivo.txt");
```

### 3.6 Como o exemplo certo melhora o design

[⬆️ Voltar ao Sumário](#sumário)

No exemplo correto, as responsabilidades são separadas:

- `JournalCerto` cuida do diário;
- `Persistence` cuida da gravação em arquivo.

Trecho representativo:

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

### 3.7 O que melhorou de verdade

[⬆️ Voltar ao Sumário](#sumário)

O ganho não é simplesmente "criar mais uma classe". O ganho estrutural é este:

- o diário voltou a cuidar apenas de comportamento de diário;
- a persistência ficou isolada;
- mudanças em arquivo não contaminam o domínio;
- a intenção de cada classe ficou mais clara.

### 3.8 Consequências práticas de SRP

[⬆️ Voltar ao Sumário](#sumário)

Quando o SRP é respeitado, normalmente melhoram:

- legibilidade;
- testabilidade;
- nomeação;
- reuso;
- segurança para manutenção.

### 3.9 Checklist mental para SRP

[⬆️ Voltar ao Sumário](#sumário)

Ao analisar uma classe, pergunte:

- esta classe responde a mais de um tipo de interesse do sistema?
- mudanças de interface, domínio e persistência cairiam no mesmo arquivo?
- o nome da classe ainda combina com tudo o que ela está fazendo?
- estou adicionando "só mais um método" que pertence a outro contexto?

Se essas perguntas começarem a incomodar, SRP provavelmente está pedindo atenção.

---

<img width="463" height="431" alt="image" src="https://github.com/user-attachments/assets/69a75dac-b209-4ea2-8cf2-66a39367c64b" />


## 4. Open-Closed Principle (OCP)

[⬆️ Voltar ao Sumário](#sumário)

### 4.1 Definição

[⬆️ Voltar ao Sumário](#sumário)

O **Open-Closed Principle** afirma que entidades de software devem estar **abertas para extensão, mas fechadas para modificação**.

Essa frase costuma ser mal interpretada. Ela não quer dizer "nunca mais toque no código". Ela quer dizer:

**o design deve preferir crescimento por adição de novas peças, e não por reedição constante do mesmo núcleo estável.**

### 4.2 O problema que OCP tenta evitar

[⬆️ Voltar ao Sumário](#sumário)

Quando toda nova regra exige mexer de novo na mesma classe:

- o risco de quebrar comportamento antigo cresce;
- o arquivo central vira gargalo;
- a revisão de código fica mais tensa;
- o sistema depende demais de um ponto único de mudança.

### 4.3 Leitura guiada do exemplo errado

[⬆️ Voltar ao Sumário](#sumário)

Arquivos da aula:

- `SOLID/Aula02_OpenClosed/errado.cs`
- `SOLID/Aula02_OpenClosed/certo.cs`

No exemplo errado, a classe `ProductFilter` possui vários métodos específicos:

```csharp
public class ProductFilter
{
    public static IEnumerable<Product1> FilterBySize(IEnumerable<Product1> products, Size1 size) { ... }
    public static IEnumerable<Product1> FilterByColor(IEnumerable<Product1> products, Color1 color) { ... }
    public static IEnumerable<Product1> FilterBySizeAndColor(IEnumerable<Product1> products, Size1 size, Color1 color) { ... }
}
```

### 4.4 Por que o design escala mal

[⬆️ Voltar ao Sumário](#sumário)

Hoje o problema é pequeno. Amanhã surgem perguntas como:

- filtrar por peso;
- filtrar por categoria;
- filtrar por cor e categoria;
- filtrar por tamanho e peso;
- filtrar por tudo isso junto.

Cada nova combinação força mais edição dentro da mesma classe.

Esse é o sinal clássico de violação de OCP:

**a estrutura cresce reabrindo o mesmo lugar repetidamente.**

### 4.5 Pontos de C# que aparecem no exemplo

[⬆️ Voltar ao Sumário](#sumário)

#### `enum`

[⬆️ Voltar ao Sumário](#sumário)

No código aparecem `Color1` e `Size1`.

Isso são **enumerações**, ou `enum`s. Elas representam conjuntos finitos de valores nomeados.

Exemplo:

```csharp
public enum Color1
{
    Red, Green, Blue
}
```

Isso é útil para modelar propriedades com opções conhecidas.

#### `IEnumerable<Product1>`

[⬆️ Voltar ao Sumário](#sumário)

O filtro recebe uma sequência de produtos e devolve uma sequência de produtos. Isso permite usar arrays, listas ou qualquer fonte iterável sem acoplar o método a um tipo concreto.

### 4.6 Leitura guiada do exemplo correto

[⬆️ Voltar ao Sumário](#sumário)

Na versão correta, a ideia muda completamente. Em vez de escrever um método para cada regra possível, o projeto cria uma abstração de regra:

```csharp
public interface ISpecification<T>
{
    bool IsSatisfied(T item);
}
```

E uma abstração de filtro:

```csharp
public interface IFilter<T>
{
    IEnumerable<T> Filter(IEnumerable<T> items, ISpecification<T> spec);
}
```

Depois cria regras concretas, como:

- `SizeSpecification`
- `ColorSpecification`

E um filtro genérico:

```csharp
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

### 4.7 O que aconteceu conceitualmente

[⬆️ Voltar ao Sumário](#sumário)

Antes:

- a classe de filtro conhecia todas as regras.

Depois:

- a classe de filtro conhece apenas o mecanismo de aplicar uma regra;
- as regras concretas ficam fora dela.

Ou seja, o crescimento do sistema passa a acontecer por **extensão de comportamento**, e não por acúmulo de `if`s e métodos especiais.

### 4.8 O que é `ISpecification<T>`

[⬆️ Voltar ao Sumário](#sumário)

Esse trecho pode ser novo para quem ainda não está confortável com generics:

```csharp
public interface ISpecification<T>
```

O `<T>` significa que a interface é genérica. Ela funciona para qualquer tipo:

- `ISpecification<Product>`
- `ISpecification<User>`
- `ISpecification<Order>`

A interface está dizendo:

"eu represento uma regra capaz de avaliar um item do tipo `T`".

### 4.9 O que `yield return` ajuda a mostrar aqui

[⬆️ Voltar ao Sumário](#sumário)

No filtro:

```csharp
yield return i;
```

o método vai devolvendo os itens aprovados um a um. Isso é elegante porque combina bem com a ideia de sequência filtrada.

Didaticamente, o ponto principal não é performance. O ponto principal é que o filtro produz uma sequência de saída sem precisar conhecer previamente quantos itens irão passar.

### 4.10 Consequência prática do OCP

[⬆️ Voltar ao Sumário](#sumário)

Agora, para adicionar uma regra nova, em vez de editar `BetterFilter`, você cria outra especificação:

```csharp
public class WeightSpecification : ISpecification<Product>
{
    public bool IsSatisfied(Product item) { ... }
}
```

Esse é o centro do princípio.

### 4.11 Checklist mental para OCP

[⬆️ Voltar ao Sumário](#sumário)

- estou adicionando novos `if`s e novos métodos toda vez que surge uma variante?
- existe um núcleo que deveria permanecer estável?
- consigo introduzir novo comportamento sem editar esse núcleo?
- estou crescendo por extensão ou por remendo?

---

<img width="1400" height="906" alt="image" src="https://github.com/user-attachments/assets/ed313065-e979-4d3f-9940-b3cff23224e2" />


## 5. Liskov Substitution Principle (LSP)

[⬆️ Voltar ao Sumário](#sumário)

### 5.1 Definição

[⬆️ Voltar ao Sumário](#sumário)

O **Liskov Substitution Principle** afirma que objetos de uma subclasse devem poder substituir objetos da classe base sem quebrar o comportamento esperado pelo código cliente.

Essa é talvez a letra mais mal compreendida do SOLID.

Muita gente acha que LSP é apenas:

- "usar herança direito";
- "declarar `virtual` e `override`";
- "não esquecer polimorfismo".

Mas o ponto central é mais profundo:

**LSP trata de contrato comportamental.**

### 5.2 O que significa "quebrar a substituição"

[⬆️ Voltar ao Sumário](#sumário)

Se um método funciona corretamente com `Rectangle`, ele deveria continuar funcionando corretamente se receber um objeto que afirma ser um `Rectangle`.

Se isso deixa de ser verdade, a substituição não é segura.

O problema não é sintático. O problema é semântico.

### 5.3 Leitura guiada do exemplo errado

[⬆️ Voltar ao Sumário](#sumário)

Arquivos da aula:

- `SOLID/Aula03_LiskovSubstituiton/errado.cs`
- `SOLID/Aula03_LiskovSubstituiton/certo.cs`

No arquivo errado:

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

### 5.4 O primeiro problema: o contrato do retângulo

[⬆️ Voltar ao Sumário](#sumário)

A classe `Rectangle` sugere um contrato implícito muito forte:

- largura e altura são lados independentes.

Ou seja:

```csharp
rect.Width = 4;
rect.Height = 5;
```

deveria produzir algo conceitualmente equivalente a um retângulo `4 x 5`.

Só que `Square` muda essa regra:

- ao alterar `Width`, ele altera `Height`;
- ao alterar `Height`, ele altera `Width`.

Assim, o objeto filho não respeita a expectativa associada ao pai.

### 5.5 O segundo problema: `new` oculta membro

[⬆️ Voltar ao Sumário](#sumário)

No exemplo errado, o código usa `new`:

```csharp
public new int Width
```

Aqui `new` não é "criar objeto".
Aqui `new` significa **ocultar o membro herdado**.

Isso faz surgir uma diferença perigosa entre:

```csharp
Square sq = new Square();
sq.Width = 4;

Rectangle sq2 = new Square();
sq2.Width = 4;
```

Com `new`, o membro escolhido depende do **tipo da variável**, não apenas do tipo real do objeto.

Isso torna o comportamento mais surpreendente e ajuda a expor a fragilidade da modelagem.

### 5.6 O que a aula "certa" realmente ensina

[⬆️ Voltar ao Sumário](#sumário)

No arquivo `certo.cs`, o projeto muda para `virtual` e `override`:

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

Isso ensina muito bem uma diferença de linguagem:

- `new` oculta;
- `override` sobrescreve um membro virtual;
- `virtual` + `override` formam polimorfismo real.

### 5.7 A nuance importante: isso explica polimorfismo, mas não resolve totalmente o LSP

[⬆️ Voltar ao Sumário](#sumário)

Aqui está um ponto importante para o leitor mais atento:

o uso de `override` melhora o mecanismo técnico de substituição, mas **não elimina automaticamente o problema conceitual de modelagem** entre `Rectangle` e `Square`.

Por quê?

Porque o contrato comportamental continua suspeito:

- `Rectangle` sugere independência entre largura e altura;
- `Square` continua impondo dependência entre elas.

Então, do ponto de vista estritamente conceitual:

- o arquivo `certo.cs` é ótimo para explicar **polimorfismo correto**;
- mas a relação `Square : Rectangle2` continua sendo uma escolha que merece crítica se o objetivo for modelagem perfeita de LSP.

Essa honestidade conceitual é importante num material didático.

### 5.8 Qual seria uma direção de modelagem melhor?

[⬆️ Voltar ao Sumário](#sumário)

Em muitos projetos, a melhor solução não é fazer `Square` herdar `Rectangle`.

Uma modelagem melhor pode ser:

- ambos herdarem de uma abstração comum, como `Shape`;
- ou ambos implementarem um contrato mais genérico, como algo que calcula área;
- ou serem tipos separados sem relação de herança direta.

Ou seja, às vezes o problema não é "como sobrescrever", mas sim:

**"essa herança deveria existir?"**

Essa é uma pergunta de design muito mais madura.

### 5.9 Checklist mental para LSP

[⬆️ Voltar ao Sumário](#sumário)

- a subclasse mantém as expectativas principais da classe base?
- o código cliente consegue tratá-la como base sem comportamento surpresa?
- a herança representa um contrato real, ou apenas reaproveitamento?
- estou tentando forçar uma taxonomia que parece elegante, mas não respeita semântica?

---

<img width="579" height="530" alt="image" src="https://github.com/user-attachments/assets/f6bef253-b7f3-40bb-8fac-b3084ca3d5ce" />


## 6. Interface Segregation Principle (ISP)

[⬆️ Voltar ao Sumário](#sumário)

### 6.1 Definição

[⬆️ Voltar ao Sumário](#sumário)

O **Interface Segregation Principle** afirma que nenhuma classe deve ser forçada a depender de métodos que não usa.

Traduzindo para uma linguagem mais direta:

**interfaces devem ser pequenas, específicas e honestas.**

### 6.2 O problema que ISP combate

[⬆️ Voltar ao Sumário](#sumário)

Quando uma interface fica grande demais:

- classes simples são obrigadas a implementar coisas sem sentido;
- surgem métodos vazios;
- aparecem `NotImplementedException`s artificiais;
- o contrato deixa de representar capacidade real.

### 6.3 Leitura guiada do exemplo errado

[⬆️ Voltar ao Sumário](#sumário)

Arquivos da aula:

- `SOLID/Aula04_InterfaceSegregation/errado.cs`
- `SOLID/Aula04_InterfaceSegregation/certo.cs`

No exemplo errado existe uma interface única:

```csharp
public interface IMachine2
{
    void Print(Document2 d);
    void Scan(Document2 d);
    void Fax(Document2 d);
}
```

Isso força qualquer implementação de `IMachine2` a declarar os três comportamentos.

### 6.4 Por que isso é ruim

[⬆️ Voltar ao Sumário](#sumário)

Uma impressora antiga talvez só imprima. Se ela é obrigada a implementar:

- `Scan`;
- `Fax`;

então o contrato está mentindo.

O problema não é só "ter métodos a mais". O problema é que a interface deixou de expressar capacidades reais e passou a impor um pacote artificial de obrigações.

### 6.5 O sinal de alerta: `NotImplementedException`

[⬆️ Voltar ao Sumário](#sumário)

No exemplo errado, `OldFashionedPrinter` acaba com membros assim:

```csharp
public void Scan(Document2 d)
{
    throw new NotImplementedException();
}
```

Esse tipo de implementação costuma ser um cheiro forte de design ruim.

Nem toda exceção indica problema de modelagem, claro. Mas quando uma classe precisa lançar `NotImplementedException` apenas porque o contrato a obrigou a fingir capacidades que ela não tem, o ISP provavelmente foi violado.

### 6.6 Leitura guiada do exemplo correto

[⬆️ Voltar ao Sumário](#sumário)

A correção divide o contrato grande em contratos pequenos:

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

Depois compõe esses contratos:

```csharp
public interface IMultiFunctionDevice : IPrinter, IScanner, IFax
{
}
```

### 6.7 O que significa "uma interface herdando outra"

[⬆️ Voltar ao Sumário](#sumário)

Isso pode soar estranho para iniciantes.

Quando o código faz:

```csharp
public interface IMultiFunctionDevice : IPrinter, IScanner, IFax
```

ele está dizendo:

"qualquer coisa que seja um `IMultiFunctionDevice` também precisa cumprir os contratos de impressão, scanner e fax".

Ou seja, uma interface também pode compor outras interfaces.

### 6.8 O papel de `PhotoCopier`

[⬆️ Voltar ao Sumário](#sumário)

No exemplo correto:

```csharp
public class PhotoCopier : IPrinter, IScanner
```

isso comunica algo muito bom sobre o design:

- a classe implementa só o que faz sentido para ela;
- o contrato combina com a capacidade real do objeto.

Essa aderência entre contrato e realidade é o que ISP quer preservar.

### 6.9 O papel de `MultiFunctionPrinter`

[⬆️ Voltar ao Sumário](#sumário)

A classe `MultiFunctionPrinter` recebe no construtor:

- um `IPrinter`;
- um `IScanner`;
- um `IFax`.

Isso é interessante por dois motivos:

1. mostra ISP na prática;
2. aproxima o leitor da ideia de **composição**.

A classe não precisa implementar tudo sozinha do zero. Ela pode delegar cada responsabilidade a uma dependência especializada.

### 6.10 O que isso ensina além do ISP

[⬆️ Voltar ao Sumário](#sumário)

Essa aula também ensina uma lição arquitetural valiosa:

**contratos menores favorecem composição.**

Quando as interfaces ficam específicas:

- fica mais fácil reutilizar implementações;
- fica mais fácil montar objetos maiores a partir de capacidades menores;
- fica mais fácil trocar uma parte sem mexer em todas as outras.

### 6.11 Checklist mental para ISP

[⬆️ Voltar ao Sumário](#sumário)

- esta interface está agrupando capacidades demais?
- alguma implementação precisa inventar comportamento que não tem?
- estou vendo `NotImplementedException` por causa de contrato ruim?
- posso quebrar a interface em contratos menores e mais honestos?a

---

<img width="640" height="482" alt="image" src="https://github.com/user-attachments/assets/d1e86299-46e3-488d-9667-e412cbf8295c" />


## 7. Dependency Inversion Principle (DIP)

[⬆️ Voltar ao Sumário](#sumário)

### 7.1 Definição

[⬆️ Voltar ao Sumário](#sumário)

O **Dependency Inversion Principle** afirma que módulos de alto nível não devem depender de módulos de baixo nível. Ambos devem depender de abstrações.

Também afirma que abstrações não devem depender de detalhes; detalhes devem depender de abstrações.

Traduzindo:

- a regra de negócio não deveria ficar amarrada à tecnologia concreta;
- o código que expressa intenção do sistema deveria depender de contratos estáveis;
- o detalhe de armazenamento, framework ou acesso deveria ficar atrás desses contratos.

### 7.2 O que são alto nível e baixo nível?

[⬆️ Voltar ao Sumário](#sumário)

Essa distinção é importante:

- **alto nível**: parte do sistema que expressa política, regra, intenção de negócio;
- **baixo nível**: parte que lida com detalhe técnico, armazenamento, transporte, infraestrutura, formato de dados.

No exemplo da aula:

- `Research2` e `Research` representam o alto nível;
- `RelationShips2` e `RelationShips` representam o baixo nível.

### 7.3 Leitura guiada do exemplo errado

[⬆️ Voltar ao Sumário](#sumário)

Arquivos da aula:

- `SOLID/Aula05_DependencyInversion/errado.cs`
- `SOLID/Aula05_DependencyInversion/certo.cs`

No exemplo errado:

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

### 7.4 Onde está a violação

[⬆️ Voltar ao Sumário](#sumário)

O problema aparece em duas camadas.

#### Primeira camada: dependência concreta

[⬆️ Voltar ao Sumário](#sumário)

`Research2` depende diretamente de `RelationShips2`.

Ou seja, a regra de alto nível conhece a classe concreta de baixo nível.

#### Segunda camada: dependência do formato interno

[⬆️ Voltar ao Sumário](#sumário)

Além disso, `Research2` conhece:

- que existe uma lista;
- que a lista guarda tuplas;
- que o pai está em `Item1`;
- que o tipo da relação está em `Item2`;
- que a outra pessoa está em `Item3`.

Isso significa que o alto nível não está perguntando:

"quais são os filhos de John?"

Ele está fazendo algo muito mais frágil:

"deixa eu inspecionar manualmente sua estrutura interna para descobrir isso".

### 7.5 Ponto de C# que pode não ser familiar: LINQ `Where`

[⬆️ Voltar ao Sumário](#sumário)

No trecho:

```csharp
relations.Where(x => x.Item1.Name == "John" && x.Item2 == RelationShip.Parent)
```

`Where` é um operador LINQ de filtragem.

Ele recebe uma função que responde `true` ou `false` para cada item, e mantém só os itens aprovados.

Aqui a função está dizendo:

- o primeiro elemento da tupla tem nome `"John"`;
- o tipo da relação é `Parent`.

Do ponto de vista didático, esse trecho é ótimo porque mostra uma regra de negócio lendo estrutura interna demais.

### 7.6 Leitura guiada do exemplo correto

[⬆️ Voltar ao Sumário](#sumário)

Na solução correta, surge uma abstração:

```csharp
public interface IRelationshipBrowser
{
    IEnumerable<Person> FindAllChildrenOf(string name);
}
```

Agora o alto nível depende dela:

```csharp
public Research(IRelationshipBrowser browser)
{
    foreach (var person in browser.FindAllChildrenOf("John"))
    {
        WriteLine($"John has a child called {person.Name}");
    }
}
```

E o baixo nível implementa o contrato:

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

### 7.7 O que mudou estruturalmente

[⬆️ Voltar ao Sumário](#sumário)

Antes:

- o alto nível conhecia a implementação concreta e o formato interno.

Depois:

- o alto nível conhece só a pergunta de negócio de que precisa;
- o baixo nível continua livre para armazenar como quiser;
- a abstração faz a ponte entre os dois.

Essa inversão é o coração do DIP.

### 7.8 O que é "inversão" aqui?

[⬆️ Voltar ao Sumário](#sumário)

O nome pode soar abstrato. A inversão é a seguinte:

em um design ingênuo, o alto nível aponta para o baixo nível.

Com DIP, ambos apontam para uma abstração comum:

- o alto nível usa a abstração;
- o baixo nível implementa a abstração.

Assim, a direção da dependência deixa de seguir a implementação concreta e passa a seguir o contrato.

### 7.9 Relação com injeção de dependência

[⬆️ Voltar ao Sumário](#sumário)

Muita gente confunde DIP com "usar framework de DI".

Não são a mesma coisa.

- **DIP** é um princípio de design.
- **injeção de dependência** é uma técnica de fornecer dependências.
- **container de DI** é uma ferramenta para automatizar essa técnica.

Você pode aplicar DIP sem framework nenhum, como o exemplo faz ao passar `IRelationshipBrowser` no construtor.

### 7.10 Checklist mental para DIP

[⬆️ Voltar ao Sumário](#sumário)

- a regra de negócio depende diretamente de uma classe concreta?
- o alto nível enxerga detalhes de armazenamento, HTTP, banco ou framework?
- o que o consumidor realmente precisa é um contrato ou uma implementação específica?
- se a infraestrutura mudar, o alto nível continua igual?

---

## 8. Como os princípios se conectam entre si

[⬆️ Voltar ao Sumário](#sumário)

Um erro didático comum é estudar as cinco letras como se fossem cinco temas isolados. Na prática, elas se reforçam.

### 8.1 SRP e OCP

[⬆️ Voltar ao Sumário](#sumário)

Quando classes têm responsabilidades mais bem separadas:

- fica mais fácil estender partes do sistema;
- fica menos necessário reabrir uma classe central toda hora.

Ou seja, SRP frequentemente prepara o terreno para OCP.

### 8.2 ISP e DIP

[⬆️ Voltar ao Sumário](#sumário)

Quando interfaces são pequenas e específicas:

- fica mais fácil depender apenas do contrato necessário;
- o alto nível deixa de carregar dependências desnecessárias.

Ou seja, ISP fortalece DIP.

### 8.3 LSP como guardião da abstração

[⬆️ Voltar ao Sumário](#sumário)

Se abstrações e heranças não respeitam contrato comportamental, toda a arquitetura acima delas fica instável.

LSP ajuda a garantir que:

- a abstração prometida realmente pode ser usada com segurança;
- a polimorfia não é apenas formal, mas confiável.

### 8.4 A visão madura

[⬆️ Voltar ao Sumário](#sumário)

Em sistemas reais, o objetivo não é "aplicar cinco regras".

O objetivo é:

- reduzir acoplamento desnecessário;
- aumentar clareza;
- preservar estabilidade do sistema diante da mudança;
- permitir evolução com menos medo.

SOLID é um conjunto de lentes para isso.

---

## 9. Erros comuns ao estudar SOLID

[⬆️ Voltar ao Sumário](#sumário)

### 9.1 Criar interface para tudo

[⬆️ Voltar ao Sumário](#sumário)

Nem toda classe precisa nascer com interface.

Interface faz sentido quando:

- há múltiplas implementações plausíveis;
- o consumidor deve depender de contrato;
- a abstração melhora o desenho;
- há fronteira arquitetural importante.

Criar interface só por ritual pode produzir burocracia.

### 9.2 Confundir "muitos arquivos" com "bom design"

[⬆️ Voltar ao Sumário](#sumário)

Separar tudo em dezenas de classes minúsculas não garante qualidade.

Boa separação é aquela que:

- reduz confusão;
- melhora o contrato;
- deixa a mudança mais localizada.

### 9.3 Achar que herança é sempre reaproveitamento elegante

[⬆️ Voltar ao Sumário](#sumário)

Muitas violações de LSP nascem porque alguém tentou usar herança apenas para evitar duplicação.

Herança não é só compartilhamento de código. Ela cria vínculo semântico forte.

### 9.4 Aplicar abstrações cedo demais

[⬆️ Voltar ao Sumário](#sumário)

Se o sistema ainda não mostrou variação real, às vezes uma abstração antecipada só aumenta ruído.

Boa engenharia também sabe esperar o momento certo.

### 9.5 Decorar definição sem aprender a reconhecer sintomas

[⬆️ Voltar ao Sumário](#sumário)

Saber repetir:

"aberto para extensão, fechado para modificação"

não significa que você sabe enxergar um ponto de fragilidade no código.

O aprendizado real começa quando você consegue olhar um arquivo e perceber:

- onde está o acoplamento;
- onde está o contrato desonesto;
- onde a responsabilidade foi misturada;
- onde a abstração ficou fraca.

---

## 10. Conclusão

[⬆️ Voltar ao Sumário](#sumário)

SOLID não foi criado para deixar o código "mais sofisticado". Foi criado para ajudar o software a envelhecer melhor.

Neste projeto, cada aula mostra um tipo de fragilidade muito comum:

- **SRP:** uma classe começa a misturar domínio e persistência;
- **OCP:** novas regras empurram mudanças para o mesmo arquivo central;
- **LSP:** uma herança parece válida, mas quebra a expectativa do contrato;
- **ISP:** uma interface grande demais obriga classes a fingirem capacidades;
- **DIP:** o alto nível passa a depender da implementação concreta e do formato interno dos dados.

Se o leitor terminar este capítulo com uma mudança de postura, o objetivo já foi cumprido. Essa mudança é:

em vez de olhar apenas para "o que o código faz", começar a perguntar também:

- **o que este código está acoplando?**
- **que tipo de mudança ele facilita?**
- **que tipo de mudança ele torna perigosa?**
- **o contrato que ele promete é honesto?**

É nesse ponto que design deixa de ser decoração conceitual e passa a se tornar ferramenta de engenharia.

---

## Resumo Executivo

[⬆️ Voltar ao Sumário](#sumário)

### SRP

[⬆️ Voltar ao Sumário](#sumário)

Uma classe deve ter uma única razão para mudar. No projeto, `JournalErrado` mistura domínio com persistência. `JournalCerto` e `Persistence` separam essas preocupações.

### OCP

[⬆️ Voltar ao Sumário](#sumário)

O sistema deve crescer preferencialmente por extensão. No projeto, `ProductFilter` acumula filtros específicos, enquanto `BetterFilter` passa a receber regras externas via `ISpecification<T>`.

### LSP

[⬆️ Voltar ao Sumário](#sumário)

A subclasse precisa respeitar o contrato da classe base. No projeto, `Square` expõe a fragilidade da relação com `Rectangle`. O uso de `override` melhora o polimorfismo, mas não elimina automaticamente o problema conceitual da modelagem.

### ISP

[⬆️ Voltar ao Sumário](#sumário)

Interfaces devem ser pequenas e específicas. No projeto, `IMachine2` força implementações artificiais; `IPrinter`, `IScanner` e `IFax` tornam o contrato mais honesto.

### DIP

[⬆️ Voltar ao Sumário](#sumário)

Módulos de alto nível devem depender de abstrações. No projeto, `Research2` conhece detalhes de `RelationShips2`; `Research` passa a depender de `IRelationshipBrowser`.

---

## Fechamento

[⬆️ Voltar ao Sumário](#sumário)

O uso ideal deste capítulo é em conjunto com os arquivos da pasta `SOLID/`. Leia a explicação, abra o código, volte ao texto, compare o exemplo errado com o exemplo certo e tente verbalizar, com suas próprias palavras, qual fragilidade estrutural cada aula está combatendo.

Quando você consegue fazer isso, SOLID deixa de ser um conjunto de frases famosas e começa a virar raciocínio de engenharia.
