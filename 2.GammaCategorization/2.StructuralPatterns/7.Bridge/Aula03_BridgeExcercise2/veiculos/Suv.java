package veiculos;

public class Suv extends Veiculo {
    public Suv(Motor motor) {
        super(motor);
    }

    @Override
    public void dirigir() {
        System.out.print("[SUV Robusta]: ");
        super.dirigir();
    }

    @Override
    public void acelerar() {
        System.out.print("[SUV Potente]: ");
        super.acelerar();
    }
}