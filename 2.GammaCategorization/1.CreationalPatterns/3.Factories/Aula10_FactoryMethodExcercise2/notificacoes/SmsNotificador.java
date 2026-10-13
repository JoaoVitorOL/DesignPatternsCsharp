package notificacoes;

public class SmsNotificador implements Notificador {
    @Override
    public void enviar(String destinatario, String mensagem) {
        System.out.println("[SMS] Enviando para " + destinatario + " -> " + mensagem);
    }
}