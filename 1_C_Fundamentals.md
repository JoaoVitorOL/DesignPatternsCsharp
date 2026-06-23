# 🔷 Guia Técnico: C# do Zero ao Avançado

> **Nível:** Zero ao Avançado
> **Linguagem:** C# (C-Sharp)
> **Fonte de referência:** [docs.microsoft.com/dotnet/csharp](https://docs.microsoft.com/en-us/dotnet/csharp/)
> **Versão de referência:** C# 12 / .NET 8 (LTS)

---

## Prefácio

Este guia nasceu da necessidade de reunir, num lugar só, o caminho do C# do zero até tópicos avançados — sem depender de dezenas de abas abertas ou de explicações picadas em pedaços. A ideia não é substituir a documentação oficial da Microsoft, e sim servir de mapa: do que é o CLR e por que o C# compila para IL, até padrões de projeto aplicados dentro do Unity e do Godot.

A estrutura segue a lógica de um livro técnico: capítulos numerados, sumário clicável e um glossário ao final para consulta rápida de termos. Dá pra ler do início ao fim ou pular direto pro capítulo que precisar — cada seção foi escrita para ser entendida por si só, sem depender demais do que veio antes.

Bons estudos.

---

## Sumário

- [Parte 1 — Introdução e Contextualização](#parte-1-introdução-e-contextualização)
  - [1.1 O que é C#?](#11-o-que-é-c)
  - [1.2 Por que aprender C# em 2024/2025?](#12-por-que-aprender-c-em-20242025)
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
- [Parte 15 — Coleções](#parte-15-coleções)
  - [15.1 Tipos de coleções principais](#151-tipos-de-coleções-principais)
  - [15.2 List\<T\>](#152-listt)
  - [15.3 Dictionary\<TKey, TValue\>](#153-dictionarytkey-tvalue)
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
- [Glossário](#glossário)

---

## Parte 1 — Introdução e Contextualização

[⬆️ Voltar ao Sumário](#sumário)


---

### 1.1 O que é C#?

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

---

### 1.2 Por que aprender C# em 2024/2025?

C# é uma linguagem com escopo extremamente amplo:

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

C# é consistentemente ranqueada entre as 5 linguagens mais usadas no mundo (índice TIOBE, Stack Overflow Survey) e possui uma das comunidades mais ativas no GitHub.

---

### 1.3 Estrutura de um programa C#

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

---

## Parte 2 — Namespaces e Using

[⬆️ Voltar ao Sumário](#sumário)


---

### 2.1 Namespaces

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

---

### 2.2 Using Directives

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

---

## Parte 3 — Variáveis e Tipos

[⬆️ Voltar ao Sumário](#sumário)


---

### 3.1 O que é uma variável?

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

---

### 3.2 Tipos de valor vs tipos de referência

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

---

### 3.3 Nullable Types — tipos que aceitam null

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

---

### 3.4 `var` — inferência de tipo

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

---

### 3.5 `const` e `readonly`

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

---

## Parte 4 — String e suas Peculiaridades

[⬆️ Voltar ao Sumário](#sumário)


---

### 4.1 String é um tipo de referência imutável

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

---

### 4.2 Imutabilidade e StringBuilder

```csharp
string nome = "Ana";
nome = nome + " Silva"; // cria novo objeto — original "Ana" é descartado

// Concatenação em loop — ineficiente
string resultado = "";
for (int i = 0; i < 1000; i++)
    resultado += i; // cria um novo objeto a cada iteração

// StringBuilder — eficiente para concatenações múltiplas
var sb = new System.Text.StringBuilder();
for (int i = 0; i < 1000; i++)
{
    sb.Append(i);
    if (i < 999) sb.Append(", ");
}
string eficiente = sb.ToString();

// API do StringBuilder
var builder = new System.Text.StringBuilder("Olá");
builder.Append(" Mundo");         // "Olá Mundo"
builder.Insert(3, ",");           // "Olá, Mundo"
builder.Replace("Mundo", "C#");   // "Olá, C#"
builder.Remove(4, 1);             // "Olá C#"
Console.WriteLine(builder.Length); // tamanho atual
```

---

### 4.3 String Interpolation e verbatim strings

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

---

### 4.4 Métodos importantes de string

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

---

## Parte 5 — Modificadores de Acesso

[⬆️ Voltar ao Sumário](#sumário)


---

### 5.1 Os modificadores de acesso do C#

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

---

### 5.2 Boas práticas com modificadores

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


---

### 6.1 O que são Properties?

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

---

### 6.2 Expression-bodied members

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

---

## Parte 7 — Palavras-chave Especiais do C#

[⬆️ Voltar ao Sumário](#sumário)


---

### 7.1 `static`

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

---

### 7.2 `sealed`

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

---

### 7.3 `abstract`

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

---

### 7.4 `virtual` e `override`

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

---

### 7.5 `this` e `base`

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

---

### 7.6 `is`, `as` e Pattern Matching

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

---

### 7.7 `using` para gerenciamento de recursos

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

---

### 7.8 `ref`, `out` e `in`

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

---

## Parte 8 — Controle de Fluxo

[⬆️ Voltar ao Sumário](#sumário)


---

### 8.1 `if / else if / else`

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

---

### 8.2 `switch` e switch expressions

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

---

### 8.3 Loops

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

---

## Parte 9 — Métodos

[⬆️ Voltar ao Sumário](#sumário)


---

### 9.1 Declaração de métodos

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

---

### 9.2 Métodos de extensão (Extension Methods)

C# permite adicionar métodos a tipos existentes sem herança ou modificação do código original.

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

---

### 9.3 Sobrecarga de métodos

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


---

### 10.1 Enums básicos

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

---

### 10.2 Flags enum — bitmask

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


---

### 11.1 Estrutura completa de uma classe

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

---

### 11.2 Construtores em Profundidade

#### 11.2.1 Definição

| Termo | Definição |
|---|---|
| Construtor | Método especial executado automaticamente no momento da criação de uma instância (`new`), responsável por inicializar o estado do objeto. |
| Assinatura | Mesmo nome da classe. Não possui tipo de retorno (nem mesmo `void`). |
| Quantidade | Uma classe pode ter múltiplos construtores, desde que cada um tenha uma lista de parâmetros distinta (sobrecarga). |

#### 11.2.2 Construtor padrão (implícito)

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

#### 11.2.3 Construtor parametrizado

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

#### 11.2.4 Sobrecarga de construtores

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

#### 11.2.5 Encadeamento de construtores com `this(...)`

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

#### 11.2.6 Chamada ao construtor da classe base com `base(...)`

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

#### 11.2.7 Ordem de execução em uma hierarquia de herança

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

#### 11.2.8 Construtor estático

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

#### 11.2.9 Construtor privado

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

#### 11.2.10 Construtores primários (Primary Constructors — C# 12)

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

#### 11.2.11 Tabela-resumo dos tipos de construtor

| Tipo | Modificador | Quando executa | Uso típico |
|---|---|---|---|
| Padrão (implícito) | `public` (gerado) | Na ausência de qualquer construtor declarado | Classes simples sem necessidade de inicialização |
| Parametrizado | `public`/outro | A cada `new` com os argumentos correspondentes | Inicialização obrigatória de estado |
| Estático | (nenhum) | Uma única vez, antes do primeiro uso do tipo | Inicialização de campos `static` |
| Privado | `private` | Apenas internamente à própria classe | Singleton, métodos fábrica |
| Primário (C# 12) | conforme classe | Igual ao parametrizado | Redução de verbosidade em classes simples |

---

### 11.3 Records (C# 9+)

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

---

### 11.4 Padrão Builder

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

> Em C# moderno, o padrão builder frequentemente é substituído por **object initializers** e **init-only properties**, que oferecem sintaxe mais limpa sem classe auxiliar.

---

## Parte 12 — Herança e Polimorfismo

[⬆️ Voltar ao Sumário](#sumário)


---

### 12.1 Herança em C#

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

---

### 12.2 Interfaces

```csharp
public interface IPagavel
{
    double CalcularTotal();
    void ProcessarPagamento(string metodo);

    // Método default (C# 8+) — implementação padrão
    string GerarRecibo() => $"Recibo emitido em: {DateTime.Now:dd/MM/yyyy}";

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
    public double Valor  { get; }
    private bool  _cancelado;

    public Pedido(double valor) => Valor = valor;

    public double CalcularTotal()                     => Valor * 1.1;
    public void ProcessarPagamento(string metodo)     => Console.WriteLine($"Pedido {Id} pago via {metodo}");
    public bool Cancelar(string motivo)
    {
        if (_cancelado) return false;
        _cancelado = true;
        return true;
    }
}
```

**Classe abstrata vs Interface em C#:**

| Característica | Classe abstrata | Interface |
|---|---|---|
| Herança múltipla | Não (apenas uma) | Sim (várias) |
| Campos de instância | Sim | Não |
| Construtores | Sim | Não |
| Métodos com implementação | Sim | Sim (`default`, C# 8+) |
| Modificadores de acesso | Qualquer | `public` por padrão |
| Quando usar | Relação "é um", compartilhar estado e comportamento | Contrato de comportamento |

---

## Parte 13 — Delegates, Events e Lambdas

[⬆️ Voltar ao Sumário](#sumário)


---

### 13.1 Delegates — ponteiros de método tipados

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

---

### 13.2 Func, Action e Predicate

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

---

### 13.3 Expressões Lambda

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

---

### 13.4 Eventos (Events)

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

---

## Parte 14 — LINQ (Language Integrated Query)

[⬆️ Voltar ao Sumário](#sumário)


---

### 14.1 O que é LINQ?

LINQ é um conjunto de funcionalidades que permite escrever queries diretamente em C#, de forma tipada e integrada ao compilador. Funciona sobre qualquer `IEnumerable<T>` ou `IQueryable<T>`.

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

---

### 14.2 Operadores LINQ principais

```csharp
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
bool algum = nomes.Any(n => n.StartsWith("A"));    // true
bool todos = nomes.All(n => n.Length > 2);          // true
bool nenhum = nomes.None(n => n.Length > 10);       // (use !Any())

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

---

## Parte 15 — Coleções

[⬆️ Voltar ao Sumário](#sumário)


---

### 15.1 Tipos de coleções principais

```
IEnumerable<T> (somente leitura, lazy)
└── ICollection<T>
    ├── IList<T>
    │   ├── List<T>          — lista dinâmica (array redimensionável)
    │   └── Array            — tamanho fixo
    ├── ISet<T>
    │   ├── HashSet<T>       — sem ordem, sem duplicatas, O(1)
    │   ├── SortedSet<T>     — ordenado, sem duplicatas, O(log n)
    │   └── LinkedHashSet    — (não existe nativo; use SortedSet ou LinkedList)
    └── Queue<T>             — fila FIFO
    └── Stack<T>             — pilha LIFO

IDictionary<TKey, TValue>
├── Dictionary<TKey, TValue>  — hash map, O(1)
├── SortedDictionary<K, V>    — ordenado por chave, O(log n)
└── ConcurrentDictionary<K,V> — thread-safe
```

---

### 15.2 List\<T\>

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

// Listas imutáveis
var imutavel = new List<string> { "A", "B" }.AsReadOnly();
// imutavel.Add("C"); // NotSupportedException
```

---

### 15.3 Dictionary\<TKey, TValue\>

```csharp
var estoque = new Dictionary<string, int>
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

---

## Parte 16 — Async/Await e Programação Assíncrona

[⬆️ Voltar ao Sumário](#sumário)


---

### 16.1 O modelo assíncrono do C#

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

---

### 16.2 Padrões de uso

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

---

### 16.3 Task vs ValueTask

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

---

## Parte 17 — Generics

[⬆️ Voltar ao Sumário](#sumário)


---

### 17.1 Tipos parametrizados

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

---

### 17.2 Constraints (restrições)

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

---

### 17.3 Covariância e contravariância

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


---

### 18.1 `try / catch / finally`

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

---

### 18.2 Exceções customizadas

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

---

### 18.3 Hierarquia de exceções

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


---

### 19.1 Attributes embutidos

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

---

### 19.2 Criando Attributes customizados

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

---

## Parte 20 — Tipos Especiais Modernos do C#

[⬆️ Voltar ao Sumário](#sumário)


---

### 20.1 Tuple e ValueTuple

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

---

### 20.2 Span\<T\> e Memory\<T\> — zero-allocation slicing

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

---

### 20.3 Sealed classes com Pattern Matching (como Discriminated Union)

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

---

## Parte 21 — Threads e Concorrência

[⬆️ Voltar ao Sumário](#sumário)


---

### 21.1 Thread básico e ThreadPool

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

---

### 21.2 Task Parallel Library (TPL)

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

---

### 21.3 Sincronização

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


---

### 22.1 Reflection

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

---

### 22.2 Source Generators (C# 9+)

Source Generators são compiladores que executam durante a build e geram código C# adicional automaticamente. Usados extensivamente em frameworks modernos (System.Text.Json, EF Core, Dapper, etc.) para eliminar Reflection em runtime.

```csharp
// Exemplo: JsonSerializerContext gerado automaticamente
[JsonSerializable(typeof(Usuario))]
[JsonSerializable(typeof(List<Usuario>))]
public partial class MeuJsonContext : JsonSerializerContext { }

// O compilador gera o código de serialização — sem Reflection em runtime
string json = JsonSerializer.Serialize(usuario, MeuJsonContext.Default.Usuario);
```

---

### 22.3 Unsafe code e ponteiros

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
| **Delegate** | Tipo que representa referência a método com assinatura específica |
| **Event** | Delegate encapsulado para padrão publisher/subscriber |
| **Lambda** | Implementação compacta inline de delegate / interface funcional |
| **LINQ** | API de queries integrada à linguagem sobre `IEnumerable<T>` |
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

**Unity** é o motor de jogos mais utilizado no mundo para jogos mobile, indie e AA. C# é a **única linguagem de scripting oficial** do Unity. Compreender C# bem é pré-requisito direto para trabalhar com Unity.

Unity utiliza uma versão do runtime .NET chamada **Mono** (em builds legadas) e **IL2CPP** para builds de plataformas como iOS e consoles. IL2CPP converte o IL do .NET para C++ nativo antes da compilação, eliminando a necessidade da JIT no dispositivo alvo.

```
Código C# → IL/CIL → IL2CPP → C++ → binário nativo da plataforma
```

---

### 23.2 MonoBehaviour — a classe base dos scripts Unity

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

---

### 23.3 Ciclo de vida do MonoBehaviour

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

---

### 23.4 ScriptableObject — dados desacoplados do GameObject

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

---

### 23.5 Coroutines — execução assíncrona sem async/await

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

---

### 23.6 Unity Events e C# Events

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

---

### 23.7 Boas práticas de performance no Unity

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

---

### 23.8 Padrões de design comuns em jogos com C#

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

---

### 23.9 C# no Godot 4

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

---

### 23.10 Diferenças entre C# padrão e C# no Unity

| Aspecto | C# Padrão (.NET) | C# no Unity |
|---|---|---|
| Runtime | .NET 6/7/8 | Mono ou IL2CPP |
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
| **Beecrowd** | Sim | Altamente aderente | Lógica de programação pura, matemática e estruturas de dados |
| **Exercism** | Sim | Baixa aderência (foco em projetos) | Idiomatismo da linguagem, legibilidade e TDD local |
| **CodeSignal Learn** | Sim | Média aderência (trilhas guiadas) | Código limpo, princípios SOLID e padrões de projeto aplicados |
| **Coderbyte** | Sim | Altamente aderente | Desafios algorítmicos clássicos e projetos multi-arquivos de mercado |
| **HackerRank** | Sim | Altamente aderente | Resolução de problemas, estruturas de dados e proficiência em sintaxe |
| **DesignPatterns101** | Sim | Média aderência (exercícios práticos) | Padrões de projeto GoF aplicados exclusivamente em C# |
| **Project Euler** | Sim | Altamente aderente (resposta única) | Raciocínio matemático-computacional profundo e otimização lógica |

---

## Glossário

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
- **Jobs System** — sistema da Unity para distribuir cálculos entre múltiplas threads de forma segura. → [23.7 Boas práticas de performance no Unity](#237-boas-práticas-de-performance-no-unity)
- **Lambda (expressão lambda)** — função anônima e compacta, geralmente usada com delegates e LINQ. → [13.3 Expressões Lambda](#133-expressões-lambda)
- **LINQ (Language Integrated Query)** — conjunto de operadores para consultar coleções de forma declarativa, similar a SQL. → [14.1 O que é LINQ?](#141-o-que-é-linq)
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
