# Capítulo 2 - Builder

**por Gang of Four (GoF)**  

> **Base conceitual:** *Design Patterns: Elements of Reusable Object-Oriented Software*  
> **Objetivo deste capítulo:** introduzir a categorização clássica dos Design Patterns e posicionar o padrão `Builder` dentro desse mapa conceitual

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

Este capítulo começa por esse panorama porque o padrão `Builder` só fica realmente claro quando o leitor entende o lugar que ele ocupa no ecossistema dos padrões.

---

## Como ler este capítulo

[⬆️ Voltar ao Sumário](#sumário)

Este material foi escrito com duas intenções ao mesmo tempo:

1. **Dar visão de mapa:** mostrar como os padrões GoF foram agrupados.
2. **Preparar o terreno para o Builder:** explicar por que ele pertence aos padrões criacionais e qual tipo de problema ele resolve.

Se você está começando, leia em ordem. Se já conhece alguns padrões, use este texto como uma introdução conceitual para alinhar terminologia e contexto antes de mergulhar em implementações.

---

## Sumário

1. [Por que os padrões são agrupados](#1-por-que-os-padrões-são-agrupados)
2. [Creational Patterns](#2-creational-patterns)
3. [Structural Patterns](#3-structural-patterns)
4. [Behavioral Patterns](#4-behavioral-patterns)
5. [Onde o Builder entra nesse mapa](#5-onde-o-builder-entra-nesse-mapa)
6. [Conclusão](#6-conclusão)

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

## 6. Conclusão

[⬆️ Voltar ao Sumário](#sumário)

Antes de estudar qualquer padrão específico, vale muito a pena compreender o mapa geral dos padrões GoF.

A **Gamma Categorization** mostra que os padrões clássicos se distribuem em três famílias:

- **Creational Patterns**, que tratam da criação de objetos;
- **Structural Patterns**, que tratam da composição e organização estrutural;
- **Behavioral Patterns**, que tratam da colaboração e do comportamento entre objetos.

Dentro desse panorama, o `Builder` aparece como um padrão criacional voltado à construção passo a passo de objetos complexos, separando o ato de montar o objeto da forma final como ele será representado.

Essa introdução é importante porque ela evita um erro muito comum: estudar o `Builder` como uma técnica isolada, sem perceber que ele faz parte de uma discussão maior sobre **criação de objetos, desacoplamento e clareza de design**.

Nos próximos trechos deste capítulo, o foco natural passa a ser justamente esse: entender com profundidade o problema que o `Builder` resolve, por que ele existe e em que tipo de design ele realmente faz diferença.
