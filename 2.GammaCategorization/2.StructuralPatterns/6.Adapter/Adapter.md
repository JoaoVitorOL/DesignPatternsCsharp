<a id="inicio"></a>


**Família GoF:** estrutural. **Escopo:** classe ou objeto; os exemplos locais usam composição de objetos.

# Design Pattern: Adapter

## Sumário
[↑ Voltar ao início](#inicio)

- [1. O que o Adapter representa?](#sec-001)
  - [O Problema que ele resolve:](#sec-002)
- [2. Em que situações se usa?](#sec-003)
- [3. Instruções passo a passo de como implementar (em Java)](#sec-004)
  - [Exemplo básico de implementação:](#sec-005)
- [4. Explicação detalhada do Exemplo Prático (Painel de Clima)](#sec-006)
  - [A Interface Alvo: PrevisaoService](#sec-007)
  - [A Classe Cliente: PainelClima](#sec-008)
  - [Os Serviços Incompatíveis (Adaptees) e seus Adaptadores](#sec-009)
  - [A Execução: Main](#sec-010)
- [5. Implementação real do repositório: Square como IRectangle](#sec-011)
- [Outros exemplos de Adapter na pasta](#sec-012)
- [6. Leitura passo a passo do Adapter do quadrado](#sec-013)
- [Diferencie Adapter de padrões parecidos](#sec-014)
- [Variações e aprofundamentos](#sec-015)
- [46. Vector/Raster](#sec-016)
  - [46.1 O que é Vetorização vs. Rasterização?](#sec-017)
  - [46.2 O problema do exemplo](#sec-018)
  - [46.3 Como o Adapter resolve isso](#sec-019)
  - [46.4 Participantes do padrão no exemplo](#sec-020)
  - [46.5 Anatomia e fluxo de execução](#sec-021)
  - [46.6 Análise do código linha por linha](#sec-022)
  - [46.7 Como o cliente usa isso no código](#sec-023)
  - [46.8 O possível custo de conversões repetidas](#sec-024)
- [47. Adapter Caching](#sec-025)
  - [47.1 O que o Adapter Caching representa](#sec-026)
  - [47.2 O problema de desempenho que ele resolve](#sec-027)
  - [47.3 Igualdade, códigos hash e chaves do cache](#sec-028)
  - [47.4 Mudança de Arquitetura: De Collection<T> para IEnumerable<T>](#sec-029)
  - [47.5 Anatomia das Variáveis no Exemplo Prático](#sec-030)
  - [47.6 Comparação Direta: Adapter Tradicional vs. Adapter com Cache](#sec-031)
  - [47.7 Análise detalhada do código de exemplo](#sec-032)
  - [47.7.1 Fluxo de Execução Passo a Passo](#sec-033)
  - [47.7.2 Equivalência de Valor na Prática (Equals e GetHashCode)](#sec-034)
  - [47.8 Quando usar e quando evitar](#sec-035)
- [48. Generic Value Adapter](#sec-036)
  - [48.1 O Problema da Dimensionalidade em Estruturas de Dados](#sec-037)
  - [48.2 A Solução: Interfaces como Portadores de Valores (Value Adaptors)](#sec-038)
  - [48.3 O Adaptador Genérico e a Assinatura de Metaprogramação](#sec-039)
  - [48.4 Especialização e Herança Segura](#sec-040)
  - [48.5 Sintaxe de Uso no Mundo Real](#sec-041)
  - [48.5.2 O que o arquivo local realmente executa](#sec-042)
  - [48.6 Vantagens e Trade-offs](#sec-043)
  - [48.7 Resumo Mental Curto](#sec-044)
- [49. Adapter in Dependency Injection](#sec-045)
  - [49.1 O que é](#sec-046)
  - [49.1.1 Entendendo o Autofac](#sec-047)
  - [49.2 O problema que ele resolve](#sec-048)
  - [49.2.1 Leitura concreta do conflito](#sec-049)
  - [49.3 A solução: como o container resolve isso no código](#sec-050)
  - [49.4 O exemplo prático: AdapterDependency.cs](#sec-051)
  - [49.5 O uso de Metadados (Meta<T>)](#sec-052)
  - [49.6 Cuidados com registros concorrentes](#sec-053)
  - [49.7 Quando utilizar](#sec-054)
- [Coding Exercise 5: Adapter Coding Exercise](#sec-055)
  - [O que o exercício quer de verdade](#sec-056)
  - [Como ler o uso esperado](#sec-057)
  - [Participantes do padrão no exercício](#sec-058)
  - [O que faz este exercício ser um Adapter](#sec-059)
  - [Leitura guiada da solução](#sec-060)
  - [O que o aluno precisa enxergar para conseguir reproduzir sozinho](#sec-061)
  - [O que costuma confundir aqui](#sec-062)
  - [Como validar mentalmente se a solução está correta](#sec-063)


<a id="sec-001"></a>
## 1. O que o Adapter representa?

[↑ Voltar ao início](#inicio)
O padrão **Adapter (Adaptador)** é um padrão de projeto estrutural que permite a colaboração entre objetos com interfaces incompatíveis, criando um objeto intermediário (o adaptador) responsável por traduzir uma interface em outra.

<a id="sec-002"></a>
### O Problema que ele resolve:

[↑ Voltar ao início](#inicio)
Em sistemas que precisam integrar bibliotecas de terceiros ou serviços externos, a incompatibilidade de interfaces e formatos de dados gera problemas práticos:
- **Alto acoplamento:** O código cliente passa a conhecer detalhes da biblioteca externa.
- **Conversões espalhadas:** Lógicas de tradução de dados (como XML para JSON ou conversões de unidades) ficam espalhadas pelo sistema.
- **Vulnerabilidade a mudanças:** Qualquer mudança no fornecedor impacta vários pontos do código.
- **Impossibilidade de modificação:** Muitas vezes a biblioteca externa não pode ser modificada (por ser de terceiros ou já estar em produção em outros sistemas).

---

<a id="sec-003"></a>
## 2. Em que situações se usa?

[↑ Voltar ao início](#inicio)
Use o Adapter quando:
1. **Incompatibilidade de interfaces:** Você deseja usar uma classe existente, mas sua interface não é compatível com o restante da aplicação.
2. **Reutilização de código legado/terceiros:** Precisa reutilizar um componente ou biblioteca externa sem modificar seu código-fonte original.
3. **Isolamento de tradução:** Deseja concentrar a conversão de dados, formatos, parâmetros ou convenções em um único lugar, mantendo o cliente limpo.
4. **Aplicação do OCP:** Quer adicionar novos fornecedores ou serviços no futuro sem alterar o código cliente existente.

---

<a id="sec-004"></a>
## 3. Instruções passo a passo de como implementar (em Java)

[↑ Voltar ao início](#inicio)

1. **Identificar a interface esperada:** Reconhecer a interface que o cliente já utiliza e espera.
2. **Identificar o serviço incompatível:** Localizar a classe ou biblioteca externa que precisa ser reutilizada.
3. **Criar a classe adaptadora:** Implementar a interface esperada pelo cliente dentro do adaptador.
4. **Armazenar referência:** Fazer o adaptador armazenar uma referência para o serviço real incompatível.
5. **Traduzir chamadas e dados:** Realizar a tradução de métodos, parâmetros, unidades ou formatos dentro do adaptador e delegar para o serviço real.
6. **Depender da abstração:** Garantir que o cliente dependa estritamente da abstração esperada.

<a id="sec-005"></a>
### Exemplo básico de implementação:

[↑ Voltar ao início](#inicio)
```java
public class AccuWeatherAdapter implements PrevisaoService {
    private final AccuWeatherApi accuWeatherApi;

    public AccuWeatherAdapter(AccuWeatherApi accuWeatherApi) {
        this.accuWeatherApi = accuWeatherApi;
    }

    @Override
    public int obterTemperatura(String cidade) {
        int tempFahrenheit = accuWeatherApi.getTemperature(cidade);
        // Conversão de Fahrenheit para Celsius
        return (int) ((tempFahrenheit - 32) * 5 / 9);
    }
}

```

---

<a id="sec-006"></a>
## 4. Explicação detalhada do Exemplo Prático (Painel de Clima)

[↑ Voltar ao início](#inicio)

O código fornecido demonstra o padrão **Adapter** aplicado em um painel meteorológico, integrando diferentes APIs de previsão do tempo (AccuWeather e OpenWeather) de forma transparente para a classe cliente.

<a id="sec-007"></a>
### A Interface Alvo: `PrevisaoService`

[↑ Voltar ao início](#inicio)

Define o contrato padrão esperado pelo cliente.

```java
package clima;

public interface PrevisaoService {
    int obterTemperatura(String cidade);
}

```

---

<a id="sec-008"></a>
### A Classe Cliente: `PainelClima`

[↑ Voltar ao início](#inicio)

Depende estritamente da abstração `PrevisaoService`, desconhecendo os detalhes das APIs de clima subjacentes.

```java
package clima;

public class PainelClima {
    private final PrevisaoService previsaoService;

    // Recebe qualquer implementação de PrevisaoService injetada via construtor
    public PainelClima(PrevisaoService previsaoService) {
        this.previsaoService = previsaoService;
    }

    public void exibir(String cidade) {
        int temperatura = previsaoService.obterTemperatura(cidade);
        System.out.println("[PainelClima] A temperatura atual em " + cidade + " é de " + temperatura + "°C.");
    }
}

```

---

<a id="sec-009"></a>
### Os Serviços Incompatíveis (Adaptees) e seus Adaptadores

[↑ Voltar ao início](#inicio)

* **OpenWeather:** Retorna temperatura em `double` e usa o método `temperatura(cidade)`. O `OpenWeatherAdapter` traduz a chamada para o padrão `PrevisaoService`.
* **AccuWeather:** Retorna temperatura em Fahrenheit (`int`) via `getTemperature(cidade)`. O `AccuWeatherAdapter` traduz a chamada e converte a unidade para Celsius.

```java
package clima;

public class OpenWeatherApi {
    public double temperatura(String cidade) {
        // Retorna a temperatura em Celsius como double
        if (cidade.equalsIgnoreCase("São Paulo")) {
            return 28.5;
        }
        return 22.0;
    }
}

public class OpenWeatherAdapter implements PrevisaoService {
    private final OpenWeatherApi openWeatherApi;

    public OpenWeatherAdapter(OpenWeatherApi openWeatherApi) {
        this.openWeatherApi = openWeatherApi;
    }

    @Override
    public int obterTemperatura(String cidade) {
        double tempCelsius = openWeatherApi.temperatura(cidade);
        return (int) tempCelsius; // Conversão de double para int
    }
}

```

```java
package clima;

public class AccuWeatherApi {
    public int getTemperature(String cidade) {
        // Retorna a temperatura em Fahrenheit como int
        if (cidade.equalsIgnoreCase("São Paulo")) {
            return 82; // 82°F equivale a aprox. 27.7°C
        }
        return 72; // 72°F equivale a aprox. 22.2°C
    }
}

public class AccuWeatherAdapter implements PrevisaoService {
    private final AccuWeatherApi accuWeatherApi;

    public AccuWeatherAdapter(AccuWeatherApi accuWeatherApi) {
        this.accuWeatherApi = accuWeatherApi;
    }

    @Override
    public int obterTemperatura(String cidade) {
        int tempFahrenheit = accuWeatherApi.getTemperature(cidade);
        // Conversão de Fahrenheit para Celsius
        return (int) ((tempFahrenheit - 32) * 5 / 9);
    }
}

```

---

<a id="sec-010"></a>
### A Execução: `Main`

[↑ Voltar ao início](#inicio)

Demonstra a troca transparente de fornecedores através da injeção dos adaptadores.

```java
package clima;

public class Main {
    public static void main(String[] args) {
        String cidade = "São Paulo";

        System.out.println("=== 1. PAINEL CLIMA USANDO OPENWEATHER API (Via Adaptador) ===");
        PrevisaoService servicoOpenWeather = new OpenWeatherAdapter(new OpenWeatherApi());
        PainelClima painel1 = new PainelClima(servicoOpenWeather);
        painel1.exibir(cidade);

        System.out.println("\n=== 2. PAINEL CLIMA USANDO ACCUWEATHER API (Via Adaptador) ===");
        PrevisaoService servicoAccuWeather = new AccuWeatherAdapter(new AccuWeatherApi());

        // Demonstra que o PainelClima permanece exatamente o mesmo, trocando apenas o adaptador injetado
        PainelClima painel2 = new PainelClima(servicoAccuWeather);
        painel2.exibir(cidade);
    }
}

/*
 * ========================================================================================
 * EXPLICAÇÃO: COMO ADICIONAR UM TERCEIRO FORNECEDOR SEM ALTERAR O PAINELCLIMA
 * ========================================================================================
 * Para integrar um novo fornecedor de terceiros (por exemplo, WeatherStackApi com um método
 * próprio 'buscarClima(String local)'), seguindo o Princípio Aberto/Fechado (OCP), nós não
 * precisamos tocar em nenhuma linha de código da classe 'PainelClima' nem nas classes anteriores.
 *
 * O processo exigiria apenas duas etapas isoladas:
 * 1. Manter a classe da API de terceiros intacta (conforme restrição do fornecedor).
 * 2. Criar uma nova classe adaptadora (ex: WeatherStackAdapter implements PrevisaoService)
 *    que traduz a chamada de 'obterTemperatura(String cidade)' para o método específico da nova API,
 *    realizando as conversões de dados necessárias.
 *
 * Como o 'PainelClima' interage estritamente com a interface 'PrevisaoService', ele aceitará
 * o novo adaptador injetado de forma transparente, sem qualquer impacto ou reescrita no fluxo existente.
 */

```

```

---
Tem alguma dúvida sobre este padrão ou gostaria de ver outro Design Pattern estruturado desta mesma forma?

```

<a id="sec-011"></a>
## 5. Implementação real do repositório: `Square` como `IRectangle`

[↑ Voltar ao início](#inicio)

O exercício em [`AdapterExcercise.cs`](Aula05_AdapterExcercise/AdapterExcercise.cs) mostra o Adapter de objeto em C#. `Square` é o *adaptee*: tem um campo `Side`, mas não oferece o contrato que o cliente consome. `IRectangle` é o *target*, com `Width` e `Height`. `SquareToRectangleAdapter` implementa esse contrato, recebe o quadrado por composição no construtor e mapeia ambas as propriedades para `Side`.

```csharp
public class SquareToRectangleAdapter : IRectangle
{
    private readonly Square _square;
    public SquareToRectangleAdapter(Square square) =>
        _square = square ?? throw new ArgumentNullException(nameof(square));
    public int Width => _square.Side;
    public int Height => _square.Side;
}
```

O cliente chama `adapter.Area()` através do contrato `IRectangle`; ele não precisa conhecer `Square`. Como o adaptador guarda a referência, uma alteração posterior em `square.Side` aparece nas duas propriedades na próxima leitura. A responsabilidade do adapter é traduzir a forma de acesso; ele não copia nem altera a classe existente.

Outros estudos no diretório demonstram conversão entre representações, cache, adaptador genérico e registro por injeção de dependência. O exercício `AdapterDependencyInjection` aplica a mesma ideia na fronteira com dependências. Em integrações reais, prefira validar entradas e manter explícitos os arredondamentos, unidades e erros da conversão.


<a id="sec-012"></a>
## Outros exemplos de Adapter na pasta

[↑ Voltar ao início](#inicio)

Além do exercício Square/IRectangle, o projeto contém diferentes motivos para adaptar contratos:

- [VectorRasterDemo.cs](Aula01_Vector%26RasterDemo/VectorRasterDemo.cs) adapta uma linha vetorial para que um consumidor possa enumerar pontos rasterizados.
- [AdapterCaching.cs](Aula02_AdapterCaching/AdapterCaching.cs) e [OutroExemplo.cs](Aula02_AdapterCaching/OutroExemplo.cs) acrescentam cache à adaptação de linhas para pontos e comparam a forma de expor a coleção.
- [GenericValueAdapter.cs](Aula03_GenericValueAdapter/GenericValueAdapter.cs) e [OutroExemplo.cs](Aula03_GenericValueAdapter/OutroExemplo.cs) demonstram um adaptador genérico para compatibilizar tipos de valores/vetores.
- [AdapterDependency.cs](Aula04_AdapterDependencyInjection/AdapterDependency.cs) usa o container Autofac para adaptar comandos a botões com metadados.
- [AdapterExcercise.cs](Aula05_AdapterExcercise/AdapterExcercise.cs) contém o exercício corrigido Square → IRectangle.

Em todos, localize o contrato esperado pelo cliente, o serviço/objeto adaptado e a tradução que a camada intermediária faz. Alguns exemplos usam herança de coleção ou reflection para fins didáticos; em aplicações novas, composição e contratos explícitos costumam tornar o limite mais claro.


<a id="sec-013"></a>
## 6. Leitura passo a passo do Adapter do quadrado

[↑ Voltar ao início](#inicio)

No exercício, o cliente já possui uma operação de área que trabalha com IRectangle. A classe Square representa um quadrado, mas oferece Side em vez de Width e Height. O problema não é calcular a área do quadrado; é fazê-lo participar do fluxo existente que espera o contrato retangular.

SquareToRectangleAdapter recebe o Square no construtor e implementa IRectangle. Width e Height leem o mesmo Side, pois num quadrado os lados são iguais. A extensão Area() passa a funcionar sem alterar Square nem o algoritmo cliente. O adaptador guarda a referência, não copia o quadrado; por isso uma mudança posterior em Side aparece na leitura seguinte do adapter.

Observe o limite de responsabilidade: o adapter traduz o formato de interface. Ele não deve incorporar regras de domínio desconexas. Nos exemplos Vector/Raster e Autofac, a tradução é mais elaborada, mas a pergunta é a mesma: qual contrato o cliente espera, qual API incompatível existe e qual correspondência precisa ser feita?

Use Adapter para integrar código que não pode ou não deve ser modificado. Teste especialmente unidades, convenções de erro e comportamento de dados ausentes, porque esses detalhes também fazem parte da tradução.


<a id="sec-014"></a>
## Diferencie Adapter de padrões parecidos

[↑ Voltar ao início](#inicio)

Adapter responde a uma incompatibilidade existente: o cliente fala um contrato, e o componente disponível fala outro. No exercício, o cliente aceita IRectangle; Square só oferece Side. O adapter converte a forma de acesso, sem mudar a intenção do cliente nem do quadrado.

Bridge começa antes: separa duas dimensões que o design quer variar independentemente. Decorator parte de um contrato compatível e acrescenta responsabilidade. Facade simplifica o uso de várias classes. Essas intenções podem produzir classes com composição, então a estrutura visual sozinha não identifica o padrão.

Ao criar um adapter real, documente as regras de tradução: unidades, timezone, códigos de erro, valores nulos, arredondamento e limites. A conversão não é um detalhe neutro; se perder precisão ou mudar semântica, o cliente receberá um contrato aparentemente compatível com comportamento incorreto.

<a id="sec-015"></a>
## Variações e aprofundamentos

[↑ Voltar ao início](#inicio)

As seções abaixo reúnem variações, exemplos e decisões que complementam a explicação principal deste padrão. Elas preservam a linguagem e o contexto do material didático original.

<a id="sec-016"></a>
## 46. Vector/Raster

[↑ Voltar ao início](#inicio)



Esta parte corresponde ao arquivo `StructuralPatterns/6.Adapter/Aula01_Vector&RasterDemo/VectorRasterDemo.cs`.

O exemplo **Vector/Raster** apresenta a necessidade clássica do padrão `Adapter`: fazer com que dados estruturados em um formato geométrico (vetores) possam ser consumidos por uma API que só entende renderização pixel a pixel (rasterização).

<a id="sec-017"></a>
### 46.1 O que é Vetorização vs. Rasterização?

[↑ Voltar ao início](#inicio)

Antes de analisar o código, é fundamental compreender a diferença fundamental entre essas duas formas de representar imagens:

Vetorização (*vector graphics*) representa elementos por formas e parâmetros geométricos, em vez de armazenar cada pixel final. Isso permite recalcular a imagem em escalas diferentes sem a pixelização inerente à ampliação de um bitmap, embora precisão numérica, espessura, efeitos e rasterização final ainda imponham limites.

Rasterização (Raster): É a conversão de uma imagem geométrica em uma grade de pontos microscópicos chamados pixels (uma matriz de pontos ou bitmap). Uma tela física (como o monitor do seu computador ou a tela do celular) é um dispositivo estritamente rasterizado: ela não sabe o que é um "círculo" ou uma "linha", ela apenas sabe acender pixels individuais de cores específicas em coordenadas (coluna, linha).

Portanto, rasterizar é o ato de pegar uma fórmula geométrica matemática (vetor) e "desenhá-la" mapeando quais pixels específicos da tela precisam ser pintados para representar aquela forma visualmente.

<a id="sec-018"></a>
### 46.2 O problema do exemplo

[↑ Voltar ao início](#inicio)

Imagine que temos uma biblioteca gráfica legada ou de terceiros responsável por desenhar na tela. Ela trabalha exclusivamente de forma **rasterizada**, ou seja, ela possui um método que desenha pixels individuais em coordenadas específicas:

```csharp
public void DrawPoint(Point point)
{
    // Desenha um único pixel na tela
}

```

Por outro lado, o nosso sistema trabalha com uma representação muito mais rica e eficiente: **vetores**. Nós definimos formas geométricas complexas por meio de linhas, retângulos e caminhos matemáticos:

```csharp
public class VectorObject : List<Line> {}

public class VectorRectangle : VectorObject
{
    public VectorRectangle(int x, int y, int width, int height)
    {
        Add(new Line(new Point(x, y), new Point(x + width, y)));
        // ... adiciona os 4 lados do retângulo
    }
}

```

O problema de design surge quando tentamos desenhar os nossos objetos vetoriais usando a API de rasterização.

**Como desenhar uma linha ou um retângulo vetorial se a única ferramenta que temos é um método que pinta pixels individuais?**

<a id="sec-019"></a>
### 46.3 Como o Adapter resolve isso

[↑ Voltar ao início](#inicio)

O `Adapter` entra como tradutor intermediário. Ele recebe uma `Line` e a expõe como sequência de `Point` para o cliente raster. **O algoritmo local cobre somente linhas perfeitamente horizontais ou verticais**; linhas diagonais exigiriam um algoritmo adicional, como Bresenham.

```csharp
public class LineToPointAdapter : IEnumerable<Point>
{
    private readonly List<Point> points = new List<Point>();

    public LineToPointAdapter(Line line)
    {
        // Algoritmo para converter a linha vetorial em uma série de pontos (pixels)
        // preenche a lista 'points' com cada pixel necessário para representar a linha
    }

    public IEnumerator<Point> GetEnumerator() => points.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

```

Dessa forma, o código cliente que precisa renderizar os objetos pode manter o seu fluxo sem precisar conhecer a matemática de conversão, e a API de rasterização não precisa ser modificada para entender o que é uma linha.

<a id="sec-020"></a>
### 46.4 Participantes do padrão no exemplo

[↑ Voltar ao início](#inicio)

Mapeando a estrutura do exemplo para os papéis clássicos do GoF:

- **Target (Alvo):** A sequência/coleção de `Point` que o cliente consegue iterar e entregar a `DrawPoint`.
- **Adaptee (Adaptado):** A `Line` vetorial recebida pelo adaptador; `VectorObject` apenas agrupa várias linhas no fluxo externo.
- **Adapter (Adaptador):** A classe que faz a ponte de conversão. No exemplo, `LineToPointAdapter`. Ele traduz uma `Line` em uma coleção de `Point`.
- **Client (Cliente):** O código que deseja desenhar as formas vetoriais na tela de pixels.

<a id="sec-021"></a>
### 46.5 Anatomia e fluxo de execução

[↑ Voltar ao início](#inicio)

Para entender o ciclo da conversão, o fluxo acontece em etapas:

1. O cliente cria um objeto de domínio moderno: `new VectorRectangle(1, 1, 10, 10)`.
2. Para cada linha desse retângulo, o cliente instancia o adaptador: `new LineToPointAdapter(line)`.
3. No momento em que o adaptador é instanciado, o seu construtor executa o cálculo de rasterização (calculando todos os pontos intermediários entre o início e o fim da linha) e os armazena na memória interna.
4. O cliente itera sobre esses pontos gerados pelo adaptador e chama a função rasterizadora `DrawPoint(point)`.

---

<a id="sec-022"></a>
### 46.6 Análise do código linha por linha

[↑ Voltar ao início](#inicio)

Vamos dissecar o código fornecido para entender sua estrutura interna.

#### A assinatura da classe

```csharp
public class LineToPointAdapter : Collection<Point>

```

- **O que está acontecendo:** A classe se chama `LineToPointAdapter` (Adaptador de Linha para Ponto). Ela herda diretamente de `Collection<Point>`.
- **Por que isso é feito:** Ao herdar de `Collection<Point>`, o próprio Adapter **se torna** uma lista/coleção de pontos. Qualquer método no sistema que espere receber uma lista de pontos agora aceitará uma instância deste Adapter.

#### O contador de diagnóstico

```csharp
private static int count = 0;

```

- **O que está acontecendo:** Um campo estático inteiro que serve como um contador global.
- **Por que isso é feito:** Toda vez que um novo adaptador for criado, esse contador será incrementado. Isso nos ajudará a ver no console quantas vezes a conversão foi executada, evidenciando o problema de performance.

#### O construtor e o log inicial

```csharp
public LineToPointAdapter(Line line)
{
  WriteLine($"{++count}: Generating points for line [{line.Start.X},{line.Start.Y}]-[{line.End.X},{line.End.Y}] (no caching)");

```

- **O que está acontecendo:** O construtor recebe a `Line` que precisa ser adaptada. Imediatamente, ele incrementa o contador (`++count`) e imprime no console a mensagem informando que está gerando os pontos para aquela coordenada específica, sem cache (`no caching`).

#### Identificação dos limites matemáticos

```csharp
  int left = Math.Min(line.Start.X, line.End.X);
  int right = Math.Max(line.Start.X, line.End.X);
  int top = Math.Min(line.Start.Y, line.End.Y);
  int bottom = Math.Max(line.Start.Y, line.End.Y);

  int dx = right - left;
  int dy = line.End.Y - line.Start.Y;

```

- **O que está acontecendo:** Como a linha pode ter sido desenhada de cima para baixo, de baixo para cima, da esquerda para a direita ou vice-versa, o algoritmo precisa descobrir os extremos absolutos nos eixos X e Y.
- `left` e `right` definem o início e o fim no eixo horizontal.
- `top` e `bottom` definem o início e o fim no eixo vertical.
- `dx` (Delta X) mede a largura da variação horizontal. Se for `0`, a linha é estritamente vertical.
- `dy` (Delta Y) mede a variação vertical. Se for `0`, a linha é estritamente horizontal.

#### O algoritmo de rasterização

O trecho a seguir é onde a mágica do padrão acontece. O Adapter usa as propriedades da linha para preencher a si mesmo com pontos.

##### Caso 1: linha perfeitamente vertical

```csharp
  if (dx == 0)
  {
    for (int y = top; y <= bottom; ++y)
    {
      Add(new Point(left, y));
    }
  }

```

- **O que está acontecendo:** Se a linha não varia no eixo X (`dx == 0`), significa que ela é vertical. O código faz um laço de repetição (`for`) que começa no topo (`top`) e vai até a base (`bottom`).
- **O Ponto Chave:** A cada iteração, ele executa o método `Add(new Point(left, y))`. Esse método `Add` pertence à classe base `Collection<Point>`. Ou seja, o adaptador está inserindo novos pontos dentro de si mesmo.

##### Caso 2: linha perfeitamente horizontal

```csharp
  else if (dy == 0)
  {
    for (int x = left; x <= right; ++x)
    {
      Add(new Point(x, top));
    }
  }
}

```

- **O que está acontecendo:** Se a variação no eixo Y for zero (`dy == 0`), a linha é horizontal. O laço percorre da esquerda (`left`) para a direita (`right`), mantendo a mesma altura (`top`), e adiciona os pontos correspondentes à sua própria coleção.

---

<a id="sec-023"></a>
### 46.7 Como o cliente usa isso no código

[↑ Voltar ao início](#inicio)

Sem o Adapter, o cliente teria que calcular esses loops manualmente toda vez que fosse desenhar. Com o Adapter, o uso fica limpo e transparente:

```csharp
Line minhaLinha = new Line(new Point(1, 1), new Point(1, 10));

// O Adapter faz a conversão no construtor
var adaptador = new LineToPointAdapter(minhaLinha);

// O sistema gráfico consome o adaptador como se fosse uma simples lista de pontos
DesenharPontosNaTela(adaptador);

```

---

<a id="sec-024"></a>
### 46.8 O possível custo de conversões repetidas

[↑ Voltar ao início](#inicio)

Embora o padrão resolva a incompatibilidade, recriar o adaptador recalcula os pontos e aloca novas coleções. Isso **pode** se tornar um problema de desempenho quando as mesmas linhas são adaptadas repetidamente; a relevância precisa ser confirmada com perfil e medições.

**Por que este código é ineficiente?**
Em sistemas gráficos, a tela é redesenhada dezenas de vezes por segundo (frames). Se você tiver um objeto que desenha um retângulo feito de 4 linhas, e a tela for atualizada, o sistema chamará o construtor do `LineToPointAdapter` repetidamente para as **mesmas linhas**.

Toda vez que o construtor roda:

1. Ele aloca memória para novos objetos `Point`.
2. Ele executa os laços `for` matemáticos novamente.
3. O console exibirá uma enxurrada de logs através do `++count`.

Se a linha não mudou, um cache pode evitar o recálculo. Essa otimização troca CPU e alocações transitórias por memória retida, políticas de igualdade, invalidação e concorrência; ela não deve ser aplicada automaticamente.

Para deixar esse problema de performance bem visual, imagine que estamos rodando um programa que desenha um simples **quadrado** (composto por 4 linhas) na tela.

Em uma aplicação gráfica típica, a tela é atualizada constantemente (por exemplo, a cada ciclo de renderização ou quando você move a janela). Sem um mecanismo de cache, veja o que acontece no terminal após apenas **3 frames** (renderizações) de atualização de tela:

```text
=================== FRAME 1 ===================
1: Generating points for line [0,0]-[0,10] (no caching)
2: Generating points for line [0,10]-[10,10] (no caching)
3: Generating points for line [10,10]-[10,0] (no caching)
4: Generating points for line [10,0]-[0,0] (no caching)
[Sistema] Frame 1 desenhado com sucesso.

=================== FRAME 2 ===================
5: Generating points for line [0,0]-[0,10] (no caching)
6: Generating points for line [0,10]-[10,10] (no caching)
7: Generating points for line [10,10]-[10,0] (no caching)
8: Generating points for line [10,0]-[0,0] (no caching)
[Sistema] Frame 2 desenhado com sucesso.

=================== FRAME 3 ===================
9: Generating points for line [0,0]-[0,10] (no caching)
10: Generating points for line [0,10]-[10,10] (no caching)
11: Generating points for line [10,10]-[10,0] (no caching)
12: Generating points for line [10,0]-[0,0] (no caching)
[Sistema] Frame 3 desenhado com sucesso.

===============================================
ESTATÍSTICAS DE EXECUÇÃO (Sem Cache):
- Total de Linhas Desenhadas: 4
- Vezes que a tela foi atualizada: 3
- Objetos de Adapter criados: 12
- Cálculos matemáticos repetidos inutilmente: 8 vezes!
===============================================

```

#### O diagnóstico do terminal

- **Desperdício de CPU:** Note que as linhas do Frame 2 e do Frame 3 têm **exatamente as mesmas coordenadas** do Frame 1. O sistema perdeu tempo recalculando e recriando cada `Point` que já havia sido calculado milissegundos atrás.
- **Pressão no Garbage Collector (GC):** o aumento da taxa de alocação pode tornar as coletas mais frequentes e afetar latência. A duração das pausas depende do modo de GC, geração, sobrevivência dos objetos e carga; não é possível concluir que haverá travamentos apenas contando objetos no exemplo.

---

<a id="sec-025"></a>
## 47. Adapter Caching

[↑ Voltar ao início](#inicio)



<a id="sec-026"></a>
### 47.1 O que o Adapter Caching representa

[↑ Voltar ao início](#inicio)

O **Adapter Caching** combina o padrão estrutural **Adapter** com um cache dos resultados da conversão. Enquanto o Adapter compatibiliza interfaces, o cache evita repetir uma conversão quando a mesma entrada lógica reaparece.

O nome desta aula descreve uma composição de técnicas. **Caching/memoização não é uma variante canônica adicional do Adapter no catálogo GoF.**

Ele surge para responder à seguinte pergunta de design:

> **Como adaptar dados repetidos sem refazer cálculos e alocações desnecessárias?**

<a id="sec-027"></a>
### 47.2 O problema de desempenho que ele resolve

[↑ Voltar ao início](#inicio)

Em cenários de renderização ou processamento de dados, o cliente pode solicitar várias vezes a conversão do mesmo valor de origem, como redesenhar uma linha estática a cada quadro.

No Adapter convencional (sem cache):

- o algoritmo de conversão é executado novamente;
- objetos temporários equivalentes podem aumentar a pressão sobre o coletor de lixo.

O cache armazena conversões anteriores em uma estrutura indexada, normalmente um dicionário, e reutiliza a representação já calculada quando encontra uma chave equivalente.

<a id="sec-028"></a>
### 47.3 Igualdade, códigos hash e chaves do cache

[↑ Voltar ao início](#inicio)

Para usar `Line` como chave, `Equals` deve representar a igualdade lógica e `GetHashCode` deve ser coerente com ela: objetos considerados iguais precisam produzir o mesmo código hash. O inverso não é garantido, pois objetos diferentes podem colidir.

O arquivo da aula usa `Dictionary<int, List<Point>>` e armazena apenas `line.GetHashCode()` como chave. Essa escolha é didática, mas não é segura em produção: uma colisão pode fazer linhas diferentes compartilharem o mesmo resultado. Prefira `Dictionary<Line, List<Point>>` com igualdade por valor, um tipo-chave imutável ou outra chave que preserve todos os dados relevantes.

Em código moderno, `HashCode.Combine` expressa a combinação com mais clareza:

```csharp
public override int GetHashCode()
{
    return HashCode.Combine(Start, End);
}

```

No algoritmo manual existente no arquivo, `unchecked` permite o comportamento de *wrap-around* caso a multiplicação exceda o intervalo de `int`. Isso evita uma `OverflowException` em contexto verificado, mas não impede colisões nem torna o hash único.

<a id="sec-029"></a>
### 47.4 Mudança de Arquitetura: De `Collection<T>` para `IEnumerable<T>`

[↑ Voltar ao início](#inicio)

Uma alteração do exemplo diz respeito à herança versus interface:

- **Adapter Básico:** Frequentemente herda de uma coleção concreta (ex: `Collection<Point>`), forçando cada nova instância do adaptador a manter fisicamente uma lista redundante de pontos na memória.
- **Adapter Otimizado com Cache:** Passa a implementar apenas `IEnumerable<Point>`. Em vez de duplicar dados, o método `GetEnumerator()` do adaptador simplesmente redireciona quem o está lendo para a coleção já armazenada de forma centralizada no cache estático global.

```csharp
public IEnumerator<Point> GetEnumerator()
{
  // Redireciona a iteração diretamente para a lista armazenada no cache estático central
  return cache[hash].GetEnumerator();
}

```

<a id="sec-030"></a>
### 47.5 Anatomia das Variáveis no Exemplo Prático

[↑ Voltar ao início](#inicio)

Mapeando o código do arquivo `Aula02_AdapterWithCaching`:

- **`cache` (campo estático):** O dicionário `Dictionary<int, List<Point>>` que, no exemplo, mapeia o código hash da linha à lista de pixels calculados. Como códigos hash podem colidir, essa chave deve ser substituída em código de produção.
- **`hash` (campo privado):** Armazena o código hash gerado no construtor para a linha atual, servindo como o localizador rápido dentro do cache.
- **`count` (campo estático):** Um contador de diagnóstico que mostra quantas vezes o caminho de geração foi executado. Ele não prova sozinho correção, ausência de colisões ou segurança concorrente.

<a id="sec-031"></a>
### 47.6 Comparação Direta: Adapter Tradicional vs. Adapter com Cache

[↑ Voltar ao início](#inicio)

| Característica | Adapter Tradicional (Aula 01) | Adapter com Caching (Aula 02) |
| --- | --- | --- |
| **Geração de Dados** | Recalcula em toda nova instanciação. | Consulta o cache; só calcula se for inédito. |
| **Estrutura Base** | Herda de `Collection<Point>`. | Implementa `IEnumerable<Point>`. |
| **Alocação e retenção** | Cria novas listas e pontos a cada adaptação. | Reutiliza listas, mas mantém resultados vivos enquanto o cache estático existir. |
| **Impacto no GC** | Pode elevar a taxa de alocação. | Reduz alocações repetidas, mas aumenta memória retida; o efeito deve ser medido. |

---

<a id="sec-032"></a>
### 47.7 Análise detalhada do código de exemplo

[↑ Voltar ao início](#inicio)

Para compreender a fundo o impacto prático dessa otimização, analisamos os pilares estruturais e o fluxo de execução implementados no código:

```csharp
// Exemplo real simplificado para fins didáticos
public class LineToPointAdapter : IEnumerable<Point>
{
    private static int count = 0;
    private static Dictionary<int, List<Point>> cache = new Dictionary<int, List<Point>>();
    private int hash;

    public LineToPointAdapter(Line line)
    {
        hash = line.GetHashCode(); // 1. Gera e armazena a identidade da linha

        if (cache.ContainsKey(hash)) return; // 2. Se já existe, aborta e aproveita o cache

        // 3. Caso contrário, executa a matemática custosa
        WriteLine($"{++count}: Generating points...");
        List<Point> points = new List<Point>();

        // [... cálculo de interpolação de pontos ...]

        cache.Add(hash, points); // 4. Armazena o resultado no cache estático
    }

    public IEnumerator<Point> GetEnumerator()
    {
        // 5. Encaminha o leitor para o cache centralizado
        return cache[hash].GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

```

<a id="sec-033"></a>
### 47.7.1 Fluxo de Execução Passo a Passo

[↑ Voltar ao início](#inicio)

```text
             Instanciação: new LineToPointAdapter(line)
                                 │
                       Gerar GetHashCode() da Line
                                 │
                     Existe esse hash no Cache?
                    /                          \
                 (Sim)                         (Não)
                  /                              \
         Retorna imediatamente              Executa a matemática
        [Evita CPU e Memória]               Gera List<Point>
                                            Salva no Cache
                                                   │
                                            Retorna a List<Point>

```

1. **Geração do código hash:** No construtor, `line.GetHashCode()` é executado. Duas linhas consideradas iguais devem produzir o mesmo código; linhas diferentes também podem produzir o mesmo valor por colisão.
2. **Verificação de Existência:** A verificação rápida `cache.ContainsKey(hash)` avalia se a linha já foi convertida anteriormente.

   - **Hit (sucesso no cache):** O construtor é encerrado imediatamente. Nenhuma computação matemática ocorre e nenhuma lista nova é instanciada na memória.
   - **Miss (falha no cache):** O cálculo matemático de interpolação é acionado. A lista temporária `points` é criada, preenchida e adicionada ao dicionário estático `cache` sob a chave do respectivo `hash`.

3. **Redirecionamento do enumerador (`IEnumerable<T>`):** Quando um `foreach` consome o adaptador, a iteração chama `GetEnumerator()`. Em vez de ler uma coleção local, o método retorna `cache[hash].GetEnumerator()`.

> **Impacto arquitetural:** cada adaptador guarda apenas a chave, mas a lista permanece retida pelo cache estático. O desenho reduz duplicação por ocorrência e centraliza memória; não elimina o custo de armazenamento.

<a id="sec-034"></a>
### 47.7.2 Equivalência de Valor na Prática (`Equals` e `GetHashCode`)

[↑ Voltar ao início](#inicio)

Para suportar esse fluxo, as classes `Point` e `Line` precisam redefinir como o .NET realiza comparações de igualdade:

```csharp
public override bool Equals(object obj)
{
    if (ReferenceEquals(null, obj)) return false;
    if (ReferenceEquals(this, obj)) return true;
    if (obj.GetType() != this.GetType()) return false;
    return Equals((Point) obj); // Compara as propriedades X e Y
}

public override int GetHashCode()
{
    unchecked
    {
        return (X * 397) ^ Y; // Multiplicador primo para dispersão de hash
    }
}

```

- **Sem essa sobreposição:** Duas instâncias de `Line` com as mesmas coordenadas seriam tratadas como chaves diferentes se o dicionário usasse as próprias linhas com igualdade por referência, reduzindo a eficácia do cache e aumentando seu consumo de memória.
- **O operador `unchecked`:** Permite *wrap-around* em estouro aritmético no cálculo manual do hash. Ele não trata estouro de pilha e não garante desempenho ou unicidade.

---

<a id="sec-035"></a>
### 47.8 Quando usar e quando evitar

[↑ Voltar ao início](#inicio)

#### Use quando

- O processo de adaptação/tradução exigir processamento matemático, I/O ou operações custosas de análise sintática.
- Os objetos de origem forem imutáveis ou houver uma política explícita de invalidação. Imutabilidade mantém a chave estável, mas não elimina colisões de hash.
- O mesmo conjunto de dados precisar ser adaptado e lido repetidamente em loops ou rotinas de renderização.

#### Evite ou use com cautela se

- Os dados de origem forem altamente mutáveis. Alterações nas propriedades do objeto invalidam o Hash antigo, exigindo rotinas complexas de invalidação de cache para evitar renderizações inconsistentes.
- A quantidade de objetos distintos convertidos for muito grande, pois o dicionário estático pode crescer indefinidamente. Nesse caso, use limites, expiração ou uma política de descarte, como LRU (*Least Recently Used*).
- Houver acesso concorrente: o `Dictionary` comum e a sequência `ContainsKey`/`Add` do exemplo não tornam a criação atômica. É preciso sincronização ou uma estrutura/política apropriada.

---

<a id="sec-036"></a>
## 48. Generic Value Adapter

[↑ Voltar ao início](#inicio)


Esta parte apresenta um adaptador de valores baseado em genéricos, voltado principalmente à reutilização de código e à distinção de dimensões pelo sistema de tipos.

Apesar do nome adotado pelo curso, esta técnica **não é o Adapter canônico do GoF**: não há um `Adaptee` com interface incompatível sendo convertido para um `Target` esperado pelo cliente. Trata-se de um idioma de modelagem de valores no sistema de tipos do C#.

O **Generic Value Adapter** aborda um problema de design muito específico: a impossibilidade de passar valores literais (como números inteiros) como argumentos genéricos no C#, algo que é nativamente suportado em linguagens como C++ através de *Templates* (ex: `template<typename T, int Size> class Vector`).

<a id="sec-037"></a>
### 48.1 O Problema da Dimensionalidade em Estruturas de Dados

[↑ Voltar ao início](#inicio)

Imagine que você está desenvolvendo uma biblioteca gráfica ou de física que precisa lidar com vetores matemáticos de diferentes dimensões: um vetor bidimensional ($X, Y$), um tridimensional ($X, Y, Z$) e, em casos complexos de machine learning ou física quântica, vetores com 4 ou mais dimensões.

Uma abordagem ingênua seria criar uma classe abstrata e herdá-la:

```csharp
public class Vector2D { public double X, Y; }
public class Vector3D { public double X, Y, Z; }

```

**O problema:** Você terá que duplicar toda a lógica matemática (soma, subtração, produto escalar) para cada uma das classes separadamente.

Outra abordagem seria usar um array interno:

```csharp
public class Vector
{
    private double[] data;
    public Vector(int size) => data = new double[size];
}

```

**O problema:** A dimensão não faz parte do tipo. Assim, sem validações adicionais, o programa pode tentar combinar vetores de tamanhos incompatíveis e detectar o erro apenas em tempo de execução. A alocação do array continua existindo também na solução genérica apresentada adiante.

---

<a id="sec-038"></a>
### 48.2 A Solução: Interfaces como Portadores de Valores (Value Adaptors)

[↑ Voltar ao início](#inicio)

Para entender por que precisamos dessa estrutura no C#, vale a pena olhar para como outras linguagens resolvem esse problema de forma nativa e qual é o objetivo de imitá-las.

#### O Paradigma em Outras Linguagens (O modelo do C++)

No C++, existe um conceito chamado **Non-type Template Parameters** (Parâmetros de Template Não-Tipo). Ele permite que você passe valores literais diretamente na assinatura de um tipo genérico:

```cpp
// Em C++ isso é nativo, válido e extremamente comum:
template<typename T, int Size>
class Vector {
    T data[Size]; // Tamanho incorporado ao tipo no exemplo de C++.
};

Vector<float, 3> vector3D; // Um tipo único e imutável de tamanho 3

```

No C#, o sistema de genéricos é estritamente limitado a **Tipos** (classes, structs, interfaces). Não é possível fazer algo como `Vector<float, 3>`. É por isso que precisamos do **Generic Value Adapter**: nós "adaptamos" um valor numérico bruto (como o `3`) envolvendo-o em um Tipo de verdade (uma classe ou struct) que o compilador do C# consiga aceitar.

#### Qual é a finalidade real de fazer isso?

Não se trata apenas de um "truque de sintaxe". A adaptação busca dois objetivos principais:

1. **Segurança de tipos:** Ao transformar a dimensão em parte do tipo, operações declaradas apenas entre vetores da mesma dimensão podem ser rejeitadas pelo compilador quando os tipos não correspondem.
2. **Reutilização:** A lógica comum permanece nas classes genéricas, reduzindo a duplicação entre vetores especializados.

O exemplo não elimina alocações: ele instancia `TDimension` e cria um array `TDataType[]`. Otimizações como *inlining* dependem do código gerado e das decisões do JIT; não são garantidas pelo padrão e devem ser verificadas por medição quando forem relevantes.

---

#### 1. Criando as Interfaces de Dimensão

Primeiro, definimos um contrato comum. Qualquer adaptador que queira representar uma dimensão numérica na assinatura genérica precisará implementar esta interface para expor seu valor inteiro correspondente.

```csharp
public interface IInteger
{
    int Value { get; }
}

```

#### 2. Criando os Adaptadores de Valor Fixo (Os Metadados)

Agora, criamos classes leves que implementam essa interface. Elas não guardam dados; servem exclusivamente como "metadados" para que o compilador diferencie os tamanhos através do sistema de tipos. Cada classe atua como o adaptador que "traduz" o número bruto em um Tipo aceitável para o C#.

```csharp
public static class Dimensions
{
    // Representa o literal 2 como um Tipo
    public class Two : IInteger
    {
        public int Value => 2;
    }

    // Representa o literal 3 como um Tipo
    public class Three : IInteger
    {
        public int Value => 3;
    }
}

```

---

<a id="sec-039"></a>
### 48.3 O Adaptador Genérico e a Assinatura de Metaprogramação

[↑ Voltar ao início](#inicio)

Com as estruturas de dimensão prontas, o exemplo cria uma classe `Vector` genérica. A assinatura utiliza três parâmetros genéricos e duas restrições; isso favorece reutilização e tipos mais específicos, mas não constitui, por si só, uma garantia de alto desempenho:

```csharp
public class Vector<TDerivedClass, TDataType, TDimension>
    where TDimension : IInteger, new()
    where TDerivedClass : Vector<TDerivedClass, TDataType, TDimension>, new()

```

#### Desmistificando os Parâmetros Genéricos

- **`TDerivedClass`**: A classe filha (concreta) que herda de `Vector`. Isso viabiliza o padrão **CRTP** (*Curiously Recurring Template Pattern*), garantindo que a classe pai conheça a assinatura da filha e consiga instanciá-la e retorná-la diretamente sem conversões de tipo (*casts*).
- **`TDataType`**: O tipo primitivo dos dados numéricos reais que serão armazenados (ex: `int`, `float`, `double`).
- **`TDimension`**: O adaptador que carrega a informação de tamanho desejada (ex: `Dimensions.Two`).

#### Desmistificando as Restrições (`where`)

- **`where TDimension : IInteger, new()`**: Exige que a dimensão herde do nosso contrato e possua um construtor vazio. Isso nos permite chamar `new TDimension().Value` em tempo de execução para descobrir o tamanho físico necessário sem expor o array a tamanhos dinâmicos incorretos.
- **`where TDerivedClass : Vector<...>, new()`**: Obriga que a classe informada em `TDerivedClass` seja filha legítima da base, permitindo ao método fábrica `Create` dar um `new TDerivedClass()` e retornar o tipo exato e especializado do cliente.

```csharp
public class Vector<TDerivedClass, TDataType, TDimension>
    where TDimension : IInteger, new()
    where TDerivedClass : Vector<TDerivedClass, TDataType, TDimension>, new()
{
    protected TDataType[] data;

    // Construtor Padrão: Inicializa o array com o tamanho exato extraído de TDimension
    public Vector()
    {
        data = new TDataType[new TDimension().Value];
    }

    // Construtor de Inicialização: Limita o preenchimento ao tamanho físico exigido
    public Vector(params TDataType[] values)
    {
        var requiredSize = new TDimension().Value;
        data = new TDataType[requiredSize];
        var providedSize = values.Length;

        for (int i = 0; i < Math.Min(requiredSize, providedSize); ++i)
            data[i] = values[i];
    }

    // Método-fábrica estático: retorna o tipo especializado da classe derivada
    public static TDerivedClass Create(params TDataType[] values)
    {
        var result = new TDerivedClass();
        var requiredSize = new TDimension().Value;
        result.data = new TDataType[requiredSize];
        var providedSize = values.Length;

        for (int i = 0; i < Math.Min(requiredSize, providedSize); ++i)
            result.data[i] = values[i];

        return result;
    }

    public TDataType this[int index]
    {
        get => data[index];
        set => data[index] = value;
    }

    public TDataType X
    {
        get => data[0];
        set => data[0] = value;
    }
}

```

---

<a id="sec-040"></a>
### 48.4 Especialização e Herança Segura

[↑ Voltar ao início](#inicio)

Para tornar o uso elegante, herdamos do adaptador genérico amarrando os tipos de dados e liberando a implementação de operadores matemáticos que só fazem sentido para tipos específicos:

```csharp
// Especialização intermediária para floats
public class VectorOfFloat<TDerivedClass, TDimension>
    : Vector<TDerivedClass, float, TDimension>
    where TDimension : IInteger, new()
    where TDerivedClass : Vector<TDerivedClass, float, TDimension>, new()
{
}

// Especialização intermediária para inteiros com suporte a soma
public class VectorOfInt<TDimension> : Vector<VectorOfInt<TDimension>, int, TDimension>
    where TDimension : IInteger, new()
{
    public VectorOfInt() { }
    public VectorOfInt(params int[] values) : base(values) { }

    // O compilador garante que só podemos somar vetores que possuem a MESMA assinatura TDimension!
    public static VectorOfInt<TDimension> operator +
      (VectorOfInt<TDimension> leftVector, VectorOfInt<TDimension> rightVector)
    {
        var result = new VectorOfInt<TDimension>();
        var dimensionValue = new TDimension().Value;

        for (int i = 0; i < dimensionValue; i++)
        {
            result[i] = leftVector[i] + rightVector[i];
        }

        return result;
    }
}

```

#### Implementações Concretas Limpas

A herança final fica extremamente curta e livre de código duplicado:

```csharp
// Vetor de inteiros com tamanho exato de 2 posições
public class Vector2i : VectorOfInt<Dimensions.Two>
{
    public Vector2i() { }
    public Vector2i(params int[] values) : base(values) { }
}

// Vetor de floats com tamanho exato de 3 posições
public class Vector3f : VectorOfFloat<Vector3f, Dimensions.Three>
{
    public override string ToString() => $"{string.Join(",", data)}";
}

```

---

<a id="sec-041"></a>
### 48.5 Sintaxe de Uso no Mundo Real

[↑ Voltar ao início](#inicio)

Graças aos tipos de dimensão, as operações cuja assinatura exige o mesmo `TDimension` rejeitam dimensões diferentes em tempo de compilação. Isso não valida automaticamente índices, quantidade de valores recebidos ou todas as regras matemáticas do domínio:

```csharp
static void Main(string[] args)
{
    // 1. Criação de vetores de 2 dimensões
    var vectorA = new Vector2i(1, 2);
    vectorA[0] = 0;

    var vectorB = new Vector2i(3, 2);

    // Operação válida: Ambos são do tipo Vector2i
    var result = vectorA + vectorB;

    // 2. Criação de vetor de 3 dimensões usando um método-fábrica estático
    // Graças ao CRTP, o tipo retornado é exatamente 'Vector3f' (e não o tipo genérico pai)
    Vector3f vector3D = Vector3f.Create(3.5f, 2.2f, 1.0f);

    // O código abaixo geraria um ERRO DE COMPILAÇÃO imediato:
    // var erro = vectorA + vector3D; // Tipos incompatíveis! (Vector2i vs VectorOfInt<Three>)
}

```

---

<a id="sec-042"></a>
### 48.5.2 O que o arquivo local realmente executa

[↑ Voltar ao início](#inicio)

O arquivo `GenericValueAdapter.cs` não contém `Vector2f`, atributos `[Test]` nem asserções de um framework de testes. Seu `Main` cria dois `Vector2i`, soma-os e também cria um `Vector3f` por um método-fábrica estático.

Uma verificação automatizada poderia ser escrita separadamente, mas não deve ser apresentada como se já existisse no repositório. No código atual, a soma percorre duas posições porque `Vector2i` fixa `TDimension` como `Dimensions.Two`; os dados continuam armazenados em arrays alocados no heap.

---

<a id="sec-043"></a>
### 48.6 Vantagens e Trade-offs

[↑ Voltar ao início](#inicio)

| Vantagem | Descrição |
| --- | --- |
| **Segurança em tempo de compilação** | Impede as combinações incompatíveis que as assinaturas genéricas efetivamente modelam. |
| **Redução de duplicação** | Algoritmos comuns podem ser escritos uma vez nas classes genéricas base. |
| **Possibilidade de otimização** | O JIT pode otimizar partes do código genérico, mas o padrão não garante *inlining*, eliminação de alocações ou desenrolamento de laços. |

- **Trade-off:** A sintaxe de assinatura com genéricos encadeados (`Vector<TDerivedClass, TDataType, TDimension>`) pode se tornar densa e complexa de explicar e manter para desenvolvedores iniciantes na equipe.

---

<a id="sec-044"></a>
### 48.7 Resumo Mental Curto

[↑ Voltar ao início](#inicio)

- **Generic Value Adapter:** Representa valores como tipos auxiliares para que informações como a dimensão participem da assinatura genérica. O principal ganho é distinguir tipos e reutilizar código; desempenho precisa ser medido separadamente.

---

<a id="sec-045"></a>
## 49. Adapter in Dependency Injection

[↑ Voltar ao início](#inicio)



<a id="sec-046"></a>
### 49.1 O que é

[↑ Voltar ao início](#inicio)

**Adapter in Dependency Injection** é a aplicação do padrão estrutural Adapter com a regra de adaptação registrada no **container de DI** — neste caso, por meio da API `RegisterAdapter` do Autofac.

Nas implementações tradicionais do padrão (como vimos em `VectorRasterDemo` ou `AdapterCaching`), a criação do Adapter é manual: o próprio desenvolvedor precisa codificar a instanciação explícita (ex: `new LineToPointAdapter(line)`). Aqui, a criação e a resolução do Adapter passam a acontecer sob demanda de forma declarativa na configuração do container.

<a id="sec-047"></a>
### 49.1.1 Entendendo o Autofac

[↑ Voltar ao início](#inicio)


Para que a solução do **Adapter via Container de DI** funcione, nós adicionamos uma biblioteca externa ao projeto através do namespace `using Autofac;`. Mas o que é essa ferramenta e por que ela é tão importante aqui?

---

#### 49.1.2 O que é o Autofac e para que serve?

O **Autofac** é um container de **Injeção de Dependência** de terceiros para .NET. A documentação oficial oferece registro de adapters para transformar cada serviço de um contrato de origem em outro contrato.

Classes de negócio se beneficiam quando recebem dependências explícitas em vez de localizar implementações concretas internamente. Um container como o Autofac pode automatizar a composição, mas usar `new` no *composition root* continua sendo uma alternativa válida para sistemas pequenos.

Sua função resume-se em duas etapas simples:

1. **Registro:** Você diz ao Autofac como criar cada objeto do seu sistema (ex: *"Sempre que alguém pedir uma interface `ICommand`, crie uma instância de `SaveCommand`"*).
2. **Resolução:** Quando o Autofac resolve um objeto como `Editor`, ele seleciona os construtores e registros compatíveis e cria ou reutiliza dependências conforme a configuração e seus lifetimes.

---

#### 49.1.3 Como o Autofac é usado no exemplo?

No arquivo do nosso exemplo, o Autofac é inicializado, configurado e executado através de três classes e métodos principais:

##### 1. O `ContainerBuilder`

É o objeto que usamos para registrar nossas "receitas" de criação. Nele, dizemos o que o nosso sistema possui:

```csharp
var b = new ContainerBuilder();

```

##### 2. O registro dos comandos e metadados

Nós dizemos ao Autofac: *"Guarde a receita do `OpenCommand` e do `SaveCommand`. Ambos servem como um `ICommand`"*.

Além disso, usamos o método `.WithMetadata()` para colar uma "etiqueta" invisível nesses comandos. Essa etiqueta diz qual deve ser o nome visual do botão que vai envelopar esse comando mais tarde:

```csharp
b.RegisterType<OpenCommand>()  // Registra a classe concreta
 .As<ICommand>()               // Expõe como a interface genérica
 .WithMetadata("Name", "Open"); // Cola uma etiqueta com a string "Open"

```

##### 3. A regra do Adapter (`RegisterAdapter`)

Aqui está o recurso exclusivo do Autofac que resolve o nosso padrão de projeto. Nós dizemos ao builder:

> *"Se algum componente pedir um `Button`, mas eu só tiver `ICommand` registrados no estoque, pegue o comando, leia a etiqueta de metadados (`Meta<ICommand>`) com o nome, passe ambos para o construtor do `Button` e entregue o botão pronto!"*

```csharp
b.RegisterAdapter<Meta<ICommand>, Button>(cmd =>
    new Button(cmd.Value, (string)cmd.Metadata["Name"]));

```

##### 4. O `IContainer` e a resolução

Depois de configurar todas as regras na prancheta (`ContainerBuilder`), nós "construímos" o container final chamando `.Build()`.

Depois de `Build()`, o cliente pede ao container que resolva o `Editor`. O operador `new` não desaparece: as funções de registro e o próprio container continuam construindo objetos internamente; o consumidor apenas deixa de fazer essa composição manualmente.

```csharp
using (var container = b.Build())
{
    // O Autofac cria o Editor, percebe que ele precisa de Buttons,
    // ativa a regra do Adapter, transforma os ICommands em Buttons
    // e nos entrega o Editor totalmente montado e pronto!
    var editor = container.Resolve<Editor>();

    editor.ClickAll(); // Executa o clique de todos os botões adaptados!
}

```

Sem `RegisterAdapter`, a mesma composição poderia ser escrita com factories ou registros explícitos. O Autofac reduz esse código de infraestrutura e integra a adaptação aos lifetimes do container; não é necessário presumir que a alternativa exigiria reflection.

<a id="sec-048"></a>
### 49.2 O problema que ele resolve

[↑ Voltar ao início](#inicio)

Imagine o seguinte cenário de design em uma aplicação:

- Temos um componente chamado `Editor` que necessita exibir e interagir com um conjunto de botões na tela. Portanto, o construtor do `Editor` depende de uma coleção de botões: `IEnumerable<Button>`.
- Por outro lado, as nossas operações e lógicas de negócio reais são mapeadas e registradas no container como comandos puros (`ICommand`), tais como `SaveCommand` e `OpenCommand`. Nenhum objeto do tipo `Button` é registrado diretamente no container.
- O tipo `Button` **não implementa** a interface `ICommand`. Em vez disso, ele utiliza a **composição** para envolver um comando interno (comportando-se como o *Target* do padrão).

Temos uma **incompatibilidade clássica de assinaturas/interfaces**: o `Editor` exige receber uma lista de `Button`, mas o container de DI só possui instâncias de `ICommand` registradas.

<a id="sec-049"></a>
### 49.2.1 Leitura concreta do conflito

[↑ Voltar ao início](#inicio)

Para entender o problema real, imagine que estamos desenvolvendo o painel de controle (o `Editor`) de um sistema de automação.

#### 1. O que o `Editor` precisa para funcionar?

O `Editor` é um componente visual (UI) que renderiza botões na tela. Ele não quer saber o que cada botão faz internamente; ele apenas precisa de uma lista de botões para desenhar na tela e disparar o clique quando o usuário interagir.

Por isso, o construtor dele exige uma coleção de botões:

```csharp
public class Editor
{
    // O Editor exige receber uma lista de objetos do tipo Button
    public Editor(IEnumerable<Button> buttons) { ... }
}

```

#### 2. O que o container possui nos registros?

Do outro lado, nossa lógica de negócios (como salvar arquivos ou abrir documentos) foi escrita usando o padrão Command (`ICommand`). No nosso container de Injeção de Dependência (DI), nós registramos essas operações de negócio:

- `SaveCommand` (que implementa `ICommand`)
- `OpenCommand` (que implementa `ICommand`)

Nenhum objeto do tipo `Button` está registrado no container. O container só conhece e sabe criar `ICommand`s.

---

#### 3. Onde ocorre o conflito de composição?

O tipo `Button` **não implementa** a interface `ICommand`. Em vez disso, o `Button` apenas *envolve* um comando (composição):

```csharp
public class Button
{
    private ICommand command;

    // O Button precisa receber um comando para saber o que fazer ao ser clicado
    public Button(ICommand command, string name) { ... }
}

```

Sem um registro de `Button` ou adapter:

1. O `Editor` solicita `IEnumerable<Button>`.
2. O Autofac resolve todas as implementações registradas de `Button`.
3. Se nenhuma existir, a relação implícita de coleção produz uma coleção vazia; o `Editor` pode ser criado, mas não terá botões. O adapter é o que transforma os comandos registrados em serviços `Button`.

#### A solução manual (Sem o Adapter no DI)

Sem o recurso automático, você seria obrigado a poluir seu código de inicialização instanciando manualmente cada botão para poder passá-los ao construtor do `Editor`:

```csharp
// Acoplamento manual e repetitivo:
var openCmd = new OpenCommand();
var saveCmd = new SaveCommand();

var openButton = new Button(openCmd, "Open");
var saveButton = new Button(saveCmd, "Save");

var editor = new Editor(new[] { openButton, saveButton });

```

Essa composição manual é correta, mas passa ao *composition root* a responsabilidade de criar cada botão e escolher seus lifetimes. O registro de adapter é vantajoso quando o projeto já usa Autofac e deseja centralizar essa regra.

O registro de adapter resolve isso de forma declarativa: para cada serviço de origem `Meta<ICommand>` disponível, o Autofac registra a transformação que produz um `Button`.

<a id="sec-050"></a>
### 49.3 A solução: como o container resolve isso no código

[↑ Voltar ao início](#inicio)

O container de Injeção de Dependência (neste caso, o Autofac) resolve esse "bloqueio de fiação" atuando como uma fábrica automatizada que transforma os tipos em tempo de execução. Ele faz isso através de uma instrução específica na configuração do sistema (`Composition Root`).

Em vez de você instanciar os botões um a um, você define uma **regra de conversão global** usando o método `RegisterAdapter`.

Aqui está o trecho exato de código que faz a mágica acontecer:

```csharp
// 1. O Autofac detecta os comandos disponíveis no sistema
b.RegisterType<OpenCommand>().As<ICommand>().WithMetadata("Name", "Open");
b.RegisterType<SaveCommand>().As<ICommand>().WithMetadata("Name", "Save");

// 2. A MÁGICA: Define a regra de adaptação automática
b.RegisterAdapter<Meta<ICommand>, Button>(cmd =>
    new Button(cmd.Value, (string)cmd.Metadata["Name"]));

```

---

#### Passo a passo da resolução

Quando a aplicação inicializa e solicita o `Editor`, o container realiza o seguinte fluxo de trás para frente:

1. **A Demanda do Cliente:** O `Editor` avisa ao container: *"Preciso de uma lista de botões (`IEnumerable<Button>`) para nascer"*.
2. **A Busca do Container:** O container olha para o seu catálogo de registros e percebe que não tem nenhum `Button` cadastrado diretamente, mas encontra a regra `RegisterAdapter<Meta<ICommand>, Button>`.
3. **A ativação da regra:** o registro informa como converter cada `Meta<ICommand>` resolvido em `Button`.
4. **As origens:** os registros de `OpenCommand` e `SaveCommand`, expostos como `ICommand` com metadados, fornecem os valores de origem.
5. **A Execução da Lambda (A Transformação):** Para cada `ICommand` encontrado, o container executa a expressão lambda que passamos no registro:

   - Ele pega o `OpenCommand` (`cmd.Value`) e o nome `"Open"` (`cmd.Metadata["Name"]`) e cria `new Button(OpenCommand, "Open")`.
   - Ele pega o `SaveCommand` (`cmd.Value`) e o nome `"Save"` (`cmd.Metadata["Name"]`) e cria `new Button(SaveCommand, "Save")`.

6. **A entrega:** O container agrupa os novos objetos `Button` em uma coleção e os injeta no construtor do `Editor`.

#### Resultado prático

O `Editor` recebe `IEnumerable<Button>`, e a criação dos botões fica centralizada na configuração. O adapter continua sendo a transformação `Meta<ICommand> -> Button`; o container apenas executa e gerencia essa regra.

---

#### A regra `RegisterAdapter`

Em vez de forçar o programador a instanciar e converter manualmente cada comando em um botão no código de inicialização, configuramos o container de DI usando o método `RegisterAdapter<TFrom, TTo>()` do Autofac.

Esse método instrui o container:

> *"Para cada serviço `TFrom` registrado, disponibilize um serviço `TTo` produzido por esta função de adaptação."*

O padrão de projeto continua sendo o **Adapter estrutural**, mas sua instanciação e acoplamento de ciclo de vida são resolvidos de maneira transparente pela infraestrutura de DI da aplicação.

---

<a id="sec-051"></a>
### 49.4 O exemplo prático: `AdapterDependency.cs`

[↑ Voltar ao início](#inicio)

No arquivo `AdapterDependency.cs`, essa mecânica é implementada de forma explícita utilizando os recursos do Autofac.

#### 1. As Classes de Origem (*Adaptees*)

Temos a abstração do comando e as implementações que realizam as operações:

```csharp
public interface ICommand
{
    void Execute();
}

public class SaveCommand : ICommand
{
    public void Execute() => Console.WriteLine("Saving current file");
}

public class OpenCommand : ICommand
{
    public void Execute() => Console.WriteLine("Opening a file");
}

```

#### 2. O Tipo Alvo (*Target/Adapter*)

O `Button` funciona como o adaptador estrutural. Ele não implementa `ICommand`, mas encapsula um comando para dispará-lo no método `Click()`:

```csharp
public class Button
{
    private ICommand command;
    private string name;

    public Button(ICommand command, string name)
    {
        // Guard Clause / Fail-fast para evitar botões órfãos de comando
        this.command = command ?? throw new ArgumentNullException(nameof(command));
        this.name = name;
    }

    public void Click() => command.Execute();
    public void PrintMe() => Console.WriteLine($"I am a button called {name}");
}

```

#### 3. O Cliente (*Client*)

O `Editor` é o consumidor que exige a coleção adaptada de botões:

```csharp
public class Editor
{
    private readonly IEnumerable<Button> buttons;
    public IEnumerable<Button> Buttons => buttons;

    public Editor(IEnumerable<Button> buttons)
    {
        this.buttons = buttons; // Espera receber Buttons adaptados dos ICommands
    }

    public void ClickAll()
    {
        foreach (var btn in buttons) btn.Click();
    }
}

```

#### 4. O Acoplamento no Container de DI

No método de entrada, registramos os comandos com seus respectivos **metadados** (para identificar os nomes dos botões) e definimos as rotas de adaptação:

```csharp
var b = new ContainerBuilder();

// 1. Registra os comandos e anexa metadados para identificação posterior
b.RegisterType<OpenCommand>().As<ICommand>().WithMetadata("Name", "Open");
b.RegisterType<SaveCommand>().As<ICommand>().WithMetadata("Name", "Save");

// 2. Adapta cada comando com seus metadados para Button
b.RegisterAdapter<Meta<ICommand>, Button>(cmd =>
    new Button(cmd.Value, (string)cmd.Metadata["Name"]));

b.RegisterType<Editor>();

```

---

<a id="sec-052"></a>
### 49.5 O uso de Metadados (`Meta<T>`)

[↑ Voltar ao início](#inicio)

O Autofac introduz um recurso avançado chamado **Metadados** através da classe genérica `Meta<T>`.

- Ao registrar os comandos concretos com `.WithMetadata("Name", "Value")`, conseguimos anexar dados extras à declaração que não pertencem à assinatura do contrato `ICommand`.
- No momento em que o Adapter é resolvido, o container entrega um wrapper do tipo `Meta<ICommand>`. Ele nos dá acesso simultâneo a duas coisas:

1. `.Value`: A instância real do objeto adaptado (`ICommand`).
2. `.Metadata`: Um dicionário contendo as informações de metadados associadas no registro (ex: `"Name"`).

Desta forma, o Adapter consegue recuperar dinamicamente o nome do botão sem que o `ICommand` precise expor uma propriedade de apresentação visual que não faz sentido para a sua responsabilidade de negócio.

<a id="sec-053"></a>
### 49.6 Cuidados com registros concorrentes

[↑ Voltar ao início](#inicio)

O arquivo local registra duas rotas para produzir `Button`: uma a partir de `ICommand` e outra a partir de `Meta<ICommand>`. O comentário do próprio arquivo declara incerteza sobre o resultado dessa competição. Como essa conclusão não está sustentada pela documentação oficial consultada, ela não deve ser apresentada como limitação comprovada do Autofac.

No exemplo didático, mantenha apenas a rota que corresponde ao resultado desejado — aqui, `Meta<ICommand> -> Button` — e cubra a resolução de `IEnumerable<Button>` com um teste. Isso evita depender de precedência implícita entre registros que produzem o mesmo serviço.

<a id="sec-054"></a>
### 49.7 Quando utilizar

[↑ Voltar ao início](#inicio)

- Quando a aplicação já utiliza um container de DI robusto (como o Autofac) para gerenciar o ciclo de vida dos objetos.
- Quando há uma incompatibilidade clara entre as interfaces expostas pelos seus serviços internos/infraestrutura e as interfaces esperadas pelos componentes visuais ou consumidores de alto nível.
- Quando se deseja manter o código de criação do adaptador centralizado em um único arquivo de inicialização de infraestrutura (*Composition Root*), mantendo as classes de negócio limpas de instanciações manuais.

---



<a id="sec-055"></a>
## Coding Exercise 5: Adapter Coding Exercise

[↑ Voltar ao início](#inicio)

Esta parte corresponde ao mapeamento prático do padrão **Adapter** aplicado a um desafio clássico de compatibilidade de interfaces. O objetivo deste exercício é consolidar como podemos fazer duas classes com assinaturas ou conceitos geometricamente distintos trabalharem juntas sem modificar nenhuma delas diretamente.

---

<a id="sec-056"></a>
### O que o exercício quer de verdade

[↑ Voltar ao início](#inicio)

O enunciado típico deste exercício coloca o seguinte problema: você tem uma classe existente chamada `Square` (Quadrado), que possui apenas a propriedade `Side` (Lado). Por outro lado, o seu sistema possui uma interface ou um método que calcula propriedades de figuras geométricas, mas que só aceita objetos do tipo `Rectangle` (Retângulo) ou que implementem uma interface `IRectangle`, exigindo explicitamente as propriedades `Width` (Largura) e `Height` (Altura).

O seu objetivo não é reescrever a classe `Square` para transformá-la em um retângulo. O desafio é:

> **Como criar um intermediário que exponha a interface de um Retângulo a partir de uma instância existente de um Quadrado?**

---

<a id="sec-057"></a>
### Como ler o uso esperado

[↑ Voltar ao início](#inicio)

O uso esperado do padrão no código cliente segue esta dinâmica:

```csharp
var square = new Square { Side = 11 };
IRectangle adapter = new SquareToRectangleAdapter(square);

Console.WriteLine($"Width: {adapter.Width}, Height: {adapter.Height}");

```

Análise das etapas:

1. `new Square { Side = 11 }` cria o objeto **legado ou incompatível** que precisamos usar.
2. `new SquareToRectangleAdapter(square)` cria o **Adapter**, injetando o quadrado original como dependência.
3. O cliente lê `adapter.Width` e `adapter.Height` de forma transparente, interagindo com o contrato que ele esperava (`IRectangle`), enquanto o adapter traduz internamente essa solicitação para a propriedade `Side` do quadrado subjacente.

---

<a id="sec-058"></a>
### Participantes do padrão no exercício

[↑ Voltar ao início](#inicio)

Mapeando as peças para os papéis clássicos do GoF:

- **Target (Alvo):** `IRectangle` (ou a classe `Rectangle`). É a interface que o cliente deseja e sabe como usar.
- **Adaptee (Adaptado):** `Square`. É a classe existente que possui o comportamento/dados necessários, mas cuja interface é incompatível.
- **Adapter (Adaptador):** `SquareToRectangleAdapter`. A classe que envelopa o *Adaptee* e implementa o *Target*, traduzindo as chamadas.
- **Client (Cliente):** O código que consome o `IRectangle`.

---

<a id="sec-059"></a>
### O que faz este exercício ser um Adapter

[↑ Voltar ao início](#inicio)

O que caracteriza o **Adapter Pattern** aqui é o papel de **wrapper** (invólucro) focado em compatibilização de assinaturas:

- Ele **não altera** o estado interno do Quadrado original.
- Ele **não adiciona** novas regras de negócio complexas.
- Ele apenas serve como um tradutor em tempo de execução: quando o cliente pede a largura de um retângulo, o adaptador responde com o tamanho do lado do quadrado.

Se o cliente fizesse conversões manuais, a responsabilidade ficaria espalhada. Forçar `Square` a herdar de um `Rectangle` **mutável com largura e altura independentes** costuma violar o contrato de substituição; com uma abstração somente leitura como `IRectangle`, esse problema específico não aparece automaticamente.

---

<a id="sec-060"></a>
### Leitura guiada da solução

[↑ Voltar ao início](#inicio)

A estrutura do código que resolve o exercício é dividida em três partes claras:

#### 1. As Classes Base (Incompatíveis)

```csharp
public class Square
{
    public int Side;
}

public interface IRectangle
{
    int Width { get; }
    int Height { get; }
}

```

#### 2. O Adaptador (A Solução)

```csharp
public class SquareToRectangleAdapter : IRectangle
{
    private readonly Square square;

    public SquareToRectangleAdapter(Square square)
    {
        this.square = square;
    }

    public int Width => square.Side;
    public int Height => square.Side;
}

```

#### 3. O Fluxo de Execução

Internamente, as propriedades `Width` e `Height` usam a sintaxe de expressão (*expression-bodied members*) do C# para redirecionar a leitura diretamente para `square.Side`. Se o valor de `Side` mudar no objeto original, o adaptador reflete essa mudança instantaneamente porque ele guarda uma referência ao objeto real, e não uma cópia estática dos valores.

---

<a id="sec-061"></a>
### O que o aluno precisa enxergar para conseguir reproduzir sozinho

[↑ Voltar ao início](#inicio)

Se você precisar criar qualquer outro adaptador no futuro, o fluxo de design é idêntico:

1. Identifique a interface que o seu código atual **exige** (*Target*).
2. Identifique a classe que contém os dados ou a lógica que você **quer aproveitar** (*Adaptee*).
3. Crie uma nova classe que **implemente ou herde o Target**.
4. Injete o *Adaptee* no construtor dessa nova classe.
5. Em cada método ou propriedade da nova classe, faça a conversão dos parâmetros e repasse a chamada para o objeto injetado.

---

<a id="sec-062"></a>
### O que costuma confundir aqui

[↑ Voltar ao início](#inicio)

- **Confundir Adapter com Facade:** O *Adapter* muda a interface de um único objeto para torná-la compatível com outra assinatura. O *Facade* cria uma interface inteiramente nova e simplificada para um subsistema inteiro de várias classes.
- **Armazenar valores em vez da referência:** Um erro comum é ler `square.Side` no construtor do adaptador e salvar em campos locais `width` e `height`. Se o quadrado for modificado depois da criação do adaptador, o adaptador ficará desatualizado. Guardar a referência do objeto `Square` garante consistência dinâmica.
- **Tentar herdar em vez de compor:** Tentar fazer `SquareToRectangleAdapter` herdar de `Square` e `Rectangle` ao mesmo tempo (herança múltipla não é suportada diretamente dessa forma em C#). A composição (*segurar uma instância de Square*) é o caminho correto e flexível (conhecido como *Object Adapter*).

---

<a id="sec-063"></a>
### Como validar mentalmente se a solução está correta

[↑ Voltar ao início](#inicio)

- O adaptador implementa a interface que o cliente precisa?
- O adaptador recebe a classe incompatível por injeção?
- Tanto `Width` quanto `Height` retornam exatamente o valor correspondente ao `Side` do quadrado adaptado?
- Se o valor do lado do quadrado mudar na memória, o adaptador reflete o novo valor na próxima leitura?

Se todas as respostas forem positivas, a implementação cumpre a intenção de compatibilização. Em produção, acrescente também validação de `null` e documente se o adapter deve refletir alterações posteriores no `Square`.

---


<a id="sec-glossario"></a>
## Glossário
[↑ Voltar ao início](#inicio)

- **Target (alvo):** interface que o cliente espera usar.
- **Adaptee:** classe existente cuja interface é incompatível com o alvo.
- **Adapter (adaptador):** converte chamadas do alvo para a interface existente.
- **Composição:** técnica em que o adaptador mantém referência ao objeto adaptado.
