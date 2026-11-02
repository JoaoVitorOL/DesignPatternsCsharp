<a id="inicio"></a>
# Projeto: sala de chat com anti-pattern (Mediator)

## Sumário
[↑ Voltar ao início](#inicio)

- [Como rodar](#sec-001)
- [O cenário](#sec-002)
- [O que observar](#sec-003)
- [Tarefa](#sec-004)


Sala de chat simplificada em que cada **usuário** fala diretamente com os
demais. O código funciona, mas os usuários se conhecem entre si, formando uma
**malha de referências**.

O objetivo deste projeto é **rastrear** o problema de design e propor uma
solução usando o padrão **Mediator**. Não é um projeto pronto: o código funciona,
mas as ligações estão espalhadas e mudam a cada entrada ou saída de usuário.

<a id="sec-001"></a>
## Como rodar

[↑ Voltar ao início](#inicio)

O projeto é Maven (Java 17) e abre direto no Eclipse/IntelliJ.

- **Eclipse/IntelliJ:** importe a pasta do projeto e execute a classe `Main`.
- **Linha de comando:**
  ```bash
  mvn compile
  java -cp target/classes br.venson.net.designpatterns.mediator.Main
  ```

<a id="sec-002"></a>
## O cenário

[↑ Voltar ao início](#inicio)

- `Usuario`: nome e uma lista de **contatos** (outros usuários).
- `Usuario.adicionarContato(...)`: cria a ligação direta com outro usuário.
- `Usuario.enviar(...)`: percorre a lista e chama `receber(...)` em cada contato.
- `Main`: cria três usuários e **liga cada um a todos os outros** na mão.

<a id="sec-003"></a>
## O que observar

[↑ Voltar ao início](#inicio)

1. Cada `Usuario` guarda referências **diretas** aos demais: N usuários geram
   N × (N − 1) ligações.
2. Incluir um usuário novo obriga a adicionar contatos em **todas** as listas.
3. Remover um usuário pode deixar **referências soltas**.
4. As regras de conversa (todos falam? só o admin? privado?) ficariam
   **espalhadas** em cada `Usuario`.
5. É possível reusar `Usuario` em outra sala, ou em um contexto privado, sem
   reescrever a classe?

<a id="sec-004"></a>
## Tarefa

[↑ Voltar ao início](#inicio)

Rastreie os problemas de design deste código e proponha uma refatoração com o
padrão **Mediator**, deixando claros os papéis: **interface de mediador**
(`MediadorChat`), **mediador concreto** (`SalaChat`) e **colegas** (`Usuario`)
que passam a conhecer apenas o mediador.


<a id="sec-glossario"></a>
## Glossário
[↑ Voltar ao início](#inicio)

- **Mediator:** objeto que coordena a comunicação entre participantes.
- **Colleague:** participante que comunica necessidades ao mediador.
- **Acoplamento:** grau de dependência direta entre componentes.
- **Event broker:** intermediário que distribui eventos aos inscritos.
