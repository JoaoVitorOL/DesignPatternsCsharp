<a id="inicio"></a>
# Projeto: jogo com anti-pattern (Memento)

## Sumário
[↑ Voltar ao início](#inicio)

- [Como rodar](#sec-001)
- [O cenário](#sec-002)
- [O que observar](#sec-003)
- [Tarefa](#sec-004)


Jogo simplificado que oferece **checkpoints**: o jogador salva o progresso e pode
voltar ao ponto salvo. O código funciona, mas o estado do `Jogo` está **totalmente
exposto** (getters/setters para tudo) e um `GerenciadorSave` externo copia os campos
**um a um** para um `String[]`.

O objetivo deste projeto é **rastrear** o problema de design e propor uma solução
usando o padrão **Memento**. Não é um projeto pronto: o código funciona, mas o
encapsulamento está quebrado e o histórico é frágil.

<a id="sec-001"></a>
## Como rodar

[↑ Voltar ao início](#inicio)

O projeto é Maven (Java 17) e abre direto no Eclipse/IntelliJ.

- **Eclipse/IntelliJ:** importe a pasta do projeto e execute a classe `Main`.
- **Linha de comando:**
  ```bash
  mvn compile
  java -cp target/classes br.venson.net.designpatterns.memento.Main
  ```

<a id="sec-002"></a>
## O cenário

[↑ Voltar ao início](#inicio)

- `Jogo`: guarda fase, vidas, pontuação e posição — com getters/setters para **tudo**.
- `GerenciadorSave`: monta um `String[]` com os campos, na ordem, e os converte de volta.
- `Main`: joga, salva um checkpoint, joga de novo e restaura.

<a id="sec-003"></a>
## O que observar

[↑ Voltar ao início](#inicio)

1. O `Jogo` expõe **todos** os atributos; qualquer classe de fora pode mudar o estado.
2. O `GerenciadorSave` **conhece cada campo** e a ordem deles (acoplamento forte).
3. Adicionar um atributo ao `Jogo` obriga a editar o gerenciador em vários pontos.
4. O histórico é uma lista de `String[]`: perde **tipo**, validação e legibilidade.
5. É possível manter um estado restaurado **consistente** sem expor o `Jogo`?

<a id="sec-004"></a>
## Tarefa

[↑ Voltar ao início](#inicio)

Rastreie os problemas de design deste código e proponha uma refatoração com o
padrão **Memento**, deixando claros os papéis: o **memento** (`Checkpoint`,
imutável), o **originator** (`Jogo`, que cria e restaura) e o **caretaker**
(`GerenciadorSave`, que apenas guarda os mementos).


<a id="sec-glossario"></a>
## Glossário
[↑ Voltar ao início](#inicio)

- **Originator:** objeto cujo estado será salvo e restaurado.
- **Memento:** registro de um estado anterior do originator.
- **Caretaker:** guarda mementos sem interpretar seu conteúdo.
- **Snapshot:** cópia do estado em um instante específico.
- **Undo/redo:** navegação entre estados anteriores e posteriores.
