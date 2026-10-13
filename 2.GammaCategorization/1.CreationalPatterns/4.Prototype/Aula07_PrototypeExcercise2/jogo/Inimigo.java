package jogo;

public class Inimigo implements InimigoPrototype {
    private String tipo;
    private int vida;
    private int dano;
    private Arma arma;

    // 1. Construtor padrão para inicializar o protótipo original base
    public Inimigo(String tipo, int vida, int dano, Arma arma) {
        this.tipo = tipo;
        this.vida = vida;
        this.dano = dano;
        this.arma = arma;
    }

    // 2. Construtor de cópia (Deep Copy): copia os atributos primitivos e clona profundamente a Arma
    public Inimigo(Inimigo base) {
        this.tipo = base.tipo;
        this.vida = base.vida;
        this.dano = base.dano;
        // Cópia profunda da arma para garantir independência total entre os objetos
        this.arma = base.arma != null ? base.arma.clonar() : null;
    }

    // 3. Implementação do contrato de clonagem exigido pela interface InimigoPrototype
    @Override
    public InimigoPrototype clonar() {
        return new Inimigo(this);
    }

    // 4. Getters e Setters para ajuste das variações após a clonagem
    public String getTipo() { return tipo; }
    public void setTipo(String tipo) { this.tipo = tipo; }

    public int getVida() { return vida; }
    public void setVida(int vida) { this.vida = vida; }

    public int getDano() { return dano; }
    public void setDano(int dano) { this.dano = dano; }

    public Arma getArma() { return arma; }
    public void setArma(Arma arma) { this.arma = arma; }

    @Override
    public String toString() {
        return "Inimigo [Tipo: " + tipo + ", Vida: " + vida + ", Dano: " + dano + ", " + arma + "]";
    }
}