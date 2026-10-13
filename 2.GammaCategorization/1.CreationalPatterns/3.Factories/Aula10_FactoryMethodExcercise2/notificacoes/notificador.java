package notificacoes;

public interface Notificador {
    void enviar(String destinatario, String mensagem);
}