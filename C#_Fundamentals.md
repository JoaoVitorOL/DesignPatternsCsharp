# 🔷 Guia Técnico: C# do Zero ao Avançado

> **Nível:** Zero ao Avançado
> **Linguagem:** C# (C-Sharp)
> **Fonte de referência principal:** [Microsoft Learn — C#](https://learn.microsoft.com/en-us/dotnet/csharp/)
> **Versão de referência:** C# 14 / .NET 10 (LTS), com observações compatíveis com .NET 8+ quando relevante
> **Atualizado em:** 24/06/2026

---

## Prefácio

[⬆️ Voltar ao Sumário](#sumário)

Este guia nasceu para resolver um problema comum: muita gente aprende C# decorando palavras-chave, nomes de tipos e pequenas receitas de código, mas sem construir um modelo mental sólido da linguagem. O resultado é que a pessoa até consegue escrever alguns exemplos, porém trava quando precisa projetar software de verdade, ler código de produção, depurar comportamento inesperado ou decidir entre duas abordagens aparentemente válidas.

O objetivo aqui é transformar o arquivo em algo mais próximo de um livro técnico do que de uma folha de cola. Em vez de apenas listar recursos, a ideia é explicar por que cada recurso existe, que problema ele resolve, qual o custo de usá-lo e quais erros aparecem com frequência em code review e manutenção de sistemas reais.

Este material não substitui a documentação oficial da Microsoft. Pelo contrário: ele foi revisado à luz do Microsoft Learn e usa a documentação oficial como fonte primária para consolidar definições, comportamento da linguagem e versão de referência. Pense neste guia como uma ponte entre a teoria oficial e a prática de engenharia.

Bons estudos.

---

## Como usar este guia

[⬆️ Voltar ao Sumário](#sumário)

Você pode ler este material de três formas:

1. **Trilha júnior:** leia na ordem, faça os exemplos, e priorize entender o modelo mental antes de memorizar sintaxe.
2. **Trilha pleno:** use o guia como referência de design e revisão de código; foque em tipos, coleções, interfaces, LINQ, async e tratamento de exceções.
3. **Trilha sênior:** leia procurando trade-offs, custo de abstração, semântica de execução, efeitos de performance e impacto arquitetural.

Ao longo do texto, pense sempre nestas quatro perguntas:

- **O que isso representa?** Conceito ou contrato.
- **Quando eu usaria isso?** Cenário prático.
- **O que isso custa?** Complexidade, alocação, acoplamento ou legibilidade.
- **Qual erro é comum aqui?** Armadilha de engenharia.

---

## Sumário

- [Como usar este guia](#como-usar-este-guia)
- [Parte 1 — Introdução e Contextualização](#parte-1-introdução-e-contextualização)
  - [1.1 O que é C#?](#11-o-que-é-c)
  - [1.2 Por que aprender C# em 2026?](#12-por-que-aprender-c-em-2026)
  - [1.3 Estrutura de um programa C#](#13-estrutura-de-um-programa-c)
- [Parte 2 — Namespaces e Using](#parte-2-namespaces-e-using)
  - [2.1 Namespaces](#21-namespaces)
  - [2.2 Using Directives](#22-using-directives)
- [Parte 3 — Variáveis e Tipos](#parte-3-variáveis-e-tipos)
  - [3.1 O que é uma variável?](#31-o-que-é-uma-variável)
  - [3.2 Tipos de valor vs tipos de referência](#32-tipos-de-valor-vs-tipos-de-referência)
  - [3.3 Nullable Types — tipos que aceitam null](#33-nullable-types-tipos-que-aceitam-null)
  - [3.4 `var` — inferência de tipo](#34-var-inferência-de-tipo)
  - [3.5 `const` e `readonly`](#35-const-e-readonly)
- [Parte 4 — String e suas Peculiaridades](#parte-4-string-e-suas-peculiaridades)
  - [4.1 String é um tipo de referência imutável](#41-string-é-um-tipo-de-referência-imutável)
  - [4.2 Imutabilidade e StringBuilder](#42-imutabilidade-e-stringbuilder)
  - [4.3 String Interpolation e verbatim strings](#43-string-interpolation-e-verbatim-strings)
  - [4.4 Métodos importantes de string](#44-métodos-importantes-de-string)
  - [4.5 Classes e funções predefinidas essenciais do .NET](#45-classes-e-funções-predefinidas-essenciais-do-net)
- [Parte 5 — Modificadores de Acesso](#parte-5-modificadores-de-acesso)
  - [5.1 Os modificadores de acesso do C#](#51-os-modificadores-de-acesso-do-c)
  - [5.2 Boas práticas com modificadores](#52-boas-práticas-com-modificadores)
- [Parte 6 — Propriedades (Properties)](#parte-6-propriedades-properties)
  - [6.1 O que são Properties?](#61-o-que-são-properties)
  - [6.2 Expression-bodied members](#62-expression-bodied-members)
- [Parte 7 — Palavras-chave Especiais do C#](#parte-7-palavras-chave-especiais-do-c)
  - [7.1 `static`](#71-static)
  - [7.2 `sealed`](#72-sealed)
  - [7.3 `abstract`](#73-abstract)
  - [7.4 `virtual` e `override`](#74-virtual-e-override)
  - [7.5 `this` e `base`](#75-this-e-base)
  - [7.6 `is`, `as` e Pattern Matching](#76-is-as-e-pattern-matching)
  - [7.7 `using` para gerenciamento de recursos](#77-using-para-gerenciamento-de-recursos)
  - [7.8 `ref`, `out` e `in`](#78-ref-out-e-in)
- [Parte 8 — Controle de Fluxo](#parte-8-controle-de-fluxo)
  - [8.1 `if / else if / else`](#81-if-else-if-else)
  - [8.2 `switch` e switch expressions](#82-switch-e-switch-expressions)
  - [8.3 Loops](#83-loops)
- [Parte 9 — Métodos](#parte-9-métodos)
  - [9.1 Declaração de métodos](#91-declaração-de-métodos)
  - [9.2 Métodos de extensão (Extension Methods)](#92-métodos-de-extensão-extension-methods)
  - [9.3 Sobrecarga de métodos](#93-sobrecarga-de-métodos)
- [Parte 10 — Enums](#parte-10-enums)
  - [10.1 Enums básicos](#101-enums-básicos)
  - [10.2 Flags enum — bitmask](#102-flags-enum-bitmask)
- [Parte 11 — Classes e Objetos](#parte-11-classes-e-objetos)
  - [11.1 Estrutura completa de uma classe](#111-estrutura-completa-de-uma-classe)
  - [11.2 Construtores em Profundidade](#112-construtores-em-profundidade)
  - [11.3 Records (C# 9+)](#113-records-c-9)
  - [11.4 Padrão Builder](#114-padrão-builder)
- [Parte 12 — Herança e Polimorfismo](#parte-12-herança-e-polimorfismo)
  - [12.1 Herança em C#](#121-herança-em-c)
  - [12.2 Interfaces](#122-interfaces)
- [Parte 13 — Delegates, Events e Lambdas](#parte-13-delegates-events-e-lambdas)
  - [13.1 Delegates — ponteiros de método tipados](#131-delegates-ponteiros-de-método-tipados)
  - [13.2 Func, Action e Predicate](#132-func-action-e-predicate)
  - [13.3 Expressões Lambda](#133-expressões-lambda)
  - [13.4 Eventos (Events)](#134-eventos-events)
- [Parte 14 — LINQ (Language Integrated Query)](#parte-14-linq-language-integrated-query)
  - [14.1 O que é LINQ?](#141-o-que-é-linq)
  - [14.2 Operadores LINQ principais](#142-operadores-linq-principais)
  - [14.3 `IEnumerable<T>` e o contrato fundamental das sequências](#143-ienumerablet-e-o-contrato-fundamental-das-sequências)
  - [14.4 `IQueryable<T>` e queries traduzíveis para outra fonte](#144-iqueryablet-e-queries-traduzíveis-para-outra-fonte)
  - [14.5 Execução adiada, materialização e armadilhas](#145-execução-adiada-materialização-e-armadilhas)
- [Parte 15 — Coleções](#parte-15-coleções)
  - [15.1 Tipos de coleções principais](#151-tipos-de-coleções-principais)
  - [15.2 List\<T\>](#152-listt)
  - [15.3 Dictionary\<TKey, TValue\>](#153-dictionarytkey-tvalue)
  - [15.4 Como escolher a coleção certa](#154-como-escolher-a-coleção-certa)
- [Parte 16 — Async/Await e Programação Assíncrona](#parte-16-asyncawait-e-programação-assíncrona)
  - [16.1 O modelo assíncrono do C#](#161-o-modelo-assíncrono-do-c)
  - [16.2 Padrões de uso](#162-padrões-de-uso)
  - [16.3 Task vs ValueTask](#163-task-vs-valuetask)
- [Parte 17 — Generics](#parte-17-generics)
  - [17.1 Tipos parametrizados](#171-tipos-parametrizados)
  - [17.2 Constraints (restrições)](#172-constraints-restrições)
  - [17.3 Covariância e contravariância](#173-covariância-e-contravariância)
- [Parte 18 — Tratamento de Exceções](#parte-18-tratamento-de-exceções)
  - [18.1 `try / catch / finally`](#181-try-catch-finally)
  - [18.2 Exceções customizadas](#182-exceções-customizadas)
  - [18.3 Hierarquia de exceções](#183-hierarquia-de-exceções)
- [Parte 19 — Attributes (Annotations)](#parte-19-attributes-annotations)
  - [19.1 Attributes embutidos](#191-attributes-embutidos)
  - [19.2 Criando Attributes customizados](#192-criando-attributes-customizados)
- [Parte 20 — Tipos Especiais Modernos do C#](#parte-20-tipos-especiais-modernos-do-c)
  - [20.1 Tuple e ValueTuple](#201-tuple-e-valuetuple)
  - [20.2 Span\<T\> e Memory\<T\> — zero-allocation slicing](#202-spant-e-memoryt-zero-allocation-slicing)
  - [20.3 Sealed classes com Pattern Matching (como Discriminated Union)](#203-sealed-classes-com-pattern-matching-como-discriminated-union)
- [Parte 21 — Threads e Concorrência](#parte-21-threads-e-concorrência)
  - [21.1 Thread básico e ThreadPool](#211-thread-básico-e-threadpool)
  - [21.2 Task Parallel Library (TPL)](#212-task-parallel-library-tpl)
  - [21.3 Sincronização](#213-sincronização)
- [Parte 22 — Interoperabilidade e Recursos Avançados](#parte-22-interoperabilidade-e-recursos-avançados)
  - [22.1 Reflection](#221-reflection)
  - [22.2 Source Generators (C# 9+)](#222-source-generators-c-9)
  - [22.3 Unsafe code e ponteiros](#223-unsafe-code-e-ponteiros)
- [Resumo Geral — Conceitos Fundamentais](#resumo-geral-conceitos-fundamentais)
- [Parte 23 — C# no Contexto de Game Development](#parte-23-c-no-contexto-de-game-development)
  - [23.1 C# e Unity — a combinação dominante](#231-c-e-unity-a-combinação-dominante)
  - [23.2 MonoBehaviour — a classe base dos scripts Unity](#232-monobehaviour-a-classe-base-dos-scripts-unity)
  - [23.3 Ciclo de vida do MonoBehaviour](#233-ciclo-de-vida-do-monobehaviour)
  - [23.4 ScriptableObject — dados desacoplados do GameObject](#234-scriptableobject-dados-desacoplados-do-gameobject)
  - [23.5 Coroutines — execução assíncrona sem async/await](#235-coroutines-execução-assíncrona-sem-asyncawait)
  - [23.6 Unity Events e C# Events](#236-unity-events-e-c-events)
  - [23.7 Boas práticas de performance no Unity](#237-boas-práticas-de-performance-no-unity)
  - [23.8 Padrões de design comuns em jogos com C#](#238-padrões-de-design-comuns-em-jogos-com-c)
  - [23.9 C# no Godot 4](#239-c-no-godot-4)
  - [23.10 Diferenças entre C# padrão e C# no Unity](#2310-diferenças-entre-c-padrão-e-c-no-unity)
- [Anexo A — Plataformas de Prática Recomendadas](#anexo-a--plataformas-de-prática-recomendadas)
- [Anexo B — Referências Oficiais Consultadas](#anexo-b--referências-oficiais-consultadas)
- [Glossário](#glossário)

---

## Parte 1 — Introdução e Contextualização

[⬆️ Voltar ao Sumário](#sumário)

---

### 1.1 O que é C#?

[⬆️ Voltar ao Sumário](#sumário)

C# é uma **linguagem de programação orientada a objetos, estaticamente tipada, de propósito geral e fortemente tipada**, criada pela Microsoft em 2000, liderada por Anders Hejlsberg (o mesmo criador do Delphi e TypeScript). É a linguagem principal do ecossistema **.NET**.

Assim como Java, C# compila para um formato intermediário — o **IL (Intermediate Language)**, também chamado de **CIL (Common Intermediate Language)** — que é executado sobre o **CLR (Common Language Runtime)**, a máquina virtual do .NET. O mesmo princípio "escreva uma vez, execute em qualquer lugar" se aplica dentro do ecossistema .NET.

```
Código C# (.cs)
        ↓  compilador (csc / Roslyn)
    IL / CIL (assembly .dll ou .exe)
        ↓  CLR — JIT (Just-In-Time) compila em tempo de execução
  Instruções nativas do sistema operacional
```

O compilador moderno do C# é chamado **Roslyn** e é open-source. O runtime moderno é o **.NET** (anteriormente chamado .NET Core), enquanto o **.NET Framework** é a versão legada, exclusiva do Windows.

**Como interpretar o exemplo:** O diagrama mostra que o codigo C# nao roda direto no sistema operacional; ele passa por compilacao para IL e depois pelo runtime do .NET. Esse fluxo explica por que o ecossistema consegue oferecer coleta de lixo, verificacao de tipos, tratamento consistente de excecoes e portabilidade entre plataformas.

---

### 1.2 Por que aprender C# em 2026?

[⬆️ Voltar ao Sumário](#sumário)

Em **24 de junho de 2026**, a Microsoft lista **.NET 10 (LTS)**, **.NET 9** e **.NET 8 (LTS)** como versões suportadas, e a documentação oficial marca o **C# 14** como a versão mais recente da linguagem. Isso importa porque C# não é uma linguagem parada: ela evolui sem perder compatibilidade com os fundamentos. Você investe no núcleo da linguagem e continua aproveitando esse conhecimento por muitos anos.

C# tem um escopo extremamente amplo:

| Contexto | Ferramentas/Frameworks |
|---|---|
| **Game Development** | Unity (motor mais usado no mundo), Godot (suporte C#) |
| **Web Backend** | ASP.NET Core — alta performance, usado por empresas como Stack Overflow |
| **Desktop (Windows)** | WPF, WinUI, MAUI |
| **Mobile** | .NET MAUI (cross-platform iOS/Android) |
| **Cloud / Serverless** | Azure Functions, AWS Lambda (.NET) |
| **APIs REST e gRPC** | ASP.NET Core Web API |
| **Machine Learning** | ML.NET |
| **IoT** | .NET para dispositivos embarcados |

C# continua sendo uma linguagem excelente para quem quer combinar **fundamentos fortes de engenharia** com **mercado amplo**. Ela tem um sistema de tipos maduro, uma biblioteca padrão extensa, tooling profissional e uma curva de crescimento muito boa: dá para começar com console apps simples e chegar em APIs distribuídas, engines de jogos, processamento assíncrono, tooling, automação e bibliotecas de alta performance.

**Como interpretar o exemplo:** A tabela nao serve apenas para listar mercados; ela mostra que a mesma base da linguagem reaparece em contextos muito diferentes, do backend ao desenvolvimento de jogos. Isso significa que estudar fundamentos de C# rende em varias areas ao mesmo tempo, mesmo quando o framework muda.

---

### 1.3 Estrutura de um programa C#

[⬆️ Voltar ao Sumário](#sumário)

**Top-level statements (C# 9+)** — forma moderna e simplificada:

```csharp
// Arquivo: Program.cs
// A partir do C# 9, o ponto de entrada pode ser escrito diretamente,
// sem a classe Program e o método Main explícitos.

Console.WriteLine("Olá, Mundo!");
```

**Forma clássica** — necessária para entender projetos legados e estruturas mais complexas:

```csharp
// Todo arquivo C# pode declarar um namespace para organizar o código.
namespace MeuProjeto;

// A classe Program é o contêiner tradicional do ponto de entrada.
public class Program
{
    // O método Main é o ponto de entrada clássico.
    // 'static'      → pertence à classe, não a uma instância
    // 'void'        → não retorna valor (pode retornar int para código de saída)
    // 'string[] args' → argumentos da linha de comando
    public static void Main(string[] args)
    {
        // Console.WriteLine escreve no fluxo de saída padrão e pula uma linha.
        Console.WriteLine("Olá, Mundo!");
    }
}
```

**Componentes estruturais:**

| Elemento | O que é | Por que existe |
|---|---|---|
| `namespace` | Agrupamento lógico de tipos | Evita colisões de nome entre bibliotecas |
| `class NomeDaClasse` | Declaração de tipo | Todo código vive dentro de um tipo em C# |
| `static void Main` | Método de entrada | O CLR precisa de um ponto de partida conhecido |
| Ponto e vírgula `;` | Delimitador de instrução | Indica o fim de cada instrução |
| Chaves `{}` | Delimitador de escopo | Define onde começa e termina um bloco |

**Como interpretar o exemplo:** Os dois formatos existem porque a linguagem evoluiu para reduzir verbosidade sem abandonar compatibilidade com projetos antigos. Entender tanto `top-level statements` quanto a forma classica com `Program` e `Main` ajuda voce a ler codigo moderno e legado com a mesma naturalidade.

---

## Parte 2 — Namespaces e Using

[⬆️ Voltar ao Sumário](#sumário)

---

### 2.1 Namespaces

[⬆️ Voltar ao Sumário](#sumário)

Um **namespace** é um contêiner lógico que agrupa tipos relacionados, evitando conflitos de nome entre bibliotecas diferentes.

```csharp
// Declaração de namespace — estilo moderno (C# 10+, file-scoped)
namespace MinhaEmpresa.Projeto.Modelos;

public class Usuario { }

// Declaração de namespace — estilo clássico (com chaves)
namespace MinhaEmpresa.Projeto.Servicos
{
    public class UsuarioServico { }
}
```

Por convenção, namespaces seguem a estrutura: `Empresa.Produto.Camada`:

```
MinhaEmpresa.App.Modelos
MinhaEmpresa.App.Servicos
MinhaEmpresa.App.Repositorios
Microsoft.AspNetCore.Mvc
System.Collections.Generic
```

**Como interpretar o exemplo:** O namespace funciona como endereco logico dos tipos, e nao apenas como enfeite visual. Uma boa estrutura de namespaces comunica dominio, camada e responsabilidade antes mesmo de voce abrir a implementacao da classe.

---

### 2.2 Using Directives

[⬆️ Voltar ao Sumário](#sumário)

Para usar um tipo de outro namespace, utilize `using`.

```csharp
// Import de namespace específico
using System;
using System.Collections.Generic;
using System.Linq;

// Alias para namespace ou tipo
using Inteiros = System.Collections.Generic.List<int>;

// Using estático — importa membros estáticos diretamente
using static System.Math;
using static System.Console;

// Global using (C# 10+) — aplicado a todos os arquivos do projeto
global using System;
global using System.Collections.Generic;

public class Exemplo
{
    public void Metodo()
    {
        // Com 'using static System.Math':
        double raiz = Sqrt(16);   // sem prefixo Math.
        double pi   = PI;

        // Com 'using static System.Console':
        WriteLine("Sem Console.");
    }
}
```

> ⚠️ Projetos .NET modernos (SDK-style) incluem `global using` implícitos para namespaces comuns (`System`, `System.Collections.Generic`, etc.) quando `<ImplicitUsings>enable</ImplicitUsings>` está configurado no `.csproj`. Isso significa que muitos `using` são desnecessários em projetos novos.

**Como interpretar o exemplo:** `using` reduz ruido visual ao esconder nomes totalmente qualificados que nao agregam valor a toda leitura. Alias, `using static` e `global using` mostram que C# trata legibilidade e ergonomia como parte do desenho da linguagem.

---

## Parte 3 — Variáveis e Tipos

[⬆️ Voltar ao Sumário](#sumário)

---

### 3.1 O que é uma variável?

[⬆️ Voltar ao Sumário](#sumário)

Uma variável é um **espaço nomeado na memória** que armazena um valor. Em C#, todo valor tem um tipo definido em tempo de compilação.

```csharp
// Sintaxe: Tipo nomeDaVariavel = valor;
string nome    = "Ana";
int    idade   = 28;
double preco   = 49.90;
bool   ativo   = true;

// Inferência de tipo com 'var'
var cidade = "São Paulo"; // compilador infere: string
var total  = 150.75;      // compilador infere: double
```

**Como interpretar o exemplo:** Toda variavel em C# liga tres ideias: nome, tipo e valor atual. Mesmo com `var`, o tipo continua existindo e sendo conhecido pelo compilador; a diferenca e apenas que ele faz a inferencia para voce.

---

### 3.2 Tipos de valor vs tipos de referência

[⬆️ Voltar ao Sumário](#sumário)

Esta é a distinção fundamental do sistema de tipos do C#.

**Tipos de valor** armazenam o dado diretamente onde a variável está alocada (geralmente na stack). Quando você atribui um tipo de valor a outro, uma **cópia** é feita.

**Tipos de referência** armazenam um endereço (referência) para um objeto no heap. Quando você atribui um tipo de referência a outro, ambas as variáveis apontam para o **mesmo objeto**.

```csharp
// Tipo de valor — cópia
int a = 10;
int b = a;   // b recebe uma CÓPIA de 10
b = 20;
Console.WriteLine(a); // ainda 10

// Tipo de referência — mesma referência
int[] listaA = { 1, 2, 3 };
int[] listaB = listaA; // listaB aponta para o MESMO array
listaB[0] = 99;
Console.WriteLine(listaA[0]); // 99 — o mesmo objeto foi modificado
```

**Tipos de valor primitivos do C#:**

| Tipo C# | Alias .NET | Tamanho | Valor padrão | Uso |
|---|---|---|---|---|
| `sbyte` | `SByte` | 8 bits | `0` | Inteiro com sinal pequeno |
| `byte` | `Byte` | 8 bits | `0` | Inteiro sem sinal pequeno / dados binários |
| `short` | `Int16` | 16 bits | `0` | Inteiro pequeno |
| `ushort` | `UInt16` | 16 bits | `0` | Inteiro sem sinal |
| `int` | `Int32` | 32 bits | `0` | **Inteiro — o mais comum** |
| `uint` | `UInt32` | 32 bits | `0` | Inteiro sem sinal |
| `long` | `Int64` | 64 bits | `0L` | Inteiros grandes (ex: IDs, timestamps) |
| `ulong` | `UInt64` | 64 bits | `0UL` | Inteiro sem sinal grande |
| `float` | `Single` | 32 bits | `0.0f` | Decimal com precisão menor |
| `double` | `Double` | 64 bits | `0.0` | **Decimal — o mais comum** |
| `decimal` | `Decimal` | 128 bits | `0.0m` | **Precisão financeira (sem erro de ponto flutuante)** |
| `char` | `Char` | 16 bits | `'\0'` | Um caractere Unicode |
| `bool` | `Boolean` | 1 bit | `false` | Verdadeiro ou falso |

> ⚠️ `decimal` é o tipo correto para cálculos monetários. `double` e `float` usam representação binária que pode acumular erros de arredondamento (`0.1 + 0.2 != 0.3`). `decimal` usa representação decimal de base 10, eliminando esse problema para a maioria dos casos financeiros.

**Structs são tipos de valor:**

```csharp
// struct — tipo de valor definido pelo usuário
struct Ponto
{
    public int X;
    public int Y;
}

Ponto p1 = new Ponto { X = 10, Y = 20 };
Ponto p2 = p1; // cópia completa
p2.X = 99;
Console.WriteLine(p1.X); // ainda 10
```

**Como interpretar o exemplo:** O que realmente importa aqui nao e decorar `stack` e `heap`, mas entender a semantica de copia e compartilhamento. Quando voce domina isso, passa a prever melhor efeitos colaterais, mutabilidade e comportamento de parametros e colecoes.

---

### 3.3 Nullable Types — tipos que aceitam null

[⬆️ Voltar ao Sumário](#sumário)

Por padrão, tipos de valor **não podem ser null**. Para permitir null em tipos de valor, use o sufixo `?`.

```csharp
int  numero  = null; // ERRO DE COMPILAÇÃO — int não aceita null
int? nullable = null; // OK — Nullable<int>

// Verificação
if (nullable.HasValue)
    Console.WriteLine(nullable.Value);

// Operador de coalescência nula — retorna o valor à direita se o da esquerda for null
int resultado = nullable ?? 0; // 0 se nullable for null

// Operador de acesso condicional (null-conditional)
string? nome = null;
int? tamanho = nome?.Length; // null se nome for null, sem NullReferenceException

// Null-forgiving operator — suprime aviso de null do compilador (use com cautela)
string valor = nome!; // diz ao compilador: "confie em mim, não é null"
```

**Nullable Reference Types (C# 8+ com `<Nullable>enable</Nullable>` no projeto):**

```csharp
// Com nullable habilitado, o compilador rastreia nullability
string  naoNula = "texto";   // nunca null — compilador avisa se atribuir null
string? podeNula = null;      // pode ser null — deve ser verificado antes de usar

void Processar(string? entrada)
{
    // Sem verificação → compilador emite aviso CS8602 (possível null dereference)
    Console.WriteLine(entrada.Length); // AVISO

    // Com verificação → compilador aceita
    if (entrada is not null)
        Console.WriteLine(entrada.Length); // OK
}
```

**Como interpretar o exemplo:** Os operadores mostrados existem para tornar a ausencia de valor visivel no contrato, em vez de deixar `null` circular de forma implicita. Em codigo profissional, isso reduz muito `NullReferenceException` e melhora a clareza das APIs.

---

### 3.4 `var` — inferência de tipo

[⬆️ Voltar ao Sumário](#sumário)

```csharp
// var é equivalente ao tipo explícito — inferido pelo compilador
var nome  = "Ana";                         // string
var lista = new List<string>();            // List<string>
var mapa  = new Dictionary<string, int>(); // Dictionary<string, int>

// var NÃO é tipagem dinâmica — o tipo é fixo em compile-time
var numero = 42;
numero = "texto"; // ERRO DE COMPILAÇÃO — número é int, não string
```

> Em C#, `var` é sempre inferência de tipo local, não equivalente ao `dynamic` (que é tipagem dinâmica em runtime). `var` e `dynamic` são conceitos completamente diferentes.

**Como interpretar o exemplo:** O exemplo desfaz a confusao mais comum: `var` nao significa tipagem dinamica, e sim tipagem estatica com inferencia local. Use `var` quando ele reduz repeticao sem esconder um tipo importante para o leitor.

---

### 3.5 `const` e `readonly`

[⬆️ Voltar ao Sumário](#sumário)

```csharp
// const — constante de compile-time; deve ser inicializada na declaração
// Apenas tipos primitivos e string são permitidos
public const double PI    = 3.14159265358979;
public const int    MAX   = 100;
public const string VERSAO = "1.0.0";

// readonly — campo que só pode ser atribuído na declaração ou no construtor
public class Configuracao
{
    public readonly string Host;
    public readonly int Porta;

    public Configuracao(string host, int porta)
    {
        Host  = host;  // OK — dentro do construtor
        Porta = porta;
    }
    // Após o construtor, Host e Porta não podem ser reatribuídos
}

// readonly permite tipos complexos (objetos, arrays)
private readonly List<string> _itens = new();
// _itens = new List<string>(); // ERRO — reatribuição da referência
// _itens.Add("item");          // OK — modifica o objeto, não a referência
```

**Diferença entre `const` e `readonly`:**

| Característica | `const` | `readonly` |
|---|---|---|
| Quando é resolvido | Compile-time | Runtime |
| Tipos permitidos | Primitivos e `string` | Qualquer tipo |
| Onde pode ser inicializado | Apenas na declaração | Declaração ou construtor |
| Pode ser `static` | Sempre é `static` implicitamente | Pode ser `static` ou de instância |

**Como interpretar o exemplo:** A diferenca entre os dois nao e so sintatica; ela muda tempo de resolucao, flexibilidade e evolucao da API. `const` representa valor totalmente fixo e embutido pelo compilador, enquanto `readonly` protege o campo ou referencia depois da construcao do objeto.

---

## Parte 4 — String e suas Peculiaridades

[⬆️ Voltar ao Sumário](#sumário)

---

### 4.1 String é um tipo de referência imutável

[⬆️ Voltar ao Sumário](#sumário)

Em C#, `string` é um tipo de referência (`System.String`), mas se comporta de forma especial: é **imutável** e possui otimizações de interning (similar ao String Pool do Java).

```csharp
// Comparação com == funciona por CONTEÚDO em string (diferente de Java!)
// Isso porque o operador == foi sobrescrito na classe String do C#
string a = "Java";
string b = "Java";
string c = new string("Java".ToCharArray());

Console.WriteLine(a == b);          // true — conteúdo igual
Console.WriteLine(a == c);          // true — conteúdo igual (C# faz isso por padrão)
Console.WriteLine(object.ReferenceEquals(a, c)); // false — referências diferentes

// Em C#, use == para comparar strings (ao contrário do Java, onde deve-se usar .Equals())
// Mas para comparações case-insensitive ou culturais, use StringComparison:
bool igual = string.Equals("abc", "ABC", StringComparison.OrdinalIgnoreCase); // true
```

**Como interpretar o exemplo:** `string` e um caso especial em C#: embora seja tipo de referencia, sua semantica foi desenhada para ser mais segura no uso cotidiano. Por isso `==` compara conteudo, mas comparacoes profissionais ainda devem preferir `StringComparison` quando cultura e caixa importam.

---

### 4.2 Imutabilidade e StringBuilder

[⬆️ Voltar ao Sumário](#sumário)

```csharp
using System.Text;

string nome = "Ana";
nome = nome + " Silva"; // cria outro objeto string

// Concatenação repetida em loop — muitas alocações
string resultado = "";
for (int i = 0; i < 1000; i++)
{
    resultado += i; // cria um novo objeto a cada iteração
}

// StringBuilder — buffer mutável para montagem incremental
var sb = new StringBuilder(capacity: 64);
sb.Append("Itens: ");
for (int i = 0; i < 5; i++)
{
    if (i > 0) sb.Append(", ");
    sb.Append(i);
}
string eficiente = sb.ToString();

// API mais usada no dia a dia
var builder = new StringBuilder("Olá", capacity: 32);
builder.Append(" Mundo");             // adiciona no fim
builder.Insert(3, ",");               // insere em um índice específico
builder.Replace("Mundo", "C#");       // substitui ocorrências
builder.AppendLine("!");              // adiciona texto + quebra de linha
Console.WriteLine(builder.ToString());

var relatorio = new StringBuilder(64);
relatorio.EnsureCapacity(128);        // reserva espaço para evitar realocações
relatorio.AppendLine("Resumo:");
relatorio.AppendFormat("- Total: {0:C}", 25);

Console.WriteLine(relatorio.Length);   // tamanho atual do conteúdo
Console.WriteLine(relatorio.Capacity); // espaço reservado no buffer
relatorio.Clear();                     // reaproveita o buffer
```

**Como interpretar o exemplo:** O custo da concatenação aparece porque cada mudança em `string` gera um novo objeto. `StringBuilder`, do namespace `System.Text`, mantém um buffer mutável e por isso é mais adequado para montagem repetida de texto, principalmente em laços, relatórios, HTML, logs e payloads textuais.

**Regra prática:** prefira `string` com interpolação (`$"..."`) quando o texto é pequeno e nasce quase pronto. Prefira `string.Join(...)` quando você só quer unir uma coleção. Traga `StringBuilder` quando o texto cresce pedaço por pedaço, em várias operações.

**Armadilha comum:** `Length` é o tamanho do conteúdo atual; `Capacity` é o espaço reservado no buffer. Elas não representam a mesma coisa.

> **Referência oficial:** [Microsoft Learn — Usando a classe StringBuilder no .NET](https://learn.microsoft.com/pt-br/dotnet/standard/base-types/stringbuilder)

---

### 4.3 String Interpolation e verbatim strings

[⬆️ Voltar ao Sumário](#sumário)

```csharp
string nome  = "Ana";
int    idade = 28;

// Concatenação clássica — verbosa
string msg1 = "Nome: " + nome + ", Idade: " + idade;

// String.Format — mais legível
string msg2 = string.Format("Nome: {0}, Idade: {1}", nome, idade);

// String interpolation (C# 6+) — preferida
string msg3 = $"Nome: {nome}, Idade: {idade}";

// Expressões dentro da interpolação
string msg4 = $"Nasceu em {DateTime.Now.Year - idade}. Maioridade: {(idade >= 18 ? "Sim" : "Não")}";

// Verbatim string — @ ignora caracteres de escape (útil para caminhos de arquivo)
string caminho1 = "C:\\Users\\Ana\\Documentos\\arquivo.txt"; // sem @
string caminho2 = @"C:\Users\Ana\Documentos\arquivo.txt";   // com @ — mais legível

// Verbatim + interpolação
string caminho3 = $@"C:\Users\{nome}\Documentos";

// Raw string literals (C# 11+) — sem nenhum escape necessário
string json = """
    {
        "nome": "Ana",
        "idade": 28
    }
    """;
```

**Como interpretar o exemplo:** Cada forma de escrever texto ataca um problema de legibilidade diferente. Interpolacao e melhor para misturar valores com texto; `@` ajuda com caminhos e escapes; e raw strings servem muito bem para conteudos multilinha, como JSON e templates.

---

### 4.4 Métodos importantes de string

[⬆️ Voltar ao Sumário](#sumário)

```csharp
string s = "  Olá, C#!  ";

// Inspeção
Console.WriteLine(s.Length);                  // 12
Console.WriteLine(string.IsNullOrEmpty(s));   // false
Console.WriteLine(string.IsNullOrWhiteSpace(" ")); // true
Console.WriteLine(s.Contains("C#"));          // true
Console.WriteLine(s.StartsWith("  Olá"));     // true
Console.WriteLine(s.EndsWith("!  "));         // true
Console.WriteLine(s.IndexOf("C#"));           // 6

// Transformação
Console.WriteLine(s.Trim());                  // "Olá, C#!"
Console.WriteLine(s.TrimStart());             // "Olá, C#!  "
Console.WriteLine(s.ToUpper());               // "  OLÁ, C#!  "
Console.WriteLine(s.ToLower());               // "  olá, c#!  "
Console.WriteLine(s.Replace("C#", "Java"));   // "  Olá, Java!  "

// Extração
Console.WriteLine(s.Substring(6));            // "C#!  "
Console.WriteLine(s.Substring(6, 2));         // "C#"
Console.WriteLine(s[6]);                      // 'C' — indexação direta

// Divisão
string   csv    = "Ana,Bruno,Carlos";
string[] partes = csv.Split(',');              // ["Ana", "Bruno", "Carlos"]

// Junção
string junto = string.Join(", ", "Ana", "Bruno", "Carlos"); // "Ana, Bruno, Carlos"

// Conversão
string numStr = 42.ToString();                // int → string
int    num    = int.Parse("42");              // string → int (lança exceção se inválido)
bool   ok     = int.TryParse("abc", out int val); // seguro — retorna false sem exceção
```

**Como interpretar o exemplo:** A lista de metodos fica mais facil de entender quando voce a enxerga por categoria: inspecionar, transformar, extrair e converter. Esse modelo mental ajuda inclusive a evitar erros comuns, como usar `Parse` em entrada externa quando o fluxo correto e `TryParse`.

---

### 4.5 Classes e funções predefinidas essenciais do .NET

[⬆️ Voltar ao Sumário](#sumário)

C# não é só sintaxe e palavras-chave. Pela documentação oficial da Microsoft, muitos recursos que o iniciante percebe como "funções da linguagem" são, na prática, **tipos e métodos da biblioteca base do .NET**. O namespace `System` é a raiz dos tipos fundamentais, e vários keywords do C# são apenas aliases de tipos do .NET, como `int` → `System.Int32` e `string` → `System.String`.

```csharp
using System;
using System.Globalization;
using System.IO;

Console.Write("Nome: ");
string? nomeLido = Console.ReadLine();

double raiz = Math.Sqrt(144);
int notaAjustada = Math.Clamp(150, 0, 100); // 100

DateTime agoraLocal = DateTime.Now;
DateTime agoraUtc   = DateTime.UtcNow;
DateOnly hoje       = DateOnly.FromDateTime(agoraLocal);
TimeOnly horario    = TimeOnly.FromDateTime(agoraLocal);
TimeSpan timeout    = TimeSpan.FromSeconds(30);

Guid id = Guid.NewGuid();

var random = new Random();
int dado = random.Next(1, 7); // 1 a 6

bool okNumero = int.TryParse("42", out int numero);
bool okData = DateTime.TryParseExact(
    "25/06/2026",
    "dd/MM/yyyy",
    CultureInfo.InvariantCulture,
    DateTimeStyles.None,
    out DateTime data);

string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
string arquivo = Path.Combine(desktop, "resumo.txt");
File.WriteAllText(arquivo, $"Id={id}; Dado={dado}; NumeroValido={okNumero}");
bool existe = File.Exists(arquivo);
```

| Tipo/API | Quando aparece no dia a dia | Membros comuns | Cuidado profissional |
| --- | --- | --- | --- |
| `Console` | Apps de terminal, demos, ferramentas internas | `WriteLine`, `Write`, `ReadLine`, `Error` | saída normal e erro podem seguir fluxos diferentes |
| `Math` | Cálculos numéricos | `Abs`, `Clamp`, `Round`, `Min`, `Max`, `Sqrt`, `Pow` | arredondamento e ponto flutuante exigem atenção |
| `DateTime` | Data e hora completas | `Now`, `UtcNow`, `AddDays`, `ToString`, `TryParse` | prefira UTC para integrações e persistência |
| `DateOnly` / `TimeOnly` | Quando você quer só data ou só hora | `FromDateTime`, `AddDays`, `AddHours`, `Parse` | mais claro do que usar `DateTime` "pela metade" |
| `TimeSpan` | Duração, timeout, intervalo | `FromSeconds`, `FromMinutes`, `TotalSeconds` | não use para representar um instante no calendário |
| `Guid` | Identificadores únicos | `NewGuid`, `Parse`, `TryParse`, `ToString("N")` | não é identificador legível e não substitui regra de negócio |
| `Random` | Sorteio, simulação, jogos, testes | `Next`, `NextDouble`, `NextInt64`, `NextBytes` | não use para senha, token ou criptografia |
| `Convert` e `TryParse` | Conversão entre tipos e parsing | `Convert.ToInt32`, `int.TryParse`, `DateTime.TryParseExact` | entrada externa deve preferir `TryParse` |
| `Environment` | Dados do processo e da máquina | `MachineName`, `UserName`, `Version`, `GetEnvironmentVariable` | o resultado depende do ambiente onde o app roda |
| `Path`, `File`, `Directory` | Caminhos, arquivos e pastas | `Combine`, `GetExtension`, `ReadAllText`, `WriteAllText`, `Exists`, `CreateDirectory` | não monte caminhos manualmente com `/` ou `\\` |

**Como interpretar o exemplo:** Em C#, "funções predefinidas" normalmente são métodos estáticos ou membros de tipos da BCL (Base Class Library), e não funções soltas como em linguagens script. Isso muda a forma correta de estudar: vale mais aprender **famílias de responsabilidade** (`System`, `System.IO`, `System.Text`, `System.Collections.Generic`) do que decorar chamadas isoladas.

**Regra prática:** se um recurso não é sintaxe (`if`, `switch`, `foreach`, `using`) nem palavra-chave (`class`, `public`, `async`), há uma boa chance de você estar olhando para uma API do .NET. `Console.WriteLine`, `Math.Sqrt`, `Guid.NewGuid`, `File.ReadAllText` e `Path.Combine` são exemplos clássicos.

**Mapa mental útil:** este guia já detalha alguns desses tipos em seções próprias, como `string` e `StringBuilder` na Parte 4, `List<T>` e `Dictionary<TKey, TValue>` na Parte 15, exceções na Parte 18 e LINQ na Parte 14. O importante é enxergar tudo isso como parte do mesmo ecossistema da linguagem.

> **Referências oficiais:** [Overview of core .NET libraries](https://learn.microsoft.com/en-us/dotnet/standard/class-library-overview), [Built-in types (C# reference)](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/built-in-types), [Console](https://learn.microsoft.com/en-us/dotnet/api/system.console?view=net-10.0), [Math](https://learn.microsoft.com/en-us/dotnet/api/system.math?view=net-10.0), [DateTime](https://learn.microsoft.com/en-us/dotnet/api/system.datetime?view=net-10.0), [DateOnly e TimeOnly](https://learn.microsoft.com/en-us/dotnet/standard/datetime/how-to-use-dateonly-timeonly), [TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan?view=net-10.0), [Guid](https://learn.microsoft.com/en-us/dotnet/api/system.guid?view=net-10.0), [Random](https://learn.microsoft.com/en-us/dotnet/api/system.random?view=net-10.0), [Convert](https://learn.microsoft.com/en-us/dotnet/api/system.convert?view=net-10.0), [Environment](https://learn.microsoft.com/en-us/dotnet/api/system.environment?view=net-10.0), [File](https://learn.microsoft.com/en-us/dotnet/api/system.io.file?view=net-10.0), [Path](https://learn.microsoft.com/en-us/dotnet/api/system.io.path?view=net-10.0)

---

## Parte 5 — Modificadores de Acesso

[⬆️ Voltar ao Sumário](#sumário)

---

### 5.1 Os modificadores de acesso do C#

[⬆️ Voltar ao Sumário](#sumário)

C# possui mais modificadores de acesso do que Java, oferecendo controle mais granular.

```
┌────────────────────────────────────────────────────────────────────────────────┐
│ Modificador          │ Mesma classe │ Mesmo assembly │ Subclasse │ Qualquer    │
├──────────────────────┼──────────────┼────────────────┼───────────┼────────────┤
│ public               │      ✅      │      ✅        │     ✅    │     ✅     │
│ private              │      ✅      │      ❌        │     ❌    │     ❌     │
│ protected            │      ✅      │      ❌        │     ✅    │     ❌     │
│ internal             │      ✅      │      ✅        │     ❌    │     ❌     │
│ protected internal   │      ✅      │      ✅        │     ✅    │     ❌     │
│ private protected    │      ✅      │      ❌   (*)  │     ✅    │     ❌     │
└────────────────────────────────────────────────────────────────────────────────┘
(*) private protected: subclasses dentro do mesmo assembly
```

> **Assembly** é a unidade de compilação do .NET — um arquivo `.dll` ou `.exe`. `internal` é o equivalente aproximado ao `package-private` do Java, mas com escopo de assembly em vez de pacote.

**Como interpretar o exemplo:** Modificador de acesso nao serve apenas para esconder membros; ele define quem pode colaborar com quem dentro do sistema. Quando voce trata `public`, `private`, `protected`, `internal` e os compostos como fronteiras de design, suas APIs ficam menores e menos acopladas.

---

### 5.2 Boas práticas com modificadores

[⬆️ Voltar ao Sumário](#sumário)

```csharp
public class ContaBancaria
{
    // Campos: sempre private
    private double _saldo;
    private string _titular;

    // Propriedade pública com getter/setter encapsulado
    // (C# usa Properties — mais poderoso que getters/setters do Java)
    public string Titular
    {
        get => _titular;
        private set => _titular = value ?? throw new ArgumentNullException(nameof(value));
    }

    // Auto-property — sintaxe simplificada (compilador gera o campo privado)
    public string NumeroConta { get; private set; }

    // Propriedade readonly (init-only — C# 9+)
    public DateTime DataAbertura { get; init; }

    // Construtor
    public ContaBancaria(string titular, double saldoInicial)
    {
        Titular       = titular;
        _saldo        = saldoInicial;
        NumeroConta   = Guid.NewGuid().ToString("N")[..8].ToUpper();
        DataAbertura  = DateTime.Now;
    }

    // Propriedade calculada (sem campo backing)
    public bool EstaNegativa => _saldo < 0;

    // Método público
    public bool Sacar(double valor)
    {
        if (!ValorValido(valor) || valor > _saldo) return false;
        _saldo -= valor;
        return true;
    }

    // Método privado auxiliar
    private bool ValorValido(double valor) => valor > 0;
}
```

---

## Parte 6 — Propriedades (Properties)

[⬆️ Voltar ao Sumário](#sumário)

**Como interpretar o exemplo:** A `ContaBancaria` mostra um padrao muito comum em C#: campos privados para estado interno e propriedades publicas para exposicao controlada. Isso permite validar entrada, manter invariantes e evoluir a implementacao sem quebrar a interface visivel.

---

### 6.1 O que são Properties?

[⬆️ Voltar ao Sumário](#sumário)

C# introduz o conceito de **propriedade** como cidadão de primeira classe da linguagem — não é apenas um padrão de codificação (como getters/setters em Java), mas uma construção sintática nativa.

```csharp
public class Pessoa
{
    // Propriedade com lógica nos acessores
    private int _idade;
    public int Idade
    {
        get { return _idade; }
        set
        {
            if (value < 0 || value > 150)
                throw new ArgumentOutOfRangeException(nameof(Idade), "Idade inválida.");
            _idade = value;
        }
    }

    // Auto-property — compilador gera o campo privado automaticamente
    public string Nome { get; set; }

    // Auto-property somente leitura
    public string Id { get; } = Guid.NewGuid().ToString();

    // Auto-property com setter restrito (apenas no construtor ou init)
    public string Email { get; init; }

    // Propriedade calculada (expression-bodied)
    public string NomeCompleto => $"{Nome} (ID: {Id})";

    // Propriedade estática
    public static int TotalInstancias { get; private set; }

    public Pessoa(string nome, string email)
    {
        Nome  = nome;
        Email = email;
        TotalInstancias++;
    }
}

// Uso
var p = new Pessoa("Ana", "ana@email.com") { Idade = 28 };
Console.WriteLine(p.Nome);        // "Ana"
Console.WriteLine(p.NomeCompleto); // "Ana (ID: ...)"
p.Email = "novo@email.com";       // ERRO se Email for { get; init; } após construção
```

**Como interpretar o exemplo:** Properties parecem campos para quem consome a classe, mas se comportam como acessos controlados por `get`, `set` ou `init`. Esse recurso existe para encapsular validacao, calculo e evolucao interna sem mudar o contrato externo.

Em termos de vocabulário, é aqui que entram os **getters** e **setters**:

- `get` é o acessor de **leitura**. Ele define o que acontece quando alguém consulta a propriedade.
- `set` é o acessor de **escrita**. Ele define o que acontece quando alguém atribui um novo valor.
- Dentro do `set`, a palavra-chave implícita `value` representa o valor que está sendo atribuído.
- `init` é uma forma especial de escrita permitida apenas durante a inicialização do objeto.
- `private set` significa: leitura pública, escrita restrita à própria classe.

Em outras palavras, quando alguém escreve `p.Nome`, o `get` é executado. Quando alguém escreve `p.Nome = "Ana"`, o `set` é executado. Isso é importante porque a propriedade não é apenas um campo exposto; ela é um **ponto de controle**. Você pode validar entrada, impedir estados inválidos, calcular valores dinamicamente ou até trocar a implementação interna sem mudar a forma de uso da API.

Outro conceito importante é o de **backing field** (campo de apoio). No exemplo, `_idade` é o campo real onde o valor fica armazenado, enquanto `Idade` é a propriedade que controla o acesso a esse campo. Já em `public string Nome { get; set; }`, o compilador cria esse campo automaticamente, e por isso chamamos essa forma de **auto-property**.

Essa distinção ajuda muito quem vem de Java. Em Java, normalmente você vê métodos como `getNome()` e `setNome(...)`. Em C#, a linguagem elevou esse padrão para a sintaxe nativa com `Nome { get; set; }`. O efeito arquitetural é parecido, mas a ergonomia é melhor, e várias bibliotecas do ecossistema .NET entendem properties como parte central do modelo de objetos, incluindo serialização, data binding, ORMs e ferramentas de inspeção.

---

### 6.2 Expression-bodied members

[⬆️ Voltar ao Sumário](#sumário)

C# permite sintaxe compacta para métodos e propriedades com uma única expressão:

```csharp
public class Calculadora
{
    // Método expression-bodied
    public int Somar(int a, int b) => a + b;
    public double Potencia(double base_, int exp) => Math.Pow(base_, exp);

    // Propriedade expression-bodied
    public string Descricao => "Calculadora simples";

    // Construtor expression-bodied
    private string _nome;
    public Calculadora(string nome) => _nome = nome;
}
```

Antes de avançar, vale fixar uma leitura mental importante do operador `=>` em C#.

Ele aparece em dois contextos muito comuns:

1. **Expression-bodied member:** quando `=>` aparece em método, propriedade, construtor ou outro membro da classe, ele significa algo como **"este membro devolve ou executa diretamente esta expressão"**.
2. **Lambda expression:** quando `=>` aparece entre parâmetros e um corpo inline, ele significa algo como **"receba isto e faça aquilo"**.

Neste trecho atual, todos os usos são do primeiro tipo.

Exemplo:

```csharp
public int Somar(int a, int b) => a + b;
```

é equivalente a:

```csharp
public int Somar(int a, int b)
{
    return a + b;
}
```

Da mesma forma:

```csharp
public string Descricao => "Calculadora simples";
```

é equivalente a:

```csharp
public string Descricao
{
    get { return "Calculadora simples"; }
}
```

E:

```csharp
public Calculadora(string nome) => _nome = nome;
```

é equivalente a:

```csharp
public Calculadora(string nome)
{
    _nome = nome;
}
```

Perceba a ideia central: o `=>` não cria uma regra mágica nova; ele apenas substitui um bloco mais verboso quando a lógica cabe em uma única expressão ou ação direta.

**Como interpretar o exemplo:** Essa sintaxe faz sentido quando a intencao inteira cabe em uma unica expressao. O ganho nao e apenas escrever menos, e sim tornar obvio que aquele membro tem logica direta e nao precisa de um bloco completo.

**Regra prática:** se o membro cabe naturalmente em uma linha e continua claro, `=>` melhora a leitura. Se a lógica começa a exigir várias decisões, validações ou efeitos colaterais, normalmente um bloco com `{ ... }` volta a ser mais legível.

---

## Parte 7 — Palavras-chave Especiais do C#

[⬆️ Voltar ao Sumário](#sumário)

---

### 7.1 `static`

[⬆️ Voltar ao Sumário](#sumário)

```csharp
public class Contador
{
    // Campo estático — compartilhado por todas as instâncias
    private static int _totalInstancias = 0;

    // Campo de instância — cada objeto tem o seu
    public int Id { get; }

    public Contador()
    {
        _totalInstancias++;
        Id = _totalInstancias;
    }

    // Método estático — não acessa membros de instância
    public static int GetTotal() => _totalInstancias;

    // Classe estática — não pode ser instanciada, todos os membros devem ser estáticos
    public static class Utilitarios
    {
        public static string FormatarId(int id) => $"ID-{id:D4}";
    }
}

// Construtor estático — executado uma única vez ao carregar o tipo
public class Configuracao
{
    public static readonly Dictionary<string, string> Valores;

    static Configuracao() // sem modificador de acesso — sempre chamado pelo CLR
    {
        Valores = new Dictionary<string, string>
        {
            ["host"] = "localhost",
            ["porta"] = "8080"
        };
    }
}
```

**Como interpretar o exemplo:** `static` separa o que pertence ao tipo do que pertence a cada instancia. Isso e excelente para utilitarios e estado compartilhado, mas exige cuidado porque qualquer dado estatico tende a viver mais e acoplar mais partes do sistema.

---

### 7.2 `sealed`

[⬆️ Voltar ao Sumário](#sumário)

Equivalente ao `final` de classe em Java — impede herança.

```csharp
// Classe sealed — não pode ser herdada
public sealed class Singleton
{
    private static Singleton? _instancia;
    private Singleton() { }
    public static Singleton Instancia => _instancia ??= new Singleton();
}

// Método sealed em override — impede sobrescrita na cadeia de herança
public class Animal
{
    public virtual void Respirar() => Console.WriteLine("Respirando...");
}

public class Mamifero : Animal
{
    public sealed override void Respirar() => Console.WriteLine("Pulmões"); // não pode ser sobrescrito além daqui
}
```

**Como interpretar o exemplo:** `sealed` e uma forma explicita de dizer que aquela linha de extensao nao deve continuar. Em design orientado a objetos, isso protege invariantes e evita que heranca seja usada onde o tipo nao foi pensado para ser prolongado.

---

### 7.3 `abstract`

[⬆️ Voltar ao Sumário](#sumário)

```csharp
public abstract class Forma
{
    public string Cor { get; set; }

    public Forma(string cor) => Cor = cor;

    // Método abstrato — sem implementação, subclasses DEVEM implementar
    public abstract double CalcularArea();
    public abstract double CalcularPerimetro();

    // Método concreto — herdado como está
    public void ExibirInfo() =>
        Console.WriteLine($"Cor: {Cor} | Área: {CalcularArea():F2}");
}

// new Forma("azul"); // ERRO — não pode instanciar classe abstrata

public class Circulo : Forma
{
    public double Raio { get; }

    public Circulo(string cor, double raio) : base(cor) => Raio = raio;

    public override double CalcularArea()      => Math.PI * Raio * Raio;
    public override double CalcularPerimetro() => 2 * Math.PI * Raio;
}
```

**Como interpretar o exemplo:** Uma classe abstrata captura o que e comum a uma familia de tipos sem fingir que ja e concreta o bastante para ser instanciada. Ela define o que toda subclasse deve fornecer e o que a base ja consegue entregar pronto.

---

### 7.4 `virtual` e `override`

[⬆️ Voltar ao Sumário](#sumário)

Em C#, métodos **não são virtuais por padrão** (ao contrário do Java). É necessário declarar explicitamente com `virtual` para permitir sobrescrita.

```csharp
public class Animal
{
    // virtual — pode ser sobrescrito em subclasses
    public virtual void EmitirSom() => Console.WriteLine("...");

    // Sem virtual — não pode ser sobrescrito (apenas ocultado com 'new')
    public void Respirar() => Console.WriteLine("Respirando");
}

public class Cachorro : Animal
{
    // override — sobrescreve o método virtual da classe base
    public override void EmitirSom() => Console.WriteLine("Au au!");

    // 'new' — oculta o método da classe base (diferente de override — sem polimorfismo)
    public new void Respirar() => Console.WriteLine("Respirando (Cachorro)");
}

Animal a  = new Cachorro();
a.EmitirSom(); // "Au au!" — polimorfismo funciona com virtual/override
a.Respirar();  // "Respirando" — sem polimorfismo, chama a versão de Animal
```

**Como interpretar o exemplo:** O exemplo deixa claro que polimorfismo em C# acontece apenas quando a base abre a porta com `virtual` e a derivada participa com `override`. `new` nao entra no mesmo contrato; ele apenas esconde um membro e pode surpreender quem enxerga o objeto pelo tipo da base.

---

### 7.5 `this` e `base`

[⬆️ Voltar ao Sumário](#sumário)

```csharp
public class Pessoa
{
    public string Nome { get; }
    public int    Idade { get; }

    // 1. Chamar outro construtor da mesma classe com this(...)
    public Pessoa(string nome) : this(nome, 0) { }

    public Pessoa(string nome, int idade)
    {
        Nome  = nome;
        Idade = idade;
    }

    // 2. Retornar a instância atual (Fluent API)
    public Pessoa ComIdade(int idade)
    {
        // Não pode modificar — Nome e Idade são { get; }
        // Este padrão é mais comum com campos mutáveis
        return this;
    }
}

public class Funcionario : Pessoa
{
    public string Cargo { get; }

    // base(...) — chama o construtor da classe base
    public Funcionario(string nome, int idade, string cargo) : base(nome, idade)
    {
        Cargo = cargo;
    }

    public override string ToString()
    {
        // base.ToString() — chama o método da classe base
        return $"{base.ToString()} | Cargo: {Cargo}";
    }
}
```

**Como interpretar o exemplo:** `this` e `base` existem para deixar claro de onde vem a chamada: da propria classe atual ou da implementacao herdada. Essa explicitude ajuda a ler melhor construcao de objetos, reutilizacao de logica e encadeamento em hierarquias.

---

### 7.6 `is`, `as` e Pattern Matching

[⬆️ Voltar ao Sumário](#sumário)

```csharp
object obj = "Olá, C#!";

// is — verifica o tipo
if (obj is string)
    Console.WriteLine("É uma string");

// is com declaração de variável (C# 7+)
if (obj is string texto)
    Console.WriteLine(texto.ToUpper()); // 'texto' está disponível aqui

// as — cast seguro; retorna null se falhar (não lança exceção)
string? s = obj as string;
if (s != null)
    Console.WriteLine(s.Length);

// Cast explícito — lança InvalidCastException se falhar
string s2 = (string)obj; // OK se obj for string
// int i = (int)obj;      // InvalidCastException!

// Pattern Matching em switch (C# 8+)
static string Descrever(object obj) => obj switch
{
    int n when n < 0    => "Número negativo",
    int n when n == 0   => "Zero",
    int n               => $"Número positivo: {n}",
    string s            => $"String: '{s}'",
    null                => "Nulo",
    _                   => $"Outro tipo: {obj.GetType().Name}"
};

// Tuple pattern
static string ClassificarPonto(int x, int y) => (x, y) switch
{
    (0, 0)          => "Origem",
    (> 0, > 0)      => "Primeiro quadrante",
    (< 0, > 0)      => "Segundo quadrante",
    _               => "Outro"
};
```

**Como interpretar o exemplo:** O C# moderno prefere pattern matching porque ele combina teste, extracao e classificacao de forma mais segura e expressiva. Em vez de varios casts espalhados, voce descreve a forma do valor e o compilador ajuda a manter o fluxo correto.

---

### 7.7 `using` para gerenciamento de recursos

[⬆️ Voltar ao Sumário](#sumário)

```csharp
// using statement — chama Dispose() automaticamente ao sair do escopo
// Equivalente ao try-with-resources do Java
using (var conexao = new SqlConnection(connectionString))
{
    conexao.Open();
    // usar conexao...
} // Dispose() chamado automaticamente

// using declaration (C# 8+) — mais conciso; descarta ao sair do escopo do bloco
using var reader = new StreamReader("arquivo.txt");
string conteudo = reader.ReadToEnd();
// reader.Dispose() é chamado ao final do método/bloco
```

**Como interpretar o exemplo:** Aqui `using` nao tem a ver com importar namespaces; ele existe para descarte deterministico de recursos que o GC nao fecha no tempo certo. Arquivos, conexoes e streams sao exemplos classicos em que esperar a coleta de lixo nao e uma estrategia aceitavel.

---

### 7.8 `ref`, `out` e `in`

[⬆️ Voltar ao Sumário](#sumário)

```csharp
// ref — passa por referência (leitura e escrita; deve ser inicializado antes)
void Dobrar(ref int valor) => valor *= 2;

int numero = 5;
Dobrar(ref numero);
Console.WriteLine(numero); // 10

// out — passa por referência para saída (não precisa estar inicializado)
bool TryParseCustom(string s, out int resultado)
{
    return int.TryParse(s, out resultado); // preenche 'resultado'
}

if (TryParseCustom("42", out int valor))
    Console.WriteLine(valor); // 42

// in — passa por referência somente leitura (sem cópia, sem modificação)
void ImprimirPonto(in Ponto p) => Console.WriteLine($"({p.X}, {p.Y})");
// p.X = 0; // ERRO — 'in' é somente leitura
```

**Como interpretar o exemplo:** Os tres modificadores deixam explicita a intencao de passagem por referencia. `ref` compartilha leitura e escrita, `out` obriga preenchimento de saida, e `in` evita copia sem permitir modificacao do argumento pelo metodo chamado.

---

## Parte 8 — Controle de Fluxo

[⬆️ Voltar ao Sumário](#sumário)

---

### 8.1 `if / else if / else`

[⬆️ Voltar ao Sumário](#sumário)

```csharp
int nota = 75;

if (nota >= 90)
    Console.WriteLine("Conceito A");
else if (nota >= 70)
    Console.WriteLine("Conceito B");
else if (nota >= 50)
    Console.WriteLine("Conceito C");
else
    Console.WriteLine("Reprovado");

// Operador ternário
string status = nota >= 50 ? "Aprovado" : "Reprovado";

// Null-coalescing
string? nome = null;
string display = nome ?? "Sem nome"; // "Sem nome"

// Null-coalescing assignment (C# 8+)
nome ??= "Padrão"; // atribui apenas se nome for null
```

**Como interpretar o exemplo:** Esse bloco representa a forma mais direta de modelar decisoes booleanas. O mesmo trecho tambem mostra que C# moderno oferece formas orientadas a valor, como operador ternario e nulidade, para escrever escolhas pequenas de forma mais concisa.

---

### 8.2 `switch` e switch expressions

[⬆️ Voltar ao Sumário](#sumário)

```csharp
// switch statement clássico
int dia = 3;
switch (dia)
{
    case 1:
        Console.WriteLine("Segunda-feira");
        break;
    case 2:
        Console.WriteLine("Terça-feira");
        break;
    case 3:
        Console.WriteLine("Quarta-feira");
        break;
    case 6:
    case 7:
        Console.WriteLine("Fim de semana");
        break;
    default:
        Console.WriteLine("Dia inválido");
        break;
}

// switch expression (C# 8+) — retorna um valor
string descricao = dia switch
{
    1 => "Segunda-feira",
    2 => "Terça-feira",
    3 => "Quarta-feira",
    6 or 7 => "Fim de semana",    // múltiplos valores com 'or'
    _ => "Dia inválido"           // _ é o default
};

// switch com when (guard clause)
string classificar(int n) => n switch
{
    < 0          => "Negativo",
    0            => "Zero",
    > 0 and < 10 => "Pequeno positivo",
    _            => "Grande positivo"
};
```

**Como interpretar o exemplo:** O `switch` classico organiza varios caminhos por valor, enquanto a switch expression aproxima o controle de fluxo de uma classificacao declarativa. Em C# moderno, isso torna o `switch` cada vez mais util para padroes e formas, nao apenas para substituir varios `if`s`.

---

### 8.3 Loops

[⬆️ Voltar ao Sumário](#sumário)

```csharp
// for
for (int i = 0; i < 5; i++)
    Console.WriteLine(i); // 0, 1, 2, 3, 4

// foreach — para qualquer IEnumerable
string[] nomes = { "Ana", "Bruno", "Carlos" };
foreach (string nome in nomes)
    Console.WriteLine(nome);

// while
int contador = 0;
while (contador < 5)
{
    Console.WriteLine(contador);
    contador++;
}

// do-while
int tentativas = 0;
do
{
    Console.WriteLine($"Tentativa {tentativas + 1}");
    tentativas++;
} while (tentativas < 3);

// break e continue
for (int i = 0; i < 10; i++)
{
    if (i == 5) break;    // para o loop
    if (i % 2 == 0) continue; // pula para próxima iteração
    Console.WriteLine(i); // imprime 1, 3
}
```

**Como interpretar o exemplo:** Cada laco existe para uma intencao principal: `for` para contagem, `foreach` para sequencias, `while` para condicao e `do-while` para garantir ao menos uma execucao. `break` e `continue` refinam esse fluxo, mas em excesso podem deixar a leitura mais dificil.

---

## Parte 9 — Métodos

[⬆️ Voltar ao Sumário](#sumário)

---

### 9.1 Declaração de métodos

[⬆️ Voltar ao Sumário](#sumário)

```csharp
// modificadorDeAcesso tipoDeRetorno NomeDoMetodo(Tipo parametro)
public int Somar(int a, int b) => a + b;

// Parâmetros opcionais com valor padrão
public void Log(string mensagem, string nivel = "INFO", bool timestamp = true)
{
    string prefixo = timestamp ? $"[{DateTime.Now:HH:mm:ss}] " : "";
    Console.WriteLine($"{prefixo}[{nivel}] {mensagem}");
}

Log("Iniciando...");                     // usa defaults
Log("Erro!", "ERROR");                   // nivel = "ERROR"
Log("Teste", timestamp: false);          // argumento nomeado — pula 'nivel'

// Parâmetros varargs (params)
public int SomarTodos(params int[] numeros)
{
    int total = 0;
    foreach (int n in numeros) total += n;
    return total;
}

SomarTodos();              // 0
SomarTodos(1, 2, 3, 4);    // 10
SomarTodos(new int[]{1,2}); // array explícito também funciona
```

**Como interpretar o exemplo:** A assinatura de um metodo e um contrato de uso: ela informa retorno, parametros e ergonomia de chamada. Recursos como parametros opcionais, nomeados e `params` tornam a API mais amigavel, mas tambem moldam a forma como outros desenvolvedores entendem o que e obrigatorio.

---

### 9.2 Métodos de extensão (Extension Methods)

[⬆️ Voltar ao Sumário](#sumário)

C# permite adicionar métodos a tipos existentes sem herança ou modificação do código original.

> Observação de versão: os exemplos abaixo usam o formato clássico de extension method (`static` + `this` no primeiro parâmetro), que ainda é o mais comum em bases de código. Em C# 14, a linguagem também ganhou **extension members**, uma sintaxe mais moderna para esse mesmo conceito.

```csharp
// Métodos de extensão devem estar em uma classe estática
public static class StringExtensions
{
    // 'this' no primeiro parâmetro indica que é um método de extensão de 'string'
    public static bool EhPalindromo(this string texto)
    {
        string limpo = texto.ToLower().Replace(" ", "");
        return limpo == new string(limpo.Reverse().ToArray());
    }

    public static string Capitalizar(this string texto)
    {
        if (string.IsNullOrEmpty(texto)) return texto;
        return char.ToUpper(texto[0]) + texto[1..].ToLower();
    }
}

// Uso — parece um método nativo de string
bool eh   = "arara".EhPalindromo();    // true
string cap = "jOÃo".Capitalizar();     // "João"

// Métodos de extensão em IEnumerable
public static class EnumerableExtensions
{
    public static IEnumerable<T> FiltrarNulos<T>(this IEnumerable<T?> source)
        where T : class
        => source.Where(x => x != null)!;
}
```

**Como interpretar o exemplo:** Um metodo de extensao parece nativo do tipo alvo, mas continua sendo um metodo estatico com sintaxe especial. Isso e excelente para criar APIs fluentes, desde que voce nao esconda dependencias nem espalhe comportamento sem criterio.

---

### 9.3 Sobrecarga de métodos

[⬆️ Voltar ao Sumário](#sumário)

```csharp
public class Geometria
{
    public double CalcularArea(double lado)                        => lado * lado;
    public double CalcularArea(double largura, double altura)      => largura * altura;
    public double CalcularArea(double b, double h, bool triangulo) => b * h / 2;
}
```

---

## Parte 10 — Enums

[⬆️ Voltar ao Sumário](#sumário)

**Como interpretar o exemplo:** Sobrecarga funciona bem quando varias assinaturas representam a mesma ideia com entradas diferentes. Quando as versoes passam a fazer coisas semanticamente distantes, o recurso deixa de melhorar a API e comeca a confundir o leitor.

---

### 10.1 Enums básicos

[⬆️ Voltar ao Sumário](#sumário)

```csharp
public enum DiaDaSemana
{
    Segunda = 1,
    Terca   = 2,
    Quarta  = 3,
    Quinta  = 4,
    Sexta   = 5,
    Sabado  = 6,
    Domingo = 7
}

DiaDaSemana hoje = DiaDaSemana.Quarta;

// Métodos utilitários
string nome  = hoje.ToString();                          // "Quarta"
int valor    = (int)hoje;                                // 3
DiaDaSemana d = (DiaDaSemana)5;                          // Sexta
DiaDaSemana e = Enum.Parse<DiaDaSemana>("Segunda");       // Segunda
bool valido  = Enum.IsDefined(typeof(DiaDaSemana), 8);   // false

// Enum em switch expression
string tipo = hoje switch
{
    DiaDaSemana.Sabado or DiaDaSemana.Domingo => "Fim de semana",
    _ => "Dia útil"
};
```

**Como interpretar o exemplo:** `enum` existe para dar nome semantico a conjuntos finitos de estados, evitando numeros magicos pelo codigo. O cast e o parse lembram que ainda existe uma base numerica por baixo, entao valores externos continuam precisando de validacao.

---

### 10.2 Flags enum — bitmask

[⬆️ Voltar ao Sumário](#sumário)

```csharp
// [Flags] permite combinar valores como bitmask
[Flags]
public enum Permissoes
{
    Nenhuma   = 0,
    Leitura   = 1,        // 0001
    Escrita   = 2,        // 0010
    Exclusao  = 4,        // 0100
    Admin     = 8,        // 1000
    Total     = Leitura | Escrita | Exclusao | Admin  // 1111
}

Permissoes usuario = Permissoes.Leitura | Permissoes.Escrita; // 0011

// Verificar se tem uma permissão
bool podeEscrever = usuario.HasFlag(Permissoes.Escrita); // true
bool podeExcluir  = usuario.HasFlag(Permissoes.Exclusao); // false

// Adicionar/remover permissão
usuario |= Permissoes.Exclusao;  // adiciona
usuario &= ~Permissoes.Escrita;  // remove
```

---

## Parte 11 — Classes e Objetos

[⬆️ Voltar ao Sumário](#sumário)

**Como interpretar o exemplo:** Aqui o `enum` nao representa estados unicos, e sim combinacoes de permissoes. O detalhe tecnico decisivo e usar potencias de dois, para que cada bit represente uma capacidade independente e possa ser composto sem ambiguidade.

---

### 11.1 Estrutura completa de uma classe

[⬆️ Voltar ao Sumário](#sumário)

```csharp
public class ContaBancaria
{
    // ─── CAMPOS privados ──────────────────────────────────────────────────
    private double _saldo;
    private static int _totalContas = 0;

    // ─── CONSTANTE ────────────────────────────────────────────────────────
    public const double TaxaOperacao = 0.02;

    // ─── PROPRIEDADES ─────────────────────────────────────────────────────
    public string Titular    { get; }
    public string Numero     { get; }
    public double Saldo      => _saldo; // propriedade calculada (read-only)
    public bool   EstaNegativa => _saldo < 0;

    public static int TotalContas => _totalContas;

    // ─── CONSTRUTOR ───────────────────────────────────────────────────────
    public ContaBancaria(string titular, double saldoInicial = 0)
    {
        Titular = titular ?? throw new ArgumentNullException(nameof(titular));
        _saldo  = saldoInicial;
        _totalContas++;
        Numero  = $"CC-{_totalContas:D4}";
    }

    // ─── MÉTODOS ──────────────────────────────────────────────────────────
    public void Depositar(double valor)
    {
        if (valor <= 0)
            throw new ArgumentException("Valor deve ser positivo.", nameof(valor));
        _saldo += valor;
    }

    public bool Sacar(double valor)
    {
        if (valor <= 0 || valor > _saldo) return false;
        _saldo -= valor;
        return true;
    }

    // ─── SOBRESCRITA DE Object ────────────────────────────────────────────
    public override string ToString() =>
        $"ContaBancaria {{ Titular={Titular}, Saldo={_saldo:C}, Numero={Numero} }}";

    public override bool Equals(object? obj) =>
        obj is ContaBancaria outra && Numero == outra.Numero;

    public override int GetHashCode() => Numero.GetHashCode();
}
```

**Como interpretar o exemplo:** A classe foi montada como mapa dos principais elementos de um tipo orientado a objetos em C#: campos, propriedades, construtor, metodos e sobrescritas. Ler uma classe com essa lente ajuda a ver que ela nao guarda apenas dados; ela define identidade, invariantes e integracao com o resto do runtime.

---

### 11.2 Construtores em Profundidade

[⬆️ Voltar ao Sumário](#sumário)

#### 11.2.1 Definição

[⬆️ Voltar ao Sumário](#sumário)

| Termo | Definição |
|---|---|
| Construtor | Método especial executado automaticamente no momento da criação de uma instância (`new`), responsável por inicializar o estado do objeto. |
| Assinatura | Mesmo nome da classe. Não possui tipo de retorno (nem mesmo `void`). |
| Quantidade | Uma classe pode ter múltiplos construtores, desde que cada um tenha uma lista de parâmetros distinta (sobrecarga). |

**Como interpretar o exemplo:** A tabela organiza o construtor como contrato de nascimento do objeto, e nao como um metodo qualquer. Essa distincao importa porque o construtor comunica o que e obrigatorio para a instancia existir em estado coerente.

#### 11.2.2 Construtor padrão (implícito)

[⬆️ Voltar ao Sumário](#sumário)

Caso nenhum construtor seja declarado explicitamente, o compilador gera um **construtor padrão sem parâmetros** automaticamente. No momento em que qualquer construtor é declarado pelo programador, essa geração automática deixa de ocorrer.

```csharp
public class Produto
{
    public string Nome;
}

// O compilador injeta, de forma implícita, o equivalente a:
// public Produto() { }

var p = new Produto(); // válido — construtor implícito disponível
```

| Linha | Explicação |
|---|---|
| `public class Produto { public string Nome; }` | Classe sem nenhum construtor explícito. |
| `var p = new Produto();` | Funciona porque o compilador gerou um construtor padrão sem parâmetros. |

> Assim que se declara `public Produto(string nome) { ... }`, o construtor sem parâmetros desaparece e `new Produto()` passa a gerar erro de compilação, salvo se for declarado manualmente.

**Como interpretar o exemplo:** O comportamento implicito do compilador resolve casos simples, mas tambem gera surpresa quando a classe ganha seu primeiro construtor explicito. Saber que o construtor sem parametros desaparece nessa hora evita muitos erros de instanciacao.

#### 11.2.3 Construtor parametrizado

[⬆️ Voltar ao Sumário](#sumário)

```csharp
public class Produto
{
    public string Nome { get; }
    public double Preco { get; }

    // Construtor parametrizado — recebe valores para inicializar o estado do objeto
    public Produto(string nome, double preco)
    {
        // Validação de pré-condição antes de atribuir o estado
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome não pode ser vazio.", nameof(nome));

        Nome  = nome;   // atribuição ao membro público (propriedade somente leitura)
        Preco = preco;  // atribuição ao membro público (propriedade somente leitura)
    }
}

var p = new Produto("Teclado", 199.90);
```

| Linha | Explicação |
|---|---|
| `public Produto(string nome, double preco)` | Construtor que exige dois argumentos para que o objeto seja criado. |
| `if (string.IsNullOrWhiteSpace(nome)) throw ...` | Validação executada antes de qualquer atribuição — garante estado consistente. |
| `Nome = nome;` | Inicializa a propriedade `Nome`, que só pode ser atribuída dentro da classe (`get;` sem `set`). |

**Como interpretar o exemplo:** Esse formato e o mais importante quando o objeto depende de dados essenciais para existir com sentido. Repare que a validacao acontece antes da atribuicao, o que reforca a boa pratica de falhar cedo em vez de permitir estado invalido.

#### 11.2.4 Sobrecarga de construtores

[⬆️ Voltar ao Sumário](#sumário)

Assim como métodos, construtores podem ser sobrecarregados — múltiplas assinaturas, mesmo nome.

```csharp
public class Retangulo
{
    public double Largura { get; }
    public double Altura  { get; }

    // Sobrecarga 1 — dois parâmetros distintos
    public Retangulo(double largura, double altura)
    {
        Largura = largura;
        Altura  = altura;
    }

    // Sobrecarga 2 — um único parâmetro (quadrado)
    public Retangulo(double lado)
    {
        Largura = lado;
        Altura  = lado;
    }

    // Sobrecarga 3 — sem parâmetros (valores padrão)
    public Retangulo()
    {
        Largura = 1;
        Altura  = 1;
    }
}
```

| Linha | Explicação |
|---|---|
| `public Retangulo(double largura, double altura)` | Assinatura com dois parâmetros do tipo `double`. |
| `public Retangulo(double lado)` | Assinatura com um único parâmetro — distinta da anterior pela quantidade de argumentos. |
| `public Retangulo()` | Assinatura sem parâmetros — válida porque as duas anteriores existem como sobrecargas explícitas. |

**Como interpretar o exemplo:** Sobrecargas oferecem caminhos diferentes para criar o mesmo tipo, mas o objeto final continua sendo conceitualmente o mesmo. Use esse recurso para representar inicializacoes legitimas, e nao para misturar modelos mentais diferentes sob o mesmo nome de classe.

#### 11.2.5 Encadeamento de construtores com `this(...)`

[⬆️ Voltar ao Sumário](#sumário)

Evita duplicação de lógica de inicialização entre sobrecargas — um construtor delega para outro da mesma classe.

```csharp
public class Retangulo
{
    public double Largura { get; }
    public double Altura  { get; }

    // Construtor "principal" — contém toda a lógica de inicialização e validação
    public Retangulo(double largura, double altura)
    {
        if (largura <= 0 || altura <= 0)
            throw new ArgumentOutOfRangeException("Dimensões devem ser positivas.");

        Largura = largura;
        Altura  = altura;
    }

    // Delega para o construtor principal — evita repetir a validação
    public Retangulo(double lado) : this(lado, lado) { }

    // Delega para o construtor principal com valores padrão
    public Retangulo() : this(1, 1) { }
}
```

| Linha | Explicação |
|---|---|
| `public Retangulo(double lado) : this(lado, lado)` | O `: this(...)` invoca o construtor de dois parâmetros antes de executar o corpo (vazio) deste. |
| `public Retangulo() : this(1, 1)` | Mesmo princípio — reaproveita a validação definida em um único ponto. |

> **Ordem de execução:** o construtor referenciado em `this(...)` é executado **integralmente primeiro**; só então o corpo do construtor atual é executado.

**Como interpretar o exemplo:** `this(...)` evita duplicacao porque transforma um construtor em ponto central de validacao e inicializacao. Esse padrao e valioso quando voce quer varias portas de entrada para o mesmo tipo, mas precisa manter uma unica fonte de verdade.

#### 11.2.6 Chamada ao construtor da classe base com `base(...)`

[⬆️ Voltar ao Sumário](#sumário)

```csharp
public class Pessoa
{
    public string Nome { get; }

    public Pessoa(string nome)
    {
        Nome = nome ?? throw new ArgumentNullException(nameof(nome));
    }
}

public class Funcionario : Pessoa
{
    public decimal Salario { get; }

    // base(nome) — invoca o construtor de Pessoa antes do corpo deste construtor
    public Funcionario(string nome, decimal salario) : base(nome)
    {
        Salario = salario;
    }
}
```

| Linha | Explicação |
|---|---|
| `: base(nome)` | Encaminha o argumento `nome` para o construtor de `Pessoa`, garantindo que `Nome` seja inicializado pela classe base. |
| `Salario = salario;` | Executado **após** o construtor da base ter concluído. |

> Caso a classe base não possua construtor sem parâmetros, toda classe derivada **deve** chamar explicitamente um construtor da base via `base(...)`; caso contrário ocorre erro de compilação.

**Como interpretar o exemplo:** `base(...)` garante que a classe derivada respeite o processo de inicializacao da base, em vez de tentar reconstruir isso manualmente. Em outras palavras, a subclasse nao deve pular as invariantes da classe pai; ela deve completa-las.

#### 11.2.7 Ordem de execução em uma hierarquia de herança

[⬆️ Voltar ao Sumário](#sumário)

| Ordem | Etapa |
|---|---|
| 1 | Inicializadores de campo da classe **base** (ex.: `private int _x = 10;`) |
| 2 | Corpo do construtor da classe **base** |
| 3 | Inicializadores de campo da classe **derivada** |
| 4 | Corpo do construtor da classe **derivada** |

```csharp
public class Base
{
    private int _valor = ImprimirEFazer("Inicializador de campo (Base)", 1);
    public Base() => Console.WriteLine("Construtor (Base)");

    private static int ImprimirEFazer(string msg, int retorno)
    {
        Console.WriteLine(msg);
        return retorno;
    }
}

public class Derivada : Base
{
    private int _valor2 = 99;
    public Derivada() => Console.WriteLine("Construtor (Derivada)");
}

var d = new Derivada();
// Saída, na ordem:
// Inicializador de campo (Base)
// Construtor (Base)
// Construtor (Derivada)
```

**Como interpretar o exemplo:** A ordem mostrada explica por que certos membros ainda nao estao prontos quando outro codigo da hierarquia tenta acessa-los cedo demais. Esse entendimento evita bugs sutis de inicializacao, principalmente em classes derivadas.

#### 11.2.8 Construtor estático

[⬆️ Voltar ao Sumário](#sumário)

Já introduzido na [Parte 7.1](#71-static); reapresentado aqui no contexto específico de inicialização de classe.

| Característica | Descrição |
|---|---|
| Assinatura | Sem modificador de acesso, sem parâmetros: `static NomeDaClasse() { ... }` |
| Execução | Uma única vez, automaticamente, antes do primeiro uso do tipo (acesso a membro estático ou primeira instanciação) — controlada pelo CLR, não pelo programador. |
| Finalidade | Inicializar campos `static` que exigem lógica não trivial. |

```csharp
public class Configuracao
{
    public static readonly Dictionary<string, string> Valores;

    // Construtor estático — sem 'public', sem parâmetros
    static Configuracao()
    {
        Valores = new Dictionary<string, string>
        {
            ["host"]  = "localhost",
            ["porta"] = "8080"
        };
    }
}
```

**Como interpretar o exemplo:** O construtor estatico existe para inicializacao do tipo, nao do objeto. Ele roda uma unica vez, sob controle do CLR, e por isso deve ser reservado para setup realmente global do tipo.

#### 11.2.9 Construtor privado

[⬆️ Voltar ao Sumário](#sumário)

Impede a criação de instâncias fora da própria classe. Aplicado tipicamente em padrões como Singleton ou em classes que expõem apenas métodos fábrica (`static`).

```csharp
public sealed class ConfiguracaoGlobal
{
    private static readonly ConfiguracaoGlobal _instancia = new();

    // Construtor privado — 'new ConfiguracaoGlobal()' fora desta classe gera erro de compilação
    private ConfiguracaoGlobal() { }

    public static ConfiguracaoGlobal Instancia => _instancia;
}

// var c = new ConfiguracaoGlobal(); // ERRO de compilação — construtor inacessível
var c = ConfiguracaoGlobal.Instancia; // única forma válida de obter a instância
```

**Como interpretar o exemplo:** Tornar o construtor privado restringe quem pode criar instancias e por qual caminho. Isso e util quando a propria classe precisa controlar cardinalidade, cache, factories ou politicas especiais de criacao.

#### 11.2.10 Construtores primários (Primary Constructors — C# 12)

[⬆️ Voltar ao Sumário](#sumário)

Recurso introduzido no C# 12: permite declarar os parâmetros do construtor diretamente na assinatura da classe, eliminando a necessidade de um bloco de construtor explícito para os casos simples de atribuição direta.

```csharp
// Os parâmetros (nome, preco) ficam disponíveis em toda a classe,
// não apenas dentro de um construtor — comportam-se como parâmetros de escopo de classe.
public class Produto(string nome, double preco)
{
    // Propriedade que apenas expõe o parâmetro do construtor primário
    public string Nome  { get; } = nome;
    public double Preco { get; } = preco;

    // O parâmetro 'preco' pode ser usado diretamente em métodos, sem precisar de campo próprio
    public double PrecoComDesconto(double percentual) => preco * (1 - percentual);
}

var p = new Produto("Teclado", 199.90);
```

| Linha | Explicação |
|---|---|
| `public class Produto(string nome, double preco)` | Declara o construtor primário — `nome` e `preco` tornam-se parâmetros acessíveis em todo o corpo da classe. |
| `public string Nome { get; } = nome;` | Atribuição de inicialização de propriedade a partir do parâmetro do construtor primário. |
| `preco * (1 - percentual)` | Uso direto do parâmetro `preco` dentro de um método, sem necessidade de campo `_preco` intermediário. |

> **Atenção:** Eu, Claude, não estou 100% certo de todos os detalhes finos de captura de parâmetros de construtores primários (por exemplo, regras exatas de quando o compilador gera um campo oculto versus reavalia o parâmetro a cada acesso) — recomenda-se validar esse comportamento específico na documentação oficial da Microsoft antes de aplicar em código de produção sensível a performance.

**Como interpretar o exemplo:** O ganho aqui e reduzir boilerplate em classes cujo construtor apenas recebe e expoe dados. O cuidado e nao sacrificar legibilidade: quando a classe cresce demais, a forma tradicional pode voltar a ser mais clara.

#### 11.2.11 Tabela-resumo dos tipos de construtor

[⬆️ Voltar ao Sumário](#sumário)

| Tipo | Modificador | Quando executa | Uso típico |
|---|---|---|---|
| Padrão (implícito) | `public` (gerado) | Na ausência de qualquer construtor declarado | Classes simples sem necessidade de inicialização |
| Parametrizado | `public`/outro | A cada `new` com os argumentos correspondentes | Inicialização obrigatória de estado |
| Estático | (nenhum) | Uma única vez, antes do primeiro uso do tipo | Inicialização de campos `static` |
| Privado | `private` | Apenas internamente à própria classe | Singleton, métodos fábrica |
| Primário (C# 12) | conforme classe | Igual ao parametrizado | Redução de verbosidade em classes simples |

**Como interpretar o exemplo:** A tabela final deixa claro que o tipo de construtor escolhido comunica intencao arquitetural, e nao apenas gosto sintatico. Em geral, o melhor construtor e o mais simples que ainda deixa o objeto nascer valido e facil de usar.

**Como interpretar o exemplo:** Todo o capitulo de construtores gira em torno da mesma ideia: o momento do `new` e a hora de garantir que o objeto nasca valido. Os diferentes estilos existem para equilibrar clareza, reaproveitamento da logica de inicializacao e protecao de invariantes.

---

### 11.3 Records (C# 9+)

[⬆️ Voltar ao Sumário](#sumário)

Records são tipos de dados imutáveis com `Equals`, `GetHashCode` e `ToString` gerados automaticamente. Equivalente e superior aos Records do Java 16+.

```csharp
// Record de posicional — uma linha
public record Ponto(double X, double Y);

Ponto p1 = new Ponto(3.0, 4.0);
Ponto p2 = new Ponto(3.0, 4.0);

Console.WriteLine(p1.X);          // 3.0
Console.WriteLine(p1);            // "Ponto { X = 3, Y = 4 }"
Console.WriteLine(p1 == p2);      // true — comparação por valor

// 'with' expression — cria cópia com campos alterados (imutável)
Ponto p3 = p1 with { X = 10.0 };  // p3 = (10, 4); p1 não muda

// Record com validação e membros adicionais
public record Temperatura(double Valor, string Unidade)
{
    // Construtor compacto com validação
    public Temperatura
    {
        if (!new[] { "C", "F", "K" }.Contains(Unidade))
            throw new ArgumentException($"Unidade inválida: {Unidade}");
    }

    public double EmCelsius() => Unidade switch
    {
        "C" => Valor,
        "F" => (Valor - 32) * 5 / 9,
        "K" => Valor - 273.15,
        _   => throw new InvalidOperationException()
    };
}

// Record struct (C# 10+) — tipo de valor imutável
public record struct Coordenada(double Latitude, double Longitude);
```

**Como interpretar o exemplo:** Records sao ideais quando o foco do tipo esta nos dados e nao na identidade da instancia. O exemplo destaca tres ideias centrais: igualdade por valor, copia imutavel com `with` e sintaxe compacta para modelos orientados a dados.

---

### 11.4 Padrão Builder

[⬆️ Voltar ao Sumário](#sumário)

```csharp
public class Pedido
{
    public string  Cliente          { get; }
    public string  Produto          { get; }
    public int     Quantidade       { get; }
    public string  EnderecoEntrega  { get; }
    public bool    EntregaExpressa  { get; }

    private Pedido(Builder b)
    {
        Cliente         = b.Cliente;
        Produto         = b.Produto;
        Quantidade      = b.Quantidade;
        EnderecoEntrega = b.EnderecoEntrega;
        EntregaExpressa = b.EntregaExpressa;
    }

    public class Builder
    {
        public string  Cliente         { get; }
        public string  Produto         { get; }
        public int     Quantidade      { get; private set; } = 1;
        public string  EnderecoEntrega { get; private set; } = "";
        public bool    EntregaExpressa { get; private set; } = false;

        public Builder(string cliente, string produto)
        {
            Cliente = cliente;
            Produto = produto;
        }

        public Builder ComQuantidade(int qtd)        { Quantidade      = qtd;  return this; }
        public Builder ComEndereco(string endereco)  { EnderecoEntrega = endereco; return this; }
        public Builder ComEntregaExpressa()          { EntregaExpressa = true; return this; }
        public Pedido  Build()                       => new Pedido(this);
    }
}

// Uso
var pedido = new Pedido.Builder("Ana", "Teclado")
    .ComQuantidade(2)
    .ComEndereco("Rua das Flores, 100")
    .ComEntregaExpressa()
    .Build();
```

Leitura guiada do trecho mais importante:

- `ComQuantidade(...)`, `ComEndereco(...)` e `ComEntregaExpressa()` retornam o próprio `Builder`.
- Esse `return this` permite continuar a chamada na mesma expressão.
- `Build()` é o ponto em que o objeto final `Pedido` é materializado.

Ou seja, esta linha:

```csharp
public Pedido Build() => new Pedido(this);
```

deve ser lida assim:

- crie um novo `Pedido`;
- passe o builder atual (`this`) para o construtor privado;
- o construtor do `Pedido` copia do builder os valores acumulados.

Em outras palavras, o builder funciona como uma "área de montagem temporária". O produto final só nasce no `Build()`.

> Em C# moderno, o padrão builder frequentemente é substituído por **object initializers** e **init-only properties**, que oferecem sintaxe mais limpa sem classe auxiliar.

---

## Parte 12 — Herança e Polimorfismo

[⬆️ Voltar ao Sumário](#sumário)

**Como interpretar o exemplo:** Builder resolve bem objetos com poucos parametros obrigatorios e varios opcionais, especialmente quando o conjunto de construtores ficaria ilegivel. Ele tambem aproxima a chamada da forma como a pessoa pensa no objeto: primeiro configura, depois materializa com `Build()`.

---

### 12.1 Herança em C#

[⬆️ Voltar ao Sumário](#sumário)

Herança é o mecanismo pelo qual um tipo especializado reutiliza e estende outro tipo mais geral. Em termos de modelagem, herança costuma representar uma relação **"é um"**:

- um `Cachorro` **é um** `Animal`;
- um `FuncionarioCLT` **é um** `Funcionario`;
- um `ArquivoCsv` **é um** `ArquivoImportavel`.

Em C#, herança de classes é **simples**: uma classe pode herdar de **uma única classe base**, mas pode implementar **várias interfaces**. Isso é intencional. A linguagem quer evitar hierarquias excessivamente complexas e, ao mesmo tempo, permitir composição de comportamento por contratos.

O motivo mais importante para herdar não é "reaproveitar código", e sim habilitar **polimorfismo**. Ou seja: escrever código que trabalha com a classe base e, em tempo de execução, executa o comportamento especializado da subclasse.

```csharp
public class Animal
{
    public string Nome  { get; }
    public int    Idade { get; }

    public Animal(string nome, int idade)
    {
        Nome  = nome;
        Idade = idade;
    }

    public virtual void EmitirSom() =>
        Console.WriteLine($"{Nome} faz um som.");

    public override string ToString() => $"{GetType().Name}: {Nome}";
}

public class Cachorro : Animal
{
    public string Raca { get; }

    public Cachorro(string nome, int idade, string raca) : base(nome, idade)
        => Raca = raca;

    public override void EmitirSom() =>
        Console.WriteLine($"{Nome} diz: Au au!");

    public void BuscarObjeto() =>
        Console.WriteLine($"{Nome} foi buscar o objeto!");
}
```

Quando você escreve:

```csharp
Animal animal = new Cachorro("Bolt", 4, "Vira-lata");
animal.EmitirSom();
```

o tipo da variável é `Animal`, mas o comportamento executado é o de `Cachorro`, porque `EmitirSom` foi marcado como `virtual` na base e `override` na derivada. Esse é o centro do polimorfismo orientado a objetos em C#.

Regras práticas importantes:

- Use herança quando a subclasse realmente respeita o contrato da classe base.
- Prefira composição quando o comportamento varia por combinação de capacidades, e não por taxonomia.
- Mantenha hierarquias rasas; profundidade demais costuma aumentar acoplamento e fragilidade.
- Uma classe base ruim gera o problema clássico de **base class frágil**: qualquer mudança nela pode quebrar várias subclasses.

Em engenharia de software real, a pergunta não é "posso herdar?", mas sim **"a herança expressa corretamente o domínio?"**. Se a resposta for "mais ou menos", provavelmente uma interface ou composição produzirá um design melhor.

**Como interpretar o exemplo:** O trecho com `Animal animal = new Cachorro(...)` e o teste mais importante da heranca: o consumidor trabalha com a abstracao e o runtime despacha o comportamento especializado correto. Esse e o uso saudavel da heranca em C#: representar um verdadeiro relacionamento `e um` sem quebrar o contrato da base.

---

### 12.2 Interfaces

[⬆️ Voltar ao Sumário](#sumário)

Se herança responde à pergunta **"o que esse tipo é?"**, interface responde à pergunta **"o que esse tipo sabe fazer?"**.

Uma interface é um **contrato de comportamento**. Ela não existe para modelar identidade, e sim para descrever capacidades consumíveis por outros tipos. Por isso, classes, `record`s e `struct`s que não têm nenhuma relação de parentesco podem implementar a mesma interface e participar do mesmo fluxo de processamento.

Em outras palavras, uma interface permite programar contra uma **abstração estável**, em vez de programar contra uma implementação específica.

```csharp
public interface IPagavel
{
    decimal CalcularTotal();
    void ProcessarPagamento(string metodo);

    // Método default (C# 8+) — implementação padrão
    string GerarRecibo() => $"Recibo emitido em: {DateTime.UtcNow:dd/MM/yyyy}";

    // Método estático em interface (C# 8+)
    static bool ValidarMetodo(string metodo) =>
        metodo is "PIX" or "CARTAO" or "BOLETO";
}

public interface ICancelavel
{
    bool Cancelar(string motivo);
}

// Implementa múltiplas interfaces
public class Pedido : IPagavel, ICancelavel
{
    public string Id     { get; } = Guid.NewGuid().ToString("N")[..8];
    public decimal Valor { get; }
    private bool  _cancelado;

    public Pedido(decimal valor) => Valor = valor;

    public decimal CalcularTotal()                    => Valor * 1.10m;
    public void ProcessarPagamento(string metodo)     => Console.WriteLine($"Pedido {Id} pago via {metodo}");
    public bool Cancelar(string motivo)
    {
        if (_cancelado) return false;
        _cancelado = true;
        return true;
    }
}
```

O detalhe mais importante aqui não é a sintaxe. É o efeito arquitetural:

- qualquer código que precise apenas "processar algo pagável" pode depender de `IPagavel`;
- esse código não precisa conhecer a classe concreta `Pedido`;
- amanhã você pode introduzir `Assinatura`, `Fatura`, `CompraInternacional` ou `OrdemDeServico` sem mudar o consumidor, desde que todos implementem o mesmo contrato.

Esse princípio aparece o tempo todo em código profissional: logging, autenticação, persistência, mensageria, notificações, estratégias de cálculo, gateways externos, cache e integração com APIs.

Exemplo de consumo por interface:

```csharp
public static class CheckoutService
{
    public static void Finalizar(IPagavel pagavel, string metodo)
    {
        if (!IPagavel.ValidarMetodo(metodo))
            throw new ArgumentException("Método inválido.", nameof(metodo));

        Console.WriteLine($"Total: {pagavel.CalcularTotal():C}");
        pagavel.ProcessarPagamento(metodo);
    }
}

IPagavel pedido = new Pedido(199.90m);
CheckoutService.Finalizar(pedido, "PIX");
```

Perceba o papel da variável `IPagavel pedido`: quando você usa uma referência do tipo interface, o compilador deixa acessível apenas o que faz parte do contrato. Isso reduz acoplamento e força o consumidor a depender somente do que foi prometido pela abstração.

#### O que uma interface pode ter hoje?

[⬆️ Voltar ao Sumário](#sumário)

Nas versões modernas do C#, interfaces deixaram de ser "apenas assinaturas vazias". Segundo a documentação oficial, elas continuam sendo contratos, mas podem ter recursos avançados:

- membros abstratos tradicionais;
- implementações padrão (`default interface members`);
- membros estáticos;
- membros `static abstract`, úteis para contratos estáticos em generics.

Exemplo conceitual de contrato estático:

```csharp
public interface IParsableId<TSelf> where TSelf : IParsableId<TSelf>
{
    static abstract TSelf Parse(string value);
}
```

Esse tipo de recurso aparece em cenários mais avançados, como matemática genérica, factories tipadas e APIs orientadas a constraints. Para júnior e pleno, o mais importante é entender que a ideia central da interface continua sendo: **definir um contrato reutilizável e implementável por múltiplos tipos**.

#### Implementação implícita e explícita

[⬆️ Voltar ao Sumário](#sumário)

Na forma mais comum, a implementação é **implícita**: os membros são públicos e ficam disponíveis diretamente na classe.

Mas há casos em que duas interfaces possuem membros com a mesma assinatura e semânticas diferentes. Nessa hora, entra a **implementação explícita**:

```csharp
public interface IMetric
{
    double GetDistance(); // metros
}

public interface IImperial
{
    double GetDistance(); // pés
}

public class Distancia : IMetric, IImperial
{
    private readonly double _metros;

    public Distancia(double metros) => _metros = metros;

    double IMetric.GetDistance()   => _metros;
    double IImperial.GetDistance() => _metros * 3.28084;
}

var distancia = new Distancia(10);
Console.WriteLine(((IMetric)distancia).GetDistance());   // 10
Console.WriteLine(((IImperial)distancia).GetDistance()); // 32.8084
```

Quando a implementação é explícita:

- o membro não aparece diretamente na API pública da classe;
- ele só pode ser acessado por uma referência da interface;
- isso ajuda a resolver conflitos ou a esconder detalhes que só fazem sentido dentro do contrato.

**Classe abstrata vs Interface em C#:**

| Característica | Classe abstrata | Interface |
|---|---|---|
| Herança múltipla | Não (apenas uma) | Sim (várias) |
| Campos de instância | Sim | Não |
| Construtores | Sim | Não |
| Métodos com implementação | Sim | Sim (`default`, C# 8+) |
| Modificadores de acesso | Qualquer | `public` por padrão |
| Quando usar | Relação "é um", compartilhar estado e comportamento | Contrato de comportamento |

Regras práticas para decidir:

- Use **classe abstrata** quando vários tipos compartilham estado, invariantes e implementação base.
- Use **interface** quando o foco é capacidade, desacoplamento e composição arquitetural.
- Evite criar interface "por reflexo" para toda classe. Interface faz sentido quando há mais de uma implementação possível, quando você quer desacoplar consumidores, ou quando a abstração melhora o desenho do sistema.

Em termos de arquitetura, interface é um dos recursos mais importantes do C# para aplicar **DIP (Dependency Inversion Principle)**, testes automatizados e substituição de infraestrutura sem reescrever o domínio.

**Como interpretar o exemplo:** A forca da interface aparece quando o consumidor passa a conhecer a capacidade e nao a classe concreta. Esse movimento reduz acoplamento, melhora testabilidade e facilita evolucao do sistema sem reescrever quem usa a API.

---

## Parte 13 — Delegates, Events e Lambdas

[⬆️ Voltar ao Sumário](#sumário)

---

### 13.1 Delegates — ponteiros de método tipados

[⬆️ Voltar ao Sumário](#sumário)

Um **delegate** é um tipo que representa uma referência a um método com uma assinatura específica. É a base do sistema de eventos e lambdas em C#.

```csharp
// Declaração de um tipo delegate
public delegate int Operacao(int a, int b);

// Uso
int Somar(int a, int b) => a + b;
int Subtrair(int a, int b) => a - b;

Operacao op = Somar;
Console.WriteLine(op(5, 3));  // 8

op = Subtrair;
Console.WriteLine(op(5, 3));  // 2

// Multicast delegate — encadeia múltiplos métodos
public delegate void Notificador(string mensagem);

void LogConsole(string msg) => Console.WriteLine($"Console: {msg}");
void LogArquivo(string msg) => Console.WriteLine($"Arquivo: {msg}");

Notificador notificar = LogConsole;
notificar += LogArquivo;  // encadeia
notificar("Evento ocorreu"); // chama os dois

notificar -= LogConsole; // remove
```

**Como interpretar o exemplo:** Delegate e a forma que C# usa para tratar comportamento como valor de primeira classe, mas com seguranca de assinatura. O exemplo de multicast mostra que, alem de apontar para um metodo, um delegate pode representar uma cadeia de callbacks executados em sequencia.

---

### 13.2 Func, Action e Predicate

[⬆️ Voltar ao Sumário](#sumário)

C# fornece delegates genéricos pré-definidos para os casos mais comuns:

| Delegate | Assinatura | Descrição |
|---|---|---|
| `Func<T, TResult>` | `TResult Invoke(T arg)` | Transforma T em TResult |
| `Func<T1, T2, TResult>` | `TResult Invoke(T1, T2)` | Transforma T1+T2 em TResult |
| `Action<T>` | `void Invoke(T arg)` | Consome T sem retorno |
| `Action<T1, T2>` | `void Invoke(T1, T2)` | Consome T1+T2 sem retorno |
| `Predicate<T>` | `bool Invoke(T arg)` | Testa condição sobre T |

```csharp
// Func — transforma
Func<string, int>    tamanho   = s => s.Length;
Func<int, int, int>  somar     = (a, b) => a + b;
Func<int>            aleatorio = () => new Random().Next(100);

// Action — consome
Action<string>       imprimir  = s => Console.WriteLine(s);
Action<string, int>  log       = (msg, nivel) => Console.WriteLine($"[{nivel}] {msg}");

// Predicate
Predicate<int>       positivo  = n => n > 0;
bool r = positivo(-5); // false
```

**Como interpretar o exemplo:** Esses delegates genericos existem para evitar a criacao de tipos customizados em cenarios muito comuns. Quando voce domina `Func`, `Action` e `Predicate`, passa a ler com mais naturalidade APIs modernas do .NET e do LINQ.

---

### 13.3 Expressões Lambda

[⬆️ Voltar ao Sumário](#sumário)

```csharp
// Lambda de expressão — uma linha
Func<int, int>    dobrar   = x => x * 2;
Func<int, bool>   positivo = x => x > 0;
Action<string>    imprimir = msg => Console.WriteLine(msg);

// Lambda com bloco
Func<string, string> formatar = texto =>
{
    texto = texto.Trim().ToLower();
    return char.ToUpper(texto[0]) + texto[1..];
};

// Lambda com múltiplos parâmetros
Func<int, int, int> somar = (a, b) => a + b;

// Lambda sem parâmetros
Func<DateTime> agora = () => DateTime.Now;
```

Aqui o `=>` já está no segundo sentido importante de C#: **lambda expression**.

Agora a leitura mental não é mais "este método devolve isto". A leitura passa a ser:

- o lado esquerdo diz **quais entradas a função recebe**
- o lado direito diz **o que essa função faz ou devolve**

Exemplos:

```csharp
x => x * 2
```

significa:

- receba um valor chamado `x`
- devolva `x * 2`

Já:

```csharp
(a, b) => a + b
```

significa:

- receba `a` e `b`
- devolva a soma

E:

```csharp
() => DateTime.Now
```

significa:

- não receba parâmetro nenhum
- devolva a data/hora atual

Uma equivalência útil:

```csharp
Func<int, int> dobrar = x => x * 2;
```

é conceitualmente parecido com:

```csharp
int Dobrar(int x)
{
    return x * 2;
}
```

mas, na lambda, essa função fica inline, no próprio ponto de uso.

Quando a lógica não cabe bem em uma expressão curta, você pode usar lambda com bloco:

```csharp
Func<string, string> formatar = texto =>
{
    string limpo = texto.Trim();
    return limpo.ToUpperInvariant();
};
```

Essa forma ajuda a enxergar outra diferença importante:

- em **expression-bodied members**, o `=>` pertence à definição de um membro da classe
- em **lambdas**, o `=>` pertence à definição de uma função anônima inline

**Como interpretar o exemplo:** Lambda e a forma compacta de escrever uma funcao anonima no ponto em que ela e usada. O valor real aparece quando voce percebe que ela aproxima o codigo da intencao, sem obrigar a nomear um metodo auxiliar para algo muito local.

**Regra prática:** use lambda quando o comportamento é pequeno, local e faz mais sentido perto do uso. Se a lógica cresce, ganha nome próprio ou precisa ser reutilizada em muitos lugares, transformar aquilo em método normal costuma melhorar a manutenção.

---

### 13.4 Eventos (Events)

[⬆️ Voltar ao Sumário](#sumário)

Eventos são delegates com encapsulamento adicional — seguem o padrão publicador/assinante (publisher/subscriber).

```csharp
public class Botao
{
    // Declara o evento baseado no delegate EventHandler
    public event EventHandler? Clicado;

    // Método que dispara o evento
    protected virtual void OnClicado()
    {
        Clicado?.Invoke(this, EventArgs.Empty); // ?. evita NullReferenceException
    }

    public void Clicar() => OnClicado();
}

// Evento com dados customizados
public class BotaoComDados
{
    public event EventHandler<string>? MensagemEnviada;

    public void Enviar(string msg) =>
        MensagemEnviada?.Invoke(this, msg);
}

// Assinando e cancelando assinatura
var botao = new Botao();

void Handler(object? sender, EventArgs e) =>
    Console.WriteLine("Botão clicado!");

botao.Clicado += Handler;  // assina
botao.Clicar();            // "Botão clicado!"
botao.Clicado -= Handler;  // cancela assinatura
```

**Como interpretar o exemplo:** `event` encapsula um delegate para permitir inscricao e cancelamento sem liberar disparo externo arbitrario. Assim, o publicador controla quando algo aconteceu, e os assinantes controlam apenas como reagir.

---

## Parte 14 — LINQ (Language Integrated Query)

[⬆️ Voltar ao Sumário](#sumário)

---

### 14.1 O que é LINQ?

[⬆️ Voltar ao Sumário](#sumário)

LINQ significa **Language Integrated Query**. O nome é importante: não é só uma biblioteca, nem só uma sintaxe. LINQ é a combinação de:

- recursos da linguagem C#;
- extension methods;
- lambdas;
- inferência de tipos;
- tipos de sequência como `IEnumerable<T>` e `IQueryable<T>`;
- operadores padrão definidos em `System.Linq`.

Segundo a documentação oficial, LINQ integra capacidades de consulta diretamente à linguagem, em vez de obrigar você a trabalhar com strings soltas sem validação em tempo de compilação. Isso torna consultas mais seguras, refatoráveis e compatíveis com IntelliSense.

O modelo mental mais útil é este:

1. você tem uma **fonte de dados**;
2. você descreve uma **query**;
3. a query é **executada** quando seus resultados são realmente necessários.

LINQ funciona tanto para dados em memória quanto para dados remotos:

- com `IEnumerable<T>`, normalmente estamos falando de **LINQ to Objects**;
- com `IQueryable<T>`, geralmente estamos falando de uma query que pode ser **traduzida** por um provider, como SQL via Entity Framework.

Outro detalhe importante da documentação oficial: a sintaxe de query (`from`, `where`, `select`) é convertida pelo compilador para chamadas equivalentes de método. Ou seja, a linguagem oferece duas formas de expressar a mesma ideia.

```csharp
var numeros = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

// Sintaxe de método (fluent) — preferida
var resultado = numeros
    .Where(n => n % 2 == 0)         // [2, 4, 6, 8, 10]
    .Select(n => n * n)             // [4, 16, 36, 64, 100]
    .Where(n => n > 20)             // [36, 64, 100]
    .ToList();

// Sintaxe de query — mais próxima do SQL
var resultado2 = (from n in numeros
                  where n % 2 == 0
                  let quadrado = n * n
                  where quadrado > 20
                  orderby quadrado
                  select quadrado).ToList();
```

As duas formas acima descrevem um **pipeline de transformação**:

- `Where` filtra;
- `Select` projeta;
- `OrderBy` ordena;
- `ToList` materializa o resultado.

Na prática profissional, a sintaxe de método costuma ser preferida porque compõe melhor com APIs modernas. A sintaxe de query continua sendo muito útil em consultas com múltiplos `from`, `join`, `group` e `let`, onde a legibilidade fica mais próxima de SQL.

Também é essencial entender que LINQ não é apenas "a maneira bonita de iterar listas". Ele é uma das principais ferramentas de modelagem de fluxo de dados em C#:

- transforma coleções;
- expressa regras de negócio de forma declarativa;
- reduz loops imperativos repetitivos;
- pode ser otimizado por provedores externos.

**Como interpretar o exemplo:** O ponto central e enxergar a query como pipeline declarativo: fonte, operadores e momento de materializacao. Quando voce pensa assim, LINQ deixa de parecer magica elegante e passa a ser uma forma precisa de modelar transformacao de dados.

---

### 14.2 Operadores LINQ principais

[⬆️ Voltar ao Sumário](#sumário)

Os operadores do LINQ podem ser entendidos por categoria. Isso ajuda mais do que decorar nomes isolados.

```csharp
var numeros = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
var nomes = new List<string> { "Ana", "Bruno", "Carla", "Diego", "Eva" };

// Filtro e transformação
var maioresQue3 = nomes.Where(n => n.Length > 3).ToList();         // Bruno, Carla, Diego
var maiusculos  = nomes.Select(n => n.ToUpper()).ToList();          // ANA, BRUNO...
var tamanhos    = nomes.Select(n => new { Nome = n, Tam = n.Length }); // tipo anônimo

// Ordenação
var ordenados    = nomes.OrderBy(n => n).ToList();
var decrescente  = nomes.OrderByDescending(n => n.Length).ToList();
var multiOrdem   = nomes.OrderBy(n => n.Length).ThenBy(n => n).ToList();

// Agregação
int total        = numeros.Sum();
double media     = numeros.Average();
int maximo       = numeros.Max();
int minimo       = numeros.Min();
long contagem    = numeros.Count(n => n > 5); // 5

// Verificação
bool algum  = nomes.Any(n => n.StartsWith("A"));    // true
bool todos  = nomes.All(n => n.Length > 2);         // true
bool nenhum = !nomes.Any(n => n.Length > 10);       // true

// Busca
string? primeira = nomes.FirstOrDefault(n => n.Length == 3); // "Ana"
string? unica    = nomes.SingleOrDefault(n => n == "Eva");    // "Eva"
// Single lança exceção se houver mais de um resultado

// Agrupamento
var porTamanho = nomes
    .GroupBy(n => n.Length)
    .ToDictionary(g => g.Key, g => g.ToList());
// { 3: ["Ana", "Eva"], 5: ["Bruno", "Carla", "Diego"] }

// Junção
var pedidos = new[] { new { Id = 1, NomeCliente = "Ana" } };
var clientes = new[] { new { Nome = "Ana", Email = "ana@email.com" } };

var join = pedidos.Join(
    clientes,
    p => p.NomeCliente,
    c => c.Nome,
    (p, c) => new { p.Id, c.Email }
);

// Conversão
int[]      array    = nomes.ToArray();
List<string> lista  = nomes.ToList();
var mapa            = nomes.ToDictionary(n => n, n => n.Length);
HashSet<string> set = nomes.ToHashSet();

// Paginação
var pagina2 = nomes.Skip(2).Take(2).ToList(); // ["Carla", "Diego"]

// Flatten (SelectMany)
var listas = new List<List<int>> { new(){1,2}, new(){3,4}, new(){5} };
var flat   = listas.SelectMany(l => l).ToList(); // [1,2,3,4,5]
```

| Operador | Tipo | O que faz |
|---|---|---|
| `Where` | Filtro | Mantém elementos que satisfazem a condição |
| `Select` | Projeção | Transforma cada elemento |
| `SelectMany` | Projeção | Achata coleções aninhadas |
| `OrderBy/ThenBy` | Ordenação | Ordena por critério |
| `GroupBy` | Agrupamento | Agrupa por chave |
| `Join` | Junção | Combina duas coleções por chave |
| `Any/All` | Quantificação | Verifica condição |
| `First/Last/Single` | Elemento | Obtém elemento específico |
| `Sum/Count/Min/Max/Average` | Agregação | Reduz para valor único |
| `Skip/Take` | Partição | Paginação |
| `Distinct` | Conjunto | Remove duplicatas |
| `Union/Intersect/Except` | Conjunto | Operações de conjunto |

Leitura prática das principais categorias:

- **Filtro:** `Where` responde "quais itens permanecem?"
- **Projeção:** `Select` responde "em que formato quero cada item?"
- **Projeção achatada:** `SelectMany` responde "quero uma sequência única a partir de várias internas".
- **Ordenação:** `OrderBy`, `ThenBy` organizam o resultado.
- **Agrupamento:** `GroupBy` cria grupos por chave.
- **Agregação:** `Sum`, `Count`, `Average`, `Max`, `Min` reduzem uma sequência a um valor.
- **Busca pontual:** `First`, `FirstOrDefault`, `Single`, `SingleOrDefault` procuram um ou poucos elementos.
- **Conjunto:** `Distinct`, `Union`, `Intersect`, `Except` trabalham com semântica de coleção matemática.

Algumas diferenças críticas que todo engenheiro C# precisa dominar:

- `Any()` costuma ser preferível a `Count() > 0` quando você só quer saber se existe ao menos um elemento.
- `FirstOrDefault()` aceita zero ou mais resultados; ele só devolve o primeiro.
- `SingleOrDefault()` exige no máximo um resultado; se houver dois, isso geralmente indica erro de regra de negócio.
- `OrderBy()` parece barato na escrita, mas frequentemente precisa ler toda a fonte para só então produzir o primeiro item.

Em outras palavras: LINQ deixa o código conciso, mas não elimina o custo computacional. Código declarativo continua tendo custo real.

**Como interpretar o exemplo:** A categoria de cada operador importa mais do que decorar todas as assinaturas. Quem domina filtro, projecao, agregacao, busca, agrupamento e conjunto consegue ler quase qualquer pipeline LINQ com rapidez.

---

### 14.3 `IEnumerable<T>` e o contrato fundamental das sequências

[⬆️ Voltar ao Sumário](#sumário)

`IEnumerable<T>` é uma das interfaces mais importantes de todo o ecossistema .NET. A documentação oficial a descreve como a interface base das coleções genéricas que podem ser enumeradas com `foreach`.

O que isso significa, de forma direta?

- `IEnumerable<T>` não diz que a estrutura é lista, array, banco, fila ou conjunto.
- `IEnumerable<T>` não promete indexação.
- `IEnumerable<T>` não promete mutabilidade nem imutabilidade.
- `IEnumerable<T>` promete apenas uma coisa: **"eu consigo fornecer elementos de tipo `T` em sequência"**.

Por baixo dos panos, isso acontece via um enumerador:

- `GetEnumerator()`
- `MoveNext()`
- `Current`

É justamente esse contrato que permite que `foreach` funcione.

Exemplo de implementação com `yield return`:

```csharp
public static IEnumerable<int> NumerosParesAte(int limite)
{
    for (int i = 0; i <= limite; i++)
    {
        if (i % 2 == 0)
            yield return i;
    }
}

foreach (int numero in NumerosParesAte(10))
    Console.WriteLine(numero);
```

O `yield return` é importante porque mostra que uma sequência pode ser **gerada sob demanda**, em vez de completamente construída antes do uso.

#### Quando expor `IEnumerable<T>`?

[⬆️ Voltar ao Sumário](#sumário)

Use `IEnumerable<T>` como tipo de retorno quando:

- o consumidor só precisa iterar;
- você quer esconder a estrutura concreta usada internamente;
- você quer dar flexibilidade para trocar `List<T>`, array, generator ou outra fonte sem quebrar a API.

Exemplo:

```csharp
public sealed class CatalogoProdutos
{
    private readonly List<string> _produtos = new();

    public void Adicionar(string produto) => _produtos.Add(produto);

    public IEnumerable<string> Listar() => _produtos;
}
```

Isso é melhor do que retornar `List<string>` quando o chamador não precisa de operações específicas da lista.

#### O que `IEnumerable<T>` não garante

[⬆️ Voltar ao Sumário](#sumário)

Essa distinção evita muitos bugs de design:

- `IEnumerable<T>` **não é sinônimo de coleção materializada**;
- `IEnumerable<T>` **não é sinônimo de read-only collection**;
- `IEnumerable<T>` **não é sinônimo de operação barata**.

Uma mesma sequência pode:

- recalcular resultados a cada enumeração;
- executar código com efeitos colaterais;
- representar algo potencialmente infinito;
- encapsular uma fonte cara de percorrer.

Armadilhas comuns:

- Enumerar a mesma sequência várias vezes sem perceber.
- Assumir que `Count()` é barato em qualquer `IEnumerable<T>`.
- Retornar `IEnumerable<T>` quando o consumidor precisa de índice e `Count`; nesses casos, `IReadOnlyList<T>` ou `IReadOnlyCollection<T>` podem comunicar melhor a intenção.

Em resumo: `IEnumerable<T>` é o contrato ideal para **sequência**, não necessariamente para **coleção rica**.

**Como interpretar o exemplo:** O exemplo com `yield return` reforca que sequencia nao e sinonimo de lista pronta. Em design de API, expor `IEnumerable<T>` e prometer iteracao, e nao indexacao, contagem barata ou armazenamento materializado.

---

### 14.4 `IQueryable<T>` e queries traduzíveis para outra fonte

[⬆️ Voltar ao Sumário](#sumário)

`IQueryable<T>` herda de `IEnumerable<T>`, mas representa algo conceitualmente diferente.

De acordo com a documentação oficial:

- queries sobre `IEnumerable<T>` são compiladas em **delegates**;
- queries sobre `IQueryable<T>` são compiladas em **expression trees**;
- o provider interpreta essa árvore de expressão e decide como executá-la.

Isso significa que `IQueryable<T>` não representa apenas "dados já disponíveis". Ele representa uma **consulta ainda descritiva**, que pode ser traduzida para outra linguagem ou motor de execução, como SQL.

Exemplo típico com Entity Framework:

```csharp
IQueryable<Usuario> ativos = contexto.Usuarios
    .Where(u => u.Ativo)
    .OrderBy(u => u.Nome);

List<Usuario> primeiros10 = ativos
    .Take(10)
    .ToList();
```

Nesse fluxo:

- `contexto.Usuarios` é um provider de query;
- `Where`, `OrderBy` e `Take` constroem uma árvore de expressão;
- `ToList()` força a execução;
- o provider pode traduzir isso para SQL.

#### Por que isso importa?

[⬆️ Voltar ao Sumário](#sumário)

Porque algumas operações são executadas de forma diferente dependendo da fonte:

- em `IEnumerable<T>`, a filtragem acontece em memória;
- em `IQueryable<T>`, a filtragem pode acontecer no banco;
- isso muda performance, volume de dados transferidos e até semântica de comparação.

#### Armadilhas reais com `IQueryable<T>`

[⬆️ Voltar ao Sumário](#sumário)

- Nem todo método C# pode ser traduzido pelo provider.
- Chamar `ToList()` cedo demais materializa dados cedo demais e corta a composição da query.
- Chamar `AsEnumerable()` muda o restante da pipeline para execução local.
- Retornar `IQueryable<T>` indiscriminadamente de camadas de aplicação pode vazar detalhes de persistência e criar consultas difíceis de controlar.

Exemplo de mudança de fronteira:

```csharp
var consultaBanco = contexto.Usuarios.Where(u => u.Ativo);   // IQueryable<Usuario>
var consultaMemoria = consultaBanco.AsEnumerable()           // IEnumerable<Usuario>
                                 .Where(u => u.Nome.Length > 3);
```

Depois de `AsEnumerable()`, o restante não é mais traduzido pelo provider; passa a rodar em memória.

Regra prática:

- use `IQueryable<T>` quando você está deliberadamente compondo consultas traduzíveis;
- use `IEnumerable<T>` quando o dado já foi materializado ou quando o contrato é somente de enumeração;
- não misture os dois sem saber em que lado da fronteira de execução você está.

**Como interpretar o exemplo:** `IQueryable<T>` muda a conversa porque a consulta deixa de ser apenas uma sequencia local e passa a ser uma descricao que outro provider pode reinterpretar, como SQL em um ORM. O exemplo existe justamente para treinar a pergunta: isso vai rodar no banco ou em memoria?

---

### 14.5 Execução adiada, materialização e armadilhas

[⬆️ Voltar ao Sumário](#sumário)

Uma das ideias mais importantes do LINQ é **deferred execution**: a query normalmente não roda quando você a escreve, e sim quando alguém consome seus resultados.

Exemplo:

```csharp
var origem = new List<int> { 1, 2, 3, 4, 5 };

var consulta = origem.Where(n =>
{
    Console.WriteLine($"Filtrando {n}");
    return n % 2 == 0;
});

Console.WriteLine("Query criada.");

foreach (int item in consulta)
    Console.WriteLine($"Resultado: {item}");
```

A saída mostra que a filtragem só acontece durante a enumeração.

#### Deferred vs immediate

[⬆️ Voltar ao Sumário](#sumário)

Operadores comuns por comportamento:

| Operador | Execução | Observação |
|---|---|---|
| `Where`, `Select`, `Skip`, `Take` | Adiada | Normalmente streaming |
| `OrderBy`, `GroupBy` | Adiada | Mas costumam precisar bufferizar antes de devolver resultados |
| `Count`, `Any`, `First`, `Single` | Imediata | Produzem valor escalar ou decisão imediata |
| `ToList`, `ToArray`, `ToDictionary` | Imediata | Materializam os resultados |

#### O que é materializar?

[⬆️ Voltar ao Sumário](#sumário)

Materializar é transformar a query em uma estrutura concreta, como:

- `List<T>`
- `T[]`
- `Dictionary<TKey, TValue>`
- `HashSet<T>`

Exemplo:

```csharp
IEnumerable<int> consulta = numeros.Where(n => n % 2 == 0);
List<int> lista = consulta.ToList(); // aqui os dados são efetivamente percorridos e copiados
```

Materializar é útil quando:

- você quer evitar múltiplas enumerações caras;
- você quer congelar o estado atual do resultado;
- você precisa de uma estrutura com indexação, contagem rápida ou reuso intensivo.

#### Armadilhas que aparecem em produção

[⬆️ Voltar ao Sumário](#sumário)

- Criar uma query acreditando que ela já executou.
- Enumerar a mesma query várias vezes e repetir trabalho sem perceber.
- Misturar lógica com efeito colateral dentro de `Where` ou `Select`.
- Em `IQueryable<T>`, chamar métodos não traduzíveis e descobrir o problema só em runtime.

Boas práticas:

- Use LINQ para descrever transformação de dados, não para esconder efeitos colaterais.
- Materialize conscientemente nas fronteiras certas.
- Quando a query fica opaca demais, quebre em variáveis intermediárias com nomes bons.
- Em contexto de banco de dados, sempre pense em **onde** o código está executando: servidor ou memória local.

---

## Parte 15 — Coleções

[⬆️ Voltar ao Sumário](#sumário)

**Como interpretar o exemplo:** Execucao adiada e uma das maiores fontes de surpresa em LINQ, porque escrever a query nao significa executa-la. O exercicio mental correto e sempre perguntar onde a pipeline e consumida e em que ponto o resultado vira dados concretos.

---

### 15.1 Tipos de coleções principais

[⬆️ Voltar ao Sumário](#sumário)

Escolher a coleção certa faz diferença em legibilidade, complexidade algorítmica e custo operacional. Em C#, a melhor escolha quase nunca é "a que eu lembro primeiro", e sim "a que comunica melhor a intenção do domínio e do acesso".

Antes dos tipos concretos, vale entender os contratos mais importantes:

```text
IEnumerable<T>            -> consigo enumerar itens em sequência
ICollection<T>            -> consigo enumerar + contar + adicionar/remover
IReadOnlyCollection<T>    -> consigo enumerar + Count, sem mutação exposta
IList<T>                  -> acesso por índice + mutação
IReadOnlyList<T>          -> acesso por índice sem mutação exposta
ISet<T>                   -> semântica de conjunto (unicidade)
IDictionary<TKey, TValue> -> mapeamento chave/valor
```

Agora sim, os tipos concretos mais comuns:

| Tipo | Estrutura mental | Pontos fortes | Cuidado principal |
|---|---|---|---|
| `T[]` | Array de tamanho fixo | Muito simples, rápido, ótimo para interop | Tamanho imutável |
| `List<T>` | Array dinâmico | Índice rápido, uso geral excelente | Inserções/remover no meio custam `O(n)` |
| `LinkedList<T>` | Nós encadeados | Inserções/remoções por nó | Péssimo para acesso por índice; raramente é a melhor escolha |
| `HashSet<T>` | Conjunto baseado em hash | Teste de pertencimento rápido, unicidade | Não preserva ordem |
| `SortedSet<T>` | Conjunto ordenado | Unicidade + ordenação | Operações tendem a ser `O(log n)` |
| `Queue<T>` | Fila FIFO | Processamento em ordem de chegada | Não é para acesso aleatório |
| `Stack<T>` | Pilha LIFO | Backtracking, undo, parsing | Semântica específica |
| `Dictionary<TKey,TValue>` | Tabela hash | Busca por chave muito rápida | Chave deve ser estável e bem comparável |
| `SortedDictionary<TKey,TValue>` | Mapa ordenado | Chaves sempre em ordem | Mais caro que `Dictionary` comum |
| `ConcurrentDictionary<TKey,TValue>` | Dicionário thread-safe | Concorrência | Não substitui desenho correto de sincronização |

Repare em um ponto sutil, porém crítico:

- `IEnumerable<T>` fala sobre **enumeração**.
- `List<T>` fala sobre **estrutura concreta**.
- `IReadOnlyList<T>` fala sobre **capacidade de leitura por índice**.

Confundir essas coisas leva a APIs mal desenhadas.

**Como interpretar o exemplo:** A secao separa contratos de colecao de implementacoes concretas porque esse erro aparece o tempo todo em APIs. Saber se voce esta prometendo iteracao, indice, cardinalidade, unicidade ou mapeamento por chave ja resolve boa parte das escolhas de forma honesta.

---

### 15.2 List\<T\>

[⬆️ Voltar ao Sumário](#sumário)

`List<T>` é a coleção "padrão ouro" do C# para a maioria dos cenários em memória. Internamente, ela funciona como um **array redimensionável**.

Isso traz uma combinação excelente para uso geral:

- leitura por índice em `O(1)`;
- iteração rápida;
- API conhecida e versátil;
- crescimento automático da capacidade.

O ponto importante é entender que esse crescimento tem custo eventual. Quando a capacidade interna é estourada, a lista precisa alocar um array maior e copiar os itens.

```csharp
var nomes = new List<string> { "Ana", "Bruno", "Carlos" };

nomes.Add("Diego");
nomes.Insert(1, "Eduardo"); // insere na posição 1
nomes.AddRange(new[] { "Fabi", "Gabi" }); // adiciona múltiplos

Console.WriteLine(nomes[0]);             // "Ana"
Console.WriteLine(nomes.Count);          // 6
Console.WriteLine(nomes.Contains("Ana")); // true
Console.WriteLine(nomes.IndexOf("Carlos")); // 2

nomes.Remove("Ana");    // remove por valor
nomes.RemoveAt(0);      // remove por índice
nomes.Sort();           // ordena in-place
nomes.Sort((a, b) => a.Length.CompareTo(b.Length)); // custom

// Capacidade inicial ajuda quando você já sabe aproximadamente o tamanho
var ids = new List<int>(capacity: 1000);

// Listas imutáveis
var imutavel = new List<string> { "A", "B" }.AsReadOnly();
// imutavel.Add("C"); // NotSupportedException
```

Quando `List<T>` é excelente:

- você precisa de uma sequência ordenada por posição;
- quer adicionar muito ao final;
- quer percorrer várias vezes;
- precisa de índice.

Quando `List<T>` não é a melhor escolha:

- você precisa apenas testar pertencimento com frequência: use `HashSet<T>`;
- você precisa mapear por chave: use `Dictionary<TKey,TValue>`;
- você precisa garantir imutabilidade compartilhada: considere coleções imutáveis;
- você expõe a lista só para leitura: talvez `IReadOnlyList<T>` comunique melhor a intenção da API.

Custos clássicos de `List<T>`:

- `Add` no final: amortizado `O(1)`;
- acesso por índice: `O(1)`;
- `Insert` no meio: `O(n)`;
- `RemoveAt` no meio: `O(n)`;
- busca por valor com `Contains`: `O(n)`.

**Como interpretar o exemplo:** `List<T>` e excelente porque equilibra simplicidade e desempenho para a maioria dos cenarios, mas continua sendo um array redimensionavel com custos especificos. Entender esses custos evita o habito ruim de trata-la como solucao universal so por ser familiar.

---

### 15.3 Dictionary\<TKey, TValue\>

[⬆️ Voltar ao Sumário](#sumário)

`Dictionary<TKey, TValue>` é a escolha natural quando você quer recuperar um valor a partir de uma chave.

Mentalmente, ele responde a perguntas como:

- "qual o usuário com este id?"
- "qual a configuração para esta feature?"
- "qual a quantidade em estoque deste SKU?"

Na maioria dos casos, a busca por chave é próxima de `O(1)`, porque a estrutura é baseada em hash. Mas esse desempenho depende de dois fatores:

- uma boa função de hash para a chave;
- uma regra de igualdade coerente.

```csharp
var estoque = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
{
    ["Teclado"]  = 15,
    ["Mouse"]    = 30,
    ["Monitor"]  = 8
};

Console.WriteLine(estoque["Mouse"]);                   // 30
// estoque["Fone"]                                     // KeyNotFoundException!
Console.WriteLine(estoque.GetValueOrDefault("Fone", 0)); // 0 — seguro

// TryGetValue — padrão seguro
if (estoque.TryGetValue("Teclado", out int qtd))
    Console.WriteLine(qtd);

// Adicionar/atualizar
estoque["Webcam"]    = 12;              // adiciona se não existe, atualiza se existe
estoque.TryAdd("Monitor", 99);          // adiciona apenas se não existir

// Iteração
foreach (var (chave, valor) in estoque) // desestruturação
    Console.WriteLine($"{chave}: {valor}");

// LINQ em dicionários
var caros = estoque.Where(kv => kv.Value > 10)
                   .ToDictionary(kv => kv.Key, kv => kv.Value);
```

O `StringComparer.OrdinalIgnoreCase` do exemplo é um detalhe de engenharia muito útil: ele faz o dicionário tratar `"mouse"` e `"Mouse"` como a mesma chave, com comparação ordinal e sem depender de cultura.

Boas práticas com `Dictionary<TKey, TValue>`:

- Prefira `TryGetValue` quando a chave pode não existir.
- Evite usar o indexador para leitura se a ausência da chave for esperada.
- Não use chaves mutáveis cujo valor lógico possa mudar enquanto elas estão no dicionário.
- Se a chave for um tipo customizado, garanta que `Equals` e `GetHashCode` sejam coerentes.

Armadilhas comuns:

- Tratar `Dictionary` como se fosse ordenado. Ele não é um tipo de ordenação semântica.
- Usar `ContainsKey` seguido de indexador em vez de um único `TryGetValue`.
- Expor `Dictionary` mutável para qualquer camada e perder controle sobre invariantes.

**Como interpretar o exemplo:** O foco do dicionario nao e ordem, e sim acesso rapido por chave. O exemplo tambem ensina que escolher o comparador correto e usar `TryGetValue` ou `GetValueOrDefault` costuma produzir codigo mais robusto do que assumir que a chave sempre existira.

---

### 15.4 Como escolher a coleção certa

[⬆️ Voltar ao Sumário](#sumário)

Se você decorar só uma parte deste capítulo, que seja esta tabela:

| Se você precisa... | Prefira... | Porque comunica melhor |
|---|---|---|
| Sequência simples para iteração | `IEnumerable<T>` | O consumidor só percorre |
| Sequência com `Count` garantido | `IReadOnlyCollection<T>` | Expõe cardinalidade sem mutação |
| Sequência com índice | `IReadOnlyList<T>` ou `List<T>` | Expõe acesso posicional |
| Estrutura de uso geral em memória | `List<T>` | Melhor custo/benefício geral |
| Pertencimento rápido sem duplicatas | `HashSet<T>` | Semântica de conjunto |
| Chave para valor | `Dictionary<TKey, TValue>` | Busca direta por chave |
| Ordem de processamento FIFO | `Queue<T>` | Semântica de fila |
| Ordem de processamento LIFO | `Stack<T>` | Semântica de pilha |
| Concorrência com chave/valor | `ConcurrentDictionary<TKey,TValue>` | API pensada para múltiplas threads |

Além da coleção concreta, pense no **tipo que sua API expõe**:

- Retorne `List<T>` quando o chamador realmente precisa dos comportamentos de lista.
- Retorne `IReadOnlyList<T>` quando ele só precisa ler por índice.
- Retorne `IEnumerable<T>` quando ele só precisa enumerar.
- Retorne `IQueryable<T>` apenas quando você quer compor uma query traduzível deliberadamente.

Essa distinção melhora encapsulamento e deixa o contrato mais honesto.

Em código maduro, escolher coleção não é detalhe. É parte do design.

---

## Parte 16 — Async/Await e Programação Assíncrona

[⬆️ Voltar ao Sumário](#sumário)

**Como interpretar o exemplo:** A tabela resume um principio central de design: escolha primeiro a semantica do acesso e so depois a estrutura concreta. A melhor colecao nao e a mais famosa, e sim a que comunica com honestidade o que o consumidor podera fazer com os dados.

---

### 16.1 O modelo assíncrono do C#

[⬆️ Voltar ao Sumário](#sumário)

C# possui suporte nativo e profundo para programação assíncrona com `async`/`await`. É uma das implementações mais elegantes dessa funcionalidade entre as linguagens modernas.

```csharp
using System.Net.Http;

// Método assíncrono — deve retornar Task, Task<T> ou ValueTask<T>
public async Task<string> BuscarDadosAsync(string url)
{
    using var cliente = new HttpClient();

    // await libera a thread atual enquanto aguarda o resultado
    // (não bloqueia — diferente de .Result que bloqueia)
    string resposta = await cliente.GetStringAsync(url);

    return resposta.ToUpper();
}

// Método assíncrono sem retorno
public async Task LogarAsync(string mensagem)
{
    await File.WriteAllTextAsync("log.txt", mensagem);
    Console.WriteLine($"Logado: {mensagem}");
}

// Consumindo
public async Task ExemploAsync()
{
    string dados = await BuscarDadosAsync("https://api.exemplo.com/dados");
    Console.WriteLine(dados);
}
```

**Como interpretar o exemplo:** O ganho de `async` e `await` nao e criar varias threads automaticamente, e sim permitir espera sem bloqueio da thread chamadora. Isso deixa o fluxo legivel como codigo sequencial, mas com comportamento muito melhor para operacoes de I/O.

---

### 16.2 Padrões de uso

[⬆️ Voltar ao Sumário](#sumário)

```csharp
// Executar múltiplas tasks em paralelo
public async Task ExemploParaleloAsync()
{
    Task<string> task1 = BuscarDadosAsync("https://api1.com");
    Task<string> task2 = BuscarDadosAsync("https://api2.com");

    // Aguarda ambas ao mesmo tempo (paralelo)
    string[] resultados = await Task.WhenAll(task1, task2);

    // Aguarda a primeira que completar
    string primeiraResposta = await Task.WhenAny(task1, task2)
                                        .ContinueWith(t => t.Result.Result);
}

// CancellationToken — permite cancelar operações assíncronas
public async Task OperacaoCancelavelAsync(CancellationToken ct = default)
{
    for (int i = 0; i < 100; i++)
    {
        ct.ThrowIfCancellationRequested(); // lança OperationCanceledException se cancelado
        await Task.Delay(100, ct);
        Console.WriteLine($"Progresso: {i + 1}%");
    }
}

// Uso com cancelamento
var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5)); // cancela em 5s
try
{
    await OperacaoCancelavelAsync(cts.Token);
}
catch (OperationCanceledException)
{
    Console.WriteLine("Operação cancelada.");
}

// ConfigureAwait(false) — evita retorno ao contexto original (útil em bibliotecas)
string dados = await BuscarDadosAsync("url").ConfigureAwait(false);
```

**Como interpretar o exemplo:** O trecho junta tres padroes muito comuns: coordenacao de multiplas tasks, cancelamento cooperativo e cuidado com contexto de sincronizacao. Quando esses conceitos ficam claros, o restante da programacao assincrona em C# passa a formar um modelo coerente.

---

### 16.3 Task vs ValueTask

[⬆️ Voltar ao Sumário](#sumário)

```csharp
// Task — aloca um objeto no heap; adequado para operações realmente assíncronas
public async Task<int> OperacaoAsync() { await Task.Delay(100); return 42; }

// ValueTask — evita alocação quando o resultado é frequentemente síncrono
public async ValueTask<int> OperacaoRapidaAsync(bool usarCache)
{
    if (usarCache) return 42; // caminho síncrono — sem alocação de Task
    await Task.Delay(100);
    return 42;
}
```

**Como interpretar o exemplo:** `ValueTask` existe para cenarios em que o caminho sincrono e frequente e a alocacao de `Task` se torna custo relevante, mas isso nao o transforma em escolha padrao. Em geral, prefira `Task` por simplicidade e so use `ValueTask` com beneficio mensuravel.

---

## Parte 17 — Generics

[⬆️ Voltar ao Sumário](#sumário)

---

### 17.1 Tipos parametrizados

[⬆️ Voltar ao Sumário](#sumário)

```csharp
// Classe genérica
public class Repositorio<T> where T : class
{
    private readonly List<T> _itens = new();

    public void Adicionar(T item)     => _itens.Add(item);
    public T?   Buscar(Predicate<T> criterio) => _itens.Find(criterio);
    public IEnumerable<T> BuscarTodos()  => _itens.AsReadOnly();
}

var repo = new Repositorio<Usuario>();
repo.Adicionar(new Usuario());
```

**Como interpretar o exemplo:** Generics permitem escrever uma estrutura ou algoritmo uma vez e ainda preservar seguranca de tipo em compile-time. O repositorio generico mostra justamente essa ideia: a logica e reaproveitada, mas o compilador continua sabendo qual tipo concreto esta sendo manipulado.

---

### 17.2 Constraints (restrições)

[⬆️ Voltar ao Sumário](#sumário)

```csharp
// where T : class          — T deve ser tipo de referência
// where T : struct         — T deve ser tipo de valor
// where T : new()          — T deve ter construtor sem parâmetros
// where T : NomeClasse     — T deve herdar de NomeClasse
// where T : IInterface     — T deve implementar a interface
// where T : notnull        — T não pode ser nullable
// where T : unmanaged      — T deve ser tipo não gerenciado (C# 7.3+)

public T Criar<T>() where T : new() => new T();

public void Processar<T>(T item)
    where T : class, IComparable<T>, new()
{
    // item é classe, comparável, e tem construtor sem parâmetros
}

// Método genérico
public static T PrimeiroOuPadrao<T>(IEnumerable<T> colecao, T valorPadrao = default!)
{
    foreach (var item in colecao)
        return item;
    return valorPadrao;
}
```

**Como interpretar o exemplo:** Constraint e a forma de limitar o universo dos tipos aceitos para que o corpo generico possa assumir certas capacidades. Em vez de `qualquer T`, o codigo passa a trabalhar com um `T` que, por contrato, e instanciavel, comparavel ou compativel com outra abstracao.

---

### 17.3 Covariância e contravariância

[⬆️ Voltar ao Sumário](#sumário)

```csharp
// Covariância (out) — pode usar tipo mais derivado
// Ex: IEnumerable<Cachorro> pode ser atribuído a IEnumerable<Animal>
IEnumerable<Cachorro> cachorros = new List<Cachorro>();
IEnumerable<Animal>   animais   = cachorros; // OK — covariante

// Contravariância (in) — pode usar tipo menos derivado
// Ex: Action<Animal> pode ser atribuído a Action<Cachorro>
Action<Animal>   processarAnimal   = a => Console.WriteLine(a.Nome);
Action<Cachorro> processarCachorro = processarAnimal; // OK — contravariante

// Interface covariante
public interface ILeitor<out T> { T Ler(); }

// Interface contravariante
public interface IEscritor<in T> { void Escrever(T item); }
```

---

## Parte 18 — Tratamento de Exceções

[⬆️ Voltar ao Sumário](#sumário)

**Como interpretar o exemplo:** A direcao da variancia fica clara quando voce pensa em fluxo de dados: quem produz valores pode ser covariante, e quem consome valores pode ser contravariante. Esse raciocinio evita decorar regras e ajuda a entender por que algumas atribuicoes sao seguras.

---

### 18.1 `try / catch / finally`

[⬆️ Voltar ao Sumário](#sumário)

```csharp
try
{
    int resultado = 10 / 0;
}
catch (DivideByZeroException ex)
{
    Console.WriteLine($"Divisão por zero: {ex.Message}");
}
catch (OverflowException ex) when (ex.Message.Contains("aritmético"))
{
    // 'when' — filtro de exceção (C# 6+): captura apenas se a condição for verdadeira
    Console.WriteLine("Overflow aritmético");
}
catch (Exception ex)
{
    Console.WriteLine($"Erro genérico: {ex.Message}");
    throw; // relança a exceção ORIGINAL (mantém o stack trace)
    // throw ex; // NÃO FAÇA — redefine o stack trace
}
finally
{
    Console.WriteLine("Executado sempre");
}
```

**Como interpretar o exemplo:** O bloco mostra a ordem correta de captura: excecoes especificas primeiro, generica por ultimo, e `finally` para limpeza obrigatoria. O detalhe de `throw;` versus `throw ex;` e vital porque afeta o stack trace e, portanto, a qualidade do diagnostico.

---

### 18.2 Exceções customizadas

[⬆️ Voltar ao Sumário](#sumário)

```csharp
// Exceção customizada
public class SaldoInsuficienteException : InvalidOperationException
{
    public double SaldoAtual      { get; }
    public double ValorSolicitado { get; }

    public SaldoInsuficienteException(double saldoAtual, double valorSolicitado)
        : base($"Saldo insuficiente. Atual: {saldoAtual:C} | Solicitado: {valorSolicitado:C}")
    {
        SaldoAtual      = saldoAtual;
        ValorSolicitado = valorSolicitado;
    }
}

// Hierarquia de exceções de domínio
public class DomainException : Exception
{
    public string Codigo { get; }
    public DomainException(string codigo, string mensagem) : base(mensagem)
        => Codigo = codigo;
}

public class NaoEncontradoException : DomainException
{
    public NaoEncontradoException(string entidade, object id)
        : base("NAO_ENCONTRADO", $"{entidade} com ID '{id}' não encontrado.") { }
}
```

**Como interpretar o exemplo:** Excecoes customizadas fazem mais sentido quando carregam contexto de dominio e nao apenas um nome diferente. Ao adicionar propriedades e uma hierarquia pensada, a excecao passa a servir tambem como estrutura util para observabilidade e tratamento especializado.

---

### 18.3 Hierarquia de exceções

[⬆️ Voltar ao Sumário](#sumário)

```
Exception
├── SystemException
│   ├── NullReferenceException    — acesso a referência nula
│   ├── IndexOutOfRangeException  — índice fora do array
│   ├── InvalidCastException      — cast inválido
│   ├── OverflowException         — overflow aritmético
│   ├── DivideByZeroException     — divisão por zero
│   ├── StackOverflowException    — recursão infinita (não capturável)
│   ├── OutOfMemoryException      — sem memória (raramente capturável)
│   └── InvalidOperationException — estado inválido para a operação
├── IOException
│   ├── FileNotFoundException
│   └── DirectoryNotFoundException
├── ArgumentException
│   ├── ArgumentNullException
│   └── ArgumentOutOfRangeException
└── ApplicationException (use DomainException customizada em vez desta)
```

---

## Parte 19 — Attributes (Annotations)

[⬆️ Voltar ao Sumário](#sumário)

**Como interpretar o exemplo:** A arvore ajuda a perceber que capturar excecao em C# tambem e decisao de modelagem: quanto mais alto voce captura, mais geral e menos especifico fica o tratamento. Conhecer as familias principais ajuda a escrever `catch` mais intencionais.

---

### 19.1 Attributes embutidos

[⬆️ Voltar ao Sumário](#sumário)

```csharp
public class Exemplo
{
    [Obsolete("Use NovoMetodo() em vez deste.", error: false)] // error: true lança erro de compilação
    public void MetodoAntigo() { }

    public void NovoMetodo() { }
}

// Suprime aviso específico do compilador
#pragma warning disable CS0618
MetodoAntigo();
#pragma warning restore CS0618

// Atributos de serialização
using System.Text.Json.Serialization;

public class Produto
{
    [JsonPropertyName("product_name")]  // nome no JSON
    public string Nome { get; set; }

    [JsonIgnore]                         // não serializa este campo
    public string Senha { get; set; }

    [JsonConverter(typeof(DateTimeConverter))] // conversor customizado
    public DateTime DataCriacao { get; set; }
}
```

**Como interpretar o exemplo:** Attribute e metadado declarativo: ele nao muda o comportamento por si so, mas informa a compiladores, bibliotecas e frameworks como tratar aquele membro. O exemplo mostra usos comuns como orientacao do compilador com `[Obsolete]` e orientacao de serializacao com `System.Text.Json`.

---

### 19.2 Criando Attributes customizados

[⬆️ Voltar ao Sumário](#sumário)

```csharp
using System;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class,
                AllowMultiple = false,
                Inherited = true)]
public class LogAttribute : Attribute
{
    public string Descricao { get; }
    public bool   Cronometrar { get; set; }

    public LogAttribute(string descricao) => Descricao = descricao;
}

// Uso
[Log("Operação de busca", Cronometrar = true)]
public Usuario BuscarUsuario(int id)
{
    // ...
    return null!;
}

// Lendo via Reflection em tempo de execução
var metodo = typeof(MinhaClasse).GetMethod("BuscarUsuario");
var attr   = metodo?.GetCustomAttribute<LogAttribute>();
if (attr != null)
    Console.WriteLine($"Log: {attr.Descricao}, Cronometrar: {attr.Cronometrar}");
```

**Como interpretar o exemplo:** Criar um atributo customizado e enriquecer o codigo com intencao que pode ser lida depois por reflexao, tooling ou infraestrutura propria. Esse padrao e valioso quando voce quer descrever comportamento de forma declarativa em vez de espalhar configuracoes manuais.

---

## Parte 20 — Tipos Especiais Modernos do C#

[⬆️ Voltar ao Sumário](#sumário)

---

### 20.1 Tuple e ValueTuple

[⬆️ Voltar ao Sumário](#sumário)

```csharp
// Tuple nomeada (ValueTuple — C# 7+)
(string Nome, int Idade) pessoa = ("Ana", 28);
Console.WriteLine(pessoa.Nome);  // "Ana"
Console.WriteLine(pessoa.Idade); // 28

// Desestruturação
var (nome, idade) = pessoa;
Console.WriteLine(nome); // "Ana"

// Retorno de múltiplos valores de método
public (double Minimo, double Maximo, double Media) Estatisticas(List<double> valores)
{
    return (valores.Min(), valores.Max(), valores.Average());
}

var (min, max, media) = Estatisticas(new List<double> { 1, 5, 3, 2, 4 });
```

**Como interpretar o exemplo:** Tuplas resolvem bem o problema de devolver ou agrupar poucos valores relacionados sem criar um tipo formal de imediato. O cuidado e que, quando a estrutura ganha significado proprio de dominio, um tipo nomeado costuma comunicar melhor.

---

### 20.2 Span\<T\> e Memory\<T\> — zero-allocation slicing

[⬆️ Voltar ao Sumário](#sumário)

```csharp
// Span<T> — fatia de memória sem alocação (stack only)
int[] array = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

Span<int> fatia = array.AsSpan(2, 5); // elementos 2 a 6, sem nova alocação
foreach (int n in fatia)
    Console.Write(n + " "); // 3 4 5 6 7

// Modificação afeta o array original (mesma memória)
fatia[0] = 99;
Console.WriteLine(array[2]); // 99

// String sem alocação
ReadOnlySpan<char> texto = "Olá, Mundo!".AsSpan(5, 5);
Console.WriteLine(texto.ToString()); // "Mundo"
```

**Como interpretar o exemplo:** `Span<T>` mostra que e possivel trabalhar com janelas de memoria sem copiar dados, algo muito util em parsing e processamento de buffers. A contrapartida e lidar com regras mais rigidas de tempo de vida e seguranca.

---

### 20.3 Sealed classes com Pattern Matching (como Discriminated Union)

[⬆️ Voltar ao Sumário](#sumário)

```csharp
// Hierarquia fechada representando resultado de operação
public abstract record Resultado<T>
{
    public sealed record Sucesso(T Valor) : Resultado<T>;
    public sealed record Falha(string Mensagem, Exception? Causa = null) : Resultado<T>;
}

// Uso com pattern matching
Resultado<int> resultado = new Resultado<int>.Sucesso(42);

string descricao = resultado switch
{
    Resultado<int>.Sucesso(var valor)   => $"OK: {valor}",
    Resultado<int>.Falha(var msg, _)    => $"ERRO: {msg}",
    _                                   => "Desconhecido"
};
```

**Como interpretar o exemplo:** A combinacao de `record`, tipos fechados e pattern matching cria uma forma elegante de modelar estados possiveis sem deixar brechas para formatos invalidos. O resultado e um fluxo de tratamento mais explicito e mais seguro.

---

## Parte 21 — Threads e Concorrência

[⬆️ Voltar ao Sumário](#sumário)

---

### 21.1 Thread básico e ThreadPool

[⬆️ Voltar ao Sumário](#sumário)

```csharp
using System.Threading;

// Criação direta de thread (evite — prefira Task/async)
Thread t = new Thread(() =>
{
    Console.WriteLine($"Thread ID: {Thread.CurrentThread.ManagedThreadId}");
});
t.Start();
t.Join(); // aguarda o término

// ThreadPool — pool gerenciado (preferível para trabalho curto)
ThreadPool.QueueUserWorkItem(_ =>
{
    Console.WriteLine("Executando no pool");
});
```

**Como interpretar o exemplo:** O exemplo existe mais como base conceitual do que como recomendacao diaria. Em aplicacoes modernas, criar `Thread` manualmente e raro; o aprendizado importante e entender a diferenca entre thread real, pool gerenciado e abstracoes mais altas.

---

### 21.2 Task Parallel Library (TPL)

[⬆️ Voltar ao Sumário](#sumário)

```csharp
using System.Threading.Tasks;

// Task simples
Task tarefa = Task.Run(() => Console.WriteLine("Executando"));
await tarefa;

// Task com retorno
Task<int> calculo = Task.Run(() =>
{
    int soma = 0;
    for (int i = 0; i < 1_000_000; i++) soma += i;
    return soma;
});
int resultado = await calculo;

// Parallel.For e Parallel.ForEach
Parallel.For(0, 10, i => Console.WriteLine($"Item {i}"));
Parallel.ForEach(nomes, nome => Processar(nome));

// PLINQ — LINQ paralelo
var resultados = numeros
    .AsParallel()
    .WithDegreeOfParallelism(4)
    .Where(n => n % 2 == 0)
    .Select(n => n * n)
    .ToList();
```

**Como interpretar o exemplo:** A TPL oferece uma camada mais expressiva para concorrencia e paralelismo do que a manipulacao manual de threads. O essencial e distinguir trabalho CPU-bound, onde `Parallel` e PLINQ ajudam, de I/O-bound, onde `async` continua sendo o modelo certo.

---

### 21.3 Sincronização

[⬆️ Voltar ao Sumário](#sumário)

```csharp
// lock — exclusão mútua (equivalente ao synchronized do Java)
private readonly object _lock = new object();
private int _contador = 0;

public void Incrementar()
{
    lock (_lock)
    {
        _contador++;
    }
}

// Interlocked — operações atômicas sem lock (mais eficiente)
private int _contadorAtomico = 0;
Interlocked.Increment(ref _contadorAtomico);
Interlocked.Add(ref _contadorAtomico, 5);

// SemaphoreSlim — limita o número de acessos simultâneos
private readonly SemaphoreSlim _semaforo = new SemaphoreSlim(3); // max 3 simultâneos

public async Task AcessarRecursoAsync()
{
    await _semaforo.WaitAsync();
    try
    {
        await OperacaoAsync();
    }
    finally
    {
        _semaforo.Release();
    }
}
```

---

## Parte 22 — Interoperabilidade e Recursos Avançados

[⬆️ Voltar ao Sumário](#sumário)

**Como interpretar o exemplo:** Sincronizacao existe para proteger estado compartilhado, mas cada ferramenta tem custo e semantica proprios. `lock`, `Interlocked` e `SemaphoreSlim` nao sao equivalentes; escolher a menor ferramenta que resolve o problema ajuda a manter corretude sem pagar complexidade desnecessaria.

---

### 22.1 Reflection

[⬆️ Voltar ao Sumário](#sumário)

```csharp
using System.Reflection;

Type tipo = typeof(ContaBancaria);

// Inspecionar membros
Console.WriteLine(tipo.Name);        // "ContaBancaria"
Console.WriteLine(tipo.FullName);    // "MeuNamespace.ContaBancaria"

// Listar propriedades
foreach (PropertyInfo prop in tipo.GetProperties())
    Console.WriteLine($"{prop.PropertyType.Name} {prop.Name}");

// Criar instância dinamicamente
object instancia = Activator.CreateInstance(tipo, "Ana", 1000.0)!;

// Invocar método por nome
MethodInfo metodo = tipo.GetMethod("Depositar")!;
metodo.Invoke(instancia, new object[] { 500.0 });
```

**Como interpretar o exemplo:** Reflection permite que o programa inspecione tipos e invoque membros dinamicamente em runtime, o que e poderoso para tooling, frameworks e integracao generica. O preco e perder parte da previsibilidade e da seguranca do codigo fortemente tipado.

---

### 22.2 Source Generators (C# 9+)

[⬆️ Voltar ao Sumário](#sumário)

Source Generators são compiladores que executam durante a build e geram código C# adicional automaticamente. Usados extensivamente em frameworks modernos (System.Text.Json, EF Core, Dapper, etc.) para eliminar Reflection em runtime.

```csharp
// Exemplo: JsonSerializerContext gerado automaticamente
[JsonSerializable(typeof(Usuario))]
[JsonSerializable(typeof(List<Usuario>))]
public partial class MeuJsonContext : JsonSerializerContext { }

// O compilador gera o código de serialização — sem Reflection em runtime
string json = JsonSerializer.Serialize(usuario, MeuJsonContext.Default.Usuario);
```

**Como interpretar o exemplo:** Source Generators deslocam trabalho para a compilacao, reduzindo reflexao e boilerplate em runtime. O exemplo do contexto JSON mostra justamente essa troca: em vez de descobrir tudo dinamicamente na execucao, o compilador ja produz o codigo especializado.

---

### 22.3 Unsafe code e ponteiros

[⬆️ Voltar ao Sumário](#sumário)

```csharp
// Requer /unsafe no compilador ou <AllowUnsafeBlocks>true</AllowUnsafeBlocks> no .csproj
unsafe
{
    int valor = 42;
    int* ponteiro = &valor; // endereço de memória
    Console.WriteLine(*ponteiro); // 42

    // Manipulação direta de arrays com ponteiro
    int[] array = { 1, 2, 3 };
    fixed (int* ptr = array)
    {
        for (int i = 0; i < array.Length; i++)
            Console.WriteLine(*(ptr + i));
    }
}
```

**Como interpretar o exemplo:** `unsafe` remove parte das protecoes que normalmente tornam C# seguro e previsivel. Isso so se justifica em interop, manipulaÃ§Ã£o de memoria ou otimizacoes muito especificas, porque o desenvolvedor passa a assumir riscos que o runtime costuma administrar.

---

## Resumo Geral — Conceitos Fundamentais

[⬆️ Voltar ao Sumário](#sumário)

| Conceito | Definição resumida |
|---|---|
| **CLR** | Runtime do .NET — executa IL/CIL sobre qualquer SO |
| **IL / CIL** | Código intermediário produzido pelo compilador Roslyn |
| **Tipo de valor** | Armazena dado diretamente; cópia na atribuição: `int`, `struct`, `enum` |
| **Tipo de referência** | Armazena endereço para objeto no heap: `class`, `string`, arrays |
| **Nullable type** | `T?` — tipo de valor que aceita null via `Nullable<T>` |
| **`const`** | Constante de compile-time; apenas primitivos e `string` |
| **`readonly`** | Campo atribuível somente na declaração ou construtor |
| **`sealed`** | Classe não herdável; método não sobrescrevível além deste ponto |
| **`abstract`** | Classe não instanciável; método sem corpo, subclasses devem implementar |
| **`virtual`/`override`** | Habilita polimorfismo em C# (métodos não são virtuais por padrão) |
| **`this`** | Referência para a instância atual |
| **`base`** | Referência para a classe base |
| **`is`/`as`** | Verifica tipo (is) e faz cast seguro (as) |
| **Property** | Encapsula get/set como construção nativa da linguagem |
| **`var`** | Inferência de tipo local — tipo fixo em compile-time |
| **`using`** | Import de namespace ou gerenciamento de recursos (IDisposable) |
| **`internal`** | Visível apenas dentro do mesmo assembly (≈ package-private do Java) |
| **`protected internal`** | Visível no assembly + subclasses de qualquer assembly |
| **`private protected`** | Visível apenas em subclasses dentro do mesmo assembly |
| **Interface** | Contrato de comportamento desacoplado da implementação concreta |
| **Delegate** | Tipo que representa referência a método com assinatura específica |
| **Event** | Delegate encapsulado para padrão publisher/subscriber |
| **Lambda** | Implementação compacta inline de delegate / interface funcional |
| **`IEnumerable<T>`** | Contrato básico de sequência enumerável, ideal para iteração |
| **`IQueryable<T>`** | Contrato de query traduzível por um provider externo |
| **LINQ** | Modelo de consulta integrada à linguagem sobre sequências e providers |
| **`async`/`await`** | Programação assíncrona não-bloqueante nativa |
| **`Task<T>`** | Representa operação assíncrona com resultado |
| **Record** | Tipo imutável com igualdade por valor, `ToString` e `with` automáticos |
| **Tuple** | Agrupamento de valores nomeados sem criar classe |
| **Pattern Matching** | Inspeção e desestruturação de tipos em `is` e `switch` |
| **`lock`** | Exclusão mútua para um bloco de código (≈ `synchronized` do Java) |
| **`Span<T>`** | Fatia de memória sem alocação |
| **Extension Method** | Método adicionado a tipo existente sem herança |
| **Generics** | Tipos parametrizados com segurança em compile-time |
| **Attribute** | Metadados declarativos processados por compilador, runtime ou framework |
| **Reflection** | Inspeção e invocação dinâmica de tipos em runtime |

---

## Parte 23 — C# no Contexto de Game Development

[⬆️ Voltar ao Sumário](#sumário)

---

### 23.1 C# e Unity — a combinação dominante

[⬆️ Voltar ao Sumário](#sumário)

**Unity** é o motor de jogos mais utilizado no mundo para jogos mobile, indie e AA. C# é a **única linguagem de scripting oficial** do Unity. Compreender C# bem é pré-requisito direto para trabalhar com Unity.

Unity utiliza uma versão do runtime .NET chamada **Mono** (em builds legadas) e **IL2CPP** para builds de plataformas como iOS e consoles. IL2CPP converte o IL do .NET para C++ nativo antes da compilação, eliminando a necessidade da JIT no dispositivo alvo.

```
Código C# → IL/CIL → IL2CPP → C++ → binário nativo da plataforma
```

**Como interpretar o exemplo:** O pipeline IL para IL2CPP mostra que o C# de Unity continua sendo C#, mas executado sob restricoes e etapas de build muito particulares do engine. Essa diferenca explica por que certos habitos da linguagem convivem com preocupacoes de AOT, GC e frame loop.

---

### 23.2 MonoBehaviour — a classe base dos scripts Unity

[⬆️ Voltar ao Sumário](#sumário)

Todo script que você cria no Unity herda de `MonoBehaviour`, que fornece o ciclo de vida do game object.

```csharp
using UnityEngine;

// Todo script Unity herda de MonoBehaviour
public class Jogador : MonoBehaviour
{
    // ─── CAMPOS SERIALIZÁVEIS ─────────────────────────────────────────────
    // [SerializeField] expõe o campo privado no Inspector da Unity
    [SerializeField] private float _velocidade    = 5f;
    [SerializeField] private float _velocidadePulo = 8f;
    [SerializeField] private Transform _ponto de chao; // referência a outro objeto

    // Campos públicos também aparecem no Inspector (mas prefira [SerializeField] private)
    public int    Vida    = 100;
    public string NomeJogador;

    // ─── REFERÊNCIAS A COMPONENTES ────────────────────────────────────────
    private Rigidbody2D     _rb;
    private Animator        _animator;
    private SpriteRenderer  _sprite;

    // ─── AWAKE — chamado ao instanciar o objeto (mesmo se inativo) ────────
    private void Awake()
    {
        // Obtém componentes do mesmo GameObject — mais eficiente que GetComponent no Update
        _rb       = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _sprite   = GetComponent<SpriteRenderer>();
    }

    // ─── START — chamado no primeiro frame, após todos os Awake ──────────
    private void Start()
    {
        // Inicializações que dependem de outros objetos já inicializados
        Debug.Log($"Jogador '{NomeJogador}' inicializado.");
    }

    // ─── UPDATE — chamado todo frame ─────────────────────────────────────
    private void Update()
    {
        ProcessarEntrada();
        AtualizarAnimacao();
    }

    // ─── FIXEDUPDATE — chamado em intervalos fixos (física) ──────────────
    // Use para qualquer coisa relacionada a Rigidbody/física
    private void FixedUpdate()
    {
        Mover();
    }

    // ─── LATEUPDATE — chamado após todos os Update ────────────────────────
    // Útil para câmera que deve seguir o player após ele se mover
    private void LateUpdate()
    {
        // Lógica de câmera aqui
    }

    private void ProcessarEntrada()
    {
        // Input.GetAxisRaw retorna -1, 0 ou 1 sem suavização
        float horizontal = Input.GetAxisRaw("Horizontal");

        // Inverte o sprite conforme a direção
        if (horizontal != 0)
            _sprite.flipX = horizontal < 0;
    }

    private void Mover()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        _rb.velocity = new Vector2(horizontal * _velocidade, _rb.velocity.y);
    }

    private void AtualizarAnimacao()
    {
        bool estaMovendo = Mathf.Abs(_rb.velocity.x) > 0.1f;
        _animator.SetBool("EstaMovendo", estaMovendo);
    }

    // ─── COLISÕES ─────────────────────────────────────────────────────────
    private void OnCollisionEnter2D(Collision2D colisao)
    {
        if (colisao.gameObject.CompareTag("Inimigo"))
        {
            TomarDano(10);
        }
    }

    private void OnTriggerEnter2D(Collider2D outro)
    {
        if (outro.CompareTag("Coletavel"))
        {
            Destroy(outro.gameObject);
            // coletar item
        }
    }

    public void TomarDano(int dano)
    {
        Vida -= dano;
        if (Vida <= 0) Morrer();
    }

    private void Morrer()
    {
        Debug.Log("Game Over");
        // Destroy(gameObject); // destrói o objeto
    }
}
```

**Como interpretar o exemplo:** O script evidencia que, no Unity, o modelo de programacao e guiado pelo ciclo de vida da engine e pela serializacao do Inspector, nao por um `Main` tradicional. Ler `MonoBehaviour` corretamente significa entender o contrato entre seu codigo e o runtime do jogo.

---

### 23.3 Ciclo de vida do MonoBehaviour

[⬆️ Voltar ao Sumário](#sumário)

```
Instanciação
    │
    ▼
Awake()          — sempre chamado, mesmo se o componente estiver desabilitado
    │
    ▼
OnEnable()       — chamado quando o componente é habilitado
    │
    ▼
Start()          — chamado no primeiro frame em que o componente está ativo
    │
    ▼ ─────────────────────────────── LOOP DE JOGO ──────────────────────────
    │
    ▼
FixedUpdate()    — física (intervalo fixo, por padrão 0.02s / 50Hz)
    │
    ▼
Update()         — lógica de jogo (uma vez por frame)
    │
    ▼
LateUpdate()     — pós-update (câmera, ajustes finais)
    │
    ▼ ─────────────────────────────── FIM DO LOOP ───────────────────────────
    │
    ▼
OnDisable()      — quando o componente é desabilitado
    │
    ▼
OnDestroy()      — quando o objeto é destruído
```

**Como interpretar o exemplo:** O diagrama existe para responder uma pergunta pratica de todo iniciante em Unity: em qual metodo essa logica deve ficar? Quando voce entende o papel e o tempo de `Awake`, `Start`, `Update`, `FixedUpdate` e companhia, muitos bugs de timing deixam de parecer aleatorios.

---

### 23.4 ScriptableObject — dados desacoplados do GameObject

[⬆️ Voltar ao Sumário](#sumário)

`ScriptableObject` é um asset de dados reutilizável que vive fora da hierarquia de cenas. Ideal para configurações, estatísticas de personagens, itens, etc.

```csharp
using UnityEngine;

// CreateAssetMenu permite criar o asset pelo menu do Editor
[CreateAssetMenu(fileName = "NovoItem", menuName = "Jogo/Item")]
public class ItemDados : ScriptableObject
{
    [Header("Identificação")]
    public string NomeItem;
    public string Descricao;
    public Sprite Icone;

    [Header("Estatísticas")]
    public int     Dano;
    public int     Defesa;
    public float   Peso;
    public Raridade raridade;

    [Header("Comportamento")]
    public bool   Empilhavel    = true;
    public int    MaxPilha      = 99;
    public bool   Consumivel    = false;

    public enum Raridade { Comum, Raro, Epico, Lendario }
}

// Um script de personagem que usa o ScriptableObject
public class EquipamentoJogador : MonoBehaviour
{
    [SerializeField] private ItemDados _espada;
    [SerializeField] private ItemDados _armadura;

    private void Start()
    {
        int danoTotal = _espada.Dano + CalcularBonus();
        Debug.Log($"Equipando {_espada.NomeItem} — Dano: {danoTotal}");
    }

    private int CalcularBonus() => 0; // lógica de bonus
}
```

**Vantagens do ScriptableObject:**
- Dados compartilhados entre múltiplas instâncias sem duplicação
- Editável no Inspector sem código adicional
- Alterações em tempo de Play Mode **persistem** (ao contrário de campos de MonoBehaviour)
- Facilita balanceamento de jogo sem recompilar o código

**Como interpretar o exemplo:** O valor do `ScriptableObject` esta em separar dados de configuracao das instancias de cena, reduzindo duplicacao e acoplamento com `MonoBehaviour`. Em jogos, isso melhora reuso, balanceamento e o fluxo de trabalho entre programacao e design.

---

### 23.5 Coroutines — execução assíncrona sem async/await

[⬆️ Voltar ao Sumário](#sumário)

Coroutines são o mecanismo tradicional do Unity para execução temporizada e assíncrona **antes** do suporte completo a `async`/`await`.

```csharp
using System.Collections;
using UnityEngine;

public class EfeitosVisuais : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _sprite;

    // Coroutine — retorna IEnumerator e usa 'yield return' para pausar
    private IEnumerator PiscarAsync(int vezes, float intervalo)
    {
        for (int i = 0; i < vezes; i++)
        {
            _sprite.enabled = false;
            yield return new WaitForSeconds(intervalo); // aguarda N segundos
            _sprite.enabled = true;
            yield return new WaitForSeconds(intervalo);
        }
    }

    private IEnumerator FadeOut(float duracao)
    {
        float tempo = 0;
        Color corInicial = _sprite.color;

        while (tempo < duracao)
        {
            tempo += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, tempo / duracao);
            _sprite.color = new Color(corInicial.r, corInicial.g, corInicial.b, alpha);
            yield return null; // aguarda o próximo frame
        }

        _sprite.color = new Color(corInicial.r, corInicial.g, corInicial.b, 0f);
    }

    // Iniciando e parando coroutines
    public void AoTomarDano()
    {
        StartCoroutine(PiscarAsync(3, 0.1f));
    }

    public void AoMorrer()
    {
        StartCoroutine(FadeOut(1.5f));
    }

    // Tipos de yield em Unity:
    // yield return null;                       — aguarda 1 frame
    // yield return new WaitForSeconds(n);      — aguarda N segundos
    // yield return new WaitForFixedUpdate();   — aguarda FixedUpdate
    // yield return new WaitUntil(() => cond);  — aguarda condição ser verdadeira
    // yield return new WaitWhile(() => cond);  — aguarda enquanto condição verdadeira
    // yield return StartCoroutine(outra);      — aguarda outra coroutine terminar
}
```

**Como interpretar o exemplo:** Coroutine em Unity nao e thread nem `Task`; ela e uma rotina cooperativa pausada e retomada ao longo dos frames. Isso combina muito bem com efeitos temporizados, esperas condicionais e sequencias visuais integradas ao loop do jogo.

---

### 23.6 Unity Events e C# Events

[⬆️ Voltar ao Sumário](#sumário)

```csharp
using UnityEngine;
using UnityEngine.Events;

public class SistemaVida : MonoBehaviour
{
    [SerializeField] private int _vidaMaxima = 100;

    private int _vidaAtual;

    // UnityEvent — configurável no Inspector (arrastar métodos de outros objetos)
    [SerializeField] public UnityEvent             AoMorrer;
    [SerializeField] public UnityEvent<int>        AoTomarDano; // parâmetro visível no Inspector

    // C# event — mais performático, configurado via código
    public event Action<int, int>? VidaAlterada; // (vidaAtual, vidaMaxima)

    private void Awake() => _vidaAtual = _vidaMaxima;

    public void TomarDano(int dano)
    {
        _vidaAtual = Mathf.Max(0, _vidaAtual - dano);

        // Dispara ambos
        AoTomarDano.Invoke(dano);
        VidaAlterada?.Invoke(_vidaAtual, _vidaMaxima);

        if (_vidaAtual == 0)
            AoMorrer.Invoke();
    }
}

// Assinando via código
public class HUD : MonoBehaviour
{
    [SerializeField] private SistemaVida _vidaJogador;

    private void Awake()
    {
        _vidaJogador.VidaAlterada += AtualizarBarraDeVida;
    }

    private void OnDestroy()
    {
        // Sempre cancele a assinatura ao destruir para evitar memory leaks
        _vidaJogador.VidaAlterada -= AtualizarBarraDeVida;
    }

    private void AtualizarBarraDeVida(int atual, int maximo)
    {
        float percentual = (float)atual / maximo;
        // atualizar UI aqui
    }
}
```

**Como interpretar o exemplo:** O contraste entre `UnityEvent` e `event` do C# mostra duas filosofias uteis: uma voltada ao editor e outra ao codigo. Em times reais, a escolha depende tanto de arquitetura quanto do fluxo de trabalho entre programadores, UI e designers.

---

### 23.7 Boas práticas de performance no Unity

[⬆️ Voltar ao Sumário](#sumário)

Em Unity, otimização é crítica porque o jogo precisa rodar a 30–60+ FPS. C# oferece ferramentas específicas para isso.

**1. Caching de componentes:**

```csharp
// ERRADO — GetComponent tem custo; chamado todo frame
private void Update()
{
    GetComponent<Rigidbody2D>().velocity = Vector2.zero; // alocação + lookup todo frame
}

// CORRETO — cache no Awake
private Rigidbody2D _rb;
private void Awake() => _rb = GetComponent<Rigidbody2D>();
private void Update() => _rb.velocity = Vector2.zero; // direto ao campo
```

**2. Evitar alocações no loop de jogo:**

```csharp
// ERRADO — 'new' no Update aloca memória; gera GC (Garbage Collection) = stuttering
private void Update()
{
    Vector3 pos = new Vector3(x, y, z); // alocação todo frame
    string texto = "Vida: " + vida;     // string concatenation alloca
}

// CORRETO — reutilizar ou usar structs (tipos de valor)
// Vector3 é um struct — não aloca no heap
private Vector3 _posicaoTemp; // reutilizado

private void Update()
{
    _posicaoTemp.Set(x, y, z); // sem 'new'
    texto = $"Vida: {vida}";   // interpolação é ligeiramente melhor, mas ainda aloca
    // Para texto crítico, use StringBuilder com cache
}
```

**3. Comparação de tags sem alocação:**

```csharp
// ERRADO — gameObject.tag cria uma string (alocação)
if (colisao.gameObject.tag == "Inimigo") { }

// CORRETO — CompareTag não aloca
if (colisao.gameObject.CompareTag("Inimigo")) { }
```

**4. Object Pooling — reutilização de objetos:**

```csharp
using System.Collections.Generic;
using UnityEngine;

// Pool genérico para evitar Instantiate/Destroy frequentes
public class ObjectPool<T> where T : MonoBehaviour
{
    private readonly Queue<T> _pool = new();
    private readonly T        _prefab;
    private readonly Transform _parent;

    public ObjectPool(T prefab, int tamanhoInicial, Transform parent = null)
    {
        _prefab = prefab;
        _parent = parent;
        for (int i = 0; i < tamanhoInicial; i++)
            CriarNovoItem();
    }

    public T Obter()
    {
        if (_pool.Count == 0) CriarNovoItem();
        T item = _pool.Dequeue();
        item.gameObject.SetActive(true);
        return item;
    }

    public void Devolver(T item)
    {
        item.gameObject.SetActive(false);
        _pool.Enqueue(item);
    }

    private void CriarNovoItem()
    {
        T item = Object.Instantiate(_prefab, _parent);
        item.gameObject.SetActive(false);
        _pool.Enqueue(item);
    }
}

// Uso — pool de projéteis
public class AtirarProjeteis : MonoBehaviour
{
    [SerializeField] private Projetil _prefab;

    private ObjectPool<Projetil> _pool;

    private void Awake() =>
        _pool = new ObjectPool<Projetil>(_prefab, 20, transform);

    public void Atirar(Vector2 direcao)
    {
        Projetil projetil = _pool.Obter();
        projetil.Iniciar(direcao, _pool);
    }
}
```

**5. Jobs System e Burst Compiler — computação paralela de alta performance:**

```csharp
using Unity.Jobs;
using Unity.Collections;
using Unity.Burst;

// [BurstCompile] — compila com LLVM para código nativo de alta performance
[BurstCompile]
public struct CalcularPosicaoJob : IJobParallelFor
{
    // NativeArray — arrays gerenciados fora do GC do C# (alocação explícita)
    public NativeArray<float3> Posicoes;
    public NativeArray<float3> Velocidades;
    public float DeltaTime;

    public void Execute(int i)
    {
        Posicoes[i] += Velocidades[i] * DeltaTime;
    }
}

// Uso
public class SistemaParticulas : MonoBehaviour
{
    private NativeArray<float3> _posicoes;
    private NativeArray<float3> _velocidades;

    private void Update()
    {
        var job = new CalcularPosicaoJob
        {
            Posicoes    = _posicoes,
            Velocidades = _velocidades,
            DeltaTime   = Time.deltaTime
        };

        // Executa em múltiplas threads, 64 itens por thread
        JobHandle handle = job.Schedule(_posicoes.Length, 64);
        handle.Complete(); // aguarda conclusão
    }

    private void OnDestroy()
    {
        // NativeArrays devem ser descartados explicitamente (não são gerenciados pelo GC)
        _posicoes.Dispose();
        _velocidades.Dispose();
    }
}
```

**Como interpretar o exemplo:** Todos os exemplos desta secao giram em torno da mesma regra: o frame loop amplifica pequenos custos porque eles se repetem o tempo todo. Cache de componentes, reducao de alocacao, pooling e jobs existem para tirar trabalho caro do caminho mais quente do jogo.

---

### 23.8 Padrões de design comuns em jogos com C#

[⬆️ Voltar ao Sumário](#sumário)

**Singleton:**

```csharp
public class GerenciadorDeJogo : MonoBehaviour
{
    public static GerenciadorDeJogo Instancia { get; private set; }

    private void Awake()
    {
        if (Instancia != null && Instancia != this)
        {
            Destroy(gameObject);
            return;
        }
        Instancia = this;
        DontDestroyOnLoad(gameObject); // persiste entre cenas
    }

    public void CarregarProximaFase() { /* ... */ }
}

// Uso em outros scripts
GerenciadorDeJogo.Instancia.CarregarProximaFase();
```

**State Machine:**

```csharp
public abstract class EstadoJogador
{
    protected Jogador Jogador;

    public EstadoJogador(Jogador jogador) => Jogador = jogador;

    public abstract void Entrar();
    public abstract void Atualizar();
    public abstract void Sair();
}

public class EstadoEmPe : EstadoJogador
{
    public EstadoEmPe(Jogador j) : base(j) { }

    public override void Entrar()   => Jogador.Animator.SetTrigger("EmPe");
    public override void Sair()     { }
    public override void Atualizar()
    {
        if (Input.GetButton("Jump"))
            Jogador.TrocarEstado(new EstadoPulando(Jogador));
        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.1f)
            Jogador.TrocarEstado(new EstadoCorrendo(Jogador));
    }
}

public class Jogador : MonoBehaviour
{
    public Animator Animator { get; private set; }
    private EstadoJogador _estado;

    private void Awake()
    {
        Animator = GetComponent<Animator>();
        _estado  = new EstadoEmPe(this);
        _estado.Entrar();
    }

    private void Update() => _estado.Atualizar();

    public void TrocarEstado(EstadoJogador novoEstado)
    {
        _estado.Sair();
        _estado = novoEstado;
        _estado.Entrar();
    }
}
```

**Observer com C# Events:**

```csharp
// Sistema de eventos globalm desacoplado
public static class EventosCentral
{
    public static event Action<int>?   PontosAlterados;
    public static event Action?        JogoIniciado;
    public static event Action<string>? FaseConcluida;

    public static void DispararPontos(int pontos)    => PontosAlterados?.Invoke(pontos);
    public static void DispararJogoIniciado()        => JogoIniciado?.Invoke();
    public static void DispararFaseConcluida(string fase) => FaseConcluida?.Invoke(fase);
}

// Qualquer script pode se inscrever sem referência direta
public class PainelPontuacao : MonoBehaviour
{
    private void OnEnable()  => EventosCentral.PontosAlterados += AtualizarPainel;
    private void OnDisable() => EventosCentral.PontosAlterados -= AtualizarPainel;

    private void AtualizarPainel(int novos) => /* atualizar UI */ Debug.Log(novos);
}
```

**Como interpretar o exemplo:** Os padroes apresentados aparecem muito em jogos porque estados, eventos e objetos de vida curta precisam ser coordenados o tempo inteiro. O mais importante nao e decorar os nomes, e sim entender o problema que cada padrao resolve e o custo que ele traz.

---

### 23.9 C# no Godot 4

[⬆️ Voltar ao Sumário](#sumário)

O Godot 4 possui suporte oficial ao C# via .NET 6+. A estrutura é similar ao Unity, mas com suas particularidades:

```csharp
using Godot;

// No Godot, a classe base é Node (ou Node2D, Node3D, CharacterBody2D, etc.)
public partial class Jogador : CharacterBody2D
{
    [Export] public float Velocidade    = 200f; // [Export] = [SerializeField] do Unity
    [Export] public float VelocidadePulo = 400f;

    private const float Gravidade = ProjectSettings
        .GetSetting("physics/2d/default_gravity").AsSingle();

    public override void _Ready() // equivalente ao Start() do Unity
    {
        GD.Print("Jogador pronto!"); // equivalente ao Debug.Log
    }

    public override void _Process(double delta) // equivalente ao Update()
    {
        // lógica não-física
    }

    public override void _PhysicsProcess(double delta) // equivalente ao FixedUpdate()
    {
        Vector2 velocidade = Velocity;

        // Aplica gravidade
        if (!IsOnFloor())
            velocidade.Y += Gravidade * (float)delta;

        // Movimento horizontal
        float direcao = Input.GetAxis("ui_left", "ui_right");
        velocidade.X = direcao * Velocidade;

        Velocity = velocidade;
        MoveAndSlide(); // move o corpo e resolve colisões
    }
}

// Signals — equivalente ao UnityEvent/C# Events
public partial class Inimigo : Node2D
{
    [Signal]
    public delegate void MorreuEventHandler(int pontosGanhos);

    public void TomarDano(int dano)
    {
        // Emite o signal
        EmitSignal(SignalName.Morreu, 100);
    }
}
```

**Como interpretar o exemplo:** Mudar de engine nao apaga os fundamentos da linguagem; o que muda e o vocabulario do runtime e do editor. Quem entende bem ciclo de vida, callbacks e composicao em C# se adapta muito melhor as diferencas entre Unity e Godot.

---

### 23.10 Diferenças entre C# padrão e C# no Unity

[⬆️ Voltar ao Sumário](#sumário)

| Aspecto | C# Padrão (.NET) | C# no Unity |
|---|---|---|
| Runtime | .NET 8/9/10 | Mono ou IL2CPP |
| `async`/`await` | Totalmente suportado | Suportado, mas prefira Coroutines para lógica de jogo |
| Coleções genéricas | System.Collections.Generic | Mesmas + `NativeArray<T>` (Jobs) |
| `Span<T>` | Totalmente suportado | Suportado em Mono; pleno no IL2CPP |
| Reflection | Totalmente suportado | Parcialmente limitada com IL2CPP (AOT) |
| Source Generators | Totalmente suportados | Suporte crescente (Unity 2022.2+) |
| Garbage Collector | Generacional (GC moderno) | Boehm GC (Mono) — mais propenso a stuttering |
| `float` vs `double` | Use conforme necessário | Prefira `float` — GPU e APIs Unity usam `float` |
| `new` no Update | Não há restrição | **Evite** — cada `new` de objeto de referência gera GC |
| Debug | `Console.WriteLine` | `Debug.Log` / `Debug.LogError` / `Debug.LogWarning` |

---

## Anexo A — Plataformas de Prática Recomendadas

[⬆️ Voltar ao Sumário](#sumário)

[Link do Miro](https://miro.com/welcomeonboard/K3lDaXB2V0MxVkNMTG5MaEN0UjdTd3cxbjl0QWJzVnZ6bHN5QkhmY05vTFo0bThKRldvTnpTUEdXcWd6UVNqRDFXVlhka1M1dDhITWhWR1RPZ2J2QTh3Lzg1c3lQT1V3d0h2Rk9KVjdXTU9hREpPZ0dIa0R0WjZBK1A0S0tVc0ZzVXVvMm53MW9OWFg5bkJoVXZxdFhRPT0hdjE=?share_link_id=115677346406)

Plataformas externas para praticar C# e lógica de programação, cada uma com um foco pedagógico diferente:

| Plataforma | Suporte a C# | Formato Estilo Exame | Foco Pedagógico e Conceitual |
| --- | --- | --- | --- |
| **Codility** | Sim | Altamente aderente | Complexidade algorítmica, corretude e desempenho sob estresse |
| **CodeWars** | Sim | Altamente aderente | Complexidade algorítmica, corretude e desempenho sob estresse |
| **Beecrowd** | Sim | Altamente aderente | Lógica de programação pura, matemática e estruturas de dados |
| **Exercism** | Sim | Baixa aderência (foco em projetos) | Idiomatismo da linguagem, legibilidade e TDD local |
| **CodeSignal Learn** | Sim | Média aderência (trilhas guiadas) | Código limpo, princípios SOLID e padrões de projeto aplicados |
| **Coderbyte** | Sim | Altamente aderente | Desafios algorítmicos clássicos e projetos multi-arquivos de mercado |
| **HackerRank** | Sim | Altamente aderente | Resolução de problemas, estruturas de dados e proficiência em sintaxe |
| **DesignPatterns101** | Sim | Média aderência (exercícios práticos) | Padrões de projeto GoF aplicados exclusivamente em C# |
| **Project Euler** | Sim | Altamente aderente (resposta única) | Raciocínio matemático-computacional profundo e otimização lógica |

---

## Anexo B — Referências Oficiais Consultadas

[⬆️ Voltar ao Sumário](#sumário)

As definições, distinções conceituais e atualizações de versão deste guia foram revisadas com base principalmente no **Microsoft Learn**:

- [C# documentation](https://learn.microsoft.com/en-us/dotnet/csharp/)
- [A tour of C#](https://learn.microsoft.com/en-us/dotnet/csharp/tour-of-csharp/)
- [What's new in C# 14](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-14)
- [.NET releases and support](https://learn.microsoft.com/en-us/dotnet/core/releases-and-support)
- [Overview of core .NET libraries](https://learn.microsoft.com/en-us/dotnet/standard/class-library-overview)
- [Built-in types (C# reference)](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/built-in-types)
- [Console Class (System)](https://learn.microsoft.com/en-us/dotnet/api/system.console?view=net-10.0)
- [Math Class (System)](https://learn.microsoft.com/en-us/dotnet/api/system.math?view=net-10.0)
- [DateTime Struct (System)](https://learn.microsoft.com/en-us/dotnet/api/system.datetime?view=net-10.0)
- [How to use the DateOnly and TimeOnly structures](https://learn.microsoft.com/en-us/dotnet/standard/datetime/how-to-use-dateonly-timeonly)
- [TimeSpan Struct (System)](https://learn.microsoft.com/en-us/dotnet/api/system.timespan?view=net-10.0)
- [Guid Struct (System)](https://learn.microsoft.com/en-us/dotnet/api/system.guid?view=net-10.0)
- [Random Class (System)](https://learn.microsoft.com/en-us/dotnet/api/system.random?view=net-10.0)
- [Convert Class (System)](https://learn.microsoft.com/en-us/dotnet/api/system.convert?view=net-10.0)
- [Environment Class (System)](https://learn.microsoft.com/en-us/dotnet/api/system.environment?view=net-10.0)
- [File Class (System.IO)](https://learn.microsoft.com/en-us/dotnet/api/system.io.file?view=net-10.0)
- [Path Class (System.IO)](https://learn.microsoft.com/en-us/dotnet/api/system.io.path?view=net-10.0)
- [Directory Class (System.IO)](https://learn.microsoft.com/en-us/dotnet/api/system.io.directory?view=net-10.0)
- [Interfaces - define behavior for multiple types](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/types/interfaces)
- [Language Integrated Query (LINQ)](https://learn.microsoft.com/en-us/dotnet/csharp/linq/)
- [Standard query operators overview](https://learn.microsoft.com/en-us/dotnet/csharp/linq/standard-query-operators/)
- [`IEnumerable<T>` Interface](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1?view=net-10.0)
- [`IQueryable<T>` Interface](https://learn.microsoft.com/en-us/dotnet/api/system.linq.iqueryable-1?view=net-10.0)
- [Usando a classe StringBuilder no .NET](https://learn.microsoft.com/pt-br/dotnet/standard/base-types/stringbuilder)
- [Introduction to LINQ queries](https://learn.microsoft.com/en-us/dotnet/csharp/linq/get-started/introduction-to-linq-queries)
- [Explicit Interface Implementation](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/interfaces/explicit-interface-implementation)

Sugestão de estudo: use este guia para construir o modelo mental e a documentação oficial para validar detalhes de comportamento, APIs e mudanças de versão.

---

## Glossário

[⬆️ Voltar ao Sumário](#sumário)

> Termos-chave usados ao longo do guia, em ordem alfabética. Cada item linka direto para a seção onde o assunto é explicado em detalhe.

- **`abstract`** — modificador que marca uma classe ou membro como incompleto, obrigando subclasses a fornecer a implementação. → [7.3 `abstract`](#73-abstract)
- **`async` / `await`** — palavras-chave que permitem escrever código assíncrono em estilo sequencial, sem bloquear a thread. → [16.1 O modelo assíncrono do C#](#161-o-modelo-assíncrono-do-c)
- **Attribute** — metadado anexado a um tipo ou membro, lido em tempo de compilação ou execução (ex: `[Obsolete]`). → [19.1 Attributes embutidos](#191-attributes-embutidos)
- **Builder Pattern** — padrão de projeto para construir objetos complexos passo a passo, geralmente com sintaxe fluente. → [11.4 Padrão Builder](#114-padrão-builder)
- **Burst Compiler** — compilador da Unity que converte código C# em código nativo de alta performance via LLVM. → [23.7 Boas práticas de performance no Unity](#237-boas-práticas-de-performance-no-unity)
- **CLR (Common Language Runtime)** — máquina virtual do .NET que executa o código compilado (IL), análoga à JVM do Java. → [1.1 O que é C#?](#11-o-que-é-c)
- **Constraints (restrições de generics)** — regras que limitam quais tipos podem ser usados num tipo genérico. → [17.2 Constraints (restrições)](#172-constraints-restrições)
- **Construtor** — método especial, sem tipo de retorno, executado na criação de uma instância (`new`), responsável por inicializar seu estado. → [11.2 Construtores em Profundidade](#112-construtores-em-profundidade)
- **`const` / `readonly`** — modificadores para valores imutáveis; `const` é resolvido em tempo de compilação, `readonly` em tempo de execução. → [3.5 `const` e `readonly`](#35-const-e-readonly)
- **Coroutine** — mecanismo do Unity para executar código ao longo de vários frames, sem usar `async`/`await`. → [23.5 Coroutines](#235-coroutines-execução-assíncrona-sem-asyncawait)
- **Delegate** — tipo que representa uma referência tipada a um método, base para eventos e lambdas. → [13.1 Delegates](#131-delegates-ponteiros-de-método-tipados)
- **Enum / Flags enum** — tipo que representa um conjunto fixo de valores nomeados; com `[Flags]`, permite combinação via bitmask. → [10.1 Enums básicos](#101-enums-básicos)
- **Event** — mecanismo baseado em delegates para notificar múltiplos assinantes sobre uma ocorrência. → [13.4 Eventos (Events)](#134-eventos-events)
- **Extension Method** — método que "adiciona" comportamento a um tipo existente sem modificá-lo ou herdar dele. → [9.2 Métodos de extensão](#92-métodos-de-extensão-extension-methods)
- **Generics** — recurso que permite escrever tipos e métodos parametrizados por tipo, mantendo segurança de tipos. → [17.1 Tipos parametrizados](#171-tipos-parametrizados)
- **IL / CIL (Intermediate Language)** — formato intermediário para o qual o C# é compilado antes de ser executado pelo CLR. → [1.1 O que é C#?](#11-o-que-é-c)
- **Interface** — contrato que define quais membros uma classe deve implementar, sem fornecer implementação própria. → [12.2 Interfaces](#122-interfaces)
- **`IEnumerable<T>`** — contrato fundamental de sequência enumerável; diz que um tipo pode fornecer elementos em ordem de iteração, sem prometer índice ou materialização. → [14.3 `IEnumerable<T>`](#143-ienumerablet-e-o-contrato-fundamental-das-sequências)
- **`IQueryable<T>`** — contrato de consulta traduzível por um provider, geralmente usado para fontes remotas como bancos de dados. → [14.4 `IQueryable<T>`](#144-iqueryablet-e-queries-traduzíveis-para-outra-fonte)
- **Jobs System** — sistema da Unity para distribuir cálculos entre múltiplas threads de forma segura. → [23.7 Boas práticas de performance no Unity](#237-boas-práticas-de-performance-no-unity)
- **Lambda (expressão lambda)** — função anônima e compacta, geralmente usada com delegates e LINQ. → [13.3 Expressões Lambda](#133-expressões-lambda)
- **LINQ (Language Integrated Query)** — conjunto de recursos da linguagem e da biblioteca para consultar e transformar dados de forma tipada e declarativa. → [14.1 O que é LINQ?](#141-o-que-é-linq)
- **MonoBehaviour** — classe base da qual todo script que vive numa GameObject do Unity deriva. → [23.2 MonoBehaviour](#232-monobehaviour-a-classe-base-dos-scripts-unity)
- **Namespace** — contêiner lógico que agrupa tipos relacionados, evitando colisão de nomes. → [2.1 Namespaces](#21-namespaces)
- **Nullable Type** — tipo de valor que pode aceitar `null` além do seu valor normal (ex: `int?`). → [3.3 Nullable Types](#33-nullable-types-tipos-que-aceitam-null)
- **Pattern Matching** — sintaxe para testar e desestruturar valores com base em forma/tipo (`is`, `switch` expressions). → [7.6 `is`, `as` e Pattern Matching](#76-is-as-e-pattern-matching)
- **Properties** — membros que expõem um valor através de `get`/`set`, encapsulando acesso a um campo. → [6.1 O que são Properties?](#61-o-que-são-properties)
- **Record** — tipo de referência (ou valor) focado em imutabilidade e igualdade por valor, introduzido no C# 9. → [11.3 Records (C# 9+)](#113-records-c-9)
- **Reflection** — capacidade de inspecionar tipos, métodos e atributos em tempo de execução. → [22.1 Reflection](#221-reflection)
- **`ref` / `out` / `in`** — modificadores de parâmetro para passar argumentos por referência em vez de por valor. → [7.8 `ref`, `out` e `in`](#78-ref-out-e-in)
- **Roslyn** — compilador open-source moderno do C#, também usado para análise estática e Source Generators. → [1.1 O que é C#?](#11-o-que-é-c)
- **`sealed`** — modificador que impede uma classe de ser herdada (ou um método de ser sobrescrito novamente). → [7.2 `sealed`](#72-sealed)
- **ScriptableObject** — tipo de asset da Unity para armazenar dados independentes de uma instância de GameObject. → [23.4 ScriptableObject](#234-scriptableobject-dados-desacoplados-do-gameobject)
- **Singleton** — padrão de projeto que garante uma única instância acessível globalmente de uma classe. → [23.8 Padrões de design comuns em jogos com C#](#238-padrões-de-design-comuns-em-jogos-com-c)
- **Source Generator** — componente que gera código C# adicional em tempo de compilação. → [22.2 Source Generators](#222-source-generators-c-9)
- **Span\<T\> / Memory\<T\>** — estruturas para trabalhar com "fatias" de memória contígua sem alocação extra. → [20.2 Span\<T\> e Memory\<T\>](#202-spant-e-memoryt-zero-allocation-slicing)
- **`static`** — modificador que faz um membro pertencer ao tipo, não a uma instância específica. → [7.1 `static`](#71-static)
- **State Machine** — padrão que organiza o comportamento de um objeto em estados distintos com transições explícitas. → [23.8 Padrões de design comuns em jogos com C#](#238-padrões-de-design-comuns-em-jogos-com-c)
- **StringBuilder** — classe mutável para concatenar strings repetidamente sem o custo de criar novas instâncias a cada operação. → [4.2 Imutabilidade e StringBuilder](#42-imutabilidade-e-stringbuilder)
- **Task / ValueTask** — representações de uma operação assíncrona em andamento; `ValueTask` evita alocação em cenários de alta performance. → [16.3 Task vs ValueTask](#163-task-vs-valuetask)
- **Unsafe code** — blocos de código que permitem manipulação direta de ponteiros, fora da supervisão normal do CLR. → [22.3 Unsafe code e ponteiros](#223-unsafe-code-e-ponteiros)
- **`var`** — palavra-chave que permite ao compilador inferir o tipo de uma variável a partir do valor atribuído. → [3.4 `var` — inferência de tipo](#34-var-inferência-de-tipo)
- **`virtual` / `override`** — modificadores que permitem que um método seja redefinido por uma subclasse. → [7.4 `virtual` e `override`](#74-virtual-e-override)

**Como interpretar o exemplo:** A tabela lembra que saber C# nao e exatamente o mesmo que saber C# dentro de uma engine especifica. Em Unity, runtime, GC, AOT e APIs de jogo fazem alguns habitos do C# padrao continuarem validos e outros precisarem de adaptacao consciente.
