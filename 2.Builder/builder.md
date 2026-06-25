# Capítulo 2 - Builder

**por Erich Gamma, Richard Helm, Ralph Johnson e John Vlissides (Gang of Four - GoF)**

> **Livro de referência principal:** *Design Patterns: Elements of Reusable Object-Oriented Software*
> **Autores da obra-base:** Erich Gamma, Richard Helm, Ralph Johnson e John Vlissides
> **Objetivo deste capítulo:** explicar o padrão `Builder` dentro da **Gamma Categorization**, mostrar o problema que ele resolve e conectar a teoria ao exemplo atual de HTML em `SemBuilder.cs` e `Builder.cs`

---

## Prefácio

[⬆️ Voltar ao Sumário](#sumário)

Uma das maiores dificuldades ao estudar Design Patterns é perceber que quase nenhum padrão existe para “deixar o código bonito”. Padrões existem porque certos problemas se repetem em sistemas reais: criação confusa de objetos, acoplamento excessivo, dependência de classes concretas, estruturas difíceis de evoluir e colaboração mal distribuída entre componentes.

O `Builder` faz parte exatamente desse universo. Ele não é um truque de sintaxe, não é uma classe utilitária e não é apenas “um jeito mais chique de usar construtor”. Ele é uma resposta de design para um problema muito específico:

**como construir objetos complexos sem forçar o código cliente a conhecer e executar cada detalhe dessa montagem?**

Este capítulo foi escrito para responder isso com calma, no mesmo estilo dos outros materiais do projeto:

- primeiro, construindo o mapa conceitual;
- depois, explicando o problema;
- em seguida, conectando a teoria ao exemplo concreto;
- por fim, destacando custo, benefício e armadilhas.

---

## Como usar este capítulo

[⬆️ Voltar ao Sumário](#sumário)

Leia este material com quatro perguntas na cabeça:

- **O que isso representa?** Um padrão criacional voltado à montagem de objetos complexos.
- **Quando eu usaria isso?** Quando a criação de um objeto fica longa, repetitiva, confusa ou cheia de combinações.
- **O que isso custa?** Mais tipos, mais abstração e uma pequena camada extra de design.
- **Qual erro é comum aqui?** Confundir o padrão `Builder` com `System.Text.StringBuilder`, ou usar `Builder` quando um construtor simples resolveria melhor.

Se você está estudando a aula agora, pense neste capítulo como complemento direto para os dois primeiros exemplos práticos:

- `2.Builder/Aula01_builder/SemBuilder.cs`
- `2.Builder/Aula01_builder/Builder.cs`

---

## Sumário

1. [Gamma Categorization e o lugar do Builder](#1-gamma-categorization-e-o-lugar-do-builder)
2. [Overview do Builder](#2-overview-do-builder)
3. [O problema: Life Without Builder](#3-o-problema-life-without-builder)
4. [A solução: Builder](#4-a-solução-builder)
5. [Anatomia do padrão no sentido GoF](#5-anatomia-do-padrão-no-sentido-gof)
6. [Leitura guiada do exemplo atual](#6-leitura-guiada-do-exemplo-atual)
7. [Quando usar Builder](#7-quando-usar-builder)
8. [Custos e trade-offs](#8-custos-e-trade-offs)
9. [Erros comuns e confusões frequentes](#9-erros-comuns-e-confusões-frequentes)
10. [Comparações importantes](#10-comparações-importantes)
11. [Conclusão](#11-conclusão)
12. [Referências bibliográficas](#12-referências-bibliográficas)

---

## 1. Gamma Categorization e o lugar do Builder

[⬆️ Voltar ao Sumário](#sumário)

O livro clássico do GoF organiza os padrões em três grandes famílias. Essa divisão é conhecida como **Gamma Categorization**, em referência a **Erich Gamma**, um dos autores da obra.

As três famílias são:

- **Creational Patterns**: focados em criação de objetos;
- **Structural Patterns**: focados em composição de classes e objetos;
- **Behavioral Patterns**: focados em comunicação, decisão e comportamento.

Essa organização é importante porque impede o estudo decorativo. Em vez de perguntar apenas “qual padrão eu conheço?”, a classificação força uma pergunta melhor:

**que tipo de problema de design eu tenho na mão?**

No caso do `Builder`, o problema está na **criação**. Por isso ele pertence ao grupo dos **Creational Patterns**.

Mas ele não trata de criação no mesmo sentido de `Factory Method` ou `Abstract Factory`.

- `Factory` tende a responder melhor a pergunta: **qual objeto concreto deve nascer?**
- `Builder` tende a responder melhor a pergunta: **como esse objeto complexo deve ser montado?**

Essa diferença é conceitual, mas muda muito a arquitetura.

**Como interpretar essa distinção:** um padrão criacional não fala apenas sobre instanciar com `new`. Ele fala sobre controlar o nascimento do objeto de forma que o cliente não precise carregar responsabilidade demais. O `Builder` faz isso separando o processo de montagem da forma final do objeto.

---

## 2. Overview do Builder

[⬆️ Voltar ao Sumário](#sumário)

Em termos clássicos do GoF, o `Builder` existe para **separar a construção de um objeto complexo de sua representação**, permitindo que o mesmo processo de construção produza representações diferentes.

Traduzindo isso para linguagem mais direta:

- existe um objeto final que não é trivial de montar;
- essa montagem possui várias etapas;
- o cliente não deveria conhecer todos os detalhes dessas etapas;
- portanto, criamos um objeto especializado em conduzir a construção.

O ganho não é apenas “escrever menos”. O ganho principal é:

- melhorar a legibilidade;
- centralizar a lógica de montagem;
- reduzir duplicação;
- permitir evolução mais segura da construção;
- expressar intenção com mais clareza.

### 2.1 A pergunta que o Builder responde

[⬆️ Voltar ao Sumário](#sumário)

O `Builder` entra em cena quando o problema real é este:

**“Meu objeto final até pode ser criado, mas o processo de criação está feio, repetitivo, frágil ou difícil de ler.”**

Exemplos comuns:

- geração de HTML;
- montagem de consultas complexas;
- criação de documentos, relatórios ou payloads;
- construção de objetos com muitas partes opcionais;
- composição de estruturas em árvore;
- APIs que precisam ser lidas quase como uma descrição de intenção.

### 2.2 Builder não é StringBuilder

[⬆️ Voltar ao Sumário](#sumário)

Esse é um ponto fundamental para não se perder na aula.

Em `SemBuilder.cs`, você usa `System.Text.StringBuilder`. Isso **não** é o padrão de projeto `Builder`. É apenas uma classe da biblioteca do .NET para montar texto de forma eficiente.

As diferenças são:

- `StringBuilder` é uma **estrutura mutável para texto**;
- `Builder Pattern` é uma **estratégia de design para construção de objetos complexos**.

No exemplo da aula:

- sem o padrão Builder, você usa `StringBuilder` para montar HTML “na unha”;
- com o padrão Builder, você cria tipos (`HtmlElement`, `HtmlBuilder`) para modelar a estrutura e esconder os detalhes da montagem.

**Regra prática:** `StringBuilder` resolve custo de concatenação. `Builder Pattern` resolve custo cognitivo e estrutural da criação.

---

## 3. O problema: Life Without Builder

[⬆️ Voltar ao Sumário](#sumário)

No arquivo `SemBuilder.cs`, a ideia da aula é mostrar como fica a criação de HTML quando o cliente faz tudo manualmente.

```csharp
var hello = "hello";
var sb = new System.Text.StringBuilder();
sb.Append("<p>");
sb.Append(hello);
sb.Append("</p>");
WriteLine(sb);

var words = new[] { "hello", "world" };
sb.Clear();
sb.Append("<ul>");
foreach (var word in words)
{
    sb.AppendFormat("<li>{0}</li>", word);
}
sb.Append("</ul>");
WriteLine(sb);
```

À primeira vista isso parece aceitável. E para um exemplo pequeno, realmente é. O ponto da aula não é dizer que esse código “está proibido”. O ponto é mostrar o que acontece quando esse estilo começa a crescer.

### 3.1 O que o cliente está fazendo aqui

[⬆️ Voltar ao Sumário](#sumário)

No exemplo acima, o código cliente:

- sabe quais tags abrir;
- sabe em que ordem abrir;
- sabe quais tags fechar;
- sabe o formato de cada item;
- sabe como iterar;
- sabe como transformar tudo em string final.

Ou seja: o cliente não está apenas “pedindo uma lista HTML”.

Ele está **executando manualmente o processo inteiro de montagem**.

### 3.2 O que começa a dar errado quando isso cresce

[⬆️ Voltar ao Sumário](#sumário)

Esse estilo tem alguns custos claros:

- mistura intenção com detalhe operacional;
- espalha conhecimento de HTML no código cliente;
- dificulta reutilização da montagem;
- facilita esquecer abertura, fechamento ou ordem correta de partes;
- piora a legibilidade quando surgem estruturas aninhadas.

Se você quiser depois montar:

- listas dentro de listas;
- seções com cabeçalho e rodapé;
- atributos opcionais;
- estruturas condicionais;
- múltiplas formas de renderização;

esse estilo começa a escalar mal.

**Como interpretar o exemplo:** o problema não é usar `StringBuilder`. O problema é que a responsabilidade de construção está no lugar errado. O cliente virou “mestre de obras” de algo que deveria ser encapsulado.

**Armadilha comum:** olhar `SemBuilder.cs` e concluir que a aula está criticando concatenação de string. Não. A crítica verdadeira é sobre **responsabilidade de montagem**.

---

## 4. A solução: Builder

[⬆️ Voltar ao Sumário](#sumário)

No arquivo `Builder.cs`, a aula muda o modelo mental. Em vez de pensar em HTML como texto bruto, o código passa a pensar em HTML como **estrutura**.

Essa mudança é o coração do padrão.

Antes:

- o cliente descrevia cada pedaço da string.

Agora:

- o cliente descreve a estrutura que quer;
- um objeto especializado cuida de montar essa estrutura;
- outro objeto sabe renderizá-la como string.

### 4.1 A ideia central do exemplo HTML

[⬆️ Voltar ao Sumário](#sumário)

O exemplo atual introduz duas ideias:

- `HtmlElement`: representa um nó da estrutura HTML;
- `HtmlBuilder`: representa o processo de construção dessa estrutura.

Isso já é uma aproximação do espírito do GoF:

- existe um **produto** sendo montado;
- existe um **builder** que monta o produto por etapas;
- o cliente interage com uma API mais semântica.

### 4.2 O ganho de leitura

[⬆️ Voltar ao Sumário](#sumário)

Compare mentalmente estas duas intenções:

Sem builder:

```csharp
sb.Append("<ul>");
foreach (var word in words)
{
    sb.AppendFormat("<li>{0}</li>", word);
}
sb.Append("</ul>");
```

Com builder:

```csharp
var builder = new HtmlBuilder("ul");
builder.AddChild("li", "hello");
builder.AddChild("li", "world");
WriteLine(builder.ToString());
```

No segundo caso, o cliente não fala mais em “abre tag, fecha tag, concatena string”. Ele fala em:

- criar uma lista;
- adicionar filhos;
- obter a representação final.

Isso é um salto de nível de abstração.

**Como interpretar o exemplo:** o `Builder` não elimina trabalho; ele o move para o lugar certo. O código cliente fica mais declarativo, enquanto a lógica concreta de montagem fica encapsulada.

---

## 5. Anatomia do padrão no sentido GoF

[⬆️ Voltar ao Sumário](#sumário)

Na formulação clássica, o padrão `Builder` costuma envolver estes participantes:

- **Product**: o objeto complexo final;
- **Builder**: a abstração que define etapas de construção;
- **ConcreteBuilder**: a implementação concreta dessas etapas;
- **Director**: o coordenador que sabe em que ordem chamar as etapas;
- **Client**: quem inicia a construção e consome o resultado.

### 5.1 Mapeando isso para o seu exemplo

[⬆️ Voltar ao Sumário](#sumário)

No seu caso atual:

- `HtmlElement` funciona como o **Product**;
- `HtmlBuilder` funciona como o **ConcreteBuilder**;
- o `Demo/Main` funciona como o **Client**;
- o **Director** não aparece como classe separada.

Isso é importante: muitas implementações modernas de `Builder` em C# **não usam um Director explícito**. O próprio cliente chama o builder diretamente.

Portanto, a ausência de `Director` no exemplo **não significa** que o padrão deixou de existir. Significa apenas que a versão usada na aula é uma versão mais enxuta.

### 5.2 O que é o Director e por que ele nem sempre aparece

[⬆️ Voltar ao Sumário](#sumário)

O `Director` é útil quando:

- existe uma sequência fixa de passos;
- você quer encapsular essa sequência;
- a mesma receita de montagem precisa ser reaproveitada várias vezes.

Exemplo mental:

- “montar relatório padrão”;
- “montar pedido premium”;
- “montar configuração default do sistema”.

No seu exemplo HTML, como o cliente está chamando poucos passos diretamente, o `Director` seria mais barulho do que ajuda.

**Regra prática:** se a construção já ficou legível e o cliente não está repetindo uma receita complexa, um `Director` separado pode ser opcional.

---

## 6. Leitura guiada do exemplo atual

[⬆️ Voltar ao Sumário](#sumário)

### 6.1 `HtmlElement`

[⬆️ Voltar ao Sumário](#sumário)

`HtmlElement` existe para modelar a estrutura, não a montagem.

Ele concentra:

- o nome da tag (`Name`);
- o conteúdo textual (`Text`);
- os elementos filhos (`Elements`).

Isso transforma o HTML em uma árvore.

Exemplo mental:

```text
ul
├── li ("hello")
└── li ("world")
```

Essa modelagem já melhora a clareza porque o sistema deixa de pensar apenas em texto linear e passa a pensar em hierarquia.

### 6.2 `ToStringImpl`

[⬆️ Voltar ao Sumário](#sumário)

O método `ToStringImpl` é o ponto onde a estrutura vira texto.

Ele:

- abre a tag atual;
- escreve o conteúdo textual, se houver;
- percorre os filhos;
- fecha a tag atual.

Ou seja: a renderização ficou encapsulada dentro do próprio produto.

Isso é muito melhor do que espalhar a lógica de renderização pelo cliente.

### 6.3 `HtmlBuilder`

[⬆️ Voltar ao Sumário](#sumário)

`HtmlBuilder` existe para representar o **processo de construção**.

Ele guarda:

- a raiz da estrutura (`root`);
- o nome da raiz (`rootName`);
- as operações que montam a árvore (`AddChild`, `Clear`).

O cliente não precisa saber como um filho é anexado internamente. Ele apenas pede:

- “adicione um item `li` com tal texto”.

Essa diferença parece pequena num exemplo de aula, mas em sistemas maiores isso reduz muito código duplicado e responsabilidade espalhada.

### 6.4 O fluxo real da construção

[⬆️ Voltar ao Sumário](#sumário)

O fluxo do exemplo é este:

1. O cliente cria `HtmlBuilder("ul")`.
2. O builder prepara um `HtmlElement` raiz com nome `ul`.
3. O cliente chama `AddChild("li", "hello")`.
4. O builder cria um novo `HtmlElement` filho e o adiciona à raiz.
5. O cliente repete os passos necessários.
6. No final, `ToString()` produz a representação textual completa.

Em formato de diagrama:

```text
Client
  -> HtmlBuilder
      -> cria/organiza HtmlElement(s)
          -> gera string final quando solicitado
```

### 6.5 Onde está a diferença real em relação ao arquivo `SemBuilder.cs`

[⬆️ Voltar ao Sumário](#sumário)

Em `SemBuilder.cs`:

- o cliente constrói diretamente a string.

Em `Builder.cs`:

- o cliente constrói uma estrutura de objetos;
- a string aparece apenas como etapa final de representação.

Essa é a comparação central da aula.

**Como interpretar o exemplo:** o padrão `Builder` não é sobre “usar método com nome bonito”. Ele é sobre tirar do cliente a obrigação de montar manualmente o objeto final.

---

## 7. Quando usar Builder

[⬆️ Voltar ao Sumário](#sumário)

Use `Builder` quando:

- o objeto final possui várias partes;
- a ordem ou o agrupamento dessas partes importa;
- há muitos elementos opcionais;
- o construtor normal começa a ficar ilegível;
- a montagem está se repetindo em vários lugares;
- você quer uma API de criação mais semântica do que uma sequência de `new` + `set`.

No espírito do GoF, o `Builder` brilha quando a construção é complexa o bastante para merecer uma abstração própria.

### 7.1 Sinais de que o Builder pode ajudar

[⬆️ Voltar ao Sumário](#sumário)

Alguns sinais clássicos:

- construtores com muitos parâmetros;
- blocos repetidos de configuração;
- código cliente cheio de detalhes de montagem;
- criação de estruturas hierárquicas;
- necessidade de montar o mesmo produto de formas diferentes.

---

## 8. Custos e trade-offs

[⬆️ Voltar ao Sumário](#sumário)

Como qualquer padrão, `Builder` não é gratuito.

Ele traz ganhos, mas também custos:

- mais classes e mais tipos no sistema;
- mais abstração para ler;
- possível exagero de design em cenários simples;
- necessidade de manter a API do builder coerente com a evolução do produto.

### 8.1 Quando ele é exagero

[⬆️ Voltar ao Sumário](#sumário)

Se o objeto:

- tem poucos campos;
- quase não varia;
- já nasce claro com construtor ou object initializer;
- não exige etapas de montagem;

então `Builder` pode ser overengineering.

**Regra prática:** se a criação não está causando dor real, talvez você ainda não precise de um builder.

---

## 9. Erros comuns e confusões frequentes

[⬆️ Voltar ao Sumário](#sumário)

### 9.1 Confundir `Builder Pattern` com `StringBuilder`

[⬆️ Voltar ao Sumário](#sumário)

Esse é o erro mais comum nesta aula.

- `StringBuilder` é uma classe concreta do .NET;
- `Builder Pattern` é uma decisão de design.

Eles podem aparecer juntos, como no seu exemplo, mas não são a mesma ideia.

### 9.2 Achar que Builder sempre precisa ser fluente

[⬆️ Voltar ao Sumário](#sumário)

Não precisa.

O builder da aula atual é **não fluente**:

```csharp
builder.AddChild("li", "hello");
builder.AddChild("li", "world");
```

No fluent builder, você normalmente encadeia chamadas:

```csharp
builder.AddChildFluent("li", "hello")
       .AddChildFluent("li", "world");
```

Mas fluência é um estilo de API, não a essência do padrão.

### 9.3 Achar que o Builder serve para qualquer objeto

[⬆️ Voltar ao Sumário](#sumário)

Não serve.

Se a construção não é complexa, o padrão pode só adicionar ruído.

### 9.4 Esquecer que o objetivo é reduzir responsabilidade do cliente

[⬆️ Voltar ao Sumário](#sumário)

Se você cria um builder, mas o cliente continua precisando saber todos os detalhes internos da montagem, então o padrão foi aplicado de forma fraca.

O builder bom reduz atrito mental para quem usa a API.

---

## 10. Comparações importantes

[⬆️ Voltar ao Sumário](#sumário)

### 10.1 Builder vs Factory

[⬆️ Voltar ao Sumário](#sumário)

- **Factory** enfatiza decisão de qual objeto criar.
- **Builder** enfatiza processo de como montar o objeto.

Factory responde mais a “qual produto?”.
Builder responde mais a “como montar este produto?”.

### 10.2 Builder vs construtor tradicional

[⬆️ Voltar ao Sumário](#sumário)

Construtor tradicional funciona muito bem quando:

- há poucos parâmetros;
- a intenção continua clara;
- a criação é direta.

Builder entra melhor quando:

- há várias partes;
- há combinações opcionais;
- a leitura do construtor ficou ruim;
- a montagem ficou procedural demais no cliente.

### 10.3 Builder vs object initializer

[⬆️ Voltar ao Sumário](#sumário)

Em C# moderno, object initializers resolvem muitos cenários simples:

```csharp
var pessoa = new Pessoa
{
    Nome = "Ana",
    Idade = 28
};
```

Mas isso ainda é diferente de um builder completo. Object initializer expõe estado. Builder encapsula processo.

### 10.4 Builder vs Composite no exemplo HTML

[⬆️ Voltar ao Sumário](#sumário)

Esse ponto é sutil e muito interessante.

No exemplo HTML:

- `HtmlElement` lembra um **Composite**, porque modela árvore pai-filho;
- `HtmlBuilder` representa o **Builder**, porque cuida da construção dessa árvore.

Ou seja: padrões diferentes podem coexistir no mesmo exemplo.

### 10.5 Builder hoje, Fluent Builder depois

[⬆️ Voltar ao Sumário](#sumário)

O que você está vendo agora é a forma conceitualmente mais simples:

- o cliente chama métodos de montagem;
- o builder organiza o produto;
- no final a estrutura é emitida.

As aulas seguintes expandem isso para:

- **Fluent Builder**: foco em API encadeável;
- **Recursive Generics**: foco em herança com fluência preservada;
- **Stepwise Builder**: foco em restringir ordem e obrigatoriedade de passos;
- **Functional Builder**: foco em composição funcional de mutações;
- **Faceted Builder**: foco em construção por aspectos diferentes do mesmo objeto.

Essa progressão faz sentido porque a ideia central permanece a mesma: **tirar complexidade de construção do cliente e colocá-la numa abstração própria**.

---

## 11. Conclusão

[⬆️ Voltar ao Sumário](#sumário)

Dentro da **Gamma Categorization**, o `Builder` pertence aos **Creational Patterns** porque seu problema central é a criação de objetos. Mas sua especialidade não é decidir “qual objeto concreto nasce”; sua especialidade é organizar **como um objeto complexo é montado**.

Na sua aula atual, isso aparece de forma muito clara:

- em `SemBuilder.cs`, o cliente monta HTML diretamente como texto;
- em `Builder.cs`, o cliente passa a montar uma estrutura de objetos com ajuda de um builder.

Essa é a transformação didática principal:

- sair de uma criação procedural e espalhada;
- ir para uma criação encapsulada e semântica.

Se você fixar bem este ponto, o restante das variantes do capítulo fica muito mais natural.

**Resumo em uma frase:** o `Builder` do GoF existe para que o cliente descreva o que quer construir sem carregar sozinho o peso de como aquilo deve ser montado.

---

## 12. Referências bibliográficas

[⬆️ Voltar ao Sumário](#sumário)

Referência principal utilizada neste capítulo:

- **Gamma, Erich; Helm, Richard; Johnson, Ralph; Vlissides, John.** *Design Patterns: Elements of Reusable Object-Oriented Software*. Addison-Wesley, 1994.

Referências complementares de contexto já usadas no projeto:

- `1.SOLID/SOLID.md`
- `C#_Fundamentals.md`
- `2.Builder/Aula01_builder/SemBuilder.cs`
- `2.Builder/Aula01_builder/Builder.cs`

Observações importantes:

- o termo **Gang of Four (GoF)** se refere aos quatro autores acima;
- a expressão **Gamma Categorization** faz referência a **Erich Gamma**;
- o `Builder` desta aula é o **padrão de projeto GoF**, não a classe `System.Text.StringBuilder` do .NET.
