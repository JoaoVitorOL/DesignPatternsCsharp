
public class Pedido {
    private final double valor;
    private final double peso;
    private final String regiao;

    public Pedido(double valor, double peso, String regiao) {
        this.valor = valor;
        this.peso = peso;
        this.regiao = regiao;
    }

    public double getValor() {
        return valor;
    }

    public double getPeso() {
        return peso;
    }

    public String getRegiao() {
        return regiao;
    }
}