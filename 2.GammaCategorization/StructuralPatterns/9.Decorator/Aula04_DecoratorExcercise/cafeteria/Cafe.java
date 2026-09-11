package cafeteria;

public class Cafe implements Bebida {
    @Override
    public String getDescricao() {
        return "Café";
    }

    @Override
    public double custo() {
        return 5.0; // Preço base do café
    }
}