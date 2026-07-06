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

<a id="sumário"></a>

## Sumário Geral

- [Como usar este guia](#como-usar-este-guia)
- **Parte 1 — Introdução e Contextualização**
  - [1.1 O que é C#?](#11-o-que-é-c)
  - [1.2 O que é um assembly?](#12-o-que-é-um-assembly)
  - [1.3 Por que aprender C# em 2026?](#13-por-que-aprender-c-em-2026)
  - [1.4 Estrutura de um programa C#](#14-estrutura-de-um-programa-c)
- **Parte 2 — Namespaces e Using**
  - [2.1 Namespaces](#21-namespaces)
  - [2.2 Using Directives](#22-using-directives)
- **Parte 3 — Variáveis e Tipos**
  - [3.1 O que é uma variável?](#31-o-que-é-uma-variável)
  - [3.2 Tipos de valor vs tipos de referência](#32-tipos-de-valor-vs-tipos-de-referência)
  - [3.3 Nullable Types — tipos que aceitam null](#33-nullable-types-tipos-que-aceitam-null)
  - [3.4 `var` — inferência de tipo](#34-var-inferência-de-tipo)
  - [3.5 `const` e `readonly`](#35-const-e-readonly)
  - [3.6 Referências diretas, indiretas e fracas](#36-referências-diretas-indiretas-e-fracas)
- **Parte 4 — String e suas Peculiaridades**
  - [4.1 String é um tipo de referência imutável](#41-string-é-um-tipo-de-referência-imutável)
  - [4.2 Imutabilidade e StringBuilder](#42-imutabilidade-e-stringbuilder)
  - [4.3 String Interpolation e verbatim strings](#43-string-interpolation-e-verbatim-strings)
  - [4.4 Métodos importantes de string](#44-métodos-importantes-de-string)
  - [4.5 Classes e funções predefinidas essenciais do .NET](#45-classes-e-funções-predefinidas-essenciais-do-net)
- **Parte 5 — Modificadores de Acesso**
  - [5.1 Os modificadores de acesso do C#](#51-os-modificadores-de-acesso-do-c)
  - [5.2 Boas práticas com modificadores](#52-boas-práticas-com-modificadores)
- **Parte 6 — Propriedades (Properties)**
  - [6.1 O que são Properties?](#61-o-que-são-properties)
  - [6.2 Expression-bodied members](#62-expression-bodied-members)
- **Parte 7 — Palavras-chave Especiais do C#**
  - [7.1 `static`](#71-static)
  - [7.2 `sealed`](#72-sealed)
  - [7.3 `abstract`](#73-abstract)
  - [7.4 `virtual` e `override`](#74-virtual-e-override)
  - [7.5 `this` e `base`](#75-this-e-base)
  - [7.6 `is`, `as` e Pattern Matching](#76-is-as-e-pattern-matching)
  - [7.7 `using` para gerenciamento de recursos](#77-using-para-gerenciamento-de-recursos)
  - [7.8 `ref`, `out` e `in`](#78-ref-out-e-in)
  - [7.9 Conversões definidas pelo usuário (`implicit` e `explicit`)](#79-conversões-definidas-pelo-usuário-implicit-e-explicit)
  - [7.10 `nameof`, `typeof` e `default`](#710-nameof-typeof-e-default)
  - [7.11 `partial` e `file`](#711-partial-e-file)
  - [7.12 `yield`](#712-yield)
  - [7.13 `lock`](#713-lock)
  - [7.14 `async` e `await`](#714-async-e-await)
  - [7.15 `required` e `init`](#715-required-e-init)
- **Parte 8 — Controle de Fluxo**
  - [8.1 `if / else if / else`](#81-if-else-if-else)
  - [8.2 `switch` e switch expressions](#82-switch-e-switch-expressions)
  - [8.3 Loops](#83-loops)
- **Parte 9 — Métodos**
  - [9.1 Declaração de métodos](#91-declaração-de-métodos)
  - [9.2 Métodos de extensão (Extension Methods)](#92-métodos-de-extensão-extension-methods)
  - [9.3 Sobrecarga de métodos](#93-sobrecarga-de-métodos)
- **Parte 10 — Enums**
  - [10.1 Enums básicos](#101-enums-básicos)
  - [10.2 Flags enum — bitmask](#102-flags-enum-bitmask)
- **Parte 11 — Classes e Objetos**
  - [11.1 Estrutura completa de uma classe](#111-estrutura-completa-de-uma-classe)
  - [11.2 Construtores em Profundidade](#112-construtores-em-profundidade)
  - [11.3 Records (C# 9+)](#113-records-c-9)
  - [11.4 Padrão Builder](#114-padrão-builder)
- **Parte 12 — Herança e Polimorfismo**
  - [12.1 Herança em C#](#121-herança-em-c)
  - [12.2 Interfaces](#122-interfaces)
- **Parte 13 — Delegates, Events e Lambdas**
  - [13.1 Delegates — ponteiros de método tipados](#131-delegates-ponteiros-de-método-tipados)
  - [13.2 Func, Action e Predicate](#132-func-action-e-predicate)
  - [13.3 Expressões Lambda](#133-expressões-lambda)
  - [13.4 Eventos (Events)](#134-eventos-events)
- **Parte 14 — LINQ (Language Integrated Query)**
  - [14.1 O que é LINQ?](#141-o-que-é-linq)
  - [14.2 Operadores LINQ principais](#142-operadores-linq-principais)
  - [14.3 `IEnumerable<T>` e o contrato fundamental das sequências](#143-ienumerablet-e-o-contrato-fundamental-das-sequências)
  - [14.4 `IQueryable<T>` e queries traduzíveis para outra fonte](#144-iqueryablet-e-queries-traduzíveis-para-outra-fonte)
  - [14.5 Execução adiada, materialização e armadilhas](#145-execução-adiada-materialização-e-armadilhas)
- **Parte 15 — Coleções**
  - [15.1 Tipos de coleções principais](#151-tipos-de-coleções-principais)
  - [15.2 List<T>](#152-listt)
  - [15.3 Dictionary<TKey, TValue>](#153-dictionarytkey-tvalue)
  - [15.4 Como escolher a coleção certa](#154-como-escolher-a-coleção-certa)
- **Parte 16 — Async/Await e Programação Assíncrona**
  - [16.1 O modelo assíncrono do C#](#161-o-modelo-assíncrono-do-c)
  - [16.2 Padrões de uso](#162-padrões-de-uso)
  - [16.3 Task vs ValueTask](#163-task-vs-valuetask)
- **Parte 17 — Generics**
  - [17.1 Tipos parametrizados](#171-tipos-parametrizados)
  - [17.2 Constraints (restrições)](#172-constraints-restrições)
  - [17.3 Covariância e contravariância](#173-covariância-e-contravariância)
- **Parte 18 — Tratamento de Exceções**
  - [18.1 `try / catch / finally`](#181-try-catch-finally)
  - [18.2 Exceções customizadas](#182-exceções-customizadas)
  - [18.3 Hierarquia de exceções](#183-hierarquia-de-exceções)
  - [18.4 Exceções de argumento e implementação comuns](#184-exceções-de-argumento-e-implementação-comuns)
- **Parte 19 — Attributes (Annotations)**
  - [19.1 Attributes embutidos](#191-attributes-embutidos)
  - [19.2 Criando Attributes customizados](#192-criando-attributes-customizados)
- **Parte 20 — Tipos Especiais Modernos do C#**
  - [20.1 Tuple e ValueTuple](#201-tuple-e-valuetuple)
  - [20.2 `WeakReference<T>` e referências fracas no GC](#202-weakreferencet-e-referências-fracas-no-gc)
  - [20.3 Span<T> e Memory<T> — zero-allocation slicing](#203-spant-e-memoryt-zero-allocation-slicing)
  - [20.4 Sealed classes com Pattern Matching (como Discriminated Union)](#204-sealed-classes-com-pattern-matching-como-discriminated-union)
- **Parte 21 — Threads e Concorrência**
  - [21.1 Thread básico e ThreadPool](#211-thread-básico-e-threadpool)
  - [21.2 Task Parallel Library (TPL)](#212-task-parallel-library-tpl)
  - [21.3 Sincronização](#213-sincronização)
- **Parte 22 — Interoperabilidade e Recursos Avançados**
  - [22.1 Reflection](#221-reflection)
  - [22.2 Dependency Injection (DI)](#222-dependency-injection-di)
  - [22.3 Source Generators (C# 9+)](#223-source-generators-c-9)
  - [22.4 Unsafe code e ponteiros](#224-unsafe-code-e-ponteiros)
- [Resumo Geral — Conceitos Fundamentais](#resumo-geral-conceitos-fundamentais)
- **Parte 23 — C# no Contexto de Game Development**
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
- **Parte 24 — Arquitetura de Aplicações C#/.NET**
  - [24.1 C# não impõe arquitetura](#241-c-não-impõe-arquitetura)
  - [24.2 Arquitetura em camadas](#242-arquitetura-em-camadas)
  - [24.3 Clean Architecture, Onion e Ports and Adapters](#243-clean-architecture-onion-e-ports-and-adapters)
  - [24.4 Domain-Driven Design (DDD)](#244-domain-driven-design-ddd)
  - [24.5 CQRS e separação entre comandos e consultas](#245-cqrs-e-separação-entre-comandos-e-consultas)
  - [24.6 Event-Driven Architecture](#246-event-driven-architecture)
  - [24.7 Microservices em .NET](#247-microservices-em-net)
  - [24.8 Padrões enterprise clássicos](#248-padrões-enterprise-clássicos)
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

**Como interpretar o exemplo:** O diagrama mostra que o código C# não roda direto no sistema operacional; ele passa por compilação para IL e depois pelo runtime do .NET. Esse fluxo explica por que o ecossistema consegue oferecer coleta de lixo, verificação de tipos, tratamento consistente de exceções e portabilidade entre plataformas.

---

### 1.2 O que é um assembly?

[⬆️ Voltar ao Sumário](#sumário)

`Assembly` é uma das ideias mais importantes do ecossistema .NET e muita gente começa usando sem perceber.

Em termos bem diretos:

- um assembly é a **unidade de compilação, empacotamento, reutilização e versionamento** do .NET;
- na prática, ele costuma aparecer como um arquivo `.dll` ou `.exe`;
- dentro dele ficam o código compilado em `IL/CIL`, metadados sobre tipos e, muitas vezes, recursos como arquivos embutidos.

Se você já viu este fluxo:

```text
Codigo C# (.cs)
        ↓ compilador
Assembly .NET (.dll ou .exe)
        ↓ runtime do .NET
Execucao
```

então o assembly é justamente o "pacote" que o compilador produz para o runtime consumir.

#### O que costuma existir dentro de um assembly

Em visão de iniciante, pense nele como um contêiner técnico que normalmente reúne:

- **manifesto do assembly**: identidade, versão e referências;
- **metadados dos tipos**: classes, interfaces, structs, enums, métodos, propriedades;
- **código IL/CIL**: instruções intermediárias geradas pelo compilador;
- **recursos**: strings, imagens, arquivos incorporados e afins.

Você não precisa decorar isso de início, mas ajuda a entender que o assembly não é "só um arquivo qualquer". Ele descreve para o runtime o que existe ali dentro e como aquilo se conecta ao restante da aplicação.

#### Para que assemblies servem na prática

Assemblies importam porque são a fronteira real de várias decisões do .NET:

- **distribuição**: bibliotecas e aplicações são entregues em assemblies;
- **reutilização**: um projeto pode referenciar o assembly de outro;
- **versionamento**: versões e dependências são rastreadas nesse nível;
- **controle de acesso**: `internal` vale dentro do mesmo assembly;
- **reflection**: o programa pode inspecionar tipos dentro de um assembly carregado;
- **organização de solução**: separar domínio, infraestrutura, UI e testes normalmente significa separar assemblies.

#### Diferença entre assembly, projeto, namespace e pacote

Esse é um ponto que costuma confundir iniciantes.

- **arquivo `.cs`**: contém código-fonte;
- **namespace**: organiza nomes logicamente;
- **projeto (`.csproj`)**: define como compilar aquele conjunto de arquivos;
- **assembly**: é o artefato compilado gerado pelo projeto;
- **pacote NuGet**: é um pacote de distribuição que pode carregar um ou mais assemblies.

Regra mental curta:

- `namespace` organiza nomes;
- `projeto` organiza compilação;
- `assembly` organiza o que o .NET realmente carrega e referencia.

#### Exemplo mental concreto

Imagine esta solução:

- `MinhaApp.Console`
- `MinhaApp.Core`

O projeto `MinhaApp.Core` pode compilar para algo como:

```text
MinhaApp.Core.dll
```

Esse `.dll` é um assembly.

Depois, `MinhaApp.Console` referencia esse assembly para usar as classes de domínio sem copiar código.

É por isso que, quando você escreve:

```csharp
internal class PedidoService
{
}
```

o `internal` não quer dizer "visível para o mesmo namespace".

Ele quer dizer:

- visível para qualquer código do **mesmo assembly**.

#### Por que isso é importante cedo

Entender assembly cedo ajuda você a compreender melhor:

- por que `internal` existe;
- como bibliotecas são consumidas;
- o que significa "referenciar um projeto";
- por que reflection fala tanto em `Assembly`, `Type` e `MemberInfo`;
- por que uma aplicação grande costuma ser dividida em múltiplos assemblies.

**Como interpretar o exemplo:** Se `class` e `interface` ajudam você a pensar em design dentro do código, `assembly` ajuda você a pensar em fronteiras do código depois que ele foi compilado. É um conceito de linguagem + runtime + arquitetura ao mesmo tempo.

---

### 1.3 Por que aprender C# em 2026?

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

**Como interpretar o exemplo:** A tabela não serve apenas para listar mercados; ela mostra que a mesma base da linguagem reaparece em contextos muito diferentes, do backend ao desenvolvimento de jogos. Isso significa que estudar fundamentos de C# rende em várias áreas ao mesmo tempo, mesmo quando o framework muda.

---

### 1.4 Estrutura de um programa C#

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

**Como interpretar o exemplo:** Os dois formatos existem porque a linguagem evoluiu para reduzir verbosidade sem abandonar compatibilidade com projetos antigos. Entender tanto `top-level statements` quanto a forma clássica com `Program` e `Main` ajuda você a ler código moderno e legado com a mesma naturalidade.

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

**Como interpretar o exemplo:** O namespace funciona como endereço lógico dos tipos, e não apenas como enfeite visual. Uma boa estrutura de namespaces comunica domínio, camada e responsabilidade antes mesmo de você abrir a implementação da classe.

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

> ⚠️ Projetos .NET modernos (SDK-style) incluem `global using` implícitos para namespaces comuns (`System`, `System.Collections.Generic`, etc.) quando `<Nullable>enable</Nullable>` está configurado no `.csproj`. Isso significa que muitos `using` são desnecessários em projetos novos.

**Como interpretar o exemplo:** `using` reduz ruído visual ao esconder nomes totalmente qualificados que não agregam valor a toda leitura. Aliás, `using static` e `global using` mostram que C# trata legibilidade e ergonomia como parte do desenho da linguagem.

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

**Como interpretar o exemplo:** Toda variável em C# liga três ideias: nome, tipo e valor atual. Mesmo com `var`, o tipo continua existindo e sendo conhecido pelo compilador; a diferença é apenas que ele faz a inferência para você.

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

**Como interpretar o exemplo:** O que realmente importa aqui não é decorar `stack` e `heap`, mas entender a semântica de cópia e compartilhamento. Quando você domina isso, passa a prever melhor efeitos colaterais, mutabilidade e comportamento de parâmetros e coleções.

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

**Como interpretar o exemplo:** Os operadores mostrados existem para tornar a ausência de valor visível no contrato, em vez de deixar `null` circular de forma implícita. Em código profissional, isso reduz muito `NullReferenceException` e melhora a clareza das APIs.

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

**Como interpretar o exemplo:** O exemplo desfaz a confusão mais comum: `var` não significa tipagem dinâmica, e sim tipagem estática com inferência local. Use `var` quando ele reduz repetição sem esconder um tipo importante para o leitor.

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

**Como interpretar o exemplo:** A diferença entre os dois não é só sintática; ela muda tempo de resolução, flexibilidade e evolução da API. `const` representa valor totalmente fixo e embutido pelo compilador, enquanto `readonly` protege o campo ou referência depois da construção do objeto.

---

### 3.6 Referências diretas, indiretas e fracas

[⬆️ Voltar ao Sumário](#sumário)

Quando esse assunto aparece em exemplos como `ITheme` e `Ref<ITheme>`, é fácil misturar ideias que, tecnicamente, pertencem a eixos diferentes.

O jeito mais claro de organizar isso é separar duas perguntas:

1. **Como eu chego até o objeto?**
2. **Essa referência impede o GC de coletar o objeto?**

Essas perguntas produzem duas distinções diferentes:

- **direta vs indireta** descreve o caminho de acesso ao objeto;
- **forte vs fraca** descreve a relação da referência com o Garbage Collector.

> Observação importante: "referência direta" e "referência indireta" são rótulos didáticos úteis, mas o termo formal do runtime para a referência comum do dia a dia é **referência forte**.

```csharp
public interface ITheme
{
    string Name { get; }
}

public sealed class DarkTheme : ITheme
{
    public string Name => "DarkTheme";
}

public sealed class Ref<T> where T : class
{
    public T Value { get; set; }
    public Ref(T value) => Value = value;
}

ITheme directTheme = new DarkTheme();
var handle = new Ref<ITheme>(new DarkTheme());
var weak = new WeakReference<ITheme>(directTheme);
```

#### 3.6.1 Referência direta

Quando você escreve:

```csharp
ITheme directTheme = new DarkTheme();
```

o código cliente segura o próprio objeto de domínio diretamente.

Leitura mental:

- a variável `directTheme` aponta direto para o tema;
- não existe uma caixa intermediária;
- esse acesso é **direto**;
- e, no uso normal, também é uma **referência forte**.

Em forma de seta:

`cliente -> directTheme -> DarkTheme`

#### 3.6.2 Referência indireta

Quando você escreve:

```csharp
var handle = new Ref<ITheme>(new DarkTheme());
```

o cliente já não segura o tema diretamente. Ele segura um objeto intermediário.

Leitura mental:

- `handle` aponta para a caixa `Ref<ITheme>`;
- o tema real está dentro de `handle.Value`;
- para chegar ao tema, o cliente passa pelo handle;
- então o acesso ao tema é **indireto**.

Em forma de seta:

`cliente -> handle -> Value -> DarkTheme`

Essa é exatamente a ideia do exemplo do projeto:

- cada cliente não recebe `ITheme` diretamente;
- cada cliente recebe um `Ref<ITheme>`;
- a factory pode trocar `handle.Value` depois;
- o cliente continua com o mesmo handle, mas passa a ver outro tema.

Ponto crucial: **indireta não significa fraca**.

No exemplo acima, `handle` é uma referência forte para a caixa, e a caixa mantém uma referência forte para o tema em `Value`.

#### 3.6.3 Referência fraca

Quando você escreve:

```csharp
var weak = new WeakReference<ITheme>(directTheme);
```

você não está dizendo "quero usar esse objeto normalmente". Você está dizendo:

"quero tentar observá-lo depois, sem ser o responsável por mantê-lo vivo".

Leitura mental:

- `weak` sabe qual era o objeto;
- mas não conta como posse forte;
- se só restarem weak references, o GC pode coletar o objeto;
- por isso o acesso posterior precisa usar `TryGetTarget(...)`.

```csharp
if (weak.TryGetTarget(out var aliveTheme))
{
    Console.WriteLine(aliveTheme.Name);
}
else
{
    Console.WriteLine("O objeto já foi coletado.");
}
```

#### 3.6.4 A diferença certa: não confunda os eixos

O ponto mais importante desta seção é este:

- **direta vs indireta** fala sobre o **caminho** até o objeto;
- **forte vs fraca** fala sobre **tempo de vida e GC**.

Isso significa que:

- uma referência pode ser **direta e forte**;
- um acesso pode ser **indireto e ainda assim forte**;
- uma `WeakReference<T>` é **fraca**, mesmo que a variável que a guarda seja uma referência comum para o wrapper `WeakReference<T>`.

Tabela mental rápida:

| Caso | Como o cliente chega ao objeto | Impede coleta pelo GC? |
|---|---|---|
| `ITheme theme = new DarkTheme();` | Direto | Sim, enquanto essa referência forte existir |
| `Ref<ITheme> handle = new Ref<ITheme>(...)` | Indireto, via `handle.Value` | Sim, enquanto `handle` e `Value` mantiverem o objeto alcançável |
| `WeakReference<T> weak = new WeakReference<T>(theme);` | Indireto, via `TryGetTarget(...)` | Não |

#### 3.6.5 Ligando isso ao projeto

Na aula `Object Tracking and Bulk Replacement`, os três conceitos aparecem juntos, mas com papéis diferentes:

- em **Object Tracking**, a factory guarda `WeakReference<T>` para observar temas sem mantê-los vivos artificialmente;
- em **Bulk Replacement**, o cliente recebe `Ref<ITheme>` para que a factory possa trocar o `Value` depois;
- nesses dois casos, a centralização da criação pela factory é a base, mas tracking e bulk replacement continuam sendo capacidades opcionais.

Se você guardar só uma frase desta seção, guarde esta:

**`Ref<ITheme>` é indireção controlada; `WeakReference<T>` é observação sem posse forte.**

**Como interpretar o exemplo:** O erro mais comum é tratar `handle`, `weak reference` e `referência normal` como se fossem apenas versões diferentes da mesma coisa. Não são. `Ref<ITheme>` resolve um problema de indireção e substituição; `WeakReference<T>` resolve um problema de observação sem estender tempo de vida; e a referência direta comum continua sendo a forma padrão de usar objetos no dia a dia.

> **Referências oficiais:** [WeakReference<T>](https://learn.microsoft.com/en-us/dotnet/api/system.weakreference-1?view=net-10.0), [Weak references](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/weak-references)

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

**Como interpretar o exemplo:** `string` é um caso especial em C#: embora seja tipo de referência, sua semântica foi desenhada para ser mais segura no uso cotidiano. Por isso `==` compara conteúdo, mas comparações profissionais ainda devem preferir `StringComparison` quando cultura e caixa importam.

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

**Como interpretar o exemplo:** Cada forma de escrever texto ataca um problema de legibilidade diferente. Interpolação é melhor para misturar valores com texto; `@` ajuda com caminhos e escapes; e raw strings servem muito bem para conteúdos multilinha, como JSON e templates.

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

**Como interpretar o exemplo:** A lista de métodos fica mais fácil de entender quando você a enxerga por categoria: inspecionar, transformar, extrair e converter. Esse modelo mental ajuda inclusive a evitar erros comuns, como usar `Parse` em entrada externa quando o fluxo correto é `TryParse`.

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

**Como interpretar o exemplo:** Modificador de acesso não serve apenas para esconder membros; ele define quem pode colaborar com quem dentro do sistema. Quando você trata `public`, `private`, `protected`, `internal` e os compostos como fronteiras de design, suas APIs ficam menores e menos acopladas.

#### 5.1.1 O que cada modificador permite na prática

- **`private`**: só o próprio tipo acessa. É o mais fechado para membros e, por padrão, o mais seguro em termos de encapsulamento. Uso comum: campos, helpers internos, validações, partes sensíveis do estado e construtores que não devem ser chamados livremente.
- **`protected`**: o próprio tipo e classes derivadas acessam. Uso comum: pontos de extensão em herança, quando subclasses realmente precisam participar da implementação.
- **`internal`**: qualquer código do mesmo assembly acessa. Uso comum: cooperação entre classes do mesmo projeto ou biblioteca, detalhes internos de framework e tipos auxiliares que não devem virar API pública.
- **`protected internal`**: abre em duas direções ao mesmo tempo: mesmo assembly **ou** subclasses em qualquer assembly. Uso comum: raro; só faz sentido quando você quer permitir colaboração interna ampla e também extensão por herança fora da biblioteca.
- **`private protected`**: o próprio tipo e subclasses, mas apenas dentro do mesmo assembly. Uso comum: frameworks e bibliotecas que querem permitir herança controlada sem abrir tanto quanto `protected`.
- **`public`**: qualquer código que enxergue o tipo pode acessar. É o mais aberto e, por padrão, o que mais aumenta a superfície de uso indevido, acoplamento e manutenção futura. Uso comum: somente no contrato que você quer realmente expor para consumidores.

Além desses, C# moderno também possui:

- **`file`**: vale para tipos de topo e limita o acesso ao mesmo arquivo `.cs`. É excelente para helpers muito locais, usados só para apoiar um tipo principal sem “vazar” nem para o resto do assembly.

#### 5.1.2 Do mais fechado ao mais aberto

Leitura mental útil:

- para **membros**, o mais fechado por padrão é `private`;
- para **tipos de topo**, o mais fechado por padrão é `file`;
- o mais aberto é `public`.

Entre os intermediários, a comparação exige cuidado:

- `protected` e `internal` não formam uma escada perfeita;
- `protected` abre para **herança**;
- `internal` abre para **colaboração dentro do assembly**;
- `protected internal` é o mais permissivo entre os modificadores protegidos;
- `private protected` é o mais restritivo entre os modificadores protegidos.

#### 5.1.3 Segurança, boa prática e vulnerabilidades

Aqui existe uma nuance importante:

- modificador de acesso ajuda muito com **encapsulamento** e **redução de superfície de uso indevido**;
- mas ele **não substitui** autenticação, autorização, validação de entrada, criptografia ou gerenciamento de segredos.

Então, quando alguém diz “mais seguro” neste contexto, o sentido correto é:

- **mais restritivo por padrão**;
- **menos código consegue tocar naquele membro**;
- **menor chance de uso acidental, acoplamento desnecessário e exposição indevida da API**.

Leitura direta de risco:

- **`public`** é o mais exposto e, por isso, o que mais aumenta a superfície de uso indevido.
- **`protected internal`** também merece bastante atenção, porque abre ao mesmo tempo para o assembly e para subclasses externas.
- **`internal`** parece “seguro” à primeira vista, mas ainda deixa o membro acessível para qualquer código do mesmo assembly; em projetos grandes isso já é bastante gente.
- **`private`** e **`file`** são os mais fechados por padrão, porque restringem muito mais quem consegue tocar no código.

Mas existe o outro lado:

- o mais fechado **não é automaticamente o melhor em qualquer cenário**;
- **`private`** demais pode deixar um design rígido quando a classe foi pensada para extensão, customização ou testes por colaboração interna;
- **`file`** demais pode esconder tipos que talvez merecessem viver como peças reaproveitáveis do assembly;
- em bibliotecas e frameworks extensíveis, fechar tudo cedo demais pode tornar a API difícil de evoluir sem retrabalho.

Boa prática geral:

- comece sempre com o modificador **mais restritivo que ainda funciona**;
- só abra para `internal`, `protected` ou `public` quando existir uma necessidade real de design;
- cada `public` novo vira parte do contrato que outros podem depender;
- quanto mais aberta a API, maior a chance de abuso, mau uso, dificuldade de refatoração e bugs por acoplamento externo.

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

**Regra prática por cenário:**

- comece pensando em **`private`** como seu padrão inicial.
  Ideia mental: "ninguém de fora precisa mexer nisso".
  Use para campos, métodos auxiliares e detalhes internos da classe.
- use **`file`** quando um tipo auxiliar só faz sentido naquele arquivo.
  Ideia mental: "isso existe só para apoiar este arquivo, não o projeto inteiro".
- use **`internal`** quando outras classes do mesmo projeto precisam acessar, mas código de fora da biblioteca não deve ver.
  Ideia mental: "é público por dentro, fechado por fora".
- use **`protected`** quando subclasses realmente precisam acessar ou sobrescrever algo.
  Ideia mental: "isso é para quem herdar de mim, não para qualquer consumidor".
- use **`private protected`** quando essa herança deve ficar ainda mais controlada.
  Ideia mental: "só subclasses do mesmo assembly podem usar".
- use **`protected internal`** com cautela, porque ele abre em duas direções ao mesmo tempo.
  Ideia mental: "subclasses podem usar, e o assembly inteiro também".
  Como regra de iniciante: se você não tem um motivo muito claro, provavelmente não precisa dele.
- use **`public`** apenas no que faz parte da API intencional.
  Ideia mental: "estou prometendo que outros códigos podem depender disso".
  Tudo que vira `public` fica mais difícil de mudar depois sem quebrar alguém.
  Em termos de exposição, ele é o que mais pede atenção.

Se bater dúvida, use esta ordem de decisão:

1. Ninguém de fora precisa acessar? Use **`private`**.
2. Só este arquivo precisa conhecer o tipo? Use **`file`**.
3. Só o mesmo projeto precisa acessar? Use **`internal`**.
4. Só subclasses precisam acessar? Use **`protected`**.
5. O acesso precisa ser realmente aberto para qualquer consumidor? Aí sim use **`public`**.

Resumo mental final:

- **mais exposto / mais fácil de abusar por padrão:** `public` (Mais cuidado ao usar)
- **mais fechado / pode ficar rígido demais se usado sem pensar:** `private`

---

## Parte 6 — Propriedades (Properties)

[⬆️ Voltar ao Sumário](#sumário)

**Como interpretar o exemplo:** A `ContaBancaria` mostra um padrão muito comum em C#: campos privados para estado interno e propriedades públicas para exposição controlada. Isso permite validar entrada, manter invariantes e evoluir a implementação sem quebrar a interface visível.

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

**Como interpretar o exemplo:** Properties parecem campos para quem consome a classe, mas se comportam como acessos controlados por `get`, `set` ou `init`. Esse recurso existe para encapsular validação, cálculo e evolução interna sem mudar o contrato externo.

Em termos de vocabulário, é aqui que entram os **getters** e **setters**:

- `get` é o acessor de **leitura**. Ele define o que acontece quando alguém consulta a propriedade.
- `set` é o acessor de **escrita**. Ele define o que acontece quando alguém atribui um novo valor.
- Dentro do `set`, a palavra-chave implícita `value` representa o valor que está sendo atribuído.
- `init` é uma forma especial de escrita permitida apenas durante a inicialização do objeto.
- `private set` significa: leitura pública, escrita restrita à própria classe.

Em outras palavras, quando alguém escreve `p.Nome`, o `get` é executado. Quando alguém escreve `p.Nome = "Ana"`, o `set` é executado. Isso é importante porque a propriedade não é apenas um campo exposto; ela é um **ponto de controle**. Você pode validar entrada, impedir estados inválidos, calcular valores dinamicamente ou até trocar a implementação interna sem mudar a forma de uso da API.

Outro conceito importante é o de **backing field** (campo de apoio). No exemplo, `_idade` é o campo real onde o valor fica armazenado, enquanto `Idade` é a propriedade que controla o acesso a esse campo. Já em `public string Nome { get; set; }`, o compilador cria esse campo automaticamente, e por isso chamamos essa forma de **auto-property**.

Essa distinção ajuda muito quem vem de Java. Em Java, normalmente você vê métodos como `getNome()` e `setNome(...)`. Em C#, a linguagem elevou esse padrão para a sintaxe nativa com `Nome { get; set; }`. O efeito arquitetural é parecido, mas a ergonomia é melhor, e várias bibliotecas do ecossistema .NET entendem properties como parte central do modelo de objetos, incluindo serialização, data binding, ORMs e ferramentas de inspeção.

#### Como ler um `get` mais "forte", com lógica real

[⬆️ Voltar ao Sumário](#sumário)

Nem todo `get` é apenas "devolver um campo".

Às vezes o `get` executa lógica real sempre que a propriedade é acessada. Isso aparece no projeto no exemplo da factory de temas, em uma propriedade que monta um relatório textual a partir do estado atual da coleção:

```csharp
public string Info
{
    get
    {
        var sb = new StringBuilder();

        for (int i = 0; i < themes.Count; i++)
        {
            var reference = themes[i];

            if (reference.TryGetTarget(out var theme))
            {
                sb.Append("Theme #")
                  .Append(i + 1)
                  .Append(": ")
                  .Append(theme is DarkTheme ? "DarkTheme" : "LightTheme")
                  .Append(" -> TextColor: ")
                  .Append(theme.TextColor)
                  .Append(", BackgroundColor: ")
                  .Append(theme.BackgroundColor)
                  .AppendLine();
            }
            else
            {
                sb.Append("Theme #")
                  .Append(i + 1)
                  .AppendLine(": collected");
            }
        }

        return sb.ToString();
    }
}
```

Leitura correta desse código:

- quando alguém escreve `factory.Info`, o `get` é chamado;
- esse `get` cria um `StringBuilder` novo;
- percorre a coleção `themes`;
- tenta recuperar cada objeto com `TryGetTarget(...)`;
- decide o texto de saída com `if`;
- e só no final devolve a string pronta com `return sb.ToString()`.

Ou seja: a propriedade `Info` não está apenas expondo um valor armazenado. Ela está **calculando** um valor sob demanda.

Isso leva a uma ideia fundamental sobre properties:

- `get` pode conter lógica arbitrária de C#;
- mas, do ponto de vista de quem consome a API, essa lógica fica escondida atrás da sintaxe de acesso a propriedade.

Por isso, o leitor precisa treinar esta pergunta:

**"esta propriedade lê estado pronto ou recalcula algo toda vez?"**

#### O que o `get` representa tecnicamente?

Do ponto de vista do compilador e do runtime, o `get` é um **método acessor**.

Então:

- `p.Nome` parece um campo na leitura;
- mas por baixo o compilador está chamando algo equivalente a um método getter.

É exatamente por isso que um `get` pode:

- ler um campo;
- validar estado;
- montar um objeto;
- percorrer listas;
- consultar outra dependência;
- ou combinar várias fontes para produzir o valor.

#### Quando um `get` assim faz sentido?

Esse estilo faz sentido quando:

- o valor representa uma visão derivada do estado atual;
- a leitura precisa refletir o momento presente;
- a API continua intuitiva como "uma propriedade do objeto".

No caso de `Info`, isso faz bastante sentido porque a propriedade representa um relatório textual do estado atual das referências rastreadas.

#### Cuidados de design com `get`

Como a sintaxe de propriedade sugere algo barato e previsível, vale tomar cuidado quando o `get` começa a fazer trabalho pesado.

Boas perguntas de design:

- esse cálculo é rápido o bastante para parecer leitura de propriedade?
- acessar a propriedade várias vezes repete trabalho caro?
- há efeitos colaterais escondidos demais para continuar sendo uma boa property?

Regra prática útil:

- se a operação parece uma **consulta barata e natural do estado**, propriedade costuma fazer sentido;
- se a operação é pesada, surpreendente, lenta ou com efeitos colaterais relevantes, um método como `BuildInfoReport()` ou `GetTrackingInfo()` pode comunicar melhor a intenção.

#### `get` e o projeto atual

O projeto já usa properties em vários níveis:

- properties simples, como `TextColor` e `BackgroundColor`, que apenas devolvem um valor;
- properties com encapsulamento, como `private set` e `init`;
- properties calculadas, como `NomeCompleto`;
- e properties com lógica mais rica, como `Info`, que monta um relatório completo em tempo de acesso.

Esse contraste é didaticamente ótimo porque mostra que `get` não significa "campo disfarçado". Significa "ponto de leitura controlada", que pode ser simples ou sofisticado dependendo do contrato da API.

> **Referências oficiais:** [Properties - C#](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/properties), [Using Properties](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/using-properties), [The `get` keyword](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/get)

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

**Como interpretar o exemplo:** Essa sintaxe faz sentido quando a intenção inteira cabe em uma única expressão. O ganho não é apenas escrever menos, e sim tornar óbvio que aquele membro tem lógica direta e não precisa de um bloco completo.

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

**Como interpretar o exemplo:** `static` separa o que pertence ao tipo do que pertence a cada instância. Isso é excelente para utilitários e estado compartilhado, mas exige cuidado porque qualquer dado estático tende a viver mais e acoplar mais partes do sistema.

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

**Como interpretar o exemplo:** `sealed` é uma forma explícita de dizer que aquela linha de extensão não deve continuar. Em design orientado a objetos, isso protege invariantes e evita que herança seja usada onde o tipo não foi pensado para ser prolongado.

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

**Como interpretar o exemplo:** Uma classe abstrata captura o que é comum a uma família de tipos sem fingir que já é concreta o bastante para ser instanciada. Ela define o que toda subclasse deve fornecer e o que a base já consegue entregar pronto.

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

**Como interpretar o exemplo:** O exemplo deixa claro que polimorfismo em C# acontece apenas quando a base abre a porta com `virtual` e a derivada participa com `override`. `new` não entra no mesmo contrato; ele apenas esconde um membro e pode surpreender quem enxerga o objeto pelo tipo da base.

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

**Como interpretar o exemplo:** `this` e `base` existem para deixar claro de onde vem a chamada: da própria classe atual ou da implementação herdada. Essa explicitude ajuda a ler melhor construção de objetos, reutilização de lógica e encadeamento em hierarquias.

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

**Como interpretar o exemplo:** O C# moderno prefere pattern matching porque ele combina teste, extração e classificação de forma mais segura e expressiva. Em vez de vários casts espalhados, você descreve a forma do valor e o compilador ajuda a manter o fluxo correto.

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

**Como interpretar o exemplo:** Aqui `using` não tem a ver com importar namespaces; ele existe para descarte determinístico de recursos que o GC não fecha no tempo certo. Arquivos, conexões e streams são exemplos clássicos em que esperar a coleta de lixo não é uma estratégia aceitável.

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

**Como interpretar o exemplo:** Os três modificadores deixam explícita a intenção de passagem por referência. `ref` compartilha leitura e escrita, `out` obriga preenchimento de saída, e `in` evita cópia sem permitir modificação do argumento pelo método chamado.

---

### 7.9 Conversões definidas pelo usuário (`implicit` e `explicit`)

[⬆️ Voltar ao Sumário](#sumário)

```csharp
public class Person
{
    public string Nome { get; set; } = string.Empty;
}

public class PersonBuilder
{
    private readonly Person _person = new Person { Nome = "Ana" };

    // Conversão implícita: o chamador pode usar o builder onde um Person é esperado
    public static implicit operator Person(PersonBuilder builder)
        => builder._person;
}

PersonBuilder builder = new PersonBuilder();
Person pessoa = builder; // chama implicit operator sem cast explícito

public readonly struct Metros
{
    public double Valor { get; }
    public Metros(double valor) => Valor = valor;

    // Conversão explícita: exige cast no ponto de uso
    public static explicit operator double(Metros m) => m.Valor;
}

double distancia = (double)new Metros(12.5);
```

Quando um tipo declara `public static implicit operator ...` ou `public static explicit operator ...`, ele está ensinando ao compilador como converter entre tipos customizados.

Essa ideia aparece no projeto no `FacetedBuilder`, com um `implicit operator Person(PersonBuilder builder)`, para permitir que o builder final seja tratado como `Person` sem exigir uma chamada manual de `Build()`.

Regra de design importante:

- conversões **implícitas** devem ser seguras, previsíveis e não surpreender;
- conversões **explícitas** são mais adequadas quando há perda de informação, custo relevante ou risco de interpretação errada.

Em termos práticos:

- `implicit` melhora ergonomia da API;
- `explicit` melhora clareza quando a conversão merece atenção visual do leitor.

**Como interpretar o exemplo:** Esse recurso permite criar APIs mais fluentes, mas também pode esconder trabalho demais se usado sem critério. A documentação oficial recomenda que conversões implícitas sempre sejam bem-comportadas; se a conversão puder falhar, lançar exceção ou perder significado, prefira `explicit`.

> **Referência oficial:** [Microsoft Learn — User-defined explicit and implicit conversion operators](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/user-defined-conversion-operators)

---

### 7.10 `nameof`, `typeof` e `default`

[⬆️ Voltar ao Sumário](#sumário)

```csharp
public class Usuario
{
    public string Nome { get; init; } = string.Empty;
}

void ValidarNome(string nome)
{
    if (string.IsNullOrWhiteSpace(nome))
        throw new ArgumentException("Nome inválido.", nameof(nome));
}

Type tipoUsuario = typeof(Usuario);

static T PrimeiroOuPadrao<T>(IEnumerable<T> itens)
{
    foreach (var item in itens)
        return item;

    return default!;
}
```

Essas três palavras aparecem o tempo todo em código profissional, mas com papéis bem diferentes:

- `nameof(...)` devolve o **nome textual** de um símbolo com segurança de refatoração;
- `typeof(...)` devolve o **objeto `Type`** correspondente ao tipo;
- `default` representa o **valor padrão** de um tipo.

Quando isso aparece na prática:

- `nameof` é muito usado em exceções, validação, logging, atributos e mensagens de erro;
- `typeof` aparece em reflexão, DI, serializers, registradores de handlers, ORMs e metaprogramação;
- `default` aparece bastante em genéricos, retorno padrão e inicialização neutra.

**Como interpretar o exemplo:** essas palavras ajudam o código a conversar melhor com o compilador e com o runtime. Em vez de espalhar strings mágicas e valores "neutros" manuais, você pede informação estrutural da linguagem de forma segura e expressiva.

> **Referências oficiais:** [The `nameof` expression](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/nameof), [Type-testing operators and cast expressions](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/type-testing-and-cast), [Default values of C# types](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/default-values)

---

### 7.11 `partial` e `file`

[⬆️ Voltar ao Sumário](#sumário)

```csharp
// Pedido.Entidade.cs
public partial class Pedido
{
    public int Id { get; set; }
}

// Pedido.Regras.cs
public partial class Pedido
{
    public bool EstaValido() => Id > 0;
}

file sealed class PedidoMapper
{
    public static string ParaTexto(Pedido pedido) => $"Pedido #{pedido.Id}";
}
```

Essas duas palavras ajudam a organizar código real:

- `partial` divide o mesmo tipo em mais de um arquivo;
- `file` limita um tipo ao arquivo atual.

Quando isso aparece na prática:

- `partial` é muito comum com **source generators**, `JsonSerializerContext`, WinForms, Razor, Entity Framework e código gerado em geral;
- `file` é excelente para helpers que só fazem sentido naquele arquivo e não deveriam "vazar" para o resto do assembly.

**Como interpretar o exemplo:** `partial` existe para composição e expansão controlada de tipos. `file` existe para contenção máxima de helpers locais. Os dois têm muito valor profissional porque ajudam a manter código grande mais organizado sem abrir API desnecessária.

> **Referências oficiais:** [Partial Classes and Members](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/partial-classes-and-methods), [Partial members](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/partial-member), [File keyword](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/file)

---

### 7.12 `yield`

[⬆️ Voltar ao Sumário](#sumário)

```csharp
public static IEnumerable<int> GerarPares(int max)
{
    for (int i = 0; i <= max; i += 2)
        yield return i;
}

foreach (var n in GerarPares(6))
    Console.WriteLine(n);
```

`yield return` permite gerar uma sequência **sob demanda**, sem construir a coleção inteira antes.

Quando isso aparece na prática:

- iteradores customizados;
- pipelines de dados;
- leitura progressiva de resultados;
- integração com LINQ e `IEnumerable<T>`.

**Como interpretar o exemplo:** `yield` é uma das chaves para entender o modelo de execução adiada do C#. Ele faz muita diferença para leitura de código profissional, porque várias APIs parecem "devolver uma coleção", mas na verdade devolvem uma sequência produzida aos poucos.

> **Referência oficial:** [The `yield` statement](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/yield)

---

### 7.13 `lock`

[⬆️ Voltar ao Sumário](#sumário)

```csharp
public class ContadorSeguro
{
    private readonly object _lock = new();
    private int _valor;

    public void Incrementar()
    {
        lock (_lock)
        {
            _valor++;
        }
    }

    public int Ler()
    {
        lock (_lock)
        {
            return _valor;
        }
    }
}
```

`lock` cria uma região de exclusão mútua: enquanto uma thread está dentro do bloco, outra não entra no mesmo lock.

Quando isso aparece na prática:

- proteger estado compartilhado mutável;
- evitar condições de corrida;
- garantir consistência em leituras e escritas concorrentes.

**Como interpretar o exemplo:** `lock` não é palavra de "performance", e sim de **corretude**. Em carreira profissional, entender quando um estado precisa de sincronização é mais importante do que decorar a sintaxe.

> **Referência oficial:** [The `lock` statement](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/lock)

---

### 7.14 `async` e `await`

[⬆️ Voltar ao Sumário](#sumário)

```csharp
public async Task<string> LerArquivoAsync(string caminho)
{
    using var reader = new StreamReader(caminho);
    return await reader.ReadToEndAsync();
}
```

Essas duas palavras são centrais na vida profissional com C# moderno:

- `async` marca que o método participa do modelo assíncrono;
- `await` suspende a continuação daquele método até a operação terminar, sem bloquear a thread chamadora.

Quando isso aparece na prática:

- chamadas HTTP;
- banco de dados;
- acesso a arquivo;
- filas, mensageria, cloud e I/O em geral.

**Como interpretar o exemplo:** `async`/`await` não é só "sintaxe bonita". É uma das fundações de backend moderno em .NET. O tema aparece em profundidade na **Parte 16**, mas já merece presença aqui porque é palavra-chave que você vai encontrar o tempo todo em produção.

> **Referências oficiais:** [Asynchronous programming with async and await](https://learn.microsoft.com/en-us/dotnet/csharp/asynchronous-programming/), [Async return types](https://learn.microsoft.com/en-us/dotnet/csharp/asynchronous-programming/async-return-types)

---

### 7.15 `required` e `init`

[⬆️ Voltar ao Sumário](#sumário)

```csharp
public class Cliente
{
    public required string Nome { get; init; }
    public required string Email { get; init; }
    public DateTime CriadoEm { get; init; } = DateTime.UtcNow;
}

var cliente = new Cliente
{
    Nome = "Ana",
    Email = "ana@empresa.com"
};
```

Essas palavras ajudam a modelar objetos mais corretos já no momento da criação:

- `required` diz ao compilador que aquele membro **precisa** ser inicializado;
- `init` permite escrita apenas durante a inicialização do objeto.

Quando isso aparece na prática:

- DTOs;
- configurações;
- request models;
- records e objetos de transporte de dados;
- modelos que devem nascer completos e depois ficar estáveis.

**Como interpretar o exemplo:** `required` e `init` aproximam ergonomia e segurança de modelagem. Eles reduzem objetos parcialmente montados e ajudam a expressar melhor a intenção do domínio sem recorrer imediatamente a construtores gigantes.

> **Referências oficiais:** [The `required` modifier](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/required), [Init only setters](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/init)

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

**Como interpretar o exemplo:** Esse bloco representa a forma mais direta de modelar decisões booleanas. O mesmo trecho também mostra que C# moderno oferece formas orientadas a valor, como operador ternário e nulidade, para escrever escolhas pequenas de forma mais concisa.

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

**Como interpretar o exemplo:** O `switch` clássico organiza vários caminhos por valor, enquanto a switch expression aproxima o controle de fluxo de uma classificação declarativa. Em C# moderno, isso torna o `switch` cada vez mais útil para padrões e formas, não apenas para substituir vários `if`s`.

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

**Como interpretar o exemplo:** Cada laço existe para uma intenção principal: `for` para contagem, `foreach` para sequências, `while` para condição e `do-while` para garantir ao menos uma execução. `break` e `continue` refinam esse fluxo, mas em excesso podem deixar a leitura mais difícil.

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

**Como interpretar o exemplo:** A assinatura de um método é um contrato de uso: ela informa retorno, parâmetros e ergonomia de chamada. Recursos como parâmetros opcionais, nomeados e `params` tornam a API mais amigável, mas também moldam a forma como outros desenvolvedores entendem o que é obrigatório.

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
string cap = "jOão".Capitalizar();     // "João"

// Métodos de extensão em IEnumerable
public static class EnumerableExtensions
{
    public static IEnumerable<T> FiltrarNulos<T>(this IEnumerable<T?> source)
        where T : class
        => source.Where(x => x != null)!;
}
```

**Como interpretar o exemplo:** Um método de extensão parece nativo do tipo alvo, mas continua sendo um método estático com sintaxe especial. Isso é excelente para criar APIs fluentes, desde que você não esconda dependências nem espalhe comportamento sem critério.

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

**Como interpretar o exemplo:** Sobrecarga funciona bem quando várias assinaturas representam a mesma ideia com entradas diferentes. Quando as versões passam a fazer coisas semanticamente distantes, o recurso deixa de melhorar a API e começa a confundir o leitor.

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

**Como interpretar o exemplo:** `enum` existe para dar nome semântico a conjuntos finitos de estados, evitando números mágicos pelo código. O cast e o parse lembram que ainda existe uma base numérica por baixo, então valores externos continuam precisando de validação.

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

**Como interpretar o exemplo:** Aqui o `enum` não representa estados únicos, e sim combinações de permissões. O detalhe técnico decisivo é usar potências de dois, para que cada bit represente uma capacidade independente e possa ser composto sem ambiguidade.

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

**Como interpretar o exemplo:** A classe foi montada como mapa dos principais elementos de um tipo orientado a objetos em C#: campos, propriedades, construtor, métodos e sobrescritas. Ler uma classe com essa lente ajuda a ver que ela não guarda apenas dados; ela define identidade, invariantes e integração com o resto do runtime.

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

**Como interpretar o exemplo:** A tabela organiza o construtor como contrato de nascimento do objeto, e não como um método qualquer. Essa distinção importa porque o construtor comunica o que é obrigatório para a instância existir em estado coerente.

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

**Como interpretar o exemplo:** O comportamento implícito do compilador resolve casos simples, mas também gera surpresa quando a classe ganha seu primeiro construtor explícito. Saber que o construtor sem parâmetros desaparece nessa hora evita muitos erros de instânciação.

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

**Como interpretar o exemplo:** Esse formato é o mais importante quando o objeto depende de dados essenciais para existir com sentido. Repare que a validação acontece antes da atribuição, o que reforça a boa prática de falhar cedo em vez de permitir estado inválido.

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

**Como interpretar o exemplo:** Sobrecargas oferecem caminhos diferentes para criar o mesmo tipo, mas o objeto final continua sendo conceitualmente o mesmo. Use esse recurso para representar inicializações legítimas, e não para misturar modelos mentais diferentes sob o mesmo nome de classe.

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

**Como interpretar o exemplo:** `this(...)` evita duplicação porque transforma um construtor em ponto central de validação e inicialização. Esse padrão é valioso quando você quer várias portas de entrada para o mesmo tipo, mas precisa manter uma única fonte de verdade.

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

**Como interpretar o exemplo:** `base(...)` garante que a classe derivada respeite o processo de inicialização da base, em vez de tentar reconstruir isso manualmente. Em outras palavras, a subclasse não deve pular as invariantes da classe pai; ela deve completa-las.

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

**Como interpretar o exemplo:** A ordem mostrada explica por que certos membros ainda não estão prontos quando outro código da hierarquia tenta acessá-los cedo demais. Esse entendimento evita bugs sutis de inicialização, principalmente em classes derivadas.

#### 11.2.8 Construtor estático

[⬆️ Voltar ao Sumário](#sumário)

Já introduzido na [Parte 7.1](#71-static); reapresentado aqui no contexto específico de inicialização de classe.

| Característica | Descrição |
|---|---|
| Assinatura | Sem modificador de acesso, sem parâmetros: `static NomeDaClasse() { ... }` |
| Execução | Uma única vez, automaticamente, antes do primeiro uso do tipo (acesso a membro estático ou primeira instânciação) — controlada pelo CLR, não pelo programador. |
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

**Como interpretar o exemplo:** O construtor estático existe para inicialização do tipo, não do objeto. Ele roda uma única vez, sob controle do CLR, e por isso deve ser reservado para setup realmente global do tipo.

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

**Como interpretar o exemplo:** Tornar o construtor privado restringe quem pode criar instâncias e por qual caminho. Isso é útil quando a própria classe precisa controlar cardinalidade, cache, factories ou políticas especiais de criação.

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

**Como interpretar o exemplo:** O ganho aqui é reduzir boilerplate em classes cujo construtor apenas recebe e expõe dados. O cuidado é não sacrificar legibilidade: quando a classe cresce demais, a forma tradicional pode voltar a ser mais clara.

#### 11.2.11 Tabela-resumo dos tipos de construtor

[⬆️ Voltar ao Sumário](#sumário)

| Tipo | Modificador | Quando executa | Uso típico |
|---|---|---|---|
| Padrão (implícito) | `public` (gerado) | Na ausência de qualquer construtor declarado | Classes simples sem necessidade de inicialização |
| Parametrizado | `public`/outro | A cada `new` com os argumentos correspondentes | Inicialização obrigatória de estado |
| Estático | (nenhum) | Uma única vez, antes do primeiro uso do tipo | Inicialização de campos `static` |
| Privado | `private` | Apenas internamente à própria classe | Singleton, métodos fábrica |
| Primário (C# 12) | conforme classe | Igual ao parametrizado | Redução de verbosidade em classes simples |

**Como interpretar o exemplo:** A tabela final deixa claro que o tipo de construtor escolhido comunica intenção arquitetural, e não apenas gosto sintático. Em geral, o melhor construtor é o mais simples que ainda deixa o objeto nascer válido e fácil de usar.

**Como interpretar o exemplo:** Todo o capítulo de construtores gira em torno da mesma ideia: o momento do `new` é a hora de garantir que o objeto nasça válido. Os diferentes estilos existem para equilibrar clareza, reaproveitamento da lógica de inicialização e proteção de invariantes.

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

**Como interpretar o exemplo:** Records são ideais quando o foco do tipo está nos dados e não na identidade da instância. O exemplo destaca três ideias centrais: igualdade por valor, cópia imutável com `with` e sintaxe compacta para modelos orientados a dados.

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
- o construtor do `Pedido` cópia do builder os valores acumulados.

Em outras palavras, o builder funciona como uma "área de montagem temporária". O produto final só nasce no `Build()`.

> Em C# moderno, o padrão builder frequentemente é substituído por **object initializers** e **init-only properties**, que oferecem sintaxe mais limpa sem classe auxiliar.

---

## Parte 12 — Herança e Polimorfismo

[⬆️ Voltar ao Sumário](#sumário)

**Como interpretar o exemplo:** Builder resolve bem objetos com poucos parâmetros obrigatórios e vários opcionais, especialmente quando o conjunto de construtores ficaria ilegível. Ele também aproxima a chamada da forma como a pessoa pensa no objeto: primeiro configura, depois materializa com `Build()`.

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

**Como interpretar o exemplo:** O trecho com `Animal animal = new Cachorro(...)` é o teste mais importante da herança: o consumidor trabalha com a abstração e o runtime despacha o comportamento especializado correto. Esse é o uso saudável da herança em C#: representar um verdadeiro relacionamento `é um` sem quebrar o contrato da base.

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

**Como interpretar o exemplo:** A força da interface aparece quando o consumidor passa a conhecer a capacidade e não a classe concreta. Esse movimento reduz acoplamento, melhora testabilidade e facilita evolução do sistema sem reescrever quem usa a API.

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

**Como interpretar o exemplo:** Delegate é a forma que C# usa para tratar comportamento como valor de primeira classe, mas com segurança de assinatura. O exemplo de multicast mostra que, além de apontar para um método, um delegate pode representar uma cadeia de callbacks executados em sequência.

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

**Como interpretar o exemplo:** Esses delegates genéricos existem para evitar a criação de tipos customizados em cenários muito comuns. Quando você domina `Func`, `Action` e `Predicate`, passa a ler com mais naturalidade APIs modernas do .NET e do LINQ.

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

**Como interpretar o exemplo:** Lambda é a forma compacta de escrever uma função anônima no ponto em que ela é usada. O valor real aparece quando você percebe que ela aproxima o código da intenção, sem obrigar a nomear um método auxiliar para algo muito local.

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

**Como interpretar o exemplo:** `event` encapsula um delegate para permitir inscrição e cancelamento sem liberar disparo externo arbitrário. Assim, o publicador controla quando algo aconteceu, e os assinantes controlam apenas como reagir.

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

**Como interpretar o exemplo:** O ponto central é enxergar a query como pipeline declarativo: fonte, operadores e momento de materialização. Quando você pensa assim, LINQ deixa de parecer mágica elegante e passa a ser uma forma precisa de modelar transformação de dados.

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

**Como interpretar o exemplo:** A categoria de cada operador importa mais do que decorar todas as assinaturas. Quem domina filtro, projeção, agregação, busca, agrupamento e conjunto consegue ler quase qualquer pipeline LINQ com rapidez.

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

Isso é melhor do que retornar `List<T>` quando o chamador não precisa de operações específicas da lista.

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

**Como interpretar o exemplo:** O exemplo com `yield return` reforça que sequência não é sinônimo de lista pronta. Em design de API, expor `IEnumerable<T>` é prometer iteração, e não indexação, contagem barata ou armazenamento materializado.

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

**Como interpretar o exemplo:** `IQueryable<T>` muda a conversa porque a consulta deixa de ser apenas uma sequência local e passa a ser uma descrição que outro provider pode reinterpretar, como SQL em um ORM. O exemplo existe justamente para treinar a pergunta: isso vai rodar no banco ou em memória?

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

**Como interpretar o exemplo:** Execução adiada é uma das maiores fontes de surpresa em LINQ, porque escrever a query não significa executá-la. O exercício mental correto é sempre perguntar onde a pipeline é consumida e em que ponto o resultado vira dados concretos.

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
| `Dictionary<TKey, TValue>` | Tabela hash | Busca por chave muito rápida | Chave deve ser estável e bem comparável |
| `SortedDictionary<TKey, TValue>` | Mapa ordenado | Chaves sempre em ordem | Mais caro que `Dictionary` comum |
| `ConcurrentDictionary<TKey, TValue>` | Dicionário thread-safe | Concorrência | Não substitui desenho correto de sincronização |

Repare em um ponto sutil, porém crítico:

- `IEnumerable<T>` fala sobre **enumeração**.
- `List<T>` fala sobre **estrutura concreta**.
- `IReadOnlyList<T>` fala sobre **capacidade de leitura por índice**.

Confundir essas coisas leva a APIs mal desenhadas.

**Como interpretar o exemplo:** A seção separa contratos de coleção de implementações concretas porque esse erro aparece o tempo todo em APIs. Saber se você está prometendo iteração, índice, cardinalidade, unicidade ou mapeamento por chave já resolve boa parte das escolhas de forma honesta.

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
- você precisa mapear por chave: use `Dictionary<TKey, TValue>`;
- você precisa garantir imutabilidade compartilhada: considere coleções imutáveis;
- você expõe a lista só para leitura: talvez `IReadOnlyList<T>` comunique melhor a intenção da API.

Custos clássicos de `List<T>`:

- `Add` no final: amortizado `O(1)`;
- acesso por índice: `O(1)`;
- `Insert` no meio: `O(n)`;
- `RemoveAt` no meio: `O(n)`;
- busca por valor com `Contains`: `O(n)`.

**Como interpretar o exemplo:** `List<T>` é excelente porque equilibra simplicidade e desempenho para a maioria dos cenários, mas continua sendo um array redimensionável com custos específicos. Entender esses custos evita o hábito ruim de tratá-la como solução universal só por ser familiar.

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

**Como interpretar o exemplo:** O foco do dicionário não é ordem, e sim acesso rápido por chave. O exemplo também ensina que escolher o comparador correto e usar `TryGetValue` ou `GetValueOrDefault` costuma produzir código mais robusto do que assumir que a chave sempre existirá.

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
| Concorrência com chave/valor | `ConcurrentDictionary<TKey, TValue>` | API pensada para múltiplas threads |

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

**Como interpretar o exemplo:** A tabela resume um princípio central de design: escolha primeiro a semântica do acesso e só depois a estrutura concreta. A melhor coleção não é a mais famosa, e sim a que comunica com honestidade o que o consumidor poderá fazer com os dados.

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

**Como interpretar o exemplo:** O ganho de `async` e `await` não é criar várias threads automaticamente, e sim permitir espera sem bloqueio da thread chamadora. Isso deixa o fluxo legível como código sequencial, mas com comportamento muito melhor para operações de I/O.

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

**Como interpretar o exemplo:** O trecho junta três padrões muito comuns: coordenação de múltiplas tasks, cancelamento cooperativo e cuidado com contexto de sincronização. Quando esses conceitos ficam claros, o restante da programação assíncrona em C# passa a formar um modelo coerente.

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

**Como interpretar o exemplo:** `ValueTask` existe para cenários em que o caminho síncrono é frequente e a alocação de `Task` se torna custo relevante, mas isso não o transforma em escolha padrão. Em geral, prefira `Task` por simplicidade e só use `ValueTask` com benefício mensurável.

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

**Como interpretar o exemplo:** Generics permitem escrever uma estrutura ou algoritmo uma vez e ainda preservar segurança de tipo em compile-time. O repositório genérico mostra justamente essa ideia: a lógica é reaproveitada, mas o compilador continua sabendo qual tipo concreto está sendo manipulado.

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

**Como interpretar o exemplo:** Constraint é a forma de limitar o universo dos tipos aceitos para que o corpo genérico possa assumir certas capacidades. Em vez de `qualquer T`, o código passa a trabalhar com um `T` que, por contrato, é instanciável, comparável ou compatível com outra abstração.

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

**Como interpretar o exemplo:** A direção da variância fica clara quando você pensa em fluxo de dados: quem produz valores pode ser covariante, e quem consome valores pode ser contravariante. Esse raciocínio evita decorar regras e ajuda a entender por que algumas atribuições são seguras.

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

**Como interpretar o exemplo:** O bloco mostra a ordem correta de captura: exceções específicas primeiro, genérica por último, e `finally` para limpeza obrigatória. O detalhe de `throw;` versus `throw ex;` é vital porque afeta o stack trace e, portanto, a qualidade do diagnóstico.

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

**Como interpretar o exemplo:** Exceções customizadas fazem mais sentido quando carregam contexto de domínio e não apenas um nome diferente. Ao adicionar propriedades e uma hierarquia pensada, a exceção passa a servir também como estrutura útil para observabilidade e tratamento especializado.

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

**Como interpretar o exemplo:** A árvore ajuda a perceber que capturar exceção em C# também é decisão de modelagem: quanto mais alto você captura, mais geral e menos específico fica o tratamento. Conhecer as famílias principais ajuda a escrever `catch` mais intencionais.

---

### 18.4 Exceções de argumento e implementação comuns

[⬆️ Voltar ao Sumário](#sumário)

```csharp
public class Produto
{
    public string Nome { get; }

    public Produto(string nome, int estoqueInicial)
    {
        ArgumentNullException.ThrowIfNull(nome);

        if (estoqueInicial < 0)
            throw new ArgumentOutOfRangeException(nameof(estoqueInicial),
                "Estoque não pode ser negativo.");

        Nome = nome;
    }
}

public interface IScanner
{
    void Scan();
}

public class ImpressoraAntiga : IScanner
{
    public void Scan()
    {
        throw new NotImplementedException();
    }
}
```

Três exceções aparecem com frequência no projeto e merecem leitura separada:

- `ArgumentNullException`: use quando um argumento obrigatório recebeu `null`;
- `ArgumentOutOfRangeException`: use quando o argumento existe, mas seu valor está fora do intervalo aceito;
- `NotImplementedException`: use para sinalizar que um método ou operação ainda não foi implementado.

Diferença conceitual importante:

- `ArgumentNullException` e `ArgumentOutOfRangeException` validam **entrada inválida do chamador**;
- `NotImplementedException` sinaliza **incompletude da implementação**, e não erro de parâmetro.

No contexto do projeto, `ArgumentNullException` e `ArgumentOutOfRangeException` aparecem em construtores, builders e factories para proteger invariantes logo na entrada. Já `NotImplementedException` aparece didaticamente na aula de ISP para mostrar um contrato ruim: a classe foi forçada a fingir uma capacidade que não possui.

Boa prática moderna:

- em .NET atual, `ArgumentNullException.ThrowIfNull(...)` reduz ruído em guard clauses;
- para faixas numéricas, `ArgumentOutOfRangeException` comunica intenção melhor do que `ArgumentException` genérica;
- `NotImplementedException` deve ser temporária ou pedagógica; em código de produção, se ela vira estado permanente, normalmente revela desenho inadequado da API.

**Como interpretar o exemplo:** Essas exceções não são detalhes cosméticos. Elas comunicam contrato. Quem lê `ArgumentNullException` entende que a API exige presença; quem lê `ArgumentOutOfRangeException` entende que existe um domínio válido de valores; quem lê `NotImplementedException` entende que a operação prometida ainda não existe — ou talvez nem devesse ter sido prometida.

> **Referências oficiais:** [ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception?view=net-10.0), [ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception?view=net-10.0), [NotImplementedException](https://learn.microsoft.com/en-us/dotnet/api/system.notimplementedexception?view=net-10.0)

---

## Parte 19 — Attributes (Annotations)

[⬆️ Voltar ao Sumário](#sumário)

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

**Como interpretar o exemplo:** Attribute é metadado declarativo: ele não muda o comportamento por si só, mas informa a compiladores, bibliotecas e frameworks como tratar aquele membro. O exemplo mostra usos comuns como orientação do compilador com `[Obsolete]` e orientação de serialização com `System.Text.Json`.

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

**Como interpretar o exemplo:** Criar um atributo customizado é enriquecer o código com intenção que pode ser lida depois por reflexão, tooling ou infraestrutura própria. Esse padrão é valioso quando você quer descrever comportamento de forma declarativa em vez de espalhar configurações manuais.

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

**Como interpretar o exemplo:** Tuplas resolvem bem o problema de devolver ou agrupar poucos valores relacionados sem criar um tipo formal de imediato. O cuidado é que, quando a estrutura ganha significado próprio de domínio, um tipo nomeado costuma comunicar melhor.

---

### 20.2 `WeakReference<T>` e referências fracas no GC

[⬆️ Voltar ao Sumário](#sumário)

```csharp
public class Tema
{
    public string Nome { get; set; } = string.Empty;
}

Tema? tema = new Tema { Nome = "DarkTheme" };
var weak = new WeakReference<Tema>(tema);

// Enquanto existe uma referência forte, o objeto continua claramente alcançável
if (weak.TryGetTarget(out var alvoVivo))
{
    Console.WriteLine(alvoVivo.Nome);
}

tema = null; // remove esta referência forte

// Em algum ciclo futuro do GC, o objeto pode ou não ainda existir aqui
if (weak.TryGetTarget(out var talvezAindaVivo))
{
    Console.WriteLine($"Ainda existe: {talvezAindaVivo.Nome}");
}
else
{
    Console.WriteLine("O objeto já foi coletado.");
}
```

Para entender `WeakReference<T>`, primeiro vale separar dois conceitos:

- **referência forte**: a referência comum do dia a dia em C#; enquanto o objeto continua alcançável por referências fortes, ele não deve ser coletado;
- **referência fraca**: uma referência que aponta para o objeto, mas não impede o Garbage Collector de recuperá-lo quando não restarem referências fortes.

Se a sua dúvida também envolver `Ref<ITheme>` e handles, consulte a seção [3.6 Referências diretas, indiretas e fracas](#36-referências-diretas-indiretas-e-fracas). Lá o foco é o caminho de acesso ao objeto; aqui, o foco é a relação da referência com o GC.

Em linguagem simples:

- uma variável como `Tema tema = new Tema();` é uma referência forte;
- uma `WeakReference<T>` é mais parecida com um "atalho observável", e não com posse real do objeto.

Esse tipo aparece no projeto na aula `Object Tracking and Bulk Replacement`, em que a factory quer:

- saber quais objetos já nasceram;
- sem ser a responsável por mantê-los vivos para sempre.

Importante: isso não significa que toda factory naturalmente evolui para tracking ou bulk replacement.

Essas ideias devem ser vistas como capacidades opcionais que podem ser adicionadas a uma factory quando a centralização da criação precisa resolver algo a mais além de instanciar.

#### O que significa "handle" nesse exemplo?

Na aula do projeto, a classe `Ref<ITheme>` é descrita como um **handle mutável**.

Aqui, "handle" está sendo usado no sentido mais geral de software design:

- é um objeto intermediário que você segura;
- ele te dá acesso indireto a outro objeto;
- e esse nível de indireção permite trocar o alvo real sem trocar a referência externa.

Uma imagem mental simples ajuda bastante:

- `Ref<ITheme>` é a caixa;
- o cliente segura a caixa;
- o tema atual fica dentro da caixa, em `Value`;
- a factory pode trocar o conteúdo da caixa;
- o cliente continua com a mesma caixa, mas passa a enxergar outro tema.

Leitura mental:

- o cliente segura o handle;
- o handle aponta para o objeto atual;
- a factory pode trocar o objeto apontado;
- o cliente continua com o mesmo handle, mas passa a enxergar outro conteúdo.

Em forma de seta:

`cliente -> Ref<ITheme> -> Value -> ITheme atual`

Logo, neste exemplo:

- o handle é `Ref<ITheme>`;
- o tema real não é o handle;
- `Value` é o ponto pelo qual o handle alcança o tema atual.

Importante distinguir dois usos da palavra:

- no exemplo do projeto, handle significa **referência indireta controlada**;
- em APIs como `SafeHandle`, handle significa um **identificador/encapsulamento seguro de recurso de sistema operacional**.

Os dois usos compartilham a ideia de "algo que você segura para chegar em outra coisa", mas não são o mesmo conceito técnico.

#### O que seria a "troca em massa" do bulk replacement?

No exemplo do projeto, "troca em massa" significa atualizar vários clientes com uma única chamada da factory.

Imagine que três partes da aplicação estejam segurando estes handles:

- `headerTheme`
- `sidebarTheme`
- `footerTheme`

Se todos eles apontarem para temas escuros, uma única chamada:

```csharp
replaceableFactory.ReplaceTheme(dark: false);
```

faz a factory percorrer todos os handles vivos e trocar o `Value` de cada um para um novo `LightTheme`.

Então, o "em massa" está aqui:

- não é um único objeto sendo trocado;
- são vários handles sendo atualizados numa operação centralizada;
- os clientes não precisam recriar seus temas um por um.

Ponto importante:

- no **object tracking**, a factory quer observar objetos já criados;
- no **bulk replacement**, a factory quer atualizar o objeto ativo visto por vários clientes.

Por isso, o bulk replacement costuma usar algum rastreamento internamente, mas com outro foco:

- no tracking, a factory rastreia para inspecionar;
- no bulk replacement, a factory rastreia para conseguir substituir.

Leitura mais precisa:

- factory é o mecanismo base de centralizar a criação;
- object tracking é uma capacidade opcional baseada nessa centralização;
- bulk replacement é outra capacidade opcional, também baseada nessa centralização.

#### O que o GC faz nesse cenário?

O Garbage Collector do .NET gerência a memória dos objetos no heap gerenciado. Quando um objeto deixa de ser alcançável por referências fortes, ele se torna elegível para coleta. Se só restarem weak references, isso não é suficiente para preservar o objeto.

Ponto crucial:

- `WeakReference<T>` **não garante** que o objeto existirá no próximo acesso;
- ela só permite **tentar** recuperar o alvo com `TryGetTarget(...)`.

#### Como ler `TryGetTarget(...)`

`TryGetTarget(out T target)` devolve:

- `true` quando o objeto ainda existe e o `out` recebe uma referência forte temporária para uso imediato;
- `false` quando o objeto já foi coletado.

Isso explica por que o uso correto de weak reference quase sempre começa com um `if`.

#### Quando isso faz sentido?

Casos clássicos:

- caches em que você quer reaproveitar um objeto se ele ainda estiver vivo;
- rastreamento de objetos sem "posse" deles;
- estruturas auxiliares de observação, como no exemplo das factories do projeto.

Quando isso **não** faz sentido:

- como substituto geral para referências comuns;
- quando a lógica depende de o objeto continuar existindo com previsibilidade;
- quando você quer controle determinístico de tempo de vida — isso é outra conversa (`IDisposable`, `using`, ownership explícito).

**Como interpretar o exemplo:** `WeakReference<T>` não é um "jeito avançado de guardar objetos". É um jeito específico de observá-los sem assumir propriedade de tempo de vida. A diferença entre "eu uso este objeto" e "eu só quero saber se ele ainda existe" é exatamente o que torna essa API importante.

> **Referências oficiais:** [WeakReference<T>](https://learn.microsoft.com/en-us/dotnet/api/system.weakreference-1?view=net-10.0), [WeakReference<T>.TryGetTarget](https://learn.microsoft.com/en-us/dotnet/api/system.weakreference-1.trygettarget?view=net-10.0), [Fundamentos de garbage collection](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/fundamentals)

---

### 20.3 Span\<T\> e Memory\<T\> — zero-allocation slicing

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

**Como interpretar o exemplo:** `Span<T>` mostra que é possível trabalhar com janelas de memória sem copiar dados, algo muito útil em parsing e processamento de buffers. A contrapartida é lidar com regras mais rígidas de tempo de vida e segurança.

---

### 20.4 Sealed classes com Pattern Matching (como Discriminated Union)

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

**Como interpretar o exemplo:** A combinação de `record`, tipos fechados e pattern matching cria uma forma elegante de modelar estados possíveis sem deixar brechas para formatos inválidos. O resultado é um fluxo de tratamento mais explícito e mais seguro.

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

**Como interpretar o exemplo:** O exemplo existe mais como base conceitual do que como recomendação diária. Em aplicações modernas, criar `Thread` manualmente é raro; o aprendizado importante é entender a diferença entre thread real, pool gerenciado e abstrações mais altas.

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

**Como interpretar o exemplo:** A TPL oferece uma camada mais expressiva para concorrência e paralelismo do que a manipulação manual de threads. O essencial é distinguir trabalho CPU-bound, onde `Parallel` e PLINQ ajudam, de I/O-bound, onde `async` continua sendo o modelo certo.

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

**Como interpretar o exemplo:** Sincronização existe para proteger estado compartilhado, mas cada ferramenta tem custo e semântica próprios. `lock`, `Interlocked` e `SemaphoreSlim` não são equivalentes; escolher a menor ferramenta que resolve o problema ajuda a manter corretude sem pagar complexidade desnecessária.

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

**Como interpretar o exemplo:** Reflection permite que o programa inspecione tipos e invoque membros dinamicamente em runtime, o que é poderoso para tooling, frameworks e integração genérica. O preço é perder parte da previsibilidade e da segurança do código fortemente tipado.

Leitura mental rápida:

- com código normal, você já sabe em compile-time quais tipos vai usar;
- com reflection, o programa descobre ou manipula tipos em runtime.

Na prática, reflection responde perguntas como:

- "que classes existem neste assembly?";
- "essa classe implementa tal interface?";
- "consigo criar uma instância desse tipo agora?";
- "quais propriedades e métodos esse tipo possui?".

É por isso que reflection aparece em cenários como:

- frameworks;
- ORMs;
- serialização;
- leitura de attributes;
- sistemas de plugin;
- infraestrutura genérica.

No exemplo da `Aula08_AbstractFactoryOCP`, reflection é usada justamente para descobrir, em tempo de execução, quais classes implementam `IHotDrinkFactory`.

---

### 22.2 Dependency Injection (DI)

[⬆️ Voltar ao Sumário](#sumário)

```csharp
public interface IMessageWriter
{
    void Write(string message);
}

public class ConsoleMessageWriter : IMessageWriter
{
    public void Write(string message) => Console.WriteLine(message);
}

public class Worker
{
    private readonly IMessageWriter _writer;

    public Worker(IMessageWriter writer)
    {
        _writer = writer;
    }

    public void Run()
    {
        _writer.Write("Executando...");
    }
}
```

**Como interpretar o exemplo:** Dependency Injection (`DI`) é um jeito de uma classe receber as dependências de que precisa, em vez de criá-las sozinha com `new`.

No exemplo acima, `Worker` precisa de algo que escreva mensagens, mas ele não cria `ConsoleMessageWriter` diretamente.

Ele apenas declara sua necessidade no construtor:

```csharp
IMessageWriter writer
```

Essa é a ideia central:

- a classe diz do que precisa;
- outra parte do sistema entrega isso a ela.

Distinção importante para carreira profissional:

- **DI** é o princípio/padrão;
- **DI container** é a ferramenta que automatiza registro, criação e entrega dessas dependências.

Nuance importante:

- `DI` e `reflection` não são opostos absolutos;
- muitos containers usam reflection internamente para construir objetos;
- mas, do ponto de vista da sua classe, ela continua apenas declarando dependências no construtor.

Em .NET, o container costuma trabalhar com registros como:

```csharp
services.AddSingleton<IMessageWriter, ConsoleMessageWriter>();
```

e depois injeta a implementação certa no construtor da classe consumidora.

Ganhos comuns de DI:

- menos acoplamento a classes concretas;
- mais testabilidade;
- composição mais clara da aplicação;
- troca de implementações com menos impacto.

Conexão útil com `Abstract Factory`:

- com reflection, a máquina descobre sozinha quais factories existem;
- com DI, a máquina poderia receber `IEnumerable<IHotDrinkFactory>` já pronto.

---

### 22.3 Source Generators (C# 9+)

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

**Como interpretar o exemplo:** Source Generators deslocam trabalho para a compilação, reduzindo reflexão e boilerplate em runtime. O exemplo do contexto JSON mostra justamente essa troca: em vez de descobrir tudo dinamicamente na execução, o compilador já produz o código especializado.

---

### 22.4 Unsafe code e ponteiros

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

**Como interpretar o exemplo:** `unsafe` remove parte das proteções que normalmente tornam C# seguro e previsível. Isso só se justifica em interop, manipulação de memória ou otimizações muito específicas, porque o desenvolvedor passa a assumir riscos que o runtime costuma administrar.

---

## Resumo Geral — Conceitos Fundamentais

[⬆️ Voltar ao Sumário](#sumário)

| Conceito | Definição resumida |
|---|---|
| **CLR** | Runtime do .NET — executa IL/CIL sobre qualquer SO |
| **IL / CIL** | Código intermediário produzido pelo compilador Roslyn |
| **Tipo de valor** | Armazena dado diretamente; cópia na atribuição: `int`, `struct`, `enum` |
| **Tipo de referência** | Armazena endereço para objeto no heap: `class`, `string`, arrays |
| **Assembly** | Unidade compilada do .NET, normalmente `.dll` ou `.exe`, com IL, metadados e referências |
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
| **`private`** | Visível apenas no próprio tipo; menor superfície de acesso |
| **`protected`** | Visível no tipo e em subclasses |
| **`public`** | Visível por qualquer código que enxergue o tipo; maior superfície |
| **`internal`** | Visível apenas dentro do mesmo assembly (≈ package-private do Java) |
| **`protected internal`** | Visível no assembly + subclasses de qualquer assembly |
| **`private protected`** | Visível apenas em subclasses dentro do mesmo assembly |
| **`file`** | Tipo visível apenas no mesmo arquivo fonte |
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
| **Dependency Injection (DI)** | Entrega de dependências de fora para dentro da classe, em vez de criá-las internamente |

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

**Como interpretar o exemplo:** O pipeline IL para IL2CPP mostra que o C# de Unity continua sendo C#, mas executado sob restrições e etapas de build muito particulares do engine. Essa diferença explica por que certos hábitos da linguagem convivem com preocupações de AOT, GC e frame loop.

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

**Como interpretar o exemplo:** O script evidencia que, no Unity, o modelo de programação é guiado pelo ciclo de vida da engine e pela serialização do Inspector, não por um `Main` tradicional. Ler `MonoBehaviour` corretamente significa entender o contrato entre seu código e o runtime do jogo.

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

**Como interpretar o exemplo:** O diagrama existe para responder uma pergunta prática de todo iniciante em Unity: em qual método essa lógica deve ficar? Quando você entende o papel e o tempo de `Awake`, `Start`, `Update`, `FixedUpdate` e companhia, muitos bugs de timing deixam de parecer aleatórios.

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

**Como interpretar o exemplo:** O valor do `ScriptableObject` está em separar dados de configuração das instâncias de cena, reduzindo duplicação e acoplamento com `MonoBehaviour`. Em jogos, isso melhora reuso, balanceamento e o fluxo de trabalho entre programação e design.

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

**Como interpretar o exemplo:** Coroutine em Unity não é thread nem `Task`; ela é uma rotina cooperativa pausada e retomada ao longo dos frames. Isso combina muito bem com efeitos temporizados, esperas condicionais e sequências visuais integradas ao loop do jogo.

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

**Como interpretar o exemplo:** O contraste entre `UnityEvent` e `event` do C# mostra duas filosofias úteis: uma voltada ao editor e outra ao código. Em times reais, a escolha depende tanto de arquitetura quanto do fluxo de trabalho entre programadores, UI e designers.

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

**Como interpretar o exemplo:** Todos os exemplos desta seção giram em torno da mesma regra: o frame loop amplifica pequenos custos porque eles se repetem o tempo todo. Cache de componentes, redução de alocação, pooling e jobs existem para tirar trabalho caro do caminho mais quente do jogo.

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

**Como interpretar o exemplo:** Os padrões apresentados aparecem muito em jogos porque estados, eventos e objetos de vida curta precisam ser coordenados o tempo inteiro. O mais importante não é decorar os nomes, e sim entender o problema que cada padrão resolve e o custo que ele traz.

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

**Como interpretar o exemplo:** Mudar de engine não apaga os fundamentos da linguagem; o que muda é o vocabulário do runtime e do editor. Quem entende bem ciclo de vida, callbacks e composição em C# se adapta muito melhor às diferenças entre Unity e Godot.

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

## Parte 24 — Arquitetura de Aplicações C#/.NET

[⬆️ Voltar ao Sumário](#sumário)

Esta parte não tenta transformar C# em uma "linguagem arquitetural". C# é uma linguagem; arquitetura é uma decisão de design sobre como organizar responsabilidades, dependências, dados, regras de negócio, integrações e deploy.

O que existe, na prática, é um ecossistema .NET com padrões muito comuns: aplicações web ASP.NET Core, APIs, workers, bibliotecas, serviços, sistemas distribuídos, aplicações desktop, jogos e aplicações cloud-native. A documentação oficial da Microsoft trata várias dessas arquiteturas nos guias de [.NET Architecture](https://learn.microsoft.com/en-us/dotnet/architecture/).

O objetivo aqui é dar ao leitor um mapa curto e tecnicamente honesto: entender quais padrões aparecem com frequência em projetos C#/.NET, que problema cada um tenta resolver e onde estudar pela fonte original ou oficial.

---

### 24.1 C# não impõe arquitetura

[⬆️ Voltar ao Sumário](#sumário)

C# oferece recursos de linguagem e runtime que facilitam certos estilos arquiteturais:

- interfaces;
- classes abstratas;
- generics;
- delegates e events;
- attributes;
- reflection;
- async/await;
- LINQ;
- records;
- dependency injection no ecossistema .NET.

Mas nenhum desses recursos obriga a aplicação a seguir uma arquitetura específica.

Uma aplicação C# pode ser:

- monolítica;
- modular;
- em camadas;
- orientada a domínio;
- baseada em eventos;
- distribuída em microservices;
- um jogo com arquitetura baseada em componentes;
- um script simples de console.

A pergunta arquitetural correta não é "qual arquitetura C# usa?", e sim:

**qual organização deixa este sistema mais simples de evoluir, testar, manter e operar?**

**Fonte oficial:** Microsoft Learn — [.NET Architecture guides](https://learn.microsoft.com/en-us/dotnet/architecture/).

---

### 24.2 Arquitetura em camadas

[⬆️ Voltar ao Sumário](#sumário)

A arquitetura em camadas organiza a aplicação por responsabilidades horizontais. Um desenho comum em C#/.NET é:

```text
Presentation / API
        ↓
Application / Use Cases
        ↓
Domain / Business Rules
        ↓
Infrastructure / Database, Files, External APIs
```

Cada camada tem um papel:

- **Presentation/API:** recebe requisições, valida formato de entrada e devolve resposta.
- **Application:** coordena casos de uso, transações e chamadas ao domínio.
- **Domain:** concentra regras de negócio.
- **Infrastructure:** conversa com banco de dados, mensageria, arquivos e serviços externos.

O erro comum é transformar "camadas" em apenas pastas com nomes bonitos, mas deixar qualquer camada chamar qualquer outra. A arquitetura só existe de verdade quando as dependências são controladas.

Em projetos ASP.NET Core, isso costuma aparecer como:

```text
MyApp.Api
MyApp.Application
MyApp.Domain
MyApp.Infrastructure
```

Não é obrigatório separar em projetos diferentes desde o início, mas essa separação ajuda quando o sistema cresce.

**Fonte oficial:** Microsoft Learn — [Common web application architectures](https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures).

---

### 24.3 Clean Architecture, Onion e Ports and Adapters

[⬆️ Voltar ao Sumário](#sumário)

A documentação da Microsoft explica que aplicações que seguem o **Dependency Inversion Principle** e princípios de **Domain-Driven Design** tendem a chegar a uma arquitetura parecida, conhecida por nomes como:

- Hexagonal Architecture;
- Ports and Adapters;
- Onion Architecture;
- Clean Architecture.

A ideia central é inverter a dependência:

```text
Interface externa → Application → Domain
Infrastructure → implementa contratos definidos pelo núcleo
```

O núcleo da aplicação não deve depender diretamente de detalhes como:

- Entity Framework Core;
- banco SQL específico;
- API externa específica;
- fila específica;
- framework web específico.

Em vez disso, o núcleo define contratos:

```csharp
public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(OrderId id);
    Task SaveAsync(Order order);
}
```

E a infraestrutura implementa:

```csharp
public sealed class EfOrderRepository : IOrderRepository
{
    public Task<Order?> GetByIdAsync(OrderId id)
    {
        // implementação com EF Core
    }
}
```

O benefício é proteger regras de negócio contra detalhes técnicos. O custo é mais abstração. Em sistemas pequenos, isso pode ser exagero; em sistemas com domínio complexo, integrações e vida longa, costuma pagar o investimento.

**Fonte oficial:** Microsoft Learn — [Common web application architectures](https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures).

---

### 24.4 Domain-Driven Design (DDD)

[⬆️ Voltar ao Sumário](#sumário)

`Domain-Driven Design` é uma abordagem para sistemas em que a complexidade principal está no domínio de negócio, não apenas em telas, CRUD ou infraestrutura.

Em C#/.NET, DDD costuma aparecer com:

- entidades;
- value objects;
- aggregates;
- domain services;
- repositories;
- domain events;
- ubiquitous language;
- bounded contexts.

Um exemplo de `Value Object` em C#:

```csharp
public readonly record struct Money(decimal Amount, string Currency)
{
    public Money
    {
        if (Amount < 0)
            throw new ArgumentOutOfRangeException(nameof(Amount));

        if (string.IsNullOrWhiteSpace(Currency))
            throw new ArgumentException("Currency is required.", nameof(Currency));
    }
}
```

O ponto não é usar `record` porque é moderno. O ponto é representar um conceito do domínio com igualdade por valor e invariantes claras.

Uma leitura importante:

- se o sistema é CRUD simples, DDD completo pode ser peso desnecessário;
- se o sistema tem regras de negócio ricas, vocabulário complexo e muitas exceções, DDD ajuda a organizar o modelo mental.

**Fonte oficial Microsoft:** [Designing a DDD-oriented microservice](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/ddd-oriented-microservice).  
**Livro original:** Eric Evans — *Domain-Driven Design: Tackling Complexity in the Heart of Software*. Referência do autor: [Domain Language — DDD resources](https://www.domainlanguage.com/ddd/).

---

### 24.5 CQRS e separação entre comandos e consultas

[⬆️ Voltar ao Sumário](#sumário)

`CQRS` significa **Command and Query Responsibility Segregation**.

A ideia é separar operações que mudam estado de operações que apenas leem estado:

- **Command:** altera algo no sistema.
- **Query:** consulta algo sem alterar estado observável.

Exemplo mental:

```text
CreateOrderCommand      → muda estado
GetOrderDetailsQuery    → apenas consulta
```

Em C#, isso costuma aparecer com tipos separados:

```csharp
public sealed record CreateOrderCommand(Guid CustomerId, IReadOnlyList<OrderItemInput> Items);

public sealed record GetOrderDetailsQuery(Guid OrderId);
```

CQRS não exige microservices, filas, event sourcing ou bancos separados. Essas coisas podem aparecer em arquiteturas maiores, mas não fazem parte da definição mínima.

Use CQRS quando a separação deixar o sistema mais claro:

- muitas regras no caminho de escrita;
- leituras com formatos diferentes do modelo de escrita;
- telas que precisam de projeções otimizadas;
- domínio com comandos importantes de negócio.

Evite CQRS quando ele só duplica classes sem reduzir complexidade.

**Fonte oficial:** Microsoft Learn — [.NET Microservices: DDD and CQRS patterns](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/).

---

### 24.6 Event-Driven Architecture

[⬆️ Voltar ao Sumário](#sumário)

Em uma arquitetura orientada a eventos, partes do sistema reagem a fatos que já aconteceram.

Exemplos:

```text
OrderCreated
PaymentApproved
InventoryReserved
CustomerRegistered
```

Em C#, um evento de domínio pode ser modelado como um tipo simples:

```csharp
public sealed record OrderCreated(Guid OrderId, Guid CustomerId, DateTimeOffset OccurredAt);
```

A diferença importante:

- **comando:** pede que algo aconteça;
- **evento:** informa que algo já aconteceu.

Eventos ajudam quando diferentes partes do sistema precisam reagir sem acoplamento direto. Por exemplo, depois de `OrderCreated`, uma aplicação pode:

- enviar e-mail;
- reservar estoque;
- publicar mensagem;
- atualizar projeção de leitura;
- registrar auditoria.

O cuidado é não transformar tudo em evento cedo demais. Eventos trazem desafios de ordem, duplicidade, idempotência, rastreabilidade e consistência eventual.

**Fonte oficial:** Microsoft Learn — [Domain events: Design and implementation](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/domain-events-design-implementation).

---

### 24.7 Microservices em .NET

[⬆️ Voltar ao Sumário](#sumário)

Microservices organizam um sistema como um conjunto de serviços pequenos e independentes, cada um com responsabilidade própria e deploy independente.

No ecossistema .NET, isso costuma envolver:

- ASP.NET Core Web APIs;
- containers;
- mensageria;
- bancos por serviço;
- observabilidade;
- versionamento de contratos;
- resiliência em chamadas remotas;
- autenticação e autorização distribuídas.

O ponto mais importante: microservices não são "classes pequenas pela rede". Eles são uma decisão operacional e organizacional.

Use microservices quando houver motivos reais como:

- domínios com ciclos de evolução independentes;
- times diferentes responsáveis por partes diferentes;
- necessidade de escalar partes específicas do sistema;
- fronteiras de negócio claras.

Evite microservices quando a aplicação ainda não tem fronteiras bem compreendidas. Nesse caso, um monólito modular em C# pode ser mais simples, mais barato e mais confiável.

**Fonte oficial:** Microsoft Learn — [.NET Microservices: Architecture for Containerized .NET Applications](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/).

---

### 24.8 Padrões enterprise clássicos

[⬆️ Voltar ao Sumário](#sumário)

Além dos padrões GoF, aplicações C#/.NET de negócio frequentemente usam padrões catalogados em *Patterns of Enterprise Application Architecture*, de Martin Fowler.

Alguns que aparecem muito em projetos C#:

| Padrão | Ideia central | Onde aparece em C#/.NET |
|---|---|---|
| Repository | Media o domínio e o mapeamento de dados | Interfaces como `ICustomerRepository` |
| Unit of Work | Coordena mudanças de uma transação | `DbContext` costuma cumprir esse papel no EF Core |
| Service Layer | Define operações de aplicação | Serviços de caso de uso em `Application` |
| Data Transfer Object | Carrega dados entre processos/camadas | Requests/responses de API |
| Domain Model | Modelo com dados e comportamento de domínio | Entidades, value objects e aggregates |
| Active Record | Objeto mistura dados, persistência e lógica | Mais comum em ORMs de estilo diferente; em .NET moderno, use com cuidado |

Esses padrões são úteis, mas não devem ser aplicados automaticamente.

Exemplo: se você usa Entity Framework Core, criar um `Repository` genérico por cima de `DbSet<T>` pode apenas repetir a API do EF sem agregar semântica. Já um repositório específico do domínio pode ser útil quando expressa operações reais:

```csharp
public interface ICustomerRepository
{
    Task<Customer?> FindByEmailAsync(Email email);
    Task<IReadOnlyList<Customer>> FindActiveCustomersAsync();
}
```

O critério é simples:

- se o padrão dá nome a uma responsabilidade real, ele ajuda;
- se só adiciona camada por hábito, ele atrapalha.

**Livro original e catálogo:** Martin Fowler — [Patterns of Enterprise Application Architecture](https://martinfowler.com/books/eaa.html) e [catalog of PoEAA patterns](https://martinfowler.com/eaaCatalog/).

---

## Anexo A — Plataformas de Prática Recomendadas

[⬆️ Voltar ao Sumário](#sumário)



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
- [Assemblies in .NET](https://learn.microsoft.com/en-us/dotnet/standard/assembly/)
- [Assembly contents](https://learn.microsoft.com/en-us/dotnet/standard/assembly/contents)
- [Interfaces - define behavior for multiple types](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/types/interfaces)
- [Reflection in .NET](https://learn.microsoft.com/en-us/dotnet/fundamentals/reflection/overview)
- [.NET dependency injection](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection/overview)
- [Dependency injection guidelines](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection/guidelines)
- [Properties - C#](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/properties)
- [Using Properties](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/using-properties)
- [The `get` keyword](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/get)
- [User-defined explicit and implicit conversion operators](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/user-defined-conversion-operators)
- [Language Integrated Query (LINQ)](https://learn.microsoft.com/en-us/dotnet/csharp/linq/)
- [Standard query operators overview](https://learn.microsoft.com/en-us/dotnet/csharp/linq/standard-query-operators/)
- [`IEnumerable<T>` Interface](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1?view=net-10.0)
- [`IQueryable<T>` Interface](https://learn.microsoft.com/en-us/dotnet/api/system.linq.iqueryable-1?view=net-10.0)
- [ArgumentNullException Class](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception?view=net-10.0)
- [ArgumentOutOfRangeException Class](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception?view=net-10.0)
- [NotImplementedException Class](https://learn.microsoft.com/en-us/dotnet/api/system.notimplementedexception?view=net-10.0)
- [Usando a classe StringBuilder no .NET](https://learn.microsoft.com/pt-br/dotnet/standard/base-types/stringbuilder)
- [`WeakReference<T>` Class](https://learn.microsoft.com/en-us/dotnet/api/system.weakreference-1?view=net-10.0)
- [`WeakReference<T>.TryGetTarget(T)` Method](https://learn.microsoft.com/en-us/dotnet/api/system.weakreference-1.trygettarget?view=net-10.0)
- [Fundamentals of garbage collection](https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/fundamentals)
- [Introduction to LINQ queries](https://learn.microsoft.com/en-us/dotnet/csharp/linq/get-started/introduction-to-linq-queries)
- [Explicit Interface Implementation](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/interfaces/explicit-interface-implementation)
- [.NET Architecture guides](https://learn.microsoft.com/en-us/dotnet/architecture/)
- [Common web application architectures](https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures)
- [.NET Microservices: Architecture for Containerized .NET Applications](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/)
- [Designing a DDD-oriented microservice](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/ddd-oriented-microservice)
- [Domain events: Design and implementation](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/domain-events-design-implementation)
- [Martin Fowler — Patterns of Enterprise Application Architecture](https://martinfowler.com/books/eaa.html)
- [Martin Fowler — Catalog of Patterns of Enterprise Application Architecture](https://martinfowler.com/eaaCatalog/)
- [Domain Language — DDD resources](https://www.domainlanguage.com/ddd/)

Sugestão de estudo: use este guia para construir o modelo mental e a documentação oficial para validar detalhes de comportamento, APIs e mudanças de versão.

---

## Glossário

[⬆️ Voltar ao Sumário](#sumário)

> Termos-chave usados ao longo do guia, em ordem alfabética. Cada item linka direto para a seção onde o assunto é explicado em detalhe.

- **`abstract`** — modificador que marca uma classe ou membro como incompleto, obrigando subclasses a fornecer a implementação. → [7.3 `abstract`](#73-abstract)
- **`async` / `await`** — palavras-chave que permitem escrever código assíncrono em estilo sequencial, sem bloquear a thread. → [16.1 O modelo assíncrono do C#](#161-o-modelo-assíncrono-do-c)
- **Assembly** — unidade compilada do .NET que agrupa IL, metadados, recursos e identidade, normalmente em `.dll` ou `.exe`. → [1.2 O que é um assembly?](#12-o-que-é-um-assembly)
- **Attribute** — metadado anexado a um tipo ou membro, lido em tempo de compilação ou execução (ex: `[Obsolete]`). → [19.1 Attributes embutidos](#191-attributes-embutidos)
- **Builder Pattern** — padrão de projeto para construir objetos complexos passo a passo, geralmente com sintaxe fluente. → [11.4 Padrão Builder](#114-padrão-builder)
- **Burst Compiler** — compilador da Unity que converte código C# em código nativo de alta performance via LLVM. → [23.7 Boas práticas de performance no Unity](#237-boas-práticas-de-performance-no-unity)
- **Clean Architecture** — estilo arquitetural que protege o núcleo de domínio/aplicação contra detalhes externos como banco, framework web e APIs externas. → [24.3 Clean Architecture, Onion e Ports and Adapters](#243-clean-architecture-onion-e-ports-and-adapters)
- **CLR (Common Language Runtime)** — máquina virtual do .NET que executa o código compilado (IL), análoga à JVM do Java. → [1.1 O que é C#?](#11-o-que-é-c)
- **Constraints (restrições de generics)** — regras que limitam quais tipos podem ser usados num tipo genérico. → [17.2 Constraints (restrições)](#172-constraints-restrições)
- **Conversão definida pelo usuário (`implicit` / `explicit`)** — mecanismo que permite a um tipo ensinar ao compilador como convertê-lo para outro tipo com ou sem cast explícito. → [7.9 Conversões definidas pelo usuário (`implicit` e `explicit`)](#79-conversões-definidas-pelo-usuário-implicit-e-explicit)
- **Construtor** — método especial, sem tipo de retorno, executado na criação de uma instância (`new`), responsável por inicializar seu estado. → [11.2 Construtores em Profundidade](#112-construtores-em-profundidade)
- **`const` / `readonly`** — modificadores para valores imutáveis; `const` é resolvido em tempo de compilação, `readonly` em tempo de execução. → [3.5 `const` e `readonly`](#35-const-e-readonly)
- **Coroutine** — mecanismo do Unity para executar código ao longo de vários frames, sem usar `async`/`await`. → [23.5 Coroutines](#235-coroutines-execução-assíncrona-sem-asyncawait)
- **CQRS** — separação entre comandos que alteram estado e consultas que apenas leem estado. → [24.5 CQRS e separação entre comandos e consultas](#245-cqrs-e-separação-entre-comandos-e-consultas)
- **Delegate** — tipo que representa uma referência tipada a um método, base para eventos e lambdas. → [13.1 Delegates](#131-delegates-ponteiros-de-método-tipados)
- **Dependency Injection (DI)** — padrão em que uma classe recebe suas dependências de fora, em vez de instanciá-las diretamente. → [22.2 Dependency Injection (DI)](#222-dependency-injection-di)
- **Domain-Driven Design (DDD)** — abordagem de design que organiza o software em torno de um modelo de domínio e de uma linguagem compartilhada. → [24.4 Domain-Driven Design (DDD)](#244-domain-driven-design-ddd)
- **Event-Driven Architecture** — arquitetura em que partes do sistema reagem a eventos que representam fatos já ocorridos. → [24.6 Event-Driven Architecture](#246-event-driven-architecture)
- **Enum / Flags enum** — tipo que representa um conjunto fixo de valores nomeados; com `[Flags]`, permite combinação via bitmask. → [10.1 Enums básicos](#101-enums-básicos)
- **Event** — mecanismo baseado em delegates para notificar múltiplos assinantes sobre uma ocorrência. → [13.4 Eventos (Events)](#134-eventos-events)
- **Extension Method** — método que "adiciona" comportamento a um tipo existente sem modificá-lo ou herdar dele. → [9.2 Métodos de extensão](#92-métodos-de-extensão-extension-methods)
- **Generics** — recurso que permite escrever tipos e métodos parametrizados por tipo, mantendo segurança de tipos. → [17.1 Tipos parametrizados](#171-tipos-parametrizados)
- **Garbage Collector (GC)** — componente do runtime .NET que recupera memória de objetos não mais alcançáveis por referências fortes. → [20.2 `WeakReference<T>` e referências fracas no GC](#202-weakreferencet-e-referências-fracas-no-gc)
- **IL / CIL (Intermediate Language)** — formato intermediário para o qual o C# é compilado antes de ser executado pelo CLR. → [1.1 O que é C#?](#11-o-que-é-c)
- **Interface** — contrato que define quais membros uma classe deve implementar, sem fornecer implementação própria. → [12.2 Interfaces](#122-interfaces)
- **`IEnumerable<T>`** — contrato fundamental de sequência enumerável; diz que um tipo pode fornecer elementos em ordem de iteração, sem prometer índice ou materialização. → [14.3 `IEnumerable<T>`](#143-ienumerablet-e-o-contrato-fundamental-das-sequências)
- **`IQueryable<T>`** — contrato de consulta traduzível por um provider, geralmente usado para fontes remotas como bancos de dados. → [14.4 `IQueryable<T>`](#144-iqueryablet-e-queries-traduzíveis-para-outra-fonte)
- **Jobs System** — sistema da Unity para distribuir cálculos entre múltiplas threads de forma segura. → [23.7 Boas práticas de performance no Unity](#237-boas-práticas-de-performance-no-unity)
- **Lambda (expressão lambda)** — função anônima e compacta, geralmente usada com delegates e LINQ. → [13.3 Expressões Lambda](#133-expressões-lambda)
- **LINQ (Language Integrated Query)** — conjunto de recursos da linguagem e da biblioteca para consultar e transformar dados de forma tipada e declarativa. → [14.1 O que é LINQ?](#141-o-que-é-linq)
- **MonoBehaviour** — classe base da qual todo script que vive numa GameObject do Unity deriva. → [23.2 MonoBehaviour](#232-monobehaviour-a-classe-base-dos-scripts-unity)
- **Microservices** — estilo arquitetural que organiza uma aplicação como serviços independentes com responsabilidades e deploys próprios. → [24.7 Microservices em .NET](#247-microservices-em-net)
- **Namespace** — contêiner lógico que agrupa tipos relacionados, evitando colisão de nomes. → [2.1 Namespaces](#21-namespaces)
- **Nullable Type** — tipo de valor que pode aceitar `null` além do seu valor normal (ex: `int?`). → [3.3 Nullable Types](#33-nullable-types-tipos-que-aceitam-null)
- **Pattern Matching** — sintaxe para testar e desestruturar valores com base em forma/tipo (`is`, `switch` expressions). → [7.6 `is`, `as` e Pattern Matching](#76-is-as-e-pattern-matching)
- **Property accessor `get`** — método acessor de leitura de uma propriedade; pode devolver um campo, calcular um valor sob demanda ou montar uma visão derivada do estado atual. → [6.1 O que são Properties?](#61-o-que-são-properties)
- **Properties** — membros que expõem um valor através de `get`/`set`, encapsulando acesso a um campo. → [6.1 O que são Properties?](#61-o-que-são-properties)
- **Record** — tipo de referência (ou valor) focado em imutabilidade e igualdade por valor, introduzido no C# 9. → [11.3 Records (C# 9+)](#113-records-c-9)
- **Reflection** — capacidade de inspecionar tipos, métodos e atributos em tempo de execução. → [22.1 Reflection](#221-reflection)
- **`ref` / `out` / `in`** — modificadores de parâmetro para passar argumentos por referência em vez de por valor. → [7.8 `ref`, `out` e `in`](#78-ref-out-e-in)
- **Roslyn** — compilador open-source moderno do C#, também usado para análise estática e Source Generators. → [1.1 O que é C#?](#11-o-que-é-c)
- **`sealed`** — modificador que impede uma classe de ser herdada (ou um método de ser sobrescrito novamente). → [7.2 `sealed`](#72-sealed)
- **ScriptableObject** — tipo de asset da Unity para armazenar dados independentes de uma instância de GameObject. → [23.4 ScriptableObject](#234-scriptableobject-dados-desacoplados-do-gameobject)
- **Singleton** — padrão de projeto que garante uma única instância acessível globalmente de uma classe. → [23.8 Padrões de design comuns em jogos com C#](#238-padrões-de-design-comuns-em-jogos-com-c)
- **Source Generator** — componente que gera código C# adicional em tempo de compilação. → [22.3 Source Generators](#223-source-generators-c-9)
- **Span\<T\> / Memory\<T\>** — estruturas para trabalhar com "fatias" de memória contígua sem alocação extra. → [20.3 Span\<T\> e Memory\<T\> — zero-allocation slicing](#203-spant-e-memoryt-zero-allocation-slicing)
- **`static`** — modificador que faz um membro pertencer ao tipo, não a uma instância específica. → [7.1 `static`](#71-static)
- **State Machine** — padrão que organiza o comportamento de um objeto em estados distintos com transições explícitas. → [23.8 Padrões de design comuns em jogos com C#](#238-padrões-de-design-comuns-em-jogos-com-c)
- **StringBuilder** — classe mutável para concatenar strings repetidamente sem o custo de criar novas instâncias a cada operação. → [4.2 Imutabilidade e StringBuilder](#42-imutabilidade-e-stringbuilder)
- **Task / ValueTask** — representações de uma operação assíncrona em andamento; `ValueTask` evita alocação em cenários de alta performance. → [16.3 Task vs ValueTask](#163-task-vs-valuetask)
- **Unsafe code** — blocos de código que permitem manipulação direta de ponteiros, fora da supervisão normal do CLR. → [22.4 Unsafe code e ponteiros](#224-unsafe-code-e-ponteiros)
- **`var`** — palavra-chave que permite ao compilador inferir o tipo de uma variável a partir do valor atribuído. → [3.4 `var` — inferência de tipo](#34-var-inferência-de-tipo)
- **`virtual` / `override`** — modificadores que permitem que um método seja redefinido por uma subclasse. → [7.4 `virtual` e `override`](#74-virtual-e-override)
- **Handle** — objeto intermediário usado para alcançar outro objeto ou recurso por indireção; no projeto, `Ref<ITheme>` funciona como um handle mutável para o tema atual. → [20.2 `WeakReference<T>` e referências fracas no GC](#202-weakreferencet-e-referências-fracas-no-gc)
- **WeakReference\<T\>** — referência fraca para um objeto que permite observá-lo sem impedir que o GC o colete quando não restarem referências fortes. → [20.2 `WeakReference<T>` e referências fracas no GC](#202-weakreferencet-e-referências-fracas-no-gc)

**Como interpretar o exemplo:** A tabela lembra que saber C# não é exatamente o mesmo que saber C# dentro de uma engine específica. Em Unity, runtime, GC, AOT e APIs de jogo fazem alguns hábitos do C# padrão continuarem válidos e outros precisarem de adaptação consciente.
