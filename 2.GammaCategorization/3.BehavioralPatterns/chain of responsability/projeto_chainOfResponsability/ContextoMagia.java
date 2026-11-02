
public class ContextoMagia {
    private final Jogador jogador;
    private final String nomeMagia;
    private final int custoMana;
    private final double cooldownRestante;
    private final double distanciaAlvo;

    public ContextoMagia(Jogador jogador, String nomeMagia, int custoMana, double cooldownRestante, double distanciaAlvo) {
        this.jogador = jogador;
        this.nomeMagia = nomeMagia;
        this.custoMana = custoMana;
        this.cooldownRestante = cooldownRestante;
        this.distanciaAlvo = distanciaAlvo;
    }

    public Jogador getJogador() { return jogador; }
    public String getNomeMagia() { return nomeMagia; }
    public int getCustoMana() { return custoMana; }
    public double getCooldownRestante() { return cooldownRestante; }
    public double getDistanciaAlvo() { return distanciaAlvo; }
}

class Jogador {
    private final String nome;
    private boolean vivo;
    private boolean atordoado;
    private int manaAtual;

    public Jogador(String nome, boolean vivo, boolean atordoado, int manaAtual) {
        this.nome = nome;
        this.vivo = vivo;
        this.atordoado = atordoado;
        this.manaAtual = manaAtual;
    }

    public boolean isVivo() { return vivo; }
    public boolean isAtordoado() { return atordoado; }
    public int getManaAtual() { return manaAtual; }
    public void gastarMana(int quantidade) { this.manaAtual -= quantidade; }
    public String getNome() { return nome; }
}