package cafeteria;

public abstract class Complemento implements Bebida {
    protected final Bebida bebida;

    public Complemento(Bebida bebida) {
        this.bebida = bebida;
    }

    @Override
    public String getDescricao() {
        return bebida.getDescricao();
    }

    @Override
    public double custo() {
        return bebida.custo();
    }
}