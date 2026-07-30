# 🦎 Guia Técnico: Komodo do Zero ao Avançado

> **Nível:** Zero ao Avançado  
> **Linguagem:** Komodo  
> **Fontes de referência principais:** [The Komodo Programming Language](https://komodo-lang.org/book/), [repositório oficial](https://github.com/danilopedraza/komodo), [releases oficiais](https://github.com/danilopedraza/komodo/releases) e exemplos do projeto  
> **Versão de referência:** Komodo v0.5.0, release mais recente publicada em 06/04/2026; documentação conferida também no branch `main` em 30/07/2026  
> **Atualizado em:** 30/07/2026

---

## Nota de Escopo

[⬆️ Voltar ao Sumário](#sumário)

Este guia trata de **Komodo como linguagem de programação**: a linguagem experimental documentada em `komodo-lang.org` e implementada no repositório `danilopedraza/komodo`.

Não é um guia sobre:

- Komodo IDE ou Komodo Edit, da ActiveState;
- a plataforma de deploy/automação `komo.do`;
- o animal dragão-de-komodo.

Essa distinção importa porque há vários projetos chamados Komodo. O contexto aqui é linguagem, sintaxe, interpretador, REPL, arquivos `.komodo`, pattern matching, funções, tipos, biblioteca padrão e limitações.

---

## Prefácio

[⬆️ Voltar ao Sumário](#sumário)

Komodo é uma linguagem pequena, experimental e opinativa. Ela não tenta competir com C#, Java, C++, SQL ou Python como plataforma geral de produção. A proposta documentada pelo próprio projeto é outra: permitir testar ideias rapidamente, especialmente em problemas com estruturas discretas simples, como números, palavras, listas, conjuntos, padrões e transformações.

O risco de aprender Komodo como uma lista de comandos é perder o que torna a linguagem interessante. Komodo é orientada a expressões: quase tudo retorna valor. Usa pattern matching como ferramenta central de modelagem. Permite definir funções por múltiplos padrões. Trata funções como valores. Possui tipagem fraca, tipos embutidos simples e uma biblioteca padrão pequena. Também oferece memoização explícita em definições de função, algo especialmente útil em recursão.

Ao mesmo tempo, Komodo ainda é instável. A documentação oficial diz que a linguagem está em desenvolvimento ativo e que algumas escolhas existem por preferência pessoal do autor. O changelog mostra mudanças relevantes entre versões recentes, como mutabilidade em loops, cons notation, dicionários com notação semelhante a objeto, módulos `math`, `json` e `time`, correções de pattern matching e binário estaticamente linkado em `v0.5.0`.

Este guia, portanto, deve ser lido como um mapa técnico de uma linguagem jovem: útil para estudar ideias de design de linguagem, prototipação e programação com padrões, mas não como promessa de estabilidade de API ou recomendação automática para produção.

---

## Como usar este guia

[⬆️ Voltar ao Sumário](#sumário)

Há três trilhas:

1. **Trilha iniciante:** leia as Partes 1 a 10, use o REPL, execute arquivos `.komodo` e pratique tipos, listas, conjuntos, dicionários e funções.
2. **Trilha de linguagem:** avance pelas Partes 11 a 18, focando pattern matching, destructuring, `case`, `if`, `for`, mutabilidade, imports e biblioteca padrão.
3. **Trilha crítica:** leia as Partes 19 a 24, catálogos e anexos para entender limitações, ecossistema, versão, performance, tooling e critérios de adoção.

Ao estudar qualquer recurso de Komodo, responda:

1. Isso é uma expressão ou uma construção que só produz efeito?
2. O comportamento é melhor modelado por pattern matching, `case`, `if`, `for` ou função de biblioteca?
3. O valor que estou manipulando é lista, conjunto, tupla, dicionário, string ou função?
4. A ordem dos padrões pode mudar o resultado?
5. Esta variável precisa mesmo ser `var`, ou pode ser `let`?
6. O código depende de recurso recente ou instável?

> **Regra de laboratório:** execute exemplos em uma instalação descartável ou no REPL. Komodo é excelente para explorar ideias, mas a própria documentação alerta que a linguagem ainda é instável e lenta para muitos cenários.

---

<a id="sumário"></a>

## Sumário Geral

### Como o conteúdo está organizado

| Bloco | Partes | Assuntos centrais | Resultado esperado | Comece por |
|---|---:|---|---|---|
| 1. Base da linguagem | 1-4 | propósito, versão, instalação, REPL, arquivos e execução | entender o que Komodo é e como experimentar com segurança | [Parte 1](#parte-1--introdução-e-contextualização) |
| 2. Modelo de valores | 5-10 | expressões, `let`, tipos, operadores, listas, conjuntos, tuplas e dicionários | manipular dados básicos e coleções sem confundir contratos | [Parte 5](#parte-5--expressões-valores-e-declarações) |
| 3. Funções e pattern matching | 11-15 | funções nomeadas, anônimas, padrões, destructuring, `case` e memoização | escrever regras compactas e recursivas com leitura clara | [Parte 11](#parte-11--funções-como-valores) |
| 4. Controle, estado e módulos | 16-20 | `if`, `for`, mutabilidade, imports, standard library e pseudo-métodos | escolher entre estilo declarativo e imperativo quando necessário | [Parte 16](#parte-16--if-for-e-controle-de-fluxo) |
| 5. Ferramentas e engenharia | 21-24 | VS Code, exemplos, testes, performance, limitações e evolução | usar Komodo como laboratório técnico sem exagerar o papel da linguagem | [Parte 21](#parte-21--ferramentas-repl-arquivos-e-vscode) |
| 6. Consulta | 25-26 | catálogos de sintaxe, operadores, palavras-chave e módulos | consultar rapidamente a superfície da linguagem | [Parte 25](#parte-25--catálogo-prático-da-linguagem) |
| 7. Revisão | Anexos | trilhas, referências e glossário | aprofundar nas fontes oficiais e revisar conceitos | [Anexo A](#anexo-a--trilhas-de-estudo-e-prática) |

### Atalhos por pergunta prática

| Se você quer saber... | Consulte primeiro |
|---|---|
| o que é Komodo e para que serve | [Parte 1](#parte-1--introdução-e-contextualização) |
| como instalar e rodar | [Partes 3](#parte-3--instalação-e-ambiente) e [4](#parte-4--repl-arquivos-e-fluxo-de-execução) |
| como declarar valores | [Parte 5](#parte-5--expressões-valores-e-declarações) |
| quais tipos existem | [Parte 6](#parte-6--tipos-embutidos) |
| como listas, conjuntos e dicionários funcionam | [Partes 8](#parte-8--listas-tuplas-e-strings) e [9](#parte-9--sets-dicts-e-estruturas-discretas) |
| como escrever função recursiva | [Partes 11](#parte-11--funções-como-valores) e [12](#parte-12--funções-nomeadas-e-múltiplos-padrões) |
| como usar pattern matching | [Partes 13](#parte-13--pattern-matching-como-modelo-mental), [14](#parte-14--destructuring-e-cons-notation) e [15](#parte-15--case-expressions) |
| como usar `if` e `for` | [Parte 16](#parte-16--if-for-e-controle-de-fluxo) |
| quando usar `var` | [Parte 17](#parte-17--mutabilidade-com-var) |
| como importar código ou biblioteca padrão | [Partes 18](#parte-18--imports-módulos-e-escopo) e [19](#parte-19--biblioteca-padrão) |
| onde estão limitações e riscos | [Partes 23](#parte-23--limitações-performance-e-uso-realista) e [24](#parte-24--evolução-versões-e-compatibilidade) |
| quais operadores e palavras-chave lembrar | [Parte 25](#parte-25--catálogo-prático-da-linguagem) |

### Índice detalhado

**Bloco 1 — Base da linguagem**

- **[Parte 1 — Introdução e Contextualização](#parte-1--introdução-e-contextualização)**
  - [1.1 O que é Komodo?](#11-o-que-é-komodo)
  - [1.2 O que Komodo tenta facilitar](#12-o-que-komodo-tenta-facilitar)
  - [1.3 O que Komodo não tenta ser](#13-o-que-komodo-não-tenta-ser)
- **[Parte 2 — Inspirações, Filosofia e Status](#parte-2--inspirações-filosofia-e-status)**
- **[Parte 3 — Instalação e Ambiente](#parte-3--instalação-e-ambiente)**
- **[Parte 4 — REPL, Arquivos e Fluxo de Execução](#parte-4--repl-arquivos-e-fluxo-de-execução)**

**Bloco 2 — Modelo de valores**

- **[Parte 5 — Expressões, Valores e Declarações](#parte-5--expressões-valores-e-declarações)**
- **[Parte 6 — Tipos Embutidos](#parte-6--tipos-embutidos)**
- **[Parte 7 — Operadores, Igualdade e Ordem](#parte-7--operadores-igualdade-e-ordem)**
- **[Parte 8 — Listas, Tuplas e Strings](#parte-8--listas-tuplas-e-strings)**
- **[Parte 9 — Sets, Dicts e Estruturas Discretas](#parte-9--sets-dicts-e-estruturas-discretas)**
- **[Parte 10 — Compreensões, Ranges e Membership](#parte-10--compreensões-ranges-e-membership)**

**Bloco 3 — Funções e pattern matching**

- **[Parte 11 — Funções como Valores](#parte-11--funções-como-valores)**
- **[Parte 12 — Funções Nomeadas e Múltiplos Padrões](#parte-12--funções-nomeadas-e-múltiplos-padrões)**
- **[Parte 13 — Pattern Matching como Modelo Mental](#parte-13--pattern-matching-como-modelo-mental)**
- **[Parte 14 — Destructuring e Cons Notation](#parte-14--destructuring-e-cons-notation)**
- **[Parte 15 — case Expressions](#parte-15--case-expressions)**

**Bloco 4 — Controle, estado e módulos**

- **[Parte 16 — if, for e Controle de Fluxo](#parte-16--if-for-e-controle-de-fluxo)**
- **[Parte 17 — Mutabilidade com var](#parte-17--mutabilidade-com-var)**
- **[Parte 18 — Imports, Módulos e Escopo](#parte-18--imports-módulos-e-escopo)**
- **[Parte 19 — Biblioteca Padrão](#parte-19--biblioteca-padrão)**
- **[Parte 20 — Pseudo-Métodos e Estilo de Chamada](#parte-20--pseudo-métodos-e-estilo-de-chamada)**

**Bloco 5 — Ferramentas e engenharia**

- **[Parte 21 — Ferramentas: REPL, Arquivos e VS Code](#parte-21--ferramentas-repl-arquivos-e-vscode)**
- **[Parte 22 — Testes, assert e Exemplos](#parte-22--testes-assert-e-exemplos)**
- **[Parte 23 — Limitações, Performance e Uso Realista](#parte-23--limitações-performance-e-uso-realista)**
- **[Parte 24 — Evolução, Versões e Compatibilidade](#parte-24--evolução-versões-e-compatibilidade)**

**Consulta e revisão**

- **[Parte 25 — Catálogo Prático da Linguagem](#parte-25--catálogo-prático-da-linguagem)**
- **[Parte 26 — Comparações e Critérios de Adoção](#parte-26--comparações-e-critérios-de-adoção)**
- **[Anexo A — Trilhas de Estudo e Prática](#anexo-a--trilhas-de-estudo-e-prática)**
- **[Anexo B — Referências Oficiais Consultadas](#anexo-b--referências-oficiais-consultadas)**
- **[Glossário](#glossário)**

---

## Parte 1 — Introdução e Contextualização

[⬆️ Voltar ao Sumário](#sumário)

### 1.1 O que é Komodo?

Komodo é uma linguagem de programação interpretada, experimental e orientada à prototipação de ideias. O repositório oficial descreve o projeto como um monorepo que contém interpretador, documentação, exemplos, extensão VS Code, biblioteca padrão, site e scripts de instalação.

O interpretador principal é implementado em Rust. Há também um projeto que compila o interpretador para WebAssembly, usado pelo playground web.

Uma visão curta:

| Aspecto | Komodo |
|---|---|
| Categoria | linguagem experimental e interpretada |
| Implementação principal | Rust |
| Arquivos | `.komodo` |
| Uso inicial | REPL ou execução de arquivo |
| Modelo dominante | expressões, funções e pattern matching |
| Tipagem | fraca, com tipos embutidos simples |
| Custom types | não há tipos definidos pelo usuário na documentação atual |
| Estado do projeto | ativo e instável |

### 1.2 O que Komodo tenta facilitar

Komodo é interessante quando o problema pode ser expresso como transformação sobre:

- números;
- caracteres;
- strings;
- listas;
- conjuntos;
- tuplas;
- dicionários;
- padrões recursivos;
- regras pequenas;
- protótipos de algoritmo.

Exemplo mental:

```komodo
let sum([]) := 0
let sum([first|tail]) := first + sum(tail)

sum([1, 2, 3])
```

Esse estilo aproxima Komodo de linguagens em que a forma dos dados guia o código. Em vez de começar por laços e variáveis auxiliares, você descreve casos.

### 1.3 O que Komodo não tenta ser

Komodo não é, hoje, uma linguagem de produção geral. A própria documentação diz que ela é experimental, instável e lenta. Isso muda a expectativa correta.

| Use Komodo para | Evite usar Komodo para |
|---|---|
| estudar design de linguagem | sistemas críticos |
| testar algoritmos pequenos | aplicações com alto desempenho |
| explorar pattern matching | serviços de longa vida |
| resolver exercícios discretos | ecossistemas com muitas dependências |
| ensinar recursão e expressões | integração empresarial estável |

O valor de Komodo está mais no laboratório conceitual do que no deploy.

---

## Parte 2 — Inspirações, Filosofia e Status

[⬆️ Voltar ao Sumário](#sumário)

A documentação oficial cita inspirações como Picat, SETL, Wolfram Language, Python e JavaScript. Isso ajuda a entender a mistura:

| Influência | Ideia aproximada que aparece em Komodo |
|---|---|
| Picat/Prolog/Erlang | pattern matching e cons notation |
| SETL | trabalho confortável com conjuntos e estruturas discretas |
| Wolfram Language | manipulação simbólica e prototipação expressiva |
| Python/JavaScript | flexibilidade de scripting e sintaxe acessível |

Komodo faz escolhas deliberadas:

- quase tudo retorna valor;
- funções podem ser definidas por padrões;
- o mesmo nome de função pode ter várias regras;
- `let` cria valores imutáveis;
- `var` existe, mas é limitado;
- a linguagem permite passar valores de qualquer tipo para funções;
- memoização é recurso de linguagem;
- a biblioteca padrão é pequena.

O ponto delicado: como a linguagem está em desenvolvimento ativo, exemplos antigos do livro e do apêndice podem não refletir perfeitamente o estado mais recente do interpretador. Quando houver dúvida, priorize:

1. release notes;
2. exemplos executados em CI;
3. código da biblioteca padrão;
4. documentação do livro;
5. experimentação no REPL.

---

## Parte 3 — Instalação e Ambiente

[⬆️ Voltar ao Sumário](#sumário)

Na versão de referência, a documentação oficial oferece duas rotas: baixar binário Linux x86-64 ou compilar do código-fonte.

### Instalação por script

```bash
curl --proto '=https' --tlsv1.2 -sSf https://komodo-lang.org/install.sh | sh
```

Esse estilo é prático, mas merece cuidado. Em ambiente de estudo, tudo bem. Em ambiente controlado, leia o script antes de executá-lo com privilégios.

### Instalação local por binário

```bash
wget https://github.com/danilopedraza/komodo/releases/download/v0.5.0/komodo
chmod +x komodo
mv komodo "$HOME/.local/bin"
```

Depois:

```bash
komodo
```

### Build com Rust

```bash
git clone https://github.com/danilopedraza/komodo.git
cd komodo/core
cargo build --release --all-features
chmod +x target/release/komodo
cp target/release/komodo "$HOME/.local/bin"
```

Também é possível experimentar com:

```bash
cargo run --all-features
```

### Requisitos práticos

| Cenário | Requisito |
|---|---|
| usar binário oficial | Linux x86-64, conforme release atual |
| compilar | Rust toolchain |
| desenvolver o projeto | Rust, Node para extensão, mdBook para documentação, Nix opcional |
| editor | VS Code/VSCodium com extensão de syntax highlighting |

---

## Parte 4 — REPL, Arquivos e Fluxo de Execução

[⬆️ Voltar ao Sumário](#sumário)

Komodo pode ser usado de duas formas principais.

### REPL

```bash
komodo
```

O REPL avalia expressões:

```komodo
>>> 2 + 2
4
```

Declarações também retornam valor:

```komodo
>>> let x := 1 + 0.5
1.5
>>> x + 0.5
2.0
```

### Arquivos

Crie `hello.komodo`:

```komodo
println("Hello, World!")
```

Execute:

```bash
komodo hello.komodo
```

### Modelo de execução

O fluxo mental é:

```text
arquivo .komodo ou entrada REPL
  -> lexer/parser
  -> verificação/inferência limitada
  -> ambiente de nomes
  -> execução pelo interpretador
  -> valor final ou erro
```

O REPL imprime o valor retornado por expressões. Por isso funções como `print` e `println`, que retornam `()`, podem mostrar esse `()` após o efeito de saída quando usadas interativamente.

---

## Parte 5 — Expressões, Valores e Declarações

[⬆️ Voltar ao Sumário](#sumário)

Komodo é orientada a expressões. Isso significa que construções comuns produzem valores.

### `let`

`let` declara valor imutável:

```komodo
let n := 10
let msg := "valor: " + String(n)
```

A expressão de declaração retorna o valor declarado.

### Blocos por indentação

Blocos podem ser escritos por indentação:

```komodo
let report(n) :=
    let doubled := n * 2
    println(String(doubled))
    doubled
```

A última expressão do bloco é o resultado.

### Comentários

```komodo
# isto é um comentário
println("ok")
```

### O vazio: `()`

A tupla vazia `()` aparece como valor de ausência. Builtins como `println` retornam `()`. Laços `for` também retornam `()`.

---

## Parte 6 — Tipos Embutidos

[⬆️ Voltar ao Sumário](#sumário)

Komodo tem um sistema de tipos pequeno e sem tipos customizados pelo usuário.

| Tipo | Exemplo | Ideia |
|---|---|---|
| `Integer` | `10`, `0b1010`, `0o12`, `0x0a` | inteiro assinado de tamanho arbitrário |
| `Float` | `0.5`, `10.25` | número de ponto flutuante |
| `Fraction` | `1 // 2` | racional formado por inteiros |
| `Char` | `'a'`, `'\\'` | caractere |
| `String` | `"texto"` | sequência textual |
| `Tuple` | `()`, `(1, "x")` | agrupamento ordenado |
| `List` | `[1, 2, 3]` | sequência ordenada |
| `Set` | `{1, 2, 3}` | coleção sem repetição e sem ordem estável |
| `Dict` | `{"a" => 1}` | pares chave-valor |
| `Function` | `x -> x * 2` | função como valor |
| `Range` | `0..10` | intervalo com fim exclusivo |
| booleanos | `true`, `false` | valores lógicos |

### Números

```komodo
let a := 0b1011
let b := 0o13
let c := 11
let d := 0x0b

assert({a, b, c, d} = {11})
```

### Conversões

```komodo
Integer(10.8)
Float(1 // 2)
String([1, 2])
List(0..3)
Set([1, 1, 2])
```

`Integer` também passou a parsear strings decimais em `v0.5.0`, segundo release notes.

---

## Parte 7 — Operadores, Igualdade e Ordem

[⬆️ Voltar ao Sumário](#sumário)

Operadores em Komodo dependem do tipo dos operandos.

### Aritmética

```komodo
1 + 2
5 - 3
4 * 7
7 / 2
7 % 2
2 ** 10
```

Inteiros, floats e frações interagem, mas o resultado depende da operação e dos tipos envolvidos. Exemplo importante dos exemplos oficiais:

```komodo
assert(1 / 2 = 0)
assert(1.0 / 2 = 0.5)
assert((1 // 10) * 7 = 7 // 10)
```

### Comparação e lógica

```komodo
1 < 2
1 <= 1
2 > 1
2 >= 2
"a" = "a"
"a" /= "b"
true && false
true || false
!false
```

### Bits

```komodo
1 << 8
256 >> 4
5 & 3
5 ^ 3
~1
```

### Membership

```komodo
1 in [1, 2]
3 in {1, 2}
"x" in {"x", "y"}
```

---

## Parte 8 — Listas, Tuplas e Strings

[⬆️ Voltar ao Sumário](#sumário)

### Listas

Listas são sequências ordenadas:

```komodo
let values := [1, 2, 3]
```

Concatenação:

```komodo
[1, 2] + [3]
```

Repetição:

```komodo
[1, 3] * 2
```

Cons notation:

```komodo
[0|[1, 2, 3]]
```

Compreensão:

```komodo
[x * 2 for x in 0..5]
```

### Tuplas

Tuplas agrupam valores:

```komodo
let point := (10, 20)
let nothing := ()
```

Use tuplas quando a intenção é agrupar, não iterar ou transformar como lista.

### Strings e chars

```komodo
"ab" + "cd"
"ab" + 'c'
'z' * 3
"ha" * 2
```

Komodo permite tratar strings com padrões de cons em exemplos oficiais:

```komodo
let reverse("") := ""
let reverse([first|tail]: String) := reverse(tail) + first
```

Esse recurso é poderoso, mas reforça que pattern matching é parte central da linguagem.

---

## Parte 9 — Sets, Dicts e Estruturas Discretas

[⬆️ Voltar ao Sumário](#sumário)

### Sets

Sets removem duplicatas e não preservam uma ordem confiável.

```komodo
let A := {1, 2}
let B := {2, 3}

A + B
A - B
1 in A
{1} <= {1, 2}
```

Cons notation em sets:

```komodo
{0|{1, 2}}
```

Compreensão:

```komodo
{k % 2 for k in 0..10}
```

### Dicts

Dicionários guardam pares:

```komodo
let data := {
    "name" => "Ada",
    "age" => 36
}
```

Acesso por chave:

```komodo
data["name"]
```

Quando a chave é string, há notação por ponto:

```komodo
data.name
```

Exemplo com função como valor:

```komodo
let obj := {
    "answer" => () -> 42
}

obj.answer()
```

Esse estilo se parece com objeto, mas Komodo não tem classes nem tipos customizados na documentação atual. É melhor pensar em dicionário com açúcar sintático.

---

## Parte 10 — Compreensões, Ranges e Membership

[⬆️ Voltar ao Sumário](#sumário)

Ranges usam `..` e excluem o limite final:

```komodo
0..5
```

List comprehension:

```komodo
[2 ** k for k in 0..4]
```

Set comprehension:

```komodo
{k % 3 for k in 0..10}
```

Membership:

```komodo
2 in [1, 2, 3]
2 in {1, 3}
```

As compreensões são especialmente úteis porque combinam com o foco de Komodo: escrever transformações sobre estruturas pequenas com pouco código.

---

## Parte 11 — Funções como Valores

[⬆️ Voltar ao Sumário](#sumário)

Funções são valores em Komodo.

Função anônima:

```komodo
x -> x * 2
```

Guardar função em valor:

```komodo
let double := x -> x * 2
double(10)
```

Função com bloco:

```komodo
let inspect := value ->
    println(value)
    value
```

Funções podem ser passadas para outras funções:

```komodo
from utils import map

[1, 2, 3].map(x -> x * 10)
```

Essa combinação torna Komodo confortável para operações de ordem superior, mesmo com biblioteca padrão pequena.

---

## Parte 12 — Funções Nomeadas e Múltiplos Padrões

[⬆️ Voltar ao Sumário](#sumário)

Funções nomeadas podem ser definidas por padrões:

```komodo
let fib(0) := 0
let fib(1) := 1
let fib(n) := fib(n - 1) + fib(n - 2)
```

Cada definição acrescenta uma regra para o mesmo nome. A ordem e a especificidade importam: padrões mais específicos devem aparecer antes de padrões genéricos.

### Função com mais de um argumento

```komodo
let max(a, b) :=
    if a > b then a else b
```

### Anotações de tipo em padrões

```komodo
let max([first|tail]: List) :=
    max(first, max(tail))
```

Anotações ajudam o pattern matching a restringir caso, mas Komodo continua fraca em termos de enforcement geral de tipos.

### Alternativas com `||`

Versões recentes permitem casar o mesmo corpo contra mais de um padrão:

```komodo
let sum({} || []) := 0
let sum({first|tail} || [first|tail]) := first + sum(tail)
```

Esse recurso reduz duplicação quando listas e conjuntos podem ser tratados por lógica parecida.

---

## Parte 13 — Pattern Matching como Modelo Mental

[⬆️ Voltar ao Sumário](#sumário)

Pattern matching é o coração de Komodo. A ideia é escrever casos:

```text
quando a entrada tiver esta forma
  produza este resultado
```

Exemplo FizzBuzz:

```komodo
let fizzBuzz(n: Integer) :=
    case (n % 3, n % 5) do
        (0, 0) => "fizzbuzz"
        (0, _) => "fizz"
        (_, 0) => "buzz"
        _ => n
```

Padrões úteis:

| Padrão | Ideia |
|---|---|
| `0` | valor literal |
| `_` | qualquer valor ignorado |
| `name` | captura valor |
| `[first|tail]` | lista/string com cabeça e resto |
| `{some|rest}` | set com um elemento e resto |
| `(a, b)` | tupla com duas posições |
| `x: Type` | valor compatível com tipo |
| `p1 || p2` | alternativa de padrões |

O erro comum é usar pattern matching para tudo. Em alguns casos, um `if` é mais legível.

---

## Parte 14 — Destructuring e Cons Notation

[⬆️ Voltar ao Sumário](#sumário)

Destructuring aplica pattern matching em declarações.

```komodo
let data := [5, 442, 533, 2, 5334]
let [first, _, _, fourth, ..] := data

assert(first = 5)
assert(fourth = 2)
```

Símbolos importantes:

| Símbolo | Uso |
|---|---|
| `_` | ignora um valor |
| `..` | ignora o restante |
| `[first|tail]` | decompõe lista/string |
| `{some|rest}` | decompõe set |

### Cons notation

List:

```komodo
[first|tail]
```

Set:

```komodo
{some|rest}
```

Como construção:

```komodo
[0|[1, 2]]
{0|{1, 2}}
```

Como padrão:

```komodo
let sum([]) := 0
let sum([first|tail]) := first + sum(tail)
```

---

## Parte 15 — case Expressions

[⬆️ Voltar ao Sumário](#sumário)

`case` permite usar pattern matching sem criar uma função nomeada.

```komodo
let classify(n) :=
    case n % 2 do
        0 => "even"
        1 => "odd"
```

Com tupla:

```komodo
let response(status, body) :=
    case (status, body) do
        (200, _) => "ok"
        (404, _) => "not found"
        (_, "") => "empty"
        _ => "other"
```

Regra: o primeiro padrão compatível vence. Coloque casos específicos antes do caso genérico `_`.

`case` é bom quando:

- a decisão depende da forma do valor;
- há múltiplos casos discretos;
- a alternativa a isso seria uma cadeia ruidosa de `if`;
- você não quer nomear uma função auxiliar.

---

## Parte 16 — if, for e Controle de Fluxo

[⬆️ Voltar ao Sumário](#sumário)

Komodo prefere expressões e padrões, mas também possui `if` e `for`.

### `if`

`if` é expressão e exige `else`.

```komodo
let parityMessage(n: Integer) :=
    if n % 2 = 0 then
        String(n) + " is even"
    else
        String(n) + " is odd"
```

Não há `if` incompleto. Como ele retorna valor, os dois caminhos precisam existir.

### `for`

`for` percorre valores:

```komodo
for k in 0..5 do
    println(k)
```

Forma curta:

```komodo
for k in 0..5 do println(k)
```

`for` retorna `()`. Na versão documentada, não há `break` nem `continue`.

---

## Parte 17 — Mutabilidade com var

[⬆️ Voltar ao Sumário](#sumário)

Komodo é imutável por padrão.

```komodo
let x := 1
```

Para mutabilidade:

```komodo
var sum := 0

for i in 0..10 do
    sum := sum + i
```

### Limitações

Funções formam barreiras importantes para estado mutável. Uma variável mutável declarada fora da função não pode ser alterada de dentro dela de forma livre.

Modelo mental:

```text
let
  valor imutável
  preferido para código declarativo

var
  valor reatribuível
  útil em loops e acumuladores
  limitado por escopo
```

Use `var` quando a alternativa declarativa piorar muito a leitura.

---

## Parte 18 — Imports, Módulos e Escopo

[⬆️ Voltar ao Sumário](#sumário)

Komodo permite importar nomes específicos de arquivos e módulos.

### Importar arquivo local

```komodo
from "./fib.komodo" import fib

fib(10)
```

Vários nomes:

```komodo
from "./math_extra.komodo" import (square, cube)
```

### Importar standard library

```komodo
from utils import map
from math import sqrt
```

### Escopo de imports

Imports são específicos ao bloco:

```komodo
let f(x) :=
    from "./helpers.komodo" import helper
    helper(x)
```

`helper` não fica disponível fora do corpo de `f`.

### O que não funciona

A documentação atual avisa que importar um arquivo inteiro com `import foo` ainda não é suportado. Use `from module import name`.

---

## Parte 19 — Biblioteca Padrão

[⬆️ Voltar ao Sumário](#sumário)

A biblioteca padrão de Komodo é pequena. Parte dela está escrita em Komodo (`std/utils.komodo`, `std/math.komodo`) e parte aparece implementada no interpretador (`json`, `time`).

### `utils`

Funções comuns:

| Função | Uso |
|---|---|
| `map` | transformar elementos de lista, set ou range |
| `reduce` | acumular valores |
| `fold` | reduzir sem valor inicial explícito |
| `filter` | manter elementos que passam em predicado |
| `sum` | somar container |
| `prod` | multiplicar container |
| `some` | algum elemento satisfaz predicado |
| `every` | todos satisfazem predicado |
| `indexOf` | índice de valor em lista, ou `()` se não encontrar |
| `inspect` | imprime e retorna o valor |

Exemplo:

```komodo
from utils import (map, filter, sum)

let values := [1, 2, 3, 4]
let doubled := values.map(x -> x * 2)
let evens := doubled.filter(x -> x % 2 = 0)

evens.sum()
```

### `math`

Documentação e release notes citam:

- trigonométricas;
- exponenciais;
- raízes;
- arredondamento;
- `abs`;
- `hypot`;
- constantes/funções como `PI`/`pi`.

Exemplo:

```komodo
from math import sqrt

sqrt(9)
```

### `json`

```komodo
from json import (parse, stringify)

let data := parse("{\"x\": 1}")
stringify(data)
```

### `time`

```komodo
from time import (time, sleep)

let start := time()
sleep(1)
time() - start
```

Como o projeto é instável, confira a release usada e teste no REPL.

---

## Parte 20 — Pseudo-Métodos e Estilo de Chamada

[⬆️ Voltar ao Sumário](#sumário)

Os exemplos oficiais usam chamadas como:

```komodo
[1, 2, 3].map(x -> x * 2)
```

Isso deve ser lido como conveniência sintática sobre função importada, não como método de classe em orientação a objetos tradicional.

Também há acesso por ponto em dicionários com chaves string:

```komodo
let data := {"value" => 10}
data.value
```

E há dicionários contendo funções:

```komodo
let obj := {
    "answer" => () -> 42
}

obj.answer()
```

Regra mental:

| Sintaxe | Interpretação segura |
|---|---|
| `data.key` | acesso a chave string em dicionário |
| `list.map(fn)` | chamada conveniente de função de biblioteca |
| `obj.fn()` | função guardada em dicionário ou estilo pseudo-OOP |

Não projete Komodo como se houvesse classes, interfaces ou herança. A linguagem atual não documenta esse modelo.

---

## Parte 21 — Ferramentas: REPL, Arquivos e VS Code

[⬆️ Voltar ao Sumário](#sumário)

### REPL

Use para testar expressão curta:

```bash
komodo
```

Bom para:

- operadores;
- pattern matching pequeno;
- casts;
- imports simples;
- comportamento de biblioteca padrão.

### Arquivos

Use `.komodo` para exemplos persistentes:

```bash
komodo exemplo.komodo
```

### VS Code/VSCodium

Há extensão oficial para syntax highlighting. A documentação atual descreve a extensão como simples: ela adiciona destaque de sintaxe.

### Playground web

Release notes de `v0.5.0` indicam que o interpretador voltou a compilar para Wasm e que o playground web voltou a funcionar.

### Desenvolvimento do interpretador

O repositório usa Rust para o core e possui setup com Nix. Para contribuir no interpretador, o caminho real passa pelo repositório, testes e Makefile, não apenas pelo livro.

---

## Parte 22 — Testes, assert e Exemplos

[⬆️ Voltar ao Sumário](#sumário)

Komodo possui `assert`.

```komodo
assert(1 + 1 = 2)
assert(fib(10) = 55, "fib(10) deveria ser 55")
```

Se a condição não for `true`, a execução falha.

### Estilo de exemplos

O repositório oficial usa arquivos `.komodo` com `assert` para validar comportamento. Isso é uma boa prática para estudar:

```komodo
let reverse("") := ""
let reverse([first|tail]: String) := reverse(tail) + first

assert(reverse("foo") = "oof")
```

### Projeto de prática

Crie uma pasta:

```text
komodo-lab/
  fib.komodo
  collections.komodo
  pattern_matching.komodo
  imports.komodo
```

Em cada arquivo:

- escreva funções pequenas;
- adicione `assert`;
- rode com `komodo arquivo.komodo`;
- refatore para usar pattern matching ou biblioteca padrão.

---

## Parte 23 — Limitações, Performance e Uso Realista

[⬆️ Voltar ao Sumário](#sumário)

A documentação oficial é direta: Komodo é lenta atualmente e isso limita casos de uso.

Limitações importantes:

| Limitação | Consequência |
|---|---|
| linguagem instável | código pode quebrar entre versões |
| performance baixa | não usar para workload pesado |
| ecossistema pequeno | pouca biblioteca pronta |
| tipos customizados ausentes | modelagem de domínio fica limitada |
| sem classes/interfaces | não é OO tradicional |
| `for` sem `break`/`continue` | alguns algoritmos ficam menos naturais |
| import de módulo inteiro ausente | imports precisam ser explícitos |
| tooling inicial | editor tem destaque de sintaxe, não IDE completa |

Use Komodo quando o benefício expressivo supera essas limitações:

- exercícios;
- ensino;
- prototipação;
- exploração de algoritmos;
- estudo de pattern matching;
- comparação entre paradigmas.

---

## Parte 24 — Evolução, Versões e Compatibilidade

[⬆️ Voltar ao Sumário](#sumário)

Resumo de evolução recente segundo changelog e releases:

| Versão | Mudanças relevantes |
|---|---|
| `v0.1.0` | `var`, cons notation, blocos por indentação |
| `0.2.0` | biblioteca padrão com `utils`, `KOMODO_STD`, correções de imports |
| `0.3.0` | notação de dicionário semelhante a objeto |
| `0.4.0` | mutabilidade em loops, destructuring em loops, `||` em padrões, `math`, `json`, `time` |
| `0.4.1` | correções em `json.stringify` e pattern matching de listas |
| `v0.5.0` | correção de list-cons com set, `inspect`, parse de string em `Integer`, binário estaticamente linkado, Wasm/playground |

Critérios antes de depender de uma versão:

```text
[ ] Fixei a versão do binário.
[ ] Testei exemplos com `assert`.
[ ] Li release notes.
[ ] Evitei usar comportamento não documentado.
[ ] Tenho plano para atualizar exemplos se a linguagem mudar.
```

---

## Parte 25 — Catálogo Prático da Linguagem

[⬆️ Voltar ao Sumário](#sumário)

### 25.1 Palavras-chave

| Palavra | Uso |
|---|---|
| `as` | alias em import |
| `do` | parte de `for` e `case` |
| `else` | ramo obrigatório de `if` |
| `false` | booleano falso |
| `for` | laço |
| `from` | import seletivo |
| `if` | expressão condicional |
| `import` | import de módulo/nome |
| `in` | membership |
| `let` | declaração imutável |
| `then` | parte de `if` |
| `true` | booleano verdadeiro |
| `var` | declaração mutável |

### 25.2 Operadores principais

| Operador | Uso |
|---|---|
| `:=` | atribuição/declaração |
| `=>` | ramo de `case` e par de dict |
| `->` | função anônima |
| `..` | range |
| `||` | OR lógico ou alternativa de padrão |
| `&&` | AND lógico |
| `!` | negação lógica |
| `=` | igualdade |
| `/=` | desigualdade |
| `<`, `<=`, `>`, `>=` | comparação |
| `+`, `-`, `*`, `/`, `%`, `**` | aritmética |
| `//` | fração |
| `&`, `^`, `~`, `<<`, `>>` | bits |
| `in` | membership |
| `[first|tail]` | cons de lista/string |
| `{some|rest}` | cons de set |

### 25.3 Formas de dados

```komodo
()
(1, "x")
[1, 2, 3]
{1, 2, 3}
{"name" => "Ada"}
"texto"
'x'
0..10
```

### 25.4 Builtins

| Função | Papel |
|---|---|
| `print` | imprime sem quebra de linha |
| `println` | imprime com quebra de linha |
| `getln` | lê linha da entrada padrão |
| `assert` | valida condição |
| `Integer` | converte para inteiro |
| `Float` | converte para float |
| `List` | converte set/range para lista |
| `Set` | converte list/range para set |
| `String` | representação textual |
| `len` | tamanho de lista ou set |
| `sorted` | lista ordenada |

---

## Parte 26 — Comparações e Critérios de Adoção

[⬆️ Voltar ao Sumário](#sumário)

Komodo pode ser comparada por intenção, não por maturidade.

| Linguagem | Comparação honesta |
|---|---|
| Python | Python é geral e maduro; Komodo é menor e mais focada em padrões/estruturas discretas |
| JavaScript | JS tem ecossistema enorme; Komodo evita parte da complexidade, mas não compete em plataforma |
| Prolog/Erlang/Picat | Komodo compartilha gosto por padrões e cons notation, mas não é equivalente a esses runtimes |
| Wolfram Language | Komodo tem inspiração expressiva, mas não possui o ecossistema matemático/simbólico da Wolfram |
| SETL | aproximação conceitual por conjuntos e estruturas discretas |

Adote Komodo se:

- você quer estudar ideias de linguagem;
- o problema cabe em protótipo pequeno;
- pattern matching deixa o código mais claro;
- performance não é requisito;
- instabilidade é aceitável.

Não adote se:

- precisa de estabilidade;
- precisa de bibliotecas maduras;
- precisa de interoperabilidade ampla;
- precisa de deploy confiável;
- precisa de tooling de produção.

---

## Anexo A — Trilhas de Estudo e Prática

[⬆️ Voltar ao Sumário](#sumário)

### Trilha A1 — Primeira tarde

1. Instale Komodo ou use o REPL.
2. Execute `println("Hello, World!")`.
3. Teste números, strings, listas e sets.
4. Escreva `sum` recursivo com `[first|tail]`.
5. Escreva `fizzBuzz` com `case`.
6. Use `assert` em todos os exemplos.

### Trilha A2 — Pattern matching

1. Reimplemente `reverse`.
2. Faça destructuring de listas.
3. Use `_` e `..`.
4. Escreva uma função com alternativas `||`.
5. Transforme uma cadeia de `if` em `case`.
6. Depois transforme de volta quando `if` ficar mais claro.

### Trilha A3 — Biblioteca e módulos

1. Crie `fib.komodo`.
2. Importe `fib` em outro arquivo.
3. Use `from utils import (map, filter, reduce)`.
4. Use `from math import sqrt`.
5. Teste `json.parse` e `json.stringify`.
6. Crie uma suíte simples de `assert`.

### Projeto final sugerido

Implemente um analisador pequeno de palavras:

- contar frequência de caracteres;
- remover caracteres repetidos;
- encontrar prefixos;
- serializar resultado em JSON;
- usar funções pequenas;
- usar pattern matching para casos base;
- usar `assert` para exemplos conhecidos.

---

## Anexo B — Referências Oficiais Consultadas

[⬆️ Voltar ao Sumário](#sumário)

### Documentação e código

- [The Komodo Programming Language](https://komodo-lang.org/book/)
- [Komodo GitHub repository](https://github.com/danilopedraza/komodo)
- [Komodo releases](https://github.com/danilopedraza/komodo/releases)
- [Komodo examples](https://github.com/danilopedraza/komodo/tree/main/examples)
- [Komodo standard library](https://github.com/danilopedraza/komodo/tree/main/std)

### Ferramentas

- [Komodo docs — VSCode Extension](https://komodo-lang.org/book/using_komodo/vscode_extension.html)
- [OpenVSX — Komodo extension](https://open-vsx.org/extension/komodo/komodo-analyzer)
- [Rust installation](https://www.rust-lang.org/learn/get-started)

### Inspirações citadas pela documentação oficial

- [Picat](https://picat-lang.org/)
- SETL
- [Wolfram Language](https://www.wolfram.com/language/)

---

## Glossário

[⬆️ Voltar ao Sumário](#sumário)

| Termo | Definição resumida |
|---|---|
| anonymous function | função sem nome, escrita com `->` |
| `assert` | builtin que falha quando condição não é verdadeira |
| builtin | função ou tipo disponível sem import |
| `case` | expressão de pattern matching local |
| cons notation | forma `[first|tail]` ou `{some|rest}` para construir/decompor coleções |
| destructuring | declaração que extrai partes de um valor por padrão |
| dict | coleção de pares chave-valor |
| expression-oriented | modelo em que quase toda construção retorna valor |
| fraction | número racional escrito com `//` |
| function pattern | regra de função associada a uma forma de entrada |
| `let` | declaração imutável |
| memoization | cache de resultados de função, ativado com `memoize` em definições |
| pattern matching | comparação estrutural de valor contra padrão |
| pseudo-método | chamada conveniente como `lista.map(fn)` sem classes tradicionais |
| range | intervalo escrito com `..`, com fim exclusivo |
| REPL | ambiente interativo read-eval-print-loop |
| set | coleção sem repetição e sem ordem confiável |
| standard library | módulos importáveis como `utils`, `math`, `json` e `time` |
| tuple | agrupamento ordenado, incluindo `()` como valor vazio |
| `var` | declaração reatribuível, com limitações de escopo |
| weak typing | ausência de regras rígidas de tipo antes de chamadas, com validações em runtime/padrões |

---

> **Encerramento:** Komodo é uma boa linguagem para treinar o olhar para expressões, padrões e estruturas discretas. Use-a como laboratório: escreva pouco, teste muito, leia os padrões com calma e lembre que a instabilidade faz parte do território.
