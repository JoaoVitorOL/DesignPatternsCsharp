package veiculos;

public class MotorEletrico implements Motor {
    @Override
    public void ligar() {
        System.out.println("Silêncio... Sistema elétrico ativado e pronto.");
    }

    @Override
    public void acelerar() {
        System.out.println("Acelerando instantaneamente de forma 100% silenciosa.");
    }
}