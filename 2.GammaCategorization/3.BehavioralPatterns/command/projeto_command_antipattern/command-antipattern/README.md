<a id="inicio"></a>
# Projeto: editor com anti-pattern (Command)

## Sumário
[↑ Voltar ao início](#inicio)

- [Como rodar](#sec-001)
- [O cenário](#sec-002)
- [O que observar](#sec-003)
- [Tarefa](#sec-004)


Editor de texto simplificado que oferece **macros**: o usuário grava uma sequência
de ações e pode executá-la depois. O código funciona, mas a macro é uma lista de
**strings** interpretada por um `switch` dentro do invocador, que conhece cada
operação e **não** consegue desfazer nada.

O objetivo deste projeto é **rastrear** o problema de design e propor uma solução
usando o padrão **Command**. Não é um projeto pronto: o código funciona, mas o
invocador está acoplado às operações e o pedido não é um objeto.

<a id="sec-001"></a>
## Como rodar

[↑ Voltar ao início](#inicio)

O projeto é Maven (Java 17) e abre direto no Eclipse/IntelliJ.

- **Eclipse/IntelliJ:** importe a pasta do projeto e execute a classe `Main`.
- **Linha de comando:**
  ```bash
  mvn compile
  java -cp target/classes br.venson.net.designpatterns.command.Main
  ```

<a id="sec-002"></a>
## O cenário

[↑ Voltar ao início](#inicio)

- `Editor`: receiver que altera o texto (`digitar`, `apagar`).
- `Macro`: lista de strings com ações, interpretada por `switch` no `executar()`.
- `Main`: monta uma macro (`DIGITAR`, `DIGITAR`, `APAGAR`) e a executa.

<a id="sec-003"></a>
## O que observar

[↑ Voltar ao início](#inicio)

1. A `Macro` **conhece cada operação** e sua sintaxe (`"DIGITAR:..."`).
2. Adicionar uma ação nova (ex.: `SUBSTITUIR`) obriga a **editar o `switch`**.
3. As ações são **strings**: sem tipo, sem validação, sujeitas a erro de formato.
4. **Não há desfazer**: nada guarda o estado anterior de cada passo.
5. Como **enfileirar, agendar ou registrar** uma ação se o pedido não é um objeto?

<a id="sec-004"></a>
## Tarefa

[↑ Voltar ao início](#inicio)

Rastreie os problemas de design deste código e proponha uma refatoração com o
padrão **Command**, deixando claros os papéis: a **interface de comando**
(`Comando`, com `executar()` e `desfazer()`), os **comandos concretos**
(`DigitarComando`, `ApagarComando`), o **receiver** (`Editor`) e o **invoker**
(`Macro`).


<a id="sec-glossario"></a>
## Glossário
[↑ Voltar ao início](#inicio)

- **Command:** objeto que representa uma ação e seus dados.
- **Invoker:** componente que solicita a execução de um comando.
- **Receiver:** objeto que realiza o trabalho solicitado.
- **Undo:** operação que tenta reverter o efeito de um comando.
- **Macro command:** comando composto que executa vários comandos.
