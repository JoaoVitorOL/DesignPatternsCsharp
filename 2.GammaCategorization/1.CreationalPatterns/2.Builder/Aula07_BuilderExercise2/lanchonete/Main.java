package lanchonete;

public class Main {
    public static void main(String[] args) {
        System.out.println("=== 1. MONTAGEM VIA API FLUENTE (APENAS OBRIGATÓRIOS) ===");
        try {
            Lanche lancheSimples = new Lanche.Builder()
                .setPao("Pão Francês")
                .setProteina("Ovo")
                .build();

            System.out.println(lancheSimples);
        } catch (Exception ex) {
            System.out.println(ex.getMessage());
        }

        System.out.println("\n=== 2. MONTAGEM VIA API FLUENTE (COMPLETO) ===");
        Lanche lanchePersonalizado = new Lanche.Builder()
            .setPao("Pão Integral")
            .setProteina("Frango Grelhado")
            .setQueijo("Minas Frescal")
            .addVegetal("Alface")
            .addVegetal("Tomate")
            .addVegetal("Cenoura Ralada")
            .setMolho("Iogurte com Ervas")
            .setBemPassado(true)
            .setObservacoes("Cortar o lanche ao meio.")
            .build();

        System.out.println(lanchePersonalizado);

        System.out.println("\n=== 3. GERAÇÃO DE LANCHES ATRAVÉS DO DIRECTOR ===");
        // Criamos instâncias novas do Builder para cada receita do director
        LancheDirector director1 = new LancheDirector(new Lanche.Builder());
        System.out.println(director1.criarMisto());

        LancheDirector director2 = new LancheDirector(new Lanche.Builder());
        System.out.println(director2.criarXSalada());

        LancheDirector director3 = new LancheDirector(new Lanche.Builder());
        System.out.println(director3.criarEspecialDaCasa());

        System.out.println("\n=== 4. TESTE DE VALIDAÇÃO (FALHA ESPERADA) ===");
        try {
            System.out.println("Tentando montar um lanche sem informar a proteína...");
            Lanche lancheInvalido = new Lanche.Builder()
                .setPao("Pão Francês")
                .setQueijo("Mussarela")
                .build(); // Deve lançar IllegalStateException
        } catch (IllegalStateException ex) {
            System.out.println("[EXCEÇÃO CAPTURADA COM SUCESSO]: " + ex.getMessage());
        }
    }
}