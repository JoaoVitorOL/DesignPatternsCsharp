package projeto_iterator_corrigido;

public class Recomendador {

    public void sugerirFavoritas(Playlist playlist) {
        Iterator<Faixa> iterator = playlist.criarIterator();
        System.out.println("Favoritas:");
        while (iterator.hasNext()) {
            Faixa faixa = iterator.next();
            if (faixa.isFavorita()) {
                System.out.println("  * " + faixa);
            }
        }
    }
}