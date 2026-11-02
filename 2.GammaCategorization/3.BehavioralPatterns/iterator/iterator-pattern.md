<a id="inicio"></a>
# Iterator — percurso de uma playlist

## Sumário
[↑ Voltar ao início](#inicio)

- [O que o padrão faz](#sec-001)
- [Implementação corrigida](#sec-002)
- [O que os clientes ganham com a abstração](#sec-003)
- [Arquivos para acompanhar](#sec-004)
- [Iterador próprio ou foreach da linguagem?](#sec-005)
- [Variações de percurso](#sec-006)


**Família GoF:** comportamental. **Escopo predominante:** objetos de coleção e percurso.

<a id="sec-001"></a>
## O que o padrão faz

[↑ Voltar ao início](#inicio)

Oferece um modo uniforme de percorrer uma coleção sem expor como os dados são armazenados. A coleção cria o iterador; o iterador controla posição e próximo elemento.

<a id="sec-002"></a>
## Implementação corrigida

[↑ Voltar ao início](#inicio)

Em [projeto_iterator_corrigido](projeto_iterator_corrigido), Playlist mantém a lista privada e cria PlaylistIterator. O iterador guarda posicao, hasNext() verifica o limite e next() retorna a faixa e avança; depois do fim, lança NoSuchElementException.

Player percorre a playlist, Recomendador filtra favoritas e RelatorioPlaylist soma duração. Todos dependem de Iterator<Faixa>, não do armazenamento interno.

    Iterator<Faixa> iterator = playlist.criarIterator();
    while (iterator.hasNext()) {
        Faixa faixa = iterator.next();
        // processar a faixa
    }

Player.tocarEmbaralhado ainda percorre na ordem original; o comentário menciona uma extensão futura. Em C#, IEnumerable<T>/IEnumerator<T> e foreach geralmente fornecem esse mecanismo.


<a id="sec-003"></a>
## O que os clientes ganham com a abstração

[↑ Voltar ao início](#inicio)

O benefício da playlist não é apenas evitar um for com índice. Player, Recomendador e RelatorioPlaylist podem percorrer músicas sem conhecer a coleção interna, e Playlist pode trocar sua representação sem obrigar esses consumidores a mudar. Cada classe cliente responde a uma pergunta diferente (tocar, filtrar ou resumir), mas todas reutilizam o mesmo protocolo de navegação.

O estado posicao pertence ao PlaylistIterator, não à Playlist nem ao Player. Por isso dois clientes podem iniciar percursos independentes. hasNext permite controlar o fim antes de pedir o próximo elemento; next avança exatamente uma posição e sinaliza uso incorreto depois do fim.

O exemplo mantém referência à lista original. Se a playlist for modificada durante a iteração, o comportamento depende da coleção usada; em uma aplicação real deve-se decidir se o iterador observa alterações, usa snapshot ou proíbe modificação concorrente. A classe tocarEmbaralhado é apenas uma demonstração de intenção futura, pois ainda não embaralha.


<a id="sec-004"></a>
## Arquivos para acompanhar

[↑ Voltar ao início](#inicio)

- [Playlist.java](projeto_iterator_corrigido/Playlist.java) esconde a coleção e cria percursos.
- [Iterator.java](projeto_iterator_corrigido/Iterator.java) define o contrato próprio do exemplo.
- [PlaylistIterator.java](projeto_iterator_corrigido/PlaylistIterator.java) controla a posição.
- [Player.java](projeto_iterator_corrigido/Player.java), [Recomendador.java](projeto_iterator_corrigido/Recomendador.java) e [RelatorioPlaylist.java](projeto_iterator_corrigido/RelatorioPlaylist.java) são clientes distintos do mesmo iterador.


<a id="sec-005"></a>
## Iterador próprio ou foreach da linguagem?

[↑ Voltar ao início](#inicio)

No Java do exemplo, a interface Iterator é didática e explicita hasNext/next. Em C#, normalmente prefira IEnumerable<T>/IEnumerator<T>, foreach e yield return: consumidores já conhecem esse protocolo, e LINQ pode operar sobre ele. Um iterador próprio continua justificável se a API precisar expor operações de navegação incomuns ou manter um percurso com estado especializado.

A coleção precisa decidir o que significa modificar dados durante a enumeração. Algumas coleções lançam exceção; outras iteram sobre uma cópia; estruturas concorrentes oferecem garantias específicas. Não prometa estabilidade se ela não existe.

Um iterador também não deve vazar recursos. Ao percorrer arquivo, stream ou banco, o enumerador pode precisar ser descartado; foreach normalmente cuida do Dispose em C#. Entender quem é dono do recurso faz parte do contrato de iteração.


<a id="sec-006"></a>
## Variações de percurso

[↑ Voltar ao início](#inicio)

O projeto usa um objeto iterador explícito, com hasNext/next e posição interna. Um método iterador pode esconder essa classe e produzir elementos sob demanda; em C#, yield return gera IEnumerator<T> com menos código. Esse protocolo padrão permite foreach, LINQ e interoperabilidade com outras bibliotecas.

Duck typing significa que algumas linguagens reconhecem o formato do iterador sem exigir uma interface nominal; isso não elimina a necessidade de contrato claro. Em C#, IEnumerable<T> e IEnumerator<T> são interfaces explícitas e preferíveis para uso comum.

A ordem pode ser por índice, em profundidade numa árvore, filtrada ou calculada sob demanda. O nome tocarEmbaralhado no projeto não implementa shuffle: para isso seria preciso fornecer uma sequência aleatória real, por cópia ou por iterador especializado. Coleções baseadas em arrays também podem expor propriedades como uma sequência sem alocar um objeto por item.

Defina o que acontece se a coleção mudar durante a iteração e quem descarta recursos quando o percurso lê arquivo, stream ou banco de dados.


<a id="sec-glossario"></a>
## Glossário
[↑ Voltar ao início](#inicio)

- **Agregado:** coleção que fornece acesso aos seus elementos.
- **Iterador:** percorre uma coleção sem expor sua representação interna.
- **Cursor:** posição atual do percurso.
- **Percurso:** ordem em que os elementos são visitados.
