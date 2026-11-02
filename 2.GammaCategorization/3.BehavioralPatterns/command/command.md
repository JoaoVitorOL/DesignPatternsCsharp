<a id="inicio"></a>
# Command — editor com operações desfazíveis

## Sumário
[↑ Voltar ao início](#inicio)

- [O que o padrão faz](#sec-001)
- [Implementação corrigida](#sec-002)
- [Acompanhe a macro do editor](#sec-003)
- [Arquivos para acompanhar](#sec-004)
- [Command para undo, filas e auditoria](#sec-005)
- [Variações: histórico, fila e Composite Command](#sec-006)


**Família GoF:** comportamental. **Escopo predominante:** objetos que representam solicitações.

<a id="sec-001"></a>
## O que o padrão faz

[↑ Voltar ao início](#inicio)

Transforma uma solicitação em objeto, separando quem pede a operação de quem a executa. O objeto comando pode guardar parâmetros, ser armazenado, agrupado ou desfeito.

<a id="sec-002"></a>
## Implementação corrigida

[↑ Voltar ao início](#inicio)

Em [projeto_command_corrigidos](projeto_command_corrigidos), Comando define executar() e desfazer(); Editor é o receptor. DigitarComando guarda o texto a inserir e desfaz removendo essa quantidade de caracteres. ApagarComando guarda quantidade e captura o trecho removido em textoRemovido para restaurá-lo.

Macro implementa Comando, executa vários comandos na ordem e os desfaz na ordem inversa (LIFO). Main monta uma macro e demonstra execução e undo. Compare com [projeto_command_antipattern](projeto_command_antipattern/command-antipattern), que mostra o desenho anterior.

O exemplo supõe que apagar remove do fim do texto. Uma exclusão em qualquer posição teria que guardar posição e conteúdo. Macro também não trata falha parcial; uma versão de produção precisa definir rollback.


<a id="sec-003"></a>
## Acompanhe a macro do editor

[↑ Voltar ao início](#inicio)

Sem Command, uma interface de editor poderia executar ações diretamente e depois não teria uma representação clara do que ocorreu. Comandos guardam tanto a intenção quanto os dados: DigitarComando sabe qual texto inserir; ApagarComando sabe quantos caracteres remover e, após executar, qual texto precisa recuperar.

O Editor é o receptor porque conhece a estrutura StringBuilder e sabe alterar o texto. Macro é quem dispara os comandos, sem precisar conhecer o funcionamento interno de cada ação. Essa separação é a razão de o invocador poder armazenar uma lista de Comando e executar uma sequência heterogênea.

Undo mostra por que comandos podem precisar de estado próprio. Para apagar, a operação inversa não pode ser calculada só a partir da quantidade apagada; é necessário guardar o trecho real. A ordem inversa da macro é essencial: primeiro se desfaz a última mudança, depois as anteriores. O padrão habilita undo, mas não torna qualquer operação reversível automaticamente.


<a id="sec-004"></a>
## Arquivos para acompanhar

[↑ Voltar ao início](#inicio)

- [Comando.java](projeto_command_corrigidos/Comando.java) define executar/desfazer.
- [Editor.java](projeto_command_corrigidos/Editor.java) é o receptor que altera o texto.
- [DigitarComando.java](projeto_command_corrigidos/DigitarComando.java) e [ApagarComando.java](projeto_command_corrigidos/ApagarComando.java) encapsulam ações e dados para undo.
- [Macro.java](projeto_command_corrigidos/Macro.java) executa a sequência e desfaz em ordem inversa; [Main.java](projeto_command_corrigidos/Main.java) mostra o uso.


<a id="sec-005"></a>
## Command para undo, filas e auditoria

[↑ Voltar ao início](#inicio)

O mesmo formato de comando pode servir a finalidades diferentes. Para undo, cada comando precisa guardar informação para reconstruir o estado anterior. Para fila, precisa ser serializável ou transformar-se num DTO com identificadores estáveis; uma referência direta ao Editor em memória não pode ser enviada sem mais a outro processo. Para auditoria, registre quem solicitou, quando, resultado e correlação sem expor dados sensíveis.

No exemplo, Macro mantém objetos Comando e os executa no processo atual. O método desfazer é específico ao editor; não significa que todos os commands de um sistema devam ter undo. Para ações externas, como enviar e-mail, pode não existir inversa; pode ser possível executar uma compensação de negócio, mas ela não garante retorno ao estado exato.

Se um comando for repetido por retry, pense em idempotência. Se executar duas vezes cobra duas vezes, o invoker precisa de chave de idempotência ou controle de estado. Encapsular a chamada facilita aplicar essas políticas, mas não as cria automaticamente.


<a id="sec-006"></a>
## Variações: histórico, fila e Composite Command

[↑ Voltar ao início](#inicio)

O exemplo local usa comandos síncronos em memória e uma Macro que compõe comandos. Outras aplicações podem colocar comandos numa fila, gravá-los para auditoria ou serializá-los para execução posterior. Nesses casos, evite guardar uma referência a um objeto em memória como Editor; transporte identificadores e dados imutáveis, e resolva o receptor no momento da execução.

Undo pode ser implementado por uma operação inversa (digitar → apagar) ou por snapshot do estado anterior (Memento). O primeiro costuma ser menor, mas exige que a inversa seja correta; snapshot facilita restauração, mas pode custar memória. Nem todo comando é reversível: pagamento externo, envio de e-mail e entrega podem admitir compensação de negócio, mas não um retorno exato.

Uma Macro é Composite Command: o cliente executa uma unidade que contém outras. Para desfazer, percorre os filhos em ordem inversa. Se a execução falhar no meio, deve-se definir se há rollback dos comandos concluídos ou se o estado parcial é registrado.


<a id="sec-glossario"></a>
## Glossário
[↑ Voltar ao início](#inicio)

- **Command:** objeto que representa uma ação e seus dados.
- **Invoker:** componente que solicita a execução de um comando.
- **Receiver:** objeto que realiza o trabalho solicitado.
- **Undo:** operação que tenta reverter o efeito de um comando.
- **Macro command:** comando composto que executa vários comandos.
