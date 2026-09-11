package moveis;

public class ConfiguradorDeSala {
    private final Cadeira cadeira;
    private final Sofa sofa;
    private final MesaDeCentro mesaDeCentro;

    // Recebe a fábrica abstrata no construtor
    public ConfiguradorDeSala(FabricaMobilia fabrica) {
        this.cadeira = fabrica.criarCadeira();
        this.sofa = fabrica.criarSofa();
        this.mesaDeCentro = fabrica.criarMesaDeCentro();
    }

    public void exibir() {
        System.out.println("--- Configuração da Sala de Estar ---");
        cadeira.assentar();
        sofa.deitar();
        mesaDeCentro.apoiar();
    }
}