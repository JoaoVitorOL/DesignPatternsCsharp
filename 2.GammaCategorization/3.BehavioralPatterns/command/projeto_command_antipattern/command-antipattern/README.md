# Projeto: editor com anti-pattern (Command)

Editor de texto simplificado que oferece **macros**: o usuário grava uma sequência
de ações e pode executá-la depois. O código funciona, mas a macro é uma lista de
**strings** interpretada por um `switch` dentro do invocador, que conhece cada
operação e **não** consegue desfazer nada.

O objetivo deste projeto é **rastrear** o problema de design e propor uma solução
usando o padrão **Command**. Não é um projeto pronto: o código funciona, mas o
invocador está acoplado às operações e o pedido não é um objeto.

## Como rodar

O projeto é Maven (Java 17) e abre direto no Eclipse/IntelliJ.

- **Eclipse/IntelliJ:** importe a pasta do projeto e execute a classe `Main`.
- **Linha de comando:**
  ```bash
  mvn compile
  java -cp target/classes br.venson.net.designpatterns.command.Main
  ```

## O cenário

- `Editor`: receiver que altera o texto (`digitar`, `apagar`).
- `Macro`: lista de strings com ações, interpretada por `switch` no `executar()`.
- `Main`: monta uma macro (`DIGITAR`, `DIGITAR`, `APAGAR`) e a executa.

## O que observar

1. A `Macro` **conhece cada operação** e sua sintaxe (`"DIGITAR:..."`).
2. Adicionar uma ação nova (ex.: `SUBSTITUIR`) obriga a **editar o `switch`**.
3. As ações são **strings**: sem tipo, sem validação, sujeitas a erro de formato.
4. **Não há desfazer**: nada guarda o estado anterior de cada passo.
5. Como **enfileirar, agendar ou registrar** uma ação se o pedido não é um objeto?

## Tarefa

Rastreie os problemas de design deste código e proponha uma refatoração com o
padrão **Command**, deixando claros os papéis: a **interface de comando**
(`Comando`, com `executar()` e `desfazer()`), os **comandos concretos**
(`DigitarComando`, `ApagarComando`), o **receiver** (`Editor`) e o **invoker**
(`Macro`).
