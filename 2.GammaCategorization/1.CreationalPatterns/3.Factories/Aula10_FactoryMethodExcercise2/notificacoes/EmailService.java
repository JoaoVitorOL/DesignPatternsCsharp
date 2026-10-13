package notificacoes;

public class EmailService extends NotificacaoService {
    @Override
    protected Notificador criarNotificador() {
        return new EmailNotificador();
    }
}