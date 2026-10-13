
public class RelatorioPedido {

   private IUsuario usuario; // Removido o 'final' para permitir alteração

    public RelatorioPedido(IUsuario usuario) {
        this.usuario = usuario;
    }

    // Método para trocar a estratégia em runtime no mesmo objeto
    public void setUsuario(IUsuario usuario) {
        this.usuario = usuario;
    }

    public String formatar(Pedido pedido) {
        String etiqueta = usuario.revelarEtiqueta();
        double valorDesconto = usuario.calcularDesconto(pedido);
        double valorFrete = usuario.calcularFrete(pedido);
        double total = pedido.getValor() - valorDesconto + valorFrete;

        return String.format(
                "%s | valor: %.2f | desconto: %.2f | frete: %.2f | total: %.2f",
                etiqueta, pedido.getValor(), valorDesconto, valorFrete, total);
    }
}
