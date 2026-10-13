package notificacoes;

public abstract class NotificacaoService {

    // Método fábrica abstrato que as subclasses concretas devem implementar
    protected abstract Notificador criarNotificador();

    // Fluxo comum compartilhado por todos os tipos de notificação
    public void notificar(String destinatario, String mensagem) {
        System.out.println("\n----------------------------------------");
        
        // 1. Montar a mensagem padrão do portal
        String mensagemPronta = montarMensagem(mensagem);
        
        // 2. Chamar o método fábrica para instanciar o canal correto
        Notificador notificador = criarNotificador();
        
        // 3. Enviar a mensagem pelo canal escolhido
        notificador.enviar(destinatario, mensagemPronta);
        
        // 4. Registrar no log do sistema
        registrarLog(destinatario, notificador.getClass().getSimpleName());
    }

    private String montarMensagem(String mensagemOriginal) {
        return "[Portal Acadêmico] " + mensagemOriginal;
    }

    private void registrarLog(String destinatario, String canal) {
        System.out.println("[LOG] Sucesso! Canal utilizado: " + canal + " | Destinatário: " + destinatario);
    }
}