# Atividade 13: Iterator

## Exercício 1: Aplicações


- Para cada cenário abaixo, indique se o padrão Iterator é apropriado ou não e justifique em 2–3 frases.


1. Um app de música precisa percorrer uma playlist por várias ordens (sequencial, embaralhada, só favoritas), sem que o restante do código conheça a estrutura interna da coleção.

   - O Iterator é adequado. Pois, será necessário percorrer entre elementos de estrtuturas diferentes


2. Um explorador de arquivos precisa percorrer uma árvore de pastas e arquivos sem que a interface exponha como os nós estão ligados.

    - O iterator é adequado. Pois, será necessário percorrer elementos sem expôr a estrutura.

3. Um sistema precisa ler um arquivo gigante por partes (streaming), entregando um registro por vez em vez de carregar tudo na memória.

   - O Iterator é adequado, já que trabalha com uma entrega em loop.


4. Um array fixo de tamanho conhecido, percorrido sempre por um único laço for simples dentro da própria classe dona do array.

    - O iterator não é necessário. Porque, há apenas um tipo de estrutura para ser iterado.


5. Uma conexão de banco de dados que precisa expor as linhas de um ResultSet uma a uma, escondendo o cursor e a estrutura da tabela.

  - O iterator é adequado, já que envolve ocultação de informações em uma estrutura de dados que é iterada.




  # Exercício 2: Rastreando o anti-pattern em um projeto real



1. Playlist.getFaixas() devolve a própria lista interna. Quais são as consequências de encapsulamento disso? Que clientes passam a depender da estrutura da playlist?

Consequências de encapsulamento e clientes dependentes:

Consequências: Quebra total do encapsulamento. O código externo ganha acesso direto para modificar, adicionar ou remover elementos da lista interna sem qualquer validação da classe Playlist.

Clientes dependentes: Player, Recomendador e RelatorioPlaylist, que passam a depender diretamente de uma estrutura baseada em índices (List).



2. Ao rodar o Main, observe a ordem da playlist antes e depois de tocarEmbaralhado. Explique por que a ordem original mudou.


Porque o método tocarEmbaralhado aplicou Collections.shuffle diretamente sobre a referência da lista original devolvida por getFaixas(). Como não é feita uma cópia, o estado interno da Playlist fica permanentemente alterado.   


3. A travessia por índice se repete em três clientes (Player, Recomendador, RelatorioPlaylist). O que acontece se a Playlist trocar sua estrutura interna (array, lista ligada, páginas)? Quantos pontos quebram?

O que acontece: Se a Playlist mudar a sua estrutura interna (para um array, lista ligada ou páginas), o acesso direto por índice (get(i)) deixará de compilar ou de funcionar corretamente.

Quantos pontos quebram: Quebram 3 pontos (exatamente nos três clientes que implementam a travessia por índice: Player, Recomendador e RelatorioPlaylist).


4. Proponha uma refatoração com o padrão Iterator, deixando explícitos:
a interface de iterador (temProxima()/proxima());
o iterador concreto que guarda a posição;
o agregado Playlist, que cria o iterador.

 <br> a) A interface de iterador: Define o contrato padronizado de travessia através dos métodos temProxima() e proxima().
 <br >b) O iterador concreto: A classe PlaylistIterator, que armazena internamente a posição atual do percurso sobre a coleção de faixas.  
 <br> c) O agregado Playlist: A classe Playlist, que oculta sua estrutura de dados interna (List<Faixa>) e expõe apenas o método de criação do iterador.

5. Como a solução permite novas formas de travessia (embaralhada, só favoritas) sem duplicar laços e sem expor a estrutura interna?

A solução permite criar novas formas de travessia (como percursos embaralhados ou filtrados apenas por favoritas) encapsulando essas regras dentro de iteradores concretos especializados (ou aplicando o padrão nos iteradores). Como o contrato exposto aos clientes é sempre o mesmo (temProxima() e proxima()), os clientes consomem os dados de forma idêntica, sem duplicar laços de repetição baseados em índices e sem que a estrutura interna da Playlist precise ser exposta ou modificada indevidamente.

Entrega: um diagrama de classes (antes/depois), o código refatorado e um parágrafo justificando cada decisão de design.