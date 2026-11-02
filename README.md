# Design Patterns, SOLID e Engenharia de Software

Repositório de estudo com explicações didáticas, exemplos de código e exercícios sobre design de software. O conteúdo combina os cinco princípios SOLID, os 23 padrões GoF e o padrão Null Object, além de materiais complementares de programação e infraestrutura.

O objetivo é ajudar o estudante a reconhecer problemas de design, entender as decisões por trás de cada padrão e acompanhar sua aplicação em código. Os padrões são apresentados como ferramentas para situações específicas; não como estruturas que precisam ser aplicadas em todo projeto.

## Conteúdo do repositório

### Princípios SOLID

O capítulo de [SOLID](1.SOLID/SOLID.md) explica cada princípio com exemplos C# de versões problemáticas e refatoradas:

- **SRP — Single Responsibility Principle:** organize responsabilidades para que uma unidade de código tenha motivos coesos para mudar.
- **OCP — Open-Closed Principle:** permita estender comportamentos sem alterar continuamente código já estável.
- **LSP — Liskov Substitution Principle:** preserve os contratos para que implementações possam substituir suas abstrações.
- **ISP — Interface Segregation Principle:** prefira contratos específicos a interfaces que obrigam clientes a depender de operações que não usam.
- **DIP — Dependency Inversion Principle:** faça políticas de alto nível dependerem de abstrações, não de detalhes concretos.

O material também aborda conceitos de C# necessários para ler os exemplos, relações entre os princípios, sintomas de violações e armadilhas comuns ao aplicar SOLID.

### Padrões de criação

Estudam diferentes formas de criar objetos e famílias de objetos:

- [Abstract Factory](2.GammaCategorization/1.CreationalPatterns/3.Factories/padrao_abstract_factory.md) — cria famílias de produtos relacionados.
- [Builder](2.GammaCategorization/1.CreationalPatterns/2.Builder/padrao_builder.md) — monta objetos complexos em etapas.
- [Factory Method](2.GammaCategorization/1.CreationalPatterns/3.Factories/padrao_factory_method.md) — delega a subclasses ou implementações a escolha do produto concreto.
- [Prototype](2.GammaCategorization/1.CreationalPatterns/4.Prototype/padrao_prototype.md) — cria objetos a partir da cópia de protótipos.
- [Singleton](2.GammaCategorization/1.CreationalPatterns/5.Singleton/padrao_singleton.md) — controla a criação e o acesso a uma instância compartilhada.

### Padrões estruturais

Tratam da composição de objetos e da organização das relações entre eles:

- [Adapter](2.GammaCategorization/2.StructuralPatterns/6.Adapter/Adapter.md) — adapta uma interface existente à interface esperada pelo cliente.
- [Bridge](2.GammaCategorization/2.StructuralPatterns/7.Bridge/Bridge.md) — separa duas dimensões que precisam variar independentemente.
- [Composite](2.GammaCategorization/2.StructuralPatterns/8.Composite/Composite.md) — permite tratar objetos individuais e grupos por um contrato comum.
- [Decorator](2.GammaCategorization/2.StructuralPatterns/9.Decorator/Decorator.md) — acrescenta responsabilidades envolvendo objetos.
- [Facade](2.GammaCategorization/2.StructuralPatterns/10.Facade/Facade.md) — oferece uma entrada simplificada para um subsistema.
- [Flyweight](2.GammaCategorization/2.StructuralPatterns/11.Flyweight/Flyweight.md) — reduz duplicação ao compartilhar estado repetido.
- [Proxy](2.GammaCategorization/2.StructuralPatterns/12.Proxy/Proxy.md) — controla o acesso a outro objeto por meio de um substituto.

### Padrões comportamentais

Focam na colaboração entre objetos, na distribuição de responsabilidades e na variação de comportamentos:

- [Chain of Responsibility](2.GammaCategorization/3.BehavioralPatterns/chain%20of%20responsability/chain-of-responsibility.md) — encaminha uma solicitação por uma cadeia de possíveis responsáveis.
- [Command](2.GammaCategorization/3.BehavioralPatterns/command/command.md) — representa uma ação como objeto, o que permite organizá-la, enfileirá-la ou desfazê-la.
- [Interpreter](2.GammaCategorization/3.BehavioralPatterns/interpreter/interpreter.md) — representa e avalia expressões de uma linguagem pequena.
- [Iterator](2.GammaCategorization/3.BehavioralPatterns/iterator/iterator-pattern.md) — percorre uma coleção sem expor sua estrutura interna.
- [Mediator](2.GammaCategorization/3.BehavioralPatterns/mediator/mediator-pattern.md) — centraliza a coordenação entre participantes.
- [Memento](2.GammaCategorization/3.BehavioralPatterns/memento/memento.md) — registra e restaura estados anteriores sem expor sua representação interna.
- [Observer](2.GammaCategorization/3.BehavioralPatterns/observer/observer-pattern.md) — notifica participantes inscritos quando um estado muda.
- [State](2.GammaCategorization/3.BehavioralPatterns/state/state.md) — varia o comportamento do contexto conforme seu estado atual.
- [Strategy](2.GammaCategorization/3.BehavioralPatterns/strategy/strategy-pattern.md) — permite trocar algoritmos ou regras por implementações intercambiáveis.
- [Template Method](2.GammaCategorization/3.BehavioralPatterns/template%20method/template-method.md) — fixa o esqueleto de um algoritmo e permite personalizar algumas etapas.
- [Visitor](2.GammaCategorization/3.BehavioralPatterns/visitor/visitor.md) — adiciona operações a uma estrutura de tipos sem concentrar todas elas nesses tipos.

### Padrão complementar

- [Null Object](2.GammaCategorization/3.BehavioralPatterns/null%20object/null-object.md) — fornece um comportamento neutro explícito para uma dependência opcional. É estudado junto aos padrões comportamentais, embora não faça parte dos 23 padrões GoF.

## Como estudar

Uma sequência sugerida é:

1. Leia o capítulo de SOLID para conhecer princípios de responsabilidade, extensão, substituição e dependências.
2. Escolha uma família de padrões — criação, estrutura ou comportamento — e leia a página individual do padrão.
3. Percorra o exemplo de código indicado na página e identifique o problema que ele resolve, os papéis dos objetos e o fluxo de chamadas.
4. Compare as variações e padrões próximos. Observe também os custos e as situações em que o padrão não ajuda.
5. Faça as atividades e, nos exercícios com anti-pattern, tente diagnosticar o problema antes de consultar a explicação.

As páginas individuais foram organizadas para serem consultadas sem depender de um guia central. Cada uma inclui sumário, links para voltar ao início das seções e glossário. As explicações apresentam a intenção do padrão, sua aplicação prática, os exemplos relacionados no repositório e, quando pertinente, variações e critérios de escolha.

## Exemplos e linguagens

O repositório reúne exemplos em **C#** e **Java**. A linguagem depende da aula ou do exercício: muitos exemplos de criação e estrutura são C#, enquanto vários projetos comportamentais corrigidos e exercícios são Java. Algumas páginas também incluem exemplos pequenos para explicar uma ideia; consulte os caminhos citados na própria página para distinguir esses exemplos dos arquivos executáveis.

Os projetos são independentes e não existe uma solução única na raiz. Muitos exemplos são arquivos de aula sem configuração de build própria. Quatro projetos de anti-pattern comportamentais possuem configuração Maven e usam Java 17:

- Command: macro de editor representada por strings e tratada com switch.
- Mediator: usuários de uma sala de chat ligados diretamente uns aos outros.
- Memento: estado do jogo exposto e copiado manualmente pelo gerenciador de saves.
- Observer: estação meteorológica acoplada diretamente a cada display.

Esses projetos funcionam como exercícios de diagnóstico. As versões corrigidas correspondentes ficam em pastas separadas e são explicadas nas páginas dos padrões.

### Executar um projeto Maven de anti-pattern

Instale Java 17 e Maven. Em seguida, entre na pasta do projeto que deseja executar. Por exemplo, para o exercício de Command:

~~~sh
cd "2.GammaCategorization/3.BehavioralPatterns/command/projeto_command_antipattern/command-antipattern"
mvn compile
java -cp target/classes br.venson.net.designpatterns.command.Main
~~~

Os outros três projetos seguem o mesmo fluxo, mas cada um tem seu próprio README com a classe principal e o comando Java correspondentes. Também é possível importar a pasta do projeto diretamente em uma IDE Java.

### Executar os exemplos C# e os demais exemplos Java

Os exemplos C# e vários exemplos Java estão organizados como arquivos de aula ou exercícios separados, sem um arquivo de solução ou build global na raiz. Abra o diretório indicado pela página do padrão em uma IDE compatível ou compile os arquivos daquela aula conforme a configuração local do exercício. Não assuma que todos os diretórios possam ser compilados juntos: são exemplos independentes e alguns reutilizam nomes de classes em contextos diferentes.

## Organização das pastas

~~~text
.
├── 1.SOLID/
│   ├── SOLID.md
│   └── exemplos C# certos e errados por princípio
├── 2.GammaCategorization/
│   ├── 1.CreationalPatterns/
│   ├── 2.StructuralPatterns/
│   └── 3.BehavioralPatterns/
├── C#_Fundamentals.md
├── C++_Fundamentals.md
├── GDSCRIPT.md
├── Komodo_Fundamentals.md
├── NGINX_Proxy_Fundamentals.md
├── Ubuntu_Server_Fundamentals.md
└── ventoy-e-bootable-usb.md
~~~

As pastas de padrões agrupam páginas explicativas, exemplos de código, exercícios e, em alguns casos, versões corrigidas e versões com problemas de design. Os outros arquivos Markdown da raiz são anotações complementares sobre linguagens, ferramentas e infraestrutura; eles não fazem parte do currículo principal de SOLID e GoF.

## Requisitos

- Um leitor de Markdown para estudar as explicações.
- **Java 17 e Maven** para executar os quatro projetos Maven de anti-pattern.
- Uma IDE ou SDK compatível com a linguagem do exemplo para compilar os demais arquivos C# ou Java.

Não há comando global de instalação, build ou testes para o repositório inteiro.

## Contribuições e licença

Este repositório tem finalidade didática. Ao acrescentar um exemplo, mantenha-o pequeno o bastante para ensinar uma ideia, indique a linguagem e os arquivos envolvidos e explique o problema que o padrão resolve e seus custos. Não há arquivo de licença na raiz; consulte o responsável pelo repositório antes de reutilizar o conteúdo fora dos termos permitidos pelo seu contexto.
