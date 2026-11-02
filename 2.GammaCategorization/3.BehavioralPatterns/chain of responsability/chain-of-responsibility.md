<a id="inicio"></a>
# Chain of Responsibility — validação de magia

## Sumário
[↑ Voltar ao início](#inicio)

- [O que o padrão faz](#sec-001)
- [Implementação no projeto](#sec-002)
- [Aplicando a ideia ao fluxo de uma magia](#sec-003)
- [Arquivos para acompanhar](#sec-004)
- [Variações: cadeia ligada e broker](#sec-005)


**Família GoF:** comportamental. **Escopo predominante:** objetos ligados em uma sequência de tratadores.

<a id="sec-001"></a>
## O que o padrão faz

[↑ Voltar ao início](#inicio)

Organiza uma solicitação em uma sequência de handlers. Cada um aplica uma regra e decide se rejeita ou encaminha ao próximo. O cliente inicia a cadeia sem conhecer todas as regras.

<a id="sec-002"></a>
## Implementação no projeto

[↑ Voltar ao início](#inicio)

Em [projeto_chainOfResponsability](projeto_chainOfResponsability), ValidadorMagia define proximo(...) e validar(ContextoMagia). ValidadorStatus, ValidadorMana, ValidadorCooldown e ValidadorAlcance verificam condições independentes. Main monta Status → Mana → Cooldown → Alcance.

Se uma regra falha, retorna false sem delegar; se passa, chama o próximo validador. O último elo retorna sucesso. ContextoMagia reúne jogador, magia, custo, cooldown e distância. O chamador consome mana apenas depois da cadeia inteira aprovar, evitando efeitos parciais.

    ValidadorMagia cadeia = new ValidadorStatus();
    cadeia.proximo(new ValidadorMana())
          .proximo(new ValidadorCooldown())
          .proximo(new ValidadorAlcance());
    boolean podeLancar = cadeia.validar(contexto);

A ordem determina qual falha aparece primeiro. Se for necessário exibir todas as falhas, acumule resultados em vez de interromper. O exemplo implementa handlers ligados diretamente, não a variação baseada em broker.


<a id="sec-003"></a>
## Aplicando a ideia ao fluxo de uma magia

[↑ Voltar ao início](#inicio)

Uma magia só pode ser lançada se várias condições forem verdadeiras. Colocar todas as verificações num único método faria crescer uma sequência de ifs e misturaria regras diferentes. No exemplo, cada Validador representa uma pergunta de negócio: o personagem está vivo e sem stun? Tem mana? O cooldown terminou? O alvo está dentro do alcance?

A cadeia muda a organização, não a necessidade dessas regras. Cada classe continua responsável por sua condição, enquanto o encadeamento define a ordem e o fluxo de interrupção. Isso facilita incluir, remover ou reorganizar validadores sem fazer um único método conhecer todas as políticas. Também permite testar cada regra isoladamente.

O código não gasta mana dentro dos validadores. Essa escolha torna as validações consultas e deixa a alteração de estado para depois do sucesso completo. Se um validador falhar, o chamador não precisa desfazer mana já consumida. Para ensinar o padrão, esse detalhe evidencia que encadear handlers não significa que eles devam todos modificar a solicitação.


<a id="sec-004"></a>
## Arquivos para acompanhar

[↑ Voltar ao início](#inicio)

- [Main.java](projeto_chainOfResponsability/Main.java) monta e executa a cadeia.
- [ValidadorMagia.java](projeto_chainOfResponsability/ValidadorMagia.java) define o contrato.
- [ValidadorStatus.java](projeto_chainOfResponsability/ValidadorStatus.java), [ValidadorMana.java](projeto_chainOfResponsability/ValidadorMana.java), [ValidadorCooldown.java](projeto_chainOfResponsability/ValidadorCooldown.java) e [ValidadorAlcance.java](projeto_chainOfResponsability/ValidadorAlcance.java) mostram os elos concretos.
- [ContextoMagia.java](projeto_chainOfResponsability/ContextoMagia.java) transporta a solicitação.


<a id="sec-005"></a>
## Variações: cadeia ligada e broker

[↑ Voltar ao início](#inicio)

No exemplo local, cada handler guarda diretamente o próximo; é a Method Chain. A ordem é explícita no Main, cada etapa pode interromper e o chamador sabe que a resposta final é booleana. Essa forma é fácil de seguir quando a sequência é curta e a ordem faz parte da regra.

Uma variação com broker deixa cada handler se registrar num objeto central. O broker publica uma consulta e combina respostas, sem os handlers manterem ponteiros uns para os outros. Essa forma reduz acoplamento da topologia e permite inscrição dinâmica, mas torna prioridade e ordem de execução menos visíveis. Um event broker dessa natureza pode se parecer com Mediator; a intenção continua sendo distribuir uma solicitação a possíveis tratadores.

CQS (Command Query Separation) é um princípio que ajuda a estruturar essas cadeias: validações consultam condições; mutações, como gastar mana, ocorrem depois que a decisão foi tomada. Evite handlers que alteram estado e depois passam a solicitação adiante sem um contrato de rollback.


<a id="sec-glossario"></a>
## Glossário
[↑ Voltar ao início](#inicio)

- **Handler:** componente que trata uma solicitação ou a encaminha.
- **Cadeia:** sequência de manipuladores percorrida pela solicitação.
- **Encaminhamento:** passagem da solicitação ao próximo manipulador.
- **Broker:** componente que distribui eventos ou comandos aos inscritos.
