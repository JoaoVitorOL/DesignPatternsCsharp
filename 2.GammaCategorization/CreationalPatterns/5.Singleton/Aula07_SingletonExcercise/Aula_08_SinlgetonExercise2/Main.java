public class Main {
    public static void main(String[] args) {
        // Criando objetos dos órgãos
        POLICIA policia = new POLICIA();
        BOMBEIRO bombeiros = new BOMBEIRO();
        SAMU samu = new SAMU();

        // Comprovando que a CentralDeAlertas é a mesma instância através do hashCode()
        System.out.println("=== COMPROVAÇÃO DE INSTÂNCIA ÚNICA (HASHCODE) ===");
        System.out.println("Hash via Polícia:   " + CentralDeAlertas.getInstancia().hashCode());
        System.out.println("Hash via Bombeiros: " + CentralDeAlertas.getInstancia().hashCode());
        System.out.println("Hash via SAMU:      " + CentralDeAlertas.getInstancia().hashCode());
        System.out.println("===============================================\n");

        // Cada órgão envia um alerta
        policia.enviarAlerta("Ocorrência de roubo na Zona Sul.");
        bombeiros.enviarAlerta("Princípio de incêndio em edificação comercial.");
        samu.enviarAlerta("Acidente de trânsito grave na Rodovia Principal.");

        // Listando a partir de órgãos diferentes para provar o compartilhamento do histórico
        System.out.println("--- Histórico visualizado pela POLÍCIA ---");
        policia.listarAlertas().forEach(System.out::println);

        System.out.println("\n--- Histórico visualizado pelo SAMU ---");
        samu.listarAlertas().forEach(System.out::println);
    }
}