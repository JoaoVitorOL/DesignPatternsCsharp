# Capítulo 2 - Builder

**por Erich Gamma, Richard Helm, Ralph Johnson e John Vlissides (Gang of Four - GoF)**

> **Livro de referência principal:** *Design Patterns: Elements of Reusable Object-Oriented Software*  
> **Objetivo deste capítulo:** organizar a trilha de `Builder` na mesma sequência do curso, conectando cada aula aos arquivos reais desta pasta

---

## Prefácio

[⬆️ Voltar ao Sumário](#sumário)

Este material foi reorganizado para seguir a mesma ordem da trilha mostrada nas aulas.

Em vez de estudar `Builder` como um bloco único e solto, a ideia aqui é acompanhar o caminho natural do capítulo:

- primeiro entender a **Gamma Categorization**;
- depois enxergar o **Overview** do padrão;
- em seguida comparar **Life Without Builder** com **Builder**;
- depois avançar para as variações, como **Fluent Builder** e **Recursive Generics**;
- por fim, deixar mapeadas as próximas aulas da trilha: `Stepwise`, `Functional`, `Faceted`, exercício e resumo.

O objetivo é que o arquivo funcione como um guia de leitura do capítulo inteiro, e não apenas como uma explicação isolada de um único arquivo.

---

## Como ler este capítulo

[⬆️ Voltar ao Sumário](#sumário)

Use este documento em duas camadas:

1. **Camada conceitual:** para entender o papel do padrão `Builder` dentro do mapa GoF.
2. **Camada prática:** para relacionar cada aula aos arquivos desta pasta.

Arquivos já presentes no repositório:

- `2.Builder/Aula01_builder/SemBuilder.cs`
- `2.Builder/Aula01_builder/Builder.cs`
- `2.Builder/Aula02_FluentBuilder/fluentBuilder.cs`
- `2.Builder/Aula02_FluentBuilder/ErradoFluentBuilderWithRecursiveGenerics.cs`
- `2.Builder/Aula02_FluentBuilder/CertoFluentBuilderWithRecursiveGenerics.cs`
- `2.Builder/Aula03_StepWiseBuilder/StepWiseBuilder.cs`
- `2.Builder/Aula04_FunctionalBuilder/FunctionalBuilder.cs`

As seções de `Faceted Builder` e `Coding Exercise` foram mantidas porque fazem parte da trilha da imagem, mesmo que ainda não exista um arquivo local para cada uma delas.

---

## Sumário

- [9. Gamma Categorization](#9-gamma-categorization)
- [10. Overview](#10-overview)
- [11. Life Without Builder](#11-life-without-builder)
- [12. Builder](#12-builder)
- [13. Fluent Builder](#13-fluent-builder)
- [14. Fluent Builder Inheritance with Recursive Generics](#14-fluent-builder-inheritance-with-recursive-generics)
- [15. Stepwise Builder](#15-stepwise-builder)
- [16. Functional Builder](#16-functional-builder)
- [17. Faceted Builder](#17-faceted-builder)
- [Coding Exercise 1: Builder Coding Exercise](#coding-exercise-1-builder-coding-exercise)
- [18. Summary](#18-summary)
- [Referências bibliográficas](#referências-bibliográficas)

---

## 9. Gamma Categorization

[⬆️ Voltar ao Sumário](#sumário)

### 9.1 O que é

Gamma Categorization é a forma clássica de organizar os Design Patterns do GoF em famílias de problema. A ideia não é só arrumar um índice; a ideia é responder:

**que tipo de problema de design estou tentando resolver?**

Os padrões clássicos são agrupados em três categorias:

- **Creational Patterns:** lidam com criação de objetos.
- **Structural Patterns:** lidam com composição de classes e objetos.
- **Behavioral Patterns:** lidam com colaboração, fluxo e distribuição de responsabilidades.

### 9.2 Por que isso importa

Sem esse mapa, é comum estudar padrões como receitas soltas. Com esse mapa, a análise melhora:

- em vez de decorar nomes, você entende famílias de problema;
- em vez de perguntar "qual padrão eu encaixo aqui?", você começa perguntando "meu problema é de criação, estrutura ou comportamento?".

### 9.3 Onde o Builder entra

`Builder` pertence à família **Creational** porque o problema central dele é de criação. Mas não é apenas "criar com new". O foco está em:

- organizar a montagem;
- separar processo de construção e representação final;
- reduzir a carga do cliente quando o objeto fica complexo demais para ser montado manualmente.

Em outras palavras:

- `Factory` tende a responder melhor a **qual objeto deve nascer**;
- `Builder` tende a responder melhor a **como esse objeto deve ser montado**.

---

## 10. Overview

[⬆️ Voltar ao Sumário](#sumário)

### 10.1 O que o Builder representa

No sentido clássico do GoF, o `Builder` existe para **separar a construção de um objeto complexo de sua representação**.

Isso ajuda quando:

- o objeto tem várias partes;
- a construção em etapas melhora a leitura;
- o cliente começou a carregar detalhes demais da montagem.

### 10.2 O problema que ele resolve

O problema típico do `Builder` não é "como concatenar texto" e nem apenas "como instanciar". O problema é:

**como montar algo mais elaborado sem obrigar o código cliente a executar manualmente cada detalhe da construção?**

Sinais de que esse problema apareceu:

- muitas etapas de montagem;
- ordem relevante;
- partes opcionais;
- repetição de construção em vários lugares;
- cliente com cara de código procedural.

### 10.3 Participantes clássicos do padrão

Na formulação clássica, costumam aparecer estes papéis:

- **Product:** o objeto final.
- **Builder:** a abstração dos passos de construção.
- **ConcreteBuilder:** a implementação concreta desses passos.
- **Director:** o coordenador da receita.
- **Client:** quem dispara o processo e consome o resultado.

Nos exemplos desta pasta, o `Director` não aparece como classe separada. O próprio cliente conversa com o builder diretamente.

---

## 11. Life Without Builder

[⬆️ Voltar ao Sumário](#sumário)

Esta parte corresponde ao arquivo `2.Builder/Aula01_builder/SemBuilder.cs`.

Sem builder, o cliente monta o HTML manualmente, conhecendo:

- a tag de abertura;
- a tag de fechamento;
- o formato interno dos itens;
- a ordem em que tudo precisa ser concatenado.

Exemplo de estilo:

```csharp
sb.Append("<ul>");
foreach (var word in words)
{
    sb.AppendFormat("<li>{0}</li>", word);
}
sb.Append("</ul>");
```

Nesse cenário, o cliente não está apenas pedindo uma lista HTML. Ele está executando a montagem inteira.

### 11.1 O problema pedagógico desse estilo

Quando o cliente monta tudo sozinho:

- ele conhece detalhes demais da representação final;
- a lógica de construção fica espalhada;
- a leitura perde intenção de domínio;
- qualquer variação de construção tende a ser repetida.

Essa é a dor que prepara o terreno para a aula seguinte.

---

## 12. Builder

[⬆️ Voltar ao Sumário](#sumário)

Esta parte corresponde ao arquivo `2.Builder/Aula01_builder/Builder.cs`.

Aqui o cliente deixa de montar o HTML manualmente e passa a conversar com uma API de construção.

```csharp
var builder = new HtmlBuilder("ul");
builder.AddChild("li", "hello");
builder.AddChild("li", "world");
WriteLine(builder.ToString());
```

### 12.1 Como isso aparece no exemplo da aula

Mapeando os papéis do padrão:

- `HtmlElement` funciona como o **Product**;
- `HtmlBuilder` funciona como o **ConcreteBuilder**;
- `Main` funciona como o **Client**.

O ganho principal aqui não é performance. O ganho é estrutural e cognitivo:

- o cliente descreve o que quer construir;
- o builder centraliza como aquilo será montado.

### 12.2 Anatomia das variáveis do HtmlElement

`HtmlElement` representa um nó da árvore HTML.

- `Name`: nome da tag atual, como `ul`, `li` ou `p`.
- `Text`: texto interno do nó, quando existir.
- `Elements`: lista de filhos do nó atual.
- `identSize`: número de espaços usados na indentação visual da saída.

Dentro de `ToStringImpl()` aparecem duas variáveis locais importantes:

- `sb`: um `StringBuilder` usado como detalhe interno de renderização.
- `i`: a indentação correspondente ao nível atual da árvore.

**Leitura correta:** `HtmlElement` é o produto final em construção, não o builder.

### 12.3 Anatomia das variáveis do HtmlBuilder

No builder tradicional, o estado da construção fica concentrado em poucas variáveis:

- `rootName`: guarda o nome da raiz, como `ul`.
- `root`: guarda o `HtmlElement` raiz que está sendo montado aos poucos.

No método `AddChild()`:

- `childName`: nome da tag filha a ser criada.
- `childText`: conteúdo textual dessa tag.
- `e`: novo `HtmlElement` filho antes de ser adicionado a `root.Elements`.

No `Main`, a variável `builder` representa a API usada pelo cliente para descrever a construção.

### 12.4 O que define o builder tradicional

No builder tradicional, a construção acontece em passos separados.

O ponto decisivo da API é este:

- o método `AddChild()` constrói;
- mas retorna `void`;
- então a chamada termina ali.

É por isso que o uso fica assim:

```csharp
builder.AddChild("li", "hello");
builder.AddChild("li", "world");
builder.AddChild("p", "Ser ou nao ser, eis a questao");
```

### 12.5 Builder Pattern não é StringBuilder

Esse é um ponto importante para não confundir conceitos:

- `System.Text.StringBuilder` é uma classe do .NET para montar texto com eficiência.
- `Builder Pattern` é um padrão de projeto para organizar construção de objetos complexos.

No capítulo:

- em `SemBuilder.cs`, o `StringBuilder` é a ferramenta principal da solução;
- em `Builder.cs`, o foco passa para a abstração de construção.

### 12.6 Quando usar e quando não usar

Use `Builder` quando:

- o produto tiver várias partes;
- a criação em etapas melhorar a leitura;
- um construtor simples ficar ambíguo ou grande demais;
- a montagem estiver se repetindo no cliente.

Pode ser exagero quando:

- o objeto é simples;
- um construtor já comunica bem a intenção;
- não há etapas nem combinações relevantes.

### 12.7 Erros comuns

Erros frequentes ao estudar essa aula:

- achar que qualquer classe com "builder" no nome já implementa o padrão;
- confundir `Builder Pattern` com `StringBuilder`;
- criar um builder, mas continuar deixando o cliente responsável por detalhes demais;
- usar builder quando um construtor simples bastaria.

---

## 13. Fluent Builder

[⬆️ Voltar ao Sumário](#sumário)

Esta parte corresponde ao arquivo `2.Builder/Aula02_FluentBuilder/fluentBuilder.cs`.

O `Fluent Builder` continua sendo `Builder`. O que muda é a forma da API.

### 13.1 A diferença central para o builder tradicional

No builder tradicional:

- o método constrói;
- mas retorna `void`.

No fluent builder:

- o método constrói;
- e devolve o próprio builder.

Exemplo:

```csharp
builder
    .AddChildFluent("li", "hello")
    .AddChildFluent("li", "world")
    .AddChildFluent("li", "XD");
```

### 13.2 O ponto de virada da API

O fluent builder nasce quando a assinatura muda:

- de `void AddChild(...)`
- para `HtmlBuilder AddChildFluent(...)`

e o método termina com:

```csharp
return this;
```

Esse `return this` devolve a instância atual do builder. Com isso, a próxima chamada pode continuar na mesma expressão.

### 13.3 O que muda e o que não muda

O que muda:

- legibilidade da montagem;
- encadeamento;
- ergonomia da API.

O que não muda:

- o fato de continuar sendo um builder;
- a ideia de montar o mesmo produto final;
- o papel do cliente em descrever o que quer construir.

---

## 14. Fluent Builder Inheritance with Recursive Generics

[⬆️ Voltar ao Sumário](#sumário)

Esta parte corresponde aos arquivos:

- `2.Builder/Aula02_FluentBuilder/ErradoFluentBuilderWithRecursiveGenerics.cs`
- `2.Builder/Aula02_FluentBuilder/CertoFluentBuilderWithRecursiveGenerics.cs`

Aqui o problema deixa de ser apenas "retornar `this`". A questão passa a ser:

**como manter a fluência quando um builder herda de outro builder?**

### 14.1 O que dá errado na herança simples

No arquivo `ErradoFluentBuilderWithRecursiveGenerics.cs`, o problema nasce aqui:

```csharp
public PersonInfoBuilder Called(string name)
```

Depois de `Called()`, a cadeia vira `PersonInfoBuilder`. Então o método específico da classe derivada deixa de estar visível:

```csharp
builder.Called("Dmitri")
    .workAsA("cafetao");
```

O erro conceitual é este:

- a cadeia começou em `PersonJobBuilder`;
- mas o retorno rebaixou a expressão para `PersonInfoBuilder`;
- `PersonInfoBuilder` não conhece `workAsA()`.

### 14.2 Como recursive generics corrige isso

No arquivo `CertoFluentBuilderWithRecursiveGenerics.cs`, a solução usa um tipo genérico autorreferente:

```csharp
public class PersonInfoBuilder<SELF> : PersonBuilder
    where SELF : PersonInfoBuilder<SELF>
```

Agora `Called()` não devolve mais uma base fixa. Ele devolve `SELF`:

```csharp
public SELF Called(string name)
{
    person.Name = name;
    return (SELF)this;
}
```

### 14.3 Explicando o trecho mais importante

O trecho central é este:

```csharp
public class PersonJobBuilder<SELF>
    : PersonInfoBuilder<PersonJobBuilder<SELF>>
    where SELF : PersonJobBuilder<SELF>
```

Leitura em partes:

- `PersonJobBuilder<SELF>` ainda recebe o tipo concreto final da cadeia.
- `: PersonInfoBuilder<PersonJobBuilder<SELF>>` diz para a camada de `PersonInfoBuilder` que, naquela cadeia, o tipo de retorno relevante passa a ser `PersonJobBuilder<SELF>`.
- `where SELF : PersonJobBuilder<SELF>` restringe `SELF` para que ele realmente pertença a essa família de builders.

Tradução prática:

- quando `Called()` for executado, a cadeia não será degradada para uma classe base genérica demais;
- ela continuará em um tipo que ainda conhece `workAsA()`;
- por isso a fluência sobrevive mesmo com herança.

### 14.4 Como o ciclo fecha no exemplo

Dentro de `Person`, aparece:

```csharp
public class Builder : PersonJobBuilder<Builder>
{
}
```

Esse é o fechamento da cadeia. O tipo concreto final passa a ser `Builder`.

Por isso o uso funciona:

```csharp
var me = Person.New
    .Called("Dmitri")
    .workAsA("Professor")
    .Build();
```

---

## 15. Stepwise Builder

[⬆️ Voltar ao Sumário](#sumário)

Esta parte corresponde ao arquivo `2.Builder/Aula03_StepWiseBuilder/StepWiseBuilder.cs`.

O **Stepwise Builder** tenta resolver um problema diferente do fluent builder comum:

- o fluent builder melhora a leitura da API;
- o stepwise builder melhora a **seguranca da ordem** da construcao.

Em vez de apenas devolver `this`, ele devolve **interfaces diferentes para cada etapa**.

### 15.1 Qual problema ele resolve

No builder tradicional ou no fluent builder comum, o cliente pode ter liberdade demais. Dependendo da API, ele pode:

- tentar pular um passo obrigatorio;
- chamar metodos em ordem ruim;
- chegar em `Build()` cedo demais.

O stepwise builder combate isso fazendo a pergunta:

**"qual e o proximo passo valido que o cliente pode executar agora?"**

### 15.2 Como isso aparece no exemplo do carro

No exemplo atual, a ordem desejada e esta:

1. escolher o tipo do carro;
2. escolher o tamanho da roda;
3. construir o carro.

Essa ordem aparece nas interfaces:

```csharp
public interface ISpecifyCarType
{
    ISpecifyWheelSize OfType(CarType type);
}

public interface ISpecifyWheelSize
{
    IBuildCar WithWheels(int size);
}

public interface IBuildCar
{
    Car Build();
}
```

Cada interface representa um **estagio da conversa** com o builder.

- `ISpecifyCarType`: so permite escolher o tipo.
- `ISpecifyWheelSize`: so permite escolher as rodas.
- `IBuildCar`: so permite finalizar com `Build()`.

### 15.3 O ponto central da tecnica

O truque principal nao esta no objeto concreto interno. O truque esta no **tipo devolvido por cada metodo**.

Leia a cadeia assim:

- `Create()` devolve `ISpecifyCarType`;
- `OfType()` devolve `ISpecifyWheelSize`;
- `WithWheels()` devolve `IBuildCar`;
- `Build()` devolve `Car`.

Ou seja, o compilador passa a guiar a ordem:

```csharp
var car = CarBuilder.Create()
    .OfType(CarType.Crossover)
    .WithWheels(18)
    .Build();
```

Se o cliente acabou de chamar `Create()`, ele ainda nao enxerga `WithWheels()` nem `Build()`, porque esses metodos nao fazem parte da interface atual.

### 15.4 Objeto real vs tipo da referencia

Essa e a parte que mais costuma confundir no comeco.

No metodo:

```csharp
public static ISpecifyCarType Create()
{
    return new Impl();
}
```

parece estranho ler:

- a assinatura promete `ISpecifyCarType`;
- mas o `return` devolve `new Impl()`.

Isso funciona porque `Impl` implementa `ISpecifyCarType`.

Em C#, isso e normal: um objeto concreto pode ser tratado como uma interface que ele implementa.

Leitura mental correta:

- **objeto real criado na memoria:** `Impl`
- **tipo da referencia entregue ao cliente:** `ISpecifyCarType`

Essas duas coisas nao sao iguais.

No Stepwise Builder, a ordem nao e controlada por "qual objeto foi criado por baixo". A ordem e controlada por **qual interface foi entregue ao cliente nesta etapa**.

Como `ISpecifyCarType` so declara:

```csharp
ISpecifyWheelSize OfType(CarType type);
```

entao, naquele momento, o cliente so consegue chamar `OfType()`.

Ele nao consegue chamar `WithWheels()` nem `Build()` porque esses metodos nao fazem parte do tipo da referencia atual, mesmo que o objeto concreto por baixo seja um `Impl` que saiba fazer tudo.

Uma forma de abrir a cadeia e enxergar melhor isso e esta:

```csharp
ISpecifyCarType step1 = CarBuilder.Create();
ISpecifyWheelSize step2 = step1.OfType(CarType.Crossover);
IBuildCar step3 = step2.WithWheels(18);
Car car = step3.Build();
```

Agora a ideia fica mais visivel:

- `step1` so conhece `OfType()`;
- `step2` so conhece `WithWheels()`;
- `step3` so conhece `Build()`.

Entao a pergunta certa nao e:

**"qual objeto foi criado?"**

A pergunta certa e:

**"qual interface foi devolvida para o cliente agora?"**

### 15.5 Por que existe uma classe `Impl`

Dentro do `CarBuilder`, existe uma classe privada `Impl` que implementa todas as interfaces:

```csharp
private class Impl : ISpecifyCarType, ISpecifyWheelSize, IBuildCar
```

Ela sabe executar todos os passos internamente, mas o cliente nao recebe a classe concreta diretamente. O cliente recebe apenas a interface do estagio atual.

Essa e a ideia chave:

- por dentro, um unico objeto sabe tudo;
- por fora, a API so expõe o proximo passo valido.

Mesmo que o objeto real seja um `Impl`, o cliente nao consegue pedir essa classe concreta diretamente, porque ela e `private`. Isso ajuda a blindar o caminho das interfaces publicas.

### 15.6 Onde a validacao entra

No exemplo, a escolha das rodas depende do tipo do carro:

- `Crossover` aceita rodas entre `17` e `20`;
- `Sedan` aceita rodas entre `15` e `17`.

Isso aparece em `WithWheels(int size)`.

O stepwise builder nao substitui a validacao de regra de negocio. Ele trabalha junto com ela:

- as interfaces restringem a **ordem**;
- o metodo concreto ainda valida o **conteudo** do passo.

### 15.7 O que ele enfatiza

- ordem correta dos passos;
- seguranca de construcao em tempo de compilacao;
- API que representa melhor o fluxo obrigatorio do dominio.

### 15.8 Quando ele faz sentido

Ele e util quando:

- a ordem faz parte do contrato do dominio;
- existem passos obrigatorios;
- um objeto incompleto seria invalido demais para ser liberado;
- voce quer que o compilador ajude a bloquear usos errados da API.

### 15.9 Custo dessa abordagem

O ganho de seguranca costuma vir com mais tipos, mais interfaces e mais burocracia de modelagem.

Em troca, a leitura da API passa a ensinar a ordem correta de construcao.

---

## 16. Functional Builder

[⬆️ Voltar ao Sumário](#sumário)

Esta parte corresponde ao arquivo `2.Builder/Aula04_FunctionalBuilder/FunctionalBuilder.cs`.

O **Functional Builder** continua sendo uma variação de `Builder`, mas muda o modelo mental da construção por dentro.

Em vez de pensar apenas:

- "tenho um objeto e vou preenchendo ele agora"

o exemplo passa a pensar:

- "vou acumulando operações de construção e só no final vou materializar o objeto"

No arquivo atual, isso aparece nesta leitura:

```csharp
var person = new PersonBuilder()
    .Called("Sara")
    .WorksAsA("Professor")
    .Build();
```

Por fora, a cadeia lembra um fluent builder comum. A diferença importante está por dentro:

- `Called()` registra uma operação;
- `WorksAsA()` registra outra operação;
- `Build()` cria o objeto base e aplica todas as operações acumuladas.

### 16.1 O que muda em relação ao Stepwise Builder

Esta é a comparação mais importante com a aula anterior.

No **Stepwise Builder**, a pergunta central era:

**"qual é o próximo passo válido?"**

Por isso o exemplo do carro usava interfaces diferentes para cada etapa:

- `ISpecifyCarType`
- `ISpecifyWheelSize`
- `IBuildCar`

Cada retorno restringia o que o cliente podia chamar em seguida.

No **Functional Builder**, a pergunta central muda para:

**"quais operações de construção eu quero compor?"**

Aqui o objetivo principal já não é travar a ordem pelo tipo devolvido. O objetivo passa a ser acumular comportamento de forma modular.

Resumo da virada:

- `Stepwise Builder`: enfatiza ordem obrigatória.
- `Functional Builder`: enfatiza composição de passos.

### 16.2 Como isso aparece na anatomia do código

No arquivo da aula, a base genérica é esta:

```csharp
public abstract class FunctionalBuilder<TSubject, TSelf>
```

Esses dois parâmetros significam:

- `TSubject`: qual é o objeto final sendo construído.
- `TSelf`: qual é o tipo concreto do builder que está encadeando.

Isso permite reaproveitar a mecânica do builder funcional sem perder a API fluent correta no tipo concreto.

Dentro dessa classe, a variável central é:

```csharp
private readonly List<Func<TSubject, TSubject>> actions
```

Essa lista guarda a receita da construção.

Cada `Func<TSubject, TSubject>` representa uma operação que:

1. recebe o objeto em construção;
2. aplica alguma mudança;
3. devolve o mesmo objeto para a próxima operação.

### 16.3 O papel de `Do()`, `AddAction()` e `Build()`

O método:

```csharp
public TSelf Do(Action<TSubject> action)
```

recebe uma `Action<TSubject>`, isto é, uma operação que mexe no objeto mas não devolve valor.

Já a lista interna guarda `Func<TSubject, TSubject>`, porque o pipeline precisa devolver o objeto para a etapa seguinte.

Então `AddAction()` faz a ponte entre essas duas ideias:

- recebe uma `Action<TSubject>`;
- empacota essa action dentro de uma função;
- essa função muta o objeto e depois devolve esse mesmo objeto.

Depois, `Build()` aplica a receita acumulada:

```csharp
return actions.Aggregate(new TSubject(), (subject, action) => action(subject));
```

Leitura passo a passo:

1. `new TSubject()` cria uma instância vazia.
2. `Aggregate()` percorre a lista de operações.
3. cada operação recebe o mesmo objeto e devolve esse objeto já alterado.
4. o resultado da última operação vira o produto construído.

Em outras palavras: no functional builder, o objeto final só nasce "de verdade" no `Build()`.

### 16.4 Onde a extensibilidade aparece

No exemplo atual, `Called(string name)` fica dentro de `PersonBuilder`, mas `WorksAsA(string position)` aparece em `PersonBuilderExtensions`.

Esse detalhe mostra um ganho comum desse estilo:

- novos passos podem ser plugados por fora;
- a classe principal do builder não precisa concentrar tudo;
- a construção fica mais modular.

Isso conversa diretamente com a ideia de compor pequenas operações independentes.

### 16.5 Diferença direta para a aula anterior

Uma tabela mental simples ajuda:

| Aula | Pergunta principal | Técnica central | Ganho principal |
| --- | --- | --- | --- |
| `Stepwise Builder` | "qual é o próximo passo válido?" | interfaces diferentes por etapa | segurança de ordem em tempo de compilação |
| `Functional Builder` | "quais operações quero acumular?" | lista de funções aplicadas no `Build()` | composição flexível de comportamento |

Outra forma curta de guardar:

- no stepwise, o tipo devolvido controla o fluxo;
- no functional, a lista de operações controla a construção.

### 16.6 Quando ele ajuda

Ele pode ser interessante quando:

- você quer montar objetos com passos bem modulares;
- faz sentido acumular ações antes de materializar o resultado final;
- você quer adicionar novos passos sem inchar tanto a classe principal;
- a equipe está confortável com delegates, lambdas e uma leitura mais abstrata.

### 16.7 Cuidado didático

Para iniciantes, este estilo costuma parecer mais abstrato do que o builder tradicional e do que o stepwise builder.

Isso acontece porque parte da construção fica indireta:

- o objeto não está sendo mostrado como mutado o tempo todo;
- o código está guardando operações para depois;
- `Action`, `Func`, extension methods e `Aggregate()` exigem mais maturidade de leitura.

Por isso, este modelo costuma fazer mais sentido depois que a base de `Builder`, `Fluent Builder` e `Stepwise Builder` já ficou firme.

---

## 17. Faceted Builder

[⬆️ Voltar ao Sumário](#sumário)

O **Faceted Builder** aparece quando um único builder começa a ficar grande demais.

Nesse caso, a experiência de construção do mesmo produto é dividida por aspectos ou facetas.

### 17.1 O que ele enfatiza

- separação por responsabilidade dentro da construção;
- organização de APIs grandes;
- divisão da montagem do mesmo produto em áreas mais claras.

### 17.2 Exemplo mental

Um objeto grande pode ter facetas como:

- endereço;
- trabalho;
- finanças;
- configurações técnicas.

### 17.3 Leitura correta

O `Faceted Builder` não divide o produto em produtos diferentes. Ele divide a **interface de construção** do mesmo produto em partes mais administráveis.

---

## Coding Exercise 1: Builder Coding Exercise

[⬆️ Voltar ao Sumário](#sumário)

Esta seção foi criada para espelhar a trilha da imagem.

No estado atual do repositório, ainda não há um arquivo específico do exercício prático dentro de `2.Builder/`. Mesmo assim, vale registrar o objetivo pedagógico desse ponto da trilha:

- sair da leitura conceitual;
- implementar uma solução usando o padrão;
- testar se a diferença entre construção manual, builder tradicional e fluent builder realmente ficou clara.

Quando o exercício for adicionado ao repositório, esta seção pode ser expandida com:

- intenção do problema;
- participantes do padrão no exercício;
- decisões de modelagem;
- riscos e trade-offs da solução.

---

## 18. Summary

[⬆️ Voltar ao Sumário](#sumário)

### 18.1 Resumo da trilha

O mapa deste capítulo fica assim:

| Aula | Pergunta principal |
| --- | --- |
| 9. Gamma Categorization | Em que família de problema o Builder se encaixa? |
| 10. Overview | O que o Builder representa e que problema resolve? |
| 11. Life Without Builder | Como o cliente sofre quando monta tudo manualmente? |
| 12. Builder | Como separar a construção da representação final? |
| 13. Fluent Builder | Como tornar a API encadeável com `return this`? |
| 14. Fluent Builder Inheritance with Recursive Generics | Como manter a fluência mesmo com herança? |
| 15. Stepwise Builder | Como forçar ordem e passos obrigatórios? |
| 16. Functional Builder | Como pensar a construção como composição de operações? |
| 17. Faceted Builder | Como dividir um builder grande por facetas? |
| Coding Exercise 1 | Como aplicar o padrão em um problema prático? |
| 18. Summary | Como consolidar o mapa mental do capítulo? |

### 18.2 Ideia central que atravessa todas as variações

Todas as variações continuam orbitando a mesma ideia do GoF:

**tirar da mão do cliente o peso de montar sozinho um objeto complexo.**

O que muda de uma aula para outra é:

- a forma da API;
- o nível de segurança na construção;
- o uso de herança, generics ou composição;
- o custo de abstração para obter essas garantias.

### 18.3 Regra mental final

Uma forma simples de guardar o capítulo é esta:

- **Builder básico:** organiza a montagem.
- **Fluent Builder:** organiza a montagem com encadeamento.
- **Recursive Generics Builder:** preserva a fluência na herança.
- **Stepwise Builder:** restringe a ordem da construção.
- **Functional Builder:** compõe a construção como operações.
- **Faceted Builder:** divide a construção por aspectos.

---

## Referências bibliográficas

[⬆️ Voltar ao Sumário](#sumário)

Referência principal:

- **Gamma, Erich; Helm, Richard; Johnson, Ralph; Vlissides, John.** *Design Patterns: Elements of Reusable Object-Oriented Software*. Addison-Wesley, 1994.

Arquivos desta pasta usados como base prática:

- `2.Builder/Aula01_builder/SemBuilder.cs`
- `2.Builder/Aula01_builder/Builder.cs`
- `2.Builder/Aula02_FluentBuilder/fluentBuilder.cs`
- `2.Builder/Aula02_FluentBuilder/ErradoFluentBuilderWithRecursiveGenerics.cs`
- `2.Builder/Aula02_FluentBuilder/CertoFluentBuilderWithRecursiveGenerics.cs`
- `2.Builder/Aula03_StepWiseBuilder/StepWiseBuilder.cs`
- `2.Builder/Aula04_FunctionalBuilder/FunctionalBuilder.cs`

Observação importante:

- `Builder` aqui é o padrão de projeto GoF;
- `StringBuilder` é uma classe do .NET;
- os dois nomes se parecem, mas o papel conceitual é diferente.
