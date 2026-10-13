package cafeteria;

public class Chantilly extends Complemento {
    public Chantilly(Bebida bebida) {
        super(bebida);
    }

    @Override
    public String getDescricao() {
        return bebida.getDescricao() + " com chantilly";
    }

    @Override
    public double custo() {
        return bebida.custo() + 2.0;
    }
}