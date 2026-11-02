package br.venson.net.designpatterns.memento;

public class Jogo {

    private String fase;
    private int vidas;
    private int pontuacao;
    private double x;
    private double y;

    public void jogar(String fase, int vidas, int pontuacao, double x, double y) {
        this.fase = fase;
        this.vidas = vidas;
        this.pontuacao = pontuacao;
        this.x = x;
        this.y = y;
    }

    // O estado esta TODO exposto: getters e setters para cada atributo.
    // E isso que permite o GerenciadorSave copiar os campos "de fora para dentro".

    public String getFase() {
        return fase;
    }

    public void setFase(String fase) {
        this.fase = fase;
    }

    public int getVidas() {
        return vidas;
    }

    public void setVidas(int vidas) {
        this.vidas = vidas;
    }

    public int getPontuacao() {
        return pontuacao;
    }

    public void setPontuacao(int pontuacao) {
        this.pontuacao = pontuacao;
    }

    public double getX() {
        return x;
    }

    public void setX(double x) {
        this.x = x;
    }

    public double getY() {
        return y;
    }

    public void setY(double y) {
        this.y = y;
    }

    @Override
    public String toString() {
        return "Jogo{fase=" + fase + ", vidas=" + vidas + ", pontuacao=" + pontuacao
                + ", x=" + x + ", y=" + y + "}";
    }
}
