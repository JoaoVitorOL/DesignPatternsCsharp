# Capítulo 2 - Builder

**por Erich Gamma, Richard Helm, Ralph Johnson e John Vlissides (Gang of Four - GoF)**

> **Livro de referência principal:** *Design Patterns: Elements of Reusable Object-Oriented Software*
> **Autores da obra-base:** Erich Gamma, Richard Helm, Ralph Johnson e John Vlissides
> **Objetivo deste capítulo:** introduzir a categorização clássica dos Design Patterns e posicionar o padrão `Builder` dentro desse mapa conceitual, aprofundando depois o próprio `Builder` com base nos exemplos da aula

---

## Prefácio

[⬆️ Voltar ao Sumário](#sumário)

Quando alguém começa a estudar Design Patterns, é comum sentir que está diante de uma coleção de nomes soltos. `Singleton`, `Factory Method`, `Adapter`, `Observer`, `Strategy`, `Builder` e tantos outros aparecem em livros, cursos e entrevistas, mas sem um mapa mental claro tudo isso pode parecer apenas uma lista para decorar.

É exatamente por isso que a chamada **Gamma Categorization** é tão importante.

Os padrões clássicos do livro do GoF não foram apresentados como um conjunto aleatório. Eles foram organizados em **três grandes categorias**, cada uma destacando um tipo diferente de problema de design:

- **Creational Patterns**, voltados à criação de objetos;
- **Structural Patterns**, voltados à composição de classes e objetos;
- **Behavioral Patterns**, voltados à distribuição de responsabilidades e ao fluxo de comunicação.

Essa divisão não existe só para organizar capítulos. Ela existe para responder uma pergunta muito prática:

**“Que tipo de problema de design estou tentando resolver?”**

Se o problema está na forma como objetos nascem, o raciocínio tende para padrões criacionais. Se está na forma como eles se conectam, o foco tende a ser estrutural. Se está na forma como colaboram, decidem e trocam mensagens, o problema costuma ser comportamental.

Este capítulo começa por esse panorama porque o padrão `Builder` só fica realmente claro quando o leitor entende o lugar que ele ocupa no ecossistema dos padrões. Depois desse mapa, o texto entra numa seção específica sobre o próprio `Builder`, para conectar a teoria ao exemplo atual da aula.

---

## Como ler este capítulo

[⬆️ Voltar ao Sumário](#sumário)

Este material foi escrito com duas intenções ao mesmo tempo:

1. **Dar visão de mapa:** mostrar como os padrões GoF foram agrupados.
2. **Preparar o terreno para o Builder:** explicar por que ele pertence aos padrões criacionais e qual tipo de problema ele resolve.

Se você está começando, leia em ordem. Se já conhece alguns padrões, use este texto como uma introdução conceitual para alinhar terminologia e contexto antes de mergulhar em implementações.

Ao longo do texto, pense sempre nestas quatro perguntas:

- **O que isso representa?** Conceito ou contrato.
- **Quando eu usaria isso?** Cenário prático.
- **O que isso custa?** Complexidade, abstração, acoplamento ou manutenção.
- **Qual erro é comum aqui?** Armadilha conceitual ou de implementação.

Ao longo do texto, quando aparecerem expressões como **GoF**, **Gamma Categorization** ou **padrões clássicos**, a referência principal é sempre o livro:

- **Gamma, Erich; Helm, Richard; Johnson, Ralph; Vlissides, John.** *Design Patterns: Elements of Reusable Object-Oriented Software*.

---

## Sumário

1. [Por que os padrões são agrupados](#1-por-que-os-padrões-são-agrupados)
2. [Creational Patterns](#2-creational-patterns)
3. [Structural Patterns](#3-structural-patterns)
4. [Behavioral Patterns](#4-behavioral-patterns)
5. [Onde o Builder entra nesse mapa](#5-onde-o-builder-entra-nesse-mapa)
6. [O Builder em Profundidade](#6-o-builder-em-profundidade)
7. [Conclusão](#7-conclusão)
8. [Referências bibliográficas](#8-referências-bibliográficas)

---

## 1. Por que os padrões são agrupados

[⬆️ Voltar ao Sumário](#sumário)

O livro clássico do GoF organiza os padrões em três categorias por uma razão simples: **nem todo problema de orientação a objetos é do mesmo tipo**.

Alguns problemas surgem quando a criação de objetos começa a ficar rígida, repetitiva ou acoplada demais às classes concretas. Outros aparecem quando o sistema precisa combinar objetos em estruturas maiores sem perder clareza. Outros, ainda, estão ligados à forma como responsabilidades, decisões e mensagens circulam entre os participantes do sistema.

Essa organização é conhecida como **Gamma Categorization**, em referência a **Erich Gamma**, um dos autores do GoF. Ela ajuda o leitor a não estudar padrões como receitas isoladas, mas como respostas para famílias de problemas.

Em outras palavras:

- **Criacionais** perguntam: como os objetos devem ser criados?
- **Estruturais** perguntam: como os objetos e classes devem ser compostos?
- **Comportamentais** perguntam: como os objetos devem colaborar?

Essa distinção não é matemática nem rígida. Alguns padrões tocam mais de uma dimensão ao mesmo tempo. Ainda assim, essa classificação continua extremamente útil porque dá direção intelectual ao estudo.

---

## 2. Creational Patterns

[⬆️ Voltar ao Sumário](#sumário)

Os **Creational Patterns** tratam dos mecanismos de criação de objetos. A ideia central é evitar que o código cliente fique excessivamente acoplado ao processo concreto de instanciação.

Em vez de simplesmente espalhar `new` por toda parte sem critério, esses padrões perguntam:

- quem deve ser responsável por criar o objeto?
- a criação é direta ou indireta?
- o objeto nasce pronto de uma vez ou é montado em etapas?
- o cliente precisa conhecer a classe concreta ou apenas a abstração?

Essa categoria é especialmente importante porque a criação de objetos parece trivial no início, mas rapidamente se torna um ponto de acoplamento forte em sistemas reais.

Os cinco padrões criacionais clássicos são:

- **Singleton:** garante que uma classe tenha apenas uma instância e oferece um ponto global de acesso a ela.
- **Factory Method:** define uma interface para criação de objetos, mas deixa que subclasses decidam qual classe concreta será instanciada.
- **Abstract Factory:** fornece uma interface para criar famílias de objetos relacionados ou dependentes entre si.
- **Builder:** separa a construção de um objeto complexo de sua representação.
- **Prototype:** define os tipos de objetos a serem criados a partir de uma instância prototípica e gera novos objetos por cópia desse protótipo.

### 2.1 O papel especial do Builder entre os criacionais

[⬆️ Voltar ao Sumário](#sumário)

Dentro dos padrões criacionais, o `Builder` ocupa um lugar muito particular.

Enquanto alguns padrões tentam responder **quem cria** ou **qual classe concreta deve nascer**, o `Builder` se concentra em outra pergunta:

**“Como construir um objeto complexo sem tornar sua criação confusa, frágil ou acoplada demais?”**

Esse detalhe faz do `Builder` um padrão extremamente útil quando:

- o objeto possui muitos passos de construção;
- há combinações opcionais de configuração;
- o construtor tradicional ficaria grande, ambíguo ou difícil de ler;
- a montagem do objeto precisa ser separada de sua representação final.

---

## 3. Structural Patterns

[⬆️ Voltar ao Sumário](#sumário)

Os **Structural Patterns** explicam como classes e objetos podem ser montados em estruturas maiores de forma flexível, eficiente e sustentável.

Se os padrões criacionais olham para o nascimento do objeto, os estruturais olham para a **forma como as peças se conectam**.

Essa categoria costuma enfatizar temas como:

- composição em vez de acoplamento rígido;
- adaptação entre interfaces incompatíveis;
- encapsulamento de complexidade;
- construção de APIs mais claras;
- reutilização por wrapping, delegação e organização estrutural.

Os padrões estruturais clássicos são:

- **Adapter:** permite que objetos com interfaces incompatíveis consigam colaborar.
- **Bridge:** desacopla uma abstração de sua implementação para que ambas possam variar independentemente.
- **Composite:** permite compor objetos em estruturas de árvore para representar hierarquias parte-todo.
- **Decorator:** adiciona novos comportamentos dinamicamente a objetos ao envolvê-los em wrappers especializados.
- **Facade:** fornece uma interface simplificada para uma biblioteca, framework ou conjunto complexo de classes.
- **Flyweight:** usa compartilhamento para suportar grandes quantidades de objetos finos de forma eficiente.
- **Proxy:** fornece um substituto ou representante de outro objeto para controlar o acesso a ele.

Uma observação importante: muitos padrões estruturais parecem “invisíveis” para iniciantes porque não estão focados em algoritmos chamativos, e sim em desenho de interface e composição. Mas é justamente aí que boa parte da qualidade arquitetural se decide.

---

## 4. Behavioral Patterns

[⬆️ Voltar ao Sumário](#sumário)

Os **Behavioral Patterns** estão ligados a algoritmos, distribuição de responsabilidades e comunicação entre objetos.

Em vez de perguntar apenas “como criar” ou “como montar”, esses padrões perguntam:

- quem decide o quê?
- como uma requisição deve circular?
- como objetos trocam informação sem acoplamento desnecessário?
- como representar comportamento variável de modo elegante?

Essa categoria é a mais diversa do catálogo GoF, mas o núcleo comum está na coordenação do comportamento do sistema.

Os padrões comportamentais clássicos são:

- **Chain of Responsibility:** passa requisições ao longo de uma cadeia de tratadores.
- **Command:** encapsula uma requisição como objeto, permitindo parametrizar clientes com diferentes pedidos.
- **Interpreter:** dada uma linguagem, define uma representação para sua gramática junto com um interpretador.
- **Iterator:** acessa sequencialmente os elementos de uma coleção sem expor sua representação interna.
- **Mediator:** define um objeto que encapsula como um conjunto de objetos interage.
- **Memento:** captura e externaliza o estado interno de um objeto sem violar encapsulamento.
- **Observer:** define um mecanismo de assinatura para notificar múltiplos objetos sobre eventos ocorridos no objeto observado.
- **State:** permite que um objeto altere seu comportamento quando seu estado interno muda.
- **Strategy:** define uma família de algoritmos, encapsula cada um deles e os torna intercambiáveis.
- **Template Method:** define o esqueleto de um algoritmo em uma superclasse, permitindo que subclasses sobrescrevam etapas específicas.
- **Visitor:** separa algoritmos dos objetos sobre os quais eles operam.

Ao estudar essa categoria, o leitor normalmente percebe um ponto importante: orientação a objetos não trata apenas de modelar dados, mas também de organizar decisões, mensagens e variações de comportamento.

---

## 5. Onde o Builder entra nesse mapa

[⬆️ Voltar ao Sumário](#sumário)

Agora que o mapa geral está claro, fica mais fácil entender por que o `Builder` pertence ao grupo dos padrões criacionais.

O problema que ele enfrenta não é, primariamente:

- incompatibilidade entre interfaces;
- excesso de wrappers estruturais;
- fluxo de mensagens entre participantes;
- troca de algoritmos em tempo de execução.

O problema central do `Builder` é outro:

**a construção de objetos complexos pode ficar ruim demais quando concentrada em construtores enormes, parâmetros demais ou montagem desorganizada.**

Em muitos sistemas, o objeto final:

- tem várias partes;
- pode ser montado em diferentes ordens ou combinações;
- possui opções obrigatórias e opcionais;
- precisa de uma criação legível e segura.

É aí que o `Builder` se destaca. Ele permite separar:

- **o processo de construção**;
- **a representação final do objeto**.

Isso melhora:

- legibilidade;
- clareza de intenção;
- encapsulamento da montagem;
- flexibilidade na criação;
- possibilidade de representar diferentes versões do mesmo objeto complexo.

Em termos didáticos, uma forma simples de memorizar é esta:

- **Factory** costuma responder melhor a “qual objeto deve nascer?”.
- **Builder** costuma responder melhor a “como esse objeto complexo deve ser montado?”.

Essa diferença parece sutil, mas é decisiva.

---

## 6. O Builder em Profundidade

[⬆️ Voltar ao Sumário](#sumário)

Depois de localizar o `Builder` dentro da Gamma Categorization, vale dedicar uma seção ao padrão em si. A partir daqui, a pergunta deixa de ser apenas “em que categoria ele cai?” e passa a ser:

**o que exatamente o `Builder` representa, que problema ele resolve e como isso aparece no exemplo atual da aula?**

### 6.1 O que o Builder representa

[⬆️ Voltar ao Sumário](#sumário)

Em termos clássicos do GoF, o `Builder` existe para **separar a construção de um objeto complexo de sua representação**, permitindo que o mesmo processo de construção possa gerar um resultado final sem obrigar o cliente a conhecer todos os detalhes da montagem.

Traduzindo isso para linguagem mais direta:

- existe um objeto final que não é trivial de montar;
- essa montagem tem etapas;
- essas etapas podem crescer ou variar;
- o cliente não deveria carregar sozinho esse processo.

O ganho não é apenas escrever menos. O ganho principal é:

- melhorar a legibilidade do cliente;
- centralizar a lógica de montagem;
- reduzir repetição;
- permitir evolução mais segura da criação;
- expressar intenção com mais clareza.

**Como interpretar a definição:** o `Builder` não existe para “substituir qualquer construtor”. Ele aparece quando a criação deixa de ser simples o bastante para ficar confortável no próprio cliente.

### 6.2 O problema que ele resolve

[⬆️ Voltar ao Sumário](#sumário)

O problema típico do `Builder` não é “como concatenar texto” nem “como instanciar com `new`”. O problema é este:

**como construir algo mais elaborado sem obrigar o código cliente a executar manualmente cada detalhe da montagem?**

Esse problema costuma aparecer quando:

- o produto final tem muitas partes;
- a ordem dos passos importa;
- há muitos elementos opcionais;
- a criação começa a se repetir em vários lugares;
- o cliente começa a parecer procedural demais.

No espírito do GoF, o `Builder` existe para que o cliente diga mais claramente **o que quer construir**, enquanto o builder cuida de **como aquilo será montado**.

### 6.3 Participantes clássicos do padrão

[⬆️ Voltar ao Sumário](#sumário)

Na formulação clássica, o padrão `Builder` costuma envolver estes participantes:

- **Product:** o objeto complexo final.
- **Builder:** a abstração que define os passos da construção.
- **ConcreteBuilder:** a implementação concreta desses passos.
- **Director:** o coordenador que sabe em que ordem chamar as etapas.
- **Client:** quem dispara o processo e consome o resultado.

Esse desenho é útil porque separa responsabilidades:

- o produto representa o resultado;
- o builder representa o processo;
- o director representa a receita de montagem;
- o cliente apenas consome a API.

### 6.4 Como isso aparece no exemplo da aula

[⬆️ Voltar ao Sumário](#sumário)

Nos arquivos atuais da aula, a ideia aparece assim:

- `SemBuilder.cs` mostra o cliente montando HTML manualmente;
- `Builder.cs` mostra o cliente delegando a montagem para tipos próprios.

Mapeando para os papéis clássicos:

- `HtmlElement` funciona como o **Product**;
- `HtmlBuilder` funciona como o **ConcreteBuilder**;
- o `Main` funciona como o **Client**;
- o **Director** não aparece como classe separada.

Isso é importante: muitas implementações modernas de `Builder` em C# não usam um `Director` explícito. O próprio cliente chama o builder diretamente.

**Regra prática:** se a construção já ficou legível e o cliente não está repetindo uma receita complexa, um `Director` separado pode ser opcional.

### 6.5 Life Without Builder vs Builder

[⬆️ Voltar ao Sumário](#sumário)

Em `SemBuilder.cs`, o cliente trabalha neste nível:

```csharp
sb.Append("<ul>");
foreach (var word in words)
{
    sb.AppendFormat("<li>{0}</li>", word);
}
sb.Append("</ul>");
```

Aqui o cliente:

- conhece as tags;
- conhece a ordem das tags;
- conhece o formato dos itens;
- sabe como montar a saída final.

Ou seja: o cliente não está apenas pedindo uma lista HTML. Ele está **executando o processo inteiro de montagem**.

Já em `Builder.cs`, o cliente passa a trabalhar assim:

```csharp
var builder = new HtmlBuilder("ul");
builder.AddChild("li", "hello");
builder.AddChild("li", "world");
WriteLine(builder.ToString());
```

Agora o cliente não pensa mais em:

- abrir tag;
- fechar tag;
- concatenar texto;
- controlar manualmente o formato final.

Ele pensa em:

- criar a estrutura;
- adicionar partes;
- pedir a representação final.

Essa é a virada conceitual principal da aula.

**Como interpretar a comparação:** sem builder, o cliente executa a obra. Com builder, o cliente descreve a obra e o builder cuida da montagem.

### 6.6 Builder Pattern não é StringBuilder

[⬆️ Voltar ao Sumário](#sumário)

Esse é um dos pontos mais fáceis de confundir no começo.

`System.Text.StringBuilder` é uma classe do .NET para montagem eficiente de texto. Já o `Builder` do GoF é um padrão de projeto.

As diferenças são:

- `StringBuilder` resolve custo de concatenação e alocação de strings;
- `Builder Pattern` resolve custo cognitivo e estrutural da criação de objetos complexos.

No exemplo da aula:

- em `SemBuilder.cs`, o `StringBuilder` é a ferramenta principal da solução;
- em `Builder.cs`, o foco sai da ferramenta de string e passa para a abstração de construção.

**Armadilha comum:** achar que qualquer classe com “builder” no nome já implementa o padrão `Builder`. O nome pode coincidir, mas a função conceitual é outra.

### 6.7 Quando usar e quando não usar

[⬆️ Voltar ao Sumário](#sumário)

Use `Builder` quando:

- o produto tem várias partes;
- a criação em etapas melhora a leitura;
- existem combinações opcionais demais para um construtor simples;
- a montagem está ficando repetitiva no cliente;
- o objeto final é complexo o bastante para merecer uma abstração própria.

Em contrapartida, ele pode ser exagero quando:

- o objeto é simples;
- o construtor já é claro;
- um object initializer resolveria bem;
- a criação não tem etapas nem complexidade real.

**Regra prática:** se a criação não está doendo, talvez você ainda não precise de um `Builder`.

### 6.8 Erros comuns

[⬆️ Voltar ao Sumário](#sumário)

Alguns erros aparecem com frequência:

- confundir `Builder Pattern` com `StringBuilder`;
- usar builder quando um construtor simples bastava;
- criar um builder, mas continuar deixando o cliente responsável por detalhes demais;
- achar que todo builder precisa ser fluente;
- esquecer que o objetivo é reduzir responsabilidade do cliente.

No seu caso atual, a linha mais importante para fixar é esta:

- `SemBuilder.cs` mostra montagem manual de string;
- `Builder.cs` mostra encapsulamento do processo de construção.

Se essa comparação estiver clara, o restante das variações do capítulo fica muito mais natural.

---

## 7. Conclusão

[⬆️ Voltar ao Sumário](#sumário)

Antes de estudar qualquer padrão específico, vale muito a pena compreender o mapa geral dos padrões GoF.

A **Gamma Categorization** mostra que os padrões clássicos se distribuem em três famílias:

- **Creational Patterns**, que tratam da criação de objetos;
- **Structural Patterns**, que tratam da composição e organização estrutural;
- **Behavioral Patterns**, que tratam da colaboração e do comportamento entre objetos.

Dentro desse panorama, o `Builder` aparece como um padrão criacional voltado à construção passo a passo de objetos complexos, separando o ato de montar o objeto da forma final como ele será representado.

Quando olhamos o exemplo atual da aula, essa ideia sai do abstrato:

- em `SemBuilder.cs`, o cliente monta HTML diretamente;
- em `Builder.cs`, o cliente delega a montagem para um builder próprio.

Essa transição é o ponto pedagógico central deste momento do capítulo.

Nos próximos trechos da trilha, a tendência natural é ver essa mesma ideia evoluindo para APIs fluentes, builders guiados por etapas, builders funcionais e builders facetados. Mas a base continua a mesma: **tirar da mão do cliente o peso de montar sozinho algo complexo**.

---

## 8. Referências bibliográficas

[⬆️ Voltar ao Sumário](#sumário)

Referência principal utilizada na organização conceitual deste capítulo:

- **Gamma, Erich; Helm, Richard; Johnson, Ralph; Vlissides, John.** *Design Patterns: Elements of Reusable Object-Oriented Software*. Addison-Wesley, 1994.

Referências complementares de contexto usadas no projeto:

- `1.SOLID/SOLID.md`
- `C#_Fundamentals.md`
- `2.Builder/Aula01_builder/SemBuilder.cs`
- `2.Builder/Aula01_builder/Builder.cs`

Observação importante:

- o termo **Gang of Four (GoF)** se refere exatamente a esses quatro autores;
- a expressão **Gamma Categorization** faz referência a **Erich Gamma**, um dos autores da obra;
- o `Builder` desta aula é o **padrão de projeto GoF**, não a classe `System.Text.StringBuilder` do .NET.
