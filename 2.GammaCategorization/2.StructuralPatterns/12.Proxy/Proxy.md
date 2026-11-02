<a id="inicio"></a>
# Proxy — controlar o acesso a outro objeto

## Sumário
[↑ Voltar ao início](#inicio)

- [O que o padrão faz](#sec-001)
- [Implementação passo a passo: Protection Proxy](#sec-002)
- [Cuidados e quando usar](#sec-003)
- [Relação com este repositório](#sec-004)
- [Limite de segurança e ciclo de vida](#sec-005)
- [Variações e aprofundamentos](#sec-006)
- [76. Protection Proxy](#sec-007)
- [77. Property Proxy](#sec-008)
- [78. Value Proxy](#sec-009)
- [79. Composite Proxy: SoA/AoS](#sec-010)
- [80. Composite Proxy with Array-Backed Properties](#sec-011)
- [81. Dynamic Proxy for Logging](#sec-012)
- [82. Proxy vs. Decorator](#sec-013)
- [83. ViewModel](#sec-014)
- [Coding Exercise 11: Proxy Coding Exercise](#sec-015)
- [84. Bit Fragging](#sec-016)


**Família GoF:** estrutural. **Escopo predominante:** objetos; um representante controla ou media o acesso.

<a id="sec-001"></a>
## O que o padrão faz

[↑ Voltar ao início](#inicio)

Proxy representa outro objeto e oferece ao cliente uma interface equivalente ou compatível. O cliente fala com o representante; o proxy decide se, quando e como encaminhar a chamada ao objeto real.

A intenção define a variante: Protection Proxy verifica autorização; Virtual Proxy adia a criação de um objeto caro; Remote Proxy representa um objeto em outro processo; Logging Proxy registra chamadas. A forma pode se parecer com Decorator, mas Decorator adiciona responsabilidade ao comportamento enquanto Proxy controla acesso ou representação.

<a id="sec-002"></a>
## Implementação passo a passo: Protection Proxy

[↑ Voltar ao início](#inicio)

1. Defina o contrato que o cliente usará.
2. Faça o objeto real implementar esse contrato.
3. Faça o proxy implementar o mesmo contrato e guardar o objeto real.
4. Antes de delegar, aplique a política de acesso.
5. Só encaminhe a chamada se a política permitir.

    using System;

    public interface IArquivo
    {
        string Ler();
    }

    public interface IAutorizacao
    {
        bool PodeLer();
    }

    public sealed class AutorizacaoPorUsuario : IAutorizacao
    {
        private readonly bool permitido;
        public AutorizacaoPorUsuario(bool permitido) => this.permitido = permitido;
        public bool PodeLer() => permitido;
    }

    public sealed class ArquivoReal : IArquivo
    {
        private readonly string conteudo;
        public ArquivoReal(string conteudo) => this.conteudo = conteudo;
        public string Ler() => conteudo;
    }

    public sealed class ArquivoProtegido : IArquivo
    {
        private readonly IArquivo interno;
        private readonly IAutorizacao autorizacao;

        public ArquivoProtegido(IArquivo interno, IAutorizacao autorizacao)
        {
            this.interno = interno ?? throw new ArgumentNullException(nameof(interno));
            this.autorizacao = autorizacao ?? throw new ArgumentNullException(nameof(autorizacao));
        }

        public string Ler()
        {
            if (!autorizacao.PodeLer())
                throw new UnauthorizedAccessException("Leitura não autorizada.");

            return interno.Ler();
        }
    }

    IArquivo arquivo = new ArquivoProtegido(
        new ArquivoReal("dados confidenciais"),
        new AutorizacaoPorUsuario(permitido: true));
    string conteudo = arquivo.Ler();

O consumidor depende de IArquivo. Ele não precisa adicionar uma verificação de permissão antes de cada chamada; ArquivoProtegido concentra esse controle e delega a operação real somente depois da autorização. A injeção de IAutorizacao torna a regra substituível e testável.

<a id="sec-003"></a>
## Cuidados e quando usar

[↑ Voltar ao início](#inicio)

Um proxy não deve deixar de preservar as garantias do contrato do serviço real sem explicação. Também precisa definir como tratar serviço indisponível, falha de autorização e logging de dados sensíveis. Em sistemas reais, verifique a autorização no servidor que protege o recurso; uma verificação apenas no cliente pode ser contornada.

Use Proxy quando houver uma política consistente de acesso, criação tardia ou representação remota. Se o cliente só precisa de uma API de alto nível que coordena vários serviços, considere Facade. Se a intenção é empilhar comportamento adicional, considere Decorator.

<a id="sec-004"></a>
## Relação com este repositório

[↑ Voltar ao início](#inicio)

Não há arquivo de código Proxy nas pastas de padrões. O exemplo didático completo desta página mostra a variante Protection Proxy e pode ser estudado sem depender de outro documento.


<a id="sec-005"></a>
## Limite de segurança e ciclo de vida

[↑ Voltar ao início](#inicio)

O Protection Proxy concentra uma checagem perto do recurso, mas não deve ser tratado automaticamente como uma barreira de segurança suficiente. Se o recurso estiver em outro servidor, a autorização real precisa ser validada no servidor; um cliente controlado pelo usuário pode ser alterado ou contornado.

Virtual Proxy tem outra responsabilidade: adia a criação cara até o primeiro uso. Em aplicações concorrentes, esse primeiro acesso precisa ser sincronizado para evitar criar o recurso várias vezes. Remote Proxy também precisa tratar timeout, falha de rede e serialização de erros. Essas variantes compartilham a forma de representante, mas têm políticas e riscos diferentes.

O Proxy preserva um contrato para tornar seu uso transparente, porém a transparência tem limites: latência, exceções, identidade e ciclo de vida podem diferir do objeto local. Documente essas diferenças para o cliente.

<a id="sec-006"></a>
## Variações e aprofundamentos

[↑ Voltar ao início](#inicio)

As seções abaixo reúnem variações, exemplos e decisões que complementam a explicação principal deste padrão. Elas preservam a linguagem e o contexto do material didático original.

<a id="sec-007"></a>
## 76. Protection Proxy

[↑ Voltar ao início](#inicio)


Protection Proxy verifica se uma operação pode ser executada antes de encaminhar a chamada. É comum em autorização, restrição por idade, permissão de usuário ou validação de contexto.

Fluxo:

```text
cliente chama Operacao()
proxy verifica permissão
se permitido, chama objeto real
se negado, bloqueia ou devolve resposta segura
```

O cuidado é não espalhar regra de autorização por todos os clientes. A proteção fica em um ponto controlado, perto do acesso ao recurso.

---

<a id="sec-008"></a>
## 77. Property Proxy

[↑ Voltar ao início](#inicio)


Property Proxy envolve uma propriedade para controlar leitura, escrita, validação, notificação ou comparação de mudanças. Em C#, essa ideia aparece em propriedades que fazem mais do que apenas guardar um campo.

Exemplo mental:

```csharp
set
{
  if (value == field) return;
  field = value;
  OnPropertyChanged();
}
```

A propriedade vira uma pequena fronteira de controle. O cliente usa uma aparência simples, mas a classe protege invariantes ou emite eventos.

---

<a id="sec-009"></a>
## 78. Value Proxy

[↑ Voltar ao início](#inicio)


Value Proxy representa um valor simples com comportamento adicional. Em vez de usar apenas `int`, `double` ou `string`, o domínio ganha um tipo pequeno que carrega semântica.

Exemplos mentais:

```text
Percentage
Money
UserId
Meters
```

O objetivo não é criar classe para tudo. O objetivo é evitar que valores com significado diferente sejam tratados como números ou textos genéricos demais.

Regra prática:

```text
se o valor tem regra, unidade ou invariante própria,
um value proxy pode deixar o código mais honesto
```

---

<a id="sec-010"></a>
## 79. Composite Proxy: SoA/AoS

[↑ Voltar ao início](#inicio)


Composite Proxy: SoA/AoS contrasta duas formas de guardar muitos objetos pequenos.

`AoS` significa **Array of Structures**:

```text
pontos:
  Point(x, y)
  Point(x, y)
  Point(x, y)
```

`SoA` significa **Structure of Arrays**:

```text
xs:
  x, x, x

ys:
  y, y, y
```

O proxy aparece quando queremos oferecer ao cliente a aparência de objetos individuais, mesmo que internamente os dados estejam organizados em arrays separados por desempenho, memória ou acesso vetorizado.

---

<a id="sec-011"></a>
## 80. Composite Proxy with Array-Backed Properties

[↑ Voltar ao início](#inicio)


Neste tópico, propriedades de objetos aparentes são apoiadas por arrays internos. O cliente pode acessar `point.X` e `point.Y`, mas esses valores podem estar guardados em `xs[index]` e `ys[index]`.

Fluxo mental:

```text
proxy PointProxy(index)
  X -> xs[index]
  Y -> ys[index]
```

Isso preserva uma API orientada a objetos enquanto a representação interna fica otimizada para armazenamento em lote.

O cuidado é manter a indexação correta. Se o proxy guarda um índice errado ou se os arrays saem de sincronia, a aparência amigável esconde um bug difícil.

---

<a id="sec-012"></a>
## 81. Dynamic Proxy for Logging

[↑ Voltar ao início](#inicio)


Dynamic Proxy cria wrappers em tempo de execução para interceptar chamadas. O exemplo clássico é logging: antes e depois de invocar um método, o proxy registra nome, argumentos, duração ou exceções.

Fluxo:

```text
cliente chama serviço
proxy intercepta
proxy registra entrada
proxy chama serviço real
proxy registra saída
```

Esse padrão aparece em frameworks de interceptação, AOP e containers que conseguem gerar proxies dinamicamente. O ganho é evitar escrever manualmente um decorator de log para cada classe.

O custo é transparência. Se muita coisa acontece por interceptação, o código fonte deixa de contar toda a história.

---

<a id="sec-013"></a>
## 82. Proxy vs. Decorator

[↑ Voltar ao início](#inicio)


Proxy e Decorator podem ter forma parecida: ambos envolvem outro objeto e podem implementar o mesmo contrato. A diferença está na razão da existência.

| Pergunta | Proxy | Decorator |
| --- | --- | --- |
| Por que existe? | Controlar acesso. | Adicionar responsabilidade. |
| O objeto real pode nem existir ainda? | Sim, em lazy proxy. | Normalmente já existe como componente interno. |
| A política é de acesso? | Sim. | Não necessariamente. |
| A camada enriquece o comportamento? | Pode acontecer, mas não é a intenção principal. | Sim, é o centro do padrão. |

Frase curta:

> Proxy fica na porta; Decorator fica em volta.

---

<a id="sec-014"></a>
## 83. ViewModel

[↑ Voltar ao início](#inicio)


ViewModel pode ser lido como um tipo de proxy de apresentação. Ele representa o modelo para a interface do usuário, expondo dados em uma forma mais conveniente para binding, validação, comandos e notificações.

O modelo de domínio não precisa conhecer detalhes de tela. O ViewModel pode adaptar:

- nomes de propriedades;
- estados calculados;
- comandos de UI;
- mensagens de validação;
- notificações de mudança.

O cuidado é não colocar toda a regra de negócio no ViewModel. Ele deve mediar a apresentação, não substituir o domínio.

---

<a id="sec-015"></a>
## Coding Exercise 11: Proxy Coding Exercise

[↑ Voltar ao início](#inicio)


O exercício de Proxy deve deixar claro que existe um objeto representando outro. A solução precisa preservar a interface esperada pelo cliente e inserir uma política de acesso, representação ou controle.

Checklist mental:

- o proxy oferece a mesma superfície pública do objeto real;
- o cliente pode usar o proxy sem conhecer o objeto real diretamente;
- existe uma regra clara antes de encaminhar a chamada;
- o proxy não muda a intenção do serviço principal;
- o controle fica centralizado no representante.

---

<a id="sec-016"></a>
## 84. Bit Fragging

[↑ Voltar ao início](#inicio)


Bit Fragging prepara a ideia de compactar vários estados pequenos em bits. Em vez de guardar muitos booleanos como campos independentes, podemos representar flags em uma estrutura compacta e expor propriedades mais legíveis por cima.

Exemplo mental:

```text
bits:
  0000 0101

propriedades:
  IsEnabled -> bit 0
  IsVisible -> bit 2
```

O proxy aparece na API: o cliente lê propriedades com nomes claros, enquanto a representação interna trabalha com máscaras e operações bit a bit.

O cuidado é didático e prático: compactação melhora memória em certos cenários, mas reduz legibilidade se for usada sem necessidade.

---


<a id="sec-glossario"></a>
## Glossário
[↑ Voltar ao início](#inicio)

- **Subject:** contrato comum ao proxy e ao objeto real.
- **Proxy:** substituto que controla o acesso ao objeto real.
- **Real subject:** objeto que executa o trabalho principal.
- **Lazy loading:** criação ou carregamento do objeto somente quando necessário.
- **Protection proxy:** verifica permissões antes de encaminhar uma operação.
