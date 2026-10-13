import java.util.List;

public class PlaylistIterator implements Iterator<Faixa> {
    private final List<Faixa> faixas;
    private int posicao = 0;

    public PlaylistIterator(List<Faixa> faixas) {
        this.faixas = faixas;
    }

    @Override
    public boolean hasNext() {
        return posicao < faixas.size();
    }

    @Override
    public Faixa next() {
        if (!hasNext()) {
            throw new java.util.NoSuchElementException("Fim da playlist alcançado.");
        }
        return faixas.get(posicao++);
    }
}