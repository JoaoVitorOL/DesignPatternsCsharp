package veiculos;

public abstract class Veiculo {
    // Referência para a implementação (o "Bridge" que conecta as duas pontas)
    protected final Motor motor;

    public Veiculo(Motor motor) {
        this.motor = motor;
    }

    public void dirigir() {
        System.out.print("Preparando veículo -> ");
        motor.ligar();
    }

    public void acelerar() {
        motor.acelerar();
    }
}