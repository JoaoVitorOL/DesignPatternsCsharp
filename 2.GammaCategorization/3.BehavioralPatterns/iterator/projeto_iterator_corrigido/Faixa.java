package projeto_iterator_corrigido;

public class Faixa {
    private String titulo;
    private int duracaoSegundos;
    private boolean favorita;

    // Construtor
    public Faixa(String titulo, int duracaoSegundos, boolean favorita) {
        this.titulo = titulo;
        this.duracaoSegundos = duracaoSegundos;
        this.favorita = favorita;
    }

    // Getters utilizados nos outros arquivos
    public String getTitulo() {
        return titulo;
    }

    public int getDuracaoSegundos() {
        return duracaoSegundos;
    }

    public boolean isFavorita() {
        return favorita;
    }

    // toString para o System.out.println imprimir bonitinho
    @Override
    public String toString() {
        return titulo + " (" + (duracaoSegundos / 60) + " min)";
    }
}
