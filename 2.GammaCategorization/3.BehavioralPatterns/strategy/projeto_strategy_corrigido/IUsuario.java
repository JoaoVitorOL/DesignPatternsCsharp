public interface IUsuario {

    double calcularDesconto(Pedido pedido);
    double calcularFrete(Pedido pedido);
    String revelarEtiqueta();

}