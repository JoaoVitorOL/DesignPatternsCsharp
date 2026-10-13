package notificacoes;

public class PushService extends NotificacaoService {
    @Override
    protected Notificador criarNotificador() {
        return new PushNotificador();
    }
}