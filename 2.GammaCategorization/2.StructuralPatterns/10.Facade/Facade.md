<a id="inicio"></a>
# Facade — uma entrada simples para um subsistema

## Sumário
[↑ Voltar ao início](#inicio)

- [O que o padrão faz](#sec-001)
- [Como implementar](#sec-002)
- [Relação com este repositório](#sec-003)
- [Implementação mínima em C#: finalizar uma compra](#sec-004)
- [Facade não é um “God Object”](#sec-005)
- [Variações e aprofundamentos](#sec-006)
- [69. Facade](#sec-007)
- [Coding Exercise 9: Facade Coding Exercise](#sec-008)
- [Papéis técnicos no exemplo](#sec-009)


**Família GoF:** estrutural. **Escopo predominante:** objetos de subsistema coordenados por uma API de alto nível.

<a id="sec-001"></a>
## O que o padrão faz

[↑ Voltar ao início](#inicio)

Facade oferece uma API pequena por cima de várias classes que precisam colaborar. O subsistema continua existindo; a fachada coordena um caso de uso comum para que cada cliente não precise repetir a coreografia.

<a id="sec-002"></a>
## Como implementar

[↑ Voltar ao início](#inicio)

Identifique passos que vários clientes sempre executam juntos, encapsule-os em uma classe orientada ao caso de uso e mantenha os métodos públicos estreitos. Por exemplo, MortgageFacade pode consultar banco, histórico e crédito e devolver uma resposta única de aprovação. Ela delega o trabalho aos serviços internos em vez de reimplementar suas responsabilidades.

    var aprovado = mortgageFacade.AvaliarSolicitacao(cliente);

A fachada reduz acoplamento do cliente, mas pode virar God Object se agregar operações desconexas. Evite expor os detalhes do subsistema na API pública.

<a id="sec-003"></a>
## Relação com este repositório

[↑ Voltar ao início](#inicio)

Não há arquivo de código Facade nas pastas de padrões. O exemplo de compra desta página é independente e demonstra como a fachada coordena serviços sem substituir as responsabilidades deles.


<a id="sec-004"></a>
## Implementação mínima em C#: finalizar uma compra

[↑ Voltar ao início](#inicio)

A fachada coordena um caso de uso completo. Os serviços internos mantêm suas responsabilidades individuais; PurchaseFacade define em que ordem chamar estoque, pagamento e entrega.

    using System;

    public sealed class Inventory
    {
        public bool HasStock(string sku, int quantity) => true;
        public void Reserve(string sku, int quantity) { }
    }

    public sealed class Payment
    {
        public bool Charge(string customer, decimal amount) => true;
    }

    public sealed class Shipping
    {
        public string Schedule(string customer, string sku, int quantity) => "TRK-123";
    }

    public sealed class PurchaseFacade
    {
        private readonly Inventory inventory;
        private readonly Payment payment;
        private readonly Shipping shipping;

        public PurchaseFacade(Inventory inventory, Payment payment, Shipping shipping)
        {
            this.inventory = inventory;
            this.payment = payment;
            this.shipping = shipping;
        }

        public string PlaceOrder(string customer, string sku, int quantity, decimal price)
        {
            if (!inventory.HasStock(sku, quantity))
                throw new InvalidOperationException("Sem estoque.");
            if (!payment.Charge(customer, price * quantity))
                throw new InvalidOperationException("Pagamento recusado.");

            inventory.Reserve(sku, quantity);
            return shipping.Schedule(customer, sku, quantity);
        }
    }

O cliente chama PlaceOrder sem conhecer a sequência de verificações e integrações. A ordem é parte do caso de uso: não se reserva nem envia se o pagamento falhar. Em sistemas reais, estoque e pagamento podem exigir transações ou compensações; Facade organiza chamadas, mas não cria atomicidade automaticamente.

A fachada deve expressar uma operação coesa. Se começar a aceitar dezenas de opções de vários subsistemas, divida-a em fachadas por caso de uso.


<a id="sec-005"></a>
## Facade não é um “God Object”

[↑ Voltar ao início](#inicio)

Uma fachada deve coordenar um caso de uso coeso. No exemplo, PlaceOrder define uma sequência de negócio e devolve um identificador de entrega; o cliente não precisa chamar estoque, pagamento e envio separadamente. A fachada não deveria implementar cálculo de estoque, gateway de pagamento e política de entrega dentro da mesma classe: ela delega a cada serviço.

A ordem mostra tanto o benefício quanto o limite. A compra verifica estoque, cobra, reserva e agenda entrega. Se uma etapa posterior falhar depois da cobrança, Facade não desfaz automaticamente o pagamento. Essa consistência exige transação, idempotência ou operação compensatória no domínio. A fachada reduz o conhecimento do cliente, mas não substitui desenho transacional.

Use Facade para oferecer uma entrada de alto nível por caso de uso, mantendo as APIs internas disponíveis quando outros clientes precisarem delas.

<a id="sec-006"></a>
## Variações e aprofundamentos

[↑ Voltar ao início](#inicio)

As seções abaixo reúnem variações, exemplos e decisões que complementam a explicação principal deste padrão. Elas preservam a linguagem e o contexto do material didático original.

<a id="sec-007"></a>
## 69. Facade

[↑ Voltar ao início](#inicio)


Uma Facade bem desenhada representa um fluxo de uso, não apenas uma classe grande com atalhos para tudo. Ela deve esconder a coreografia de objetos internos e expor operações com vocabulário do domínio.

Exemplo mental:

```text
MortgageFacade
  checa banco
  checa histórico
  checa crédito
  devolve aprovação simples
```

O cliente não precisa saber quantos sistemas participaram da decisão. Ele só precisa de uma resposta coerente.

O cuidado é não transformar Facade em um objeto que sabe fazer qualquer coisa. Se a classe vira um menu gigante com dezenas de métodos desconexos, ela se aproxima de um God Object. Uma boa Facade é estreita, focada e alinhada a um caso de uso.

Diferença importante:

| Padrão | Intenção |
| --- | --- |
| Adapter | Tornar uma interface compatível com outra. |
| Decorator | Adicionar responsabilidade mantendo o contrato. |
| Proxy | Controlar acesso a outro objeto. |
| Facade | Simplificar acesso a um subsistema. |

---

<a id="sec-008"></a>
## Coding Exercise 9: Facade Coding Exercise

[↑ Voltar ao início](#inicio)


O exercício de Facade deve treinar a criação de uma API pequena por cima de várias peças internas. A solução ideal não apaga as classes do subsistema; ela coordena essas classes em um método que o cliente consegue usar sem conhecer a mecânica.

Checklist mental:

- identifique as classes que o cliente não deveria orquestrar diretamente;
- crie uma classe de fachada com nome orientado ao caso de uso;
- exponha poucos métodos públicos;
- mantenha validações e passos internos em ordem clara;
- não vaze detalhes internos pela assinatura pública.

Se o cliente continua precisando montar metade do subsistema antes de chamar a Facade, a fachada ainda está fina demais.

---

<a id="sec-009"></a>
## Papéis técnicos no exemplo

Os nomes **Context**, **Strategy**, **Concrete Strategy** e semelhantes descrevem responsabilidades no desenho. O código não precisa usar esses nomes literalmente: o mapeamento abaixo identifica quem exerce cada papel no exemplo desta página.

[↑ Voltar ao início](#inicio)

| Papel no padrão | Classe do exemplo | O que representa no código |
|---|---|---|
| Facade | **PurchaseFacade** | Expõe a operação de finalizar compra e coordena as chamadas internas necessárias. |
| Subsistemas | **Inventory**, **Payment**, **Shipping** | Executam responsabilidades específicas de estoque, pagamento e envio. |
| Cliente | **Código chamador de PurchaseFacade** | Solicita a operação de alto nível sem coordenar os subsistemas diretamente. |

**Leitura técnica:** a fachada organiza o fluxo, mas não deve absorver as regras internas dos subsistemas nem virar um objeto que acumula responsabilidades sem limite.

<a id="sec-glossario"></a>
## Glossário
[↑ Voltar ao início](#inicio)

- **Facade (fachada):** interface simplificada para operações de um subsistema.
- **Subsistema:** componentes internos que colaboram em uma tarefa.
- **Orquestração:** coordenação das chamadas necessárias para concluir uma operação.
