package notificacoes;

public class SmsService extends NotificacaoService {
    @Override
    protected Notificador criarNotificador() {
        return new SmsNotificador();
    }
}