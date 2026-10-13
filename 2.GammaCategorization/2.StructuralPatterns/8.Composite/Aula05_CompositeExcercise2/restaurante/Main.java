package restaurante;

public class Main {
    public static void main(String[] args) {
        System.out.println("=== 1. MONTANDO PRATOS INDIVIDUAIS (FOLHAS) ===");
        ItemMenu hamburguer = new Prato("Hambúrguer Artesanal", 25.00);
        ItemMenu batataFrita = new Prato("Batata Frita Média", 12.50);
        ItemMenu refrigerante = new Prato("Refrigerante Lata", 7.00);

        System.out.println(hamburguer.getNome() + " - R$ " + hamburguer.getPreco());
        System.out.println(batataFrita.getNome() + " - R$ " + batataFrita.getPreco());
        System.out.println(refrigerante.getNome() + " - R$ " + refrigerante.getPreco());

        System.out.println("\n=== 2. MONTANDO O COMBO DUPLO ===");
        Combo comboDuplo = new Combo("Combo Duplo");
        comboDuplo.adicionar(hamburguer);
        comboDuplo.adicionar(hamburguer);
        comboDuplo.adicionar(refrigerante);

        System.out.println("Preço do " + comboDuplo.getNome() + ": R$ " + comboDuplo.getPreco());

        System.out.println("\n=== 3. MONTANDO O COMBO FAMÍLIA (COM RECURSÃO) ===");
        Combo comboFamilia = new Combo("Combo Família");
        comboFamilia.adicionar(comboDuplo); // Inclui o Combo Duplo dentro dele!
        comboFamilia.adicionar(batataFrita);
        comboFamilia.adicionar(refrigerante);

        System.out.println("Preço do " + comboFamilia.getNome() + ": R$ " + comboFamilia.getPreco());

        System.out.println("\n=== 4. DEMONSTRANDO A UNIFORMIDADE DA INTERFACE ===");
        imprimirDetalhesItem(hamburguer);
        imprimirDetalhesItem(comboDuplo);
        imprimirDetalhesItem(comboFamilia);
    }

    private static void imprimirDetalhesItem(ItemMenu item) {
        System.out.println("[Tratamento Uniforme] Item: " + item.getNome() + " | Preço Total: R$ " + item.getPreco());
    }
}

/*
 * ========================================================================================
 * EXPLICAÇÃO: COMO ADICIONAR UM NOVO TIPO DE ITEM (EX: BEBIDAALCOOLICA)
 * ========================================================================================
 * Seguindo o Princípio Aberto/Fechado (OCP), para adicionar um novo tipo de item ao cardápio 
 * que possua uma regra de negócio específica (como uma 'BebidaAlcoolica' que aplica imposto especial 
 * sobre o preço), não é necessário alterar nenhuma linha de código da classe 'Combo' nem da 'Main'.
 * 
 * O processo exigiria apenas:
 * 1. Criar a nova classe folha implementando a interface 'ItemMenu':
 *    public class BebidaAlcoolica implements ItemMenu {
 *        private String nome;
 *        private double precoBase;
 *        public double getPreco() { return precoBase * 1.20; } // Aplica 20% de imposto
 *        public String getNome() { return nome; }
 *    }
 * 
 * Como a classe 'Combo' lida exclusivamente com a interface genérica 'ItemMenu', ela conseguirá 
 * adicionar, remover e calcular o preço de 'BebidaAlcoolica' de forma totalmente transparente e recursiva, 
 * sem saber se o filho é um prato simples, uma bebida taxada ou outro combo aninhado.
 */