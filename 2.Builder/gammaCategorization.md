# Capítulo 2 - Gamma Categorization e Builder

**por Erich Gamma, Richard Helm, Ralph Johnson e John Vlissides (Gang of Four - GoF)**

> **Livro de referência principal:** *Design Patterns: Elements of Reusable Object-Oriented Software*
> **Autores da obra-base:** Erich Gamma, Richard Helm, Ralph Johnson e John Vlissides
> **Objetivo deste capítulo:** explicar a categorização clássica dos Design Patterns com profundidade conceitual, posicionar o padrão `Builder` dentro desse mapa e conectar a teoria aos exemplos desta pasta

---

## Prefácio

[⬆️ Voltar ao Sumário](#sumário)

Muita gente aprende Design Patterns em camadas superficiais. Primeiro descobre nomes como `Singleton`, `Factory Method`, `Adapter`, `Observer`, `Strategy` e `Builder`. Depois tenta memorizar diagramas, exemplos e definições curtas. O problema é que, sem um critério de organização, todo esse conhecimento vira apenas uma prateleira de técnicas soltas.

É exatamente nesse ponto que a **Gamma Categorization** se torna importante.

Ela não surgiu para “embelezar índice de livro” nem para dar um rótulo acadêmico aos padrões. Ela existe porque os problemas de design orientado a objetos **não são todos do mesmo tipo**. Alguns nascem na criação de objetos. Outros aparecem na forma como classes e objetos se conectam. Outros emergem da distribuição de responsabilidades e do fluxo de comportamento.

Por isso, os padrões clássicos do livro do GoF foram organizados em **três grandes categorias**, cada uma destacando uma família diferente de problemas:

- **Creational Patterns**, voltados à criação de objetos;
- **Structural Patterns**, voltados à composição de classes e objetos;
- **Behavioral Patterns**, voltados à distribuição de responsabilidades e ao fluxo de comunicação.

Essa divisão não existe só para organizar capítulos. Ela existe para responder uma pergunta muito prática e muito útil em engenharia:

**“Que tipo de problema de design estou tentando resolver?”**

Se o problema está na forma como objetos nascem, o raciocínio tende para padrões criacionais. Se está na forma como eles se conectam, o foco tende para padrões estruturais. Se está na forma como colaboram, decidem e trocam mensagens, o problema costuma ser comportamental.

Este capítulo foi escrito para evitar o estudo “decoreba” de padrões. A proposta aqui é tratar `Gamma Categorization` e `Builder` como assuntos de design e engenharia, e não apenas como nomes famosos de entrevista.

---

## Como ler este capítulo

[⬆️ Voltar ao Sumário](#sumário)

Este material foi escrito em duas camadas ao mesmo tempo:

1. **Camada conceitual:** explica por que os padrões foram agrupados e que tipo de problema cada grupo tenta resolver.
2. **Camada prática:** conecta esse mapa ao padrão `Builder` e aos arquivos reais desta pasta.

Se você está começando, a melhor leitura é sequencial. Se você já conhece alguns padrões, use o texto como guia para alinhar o modelo mental antes de mergulhar nas implementações.

Ao longo do capítulo, pense sempre nestas quatro perguntas:

- **O que isso representa?** Conceito, papel ou contrato de design.
- **Quando eu usaria isso?** Cenário prático em que essa ideia realmente ajuda.
- **O que isso custa?** Complexidade, abstração, acoplamento ou manutenção.
- **Qual erro é comum aqui?** Armadilha conceitual, abuso de padrão ou leitura equivocada do problema.

Ao longo do texto, quando aparecerem expressões como **GoF**, **Gamma Categorization** ou **padrões clássicos**, a referência principal é sempre o livro:

- **Gamma, Erich; Helm, Richard; Johnson, Ralph; Vlissides, John.** *Design Patterns: Elements of Reusable Object-Oriented Software*.

---

## Sumário

- [Como ler este capítulo](#como-ler-este-capítulo)
- Parte 1 — Gamma Categorization
  - [1. Por que os padrões são agrupados](#1-por-que-os-padrões-são-agrupados)
    - [1.1 O valor de um mapa conceitual](#11-o-valor-de-um-mapa-conceitual)
    - [1.2 O que a Gamma Categorization evita](#12-o-que-a-gamma-categorization-evita)
  - [2. Creational Patterns](#2-creational-patterns)
    - [2.1 O que problemas criacionais têm em comum](#21-o-que-problemas-criacionais-têm-em-comum)
    - [2.2 Como estudar a família criacional sem decorar](#22-como-estudar-a-família-criacional-sem-decorar)
    - [2.3 O papel especial do Builder entre os criacionais](#23-o-papel-especial-do-builder-entre-os-criacionais)
  - [3. Structural Patterns](#3-structural-patterns)
    - [3.1 O que torna um problema estrutural](#31-o-que-torna-um-problema-estrutural)
    - [3.2 Por que padrões estruturais costumam parecer menos óbvios](#32-por-que-padrões-estruturais-costumam-parecer-menos-óbvios)
  - [4. Behavioral Patterns](#4-behavioral-patterns)
    - [4.1 O que torna um problema comportamental](#41-o-que-torna-um-problema-comportamental)
    - [4.2 O que essa categoria ensina sobre OO](#42-o-que-essa-categoria-ensina-sobre-oo)
  - [5. Onde o Builder entra nesse mapa](#5-onde-o-builder-entra-nesse-mapa)
    - [5.1 O que o Builder não está tentando resolver](#51-o-que-o-builder-não-está-tentando-resolver)
    - [5.2 Por que o Builder costuma ser subestimado](#52-por-que-o-builder-costuma-ser-subestimado)
- Parte 2 — Builder em Profundidade
  - [6. O Builder em Profundidade](#6-o-builder-em-profundidade)
    - [6.1 O que o Builder representa](#61-o-que-o-builder-representa)
    - [6.2 O problema que ele resolve](#62-o-problema-que-ele-resolve)
    - [6.3 Participantes clássicos do padrão](#63-participantes-clássicos-do-padrão)
    - [6.4 Como isso aparece no exemplo da aula](#64-como-isso-aparece-no-exemplo-da-aula)
    - [6.5 Life Without Builder vs Builder](#65-life-without-builder-vs-builder)
    - [6.6 Builder Pattern não é StringBuilder](#66-builder-pattern-não-é-stringbuilder)
    - [6.7 Quando usar e quando não usar](#67-quando-usar-e-quando-não-usar)
    - [6.8 Erros comuns](#68-erros-comuns)
    - [6.9 Mapa das próximas variações de Builder](#69-mapa-das-próximas-variações-de-builder)
- Parte 3 — Fechamento
  - [7. Conclusão](#7-conclusão)
  - [8. Referências bibliográficas](#8-referências-bibliográficas)

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

É importante perceber a diferença entre duas situações:

- conhecer nomes de padrões;
- saber em que família de problema seu design está tropeçando.

A primeira situação produz repertório. A segunda produz critério. E, em software real, critério costuma valer mais do que coleção de nomes.

### 1.1 O valor de um mapa conceitual

[⬆️ Voltar ao Sumário](#sumário)

Quem estuda padrões sem um mapa costuma cair em dois erros:

- tratar cada padrão como uma receita isolada;
- tentar escolher padrão por familiaridade, e não pelo tipo real do problema.

É por isso que a categorização importa tanto. Ela não serve apenas para “organizar capítulos”. Ela serve para **organizar o raciocínio de design**.

Quando você pensa em categorias antes de pensar em nomes, a pergunta muda de nível:

- em vez de “será que aqui cabe `Builder`?”;
- você começa com “meu problema é de criação, estrutura ou comportamento?”.

Esse deslocamento é pequeno na aparência, mas muito importante na prática. Ele reduz o impulso de aplicar padrão por modismo e aumenta a chance de escolher uma solução coerente com a natureza do problema.

Na prática, isso significa que a categorização ajuda você a evitar perguntas ruins como:

- “qual padrão eu consigo encaixar aqui?”;
- “qual padrão parece mais sofisticado?”;
- “qual padrão eu lembro mais rápido?”.

E ajuda a fazer perguntas melhores como:

- “meu problema é de criação, estrutura ou comportamento?”;
- “onde está o atrito principal do design?”;
- “o cliente está sofrendo com montagem, composição ou colaboração?”.

### 1.2 O que a Gamma Categorization evita

[⬆️ Voltar ao Sumário](#sumário)

A classificação de Gamma ajuda a combater alguns hábitos ruins muito comuns:

- decorar nomes sem entender intenção;
- usar padrões como ornamento arquitetural;
- misturar problemas de construção com problemas de colaboração;
- confundir dificuldade de implementação com importância conceitual.

Por exemplo:

- se o problema real é “como montar um objeto complexo”, olhar primeiro para padrões estruturais tende a dispersar o raciocínio;
- se o problema real é “como objetos se notificam sem forte acoplamento”, pensar em padrões criacionais como primeira opção também costuma ser desvio.

**Como interpretar essa seção:** a Gamma Categorization não responde tudo, mas cria um primeiro filtro extremamente valioso. Ela não escolhe o padrão por você; ela impede que você comece a análise no lugar errado.

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

É comum subestimar esse tipo de problema porque `new` parece simples. Mas “instanciar” e “desenhar bem o nascimento de um objeto” são coisas diferentes. Um sistema pode compilar com dezenas de `new` espalhados e ainda assim ter um design frágil, rígido e difícil de evoluir.

Os cinco padrões criacionais clássicos são:

- **Singleton:** garante que uma classe tenha apenas uma instância e oferece um ponto global de acesso a ela.
- **Factory Method:** define uma interface para criação de objetos, mas deixa que subclasses decidam qual classe concreta será instanciada.
- **Abstract Factory:** fornece uma interface para criar famílias de objetos relacionados ou dependentes entre si.
- **Builder:** separa a construção de um objeto complexo de sua representação.
- **Prototype:** define os tipos de objetos a serem criados a partir de uma instância prototípica e gera novos objetos por cópia desse protótipo.

### 2.1 O que problemas criacionais têm em comum

[⬆️ Voltar ao Sumário](#sumário)

Os padrões criacionais parecem diferentes entre si, mas compartilham uma ansiedade comum de design:

**deixar a criação explícita demais no cliente costuma aumentar acoplamento e reduzir flexibilidade.**

Isso pode acontecer de várias formas:

- o cliente conhece classes concretas demais;
- o processo de criação se espalha por muitos pontos do sistema;
- a montagem do objeto fica longa e confusa;
- famílias de objetos relacionados precisam nascer de forma consistente;
- copiar um objeto pronto fica melhor do que reconstruí-lo do zero.

Cada padrão criacional ataca esse problema por um ângulo específico:

- `Singleton` controla quantidade e acesso;
- `Factory Method` desloca a decisão da classe concreta;
- `Abstract Factory` organiza famílias de objetos;
- `Builder` organiza o processo de montagem;
- `Prototype` desloca a criação para cópia de instâncias existentes.

### 2.2 Como estudar a família criacional sem decorar

[⬆️ Voltar ao Sumário](#sumário)

Em vez de decorar definições, vale usar estas perguntas:

- o cliente sabe demais sobre a classe concreta?
- o problema é escolher o tipo certo ou montar o objeto certo?
- existe uma família de objetos que precisa nascer de forma coerente?
- copiar um objeto seria mais natural do que recriá-lo?
- a criação tem etapas demais para ficar clara em construtores simples?

Esse tipo de pergunta é mais útil do que tentar memorizar “assinaturas típicas” dos padrões.

**Como interpretar esta parte:** estudar padrões criacionais do jeito certo não é treinar a memória para repetir definições. É treinar o olhar para reconhecer quando o problema é realmente de criação e qual aspecto dessa criação está causando atrito.

### 2.3 O papel especial do Builder entre os criacionais

[⬆️ Voltar ao Sumário](#sumário)

Dentro dos padrões criacionais, o `Builder` ocupa um lugar muito particular.

Enquanto alguns padrões tentam responder **quem cria** ou **qual classe concreta deve nascer**, o `Builder` se concentra em outra pergunta:

**“Como construir um objeto complexo sem tornar sua criação confusa, frágil ou acoplada demais?”**

Esse detalhe faz do `Builder` um padrão extremamente útil quando:

- o objeto possui muitos passos de construção;
- há combinações opcionais de configuração;
- o construtor tradicional ficaria grande, ambíguo ou difícil de ler;
- a montagem do objeto precisa ser separada de sua representação final.

**Como interpretar essa diferença:** `Builder` não compete diretamente com todos os outros criacionais o tempo todo. Muitas vezes ele entra depois que você já percebeu que o problema não é apenas “qual classe instanciar”, mas sim “como organizar um processo de construção que ficou grande demais para o cliente carregar sozinho”.

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

É nessa família que muita gente percebe que design orientado a objetos não é só “ter classes”. É também decidir:

- como uma abstração se apresenta;
- como um objeto pode ser envolvido sem ser reescrito;
- como duas peças incompatíveis podem colaborar;
- como uma estrutura maior pode continuar legível em vez de virar emaranhado.

### 3.1 O que torna um problema estrutural

[⬆️ Voltar ao Sumário](#sumário)

Um problema estrutural normalmente aparece quando os objetos até existem, mas sua **forma de encaixe** está ruim.

Sinais comuns:

- interfaces incompatíveis impedem colaboração;
- uma API concreta está vazando complexidade demais;
- você quer compor comportamentos sem explodir herança;
- a estrutura parte-todo precisa ser representada com consistência;
- o acesso ao objeto real precisa de mediação, controle ou laziness.

Perceba que a pergunta muda bastante em relação aos criacionais.

Nos estruturais, a preocupação já não é principalmente “como o objeto nasceu?”, e sim:

**“como os objetos se conectam, se apresentam e se organizam?”**

### 3.2 Por que padrões estruturais costumam parecer menos óbvios

[⬆️ Voltar ao Sumário](#sumário)

Muita gente acha padrões estruturais mais difíceis de perceber porque eles nem sempre aparecem como “grandes mecanismos”. Às vezes o ganho deles é silencioso:

- uma interface fica mais limpa;
- um acoplamento desaparece;
- uma composição substitui uma herança ruim;
- uma biblioteca difícil ganha uma fachada mais clara.

Isso pode parecer menos impressionante do que um padrão com mais passos de execução, mas arquiteturalmente costuma ser decisivo.

**Como interpretar essa seção:** problemas estruturais raramente pedem “mais lógica”. Eles pedem melhor organização de fronteiras, composição e relações entre partes.

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

Se os criacionais perguntam “como nasce?” e os estruturais perguntam “como se encaixa?”, os comportamentais costumam perguntar algo como:

**“como decide, quem reage e por onde o comportamento circula?”**

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

### 4.1 O que torna um problema comportamental

[⬆️ Voltar ao Sumário](#sumário)

Um problema comportamental aparece quando o núcleo da dificuldade não está na criação do objeto nem na sua forma estrutural, mas na forma como responsabilidades e decisões circulam.

Exemplos de sinais:

- lógica condicional demais para escolher comportamento;
- excesso de acoplamento entre quem emite e quem reage;
- algoritmos intercambiáveis tratados como ifs espalhados;
- estados internos alterando o comportamento de maneira confusa;
- necessidade de representar comandos, roteamento ou cadeias de decisão.

Em outras palavras, a pergunta central muda para algo como:

**“quem deve decidir, quando deve decidir e como essa decisão deve viajar pelo sistema?”**

### 4.2 O que essa categoria ensina sobre OO

[⬆️ Voltar ao Sumário](#sumário)

A família comportamental lembra uma lição importante: orientação a objetos não é só encapsular dados em classes. É também distribuir comportamento de forma que o sistema:

- mude com menos atrito;
- exponha menos condicionais espalhadas;
- desacople emissor e receptor quando necessário;
- trate variação de comportamento de maneira explícita.

Esse grupo costuma ser o mais diverso justamente porque comportamento pode variar por estratégia, estado, comando, observação, visita, mediação e outros mecanismos.

**Como interpretar essa seção:** quando o problema central é “como objetos colaboram, decidem e reagem”, o raciocínio comportamental geralmente é o melhor ponto de partida.

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

Muita gente olha o `Builder` pela primeira vez e pensa: “isso é só um construtor mais elaborado”. Essa leitura perde o ponto principal. O `Builder` não existe para decorar criação com mais métodos; ele existe para retirar do cliente uma responsabilidade de montagem que começou a ficar pesada demais, frágil demais ou verbosa demais.

### 5.1 O que o Builder não está tentando resolver

[⬆️ Voltar ao Sumário](#sumário)

Uma boa forma de entender o `Builder` é olhar também para o que ele **não** está tentando resolver.

Ele não existe, primariamente, para:

- adaptar interfaces incompatíveis;
- controlar comunicação entre observadores;
- encapsular algoritmos intercambiáveis;
- representar estados dinâmicos;
- proteger acesso a objetos reais.

Tudo isso pode ser importante em outros contextos, mas não é o coração do problema do `Builder`.

O foco dele é outro:

**criação complexa demais para continuar confortável, legível e segura no código cliente.**

### 5.2 Por que o Builder costuma ser subestimado

[⬆️ Voltar ao Sumário](#sumário)

No começo, muita gente subestima o `Builder` porque pensa:

- “isso eu resolveria com um construtor”;
- “isso é só criar objeto com método”;
- “isso parece exagero”.

E às vezes realmente seria exagero. Mas o padrão passa a fazer sentido quando a montagem começa a ter:

- muitos passos;
- combinações opcionais;
- ordem relevante;
- risco de leitura ruim;
- repetição em múltiplos clientes.

Nessa hora, o `Builder` deixa de ser “ornamento” e vira uma ferramenta para reduzir atrito cognitivo e estrutural.

**Como interpretar essa seção:** o `Builder` entra no mapa dos criacionais porque seu problema central é de criação. Mas ele só mostra seu valor total quando criação deixa de significar apenas “instanciar” e passa a significar “montar corretamente algo mais complexo do que um simples `new` comunica bem”. 

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

### 6.9 Mapa das próximas variações de Builder

[⬆️ Voltar ao Sumário](#sumário)

Ao avançar na trilha, você vai perceber que o curso fala em “vários tipos de builder”. É importante interpretar isso corretamente:

- não são novos grupos da **Gamma Categorization**;
- não são padrões GoF independentes no mesmo nível de `Builder`, `Factory` ou `Adapter`;
- são **variações de implementação e uso do mesmo núcleo conceitual do Builder**.

Ou seja: a ideia central continua a mesma.

**o cliente não deve carregar sozinho o peso de montar um objeto complexo.**

O que muda de uma variação para outra é:

- a forma da API;
- o grau de segurança na ordem dos passos;
- o uso ou não de fluência;
- a forma como a construção é dividida em aspectos;
- o custo de abstração para conseguir essas garantias.

#### 6.9.1 Fluent Builder

[⬆️ Voltar ao Sumário](#sumário)

No **Fluent Builder**, os métodos do builder passam a retornar o próprio builder, permitindo encadeamento:

```csharp
builder.AddChildFluent("li", "hello")
       .AddChildFluent("li", "world");
```

O objetivo aqui não é mudar o padrão, e sim mudar a ergonomia da API.

O que ele enfatiza:

- leitura mais fluida;
- sensação de “frase de construção”;
- menos repetição do nome da variável.

O que observar quando essa aula chegar:

- a diferença entre builder comum e builder fluente;
- por que retornar `this` já muda bastante a experiência de uso;
- onde a fluência melhora a leitura e onde pode começar a esconder complexidade.

**Como interpretar essa variação:** o `Fluent Builder` não cria um novo conceito de design. Ele pega o `Builder` básico e o torna mais expressivo para o cliente.

#### 6.9.2 Fluent Builder Inheritance with Recursive Generics

[⬆️ Voltar ao Sumário](#sumário)

Essa variante aparece quando você quer **fluência + herança** ao mesmo tempo.

O problema é sutil: quando um builder base retorna `this`, a herança pode atrapalhar o tipo concreto retornado nos métodos encadeados. A solução clássica em C# é usar **recursive generics** ou **CRTP-like pattern**.

Em linguagem prática:

- você quer reaproveitar builder por herança;
- quer manter encadeamento fluente;
- mas não quer perder o tipo concreto no retorno.

O que observar quando essa aula chegar:

- por que a herança quebra a fluência ingênua;
- por que generics autorreferentes aparecem;
- como preservar o tipo concreto nos métodos encadeados.

**Custo dessa abordagem:** ela é poderosa, mas aumenta bastante a dificuldade de leitura. É uma das variações em que o ganho de API precisa justificar o peso extra da tipagem genérica.

#### 6.9.3 Stepwise Builder

[⬆️ Voltar ao Sumário](#sumário)

O **Stepwise Builder** tenta resolver outro problema: impedir que o cliente monte o objeto em uma ordem inválida ou esqueça passos obrigatórios.

Em vez de apenas oferecer uma API fluente, ele guia o cliente por etapas:

- primeiro passo;
- segundo passo permitido;
- terceiro passo liberado só depois;
- finalização apenas quando o objeto estiver num estado válido.

O que ele enfatiza:

- segurança de construção;
- restrição de ordem;
- modelagem explícita de passos obrigatórios.

O que observar quando essa aula chegar:

- como interfaces ou tipos intermediários limitam o que pode ser chamado;
- como o compilador passa a ajudar na sequência correta;
- quando isso vale a pena e quando vira burocracia demais.

**Como interpretar essa variação:** o `Stepwise Builder` troca liberdade por segurança. Ele é especialmente útil quando a ordem da construção faz parte do contrato do domínio.

#### 6.9.4 Functional Builder

[⬆️ Voltar ao Sumário](#sumário)

O **Functional Builder** muda o estilo interno da construção. Em vez de pensar apenas em métodos mutando um objeto em andamento, ele tende a compor funções, ações ou transformações.

Em termos intuitivos:

- cada passo descreve uma mutação ou transformação;
- essas transformações são acumuladas;
- no final, o objeto é materializado com base nessa composição.

O que ele enfatiza:

- composição de comportamento;
- reaproveitamento de passos menores;
- estilo mais funcional dentro de uma linguagem orientada a objetos.

O que observar quando essa aula chegar:

- como a construção passa a ser vista como composição de operações;
- o que muda mentalmente em relação ao builder clássico;
- onde isso ajuda e onde pode ficar abstrato demais para quem ainda está fixando o padrão base.

**Regra prática:** o `Functional Builder` costuma ser interessante quando você quer modularidade fina na construção, mas pode custar mais clareza para iniciantes.

#### 6.9.5 Faceted Builder

[⬆️ Voltar ao Sumário](#sumário)

O **Faceted Builder** aparece quando o objeto é tão grande que uma única interface de builder começa a ficar inchada.

Nesse caso, a construção é dividida em **facetas** ou **aspectos** do mesmo objeto.

Exemplo mental:

- faceta de endereço;
- faceta profissional;
- faceta financeira;
- faceta de configuração técnica.

O que ele enfatiza:

- separação por responsabilidade dentro da construção;
- organização de APIs muito grandes;
- visão do mesmo produto por perspectivas diferentes.

O que observar quando essa aula chegar:

- como um único produto pode ser construído por builders especializados;
- como esses builders colaboram sem virar objetos totalmente independentes;
- por que isso ajuda quando o produto tem muitos grupos de propriedades.

**Como interpretar essa variação:** o `Faceted Builder` não divide o produto em vários produtos. Ele divide a **experiência de construção** do mesmo produto em áreas mais administráveis.

#### 6.9.6 Como olhar para todas essas variações sem se perder

[⬆️ Voltar ao Sumário](#sumário)

Uma boa forma de não se confundir nas próximas aulas é usar este mapa mental:

| Variação | Pergunta principal que ela tenta responder |
| --- | --- |
| Builder básico | Como tirar do cliente os detalhes de montagem? |
| Fluent Builder | Como tornar a API de construção mais legível e encadeável? |
| Recursive Generics Builder | Como manter fluência mesmo com herança? |
| Stepwise Builder | Como forçar ordem e passos obrigatórios? |
| Functional Builder | Como compor a construção como transformações reutilizáveis? |
| Faceted Builder | Como dividir a construção de um objeto grande por áreas de responsabilidade? |

Se você guardar isso, cada nova aula entra como uma extensão natural, e não como “mais um padrão para decorar”.

**Ponto central:** todas essas variações continuam orbitando a mesma ideia do GoF: separar o processo de construção da representação final do objeto.

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
