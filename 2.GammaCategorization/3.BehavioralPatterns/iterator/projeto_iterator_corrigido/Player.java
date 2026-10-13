public class Player {

    public void tocarTudo(Playlist playlist) {
        Iterator<Faixa> iterator = playlist.criarIterator();
        while (iterator.hasNext()) {
            System.out.println("Tocando: " + iterator.next());
        }
    }

    public void tocarEmbaralhado(Playlist playlist) {
        // Se precisar embaralhar, a playlist pode fornecer uma cópia segura ou o iterador gerencia
        // (Aqui mantemos a lógica de travessia limpa via iterador)
        Iterator<Faixa> iterator = playlist.criarIterator();
        while (iterator.hasNext()) {
            System.out.println("Tocando (shuffle): " + iterator.next());
        }
    }
}