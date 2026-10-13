
public class Main {
    public static void main(String[] args) {
        EstacaoMeteorologica estacao = new EstacaoMeteorologica();

        // Criando os displays
        ISubscriber displayTemp = new DisplayTemperatura();
        ISubscriber displayStats = new DisplayEstatisticas();
        ISubscriber displayPressao = new DisplayPressao();

        // Registrando dinamicamente na estação
        estacao.inscrever(displayTemp);
        estacao.inscrever(displayStats);
        estacao.inscrever(displayPressao);

        System.out.println("--- Primeira medição ---");
        estacao.setMedicoes(25.0, 60.0, 1013.0);

        System.out.println("\n--- Removendo o display de pressão em tempo de execução ---");
        estacao.desinscrever(displayPressao);

        System.out.println("--- Segunda medição ---");
        estacao.setMedicoes(27.0, 55.0, 1010.0);
    }
}