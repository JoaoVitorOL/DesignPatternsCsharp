package lanchonete;

public class LancheDirector {
    private final Lanche.Builder builder;

    public LancheDirector(Lanche.Builder builder) {
        this.builder = builder;
    }

    public Lanche criarMisto() {
        return builder
            .setPao("Pão de Forma")
            .setProteina("Presunto")
            .setQueijo("Mussarela")
            .setBemPassado(false)
            .build();
    }

    public Lanche criarXSalada() {
        return builder
            .setPao("Pão Hambúrguer")
            .setProteina("Hambúrguer Bovino")
            .setQueijo("Prato")
            .addVegetal("Alface")
            .addVegetal("Tomate")
            .setMolho("Maionese da Casa")
            .setBemPassado(true)
            .build();
    }

    public Lanche criarEspecialDaCasa() {
        return builder
            .setPao("Pão Australiano")
            .setProteina("Costela Desfiada")
            .setQueijo("Cheddar")
            .addVegetal("Cebola Caramelizada")
            .addVegetal("Rúcula")
            .setMolho("Molho Barbecue")
            .setBemPassado(true)
            .setObservacoes("Aquecer bem o pão antes de montar.")
            .build();
    }
}