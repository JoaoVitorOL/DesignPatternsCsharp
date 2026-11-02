<a id="inicio"></a>
# Design Pattern: Builder

## Sumário
[↑ Voltar ao início](#inicio)

- [1. O que o Builder representa?](#sec-001)
  - [O Problema que ele resolve:](#sec-002)
- [2. Em que situações se usa?](#sec-003)
- [3. Instruções passo a passo de como implementar (em Java)](#sec-004)
  - [Exemplo básico de implementação:](#sec-005)
- [4. Explicação detalhada do Exemplo Prático (Montagem de Lanches)](#sec-006)
  - [A Classe Produto: Lanche e sua classe interna Builder](#sec-007)
  - [A Classe Opcional: LancheDirector](#sec-008)
  - [A Execução: Main](#sec-009)
- [5. Como o padrão aparece nos exemplos C# deste repositório](#sec-010)
- [6. Acompanhe a construção como estudante](#sec-011)
- [Decisão de design: Builder ou construtor comum?](#sec-012)
- [Variações e aprofundamentos](#sec-013)
- [13. Fluent Builder](#sec-014)
  - [13.1 A diferença central para o builder tradicional](#sec-015)
  - [13.2 O ponto de virada da API](#sec-016)
  - [13.3 O que muda e o que não muda](#sec-017)
- [14. Fluent Builder Inheritance with Recursive Generics](#sec-018)
  - [14.1 O que dá errado na herança simples](#sec-019)
  - [14.2 Como recursive generics corrige isso](#sec-020)
  - [14.3 Explicando o trecho mais importante](#sec-021)
  - [14.4 Como o ciclo fecha no exemplo](#sec-022)
- [15. Stepwise Builder](#sec-023)
  - [15.1 Qual problema ele resolve](#sec-024)
  - [15.2 Como isso aparece no exemplo do carro](#sec-025)
  - [15.3 O ponto central da técnica](#sec-026)
  - [15.4 Objeto real vs tipo da referência](#sec-027)
  - [15.5 Por que existe uma classe Impl](#sec-028)
  - [15.6 Onde a validação entra](#sec-029)
  - [15.7 O que ele enfatiza](#sec-030)
  - [15.8 Quando ele faz sentido](#sec-031)
  - [15.9 Custo dessa abordagem](#sec-032)
- [16. Functional Builder](#sec-033)
  - [16.1 O que muda em relação ao builder normal](#sec-034)
  - [16.2 O que muda em relação ao Stepwise Builder](#sec-035)
  - [16.3 Como isso aparece na anatomia do código](#sec-036)
  - [16.4 O papel de Do(), AddAction() e Build()](#sec-037)
  - [16.5 Onde a extensibilidade aparece](#sec-038)
  - [16.6 Diferença direta para as aulas anteriores](#sec-039)
  - [16.7 Quando ele ajuda](#sec-040)
  - [16.8 Cuidado didático](#sec-041)
- [17. Faceted Builder](#sec-042)
  - [17.1 O que ele enfatiza](#sec-043)
  - [17.2 Exemplo mental](#sec-044)
  - [17.3 Leitura correta](#sec-045)
  - [17.4 O que o torna diferente das aulas anteriores](#sec-046)
  - [17.5 O mecanismo técnico do exemplo](#sec-047)
- [Coding Exercise 1: Builder Coding Exercise](#sec-048)
  - [O que o exercício quer de verdade](#sec-049)
  - [Como ler o uso esperado](#sec-050)
  - [O que é o produto neste exercício](#sec-051)
  - [Participantes do padrão no exercício](#sec-052)
  - [O que faz este exercício ser um builder](#sec-053)
  - [Onde a fluência aparece](#sec-054)
  - [O que o aluno precisa enxergar para conseguir reproduzir sozinho](#sec-055)
  - [O que costuma confundir aqui](#sec-056)
  - [Como validar mentalmente se a solução está correta](#sec-057)


**Família GoF:** criacional. **Escopo predominante:** objetos e composição de etapas.

<a id="sec-001"></a>
## 1. O que o Builder representa?

[↑ Voltar ao início](#inicio)
O padrão **Builder** é um padrão de projeto criacional que tem como objetivo **separar a construção de um objeto complexo da sua representação**, permitindo que o mesmo processo de construção crie diferentes tipos e representações de objetos.

<a id="sec-002"></a>
### O Problema que ele resolve:

[↑ Voltar ao início](#inicio)
Em muitos sistemas, precisamos criar objetos com muitos atributos (vários deles opcionais ou com combinações diferentes), como um cadastro completo, uma configuração avançada ou um pedido de e-commerce. Sem o Builder, enfrentamos problemas comuns:
- **Construtor telescópico:** Construtores com dezenas de parâmetros que tornam o código difícil de ler e propenso a erros de troca de ordem de parâmetros.
- **Muitos setters soltos:** Deixam o objeto em um estado inconsistente no meio da montagem e dificultam garantir que tudo o que era necessário foi configurado.
- **Variações de construção espalhadas:** A lógica de como montar diferentes variações do objeto se espalha por todo o código cliente.

---

<a id="sec-003"></a>
## 2. Em que situações se usa?

[↑ Voltar ao início](#inicio)
Use o Builder quando:
1. **Muitos parâmetros:** Um objeto tiver muitos parâmetros, especialmente opcionais ou com combinações complexas.
2. **Evitar construtores gigantescos:** Quiser deixar a inicialização do código mais limpa, legível e segura (com API fluente).
3. **Múltiplas representações:** Você precisar construir diferentes versões ou variações do mesmo tipo de objeto usando o mesmo processo.
4. **Processo centralizado:** O processo de construção precisar ser reutilizável, isolando a lógica de validação e montagem.

---

<a id="sec-004"></a>
## 3. Instruções passo a passo de como implementar (em Java)

[↑ Voltar ao início](#inicio)

1. **Tornar o construtor do produto privado:** Garante que o objeto só possa ser instanciado através do Builder.
2. **Criar uma classe estática interna (Builder):** Com os mesmos atributos do produto para receber os valores passo a passo.
3. **Criar métodos de configuração fluentes:** Métodos que definem cada atributo e retornam `this` (o próprio Builder) para permitir o encadeamento de chamadas.
4. **Criar o método `build()`:** Responsável por validar as regras de negócio e instanciar o produto final repassando o Builder.

<a id="sec-005"></a>
### Exemplo básico de implementação:

[↑ Voltar ao início](#inicio)
```java
public class Lanche {
    private final String pao;
    private final String proteina;

    private Lanche(Builder builder) {
        this.pao = builder.pao;
        this.proteina = builder.proteina;
    }

    public static class Builder {
        private String pao;
        private String proteina;

        public Builder setPao(String pao) {
            this.pao = pao;
            return this;
        }

        public Builder setProteina(String proteina) {
            this.proteina = proteina;
            return this;
        }

        public Lanche build() {
            if (pao == null || proteina == null) {
                throw new IllegalStateException("Parâmetros obrigatórios ausentes!");
            }
            return new Lanche(this);
        }
    }
}
```

---

<a id="sec-006"></a>
## 4. Explicação detalhada do Exemplo Prático (Montagem de Lanches)

[↑ Voltar ao início](#inicio)

O código fornecido exemplifica o padrão **Builder** aplicado em um contexto de uma lanchonete, combinando o uso de uma **API Fluente** com a figura de um **Director** para orquestrar receitas prontas.

<a id="sec-007"></a>
### A Classe Produto: `Lanche` e sua classe interna `Builder`

[↑ Voltar ao início](#inicio)
A classe `Lanche` possui campos obrigatórios e opcionais (como queijo, vegetais, molhos e observações). O construtor é privado, aceitando apenas o `Builder`.
- O `Builder` usa métodos encadeados (`setPao`, `setProteina`, `addVegetal`, etc.) para configurar o lanche passo a passo.
- O método `build()` realiza validações essenciais (como garantir que pão e proteína foram informados) antes de retornar o objeto final construído.

```java
import java.util.ArrayList;
import java.util.Collections;
import java.util.List;

public class Lanche {
    private final String pao;
    private final String proteina;
    private final String queijo;
    private final List<String> vegetais;
    private final String molho;
    private final boolean bemPassado;
    private final String observacoes;

    private Lanche(Builder builder) {
        this.pao = builder.pao;
        this.proteina = builder.proteina;
        this.queijo = builder.queijo;
        this.vegetais = Collections.unmodifiableList(new ArrayList<>(builder.vegetais));
        this.molho = builder.molho;
        this.bemPassado = builder.bemPassado;
        this.observacoes = builder.observacoes;
    }

    // Getters omitidos para brevidade...

    @Override
    public String toString() {
        String vegetaisStr = vegetais.isEmpty() ? "Nenhum" : String.join(", ", vegetais);
        return "Lanche [Pão: " + pao + ", Proteína: " + proteina + ", Queijo: " + (queijo != null ? queijo : "Não") +
               ", Vegetais: [" + vegetaisStr + "], Molho: " + (molho != null ? molho : "Nenhum") +
               ", Bem Passado: " + (bemPassado ? "Sim" : "Não") + ", Observações: " + (observacoes != null ? observacoes : "Nenhuma") + "]";
    }

    public static class Builder {
        private String pao;
        private String proteina;
        private String queijo;
        private List<String> vegetais = new ArrayList<>();
        private String molho;
        private boolean bemPassado;
        private String observacoes;

        public Builder setPao(String pao) {
            this.pao = pao;
            return this;
        }

        public Builder setProteina(String proteina) {
            this.proteina = proteina;
            return this;
        }

        public Builder setQueijo(String queijo) {
            this.queijo = queijo;
            return this;
        }

        public Builder addVegetal(String vegetal) {
            this.vegetais.add(vegetal);
            return this;
        }

        public Builder setMolho(String molho) {
            this.molho = molho;
            return this;
        }

        public Builder setBemPassado(boolean bemPassado) {
            this.bemPassado = bemPassado;
            return this;
        }

        public Builder setObservacoes(String observacoes) {
            this.observacoes = observacoes;
            return this;
        }

        public Lanche build() {
            if (pao == null || pao.trim().isEmpty()) {
                throw new IllegalStateException("Erro de validação: O campo obrigatório 'pao' não foi informado.");
            }
            if (proteina == null || proteina.trim().isEmpty()) {
                throw new IllegalStateException("Erro de validação: O campo obrigatório 'proteina' não foi informado.");
            }
            return new Lanche(this);
        }
    }
}
```

---

<a id="sec-008"></a>
### A Classe Opcional: `LancheDirector`

[↑ Voltar ao início](#inicio)
O `Director` funciona como a "cozinha" que conhece receitas pré-definidas. Ele recebe um `Builder` e executa uma sequência padrão de chamadas para entregar sanduíches específicos (como Misto, X-Salada ou Especial da Casa) sem que o cliente precise se preocupar com os detalhes de montagem.

```java
public class LancheDirector {
    private final Lanche.Builder builder;

    public LancheDirector(Lanche.Builder builder) {
        this.builder = builder;
    }

    public Lanche criarMisto() {
        return builder
            .setPao("Pão de Forma")
            .setProteina("Presunto")
            .setQueijo("Mussarela")
            .setBemPassado(false)
            .build();
    }

    public Lanche criarXSalada() {
        return builder
            .setPao("Pão Hambúrguer")
            .setProteina("Hambúrguer Bovino")
            .setQueijo("Prato")
            .addVegetal("Alface")
            .addVegetal("Tomate")
            .setMolho("Maionese da Casa")
            .setBemPassado(true)
            .build();
    }

    public Lanche criarEspecialDaCasa() {
        return builder
            .setPao("Pão Australiano")
            .setProteina("Costela Desfiada")
            .setQueijo("Cheddar")
            .addVegetal("Cebola Caramelizada")
            .addVegetal("Rúcula")
            .setMolho("Molho Barbecue")
            .setBemPassado(true)
            .setObservacoes("Aquecer bem o pão antes de montar.")
            .build();
    }
}
```

---

<a id="sec-009"></a>
### A Execução: `Main`

[↑ Voltar ao início](#inicio)
A classe principal demonstra as diferentes formas de uso do padrão:
1. **Montagem via API Fluente (Apenas Obrigatórios):** Criação direta de um lanche simples utilizando apenas os métodos essenciais do builder.
2. **Montagem via API Fluente (Completo):** Encadeamento de múltiplos opcionais (queijos, múltiplos vegetais, molhos e observações).
3. **Geração através do Director:** Utilização de receitas prontas encapsuladas no `LancheDirector`.
4. **Teste de Validação:** Demonstração do tratamento de erro quando um campo obrigatório (como a proteína) é omitido, capturando a exceção lançada pelo método `build()`.

```java
package lanchonete;

public class Main {
    public static void main(String[] args) {
        System.out.println("=== 1. MONTAGEM VIA API FLUENTE (APENAS OBRIGATÓRIOS) ===");
        try {
            Lanche lancheSimples = new Lanche.Builder()
                .setPao("Pão Francês")
                .setProteina("Ovo")
                .build();

            System.out.println(lancheSimples);
        } catch (Exception ex) {
            System.out.println(ex.getMessage());
        }

        System.out.println("\n=== 2. MONTAGEM VIA API FLUENTE (COMPLETO) ===");
        Lanche lanchePersonalizado = new Lanche.Builder()
            .setPao("Pão Integral")
            .setProteina("Frango Grelhado")
            .setQueijo("Minas Frescal")
            .addVegetal("Alface")
            .addVegetal("Tomate")
            .addVegetal("Cenoura Ralada")
            .setMolho("Iogurte com Ervas")
            .setBemPassado(true)
            .setObservacoes("Cortar o lanche ao meio.")
            .build();

        System.out.println(lanchePersonalizado);

        System.out.println("\n=== 3. GERAÇÃO DE LANCHES ATRAVÉS DO DIRECTOR ===");
        LancheDirector director1 = new LancheDirector(new Lanche.Builder());
        System.out.println(director1.criarMisto());

        LancheDirector director2 = new LancheDirector(new Lanche.Builder());
        System.out.println(director2.criarXSalada());

        LancheDirector director3 = new LancheDirector(new Lanche.Builder());
        System.out.println(director3.criarEspecialDaCasa());

        System.out.println("\n=== 4. TESTE DE VALIDAÇÃO (FALHA ESPERADA) ===");
        try {
            System.out.println("Tentando montar um lanche sem informar a proteína...");
            Lanche lancheInvalido = new Lanche.Builder()
                .setPao("Pão Francês")
                .setQueijo("Mussarela")
                .build();
        } catch (IllegalStateException ex) {
            System.out.println("[EXCEÇÃO CAPTURADA COM SUCESSO]: " + ex.getMessage());
        }
    }
}
```


<a id="sec-010"></a>
## 5. Como o padrão aparece nos exemplos C# deste repositório

[↑ Voltar ao início](#inicio)

Os exemplos conceituais anteriores usam um lanche; os arquivos de aula mostram o mesmo propósito em várias formas:

- [SemBuilder.cs](Aula01_builder/SemBuilder.cs) contrasta a construção sem uma API dedicada. [Builder.cs](Aula01_builder/Builder.cs) concentra os passos de criação.
- [fluentBuilder.cs](Aula02_FluentBuilder/fluentBuilder.cs) retorna o próprio builder para encadear chamadas. Os arquivos [CertoFluentBuilderWithRecursiveGenerics.cs](Aula02_FluentBuilder/CertoFluentBuilderWithRecursiveGenerics.cs) e [ErradoFluentBuilderWithRecursiveGenerics.cs](Aula02_FluentBuilder/ErradoFluentBuilderWithRecursiveGenerics.cs) comparam a preservação do tipo concreto em builders herdados.
- [StepWiseBuilder.cs](Aula03_StepWiseBuilder/StepWiseBuilder.cs) representa cada etapa por um tipo de interface para restringir a ordem de chamadas.
- [FunctionalBuilder.cs](Aula04_FunctionalBuilder/FunctionalBuilder.cs) acumula ações que configuram o produto; [FacetedBuilder.cs](Aula05_FacetedBuilder/FacetedBuilder.cs) separa a configuração em facetas relacionadas.
- O exercício corrigido em [BuilderExcercise.cs](Aula06_BuilderExcercise/BuilderExcercise.cs) constrói a representação textual de uma classe C#. CodeBuilder guarda CodeClass, AddField adiciona CodeField e retorna this; CodeClass.ToString() gera as linhas finais.

No exercício, o cliente escreve CodeBuilder("Person").AddField("Name", "string").AddField("Age", "int"). O builder permite descrever o resultado em etapas e só então renderizá-lo. A API não tenta construir código arbitrário: seu produto é uma classe com nome e lista de campos.

As variações demonstram escolhas diferentes, não requisitos que devam ser combinados sempre. Use builder quando a construção tem etapas/opções suficientes para justificar uma API dedicada.


<a id="sec-011"></a>
## 6. Acompanhe a construção como estudante

[↑ Voltar ao início](#inicio)

Imagine que o programa precisa gerar uma classe de código com nome e campos. Sem Builder, o cliente teria de controlar a string inteira, as quebras de linha, a indentação e a ordem dos campos. Essa mistura de decisões torna fácil produzir uma classe inválida e espalha formatação pelo chamador.

Em [BuilderExcercise.cs](Aula06_BuilderExcercise/BuilderExcercise.cs), o produto é CodeClass, que contém o nome e uma lista de CodeField. CodeBuilder é a API de montagem: recebe o nome obrigatório, AddField acrescenta um campo e retorna o próprio builder para manter o encadeamento. No final, ToString delega para CodeClass.ToString, responsável por formatar o resultado. A sequência é: descrever o que se quer → acumular as partes → obter o produto final.

As aulas anteriores mostram que a interface do builder pode fazer mais do que encadear. StepWiseBuilder restringe etapas obrigatórias pelo tipo retornado; FunctionalBuilder acumula ações para aplicar ao construir; FacetedBuilder separa grupos de configuração do mesmo produto. Escolha a variação pela necessidade: ordem válida, composição de instruções ou API grande dividida por assunto.

O Builder não é necessário para todo construtor. Ele compensa quando há muitos opcionais, uma sequência de configuração, validação ou formatação final que o cliente não deveria repetir.


<a id="sec-012"></a>
## Decisão de design: Builder ou construtor comum?

[↑ Voltar ao início](#inicio)

Use um construtor comum quando o objeto tem poucos campos obrigatórios e os argumentos são fáceis de distinguir. Quando o construtor passa a receber muitos booleanos, nulls e valores opcionais, o chamador precisa memorizar a ordem e o significado de cada argumento. Builder troca essa assinatura difícil por passos nomeados, validação e uma etapa explícita de finalização.

No exemplo CodeBuilder, AddField comunica claramente qual parte está sendo adicionada, e o cliente não precisa saber como CodeClass renderiza indentação. Em objetos de domínio, Build() pode validar invariantes antes de devolver o objeto; aqui, ToString() serve como materialização do texto final. A classe Builder não é automaticamente responsável por toda validação: decida se cada passo pode validar imediatamente ou se a validação só faz sentido no produto completo.

Evite um Builder com dezenas de setters equivalentes a atribuir propriedades uma por uma quando a construção é trivial. Também evite manter o builder mutável compartilhado entre threads ou reutilizá-lo acidentalmente para criar produtos com estado misturado.

<a id="sec-013"></a>
## Variações e aprofundamentos

[↑ Voltar ao início](#inicio)

As seções abaixo reúnem variações, exemplos e decisões que complementam a explicação principal deste padrão. Elas preservam a linguagem e o contexto do material didático original.

<a id="sec-014"></a>
## 13. Fluent Builder

[↑ Voltar ao início](#inicio)


Esta parte corresponde ao arquivo `CreationalPatterns/2.Builder/Aula02_FluentBuilder/fluentBuilder.cs`.

O `Fluent Builder` continua sendo `Builder`. O que muda é a forma da API.

<a id="sec-015"></a>
### 13.1 A diferença central para o builder tradicional

[↑ Voltar ao início](#inicio)

No builder tradicional:

- o método constrói;
- mas retorna `void`.

No fluent builder:

- o método constrói;
- e devolve o próprio builder.

Exemplo:

```csharp
builder
    .AddChildFluent("li", "hello")
    .AddChildFluent("li", "world")
    .AddChildFluent("li", "XD");
```

<a id="sec-016"></a>
### 13.2 O ponto de virada da API

[↑ Voltar ao início](#inicio)

O fluent builder nasce quando a assinatura muda:

- de `void AddChild(...)`
- para `HtmlBuilder AddChildFluent(...)`

e o método termina com:

```csharp
return this;
```

Esse `return this` devolve a instância atual do builder. Com isso, a próxima chamada pode continuar na mesma expressão.

<a id="sec-017"></a>
### 13.3 O que muda e o que não muda

[↑ Voltar ao início](#inicio)

O que muda:

- legibilidade da montagem;
- encadeamento;
- ergonomia da API.

O que não muda:

- o fato de continuar sendo um builder;
- a ideia de montar o mesmo produto final;
- o papel do cliente em descrever o que quer construir.

---

<a id="sec-018"></a>
## 14. Fluent Builder Inheritance with Recursive Generics

[↑ Voltar ao início](#inicio)


Esta parte corresponde aos arquivos:

- `CreationalPatterns/2.Builder/Aula02_FluentBuilder/ErradoFluentBuilderWithRecursiveGenerics.cs`
- `CreationalPatterns/2.Builder/Aula02_FluentBuilder/CertoFluentBuilderWithRecursiveGenerics.cs`

Aqui o problema deixa de ser apenas "retornar `this`". A questão passa a ser:

**como manter a fluência quando um builder herda de outro builder?**

<a id="sec-019"></a>
### 14.1 O que dá errado na herança simples

[↑ Voltar ao início](#inicio)

No arquivo `ErradoFluentBuilderWithRecursiveGenerics.cs`, o problema nasce aqui:

```csharp
public PersonInfoBuilder Called(string name)
```

Depois de `Called()`, a cadeia vira `PersonInfoBuilder`. Então o método específico da classe derivada deixa de estar visível:

```csharp
builder.Called("Dmitri")
    .workAsA("engenheiro de software");
```

O erro conceitual é este:

- a cadeia começou em `PersonJobBuilder`;
- mas o retorno rebaixou a expressão para `PersonInfoBuilder`;
- `PersonInfoBuilder` não conhece `workAsA()`.

<a id="sec-020"></a>
### 14.2 Como recursive generics corrige isso

[↑ Voltar ao início](#inicio)

No arquivo `CertoFluentBuilderWithRecursiveGenerics.cs`, a solução usa um tipo genérico autorreferente:

```csharp
public class PersonInfoBuilder<SELF> : PersonBuilder
    where SELF : PersonInfoBuilder<SELF>
```

Agora `Called()` não devolve mais uma base fixa. Ele devolve `SELF`:

```csharp
public SELF Called(string name)
{
    person.Name = name;
    return (SELF)this;
}
```

<a id="sec-021"></a>
### 14.3 Explicando o trecho mais importante

[↑ Voltar ao início](#inicio)

O trecho central é este:

```csharp
public class PersonJobBuilder<SELF>
    : PersonInfoBuilder<PersonJobBuilder<SELF>>
    where SELF : PersonJobBuilder<SELF>
```

Leitura em partes:

- `PersonJobBuilder<SELF>` ainda recebe o tipo concreto final da cadeia.
- `: PersonInfoBuilder<PersonJobBuilder<SELF>>` diz para a camada de `PersonInfoBuilder` que, naquela cadeia, o tipo de retorno relevante passa a ser `PersonJobBuilder<SELF>`.
- `where SELF : PersonJobBuilder<SELF>` restringe `SELF` para que ele realmente pertença a essa família de builders.

Tradução prática:

- quando `Called()` for executado, a cadeia não será degradada para uma classe base genérica demais;
- ela continuará em um tipo que ainda conhece `workAsA()`;
- por isso a fluência sobrevive mesmo com herança.

<a id="sec-022"></a>
### 14.4 Como o ciclo fecha no exemplo

[↑ Voltar ao início](#inicio)

Dentro de `Person`, aparece:

```csharp
public class Builder : PersonJobBuilder<Builder>
{
}
```

Esse é o fechamento da cadeia. O tipo concreto final passa a ser `Builder`.

Por isso o uso funciona:

```csharp
var me = Person.New
    .Called("Dmitri")
    .workAsA("Professor")
    .Build();
```

---

<a id="sec-023"></a>
## 15. Stepwise Builder

[↑ Voltar ao início](#inicio)


Esta parte corresponde ao arquivo `CreationalPatterns/2.Builder/Aula03_StepWiseBuilder/StepWiseBuilder.cs`.

O **Stepwise Builder** tenta resolver um problema diferente do fluent builder comum:

- o fluent builder melhora a leitura da API;
- o stepwise builder melhora a **segurança da ordem** da construção.

Em vez de apenas devolver `this`, ele devolve **interfaces diferentes para cada etapa**.

<a id="sec-024"></a>
### 15.1 Qual problema ele resolve

[↑ Voltar ao início](#inicio)

No builder tradicional ou no fluent builder comum, o cliente pode ter liberdade demais. Dependendo da API, ele pode:

- tentar pular um passo obrigatório;
- chamar métodos em ordem ruim;
- chegar em `Build()` cedo demais.

O stepwise builder combate isso fazendo a pergunta:

> **Pergunta-chave:** qual é o próximo passo válido que o cliente pode executar agora?

<a id="sec-025"></a>
### 15.2 Como isso aparece no exemplo do carro

[↑ Voltar ao início](#inicio)

No exemplo atual, a ordem desejada é esta:

1. escolher o tipo do carro;
2. escolher o tamanho da roda;
3. construir o carro.

Essa ordem aparece nas interfaces:

```csharp
public interface ISpecifyCarType
{
    ISpecifyWheelSize OfType(CarType type);
}

public interface ISpecifyWheelSize
{
    IBuildCar WithWheels(int size);
}

public interface IBuildCar
{
    Car Build();
}
```

Cada interface representa um **estágio da conversa** com o builder.

- `ISpecifyCarType`: só permite escolher o tipo.
- `ISpecifyWheelSize`: só permite escolher as rodas.
- `IBuildCar`: só permite finalizar com `Build()`.

<a id="sec-026"></a>
### 15.3 O ponto central da técnica

[↑ Voltar ao início](#inicio)

O truque principal não está no objeto concreto interno. O truque está no **tipo devolvido por cada método**.

Leia a cadeia assim:

- `Create()` devolve `ISpecifyCarType`;
- `OfType()` devolve `ISpecifyWheelSize`;
- `WithWheels()` devolve `IBuildCar`;
- `Build()` devolve `Car`.

Ou seja, o compilador passa a guiar a ordem:

```csharp
var car = CarBuilder.Create()
    .OfType(CarType.Crossover)
    .WithWheels(18)
    .Build();
```

Se o cliente acabou de chamar `Create()`, ele ainda não enxerga `WithWheels()` nem `Build()`, porque esses métodos não fazem parte da interface atual.

<a id="sec-027"></a>
### 15.4 Objeto real vs tipo da referência

[↑ Voltar ao início](#inicio)

Essa é a parte que mais costuma confundir no começo.

No método:

```csharp
public static ISpecifyCarType Create()
{
    return new Impl();
}
```

parece estranho ler:

- a assinatura promete `ISpecifyCarType`;
- mas o `return` devolve `new Impl()`.

Isso funciona porque `Impl` implementa `ISpecifyCarType`.

Em C#, isso é normal: um objeto concreto pode ser tratado como uma interface que ele implementa.

Leitura mental correta:

- **objeto real criado na memória:** `Impl`
- **tipo da referência entregue ao cliente:** `ISpecifyCarType`

Essas duas coisas não são iguais.

No Stepwise Builder, a ordem não é controlada por "qual objeto foi criado por baixo". A ordem é controlada por **qual interface foi entregue ao cliente nesta etapa**.

Como `ISpecifyCarType` só declara:

```csharp
ISpecifyWheelSize OfType(CarType type);
```

então, naquele momento, o cliente só consegue chamar `OfType()`.

Ele não consegue chamar `WithWheels()` nem `Build()` porque esses métodos não fazem parte do tipo da referência atual, mesmo que o objeto concreto por baixo seja um `Impl` que saiba fazer tudo.

Uma forma de abrir a cadeia e enxergar melhor isso é esta:

```csharp
ISpecifyCarType step1 = CarBuilder.Create();
ISpecifyWheelSize step2 = step1.OfType(CarType.Crossover);
IBuildCar step3 = step2.WithWheels(18);
Car car = step3.Build();
```

Agora a ideia fica mais visível:

- `step1` só conhece `OfType()`;
- `step2` só conhece `WithWheels()`;
- `step3` só conhece `Build()`.

Então a pergunta certa não é:

> **Pergunta inadequada:** qual objeto foi criado?

A pergunta certa é:

> **Pergunta adequada:** qual interface foi devolvida para o cliente agora?

<a id="sec-028"></a>
### 15.5 Por que existe uma classe `Impl`

[↑ Voltar ao início](#inicio)

Dentro do `CarBuilder`, existe uma classe privada `Impl` que implementa todas as interfaces:

```csharp
private class Impl : ISpecifyCarType, ISpecifyWheelSize, IBuildCar
```

Ela sabe executar todos os passos internamente, mas o cliente não recebe a classe concreta diretamente. O cliente recebe apenas a interface do estágio atual.

Essa é a ideia chave:

- por dentro, um único objeto sabe tudo;
- por fora, a API só expõe o próximo passo válido.

Mesmo que o objeto real seja um `Impl`, o cliente não consegue pedir essa classe concreta diretamente, porque ela é `private`. Isso ajuda a blindar o caminho das interfaces públicas.

<a id="sec-029"></a>
### 15.6 Onde a validação entra

[↑ Voltar ao início](#inicio)

No exemplo, a escolha das rodas depende do tipo do carro:

- `Crossover` aceita rodas entre `17` e `20`;
- `Sedan` aceita rodas entre `15` e `17`.

Isso aparece em `WithWheels(int size)`.

O stepwise builder não substitui a validação de regra de negócio. Ele trabalha junto com ela:

- as interfaces restringem a **ordem**;
- o método concreto ainda valida o **conteúdo** do passo.

<a id="sec-030"></a>
### 15.7 O que ele enfatiza

[↑ Voltar ao início](#inicio)

- ordem correta dos passos;
- segurança de construção em tempo de compilação;
- API que representa melhor o fluxo obrigatório do domínio.

<a id="sec-031"></a>
### 15.8 Quando ele faz sentido

[↑ Voltar ao início](#inicio)

Ele é útil quando:

- a ordem faz parte do contrato do domínio;
- existem passos obrigatórios;
- um objeto incompleto seria inválido demais para ser liberado;
- você quer que o compilador ajude a bloquear usos errados da API.

<a id="sec-032"></a>
### 15.9 Custo dessa abordagem

[↑ Voltar ao início](#inicio)

O ganho de segurança costuma vir com mais tipos, mais interfaces e mais burocracia de modelagem.

Em troca, a leitura da API passa a ensinar a ordem correta de construção.

---

<a id="sec-033"></a>
## 16. Functional Builder

[↑ Voltar ao início](#inicio)


Esta parte corresponde ao arquivo `CreationalPatterns/2.Builder/Aula04_FunctionalBuilder/FunctionalBuilder.cs`.

O **Functional Builder** continua sendo uma variação de `Builder`, mas muda o modelo mental da construção por dentro.

Em vez de pensar apenas:

- "tenho um objeto e vou preenchendo ele agora"

o exemplo passa a pensar:

- "vou acumulando operações de construção e só no final vou materializar o objeto"

No arquivo atual, isso aparece nesta leitura:

```csharp
var person = new PersonBuilder()
    .Called("Sara")
    .WorksAsA("Professor")
    .Build();
```

Por fora, a cadeia lembra um fluent builder comum. A diferença importante está por dentro:

- `Called()` registra uma operação;
- `WorksAsA()` registra outra operação;
- `Build()` cria o objeto base e aplica todas as operações acumuladas.

<a id="sec-034"></a>
### 16.1 O que muda em relação ao builder normal

[↑ Voltar ao início](#inicio)

Esta é a comparação que mais ajuda a não confundir os dois estilos.

No **builder tradicional**, o modelo mental costuma ser este:

- o builder já guarda o produto em construção;
- cada método altera esse produto imediatamente;
- o estado vai ficando pronto aos poucos dentro do próprio builder.

No arquivo `Builder.cs`, isso aparece na prática:

```csharp
private HtmlElement root = new HtmlElement();

public void AddChild(string childName, string childText)
{
    var e = new HtmlElement(childName, childText);
    root.Elements.Add(e);
}
```

Leitura correta desse estilo:

- o objeto `root` já existe;
- `AddChild()` mexe nele na hora;
- depois de cada chamada, o produto já está um pouco mais montado.

No **Functional Builder**, o modelo mental muda:

- o builder não fica guardando o produto final pronto para ser modificado agora;
- ele guarda uma lista de operações;
- essas operações só serão aplicadas quando `Build()` for chamado.

No arquivo desta aula, isso aparece assim:

```csharp
private readonly List<Func<TSubject, TSubject>> actions
```

e depois:

```csharp
return actions.Aggregate(new TSubject(), (subject, action) => action(subject));
```

Leitura correta desse estilo:

- o produto final ainda não foi materializado durante os passos;
- `Called()` e `WorksAsA()` não "preenchem a pessoa agora" no mesmo sentido do builder normal;
- eles apenas acumulam instruções;
- o objeto final nasce no `Build()`.

Uma forma curta de guardar:

- **builder normal:** "modifica o produto agora"
- **functional builder:** "guarda instruções e monta no final"

<a id="sec-035"></a>
### 16.2 O que muda em relação ao Stepwise Builder

[↑ Voltar ao início](#inicio)

Esta é a comparação mais importante com a aula anterior.

No **Stepwise Builder**, a pergunta central era:

> **Stepwise Builder:** qual é o próximo passo válido?

Por isso o exemplo do carro usava interfaces diferentes para cada etapa:

- `ISpecifyCarType`
- `ISpecifyWheelSize`
- `IBuildCar`

Cada retorno restringia o que o cliente podia chamar em seguida.

No **Functional Builder**, a pergunta central muda para:

> **Functional Builder:** quais operações de construção eu quero compor?

Aqui o objetivo principal já não é travar a ordem pelo tipo devolvido. O objetivo passa a ser acumular comportamento de forma modular.

Resumo da virada:

- `Stepwise Builder`: enfatiza ordem obrigatória.
- `Functional Builder`: enfatiza composição de passos.

<a id="sec-036"></a>
### 16.3 Como isso aparece na anatomia do código

[↑ Voltar ao início](#inicio)

No arquivo da aula, a base genérica é esta:

```csharp
public abstract class FunctionalBuilder<TSubject, TSelf>
```

Esses dois parâmetros significam:

- `TSubject`: qual é o objeto final sendo construído.
- `TSelf`: qual é o tipo concreto do builder que está encadeando.

Isso permite reaproveitar a mecânica do builder funcional sem perder a API fluent correta no tipo concreto.

Dentro dessa classe, a variável central é:

```csharp
private readonly List<Func<TSubject, TSubject>> actions
```

Essa lista guarda a receita da construção.

Cada `Func<TSubject, TSubject>` representa uma operação que:

1. recebe o objeto em construção;
2. aplica alguma mudança;
3. devolve o mesmo objeto para a próxima operação.

<a id="sec-037"></a>
### 16.4 O papel de `Do()`, `AddAction()` e `Build()`

[↑ Voltar ao início](#inicio)

O método:

```csharp
public TSelf Do(Action<TSubject> action)
```

recebe uma `Action<TSubject>`, isto é, uma operação que mexe no objeto mas não devolve valor.

Já a lista interna guarda `Func<TSubject, TSubject>`, porque o pipeline precisa devolver o objeto para a etapa seguinte.

Então `AddAction()` faz a ponte entre essas duas ideias:

- recebe uma `Action<TSubject>`;
- empacota essa action dentro de uma função;
- essa função modifica o objeto e depois devolve esse mesmo objeto.

Depois, `Build()` aplica a receita acumulada:

```csharp
return actions.Aggregate(new TSubject(), (subject, action) => action(subject));
```

Leitura passo a passo:

1. `new TSubject()` cria uma instância vazia.
2. `Aggregate()` percorre a lista de operações.
3. cada operação recebe o mesmo objeto e devolve esse objeto já alterado.
4. o resultado da última operação vira o produto construído.

Em outras palavras: no functional builder, o objeto final só nasce "de verdade" no `Build()`.

<a id="sec-038"></a>
### 16.5 Onde a extensibilidade aparece

[↑ Voltar ao início](#inicio)

No exemplo atual, `Called(string name)` fica dentro de `PersonBuilder`, mas `WorksAsA(string position)` aparece em `PersonBuilderExtensions`.

Esse detalhe mostra um ganho comum desse estilo:

- novos passos podem ser plugados por fora;
- a classe principal do builder não precisa concentrar tudo;
- a construção fica mais modular.

Isso conversa diretamente com a ideia de compor pequenas operações independentes.

<a id="sec-039"></a>
### 16.6 Diferença direta para as aulas anteriores

[↑ Voltar ao início](#inicio)

Uma tabela mental simples ajuda:

| Aula | Pergunta principal | Técnica central | Ganho principal |
| --- | --- | --- | --- |
| `Builder` tradicional | "como montar o produto passo a passo?" | o builder guarda e modifica o produto durante a configuração | centralização da montagem |
| `Stepwise Builder` | "qual é o próximo passo válido?" | interfaces diferentes por etapa | segurança de ordem em tempo de compilação |
| `Functional Builder` | "quais operações quero acumular?" | lista de funções aplicadas no `Build()` | composição flexível de comportamento |

Outra forma curta de guardar:

- no builder tradicional, o produto vai sendo alterado a cada chamada;
- no stepwise, o tipo devolvido controla o fluxo;
- no functional, a lista de operações controla a construção.

<a id="sec-040"></a>
### 16.7 Quando ele ajuda

[↑ Voltar ao início](#inicio)

Ele pode ser interessante quando:

- você quer montar objetos com passos bem modulares;
- faz sentido acumular ações antes de materializar o resultado final;
- você quer adicionar novos passos sem inchar tanto a classe principal;
- a equipe está confortável com delegates, lambdas e uma leitura mais abstrata.

<a id="sec-041"></a>
### 16.8 Cuidado didático

[↑ Voltar ao início](#inicio)

Para iniciantes, este estilo costuma parecer mais abstrato do que o builder tradicional e do que o stepwise builder.

Isso acontece porque parte da construção fica indireta:

- o objeto não está sendo mostrado como modificado o tempo todo;
- o código está guardando operações para depois;
- `Action`, `Func`, extension methods e `Aggregate()` exigem mais maturidade de leitura.

Por isso, este modelo costuma fazer mais sentido depois que a base de `Builder`, `Fluent Builder` e `Stepwise Builder` já ficou firme.

---

<a id="sec-042"></a>
## 17. Faceted Builder

[↑ Voltar ao início](#inicio)


Esta parte corresponde ao arquivo `CreationalPatterns/2.Builder/Aula05_FacetedBuilder/FacetedBuilder.cs`.

O **Faceted Builder** aparece quando um único builder começa a ficar grande demais.

Nesse caso, a experiência de construção do mesmo produto é dividida por aspectos ou facetas.

<a id="sec-043"></a>
### 17.1 O que ele enfatiza

[↑ Voltar ao início](#inicio)

- separação por responsabilidade dentro da construção;
- organização de APIs grandes;
- divisão da montagem do mesmo produto em áreas mais claras.

<a id="sec-044"></a>
### 17.2 Exemplo mental

[↑ Voltar ao início](#inicio)

Um objeto grande pode ter facetas como:

- endereço;
- trabalho;
- finanças;
- configurações técnicas.

<a id="sec-045"></a>
### 17.3 Leitura correta

[↑ Voltar ao início](#inicio)

O `Faceted Builder` não divide o produto em produtos diferentes. Ele divide a **interface de construção** do mesmo produto em partes mais administráveis.

No arquivo atual, isso aparece assim:

- `Person` continua sendo um único produto final.
- `PersonBuilder` funciona como builder raiz.
- `Lives` abre a faceta de endereço.
- `Works` abre a faceta profissional.
- `PersonAddressBuilder` e `PersonJobBuilder` recebem a mesma instância de `Person`.

Ou seja: o cliente muda de "área" da API, mas continua montando o mesmo objeto.

<a id="sec-046"></a>
### 17.4 O que o torna diferente das aulas anteriores

[↑ Voltar ao início](#inicio)

Essa é a comparação mais importante para não misturar as variações:

- no **Builder tradicional**, a API ainda é uma superfície única de construção;
- no **Fluent Builder**, o ganho principal é o encadeamento com `return this`;
- no **Stepwise Builder**, o ganho principal é forçar ordem e passos obrigatórios;
- no **Functional Builder**, o ganho principal é acumular operações para aplicar no `Build()`;
- no **Faceted Builder**, o ganho principal é dividir um builder grande em sub-builders especializados.

Então a pergunta central desta aula não é:

- "como encadear melhor?"
- "como forçar ordem?"
- "como adiar a materialização?"

A pergunta central passa a ser:

> **Pergunta-chave:** como organizar a construção de um objeto grande sem transformar um único builder em uma classe inchada?

<a id="sec-047"></a>
### 17.5 O mecanismo técnico do exemplo

[↑ Voltar ao início](#inicio)

O ponto técnico mais importante do arquivo `FacetedBuilder.cs` é este:

- existe uma `Person` compartilhada;
- cada faceta recebe essa mesma instância no construtor;
- cada faceta altera apenas sua própria área de responsabilidade;
- ao final, a cadeia continua representando o mesmo produto.

Isso também ajuda a enxergar uma diferença sutil para o Stepwise Builder:

- no Stepwise, o tipo devolvido controla **qual passo é permitido agora**;
- no Faceted, o tipo devolvido controla **qual área do mesmo produto estou configurando agora**.

E também ajuda a enxergar a diferença para o Functional Builder:

- no Functional Builder, o builder guarda instruções;
- no Faceted Builder, os sub-builders mexem no produto compartilhado imediatamente.

---

<a id="sec-048"></a>
## Coding Exercise 1: Builder Coding Exercise

[↑ Voltar ao início](#inicio)


Esta parte corresponde ao arquivo `CreationalPatterns/2.Builder/Aula06_BuilderExcercise/BuilderExcercise.cs`.

Este exercício é importante porque ele pega a ideia do capítulo e a transforma em uma implementação pequena, concreta e fácil de inspecionar.

<a id="sec-049"></a>
### O que o exercício quer de verdade

[↑ Voltar ao início](#inicio)

O enunciado não está pedindo um gerador genérico de código nem um renderizador de qualquer sintaxe arbitrária.

Ele está pedindo algo mais específico:

- montar o texto de **uma classe C# simples**;
- receber o nome dessa classe;
- permitir adicionar vários campos públicos;
- devolver a representação final formatada como código.

Em outras palavras, o aluno não precisa resolver "geração de código" em geral. O aluno precisa resolver:

> **Pergunta-chave:** como encapsular a montagem de uma classe simples em uma API builder?

<a id="sec-050"></a>
### Como ler o uso esperado

[↑ Voltar ao início](#inicio)

O uso esperado é este:

```csharp
var cb = new CodeBuilder("Person")
  .AddField("Name", "string")
  .AddField("Age", "int");

Console.WriteLine(cb);
```

Essa leitura pode ser feita em etapas:

1. `new CodeBuilder("Person")` inicia a construção da classe `Person`.
2. `AddField("Name", "string")` adiciona um campo à classe em construção.
3. `AddField("Age", "int")` adiciona outro campo.
4. `Console.WriteLine(cb)` pede a renderização final do resultado.

Ou seja, o cliente não está escrevendo manualmente:

- a linha `public class Person`;
- a chave de abertura;
- a chave de fechamento;
- a indentação de cada campo;
- os `;` no final das linhas.

Esse trabalho foi centralizado no builder e no produto que ele está montando.

<a id="sec-051"></a>
### O que é o produto neste exercício

[↑ Voltar ao início](#inicio)

No exemplo atual, existe uma nuance didática útil.

O produto pode ser lido em duas camadas:

- **produto em memória:** o objeto `CodeClass`;
- **representação final visível:** a string com o código da classe.

Isso ajuda a evitar uma confusão comum.

O builder não está simplesmente concatenando string aleatoriamente a cada chamada. Ele primeiro monta um pequeno modelo da classe e, só depois, esse modelo é renderizado como texto.

<a id="sec-052"></a>
### Participantes do padrão no exercício

[↑ Voltar ao início](#inicio)

Mapeando o exercício para a linguagem do GoF:

- `CodeBuilder`: é o **Builder**.
- `CodeClass`: é o **Product** em construção.
- `CodeField`: é uma parte simples do produto.
- `Main`/cliente: é quem descreve o que quer construir e consome o resultado final.

Repare que este exercício não usa um `Director` separado. Assim como nas outras aulas da pasta, o próprio cliente conduz a sequência da montagem.

<a id="sec-053"></a>
### O que faz este exercício ser um builder

[↑ Voltar ao início](#inicio)

Este ponto vale atenção porque, à primeira vista, alguém pode pensar:

**"isso não é só um `ToString()` com `StringBuilder`?"**

Não. O `StringBuilder` aqui é apenas um detalhe interno de implementação para montar a saída textual.

O que faz a solução ser um **Builder Pattern** é a estrutura da conversa:

- existe um objeto especializado em construção;
- ele guarda o estado do produto em construção;
- ele expõe passos de montagem em vez de expor a formatação inteira;
- o cliente descreve o que quer, sem montar manualmente a saída final.

Se o cliente precisasse escrever diretamente:

- `public class ...`
- `{`
- cada linha de campo
- `}`

então estaríamos mais perto de construção manual do que de builder.

<a id="sec-054"></a>
### Onde a fluência aparece

[↑ Voltar ao início](#inicio)

Este exercício também funciona como ponte entre o **builder tradicional** e o **fluent builder**.

O motivo é simples:

- ele continua sendo um builder porque organiza a construção;
- mas `AddField(...)` devolve o próprio builder com `return this`.

Então a API ganha encadeamento:

```csharp
new CodeBuilder("Person")
  .AddField("Name", "string")
  .AddField("Age", "int");
```

Por isso, pedagogicamente, este exercício reforça duas ideias ao mesmo tempo:

- **builder:** centralizar a montagem;
- **fluência:** encadear a montagem de forma mais legível.

<a id="sec-055"></a>
### O que o aluno precisa enxergar para conseguir reproduzir sozinho

[↑ Voltar ao início](#inicio)

Se você quisesse construir outro builder parecido depois, o raciocínio seria este:

1. descubra **qual é o produto** que será montado;
2. descubra **quais partes desse produto** variam;
3. crie um builder que **guarde esse produto internamente**;
4. faça cada método de configuração **alterar o produto em construção**;
5. no final, exponha um ponto de saída que **materialize ou renderize** o resultado.

No exercício atual:

1. o produto é uma classe C# simples;
2. as partes variáveis são o nome da classe e seus campos;
3. o builder guarda uma `CodeClass`;
4. `AddField(...)` altera essa `CodeClass`;
5. `ToString()` devolve a representação final em texto.

<a id="sec-056"></a>
### O que costuma confundir aqui

[↑ Voltar ao início](#inicio)

As confusões mais comuns nesta aula costumam ser estas:

- achar que o exercício está pedindo geração genérica de código;
- confundir `Builder Pattern` com o uso interno de `StringBuilder`;
- não perceber que o produto interno é a estrutura da classe, e não apenas a string final;
- olhar o `return this` e enxergar apenas "fluência", sem perceber a construção do produto por trás.

<a id="sec-057"></a>
### Como validar mentalmente se a solução está correta

[↑ Voltar ao início](#inicio)

Uma boa solução para este exercício deve satisfazer estas perguntas:

- o cliente consegue criar a classe sem escrever manualmente a formatação?
- os campos são adicionados passo a passo?
- a API ficou simples de ler?
- a saída final respeita exatamente o formato pedido?

Se essas respostas forem "sim", então o exercício cumpriu seu objetivo pedagógico.

---


<a id="sec-glossario"></a>
## Glossário
[↑ Voltar ao início](#inicio)

- **Builder:** separa a construção de um objeto complexo da sua representação final.
- **Director:** organiza uma sequência de etapas de construção.
- **Fluent interface:** API com chamadas encadeadas para configurar um objeto.
- **Produto:** objeto final montado pelas etapas do Builder.
