package moveis;

public class Main {
    public static void main(String[] args) {
        System.out.println("=== 1. MONTANDO SALA COM O ESTILO MODERNO ===");
        FabricaMobilia fabricaModerna = new FabricaMobiliaModerna();
        ConfiguradorDeSala salaModerna = new ConfiguradorDeSala(fabricaModerna);
        salaModerna.exibir();

        System.out.println("\n=== 2. TROCANDO PARA O ESTILO VITORIANO (Com uma única mudança na inicialização) ===");
        FabricaMobilia fabricaVitoriana = new FabricaMobiliaVitoriana();
        ConfiguradorDeSala salaVitoriana = new ConfiguradorDeSala(fabricaVitoriana);
        salaVitoriana.exibir();

        
    }
}

/*
 * ========================================================================================
 * EXPLICAÇÃO: O QUE ACONTECERIA SE ALGUÉM TENTASSE MISTURAR MÓVEIS E POR QUE O ABSTRACT FACTORY EVITA ISSO
 * ========================================================================================
 * Se a criação de móveis fosse feita espalhada pelo código usando o operador 'new' diretamente 
 * (ex: new CadeiraModerna(), new SofaVitoriano()), o sistema estaria vulnerável a inconsistências de design, 
 * permitindo que peças de estilos completamente diferentes (como uma cadeira moderna minimalista combinada 
 * com um sofá vitoriano cheio de entalhes clássicos) fossem agrupadas em um mesmo kit de sala de estar.
 * 
 * O padrão Abstract Factory blinda a aplicação contra esse problema ao isolar a criação dos objetos por famílias. 
 * Como a classe cliente ('ConfiguradorDeSala') interage exclusivamente com a interface 'FabricaMobilia' e nunca 
 * instancia classes concretas diretamente, torna-se estruturalmente impossível misturar produtos de famílias diferentes. 
 * A própria fábrica garante de forma consistente que todos os móveis gerados pertençam estritamente ao mesmo estilo selecionado.
 */