<a id="inicio"></a>
# Strategy — políticas de cliente em pedidos

## Sumário
[↑ Voltar ao início](#inicio)

- [O que o padrão faz](#sec-001)
- [Implementação corrigida](#sec-002)
- [Como a troca de regra muda o resultado](#sec-003)
- [Arquivos para acompanhar](#sec-004)
- [Strategy, switch e State: escolha pelo tipo de variação](#sec-005)
- [Estratégia dinâmica, fixa e de comparação](#sec-006)
- [Papéis técnicos no exemplo](#sec-007)


**Família GoF:** comportamental. **Escopo predominante:** objetos de contexto e algoritmos intercambiáveis.

<a id="sec-001"></a>
## O que o padrão faz

[↑ Voltar ao início](#inicio)

Encapsula algoritmos alternativos sob um contrato comum. O contexto delega a regra variável à estratégia escolhida e pode trocar a implementação sem alterar o fluxo principal.

<a id="sec-002"></a>
## Implementação corrigida

[↑ Voltar ao início](#inicio)

Em [projeto_strategy_corrigido](projeto_strategy_corrigido), IUsuario declara desconto, frete e etiqueta. UsuarioComum, UsuarioVIP e UsuarioCorporativo fornecem as regras. RelatorioPedido recebe IUsuario no construtor, delega os cálculos e forma o total sem switch por perfil.

Main troca a estratégia no mesmo relatório: comum, VIP e corporativo. Para adicionar PARCEIRO, crie uma implementação de IUsuario e selecione-a na composição do sistema.

A interface agrupa três políticas porque variam juntas por perfil. Se precisarem variar independentemente, contratos menores evitam combinações artificiais. O método setUsuario deve validar null em produção.


<a id="sec-003"></a>
## Como a troca de regra muda o resultado

[↑ Voltar ao início](#inicio)

RelatorioPedido precisa montar o relatório sempre da mesma forma, mas desconto e frete dependem do perfil do cliente. Se o relatório contivesse um switch para Comum/VIP/Corporativo, cada novo perfil exigiria editar esse método e possivelmente outras classes. A interface IUsuario extrai essa variação para estratégias concretas.

O fluxo é: Main escolhe a estratégia → injeta-a em RelatorioPedido → formatar pede etiqueta, desconto e frete à estratégia → o relatório combina valores para calcular o total. A estratégia não precisa conhecer como o relatório imprime sua saída, e o relatório não precisa saber as taxas concretas. A troca por setUsuario mostra que o mesmo objeto de contexto pode usar uma política diferente depois.

Esse arranjo é especialmente útil quando as variantes são selecionadas por configuração, usuário ou contexto de execução. Se só existir uma regra estável e nenhuma variação for prevista, Strategy acrescenta tipos sem benefício. O ponto não é eliminar todo if do sistema, mas isolar o algoritmo que realmente varia.


<a id="sec-004"></a>
## Arquivos para acompanhar

[↑ Voltar ao início](#inicio)

- [IUsuario.java](projeto_strategy_corrigido/IUsuario.java) declara as políticas.
- [UsuarioComum.java](projeto_strategy_corrigido/UsuarioComum.java), [UsuarioVIP.java](projeto_strategy_corrigido/UsuarioVIP.java) e [UsuarioCorporativo.java](projeto_strategy_corrigido/UsuarioCorporativo.java) implementam regras por perfil.
- [RelatorioPedido.java](projeto_strategy_corrigido/RelatorioPedido.java) delega à estratégia atual.
- [Main.java](projeto_strategy_corrigido/Main.java) demonstra a troca em runtime; [Pedido.java](projeto_strategy_corrigido/Pedido.java) fornece os dados de entrada.


<a id="sec-005"></a>
## Strategy, switch e State: escolha pelo tipo de variação

[↑ Voltar ao início](#inicio)

Strategy é apropriado quando há algoritmos alternativos para a mesma tarefa e a escolha vem de fora do objeto: modalidade de frete, perfil de preço ou formato de exportação. O contexto mantém a sequência comum e delega o cálculo variável. Um switch pequeno e local ainda pode ser a escolha mais clara; Strategy fica mais valioso quando variantes crescem, são testadas isoladamente ou são compostas/injetadas.

State também troca um objeto por outro, mas a razão é diferente: o comportamento atual depende do estado interno e as transições fazem parte da máquina (por exemplo, rascunho → enviado → cancelado). Em Strategy, normalmente o cliente escolhe a política; em State, o objeto/transição determina o estado ativo.

No exemplo de pedidos, RelatorioPedido é contexto e IUsuario é a estratégia. Os métodos atualmente agrupam desconto, frete e etiqueta por perfil. Se o negócio passar a escolher frete e desconto independentemente, divida as estratégias em contratos menores. A fronteira deve seguir o que varia de verdade, não apenas criar uma classe para cada switch.


<a id="sec-006"></a>
## Estratégia dinâmica, fixa e de comparação

[↑ Voltar ao início](#inicio)

No exemplo, setUsuario troca a política durante a vida do mesmo RelatorioPedido: isso é estratégia dinâmica. Uma estratégia pode também ser recebida no construtor e permanecer fixa; um campo readonly torna essa escolha explícita e simplifica concorrência e raciocínio.

A seleção pode vir do usuário, de configuração ou de uma fábrica/DI. Não coloque a seleção e todo algoritmo num factory; a fábrica escolhe a estratégia, enquanto o contexto executa a tarefa comum e delega o cálculo.

Comparadores são um uso cotidiano da mesma ideia: IComparer<T> define uma regra de ordenação que pode mudar por tela ou relatório. Mantenha igualdade/comparação estáveis quando objetos estiverem em HashSet ou como chaves de Dictionary; mudar critérios após inserção pode quebrar a busca.

<a id="sec-007"></a>
## Papéis técnicos no exemplo

Os nomes **Context**, **Strategy**, **Concrete Strategy** e semelhantes descrevem responsabilidades no desenho. O código não precisa usar esses nomes literalmente: o mapeamento abaixo identifica quem exerce cada papel no exemplo desta página.

[↑ Voltar ao início](#inicio)

| Papel no padrão | Classe do exemplo | O que representa no código |
|---|---|---|
| Strategy | **IUsuario** | Contrato para as regras variáveis de desconto, frete e identificação do perfil. |
| Concrete Strategies | **UsuarioComum**, **UsuarioVIP**, **UsuarioCorporativo** | Implementam as políticas específicas de cada perfil. |
| Context | **RelatorioPedido** | Recebe IUsuario, delega os cálculos variáveis e monta o resultado do pedido. |
| Dados de domínio | **Pedido** | Guarda dados da compra; não é Strategy nem Context. |
| Cliente/composição | **Main** | Escolhe a estratégia e a fornece ao RelatorioPedido. |

**Leitura técnica:** o nome Context não aparece literalmente no código. RelatorioPedido exerce esse papel por manter o fluxo estável e delegar a parte variável a IUsuario. A estratégia pode ser substituída sem acrescentar um switch por perfil.

<a id="sec-glossario"></a>
## Glossário
[↑ Voltar ao início](#inicio)

- **Strategy:** contrato para uma família de algoritmos intercambiáveis.
- **Estratégia concreta:** implementação de um algoritmo específico.
- **Contexto:** componente que usa uma estratégia sem conhecer os detalhes.
- **Composição:** forma de fornecer uma estratégia ao contexto, por exemplo pelo construtor.
