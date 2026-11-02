<a id="inicio"></a>
## Sumário
[↑ Voltar ao início](#inicio)

- [1. O que o Decorator representa?](#sec-001)
- [2. Em que situações se usa?](#sec-002)
- [3. Como implementar passo a passo](#sec-003)
- [4. Exemplo Prático (Cafeteria)](#sec-004)
- [5. Implementações reais do repositório](#sec-005)
- [6. A diferença entre envolver e decorar](#sec-006)
- [Ordem, transparência e alternativa à herança](#sec-007)
- [Variações e aprofundamentos](#sec-008)
- [60. Custom String Builder](#sec-009)
  - [60.1 O que esta aula está tentando mostrar](#sec-010)
  - [60.2 Onde está a relação com Decorator](#sec-011)
  - [60.3 Por que não herdar de StringBuilder?](#sec-012)
  - [60.4 Leitura guiada do campo interno](#sec-013)
  - [60.5 O papel de ToString](#sec-014)
  - [60.6 O padrão que se repete nos métodos mutáveis](#sec-015)
  - [60.7 Por que esse trecho se repete tanto no código](#sec-016)
  - [60.8 O pedágio da delegação](#sec-017)
  - [60.9 Métodos que não retornam CodeBuilder](#sec-018)
  - [60.10 As propriedades delegadas](#sec-019)
  - [60.11 O método Equals merece atenção](#sec-020)
  - [60.12 Demonstração final](#sec-021)
  - [60.13 O que esta aula ensina de verdade](#sec-022)
- [61. Adapter Decorator](#sec-023)
  - [61.1 O que esta aula está tentando mostrar](#sec-024)
  - [61.2 Por que o nome é Adapter Decorator](#sec-025)
  - [61.3 O campo interno](#sec-026)
  - [61.4 A conversão implícita de string para MyStringBuilder](#sec-027)
  - [61.5 O operador +](#sec-028)
  - [61.6 Por que s += "world" poderia funcionar mesmo sem o operator +](#sec-029)
  - [61.7 O papel de ToString](#sec-030)
  - [61.8 A grande diferença para CodeBuilder](#sec-031)
  - [61.9 Por que os métodos delegados se repetem tanto](#sec-032)
  - [61.10 Propriedades delegadas](#sec-033)
  - [61.11 O método Equals](#sec-034)
  - [61.12 Demonstração final](#sec-035)
  - [61.13 O que esta aula ensina de verdade](#sec-036)
- [62. Multiple Inheritance with Interfaces and Default Interface Members](#sec-037)
  - [62.1 O que esta aula está tentando mostrar](#sec-038)
  - [62.2 O problema da herança múltipla de classes](#sec-039)
  - [62.3 O contrato IBird](#sec-040)
  - [62.4 A implementação concreta Bird](#sec-041)
  - [62.5 O contrato ILizard](#sec-042)
  - [62.6 A implementação concreta Lizard](#sec-043)
  - [62.7 Dragon implementa os dois contratos](#sec-044)
  - [62.8 Os campos internos agora dependem de interfaces](#sec-045)
  - [62.9 O construtor de conveniência](#sec-046)
  - [62.10 O construtor principal](#sec-047)
  - [62.11 Guard clauses no construtor](#sec-048)
  - [62.12 Delegação: Dragon.Fly](#sec-049)
  - [62.13 Delegação: Dragon.Crawl](#sec-050)
  - [62.14 A demonstração final](#sec-051)
  - [62.15 Por que usar isso em vez de só classes concretas](#sec-052)
  - [62.16 Relação com Decorator](#sec-053)
  - [62.17 O que esta aula ensina de verdade](#sec-054)
  - [62.18 Onde entram Default Interface Members](#sec-055)
- [63. Dynamic Decorator Composition](#sec-056)
- [64. Detecting Decorator Cycles](#sec-057)
- [65. Static Decorator Composition](#sec-058)
- [66. Decorator in Dependency Injection](#sec-059)
- [Coding Exercise 8: Decorator Coding Exercise](#sec-060)



**Família GoF:** estrutural. **Escopo predominante:** objetos encapsulados e combinados em runtime.
<a id="sec-001"></a>
## 1. O que o Decorator representa?

[↑ Voltar ao início](#inicio)

O **Decorator** é um padrão de projeto estrutural que permite adicionar novas responsabilidades a um objeto de forma dinâmica, sem modificar sua estrutura original e sem recorrer à criação de subclasses para cada combinação possível.

* **O Problema:** O uso de herança para adicionar recursos opcionais gera uma explosão combinatória de subclasses ($2^N$), tornando o código rígido e estático em tempo de compilação.
* **A Solução:** Envolver o objeto principal em camadas sucessivas de objetos decoradores que compartilham a mesma interface, delegando as chamadas e somando comportamentos em tempo de execução.

<a id="sec-002"></a>
## 2. Em que situações se usa?

[↑ Voltar ao início](#inicio)

* Quando funcionalidades opcionais precisam ser combinadas livremente em tempo de execução.
* Quando o uso de herança resultaria em uma hierarquia complexa, duplicada ou inviável de subclasses.
* Quando é necessário estender o comportamento de um objeto de maneira totalmente transparente para o código cliente.

<a id="sec-003"></a>
## 3. Como implementar passo a passo

[↑ Voltar ao início](#inicio)

* **Interface Comum:** Defina o contrato que será compartilhado pelo objeto base e pelos decoradores.
* **Componente Concreto:** Implemente a classe base com o comportamento fundamental do negócio.
* **Decorator Abstrato:** Crie uma classe que implementa a interface e armazena internamente uma referência para o componente encapsulado.
* **Decorators Concretos:** Estenda o decorator abstrato para acrescentar novos comportamentos (antes ou depois de delegar a execução para o objeto interno).

<a id="sec-004"></a>
## 4. Exemplo Prático (Cafeteria)

[↑ Voltar ao início](#inicio)

Em um sistema de cafeteria, em vez de criar dezenas de classes estáticas para cada mistura de ingredientes, você instancia a bebida base e a envolve dinamicamente com os complementos desejados:
`Bebida meuCafe = new Chantilly(new Leite(new CafeExpresso()));`
Cada camada decoradora intercepta os métodos (como `getPreco()` ou `getDescricao()`), soma o seu próprio valor/texto ao resultado da camada interna e retorna o total de forma flexível.

---

Tem alguma dúvida sobre como o Decorator se diferencia de outros padrões estruturais como o Adapter?

<a id="sec-005"></a>
## 5. Implementações reais do repositório

[↑ Voltar ao início](#inicio)

A aula [`Aula01_CustomStringBuilder.cs`](Aula01_CustomStringBuilder/Aula01_CustomStringBuilder.cs) constrói uma API encadeável sobre `StringBuilder`, mantendo um objeto interno e delegando operações. A aula [`AdapterDecorator.cs`](Aula02_AdapterDecorator/AdapterDecorator.cs) também envolve um `StringBuilder`, mas acrescenta a conversão implícita de `string` para `MyStringBuilder`; portanto, combina o papel de wrapper/delegador com uma adaptação de entrada. Ela ilustra que padrões podem colaborar e que o nome do exemplo não determina uma única intenção.

O exercício de múltipla herança por interfaces em [`InhertanceWithInterfaces.cs`](Aula03_MultipleInhertanceWithInterface/InhertanceWithInterfaces.cs) discute composição de comportamento. Para identificar um Decorator, procure um objeto que implemente o mesmo contrato do componente e contenha outro componente desse contrato. Ele acrescenta responsabilidade e delega o restante, podendo ser empilhado em runtime.

Na implementação prática, construa a cadeia explicitamente (por exemplo, componente base → compressão → criptografia) e mantenha a ordem clara: decorar uma operação pode alterar o resultado visto pelas próximas camadas. Evite wrappers que apenas duplicam a API sem acrescentar comportamento útil. Os arquivos deste diretório são exemplos de aula; não há no projeto uma versão de cafeteria correspondente à seção introdutória acima.


<a id="sec-006"></a>
## 6. A diferença entre envolver e decorar

[↑ Voltar ao início](#inicio)

A aula [Aula01_CustomStringBuilder.cs](Aula01_CustomStringBuilder/Aula01_CustomStringBuilder.cs) mostra um wrapper que delega operações a StringBuilder. A estrutura de composição aparece, mas para reconhecê-lo como Decorator é preciso procurar a intenção: o wrapper mantém o contrato do componente e acrescenta comportamento que pode ser combinado. Apenas copiar métodos para uma fachada não demonstra necessariamente decoração.

Na aula [AdapterDecorator.cs](Aula02_AdapterDecorator/AdapterDecorator.cs), MyStringBuilder guarda um StringBuilder e repassa operações. O operador implícito também converte uma string simples para o tipo esperado. A conversão tem intenção de Adapter; o wrapper/delegação lembra Decorator. O exemplo é útil para notar que uma classe pode combinar mais de um papel, então classifique pela finalidade de cada parte, não só pela forma “objeto dentro de objeto”.

Em um cenário de log/compressão, por exemplo, um componente base processa dados; decorators do mesmo contrato podem registrar, comprimir e então delegar. O cliente escolhe a combinação sem criar uma subclasse para cada combinação. A ordem da pilha importa e deve ser testada. Use Decorator quando o comportamento adicional precisa ser aplicado por instância e composto; evite se wrappers apenas duplicam uma API extensa.


<a id="sec-007"></a>
## Ordem, transparência e alternativa à herança

[↑ Voltar ao início](#inicio)

Cada decorator deve implementar o contrato do componente e guardar outro objeto desse contrato. Assim pode ser empilhado em runtime. A ordem afeta o resultado: compressão antes de criptografia não equivale a criptografar antes de comprimir; um decorator de cache antes de logging pode evitar que o logger veja chamadas atendidas pelo cache.

Use quando responsabilidades opcionais precisam ser combinadas por objeto. Não use apenas para renomear métodos incompatíveis (Adapter), simplificar um subsistema (Facade) ou controlar acesso ao objeto real (Proxy). Se todos os clientes sempre recebem a mesma combinação fixa, composição direta ou um serviço dedicado pode ser mais simples.

Um custo frequente é repetir/delegar muitos membros, especialmente quando o contrato original é grande. Mantenha interfaces pequenas e considere pipeline/middleware quando operações forem naturalmente sequenciais. Os wrappers do StringBuilder local demonstram composição/delegação, mas não formam sozinhos um decorator clássico com responsabilidade adicional; a página separa essa distinção deliberadamente.

<a id="sec-008"></a>
## Variações e aprofundamentos

[↑ Voltar ao início](#inicio)

As seções abaixo reúnem variações, exemplos e decisões que complementam a explicação principal deste padrão. Elas preservam a linguagem e o contexto do material didático original.

<a id="sec-009"></a>
## 60. Custom String Builder

[↑ Voltar ao início](#inicio)


<a id="sec-010"></a>
### 60.1 O que esta aula está tentando mostrar

[↑ Voltar ao início](#inicio)

O arquivo desta aula é:

- `StructuralPatterns/9.Decorator/Aula01_CustomStringBuilder.cs`

A ideia central é criar uma classe chamada `CodeBuilder` que envolve um `StringBuilder` real.

O objetivo não é reescrever a lógica de montagem de texto. O .NET já possui `System.Text.StringBuilder`, que é uma classe eficiente para acumular texto mutável. O objetivo da aula é outro:

**envolver um objeto existente para oferecer uma API mais conveniente ao cliente.**

Neste caso, a conveniência é permitir encadeamento fluente:

```csharp
var cb = new CodeBuilder();

cb.AppendLine("class Foo")
  .AppendLine("{")
  .AppendLine("}");
```

A diferença parece pequena, mas é importante para leitura:

```text
Sem a API fluent:
  cb.AppendLine("class Foo");
  cb.AppendLine("{");
  cb.AppendLine("}");

Com a API fluent:
  cb.AppendLine("class Foo")
    .AppendLine("{")
    .AppendLine("}");
```

O segundo formato comunica melhor a ideia de uma construção contínua do mesmo texto.

---

<a id="sec-011"></a>
### 60.2 Onde está a relação com Decorator

[↑ Voltar ao início](#inicio)


No sentido amplo, este exemplo se aproxima de Decorator porque existe um objeto externo envolvendo outro objeto:

```csharp
public class CodeBuilder
{
  private StringBuilder builder = new StringBuilder();
}
```

`CodeBuilder` não guarda texto sozinho. Ele guarda um `StringBuilder`.

Quando o cliente chama:

```csharp
cb.AppendLine("class Foo");
```

o wrapper faz:

```csharp
builder.AppendLine("class Foo");
return this;
```

Ou seja:

```text
cliente
  chama CodeBuilder.AppendLine(...)

CodeBuilder
  delega para StringBuilder.AppendLine(...)
  devolve ele mesmo para continuar a cadeia

StringBuilder
  executa a montagem real do texto
```

Esse movimento é parecido com a mecânica de Decorator:

- existe composição;
- existe delegação;
- existe uma camada externa em volta de um objeto existente;
- o cliente passa a trabalhar com a camada externa.

Mas existe uma honestidade importante aqui: **este não é um Decorator GoF perfeito**.

No Decorator clássico, normalmente existe um contrato comum:

```text
Component
  ConcreteComponent
  Decorator
```

O decorator implementa o mesmo contrato do objeto decorado. Assim, o cliente pode receber tanto o componente original quanto o decorator pela mesma abstração.

Neste arquivo, não existe uma interface como:

```csharp
public interface ITextBuilder
{
  ITextBuilder AppendLine(string value);
}
```

Também não existe um `StringBuilder` sendo consumido por uma abstração comum com `CodeBuilder`. O que existe é um wrapper manual em cima de uma classe concreta do .NET.

Portanto, a leitura mais precisa é:

> `CodeBuilder` é um wrapper com sabor de Decorator: ele envolve `StringBuilder`, delega trabalho para ele e altera a experiência de uso da API.

---

<a id="sec-012"></a>
### 60.3 Por que não herdar de StringBuilder?

[↑ Voltar ao início](#inicio)


Uma pergunta natural é:

> Se queremos um `StringBuilder` mais conveniente, por que não criar uma subclasse?

Porque `StringBuilder` é uma classe fechada para herança em C# moderno. Ela não foi projetada para ser estendida por subclasses.

Mesmo se fosse possível herdar, ainda existiria outro problema: muitos métodos de `StringBuilder` já retornam `StringBuilder`. A cadeia fluente continuaria presa ao tipo original, não necessariamente ao tipo customizado.

O wrapper resolve isso de outro jeito:

```text
Em vez de:
  CodeBuilder herda de StringBuilder

Fazemos:
  CodeBuilder possui um StringBuilder
```

Essa é uma mudança de herança para composição.

E essa troca conversa diretamente com o capítulo de padrões estruturais: em vez de tentar forçar a estrutura por herança, montamos uma nova estrutura em volta de um objeto existente.

---

<a id="sec-013"></a>
### 60.4 Leitura guiada do campo interno

[↑ Voltar ao início](#inicio)


O coração da classe é este campo:

```csharp
private StringBuilder builder = new StringBuilder();
```

Ele representa o objeto real que sabe montar texto.

`CodeBuilder` não sabe armazenar caracteres diretamente. Ele não possui buffer próprio, não controla capacidade internamente e não implementa algoritmo de concatenação. Toda essa parte continua sendo responsabilidade do `StringBuilder`.

Isso é importante porque evita confundir duas responsabilidades:

- `StringBuilder` cuida da montagem eficiente de texto;
- `CodeBuilder` cuida da forma mais conveniente de chamar essa montagem.

Em termos de design:

```text
StringBuilder:
  responsabilidade de infraestrutura textual

CodeBuilder:
  responsabilidade de API / ergonomia de uso
```

---

<a id="sec-014"></a>
### 60.5 O papel de ToString

[↑ Voltar ao início](#inicio)


O método:

```csharp
public override string ToString()
{
  return builder.ToString();
}
```

fecha o ciclo da aula.

Durante a construção, o cliente trabalha com `CodeBuilder`. No final, ele quer uma `string`.

Então `CodeBuilder.ToString()` simplesmente pergunta ao objeto interno:

```text
StringBuilder interno, qual é o texto final?
```

Isso aparece na demonstração:

```csharp
WriteLine(cb);
```

`WriteLine` consegue imprimir o objeto porque, quando recebe algo que não é uma string pronta, ele pode usar `ToString()` para obter uma representação textual.

Mentalmente:

```text
cb
  aponta para CodeBuilder

CodeBuilder.ToString()
  chama builder.ToString()

builder.ToString()
  devolve o texto acumulado
```

---

<a id="sec-015"></a>
### 60.6 O padrão que se repete nos métodos mutáveis

[↑ Voltar ao início](#inicio)


O trecho mais representativo do arquivo é este:

```csharp
public CodeBuilder Append(char[] value, int startIndex, int charCount)
{
  builder.Append(value, startIndex, charCount);
  return this;
}
```

Ele parece banal, mas contém a ideia inteira da aula.

Leia em duas partes.

Primeira parte:

```csharp
builder.Append(value, startIndex, charCount);
```

Aqui `CodeBuilder` delega o trabalho real para o `StringBuilder` interno.

Segunda parte:

```csharp
return this;
```

Aqui `CodeBuilder` devolve a própria instância, permitindo que o cliente chame outro método logo em seguida.

Sem `return this`, a chamada terminaria ali:

```csharp
cb.AppendLine("class Foo");
cb.AppendLine("{");
cb.AppendLine("}");
```

Com `return this`, a chamada pode continuar:

```csharp
cb.AppendLine("class Foo")
  .AppendLine("{")
  .AppendLine("}");
```

Esse é o contrato fluente:

```text
fazer uma alteração
devolver o mesmo objeto
permitir a próxima alteração
```

---

<a id="sec-016"></a>
### 60.7 Por que esse trecho se repete tanto no código

[↑ Voltar ao início](#inicio)


A repetição é grande, incômoda e totalmente esperada.

Ela acontece por causa de três fatores combinados.

Primeiro: `StringBuilder` tem muitos overloads.

Overload significa que existem vários métodos com o mesmo nome, mas assinaturas diferentes:

```csharp
Append(string value)
Append(char value)
Append(int value)
Append(double value)
Append(char[] value)
Append(char[] value, int startIndex, int charCount)
```

Para o C#, esses métodos não são "o mesmo método com opções". Eles são membros diferentes da API.

Segundo: composição não repassa métodos automaticamente.

Quando uma classe possui outra classe dentro dela:

```csharp
private StringBuilder builder = new StringBuilder();
```

isso não faz `CodeBuilder` ganhar automaticamente todos os métodos públicos de `StringBuilder`.

O compilador não diz:

```text
CodeBuilder tem um StringBuilder dentro,
então vou fingir que CodeBuilder também tem todos os métodos dele.
```

Não funciona assim.

Se o wrapper quer oferecer `Append(int value)`, ele precisa declarar:

```csharp
public CodeBuilder Append(int value)
{
  builder.Append(value);
  return this;
}
```

Se o wrapper quer oferecer `Append(char[] value, int startIndex, int charCount)`, ele precisa declarar:

```csharp
public CodeBuilder Append(char[] value, int startIndex, int charCount)
{
  builder.Append(value, startIndex, charCount);
  return this;
}
```

Terceiro: o wrapper quer mudar o tipo de retorno.

O `StringBuilder.Append(...)` original retorna `StringBuilder`. Mas esta aula quer que os métodos retornem `CodeBuilder`.

Esse detalhe obriga o wrapper a interceptar cada método mutável:

```text
StringBuilder.Append(...)
  retorna StringBuilder

CodeBuilder.Append(...)
  chama StringBuilder.Append(...)
  retorna CodeBuilder
```

É por isso que o arquivo fica cheio de métodos quase iguais.

O objetivo dessa repetição não é adicionar regra nova em cada método. O objetivo é criar uma casca pública parecida com `StringBuilder`, mas ajustada para fluent chaining com `CodeBuilder`.

Em uma frase:

> A repetição existe porque o wrapper está pagando o preço de parecer com `StringBuilder` enquanto altera o retorno dos métodos mutáveis.

---

<a id="sec-017"></a>
### 60.8 O pedágio da delegação

[↑ Voltar ao início](#inicio)


Esse exemplo ensina uma consequência prática de wrappers e decorators:

**quanto maior a interface do objeto envolvido, maior o trabalho para decorá-lo manualmente.**

Se o contrato fosse pequeno:

```csharp
public interface ITextAppender
{
  ITextAppender AppendLine(string value);
}
```

o wrapper teria pouco código.

Mas `StringBuilder` possui muitos métodos públicos. Se `CodeBuilder` quiser parecer familiar para quem já usa `StringBuilder`, ele precisa copiar boa parte dessa superfície:

```text
Append(...)
AppendLine(...)
AppendFormat(...)
Insert(...)
Remove(...)
Replace(...)
Capacity
Length
indexador
```

Esse é o "pedágio" da delegação manual.

Ele não significa que o design está errado. Significa que a escolha tem custo:

- o cliente ganha uma API conveniente;
- o wrapper ganha muito código repetitivo;
- cada overload exposto precisa ser mantido;
- se `StringBuilder` mudar ou ganhar novas APIs, `CodeBuilder` não acompanha automaticamente.

Essa é uma boa conexão com o **Interface Segregation Principle**:

> Decorators ficam mais simples quando o contrato que eles precisam preservar é pequeno e focado.

Quando o contrato é enorme, a classe decoradora vira um grande painel de repasses.

---

<a id="sec-018"></a>
### 60.9 Métodos que não retornam CodeBuilder

[↑ Voltar ao início](#inicio)


Nem todo método do arquivo retorna `CodeBuilder`.

Alguns métodos respondem perguntas ou executam operações cujo resultado útil não é continuar a cadeia.

Exemplo:

```csharp
public int EnsureCapacity(int capacity)
{
  return builder.EnsureCapacity(capacity);
}
```

Aqui o retorno importante é um número: a capacidade garantida.

Outro exemplo:

```csharp
public string ToString(int startIndex, int length)
{
  return builder.ToString(startIndex, length);
}
```

Aqui o retorno importante é uma string parcial.

Outro:

```csharp
public void CopyTo(int sourceIndex, char[] destination, int destinationIndex, int count)
{
  builder.CopyTo(sourceIndex, destination, destinationIndex, count);
}
```

Aqui a operação copia caracteres para um array externo. Ela não precisa devolver o wrapper.

A regra mental é:

```text
Metodo altera o texto e faz sentido continuar construindo?
  retorna CodeBuilder

Metodo responde uma pergunta ou extrai um valor?
  retorna o valor real

Metodo executa uma copia externa sem cadeia?
  pode continuar void
```

---

<a id="sec-019"></a>
### 60.10 As propriedades delegadas

[↑ Voltar ao início](#inicio)


No fim da classe aparecem propriedades como:

```csharp
public int Capacity
{
  get => builder.Capacity;
  set => builder.Capacity = value;
}

public int MaxCapacity => builder.MaxCapacity;

public int Length
{
  get => builder.Length;
  set => builder.Length = value;
}

public char this[int index]
{
  get => builder[index];
  set => builder[index] = value;
}
```

Essas propriedades seguem a mesma ideia de delegação.

`Capacity` não pertence de verdade ao `CodeBuilder`. Quem controla capacidade é o `StringBuilder` interno. O wrapper apenas expõe uma porta:

```text
CodeBuilder.Capacity
  repassa para builder.Capacity
```

O indexador também é repassado:

```csharp
cb[0] = 'C';
```

vira, por dentro:

```csharp
builder[0] = 'C';
```

Isso reforça que `CodeBuilder` não substitui o motor interno. Ele apenas controla como o cliente conversa com esse motor.

---

<a id="sec-020"></a>
### 60.11 O método Equals merece atenção

[↑ Voltar ao início](#inicio)


O arquivo possui este método:

```csharp
public bool Equals(CodeBuilder sb)
{
  return builder.Equals(sb);
}
```

A intenção parece ser comparar este `CodeBuilder` com outro `CodeBuilder`.

Mas há um detalhe: o método repassa `sb`, que é um `CodeBuilder`, diretamente para `builder.Equals(...)`, que pertence ao `StringBuilder` interno.

Isso não compara explicitamente o `StringBuilder` interno de um objeto com o `StringBuilder` interno do outro.

Uma leitura mais cuidadosa seria:

```text
este CodeBuilder
  tem builder interno A

outro CodeBuilder
  tem builder interno B

comparar A com B exigiria acessar B
```

Como `builder` é privado, o código atual não acessa diretamente o builder interno do outro objeto.

Para fins da aula, esse método não é o centro do exemplo. O centro é a delegação dos métodos de escrita e o `return this`. Mas ele é um bom lembrete: quando fazemos wrappers, nem todo repasse é automaticamente óbvio. Alguns métodos precisam de mais critério semântico.

---

<a id="sec-021"></a>
### 60.12 Demonstração final

[↑ Voltar ao início](#inicio)


A classe `Demo` usa o wrapper assim:

```csharp
var cb = new CodeBuilder();
cb.AppendLine("class Foo")
  .AppendLine("{")
  .AppendLine("}");
WriteLine(cb);
```

Fluxo completo:

```text
new CodeBuilder()
  cria um wrapper
  cria internamente um StringBuilder

AppendLine("class Foo")
  delega para builder.AppendLine("class Foo")
  retorna o mesmo CodeBuilder

AppendLine("{")
  delega para builder.AppendLine("{")
  retorna o mesmo CodeBuilder

AppendLine("}")
  delega para builder.AppendLine("}")
  retorna o mesmo CodeBuilder

WriteLine(cb)
  chama ToString()
  imprime o texto acumulado
```

Resultado conceitual:

```csharp
class Foo
{
}
```

Repare no ponto principal: o cliente não precisa conhecer o `StringBuilder` interno. Ele conversa apenas com `CodeBuilder`.

---

<a id="sec-022"></a>
### 60.13 O que esta aula ensina de verdade

[↑ Voltar ao início](#inicio)


Esta aula não está tentando provar que todo wrapper é automaticamente um Decorator clássico.

Ela ensina algo mais sutil:

- envolver um objeto existente pode melhorar a API sem reescrever sua lógica;
- composição permite reutilizar comportamento de uma classe fechada para herança;
- delegação manual pode gerar bastante repetição;
- `return this` transforma métodos mutáveis em uma API fluente;
- quanto maior a superfície do tipo envolvido, maior o custo de manter o wrapper;
- preservar "parecido com o original" e "melhorar a experiência de uso" são forças que entram em tensão.

O mapa mental final:

```text
StringBuilder
  sabe construir texto de forma eficiente

CodeBuilder
  envolve StringBuilder
  delega as operações reais
  muda o retorno dos métodos mutáveis
  permite encadeamento fluente

Cliente
  usa CodeBuilder
  não conversa diretamente com o StringBuilder interno
```

A frase para guardar:

> Custom String Builder mostra o custo e o benefício de envolver um objeto existente: ganhamos uma API mais expressiva, mas pagamos com muitos métodos de delegação repetidos.

---


<a id="sec-023"></a>
## 61. Adapter Decorator

[↑ Voltar ao início](#inicio)


<a id="sec-024"></a>
### 61.1 O que esta aula está tentando mostrar

[↑ Voltar ao início](#inicio)

O arquivo desta aula é:

- `StructuralPatterns/9.Decorator/Aula02_AdapterDecorator/AdapterDecorator.cs`

Esta aula continua a conversa da seção anterior.

Na seção 60, `CodeBuilder` envolvia um `StringBuilder` para oferecer uma API fluente. Aqui, `MyStringBuilder` também envolve um `StringBuilder`, mas adiciona uma diferença importante:

**uma `string` comum pode ser adaptada automaticamente para `MyStringBuilder`.**

Isso aparece na demonstração:

```csharp
MyStringBuilder s = "hello ";
s += "world";
WriteLine(s);
```

A linha mais interessante é a primeira:

```csharp
MyStringBuilder s = "hello ";
```

Normalmente, isso não deveria funcionar. `"hello "` é uma `string`, não um `MyStringBuilder`.

Mas a classe cria uma conversão implícita:

```csharp
public static implicit operator MyStringBuilder(string s)
```

Isso faz a linguagem aceitar uma `string` no lugar em que o código espera um `MyStringBuilder`.

Essa é a parte **Adapter** da aula.

---

<a id="sec-025"></a>
### 61.2 Por que o nome é Adapter Decorator

[↑ Voltar ao início](#inicio)


O nome da aula mistura dois padrões porque o exemplo tem sinais dos dois.

Tem sabor de **Decorator** porque existe um objeto envolvendo outro:

```csharp
public class MyStringBuilder
{
  StringBuilder sb = new StringBuilder();
}
```

`MyStringBuilder` não constrói texto diretamente. Ele delega ao `StringBuilder` interno.

Tem sabor de **Adapter** porque uma interface de uso é adaptada:

```csharp
MyStringBuilder s = "hello ";
```

O cliente entrega uma `string`, mas o sistema passa a trabalhar com `MyStringBuilder`.

Em termos mentais:

```text
Decorator:
  MyStringBuilder envolve StringBuilder
  MyStringBuilder delega operações para StringBuilder

Adapter:
  string entra no código
  conversão implícita transforma string em MyStringBuilder
```

O ponto principal da aula é perceber que padrões estruturais podem se encostar. Nem sempre o código real aparece em caixinhas puras.

A pergunta correta não é:

> Isso é 100% Adapter ou 100% Decorator?

A pergunta melhor é:

> Qual intenção estrutural está acontecendo em cada parte do código?

---

<a id="sec-026"></a>
### 61.3 O campo interno

[↑ Voltar ao início](#inicio)


O núcleo da classe é este:

```csharp
StringBuilder sb = new StringBuilder();
```

Esse campo guarda o objeto real que sabe montar texto.

`MyStringBuilder` não tem buffer próprio, não implementa algoritmo de concatenação e não controla caracteres manualmente. Ele usa o `StringBuilder` do .NET.

Isso divide as responsabilidades:

```text
StringBuilder:
  faz o trabalho real de acumular texto

MyStringBuilder:
  adapta a entrada
  delega operações
  expõe uma API parecida com StringBuilder
```

Esse é o mesmo raciocínio da seção 60: composição em vez de herança.

---

<a id="sec-027"></a>
### 61.4 A conversão implícita de string para MyStringBuilder

[↑ Voltar ao início](#inicio)


O primeiro recurso novo da aula é este:

```csharp
public static implicit operator MyStringBuilder(string s)
{
  var msb = new MyStringBuilder();
  msb.sb.Append(s);
  return msb;
}
```

Leia o método em partes.

Primeiro:

```csharp
public static implicit operator MyStringBuilder(string s)
```

Isso define uma conversão implícita. Em linguagem simples, a classe está dizendo:

```text
se você tiver uma string,
eu sei transformá-la em MyStringBuilder
sem o cliente precisar escrever cast manual.
```

Segundo:

```csharp
var msb = new MyStringBuilder();
```

Cria um wrapper vazio.

Terceiro:

```csharp
msb.sb.Append(s);
```

Coloca a string recebida dentro do `StringBuilder` interno.

Quarto:

```csharp
return msb;
```

Devolve o wrapper já preenchido.

Por isso esta linha funciona:

```csharp
MyStringBuilder s = "hello ";
```

O compilador interpreta assim:

```text
lado esquerdo espera:
  MyStringBuilder

lado direito oferece:
  string

existe conversão implicita string -> MyStringBuilder?
  sim

então:
  chama implicit operator MyStringBuilder("hello ")
```

Esse é o Adapter em ação: uma coisa que não tinha a forma esperada passa a ser aceita por meio de uma conversão.

---

<a id="sec-028"></a>
### 61.5 O operador +

[↑ Voltar ao início](#inicio)


O segundo recurso novo é este:

```csharp
public static MyStringBuilder operator +(MyStringBuilder msb, string s)
{
  msb.Append(s);
  return msb;
}
```

Esse método sobrecarrega o operador `+`.

Ele permite escrever:

```csharp
s = s + "world";
```

ou:

```csharp
s += "world";
```

O corpo do operador faz duas coisas.

Primeiro:

```csharp
msb.Append(s);
```

Adiciona a string ao conteúdo interno.

Segundo:

```csharp
return msb;
```

Devolve o próprio `MyStringBuilder`.

O fluxo fica assim:

```text
s += "world"

vira mentalmente:
  s = MyStringBuilder.operator +(s, "world")

operator +
  chama Append("world")
  devolve o mesmo MyStringBuilder
```

O detalhe importante é que este operador **muta o objeto existente**.

Ele não cria uma nova string final para depois reconstruir outro `MyStringBuilder`. Ele usa o objeto recebido e acrescenta texto no seu `StringBuilder` interno.

---

<a id="sec-029"></a>
### 61.6 Por que `s += "world"` poderia funcionar mesmo sem o operator +

[↑ Voltar ao início](#inicio)


No final do arquivo existe este comentário original:

```csharp
s += "world"; // will work even without op+ in MyStringBuilder
              // why? you figure it out!
```

A provocação é boa.

Mesmo sem o operador `+` definido em `MyStringBuilder`, o C# ainda poderia encontrar outro caminho.

O motivo é que `string` possui concatenação com objetos.

Quando o compilador vê algo como:

```csharp
s + "world"
```

e `s` não tem um `operator +` próprio, ele pode tratar isso como concatenação de string com objeto:

```text
object + string
```

Nesse caminho:

1. `s` é tratado como objeto;
2. a concatenação chama `s.ToString()`;
3. o resultado vira uma `string`;
4. `"world"` é concatenado;
5. o resultado final é uma nova `string`.

Depois entra a conversão implícita:

```text
string final
  -> implicit operator MyStringBuilder(string)
  -> novo MyStringBuilder
```

Fluxo sem `operator +` próprio:

```text
s += "world"

pode virar:
  s = (MyStringBuilder)(s.ToString() + "world")

passos:
  s.ToString()
  concatena com "world"
  cria string final
  converte string final para MyStringBuilder
```

Fluxo com `operator +` próprio:

```text
s += "world"

vira:
  s = MyStringBuilder.operator +(s, "world")

passos:
  chama Append("world")
  devolve o mesmo wrapper
```

A diferença é sutil, mas importante.

Sem `operator +`, o caminho passa por string e cria outro wrapper. Com `operator +`, o caminho expressa diretamente a intenção da classe: anexar texto ao builder existente.

---

<a id="sec-030"></a>
### 61.7 O papel de ToString

[↑ Voltar ao início](#inicio)


O método:

```csharp
public override string ToString()
{
  return sb.ToString();
}
```

continua essencial.

Ele transforma o estado interno em texto final.

No `WriteLine`:

```csharp
WriteLine(s);
```

o console precisa imprimir alguma representação de `s`. A representação vem de `ToString()`.

No caminho alternativo sem `operator +`, `ToString()` também seria usado para transformar `s` em string antes da concatenação.

Então este método participa de dois cenários:

- exibir o conteúdo final;
- permitir que o objeto participe de concatenação textual quando tratado como objeto.

---

<a id="sec-031"></a>
### 61.8 A grande diferença para CodeBuilder

[↑ Voltar ao início](#inicio)


Na seção 60, `CodeBuilder` fazia isto:

```csharp
public CodeBuilder Append(string value)
{
  builder.Append(value);
  return this;
}
```

Ou seja, depois de chamar `Append`, o cliente continuava recebendo um `CodeBuilder`.

Nesta aula, `MyStringBuilder` faz isto:

```csharp
public StringBuilder Append(string value)
{
  return sb.Append(value);
}
```

Aqui o retorno é `StringBuilder`, não `MyStringBuilder`.

Essa diferença muda a leitura da API.

Com `CodeBuilder`:

```text
cb.Append("a")
  retorna CodeBuilder

.Append("b")
  ainda chama CodeBuilder.Append(...)
```

Com `MyStringBuilder`:

```text
s.Append("a")
  retorna StringBuilder

.Append("b")
  agora chama StringBuilder.Append(...)
```

Em outras palavras:

> `CodeBuilder` mantém a cadeia dentro do wrapper. `MyStringBuilder` delega e entrega o objeto interno de volta.

Isso é um vazamento de abstração?

Em parte, sim.

Quando `MyStringBuilder.Append(...)` devolve `StringBuilder`, o cliente passa a enxergar o objeto interno. Isso pode ser aceitável em uma aula didática, mas é uma decisão importante em design real.

Se a intenção fosse manter o cliente sempre preso à casca externa, os métodos mutáveis deveriam retornar `MyStringBuilder`, como na aula anterior.

---

<a id="sec-032"></a>
### 61.9 Por que os métodos delegados se repetem tanto

[↑ Voltar ao início](#inicio)


O arquivo repete muitos métodos assim:

```csharp
public StringBuilder Append(char[] value, int startIndex, int charCount)
{
  return sb.Append(value, startIndex, charCount);
}
```

Essa repetição tem a mesma causa vista na seção 60.

`StringBuilder` possui muitos overloads:

```text
Append(string)
Append(char)
Append(int)
Append(double)
Append(char[])
Insert(...)
AppendFormat(...)
Replace(...)
```

Como `MyStringBuilder` usa composição:

```csharp
StringBuilder sb = new StringBuilder();
```

os métodos de `StringBuilder` não aparecem automaticamente em `MyStringBuilder`.

O wrapper precisa declarar cada método que deseja oferecer ao cliente.

Aqui o padrão é ainda mais simples que no `CodeBuilder`:

```text
recebe chamada no wrapper
repassa para sb
devolve o retorno de sb
```

Exemplo:

```csharp
public StringBuilder Append(int value)
{
  return sb.Append(value);
}
```

O método não adiciona regra de negócio. Ele só preserva a superfície da API original.

O custo é visualmente chato, mas conceitualmente honesto:

> quanto mais o wrapper tenta parecer com o objeto original, mais métodos de repasse ele precisa carregar.

---

<a id="sec-033"></a>
### 61.10 Propriedades delegadas

[↑ Voltar ao início](#inicio)


No final da classe aparecem:

```csharp
public int Capacity
{
  get => sb.Capacity;
  set => sb.Capacity = value;
}

public int MaxCapacity => sb.MaxCapacity;

public int Length
{
  get => sb.Length;
  set => sb.Length = value;
}

public char this[int index]
{
  get => sb[index];
  set => sb[index] = value;
}
```

Essas propriedades também são adaptações por delegação.

Quando o cliente lê:

```csharp
s.Length
```

o wrapper responde:

```csharp
sb.Length
```

Quando o cliente altera:

```csharp
s.Capacity = 100;
```

o wrapper repassa:

```csharp
sb.Capacity = 100;
```

O indexador também funciona como uma janela para o conteúdo interno:

```csharp
s[0] = 'H';
```

vira:

```csharp
sb[0] = 'H';
```

De novo: `MyStringBuilder` não é o motor. Ele é a camada externa.

---

<a id="sec-034"></a>
### 61.11 O método Equals

[↑ Voltar ao início](#inicio)


O arquivo possui:

```csharp
public bool Equals(StringBuilder sb)
{
  return this.sb.Equals(sb);
}
```

Aqui a comparação é entre o `StringBuilder` interno deste wrapper e outro `StringBuilder` recebido como parâmetro.

Isso é diferente do problema citado na seção 60.

Lá, o método recebia um `CodeBuilder`, mas repassava esse wrapper diretamente para o `StringBuilder` interno. Aqui, o parâmetro já é `StringBuilder`.

Leitura:

```text
este MyStringBuilder
  possui sb interno

metodo recebe:
  outro StringBuilder

comparacao:
  this.sb.Equals(outro StringBuilder)
```

Ainda assim, há uma decisão de API a observar: o método compara com `StringBuilder`, não com `MyStringBuilder`.

Se a intenção fosse comparar dois wrappers, a assinatura provavelmente seria outra.

---

<a id="sec-035"></a>
### 61.12 Demonstração final

[↑ Voltar ao início](#inicio)


A demonstração inteira é:

```csharp
MyStringBuilder s = "hello ";
s += "world";
WriteLine(s);
```

Fluxo com os recursos definidos no arquivo:

```text
MyStringBuilder s = "hello "
  chama conversão implícita
  cria MyStringBuilder
  coloca "hello " no StringBuilder interno

s += "world"
  chama operator +
  executa Append("world")
  devolve o mesmo MyStringBuilder

WriteLine(s)
  chama ToString()
  imprime hello world
```

Resultado:

```text
hello world
```

A aula é curta no uso, mas rica no mecanismo.

Ela junta:

- wrapper;
- delegação;
- conversão implícita;
- sobrecarga de operador;
- comportamento alternativo de concatenação de string;
- custo de expor uma API parecida com `StringBuilder`.

---

<a id="sec-036"></a>
### 61.13 O que esta aula ensina de verdade

[↑ Voltar ao início](#inicio)


Esta aula mostra uma região de fronteira entre Adapter e Decorator.

Como Decorator, `MyStringBuilder` envolve um objeto existente e delega para ele.

Como Adapter, `MyStringBuilder` aceita uma entrada que originalmente não teria o tipo esperado:

```text
string
  vira MyStringBuilder
```

O aprendizado principal não é decorar nomes de padrões. É enxergar as forças no código:

- composição permite envolver uma classe concreta;
- delegação reutiliza comportamento sem reimplementar;
- conversão implícita adapta um tipo de entrada para outro;
- `operator +` cria uma sintaxe mais natural para o cliente;
- retornar `StringBuilder` nos métodos delegados expõe o objeto interno;
- wrappers grandes repetem muitos membros quando tentam parecer com a API original.

Mapa mental final:

```text
string "hello "
  conversão implicita
  vira MyStringBuilder

MyStringBuilder
  possui StringBuilder
  delega Append, Insert, Replace...
  adapta + e += para anexar texto

StringBuilder
  continua fazendo o trabalho real
```

A frase para guardar:

> Adapter Decorator mostra que um wrapper pode ao mesmo tempo envolver um objeto existente e adaptar a forma como o cliente entra nessa API.

---


<a id="sec-037"></a>
## 62. Multiple Inheritance with Interfaces and Default Interface Members

[↑ Voltar ao início](#inicio)


<a id="sec-038"></a>
### 62.1 O que esta aula está tentando mostrar

[↑ Voltar ao início](#inicio)

O arquivo desta aula é:

- `StructuralPatterns/9.Decorator/Aula03_MultipleInhertanceWithInterface/InhertanceWithInterfaces.cs`

Esta aula começa com uma limitação importante de C#:

**uma classe não pode herdar de duas classes concretas ao mesmo tempo.**

Ou seja, isto não é permitido:

```csharp
public class Dragon : Bird, Lizard
{
}
```

Mas C# permite que uma classe implemente várias interfaces:

```csharp
public class Dragon : IBird, ILizard
{
}
```

Essa diferença é a aula inteira.

```text
Herança de classe:
  herda implementação e estado

Interface:
  herda contrato

Composição:
  reaproveita implementações concretas sem herdar delas
```

No código atual, `Dragon` faz quatro coisas ao mesmo tempo:

- implementa `IBird`;
- implementa `ILizard`;
- delega `Fly()` e `Crawl()` para objetos internos;
- coordena `Weight`, que aparece nos dois contratos.

O ponto central é:

> `Dragon` combina contratos por interface, reaproveita comportamento por composição e resolve explicitamente o estado comum chamado `Weight`.

Isso é bem mais limpo do que tentar forçar herança múltipla de classes.

---

<a id="sec-039"></a>
### 62.2 O problema da herança múltipla de classes

[↑ Voltar ao início](#inicio)


Antes de olhar o código, vale entender por que C# evita herança múltipla de classes.

Imagine que fosse possível escrever:

```csharp
public class Dragon : Bird, Lizard
{
}
```

Agora imagine que `Bird` e `Lizard` tivessem um método com o mesmo nome:

```csharp
public class Bird
{
  public void Move() { }
}

public class Lizard
{
  public void Move() { }
}
```

Se `Dragon` herdasse dos dois, qual `Move()` ele receberia?

```text
Dragon.Move()
  chama Bird.Move()?
  chama Lizard.Move()?
  chama os dois?
  obriga Dragon a resolver o conflito?
```

Esse tipo de ambiguidade é uma das razões clássicas para evitar herança múltipla de implementação.

Outro problema é estado interno.

Se `Bird` tivesse campos internos e `Lizard` também, `Dragon` herdaria duas estruturas de estado diferentes. Isso cria uma hierarquia mais difícil de entender, testar e manter.

Por isso C# escolhe uma regra mais simples:

```text
uma classe pode herdar de apenas uma classe base
mas pode implementar várias interfaces
```

Essa distinção é essencial para esta aula.

---

<a id="sec-040"></a>
### 62.3 O contrato IBird

[↑ Voltar ao início](#inicio)


O primeiro contrato do arquivo é:

```csharp
public interface IBird
{
  void Fly();
  int Weight { get; set; }
}
```

`IBird` é uma interface.

Ela não diz **como** voar e também não diz **como** o peso será guardado.

Ela apenas diz:

```text
qualquer tipo que afirma ser IBird
precisa oferecer Fly()
precisa oferecer Weight
```

Esse é o ponto de usar contratos.

O consumidor não precisa depender de uma classe concreta chamada `Bird`. Ele pode depender da capacidade:

```text
algo que sabe voar
```

No código, `Dragon` implementa `IBird`. Isso significa que `Dragon` promete ter `Fly()` e `Weight`.

---

<a id="sec-041"></a>
### 62.4 A implementação concreta Bird

[↑ Voltar ao início](#inicio)


Depois do contrato vem uma implementação concreta:

```csharp
public class Bird : IBird
{
  public int Weight { get; set; }

  public void Fly()
  {
    Console.WriteLine("Soaring in the sky");
  }
}
```

`Bird` cumpre o contrato `IBird`.

Leia assim:

```text
Bird é uma implementação concreta
da capacidade definida por IBird
```

Agora existe uma separação melhor:

```text
IBird
  contrato

Bird : IBird
  implementação concreta
  Fly()
  Weight
```

Isso é mais limpo do que fazer `Dragon` depender diretamente de `Bird`.

`Bird` pode ser a implementação padrão, mas não precisa ser a única implementação possível.

---

<a id="sec-042"></a>
### 62.5 O contrato ILizard

[↑ Voltar ao início](#inicio)


O segundo contrato é:

```csharp
public interface ILizard
{
  void Crawl();
  int Weight { get; set; }
}
```

`ILizard` representa a capacidade de rastejar e também exige a propriedade `Weight`.

De novo: a interface não implementa a ação. Ela só define a obrigação.

Em linguagem simples:

```text
qualquer tipo que afirma ser ILizard
precisa oferecer Crawl()
precisa oferecer Weight
```

No exemplo, `Dragon` também implementa `ILizard`.

Então ele promete:

```text
eu sei voar
eu sei rastejar
```

sem precisar herdar de duas classes concretas.

---

<a id="sec-043"></a>
### 62.6 A implementação concreta Lizard

[↑ Voltar ao início](#inicio)


A implementação concreta é:

```csharp
public class Lizard : ILizard
{
  public int Weight { get; set; }

  public void Crawl()
  {
    Console.WriteLine("Crawling through the land");
  }
}
```

`Lizard` cumpre o contrato `ILizard`.

O mapa fica simétrico:

```text
IBird
  Bird
    Fly()
    Weight

ILizard
  Lizard
    Crawl()
    Weight
```

Agora o exemplo tem duas capacidades modeladas por contratos, além de uma propriedade comum que aparece nos dois lados.

---

<a id="sec-044"></a>
### 62.7 Dragon implementa os dois contratos

[↑ Voltar ao início](#inicio)


O ponto mais importante do novo código é este:

```csharp
public class Dragon : IBird, ILizard
```

Isso não é herança múltipla de classes.

`Dragon` não está herdando implementação de `Bird`.

`Dragon` não está herdando implementação de `Lizard`.

Ele está implementando dois contratos.

Leia assim:

```text
Dragon promete cumprir IBird
  então precisa ter Fly()
  então precisa ter Weight

Dragon promete cumprir ILizard
  então precisa ter Crawl()
  então também precisa ter Weight
```

Aqui aparece um detalhe importante do código atualizado: `IBird` e `ILizard` declaram uma propriedade com o mesmo nome, `Weight`.

Isso não significa que `Dragon` ganha duas propriedades automaticamente.

Como ele implementa os dois contratos, ele precisa fornecer uma implementação que satisfaça ambos:

```csharp
public int Weight
{
  get
  {
    return bird.Weight;
  }
  set
  {
    bird.Weight = value;
    lizard.Weight = value;
  }
}
```

No arquivo, essa propriedade mantém os colaboradores internos sincronizados: quando alguém faz `d.Weight = 123`, o valor é repassado para o objeto que cumpre `IBird` e para o objeto que cumpre `ILizard`.

Essa é a vantagem das interfaces em C#:

```text
uma classe só pode herdar de uma classe
mas pode implementar várias interfaces
```

Então `Dragon` consegue combinar capacidades sem entrar nos problemas de herança múltipla de implementação.

---

<a id="sec-045"></a>
### 62.8 Os campos internos agora dependem de interfaces

[↑ Voltar ao início](#inicio)


No novo código, os campos internos ficaram assim:

```csharp
private readonly IBird bird;
private readonly ILizard lizard;
```

Esse é o ajuste que deixa o exemplo mais limpo.

Antes, a ideia seria:

```csharp
private Bird bird;
private Lizard lizard;
```

Isso prende `Dragon` às classes concretas.

Agora, com interfaces:

```text
Dragon não depende especificamente de Bird.
Dragon depende de IBird.

Dragon não depende especificamente de Lizard.
Dragon depende de ILizard.
```

Essa diferença parece pequena, mas muda o design.

Com campos concretos:

```text
Dragon só conversa com Bird e Lizard
```

Com campos por interface:

```text
Dragon conversa com qualquer coisa que cumpra IBird
Dragon conversa com qualquer coisa que cumpra ILizard
```

Esse é o benefício real dos contratos.

---

<a id="sec-046"></a>
### 62.9 O construtor de conveniência

[↑ Voltar ao início](#inicio)


O arquivo agora possui este construtor:

```csharp
public Dragon()
  : this(new Bird(), new Lizard())
{
}
```

Ele existe para a demonstração ficar simples:

```csharp
var d = new Dragon();
```

Esse construtor cria as implementações padrão:

```text
Bird
  para o comportamento Fly()

Lizard
  para o comportamento Crawl()
```

Depois ele repassa tudo para o construtor principal:

```text
Dragon()
  chama Dragon(new Bird(), new Lizard())
```

Esse construtor é conveniente, mas não é o mais flexível.

Ele ainda escolhe concretamente `Bird` e `Lizard`.

Isso é aceitável para a demo, porque evita poluir o `Main`. Mas o design mais interessante está no outro construtor.

---

<a id="sec-047"></a>
### 62.10 O construtor principal

[↑ Voltar ao início](#inicio)


O construtor principal é:

```csharp
public Dragon(IBird bird, ILizard lizard)
{
  this.bird = bird ?? throw new ArgumentNullException(paramName: nameof(bird));
  this.lizard = lizard ?? throw new ArgumentNullException(paramName: nameof(lizard));
}
```

Este é o coração do design.

Ele diz:

```text
para criar Dragon,
me entregue algo que cumpra IBird
e algo que cumpra ILizard
```

Repare que o construtor não pede:

```csharp
Bird bird, Lizard lizard
```

Ele pede:

```csharp
IBird bird, ILizard lizard
```

Isso permite variação.

Por exemplo, uma implementação de teste poderia ser passada no lugar de `Bird`. Uma implementação diferente de rastejar poderia ser passada no lugar de `Lizard`.

O `Dragon` não precisa saber.

Ele só precisa chamar os contratos:

```text
bird.Fly()
lizard.Crawl()
bird.Weight
lizard.Weight
```

Ou seja: `Dragon` não sabe qual é a classe concreta por trás de cada campo. Ele só sabe que o campo `bird` cumpre `IBird` e que o campo `lizard` cumpre `ILizard`.

---

<a id="sec-048"></a>
### 62.11 Guard clauses no construtor

[↑ Voltar ao início](#inicio)


Os trechos:

```csharp
bird ?? throw new ArgumentNullException(paramName: nameof(bird))
lizard ?? throw new ArgumentNullException(paramName: nameof(lizard))
```

impedem que `Dragon` nasça sem uma das capacidades.

Sem essa guarda, seria possível criar um `Dragon` quebrado.

Exemplo mental:

```text
bird = null
lizard = objeto valido
```

Depois, ao chamar:

```csharp
d.Fly();
```

o código quebraria tarde, dentro do método.

E não seria só `Fly()` ou `Crawl()` que quebraria. A propriedade `Weight` também depende desses colaboradores:

```csharp
return bird.Weight;
bird.Weight = value;
lizard.Weight = value;
```

Com a guarda no construtor, o erro aparece no momento da criação:

```text
Dragon precisa receber IBird
Dragon precisa receber ILizard
```

Isso deixa a classe mais honesta e mais fácil de diagnosticar.

---

<a id="sec-049"></a>
### 62.12 Delegação: Dragon.Fly

[↑ Voltar ao início](#inicio)


Como `Dragon` implementa `IBird`, ele precisa ter:

```csharp
public void Fly()
```

No código:

```csharp
public void Fly()
{
  bird.Fly();
}
```

Essa implementação é uma delegação.

Fluxo:

```text
cliente
  chama Dragon.Fly()

Dragon
  repassa para bird.Fly()

objeto IBird interno
  executa a lógica real de voo
```

Se o construtor padrão foi usado, esse objeto interno é um `Bird`.

Mas o método não depende disso. O campo é `IBird`.

Então o raciocínio correto é:

```text
Dragon delega Fly() para o colaborador que cumpre IBird
```

---

<a id="sec-050"></a>
### 62.13 Delegação: Dragon.Crawl

[↑ Voltar ao início](#inicio)


Como `Dragon` implementa `ILizard`, ele precisa ter:

```csharp
public void Crawl()
```

No código:

```csharp
public void Crawl()
{
  lizard.Crawl();
}
```

Fluxo:

```text
cliente
  chama Dragon.Crawl()

Dragon
  repassa para lizard.Crawl()

objeto ILizard interno
  executa a lógica real de rastejar
```

De novo, se o construtor padrão foi usado, esse objeto interno é um `Lizard`.

Mas `Dragon` depende do contrato `ILizard`, não da classe concreta.

---

<a id="sec-051"></a>
### 62.14 A demonstração final

[↑ Voltar ao início](#inicio)


A demo ficou assim:

```csharp
var d = new Dragon();
d.Fly();
d.Crawl();
d.Weight = 123;
```

Fluxo completo:

```text
new Dragon()
  chama o construtor de conveniência
  cria Bird
  cria Lizard
  repassa para Dragon(IBird, ILizard)

d.Fly()
  Dragon implementa IBird
  delega para Bird.Fly()
  imprime "Soaring in the sky"

d.Crawl()
  Dragon implementa ILizard
  delega para Lizard.Crawl()
  imprime "Crawling through the land"

d.Weight = 123
  Dragon implementa a propriedade exigida por IBird e ILizard
  repassa o mesmo peso para o colaborador IBird
  repassa o mesmo peso para o colaborador ILizard
```

Saída esperada:

```text
Soaring in the sky
Crawling through the land
```

`Weight` não aparece na saída porque `Fly()` e `Crawl()` só imprimem mensagens fixas. Mesmo assim, a linha é importante: ela prova que `Dragon` cumpriu completamente os dois contratos.

O cliente enxerga apenas o `Dragon`.

Por dentro, `Dragon` combina capacidades menores.

---

<a id="sec-052"></a>
### 62.15 Por que usar isso em vez de só classes concretas

[↑ Voltar ao início](#inicio)


Agora o código está alinhado com a dúvida levantada anteriormente.

Se `Dragon` guardasse isto:

```csharp
private Bird bird;
private Lizard lizard;
```

ele estaria preso a duas classes concretas.

Com isto:

```csharp
private readonly IBird bird;
private readonly ILizard lizard;
```

ele depende de contratos.

Essa versão é melhor quando:

- você quer testar `Dragon` sem depender de `Bird` e `Lizard` reais;
- você quer trocar a implementação de voo;
- você quer trocar a implementação de rastejar;
- você quer deixar claro que `Dragon` depende de capacidades, não de espécies concretas;
- você quer aplicar melhor Dependency Inversion.

Então a regra honesta é:

```text
Classes concretas:
  boas para exemplos pequenos e fechados

Interfaces:
  melhores quando existe variação, teste, desacoplamento ou composição real
```

Neste novo código, a demo ainda usa `Bird` e `Lizard` por conveniência, mas o design central já depende de `IBird` e `ILizard`.

---

<a id="sec-053"></a>
### 62.16 Relação com Decorator

[↑ Voltar ao início](#inicio)


Esta aula ainda não é um Decorator clássico do GoF.

Mas ela prepara uma mecânica importante:

```text
um objeto recebe uma chamada
e repassa essa chamada para um colaborador interno
```

No Decorator clássico:

```text
Decorator.Operation()
  faz algo extra
  chama inner.Operation()
```

Neste exemplo:

```text
Dragon.Fly()
  chama bird.Fly()

Dragon.Crawl()
  chama lizard.Crawl()

Dragon.Weight = valor
  repassa valor para bird.Weight
  repassa valor para lizard.Weight
```

A semelhança é a delegação. No caso de `Weight`, a delegação vira uma pequena coordenação: `Dragon` recebe um valor único e mantém os dois objetos internos alinhados.

A diferença é a intenção.

No Decorator, normalmente o wrapper preserva o mesmo contrato do objeto envolvido e adiciona comportamento ao redor.

Aqui, `Dragon` combina duas capacidades diferentes em um único objeto.

---

<a id="sec-054"></a>
### 62.17 O que esta aula ensina de verdade

[↑ Voltar ao início](#inicio)


Esta aula ensina a separar três ideias que muita gente mistura:

```text
Herança de classe:
  herda implementação e estado
  C# permite apenas uma classe base

Interface:
  define contrato
  C# permite implementar várias interfaces

Composição:
  guarda colaboradores internos
  permite reaproveitar comportamento por delegação
```

O novo desenho fica assim:

```text
IBird
  Fly()
  Weight

Bird : IBird
  implementa Fly()
  implementa Weight

ILizard
  Crawl()
  Weight

Lizard : ILizard
  implementa Crawl()
  implementa Weight

Dragon : IBird, ILizard
  possui IBird
  possui ILizard
  delega Fly()
  delega Crawl()
  implementa Weight para satisfazer os dois contratos
```

A frase para guardar:

> Multiple Inhertance with Interfaces mostra que C# troca herança múltipla de classes por uma combinação mais limpa: vários contratos, composição e delegação.

---

<a id="sec-055"></a>
### 62.18 Onde entram Default Interface Members

[↑ Voltar ao início](#inicio)


Os **Default Interface Members** permitem que uma interface em C# ofereça uma implementação padrão para alguns membros. Isso não transforma a interface em uma classe base tradicional, mas muda um pouco a conversa: além de declarar o contrato, a interface pode fornecer um comportamento comum simples.

Exemplo mental:

```csharp
public interface IBird
{
  void Fly();

  void Land()
  {
    Console.WriteLine("Landing carefully");
  }
}
```

Qualquer classe que implemente `IBird` continua obrigada a fornecer o que não tem implementação, como `Fly()`. Mas, se ela não quiser personalizar `Land()`, pode usar o comportamento padrão da interface.

O cuidado didático é não confundir isso com herança múltipla de classes:

```text
Default interface member:
  compartilha comportamento pequeno ligado ao contrato
  não carrega estado de instância como uma classe base comum
  não deve virar um depósito de lógica pesada

Classe base:
  pode guardar estado
  pode coordenar invariantes
  cria uma relação de herança mais forte
```

No contexto desta aula, membros padrão ajudam quando várias capacidades têm uma operação comum pequena, mas o objeto composto ainda precisa coordenar estado e delegação explicitamente. Se `Dragon` implementa `IBird` e `ILizard`, ele ainda precisa resolver conflitos de contrato, especialmente quando dois lados oferecem membros com o mesmo nome.

A frase para guardar:

> Default Interface Members reduzem repetição em contratos, mas não substituem composição quando o objeto precisa combinar colaboradores reais.

---

> **Nota de preparação das próximas aulas:** os tópicos abaixo foram estruturados em nível mínimo porque ainda não existem arquivos C# correspondentes no repositório para estas aulas. A ideia é deixar o mapa didático pronto: intenção, problema, participantes e cuidado principal. Quando os exemplos locais forem adicionados, cada seção pode ganhar a mesma leitura guiada linha a linha usada nos capítulos anteriores.

<a id="sec-056"></a>
## 63. Dynamic Decorator Composition

[↑ Voltar ao início](#inicio)


Dynamic Decorator Composition é a montagem de camadas em tempo de execução. Em vez de decidir no tipo da variável que um objeto sempre será decorado de uma forma fixa, o sistema pode montar uma pilha conforme configuração, permissões, ambiente, feature flags ou escolha do usuário.

Exemplo mental:

```text
IShape
  Circle
  ColoredShape(Circle)
  TransparentShape(ColoredShape(Circle))
```

O objeto final ainda é consumido pelo contrato `IShape`, mas a cadeia interna pode variar:

```text
sem decoração:
  Circle

com cor:
  ColoredShape -> Circle

com cor e transparência:
  TransparentShape -> ColoredShape -> Circle
```

O ganho é flexibilidade. O custo é que a ordem das camadas passa a importar. Log antes de cache não é a mesma coisa que cache antes de log. Validação antes de retry não é a mesma coisa que retry antes de validação.

A frase para guardar:

> Decorator dinâmico é composição em tempo de execução: o contrato continua igual, mas a pilha de responsabilidades pode mudar.

---

<a id="sec-057"></a>
## 64. Detecting Decorator Cycles

[↑ Voltar ao início](#inicio)


Decorator Cycle acontece quando uma composição deixa de ser uma cadeia saudável e passa a se referenciar de modo circular ou redundante. O caso mais perigoso é um decorator apontar, direta ou indiretamente, para ele mesmo.

Fluxo inválido:

```text
LoggingDecorator
  inner -> CachingDecorator
             inner -> LoggingDecorator
```

Se a chamada atravessa essa estrutura, ela pode nunca chegar ao objeto real. Em vez de enriquecer uma operação, a cadeia vira recursão infinita.

Uma detecção simples costuma partir de identidade de objeto:

```text
para cada camada visitada:
  se já apareceu antes, existe ciclo
  se não apareceu, registra e continua no inner
```

Também existe um ciclo conceitual: aplicar duas vezes a mesma responsabilidade quando isso não faz sentido. Dois decorators de log podem ser válidos em camadas diferentes, mas dois decorators de autorização idênticos podem indicar erro no composition root.

A frase para guardar:

> Detectar ciclos em Decorator é proteger a cadeia antes que delegação vire recursão sem saída.

---

<a id="sec-058"></a>
## 65. Static Decorator Composition

[↑ Voltar ao início](#inicio)


Static Decorator Composition monta a cadeia no tipo, geralmente com generics. Em vez de construir uma sequência arbitrária em tempo de execução, o código expressa a composição no próprio tipo concreto usado pelo cliente.

Exemplo mental:

```csharp
var shape = new ColoredShape<TransparentShape<Circle>>();
```

Esse estilo dá mais segurança de compilação e pode evitar alguns erros de ordem ou ausência de camada. O compilador passa a participar da validação da estrutura.

O custo é perder flexibilidade dinâmica. Se as camadas dependem de uma configuração lida em arquivo, de uma escolha do usuário ou de módulos carregados em runtime, uma composição puramente estática fica rígida.

Comparação rápida:

```text
Dynamic Decorator:
  monta com objetos
  muda em runtime
  exige cuidado com ordem e ciclos

Static Decorator:
  monta no tipo
  ganha segurança de compilação
  fica menos flexível para cenários configuráveis
```

A frase para guardar:

> Decorator estático troca liberdade de montagem por uma cadeia mais explícita para o compilador.

---

<a id="sec-059"></a>
## 66. Decorator in Dependency Injection

[↑ Voltar ao início](#inicio)


Decorator em Dependency Injection aparece quando o container monta as camadas no composition root. O cliente pede apenas o contrato, por exemplo `ICommandHandler`, `IRepository` ou `INotificationSender`, e recebe uma versão já envolvida por log, cache, métricas, autorização ou retry.

O cliente continua simples:

```text
classe de aplicação
  depende de IService

container entrega:
  LoggingService
    inner -> CachingService
               inner -> RealService
```

Esse uso é poderoso porque centraliza políticas transversais. Em vez de cada serviço chamar manualmente `logger`, `cache` ou `metrics`, essas responsabilidades podem ficar em decorators especializados.

O ponto de atenção é o composition root. Se a ordem das camadas fica escondida demais, o sistema pode parecer mágico. A configuração deve responder claramente:

- qual implementação real está no centro;
- quais decorators envolvem essa implementação;
- em que ordem eles são aplicados;
- quais contratos são pequenos o suficiente para serem decorados sem repetição excessiva.

A frase para guardar:

> Decorator em DI deixa o cliente depender de um contrato limpo enquanto o container monta a pilha de responsabilidades.

---

<a id="sec-060"></a>
## Coding Exercise 8: Decorator Coding Exercise

[↑ Voltar ao início](#inicio)


O exercício deve consolidar a diferença entre modificar uma classe e envolver uma instância. A solução esperada precisa preservar o contrato consumido pelo cliente e adicionar comportamento por composição.

Checklist mental:

- existe um componente base com comportamento principal;
- o decorator guarda uma referência para outro objeto compatível;
- o método público do decorator chama o objeto interno;
- a responsabilidade adicional aparece antes, depois ou ao redor da chamada;
- o cliente não precisa conhecer a classe concreta interna.

O erro comum é transformar o exercício em herança simples. Se a solução cria uma subclasse para cada combinação de recursos, ela perde justamente a vantagem do Decorator.

A frase para guardar:

> No exercício de Decorator, procure a camada ao redor do objeto, não uma árvore de subclasses.

---


<a id="sec-glossario"></a>
## Glossário
[↑ Voltar ao início](#inicio)

- **Componente:** contrato do objeto que recebe responsabilidades adicionais.
- **Decorador concreto:** objeto que envolve um componente e acrescenta comportamento.
- **Encapsulamento:** composição em que um objeto guarda outro e encaminha chamadas.
- **Composição em tempo de execução:** combinação de decoradores sem subclasses para cada combinação.
