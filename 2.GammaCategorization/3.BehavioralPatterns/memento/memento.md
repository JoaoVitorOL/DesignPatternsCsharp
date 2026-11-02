<a id="inicio"></a>
# Memento — checkpoints de um jogo

## Sumário
[↑ Voltar ao início](#inicio)

- [O que o padrão faz](#sec-001)
- [Implementação corrigida](#sec-002)
- [Por que o caretaker não deve ler os campos](#sec-003)
- [Arquivos para acompanhar](#sec-004)
- [Variações: undo/redo e snapshots persistidos](#sec-005)
- [Papéis técnicos no exemplo](#sec-006)


**Família GoF:** comportamental. **Escopo predominante:** objetos que criam, guardam e restauram snapshots.

<a id="sec-001"></a>
## O que o padrão faz

[↑ Voltar ao início](#inicio)

Captura um snapshot do estado para restaurá-lo depois sem expor os campos internos. O originator cria e restaura o snapshot; o caretaker o armazena sem interpretar seu conteúdo.

<a id="sec-002"></a>
## Implementação corrigida

[↑ Voltar ao início](#inicio)

Em [projeto_memento_corrigido](projeto_memento_corrigido), Jogo é o originator: conhece fase, vidas, pontuação e posição e implementa salvar()/restaurar(). Jogo.Checkpoint é a interface opaca para o gerenciador; CheckpointImpl armazena os dados e é privada à classe.

GerenciadorSave é o caretaker: armazena checkpoints e pede ao jogo para restaurar um deles. Main salva na Fase 1, altera o jogo e restaura. Compare com [projeto_memento_antipattern](projeto_memento_antipattern/memento-antipattern), onde o gerenciador dependia dos detalhes internos.

Os valores do exemplo são primitivos e String. Se o estado incluir objetos mutáveis, faça cópia profunda; caso contrário o passado pode mudar junto com o presente. Redo exigiria uma segunda pilha.


<a id="sec-003"></a>
## Por que o caretaker não deve ler os campos

[↑ Voltar ao início](#inicio)

Quando GerenciadorSave recebe apenas Jogo.Checkpoint, ele pode guardar e devolver o snapshot, mas não pode inspecionar vidas ou pontuação. Isso preserva uma fronteira importante: somente Jogo conhece a forma completa do próprio estado e pode garantir que o snapshot contém tudo que a restauração precisa.

A sequência em Main torna a finalidade concreta: salvar antes de uma mudança arriscada, avançar o estado e recuperar a situação anterior. O checkpoint é um valor imutável, então as alterações posteriores no jogo não reescrevem o passado. Esse encapsulamento também permite que a implementação dos campos mude sem obrigar o caretaker a mudar.

O exemplo usa uma lista e retira o checkpoint na restauração, portanto demonstra um undo simples, não histórico ilimitado nem redo. Para vários passos, limites de retenção evitam crescimento infinito de memória. Para snapshots persistidos, versionamento e compatibilidade tornam-se parte do design.


<a id="sec-004"></a>
## Arquivos para acompanhar

[↑ Voltar ao início](#inicio)

- [Jogo.java](projeto_memento_corrigido/Jogo.java) cria e restaura o checkpoint opaco.
- [GerenciadorSave.java](projeto_memento_corrigido/GerenciadorSave.java) mantém o histórico sem acessar os campos do jogo.
- [Main.java](projeto_memento_corrigido/Main.java) demonstra salvar, alterar e restaurar.


<a id="sec-005"></a>
## Variações: undo/redo e snapshots persistidos

[↑ Voltar ao início](#inicio)

Para undo, mantenha snapshots anteriores numa pilha. Ao restaurar, guarde o estado atual numa pilha de redo; ao executar uma nova edição, limpe o redo, pois o futuro antigo já não corresponde ao novo histórico. O exemplo local remove o checkpoint restaurado e demonstra apenas um passo de restauração, não redo completo.

Guardar um Memento em disco ou enviá-lo a outro processo muda sua natureza: passa a ser um formato versionado. Deve-se validar origem, versão, permissões e compatibilidade antes de restaurar. O Checkpoint vazio do exercício é opaco em memória e não define serialização.

Snapshots completos são simples de restaurar, mas podem ser caros. Sistemas de edição podem guardar deltas, snapshots periódicos ou combinar ambos. O originator continua sendo responsável por interpretar esses dados e preservar invariantes.

<a id="sec-006"></a>
## Papéis técnicos no exemplo

Os nomes **Context**, **Strategy**, **Concrete Strategy** e semelhantes descrevem responsabilidades no desenho. O código não precisa usar esses nomes literalmente: o mapeamento abaixo identifica quem exerce cada papel no exemplo desta página.

[↑ Voltar ao início](#inicio)

| Papel no padrão | Classe do exemplo | O que representa no código |
|---|---|---|
| Originator | **Jogo** | É dono do estado; cria checkpoints e restaura-se a partir deles. |
| Memento | **Jogo.Checkpoint**, implementado por **CheckpointImpl** | Captura os campos do jogo. A interface exposta é opaca e a implementação permanece privada. |
| Caretaker | **GerenciadorSave** | Guarda checkpoints sem ler ou editar seus dados internos. |
| Cliente | **Main** | Solicita um save, altera o estado e pede sua restauração. |

**Leitura técnica:** somente Jogo interpreta CheckpointImpl. GerenciadorSave manipula Checkpoint como um registro opaco, preservando o encapsulamento do Originator.

<a id="sec-glossario"></a>
## Glossário
[↑ Voltar ao início](#inicio)

- **Originator:** objeto cujo estado será salvo e restaurado.
- **Memento:** registro de um estado anterior do originator.
- **Caretaker:** guarda mementos sem interpretar seu conteúdo.
- **Snapshot:** cópia do estado em um instante específico.
- **Undo/redo:** navegação entre estados anteriores e posteriores.
