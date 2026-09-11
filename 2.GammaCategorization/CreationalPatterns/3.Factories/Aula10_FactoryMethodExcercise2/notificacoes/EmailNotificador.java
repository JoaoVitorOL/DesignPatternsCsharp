package notificacoes;

public class EmailNotificador implements Notificador {
    @Override
    public void enviar(String destinatario, String mensagem) {
        System.out.println("[EMAIL] Enviando para " + destinatario + " -> " + mensagem);
    }
}