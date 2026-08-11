# 🎮 Guia Técnico: GDScript do Zero ao Avançado

> **Nível:** Zero ao Avançado  
> **Linguagem:** GDScript  
> **Fonte de referência principal:** [Godot Docs — GDScript](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/)  
> **Versão de referência:** Godot Docs stable, branch 4.7  
> **Atualizado em:** 11/08/2026

---

## Nota de Escopo

[⬆️ Voltar ao Sumário](#sumario)

Este guia trata de **GDScript como linguagem de programação dentro do Godot Engine**. O foco é a sintaxe, o modelo de valores, o sistema gradual de tipos, classes, funções, sinais, propriedades, anotações, exports, integração com `Object`, `Node`, `Variant`, `Callable`, `Signal` e o modo como scripts participam do editor e da árvore de cena.

Não é um guia completo sobre:

- criação de jogos em Godot;
- física, renderização, animação, áudio ou rede em profundidade;
- C#, GDExtension, shader language ou VisualScript;
- design de levels, arte, pipelines de assets ou publicação em lojas.

Esses assuntos aparecem apenas quando ajudam a entender a linguagem. A fonte usada aqui é exclusivamente a documentação oficial do Godot em `docs.godotengine.org/en/stable/`.

---

## Prefácio

[⬆️ Voltar ao Sumário](#sumario)

GDScript costuma ser apresentado como uma linguagem "parecida com Python". Essa frase ajuda no primeiro contato, porque a indentação e a legibilidade lembram linguagens de script modernas. Mas ela também pode atrapalhar. GDScript não é Python, não é baseado em Python e não deve ser estudado como se fosse apenas uma variação menor dele.

GDScript é uma linguagem criada para o Godot. Seu desenho existe para conversar diretamente com cenas, nós, recursos, propriedades exportadas, sinais, referências a objetos, editor, tipos internos da engine e APIs globais. Um arquivo `.gd` não é apenas um arquivo de funções: ele é uma classe. Uma variável exportada não é apenas sintaxe: ela altera o Inspector e a serialização. Um sinal não é apenas callback: ele participa da arquitetura de comunicação entre objetos. Um `await` não é apenas pausa: ele devolve controle ao chamador e retoma execução quando um sinal ou uma corrotina termina.

O objetivo deste guia é construir um modelo mental completo. Primeiro, entender o que a linguagem é. Depois, entender como seus valores são representados. Em seguida, aprender a escrever fluxo, funções, classes e objetos. Por fim, conectar tudo isso ao editor, às anotações, à tipagem estática opcional, ao sistema de avisos e aos padrões de escrita recomendados pela documentação oficial.

Este material não substitui a documentação oficial do Godot. Ele a reorganiza em formato de livro técnico, com exemplos didáticos, leitura guiada e critérios práticos de uso.

---

## Como usar este guia

[⬆️ Voltar ao Sumário](#sumario)

Há três trilhas possíveis:

1. **Trilha iniciante:** leia as Partes 1 a 12, execute exemplos pequenos em scripts anexados a nós simples e priorize sintaxe, tipos, variáveis, funções e controle de fluxo.
2. **Trilha profissional:** avance pelas Partes 13 a 24, focando classes, sinais, exports, tipagem estática, propriedades, memória, editor e avisos.
3. **Trilha de consulta:** use as Partes 25 a 30, catálogos e anexos para lembrar palavras-chave, operadores, anotações, tipos e fontes oficiais.

Ao estudar qualquer recurso de GDScript, responda sempre:

1. Isto pertence à linguagem, ao editor, à classe `Object`, à classe `Node`, ao sistema `Variant` ou a uma API global?
2. O valor é copiado ou compartilhado por referência?
3. O tipo está declarado, inferido ou completamente dinâmico?
4. Esta variável precisa aparecer no Inspector ou deve ser apenas estado interno?
5. Esta comunicação deve ser chamada direta, `Callable`, sinal ou grupo de nós?
6. O código depende da ordem de inicialização de variáveis, `_init()`, exports, `@onready` e `_ready()`?

> **Regra de laboratório:** escreva exemplos pequenos e rode-os dentro do Godot. GDScript é profundamente integrado à engine; muito do comportamento real só aparece quando o script vive em uma cena, em um recurso ou em um nó.

---

<a id="sumario"></a>

## Sumário Geral

### Como o conteúdo está organizado

| Bloco | Partes | Assuntos centrais | Resultado esperado | Comece por |
|---|---:|---|---|---|
| 1. Base da linguagem | 1-4 | propósito, arquivos, classes, sintaxe, identificadores e palavras-chave | entender o que um script GDScript representa | [Parte 1](#parte-1--introdução-e-contextualização) |
| 2. Valores e tipos | 5-9 | literais, strings, `Variant`, tipos embutidos, variáveis, constantes, enums e operadores | manipular dados sem confundir cópia, referência e tipagem | [Parte 5](#parte-5--literais-texto-e-formatação) |
| 3. Controle e funções | 10-14 | `if`, `for`, `while`, `match`, funções, lambdas, `Callable`, sinais e `await` | escrever comportamento legível e assíncrono | [Parte 10](#parte-10--controle-de-fluxo) |
| 4. Classes e objetos | 15-18 | `class_name`, `extends`, herança, `_init`, propriedades, memória e sinais | modelar scripts como tipos integrados ao Godot | [Parte 15](#parte-15--classes-herança-e-construtores) |
| 5. Editor e tipagem | 19-24 | exports, anotações, tipagem estática, comentários de documentação, estilo e avisos | escrever scripts profissionais, navegáveis e seguros | [Parte 19](#parte-19--exports-e-inspector) |
| 6. Integração prática | 25-28 | recursos, `load`, `preload`, APIs globais, callbacks de nós e padrões de organização | conectar linguagem, engine e editor com clareza | [Parte 25](#parte-25--load-preload-e-scripts-como-recursos) |
| 7. Catálogos | 29-30 | palavras-chave, operadores, anotações, funções e armadilhas | consultar rapidamente a superfície da linguagem | [Parte 29](#parte-29--catálogo-da-linguagem) |
| 8. Revisão | Anexos | referências oficiais, trilhas e glossário | aprofundar nas fontes primárias | [Anexo A](#anexo-a--referências-oficiais-consultadas) |

### Atalhos por pergunta prática

| Se você quer saber... | Consulte primeiro |
|---|---|
| o que é GDScript e como ele se diferencia de Python | [Parte 1](#parte-1--introdução-e-contextualização) |
| como um arquivo `.gd` vira classe | [Parte 2](#parte-2--arquivos-scripts-e-classes) |
| como organizar um script legível | [Partes 3](#parte-3--sintaxe-básica-e-organização) e [23](#parte-23--guia-de-estilo-oficial) |
| quais tipos existem | [Partes 6](#parte-6--variant-e-modelo-de-tipos) e [7](#parte-7--tipos-embutidos) |
| como declarar variáveis, constantes e enums | [Parte 8](#parte-8--variáveis-constantes-enums-e-inicialização) |
| como usar `if`, `for`, `while` e `match` | [Partes 10](#parte-10--controle-de-fluxo) e [11](#parte-11--match-e-pattern-matching) |
| como escrever funções tipadas | [Parte 12](#parte-12--funções) |
| como passar funções como valor | [Parte 13](#parte-13--callable-e-lambdas) |
| como esperar sinal ou corrotina | [Parte 14](#parte-14--await-sinais-e-corrotinas) |
| como criar tipos globais com `class_name` | [Parte 15](#parte-15--classes-herança-e-construtores) |
| como criar propriedades com `get` e `set` | [Parte 16](#parte-16--propriedades-getters-setters-e-membros-estáticos) |
| quando usar `free()`, `queue_free()` e `RefCounted` | [Parte 17](#parte-17--modelo-de-objetos-e-memória) |
| como desacoplar objetos com sinais | [Parte 18](#parte-18--sinais-como-contratos-de-comunicação) |
| como expor valores no Inspector | [Parte 19](#parte-19--exports-e-inspector) |
| quais anotações existem | [Parte 20](#parte-20--anotações) |
| como usar tipagem estática sem perder flexibilidade | [Parte 21](#parte-21--tipagem-estática-opcional) |
| como documentar scripts para o editor | [Parte 22](#parte-22--comentários-de-documentação) |
| como lidar com warnings | [Parte 24](#parte-24--sistema-de-avisos-diagnóstico-e-debug) |
| quando usar `load()` ou `preload()` | [Parte 25](#parte-25--load-preload-e-scripts-como-recursos) |
| o que está em `@GDScript` e `@GlobalScope` | [Parte 26](#parte-26--funções-globais-e-escopo-global) |

### Índice detalhado

**Bloco 1 — Base da linguagem**

- **[Parte 1 — Introdução e Contextualização](#parte-1--introdução-e-contextualização)**
- **[Parte 2 — Arquivos, Scripts e Classes](#parte-2--arquivos-scripts-e-classes)**
- **[Parte 3 — Sintaxe Básica e Organização](#parte-3--sintaxe-básica-e-organização)**
- **[Parte 4 — Identificadores, Palavras-chave e Escopo](#parte-4--identificadores-palavras-chave-e-escopo)**

**Bloco 2 — Valores e tipos**

- **[Parte 5 — Literais, Texto e Formatação](#parte-5--literais-texto-e-formatação)**
- **[Parte 6 — Variant e Modelo de Tipos](#parte-6--variant-e-modelo-de-tipos)**
- **[Parte 7 — Tipos Embutidos](#parte-7--tipos-embutidos)**
- **[Parte 8 — Variáveis, Constantes, Enums e Inicialização](#parte-8--variáveis-constantes-enums-e-inicialização)**
- **[Parte 9 — Operadores e Expressões](#parte-9--operadores-e-expressões)**

**Bloco 3 — Controle e funções**

- **[Parte 10 — Controle de Fluxo](#parte-10--controle-de-fluxo)**
- **[Parte 11 — match e Pattern Matching](#parte-11--match-e-pattern-matching)**
- **[Parte 12 — Funções](#parte-12--funções)**
- **[Parte 13 — Callable e Lambdas](#parte-13--callable-e-lambdas)**
- **[Parte 14 — await, Sinais e Corrotinas](#parte-14--await-sinais-e-corrotinas)**

**Bloco 4 — Classes e objetos**

- **[Parte 15 — Classes, Herança e Construtores](#parte-15--classes-herança-e-construtores)**
- **[Parte 16 — Propriedades, Getters, Setters e Membros Estáticos](#parte-16--propriedades-getters-setters-e-membros-estáticos)**
- **[Parte 17 — Modelo de Objetos e Memória](#parte-17--modelo-de-objetos-e-memória)**
- **[Parte 18 — Sinais como Contratos de Comunicação](#parte-18--sinais-como-contratos-de-comunicação)**

**Bloco 5 — Editor e tipagem**

- **[Parte 19 — Exports e Inspector](#parte-19--exports-e-inspector)**
- **[Parte 20 — Anotações](#parte-20--anotações)**
- **[Parte 21 — Tipagem Estática Opcional](#parte-21--tipagem-estática-opcional)**
- **[Parte 22 — Comentários de Documentação](#parte-22--comentários-de-documentação)**
- **[Parte 23 — Guia de Estilo Oficial](#parte-23--guia-de-estilo-oficial)**
- **[Parte 24 — Sistema de Avisos, Diagnóstico e Debug](#parte-24--sistema-de-avisos-diagnóstico-e-debug)**

**Bloco 6 — Integração prática**

- **[Parte 25 — load, preload e Scripts como Recursos](#parte-25--load-preload-e-scripts-como-recursos)**
- **[Parte 26 — Funções Globais e Escopo Global](#parte-26--funções-globais-e-escopo-global)**
- **[Parte 27 — Callbacks de Node e Ciclo de Cena](#parte-27--callbacks-de-node-e-ciclo-de-cena)**
- **[Parte 28 — Organização Profissional de Scripts](#parte-28--organização-profissional-de-scripts)**

**Bloco 7 — Catálogos e revisão**

- **[Parte 29 — Catálogo da Linguagem](#parte-29--catálogo-da-linguagem)**
- **[Parte 30 — Armadilhas Frequentes](#parte-30--armadilhas-frequentes)**
- **[Anexo A — Referências Oficiais Consultadas](#anexo-a--referências-oficiais-consultadas)**
- **[Anexo B — Glossário](#anexo-b--glossário)**

---

## Parte 1 — Introdução e Contextualização

[⬆️ Voltar ao Sumário](#sumario)

### 1.1 O que é GDScript?

GDScript é a linguagem de script integrada ao Godot Engine. A documentação oficial a descreve como uma linguagem de alto nível, orientada a objetos, imperativa e gradualmente tipada, construída para Godot. Ela usa sintaxe baseada em indentação e foi desenhada para ser otimizada e fortemente integrada à engine.

Essa definição contém quatro ideias importantes:

- **alto nível:** o programador manipula objetos, nós, recursos, sinais e coleções, não detalhes de baixo nível da engine;
- **orientada a objetos:** scripts são classes e podem herdar de classes da engine, classes globais, arquivos `.gd` e classes internas;
- **imperativa:** o código expressa passos de execução com atribuições, chamadas, laços e decisões;
- **gradualmente tipada:** o mesmo projeto pode ter código dinâmico e código com tipos estáticos declarados.

GDScript é independente de Python. A semelhança visual existe, mas a semântica real pertence ao Godot: `Variant`, `Object`, `Node`, `Signal`, `Callable`, exports, recursos e o editor são parte central do cotidiano da linguagem.

### 1.2 Por que GDScript existe?

GDScript existe para tornar a criação de jogos e ferramentas no Godot rápida, integrada e produtiva. A linguagem conhece os tipos da engine, conversa com o Inspector, permite exportar propriedades, conectar sinais, carregar recursos e escrever callbacks que o ciclo de vida de `Node` chama automaticamente.

Em uma aplicação comum, uma linguagem generalista precisa adaptar-se ao framework. Em Godot, GDScript nasce dentro da engine. Por isso, decisões que parecem "especiais" fazem sentido no contexto:

| Recurso | O que resolve |
|---|---|
| `extends Node` | liga um script ao comportamento de um nó |
| `@export` | expõe propriedades no Inspector e salva valores em cena/recurso |
| `signal` | cria comunicação desacoplada entre objetos |
| `await` | espera sinais e corrotinas sem bloquear a engine inteira |
| `class_name` | registra um script como tipo global no editor |
| `@tool` | permite executar código no editor |

### 1.3 Linguagem, engine e editor não são a mesma camada

Uma boa leitura de GDScript separa três camadas:

| Camada | Exemplo | Pergunta correta |
|---|---|---|
| Linguagem | `var`, `func`, `if`, `match`, `class`, `await` | qual é a sintaxe e a semântica? |
| Engine | `Object`, `Node`, `Resource`, `SceneTree`, `Callable`, `Signal` | qual contrato da API está sendo usado? |
| Editor | `@export`, `@tool`, `@icon`, documentação `##` | como isso afeta Inspector, autocomplete e execução no editor? |

Confundir essas camadas gera más decisões. Por exemplo, `@export` parece apenas uma anotação de variável, mas seu efeito real envolve editor, serialização e arquivos de cena ou recurso. `queue_free()` parece apenas uma chamada de método, mas em `Node` afeta também a árvore de filhos.

### 1.4 Versão de referência

Este guia usa a documentação oficial estável do Godot, branch 4.7. A própria documentação oficial indica que a página `stable` corresponde à branch 4.7 no momento desta revisão. Recursos citados como `Dictionary[KeyType, ValueType]`, `@abstract`, `@export_tool_button` e outras anotações seguem esse contexto.

---

## Parte 2 — Arquivos, Scripts e Classes

[⬆️ Voltar ao Sumário](#sumario)

### 2.1 Um arquivo `.gd` é uma classe

Em GDScript, um arquivo de script representa uma classe. Se você cria `player.gd`, esse arquivo contém a definição de um tipo. Por padrão, essa classe é anônima: ela pode ser referenciada por caminho, carregada como recurso ou anexada a um nó.

```gdscript
# player.gd
extends CharacterBody2D

var speed: float = 300.0

func _physics_process(delta: float) -> void:
    velocity.x = speed
    move_and_slide()
```

Esse arquivo não está "solto". Ele define uma classe que herda de `CharacterBody2D`. Quando anexado a um nó, o nó passa a ter as variáveis, funções e sinais declarados no script.

### 2.2 `extends`

`extends` define a classe base do script. Um script pode herdar de uma classe global da engine, de outro arquivo `.gd` ou de uma classe interna.

```gdscript
extends Node
```

```gdscript
extends "res://actors/enemy_base.gd"
```

```gdscript
extends "res://state_machine/state.gd".InnerState
```

Se nenhuma herança for declarada, a classe herda de `RefCounted`.

### 2.3 `class_name`

`class_name` registra o script como uma classe global do projeto. Isso permite usá-lo como tipo, escolhê-lo no editor e instanciá-lo sem carregar o arquivo manualmente.

```gdscript
class_name InventoryItem
extends Resource

@export var display_name: String
@export var max_stack: int = 1
```

Depois disso, outro script pode escrever:

```gdscript
var item: InventoryItem
```

Use `class_name` quando o tipo fizer parte da linguagem do seu projeto: estados, recursos de dados, componentes reutilizáveis, inimigos base, habilidades, itens, serviços de domínio do jogo.

### 2.4 `@icon`

`@icon` associa um ícone à classe registrada. O efeito principal é editorial: o tipo aparece com ícone próprio em diálogos do editor.

```gdscript
@icon("res://icons/inventory_item.svg")
class_name InventoryItem
extends Resource
```

### 2.5 Caminhos de recurso

Godot usa caminhos como `res://` para arquivos dentro do projeto. Um script pode carregar outro script como recurso:

```gdscript
const Enemy = preload("res://actors/enemy.gd")

func spawn() -> void:
    var enemy = Enemy.new()
```

Essa sintaxe é importante porque classes armazenadas em arquivos são recursos (`GDScript`) e precisam ser carregadas para serem usadas em outros scripts, salvo quando foram registradas com `class_name`.

---

## Parte 3 — Sintaxe Básica e Organização

[⬆️ Voltar ao Sumário](#sumario)

### 3.1 Blocos por indentação

GDScript usa indentação para delimitar blocos. Estruturas como `if`, `for`, `while`, `match`, `func` e `class` terminam sua linha de abertura com `:` e recebem um bloco indentado.

```gdscript
func take_damage(amount: int) -> void:
    health -= amount
    if health <= 0:
        defeated.emit()
```

A indentação não é enfeite visual; ela faz parte da estrutura do programa.

### 3.2 Comentários comuns

Tudo que aparece depois de `#` até o fim da linha é comentário.

```gdscript
# Isto explica a intenção do bloco seguinte.
health = max(health - amount, 0)
```

Comentários devem explicar intenção, invariantes ou decisões. Não devem repetir mecanicamente o que a linha já diz.

### 3.3 Comentários de documentação

Comentários iniciados por `##` documentam scripts, membros e variáveis exportadas. Eles são tratados de modo especial pelo editor e podem virar documentação XML.

```gdscript
## Recurso que descreve um item coletável.
##
## Use este recurso em inventários, lojas e recompensas.
class_name ItemData
extends Resource

## Nome mostrado ao jogador.
@export var display_name: String
```

### 3.4 Regiões de código

O editor do Godot entende regiões com `#region` e `#endregion`.

```gdscript
#region Inventory
func add_item(item: ItemData) -> void:
    pass

func remove_item(item: ItemData) -> void:
    pass
#endregion
```

Regiões ajudam a dobrar trechos grandes no editor, mas não devem ser usadas para esconder código confuso. Se a região vira muleta, talvez o script precise ser dividido.

### 3.5 Continuação de linha

Uma linha pode continuar na próxima com `\`, mas o guia de estilo oficial prefere parênteses para expressões longas, pois eles facilitam refatoração.

```gdscript
var direction = (
    "right" if velocity.x > 0.0
    else "left" if velocity.x < 0.0
    else "idle"
)
```

### 3.6 Uma instrução por linha

GDScript permite algumas formas compactas, como:

```gdscript
func square(x): return x * x
```

Mas o estilo oficial recomenda evitar combinar muitas instruções na mesma linha. A exceção prática mais aceitável é o operador ternário, quando curto e legível.

---

## Parte 4 — Identificadores, Palavras-chave e Escopo

[⬆️ Voltar ao Sumário](#sumario)

### 4.1 Identificadores

Identificadores nomeiam variáveis, funções, classes, sinais e constantes. Eles podem conter letras, dígitos e `_`, mas não podem começar por dígito. São sensíveis a maiúsculas e minúsculas.

```gdscript
var score = 10
var Score = 20 # outro identificador
```

A documentação oficial também permite muitos caracteres Unicode em identificadores, com restrições para caracteres confundíveis e emojis. Em projetos profissionais, porém, prefira nomes ASCII claros, especialmente quando há colaboração, busca por texto e integração com ferramentas externas.

### 4.2 Palavras-chave

Palavras-chave são tokens reservados. Não podem ser usadas como nomes de variáveis ou funções.

| Grupo | Palavras-chave |
|---|---|
| Fluxo | `if`, `elif`, `else`, `for`, `while`, `match`, `when`, `break`, `continue`, `pass`, `return` |
| Classes | `class`, `class_name`, `extends`, `is`, `as`, `self`, `super` |
| Membros | `signal`, `func`, `static`, `const`, `enum`, `var` |
| Execução | `await`, `assert`, `breakpoint`, `preload` |
| Transição | `yield` |
| Retorno | `void` |
| Constantes globais de linguagem | `PI`, `TAU`, `INF`, `NAN` |

Operadores textuais como `in`, `not`, `and` e `or`, bem como nomes de tipos embutidos, também são reservados.

### 4.3 Escopo

Dentro de uma função, a busca por nomes segue esta prioridade:

1. variável local;
2. membro da classe;
3. escopo global.

```gdscript
var value = 10

func print_value() -> void:
    var value = 20
    print(value)      # 20
    print(self.value) # 10
```

`self` representa a instância atual. Ele existe sempre, mas normalmente só deve ser usado quando melhora a clareza ou quando há conflito de nomes.

### 4.4 `self` e responsabilidade

A documentação alerta para um problema de design: acessar membros esperados apenas em classes filhas a partir de uma classe base torna a relação difícil de entender. Se uma base precisa chamar algo de uma subclasse, considere um método abstrato, um sinal, uma composição explícita ou outro contrato mais claro.

---

## Parte 5 — Literais, Texto e Formatação

[⬆️ Voltar ao Sumário](#sumario)

### 5.1 Literais essenciais

| Literal | Significado |
|---|---|
| `null` | ausência de valor |
| `true`, `false` | valores booleanos |
| `45` | inteiro decimal |
| `0x8f51` | inteiro hexadecimal |
| `0b101010` | inteiro binário |
| `3.14`, `58.1e-10` | número de ponto flutuante |
| `"texto"`, `'texto'` | string comum |
| `"""texto"""`, `'''texto'''` | string comum multilinha |
| `r"texto"` | string raw |
| `&"name"` | `StringName` |
| `^"Node/Label"` | `NodePath` |

Números podem usar `_` como separador visual:

```gdscript
var population = 1_250_000
var mask = 0b1101_0010
var color = 0xffaa_0022
```

### 5.2 Strings comuns

Strings podem usar aspas duplas ou simples. O guia de estilo oficial prefere aspas duplas, salvo quando aspas simples reduzem escapes.

```gdscript
var message = "Hello, Godot"
var quote = 'Ela disse: "pronto".'
```

Sequências de escape comuns incluem `\n`, `\t`, `\r`, `\"`, `\'`, `\\`, `\uXXXX` e `\UXXXXXX`.

### 5.3 Strings raw

Strings raw preservam o texto de modo mais literal e são úteis em expressões regulares ou caminhos que teriam muitos escapes.

```gdscript
var pattern = r"\d+\.\d+"
```

### 5.4 `StringName`

`StringName` é uma string imutável internada: nomes iguais compartilham uma representação única. Ela é mais cara de criar, mas muito rápida de comparar. É comum em nomes de métodos, sinais, propriedades e chaves recorrentes.

```gdscript
var action: StringName = &"jump"
```

### 5.5 `NodePath`

`NodePath` representa um caminho pré-analisado para nó ou propriedade.

```gdscript
var target_path: NodePath = ^"Player/Camera2D"
```

Também existem atalhos de cena:

```gdscript
@onready var camera: Camera2D = $Player/Camera2D
@onready var hud = %HUD
```

`$NodePath` é atalho para buscar um nó. `%UniqueNode` busca um nó marcado como nome único no dono da cena.

### 5.6 Strings formatadas com `%`

GDScript suporta strings formatadas com o operador `%`.

```gdscript
var name = "Godot"
var version = 4.7
print("%s %.1f" % [name, version])
```

Especificadores comuns:

| Especificador | Uso |
|---|---|
| `%s` | converte para `String` |
| `%d` | inteiro decimal |
| `%x`, `%X` | hexadecimal |
| `%f` | número real |
| `%c` | caractere Unicode |
| `%v` | vetor |

Para imprimir `%` literal, escreva `%%`.

### 5.7 `String.format()`

`String.format()` permite substituir chaves usando array ou dictionary.

```gdscript
var text = "Player {name} has {hp} HP"
print(text.format({ "name": "Ari", "hp": 80 }))
```

Quando precisar controlar casas decimais e alinhamento numérico, o operador `%` costuma ser mais direto. Quando precisar de nomes explícitos nos placeholders, `format()` tende a ser mais legível.

### 5.8 Concatenação

Strings podem ser concatenadas com `+`, mas valores não string precisam ser convertidos com `str()`.

```gdscript
var hp = 90
print("HP: " + str(hp))
```

Para mensagens com vários valores, prefira `%` ou `format()` por legibilidade.

---

## Parte 6 — Variant e Modelo de Tipos

[⬆️ Voltar ao Sumário](#sumario)

### 6.1 O papel de `Variant`

`Variant` é o tipo central do Godot. Ele consegue armazenar quase qualquer tipo de dado da engine e serve como base para comunicação entre sistemas, propriedades, serialização, chamadas diferidas, arrays, dictionaries e APIs expostas.

Em GDScript dinâmico, uma variável pode mudar de tipo por atribuição:

```gdscript
var value = 10
value = "agora sou texto"
value = RefCounted.new()
```

Com tipagem estática, a variável mantém o tipo declarado:

```gdscript
var value: int = 10
# value = "texto" # erro
```

### 6.2 Tipagem dinâmica

Tipagem dinâmica reduz cerimônia e acelera prototipação. O custo é que alguns erros aparecem em execução, e não durante análise estática.

```gdscript
func apply_damage(target, amount):
    target.take_damage(amount)
```

Esse código funciona se `target` tiver `take_damage`. Se não tiver, o erro aparece em runtime. Essa característica se relaciona a duck typing.

### 6.3 Tipagem estática opcional

GDScript permite declarar tipos:

```gdscript
var health: int = 100
var velocity: Vector2 = Vector2.ZERO

func heal(amount: int) -> void:
    health += amount
```

Também permite inferir tipo com `:=`:

```gdscript
var direction := Vector2.RIGHT
```

Use inferência quando o tipo estiver óbvio na própria linha. Declare o tipo quando a expressão for ambígua, vier de `get_node()`, de função complexa ou de API dinâmica.

### 6.4 Cópia e referência

A documentação oficial separa os tipos embutidos alocados como valor dos tipos compartilhados por referência. Em termos práticos:

| Categoria | Comportamento |
|---|---|
| tipos como `int`, `float`, `String`, vetores e transforms | cópia em atribuição e passagem de argumento |
| `Object`, `Array`, `Dictionary` e packed arrays | referência compartilhada |

```gdscript
var a = [1, 2]
var b = a
b.append(3)
print(a) # [1, 2, 3]
```

Para copiar conteúdo, use APIs como `duplicate()` quando disponíveis, ou métodos próprios de coleção.

### 6.5 `null` e valores nulos

`null` representa ausência de valor. A documentação observa que apenas tipos que herdam de `Object` podem ter valor `null` como tipo nulo. Variants precisam conter um valor válido.

```gdscript
var enemy: Node = null
```

Para objetos que podem ter sido liberados, não basta raciocinar apenas com `null`. Use `is_instance_valid()` quando houver possibilidade de o objeto ter sido destruído.

---

## Parte 7 — Tipos Embutidos

[⬆️ Voltar ao Sumário](#sumario)

### 7.1 Tipos básicos

| Tipo | Modelo mental |
|---|---|
| `bool` | `true` ou `false` |
| `int` | inteiro de 64 bits |
| `float` | ponto flutuante de 64 bits |
| `String` | sequência Unicode |
| `StringName` | nome internado e rápido de comparar |
| `NodePath` | caminho para nó ou propriedade |

### 7.2 Tipos matemáticos e geométricos

| Tipo | Uso comum |
|---|---|
| `Vector2`, `Vector2i` | posição, direção e grade 2D |
| `Rect2`, `Rect2i` | retângulos e áreas 2D |
| `Vector3`, `Vector3i` | posição, direção e grade 3D |
| `Vector4`, `Vector4i` | dados vetoriais de quatro componentes |
| `Transform2D` | transformação 2D |
| `Plane` | plano 3D |
| `Quaternion` | rotação 3D |
| `AABB` | caixa alinhada aos eixos |
| `Basis` | base 3D de rotação e escala |
| `Transform3D` | transformação 3D |
| `Projection` | matriz de projeção |

Esses tipos são fundamentais porque a engine é espacial. Mesmo scripts simples usam vetores para movimento, posição, direção, colisão e interpolação.

### 7.3 Tipos da engine

| Tipo | Uso |
|---|---|
| `Color` | cor com componentes `r`, `g`, `b`, `a` |
| `RID` | identificador opaco usado por servidores internos |
| `Object` | base para classes que não são tipos embutidos de valor |

`Object` merece atenção especial: quase tudo que é classe da engine herda dele direta ou indiretamente. `Node`, `Resource` e `RefCounted` pertencem a essa árvore.

### 7.4 `Array`

`Array` é uma sequência dinâmica indexada a partir de `0`. Índices negativos contam do fim.

```gdscript
var values = [10, 20, 30]
print(values[0])  # 10
print(values[-1]) # 30
values.append(40)
```

Arrays sem tipo podem misturar valores:

```gdscript
var mixed = [1, "text", Vector2.ZERO]
```

### 7.5 Arrays tipados

Arrays tipados usam `Array[Type]`.

```gdscript
var enemies: Array[Node2D] = []
var scores: Array[int] = [10, 20, 30]
```

O Godot verifica escritas para impedir elementos incompatíveis. Arrays aninhados tipados como `Array[Array[int]]` não são suportados; use `Array[Array]` ou modele uma classe/recurso próprio.

### 7.6 Packed arrays

Packed arrays são coleções especializadas, geralmente mais econômicas em memória e eficientes para dados homogêneos.

| Tipo | Conteúdo |
|---|---|
| `PackedByteArray` | bytes |
| `PackedInt32Array`, `PackedInt64Array` | inteiros |
| `PackedFloat32Array`, `PackedFloat64Array` | floats |
| `PackedStringArray` | strings |
| `PackedVector2Array`, `PackedVector3Array`, `PackedVector4Array` | vetores |
| `PackedColorArray` | cores |

Use packed arrays quando volume, memória e iteração pesarem. Use `Array` comum quando a conveniência dos métodos for mais importante.

### 7.7 `Dictionary`

`Dictionary` mapeia chaves para valores.

```gdscript
var stats = {
    "hp": 100,
    "mp": 40,
}

stats["hp"] -= 10
```

GDScript também aceita sintaxe estilo Lua para chaves string que são identificadores válidos:

```gdscript
var item = {
    display_name = "Potion",
    amount = 3,
}

print(item.display_name)
```

### 7.8 Dictionaries tipados

A partir de Godot 4.4, dictionaries podem declarar tipos de chave e valor.

```gdscript
var inventory: Dictionary[String, int] = {}
var targets: Dictionary[Vector2i, Node] = {}
var metadata: Dictionary[String, Variant] = {}
```

`Dictionary` e `Dictionary[Variant, Variant]` são equivalentes. Coleções tipadas aninhadas como `Dictionary[String, Dictionary[String, int]]` não são suportadas.

### 7.9 `Signal` e `Callable`

`Signal` representa um sinal que pode ser passado como valor. `Callable` representa uma função ou método chamável.

```gdscript
var callback: Callable = _on_pressed
callback.call()

var button_signal: Signal = $Button.button_up
await button_signal
```

Esses dois tipos tornam eventos e funções valores de primeira classe no estilo do Godot.

---

## Parte 8 — Variáveis, Constantes, Enums e Inicialização

[⬆️ Voltar ao Sumário](#sumario)

### 8.1 `var`

Variáveis são declaradas com `var`. Podem ser membros da classe ou locais dentro de funções.

```gdscript
var health = 100

func take_damage(amount: int) -> void:
    var previous_health = health
    health -= amount
```

Uma variável sem valor inicial começa com `null`.

```gdscript
var selected_target
```

### 8.2 Tipos declarados

```gdscript
var health: int = 100
var label: Label
```

Quando o tipo é declarado, atribuições incompatíveis geram erro.

### 8.3 Inferência com `:=`

```gdscript
var start_position := Vector2.ZERO
var player := get_node("Player") as CharacterBody2D
```

`:=` exige que o tipo possa ser determinado. Se a expressão for ambígua, declare o tipo explicitamente.

### 8.4 Ordem de inicialização

Membros são inicializados em ordem:

1. recebem `null` ou valor padrão conforme o tipo;
2. recebem os valores escritos no script, de cima para baixo;
3. `_init()` é chamado, se existir;
4. valores exportados de cenas e recursos são atribuídos;
5. variáveis `@onready` são inicializadas em classes derivadas de `Node`;
6. `_ready()` é chamado em classes derivadas de `Node`.

Essa ordem explica muitas surpresas.

```gdscript
@export var max_health: int = 100
var health: int = max_health

func _ready() -> void:
    print(health)
```

Se `max_health` for alterado no Inspector, `health` pode não refletir automaticamente essa mudança, dependendo de quando foi inicializado. Para estado derivado de export, muitas vezes é melhor atribuir em `_ready()`.

### 8.5 `@onready`

`@onready` adia a inicialização até antes de `_ready()`.

```gdscript
@onready var health_bar: ProgressBar = $UI/HealthBar
```

É ideal para cachear nós filhos da cena. Não misture `@onready` e `@export` na mesma variável; a documentação oficial alerta que esse caso não funciona como muitos esperam e gera aviso tratado como erro por padrão.

### 8.6 Variáveis estáticas

`static var` pertence à classe, não a cada instância.

```gdscript
class_name EnemyCounter

static var spawned_count: int = 0

func _init() -> void:
    spawned_count += 1
```

Variáveis estáticas podem ter tipo, getter e setter. Não podem ser locais, nem receber `@export` ou `@onready`.

### 8.7 Constantes

`const` define valor conhecido em tempo de compilação.

```gdscript
const MAX_PARTY_SIZE: int = 4
const START_POSITION := Vector2(32, 64)
```

Use constantes para nomes de valores que não mudam durante o jogo. Constantes podem existir no corpo da classe ou dentro de funções.

### 8.8 Enums

Enums são atalhos para constantes inteiras.

```gdscript
enum State {
    IDLE,
    RUNNING,
    ATTACKING,
}

var current_state: State = State.IDLE
```

Um enum nomeado cria um dictionary constante. Suas chaves devem ser acessadas com prefixo:

```gdscript
print(State.ATTACKING)
print(State.keys())
print(State.values())
```

Valores não atribuídos começam em `0` e seguem incrementando a partir do valor anterior. Chaves diferentes podem ter o mesmo valor.

---

## Parte 9 — Operadores e Expressões

[⬆️ Voltar ao Sumário](#sumario)

### 9.1 Operadores de acesso e chamada

| Operador | Significado |
|---|---|
| `x[index]` | indexação |
| `x.attribute` | acesso a atributo |
| `foo()` | chamada direta |
| `await x` | espera sinal ou corrotina |

### 9.2 Aritmética

| Operador | Uso |
|---|---|
| `+`, `-` | soma e subtração |
| `*`, `/`, `%` | multiplicação, divisão e resto inteiro |
| `**` | potência |
| `+x`, `-x` | identidade e negação |

Se ambos operandos de `/` forem `int`, a divisão é inteira.

```gdscript
print(5 / 2)   # 2
print(5 / 2.0) # 2.5
```

Para resto com floats, use funções como `fmod()`. Para resto matemático com sinal positivo, use `posmod()` ou `fposmod()`.

### 9.3 Bits

| Operador | Uso |
|---|---|
| `~x` | NOT bit a bit |
| `x & y` | AND bit a bit |
| `x | y` | OR bit a bit |
| `x ^ y` | XOR bit a bit |
| `x << y`, `x >> y` | deslocamento |

Bits aparecem em flags, layers, máscaras e opções combináveis.

### 9.4 Comparação

```gdscript
x == y
x != y
x < y
x <= y
x > y
x >= y
```

Comparações entre tipos diferentes podem ser válidas em alguns casos e erro em outros. Para floats, prefira funções aproximadas como `is_equal_approx()` e `is_zero_approx()` quando o problema envolver arredondamento.

### 9.5 Inclusão com `in`

```gdscript
if "hp" in stats:
    print(stats.hp)

if 3 in [1, 2, 3]:
    print("encontrado")
```

`in` também é usado em `for`.

### 9.6 Lógica booleana

O guia de estilo oficial recomenda as formas em inglês:

```gdscript
if is_alive and not is_stunned:
    attack()
```

Evite preferir `&&`, `||` e `!` em código GDScript novo, ainda que sejam aceitos como aliases.

### 9.7 Ternário

```gdscript
var state = "grounded" if is_on_floor() else "airborne"
```

O ternário em GDScript tem forma `valor_se_verdadeiro if condição else valor_se_falso`.

### 9.8 Cast com `as`

```gdscript
var body := area as CharacterBody2D
if body:
    body.velocity = Vector2.ZERO
```

Para objetos, se o cast falhar, o resultado é `null`. Para tipos embutidos, o Godot tenta converter e gera erro se não conseguir.

### 9.9 Checagem de tipo com `is`

```gdscript
if entity is Enemy:
    entity.apply_damage(10)

if body is not PlayerController:
    return
```

Use `is` quando o tipo é parte do contrato. Use duck typing com cuidado quando o script aceita qualquer objeto com determinado método.

---

## Parte 10 — Controle de Fluxo

[⬆️ Voltar ao Sumário](#sumario)

### 10.1 `if`, `elif`, `else`

```gdscript
if health <= 0:
    die()
elif health < 30:
    flee()
else:
    attack()
```

Parênteses são permitidos, mas não necessários. O estilo oficial recomenda evitar parênteses desnecessários.

### 10.2 `while`

```gdscript
while enemies.size() > 0:
    var enemy = enemies.pop_back()
    enemy.queue_free()
```

`break` interrompe o laço. `continue` passa para a próxima iteração.

### 10.3 `for`

`for` itera sobre coleções, strings, dictionaries, ranges e alguns valores numéricos.

```gdscript
for enemy in enemies:
    enemy.take_damage(10)
```

Dictionaries iteram pelas chaves:

```gdscript
for key in stats:
    print("%s = %s" % [key, stats[key]])
```

### 10.4 `range()`

```gdscript
for i in range(3):
    print(i) # 0, 1, 2

for i in range(2, 8, 2):
    print(i) # 2, 4, 6
```

`range()` não aloca um array para o laço, segundo a documentação oficial.

### 10.5 Alterar array durante iteração

Se quiser alterar valores por índice, itere pelos índices.

```gdscript
for i in items.size():
    items[i] = normalize_item(items[i])
```

Atribuir à variável do laço não muda o array.

```gdscript
for item in items:
    item = null # não altera a posição no array
```

Se o item for objeto, chamar métodos nele afeta o próprio objeto.

### 10.6 `pass`

`pass` ocupa um bloco que ainda não deve executar nada.

```gdscript
func not_implemented_yet() -> void:
    pass
```

---

## Parte 11 — match e Pattern Matching

[⬆️ Voltar ao Sumário](#sumario)

### 11.1 O que é `match`

`match` ramifica execução comparando um valor contra padrões. Ele é semelhante a `switch`, mas mais expressivo.

```gdscript
match state:
    State.IDLE:
        play_idle()
    State.RUNNING:
        play_run()
    _:
        push_warning("Estado desconhecido")
```

`match` é mais estrito por tipo que `==`. Por exemplo, `1` não casa com `1.0`.

### 11.2 Padrão literal

```gdscript
match value:
    1:
        print("um")
    "start":
        print("começar")
```

### 11.3 Padrão de expressão

Casa contra uma expressão constante, identificador ou acesso a atributo.

```gdscript
match typeof(value):
    TYPE_INT:
        print("inteiro")
    TYPE_STRING:
        print("texto")
```

### 11.4 Wildcard

`_` casa qualquer coisa.

```gdscript
match command:
    "save":
        save_game()
    _:
        print("comando ignorado")
```

### 11.5 Binding

`var nome` casa qualquer valor e o vincula a uma variável local.

```gdscript
match result:
    {"ok": true, "value": var value}:
        print(value)
    var unknown:
        print("resultado inesperado: ", unknown)
```

### 11.6 Arrays

```gdscript
match point:
    [0, 0]:
        print("origem")
    [var x, 0]:
        print("eixo X em ", x)
    [var x, var y]:
        print("ponto ", x, ", ", y)
```

`..` permite padrão aberto no final.

```gdscript
match values:
    [42, ..]:
        print("começa com 42")
```

### 11.7 Dictionaries

```gdscript
match payload:
    {"type": "damage", "amount": var amount}:
        take_damage(amount)
    {"type": "heal", "amount": var amount}:
        heal(amount)
```

Em dictionary patterns, chaves precisam ser padrões constantes. Também é possível verificar apenas existência de chaves.

### 11.8 Múltiplos padrões

```gdscript
match command:
    "quit", "exit", "close":
        quit()
```

Padrões múltiplos não podem conter bindings.

### 11.9 Guards com `when`

```gdscript
match point:
    [var x, var y] when x == y:
        print("diagonal principal")
    [var x, var y] when x == -y:
        print("diagonal secundária")
```

O guard é avaliado apenas se o padrão casar.

---

## Parte 12 — Funções

[⬆️ Voltar ao Sumário](#sumario)

### 12.1 Funções pertencem a classes

Em GDScript, funções sempre pertencem a uma classe. Mesmo quando parecem funções "soltas", elas estão no corpo do script, que é uma classe.

```gdscript
func add(a, b):
    return a + b
```

Se não houver `return`, o valor retornado é `null`.

### 12.2 Parâmetros obrigatórios e opcionais

Parâmetros são obrigatórios por padrão. Parâmetros opcionais devem ficar ao final.

```gdscript
func spawn(enemy_id: StringName, amount: int = 1) -> void:
    pass
```

### 12.3 Tipos em parâmetros

```gdscript
func move_to(target: Vector2, speed: float) -> void:
    position = position.move_toward(target, speed)
```

### 12.4 Tipo de retorno

O retorno é declarado com `->`.

```gdscript
func calculate_score(base: int, multiplier: int) -> int:
    return base * multiplier
```

Funções `void` podem retornar cedo, mas não podem retornar valor.

```gdscript
func die() -> void:
    if is_queued_for_deletion():
        return
    queue_free()
```

Funções não `void` precisam retornar valor em todos os caminhos possíveis.

### 12.5 Funções de uma linha

```gdscript
func square(x: int) -> int: return x * x
```

Use com moderação. Para lógica real, prefira bloco normal.

### 12.6 Métodos virtuais da engine

Métodos como `_ready()`, `_process(delta)`, `_physics_process(delta)` e `_input(event)` são callbacks virtuais. Eles são chamados pela engine em momentos específicos.

```gdscript
func _ready() -> void:
    print("pronto")

func _process(delta: float) -> void:
    rotate(delta)
```

Não tente "sobrescrever" métodos nativos não virtuais como `queue_free()` ou `get_class()`. A documentação oficial alerta que isso não é suportado e pode gerar aviso tratado como erro.

---

## Parte 13 — Callable e Lambdas

[⬆️ Voltar ao Sumário](#sumario)

### 13.1 Funções como valores

Referenciar uma função pelo nome, sem chamá-la, produz um `Callable`.

```gdscript
func add_one(value: int) -> int:
    return value + 1

func apply(values: Array[int], operation: Callable) -> Array:
    var result: Array = []
    for value in values:
        result.append(operation.call(value))
    return result

func _ready() -> void:
    print(apply([1, 2, 3], add_one))
```

Callables devem ser executados com `.call()`. Não se usa `callable()` diretamente.

### 13.2 Lambdas

Lambdas criam `Callable` sem declarar método no escopo da classe.

```gdscript
var double := func(value: int) -> int:
    return value * 2

print(double.call(10))
```

Também podem ser nomeadas para facilitar debug:

```gdscript
var damage_formula := func calculate_damage(power: int) -> int:
    return power * 3
```

### 13.3 Captura de variáveis

Lambdas capturam o ambiente local. A documentação oficial destaca que variáveis locais são capturadas por valor no momento em que a lambda é criada.

```gdscript
var value = 10
var printer = func() -> void:
    print(value)

value = 20
printer.call() # imprime 10
```

Para arrays, dictionaries e objetos, mudanças no conteúdo compartilhado ainda podem ser observadas enquanto a referência for a mesma.

### 13.4 `Callable.bind()`

`bind()` cria uma versão do callable com argumentos adicionais.

```gdscript
button.pressed.connect(_on_button_pressed.bind(button.name))

func _on_button_pressed(button_name: String) -> void:
    print(button_name)
```

Isso é útil quando o sinal não envia toda a informação necessária ao receptor.

---

## Parte 14 — await, Sinais e Corrotinas

[⬆️ Voltar ao Sumário](#sumario)

### 14.1 O que `await` faz

`await` espera um sinal ou uma corrotina. Ao encontrar um `await` desse tipo, a função devolve controle ao chamador e retoma quando o sinal é emitido ou quando a corrotina termina.

```gdscript
func wait_button() -> bool:
    print("aguardando")
    await $Button.button_up
    print("confirmado")
    return true
```

### 14.2 Chamar corrotina

Se uma função aguarda uma corrotina e precisa do valor dela, também deve usar `await`.

```gdscript
func request_confirmation() -> void:
    var confirmed = await wait_button()
    if confirmed:
        print("ok")
```

Pedir o valor de uma corrotina sem `await` gera erro.

### 14.3 Chamada assíncrona sem valor

Se o resultado não importa, a função pode ser chamada sem `await`.

```gdscript
func start_prompt() -> void:
    wait_button()
    print("isto executa imediatamente")
```

### 14.4 Await em valor comum

Se `await` recebe uma expressão que não é sinal nem corrotina, o valor é retornado imediatamente e a função não vira corrotina.

```gdscript
func five() -> int:
    return 5

func example() -> void:
    var x = await five()
    print(x)
```

### 14.5 Retorno de sinais

Quando um sinal aguardado emite argumentos, o resultado depende da quantidade:

| Quantidade de argumentos do sinal | Valor recebido |
|---|---|
| zero | `null` |
| um | o próprio valor |
| mais de um | `Array` |

### 14.6 `yield`

`yield` permanece como palavra-chave por transição de versões antigas. Em Godot 4, o mecanismo moderno é `await`. Diferentemente do `yield` antigo, `await` não expõe um objeto de estado de função, preservando segurança de tipos.

---

## Parte 15 — Classes, Herança e Construtores

[⬆️ Voltar ao Sumário](#sumario)

### 15.1 Classes nomeadas e anônimas

Sem `class_name`, a classe é usada por caminho.

```gdscript
const Enemy = preload("res://enemy.gd")
var enemy = Enemy.new()
```

Com `class_name`, vira tipo global.

```gdscript
class_name Enemy
extends CharacterBody2D
```

### 15.2 Classes internas

```gdscript
class DamageEvent:
    var amount: int
    var source: Node

    func _init(p_amount: int, p_source: Node) -> void:
        amount = p_amount
        source = p_source
```

Classes internas são instanciadas com `.new()`.

```gdscript
var event = DamageEvent.new(10, self)
```

### 15.3 Herança

GDScript permite herança simples. Herança múltipla não é permitida.

```gdscript
extends Node2D
```

```gdscript
extends "res://actors/base_actor.gd"
```

### 15.4 `super`

`super` chama implementação da classe base.

```gdscript
func _ready() -> void:
    super()
    initialize_inventory()
```

Também pode chamar outro método da base:

```gdscript
func calculate() -> int:
    return super.calculate() + bonus
```

### 15.5 Classes abstratas

`@abstract` marca classes ou métodos abstratos.

```gdscript
@abstract class Shape:
    @abstract func area() -> float

class Circle extends Shape:
    var radius: float

    func area() -> float:
        return PI * radius * radius
```

Classes abstratas não podem ser instanciadas diretamente. Classes concretas precisam implementar métodos abstratos herdados.

### 15.6 `_init()`

O construtor de instância se chama `_init`.

```gdscript
var display_name: String

func _init(p_display_name: String = "") -> void:
    display_name = p_display_name
```

Se a classe base define `_init()` com argumentos, a classe filha precisa definir `_init()` e chamar `super(...)` de modo compatível.

```gdscript
func _init(id: StringName, amount: int) -> void:
    super(id)
    stack_amount = amount
```

### 15.7 `_static_init()`

O construtor estático se chama `_static_init`. Ele é chamado quando a classe é carregada, depois das variáveis estáticas serem inicializadas.

```gdscript
static var cache: Dictionary[String, int] = {}

static func _static_init() -> void:
    cache["created"] = Time.get_ticks_msec()
```

Ele não recebe argumentos e não retorna valor.

---

## Parte 16 — Propriedades, Getters, Setters e Membros Estáticos

[⬆️ Voltar ao Sumário](#sumario)

### 16.1 Propriedades com `get` e `set`

GDScript permite associar lógica de leitura e escrita a uma variável.

```gdscript
var milliseconds: int = 0

var seconds: int:
    get:
        return milliseconds / 1000
    set(value):
        milliseconds = value * 1000
```

Use propriedades quando a leitura ou escrita precisa validar, converter, emitir sinal, atualizar estado derivado ou proteger invariantes.

### 16.2 Exemplo com validação

```gdscript
signal health_changed(value: int)

var health: int = 100:
    set(value):
        health = clampi(value, 0, max_health)
        health_changed.emit(health)
```

Dentro do próprio setter, atribuir ao nome da variável acessa o armazenamento interno e não causa recursão infinita. Essa exceção não se propaga para outras funções chamadas pelo setter.

### 16.3 Sintaxe alternativa

```gdscript
var energy: int:
    get = get_energy,
    set = set_energy

func get_energy() -> int:
    return _energy

func set_energy(value: int) -> void:
    _energy = max(value, 0)
```

Não misture estilos de getter/setter para a mesma variável.

### 16.4 Inicialização não chama setter

Quando uma variável é inicializada, o valor inicial é escrito diretamente. Isso inclui `@onready`. Se a lógica do setter precisa valer também no início, chame-a explicitamente ou mova a inicialização para um método que passa pelo setter.

### 16.5 Funções estáticas

```gdscript
static func distance_squared(a: Vector2, b: Vector2) -> float:
    return a.distance_squared_to(b)
```

Funções estáticas não têm acesso a `self` nem a variáveis de instância. Elas podem acessar variáveis estáticas.

---

## Parte 17 — Modelo de Objetos e Memória

[⬆️ Voltar ao Sumário](#sumario)

### 17.1 `Object`

`Object` é a base das classes da engine que não são tipos embutidos de valor. Objetos podem ter propriedades, métodos, sinais, metadados e scripts anexados.

```gdscript
var node = Node.new()
print("name" in node)
print("tree_entered" in node)
```

O operador `in` pode verificar se propriedade, método ou sinal existe em um objeto.

### 17.2 `RefCounted`

Classes que herdam de `RefCounted` são liberadas automaticamente quando não há mais referências. `Resource` herda de `RefCounted`.

```gdscript
var resource = Resource.new()
```

Para classes de dados que não precisam estar na árvore de cena, `RefCounted` ou `Resource` costuma ser mais adequado que `Node`.

### 17.3 `Node`

`Node` é a base dos objetos de cena. Nós vivem em árvore, podem ter filhos, receber callbacks, pertencer a grupos e ser salvos em cenas.

Quando um `Node` é liberado, seus filhos também são liberados recursivamente.

```gdscript
queue_free()
```

### 17.4 `free()` e `queue_free()`

Objetos que não gerenciam memória automaticamente precisam ser liberados com `free()`. Para `Node`, `queue_free()` é geralmente preferido, pois agenda a remoção de forma segura no fluxo da engine.

```gdscript
enemy.queue_free()
```

### 17.5 Validade de instância

Uma variável pode continuar apontando para um objeto já liberado. Para checar, use `is_instance_valid()`.

```gdscript
if is_instance_valid(target):
    target.take_damage(10)
```

### 17.6 Referências fracas

`weakref()` cria uma referência que não impede um `RefCounted` de ser liberado.

```gdscript
var weak_file = weakref(file)
var current = weak_file.get_ref()
if current:
    current.close()
```

Use referências fracas para evitar ciclos ou dependências que prolongam a vida de objetos sem necessidade.

---

## Parte 18 — Sinais como Contratos de Comunicação

[⬆️ Voltar ao Sumário](#sumario)

### 18.1 O que é um sinal

Um sinal é uma mensagem emitida por um objeto para que outros objetos reajam. Ele reduz acoplamento porque o emissor não precisa conhecer diretamente todos os receptores.

```gdscript
signal health_depleted
```

### 18.2 Emitir sinal

```gdscript
signal health_changed(old_value, new_value)

func take_damage(amount: int) -> void:
    var old_health = health
    health -= amount
    health_changed.emit(old_health, health)
```

Os nomes de argumentos ajudam o editor e a geração de callbacks. A documentação observa que ainda é responsabilidade do emissor enviar os valores corretos.

### 18.3 Conectar sinal

```gdscript
func _ready() -> void:
    var character = get_node("Character")
    character.health_depleted.connect(_on_character_health_depleted)

func _on_character_health_depleted() -> void:
    get_tree().reload_current_scene()
```

### 18.4 Sinais e `Callable.bind()`

```gdscript
func _ready() -> void:
    for button in $Buttons.get_children():
        button.pressed.connect(_on_button_pressed.bind(button.name))

func _on_button_pressed(button_name: String) -> void:
    print("botão: ", button_name)
```

### 18.5 Quando preferir sinais

Use sinais quando:

- o emissor não deve conhecer o receptor;
- vários objetos podem reagir ao mesmo evento;
- o evento representa mudança de estado relevante;
- a cena deve conectar comportamentos sem criar dependência rígida entre scripts.

Evite sinais para substituir chamadas diretas simples entre objetos que já possuem relação explícita e estável.

---

## Parte 19 — Exports e Inspector

[⬆️ Voltar ao Sumário](#sumario)

### 19.1 O que `@export` faz

`@export` marca uma propriedade para aparecer no Inspector e ser salva em disco no arquivo de cena ou recurso.

```gdscript
@export var speed: float = 300.0
@export var display_name: String = "Potion"
```

Quando o tipo não é óbvio, declare-o.

```gdscript
@export var target: Node
@export var texture: Texture2D
```

### 19.2 Exports com ranges

```gdscript
@export_range(0, 100, 1) var health: int = 100
@export_range(0.0, 1.0, 0.01) var volume: float = 0.5
```

Hints como `"or_greater"`, `"or_less"`, `"suffix:m"` e outros podem ajustar o Inspector.

### 19.3 Enums exportados

```gdscript
enum CharacterClass {
    WARRIOR,
    MAGE,
    THIEF,
}

@export var character_class: CharacterClass = CharacterClass.WARRIOR
```

Também é possível usar `@export_enum` para limitar inteiros ou strings a opções.

```gdscript
@export_enum("Warrior", "Mage", "Thief") var class_index: int
@export_enum("Rebecca", "Mary", "Leah") var character_name: String = "Rebecca"
```

### 19.4 Arquivos e diretórios

```gdscript
@export_file("*.json") var data_file: String
@export_dir var local_folder: String
@export_global_file var external_file: String
@export_global_dir var external_folder: String
```

Use caminhos globais apenas quando o projeto realmente precisa apontar para fora de `res://`.

### 19.5 Flags e layers

```gdscript
@export_flags("Fire", "Ice", "Poison") var damage_types: int
@export_flags_2d_physics var physics_layers: int
@export_flags_3d_render var render_layers: int
```

Flags representam bits. Se a equipe não domina operações bitwise, variáveis booleanas explícitas podem ser mais legíveis.

### 19.6 Arrays exportados

```gdscript
@export var points: Array[Vector2] = []
@export var scenes: Array[PackedScene] = []
@export_range(0, 100, 1) var weights: Array[int] = []
```

Arrays exportados podem ter inicializadores, mas precisam ser expressões constantes.

### 19.7 Organização visual no Inspector

```gdscript
@export_group("Movement")
@export var speed: float = 300.0
@export var acceleration: float = 1200.0

@export_subgroup("Jump")
@export var jump_force: float = 420.0
```

Use grupos para organizar propriedades reais, não para decorar scripts pequenos.

### 19.8 `@export_storage`

`@export_storage` salva a propriedade em disco sem mostrá-la no Inspector.

```gdscript
@export_storage var internal_id: StringName
```

É útil para estado serializado que não deve ser editado manualmente.

### 19.9 `@export_custom`

`@export_custom` dá controle direto sobre hints e usage flags. A documentação oficial alerta que GDScript não valida a sintaxe dessa anotação, então erros podem produzir comportamento inesperado no Inspector.

```gdscript
@export_custom(PROPERTY_HINT_NONE, "suffix:m") var altitude: float
```

---

## Parte 20 — Anotações

[⬆️ Voltar ao Sumário](#sumario)

### 20.1 O que são anotações

Anotações começam com `@` e modificam scripts, declarações, instruções ou posições no código. Elas podem afetar o compilador GDScript, o editor e a forma como propriedades aparecem e são armazenadas.

```gdscript
@export_range(0, 100, 1)
var health: int = 100
```

Podem ser escritas uma por linha ou na mesma linha, desde que afetem a próxima declaração aplicável.

### 20.2 Catálogo oficial de anotações

| Anotação | Uso principal |
|---|---|
| `@abstract` | marca classe ou método abstrato |
| `@export` | expõe propriedade no Inspector e salva em disco |
| `@export_category` | cria categoria no Inspector |
| `@export_color_no_alpha` | exporta cor sem editar alpha |
| `@export_custom` | define hint, hint string e usage flags manualmente |
| `@export_dir` | seleciona diretório local do projeto |
| `@export_enum` | restringe valor a opções |
| `@export_exp_easing` | edita curva de easing |
| `@export_file` | seleciona arquivo local do projeto |
| `@export_file_path` | exporta caminho de arquivo |
| `@export_flags` | edita flags bitwise |
| `@export_flags_2d_navigation` | flags de navegação 2D |
| `@export_flags_2d_physics` | flags de física 2D |
| `@export_flags_2d_render` | flags de renderização 2D |
| `@export_flags_3d_navigation` | flags de navegação 3D |
| `@export_flags_3d_physics` | flags de física 3D |
| `@export_flags_3d_render` | flags de renderização 3D |
| `@export_flags_avoidance` | flags de avoidance |
| `@export_global_dir` | seleciona diretório fora do projeto |
| `@export_global_file` | seleciona arquivo fora do projeto |
| `@export_group` | agrupa propriedades exportadas |
| `@export_multiline` | edita string multilinha |
| `@export_node_path` | exporta caminho de nó, opcionalmente filtrado por tipo |
| `@export_placeholder` | mostra placeholder em campo de texto |
| `@export_range` | edita número com faixa, passo e hints |
| `@export_storage` | salva propriedade sem exibir no Inspector |
| `@export_subgroup` | cria subgrupo no Inspector |
| `@export_tool_button` | expõe botão de ferramenta no Inspector |
| `@icon` | define ícone da classe registrada |
| `@onready` | adia inicialização até antes de `_ready()` |
| `@rpc` | configura método para RPC |
| `@static_unload` | permite descarregar script com estáticos quando possível |
| `@tool` | executa script no editor |
| `@warning_ignore` | ignora aviso em uma linha ou declaração |
| `@warning_ignore_start` | inicia trecho em que avisos são ignorados |
| `@warning_ignore_restore` | restaura avisos ignorados |

### 20.3 Ordem típica no início do script

O guia de estilo oficial recomenda:

```gdscript
@tool
@icon("res://icons/custom_node.svg")
class_name CustomNode
extends Node
```

Se a classe for abstrata:

```gdscript
@abstract
class_name BaseState
extends Node
```

### 20.4 Anotações não são comentários

Comentários documentam. Anotações alteram comportamento. Trate anotações como parte do contrato público do script.

---

## Parte 21 — Tipagem Estática Opcional

[⬆️ Voltar ao Sumário](#sumario)

### 21.1 Por que usar tipos

Tipos estáticos ajudam o editor, melhoram autocomplete, tornam refatoração mais segura e permitem que o analisador detecte operações perigosas antes de executar o jogo.

```gdscript
func _physics_process(delta: float) -> void:
    velocity = velocity.move_toward(target_velocity, acceleration * delta)
```

### 21.2 Tipos declarados

```gdscript
var actor: CharacterBody2D
var speed: float = 300.0

func set_target(target: Node2D) -> void:
    actor.look_at(target.global_position)
```

### 21.3 Tipos inferidos

```gdscript
var direction := Vector2.RIGHT
var scene := preload("res://enemy.tscn")
```

Prefira `:=` quando o tipo aparece claramente no lado direito. Escreva o tipo quando a expressão não deixa claro.

### 21.4 `get_node()` e cast

```gdscript
@onready var health_bar: ProgressBar = get_node("UI/HealthBar")
```

ou:

```gdscript
@onready var health_bar := get_node("UI/HealthBar") as ProgressBar
```

A documentação observa uma diferença importante: o cast com `as` pode resultar em `null` silenciosamente se o tipo não corresponder, enquanto uma atribuição tipada pode falhar mais cedo ao carregar a cena.

### 21.5 Linhas seguras

O editor marca linhas seguras quando possui informação suficiente para saber que a operação é válida pelo tipo. Isso ajuda a identificar trechos ambíguos em código misto entre dinâmico e estático.

### 21.6 Escolha um estilo por projeto

A documentação recomenda consistência: código tipado e dinâmico podem coexistir, mas a equipe deve escolher um estilo predominante. Em projetos grandes, a tipagem estática costuma facilitar leitura, autocomplete e revisão.

```gdscript
func _ready() -> void:
    pass

func _process(delta: float) -> void:
    pass
```

### 21.7 Warnings de tipagem

Warnings como `UNTYPED_DECLARATION`, `INFERRED_DECLARATION` e `UNSAFE_*` podem ser configurados nas Project Settings. Em bases que desejam GDScript tipado, ativar avisos ajuda a manter disciplina.

---

## Parte 22 — Comentários de Documentação

[⬆️ Voltar ao Sumário](#sumario)

### 22.1 `##`

Comentários de documentação começam com `##` e devem aparecer imediatamente antes do membro documentado ou das anotações desse membro.

```gdscript
## Emitido quando a vida chega a zero.
signal health_depleted
```

### 22.2 Documentando o script

Documentação do script deve vir no topo, antes da documentação de membros.

```gdscript
class_name PlayerStats
extends Resource
## Dados persistentes do jogador.
##
## Este recurso guarda atributos que podem ser salvos,
## duplicados e editados no Inspector.
```

### 22.3 Tags de documentação

Tags oficiais de documentação incluem:

| Tag | Uso |
|---|---|
| `@tutorial` | link para tutorial relacionado |
| `@deprecated` | marca elemento obsoleto |
| `@experimental` | marca elemento experimental |

```gdscript
## Sistema antigo de inventário.
##
## @deprecated: Use InventoryModel.
class_name LegacyInventory
```

### 22.4 Membros documentáveis

Podem ser documentados:

- sinais;
- enums;
- valores de enum;
- constantes;
- variáveis;
- funções;
- classes internas.

### 22.5 Tooltip de export

Quando uma variável exportada é documentada, sua descrição pode aparecer como tooltip no editor.

```gdscript
## Velocidade máxima em pixels por segundo.
@export var max_speed: float = 300.0
```

---

## Parte 23 — Guia de Estilo Oficial

[⬆️ Voltar ao Sumário](#sumario)

### 23.1 Objetivo do estilo

Estilo não existe para enfeitar código. Ele reduz custo de leitura, melhora diffs, facilita revisão e diminui variação desnecessária entre scripts.

### 23.2 Ordem geral de um script

A ordem recomendada segue este espírito:

1. `@tool`;
2. `@icon`;
3. `class_name`;
4. `extends`;
5. documentação do script;
6. sinais;
7. enums;
8. constantes;
9. variáveis exportadas;
10. variáveis públicas;
11. variáveis privadas;
12. `@onready`;
13. `_init()`;
14. `_ready()`;
15. callbacks virtuais como `_process()` e `_physics_process()`;
16. métodos públicos;
17. métodos privados.

### 23.3 Sinais e propriedades primeiro

```gdscript
signal player_spawned(position: Vector2)

enum Job {
    KNIGHT,
    WIZARD,
    ROGUE,
}

const MAX_LIVES = 3

@export var job: Job = Job.KNIGHT
@export var max_health: int = 50

var health: int = max_health
@onready var weapon: Node2D = $Weapon
```

### 23.4 Operadores booleanos

Prefira:

```gdscript
if is_alive and not is_stunned:
    attack()
```

Evite em código novo:

```gdscript
if is_alive && !is_stunned:
    attack()
```

### 23.5 Espaços

Use espaço ao redor de operadores e depois de vírgulas.

```gdscript
position.x = target.x + 10
var values = [1, 2, 3]
```

### 23.6 Linhas e quebras

Mantenha linhas sob 100 caracteres, se possível sob 80. Para expressões longas, prefira parênteses.

```gdscript
if (
    position.x > min_x
    and position.x < max_x
    and position.y > min_y
    and position.y < max_y
):
    highlight()
```

### 23.7 Vírgula final

Use vírgula final em arrays, dictionaries e enums multilinha.

```gdscript
var items = [
    "potion",
    "sword",
    "shield",
]
```

### 23.8 Variáveis locais perto do uso

Declare variáveis locais perto da primeira utilização. Isso reduz distância mental entre valor e uso.

```gdscript
func apply_bonus() -> void:
    var bonus := calculate_bonus()
    health += bonus
```

### 23.9 Tipagem no estilo

O guia oficial recomenda inferir com `:=` quando o tipo está evidente e declarar explicitamente quando o tipo é ambíguo.

```gdscript
var direction := Vector3(1, 0, 0)
var health: int = 0
@onready var bar: ProgressBar = get_node("UI/Bar")
```

---

## Parte 24 — Sistema de Avisos, Diagnóstico e Debug

[⬆️ Voltar ao Sumário](#sumario)

### 24.1 Warnings

O Godot mostra avisos enquanto você escreve GDScript. Esses avisos apontam trechos que podem causar problemas em runtime, mas deixam a equipe decidir como tratar cada caso.

Em projetos profissionais, warnings importantes devem ser discutidos como parte do padrão de código.

### 24.2 Ignorar warning pontual

```gdscript
@warning_ignore("unused_parameter")
func _process(delta: float) -> void:
    pass
```

Use com justificativa. Ignorar warning sem entender a causa apenas transfere o problema para o futuro.

### 24.3 Ignorar trecho

```gdscript
@warning_ignore_start("unused_variable")
var temporary_a = 1
var temporary_b = 2
@warning_ignore_restore("unused_variable")
```

### 24.4 `assert`

`assert` verifica uma condição em builds de debug. Em builds release, a expressão não é avaliada. Portanto, a expressão de `assert` não deve ter efeitos colaterais.

```gdscript
assert(health >= 0, "health não deveria ser negativo")
```

### 24.5 `breakpoint`

`breakpoint` cria um ponto de parada armazenado no script.

```gdscript
func suspicious_method() -> void:
    breakpoint
    run_complex_logic()
```

### 24.6 Pilha e debug

Funções úteis de `@GDScript`:

| Função | Uso |
|---|---|
| `print_debug(...)` | imprime com informação de debug |
| `print_stack()` | imprime pilha atual |
| `get_stack()` | retorna pilha como array de dictionaries |
| `assert(condition, message)` | valida em debug |

---

## Parte 25 — load, preload e Scripts como Recursos

[⬆️ Voltar ao Sumário](#sumario)

### 25.1 Scripts são recursos

Classes armazenadas em arquivos são recursos `GDScript`. Para usá-las por caminho, carregue o recurso e instancie com `new()`.

```gdscript
const Enemy = preload("res://actors/enemy.gd")

func create_enemy():
    return Enemy.new()
```

### 25.2 `preload()`

`preload()` carrega em tempo de análise do script e exige caminho constante.

```gdscript
const FireballScene = preload("res://spells/fireball.tscn")
```

Use quando o recurso é conhecido de antemão e deve estar disponível rapidamente.

### 25.3 `load()`

`load()` carrega em tempo de execução.

```gdscript
func load_spell(path: String) -> PackedScene:
    return load(path)
```

Use quando o caminho é dinâmico ou depende de dados.

### 25.4 Caminhos relativos

A documentação oficial destaca que caminhos relativos em `load()` são prefixados com `res://`, não relativos ao script chamador. Para evitar confusão, prefira caminhos absolutos do projeto.

```gdscript
var scene = load("res://levels/level_01.tscn")
```

### 25.5 Classes globais reduzem `preload`

Se o script tem `class_name`, você pode usar o tipo diretamente.

```gdscript
var item := InventoryItem.new()
```

Ainda assim, cenas e recursos específicos normalmente continuam sendo carregados por `preload()` ou `load()`.

---

## Parte 26 — Funções Globais e Escopo Global

[⬆️ Voltar ao Sumário](#sumario)

### 26.1 `@GDScript`

`@GDScript` reúne constantes, funções e anotações acessíveis a scripts GDScript.

| Item | Uso |
|---|---|
| `PI`, `TAU`, `INF`, `NAN` | constantes matemáticas especiais |
| `Color8()` | cria `Color` a partir de componentes 8-bit |
| `assert()` | valida condição em debug |
| `char()` e `ord()` | convertem código Unicode e caractere |
| `convert()` | converte `Variant` para tipo `Variant.Type` |
| `dict_to_inst()` e `inst_to_dict()` | serialização legada de instâncias |
| `get_stack()` | obtém pilha de chamadas |
| `is_instance_of()` | checa tipo dinamicamente |
| `len()` | tamanho de string, array ou dictionary |
| `load()` e `preload()` | carregam recursos |
| `print_debug()` e `print_stack()` | diagnóstico |
| `range()` | sequência para loops |
| `type_exists()` | verifica existência de tipo |

Algumas funções são de compatibilidade ou têm alternativas modernas. Consulte a referência de classe antes de usá-las em código novo.

### 26.2 `@GlobalScope`

`@GlobalScope` contém constantes, enums, funções globais e singletons acessíveis por qualquer linguagem de script do Godot.

Categorias comuns:

| Categoria | Exemplos |
|---|---|
| matemática | `abs()`, `sin()`, `cos()`, `sqrt()`, `lerp()`, `clamp()` |
| tipos | `typeof()`, `type_string()`, `is_instance_valid()` |
| números | `is_nan()`, `is_inf()`, `is_equal_approx()` |
| erro e diagnóstico | `push_error()`, `push_warning()`, `error_string()` |
| aleatoriedade | `randf()`, `randi()`, `randf_range()` |
| serialização Variant | `var_to_bytes()`, `bytes_to_var()` |
| singletons | `Engine`, `Input`, `OS`, `ProjectSettings`, `ResourceLoader`, `Time` |

Nem tudo em `@GlobalScope` é "linguagem" no sentido estrito. Parte é API global da engine. O programador GDScript, porém, usa esses nomes no mesmo espaço mental de programação cotidiana.

### 26.3 `typeof()`

`typeof()` retorna o valor enumérico do tipo `Variant`.

```gdscript
match typeof(value):
    TYPE_NIL:
        print("null")
    TYPE_INT:
        print("int")
    TYPE_OBJECT:
        print("object")
```

### 26.4 Funções tipadas e não tipadas

A documentação sobre tipagem estática recomenda preferir equivalentes tipados quando possível. Por exemplo, em vez de depender sempre de `abs()` ou `clamp()` genéricos, use funções específicas como `absi()`, `absf()`, `clampi()` e `clampf()` quando o tipo é conhecido.

---

## Parte 27 — Callbacks de Node e Ciclo de Cena

[⬆️ Voltar ao Sumário](#sumario)

### 27.1 `Node` como base de cena

`Node` é a base dos objetos de cena. Nós são organizados em árvore. Uma árvore de nós forma uma cena, e cenas podem ser salvas e instanciadas em outras cenas.

### 27.2 Ordem de entrada e prontidão

Quando um nó entra na árvore:

1. o pai recebe `_enter_tree()` antes dos filhos;
2. os filhos recebem `_ready()` antes do pai;
3. depois disso, o nó pode processar frames, física e input se os callbacks existirem.

Esse detalhe é essencial para dependências entre pais e filhos.

### 27.3 `_ready()`

`_ready()` roda quando o nó está pronto na árvore de cena.

```gdscript
func _ready() -> void:
    $Button.pressed.connect(_on_pressed)
```

### 27.4 `_process(delta)`

`_process()` roda a cada frame, com `delta` em segundos.

```gdscript
func _process(delta: float) -> void:
    rotation += delta
```

Use para lógica dependente de frame visual.

### 27.5 `_physics_process(delta)`

`_physics_process()` roda em passo fixo de física.

```gdscript
func _physics_process(delta: float) -> void:
    velocity.y += gravity * delta
    move_and_slide()
```

Use para movimento e física.

### 27.6 `_input(event)` e `_unhandled_input(event)`

`_input()` recebe eventos de input. `_unhandled_input()` recebe eventos que não foram consumidos por outros nós, frequentemente preferível para gameplay quando UI pode capturar eventos.

```gdscript
func _unhandled_input(event: InputEvent) -> void:
    if event.is_action_pressed("jump"):
        jump()
```

### 27.7 `@tool`

Scripts com `@tool` executam no editor.

```gdscript
@tool
extends Node2D

func _ready() -> void:
    print("rodando no editor ou no jogo")
```

Tenha cuidado com `free()` e `queue_free()` em tool scripts. Como eles rodam no editor, uso incorreto pode afetar a própria cena aberta no editor.

---

## Parte 28 — Organização Profissional de Scripts

[⬆️ Voltar ao Sumário](#sumario)

### 28.1 Comece pelo contrato do script

Um script bom deixa claro:

- qual classe ele estende;
- se é tipo global;
- quais valores o designer pode editar;
- quais sinais ele emite;
- quais callbacks da engine ele implementa;
- quais métodos compõem sua API pública.

```gdscript
class_name HealthComponent
extends Node
## Controla vida, dano e cura de uma entidade.

signal depleted
signal changed(value: int)

@export var max_health: int = 100

var health: int = max_health:
    set(value):
        health = clampi(value, 0, max_health)
        changed.emit(health)
        if health == 0:
            depleted.emit()
```

### 28.2 Separe dados de cena quando fizer sentido

Use `Resource` para dados editáveis e reutilizáveis. Use `Node` para comportamento que vive na árvore de cena.

```gdscript
class_name WeaponData
extends Resource

@export var display_name: String
@export var damage: int
@export var cooldown: float
```

```gdscript
class_name WeaponController
extends Node

@export var data: WeaponData
```

### 28.3 Use sinais para eventos, não para esconder fluxo

Sinais são bons para eventos observáveis. Chamadas diretas são melhores quando existe uma relação simples e obrigatória.

| Situação | Preferência |
|---|---|
| botão avisa tela | sinal |
| vida chegou a zero | sinal |
| controlador chama método de seu componente obrigatório | chamada direta |
| muitos sistemas reagem ao mesmo evento | sinal |

### 28.4 Exports são API para o editor

Toda variável exportada vira parte da interface do script com designers, level designers e futuras versões da cena. Nomeie e documente como API pública.

```gdscript
## Velocidade máxima horizontal, em pixels por segundo.
@export_range(0.0, 1000.0, 1.0, "suffix:px/s")
var max_speed: float = 320.0
```

### 28.5 Tipagem como documentação executável

```gdscript
@export var projectile_scene: PackedScene
@onready var muzzle: Marker2D = $Muzzle

func fire(target: Vector2) -> void:
    var projectile := projectile_scene.instantiate() as Node2D
    projectile.global_position = muzzle.global_position
    projectile.look_at(target)
    get_tree().current_scene.add_child(projectile)
```

Tipos aqui documentam intenção e ajudam o editor a encontrar erros.

---

## Parte 29 — Catálogo da Linguagem

[⬆️ Voltar ao Sumário](#sumario)

### 29.1 Palavras-chave

| Palavra | Papel |
|---|---|
| `if`, `elif`, `else` | decisão |
| `for`, `while` | repetição |
| `match`, `when` | pattern matching e guards |
| `break`, `continue`, `pass`, `return` | controle de bloco |
| `class`, `class_name`, `extends` | definição de tipos |
| `is`, `as` | checagem e conversão de tipo |
| `self`, `super` | instância atual e classe base |
| `signal` | declaração de sinal |
| `func`, `static` | função e membro estático |
| `const`, `enum`, `var` | dados nomeados |
| `breakpoint`, `assert` | debug e validação |
| `preload`, `await` | recurso e assíncrono |
| `yield` | legado de corrotinas |
| `void` | ausência de retorno |

### 29.2 Operadores principais

| Categoria | Operadores |
|---|---|
| acesso | `[]`, `.`, `()` |
| assíncrono | `await` |
| tipo | `is`, `is not`, `as` |
| aritmética | `+`, `-`, `*`, `/`, `%`, `**` |
| bits | `~`, `&`, `|`, `^`, `<<`, `>>` |
| comparação | `==`, `!=`, `<`, `>`, `<=`, `>=` |
| inclusão | `in`, `not in` |
| lógica | `not`, `and`, `or` |
| ternário | `a if cond else b` |
| atribuição | `=`, `+=`, `-=`, `*=`, `/=`, `%=`, `**=`, `&=`, `|=`, `^=`, `<<=`, `>>=` |

### 29.3 Literais especiais

| Sintaxe | Tipo |
|---|---|
| `null` | nulo |
| `true`, `false` | `bool` |
| `"text"` | `String` |
| `&"name"` | `StringName` |
| `^"Path/To/Node"` | `NodePath` |
| `[1, 2, 3]` | `Array` |
| `{ "key": "value" }` | `Dictionary` |

### 29.4 Tipos centrais

| Família | Tipos |
|---|---|
| básicos | `bool`, `int`, `float`, `String`, `StringName`, `NodePath` |
| vetores e geometria | `Vector2`, `Vector2i`, `Rect2`, `Rect2i`, `Vector3`, `Vector3i`, `Vector4`, `Vector4i`, `Transform2D`, `Plane`, `Quaternion`, `AABB`, `Basis`, `Transform3D`, `Projection` |
| engine | `Color`, `RID`, `Object` |
| containers | `Array`, `Dictionary`, packed arrays |
| comunicação | `Signal`, `Callable` |

### 29.5 Anotações por família

| Família | Anotações |
|---|---|
| classe | `@abstract`, `@icon`, `@tool`, `@static_unload` |
| export básico | `@export`, `@export_storage`, `@export_custom` |
| export visual | `@export_group`, `@export_subgroup`, `@export_category`, `@export_placeholder`, `@export_multiline`, `@export_tool_button` |
| export numérico | `@export_range`, `@export_exp_easing` |
| export opções e bits | `@export_enum`, `@export_flags`, `@export_flags_2d_physics`, `@export_flags_2d_render`, `@export_flags_2d_navigation`, `@export_flags_3d_physics`, `@export_flags_3d_render`, `@export_flags_3d_navigation`, `@export_flags_avoidance` |
| export caminhos | `@export_file`, `@export_file_path`, `@export_dir`, `@export_global_file`, `@export_global_dir`, `@export_node_path` |
| inicialização | `@onready` |
| rede | `@rpc` |
| warnings | `@warning_ignore`, `@warning_ignore_start`, `@warning_ignore_restore` |

### 29.6 Funções específicas de `@GDScript`

```text
Color8, assert, char, convert, dict_to_inst, get_stack,
inst_to_dict, is_instance_of, len, load, ord, preload,
print_debug, print_stack, range, type_exists
```

### 29.7 Forma mínima de script profissional

```gdscript
class_name ExampleComponent
extends Node
## Descreve o papel do componente.

signal changed(value: int)

const DEFAULT_VALUE: int = 10

@export var max_value: int = 100

var value: int = DEFAULT_VALUE:
    set(new_value):
        value = clampi(new_value, 0, max_value)
        changed.emit(value)

@onready var label: Label = $Label


func _ready() -> void:
    label.text = str(value)


func increase(amount: int) -> void:
    value += amount
```

---

## Parte 30 — Armadilhas Frequentes

[⬆️ Voltar ao Sumário](#sumario)

### 30.1 Tratar GDScript como Python

A sintaxe lembra Python, mas o modelo é Godot: scripts são classes, exports conversam com o Inspector, `Variant` sustenta a API, sinais são objetos da engine e `Node` participa de uma árvore de cena.

### 30.2 Esquecer a ordem de inicialização

Membros são inicializados antes de `_init()`, exports de cena são aplicados depois e `@onready` roda antes de `_ready()`. Bugs de valores "sobrescritos" geralmente estão nessa sequência.

### 30.3 Misturar `@onready` e `@export`

A documentação oficial alerta que combinar `@onready` e `@export` na mesma variável não funciona como muitos esperam. Separe as responsabilidades.

### 30.4 Usar arrays dinâmicos para tudo

Arrays sem tipo são flexíveis, mas ocultam contratos. Em código de produção, prefira `Array[Type]` quando o tipo dos elementos é conhecido.

### 30.5 Ignorar que arrays e dictionaries são referências

```gdscript
var a = {}
var b = a
b.name = "Player"
print(a.name) # "Player"
```

Se precisa de cópia, use APIs de cópia em vez de atribuição simples.

### 30.6 Chamar `Callable` como função comum

```gdscript
var callback = _on_done
callback.call() # correto
```

Não use `callback()`.

### 30.7 Comparar float como inteiro

Use `is_equal_approx()` e `is_zero_approx()` quando o problema envolver imprecisão de ponto flutuante.

### 30.8 Tentar sobrescrever método nativo não virtual

Sobrescreva callbacks virtuais como `_ready()` e `_process()`. Não tente redefinir métodos nativos como `queue_free()` esperando que a engine use sua versão.

### 30.9 Usar `assert` com efeito colateral

`assert` não é avaliado em release. Isto é ruim:

```gdscript
assert(apply_damage(10))
```

Prefira:

```gdscript
var applied := apply_damage(10)
assert(applied)
```

### 30.10 Colocar lógica de gameplay em `@tool` sem proteção

Scripts `@tool` rodam no editor. Proteja código que só deve rodar durante o jogo.

---

## Anexo A — Referências Oficiais Consultadas

[⬆️ Voltar ao Sumário](#sumario)

Todas as fontes abaixo são páginas oficiais da documentação do Godot:

- [Godot Docs — stable, branch 4.7](https://docs.godotengine.org/en/stable/)
- [Godot release policy](https://docs.godotengine.org/en/stable/about/release_policy.html)
- [GDScript index](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/index.html)
- [GDScript reference](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_basics.html)
- [GDScript: An introduction to dynamic languages](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_advanced.html)
- [GDScript exported properties](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_exports.html)
- [GDScript documentation comments](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_documentation_comments.html)
- [GDScript style guide](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_styleguide.html)
- [Static typing in GDScript](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/static_typing.html)
- [GDScript warning system](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/warning_system.html)
- [GDScript format strings](https://docs.godotengine.org/en/stable/tutorials/scripting/gdscript/gdscript_format_string.html)
- [@GDScript class reference](https://docs.godotengine.org/en/stable/classes/class_%40gdscript.html)
- [@GlobalScope class reference](https://docs.godotengine.org/en/stable/classes/class_%40globalscope.html)
- [Variant class reference](https://docs.godotengine.org/en/stable/classes/class_variant.html)
- [Array class reference](https://docs.godotengine.org/en/stable/classes/class_array.html)
- [Object class reference](https://docs.godotengine.org/en/stable/classes/class_object.html)
- [Node class reference](https://docs.godotengine.org/en/stable/classes/class_node.html)

---

## Anexo B — Glossário

[⬆️ Voltar ao Sumário](#sumario)

| Termo | Significado |
|---|---|
| GDScript | linguagem de script integrada ao Godot |
| Script | arquivo `.gd` que define uma classe |
| `Variant` | tipo central que pode armazenar quase qualquer tipo da engine |
| `Object` | base das classes da engine que não são tipos embutidos de valor |
| `RefCounted` | objeto liberado automaticamente por contagem de referências |
| `Resource` | dado serializável e reutilizável, derivado de `RefCounted` |
| `Node` | objeto de cena que vive em árvore |
| Cena | árvore de nós salva e instanciável |
| `class_name` | registro de script como classe global |
| `extends` | herança |
| `@export` | exposição de propriedade no Inspector e armazenamento em disco |
| `@onready` | inicialização adiada até antes de `_ready()` |
| `@tool` | execução do script no editor |
| Sinal | mensagem emitida por objeto para callbacks conectados |
| `Callable` | valor que representa função ou método chamável |
| `await` | espera sinal ou corrotina |
| Corrotina | função que pode suspender e retomar execução |
| `match` | ramificação por padrões |
| Typed array | `Array[Type]`, array com restrição de tipo de elementos |
| Typed dictionary | `Dictionary[KeyType, ValueType]`, dictionary com restrição de chave e valor |
| Packed array | coleção homogênea especializada e compacta |
| Inspector | painel do editor que mostra propriedades editáveis |
| Safe line | linha que o analisador consegue considerar segura por informação de tipo |

---

## Fechamento

[⬆️ Voltar ao Sumário](#sumario)

Aprender GDScript bem é aprender a ler o Godot. A linguagem, sozinha, é pequena; a integração com a engine é o que a torna poderosa. Um bom script não apenas "funciona": ele deixa claro que valores são editáveis, que eventos emite, que tipo de nó espera, quando inicializa dependências, que objetos possui e quais contratos oferece aos outros scripts.

O caminho profissional é simples de enunciar e trabalhoso de praticar: escreva scripts pequenos, tipados quando o tipo comunica intenção, documentados quando viram API do projeto, com exports cuidadosamente nomeados, sinais para eventos reais e chamadas diretas para dependências claras. Assim, GDScript deixa de ser uma coleção de atalhos e passa a ser uma linguagem de arquitetura dentro do Godot.
