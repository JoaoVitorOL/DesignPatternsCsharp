package notificacoes;

public class PushNotificador implements Notificador {
    @Override
    public void enviar(String destinatario, String mensagem) {
        System.out.println("[PUSH] Enviando para o dispositivo " + destinatario + " -> " + mensagem);
    }
}