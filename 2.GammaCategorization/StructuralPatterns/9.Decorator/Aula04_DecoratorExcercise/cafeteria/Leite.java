package cafeteria;

public class Leite extends Complemento {
    public Leite(Bebida bebida) {
        super(bebida);
    }

    @Override
    public String getDescricao() {
        return bebida.getDescricao() + " com leite";
    }

    @Override
    public double custo() {
        return bebida.custo() + 1.5;
    }
}