package projeto_mediator_corrigido;

public interface MediadorChat {
    void enviar(String mensagem, Usuario destinatario, Usuario remetente);
    void adicionarUsuario(Usuario usuario);
    void removerUsuario(Usuario usuario);
}