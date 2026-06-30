# Capitulo 3 - Factories

**por Erich Gamma, Richard Helm, Ralph Johnson e John Vlissides (Gang of Four - GoF)**

> **Livro de referencia principal:** *Design Patterns: Elements of Reusable Object-Oriented Software*  
> **Objetivo deste capitulo:** organizar a trilha de `Factories` na mesma sequencia do curso, conectando cada aula aos arquivos reais desta pasta e deixando claro o papel de cada variacao

---

## Prefacio

[Voltar ao Sumario](#sumario)

Este material foi preparado para acompanhar a trilha de `Factories` mostrada nas aulas.

Aqui a ideia nao e estudar "factory" como um nome solto, mas como uma familia de solucoes para um mesmo tipo de problema:

- controlar criacao de objetos;
- esconder detalhes de instanciacao;
- dar nomes melhores para caminhos de criacao;
- centralizar politicas de nascimento, rastreamento e substituicao;
- desacoplar o cliente de classes concretas quando necessario.

Se no capitulo de `Builder` a pergunta central era "como montar?", neste capitulo a pergunta muda para:

**"quem deve criar, por qual caminho, e com quanto controle sobre essa criacao?"**

---

## Como ler este capitulo

[Voltar ao Sumario](#sumario)

Use este documento em duas camadas:

1. **Camada conceitual:** para entender o que a familia `Factory` tenta resolver dentro dos padroes criacionais.
2. **Camada pratica:** para relacionar cada aula aos arquivos desta pasta e ao terreno que ainda sera preenchido.

Arquivos ja presentes no repositorio:

- `3.Factories/Aula01_PointExample.cs`

Observacao importante sobre o estado atual da pasta:

- o arquivo `Aula01_PointExample.cs` ja existe, mas ainda pode estar vazio ou em preparacao;
- as demais secoes foram mantidas para espelhar a trilha da imagem do curso;
- quando os proximos arquivos de `Factories` forem adicionados, este documento pode ser conectado a cada implementacao concreta.

---

## Sumario

- [19. Overview](#19-overview)
- [20. Point Example](#20-point-example)
- [21. Factory Method](#21-factory-method)
- [22. Asynchronous Factory Method](#22-asynchronous-factory-method)
- [23. Factory](#23-factory)
- [24. Object Tracking and Bulk Replacement](#24-object-tracking-and-bulk-replacement)
- [25. Inner Factory](#25-inner-factory)
- [26. Abstract Factory](#26-abstract-factory)
- [27. Abstract Factory and OCP](#27-abstract-factory-and-ocp)
- [Coding Exercise 2: Factory Coding Exercise](#coding-exercise-2-factory-coding-exercise)
- [28. Summary](#28-summary)
- [Referencias bibliograficas](#referencias-bibliograficas)

---

## 19. Overview

[Voltar ao Sumario](#sumario)

### 19.1 O que a ideia de Factory representa

No sentido mais amplo, `Factory` e um jeito de tirar do cliente a responsabilidade de decidir detalhes de instanciacao.

Em vez de espalhar `new` pelo sistema inteiro, a criacao passa a acontecer por caminhos mais controlados.

Isso pode significar:

- um metodo estatico com nome melhor do que um construtor;
- uma classe separada especializada em criar objetos;
- uma fabrica interna ao proprio tipo;
- uma abstracao que produz familias inteiras de objetos relacionados.

### 19.2 O problema que ela resolve

Nem todo `new` e um problema. O problema comeca quando o nascimento do objeto carrega informacoes demais, escolhas demais ou efeitos colaterais demais.

Sinais comuns:

- construtores com intencao ambigua;
- varios modos validos de criar o mesmo tipo;
- regras de criacao espalhadas;
- necessidade de trocar implementacoes concretas sem reescrever o cliente;
- necessidade de registrar, contar, rastrear ou substituir objetos criados.

### 19.3 Factory responde a qual pergunta?

Uma forma simples de separar os padroes criacionais e esta:

- `Factory` responde melhor a **qual objeto deve nascer e por qual caminho**;
- `Builder` responde melhor a **como um objeto deve ser montado passo a passo**.

Entao, mesmo pertencendo a mesma familia criacional, eles atacam dores diferentes.

### 19.4 O que muda ao longo da trilha

Neste capitulo, o nome "Factories" cobre varias tecnicas aparentadas.

O que muda de uma aula para outra e o grau de controle sobre a criacao:

- primeiro, nomear melhor caminhos de criacao;
- depois, separar a criacao em metodos ou classes especializadas;
- depois, lidar com criacao assincrona, rastreamento e composicao interna;
- por fim, produzir familias inteiras de objetos com `Abstract Factory`.

---

## 20. Point Example

[Voltar ao Sumario](#sumario)

Esta parte corresponde ao arquivo `3.Factories/Aula01_PointExample.cs`.

### 20.1 O problema pedagogico tipico deste exemplo

O `Point Example` costuma existir para mostrar um problema muito didatico:

- o mesmo tipo pode ter mais de um caminho valido de criacao;
- mas esses caminhos nem sempre ficam claros se tudo for empurrado para o construtor.

O caso classico costuma ser um ponto 2D criado:

- por coordenadas cartesianas;
- ou por coordenadas polares.

### 20.2 Por que isso costuma ficar ruim no construtor

Se o tipo tenta resolver tudo com construtores parecidos, a intencao da chamada pode ficar ambigua.

Exemplo conceitual:

```csharp
new Point(3, 4)
```

Essa chamada quer dizer:

- `x = 3` e `y = 4`?
- ou `rho = 3` e `theta = 4`?

Quando o mesmo formato de parametros representa significados diferentes, o construtor sozinho comeca a perder clareza.

### 20.3 O que a aula costuma ensinar aqui

O ganho de uma factory nesse ponto e dar nome ao caminho de criacao.

Exemplo conceitual:

```csharp
Point.NewCartesian(3, 4)
Point.NewPolar(3, 4)
```

Agora a chamada explica a intencao por nome, e nao apenas por posicao de parametro.

### 20.4 Leitura pratica do arquivo local

No estado atual do repositorio, o arquivo `Aula01_PointExample.cs` ja existe, mas pode ainda nao estar preenchido.

Mesmo assim, a aula ja deixa preparado o mapa mental correto:

- existe um tipo central `Point`;
- existem multiplos modos validos de cria-lo;
- factory entra para tornar esses caminhos explicitos e menos ambiguos.

---

## 21. Factory Method

[Voltar ao Sumario](#sumario)

### 21.1 O que ele enfatiza

`Factory Method` enfatiza a ideia de encapsular a criacao em um metodo com nome semantico melhor do que o construtor.

Esse metodo pode ser:

- estatico;
- interno ao proprio tipo;
- ou herdado em uma hierarquia mais classica de GoF.

### 21.2 O ganho mais imediato

O ganho mais facil de sentir e este:

- o construtor costuma dizer apenas "crie";
- o factory method pode dizer "crie deste jeito".

Exemplo conceitual:

```csharp
Point.NewCartesianPoint(x, y)
Point.NewPolarPoint(rho, theta)
```

O codigo cliente deixa de depender da memoria do desenvolvedor sobre o significado posicional dos parametros.

### 21.3 O que ele nao faz sozinho

Factory Method melhora intencao e encapsulamento da criacao, mas nao resolve tudo automaticamente.

Ele nao garante por si so:

- familias inteiras de objetos relacionados;
- politica global de rastreamento;
- isolamento total entre cliente e implementacao concreta.

Para essas dores, a trilha avanca para outras variacoes de factory.

---

## 22. Asynchronous Factory Method

[Voltar ao Sumario](#sumario)

### 22.1 Qual problema ele resolve

Construtores em C# nao podem ser `async`.

Entao, quando um objeto precisa nascer depois de:

- IO;
- consulta remota;
- leitura de arquivo;
- handshake de rede;
- inicializacao assincrona pesada;

o construtor tradicional deixa de ser um bom ponto de entrada.

### 22.2 A ideia central

Nesses casos, a criacao pode sair do construtor e migrar para um metodo de fabrica assincrono.

Exemplo conceitual:

```csharp
var service = await MeuServico.CreateAsync(...);
```

Essa abordagem ajuda a evitar objetos "meio prontos".

### 22.3 A leitura correta

O objetivo nao e apenas "usar async por usar".

O objetivo e proteger uma invariavel importante:

**o cliente so recebe o objeto quando ele realmente terminou de nascer.**

---

## 23. Factory

[Voltar ao Sumario](#sumario)

### 23.1 O que muda aqui em relacao ao Factory Method

Aqui a criacao deixa de ficar apenas em metodos internos ao proprio tipo e pode migrar para uma classe separada.

Exemplo conceitual:

```csharp
var p = PointFactory.NewCartesianPoint(3, 4);
```

### 23.2 Quando isso faz sentido

Uma classe de fabrica separada pode ajudar quando:

- voce quer tirar logica de criacao de dentro da entidade principal;
- a criacao cresceu demais;
- existem dependencias auxiliares na criacao;
- voce quer centralizar politicas sem inflar o produto.

### 23.3 Trade-off

O custo e introduzir mais um tipo no design.

Entao a pergunta pratica vira:

**o ganho de clareza e controle compensa a classe extra?**

Se sim, a factory separada faz sentido. Se nao, um factory method simples pode bastar.

---

## 24. Object Tracking and Bulk Replacement

[Voltar ao Sumario](#sumario)

### 24.1 Por que esta aula existe

Esta parte mostra um ganho menos obvio de centralizar a criacao.

Quando toda criacao passa por uma factory, esse ponto central pode fazer mais do que apenas instanciar.

Ele tambem pode:

- contar objetos criados;
- registrar referencias;
- aplicar configuracoes padrao;
- trocar em massa a estrategia de criacao;
- substituir tipos concretos sem reescrever cada ponto do sistema.

### 24.2 A intuicao principal

Se o sistema inteiro faz `new` diretamente, cada chamada vira um ponto isolado e dificil de coordenar.

Se toda criacao passa por uma factory, aparece um "gargalo controlado".

Esse gargalo permite:

- observar o que nasceu;
- modificar politica de nascimento;
- migrar comportamentos sem procurar `new` espalhado pela aplicacao.

### 24.3 O que aprender com essa aula

Factory nao serve apenas para "ficar bonito".

Ela tambem pode ser uma ferramenta de governanca da criacao.

Esse ponto prepara terreno para discussoes maiores sobre:

- rastreamento;
- substituicao de implementacoes;
- crescimento do sistema sem edicao repetida do cliente.

---

## 25. Inner Factory

[Voltar ao Sumario](#sumario)

### 25.1 O que e

`Inner Factory` e uma fabrica aninhada dentro do proprio tipo, normalmente como classe interna.

Exemplo conceitual:

```csharp
var p = Point.Factory.NewCartesianPoint(3, 4);
```

### 25.2 O que ela tenta equilibrar

Ela fica no meio do caminho entre:

- deixar factory methods soltos diretamente no tipo;
- ou criar uma fabrica totalmente externa.

O ganho pode ser este:

- a fabrica continua claramente associada ao produto;
- mas a logica de criacao nao precisa poluir tanto a superficie principal do tipo.

### 25.3 Quando faz sentido

Ela pode ser util quando:

- a criacao e relevante demais para ficar perdida fora do tipo;
- mas a API principal ficaria inchada se tudo virasse metodo estatico no produto.

---

## 26. Abstract Factory

[Voltar ao Sumario](#sumario)

### 26.1 O que ela enfatiza

`Abstract Factory` nao cria apenas um objeto isolado.

Ela cria **familias de objetos relacionados**.

Essa e a virada conceitual mais importante desta parte do capitulo.

### 26.2 Exemplo mental

Imagine uma aplicacao que precisa trabalhar com familias consistentes, como:

- `WindowsButton` + `WindowsCheckbox`;
- `MacButton` + `MacCheckbox`.

O cliente nao quer escolher cada classe concreta manualmente. Ele quer pedir uma familia coerente.

### 26.3 Estrutura mental

Em geral aparece algo assim:

- uma interface de fabrica abstrata;
- varias fabricas concretas;
- varios produtos abstratos;
- varias implementacoes concretas desses produtos.

Leitura correta:

- o cliente depende de abstracoes;
- a fabrica concreta decide qual familia inteira sera entregue.

### 26.4 O ganho principal

O grande ganho aqui nao e apenas "tirar um `new`".

O ganho e garantir compatibilidade entre objetos que devem nascer juntos como um conjunto coerente.

---

## 27. Abstract Factory and OCP

[Voltar ao Sumario](#sumario)

### 27.1 Onde OCP entra

`Abstract Factory` conversa muito bem com `Open-Closed Principle`.

A ideia central e:

- o cliente depende da abstracao da fabrica;
- novas familias podem entrar como novas implementacoes concretas;
- o cliente tende a crescer por extensao, nao por reabertura constante.

### 27.2 O ganho real

Quando uma nova familia aparece, a tendencia e adicionar:

- uma nova fabrica concreta;
- novos produtos concretos daquela familia.

Se o cliente ja conversa com as abstracoes corretas, ele costuma exigir pouca ou nenhuma mudanca estrutural.

### 27.3 A nuance importante

Vale uma honestidade tecnica aqui:

`Abstract Factory` ajuda muito com a adicao de **novas familias**.

Mas, se voce adicionar um **novo tipo de produto** dentro da familia, talvez precise tocar em varias fabricas existentes.

Entao o alinhamento com OCP e forte, mas tem direcao especifica:

- cresce muito bem por novas familias;
- nao elimina todo custo de evolucao em qualquer eixo.

---

## Coding Exercise 2: Factory Coding Exercise

[Voltar ao Sumario](#sumario)

No estado atual do repositorio, ainda nao ha um arquivo especifico para o exercicio pratico de `Factory`.

Mesmo assim, vale deixar claro o objetivo pedagogico desta etapa:

- sair da leitura conceitual;
- implementar uma solucao com controle de criacao;
- validar se a diferenca entre `new` direto e criacao mediada por fabrica ficou clara.

Em geral, um bom exercicio de `Factory` testa perguntas como:

- o cliente ainda conhece demais da classe concreta?
- a criacao ficou nomeada de forma clara?
- a fabrica esta escondendo detalhes relevantes de instanciacao?
- a API de criacao ficou mais legivel do que um construtor ambiguo?

Quando o arquivo do exercicio for adicionado, esta secao pode ser expandida com:

- enunciado interpretado;
- participantes do padrao;
- armadilhas comuns;
- leitura guiada da solucao.

---

## 28. Summary

[Voltar ao Sumario](#sumario)

### 28.1 Resumo da trilha

O mapa deste capitulo fica assim:

| Aula | Pergunta principal |
| --- | --- |
| 19. Overview | Que problema a familia Factory tenta resolver? |
| 20. Point Example | Como a ambiguidade de construcao aparece em um exemplo simples? |
| 21. Factory Method | Como dar nome semantico aos caminhos de criacao? |
| 22. Asynchronous Factory Method | Como criar objetos quando a inicializacao precisa ser assincrona? |
| 23. Factory | Quando separar a criacao em uma fabrica dedicada? |
| 24. Object Tracking and Bulk Replacement | O que ganhamos ao centralizar toda criacao? |
| 25. Inner Factory | Como manter a fabrica proxima do produto sem inflar tanto sua API? |
| 26. Abstract Factory | Como criar familias inteiras de objetos relacionados? |
| 27. Abstract Factory and OCP | Como novas familias podem entrar por extensao? |
| Coding Exercise 2 | Como aplicar a ideia em um problema pratico de criacao? |
| 28. Summary | Como consolidar o mapa mental de Factories? |

### 28.2 Ideia central que atravessa todas as variacoes

Todas as variacoes orbitam a mesma intencao base:

**tirar do cliente o peso de decidir sozinho todos os detalhes de criacao.**

O que muda de uma aula para outra e:

- quanta logica de criacao esta sendo encapsulada;
- se a fabrica vive no proprio tipo ou fora dele;
- se a criacao e sincrona ou assincrona;
- se a fabrica cria um objeto isolado ou uma familia inteira.

### 28.3 Regra mental final

Uma forma curta de guardar o capitulo e esta:

- **Point Example:** mostra por que o construtor pode ficar ambiguo.
- **Factory Method:** nomeia caminhos de criacao.
- **Factory:** separa a criacao em uma peca dedicada.
- **Asynchronous Factory Method:** adia a entrega do objeto ate ele realmente nascer.
- **Inner Factory:** mantem a fabrica perto do produto.
- **Abstract Factory:** cria familias de objetos relacionados.

---

## Referencias bibliograficas

[Voltar ao Sumario](#sumario)

Referencia principal:

- **Gamma, Erich; Helm, Richard; Johnson, Ralph; Vlissides, John.** *Design Patterns: Elements of Reusable Object-Oriented Software*. Addison-Wesley, 1994.

Arquivos desta pasta usados como base pratica:

- `3.Factories/Aula01_PointExample.cs`

Observacoes importantes:

- `Factory Pattern` aqui significa um conjunto de tecnicas de criacao, e nao uma unica implementacao rigida;
- `Factory` e `Builder` pertencem a familia criacional, mas respondem a perguntas diferentes;
- quando os proximos arquivos da pasta forem adicionados, este guia pode ser ligado diretamente a cada aula concreta.
