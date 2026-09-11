package jogo;

public class Arma {
    private String nome;
    private int bonusDano;

    public Arma(String nome, int bonusDano) {
        this.nome = nome;
        this.bonusDano = bonusDano;
    }

    // Getters e Setters
    public String getNome() { return nome; }
    public void setNome(String nome) { this.nome = nome; }

    public int getBonusDano() { return bonusDano; }
    public void setBonusDano(int bonusDano) { this.bonusDano = bonusDano; }

    // Método de clonagem manual para garantir a cópia profunda
    public Arma clonar() {
        return new Arma(this.nome, this.bonusDano);
    }

    @Override
    public String toString() {
        return "Arma [Nome: " + nome + ", Bônus de Dano: " + bonusDano + "]";
    }
}