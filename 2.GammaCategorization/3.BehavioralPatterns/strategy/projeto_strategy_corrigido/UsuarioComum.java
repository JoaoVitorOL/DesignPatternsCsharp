public class UsuarioComum implements IUsuario {

   @Override
    public String revelarEtiqueta() {
        return "Cliente comum"; 
    }

    @Override
    public double calcularDesconto(Pedido pedido) {
        return pedido.getValor() * 0.0; 
    }

    @Override
    public double calcularFrete(Pedido pedido) {
        double freteBase = 25.0; 
        double fretePorPeso = pedido.getPeso() * 3.0; 
        double adicionalRegiao = pedido.getRegiao().equalsIgnoreCase("norte") ? 30.0 : 0.0; 
        return freteBase + fretePorPeso + adicionalRegiao;
    }
}