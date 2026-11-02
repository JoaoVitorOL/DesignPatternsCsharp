<a id="inicio"></a>
# State — comportamento conforme o estado atual

## Sumário
[↑ Voltar ao início](#inicio)

- [O que o padrão faz](#sec-001)
- [Como implementar](#sec-002)
- [Relação com o projeto](#sec-003)
- [Implementação mínima em C#: catraca](#sec-004)
- [Escolhas de implementação: classes, switch ou biblioteca](#sec-005)
- [Papéis técnicos no exemplo](#sec-006)


**Família GoF:** comportamental. **Escopo predominante:** objetos de contexto e estados concretos.

<a id="sec-001"></a>
## O que o padrão faz

[↑ Voltar ao início](#inicio)

Representa estados relevantes por objetos. O contexto delega as operações ao estado ativo e troca essa referência nas transições. As regras de cada estado deixam de se espalhar por grandes switch repetidos.

<a id="sec-002"></a>
## Como implementar

[↑ Voltar ao início](#inicio)

Defina uma interface State com operações dependentes do estado e crie classes como PortaAberta e PortaFechada. O contexto mantém o estado atual, delega as chamadas e instala o próximo estado quando uma transição válida ocorre. Defina também a resposta para eventos inválidos.

Para uma máquina pequena, enum e switch podem ser mais simples. State compensa quando cada estado tem regras próprias. Stateless é uma biblioteca .NET que permite declarar estados, gatilhos, guardas e transições, mas não está usada no código local.

<a id="sec-003"></a>
## Relação com o projeto

[↑ Voltar ao início](#inicio)

Não há implementação de State entre os exemplos de código das pastas de padrões deste repositório.


<a id="sec-004"></a>
## Implementação mínima em C#: catraca

[↑ Voltar ao início](#inicio)

O exemplo modela uma catraca que começa bloqueada. Inserir moeda libera a passagem; empurrar enquanto está liberada deixa passar e volta a bloquear. Cada estado interpreta as mesmas operações de maneira diferente.

    using System;

    public interface ITurnstileState
    {
        void Coin(Turnstile turnstile);
        void Push(Turnstile turnstile);
    }

    public sealed class Turnstile
    {
        private ITurnstileState state = new LockedState();
        public void SetState(ITurnstileState next) => state = next;
        public void Coin() => state.Coin(this);
        public void Push() => state.Push(this);
    }

    public sealed class LockedState : ITurnstileState
    {
        public void Coin(Turnstile turnstile)
        {
            Console.WriteLine("Moeda recebida; catraca liberada.");
            turnstile.SetState(new UnlockedState());
        }
        public void Push(Turnstile turnstile) =>
            Console.WriteLine("Bloqueada: insira uma moeda.");
    }

    public sealed class UnlockedState : ITurnstileState
    {
        public void Coin(Turnstile turnstile) =>
            Console.WriteLine("Moeda extra devolvida.");

        public void Push(Turnstile turnstile)
        {
            Console.WriteLine("Passagem liberada.");
            turnstile.SetState(new LockedState());
        }
    }

Turnstile não precisa fazer switch para cada combinação de estado e evento. Ele encaminha Coin ou Push ao objeto atual. A transição acontece explicitamente no estado que conhece a regra. Para estados sem dados, as instâncias poderiam ser compartilhadas; se estados guardarem contexto, devem ser criadas conforme necessário.

A alternativa enum + switch continua válida para uma máquina pequena. State compensa quando condicionais se repetem e cada estado ganha comportamento próprio.


<a id="sec-005"></a>
## Escolhas de implementação: classes, switch ou biblioteca

[↑ Voltar ao início](#inicio)

A implementação clássica usa um objeto por estado, como a catraca acima. Uma máquina pequena pode usar enum e switch numa função de transição: isso é simples e torna a tabela visível, desde que os casos permaneçam centralizados. Switch expressions são úteis quando estado/evento retornam um resultado sem muito efeito colateral.

Uma máquina feita à mão pode declarar transições como estado + evento → guarda, ação e próximo estado. A biblioteca Stateless para .NET oferece configuração fluente, gatilhos, guardas e consulta das transições permitidas. Ela automatiza infraestrutura, não substitui a decisão de quais estados são válidos.

Classes de estado são adequadas quando estados têm comportamentos próprios ou muitas operações condicionais. Para dois estados com uma regra pequena, a hierarquia pode ser mais difícil de ler que um switch.

<a id="sec-006"></a>
## Papéis técnicos no exemplo

Os nomes **Context**, **Strategy**, **Concrete Strategy** e semelhantes descrevem responsabilidades no desenho. O código não precisa usar esses nomes literalmente: o mapeamento abaixo identifica quem exerce cada papel no exemplo desta página.

[↑ Voltar ao início](#inicio)

| Papel no padrão | Classe do exemplo | O que representa no código |
|---|---|---|
| Context | **Turnstile** | Mantém o estado atual, encaminha eventos e recebe transições de estado. |
| State | **ITurnstileState** | Declara operações cuja resposta depende da condição da catraca. |
| Concrete States | **LockedState**, **UnlockedState** | Definem a resposta a moeda e passagem, incluindo transições válidas. |
| Cliente | **Código que aciona Turnstile** | Envia eventos sem escolher manualmente a regra de cada estado. |

**Leitura técnica:** o comportamento fica nos objetos de estado, e não apenas em um enum. A alternativa switch discutida na página pode ser adequada quando há poucos estados e pouca variação.

<a id="sec-glossario"></a>
## Glossário
[↑ Voltar ao início](#inicio)

- **Contexto:** objeto cuja resposta varia conforme o estado atual.
- **Estado concreto:** objeto que implementa um comportamento de estado.
- **Transição:** mudança de um estado para outro.
- **Máquina de estados:** modelo de estados possíveis e transições permitidas.
