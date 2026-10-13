package veiculos;

public class MotorGasolina implements Motor {
    @Override
    public void ligar() {
        System.out.println("Vrum! Motor a combustão (Gasolina) ligado.");
    }

    @Override
    public void acelerar() {
        System.out.println("Acelerando com ronco do motor e queima de combustível.");
    }
}