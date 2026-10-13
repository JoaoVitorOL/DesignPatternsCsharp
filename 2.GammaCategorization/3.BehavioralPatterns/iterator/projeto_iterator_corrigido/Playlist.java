package projeto_iterator_corrigido;

import java.util.ArrayList;
import java.util.List;

public class Playlist {
    private final List<Faixa> faixas = new ArrayList<>();

    public void adicionar(Faixa faixa) {
        faixas.add(faixa);
    }

    // Retorna o iterador em vez de expor a lista interna
    public Iterator<Faixa> criarIterator() {
        return new PlaylistIterator(faixas);
    }
}