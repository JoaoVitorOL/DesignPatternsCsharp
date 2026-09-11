package veiculos;

public class Sedan extends Veiculo {
    public Sedan(Motor motor) {
        super(motor);
    }

    @Override
    public void dirigir() {
        System.out.print("[Sedan Confortável]: ");
        super.dirigir();
    }

    @Override
    public void acelerar() {
        System.out.print("[Sedan Dinâmico]: ");
        super.acelerar();
    }
}