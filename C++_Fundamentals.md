# ⚙️ Guia Técnico: C++ do Zero ao Avançado

> **Nível:** Zero ao Avançado  
> **Linguagem:** C++  
> **Fontes principais:** [ISO/IEC 14882:2024](https://www.iso.org/standard/83626.html), [Working Draft final do C++23 — WG21 N4950](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf) e [Working Draft final do C++26 — WG21 N5050](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2026/n5050.pdf)  
> **Versão de referência:** C++23, com notas explicitamente marcadas sobre o draft do C++26  
> **Atualizado em:** 21/07/2026

---

## Prefácio

C++ costuma ser ensinado como “C com classes” ou como uma lista de recursos isolados. As duas abordagens escondem o que realmente torna a linguagem difícil e poderosa: valores e objetos têm lifetime; destruição é determinística; expressões possuem categorias de valor; o programa é formado por unidades de tradução ligadas posteriormente; templates geram código; algumas violações são diagnosticadas e outras produzem comportamento indefinido.

Este guia constrói um modelo mental em camadas. A linguagem define sintaxe e semântica; a biblioteca padrão fornece containers, algoritmos, strings, concorrência e utilitários; a implementação reúne compilador, biblioteca, linker e runtime; o sistema operacional e bibliotecas externas acrescentam contratos próprios. C++ não exige uma máquina virtual, um garbage collector, uma ABI universal, um sistema de build ou um gerenciador de pacotes único.

O texto segue o mesmo padrão didático dos guias `C#_Fundamentals.md` e `Java_Fundamentals.md`:

- definição clara antes da sintaxe;
- exemplo pequeno e contextualizado;
- **leitura guiada** dos trechos que podem confundir iniciantes;
- tabelas para comparar contratos, custos e alternativas;
- alertas sobre lifetime, ownership, invalidação e comportamento indefinido;
- referências oficiais próximas das afirmações que sustentam.

C++23 é a base estável porque corresponde ao padrão ISO vigente, publicado como ISO/IEC 14882:2024. O WG21 concluiu o working draft final do C++26 e já mantém o draft do ciclo C++29; até a publicação internacional do C++26, seu texto continua tratado aqui como antecipação. O suporte dos compiladores varia, portanto esses recursos nunca aparecem como se já fossem igualmente portáveis.

---

## Como usar este guia

Há três trilhas:

1. **Trilha iniciante:** Partes 1–16, executando os exemplos e explicando ownership, lifetime e resultado.
2. **Trilha profissional:** Partes 17–27, com templates, concorrência, toolchains, segurança, testes e produção.
3. **Trilha de consulta:** Partes 28–29, anexos e glossário para descobrir recursos prontos e fontes primárias.

Ao estudar qualquer construção, responda:

1. O recurso pertence à linguagem, à biblioteca padrão, ao compilador, ao sistema operacional ou a uma dependência?
2. Quem possui o recurso e quando seu lifetime termina?
3. Que referências, ponteiros, iteradores ou views podem ficar pendentes?
4. Há comportamento indefinido, não especificado, definido pela implementação ou apenas custo oculto?
5. O código depende de versão, extensão, ABI ou plataforma específica?

Os exemplos completos usam headers explícitos e evitam `using namespace std;`. Blocos que representam apenas uma função, classe ou fragmento de build são identificados pelo texto. Para experimentar, selecione deliberadamente o dialeto e ative avisos do compilador.

---

<a id="sumário"></a>

## Sumário Geral

### Como o conteúdo está organizado

As 29 partes começam no processo de tradução, avançam pelo modelo de objetos e chegam a engenharia de produção e ecossistema. C++ exige que sintaxe, lifetime e toolchain sejam aprendidos juntos; por isso o guia não empurra memória e build para um apêndice.

| Bloco | Partes | Assuntos centrais | Resultado esperado | Comece por |
|---|---:|---|---|---|
| 1. Base da linguagem | 1–4 | padrões, tradução, unidades, tipos, inicialização e texto | compilar programas pequenos e reconhecer os contratos fundamentais | [Parte 1](#parte-1--introdução-e-contextualização) |
| 2. Funções, objetos e lifetime | 5–12 | funções, classes, RAII, fluxo, movimento, variantes e polimorfismo | modelar recursos sem vazamentos nem referências pendentes | [Parte 5](#parte-5--funções-parâmetros-e-sobrecarga) |
| 3. Programação genérica e biblioteca | 13–18 | lambdas, algoritmos, ranges, containers, erros, templates e compile time | compor código reutilizável com contratos verificáveis | [Parte 13](#parte-13--callables-lambdas-e-callbacks) |
| 4. Memória, baixo nível e concorrência | 19–22 | ownership, allocators, representação, UB, threads, atomics, coroutines e interoperabilidade | raciocinar sobre custo, segurança e fronteiras nativas | [Parte 19](#parte-19--memória-dinâmica-ownership-e-allocators) |
| 5. Aplicações e arquitetura | 23–24 | sistemas, jogos, embedded, HPC, backend e padrões arquiteturais | posicionar C++ em domínios reais | [Parte 23](#parte-23--c-nos-principais-contextos-de-aplicação) |
| 6. Engenharia profissional | 25–27 | compiladores, CMake, dependências, testes, I/O, sanitizers, segurança e entrega | construir, verificar e publicar software reproduzível | [Parte 25](#parte-25--toolchains-build-dependências-e-qualidade) |
| 7. Catálogo e ecossistema | 28–29 | keywords, headers, tipos, algoritmos, frameworks e bibliotecas | descobrir o que já existe antes de reimplementar | [Parte 28](#parte-28--catálogo-da-linguagem-e-da-biblioteca-padrão) |
| 8. Revisão | Anexos | fontes oficiais, trilhas e glossário | aprofundar conceitos na documentação original | [Anexo A](#anexo-a--trilhas-oficiais-de-estudo-e-prática) |

### Atalhos por pergunta prática

| Se você quer saber... | Consulte primeiro |
|---|---|
| como fonte, preprocessor, compilador, object file, linker e executável se relacionam | [Partes 1](#parte-1--introdução-e-contextualização), [2](#parte-2--unidades-de-tradução-preprocessor-namespaces-e-módulos) e [25](#parte-25--toolchains-build-dependências-e-qualidade) |
| como escolher um tipo, inicializar e converter sem perda acidental | [Parte 3](#parte-3--tipos-valores-inicialização-e-referências) |
| como trabalhar com texto, Unicode, `string` e `string_view` | [Parte 4](#parte-4--texto-strings-e-unicode) |
| como garantir lifetime e ownership corretos | [Partes 7](#parte-7--lifetime-raii-e-recursos), [9](#parte-9--categorias-de-valor-cópia-e-movimento) e [19](#parte-19--memória-dinâmica-ownership-e-allocators) |
| como evitar undefined behavior e dangling references | [Partes 7](#parte-7--lifetime-raii-e-recursos) e [20](#parte-20--modelo-de-objetos-baixo-nível-e-comportamento-indefinido) |
| como escolher container, algoritmo ou view | [Partes 14](#parte-14--iteradores-algoritmos-e-ranges) e [15](#parte-15--containers-da-biblioteca-padrão) |
| como escrever templates compreensíveis com concepts | [Partes 17](#parte-17--templates-concepts-e-programação-genérica) e [18](#parte-18--programação-em-compile-time-e-metaprogramação) |
| como tratar falhas sem perder recursos | [Parte 16](#parte-16--erros-exceções-e-garantias) |
| como escrever código concorrente | [Parte 21](#parte-21--threads-concorrência-e-modelo-de-memória) |
| como configurar build, warnings, testes e sanitizers | [Partes 25](#parte-25--toolchains-build-dependências-e-qualidade) e [27](#parte-27--engenharia-para-produção) |
| qual tipo, função, header ou biblioteca já existe | [Partes 28](#parte-28--catálogo-da-linguagem-e-da-biblioteca-padrão) e [29](#parte-29--ecossistema-externo-frameworks-bibliotecas-e-ferramentas) |

### Índice detalhado

**Bloco 1 — Base da linguagem (Partes 1–4)**

- **[Parte 1 — Introdução e Contextualização](#parte-1--introdução-e-contextualização)**
  - [1.0 Nomenclatura, identificadores e estilo](#10-nomenclatura-identificadores-e-estilo)
  - [1.1 O que é C++?](#11-o-que-é-c)
  - [1.2 Compilador, linker, runtime e implementação](#12-compilador-linker-runtime-e-implementação)
  - [1.3 C++ em 2026: C++23 e o draft do C++26](#13-c-em-2026-c23-e-o-draft-do-c26)
  - [1.4 Estrutura e ponto de entrada](#14-estrutura-e-ponto-de-entrada)
  - [1.5 Tokens, comentários, statements e blocos](#15-tokens-comentários-statements-e-blocos)
  - [1.6 Mapa do iniciante e características distintivas](#16-mapa-do-iniciante-e-características-distintivas)
- **[Parte 2 — Unidades de Tradução, Preprocessor, Namespaces e Módulos](#parte-2--unidades-de-tradução-preprocessor-namespaces-e-módulos)**
  - [2.1 Declarações, definições, unidades de tradução e ODR](#21-declarações-definições-unidades-de-tradução-e-odr)
  - [2.2 `include`, include guards e macros](#22-include-include-guards-e-macros)
  - [2.3 Namespaces, `using` e ADL](#23-namespaces-using-e-adl)
  - [2.4 Headers, fontes e forward declarations](#24-headers-fontes-e-forward-declarations)
  - [2.5 Módulos e header units](#25-módulos-e-header-units)
- **[Parte 3 — Tipos, Valores, Inicialização e Referências](#parte-3--tipos-valores-inicialização-e-referências)**
  - [3.1 Tipos fundamentais, tamanho e limites](#31-tipos-fundamentais-tamanho-e-limites)
  - [3.2 Objetos, valores, escopo, storage duration e lifetime](#32-objetos-valores-escopo-storage-duration-e-lifetime)
  - [3.3 Formas de inicialização e narrowing](#33-formas-de-inicialização-e-narrowing)
  - [3.4 `const`, `volatile` e `mutable`](#34-const-volatile-e-mutable)
  - [3.5 `auto`, `decltype`, aliases e structured bindings](#35-auto-decltype-aliases-e-structured-bindings)
  - [3.6 Ponteiros, referências e `nullptr`](#36-ponteiros-referências-e-nullptr)
  - [3.7 Arrays, `std::array` e `std::span`](#37-arrays-stdarray-e-stdspan)
  - [3.8 Conversões e casts](#38-conversões-e-casts)
- **[Parte 4 — Texto, Strings e Unicode](#parte-4--texto-strings-e-unicode)**
  - [4.1 Caracteres, code units e encodings](#41-caracteres-code-units-e-encodings)
  - [4.2 `std::string` e variantes](#42-stdstring-e-variantes)
  - [4.3 `std::string_view` e lifetime](#43-stdstring_view-e-lifetime)
  - [4.4 Literais, strings raw e concatenação](#44-literais-strings-raw-e-concatenação)
  - [4.5 Formatação, parsing e streams de texto](#45-formatação-parsing-e-streams-de-texto)

**Bloco 2 — Funções, objetos e lifetime (Partes 5–12)**

- **[Parte 5 — Funções, Parâmetros e Sobrecarga](#parte-5--funções-parâmetros-e-sobrecarga)**
  - [5.1 Declaração, definição, assinatura e retorno](#51-declaração-definição-assinatura-e-retorno)
  - [5.2 Passagem por valor, referência e ponteiro](#52-passagem-por-valor-referência-e-ponteiro)
  - [5.3 Sobrecarga, argumentos padrão e resolução](#53-sobrecarga-argumentos-padrão-e-resolução)
  - [5.4 `inline`, `constexpr`, `noexcept` e atributos](#54-inline-constexpr-noexcept-e-atributos)
  - [5.5 Function pointers e parâmetros de saída](#55-function-pointers-e-parâmetros-de-saída)
- **[Parte 6 — Classes, Construção e Membros Especiais](#parte-6--classes-construção-e-membros-especiais)**
  - [6.1 `class`, `struct`, membros e acesso](#61-class-struct-membros-e-acesso)
  - [6.2 Construtores e listas de inicialização](#62-construtores-e-listas-de-inicialização)
  - [6.3 Destrutores e RAII](#63-destrutores-e-raii)
  - [6.4 Membros especiais: Rule of Zero, Three e Five](#64-membros-especiais-rule-of-zero-three-e-five)
  - [6.5 Aggregates, designated initializers e structured bindings](#65-aggregates-designated-initializers-e-structured-bindings)
  - [6.6 Membros `static`, classes aninhadas e `friend`](#66-membros-static-classes-aninhadas-e-friend)
- **[Parte 7 — Lifetime, RAII e Recursos](#parte-7--lifetime-raii-e-recursos)**
  - [7.1 Storage duration não é lifetime](#71-storage-duration-não-é-lifetime)
  - [7.2 RAII e destruição determinística](#72-raii-e-destruição-determinística)
  - [7.3 Ownership e handles](#73-ownership-e-handles)
  - [7.4 Inicialização estática e ordem](#74-inicialização-estática-e-ordem)
  - [7.5 Temporários, extensão de lifetime e dangling](#75-temporários-extensão-de-lifetime-e-dangling)
- **[Parte 8 — Controle de Fluxo, Expressões e Operadores](#parte-8--controle-de-fluxo-expressões-e-operadores)**
  - [8.1 `if`, `switch` e initializers](#81-if-switch-e-initializers)
  - [8.2 Laços e range-based `for`](#82-laços-e-range-based-for)
  - [8.3 Operadores, precedência, sequencing e curto-circuito](#83-operadores-precedência-sequencing-e-curto-circuito)
  - [8.4 Sobrecarga de operadores e `operator<=>`](#84-sobrecarga-de-operadores-e-operator)
  - [8.5 Assertions e atributos de controle](#85-assertions-e-atributos-de-controle)
- **[Parte 9 — Categorias de Valor, Cópia e Movimento](#parte-9--categorias-de-valor-cópia-e-movimento)**
  - [9.1 lvalue, prvalue, xvalue e glvalue](#91-lvalue-prvalue-xvalue-e-glvalue)
  - [9.2 Copy e move semantics](#92-copy-e-move-semantics)
  - [9.3 `std::move` não move sozinho](#93-stdmove-não-move-sozinho)
  - [9.4 Perfect forwarding](#94-perfect-forwarding)
  - [9.5 Copy elision, RVO e NRVO](#95-copy-elision-rvo-e-nrvo)
- **[Parte 10 — Enums, Unions e Tipos-Soma](#parte-10--enums-unions-e-tipos-soma)**
  - [10.1 `enum class` e enums não scoped](#101-enum-class-e-enums-não-scoped)
  - [10.2 Flags e operadores bitwise](#102-flags-e-operadores-bitwise)
  - [10.3 Unions e membro ativo](#103-unions-e-membro-ativo)
  - [10.4 `variant`, `optional` e `any`](#104-variant-optional-e-any)
- **[Parte 11 — Modelagem de Tipos e Semântica de Valor](#parte-11--modelagem-de-tipos-e-semântica-de-valor)**
  - [11.1 Invariantes e tipos fortes](#111-invariantes-e-tipos-fortes)
  - [11.2 Igualdade, ordenação e hash](#112-igualdade-ordenação-e-hash)
  - [11.3 Builders, named parameter idiom e factories](#113-builders-named-parameter-idiom-e-factories)
  - [11.4 PImpl e estabilidade de dependências](#114-pimpl-e-estabilidade-de-dependências)
- **[Parte 12 — Herança, Polimorfismo e Composição](#parte-12--herança-polimorfismo-e-composição)**
  - [12.1 Herança, `virtual`, `override` e `final`](#121-herança-virtual-override-e-final)
  - [12.2 Interfaces abstratas e destrutores virtuais](#122-interfaces-abstratas-e-destrutores-virtuais)
  - [12.3 Slicing, downcast e RTTI](#123-slicing-downcast-e-rtti)
  - [12.4 Herança múltipla e virtual](#124-herança-múltipla-e-virtual)
  - [12.5 Composição, type erasure e CRTP](#125-composição-type-erasure-e-crtp)

**Bloco 3 — Programação genérica e biblioteca (Partes 13–18)**

- **[Parte 13 — Callables, Lambdas e Callbacks](#parte-13--callables-lambdas-e-callbacks)**
  - [13.1 Lambdas e capturas](#131-lambdas-e-capturas)
  - [13.2 Lambdas genéricas, `mutable` e lifetime](#132-lambdas-genéricas-mutable-e-lifetime)
  - [13.3 `std::invoke`, `function` e `move_only_function`](#133-stdinvoke-function-e-move_only_function)
  - [13.4 Binding e adaptação](#134-binding-e-adaptação)
  - [13.5 Callbacks e Observer](#135-callbacks-e-observer)
- **[Parte 14 — Iteradores, Algoritmos e Ranges](#parte-14--iteradores-algoritmos-e-ranges)**
  - [14.1 Iteradores, sentinels e categorias](#141-iteradores-sentinels-e-categorias)
  - [14.2 Algoritmos clássicos](#142-algoritmos-clássicos)
  - [14.3 Ranges e projections](#143-ranges-e-projections)
  - [14.4 Views, lazy evaluation e dangling](#144-views-lazy-evaluation-e-dangling)
  - [14.5 Algoritmos paralelos](#145-algoritmos-paralelos)
- **[Parte 15 — Containers da Biblioteca Padrão](#parte-15--containers-da-biblioteca-padrão)**
  - [15.1 Sequenciais](#151-sequenciais)
  - [15.2 Associativos ordenados](#152-associativos-ordenados)
  - [15.3 Associativos unordered](#153-associativos-unordered)
  - [15.4 Adaptadores e containers não owning](#154-adaptadores-e-containers-não-owning)
  - [15.5 Invalidação, complexidade e escolha](#155-invalidação-complexidade-e-escolha)
  - [15.6 `pmr` e containers modernos](#156-pmr-e-containers-modernos)
- **[Parte 16 — Erros, Exceções e Garantias](#parte-16--erros-exceções-e-garantias)**
  - [16.1 Exceções e stack unwinding](#161-exceções-e-stack-unwinding)
  - [16.2 `noexcept` e terminação](#162-noexcept-e-terminação)
  - [16.3 `error_code`, `optional` e `expected`](#163-error_code-optional-e-expected)
  - [16.4 Garantias de exception safety](#164-garantias-de-exception-safety)
  - [16.5 Assertions, precondições e Contracts](#165-assertions-precondições-e-contracts)
- **[Parte 17 — Templates, Concepts e Programação Genérica](#parte-17--templates-concepts-e-programação-genérica)**
  - [17.1 Function, class, alias e variable templates](#171-function-class-alias-e-variable-templates)
  - [17.2 Dedução, CTAD e deduction guides](#172-dedução-ctad-e-deduction-guides)
  - [17.3 Especialização e instanciação](#173-especialização-e-instanciação)
  - [17.4 Variadic templates e fold expressions](#174-variadic-templates-e-fold-expressions)
  - [17.5 Concepts e requires-expressions](#175-concepts-e-requires-expressions)
  - [17.6 SFINAE, nomes dependentes e two-phase lookup](#176-sfinae-nomes-dependentes-e-two-phase-lookup)
- **[Parte 18 — Programação em Compile Time e Metaprogramação](#parte-18--programação-em-compile-time-e-metaprogramação)**
  - [18.1 `constexpr`, `consteval` e `constinit`](#181-constexpr-consteval-e-constinit)
  - [18.2 Type traits e transformação de tipos](#182-type-traits-e-transformação-de-tipos)
  - [18.3 Metaprogramação, recursion e tabelas compiladas](#183-metaprogramação-recursion-e-tabelas-compiladas)
  - [18.4 Feature-test macros](#184-feature-test-macros)
  - [18.5 Reflection no draft do C++26](#185-reflection-no-draft-do-c26)

**Bloco 4 — Memória, baixo nível e concorrência (Partes 19–22)**

- **[Parte 19 — Memória Dinâmica, Ownership e Allocators](#parte-19--memória-dinâmica-ownership-e-allocators)**
  - [19.1 `new`/`delete` e por que evitá-los diretamente](#191-newdelete-e-por-que-evitá-los-diretamente)
  - [19.2 `unique_ptr`](#192-unique_ptr)
  - [19.3 `shared_ptr` e `weak_ptr`](#193-shared_ptr-e-weak_ptr)
  - [19.4 Allocators e `pmr::memory_resource`](#194-allocators-e-pmrmemory_resource)
  - [19.5 Alinhamento, placement `new` e armazenamento bruto](#195-alinhamento-placement-new-e-armazenamento-bruto)
- **[Parte 20 — Modelo de Objetos, Baixo Nível e Comportamento Indefinido](#parte-20--modelo-de-objetos-baixo-nível-e-comportamento-indefinido)**
  - [20.1 Representação, trivialidade e standard layout](#201-representação-trivialidade-e-standard-layout)
  - [20.2 Bytes, endian, `bit_cast` e serialização](#202-bytes-endian-bit_cast-e-serialização)
  - [20.3 Undefined, unspecified e implementation-defined](#203-undefined-unspecified-e-implementation-defined)
  - [20.4 Aliasing, provenance e `launder`](#204-aliasing-provenance-e-launder)
  - [20.5 `volatile`, hardware e sinais](#205-volatile-hardware-e-sinais)
- **[Parte 21 — Threads, Concorrência e Modelo de Memória](#parte-21--threads-concorrência-e-modelo-de-memória)**
  - [21.1 `thread`, `jthread` e cancelamento cooperativo](#211-thread-jthread-e-cancelamento-cooperativo)
  - [21.2 Mutexes, locks e condition variables](#212-mutexes-locks-e-condition-variables)
  - [21.3 Semaphores, latches e barriers](#213-semaphores-latches-e-barriers)
  - [21.4 Atomics, data races e memory ordering](#214-atomics-data-races-e-memory-ordering)
  - [21.5 Futures, `async` e tarefas](#215-futures-async-e-tarefas)
  - [21.6 Deadlock, false sharing e desenho concorrente](#216-deadlock-false-sharing-e-desenho-concorrente)
- **[Parte 22 — Coroutines, Execução e Interoperabilidade](#parte-22--coroutines-execução-e-interoperabilidade)**
  - [22.1 Modelo de coroutines](#221-modelo-de-coroutines)
  - [22.2 `generator` e tipos de tarefa](#222-generator-e-tipos-de-tarefa)
  - [22.3 Execution e senders/receivers no C++26](#223-execution-e-sendersreceivers-no-c26)
  - [22.4 Interoperabilidade com C](#224-interoperabilidade-com-c)
  - [22.5 ABI, bibliotecas compartilhadas e visibilidade](#225-abi-bibliotecas-compartilhadas-e-visibilidade)
- [Checkpoint — Fundamentos da Linguagem](#checkpoint--fundamentos-da-linguagem-partes-122)

**Bloco 5 — Aplicações e arquitetura (Partes 23–24)**

- **[Parte 23 — C++ nos Principais Contextos de Aplicação](#parte-23--c-nos-principais-contextos-de-aplicação)**
  - [23.1 Sistemas, drivers e embedded](#231-sistemas-drivers-e-embedded)
  - [23.2 Games e engines](#232-games-e-engines)
  - [23.3 Desktop, mobile e multimídia](#233-desktop-mobile-e-multimídia)
  - [23.4 HPC, SIMD, GPU e computação científica](#234-hpc-simd-gpu-e-computação-científica)
  - [23.5 Finanças e baixa latência](#235-finanças-e-baixa-latência)
  - [23.6 Backend, bancos e infraestrutura](#236-backend-bancos-e-infraestrutura)
  - [23.7 Safety-critical e constrained environments](#237-safety-critical-e-constrained-environments)
- **[Parte 24 — Arquitetura de Aplicações C++](#parte-24--arquitetura-de-aplicações-c)**
  - [24.1 C++ não impõe arquitetura](#241-c-não-impõe-arquitetura)
  - [24.2 Camadas, Ports and Adapters e dependências](#242-camadas-ports-and-adapters-e-dependências)
  - [24.3 DDD e tipos de domínio](#243-ddd-e-tipos-de-domínio)
  - [24.4 ECS e data-oriented design](#244-ecs-e-data-oriented-design)
  - [24.5 Plugins e fronteiras ABI](#245-plugins-e-fronteiras-abi)
  - [24.6 Concorrência, eventos e backpressure](#246-concorrência-eventos-e-backpressure)
  - [24.7 Padrões clássicos adaptados ao C++ moderno](#247-padrões-clássicos-adaptados-ao-c-moderno)

**Bloco 6 — Engenharia profissional (Partes 25–27)**

- **[Parte 25 — Toolchains, Build, Dependências e Qualidade](#parte-25--toolchains-build-dependências-e-qualidade)**
  - [25.1 GCC, Clang e MSVC](#251-gcc-clang-e-msvc)
  - [25.2 Compilação, linking e opções essenciais](#252-compilação-linking-e-opções-essenciais)
  - [25.3 CMake e targets](#253-cmake-e-targets)
  - [25.4 Conan, vcpkg e dependências](#254-conan-vcpkg-e-dependências)
  - [25.5 Testes, documentação e análise estática](#255-testes-documentação-e-análise-estática)
  - [25.6 Sanitizers, debugger e profiling](#256-sanitizers-debugger-e-profiling)
  - [25.7 Portabilidade, ABI e builds reproduzíveis](#257-portabilidade-abi-e-builds-reproduzíveis)
- **[Parte 26 — I/O, Filesystem, Tempo e Integração de Dados](#parte-26--io-filesystem-tempo-e-integração-de-dados)**
  - [26.1 Iostreams e I/O formatada](#261-iostreams-e-io-formatada)
  - [26.2 Arquivos e filesystem](#262-arquivos-e-filesystem)
  - [26.3 I/O binária, buffers e memória mapeada](#263-io-binária-buffers-e-memória-mapeada)
  - [26.4 Chrono, calendários e fusos](#264-chrono-calendários-e-fusos)
  - [26.5 Regex, locale e parsing](#265-regex-locale-e-parsing)
  - [26.6 Networking e serialização](#266-networking-e-serialização)
- **[Parte 27 — Engenharia para Produção](#parte-27--engenharia-para-produção)**
  - [27.1 Estratégia de testes](#271-estratégia-de-testes)
  - [27.2 Logging, configuração e segredos](#272-logging-configuração-e-segredos)
  - [27.3 Observabilidade e performance](#273-observabilidade-e-performance)
  - [27.4 Segurança essencial](#274-segurança-essencial)
  - [27.5 Empacotamento, distribuição e supply chain](#275-empacotamento-distribuição-e-supply-chain)
  - [27.6 APIs, ABI e evolução](#276-apis-abi-e-evolução)

**Bloco 7 — Catálogo e ecossistema (Partes 28–29)**

- **[Parte 28 — Catálogo da Linguagem e da Biblioteca Padrão](#parte-28--catálogo-da-linguagem-e-da-biblioteca-padrão)**
  - [28.1 Linguagem, biblioteca, implementação e plataforma](#281-linguagem-biblioteca-implementação-e-plataforma)
  - [28.2 Keywords e identificadores especiais](#282-keywords-e-identificadores-especiais)
  - [28.3 Operadores, punctuators e alternative tokens](#283-operadores-punctuators-e-alternative-tokens)
  - [28.4 Tipos fundamentais, literais e sufixos](#284-tipos-fundamentais-literais-e-sufixos)
  - [28.5 Operações prontas por domínio](#285-operações-prontas-por-domínio)
  - [28.6 Containers e estruturas prontas](#286-containers-e-estruturas-prontas)
  - [28.7 Concepts, callables e contratos prontos](#287-concepts-callables-e-contratos-prontos)
  - [28.8 Headers essenciais](#288-headers-essenciais)
  - [28.9 Algoritmos que você não precisa reimplementar](#289-algoritmos-que-você-não-precisa-reimplementar)
  - [28.10 Como descobrir disponibilidade](#2810-como-descobrir-disponibilidade)
- **[Parte 29 — Ecossistema Externo: Frameworks, Bibliotecas e Ferramentas](#parte-29--ecossistema-externo-frameworks-bibliotecas-e-ferramentas)**
  - [29.1 O que é externo ao padrão](#291-o-que-é-externo-ao-padrão)
  - [29.2 Bibliotecas fundamentais e utilitários](#292-bibliotecas-fundamentais-e-utilitários)
  - [29.3 UI, multimídia e games](#293-ui-multimídia-e-games)
  - [29.4 Networking, serviços e mensageria](#294-networking-serviços-e-mensageria)
  - [29.5 Serialização, dados e persistência](#295-serialização-dados-e-persistência)
  - [29.6 Científico, matemática, GPU e visão](#296-científico-matemática-gpu-e-visão)
  - [29.7 Logging, testes e observabilidade](#297-logging-testes-e-observabilidade)
  - [29.8 Build, pacotes e automação](#298-build-pacotes-e-automação)
  - [29.9 Como avaliar e adotar uma dependência](#299-como-avaliar-e-adotar-uma-dependência)

**Anexos e consulta rápida**

- [Anexo A — Trilhas Oficiais de Estudo e Prática](#anexo-a--trilhas-oficiais-de-estudo-e-prática)
- [Anexo B — Referências Oficiais Consultadas](#anexo-b--referências-oficiais-consultadas)
- [Glossário](#glossário)

---

## Parte 1 — Introdução e Contextualização

[⬆️ Voltar ao Sumário](#sumário)

Esta parte separa a linguagem das ferramentas e estabelece o vocabulário usado no restante do guia.

---

### 1.0 Nomenclatura, identificadores e estilo

[⬆️ Voltar ao Sumário](#sumário)

O padrão C++ define quais nomes são válidos e quais são reservados, mas não impõe `camelCase`, `snake_case` ou outro estilo. Convenção de equipe não é regra da linguagem.

| Elemento | Convenção possível | Exemplo |
|---|---|---|
| tipo e concept | `PascalCase` | `Pedido`, `Hashable` |
| função e variável | `snake_case` | `calcular_total` |
| constante | padrão consistente do projeto | `max_tentativas` |
| membro privado | sufixo ou prefixo consistente | `saldo_` |
| macro | maiúsculas e prefixo específico | `PROJETO_ASSERT` |

```cpp
#include <string>
#include <utility>

namespace vendas {

class Pedido {
public:
    explicit Pedido(std::string descricao)
        : descricao_(std::move(descricao)) {}

    [[nodiscard]] const std::string& descricao() const noexcept {
        return descricao_;
    }

private:
    std::string descricao_;
};

} // namespace vendas
```

**Leitura guiada:** `explicit` impede conversão implícita indesejada; o underscore final distingue o membro; o namespace evita colisões; `[[nodiscard]]` pede diagnóstico quando o retorno é ignorado. O estilo pode mudar, mas a intenção precisa permanecer legível.

Nomes que começam com underscore seguido de maiúscula e nomes contendo dois underscores são reservados à implementação em qualquer contexto. No namespace global, nomes iniciados por underscore também são reservados. Não crie `__helper`, `_Arquivo` ou `_global`.

> **Fontes oficiais:** [N4950, identificadores e nomes globais](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf), [C++ Core Guidelines — Naming and layout](https://isocpp.github.io/CppCoreGuidelines/CppCoreGuidelines#S-naming)

---

### 1.1 O que é C++?

[⬆️ Voltar ao Sumário](#sumário)

C++ é uma linguagem de propósito geral, compilada e multiparadigma. Suporta programação procedural, orientação a objetos, programação genérica, funcional e de baixo nível. Seu modelo busca abstrações utilizáveis sem custo desnecessário: você não paga obrigatoriamente por mecanismos que não usa, mas continua responsável por respeitar seus contratos.

```text
fonte (.cpp, headers, módulos)
        ↓ preprocessamento e compilação
unidades/objetos (.o ou .obj)
        ↓ linking
executável ou biblioteca
        ↓ loader do sistema operacional
processo nativo
```

C++ não é um superconjunto perfeito do C moderno. Há programas C válidos que não são C++ válidos e diferenças de tipos, conversões, keywords e biblioteca. Compile cada linguagem no modo correto e exponha fronteiras explícitas.

> **Fontes oficiais:** [ISO/IEC 14882:2024 — escopo](https://www.iso.org/standard/83626.html), [N4950, General principles](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 1.2 Compilador, linker, runtime e implementação

[⬆️ Voltar ao Sumário](#sumário)

| Componente | Responsabilidade |
|---|---|
| preprocessor | inclusão textual, macros e compilação condicional |
| compilador | análise, diagnóstico, otimização e geração de object code |
| assembler | converte assembly em object file, quando a toolchain usa etapa separada |
| linker | resolve símbolos e produz executável ou biblioteca |
| biblioteca padrão | implementa os componentes `std` da versão suportada |
| runtime | suporte a inicialização, exceções, RTTI, alocação e detalhes da implementação |
| loader do SO | mapeia executável e bibliotecas compartilhadas no processo |

“Compilou” não significa “ligou”. Uma declaração pode satisfazer o compilador e ainda faltar uma definição no link. Também é possível ligar com sucesso e falhar em runtime ao carregar uma biblioteca compartilhada incompatível.

```cpp
// matematica.hpp
#pragma once
int somar(int a, int b);

// matematica.cpp
#include "matematica.hpp"
int somar(int a, int b) { return a + b; }

// main.cpp
#include "matematica.hpp"
#include <iostream>

int main() {
    std::cout << somar(2, 3) << '\n';
}
```

**Leitura guiada:** o header declara a função para que `main.cpp` seja compilado; `matematica.cpp` fornece a definição. Ambos os object files precisam chegar ao linker. `#pragma once` é amplamente suportado, mas não é especificado pelo padrão; include guards são a alternativa estritamente portável.

> **Fontes oficiais:** [N4950, separate translation, declarations e ODR](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf), [GCC — Overall Options](https://gcc.gnu.org/onlinedocs/gcc/Overall-Options.html), [Clang Toolchain](https://clang.llvm.org/docs/Toolchain.html)

---

### 1.3 C++ em 2026: C++23 e o draft do C++26

[⬆️ Voltar ao Sumário](#sumário)

Em julho de 2026, o padrão ISO vigente é o C++23, publicado como `ISO/IEC 14882:2024`. O N4950 é o working draft final que formou a base desse padrão. Segundo o Editors’ Report N5051, o N5050 é o draft final do C++26, o draft inicial do C++29 e a base do Draft International Standard do C++26. O N5054, posterior, já é o working draft corrente do C++29.

| Termo | Significado |
|---|---|
| padrão publicado | texto aprovado pela ISO; C++23 é a base estável atual |
| working draft | documento de trabalho público que pode mudar |
| paper Pxxxx | proposta; só integra a linguagem após adoção no working paper adequado |
| Defect Report | correção que implementações podem aplicar retroativamente |
| feature-test macro | forma padronizada de testar suporte quando disponível |
| extensão | recurso do compilador fora do modo ISO escolhido |

```bash
g++ -std=c++23 -Wall -Wextra -Wpedantic main.cpp -o app
clang++ -std=c++23 -Wall -Wextra -Wpedantic main.cpp -o app
cl /std:c++latest /W4 /EHsc main.cpp
```

**Leitura guiada:** GCC e Clang usam `-std=c++23`; `-Wpedantic` ajuda a expor extensões. No MSVC, consulte a tabela de conformidade: `/std:c++latest` habilita recursos em evolução e não equivale a uma promessa de C++26 completo.

Não teste apenas a versão nominal. Linguagem e biblioteca podem ter suporte diferente; use feature-test macros, testes de configuração e páginas oficiais da implementação.

> **Fontes oficiais:** [ISO/IEC 14882:2024](https://www.iso.org/standard/83626.html), [WG21 N4951 — relatório final do C++23](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4951.html), [WG21 N5050 — draft final do C++26](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2026/n5050.pdf), [WG21 N5051 — Editors’ Report do C++26](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2026/n5051.html), [WG21 N5055 — Editors’ Report do C++29](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2026/n5055.html), [GCC C++ status](https://gcc.gnu.org/projects/cxx-status.html), [Clang C++ status](https://clang.llvm.org/cxx_status.html), [MSVC conformance](https://learn.microsoft.com/cpp/overview/visual-cpp-language-conformance)

---

### 1.4 Estrutura e ponto de entrada

[⬆️ Voltar ao Sumário](#sumário)

Um programa hosted possui uma função global `main`. Duas formas padrão usuais são `int main()` e `int main(int argc, char* argv[])`; a implementação pode permitir formas adicionais.

```cpp
#include <iostream>
#include <string_view>

int main(int argc, char* argv[]) {
    const std::string_view nome =
        argc > 1 ? std::string_view{argv[1]} : "visitante";

    std::cout << "Olá, " << nome << "!\n";
    return 0;
}
```

| Trecho | Papel |
|---|---|
| `#include <iostream>` | disponibiliza os streams padrão |
| `argc` | quantidade de argumentos, incluindo o nome invocado |
| `argv` | array de ponteiros para strings C terminadas em zero |
| `std::string_view` | view não owning dos caracteres |
| `return 0` | término bem-sucedido; cair ao fim de `main` também retorna zero |

**Leitura guiada:** a condicional impede acesso a `argv[1]` quando ele não existe. `nome` não possui os caracteres; eles continuam pertencendo ao armazenamento de `argv` ou ao literal. Esse lifetime cobre todo o uso dentro de `main`.

Ambientes freestanding, como certos firmwares, não precisam oferecer o mesmo startup, `main` ou toda a biblioteca de uma implementação hosted.

> **Fontes oficiais:** [N4950, Main function e implementation compliance](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 1.5 Tokens, comentários, statements e blocos

[⬆️ Voltar ao Sumário](#sumário)

Depois das fases iniciais de tradução, o compilador trabalha com tokens: identificadores, keywords, literais, operators e punctuators.

```cpp
#include <string>

[[nodiscard]] std::string saudar(std::string nome) {
    // Comentário até o fim da linha.
    if (nome.empty()) {
        return "Olá!";
    }

    return "Olá, " + nome + "!";
}
```

**Leitura guiada:** declarações e `return` terminam em ponto e vírgula; o bloco do `if` não recebe `;` depois de `}`. O atributo pertence à declaração. O preprocessor vê diretivas começadas por `#` em fase anterior à análise comum das declarações.

> **Armadilha:** `if (condicao); executar();` contém um statement vazio; `executar()` roda sempre. Warnings altos normalmente detectam esse erro.

> **Fontes oficiais:** [N4950, Lexical conventions e Statements](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 1.6 Mapa do iniciante e características distintivas

[⬆️ Voltar ao Sumário](#sumário)

Poucos recursos são literalmente exclusivos. O diferencial de C++ está na combinação de controle de baixo nível, abstrações de alto nível e compatibilidade histórica.

| Todo iniciante precisa dominar | Por quê |
|---|---|
| compilação separada e linking | erros podem nascer em fases diferentes |
| inicialização e tipos | conversões implícitas e valores indeterminados causam defeitos |
| references, pointers e `nullptr` | expressam acesso, opcionalidade e indireção |
| scope, lifetime e ownership | memória válida não implica objeto vivo |
| RAII | recursos são liberados por destruição determinística |
| containers e algoritmos | reduzem código manual e contratos frágeis |
| erros e guarantees | exceções não devem vazar recursos nem corromper invariantes |
| warnings, testes e sanitizers | o compilador não diagnostica todo comportamento indefinido |

| Característica distintiva | Consequência prática |
|---|---|
| value semantics | objetos podem possuir recursos e ainda se comportar como valores |
| destruição determinística | sair do scope destrói objetos automáticos em ordem inversa |
| zero-overhead abstractions | templates e inlining podem remover camadas, sem garantia automática de velocidade |
| undefined behavior | certos erros removem qualquer exigência sobre a execução |
| value categories | overload resolution e movimento dependem da categoria da expressão |
| templates e concepts | polimorfismo em compile time gera especializações por tipo |
| ODR e ADL | organização entre unidades e lookup afetam validade e resolução |
| RAII + exceptions | stack unwinding destrói objetos já construídos |
| múltipla herança e operator overloading | poder expressivo exige contratos previsíveis |
| ausência de runtime único | ABI, alocador, exceptions, RTTI e bibliotecas dependem da implementação |

C++ não possui garbage collection obrigatório, reflection estável no C++23, um tipo padrão universal de tarefa assíncrona, módulos plenamente uniformes entre toolchains nem verificação estática de lifetime. Ferramentas e convenções complementam a linguagem, mas não substituem seus contratos.

> **Fontes oficiais:** [N4950 — General principles, Basics, Expressions e Library](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf), [C++ Core Guidelines](https://isocpp.github.io/CppCoreGuidelines/CppCoreGuidelines)

---

## Parte 2 — Unidades de Tradução, Preprocessor, Namespaces e Módulos

[⬆️ Voltar ao Sumário](#sumário)

Um programa C++ é compilado em partes. Headers tradicionais são inclusão textual; módulos introduzem uma fronteira semântica, mas dependem do suporte e do build da toolchain.

---

### 2.1 Declarações, definições, unidades de tradução e ODR

[⬆️ Voltar ao Sumário](#sumário)

Uma **declaração** introduz ou redeclara um nome. Uma **definição** fornece a entidade completa ou reserva armazenamento, conforme o caso. Depois do preprocessamento, cada fonte forma uma unidade de tradução.

```cpp
// contador.hpp
#ifndef PROJETO_CONTADOR_HPP
#define PROJETO_CONTADOR_HPP

int proximo_id();
inline constexpr int lote = 100;

#endif
```

```cpp
// contador.cpp
#include "contador.hpp"

int proximo_id() {
    static int atual = 0;
    return ++atual;
}
```

**Leitura guiada:** várias unidades podem incluir a declaração. A definição não inline de `proximo_id` deve aparecer uma vez no programa. A variável `inline constexpr` pode ser definida identicamente em várias unidades, de acordo com a ODR.

ODR significa **One Definition Rule**. Algumas violações exigem diagnóstico; outras podem produzir programa ill-formed, no diagnostic required. Não confie no linker para detectar todas.

> **Fontes oficiais:** [N4950, Declarations and definitions e One-definition rule](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 2.2 `include`, include guards e macros

[⬆️ Voltar ao Sumário](#sumário)

`#include` substitui a diretiva pelo conteúdo do header durante o preprocessamento. Include guards evitam múltiplas inclusões na mesma unidade.

```cpp
#ifndef PROJETO_CONFIG_HPP
#define PROJETO_CONFIG_HPP

namespace projeto {
inline constexpr int versao_api = 3;
}

#endif
```

**Leitura guiada:** o mesmo header pode ser incluído mais de uma vez na unidade de tradução, mas apenas a primeira passagem define seu conteúdo. O nome do guard deve ser único no projeto. A constante `inline constexpr` pode ser definida no header sem criar uma definição não inline por unidade.

Prefira funções `inline`, `constexpr`, templates, enums e constantes tipadas a macros. Macros não respeitam scope, tipos ou avaliação única:

```cpp
#define QUADRADO_PERIGOSO(x) ((x) * (x))

constexpr int quadrado(int x) noexcept {
    return x * x;
}
```

**Leitura guiada:** `QUADRADO_PERIGOSO(i++)` modifica `i` duas vezes. A função avalia o argumento uma vez e participa do sistema de tipos. Não invoque a macro com expressões que tenham efeitos colaterais.

Use compilação condicional para diferenças reais de plataforma ou disponibilidade, centralizadas em uma camada pequena. `#if 0` não é sistema de versionamento.

> **Fontes oficiais:** [N4950, Preprocessing directives](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf), [C++ Core Guidelines ES.30–ES.33](https://isocpp.github.io/CppCoreGuidelines/CppCoreGuidelines#Res-macros2)

---

### 2.3 Namespaces, `using` e ADL

[⬆️ Voltar ao Sumário](#sumário)

Namespaces agrupam nomes. Alias reduz nomes longos; `using` localizado importa uma entidade. Evite `using namespace` em headers, pois ele altera o lookup de todos os consumidores.

```cpp
#include <iostream>

namespace geometria {

struct Ponto {
    double x;
    double y;
};

void imprimir(const Ponto& p) {
    std::cout << '(' << p.x << ", " << p.y << ")\n";
}

} // namespace geometria

int main() {
    geometria::Ponto p{2.0, 3.5};
    imprimir(p); // encontrado também por ADL
}
```

**Leitura guiada:** embora `imprimir` não esteja qualificado, argument-dependent lookup procura funções nos namespaces associados ao tipo `Ponto`. ADL é essencial para customizações como `swap`, mas também pode introduzir candidatos inesperados.

O namespace `std` pertence à implementação. Em geral, não adicione declarações nele; as poucas especializações permitidas possuem regras específicas.

> **Fontes oficiais:** [N4950, Name lookup, Namespaces e namespace std](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 2.4 Headers, fontes e forward declarations

[⬆️ Voltar ao Sumário](#sumário)

Headers expõem declarações necessárias aos consumidores; fontes escondem definições que não precisam ser vistas. Templates normalmente precisam ter sua definição alcançável no ponto de instanciação.

```cpp
// relatorio.hpp
#pragma once

#include <memory>

class Impressora;

class Relatorio {
public:
    explicit Relatorio(std::shared_ptr<Impressora> impressora);
    void emitir() const;

private:
    std::shared_ptr<Impressora> impressora_;
};
```

**Leitura guiada:** a forward declaration basta para declarar um `shared_ptr<Impressora>`. A fonte que chama métodos de `Impressora` precisa incluir sua definição. Headers devem ser self-contained: um consumidor precisa poder incluí-los sem depender de ordem acidental.

Inclua o próprio header primeiro em sua fonte para testar essa propriedade. Use forward declarations quando elas realmente reduzem acoplamento, não como ritual.

> **Fontes oficiais:** [N4950, declarations, ODR e instantiation](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf), [C++ Core Guidelines SF.8–SF.11](https://isocpp.github.io/CppCoreGuidelines/CppCoreGuidelines#Rs-inc)

---

### 2.5 Módulos e header units

[⬆️ Voltar ao Sumário](#sumário)

Módulos C++20 permitem exportar declarações sem inclusão textual direta. Eles não são pacotes, bibliotecas binárias ou gerenciadores de dependência.

```cpp
// geometria.cppm
export module geometria;

export struct Ponto {
    double x;
    double y;
};

export double distancia(Ponto a, Ponto b);
```

```cpp
// consumidor.cpp
import geometria;
#include <iostream>

int main() {
    std::cout << distancia({0, 0}, {3, 4}) << '\n';
}
```

**Leitura guiada:** a module interface exporta `Ponto` e `distancia`; entidades não exportadas podem permanecer internas ao módulo. O build precisa conhecer a ordem e os artefatos de interface exigidos pela implementação.

C++23 padroniza módulos de biblioteca `std` e `std.compat`, mas disponibilidade e comandos continuam dependentes da toolchain. Não substitua headers por módulos sem confirmar suporte do compilador, biblioteca padrão, gerador de build e IDE.

> **Fontes oficiais:** [N4950, Modules e standard library modules](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf), [GCC C++ Modules](https://gcc.gnu.org/onlinedocs/gcc/C_002b_002b-Modules.html), [Clang Standard C++ Modules](https://clang.llvm.org/docs/StandardCPlusPlusModules.html), [MSVC modules](https://learn.microsoft.com/cpp/cpp/modules-cpp)

---

## Parte 3 — Tipos, Valores, Inicialização e Referências

[⬆️ Voltar ao Sumário](#sumário)

Em C++, um tipo determina representação, operações válidas, alinhamento e regras de lifetime. Não presuma tamanhos que o padrão não promete.

---

### 3.1 Tipos fundamentais, tamanho e limites

[⬆️ Voltar ao Sumário](#sumário)

| Família | Tipos | Observação |
|---|---|---|
| booleano | `bool` | valores `true` e `false` |
| caracteres | `char`, `signed char`, `unsigned char`, `wchar_t`, `char8_t`, `char16_t`, `char32_t` | tipos distintos; encoding depende do tipo e do literal |
| inteiros | variantes `short`, `int`, `long`, `long long`, signed/unsigned | larguras mínimas e ordem de tamanhos são especificadas; largura exata não |
| ponto flutuante | `float`, `double`, `long double` e tipos estendidos opcionais | precisão e representação dependem da implementação |
| vazio | `void` | ausência de valor ou tipo incompleto especial |
| nulo | `std::nullptr_t` | tipo de `nullptr` |

```cpp
#include <cstdint>
#include <iostream>
#include <limits>

int main() {
    std::cout << sizeof(int) << '\n';
    std::cout << std::numeric_limits<double>::digits10 << '\n';

    std::int32_t protocolo = 42;
    std::uint64_t contador = 0;
    std::cout << protocolo << ' ' << contador << '\n';
}
```

**Leitura guiada:** `sizeof` mede em bytes de `CHAR_BIT` bits, não necessariamente oito em toda implementação. Os aliases `int32_t` e `uint64_t` só existem quando a implementação oferece exatamente essas larguras. Para tamanhos e índices, considere `std::size_t`; para diferenças entre iteradores, `std::ptrdiff_t`.

Unsigned arithmetic usa módulo de potência de dois; overflow signed é comportamento indefinido. Isso não torna `unsigned` a escolha automática para todo valor não negativo: conversões mistas podem surpreender.

> **Fontes oficiais:** [N4950, Fundamental types, Basic concepts e Numeric limits](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 3.2 Objetos, valores, escopo, storage duration e lifetime

[⬆️ Voltar ao Sumário](#sumário)

Um **objeto** ocupa uma região de armazenamento e possui tipo e lifetime. Escopo controla onde um nome é visível; storage duration controla por quanto tempo o armazenamento existe; lifetime controla quando um objeto está vivo nesse armazenamento.

| Storage duration | Exemplo | Duração aproximada |
|---|---|---|
| automatic | variável local | entrada até saída do bloco |
| static | variável global ou `static` local | execução do programa |
| thread | `thread_local` | duração da thread |
| dynamic | objeto alocado | até desalocação explícita ou por owner |

```cpp
#include <iostream>

struct Rastreador {
    int id;
    ~Rastreador() { std::cout << "destruindo " << id << '\n'; }
};

int main() {
    Rastreador externo{1};
    {
        Rastreador interno{2};
        std::cout << "dentro\n";
    }
    std::cout << "fora\n";
}
```

**Leitura guiada:** `interno` é destruído ao sair do bloco; `externo`, ao sair de `main`. A ordem inversa de construção permite compor recursos com RAII. Uma região de memória ainda acessível não autoriza usar um objeto cujo lifetime terminou.

> **Fontes oficiais:** [N4950, Memory and objects, Scope e Object lifetime](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 3.3 Formas de inicialização e narrowing

[⬆️ Voltar ao Sumário](#sumário)

C++ possui default-, value-, direct-, copy- e list-initialization. Para código novo, chaves ajudam a rejeitar conversões narrowing, mas `initializer_list` pode alterar a resolução de construtores.

```cpp
#include <vector>

int quantidade{};           // zero
double taxa{0.15};
int arredondado = 3.0;      // permitido: conversão
// int erro{3.0};            // erro: narrowing

std::vector<int> tres_zeros(3); // três elementos com zero
std::vector<int> um_tres{3};    // um elemento com valor 3
```

**Leitura guiada:** `{}` value-initializa `quantidade`. Parênteses e chaves não são intercambiáveis em `vector`: um chama o construtor de tamanho; o outro prefere `initializer_list`. Conheça o overload set antes de escolher apenas por estilo.

Variáveis automáticas de tipo fundamental sem inicializador não recebem automaticamente zero. No C++23, usar um valor indeterminado normalmente leva a comportamento indefinido, salvo exceções estreitas para tipos byte-like. O draft C++26 separa ainda valores *erroneous* de valores indeterminados: certas leituras passam a ter **comportamento errôneo**, que continua sendo defeito do programa, mas cuja detecção é recomendada à implementação. Inicialize por padrão e não dependa dessa distinção para tornar uma leitura válida.

> **Fontes oficiais:** [N4950, Initializers, List-initialization e Indeterminate values](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf), [N5050, Erroneous behavior e Indeterminate and erroneous values](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2026/n5050.pdf), [C++ Core Guidelines ES.20–ES.23](https://isocpp.github.io/CppCoreGuidelines/CppCoreGuidelines#Res-always)

---

### 3.4 `const`, `volatile` e `mutable`

[⬆️ Voltar ao Sumário](#sumário)

`const` impede modificação por aquela interface. Não implica que o objeto jamais mude por outro alias, nem torna operações automaticamente thread-safe.

```cpp
int main() {
    int valor = 10;
    const int* leitura = &valor;       // ponteiro para int const
    int* const endereco_fixo = &valor; // ponteiro const para int

    valor = 20;
    // *leitura = 30;       // erro por essa interface
    *endereco_fixo = 30;    // permitido
}
```

**Leitura guiada:** leia declarações de dentro para fora: `leitura` aponta para `const int`; `endereco_fixo` é um ponteiro const para `int`. `const int* const` fixaria ambos.

`mutable` permite que um membro seja alterado por um método `const`, útil para cache ou mutex que não muda o valor lógico. `volatile` representa acessos observáveis especiais definidos pela implementação; não oferece atomicidade nem sincronização entre threads.

> **Fontes oficiais:** [N4950, cv-qualifiers, compound types e data races](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 3.5 `auto`, `decltype`, aliases e structured bindings

[⬆️ Voltar ao Sumário](#sumário)

`auto` deduz o tipo a partir do inicializador. Normalmente remove referência e top-level `const`; `decltype` segue regras próprias e pode preservar categoria e referência.

```cpp
#include <map>
#include <string>
#include <type_traits>

using IdPedido = unsigned long long;

int main() {
    const int original = 7;
    auto copia = original;            // int
    const auto constante = original;  // const int
    auto& referencia = original;      // const int&

    std::map<std::string, int> estoque{{"caneta", 10}};
    for (const auto& [produto, quantidade] : estoque) {
        (void)produto;
        (void)quantidade;
    }

    static_assert(std::is_same_v<decltype((referencia)), const int&>);
    (void)copia;
    (void)constante;
}
```

**Leitura guiada:** a structured binding cria nomes ligados aos componentes do elemento. O `&` evita copiar cada par. Os parênteses em `decltype((referencia))` fazem a regra da expressão devolver referência lvalue; `decltype(referencia)` usaria a regra especial de nome.

`auto` não é tipagem dinâmica: o tipo é fixado em compile time. Use alias para comunicar domínio ou simplificar tipos; um alias não cria um tipo nominal novo.

> **Fontes oficiais:** [N4950, Placeholder types, `decltype`, Alias declarations e Structured bindings](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 3.6 Ponteiros, referências e `nullptr`

[⬆️ Voltar ao Sumário](#sumário)

| Forma | Pode ser nula? | Pode ser reseated? | Semântica recomendada |
|---|---:|---:|---|
| `T` | não se aplica | não se aplica | valor próprio |
| `T&` | não por contrato da linguagem | não | acesso obrigatório não owning |
| `const T&` | não | não | leitura obrigatória sem cópia |
| `T*` | sim | sim | acesso opcional ou intervalo de baixo nível |
| smart pointer | depende | depende | ownership explícito |

```cpp
#include <iostream>

void incrementar(int& valor) {
    ++valor;
}

void imprimir_se_existir(const int* valor) {
    if (valor != nullptr) {
        std::cout << *valor << '\n';
    }
}

int main() {
    int numero = 4;
    incrementar(numero);
    imprimir_se_existir(&numero);
    imprimir_se_existir(nullptr);
}
```

**Leitura guiada:** a referência expressa parâmetro obrigatório e permite modificar `numero`. O ponteiro expressa ausência; só é dereferenciado depois do teste. `nullptr` possui tipo próprio e evita ambiguidades históricas de `0` ou `NULL`.

Ponteiro não comunica ownership sozinho. Aritmética só é válida dentro de um array apropriado e até one-past-the-end; dereferenciar one-past é inválido.

> **Fontes oficiais:** [N4950, Compound types, Pointer conversions e Null pointer](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf), [C++ Core Guidelines F.42–F.60](https://isocpp.github.io/CppCoreGuidelines/CppCoreGuidelines#Rf-ptr)

---

### 3.7 Arrays, `std::array` e `std::span`

[⬆️ Voltar ao Sumário](#sumário)

Arrays internos têm tamanho fixo e frequentemente decaem para ponteiro em parâmetros. `std::array<T, N>` preserva semântica de container; `std::span<T>` representa uma view contígua não owning.

```cpp
#include <array>
#include <iostream>
#include <span>

int soma(std::span<const int> valores) {
    int total = 0;
    for (int valor : valores) {
        total += valor;
    }
    return total;
}

int main() {
    int antigos[]{1, 2, 3};
    std::array modernos{4, 5, 6};

    std::cout << soma(antigos) << '\n';
    std::cout << soma(modernos) << '\n';
}
```

**Leitura guiada:** o mesmo algoritmo aceita array interno e `std::array` sem copiar elementos. O `span` carrega ponteiro e tamanho, mas não estende lifetime. Retornar um span para um array local deixaria a view pendente.

Use `std::vector` quando o tamanho muda ou só é conhecido em runtime e ownership dinâmico é necessário.

> **Fontes oficiais:** [N4950, Arrays, `array`, `span` e container requirements](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 3.8 Conversões e casts

[⬆️ Voltar ao Sumário](#sumário)

Conversões implícitas participam de inicialização, chamadas e operadores. Casts nomeados comunicam a intenção:

| Cast | Uso legítimo típico |
|---|---|
| `static_cast` | conversão conhecida em compile time, inclusive numérica |
| `dynamic_cast` | navegação segura em hierarquia polimórfica |
| `const_cast` | adicionar/remover cv; modificar objeto originalmente const continua inválido |
| `reinterpret_cast` | conversão de representação/ponteiro de baixo nível com regras estritas |

```cpp
#include <cstddef>
#include <limits>
#include <stdexcept>

int para_int(std::size_t valor) {
    if (valor > static_cast<std::size_t>(
                    std::numeric_limits<int>::max())) {
        throw std::overflow_error{"valor não cabe em int"};
    }
    return static_cast<int>(valor);
}
```

**Leitura guiada:** o teste ocorre antes do cast potencialmente truncante. O cast C++ nomeado deixa a conversão pesquisável. Cast estilo C pode tentar várias categorias e esconder a operação real.

> **Fontes oficiais:** [N4950, Standard conversions e Explicit type conversion](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf), [C++ Core Guidelines ES.46–ES.49](https://isocpp.github.io/CppCoreGuidelines/CppCoreGuidelines#Res-casts)

---

## Parte 4 — Texto, Strings e Unicode

[⬆️ Voltar ao Sumário](#sumário)

C++ separa containers de code units, tipos de caracteres e encoding. `std::string` não valida Unicode e não significa automaticamente UTF-8.

---

### 4.1 Caracteres, code units e encodings

[⬆️ Voltar ao Sumário](#sumário)

| Tipo/literal | Unidade | Observação |
|---|---|---|
| `char`, `"texto"` | code unit da ordinary literal encoding | encoding definido pela implementação/configuração |
| `char8_t`, `u8"texto"` | code unit UTF-8 | tipo distinto desde C++20 |
| `char16_t`, `u"texto"` | code unit UTF-16 | um code point pode usar surrogate pair |
| `char32_t`, `U"texto"` | code unit UTF-32 | code unit de 32 bits |
| `wchar_t`, `L"texto"` | wide character | largura e encoding dependem da plataforma |

```cpp
#include <cstddef>
#include <iostream>
#include <string_view>

int main() {
    constexpr std::u8string_view palavra = u8"ação";
    std::cout << palavra.size() << '\n';
}
```

**Leitura guiada:** `size()` conta code units UTF-8, não letras visíveis nem grapheme clusters. Acentos podem usar mais de um byte, e um símbolo visual pode ser composto por vários code points.

Escolha e documente encoding nas fronteiras: arquivo, terminal, rede, API do sistema e banco. Converter bytes por `static_cast` não transcodifica texto.

> **Fontes oficiais:** [N4950, Character sets, Character types e String literals](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf), [WG21 SG16 — Unicode](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/)

---

### 4.2 `std::string` e variantes

[⬆️ Voltar ao Sumário](#sumário)

`std::basic_string<CharT>` é um container owning e contíguo de code units. `std::string`, `u8string`, `u16string`, `u32string` e `wstring` são aliases para tipos de caractere diferentes.

```cpp
#include <iostream>
#include <string>

int main() {
    std::string mensagem = "Olá";
    mensagem += ", C++";
    mensagem.push_back('!');

    std::cout << mensagem << '\n';
    std::cout << mensagem.size() << " bytes/code units\n";
}
```

**Leitura guiada:** a string possui seu armazenamento e pode realocar ao crescer; ponteiros, referências e iteradores anteriores podem ser invalidados. `size()` não interpreta Unicode. `c_str()` fornece sequência terminada em zero válida até uma operação invalidante.

Evite concatenar repetidamente em loops sem considerar alocação; `reserve`, `append`, formatação ou escrita direta podem expressar melhor a intenção.

> **Fontes oficiais:** [N4950, Strings library e `basic_string`](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 4.3 `std::string_view` e lifetime

[⬆️ Voltar ao Sumário](#sumário)

`std::string_view` é uma view de caracteres: normalmente um ponteiro e tamanho. Copiá-la é barato, mas ela não possui nem prolonga o armazenamento.

```cpp
#include <iostream>
#include <string>
#include <string_view>

void logar(std::string_view texto) {
    std::cout << texto << '\n';
}

int main() {
    std::string dono = "mensagem persistente";
    logar(dono);
    logar("literal");
}
```

**Leitura guiada:** durante as chamadas, tanto `dono` quanto o literal vivem tempo suficiente. Guardar a view além da chamada exige provar que o dono sobreviverá. `string_view` não promete terminação em zero; não passe `data()` a uma API C que exige C string sem conferir o contrato.

```cpp
#include <string>
#include <string_view>

std::string_view perigoso() {
    std::string local = "acabou";
    return local; // view pendente ao retornar
}
```

**Leitura guiada:** a conversão não copia. Quando `local` é destruída, a view devolvida aponta para armazenamento inválido. Retorne `std::string` quando o resultado precisa possuir texto.

> **Fontes oficiais:** [N4950, String view classes](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf), [C++ Core Guidelines SL.str.2](https://isocpp.github.io/CppCoreGuidelines/CppCoreGuidelines#Rstr-view)

---

### 4.4 Literais, strings raw e concatenação

[⬆️ Voltar ao Sumário](#sumário)

Literais adjacentes compatíveis são concatenados em compile time. Raw string literals evitam duplicar escapes.

```cpp
#include <string>

const std::string json =
    R"json({"nome":"Ana","caminho":"C:\dados"})json";

const char* mensagem =
    "primeira parte "
    "segunda parte";
```

**Leitura guiada:** o delimitador `json` precisa coincidir no início e no fim do raw literal e não faz parte do conteúdo. Os dois literais comuns adjacentes formam um único array. `const char*` aponta para literal estático; modificá-lo é proibido.

Sufixos padronizados como `"abc"s` criam `std::string`, mas exigem o namespace de literais apropriado. Importe o literal em scope pequeno, não um namespace inteiro em header.

> **Fontes oficiais:** [N4950, String literals e Standard library literals](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 4.5 Formatação, parsing e streams de texto

[⬆️ Voltar ao Sumário](#sumário)

| Necessidade | API padrão |
|---|---|
| formatar para string | `std::format` |
| imprimir formatado no C++23 | `std::print`, `std::println` |
| converter número sem locale/alocação | `std::to_chars`, `std::from_chars` |
| composição baseada em stream | `std::ostringstream`, `std::istringstream` |
| conversão simples histórica | `std::stoi`, `std::stod` |

```cpp
#include <charconv>
#include <format>
#include <iostream>
#include <string>
#include <string_view>

int main() {
    const std::string mensagem =
        std::format("produto={} quantidade={}", "caneta", 12);
    std::cout << mensagem << '\n';

    int valor{};
    constexpr std::string_view entrada = "2048";
    const auto [fim, erro] =
        std::from_chars(entrada.data(), entrada.data() + entrada.size(), valor);

    if (erro == std::errc{} && fim == entrada.data() + entrada.size()) {
        std::cout << valor << '\n';
    }
}
```

**Leitura guiada:** `std::format` valida o formato compatível em compile time quando recebe format string apropriada. `from_chars` devolve ponteiro final e código de erro; o teste exige consumo completo, impedindo aceitar silenciosamente `"2048xyz"`.

`std::print` é C++23, mas a biblioteca instalada pode ainda não implementá-lo. Verifique a feature-test macro e a tabela oficial da implementação em vez de presumir suporte pelo compilador.

> **Fontes oficiais:** [N4950, Formatting, Print functions e Primitive numeric conversions](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf), [GCC libstdc++ status](https://gcc.gnu.org/onlinedocs/libstdc++/manual/status.html), [LLVM libc++ status](https://libcxx.llvm.org/Status/Cxx23.html), [MSVC STL status](https://github.com/microsoft/STL/wiki/Changelog)

---

## Parte 5 — Funções, Parâmetros e Sobrecarga

[⬆️ Voltar ao Sumário](#sumário)

Funções expressam contratos por tipos, qualifiers, atributos e política de erro. Uma assinatura pequena e ownership claro valem mais que comentários compensando parâmetros ambíguos.

---

### 5.1 Declaração, definição, assinatura e retorno

[⬆️ Voltar ao Sumário](#sumário)

```cpp
#include <cmath>

[[nodiscard]] double hipotenusa(double a, double b);

double hipotenusa(double a, double b) {
    return std::hypot(a, b);
}
```

**Leitura guiada:** a primeira linha declara; a segunda declaração, acompanhada do corpo, define a função. Para overload, nome e parameter-type-list participam da assinatura; apenas mudar o retorno não cria overload. `[[nodiscard]]` permite diagnóstico quando o resultado é descartado.

Prefira retornar valores: C++ permite copy elision e movimento. Retornar referência ou ponteiro exige garantir que o referent sobreviva ao chamador.

> **Fonte oficial:** [N4950, Declarations, Function definitions e Functions](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 5.2 Passagem por valor, referência e ponteiro

[⬆️ Voltar ao Sumário](#sumário)

| Forma | Use quando |
|---|---|
| `T` | precisa de cópia/movimento local ou `T` é pequeno |
| `const T&` | leitura sem cópia e argumento obrigatório |
| `T&` | função modifica argumento obrigatório |
| `T*` | acesso opcional; documente ownership |
| `std::span<T>` | sequência contígua com tamanho |
| `std::string_view` | texto não owning usado apenas durante lifetime válido |

```cpp
#include <string>
#include <utility>

class Cadastro {
public:
    void definir_nome(std::string nome) {
        nome_ = std::move(nome);
    }
private:
    std::string nome_;
};
```

**Leitura guiada:** receber por valor permite ao chamador copiar um lvalue ou mover um rvalue. A função move sua cópia para o membro. Esse padrão é simples quando a função guardará o valor; não é automaticamente ideal para objetos caros que quase nunca são movidos.

> **Fonte oficial:** [C++ Core Guidelines F.15–F.21](https://isocpp.github.io/CppCoreGuidelines/CppCoreGuidelines#Rf-conventional), [N4950, References e Initialization](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 5.3 Sobrecarga, argumentos padrão e resolução

[⬆️ Voltar ao Sumário](#sumário)

Overload resolution reúne candidatos, verifica viabilidade e escolhe a melhor sequência de conversões. Conversões implícitas, templates, ADL e constructors podem alterar o resultado.

```cpp
#include <string_view>

void registrar(int codigo);
void registrar(std::string_view texto);

void conectar(int timeout_ms = 1'000);
```

**Leitura guiada:** `registrar(42)` escolhe `int`; `registrar("ok")` usa conversão para `string_view`. Argumentos padrão são substituídos no ponto da chamada e não fazem parte do tipo da função. Declare o default uma vez, normalmente no header, e evite parâmetros booleanos opacos.

> **Armadilha:** `0`, `NULL` e ponteiros podem criar overload ambíguo; use `nullptr`. Constructors não `explicit` também introduzem conversões definidas pelo usuário.

> **Fonte oficial:** [N4950, Overloading e Default arguments](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 5.4 `inline`, `constexpr`, `noexcept` e atributos

[⬆️ Voltar ao Sumário](#sumário)

| Recurso | Contrato principal |
|---|---|
| `inline` | permite definições idênticas em várias unidades; não obriga machine-code inlining |
| `constexpr` | permite avaliação constante quando argumentos/contexto satisfazem regras |
| `consteval` | toda chamada potencialmente avaliada deve produzir constant expression |
| `noexcept` | declara que a função não deixa exceção escapar |
| `[[nodiscard]]` | pede diagnóstico ao ignorar resultado |
| `[[deprecated]]` | marca API em migração |

```cpp
constexpr int dobro(int x) noexcept { return x * 2; }
static_assert(dobro(21) == 42);
```

**Leitura guiada:** a mesma função pode rodar em compile time ou runtime. Se uma exceção escapar de função `noexcept`, `std::terminate` é chamado. Use `noexcept` quando o contrato é verdadeiro, especialmente em movimentos e destructors.

> **Fonte oficial:** [N4950, `inline`, `constexpr`, exception specifications e attributes](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 5.5 Function pointers e parâmetros de saída

[⬆️ Voltar ao Sumário](#sumário)

```cpp
int somar(int a, int b) { return a + b; }

using Operacao = int (*)(int, int);

int aplicar(Operacao op, int a, int b) {
    return op(a, b);
}
```

**Leitura guiada:** `Operacao` guarda apenas endereço de função compatível, sem estado capturado. Lambdas sem captura podem converter para function pointer; callables com estado pedem template ou wrapper como `std::function`.

Evite múltiplos parâmetros de saída quando um struct de resultado comunica nomes e invariantes. Use referência não const para saída obrigatória e ponteiro para saída opcional apenas se esse contrato for realmente melhor que retornar valor.

> **Fonte oficial:** [N4950, Pointers to functions e Function call](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

## Parte 6 — Classes, Construção e Membros Especiais

[⬆️ Voltar ao Sumário](#sumário)

Classes reúnem estado e operações. O objetivo é preservar invariantes em todas as transições, inclusive construção, cópia, movimento e destruição.

---

### 6.1 `class`, `struct`, membros e acesso

[⬆️ Voltar ao Sumário](#sumário)

`class` e `struct` têm o mesmo poder; diferem nos defaults de acesso e herança: `class` começa privado, `struct` público.

```cpp
class Conta {
public:
    explicit Conta(long saldo_inicial) : saldo_(saldo_inicial) {}
    [[nodiscard]] long saldo() const noexcept { return saldo_; }
    void depositar(long valor);
private:
    long saldo_;
};
```

**Leitura guiada:** callers não alteram `saldo_` sem passar por operações do tipo. O `const` após `saldo()` qualifica o objeto implícito; o método promete não modificar seu estado observável.

Use `struct` para agregados ou tipos com membros públicos coerentes; use `class` quando encapsulamento e invariantes forem centrais.

> **Fonte oficial:** [N4950, Classes e Member access control](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 6.2 Construtores e listas de inicialização

[⬆️ Voltar ao Sumário](#sumário)

```cpp
#include <stdexcept>
#include <string>
#include <utility>

class Usuario {
public:
    Usuario(std::string nome, int idade)
        : nome_(std::move(nome)), idade_(idade) {
        if (nome_.empty() || idade_ < 0) {
            throw std::invalid_argument{"usuário inválido"};
        }
    }
private:
    std::string nome_;
    int idade_;
};
```

**Leitura guiada:** membros são inicializados antes do corpo e na ordem de declaração, não na ordem textual da lista. Se a validação lança, membros já construídos são destruídos; o objeto `Usuario` nunca passa a existir completamente.

Use `explicit` em constructors de um argumento salvo quando a conversão implícita for parte natural do domínio. Delegating constructors centralizam regras, mas não podem inicializar outros membros além do constructor delegado.

> **Fonte oficial:** [N4950, Initialization e Constructors](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 6.3 Destrutores e RAII

[⬆️ Voltar ao Sumário](#sumário)

Destructor executa quando o lifetime termina. Ele deve liberar o recurso possuído e, em geral, não lançar.

```cpp
#include <cstdio>
#include <stdexcept>

class Arquivo {
public:
    explicit Arquivo(const char* caminho)
        : handle_(std::fopen(caminho, "rb")) {
        if (handle_ == nullptr) {
            throw std::runtime_error{"falha ao abrir arquivo"};
        }
    }

    ~Arquivo() { std::fclose(handle_); }

    Arquivo(const Arquivo&) = delete;
    Arquivo& operator=(const Arquivo&) = delete;

private:
    std::FILE* handle_;
};
```

**Leitura guiada:** o constructor estabelece ownership ou lança. O destructor fecha exatamente uma vez. Cópia foi proibida para evitar dois owners do mesmo `FILE*`; movimento poderia ser implementado, mas um wrapper pronto com deleter costuma ser preferível.

> **Fonte oficial:** [N4950, Destructors e Stack unwinding](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf), [C++ Core Guidelines C.30–C.37](https://isocpp.github.io/CppCoreGuidelines/CppCoreGuidelines#Rc-dtor)

---

### 6.4 Membros especiais: Rule of Zero, Three e Five

[⬆️ Voltar ao Sumário](#sumário)

O compilador pode declarar default constructor, copy/move constructor, copy/move assignment e destructor. Declarar alguns afeta a geração dos outros.

| Regra | Orientação |
|---|---|
| Zero | componha tipos RAII e não escreva membros especiais |
| Three | código histórico que gerencia recurso precisa copy constructor, copy assignment e destructor coerentes |
| Five | em C++11+, considere também move constructor e move assignment |

```cpp
#include <string>
#include <vector>

struct Documento {
    std::string titulo;
    std::vector<std::string> linhas;
};
```

**Leitura guiada:** `string` e `vector` já implementam cópia, movimento e destruição; `Documento` pode usar os membros gerados. Esta Rule of Zero reduz código e preserva exception safety melhor que arrays manuais.

Use `= default` para afirmar a semântica gerada e `= delete` para proibir uma operação. Teste traits/concepts se generic code depende de copyability ou movability.

> **Fonte oficial:** [N4950, Special member functions](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf), [C++ Core Guidelines C.20–C.22](https://isocpp.github.io/CppCoreGuidelines/CppCoreGuidelines#Rc-zero)

---

### 6.5 Aggregates, designated initializers e structured bindings

[⬆️ Voltar ao Sumário](#sumário)

```cpp
#include <string>

struct Servidor {
    std::string host;
    int porta{443};
    bool tls{true};
};

int main() {
    Servidor s{.host = "api.exemplo", .porta = 8443, .tls = true};
    auto& [host, porta, tls] = s;
    porta = 9443;
}
```

**Leitura guiada:** designated initializers C++20 devem seguir a ordem de declaração e só se aplicam a aggregates. A structured binding com `auto&` liga aos membros; sem `&`, criaria cópias.

Alterar constructors ou membros privados pode fazer um tipo deixar de ser aggregate. Não trate aggregate como compromisso de ABI sem regras adicionais.

> **Fonte oficial:** [N4950, Aggregates, Designated-initializer-list e Structured bindings](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 6.6 Membros `static`, classes aninhadas e `friend`

[⬆️ Voltar ao Sumário](#sumário)

Membro `static` pertence à classe, não a cada objeto. Classe aninhada é um tipo no scope da classe, sem referência automática à instância externa. `friend` concede acesso específico e não é herdado nem transitivo.

```cpp
class Chave {
public:
    static Chave criar(int valor) { return Chave{valor}; }
private:
    explicit Chave(int valor) : valor_(valor) {}
    int valor_;
};
```

**Leitura guiada:** o constructor privado impede `Chave chave{1}` fora da classe; a função `criar` pode chamá-lo porque é membro. A factory estática nomeia a intenção e pode evoluir para validar ou devolver erro. `friend` é legítimo para operadores simétricos ou colaboração estreita, mas excesso sinaliza encapsulamento mal desenhado.

> **Fonte oficial:** [N4950, Class members, Nested classes e Friends](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

## Parte 7 — Lifetime, RAII e Recursos

[⬆️ Voltar ao Sumário](#sumário)

Lifetime é o centro do C++ seguro. Recursos incluem memória, arquivos, sockets, locks, threads, transactions e registrations.

---

### 7.1 Storage duration não é lifetime

[⬆️ Voltar ao Sumário](#sumário)

Armazenamento é onde bytes residem; objeto é a entidade viva nesses bytes. Placement construction pode iniciar novo lifetime em storage existente, e destruir um objeto não desaloca necessariamente seu storage.

```cpp
#include <cstddef>
#include <memory>

int main() {
    alignas(int) std::byte espaco[sizeof(int)];
    int* valor = std::construct_at(
        reinterpret_cast<int*>(espaco), 42);
    std::destroy_at(valor);
}
```

**Leitura guiada:** este é código de infraestrutura de baixo nível. `construct_at` inicia o lifetime do `int`; `destroy_at` o encerra. Containers e allocators fazem operações semelhantes; código de aplicação normalmente não deveria fazê-las manualmente.

> **Fonte oficial:** [N4950, Object lifetime, Storage duration e Specialized memory algorithms](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 7.2 RAII e destruição determinística

[⬆️ Voltar ao Sumário](#sumário)

RAII significa adquirir recurso durante inicialização de um objeto e liberá-lo no destructor. A sigla enfatiza aquisição, mas o benefício é ligar ownership ao lifetime.

```cpp
#include <fstream>
#include <string>

void salvar(const std::string& caminho) {
    std::ofstream arquivo{caminho};
    arquivo.exceptions(std::ios::failbit | std::ios::badbit);
    arquivo << "conteúdo\n";
} // arquivo fecha aqui, inclusive se uma operação lançar
```

**Leitura guiada:** não há `close()` manual em cada caminho. O destructor de `ofstream` libera o handle. O estado da stream deve ser verificado ou configurado para lançar; abrir um objeto não garante que todas as operações tiveram sucesso.

> **Fonte oficial:** [N4950, Iostreams e Destructors](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf), [C++ Core Guidelines R.1](https://isocpp.github.io/CppCoreGuidelines/CppCoreGuidelines#Rr-raii)

---

### 7.3 Ownership e handles

[⬆️ Voltar ao Sumário](#sumário)

| Contrato | Representação comum |
|---|---|
| owner exclusivo de objeto | valor direto ou `unique_ptr<T>` |
| ownership compartilhado real | `shared_ptr<T>` |
| observador de shared ownership | `weak_ptr<T>` |
| acesso obrigatório não owning | `T&` |
| acesso opcional não owning | `T*` |
| sequência não owning | `span<T>`, `string_view` |

Não crie `shared_ptr` por hábito: contagem compartilhada custa e torna o lifetime menos local. Um raw pointer pode ser seguro quando é observador e o owner é evidente; documente essa relação.

> **Fonte oficial:** [N4950, Smart pointers](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf), [C++ Core Guidelines R.2–R.37](https://isocpp.github.io/CppCoreGuidelines/CppCoreGuidelines#S-resource)

---

### 7.4 Inicialização estática e ordem

[⬆️ Voltar ao Sumário](#sumário)

Objetos estáticos passam por static initialization e, quando necessário, dynamic initialization. A ordem dinâmica entre unidades de tradução pode ser indeterminadamente sequenciada ou não especificada conforme a categoria.

```cpp
#include <string>

const std::string& nome_aplicacao() {
    static const std::string nome = "Editor";
    return nome;
}
```

**Leitura guiada:** function-local static é inicializado na primeira passagem da declaração; desde C++11, inicialização concorrente é protegida. O objeto vive até o encerramento. Esse padrão reduz o static initialization order fiasco, mas dependências na destruição global ainda pedem cuidado.

Prefira `constexpr`/constant initialization quando possível e evite constructors globais com I/O, threads ou dependências externas.

> **Fonte oficial:** [N4950, Start and termination e Dynamic initialization](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 7.5 Temporários, extensão de lifetime e dangling

[⬆️ Voltar ao Sumário](#sumário)

```cpp
#include <string>
#include <string_view>

const std::string& texto = std::string{"temporário"}; // lifetime estendido

std::string_view ruim = std::string{"já destruída"};  // dangling após ;
```

**Leitura guiada:** uma referência local const ligada diretamente ao temporary pode estender seu lifetime. `string_view` não é referência na regra de extensão; a `string` temporária é destruída no fim da full-expression.

Iteradores e references também ficam dangling após destruição, realocação ou certas operações de container. A validade precisa ser verificada no contrato da operação concreta.

> **Fonte oficial:** [N4950, Temporary objects e Object lifetime](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

## Parte 8 — Controle de Fluxo, Expressões e Operadores

[⬆️ Voltar ao Sumário](#sumário)

Controle de fluxo parece familiar, mas inicializers, conversões contextuais, sequencing e overloads tornam a semântica específica.

---

### 8.1 `if`, `switch` e initializers

[⬆️ Voltar ao Sumário](#sumário)

```cpp
#include <map>
#include <string>

int main() {
    std::map<std::string, int> estoque{{"caneta", 10}};

    if (auto it = estoque.find("caneta"); it != estoque.end()) {
        it->second -= 1;
    }

    switch (const int status = 200; status) {
    case 200:
        break;
    default:
        break;
    }
}
```

**Leitura guiada:** o initializer fica no scope da condição e dos branches, reduzindo lifetime. `switch` não quebra automaticamente após um case; use `break`, `return` ou `[[fallthrough]]` quando a queda for intencional.

> **Fonte oficial:** [N4950, Selection statements](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 8.2 Laços e range-based `for`

[⬆️ Voltar ao Sumário](#sumário)

```cpp
#include <vector>

int main() {
    std::vector<int> valores{1, 2, 3};

    for (int& valor : valores) {
        valor *= 2;
    }

    for (const int valor : valores) {
        if (valor > 4) {
            break;
        }
    }
}
```

**Leitura guiada:** o primeiro loop usa referência para modificar elementos; o segundo copia `int`, o que é barato. Para objetos grandes, `const auto&` evita cópia. Não altere estruturalmente o container de modo que invalide o iterador oculto do loop.

`continue` avança; `break` sai do laço; `goto` existe, mas RAII e funções pequenas normalmente expressam melhor o controle e ainda preservam destruição de scopes atravessados conforme as regras.

> **Fonte oficial:** [N4950, Iteration e Jump statements](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 8.3 Operadores, precedência, sequencing e curto-circuito

[⬆️ Voltar ao Sumário](#sumário)

Precedência define agrupamento sintático; sequencing define ordem de avaliações. Parênteses melhoram leitura, mas não criam uma ordem que a linguagem não garante em todo contexto.

```cpp
struct Recurso {
    [[nodiscard]] bool pronto() const noexcept { return true; }
};

int main() {
    Recurso recurso;
    Recurso* ponteiro = &recurso;
    const bool valido =
        ponteiro != nullptr && ponteiro->pronto();
    (void)valido;
}
```

**Leitura guiada:** `&&` avalia da esquerda para a direita e usa curto-circuito, portanto o acesso só ocorre se o ponteiro não for nulo. `&` é operador bitwise e avaliaria ambos.

Overflow signed, divisão inteira por zero, shift inválido e várias modificações não sequenciadas são fontes clássicas de UB. Use operações checked, limites e tipos adequados nas fronteiras.

> **Fonte oficial:** [N4950, Expressions, Order of evaluation e Operators](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 8.4 Sobrecarga de operadores e `operator<=>`

[⬆️ Voltar ao Sumário](#sumário)

```cpp
#include <compare>

struct Distancia {
    double metros{};

    Distancia& operator+=(Distancia outra) noexcept {
        metros += outra.metros;
        return *this;
    }

    friend Distancia operator+(Distancia a, Distancia b) noexcept {
        return a += b;
    }

    auto operator<=>(const Distancia&) const = default;
};
```

**Leitura guiada:** `operator+` recebe o operando esquerdo por valor e reutiliza `+=`. A comparação defaulted gera operações coerentes conforme os membros. Operadores não mudam precedência, aridade ou curto-circuito; `operator&&` sobrecarregado não preserva o curto-circuito do built-in.

Sobrecarregue quando a semântica for natural e previsível. I/O oculto, mutação surpreendente ou custo enorme merecem método nomeado.

> **Fonte oficial:** [N4950, Overloaded operators e Comparisons](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 8.5 Assertions e atributos de controle

[⬆️ Voltar ao Sumário](#sumário)

| Recurso | Uso |
|---|---|
| `assert(expr)` | invariantes de debug; pode desaparecer com `NDEBUG` |
| `static_assert(expr)` | condição obrigatória em compile time |
| `[[fallthrough]]` | queda intencional entre cases |
| `[[likely]]`, `[[unlikely]]` | expectativa de caminho; não altera resultado |
| `[[assume(expr)]]` | C++23: permite assumir condição; violá-la produz UB |

Não use `assert` para validar input externo ou executar efeito necessário. `[[assume]]` é promessa ao otimizador, não validação.

> **Fonte oficial:** [N4950, Assertions e Attributes](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

## Parte 9 — Categorias de Valor, Cópia e Movimento

[⬆️ Voltar ao Sumário](#sumário)

Categorias de valor classificam expressões, não objetos. Elas controlam binding de referências, overload resolution, materialização de temporários e movimento.

---

### 9.1 lvalue, prvalue, xvalue e glvalue

[⬆️ Voltar ao Sumário](#sumário)

| Categoria | Intuição útil | Exemplo |
|---|---|---|
| lvalue | possui identidade persistente | variável nomeada |
| xvalue | identidade cujo recurso pode ser reutilizado | resultado de `std::move(x)` |
| prvalue | computa/inicializa um valor | `42`, `Tipo{}` |
| glvalue | lvalue ou xvalue; expressão com identidade | ambos acima |
| rvalue | prvalue ou xvalue | temporários e objetos expiring |

```cpp
#include <string>
#include <utility>

std::string texto = "abc";       // `texto` é expressão lvalue
std::string copia = texto;       // copia
std::string movida = std::move(texto); // xvalue permite mover
```

**Leitura guiada:** mesmo que seu tipo seja `std::string`, um nome é lvalue. `std::move` muda a categoria da expressão por cast; não executa transferência sozinho.

> **Fonte oficial:** [N4950, Value categories e Expressions](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 9.2 Copy e move semantics

[⬆️ Voltar ao Sumário](#sumário)

Cópia cria estado independente conforme o contrato do tipo. Movimento transfere ou reutiliza recursos e deixa a origem em estado válido, mas geralmente não especificado.

```cpp
#include <string>
#include <utility>
#include <vector>

int main() {
    std::vector<std::string> origem{"a", "b"};
    auto copia = origem;
    auto destino = std::move(origem);

    origem.clear(); // operação válida
    (void)copia;
    (void)destino;
}
```

**Leitura guiada:** não presuma que `origem` fica vazia; só use operações cujo precondition vale para qualquer estado válido. Atribuir ou destruir a origem é sempre necessário e válido conforme os requisitos usuais do tipo.

Move constructor deve deixar ambos os objetos destrutíveis e costuma ser `noexcept`, permitindo que containers movam durante realocação mantendo guarantees.

> **Fonte oficial:** [N4950, Copy/move constructors e MoveInsertable requirements](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 9.3 `std::move` não move sozinho

[⬆️ Voltar ao Sumário](#sumário)

```cpp
#include <string>
#include <utility>

void consumir(std::string valor);

void exemplo() {
    std::string nome = "Ada";
    consumir(nome);            // copia para o parâmetro
    consumir(std::move(nome)); // constrói o parâmetro por movimento
}
```

**Leitura guiada:** a primeira chamada precisa copiar `nome` porque ele é lvalue; a segunda permite que o constructor de `string` reutilize seus recursos. `std::move` é essencialmente um cast para referência rvalue. Se o destino só aceita `const T&`, continuará não havendo transferência. Não mova retorno local automaticamente: `return local;` já habilita NRVO ou movimento implícito; `return std::move(local);` pode impedir elision.

> **Fonte oficial:** [N4950, `std::move`, Implicit move e Copy elision](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 9.4 Perfect forwarding

[⬆️ Voltar ao Sumário](#sumário)

```cpp
#include <memory>
#include <utility>

template<class T, class... Args>
std::unique_ptr<T> criar(Args&&... args) {
    return std::make_unique<T>(
        std::forward<Args>(args)...);
}
```

**Leitura guiada:** `Args&&` é forwarding reference porque `Args` é deduzido. Reference collapsing preserva se cada argumento chegou como lvalue ou rvalue; `std::forward` reexpressa essa categoria. Fora desse contexto, `T&&` é apenas referência rvalue.

Perfect forwarding pode tornar mensagens de erro e overloads complexos. Use apenas em adaptadores/factories realmente genéricos.

> **Fonte oficial:** [N4950, Reference collapsing, Forwarding references e `forward`](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 9.5 Copy elision, RVO e NRVO

[⬆️ Voltar ao Sumário](#sumário)

```cpp
#include <string>

std::string criar_nome() {
    return std::string{"Grace"}; // guaranteed elision desde C++17
}

std::string criar_titulo() {
    std::string titulo = "Engenheira";
    return titulo; // NRVO permitido; caso contrário, movimento implícito
}
```

**Leitura guiada:** no primeiro retorno, o resultado é inicializado diretamente. NRVO para variável nomeada continua permitido, não obrigatório. Desenhe APIs por valor e meça antes de substituir retornos claros por output parameters.

> **Fonte oficial:** [N4950, Class copy/move e Copy elision](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

## Parte 10 — Enums, Unions e Tipos-Soma

[⬆️ Voltar ao Sumário](#sumário)

Esses recursos representam conjuntos fechados de alternativas. Prefira a forma que mantém o membro ativo e a exhaustividade explícitos.

---

### 10.1 `enum class` e enums não scoped

[⬆️ Voltar ao Sumário](#sumário)

```cpp
#include <cstdint>

enum class Estado : std::uint8_t {
    pendente,
    processando,
    concluido
};

Estado atual = Estado::pendente;
```

**Leitura guiada:** `Estado::pendente` qualifica o enumerator pelo tipo. `enum class` não injeta enumerators no scope externo e não converte implicitamente para inteiro. O underlying type explícito é útil para protocolo/ABI, mas valores externos ainda precisam ser validados antes do cast.

Enums não scoped continuam úteis em interoperabilidade e APIs históricas, porém poluem o scope e permitem conversões mais amplas.

> **Fonte oficial:** [N4950, Enumerations](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 10.2 Flags e operadores bitwise

[⬆️ Voltar ao Sumário](#sumário)

```cpp
#include <cstdint>

enum class Permissao : std::uint8_t {
    nenhuma = 0,
    ler = 1 << 0,
    escrever = 1 << 1
};

constexpr Permissao operator|(Permissao a, Permissao b) noexcept {
    return static_cast<Permissao>(
        static_cast<std::uint8_t>(a) |
        static_cast<std::uint8_t>(b));
}

constexpr auto acesso = Permissao::ler | Permissao::escrever;
```

**Leitura guiada:** cada flag usa um bit. O operador converte de modo explícito ao underlying type e volta ao enum. Defina também testes e operadores necessários; não invente semântica parcial inconsistente.

> **Fonte oficial:** [N4950, Enumerations e Bitwise operators](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 10.3 Unions e membro ativo

[⬆️ Voltar ao Sumário](#sumário)

Union reutiliza o mesmo armazenamento para membros alternativos. Em geral, ler membro que não está ativo não é uma forma portátil de type punning.

```cpp
union Numero {
    int inteiro;
    double real;
};

Numero n{.inteiro = 10};
```

**Leitura guiada:** o designated initializer torna `inteiro` o membro ativo e grava `10` no armazenamento compartilhado. Ler `n.real` em seguida não é uma conversão de `int` para `double`. Para membros não triviais, código manual precisa iniciar e encerrar lifetimes corretamente. Prefira `std::variant` para tipos-soma seguros; use union cru em ABI, embedded ou infraestrutura após compreender as regras.

> **Fonte oficial:** [N4950, Unions e Object lifetime](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 10.4 `variant`, `optional` e `any`

[⬆️ Voltar ao Sumário](#sumário)

| Tipo | Modela |
|---|---|
| `variant<A, B>` | exatamente uma alternativa tipada |
| `optional<T>` | zero ou um `T` |
| `any` | um valor copyable apagado em runtime |

```cpp
#include <iostream>
#include <string>
#include <variant>

using Resultado = std::variant<int, std::string>;

void imprimir(const Resultado& resultado) {
    std::visit([](const auto& valor) {
        std::cout << valor << '\n';
    }, resultado);
}
```

**Leitura guiada:** a lambda genérica é instanciada para cada alternativa. `std::get_if` consulta sem lançar; `std::get` lança `bad_variant_access` se a alternativa estiver errada. `variant` pode ficar `valueless_by_exception` em situações específicas.

Use `optional` para ausência esperada, não para esconder motivo de falha. Use `any` apenas quando o conjunto aberto for requisito real; ele troca exhaustividade por flexibilidade.

> **Fonte oficial:** [N4950, `variant`, `optional` e `any`](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

## Parte 11 — Modelagem de Tipos e Semântica de Valor

[⬆️ Voltar ao Sumário](#sumário)

Um bom tipo torna estados inválidos difíceis de representar e oferece operações coerentes com seu domínio.

---

### 11.1 Invariantes e tipos fortes

[⬆️ Voltar ao Sumário](#sumário)

```cpp
#include <stdexcept>

class Percentual {
public:
    explicit Percentual(double valor) : valor_(valor) {
        if (valor < 0.0 || valor > 100.0) {
            throw std::out_of_range{"percentual fora de 0..100"};
        }
    }

    [[nodiscard]] double valor() const noexcept { return valor_; }

private:
    double valor_;
};
```

**Leitura guiada:** depois de construir, toda instância satisfaz o intervalo. `explicit` evita que qualquer `double` se transforme silenciosamente em percentual. Para sistemas sem exceptions, uma factory pode retornar `expected<Percentual, Erro>`.

Aliases como `using Percentual = double` documentam, mas não impedem mistura. Wrapper nominal oferece type safety.

> **Fonte oficial:** [C++ Core Guidelines I.4, C.2 e Enum rules](https://isocpp.github.io/CppCoreGuidelines/CppCoreGuidelines#Ri-typed), [N4950, Classes](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 11.2 Igualdade, ordenação e hash

[⬆️ Voltar ao Sumário](#sumário)

```cpp
#include <compare>
#include <string>

struct Produto {
    int id;
    std::string nome;

    bool operator==(const Produto&) const = default;
    auto operator<=>(const Produto&) const = default;
};
```

**Leitura guiada:** comparação defaulted segue os membros em ordem. Se identidade de negócio for apenas `id`, escrever comparação manual pode ser correto, mas então ordering e hash precisam usar o mesmo conceito quando o container exige coerência.

Para chave em `unordered_map`, `a == b` deve implicar hashes iguais. Especialize `std::hash<T>` apenas para tipo definido pelo programa e conforme as permissões do padrão, ou forneça functor de hash ao container.

> **Fonte oficial:** [N4950, Comparisons e Unordered associative requirements](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 11.3 Builders, named parameter idiom e factories

[⬆️ Voltar ao Sumário](#sumário)

Quando muitos parâmetros opcionais têm o mesmo tipo, um options struct ou builder torna chamadas legíveis.

```cpp
#include <chrono>
#include <string>
#include <utility>

struct ClienteOpcoes {
    std::string host;
    std::chrono::milliseconds timeout{2'000};
    int tentativas{3};
};

class Cliente {
public:
    explicit Cliente(ClienteOpcoes opcoes) : opcoes_{std::move(opcoes)} {}

private:
    ClienteOpcoes opcoes_;
};

int main() {
    Cliente cliente{{.host = "api.exemplo", .tentativas = 5}};
}
```

**Leitura guiada:** os defaults ficam centralizados e designated initializers nomeiam decisões. A ordem segue a declaração. Factory é preferível quando construção pode falhar sem exception, precisa escolher implementação ou reutilizar instância.

> **Fonte oficial:** [C++ Core Guidelines C.40, C.50 e F.2](https://isocpp.github.io/CppCoreGuidelines/CppCoreGuidelines), [N4950, Initialization](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 11.4 PImpl e estabilidade de dependências

[⬆️ Voltar ao Sumário](#sumário)

PImpl armazena implementação atrás de ponteiro para tipo incompleto. Ele reduz rebuilds e exposição de layout, mas adiciona alocação/indireção e exige atenção ao destructor.

```cpp
// editor.hpp
#include <memory>

class Editor {
public:
    Editor();
    ~Editor();
    Editor(Editor&&) noexcept;
    Editor& operator=(Editor&&) noexcept;

private:
    struct Impl;
    std::unique_ptr<Impl> impl_;
};
```

**Leitura guiada:** `Impl` é incompleta no header. O destructor de `Editor` é definido na fonte onde `Impl` está completa, permitindo que `unique_ptr` destrua corretamente. Decida política de cópia explicitamente.

PImpl não cria ABI estável sozinho: calling convention, exceptions, allocator, standard library e toolchain ainda importam.

> **Fonte oficial:** [C++ Core Guidelines I.27](https://isocpp.github.io/CppCoreGuidelines/CppCoreGuidelines#Ri-pimpl), [N4950, Incomplete types e `unique_ptr`](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

## Parte 12 — Herança, Polimorfismo e Composição

[⬆️ Voltar ao Sumário](#sumário)

C++ oferece polimorfismo dinâmico, estático e por type erasure. Escolha pelo contrato e pelo ownership, não por preferência abstrata.

---

### 12.1 Herança, `virtual`, `override` e `final`

[⬆️ Voltar ao Sumário](#sumário)

```cpp
#include <string>

class Forma {
public:
    virtual ~Forma() = default;
    [[nodiscard]] virtual double area() const = 0;
};

class Circulo final : public Forma {
public:
    explicit Circulo(double raio) : raio_(raio) {}
    double area() const override { return 3.141592653589793 * raio_ * raio_; }
private:
    double raio_;
};
```

**Leitura guiada:** método pure virtual torna `Forma` abstrata. `override` pede ao compilador confirmar sobrescrita. `final` impede subclasses de `Circulo`. Herança pública modela substituibilidade “é um”.

> **Fonte oficial:** [N4950, Derived classes e Virtual functions](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 12.2 Interfaces abstratas e destrutores virtuais

[⬆️ Voltar ao Sumário](#sumário)

Se objetos serão destruídos por ponteiro para base, o destructor base precisa ser virtual e acessível. Alternativamente, torne-o protegido não virtual quando destruição polimórfica não faz parte do contrato.

```cpp
// Considerando Forma e Circulo declarados na seção anterior:
#include <memory>

std::unique_ptr<Forma> forma = std::make_unique<Circulo>(2.0);
```

**Leitura guiada:** `make_unique` constrói `Circulo`, mas o owner é convertido para `unique_ptr<Forma>`. Ao sair de scope, ele chama `delete` pela base; o destructor virtual garante a cadeia correta. Evite dados públicos na interface e preserve operações pequenas, estáveis e independentes de tipos concretos.

> **Fonte oficial:** [N4950, Virtual destructors](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf), [C++ Core Guidelines C.35](https://isocpp.github.io/CppCoreGuidelines/CppCoreGuidelines#Rc-dtor-virtual)

---

### 12.3 Slicing, downcast e RTTI

[⬆️ Voltar ao Sumário](#sumário)

Passar derivado por valor como base remove a parte derivada: object slicing. Use referência ou ponteiro para polimorfismo.

```cpp
void desenhar(const Forma& forma) {
    (void)forma.area();
}

void analisar(Forma& forma_base) {
    if (auto* circulo = dynamic_cast<Circulo*>(&forma_base)) {
        (void)circulo->area();
    }
}
```

**Leitura guiada:** `dynamic_cast` de ponteiro devolve `nullptr` quando falha; de referência lança `std::bad_cast`. O tipo base precisa ser polimórfico. Downcasts frequentes indicam que uma operação virtual, variant ou visitor talvez expresse melhor o design.

RTTI inclui `dynamic_cast` e `typeid`; toolchains podem oferecer flags para removê-lo, mas isso altera capacidades e deve ser decisão arquitetural.

> **Fonte oficial:** [N4950, Dynamic cast e Type identification](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 12.4 Herança múltipla e virtual

[⬆️ Voltar ao Sumário](#sumário)

C++ permite várias bases. Herdar múltiplas interfaces puras costuma ser simples; herdar várias implementações com estado pode criar ambiguidade e diamond.

```cpp
struct Serializavel {
    virtual ~Serializavel() = default;
    virtual void salvar() const = 0;
};

struct Observavel {
    virtual ~Observavel() = default;
    virtual void observar() = 0;
};

struct DocumentoFinal : Serializavel, Observavel {
    void salvar() const override {}
    void observar() override {}
};
```

**Leitura guiada:** `DocumentoFinal` deve implementar as duas operações abstratas e possui dois subobjetos-base independentes. O exemplo não forma um diamond; se duas bases herdassem de uma terceira base comum, virtual inheritance poderia garantir uma única instância compartilhada, ao custo de maior complexidade de construção e layout. Use-a quando o modelo realmente exige identidade compartilhada; composição costuma ser mais clara.

> **Fonte oficial:** [N4950, Multiple base classes e Virtual base classes](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 12.5 Composição, type erasure e CRTP

[⬆️ Voltar ao Sumário](#sumário)

| Técnica | Binding | Vantagem | Custo/risco |
|---|---|---|---|
| virtual interface | runtime | ABI e substituição explícitas | indireção e lifetime polimórfico |
| template/concept | compile time | otimização e contrato estático | instanciações e exposição no header |
| type erasure | runtime | guarda tipos heterogêneos sem base comum | wrapper/cópia/alocação conforme tipo |
| CRTP | compile time | customização estática | casts e acoplamento ao padrão |
| composição | direto | ownership e dependência explícitos | delegação manual |

`std::function` é type erasure de callables; `variant` é conjunto fechado; interface virtual é conjunto aberto nominal. Não use CRTP apenas para evitar uma chamada virtual sem medir.

> **Fonte oficial:** [N4950, Templates, Virtual functions e Function wrappers](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf), [C++ Core Guidelines C.120–C.139](https://isocpp.github.io/CppCoreGuidelines/CppCoreGuidelines#S-hier)

---

## Parte 13 — Callables, Lambdas e Callbacks

[⬆️ Voltar ao Sumário](#sumário)

Callable é qualquer entidade invocável: função, function pointer, objeto com `operator()`, lambda ou pointer-to-member usado por `std::invoke`.

---

### 13.1 Lambdas e capturas

[⬆️ Voltar ao Sumário](#sumário)

```cpp
#include <algorithm>
#include <vector>

std::vector<int> valores{1, 5, 8, 3};
const int limite = 4;

const auto maiores = std::count_if(
    valores.begin(), valores.end(),
    [limite](int valor) { return valor > limite; });
```

**Leitura guiada:** `[limite]` copia o valor para o closure object. `[&limite]` guardaria referência e exigiria lifetime. `[]` não captura. `[=]` e `[&]` amplos são convenientes, mas escondem dependências e podem capturar `this` de modo surpreendente.

Cada expressão lambda possui tipo closure único e não nomeado; `auto` preserva esse tipo sem type erasure.

> **Fonte oficial:** [N4950, Lambda expressions](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 13.2 Lambdas genéricas, `mutable` e lifetime

[⬆️ Voltar ao Sumário](#sumário)

```cpp
auto somar = []<class A, class B>(A a, B b) {
    return a + b;
};

auto contador = [n = 0]() mutable {
    return ++n;
};
```

**Leitura guiada:** a primeira lambda possui template parameter list explícita; cada combinação válida instancia `operator()`. A segunda altera a cópia capturada porque `mutable` remove o `const` implícito do call operator; a variável externa não existe.

Nunca retorne lambda que captura por referência variáveis locais já encerradas. Capturar `this` guarda ponteiro, não prolonga lifetime do objeto; para callback assíncrono, modele ownership conscientemente.

> **Fonte oficial:** [N4950, Lambda captures e Generic lambdas](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 13.3 `std::invoke`, `function` e `move_only_function`

[⬆️ Voltar ao Sumário](#sumário)

| Wrapper | Contrato |
|---|---|
| template parameter | sem type erasure; melhor para callable conhecido no call site |
| function pointer | função sem estado compatível |
| `std::function<R(Args...)>` | callable copyable com type erasure |
| `std::move_only_function` | C++23: callable move-only com qualifiers mais precisos |
| `std::function_ref` | C++26 draft: referência não owning a callable |

```cpp
#include <functional>
#include <iostream>

struct Alvo {
    void mostrar(int x) const { std::cout << x << '\n'; }
};

int main() {
    Alvo alvo;
    std::invoke(&Alvo::mostrar, alvo, 42);
}
```

**Leitura guiada:** `invoke` uniformiza chamada de funções, functors e pointers-to-member. `std::function` pode alocar conforme callable/implementação; não escolha por reflexo em loops quentes.

> **Fonte oficial:** [N4950, Function objects e Move-only function wrappers](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf), [N5050, `function_ref`](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2026/n5050.pdf)

---

### 13.4 Binding e adaptação

[⬆️ Voltar ao Sumário](#sumário)

Prefira lambda a `std::bind` para adaptação visível. `std::bind_front` e `bind_back` ligam argumentos nas extremidades sem placeholders; `std::not_fn` nega um predicate; `std::mem_fn` adapta pointer-to-member.

```cpp
#include <functional>

constexpr int elevar(int base, int expoente) {
    int resultado = 1;
    for (int i = 0; i < expoente; ++i) {
        resultado *= base;
    }
    return resultado;
}

int main() {
    auto quadrado = std::bind_back(elevar, 2); // C++23
    return quadrado(5) == 25 ? 0 : 1;
}
```

**Leitura guiada:** `quadrado(5)` chama `elevar(5, 2)`. Confirme suporte de `bind_back` na biblioteca C++23 instalada; uma lambda `[ ](int x){ return elevar(x, 2); }` é igualmente clara e mais amplamente disponível.

> **Fonte oficial:** [N4950, Function object binders e `bind_back`](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 13.5 Callbacks e Observer

[⬆️ Voltar ao Sumário](#sumário)

Callback síncrono pode receber callable por template ou view não owning durante a chamada. Callback armazenado precisa definir cópia/movimento, thread-safety, cancelamento e lifetime.

```cpp
#include <functional>
#include <utility>
#include <vector>

class Eventos {
public:
    using Handler = std::function<void(int)>;
    void assinar(Handler h) { handlers_.push_back(std::move(h)); }
    void emitir(int valor) const {
        for (const auto& h : handlers_) h(valor);
    }
private:
    std::vector<Handler> handlers_;
};
```

**Leitura guiada:** o publisher possui cópias/movimentos dos handlers. Falta ainda um token de unsubscribe e política para exceções/reentrância; produção precisa decidir ambos. Capturas por referência podem sobreviver menos que `Eventos`.

> **Fonte oficial:** [N4950, Function wrappers e Containers](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

## Parte 14 — Iteradores, Algoritmos e Ranges

[⬆️ Voltar ao Sumário](#sumário)

Algoritmos padronizam operações e explicitam requisitos. Ranges C++20 permitem passar ranges diretamente, usar projections e compor views lazy.

---

### 14.1 Iteradores, sentinels e categorias

[⬆️ Voltar ao Sumário](#sumário)

| Categoria/concept | Capacidades adicionais |
|---|---|
| input/output | leitura ou escrita em passagem |
| forward | múltiplas passagens |
| bidirectional | decremento |
| random access | salto e distância constante |
| contiguous | elementos contíguos na memória |

Sentinel marca fim e pode ter tipo diferente do iterador. Um par `[first, last)` inclui o primeiro e exclui o último; `last` não é dereferenciado.

> **Fonte oficial:** [N4950, Iterator library e Iterator concepts](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 14.2 Algoritmos clássicos

[⬆️ Voltar ao Sumário](#sumário)

```cpp
#include <algorithm>
#include <iostream>
#include <numeric>
#include <vector>

int main() {
    std::vector<int> valores{4, 1, 3, 1};
    std::sort(valores.begin(), valores.end());
    valores.erase(
        std::unique(valores.begin(), valores.end()),
        valores.end());

    std::cout << std::accumulate(valores.begin(), valores.end(), 0) << '\n';
}
```

**Leitura guiada:** `unique` move duplicatas adjacentes para o fim lógico e devolve novo fim; não reduz o container. Ordenar antes aproxima duplicatas. C++20 oferece `std::erase`/`erase_if` para vários casos mais simples.

Antes de escrever busca, sort, partition, set operation ou heap manual, consulte `<algorithm>` e `<numeric>`.

> **Fonte oficial:** [N4950, Algorithms e Numerics library](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 14.3 Ranges e projections

[⬆️ Voltar ao Sumário](#sumário)

```cpp
#include <algorithm>
#include <string>
#include <vector>

struct Pessoa {
    std::string nome;
    int idade;
};

int main() {
    std::vector<Pessoa> pessoas{{"Bia", 30}, {"Ana", 25}};
    std::ranges::sort(pessoas, {}, &Pessoa::nome);
}
```

**Leitura guiada:** a projection `&Pessoa::nome` extrai a chave; `{}` usa o comparator padrão. Isso evita lambda repetitiva e mantém algoritmo separado da projeção.

Ranges algorithms geralmente devolvem iteradores/result structs e protegem alguns temporários com `dangling`, mas não eliminam todo erro de lifetime.

> **Fonte oficial:** [N4950, Ranges library e Constrained algorithms](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 14.4 Views, lazy evaluation e dangling

[⬆️ Voltar ao Sumário](#sumário)

```cpp
#include <iostream>
#include <ranges>
#include <vector>

int main() {
    std::vector<int> valores{1, 2, 3, 4, 5, 6};
    auto pares_quadrados =
        valores
        | std::views::filter([](int x) { return x % 2 == 0; })
        | std::views::transform([](int x) { return x * x; });

    for (int x : pares_quadrados) {
        std::cout << x << '\n';
    }
}
```

**Leitura guiada:** a pipeline não cria `vector` intermediário; cada valor é filtrado e transformado durante iteração. A view normalmente referencia `valores`; destruí-lo ou invalidar seu armazenamento pode tornar a pipeline inválida.

Views podem cachear estado e não são automaticamente thread-safe ou multipass. Conheça `view`, `borrowed_range` e categoria do iterador.

> **Fonte oficial:** [N4950, Range adaptors e Views](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 14.5 Algoritmos paralelos

[⬆️ Voltar ao Sumário](#sumário)

Execution policies (`seq`, `par`, `par_unseq`, `unseq`) autorizam estratégias distintas. O callable precisa respeitar restrições de data race, exceções, vetorização e funções bloqueantes da policy.

```cpp
#include <algorithm>
#include <execution>
#include <vector>

int main() {
    std::vector<double> dados(1'000'000, 2.0);
    std::transform(std::execution::par,
                   dados.begin(), dados.end(), dados.begin(),
                   [](double x) { return x * x; });
}
```

**Leitura guiada:** `par` permite que diferentes elementos sejam transformados em threads distintas. Cada invocação lê e escreve somente sua posição, evitando conflito entre elementos. Isso não garante speedup: overhead, tamanho, implementação e hardware importam, e algumas implementações exigem backend adicional. Meça e ofereça fallback.

> **Fonte oficial:** [N4950, Execution policies e Parallel algorithms](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

## Parte 15 — Containers da Biblioteca Padrão

[⬆️ Voltar ao Sumário](#sumário)

Escolha por padrão de acesso, estabilidade, complexidade, layout e invalidação — não pelo nome familiar.

---

### 15.1 Sequenciais

[⬆️ Voltar ao Sumário](#sumário)

| Container | Pontos fortes | Cuidado |
|---|---|---|
| `array<T,N>` | tamanho compile time, inline | tamanho fixo |
| `vector<T>` | contíguo, cache-friendly, escolha padrão | realocação invalida pointers/iterators |
| `deque<T>` | crescimento nas pontas, acesso indexado | não contíguo |
| `list<T>` | splice e iteradores estáveis | alocações, overhead e baixa localidade |
| `forward_list<T>` | lista singly-linked mínima | API e acesso restritos |

```cpp
#include <vector>

int main() {
    std::vector<int> ids;
    ids.reserve(1'000);
    ids.push_back(10);
    ids.emplace_back(20);
}
```

**Leitura guiada:** `reserve` solicita capacidade para ao menos mil elementos, mas o tamanho continua zero. As duas inserções elevam o tamanho para dois. `emplace_back` constrói no container, mas não é sempre melhor que `push_back`; para valor já existente, `push_back` comunica melhor.

> **Fonte oficial:** [N4950, Sequence containers](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 15.2 Associativos ordenados

[⬆️ Voltar ao Sumário](#sumário)

`set`/`map` mantêm chaves ordenadas e operações logarítmicas; variantes `multi` aceitam equivalentes. Comparator define equivalência por `!comp(a,b) && !comp(b,a)`, que pode diferir de `==`.

```cpp
#include <map>
#include <string>

int main() {
    std::map<std::string, int, std::less<>> estoque;
    estoque.try_emplace("caneta", 10);

    if (auto it = estoque.find("caneta"); it != estoque.end()) {
        it->second += 1;
    }
}
```

**Leitura guiada:** `try_emplace` insere somente se a chave não existir; `find` devolve iterator para o par e `it->second` acessa o valor. `std::less<>` transparente pode permitir lookup heterogêneo com tipos compatíveis, evitando criar string temporária. `operator[]` insere valor default quando a chave falta; use `at`, `find` ou `try_emplace` conforme intenção.

> **Fonte oficial:** [N4950, Associative containers](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 15.3 Associativos unordered

[⬆️ Voltar ao Sumário](#sumário)

`unordered_map`/`unordered_set` usam hash e igualdade; complexidade média é constante, pior caso linear. Rehash invalida iteradores, mas referências/pointers para elementos seguem regras específicas.

```cpp
#include <string>
#include <unordered_map>

int main() {
    std::unordered_map<std::string, int> acessos;
    acessos.reserve(500);
    ++acessos["rota"];
}
```

**Leitura guiada:** `reserve(500)` prepara buckets para reduzir rehashes esperados; não cria 500 pares. `operator[]` insere `"rota"` com `int{}` quando ausente e depois incrementa. Hash não é criptografia e não deve ser persistido como formato estável sem contrato próprio. Input hostil pode explorar colisões conforme implementação; avalie o threat model.

> **Fonte oficial:** [N4950, Unordered associative containers](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 15.4 Adaptadores e containers não owning

[⬆️ Voltar ao Sumário](#sumário)

| Tipo | Uso |
|---|---|
| `stack` | LIFO sobre container subjacente |
| `queue` | FIFO |
| `priority_queue` | maior/prioritário no topo conforme comparator |
| `span` | view contígua |
| `mdspan` | C++23: view multidimensional |
| `string_view` | view de caracteres |

Adaptadores restringem a API para comunicar disciplina. `priority_queue` não oferece decrease-key nem iteração ordenada; algoritmos específicos podem pedir estrutura externa.

> **Fonte oficial:** [N4950, Container adaptors, `span` e `mdspan`](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 15.5 Invalidação, complexidade e escolha

[⬆️ Voltar ao Sumário](#sumário)

| Necessidade dominante | Escolha inicial |
|---|---|
| sequência geral | `vector` |
| fila dupla | `deque` |
| chave ordenada/range queries | `map`/`set` |
| lookup médio por hash | `unordered_map`/`unordered_set` |
| tamanho fixo pequeno | `array` |
| alternativa discriminada | `variant` |
| fila por prioridade | `priority_queue` |

Após qualquer inserção, erase, reserve, swap ou move, consulte a regra do container/operação antes de reutilizar iterator, pointer, reference ou view. Complexidade assintótica não substitui medição de localidade e alocação.

> **Fonte oficial:** [N4950, Container requirements](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 15.6 `pmr` e containers modernos

[⬆️ Voltar ao Sumário](#sumário)

`std::pmr` separa política de alocação em runtime por `memory_resource`. C++23 adiciona `flat_map`/`flat_set`, adaptadores ordenados sobre armazenamento sequencial; o draft C++26 contém `inplace_vector` e `hive`.

```cpp
#include <array>
#include <cstddef>
#include <memory_resource>
#include <vector>

int main() {
    std::array<std::byte, 4096> buffer{};
    std::pmr::monotonic_buffer_resource arena{
        buffer.data(), buffer.size()};
    std::pmr::vector<int> valores{&arena};
    valores.push_back(42);
}
```

**Leitura guiada:** o vector obtém storage da arena; `monotonic_buffer_resource` libera em bloco e não recupera cada allocation individualmente. O resource precisa sobreviver ao container. Não escolha PMR sem perfil de alocação e lifetime.

> **Fonte oficial:** [N4950, Memory resource e Flat associative containers](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf), [N5050, Containers library](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2026/n5050.pdf)

---

## Parte 16 — Erros, Exceções e Garantias

[⬆️ Voltar ao Sumário](#sumário)

Erros de programação, falhas esperadas e falhas excepcionais não são a mesma coisa. A API deve definir canal, contexto e estado após falha.

---

### 16.1 Exceções e stack unwinding

[⬆️ Voltar ao Sumário](#sumário)

```cpp
#include <stdexcept>
#include <string>

int porta(const std::string& texto) {
    const int valor = std::stoi(texto);
    if (valor < 1 || valor > 65'535) {
        throw std::out_of_range{"porta inválida"};
    }
    return valor;
}
```

**Leitura guiada:** `stoi` pode lançar `invalid_argument` ou `out_of_range`; a validação do domínio lança `out_of_range`. Durante propagação, objetos automáticos completamente construídos são destruídos. Capture por `const&` e do específico para o geral.

Destructor não deve deixar exception escapar durante unwinding; uma segunda exception provoca `terminate`.

> **Fonte oficial:** [N4950, Exception handling e Stack unwinding](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 16.2 `noexcept` e terminação

[⬆️ Voltar ao Sumário](#sumário)

```cpp
class Buffer {
public:
    Buffer(Buffer&&) noexcept = default;
    Buffer& operator=(Buffer&&) noexcept = default;
};
```

**Leitura guiada:** `= default` pede as operações geradas pelo compilador; `noexcept` declara que elas não deixam exception escapar. Essa especificação faz parte do tipo de função em contextos determinados e pode orientar containers. Se uma exception escapar, chama-se `std::terminate`; não há fallback para catch externo.

Use `noexcept(expr)` e traits em generic code quando a operação depende do tipo. Destructors são implicitamente non-throwing salvo condições específicas; falhas de cleanup precisam de outra estratégia.

> **Fonte oficial:** [N4950, Exception specifications e Termination](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 16.3 `error_code`, `optional` e `expected`

[⬆️ Voltar ao Sumário](#sumário)

| Canal | Indicado para |
|---|---|
| exception | falha que impede produzir resultado e deve propagar |
| `expected<T,E>` | sucesso/erro como valor explícito |
| `optional<T>` | ausência sem detalhe |
| `error_code` | ecossistemas de códigos/categorias, frequentemente SO |
| status enum | domínio fechado simples |

```cpp
#include <charconv>
#include <expected>
#include <string_view>

enum class ErroNumero { vazio, invalido };

std::expected<int, ErroNumero> converter(std::string_view texto) {
    if (texto.empty()) return std::unexpected{ErroNumero::vazio};
    int valor{};
    auto [fim, erro] =
        std::from_chars(texto.data(), texto.data() + texto.size(), valor);
    if (erro != std::errc{} || fim != texto.data() + texto.size()) {
        return std::unexpected{ErroNumero::invalido};
    }
    return valor;
}
```

**Leitura guiada:** `expected` C++23 obriga o caller a distinguir valor e erro. `from_chars` não lança. O erro de domínio é pequeno e fechado; sistemas reais podem carregar posição/contexto sem expor segredo.

> **Fonte oficial:** [N4950, `expected`, `optional` e System error support](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 16.4 Garantias de exception safety

[⬆️ Voltar ao Sumário](#sumário)

| Garantia | Estado após falha |
|---|---|
| no-throw | operação não falha por exception |
| strong | sem efeito observável, como transação |
| basic | invariantes preservados, estado pode mudar |
| none | contrato não promete estado útil |

O padrão copy-and-swap pode oferecer strong guarantee, mas custa cópia/alocação. Construa novo estado antes de publicar, use RAII para recursos intermediários e documente invalidation.

> **Fonte oficial:** [N4950, Library-wide requirements e exception safety guarantees](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf), [C++ Core Guidelines E.12–E.19](https://isocpp.github.io/CppCoreGuidelines/CppCoreGuidelines#Re-raii)

---

### 16.5 Assertions, precondições e Contracts

[⬆️ Voltar ao Sumário](#sumário)

Precondition define o que caller deve fornecer; postcondition, o resultado prometido; invariant, o que permanece verdadeiro. No C++23, expresse por tipos, validação, `assert`, documentação e análise.

O working draft final C++26 N5050 contém contract assertions e biblioteca de tratamento de violation. A sintaxe, modos de avaliação e suporte de compiladores devem ser verificados no draft/toolchain exatos antes do uso; não retroporte como macro com semântica inventada.

> **Fonte oficial:** [N5050, Contract assertions e Contract-violation handling](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2026/n5050.pdf), [N4950, Assertions](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

## Parte 17 — Templates, Concepts e Programação Genérica

[⬆️ Voltar ao Sumário](#sumário)

Templates descrevem famílias de entidades. Instanciação gera especializações; concepts tornam requisitos parte da interface e melhoram overload resolution e diagnóstico.

---

### 17.1 Function, class, alias e variable templates

[⬆️ Voltar ao Sumário](#sumário)

```cpp
#include <cstddef>

template<class T>
T maximo(T a, T b) {
    return b < a ? a : b;
}

template<class T, std::size_t N>
struct Bloco {
    T dados[N]{};
};

template<class T>
using Ponteiro = T*;

template<class T>
inline constexpr bool sempre_falso = false;
```

**Leitura guiada:** `T` é type parameter; `N` é constant template parameter. Alias template não cria tipo distinto. Variable template possui uma especialização por argumentos e `inline` evita problemas de ODR em header.

> **Fonte oficial:** [N4950, Templates e Template parameters](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 17.2 Dedução, CTAD e deduction guides

[⬆️ Voltar ao Sumário](#sumário)

```cpp
#include <utility>

std::pair par{42, 3.5}; // pair<int, double>

template<class T>
struct Caixa {
    T valor;
};

Caixa caixa{10}; // aggregate CTAD quando aplicável
```

**Leitura guiada:** `std::pair` deduz `int` e `double` a partir dos dois initializers. Para o aggregate `Caixa`, a implementação C++20 ou posterior pode formar um candidato de dedução implícito e obter `Caixa<int>`. Function templates deduzem por argumentos; class template argument deduction usa constructors e deduction guides. `auto` e CTAD podem esconder tipos relevantes à API; exponha explicitamente quando conversão, signedness ou ownership importarem.

Um guide customizado influencia apenas dedução, não cria constructor.

> **Fonte oficial:** [N4950, Deducing template arguments e Deduction guides](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 17.3 Especialização e instanciação

[⬆️ Voltar ao Sumário](#sumário)

Class templates aceitam partial specialization; function templates não — para comportamento de função, overloads/concepts costumam ser melhores. Explicit specialization deve respeitar namespace e visibilidade; explicit instantiation pode reduzir repetição de codegen/build.

```cpp
template<class T>
struct EhPonteiro { static constexpr bool valor = false; };

template<class T>
struct EhPonteiro<T*> { static constexpr bool valor = true; };
```

**Leitura guiada:** o template primário responde `false`; a partial specialization casa qualquer `T*` e responde `true`. Ela não casa referências, smart pointers ou arrays. Antes de escrever trait manual, consulte `<type_traits>`. Especialização de templates em `std` só é permitida em casos expressamente autorizados.

> **Fonte oficial:** [N4950, Template specialization e Instantiation](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 17.4 Variadic templates e fold expressions

[⬆️ Voltar ao Sumário](#sumário)

```cpp
#include <iostream>

template<class... Ts>
void imprimir_linha(const Ts&... valores) {
    ((std::cout << valores << ' '), ...);
    std::cout << '\n';
}
```

**Leitura guiada:** `Ts...` é parameter pack. O fold sobre vírgula expande uma escrita por argumento em ordem. Operadores diferentes têm identidades e associatividade distintas; pack vazio só é válido quando a forma possui regra adequada.

Variadics são base de tuples, factories e formatting, mas uma interface runtime simples não deve virar template sem necessidade.

> **Fonte oficial:** [N4950, Variadic templates, Pack expansions e Fold expressions](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 17.5 Concepts e requires-expressions

[⬆️ Voltar ao Sumário](#sumário)

```cpp
#include <concepts>

template<class T>
concept Somavel = requires(T a, T b) {
    { a + b } -> std::same_as<T>;
};

template<Somavel T>
T somar(T a, T b) {
    return a + b;
}
```

**Leitura guiada:** a requires-expression verifica que `a + b` é válido e devolve exatamente `T`. Ela não executa a expressão. Concepts devem expressar semântica, não só sintaxe; standard concepts incluem igualdade e regularidade com requisitos não verificáveis mecanicamente.

Constraints participam da seleção de overloads e subsumption. Prefira concepts nomeados quando o requisito se repete.

> **Fonte oficial:** [N4950, Constraints and concepts e Concepts library](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 17.6 SFINAE, nomes dependentes e two-phase lookup

[⬆️ Voltar ao Sumário](#sumário)

Em templates, significado pode depender dos argumentos. `typename` informa que nome dependente é tipo; `template` desambigua template member. Lookup ocorre na definição e na instanciação conforme as regras.

SFINAE remove determinadas substituições inválidas do overload set; não transforma todo erro interno em candidato descartado. Concepts substituem muitos usos de `enable_if`, mas compreender SFINAE continua necessário em código legado e implementação de biblioteca.

```cpp
template<class Container>
typename Container::value_type primeiro(const Container& c) {
    return *c.begin();
}
```

**Leitura guiada:** sem `typename`, o parser não pode presumir que `Container::value_type` nomeia tipo. O corpo ainda exige que `begin()` exista; concept poderia publicar esse contrato melhor.

> **Fonte oficial:** [N4950, Name resolution, Dependent names e SFINAE](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

## Parte 18 — Programação em Compile Time e Metaprogramação

[⬆️ Voltar ao Sumário](#sumário)

Compile time pode validar, calcular e gerar estrutura. O objetivo é melhorar contrato ou custo total, não transferir complexidade sem medida para o compilador.

---

### 18.1 `constexpr`, `consteval` e `constinit`

[⬆️ Voltar ao Sumário](#sumário)

```cpp
constexpr int fatorial(int n) {
    int resultado = 1;
    for (int i = 2; i <= n; ++i) resultado *= i;
    return resultado;
}

consteval int id_compilado(int x) { return x * 10; }

inline constinit int contador_global = 0;

static_assert(fatorial(5) == 120);
constexpr int id = id_compilado(7);
```

**Leitura guiada:** `constexpr` permite runtime também; `consteval` exige avaliação imediata; `constinit` garante static initialization, mas não torna variável const. Constant evaluation rejeita operações proibidas ou UB naquele caminho.

> **Fonte oficial:** [N4950, Constant expressions, `constexpr`, `consteval` e `constinit`](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 18.2 Type traits e transformação de tipos

[⬆️ Voltar ao Sumário](#sumário)

`<type_traits>` consulta e transforma tipos: `is_integral`, `is_trivially_copyable`, `remove_cvref`, `common_type`, `invoke_result` e outros.

```cpp
#include <type_traits>

template<class T>
using ValorBase = std::remove_cvref_t<T>;

static_assert(std::is_same_v<ValorBase<const int&>, int>);
```

**Leitura guiada:** `remove_cvref_t` remove primeiro referência e depois qualifiers `const`/`volatile`, por isso `const int&` resulta em `int`. Traits não autorizam uma operação só porque o layout “parece simples”. Use o trait exatamente ligado ao requisito, como `is_trivially_copyable` para certas cópias de bytes, e ainda respeite lifetime/alinhamento.

> **Fonte oficial:** [N4950, Metaprogramming and type traits](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 18.3 Metaprogramação, recursion e tabelas compiladas

[⬆️ Voltar ao Sumário](#sumário)

Metaprogramação moderna prefere `constexpr` functions, loops, arrays e concepts a recursion de templates quando ambas resolvem o problema.

```cpp
#include <array>
#include <cstddef>

constexpr auto quadrados() {
    std::array<int, 8> tabela{};
    for (std::size_t i = 0; i < tabela.size(); ++i) {
        tabela[i] = static_cast<int>(i * i);
    }
    return tabela;
}

inline constexpr auto tabela_quadrados = quadrados();
```

**Leitura guiada:** o loop é avaliado em compile time porque inicializa variável `constexpr`. Isso evita initialization order dinâmica. Tabelas enormes podem aumentar build/binário; compare com cálculo runtime/cache.

> **Fonte oficial:** [N4950, Constant evaluation e `array`](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 18.4 Feature-test macros

[⬆️ Voltar ao Sumário](#sumário)

```cpp
#include <version>

#if defined(__cpp_lib_expected) && __cpp_lib_expected >= 202202L
// std::expected está disponível no nível exigido.
#else
#error "Este projeto exige std::expected do C++23"
#endif
```

**Leitura guiada:** macros de biblioteca podem vir de `<version>` ou do header correspondente. O valor codifica revisão, não apenas presença. Não use apenas `__cplusplus`: uma implementação pode ter suporte parcial ou extensão.

> **Fonte oficial:** [N4950, Feature-test recommendations](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf), [WG21 SD-6](https://isocpp.org/std/standing-documents/sd-6-sg10-feature-test-recommendations)

---

### 18.5 Reflection no draft do C++26

[⬆️ Voltar ao Sumário](#sumário)

O N5050 contém reflexão estática baseada em valores de informação e splicing, permitindo inspecionar e gerar entidades em compile time. Não confunda com RTTI (`typeid`/`dynamic_cast`) nem com uma API runtime universal.

Em 2026, suporte ainda é experimental e pode exigir flags, branch de compilador ou biblioteca específica. Isole experimentos e consulte o paper adotado, draft e página de conformidade da versão concreta.

> **Fontes oficiais:** [N5050, Reflection e Splice specifiers](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2026/n5050.pdf), [GCC 16.1 release — suporte experimental](https://gcc.gnu.org/pipermail/gcc-announce/2026/000190.html), [Clang C++ status](https://clang.llvm.org/cxx_status.html)

---

## Parte 19 — Memória Dinâmica, Ownership e Allocators

[⬆️ Voltar ao Sumário](#sumário)

Memória dinâmica é apenas um recurso. A pergunta principal não é “quem chama `delete`?”, mas “qual objeto representa ownership e qual é o lifetime dos observadores?”.

---

### 19.1 `new`/`delete` e por que evitá-los diretamente

[⬆️ Voltar ao Sumário](#sumário)

`new T` aloca storage, inicia lifetime e devolve ponteiro; `delete` encerra lifetime e desaloca. Formas de array devem combinar (`new[]`/`delete[]`). Mismatch, double delete e leak são defeitos.

```cpp
int main() {
    auto ponteiro_manual = new int{42};
    delete ponteiro_manual;
    ponteiro_manual = nullptr;
}
```

**Leitura guiada:** o exemplo é válido, mas ownership fica implícito e exceções entre aquisição/liberação poderiam vazar. Em aplicação, prefira valor, container ou factory RAII:

```cpp
#include <memory>

auto valor = std::make_unique<int>(42);
```

`make_unique` liga alocação a owner imediatamente. Não use `malloc/free` para objetos C++ que exigem construção/destruição.

**Leitura guiada:** `make_unique<int>(42)` devolve um owner move-only. Quando `valor` sair de scope, seu destructor encerra o lifetime do `int` e desaloca automaticamente, inclusive se uma exception interromper o fluxo.

> **Fonte oficial:** [N4950, New/delete expressions e Dynamic memory management](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf), [C++ Core Guidelines R.10–R.15](https://isocpp.github.io/CppCoreGuidelines/CppCoreGuidelines#Rr-newdelete)

---

### 19.2 `unique_ptr`

[⬆️ Voltar ao Sumário](#sumário)

`unique_ptr<T>` representa ownership exclusivo, é move-only e destrói com `delete` ou deleter configurado.

```cpp
#include <cstdio>
#include <memory>
#include <stdexcept>

struct FecharArquivo {
    void operator()(std::FILE* arquivo) const noexcept {
        if (arquivo != nullptr) std::fclose(arquivo);
    }
};

using Arquivo = std::unique_ptr<std::FILE, FecharArquivo>;

Arquivo abrir(const char* caminho) {
    Arquivo arquivo{std::fopen(caminho, "rb")};
    if (!arquivo) throw std::runtime_error{"não abriu"};
    return arquivo;
}
```

**Leitura guiada:** o custom deleter adapta handle C. Retorno move/elide o owner. `get()` observa; `release()` abandona ownership e exige novo owner; `reset()` troca/encerra o recurso.

Para arrays dinâmicos, `unique_ptr<T[]>` existe, mas `vector<T>` normalmente oferece tamanho e operações melhores.

> **Fonte oficial:** [N4950, `unique_ptr`](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 19.3 `shared_ptr` e `weak_ptr`

[⬆️ Voltar ao Sumário](#sumário)

`shared_ptr` compartilha control block com strong/weak counts; o objeto é destruído quando a última strong ownership termina. `weak_ptr` observa sem manter vivo e precisa de `lock()`.

```cpp
#include <memory>

struct No {
    std::shared_ptr<No> filho;
    std::weak_ptr<No> pai;
};

int main() {
    auto pai = std::make_shared<No>();
    auto filho = std::make_shared<No>();
    pai->filho = filho;
    filho->pai = pai;
}
```

**Leitura guiada:** se `pai` do filho também fosse `shared_ptr`, o ciclo manteria counts positivos. `weak_ptr` quebra ownership. `make_shared` geralmente combina object/control block, mas lifetime de memória pode se estender enquanto weak references existem.

Operações sobre control block são thread-safe conforme contrato; o objeto apontado não fica automaticamente protegido contra data races.

> **Fonte oficial:** [N4950, `shared_ptr`, `weak_ptr` e shared pointer atomic access](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 19.4 Allocators e `pmr::memory_resource`

[⬆️ Voltar ao Sumário](#sumário)

Allocator separa obtenção de storage de construção de objetos. `pmr::polymorphic_allocator` delega a `memory_resource` em runtime.

| Resource | Uso típico |
|---|---|
| `new_delete_resource` | comportamento global comum |
| `monotonic_buffer_resource` | muitas alocações com liberação conjunta |
| `unsynchronized_pool_resource` | pools single-thread |
| `synchronized_pool_resource` | pools acessados concorrentemente |

Containers com allocators incompatíveis podem não transferir storage em move/swap como esperado. O resource deve sobreviver a todos os objetos que o usam.

> **Fonte oficial:** [N4950, Allocator requirements e Memory resource](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 19.5 Alinhamento, placement `new` e armazenamento bruto

[⬆️ Voltar ao Sumário](#sumário)

```cpp
#include <cstddef>
#include <memory>

struct Ponto { int x; int y; };

int main() {
    alignas(Ponto) std::byte storage[sizeof(Ponto)];
    Ponto* ponto = std::construct_at(
        reinterpret_cast<Ponto*>(storage), Ponto{2, 3});

    std::destroy_at(ponto);
}
```

**Leitura guiada:** `alignas` garante alinhamento; `construct_at` inicia lifetime; `destroy_at` encerra. O array de bytes continua existindo. Esquecer uma etapa torna acessos inválidos.

Over-aligned types, allocation functions customizadas e placement delete possuem regras adicionais. Confine código desse nível em containers/arenas testados e use sanitizers.

> **Fonte oficial:** [N4950, Alignment, New expressions, Object lifetime e `construct_at`](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

## Parte 20 — Modelo de Objetos, Baixo Nível e Comportamento Indefinido

[⬆️ Voltar ao Sumário](#sumário)

Código de baixo nível só é correto quando respeita representação, lifetime, alinhamento, type accessibility e provenance. “Funcionou no meu build” não é contrato.

---

### 20.1 Representação, trivialidade e standard layout

[⬆️ Voltar ao Sumário](#sumário)

Object representation é a sequência de `unsigned char`/`std::byte` correspondente ao objeto; value representation é o conjunto de bits que participa do valor. Padding pode existir.

```cpp
#include <type_traits>

struct Pacote {
    unsigned codigo;
    unsigned tamanho;
};

static_assert(std::is_trivially_copyable_v<Pacote>);
static_assert(std::is_standard_layout_v<Pacote>);
```

**Leitura guiada:** traits permitem operações específicas, mas não garantem ausência de padding, endian, largura fixa ou wire format. `sizeof(Pacote)` não define protocolo portátil.

> **Fonte oficial:** [N4950, Object representation, Type properties e Type traits](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 20.2 Bytes, endian, `bit_cast` e serialização

[⬆️ Voltar ao Sumário](#sumário)

```cpp
#include <array>
#include <bit>
#include <cstddef>

float origem = 1.0F;
auto bytes = std::bit_cast<std::array<std::byte, sizeof origem>>(origem);
```

**Leitura guiada:** `bit_cast` exige origem e destino de mesmo tamanho e tipos trivially copyable; o `array` foi dimensionado com `sizeof origem` para satisfazer o primeiro requisito. O resultado expõe a representação em bytes sem aliasing por pointer cast. Isso ainda não é serialização: endian, padding, versão e floating representation precisam de formato.

`memcpy` é a ferramenta padrão para copiar representações permitidas e pode ser otimizada. Não serialize object pointers, vptrs ou layout de standard containers.

> **Fonte oficial:** [N4950, `bit_cast`, C library memory e Object representation](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 20.3 Undefined, unspecified e implementation-defined

[⬆️ Voltar ao Sumário](#sumário)

| Categoria | Exigência |
|---|---|
| well-defined | padrão determina comportamento |
| implementation-defined | implementação escolhe e documenta |
| unspecified | escolhe entre possibilidades permitidas, sem obrigação de documentar |
| undefined behavior | padrão não impõe requisitos |
| erroneous behavior (draft C++26) | comportamento bem definido causado por código incorreto; recomenda-se diagnóstico |
| ill-formed | programa viola regra; diagnóstico normalmente exigido |
| IFNDR | ill-formed, no diagnostic required |

UB inclui use-after-free, out-of-bounds, data race, signed overflow e dereference inválido. Otimização pode assumir que UB não ocorre e transformar código de forma não intuitiva.

Sanitizer detecta famílias de erros em execuções testadas; não prova ausência. Warnings, tipos, RAII, fuzzing e revisão formam defesa em profundidade.

> **Fontes oficiais:** [N4950, Terms and definitions e Implementation compliance](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf), [N5050, Erroneous behavior e Implementation compliance](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2026/n5050.pdf), [Clang UndefinedBehaviorSanitizer](https://clang.llvm.org/docs/UndefinedBehaviorSanitizer.html)

---

### 20.4 Aliasing, provenance e `launder`

[⬆️ Voltar ao Sumário](#sumário)

Acessar objeto por glvalue de tipo não permitido pode ser UB. `char`, `unsigned char` e `std::byte` têm permissões especiais para examinar representação. `reinterpret_cast` não torna automaticamente um acesso type-accessible.

`std::launder` resolve casos restritos de obtenção de pointer utilizável após criação de novo objeto no mesmo storage; não “conserta” qualquer cast. Pointer provenance e operações inválidas continuam área refinada por Defect Reports e pelo draft C++26.

Prefira `bit_cast`, `memcpy`, APIs de bytes e tipos de protocolo. Isole técnicas de aliasing em infraestrutura com testes por toolchain.

> **Fonte oficial:** [N4950, Type accessibility, Pointer-interconvertibility e `launder`](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf), [N5050, Memory and objects](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2026/n5050.pdf)

---

### 20.5 `volatile`, hardware e sinais

[⬆️ Voltar ao Sumário](#sumário)

`volatile` torna certos acessos efeitos colaterais observáveis da abstract machine e tem semântica adicional definida pela implementação. Ele não cria happens-before, atomicidade ou exclusão mútua.

Para threads, use `std::atomic`/locks. Para memory-mapped I/O, siga documentação do compilador, plataforma e dispositivo: widths, barriers e ordem não vêm apenas de `volatile`. Signal handlers assíncronos só podem realizar operações permitidas pelo padrão/plataforma.

> **Fonte oficial:** [N4950, `volatile`, Signal handlers e Atomics](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf), [GCC Volatiles](https://gcc.gnu.org/onlinedocs/gcc/Volatiles.html)

---

## Parte 21 — Threads, Concorrência e Modelo de Memória

[⬆️ Voltar ao Sumário](#sumário)

Uma data race em objetos não atômicos produz UB. Corretude concorrente exige ownership, sincronização e shutdown explícitos.

---

### 21.1 `thread`, `jthread` e cancelamento cooperativo

[⬆️ Voltar ao Sumário](#sumário)

```cpp
#include <chrono>
#include <iostream>
#include <stop_token>
#include <thread>

int main() {
    std::jthread worker([](std::stop_token stop) {
        while (!stop.stop_requested()) {
            std::this_thread::sleep_for(std::chrono::milliseconds{10});
        }
        std::cout << "encerrando\n";
    });

    worker.request_stop();
} // jthread solicita stop e faz join
```

**Leitura guiada:** `stop_token` é pedido cooperativo; código precisa observá-lo. `jthread` faz join no destructor. `thread` joinable destruída chama `terminate`, então toda thread deve ser joined ou detached — detach exige provar lifetime independente.

> **Fonte oficial:** [N4950, Threads, `jthread` e Stop tokens](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 21.2 Mutexes, locks e condition variables

[⬆️ Voltar ao Sumário](#sumário)

```cpp
#include <mutex>

class Contador {
public:
    void incrementar() {
        std::lock_guard lock{mutex_};
        ++valor_;
    }
    int valor() const {
        std::lock_guard lock{mutex_};
        return valor_;
    }
private:
    mutable std::mutex mutex_;
    int valor_{};
};
```

**Leitura guiada:** lock é RAII e libera ao sair, inclusive por exception. `mutable` permite sincronizar método logicamente const. Nunca devolva referência ao dado protegido após liberar lock.

`condition_variable` deve esperar com predicate em loop, pois wakeup pode ser espúrio e a condição pode mudar antes de reacquirir mutex.

> **Fonte oficial:** [N4950, Mutual exclusion e Condition variables](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 21.3 Semaphores, latches e barriers

[⬆️ Voltar ao Sumário](#sumário)

| Tipo C++20 | Papel |
|---|---|
| `counting_semaphore` | limita/contabiliza permissões |
| `binary_semaphore` | caso de contagem binária |
| `latch` | contagem decrescente, uso único |
| `barrier` | fases reutilizáveis com completion step |

Esses tipos coordenam; não tornam dados associados atomicamente seguros sem a relação correta. Escolha primitive pelo protocolo e evite substituir fila/buffer por uma coleção de semáforos sem invariantes documentados.

> **Fonte oficial:** [N4950, Semaphores, Latches e Barriers](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 21.4 Atomics, data races e memory ordering

[⬆️ Voltar ao Sumário](#sumário)

```cpp
#include <atomic>

std::atomic<bool> pronto{false};
int dados = 0;

void produtor() {
    dados = 42;
    pronto.store(true, std::memory_order_release);
}

int consumidor() {
    while (!pronto.load(std::memory_order_acquire)) {}
    return dados;
}
```

**Leitura guiada:** release/acquire que observa o valor cria synchronizes-with; a escrita de `dados` acontece antes da leitura. Trocar por `relaxed` quebraria essa publicação, embora o bool continuasse atômico.

Comece com ordem default `seq_cst` e simplifique apenas com prova e benchmark. Lock-free não significa wait-free nem mais rápido. `atomic::wait/notify` evita polling em muitos protocolos.

> **Fonte oficial:** [N4950, Memory model e Atomic operations](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 21.5 Futures, `async` e tarefas

[⬆️ Voltar ao Sumário](#sumário)

```cpp
#include <future>

int main() {
    auto futuro = std::async(std::launch::async, [] {
        return 6 * 7;
    });

    const int resposta = futuro.get();
    return resposta == 42 ? 0 : 1;
}
```

**Leitura guiada:** policy explícita pede execução assíncrona; sem ela, a implementação pode escolher deferred. `get()` espera, devolve valor ou relança exception e normalmente só pode ser chamado uma vez.

`promise`/`future` transportam um resultado; `packaged_task` conecta callable. A biblioteca C++23 não oferece executor/thread pool geral padrão, por isso aplicações usam frameworks ou o modelo C++26 quando disponível.

> **Fonte oficial:** [N4950, Futures e `async`](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 21.6 Deadlock, false sharing e desenho concorrente

[⬆️ Voltar ao Sumário](#sumário)

Evite deadlock com ordem global ou `std::scoped_lock` para múltiplos mutexes. Não chame código externo sob lock sem contrato de reentrância. Minimize estado compartilhado e modele shutdown antes do happy path.

False sharing ocorre quando threads modificam dados independentes na mesma unidade de interferência de cache. `hardware_destructive_interference_size`, quando fornecido, ajuda layout, mas performance depende do hardware.

ThreadSanitizer detecta data races em execuções compatíveis; combine com testes determinísticos, stress e invariantes.

> **Fonte oficial:** [N4950, `scoped_lock` e Hardware interference size](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf), [Clang ThreadSanitizer](https://clang.llvm.org/docs/ThreadSanitizer.html)

---

## Parte 22 — Coroutines, Execução e Interoperabilidade

[⬆️ Voltar ao Sumário](#sumário)

Coroutines são transformação de linguagem. O tipo de retorno e seu `promise_type` definem allocation, suspensão, valores e erros; não existe um `Task<T>` universal no C++23.

---

### 22.1 Modelo de coroutines

[⬆️ Voltar ao Sumário](#sumário)

Uma função que usa `co_await`, `co_yield` ou `co_return` pode ser coroutine. O compilador cria frame contendo parâmetros, locals preservados e estado de suspensão.

| Elemento | Papel |
|---|---|
| promise object | política da coroutine |
| coroutine handle | acesso não owning ao frame |
| awaiter | `await_ready`, `await_suspend`, `await_resume` |
| initial/final suspend | política de início e término |
| frame | armazenamento do estado; allocation pode ser elidida |

Destruir handle incorretamente, retomar frame concluído ou deixar referências a locals externos expirarem causa defeitos. Use tipos de coroutine de biblioteca testada antes de escrever promise manual.

> **Fonte oficial:** [N4950, Coroutines e Coroutine support library](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 22.2 `generator` e tipos de tarefa

[⬆️ Voltar ao Sumário](#sumário)

`std::generator` C++23 é um range síncrono lazy baseado em coroutine:

```cpp
#include <generator>

std::generator<int> contar_ate(int fim) {
    for (int i = 1; i <= fim; ++i) {
        co_yield i;
    }
}
```

**Leitura guiada:** cada incremento de iteração retoma a coroutine até próximo `co_yield`. Isso não cria thread nem I/O assíncrona. O generator e referências produzidas possuem regras de lifetime próprias.

Tipos de tarefa de Asio, frameworks e plataformas têm contratos distintos de executor, cancellation e ownership; não são intercambiáveis só porque usam `co_await`.

> **Fonte oficial:** [N4950, `generator`](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 22.3 Execution e senders/receivers no C++26

[⬆️ Voltar ao Sumário](#sumário)

O draft C++26 padroniza o framework `<execution>` com senders/receivers, schedulers e algorithms de composição. Um sender descreve trabalho e completion channels; conectar a receiver produz operation state; `start` inicia.

Essa abstração é diferente de `std::execution` policies dos algoritmos paralelos. Como suporte é novo e amplo, verifique N5050 e o status da biblioteca. Não crie facade “compatível” baseada apenas em papers antigos.

> **Fonte oficial:** [N5050, Execution control library](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2026/n5050.pdf)

---

### 22.4 Interoperabilidade com C

[⬆️ Voltar ao Sumário](#sumário)

```cpp
extern "C" {
    int biblioteca_c_inicializar(void);
}
```

**Leitura guiada:** o bloco atribui C language linkage à declaração, permitindo ligá-la conforme a convenção da implementação para símbolos C. `extern "C"` não converte automaticamente layout complexo, exception, ownership ou calling convention fora do que implementation/ABI definem. Exponha tipos C, tamanhos explícitos e funções de create/destroy.

Nunca deixe exception atravessar boundary C. Capture na borda e traduza para status. Combine allocator: quem aloca oferece a função que desaloca.

> **Fonte oficial:** [N4950, Language linkage](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf), [ISO C standard catalogue](https://www.iso.org/standard/82075.html)

---

### 22.5 ABI, bibliotecas compartilhadas e visibilidade

[⬆️ Voltar ao Sumário](#sumário)

O padrão C++ não fixa name mangling, layout de vtable, exception ABI ou formato de biblioteca. ABI vem de plataforma/toolchain, como Itanium C++ ABI ou Microsoft ABI.

Fronteira binária pública deve controlar:

- símbolos exportados e calling convention;
- layout e alinhamento;
- compiler/runtime/standard library compatíveis;
- allocator e ownership;
- exceptions e RTTI;
- versão e backward compatibility.

APIs C com handles opacos costumam ser fronteira plugin mais estável. PImpl reduz exposição, mas não resolve incompatibilidade de runtime.

> **Fontes oficiais:** [Itanium C++ ABI](https://itanium-cxx-abi.github.io/cxx-abi/abi.html), [GCC Visibility](https://gcc.gnu.org/wiki/Visibility), [MSVC DLLs](https://learn.microsoft.com/cpp/build/dlls-in-visual-cpp)

---

## Checkpoint — Fundamentos da Linguagem (Partes 1–22)

[⬆️ Voltar ao Sumário](#sumário)

Antes de avançar, você deve conseguir:

1. explicar preprocessamento, compilação, ODR e linking;
2. distinguir scope, storage duration, lifetime e ownership;
3. escolher valor, referência, ponteiro, view e smart pointer;
4. escrever tipo Rule of Zero com invariantes;
5. prever invalidação de container e view;
6. explicar `std::move`, forwarding e copy elision;
7. usar algorithm/range antes de loop manual complexo;
8. distinguir exception, `expected`, absence e bug;
9. restringir templates com concepts;
10. reconhecer data race, UB e fronteira ABI.

Se algum item depende de “normalmente funciona”, volte ao capítulo e encontre a garantia formal.

---

## Parte 23 — C++ nos Principais Contextos de Aplicação

[⬆️ Voltar ao Sumário](#sumário)

C++ aparece onde controle de recursos, integração nativa, throughput, latência, footprint ou legado justificam sua complexidade. O domínio define subset, toolchain e bibliotecas.

---

### 23.1 Sistemas, drivers e embedded

[⬆️ Voltar ao Sumário](#sumário)

Sistemas operacionais, firmware e embedded podem usar implementação freestanding, sem exceptions, RTTI, heap ou biblioteca hosted completa. Essas ausências são decisões da plataforma, não propriedades universais de C++.

Prioridades:

- layout e acesso a hardware documentados pela ABI;
- startup e linker script;
- alocação determinística;
- interrupt/signal safety;
- atomics e barriers da arquitetura;
- watchdog, fault handling e atualização.

Use abstrações zero-overhead testáveis sobre registradores e handles; não espalhe macros e casts por toda aplicação.

> **Fontes oficiais:** [N4950, Freestanding implementations](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf), [GCC — Freestanding environments](https://gcc.gnu.org/onlinedocs/gcc/Standards.html), [LLVM Embedded Toolchain](https://clang.llvm.org/docs/Toolchain.html)

---

### 23.2 Games e engines

[⬆️ Voltar ao Sumário](#sumário)

Engines combinam frame budget, assets, rendering, física, áudio, scripting e tooling. C++ da engine não é necessariamente C++ padrão puro: reflection/code generation, garbage-collected objects e macros podem vir do framework.

No Unreal Engine, siga contratos de `UObject`, garbage collection, properties e build tool; raw pointer que seria apenas observador em C++ comum pode precisar wrapper específico do engine. Em SDL, lifetime de windows/renderers segue API C.

> **Fontes oficiais:** [Unreal Engine — Programming with C++](https://dev.epicgames.com/documentation/en-us/unreal-engine/programming-with-cplusplus-in-unreal-engine), [SDL Wiki](https://wiki.libsdl.org/SDL3/FrontPage)

---

### 23.3 Desktop, mobile e multimídia

[⬆️ Voltar ao Sumário](#sumário)

Qt, JUCE, wxWidgets e APIs nativas oferecem event loops, objetos, threads e ownership próprios. Não bloqueie UI thread; não destrua widget fora da thread exigida; traduza `std`/framework types nas bordas.

```text
UI/event loop → application services → domain → adapters nativos
```

Uma camada de domínio sem dependência de UI reduz rebuild, facilita testes e permite trocar framework. Mobile costuma envolver JNI/Objective-C++ e ciclos de vida impostos pela plataforma.

> **Fontes oficiais:** [Qt 6 Documentation](https://doc.qt.io/qt-6/), [Android NDK Guides](https://developer.android.com/ndk/guides), [Apple — Xcode Build Settings para C++/Objective-C++](https://developer.apple.com/documentation/xcode/build-settings-reference)

---

### 23.4 HPC, SIMD, GPU e computação científica

[⬆️ Voltar ao Sumário](#sumário)

HPC depende de layout, vetorização, memória, NUMA, comunicação e precisão. C++23 oferece `mdspan`; o draft C++26 contém `simd` e componentes numéricos adicionais. GPU usa modelos externos como CUDA, HIP, SYCL ou APIs gráficas.

Não substitua algoritmo numericamente estável por versão “mais rápida” sem análise de erro. Reduções paralelas podem mudar associação e resultado floating-point.

> **Fontes oficiais:** [N4950, `mdspan` e Parallel algorithms](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf), [NVIDIA CUDA C++ Programming Guide](https://docs.nvidia.com/cuda/cuda-c-programming-guide/), [Intel oneAPI DPC++ Guide](https://www.intel.com/content/www/us/en/docs/oneapi/programming-guide/current/overview.html)

---

### 23.5 Finanças e baixa latência

[⬆️ Voltar ao Sumário](#sumário)

Baixa latência exige percentis, jitter e tail behavior, não apenas média. Alocação, page faults, contention, cache misses, frequency scaling e syscalls podem dominar.

Práticas comuns — arenas, batching, lock-free, affinity e busy-wait — têm custos de complexidade e portabilidade. Só adote com benchmark representativo e invariantes de memória provadas. Inteiros/fixed-point podem ser apropriados para dinheiro; precisão e rounding fazem parte do domínio.

> **Fontes oficiais:** [ISO C++ Performance Technical Report draft](https://www.open-std.org/jtc1/sc22/wg21/docs/TR18015.pdf), [C++ Core Guidelines Per rules](https://isocpp.github.io/CppCoreGuidelines/CppCoreGuidelines#S-performance)

---

### 23.6 Backend, bancos e infraestrutura

[⬆️ Voltar ao Sumário](#sumário)

C++ é usado em bancos, browsers, compilers, proxies, storage e serviços de alta eficiência. A biblioteca padrão C++23 não contém HTTP, sockets portáveis, TLS, JSON ou ORM; escolha projetos externos e modele timeouts, cancellation, backpressure e shutdown.

Processo de serviço precisa de:

- limites de request e filas;
- deadlines propagados;
- pools/connection ownership;
- logging estruturado e métricas;
- graceful shutdown;
- compatibilidade de protocolo.

> **Fontes oficiais:** [Boost.Asio Documentation](https://www.boost.org/doc/libs/release/doc/html/boost_asio.html), [gRPC C++ Documentation](https://grpc.io/docs/languages/cpp/), [Protocol Buffers C++](https://protobuf.dev/reference/cpp/)

---

### 23.7 Safety-critical e constrained environments

[⬆️ Voltar ao Sumário](#sumário)

Safety-critical normalmente restringe linguagem, dynamic allocation, recursion, exceptions ou concurrency segundo risco e certificação. Não existe um único “C++ seguro” universal.

Use coding standard aplicável, qualified toolchain quando exigido, traceability, análise estática, cobertura e teste de fault. O WG21 SG23 trabalha em safety/security; perfis e propostas devem ser distinguidos do padrão publicado.

> **Fontes oficiais:** [WG21 SG23 — Safety and Security](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/), [AUTOSAR C++14 Guidelines](https://www.autosar.org/fileadmin/standards/R18-10-1/AP/AUTOSAR_RS_CPP14Guidelines.pdf), [SEI CERT C++ Coding Standard](https://wiki.sei.cmu.edu/confluence/pages/viewpage.action?pageId=88046682)

---

## Parte 24 — Arquitetura de Aplicações C++

[⬆️ Voltar ao Sumário](#sumário)

Arquitetura organiza dependências, ownership, concorrência e boundaries. C++ não obriga MVC, ECS, DDD ou microservices.

---

### 24.1 C++ não impõe arquitetura

[⬆️ Voltar ao Sumário](#sumário)

Escolha arquitetura por forças reais:

| Força | Pergunta |
|---|---|
| lifetime | quem possui cada recurso? |
| mudança | qual detalhe varia? |
| build | que include causa rebuild amplo? |
| performance | onde estão bytes, branches e sincronização? |
| deployment | quais ABI/process boundaries existem? |
| segurança | qual input cruza trust boundary? |

Não transforme todo tipo em interface virtual nem todo fluxo em template. Cada mecanismo afeta build, binário, testabilidade e ABI.

> **Fonte oficial prática:** [C++ Core Guidelines — Interfaces, Classes e Resources](https://isocpp.github.io/CppCoreGuidelines/CppCoreGuidelines)

---

### 24.2 Camadas, Ports and Adapters e dependências

[⬆️ Voltar ao Sumário](#sumário)

```text
drivers/UI/network
       ↓ implementam ports
application services
       ↓
domain/value types
```

Dependências de código devem apontar para políticas estáveis. Interfaces podem ser virtual bases, concepts, templates ou functions; ports no mesmo binário não exigem necessariamente classes abstratas.

Evite tipos de framework no domínio quando não fazem parte do conceito. Tradução nas bordas custa código, mas reduz acoplamento e facilita testes.

> **Fonte oficial prática:** [C++ Core Guidelines — Interfaces e Dependency injection](https://isocpp.github.io/CppCoreGuidelines/CppCoreGuidelines#S-interfaces)

---

### 24.3 DDD e tipos de domínio

[⬆️ Voltar ao Sumário](#sumário)

Value objects C++ combinam bem com Rule of Zero, equality e validação. Entidades precisam de identidade estável; aggregates delimitam consistência; repositories são ports, não obrigatoriamente classes CRUD.

```cpp
#include <compare>

struct PedidoId {
    unsigned long long valor;
    auto operator<=>(const PedidoId&) const = default;
};
```

**Leitura guiada:** wrapper impede misturar id de pedido com inteiro qualquer e mantém custo de um campo. Serialization e hash continuam decisões explícitas.

> **Fontes oficiais:** [N4950, Classes e Comparisons](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf), [C++ Core Guidelines — Concrete types](https://isocpp.github.io/CppCoreGuidelines/CppCoreGuidelines#Rc-concrete)

---

### 24.4 ECS e data-oriented design

[⬆️ Voltar ao Sumário](#sumário)

Entity Component System separa ids, componentes e sistemas. Data-oriented design organiza dados segundo acesso e hardware, não simplesmente “não usar objetos”.

SoA pode melhorar loops que leem poucos campos; AoS pode ser melhor para operações completas em um elemento. Medição decide. ECS não substitui invariantes: ids geracionais, remoção, iterator invalidation e scheduling precisam de contratos.

> **Fonte de implementação oficial:** [EnTT Documentation](https://skypjack.github.io/entt/)

---

### 24.5 Plugins e fronteiras ABI

[⬆️ Voltar ao Sumário](#sumário)

Uma fronteira robusta pode usar C ABI e handle opaco:

```cpp
extern "C" {
struct plugin_handle;
int plugin_create(plugin_handle** out);
void plugin_destroy(plugin_handle* plugin);
}
```

**Leitura guiada:** caller não conhece layout. A mesma biblioteca fornece destroy, evitando cruzar allocators. Versione struct/function table, valide tamanho e nunca deixe exception escapar.

Carregar DLL/SO é API do sistema, não C++ padrão. Assinatura, search path e unload seguro fazem parte do threat/lifetime model.

> **Fontes oficiais:** [N4950, Language linkage](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf), [Itanium C++ ABI](https://itanium-cxx-abi.github.io/cxx-abi/abi.html), [MSVC DLLs](https://learn.microsoft.com/cpp/build/dlls-in-visual-cpp)

---

### 24.6 Concorrência, eventos e backpressure

[⬆️ Voltar ao Sumário](#sumário)

Defina explicitamente:

- quem executa cada handler;
- se ordem é garantida;
- limite da fila;
- comportamento em saturação;
- cancellation e deadline;
- ownership da mensagem;
- política de exception.

Fila “infinita” transfere falha para memória/latência. Backpressure pode bloquear, descartar, coalescer ou rejeitar; a escolha pertence ao domínio.

> **Fonte oficial:** [N4950, Threads, Atomics e Futures](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 24.7 Padrões clássicos adaptados ao C++ moderno

[⬆️ Voltar ao Sumário](#sumário)

| Intenção | Forma moderna frequente |
|---|---|
| Strategy | template/concept, lambda ou interface |
| Factory | função retornando valor, pointer RAII ou `expected` |
| Observer | callback + subscription token |
| Decorator | composição ou wrapper template |
| State | `variant` + visitor ou polimorfismo |
| Bridge | PImpl/interface |
| Scope Guard | RAII |

Padrão é vocabulário para uma força recorrente, não meta de contagem. C++ oferece mais de uma implementação porque binding e ownership variam.

> **Fonte oficial prática:** [C++ Core Guidelines — Class hierarchies e Resources](https://isocpp.github.io/CppCoreGuidelines/CppCoreGuidelines)

---

## Parte 25 — Toolchains, Build, Dependências e Qualidade

[⬆️ Voltar ao Sumário](#sumário)

O standard não define CLI, linker, package manager ou projeto. Engenharia C++ profissional exige tratar toolchain como parte versionada do produto.

---

### 25.1 GCC, Clang e MSVC

[⬆️ Voltar ao Sumário](#sumário)

| Família | Driver comum | Biblioteca frequente | Plataforma típica |
|---|---|---|---|
| GCC | `g++` | libstdc++ | Linux e várias |
| LLVM/Clang | `clang++` | libc++ ou libstdc++ | multiplataforma |
| MSVC | `cl` | MSVC STL | Windows |

Compiler frontend e standard library podem vir de projetos diferentes. Registre versão, target triple, sysroot, linker, runtime, ABI flags e C++ standard mode.

> **Fontes oficiais:** [GCC Manual](https://gcc.gnu.org/onlinedocs/), [Clang Documentation](https://clang.llvm.org/docs/), [MSVC C++ Documentation](https://learn.microsoft.com/cpp/)

---

### 25.2 Compilação, linking e opções essenciais

[⬆️ Voltar ao Sumário](#sumário)

```bash
g++ -std=c++23 -Wall -Wextra -Wpedantic -Wconversion \
    -O2 -g main.cpp biblioteca.cpp -o app

clang++ -std=c++23 -Wall -Wextra -Wpedantic -Wconversion \
    -O2 -g main.cpp biblioteca.cpp -o app

cl /std:c++latest /W4 /permissive- /EHsc /O2 /Zi main.cpp biblioteca.cpp
```

**Leitura guiada:** warnings são diagnósticos adicionais, não prova. `-O2` otimiza; `-g` mantém debug info e pode coexistir. Flags não são portáveis entre compilers e algumas alteram ABI/semântica.

Não promova `-Werror` indiscriminadamente para consumidores de headers externos; aplique por target/código próprio. Compile em mais de uma toolchain quando portabilidade importa.

> **Fontes oficiais:** [GCC C++ Dialect/Warning Options](https://gcc.gnu.org/onlinedocs/gcc/C_002b_002b-Dialect-Options.html), [Clang Command Guide](https://clang.llvm.org/docs/CommandGuide/clang.html), [MSVC compiler options](https://learn.microsoft.com/cpp/build/reference/compiler-options)

---

### 25.3 CMake e targets

[⬆️ Voltar ao Sumário](#sumário)

```cmake
cmake_minimum_required(VERSION 3.25)
project(GuiaCpp LANGUAGES CXX)

add_library(dominio src/pedido.cpp)
target_include_directories(dominio PUBLIC include)
target_compile_features(dominio PUBLIC cxx_std_23)

add_executable(app src/main.cpp)
target_link_libraries(app PRIVATE dominio)
```

**Leitura guiada:** requirements pertencem a targets. `PUBLIC` propaga include/feature a consumidores; `PRIVATE` não. Não injete globalmente flags e include paths que deveriam seguir dependência.

Use configure/build/test separados e gerador adequado:

```bash
cmake -S . -B build -DCMAKE_BUILD_TYPE=Release
cmake --build build --config Release
ctest --test-dir build --output-on-failure
```

**Leitura guiada:** `-S` aponta para o fonte e `-B` cria uma árvore de build separada. Geradores single-config usam `CMAKE_BUILD_TYPE`; geradores multi-config escolhem a configuração com `--config`. `ctest` executa os testes registrados nessa árvore e mostra a saída quando há falha.

> **Fonte oficial:** [CMake Tutorial](https://cmake.org/cmake/help/latest/guide/tutorial/), [`target_compile_features`](https://cmake.org/cmake/help/latest/command/target_compile_features.html)

---

### 25.4 Conan, vcpkg e dependências

[⬆️ Voltar ao Sumário](#sumário)

Conan e vcpkg resolvem/constroem pacotes segundo modelos distintos; CMake não é package manager. Lockfiles, manifests, registries/remotes e binary compatibility precisam ser controlados.

| Verificação | Pergunta |
|---|---|
| versão | pin/constraint é reproduzível? |
| origem | registry e artefato são confiáveis? |
| ABI | compiler, runtime e opções combinam? |
| licença | uso/distribuição são compatíveis? |
| transitive deps | foram auditadas e travadas? |
| atualização | há processo para CVEs e regressões? |

> **Fontes oficiais:** [Conan Documentation](https://docs.conan.io/2/), [vcpkg Documentation](https://learn.microsoft.com/vcpkg/)

---

### 25.5 Testes, documentação e análise estática

[⬆️ Voltar ao Sumário](#sumário)

Teste unitário verifica tipo/algoritmo; integração verifica boundaries; property/fuzz explora espaço; regression preserva correção. CTest orquestra executáveis, não substitui framework.

Ferramentas comuns:

| Finalidade | Opções |
|---|---|
| testes | GoogleTest, Catch2, doctest |
| documentação | Doxygen |
| análise | clang-tidy, Clang Static Analyzer, MSVC `/analyze` |
| formatação | clang-format |

Fixe configurações no repositório. Supressão deve ter justificativa e scope mínimo.

> **Fontes oficiais:** [GoogleTest](https://google.github.io/googletest/), [Doxygen Manual](https://www.doxygen.nl/manual/), [clang-tidy](https://clang.llvm.org/extra/clang-tidy/), [MSVC Code Analysis](https://learn.microsoft.com/cpp/code-quality/)

---

### 25.6 Sanitizers, debugger e profiling

[⬆️ Voltar ao Sumário](#sumário)

| Ferramenta | Detecta/ajuda |
|---|---|
| AddressSanitizer | out-of-bounds, use-after-free e afins |
| UndefinedBehaviorSanitizer | classes selecionadas de UB |
| ThreadSanitizer | data races |
| MemorySanitizer | uso de memória não inicializada, com requisitos de instrumentação |
| LeakSanitizer | leaks |
| debugger | estado, breakpoints, stack e dump |
| profiler | tempo, CPU, allocation, cache conforme ferramenta |

Rode configurações separadas; sanitizers têm incompatibilidades e overhead. Não use benchmark instrumentado como número de produção.

> **Fontes oficiais:** [Clang Sanitizers](https://clang.llvm.org/docs/index.html#sanitizers), [GCC Instrumentation Options](https://gcc.gnu.org/onlinedocs/gcc/Instrumentation-Options.html), [Visual Studio C++ debugging](https://learn.microsoft.com/visualstudio/debugger/debugging-native-code)

---

### 25.7 Portabilidade, ABI e builds reproduzíveis

[⬆️ Voltar ao Sumário](#sumário)

Matriz mínima registra OS, architecture, compiler, standard library, build type e feature flags. CI deve compilar warnings altos, executar testes e ao menos uma configuração sanitizer.

Evite depender de:

- ordem/size não garantidos;
- extensões sem feature gate;
- paths/timestamps embutidos sem controle;
- locale/timezone do host;
- transitive includes;
- comportamento de debug diferente;
- ABI de containers através de fronteira incompatível.

Presets CMake codificam workflows; toolchain files descrevem cross-compilation. Reprodutibilidade exige também dependências e ambiente.

> **Fontes oficiais:** [CMake Presets](https://cmake.org/cmake/help/latest/manual/cmake-presets.7.html), [CMake Toolchains](https://cmake.org/cmake/help/latest/manual/cmake-toolchains.7.html), [Reproducible Builds](https://reproducible-builds.org/docs/)

---

## Parte 26 — I/O, Filesystem, Tempo e Integração de Dados

[⬆️ Voltar ao Sumário](#sumário)

I/O combina falha parcial, encoding, buffering e recursos externos. Sempre confira estado e defina limites.

---

### 26.1 Iostreams e I/O formatada

[⬆️ Voltar ao Sumário](#sumário)

```cpp
#include <iostream>
#include <string>

int main() {
    std::cout << "Nome: ";
    std::string nome;
    if (!std::getline(std::cin, nome)) {
        std::cerr << "falha de entrada\n";
        return 1;
    }
    std::cout << "Olá, " << nome << '\n';
}
```

**Leitura guiada:** `getline` consome uma linha e o teste verifica fail/eof. Misturar `operator>>` e `getline` exige tratar delimitador pendente. `cerr` é stream de diagnóstico.

`sync_with_stdio(false)` pode melhorar desempenho, mas depois não misture C stdio e iostream sem compreender sincronização. `std::print` oferece formatação C++23 quando implementada.

> **Fonte oficial:** [N4950, Iostreams e Print functions](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 26.2 Arquivos e filesystem

[⬆️ Voltar ao Sumário](#sumário)

```cpp
#include <filesystem>
#include <fstream>
#include <stdexcept>

void gravar(const std::filesystem::path& caminho) {
    std::ofstream saida{caminho, std::ios::trunc};
    if (!saida) throw std::runtime_error{"falha ao abrir"};
    saida << "dados\n";
    if (!saida) throw std::runtime_error{"falha ao gravar"};
}
```

**Leitura guiada:** abrir e gravar são falhas separadas. `filesystem::path` representa path nativo; sua conversão para string/encoding depende da plataforma. Canonicalizar não autoriza acessar arquivo.

Para atualização robusta, grave temporário no mesmo filesystem, flush/sync conforme durabilidade exigida e faça replace atômico apenas quando a API da plataforma garantir.

> **Fonte oficial:** [N4950, Filesystem library e File-based streams](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 26.3 I/O binária, buffers e memória mapeada

[⬆️ Voltar ao Sumário](#sumário)

```cpp
#include <array>
#include <cstddef>
#include <fstream>

int main() {
    std::array<std::byte, 16> cabecalho{};
    std::ifstream entrada{"dados.bin", std::ios::binary};
    entrada.read(reinterpret_cast<char*>(cabecalho.data()),
                 static_cast<std::streamsize>(cabecalho.size()));
    return entrada ? 0 : 1;
}
```

**Leitura guiada:** o cast adapta buffer de bytes à API histórica da stream. `gcount()`/estado indicam quantos bytes chegaram. Parseie cada campo com bounds, endian e versão; não faça cast do buffer para struct arbitrária.

Memory mapping não faz parte do C++23; use API do SO ou biblioteca, com RAII para mapping/handle e cuidado com truncation concorrente.

> **Fonte oficial:** [N4950, Byte types e Input/output library](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf), [Windows File Mapping](https://learn.microsoft.com/windows/win32/memory/file-mapping), [POSIX `mmap`](https://pubs.opengroup.org/onlinepubs/9799919799/functions/mmap.html)

---

### 26.4 Chrono, calendários e fusos

[⬆️ Voltar ao Sumário](#sumário)

`std::chrono` separa duration, time_point e clock. `steady_clock` é apropriado para timeout/medição monotônica; `system_clock` relaciona-se ao tempo civil e pode ajustar.

```cpp
#include <chrono>

using namespace std::chrono_literals;
constexpr auto timeout = 250ms;

const auto inicio = std::chrono::steady_clock::now();
// operação
const auto decorrido = std::chrono::steady_clock::now() - inicio;
```

**Leitura guiada:** duration carrega unidade no tipo e converte conforme regras. C++20 adicionou calendários e timezone database; disponibilidade de dados/atualização é operacional.

> **Fonte oficial:** [N4950, Time library](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 26.5 Regex, locale e parsing

[⬆️ Voltar ao Sumário](#sumário)

`std::regex` implementa grammars padronizadas, mas performance e Unicode não equivalem a engine especializada. Defina limite de input e evite regex ambígua em dados hostis.

Locale afeta streams e facets; parsing de protocolo costuma pedir comportamento locale-independent (`from_chars`). Datas, números e texto de UI têm requirements diferentes.

> **Fonte oficial:** [N4950, Regular expressions, Localization e `from_chars`](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 26.6 Networking e serialização

[⬆️ Voltar ao Sumário](#sumário)

C++23 não oferece sockets, HTTP, TLS, JSON ou schema serialization na biblioteca padrão. Não confunda proposal com disponibilidade.

| Necessidade | Projetos comuns |
|---|---|
| async networking | Boost.Asio |
| HTTP/WebSocket | Boost.Beast, frameworks |
| RPC/schema | gRPC + Protocol Buffers |
| JSON | Boost.JSON, nlohmann/json |
| binary schema | FlatBuffers, Cap'n Proto |

Defina framing, tamanho máximo, endian, versioning, unknown fields, authentication e timeout. Nunca deserialize diretamente bytes não confiáveis em object layout C++.

> **Fontes oficiais dos projetos:** [Boost.Asio](https://www.boost.org/doc/libs/release/doc/html/boost_asio.html), [Boost.Beast](https://www.boost.org/doc/libs/release/libs/beast/), [Protocol Buffers](https://protobuf.dev/), [FlatBuffers](https://flatbuffers.dev/)

---

## Parte 27 — Engenharia para Produção

[⬆️ Voltar ao Sumário](#sumário)

Produção exige mais que código correto no caminho feliz: observabilidade, limites, atualização, segurança e compatibilidade são contratos.

---

### 27.1 Estratégia de testes

[⬆️ Voltar ao Sumário](#sumário)

| Camada | Verifica |
|---|---|
| unit | tipos, invariantes e algoritmos |
| property | leis sobre muitas entradas |
| fuzz | parser e boundaries hostis |
| integration | filesystem, DB, rede, processo |
| concurrency stress | interleavings e shutdown |
| benchmark | regressões de custo |
| end-to-end | fluxo implantado |

Compile testes em debug e otimizado: UB e macros podem se manifestar diferentemente. Rode ASan/UBSan e TSan em jobs separados. Teste falhas de allocation/I/O quando o contrato exige recovery.

> **Fontes oficiais:** [GoogleTest Advanced Guide](https://google.github.io/googletest/advanced.html), [LLVM libFuzzer](https://llvm.org/docs/LibFuzzer.html), [Google Benchmark](https://google.github.io/benchmark/)

---

### 27.2 Logging, configuração e segredos

[⬆️ Voltar ao Sumário](#sumário)

Log deve ter nível, timestamp, evento, correlação e campos estruturados. Não registre senha, token, chave, PII ou payload inteiro por padrão. Logging em caminho de falha não deve lançar nem bloquear indefinidamente.

Configuração precisa de schema, defaults, validação, precedência e reload definido. Segredo vem de mecanismo apropriado do ambiente e deve ser redigido em diagnóstico.

Biblioteca padrão não oferece logging/configuração geral; escolha projeto externo e documente ownership/sinks/shutdown.

> **Fontes oficiais:** [OpenTelemetry C++](https://opentelemetry.io/docs/languages/cpp/), [spdlog](https://github.com/gabime/spdlog), [OWASP Logging Cheat Sheet](https://cheatsheetseries.owasp.org/cheatsheets/Logging_Cheat_Sheet.html)

---

### 27.3 Observabilidade e performance

[⬆️ Voltar ao Sumário](#sumário)

Meça wall time, CPU, allocation, contention, I/O, cache e percentis conforme hipótese. Um microbenchmark precisa impedir dead-code elimination, aquecer quando relevante e controlar ambiente.

Profilers oficiais/plataforma incluem Linux `perf`, LLVM XRay, Visual Studio Profiler e Intel VTune. Telemetria em produção deve ter sampling e cardinalidade controlados.

Otimização válida preserva semântica definida. Não “corrija” benchmark usando UB, data race ou aliasing inválido.

> **Fontes oficiais:** [Linux perf](https://perf.wiki.kernel.org/), [LLVM XRay](https://llvm.org/docs/XRay.html), [Visual Studio Performance Profiler](https://learn.microsoft.com/visualstudio/profiling/)

---

### 27.4 Segurança essencial

[⬆️ Voltar ao Sumário](#sumário)

Prioridades:

- validar comprimentos antes de aritmética/alocação;
- usar containers/views com bounds explícitos;
- evitar C string APIs inseguras;
- tratar integer overflow e signedness;
- eliminar use-after-free e race;
- limitar parser, regex, decompression e recursion;
- usar criptografia/TLS de biblioteca mantida;
- atualizar toolchain e dependências.

Warnings e sanitizers não substituem threat modeling. Secrets e inputs externos cruzam trust boundaries; converta cedo para tipos validados.

> **Fontes oficiais:** [SEI CERT C++](https://wiki.sei.cmu.edu/confluence/pages/viewpage.action?pageId=88046682), [C++ Core Guidelines](https://isocpp.github.io/CppCoreGuidelines/CppCoreGuidelines), [OWASP C/C++ Security](https://owasp.org/www-project-secure-coding-practices-quick-reference-guide/)

---

### 27.5 Empacotamento, distribuição e supply chain

[⬆️ Voltar ao Sumário](#sumário)

Entregável pode ser executável estático/dinâmico, DLL/SO, package do SO, container ou SDK. Registre runtime dependencies, minimum OS/CPU, licença, symbols e config.

Supply chain inclui:

- pin e hash de sources/binários;
- registry/remotes permitidos;
- SBOM;
- assinatura/attestation quando aplicável;
- rebuild e patch de vulnerabilidade;
- separação de símbolos/debug;
- smoke test do artefato final.

> **Fontes oficiais:** [CMake CPack](https://cmake.org/cmake/help/latest/module/CPack.html), [SLSA](https://slsa.dev/spec/), [CycloneDX](https://cyclonedx.org/docs/)

---

### 27.6 APIs, ABI e evolução

[⬆️ Voltar ao Sumário](#sumário)

Source compatibility não implica binary compatibility. Alterar membros, bases, virtuals, inline functions, templates, exception specification ou standard library pode quebrar consumidores.

Para API pública:

- documente ownership, lifetime, thread-safety e invalidation;
- use semantic versioning conforme política real;
- ofereça deprecation/migration;
- teste headers isolados;
- controle exports;
- execute ABI checker quando binário importa;
- evite expor tipos instáveis através de DLL boundary.

> **Fontes oficiais:** [GCC ABI Policy](https://gcc.gnu.org/onlinedocs/libstdc++/manual/abi.html), [LLVM ABI Compatibility Policy](https://llvm.org/docs/DeveloperPolicy.html#abi-breaking-checks), [MSVC Binary Compatibility](https://learn.microsoft.com/cpp/porting/binary-compat-2015-2017)

---

## Parte 28 — Catálogo da Linguagem e da Biblioteca Padrão

[⬆️ Voltar ao Sumário](#sumário)

Esta parte responde “isso já existe?”. Keywords pertencem à linguagem; classes e funções vêm da biblioteca padrão; OS e bibliotecas externas ficam fora do ISO C++.

---

### 28.1 Linguagem, biblioteca, implementação e plataforma

[⬆️ Voltar ao Sumário](#sumário)

| Camada | Exemplo | Quem define |
|---|---|---|
| linguagem | classes, templates, expressions, lifetime | ISO C++ |
| biblioteca padrão | `vector`, `sort`, `thread`, `filesystem` | ISO C++ |
| implementação | libstdc++, libc++, MSVC STL, ABI/runtime | fornecedor/projeto |
| toolchain | compiler, linker, archiver, debugger | fornecedor/projeto |
| plataforma | sockets, processes, GUI, mapping | OS/SDK |
| dependência | Boost, Qt, Eigen, gRPC | projeto externo |

“STL” é usado informalmente para containers, iterators e algorithms originados da Standard Template Library; a **C++ Standard Library** é mais ampla e inclui I/O, strings, concurrency, filesystem e outros.

Header padrão não é necessariamente arquivo físico, e implementação pode usar intrinsics. Não dependa de headers internos ou transitive includes.

> **Fonte oficial:** [N4950, Library introduction e Headers](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 28.2 Keywords e identificadores especiais

[⬆️ Voltar ao Sumário](#sumário)

Estas são as keywords listadas na tabela normativa do C++23:

```text
alignas alignof asm auto bool break case catch char char8_t char16_t
char32_t class concept const consteval constexpr constinit const_cast
continue co_await co_return co_yield decltype default delete do double
dynamic_cast else enum explicit export extern false float for friend goto
if inline int long mutable namespace new noexcept nullptr operator private
protected public register reinterpret_cast requires return short signed
sizeof static static_assert static_cast struct switch template this
thread_local throw true try typedef typeid typename union unsigned using
virtual void volatile wchar_t while
```

`register` está sem uso semântico, mas permanece reservado. Os tokens alfabéticos abaixo são representações alternativas de operadores e também são reservados:

```text
and and_eq bitand bitor compl not not_eq or or_eq xor xor_eq
```

No C++23, `final`, `import`, `module` e `override` são **identificadores com significado especial** em contextos específicos, não keywords incondicionais da mesma tabela. O preprocessor ainda produz tokens especiais para directives de módulo.

Keywords de extensões, atributos de fornecedor e nomes do draft C++26 dependem do modo/toolchain; não os misture à lista C++23.

> **Fonte oficial:** [N4950, §5.10–5.12 — Identifiers, Keywords, Operators](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 28.3 Operadores, punctuators e alternative tokens

[⬆️ Voltar ao Sumário](#sumário)

| Família | Exemplos |
|---|---|
| aritméticos | `+ - * / %` |
| comparação | `== != < > <= >= <=>` |
| lógicos | `! && ||` |
| bitwise | `~ & | ^ << >>` |
| assignment | `= += -= *= /= %= &= |= ^= <<= >>=` |
| acesso | `. -> .* ->* [] ()` |
| increment/decrement | `++ --` |
| outros | `?: , sizeof alignof typeid noexcept new delete co_await` |
| scope/templates | `:: < >` conforme contexto |
| preprocessor | `# ##` e digraphs |

Alternative punctuators incluem `<%`/`%>` para braces, `<:`/`:>` para brackets, `%:` para `#` e `%:%:` para `##`. São portáveis, porém raros; conhecer ajuda ao ler código legado.

Não é possível criar operador novo, mudar precedência/aridade ou sobrecarregar alguns operadores (`.`, `::`, `?:`, `sizeof` e outros). Overloads exigem ao menos um operando class/enum conforme regras.

> **Fonte oficial:** [N4950, Operators and punctuators e Overloaded operators](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 28.4 Tipos fundamentais, literais e sufixos

[⬆️ Voltar ao Sumário](#sumário)

| Literal | Exemplos | Observação |
|---|---|---|
| inteiro | `42`, `0b1010`, `052`, `0x2A` | octal começa com zero |
| separador | `1'000'000` | apostrophe não altera valor |
| suffix inteiro | `42u`, `42L`, `42LL`, `42uz` | tipo depende de base/valor/suffix |
| floating | `3.14`, `1e-3`, `0x1.2p3` | default `double`; `f` para float |
| caractere | `'A'`, `u8'A'`, `u'ç'`, `U'😀'` | tipo/encoding pelo prefixo |
| string | `"abc"`, `u8"abc"`, `R"(raw)"` | array const com terminador |
| boolean | `true`, `false` | `bool` |
| pointer | `nullptr` | `std::nullptr_t` |
| user-defined | `10ms`, `"abc"s` | operator literal visível |

Literal decimal sem suffix escolhe primeira opção signed que comporte segundo lista normativa; bases não decimais também consideram unsigned. Não presuma tipo de literal grande.

> **Fonte oficial:** [N4950, Literals e User-defined literals](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 28.5 Operações prontas por domínio

[⬆️ Voltar ao Sumário](#sumário)

| Domínio | Recursos padrão |
|---|---|
| texto | `string`, `string_view`, `format`, `print`, `charconv` |
| matemática | `<cmath>`, `<numbers>`, `<numeric>`, `complex`, `valarray` |
| bits | `<bit>`, `bitset`, `byteswap`, `popcount`, rotations |
| tempo | `chrono`, calendars, time zones |
| arquivos | `fstream`, `filesystem` |
| aleatoriedade | engines/distributions em `<random>` |
| erro | exceptions, `error_code`, `expected`, `optional` |
| tipos heterogêneos | `tuple`, `variant`, `any` |
| memória | smart pointers, allocators, PMR, uninitialized algorithms |
| concorrência | threads, locks, atomics, futures, barriers |
| source/tooling | `source_location`, `stacktrace` |

Não use `rand()` para segurança ou simulação de qualidade. Escolha engine e distribution de `<random>` e semeie conforme requisito; criptografia não está na biblioteca padrão.

> **Fonte oficial:** [N4950, Library contents](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 28.6 Containers e estruturas prontas

[⬆️ Voltar ao Sumário](#sumário)

| Estrutura necessária | Tipo padrão |
|---|---|
| array fixo | `array` |
| array dinâmico contíguo | `vector` |
| deque | `deque` |
| lista dupla/simples | `list`, `forward_list` |
| árvore ordenada set/map | `set`, `map` e variantes multi |
| hash set/map | `unordered_set`, `unordered_map` |
| flat ordered | `flat_set`, `flat_map` (C++23) |
| stack/queue/heap queue | `stack`, `queue`, `priority_queue` |
| tupla/par | `tuple`, `pair` |
| valor opcional | `optional` |
| union discriminada | `variant` |
| valor type-erased | `any` |
| view contígua | `span`, `mdspan` |
| sequência lazy | ranges/views, `generator` |

Não há árvore binária pública genérica, graph, trie, concurrent hash map ou ring buffer no C++23. Containers associativos podem internamente usar árvores, mas a estrutura não é contrato. Para estruturas ausentes, procure biblioteca mantida antes de implementar.

> **Fonte oficial:** [N4950, Containers library, Ranges e General utilities](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 28.7 Concepts, callables e contratos prontos

[⬆️ Voltar ao Sumário](#sumário)

| Necessidade | Recurso |
|---|---|
| igualdade/ordem | `same_as`, `equality_comparable`, `totally_ordered` |
| construção | `constructible_from`, `default_initializable`, `move_constructible` |
| regularidade | `semiregular`, `regular` |
| conversão | `convertible_to`, `derived_from` |
| números | `integral`, `signed_integral`, `floating_point` |
| callable | `invocable`, `regular_invocable`, `predicate`, `relation` |
| ranges | `range`, `view`, `input_range`, `contiguous_range` |
| wrappers | `function`, `move_only_function`, `reference_wrapper` |
| adaptação | `invoke`, `bind_front`, `bind_back`, `not_fn`, `mem_fn` |

Concepts semânticos exigem leis que o compilador não testa. Um tipo pode satisfazer sintaxe de `regular` e ainda violar igualdade semanticamente; testes continuam necessários.

> **Fonte oficial:** [N4950, Concepts library, Iterator concepts e Callable requirements](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 28.8 Headers essenciais

[⬆️ Voltar ao Sumário](#sumário)

| Grupo | Headers representativos |
|---|---|
| containers | `<array>`, `<vector>`, `<deque>`, `<list>`, `<map>`, `<unordered_map>`, `<set>` |
| algorithms/ranges | `<algorithm>`, `<numeric>`, `<ranges>`, `<iterator>`, `<execution>` |
| texto/I/O | `<string>`, `<string_view>`, `<format>`, `<print>`, `<charconv>`, `<iostream>`, `<fstream>` |
| utilidades | `<utility>`, `<tuple>`, `<optional>`, `<variant>`, `<expected>`, `<any>` |
| memória | `<memory>`, `<memory_resource>`, `<new>` |
| tipos/compile time | `<type_traits>`, `<concepts>`, `<limits>`, `<bit>`, `<version>` |
| tempo/arquivos | `<chrono>`, `<filesystem>` |
| erros | `<exception>`, `<stdexcept>`, `<system_error>`, `<cassert>` |
| concorrência | `<thread>`, `<stop_token>`, `<mutex>`, `<condition_variable>`, `<atomic>`, `<future>`, `<semaphore>`, `<latch>`, `<barrier>` |
| números | `<cmath>`, `<numbers>`, `<random>`, `<complex>`, `<ratio>` |
| diagnóstico | `<source_location>`, `<stacktrace>` |
| C compatibility | `<cstdint>`, `<cstddef>`, `<cstdio>`, `<cstring>` |

Inclua o header que declara o nome usado. Não dependa de outra inclusão acidental. Headers C++ `<cstdio>` etc. colocam nomes conforme regras próprias e são preferíveis às formas `.h` em código C++.

> **Fonte oficial:** [N4950, Library clauses e C library](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 28.9 Algoritmos que você não precisa reimplementar

[⬆️ Voltar ao Sumário](#sumário)

| Intenção | Procure |
|---|---|
| busca | `find`, `find_if`, `binary_search`, `lower_bound` |
| ordenação | `sort`, `stable_sort`, `partial_sort`, `nth_element` |
| seleção/contagem | `count`, `all_of`, `any_of`, `none_of` |
| transformação | `transform`, ranges views |
| remoção lógica | `remove_if`, `unique`, `erase_if` |
| sets ordenados | `set_union`, `set_intersection`, `includes` |
| heap | `make_heap`, `push_heap`, `pop_heap` |
| partição | `partition`, `stable_partition` |
| permutação | `next_permutation`, `shuffle` |
| redução/scan | `accumulate`, `reduce`, `transform_reduce`, scans |
| memory | `copy`, `move`, `uninitialized_*`, `destroy_*` |

Leia preconditions: range ordenado, overlap, iterator category, associatividade e execution policy. Um algoritmo padrão não protege contra input inválido.

> **Fonte oficial:** [N4950, Algorithms library e Numerics library](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 28.10 Como descobrir disponibilidade

[⬆️ Voltar ao Sumário](#sumário)

Fluxo recomendado:

1. procure no índice/cláusula da biblioteca do N4950;
2. confirme a versão de origem e Defect Reports;
3. verifique feature-test macro em SD-6;
4. consulte status da biblioteca (libstdc++, libc++, MSVC STL);
5. compile um probe mínimo no toolchain/target reais;
6. só então escolha fallback externo.

```cpp
#include <version>

#ifdef __cpp_lib_print
// <print> disponível no nível indicado pela macro.
#endif
```

**Leitura guiada:** incluir `<version>` reúne muitas feature-test macros sem depender de inclusão transitiva. O `#ifdef` testa presença; em código real, compare também o valor mínimo exigido, como na seção 18.4.

Sites comunitários ajudam a navegar, mas afirmação normativa deve voltar ao padrão/draft/paper e disponibilidade à documentação da implementação.

> **Fontes oficiais:** [WG21 Standards](https://www.open-std.org/jtc1/sc22/wg21/docs/standards), [SD-6](https://isocpp.org/std/standing-documents/sd-6-sg10-feature-test-recommendations), [GCC status](https://gcc.gnu.org/onlinedocs/libstdc++/manual/status.html), [libc++ status](https://libcxx.llvm.org/Status/), [MSVC conformance](https://learn.microsoft.com/cpp/overview/visual-cpp-language-conformance)

---

## Parte 29 — Ecossistema Externo: Frameworks, Bibliotecas e Ferramentas

[⬆️ Voltar ao Sumário](#sumário)

Projeto externo não “vem no C++”. Avalie versão, licença, manutenção, ABI, política de segurança, build e custo de atualização em sua plataforma.

---

### 29.1 O que é externo ao padrão

[⬆️ Voltar ao Sumário](#sumário)

São externos ao C++23:

- sockets/HTTP/TLS;
- JSON, YAML, schema RPC e ORM;
- GUI, áudio, graphics e engines;
- logging;
- unit test framework;
- CMake e package managers;
- linear algebra/GPU;
- application framework e DI container.

O fato de uma implementação distribuir algo junto não o torna ISO C++. Separe namespace, dependency manifest e documentação.

> **Fonte oficial:** [N4950, Library contents e Conforming implementations](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)

---

### 29.2 Bibliotecas fundamentais e utilitários

[⬆️ Voltar ao Sumário](#sumário)

| Projeto | Resolve | Fonte oficial |
|---|---|---|
| Boost | conjunto amplo: Asio, Beast, JSON, Graph, containers | [boost.org](https://www.boost.org/doc/) |
| Abseil | utilitários, containers, strings, time | [abseil.io](https://abseil.io/docs/cpp/) |
| Folly | infraestrutura/utilitários Meta | [GitHub oficial](https://github.com/facebook/folly) |
| EASTL | containers orientados a games | [GitHub EA](https://github.com/electronicarts/EASTL) |
| fmt | formatting que originou partes de `std::format` | [fmt.dev](https://fmt.dev/latest/) |

Verifique sobreposição com a versão da standard library. Migrar para `std` nem sempre é drop-in: contratos, ABI e features diferem.

---

### 29.3 UI, multimídia e games

[⬆️ Voltar ao Sumário](#sumário)

| Projeto | Categoria | Fonte oficial |
|---|---|---|
| Qt | UI, networking e framework de aplicação | [doc.qt.io](https://doc.qt.io/qt-6/) |
| wxWidgets | UI nativa multiplataforma | [docs.wxwidgets.org](https://docs.wxwidgets.org/) |
| JUCE | áudio e aplicações | [juce.com/learn](https://juce.com/learn/documentation/) |
| SDL | janela, input, áudio e plataforma | [wiki.libsdl.org](https://wiki.libsdl.org/SDL3/FrontPage) |
| Unreal Engine | engine e tooling | [Epic Docs](https://dev.epicgames.com/documentation/en-us/unreal-engine/) |
| Dear ImGui | immediate-mode GUI | [GitHub oficial](https://github.com/ocornut/imgui) |

Frameworks possuem object model e thread/lifetime próprios. Leia documentação da versão, não adapte apenas hábitos de `std`.

---

### 29.4 Networking, serviços e mensageria

[⬆️ Voltar ao Sumário](#sumário)

| Projeto | Uso | Fonte oficial |
|---|---|---|
| Boost.Asio | I/O assíncrona e networking | [Docs](https://www.boost.org/doc/libs/release/doc/html/boost_asio.html) |
| Boost.Beast | HTTP/WebSocket sobre Asio | [Docs](https://www.boost.org/doc/libs/release/libs/beast/) |
| gRPC | RPC e streaming | [C++ quickstart](https://grpc.io/docs/languages/cpp/) |
| libcurl | transfer protocols/HTTP client | [libcurl](https://curl.se/libcurl/) |
| ZeroMQ | messaging patterns | [Docs](https://zeromq.org/languages/cplusplus/) |
| Apache Kafka C/C++ | clients via librdkafka | [Confluent docs](https://docs.confluent.io/platform/current/clients/librdkafka/html/index.html) |

Compare event loop, executor, cancellation, TLS, proxy, DNS, backpressure e shutdown. “Async” entre bibliotecas não garante composição direta.

---

### 29.5 Serialização, dados e persistência

[⬆️ Voltar ao Sumário](#sumário)

| Projeto | Uso | Fonte oficial |
|---|---|---|
| Protocol Buffers | schema e evolução | [protobuf.dev](https://protobuf.dev/) |
| FlatBuffers | acesso orientado a buffer | [flatbuffers.dev](https://flatbuffers.dev/) |
| Cap'n Proto | schema/RPC | [capnproto.org](https://capnproto.org/cxx.html) |
| Boost.JSON | JSON | [Boost.JSON](https://www.boost.org/doc/libs/release/libs/json/) |
| nlohmann/json | JSON idiomático | [Docs](https://json.nlohmann.me/) |
| SQLite | banco embarcado C | [sqlite.org](https://www.sqlite.org/cintro.html) |
| SOCI | abstração de banco | [SOCI docs](https://soci.sourceforge.net/doc/master/) |

Valide tamanho, profundidade, desconhecidos, defaults e compatibilidade. Serialização nunca deve depender de padding/vtable/pointer do objeto.

---

### 29.6 Científico, matemática, GPU e visão

[⬆️ Voltar ao Sumário](#sumário)

| Projeto | Domínio | Fonte oficial |
|---|---|---|
| Eigen | álgebra linear template | [eigen.tuxfamily.org](https://eigen.tuxfamily.org/dox/) |
| oneMKL | matemática otimizada | [Intel Docs](https://www.intel.com/content/www/us/en/docs/onemkl/developer-reference-c/2025-0/overview.html) |
| CUDA | GPU NVIDIA | [CUDA C++ Guide](https://docs.nvidia.com/cuda/cuda-c-programming-guide/) |
| SYCL | programação heterogênea | [Khronos SYCL](https://www.khronos.org/sycl/) |
| OpenCV | visão computacional | [docs.opencv.org](https://docs.opencv.org/) |
| CGAL | geometria computacional | [doc.cgal.org](https://doc.cgal.org/) |

Alinhamento, compiler flags, vector ISA e floating model podem afetar ABI/resultado. Use matrizes oficiais de suporte.

---

### 29.7 Logging, testes e observabilidade

[⬆️ Voltar ao Sumário](#sumário)

| Projeto | Finalidade | Fonte oficial |
|---|---|---|
| spdlog | logging | [GitHub oficial](https://github.com/gabime/spdlog) |
| GoogleTest | unit/mocking | [Docs](https://google.github.io/googletest/) |
| Catch2 | testes/benchmarks | [Repositório e documentação oficiais](https://github.com/catchorg/Catch2) |
| doctest | testes leves | [GitHub oficial](https://github.com/doctest/doctest) |
| Google Benchmark | microbenchmark | [Docs](https://google.github.io/benchmark/) |
| OpenTelemetry C++ | traces, metrics e logs | [Docs](https://opentelemetry.io/docs/languages/cpp/) |

Evite log no benchmark e benchmark como teste funcional. Instrumentation deve ser configurável e não expor dados.

---

### 29.8 Build, pacotes e automação

[⬆️ Voltar ao Sumário](#sumário)

| Projeto | Papel | Fonte oficial |
|---|---|---|
| CMake | meta-build/configuração | [cmake.org](https://cmake.org/documentation/) |
| Meson | build system | [mesonbuild.com](https://mesonbuild.com/Manual.html) |
| Bazel | build monorepo/remoto | [bazel.build](https://bazel.build/docs) |
| Ninja | executor de build | [ninja-build.org](https://ninja-build.org/manual.html) |
| Conan | package manager | [docs.conan.io](https://docs.conan.io/2/) |
| vcpkg | package manager | [Microsoft Learn](https://learn.microsoft.com/vcpkg/) |
| clang-format/tidy | formatação/análise | [LLVM Extra Tools](https://clang.llvm.org/extra/) |

Gerador de build, executor e package manager são camadas diferentes. Integre por targets/manifests oficiais e evite scripts que codificam paths locais.

---

### 29.9 Como avaliar e adotar uma dependência

[⬆️ Voltar ao Sumário](#sumário)

Checklist:

1. requisito que a biblioteca resolve;
2. API, ownership, thread-safety e error model;
3. plataformas, compilers e standard modes suportados;
4. licença e notices;
5. cadence, maintainers e security policy;
6. tamanho, compile time, dependencies e ABI;
7. integração CMake/package manager;
8. testes e atualização;
9. plano de remoção/migração;
10. versão e hashes fixados.

Faça spike pequeno e benchmark apenas se custo for requisito. Encapsule dependência volátil atrás de adapter sem esconder toda capacidade útil.

> **Fontes oficiais de processo:** use sempre site, manual, release notes, repositório e política de segurança mantidos pelo próprio projeto; não escolha versão por artigo de terceiros. Para integridade da cadeia, consulte também a [especificação SLSA](https://slsa.dev/spec/) e a documentação oficial do package manager adotado.

---

## Anexo A — Trilhas Oficiais de Estudo e Prática

[⬆️ Voltar ao Sumário](#sumário)

| Recurso | Uso recomendado |
|---|---|
| [ISO C++ — Get Started](https://isocpp.org/get-started) | orientação oficial da Standard C++ Foundation |
| [ISO/IEC 14882:2024](https://www.iso.org/standard/83626.html) | padrão internacional vigente |
| [WG21 N4950](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf) | draft final público do C++23 |
| [WG21 N5050](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2026/n5050.pdf) | draft final do C++26 e base do DIS, usado para antecipações |
| [WG21 Papers](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/) | propostas, reports e histórico |
| [C++ Core Guidelines](https://isocpp.github.io/CppCoreGuidelines/CppCoreGuidelines) | práticas modernas com rationale |
| [GCC Documentation](https://gcc.gnu.org/onlinedocs/) | flags, extensions e toolchain GCC |
| [Clang Documentation](https://clang.llvm.org/docs/) | compiler, sanitizers e tools LLVM |
| [MSVC C++ Documentation](https://learn.microsoft.com/cpp/) | compiler, STL, debugger e Windows |
| [CMake Documentation](https://cmake.org/documentation/) | build baseado em targets |

Prática sugerida:

1. compile com warnings altos em dois compilers;
2. explique lifetime e ownership de cada pointer/view;
3. rode ASan/UBSan e uma configuração otimizada;
4. inspecione símbolos/object files com ferramentas da plataforma;
5. substitua loop manual por algoritmo/range quando o contrato melhorar;
6. escreva um type Rule of Zero e um move-only RAII handle;
7. provoque falhas de input e I/O;
8. leia a cláusula/paper quando comportamento surpreender.

---

## Anexo B — Referências Oficiais Consultadas

[⬆️ Voltar ao Sumário](#sumário)

### Linguagem, biblioteca e evolução

- [ISO/IEC 14882:2024 — Programming languages — C++](https://www.iso.org/standard/83626.html)
- [WG21 N4950 — Working Draft final do C++23](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4950.pdf)
- [WG21 N4951 — Editors’ Report do C++23](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2023/n4951.html)
- [WG21 N5050 — Working Draft final do C++26](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2026/n5050.pdf)
- [WG21 N5051 — Editors’ Report do draft final do C++26](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2026/n5051.html)
- [WG21 N5054 — Working Draft corrente do C++29 em 16/07/2026](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2026/n5054.pdf)
- [WG21 N5055 — Editors’ Report do C++29](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/2026/n5055.html)
- [WG21 Standards](https://www.open-std.org/jtc1/sc22/wg21/docs/standards)
- [WG21 Papers and Mailings](https://www.open-std.org/jtc1/sc22/wg21/docs/papers/)
- [WG21 Core Language Issues](https://www.open-std.org/jtc1/sc22/wg21/docs/cwg_active.html)
- [WG21 Library Issues](https://cplusplus.github.io/LWG/lwg-active.html)
- [SD-6 Feature-test Recommendations](https://isocpp.org/std/standing-documents/sd-6-sg10-feature-test-recommendations)
- [C++ Core Guidelines](https://isocpp.github.io/CppCoreGuidelines/CppCoreGuidelines)

### Implementações e toolchains

- [GCC C++ Status](https://gcc.gnu.org/projects/cxx-status.html)
- [GCC Manual](https://gcc.gnu.org/onlinedocs/)
- [libstdc++ Status](https://gcc.gnu.org/onlinedocs/libstdc++/manual/status.html)
- [Clang C++ Status](https://clang.llvm.org/cxx_status.html)
- [Clang Documentation](https://clang.llvm.org/docs/)
- [libc++ Status](https://libcxx.llvm.org/Status/)
- [Microsoft C++ Conformance](https://learn.microsoft.com/cpp/overview/visual-cpp-language-conformance)
- [Microsoft C++ Documentation](https://learn.microsoft.com/cpp/)
- [CMake Documentation](https://cmake.org/documentation/)
- [Conan Documentation](https://docs.conan.io/2/)
- [vcpkg Documentation](https://learn.microsoft.com/vcpkg/)

### Segurança, qualidade e plataforma

- [SEI CERT C++ Coding Standard](https://wiki.sei.cmu.edu/confluence/pages/viewpage.action?pageId=88046682)
- [Clang Sanitizers](https://clang.llvm.org/docs/index.html#sanitizers)
- [clang-tidy](https://clang.llvm.org/extra/clang-tidy/)
- [GoogleTest](https://google.github.io/googletest/)
- [Doxygen](https://www.doxygen.nl/manual/)
- [POSIX Base Specifications](https://pubs.opengroup.org/onlinepubs/9799919799/)
- [Microsoft Windows Native Development](https://learn.microsoft.com/windows/win32/)
- [Itanium C++ ABI](https://itanium-cxx-abi.github.io/cxx-abi/abi.html)

Para cada biblioteca da Parte 29, foi indicado o manual, site ou repositório mantido pelo próprio projeto. Essas fontes definem versões, licenças e compatibilidade que o padrão ISO C++ não controla.

---

## Glossário

[⬆️ Voltar ao Sumário](#sumário)

| Termo | Definição resumida | Aprofundamento |
|---|---|---|
| **ABI** | contrato binário de layout, símbolos, chamadas, exceptions e runtime | [22.5](#225-abi-bibliotecas-compartilhadas-e-visibilidade) |
| **ADL** | lookup que considera namespaces/classes associados aos argumentos | [2.3](#23-namespaces-using-e-adl) |
| **aggregate** | classe/array que satisfaz regras para aggregate initialization | [6.5](#65-aggregates-designated-initializers-e-structured-bindings) |
| **allocator** | política/objeto usado por containers para obter storage | [19.4](#194-allocators-e-pmrmemory_resource) |
| **argument** | expressão fornecida em uma chamada | [5.1](#51-declaração-definição-assinatura-e-retorno) |
| **array** | sequência contígua de tamanho fixo de elementos | [3.7](#37-arrays-stdarray-e-stdspan) |
| **atomic** | objeto/operação com garantias indivisíveis e memory ordering | [21.4](#214-atomics-data-races-e-memory-ordering) |
| **borrowed range** | range cujo iterator pode continuar válido após destruição do objeto range conforme concept | [14.4](#144-views-lazy-evaluation-e-dangling) |
| **callable** | entidade que pode ser invocada | [Parte 13](#parte-13--callables-lambdas-e-callbacks) |
| **class file** | termo de Java, não artefato padrão C++; C++ usa object files conforme toolchain | [1.2](#12-compilador-linker-runtime-e-implementação) |
| **closure** | objeto gerado por expressão lambda, contendo capturas | [13.1](#131-lambdas-e-capturas) |
| **concept** | predicate nomeado sobre template arguments e seus requisitos | [17.5](#175-concepts-e-requires-expressions) |
| **constant expression** | expressão avaliável nas regras de constant evaluation | [18.1](#181-constexpr-consteval-e-constinit) |
| **coroutine** | função transformada que pode suspender e retomar | [22.1](#221-modelo-de-coroutines) |
| **CTAD** | class template argument deduction | [17.2](#172-dedução-ctad-e-deduction-guides) |
| **dangling** | pointer/reference/iterator/view cujo alvo já não é utilizável | [7.5](#75-temporários-extensão-de-lifetime-e-dangling) |
| **data race** | acessos conflitantes não ordenados, ao menos um write, sem atomic apropriado | [21.4](#214-atomics-data-races-e-memory-ordering) |
| **declaration** | introduz/redeclara nome e propriedades de entidade | [2.1](#21-declarações-definições-unidades-de-tradução-e-odr) |
| **definition** | declaração que define entidade conforme sua categoria | [2.1](#21-declarações-definições-unidades-de-tradução-e-odr) |
| **destructor** | função especial executada no encerramento do lifetime de class object | [6.3](#63-destrutores-e-raii) |
| **dynamic storage** | storage obtido por allocation dinâmica | [19.1](#191-newdelete-e-por-que-evitá-los-diretamente) |
| **erroneous behavior** | categoria do draft C++26: comportamento bem definido causado por código incorreto, cujo diagnóstico é recomendado | [20.3](#203-undefined-unspecified-e-implementation-defined) |
| **exception guarantee** | promessa sobre estado/efeitos quando operação lança | [16.4](#164-garantias-de-exception-safety) |
| **feature-test macro** | macro padronizada que indica nível de suporte a recurso | [18.4](#184-feature-test-macros) |
| **forwarding reference** | `T&&` em contexto de dedução que preserva categoria por forwarding | [9.4](#94-perfect-forwarding) |
| **glvalue** | expressão com identidade: lvalue ou xvalue | [9.1](#91-lvalue-prvalue-xvalue-e-glvalue) |
| **handle** | valor que identifica recurso externo ou gerenciado | [7.3](#73-ownership-e-handles) |
| **header** | unidade nomeada importada por include/header unit; não necessariamente arquivo físico | [2.4](#24-headers-fontes-e-forward-declarations) |
| **IFNDR** | ill-formed, no diagnostic required | [20.3](#203-undefined-unspecified-e-implementation-defined) |
| **implementation-defined** | implementação escolhe e documenta comportamento | [20.3](#203-undefined-unspecified-e-implementation-defined) |
| **invalidation** | operação torna iterator/pointer/reference/view não utilizável | [15.5](#155-invalidação-complexidade-e-escolha) |
| **iterator** | objeto que navega/indica posição em sequência | [14.1](#141-iteradores-sentinels-e-categorias) |
| **lifetime** | período em que propriedades de um objeto/reference aplicam-se | [Parte 7](#parte-7--lifetime-raii-e-recursos) |
| **linker** | ferramenta que resolve símbolos e forma binário | [1.2](#12-compilador-linker-runtime-e-implementação) |
| **lvalue** | glvalue que não é xvalue | [9.1](#91-lvalue-prvalue-xvalue-e-glvalue) |
| **module** | unidade semântica C++20 com declarations exportadas/importadas | [2.5](#25-módulos-e-header-units) |
| **move semantics** | construção/atribuição que pode transferir recursos de rvalue | [9.2](#92-copy-e-move-semantics) |
| **NRVO** | elision permitida ao retornar variável local nomeada | [9.5](#95-copy-elision-rvo-e-nrvo) |
| **object** | região de storage com tipo, lifetime e propriedades associadas | [3.2](#32-objetos-valores-escopo-storage-duration-e-lifetime) |
| **ODR** | One Definition Rule que governa definições no programa | [2.1](#21-declarações-definições-unidades-de-tradução-e-odr) |
| **ownership** | responsabilidade por encerrar lifetime/liberar recurso | [7.3](#73-ownership-e-handles) |
| **parameter** | entidade declarada pela função para receber argumento | [5.1](#51-declaração-definição-assinatura-e-retorno) |
| **pointer** | valor que aponta para objeto/função ou representa nulo/past-end conforme regras | [3.6](#36-ponteiros-referências-e-nullptr) |
| **preprocessor** | fases/diretivas de macros, inclusão e compilação condicional | [2.2](#22-include-include-guards-e-macros) |
| **prvalue** | expressão cuja avaliação inicializa objeto/bit-field ou calcula valor | [9.1](#91-lvalue-prvalue-xvalue-e-glvalue) |
| **RAII** | ownership ligado à inicialização e destruição de objeto | [7.2](#72-raii-e-destruição-determinística) |
| **range** | sequência definida por início e sentinel/fim | [Parte 14](#parte-14--iteradores-algoritmos-e-ranges) |
| **reference** | alias ligado a objeto/função segundo regras de binding | [3.6](#36-ponteiros-referências-e-nullptr) |
| **RTTI** | informações/runtime operations como `typeid` e `dynamic_cast` | [12.3](#123-slicing-downcast-e-rtti) |
| **sanitizer** | instrumentação dinâmica para classes de erros | [25.6](#256-sanitizers-debugger-e-profiling) |
| **scope** | região em que nome pode ser encontrado | [3.2](#32-objetos-valores-escopo-storage-duration-e-lifetime) |
| **SFINAE** | substitution failure is not an error em contextos especificados | [17.6](#176-sfinae-nomes-dependentes-e-two-phase-lookup) |
| **storage duration** | duração mínima do storage de objeto | [7.1](#71-storage-duration-não-é-lifetime) |
| **template** | família parametrizada de entidades | [Parte 17](#parte-17--templates-concepts-e-programação-genérica) |
| **translation unit** | fonte após inclusões/conditional preprocessing conforme modelo | [2.1](#21-declarações-definições-unidades-de-tradução-e-odr) |
| **type erasure** | esconde tipo concreto atrás de interface uniforme | [12.5](#125-composição-type-erasure-e-crtp) |
| **undefined behavior** | comportamento para o qual o padrão não impõe requisitos | [20.3](#203-undefined-unspecified-e-implementation-defined) |
| **unspecified behavior** | escolha permitida sem obrigação de documentação | [20.3](#203-undefined-unspecified-e-implementation-defined) |
| **value category** | classificação de expressão como glvalue/prvalue e subcategorias | [9.1](#91-lvalue-prvalue-xvalue-e-glvalue) |
| **view** | objeto normalmente não owning que apresenta dados/range | [14.4](#144-views-lazy-evaluation-e-dangling) |
| **xvalue** | glvalue que denota objeto cujos recursos podem ser reutilizados | [9.1](#91-lvalue-prvalue-xvalue-e-glvalue) |

**Como usar a tabela:** as definições são deliberadamente curtas. Consulte a seção vinculada e depois o N4950/N5050 para wording normativo e requisitos completos.

---
