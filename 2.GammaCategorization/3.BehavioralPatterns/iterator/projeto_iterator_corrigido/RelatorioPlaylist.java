
package projeto_iterator_corrigido;
public class RelatorioPlaylist {

    public void resumo(Playlist playlist) {
        Iterator<Faixa> iterator = playlist.criarIterator();
        int totalSegundos = 0;
        int count = 0;
        
        while (iterator.hasNext()) {
            Faixa faixa = iterator.next();
            totalSegundos += faixa.getDuracaoSegundos();
            count++;
        }
        
        System.out.printf("Total de faixas: %d | duracao: %d min%n",
                count, totalSegundos / 60);
    }
}