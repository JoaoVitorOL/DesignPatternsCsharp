<a id="inicio"></a>
# Design Pattern: Prototype

## Sumário
[↑ Voltar ao início](#inicio)

- [1. O que o Prototype representa?](#sec-001)
  - [O Problema que ele resolve:](#sec-002)
- [2. Em que situações se usa?](#sec-003)
- [3. Instruções passo a passo de como implementar (em Java)](#sec-004)
  - [Exemplo básico de implementação:](#sec-005)
- [4. Explicação detalhada do Exemplo Prático (Sistema de Inimigos e Jogo)](#sec-006)
  - [A Classe Componente: Arma](#sec-007)
  - [A Interface e a Classe Protótipo: InimigoPrototype e Inimigo](#sec-008)
  - [O Registro: RegistroDePrototipos](#sec-009)
  - [A Execução: Main](#sec-010)
- [5. Como os arquivos C# praticam Prototype](#sec-011)
- [6. Seguindo a cópia profunda do exercício](#sec-012)
- [Variações e aprofundamentos](#sec-013)
- [31. ICloneable Is Problematic](#sec-014)
  - [31.1 O problema de ICloneable](#sec-015)
  - [31.2 Falta de contrato claro](#sec-016)
  - [31.3 O risco de cópia rasa](#sec-017)
  - [31.4 Quando a deep copy faz sentido](#sec-018)
  - [31.5 O problema de tipos não seguros](#sec-019)
  - [31.6 Por que isso é ruim para Prototype](#sec-020)
  - [31.7 O que fazer no lugar](#sec-021)
  - [31.8 Regra mental](#sec-022)
- [32. Copy Constructors](#sec-023)
  - [32.1 O que esse exemplo ensina](#sec-024)
  - [32.2 Como o exemplo do código representa isso](#sec-025)
  - [32.3 Por que o array precisa ser copiado](#sec-026)
  - [32.4 Por que o objeto composto também precisa ser copiado](#sec-027)
  - [32.5 Regra mental](#sec-028)
- [33. Explicit Deep Copy Interface](#sec-029)
  - [33.1 O que essa abordagem resolve](#sec-030)
  - [33.2 Como o exemplo do código representa isso](#sec-031)
  - [33.3 Por que isso é melhor do que depender de ICloneable](#sec-032)
  - [33.4 Regra mental](#sec-033)
- [34. Prototype Inheritance](#sec-034)
  - [O problema da herança no Prototype](#sec-035)
  - [Estrutura da solução](#sec-036)
  - [Facilitando a sintaxe com métodos de extensão](#sec-037)
  - [Demonstração prática de uso](#sec-038)
  - [Conclusão](#sec-039)
  - [Conceitos de C# que aparecem no exemplo](#sec-040)
- [34.1 Prototype em jogos: o caso de entidades já configuradas](#sec-041)
  - [O problema](#sec-042)
  - [A ideia do Prototype](#sec-043)
  - [Exemplo mental](#sec-044)
  - [Por que isso é interessante em design](#sec-045)
  - [Ligação com o padrão GoF](#sec-046)
- [35. Copy Through Serialization](#sec-047)
  - [35.1 O que significa serializar um objeto?](#sec-048)
  - [35.2 O que é desserialização?](#sec-049)
  - [35.3 Por que isso tem relação com Prototype?](#sec-050)
  - [35.4 O que era o BinaryFormatter?](#sec-051)
  - [35.5 O que é importante entender sobre esse exemplo?](#sec-052)
  - [35.6 Diferenças entre Prototype por serialização e as outras formas de Prototype](#sec-053)
  - [35.7 Quando essa abordagem faz sentido?](#sec-054)
- [Coding Exercise 3: Prototype Coding Exercise](#sec-055)
  - [O que o exercício quer de verdade](#sec-056)
  - [Anatomia das referências e das alocações no exemplo](#sec-057)
  - [Participantes do Padrão no Exercício](#sec-058)
  - [Código-fonte guiado e resolvido](#sec-059)


**Família GoF:** criacional. **Escopo predominante:** objetos; novas instâncias partem de protótipos.

<a id="sec-001"></a>
## 1. O que o Prototype representa?

[↑ Voltar ao início](#inicio)
O padrão **Prototype** é um padrão de projeto criacional que permite **copiar objetos existentes** — mesmo aqueles que possuem estruturas complexas ou encapsuladas — sem que o código cliente dependa de suas classes concretas.

<a id="sec-002"></a>
### O Problema que ele resolve:

[↑ Voltar ao início](#inicio)
Em sistemas onde precisamos criar muitos objetos parecidos (como inimigos em um jogo, templates ou configurações), a criação direta via `new` gera problemas práticos:
- **Código repetido e acoplado:** Muito código para configurar cada objeto do zero, acoplando o cliente às classes concretas.
- **Dificuldade de clonagem segura:** Copiar dados manualmente campo a campo é trabalhoso, propenso a erros e pode falhar ao lidar com referências internas (*shallow copy* vs *deep copy*).
- **Perda de flexibilidade:** Quando o sistema precisa clonar objetos conhecidos apenas por interfaces genéricas, instanciar diretamente via `new` torna-se inviável.

---

<a id="sec-003"></a>
## 2. Em que situações se usa?

[↑ Voltar ao início](#inicio)
Use o Prototype quando:
1. **Objetos semelhantes:** Vários objetos compartilham características base e diferem apenas em pequenos ajustes ou atributos específicos.
2. **Criação custosa:** A instanciação de um objeto envolve etapas pesadas (consultas a banco, leitura de arquivos, processamento complexo) e clonar a partir de um modelo pronto é mais eficiente.
3. **Reduzir acoplamento:** O cliente precisa criar novos objetos sem conhecer a classe exata deles, interagindo apenas com uma interface ou protótipo genérico.
4. **Registro de modelos:** Faz sentido manter modelos pré-configurados em um repositório centralizado para duplicação sob demanda.

---

<a id="sec-004"></a>
## 3. Instruções passo a passo de como implementar (em Java)

[↑ Voltar ao início](#inicio)

1. **Definir uma interface comum de clonagem:** Declarar um método (ex: `clonar()`) que será implementado pelas classes clonáveis.
2. **Implementar a cópia na classe concreta:** Criar um construtor de cópia ou método que duplique os atributos do objeto base.
3. **Garantir a Cópia Profunda (Deep Copy):** Decidir se objetos internos (como referências a outras classes) devem ser compartilhados ou clonados separadamente para manter a independência.
4. **Criar um Registro de Protótipos (Opcional):** Um repositório (`Map`) para armazenar modelos padrão prontos para serem buscados e clonados por nome ou categoria.

<a id="sec-005"></a>
### Exemplo básico de implementação:

[↑ Voltar ao início](#inicio)
```java
public interface InimigoPrototype {
    InimigoPrototype clonar();
}

public class Inimigo implements InimigoPrototype {
    private String tipo;
    private int vida;

    public Inimigo(String tipo, int vida) {
        this.tipo = tipo;
        this.vida = vida;
    }

    // Construtor de cópia
    public Inimigo(Inimigo base) {
        this.tipo = base.tipo;
        this.vida = base.vida;
    }

    @Override
    public InimigoPrototype clonar() {
        return new Inimigo(this);
    }
}
```

---

<a id="sec-006"></a>
## 4. Explicação detalhada do Exemplo Prático (Sistema de Inimigos e Jogo)

[↑ Voltar ao início](#inicio)

O código fornecido demonstra o padrão **Prototype** aplicado em um jogo, onde diferentes tipos de inimigos e suas armas são gerenciados por um registro central e clonados sob demanda, utilizando **Deep Copy**.

<a id="sec-007"></a>
### A Classe Componente: `Arma`

[↑ Voltar ao início](#inicio)
Representa o equipamento do inimigo. Possui um método de clonagem manual (`clonar()`) para garantir que, ao clonar o inimigo, a arma também seja duplicada com segurança (evitando que dois inimigos compartilhem exatamente a mesma instância de arma na memória).

```java
package jogo;

public class Arma {
    private String nome;
    private int bonusDano;

    public Arma(String nome, int bonusDano) {
        this.nome = nome;
        this.bonusDano = bonusDano;
    }

    public String getNome() { return nome; }
    public void setNome(String nome) { this.nome = nome; }

    public int getBonusDano() { return bonusDano; }
    public void setBonusDano(int bonusDano) { this.bonusDano = bonusDano; }

    public Arma clonar() {
        return new Arma(this.nome, this.bonusDano);
    }

    @Override
    public String toString() {
        return "Arma [Nome: " + nome + ", Bônus de Dano: " + bonusDano + "]";
    }
}
```

---

<a id="sec-008"></a>
### A Interface e a Classe Protótipo: `InimigoPrototype` e `Inimigo`

[↑ Voltar ao início](#inicio)
- `InimigoPrototype` define o contrato de clonagem.
- `Inimigo` implementa o contrato utilizando um **construtor de cópia (Deep Copy)**, que copia os tipos primitivos e chama `arma.clonar()` para isolar o objeto interno.

```java
package jogo;

public interface InimigoPrototype {
    InimigoPrototype clonar();
}
```

```java
package jogo;

public class Inimigo implements InimigoPrototype {
    private String tipo;
    private int vida;
    private int dano;
    private Arma arma;

    public Inimigo(String tipo, int vida, int dano, Arma arma) {
        this.tipo = tipo;
        this.vida = vida;
        this.dano = dano;
        this.arma = arma;
    }

    public Inimigo(Inimigo base) {
        this.tipo = base.tipo;
        this.vida = base.vida;
        this.dano = base.dano;
        this.arma = base.arma != null ? base.arma.clonar() : null;
    }

    @Override
    public InimigoPrototype clonar() {
        return new Inimigo(this);
    }

    public String getTipo() { return tipo; }
    public void setTipo(String tipo) { this.tipo = tipo; }

    public int getVida() { return vida; }
    public void setVida(int vida) { this.vida = vida; }

    public int getDano() { return dano; }
    public void setDano(int dano) { this.dano = dano; }

    public Arma getArma() { return arma; }
    public void setArma(Arma arma) { this.arma = arma; }

    @Override
    public String toString() {
        return "Inimigo [Tipo: " + tipo + ", Vida: " + vida + ", Dano: " + dano + ", " + arma + "]";
    }
}
```

---

<a id="sec-009"></a>
### O Registro: `RegistroDePrototipos`

[↑ Voltar ao início](#inicio)
Centraliza e armazena protótipos pré-configurados (guerreiro, mago, arqueiro, chefe) em um mapa (`Map`). O método `getPrototipo()` busca o modelo desejado e retorna diretamente um clone pronto para uso.

```java
package jogo;

import java.util.HashMap;
import java.util.Map;

public class RegistroDePrototipos {
    private final Map<String, InimigoPrototype> prototipos = new HashMap<>();

    public RegistroDePrototipos() {
        carregarPrototiposPadrao();
    }

    private void carregarPrototiposPadrao() {
        prototipos.put("guerreiro", new Inimigo("Guerreiro", 100, 20, new Arma("Espada Longa", 10)));
        prototipos.put("mago", new Inimigo("Mago", 60, 35, new Arma("Cajado Mágico", 25)));
        prototipos.put("arqueiro", new Inimigo("Arqueiro", 80, 25, new Arma("Arco Composto", 15)));
        prototipos.put("chefe", new Inimigo("Chefe", 500, 60, new Arma("Machado Gigante", 40)));
    }

    public Inimigo getPrototipo(String nome) {
        InimigoPrototype prototipo = prototipos.get(nome.toLowerCase());
        if (prototipo == null) {
            throw new IllegalArgumentException("Protótipo não encontrado: " + nome);
        }
        return (Inimigo) prototipo.clonar();
    }
}
```

---

<a id="sec-010"></a>
### A Execução: `Main`

[↑ Voltar ao início](#inicio)
A classe principal valida o comportamento do Prototype:
1. **Obtenção via Registro:** Recupera protótipos base diretamente do repositório.
2. **Criação de Variações:** Clona um guerreiro base e altera seus atributos para criar um "Guerreiro Elite" sem afetar o modelo original.
3. **Comprovação de Instâncias Distintas:** Comprova que os clones gerados possuem referências de memória diferentes (`hashCode()` distintos).
4. **Demonstração de Deep Copy:** Altera a arma apenas de um arqueiro clonado e comprova que o outro arqueiro permanece com sua arma intacta.

```java
package jogo;

public class Main {
    public static void main(String[] args) {
        RegistroDePrototipos registro = new RegistroDePrototipos();

        System.out.println("=== 1. OBTENDO INIMIGOS VIA REGISTRO DE PROTÓTIPOS ===");
        Inimigo guerreiroBase = registro.getPrototipo("guerreiro");
        Inimigo magoBase = registro.getPrototipo("mago");

        System.out.println("Original do Registro: " + guerreiroBase);
        System.out.println("Original do Registro: " + magoBase);

        System.out.println("\n=== 2. CRIANDO INIMIGO ELITE A PARTIR DO 'GUERREIRO' ===");
        Inimigo guerreiroElite = registro.getPrototipo("guerreiro");
        guerreiroElite.setTipo("Guerreiro Elite");
        guerreiroElite.setVida(250);
        guerreiroElite.setDano(45);

        System.out.println("Guerreiro Padrão (Não mudou): " + guerreiroBase);
        System.out.println("Guerreiro Elite (Modificado):   " + guerreiroElite);

        System.out.println("\n=== 3. PROVANDO QUE OS CLONES SÃO OBJETOS DISTINTOS ===");
        Inimigo guerreiroClone1 = registro.getPrototipo("guerreiro");
        Inimigo guerreiroClone2 = registro.getPrototipo("guerreiro");

        System.out.println("Hash do Clone 1: " + guerreiroClone1.hashCode());
        System.out.println("Hash do Clone 2: " + guerreiroClone2.hashCode());
        System.out.println("São referências iguais? " + (guerreiroClone1 == guerreiroClone2));

        System.out.println("\n=== 4. DEMONSTRANDO A CÓPIA PROFUNDA (DEEP COPY) ===");
        Inimigo arqueiro1 = registro.getPrototipo("arqueiro");
        Inimigo arqueiro2 = registro.getPrototipo("arqueiro");

        System.out.println("Antes da alteração:");
        System.out.println("Arqueiro 1 Arma: " + arqueiro1.getArma());
        System.out.println("Arqueiro 2 Arma: " + arqueiro2.getArma());

        arqueiro1.getArma().setNome("Arco Élfico Lendário");
        arqueiro1.getArma().setBonusDano(50);

        System.out.println("\nDepois de modificar a arma do Arqueiro 1:");
        System.out.println("Arqueiro 1 Arma (Modificado): " + arqueiro1.getArma());
        System.out.println("Arqueiro 2 Arma (Permaneceu intacta): " + arqueiro2.getArma());
    }
}
```


<a id="sec-011"></a>
## 5. Como os arquivos C# praticam Prototype

[↑ Voltar ao início](#inicio)

A pasta [4.Prototype](.) evolui do clone ambíguo para cópias com semântica explícita:

- [ICloneAbleIsBad.cs](Aula01_ICloneableIsBad/ICloneAbleIsBad.cs) discute o contrato genérico de ICloneable, que não informa se a cópia deve ser rasa ou profunda.
- [CopyConstructors.cs](Aula02_CopyConstructors/CopyConstructors.cs) usa construtor de cópia para tornar a política explícita.
- [ExplicitDeepCopyInterface.cs](Aula03_ExplicitDeepCopyInterface/ExplicitDeepCopyInterface.cs) define cópia profunda tipada.
- [PrototypeInhertance.cs](Aula04_PrototypeInhertance/PrototypeInhertance.cs) mostra como propagar cópias pela hierarquia.
- [PrototypeSerialization.cs](Aula05_PrototypeSerialization/PrototypeSerialization.cs) demonstra serialização como técnica de cópia, sujeita às limitações do formato.
- O exercício corrigido em [PrototypeExcercise.cs](Aula06_PrototypeExcercise/PrototypeExcercise.cs) implementa IPrototype<T>. Point.DeepCopy() cria outro Point; Line.DeepCopy() cria outra Line e clona Start e End.

O teste conceitual do exercício é independência: mudar um ponto da cópia não deve alterar a linha original. Copiar apenas a referência para os mesmos Point seria uma cópia rasa e violaria o requisito. Para estado mutável, cada camada precisa participar da política de cópia; serialização não é garantia universal de cópia correta.


<a id="sec-012"></a>
## 6. Seguindo a cópia profunda do exercício

[↑ Voltar ao início](#inicio)

O problema do exercício em [PrototypeExcercise.cs](Aula06_PrototypeExcercise/PrototypeExcercise.cs) é copiar uma Line que contém dois objetos Point. Copiar apenas a referência da linha ou dos pontos faria original e cópia compartilharem os mesmos objetos internos; mudar uma coordenada alteraria ambos.

O contrato IPrototype<T> torna a intenção e o tipo de retorno explícitos. Point.DeepCopy cria um Point novo com os mesmos X e Y. Line.DeepCopy cria uma Line nova e chama DeepCopy para Start e End. Assim, a cópia preserva os valores, mas não a identidade dos pontos. O teste útil não é apenas conferir coordenadas iguais: também é alterar a cópia e verificar que a original permanece intacta.

As aulas anteriores mostram decisões alternativas. Construtor de cópia e interface tipada tornam o contrato visível; ICloneable não especifica a profundidade da cópia. Em hierarquias, cada classe precisa copiar seus campos e chamar a cópia da classe base. Serialização pode ajudar, mas só copia o que o serializador inclui e pode falhar com recursos externos.

Prototype é útil quando já existe um objeto corretamente configurado e criar outro do zero seria repetitivo. A política de cópia precisa cobrir todo o estado relevante.

<a id="sec-013"></a>
## Variações e aprofundamentos

[↑ Voltar ao início](#inicio)

As seções abaixo reúnem variações, exemplos e decisões que complementam a explicação principal deste padrão. Elas preservam a linguagem e o contexto do material didático original.

<a id="sec-014"></a>
## 31. ICloneable Is Problematic

[↑ Voltar ao início](#inicio)


A primeira aula deste capítulo costuma mostrar por que `ICloneable` é uma escolha problemática em C#. A ideia central é simples: embora pareça uma solução imediata para "copiar um objeto", a interface `ICloneable` foi projetada de forma muito vaga e acaba gerando mais confusão do que clareza.

<a id="sec-015"></a>
### 31.1 O problema de `ICloneable`

[↑ Voltar ao início](#inicio)

A maior crítica a `ICloneable` é que ele não diz claramente o que significa "clone".

Em prática, isso pode significar:

- cópia rasa;
- cópia profunda;
- cópia do estado atual;
- cópia com alguma transformação;
- cópia incompleta ou semântica diferente do esperado.

Ou seja, o nome sugere uma operação simples, mas a semântica real fica aberta. Isso é ruim porque a intenção do código fica pouco explícita.

<a id="sec-016"></a>
### 31.2 Falta de contrato claro

[↑ Voltar ao início](#inicio)

A interface `ICloneable` expõe apenas um método:

```csharp
object Clone();
```

Esse contrato é muito genérico. Ele não informa:

- se a cópia é rasa ou profunda;
- se o objeto precisa de configuração extra;
- se a operação pode falhar;
- se a cópia preserva referências compartilhadas ou as duplica;
- se a classe filha precisa sobrescrever algo.

Como resultado, duas implementações diferentes podem parecer compatíveis no código, mas terem comportamentos completamente distintos.

<a id="sec-017"></a>
### 31.3 O risco de cópia rasa

[↑ Voltar ao início](#inicio)

Um dos problemas mais comuns é a cópia rasa. Nesse caso, o objeto novo recebe uma cópia dos campos, mas alguns membros internos ainda apontam para os mesmos objetos do original.

Essa é a diferença central entre `shallow copy` e `deep copy`:

- na `shallow copy`, a estrutura externa do objeto é nova, mas algumas partes internas continuam compartilhadas;
- na `deep copy`, as partes mutáveis relevantes para o isolamento são duplicadas; objetos imutáveis ou deliberadamente compartilhados não precisam ser copiados.

Isso pode causar efeitos colaterais inesperados:

```csharp
var original = new Pessoa { Nome = "Ana", Endereco = new Endereco("Rua A") };
var copia = (Pessoa)original.Clone();

copia.Endereco.Cidade = "Campinas";
```

Se a cópia foi rasa, talvez o `Endereco` do objeto clonado ainda seja o mesmo objeto do original. O resultado é que a mudança aparece em ambos.

Esse é um sinal claro de que a semântica da operação ficou ambígua. Se a intenção era que as duas instâncias fossem completamente independentes, uma `shallow copy` não basta.

<a id="sec-018"></a>
### 31.4 Quando a deep copy faz sentido

[↑ Voltar ao início](#inicio)

A `deep copy` é a escolha correta quando o objeto contém partes mutáveis que precisam ser isoladas. Por exemplo:

- uma lista de itens;
- um objeto `Address` ou `Configuration`;
- uma árvore de objetos aninhados;
- qualquer estado interno que não deva ser compartilhado entre duas instâncias.

Nesse caso, o clone precisa não só copiar o objeto principal, mas também duplicar cada referência mutável cuja identidade não deva ser compartilhada. Recursos como conexões, arquivos, locks e serviços geralmente não admitem uma “cópia profunda” genérica e exigem uma decisão própria do domínio.

<a id="sec-019"></a>
### 31.5 O problema de tipos não seguros

[↑ Voltar ao início](#inicio)

Outra crítica é o retorno do tipo `object`.

Isso significa que o código precisa fazer cast manualmente:

```csharp
var copia = (Pessoa)original.Clone();
```

Esse cast pode falhar em tempo de execução, e a interface não ajuda a evitar esse problema. Em outras palavras, a API é menos expressiva e menos segura do que seria desejável para um conceito tão importante quanto clonagem.

<a id="sec-020"></a>
### 31.6 Por que isso é ruim para Prototype

[↑ Voltar ao início](#inicio)

No contexto do `Prototype`, o problema fica ainda mais claro. O padrão exige uma ideia explícita de cópia. O código precisa comunicar com clareza:

- o que está sendo copiado;
- como a cópia acontece;
- se a cópia é rasa ou profunda;
- qual é a intenção da operação.

`ICloneable` faz o contrário: ele dá a impressão de resolver tudo, mas entrega uma abstração fraca e ambígua. Isso faz com que o padrão pareça mais complicado do que realmente é.

<a id="sec-021"></a>
### 31.7 O que fazer no lugar

[↑ Voltar ao início](#inicio)

Em vez de depender de `ICloneable`, o ideal é usar uma abordagem mais explícita, como:

- construtor de cópia;
- método `Clone()` com nome e semântica bem definidos;
- interface própria para cópia profunda;
- estratégia de clonagem controlada pelo próprio tipo.

Essa abordagem deixa o código mais legível, mais previsível e muito mais adequado ao padrão `Prototype`.

<a id="sec-022"></a>
### 31.8 Regra mental

[↑ Voltar ao início](#inicio)

Uma forma curta de lembrar isso é:

- `ICloneable` parece conveniente;
- mas entrega pouca clareza semântica;
- e para `Prototype`, clareza é essencial.

Esse é o motivo de a aula começar justamente por essa crítica: ela prepara o terreno para soluções mais robustas e explícitas.

---

<a id="sec-023"></a>
## 32. Copy Constructors

[↑ Voltar ao início](#inicio)


Outra forma de expressar a ideia de protótipo é usar um construtor de cópia. Nesse caso, a classe recebe uma instância existente e cria uma nova a partir dela, deixando a intenção do código mais explícita do que um `clone` genérico.

<a id="sec-024"></a>
### 32.1 O que esse exemplo ensina

[↑ Voltar ao início](#inicio)

O exemplo prático desta aula usa um objeto `Person` que é criado a partir de outro `Person`. A ideia é simples:

- `john` funciona como objeto base;
- `jane` é criada a partir de `john` usando um construtor de cópia;
- depois, `jane` recebe alterações locais, como trocar o primeiro nome e mudar o número da casa;
- essas mudanças não devem alterar `john`.

Nesse cenário, o construtor de cópia é a forma explícita de dizer: "quero um novo objeto com o mesmo estado inicial, mas sem compartilhar internamente os mesmos dados".

<a id="sec-025"></a>
### 32.2 Como o exemplo do código representa isso

[↑ Voltar ao início](#inicio)

No código, a criação acontece assim:

```csharp
var john = new Person(new string[] { "John", "Smith" },
    new Address("London Road", 123));

var jane = new Person(john);
```

Ou seja, `jane` não é criado do zero; ela nasce a partir de `john`.

Depois, o código faz:

```csharp
jane.Names[0] = "Jane";
jane.Address.HouseNumber = 321;
```

A intenção é mostrar que `jane` é uma cópia independente. O nome e o endereço alterados em `jane` não devem afetar `john`.

<a id="sec-026"></a>
### 32.3 Por que o array precisa ser copiado

[↑ Voltar ao início](#inicio)

Um detalhe importante aparece quando o objeto possui coleções, como `string[]`. Se o construtor de cópia apenas fizer:

```csharp
Names = other.Names;
```

então as duas instâncias passarão a compartilhar o mesmo array. Alterar `Names[0]` em uma cópia também altera na outra.

Por isso, o exemplo usa:

```csharp
Names = (string[])other.Names.Clone();
```

para garantir que o array de nomes seja independente.

<a id="sec-027"></a>
### 32.4 Por que o objeto composto também precisa ser copiado

[↑ Voltar ao início](#inicio)

No exemplo, `Person` possui um objeto `Address`. Se o construtor de cópia simplesmente reutilizasse a mesma referência, qualquer mudança em `Address` afetaria o objeto original.

Por isso, o exemplo cria uma nova instância de `Address`:

```csharp
Address = new Address(other.Address);
```

Assim, `jane` e `john` passam a ter estados internos independentes.

<a id="sec-028"></a>
### 32.5 Regra mental

[↑ Voltar ao início](#inicio)

A ideia central desta aula é:

- copiar o estado do objeto;
- evitar compartilhamento de referências internas;
- preservar a semântica de "novo objeto, mesmo estado inicial".

Essa é uma das bases mais importantes para entender o padrão `Prototype` em C#.

---

<a id="sec-029"></a>
## 33. Explicit Deep Copy Interface

[↑ Voltar ao início](#inicio)


Quando o contrato do domínio exige uma cópia profunda, uma opção clara é expor uma interface explícita para essa operação, deixando documentado quais componentes do estado serão duplicados.

<a id="sec-030"></a>
### 33.1 O que essa abordagem resolve

[↑ Voltar ao início](#inicio)

O exemplo desta aula mostra uma forma mais explícita de implementar o padrão `Prototype`. Em vez de depender de uma ideia vaga de "clone", o código define uma operação chamada `DeepCopy()`.

Essa abordagem deixa claro que:

- a cópia deve ser profunda;
- o objeto novo deve ter seu próprio estado;
- os objetos internos mutáveis selecionados pela política de cópia também devem ser duplicados;
- a intenção da operação é explícita para quem lê o código.

<a id="sec-031"></a>
### 33.2 Como o exemplo do código representa isso

[↑ Voltar ao início](#inicio)

No exemplo, `Person` implementa uma interface com o método `DeepCopy()`. Quando esse método é chamado, ele cria uma nova `Person` e também cria uma nova cópia de `Address`.

```csharp
public Person DeepCopy()
{
    return new Person(Names, Address.DeepCopy());
}
```

Esse trecho mostra a ideia central da aula:

- a cópia do objeto principal é feita;
- a cópia do objeto composto também é feita;
- a operação não depende de uma semântica implícita.

<a id="sec-032"></a>
### 33.3 Por que isso é melhor do que depender de `ICloneable`

[↑ Voltar ao início](#inicio)

A interface explícita torna o código mais legível e mais previsível. Ela responde diretamente a perguntas como:

- a cópia é profunda?
- quais partes do objeto são duplicadas?
- o comportamento é intencional e documentado?

Isso é muito mais claro do que depender de uma interface genérica cujo significado pode variar entre implementações.

<a id="sec-033"></a>
### 33.4 Regra mental

[↑ Voltar ao início](#inicio)

A ideia central desta aula é:

- deixar a clonagem explícita;
- dizer claramente que a cópia é profunda;
- evitar que o objeto clonado compartilhe estado interno com o original.

Essa é uma alternativa explícita e tipada para implementar `Prototype` em C#. Ela não é universal: para alguns domínios, um construtor de cópia, um registro imutável com `with` ou uma cópia seletiva comunica melhor a intenção.

---


<a id="sec-034"></a>
## 34. Prototype Inheritance

[↑ Voltar ao início](#inicio)


Esta parte corresponde ao arquivo `CreationalPatterns/4.Prototype/Aula04_PrototypeInhertance/PrototypeInhertance.cs`. O nome do arquivo mantém a grafia do repositório, mas o nome conceitual correto do padrão nesta aula é **Prototype Inheritance**.

O padrão de projeto **Prototype** visa criar novos objetos duplicando uma instância existente (o protótipo) em vez de criar uma nova do zero através de construtores complexos. No entanto, implementar a clonagem profunda (*Deep Copy*) se torna um desafio significativo quando introduzimos **herança** na árvore de classes.

Se uma classe base implementa uma clonagem simples (como o `MemberwiseClone()` do .NET), ela realiza apenas uma cópia rasa (*Shallow Copy*), compartilhando referências de objetos complexos. Além disso, se tentarmos usar uma abordagem convencional baseada em interfaces padrão (como `ICloneable`), a subclasse terá dificuldades para estender a lógica de clonagem da classe base sem recorrer a conversões de tipo (*type casting*) perigosas ou duplicação de código.

<a id="sec-035"></a>
### O problema da herança no Prototype

[↑ Voltar ao início](#inicio)

Quando uma subclasse herda de uma classe base, ela adiciona novos campos de estado. Para clonar o objeto da subclasse corretamente, precisamos:

1. Clonar os campos específicos da classe base.
2. Clonar os campos novos da subclasse.
3. Aplicar a política de cópia adequada aos objetos aninhados — no exemplo, endereços e arrays recebem novas instâncias.

Para resolver isso de forma elegante e fortemente tipada, o exemplo usa uma interface customizada `IPrototype<T>` combinada com o método padrão `CopyTo`.

---

<a id="sec-036"></a>
### Estrutura da solução

[↑ Voltar ao início](#inicio)

#### 1. A interface `IPrototype<T>`

Definimos um contrato que obriga os objetos a saberem como copiar suas propriedades para um objeto de destino já instanciado (`CopyTo`). A interface também traz uma implementação padrão para o método `DeepCopy()`, que cria uma nova instância e invoca o `CopyTo`.

```csharp
public interface IPrototype<T> where T : new()
{
    // Define como os dados atuais serão copiados para o objeto alvo
    void CopyTo(T target);

    // Implementação padrão que cria uma nova instância e realiza a cópia
    public T DeepCopy()
    {
        T t = new T();
        CopyTo(t);
        return t;
    }
}

```

#### 2. Objetos compostos (`Address`)

Classes usadas como propriedades também precisam de uma estratégia de cópia compatível com o isolamento desejado. No exemplo, `Address` implementa `IPrototype<Address>`; essa não é a única opção possível.

```csharp
public class Address : IPrototype<Address>
{
    public string StreetName;
    public int HouseNumber;

    public Address() { }

    public Address(string streetName, int houseNumber)
    {
        StreetName = streetName;
        HouseNumber = houseNumber;
    }

    public void CopyTo(Address target)
    {
        target.StreetName = StreetName;
        target.HouseNumber = HouseNumber;
    }

    public override string ToString() =>
        $"{nameof(StreetName)}: {StreetName}, {nameof(HouseNumber)}: {HouseNumber}";
}

```

#### 3. A classe base (`Person`)

A classe base implementa a interface e define seu método `CopyTo` como `virtual`, permitindo que as subclasses estendam seu comportamento. Note que tipos de referência como arrays (`string[]`) e objetos customizados (`Address`) precisam ser clonados explicitamente.

```csharp
public class Person : IPrototype<Person>
{
    public string[] Names;
    public Address Address;

    public Person() { }

    public Person(string[] names, Address address)
    {
        Names = names;
        Address = address;
    }

    // Método virtual para permitir que subclasses reutilizem e estendam a lógica
    public virtual void CopyTo(Person target)
    {
        target.Names = (string[]) Names.Clone(); // Cópia do Array
        target.Address = Address.DeepCopy();     // Cópia profunda do objeto composto
    }

    public override string ToString() =>
        $"{nameof(Names)}: {string.Join(",", Names)}, {nameof(Address)}: {Address}";
}

```

#### 4. A subclasse (`Employee`)

A subclasse herda de `Person` e implementa `IPrototype<Employee>`. No seu método `CopyTo`, ela chama explicitamente `base.CopyTo(target)` para garantir que toda a árvore de herança superior seja copiada, e em seguida copia seu próprio estado específico (`Salary`).

```csharp
public class Employee : Person, IPrototype<Employee>
{
    public int Salary;

    public void CopyTo(Employee target)
    {
        // Reutiliza a cópia dos campos da classe base (Names e Address)
        base.CopyTo(target);

        // Copia o campo específico da subclasse
        target.Salary = Salary;
    }

    public override string ToString() =>
        $"{base.ToString()}, {nameof(Salary)}: {Salary}";
}

```

---

<a id="sec-037"></a>
### Facilitando a sintaxe com métodos de extensão

[↑ Voltar ao início](#inicio)

Para evitar a necessidade de realizar casts manuais para a interface `IPrototype<T>` no código do cliente, criamos métodos de extensão utilitários:

```csharp
public static class DeepCopyExtensions
{
    // Permite chamar .DeepCopy() diretamente em qualquer tipo IPrototype
    public static T DeepCopy<T>(this IPrototype<T> item) where T : new()
    {
        return item.DeepCopy();
    }

    // Sobrecarga específica para a hierarquia de Person que simplifica a chamada de genéricos
    public static T DeepCopy<T>(this T person) where T : Person, new()
    {
        return ((IPrototype<T>) person).DeepCopy();
    }
}

```

---

<a id="sec-038"></a>
### Demonstração prática de uso

[↑ Voltar ao início](#inicio)

Abaixo vemos como o cliente consome a estrutura de protótipo com herança. Modificações feitas no objeto clonado (`copy`) não afetam de forma alguma o protótipo original (`john`).

```csharp
public static class Demo
{
    static void Main()
    {
        // Criando o protótipo inicial (Employee)
        var john = new Employee();
        john.Names = new[] {"John", "Doe"};
        john.Address = new Address { HouseNumber = 123, StreetName = "London Road" };
        john.Salary = 321000;

        // Clonando através do Prototype Inheritance
        var copy = john.DeepCopy();

        // Modificando os valores do objeto clonado
        copy.Names[1] = "Smith";
        copy.Address.HouseNumber++;
        copy.Salary = 123000;

        // Exibindo ambos para comprovar o isolamento total
        Console.WriteLine("Original: " + john);
        Console.WriteLine("Cópia:    " + copy);
    }
}

```

#### Saída do console

```text
Original: Names: John,Doe, StreetName: London Road, HouseNumber: 123, Salary: 321000
Cópia:    Names: John,Smith, StreetName: London Road, HouseNumber: 124, Salary: 123000

```

<a id="sec-039"></a>
### Conclusão

[↑ Voltar ao início](#inicio)

Acoplar `Prototype` com herança exige uma estratégia cuidadosa de delegação de responsabilidades. Ao usar `CopyTo(target)` e chamar `base.CopyTo(target)`, o exemplo distribui a cópia entre os níveis da hierarquia. A solução permanece correta somente enquanto cada classe copiar todo o estado relevante e respeitar a política definida; novos campos exigem manutenção explícita.

<a id="sec-040"></a>
### Conceitos de C# que aparecem no exemplo

[↑ Voltar ao início](#inicio)

Alguns trechos do código podem parecer estranhos para quem está começando. Eles são importantes porque ajudam a deixar a implementação mais flexível e mais idiomática em C#.

#### `where T : new()`

Essa é uma restrição de tipo genérico. Ela diz que o tipo `T` precisa ter um construtor público sem parâmetros. Ou seja, a classe pode ser instanciada com `new T()`.

```csharp
public interface IPrototype<T> where T : new()
```

Em termos práticos, isso significa que o código pode criar uma nova instância do tipo genérico sem precisar saber qual é o tipo concreto em tempo de compilação.

#### `base.CopyTo(target)`

A palavra-chave `base` serve para acessar membros da classe pai. Quando a subclasse chama `base.CopyTo(target)`, ela executa a implementação da classe base e depois acrescenta o que é específico dela.

```csharp
public void CopyTo(Employee target)
{
    base.CopyTo(target);
    target.Salary = Salary;
}
```

Isso evita duplicar a lógica de cópia já implementada em `Person`.

#### `this` em métodos de extensão

A palavra-chave `this` no primeiro parâmetro de um método indica que ele é um método de extensão. Isso permite chamar o método como se ele fosse membro do tipo que recebe o argumento.

```csharp
public static T DeepCopy<T>(this IPrototype<T> item)
```

Com isso, em vez de escrever algo como `DeepCopy(john)`, você pode escrever `john.DeepCopy()`. A ideia é deixar a API mais natural e legível.

#### Resumo mental

- `where T : new()` significa: “este tipo genérico pode ser criado com um construtor sem parâmetros”;
- `base` significa: “use a implementação da classe pai”;
- `this` em um método de extensão significa: “este método pode ser chamado como se fosse parte do objeto”.

---

<a id="sec-041"></a>
## 34.1 Prototype em jogos: o caso de entidades já configuradas

[↑ Voltar ao início](#inicio)


Um exemplo clássico de `Prototype` aparece em jogos, especialmente quando o engine precisa criar muitas instâncias parecidas de um mesmo tipo. O caso mais direto é o de um inimigo, um projétil, um item de inventário ou uma unidade de combate que já vem com uma configuração inicial bem definida.

Em vez de reconstruir tudo sempre do zero, o sistema pode manter um objeto-base, já configurado, e criar cópias dele para cada nova ocorrência.

<a id="sec-042"></a>
### O problema

[↑ Voltar ao início](#inicio)

Imagine que o jogo tenha uma arma, um monstro ou um efeito visual que precisa ser criado várias vezes, e cada criação exige:

- definir velocidade;
- definir dano;
- configurar textura, som e comportamento;
- inicializar estados internos e referências.

Se cada nova instância fosse montada manualmente, o código ficaria repetitivo, caro e sujeito a inconsistências.

<a id="sec-043"></a>
### A ideia do `Prototype`

[↑ Voltar ao início](#inicio)

A solução é simples e poderosa:

1. criar um objeto modelo, já pronto;
2. usar esse objeto como protótipo;
3. copiar esse protótipo para gerar uma nova instância;
4. ajustar apenas os detalhes que realmente mudam.

Esse é exatamente o tipo de cenário em que o padrão faz sentido: a criação por reconstrução completa é mais custosa do que copiar e adaptar um objeto que já representa uma forma válida.

<a id="sec-044"></a>
### Exemplo mental

[↑ Voltar ao início](#inicio)

Suponha que exista um `EnemyPrototype` com:

- vida inicial;
- velocidade;
- ataque;
- comportamento padrão.

Quando o jogo precisa criar um novo inimigo, ele não recomeça do zero. Em vez disso, ele clona o protótipo e então faz pequenas mudanças, como trocar o nome, a posição ou o nível de dificuldade.

<a id="sec-045"></a>
### Por que isso é interessante em design

[↑ Voltar ao início](#inicio)

Esse exemplo mostra que o `Prototype` não é uma "cópia qualquer". Ele é uma estratégia de criação que:

- reduz trabalho repetitivo;
- evita duplicação de lógica de inicialização;
- permite criar variações rápidas a partir de um modelo conhecido;
- torna a criação mais flexível quando os objetos têm muitos detalhes internos.

<a id="sec-046"></a>
### Ligação com o padrão GoF

[↑ Voltar ao início](#inicio)

No contexto do GoF, isso significa que o `Prototype` é útil quando:

- a criação é cara;
- a configuração inicial é complexa;
- o objeto já representa uma forma válida que deve ser reutilizada.

Em resumo, o padrão ajuda a transformar "criar do zero" em "copiar um modelo já bom".

---

<a id="sec-047"></a>
## 35. Copy Through Serialization

[↑ Voltar ao início](#inicio)


Uma alternativa prática para copiar objetos complexos é serializar e desserializar o estado do objeto. Essa abordagem pode ser útil quando a estrutura já é bem definida e se deseja uma cópia completa sem reimplementar manualmente todo o fluxo.

<a id="sec-048"></a>
### 35.1 O que significa serializar um objeto?

[↑ Voltar ao início](#inicio)

Para um iniciante, a ideia de "serialização" pode parecer abstrata. Em termos simples, serializar significa transformar um objeto em uma sequência de dados que possa ser armazenada, enviada ou reconstruída depois.

Imagine um objeto `Person` com nome, endereço e salário. Em vez de pensar em "como copiar isso na memória", a ideia é converter esse estado em algo mais simples, como uma sequência de bytes, que pode ser guardada em um fluxo e lida depois.

Esse processo é parecido com tirar uma foto do objeto em um determinado momento. A foto não é o objeto em si, ela é uma representação dele.

E, neste exemplo em particular, a serialização é realmente binária. Isso significa que o estado do objeto é gravado em bytes, e não em texto legível. O código usa um fluxo binário para escrever valores como strings, inteiros e booleanos em formato binário.

<a id="sec-049"></a>
### 35.2 O que é desserialização?

[↑ Voltar ao início](#inicio)

Desserialização é o processo inverso. Em vez de transformar um objeto em dados, o código lê esses dados de volta e reconstrói um novo objeto com o mesmo estado.

Em outras palavras:

- serialização: objeto → dados;
- desserialização: dados → novo objeto.

Esse ciclo é o que torna interessante a ideia de "copiar via serialização" no contexto do padrão `Prototype`.

<a id="sec-050"></a>
### 35.3 Por que isso tem relação com Prototype?

[↑ Voltar ao início](#inicio)

O `Prototype` existe para criar um novo objeto com base em um já existente. Se você já tem um objeto configurado e quer gerar outra versão dele, uma forma de fazer isso é:

1. pegar o objeto original;
2. transformar seu estado em uma representação serializada;
3. reconstruir um novo objeto a partir dessa representação;
4. usar esse novo objeto como cópia do protótipo.

Ou seja, em vez de montar o objeto do zero, você "captura" o estado do protótipo e o reutiliza para criar outra instância.

Essa ideia é muito parecida com a ideia do padrão: criar uma nova instância a partir de uma já existente.

<a id="sec-051"></a>
### 35.4 O que era o BinaryFormatter?

[↑ Voltar ao início](#inicio)

Historicamente, em C#, uma forma muito comum de fazer isso era usar o `BinaryFormatter`. Ele era uma API que permitia converter objetos em dados binários e depois reconstruí-los.

O detalhe importante é que, no exemplo deste repositório, a serialização não está sendo feita com `BinaryFormatter`. Ela está sendo feita de forma manual, usando `BinaryWriter` e `BinaryReader`, o que também produz dados binários, mas sem depender daquela API antiga.

O problema é que essa abordagem ficou conhecida por ser insegura e frágil. A Microsoft passou a recomendar que ela não fosse usada em aplicações modernas.

Com o tempo, o `BinaryFormatter` foi marcado como obsoleto e, em versões mais novas do .NET, deixou de ser uma opção viável para execução normal. O principal motivo é que ele depende de um mecanismo antigo de serialização que não é mais recomendado para software atual.

Por isso, o exemplo deste repositório não usa o `BinaryFormatter` diretamente. Em vez disso, ele mostra uma abordagem manual, didática e simplificada, com o intuito de explicar o conceito de serialização e desserialização sem depender dessa API antiga.

<a id="sec-052"></a>
### 35.5 O que é importante entender sobre esse exemplo?

[↑ Voltar ao início](#inicio)

Este exemplo não pretende ser a forma mais moderna, nem a forma mais recomendada de serialização em C#. Ele existe para ensinar o conceito.

O ponto central é este:

- o `Prototype` quer criar uma cópia a partir de um objeto existente;
- a serialização oferece uma forma de capturar o estado desse objeto;
- a desserialização permite reconstruir outro objeto a partir desse estado.

Essa é a ponte entre o padrão e o conceito de serialização.

<a id="sec-053"></a>
### 35.6 Diferenças entre Prototype por serialização e as outras formas de Prototype

[↑ Voltar ao início](#inicio)

É importante separar claramente essas abordagens, porque elas parecem parecidas, mas não são iguais.

#### 1. Prototype via construtor de cópia

Nesse caso, a classe recebe uma outra instância e cria uma nova a partir dela manualmente.

Exemplo mental:

- o código diz explicitamente: "crie um novo objeto e copie estes campos".
- a intenção fica clara.
- a lógica é controlada diretamente pela classe.

Essa é uma forma muito explícita e fácil de entender.

#### 2. Prototype via interface explícita (`DeepCopy()`)

Aqui a classe expõe um método próprio, como `DeepCopy()`, para indicar que a cópia deve ser feita de forma controlada e profunda.

Essa abordagem costuma ser mais limpa do que depender de uma infraestrutura externa.

#### 3. Prototype via herança (`Prototype Inheritance`)

Nesta abordagem, a lógica de clonagem é organizada em uma hierarquia de classes. A classe base define como copiar seus próprios campos, e as subclasses estendem esse comportamento para incluir o estado específico delas.

Exemplo mental:

- a classe base cuida da parte comum da cópia;
- a subclasse adiciona o que é próprio dela;
- o processo de clonagem é reutilizado de forma estruturada.

Essa forma é especialmente útil quando o sistema trabalha com herança e quer evitar duplicação de código. O ponto forte dela é a reutilização da lógica de cópia entre níveis da hierarquia.

#### 4. Prototype via serialização

Aqui a cópia não é feita "na mão" campo a campo. Em vez disso, o objeto é transformado em uma representação intermediária e depois reconstruído.

Essa abordagem pode parecer elegante, mas tem mais complexidade conceitual. Ela depende de um processo de transformação de estado, e não de uma implementação direta da clonagem.

<a id="sec-054"></a>
### 35.7 Quando essa abordagem faz sentido?

[↑ Voltar ao início](#inicio)

A serialização pode fazer sentido quando:

- o objeto já tem uma estrutura bem definida;
- o mecanismo escolhido representa todo o estado que deve participar da cópia;
- a lógica de cópia manual seria tediosa;
- o contexto permite usar um mecanismo de serialização bem suportado.

Mas serialização não é um requisito do padrão `Prototype` e não deve ser presumida como cópia completa: membros ignorados pelo serializador, recursos externos, callbacks, identidade e versionamento podem alterar o resultado. Em C# moderno, a solução mais comum é usar cópia explícita, construtor de cópia ou métodos próprios, escolhidos conforme o contrato do objeto.

<a id="sec-055"></a>
## Coding Exercise 3: Prototype Coding Exercise

[↑ Voltar ao início](#inicio)


Esta seção analisa a implementação prática do padrão **Prototype** contida no arquivo `PrototypeExcercise.cs`. O exercício demonstra a mecânica clássica de uma clonagem profunda (*Deep Copy*) em uma estrutura de dados composta.

<a id="sec-056"></a>
### O que o exercício quer de verdade

[↑ Voltar ao início](#inicio)

O objetivo deste exercício é consolidar a diferença entre cópias rasas e cópias profundas em tipos de referência:

- Criar um contrato explícito de clonagem através de uma interface genérica (`IPrototype<T>`).
- Implementar o método de cópia no objeto simples (`Point`), garantindo o isolamento de suas coordenadas.
- Implementar o método de cópia no objeto composto (`Line`), assegurando que a nova linha gerada não compartilhe referências com a linha original.

A API esperada pelo cliente final segue este fluxo:

```csharp
var line = new Line(new Point(1, 2), new Point(3, 4));
var copy = line.DeepCopy();

```

As expressões abaixo representam as condições esperadas, embora o arquivo local apenas imprima os valores e não contenha um framework de testes ou asserções executáveis:

```csharp
copy.Start.X == 1;
copy.End.Y == 4;

```

<a id="sec-057"></a>
### Anatomia das referências e das alocações no exemplo

[↑ Voltar ao início](#inicio)

Um dos pontos mais contra-intuitivos da implementação está localizado no construtor da classe `Line`:

```csharp
public Line(Point start, Point end)
{
    if (start == null) throw new ArgumentNullException(nameof(start));
    if (end == null) throw new ArgumentNullException(nameof(end));

    Start = start.DeepCopy();
    End = end.DeepCopy();
}

```

Para compreender a necessidade dessa abordagem, imagine o cenário padrão onde o cliente instancia uma nova linha:

```csharp
var line = new Line(new Point(1, 2), new Point(3, 4));

```

Na leitura direta do código-fonte, antes de eventuais otimizações do JIT, essa instrução envolve **uma instância de `Line` e seis construções de `Point`**:

1. dois `Point` são criados como argumentos;
2. os inicializadores dos campos `Start = new Point()` e `End = new Point()` executam antes do corpo do construtor;
3. o construtor cria mais dois `Point` por meio de `start.DeepCopy()` e `end.DeepCopy()`;
4. as quatro instâncias temporárias substituídas deixam de ser alcançáveis pelo programa e ficam elegíveis para coleta; não é correto afirmar quando o GC as recolherá.

Essa contagem descreve as construções expressas no código, não uma garantia sobre alocações físicas após otimizações do runtime. Os inicializadores dos campos são redundantes para o construtor que recebe os dois pontos.

#### A Justificativa Técnico-Prática: Evitando Efeitos Colaterais Ocultos

Se modificássemos o construtor para salvar diretamente as referências recebidas (`Start = start;`), as linhas poderiam compartilhar objetos `Point`. Isso não é necessariamente uma falha: é correto ou incorreto conforme o contrato de propriedade e mutabilidade do domínio.

Considere um cenário real de reaproveitamento de pontos em geometrias conectadas, como em um polígono ou malha de jogo:

```csharp
var linha1 = new Line(new Point(0, 0), new Point(5, 5));

// Criando a segunda linha REAPROVEITANDO o ponto final da linha 1
var linha2 = new Line(linha1.End, new Point(10, 10));

```

Caso o construtor não utilizasse o `.DeepCopy()` preventivo:

- As propriedades `linha1.End` e `linha2.Start` conteriam referências para a mesma instância de `Point`; o código gerenciado não precisa conhecer seu endereço físico.
- Uma modificação posterior do tipo `linha2.Start.X = 20;` alteraria secretamente a coordenada final da `linha1`, corrompendo a primeira linha sem o conhecimento do desenvolvedor.

A duplicação no construtor é uma estratégia de **cópia defensiva**: ela impede que mutações feitas pelos chamadores por meio das referências de entrada atinjam os pontos internos. Essa proteção não é absoluta, pois `Start` e `End` continuam públicos e mutáveis; encapsulamento completo exigiria restringir sua exposição ou usar tipos imutáveis.

Há ainda uma ineficiência didática em `Line.DeepCopy()`: ele clona `Start` e `End` antes de chamar um construtor que volta a cloná-los. Uma implementação enxuta copiaria cada ponto uma única vez, sem duplicar o trabalho.

---

<a id="sec-058"></a>
### Participantes do Padrão no Exercício

[↑ Voltar ao início](#inicio)

Mapeando o código do exercício para os papéis estruturais do padrão Prototype:

- **`IPrototype<T>` (Prototype Interface):** Define a assinatura tipada para o mecanismo de clonagem profunda, contornando a falta de tipagem segura da interface nativa `ICloneable` do .NET.
- **`Point` (Concrete Prototype - Produto Simples):** Sabe clonar seus próprios campos primitivos de valor (`int X, Y`), gerando uma cópia idêntica e isolada.
- **`Line` (Composite Prototype - Objeto Composto):** Gerencia objetos complexos e delega a responsabilidade de clonagem em cascata para seus próprios membros antes de retornar a nova instância.
- **`Program.Main` (Client):** Cria as configurações iniciais do protótipo e solicita a geração de novas linhas independentes a partir dele.

---

<a id="sec-059"></a>
### Código-fonte guiado e resolvido

[↑ Voltar ao início](#inicio)

Abaixo está a implementação comentada do arquivo local. Ela preserva a independência dos pontos exigida pelo exercício, mas contém as alocações redundantes explicadas acima:

```csharp
using System;

namespace Coding.Exercise
{
    // ===== 1º Interface do Prototype =====
    public interface IPrototype<T>
    {
        T DeepCopy();
    }

    // ===== 2º Classe do Produto Simples =====
    public class Point : IPrototype<Point>
    {
        public int X, Y;

        public Point() { }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        // Cada cópia de Point precisa criar uma nova instância, não reaproveitar a antiga
        public Point DeepCopy()
        {
            return new Point(X, Y); // Retorna nova alocação com os dados do objeto atual
        }
    }

    // ===== 3º Classe do Objeto Composto =====
    public class Line : IPrototype<Line>
    {
        public Point Start = new Point();
        public Point End = new Point();

        public Line() { }

        public Line(Point start, Point end)
        {
            if (start == null)
            {
                throw new ArgumentNullException(paramName: nameof(start));
            }
            if (end == null)
            {
                throw new ArgumentNullException(paramName: nameof(end));
            }

            // Isolamento preventivo no momento da construção da linha
            Start = start.DeepCopy();
            End = end.DeepCopy();
        }

        // A cópia profunda cria uma nova Line e também duplica cada Point interno
        public Line DeepCopy()
        {
            // Antes de clonar, a Line precisa ter os dois pontos configurados corretamente
            if (Start == null || End == null)
            {
                throw new InvalidOperationException("Line must have both points configured before cloning.");
            }

            // Invoca o construtor da nova linha passando novos clones gerados a partir do estado atual
            return new Line(Start.DeepCopy(), End.DeepCopy());
        }
    }

    // ===== 4º Execução do Cliente =====
    public static class Program
    {
        public static void Main()
        {
            // O cliente cria um protótipo já configurado
            var line = new Line(new Point(1, 2), new Point(3, 4));

            // O cliente pede uma cópia profunda independente do protótipo
            var copy = line.DeepCopy();

            // Cada ponto da cópia é uma instância distinta dos pontos do protótipo
            Console.WriteLine($"Cópia - Start X: {copy.Start.X}"); // Deve imprimir 1
            Console.WriteLine($"Cópia - End Y: {copy.End.Y}");     // Deve imprimir 4
        }
    }
}

```


<a id="sec-glossario"></a>
## Glossário
[↑ Voltar ao início](#inicio)

- **Prototype:** objeto usado como modelo para criar novas instâncias.
- **Clonagem:** criação de um objeto com base no estado de outro.
- **Cópia rasa:** copia referências sem duplicar os objetos referenciados.
- **Cópia profunda:** duplica também os objetos mutáveis internos necessários.
- **Registro de protótipos:** catálogo de modelos disponíveis para clonagem.
