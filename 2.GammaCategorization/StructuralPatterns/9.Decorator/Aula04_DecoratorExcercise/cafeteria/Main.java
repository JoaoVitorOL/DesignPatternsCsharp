package cafeteria;

public class Main {
    public static void main(String[] args) {
        System.out.println("=== 1. CAFÉ PURO ===");
        Bebida cafePuro = new Cafe();
        imprimirPedido(cafePuro);

        System.out.println("\n=== 2. CAFÉ COM LEITE ===");
        Bebida cafeComLeite = new Leite(new Cafe());
        imprimirPedido(cafeComLeite);

        System.out.println("\n=== 3. CAFÉ COM LEITE E CHANTILLY (Encadeado) ===");
        // O café é embrulhado pelo leite, que por sua vez é embrulhado pelo chantilly
        Bebida cafeCompleto = new Chantilly(new Leite(new Cafe()));
        imprimirPedido(cafeCompleto);
    }

    private static void imprimirPedido(Bebida bebida) {
        System.out.println("Descrição: " + bebida.getDescricao());
        System.out.println("Custo Total: R$ " + String.format("%.2f", bebida.custo()));
    }
}

/*
 * ========================================================================================
 * EXPLICAÇÃO: COMO ADICIONAR UM NOVO COMPLEMENTO (EX: CALDA DE CARAMELO)
 * ========================================================================================
 * Seguindo o Princípio Aberto/Fechado (OCP), para lançar um novo complemento na cafeteria 
 * (como 'CaldaDeCaramelo' cobrando +3.0), não é necessário alterar a classe base 'Cafe', 
 * o decorator abstrato 'Complemento', os complementos existentes ('Leite', 'Chantilly') nem a 'Main'.
 * 
 * O processo exige apenas a criação de uma nova classe que estende 'Complemento':
 *    public class CaldaDeCaramelo extends Complemento {
 *        public CaldaDeCaramelo(Bebida bebida) {
 *            super(bebida);
 *        }
 *        @Override
 *        public String getDescricao() {
 *            return bebida.getDescricao() + " com calda de caramelo";
 *        }
 *        @Override
 *        public double custo() {
 *            return bebida.custo() + 3.0;
 *        }
 *    }
 * 
 * Graças ao padrão Decorator, a nova calda pode ser combinada livremente em tempo de execução 
 * (ex: new CaldaDeCaramelo(new Leite(new Cafe()))), somando descrições e custos camada por camada 
 * de forma totalmente transparente e compatível com a interface genérica 'Bebida'.
 */