<a id="inicio"></a>
# Mediator — coordenação de uma sala de chat

## Sumário
[↑ Voltar ao início](#inicio)

- [O que o padrão faz](#sec-001)
- [Implementação corrigida](#sec-002)
- [Por que a sala é um participante importante](#sec-003)
- [Arquivos para acompanhar](#sec-004)
- [Mediator versus Observer e chamadas diretas](#sec-005)
- [Variações: sala de chat, Event Broker e MediatR](#sec-006)
- [Papéis técnicos no exemplo](#sec-007)


**Família GoF:** comportamental. **Escopo predominante:** objetos colaboradores coordenados por um mediador.

<a id="sec-001"></a>
## O que o padrão faz

[↑ Voltar ao início](#inicio)

Centraliza regras de comunicação entre objetos colaboradores. Cada participante comunica intenção ao mediador, que encaminha a solicitação; os participantes deixam de manter referências cruzadas.

<a id="sec-002"></a>
## Implementação corrigida

[↑ Voltar ao início](#inicio)

Em [projeto_mediator_corrigido](projeto_mediator_corrigido), MediadorChat é a abstração, SalaChat mantém usuários e encaminha mensagens, e Usuario conhece somente o mediador recebido no construtor. Entrada e saída também são coordenadas pela sala. Compare com [projeto_mediator_antipattern](projeto_mediator_antipattern/mediator-antipattern).

O exemplo envia mensagens a um destinatário, não faz broadcast. Usuario.enviar recebe remetente, mas o ignora e usa this; o parâmetro é redundante. SalaChat percorre a lista para encontrar o destinatário, e deveria definir o comportamento para usuários que já saíram.

Mediator reduz dependências cruzadas, mas pode virar um objeto central com responsabilidades demais. Mantenha nele as regras de interação entre colegas.


<a id="sec-003"></a>
## Por que a sala é um participante importante

[↑ Voltar ao início](#inicio)

Se cada Usuario mantivesse uma lista de todos os demais e chamasse diretamente cada destinatário, cada usuário dependeria de outros usuários e do protocolo de comunicação. Com muitos participantes, novas regras (moderação, histórico, bloqueio, broadcast) se espalhariam entre os colegas.

SalaChat concentra a topologia e o encaminhamento. Usuario conhece MediadorChat e pede envio; a sala conhece a lista de usuários e decide como localizar o destino. Para alterar a regra de encaminhamento, trabalha-se no mediador, enquanto a classe Usuario permanece focada em enviar e receber mensagens.

A abstração MediadorChat também permite que Usuario seja testado com um substituto simples, sem criar uma sala real. O custo é que a sala pode crescer demais. Uma evolução pode delegar autenticação ou moderação a serviços próprios, mantendo SalaChat como coordenadora em vez de implementadora de todas as políticas.


<a id="sec-004"></a>
## Arquivos para acompanhar

[↑ Voltar ao início](#inicio)

- [MediadorChat.java](projeto_mediator_corrigido/MediadorChat.java) declara as operações de coordenação.
- [SalaChat.java](projeto_mediator_corrigido/SalaChat.java) mantém participantes e encaminha mensagens.
- [Usuario.java](projeto_mediator_corrigido/Usuario.java) depende da abstração do mediador.
- [Main.java](projeto_mediator_corrigido/Main.java) conecta os colaboradores.


<a id="sec-005"></a>
## Mediator versus Observer e chamadas diretas

[↑ Voltar ao início](#inicio)

Na sala de chat, o mediador coordena quem envia a quem e aplica regras da sala. Observer normalmente comunica uma mudança de um sujeito para assinantes, sem necessariamente escolher destinatário ou orquestrar um diálogo entre colegas. Um Mediator pode usar eventos por dentro, mas a intenção principal é coordenar a interação.

Chamadas diretas continuam adequadas quando há um colaborador fixo e uma única relação simples. Inserir mediador em todo relacionamento adiciona indireção sem reduzir acoplamento relevante. O padrão passa a compensar quando surgem muitas referências cruzadas ou regras que precisam de um lugar único.

A sala corrigida local não implementa broadcast, privacidade ou moderação completas; trata uma mensagem direcionada. A documentação deve distinguir a intenção do padrão das funcionalidades que ainda precisariam ser adicionadas ao produto.


<a id="sec-006"></a>
## Variações: sala de chat, Event Broker e MediatR

[↑ Voltar ao início](#inicio)

A sala local é um mediator clássico: colegas falam com o mediador, e ele conhece a política de encaminhamento. Um Event Broker muda o protocolo para publicação/assinatura: produtores publicam eventos e handlers inscritos reagem. Broker favorece fan-out e extensão dinâmica, enquanto a sala tende a coordenar uma interação ou fluxo entre colegas.

MediatR é uma biblioteca .NET que fornece contratos para requests, notifications e behaviors. O request costuma ter um handler responsável por responder; uma notification pode ter vários handlers. A biblioteca fornece despacho e integração com DI, mas não define automaticamente limites transacionais, autorização ou divisão correta dos casos de uso.

Escolha chamadas diretas quando houver poucos colaboradores e fluxo simples. Use mediator quando as referências cruzadas e regras de coordenação estiverem espalhadas. Se o mediador passar a implementar todo o negócio, decomponha-o em serviços coesos.

<a id="sec-007"></a>
## Papéis técnicos no exemplo

Os nomes **Context**, **Strategy**, **Concrete Strategy** e semelhantes descrevem responsabilidades no desenho. O código não precisa usar esses nomes literalmente: o mapeamento abaixo identifica quem exerce cada papel no exemplo desta página.

[↑ Voltar ao início](#inicio)

| Papel no padrão | Classe do exemplo | O que representa no código |
|---|---|---|
| Mediator | **MediadorChat** | Declara como participantes entram na sala e encaminham mensagens. |
| Concrete Mediator | **SalaChat** | Mantém os participantes e coordena a distribuição de mensagens. |
| Colleague | **Usuario** | Envia mensagens ao mediador em vez de manter referências diretas para todos os usuários. |
| Cliente/composição | **Main** | Cria a sala, registra usuários e inicia as interações. |

**Leitura técnica:** SalaChat conhece os participantes para coordená-los; cada Usuario conhece o mediador. O padrão remove a malha de dependências diretas, mas não elimina toda comunicação.

<a id="sec-glossario"></a>
## Glossário
[↑ Voltar ao início](#inicio)

- **Mediator:** objeto que coordena a comunicação entre participantes.
- **Colleague:** participante que comunica necessidades ao mediador.
- **Acoplamento:** grau de dependência direta entre componentes.
- **Event broker:** intermediário que distribui eventos aos inscritos.
