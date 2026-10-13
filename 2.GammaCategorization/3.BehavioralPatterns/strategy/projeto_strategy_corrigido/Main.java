public class Main {
    public static void main(String[] args) {
        Pedido pedido = new Pedido(200.0, 2.0, "sul");

        // Cria o relatório com a estratégia Comum
        RelatorioPedido relatorio = new RelatorioPedido(new UsuarioComum());
        
        System.out.println("--- Comportamento Padrão (Comum) ---");
        System.out.println(relatorio.formatar(pedido));

        // Trocando a estratégia em runtime no MESMO objeto para VIP
        relatorio.setUsuario(new UsuarioVIP());
        
        System.out.println("\n--- Comportamento Alterado em Runtime (VIP) ---");
        System.out.println(relatorio.formatar(pedido));

        // Voltando para Corporativo
        relatorio.setUsuario(new UsuarioCorporativo());
        
        System.out.println("\n--- Comportamento Alterado Novamente (Corporativo) ---");
        System.out.println(relatorio.formatar(pedido));
    }
}