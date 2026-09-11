package notificacoes;

public class Main {
    public static void main(String[] args) {
        String destinatario = "aluno.ciencia@universidade.edu";
        String aviso = "Suas notas do semestre já estão disponíveis.";

        System.out.println("=== DEMONSTRAÇÃO DO FACTORY METHOD - PORTAL ACADÊMICO ===");

        // 1. Notificação via Email
        NotificacaoService emailService = new EmailService();
        emailService.notificar(destinatario, aviso);

        // 2. Notificação via SMS
        NotificacaoService smsService = new SmsService();
        smsService.notificar("(11) 98888-7777", aviso);

        // 3. Notificação via Push Notification
        NotificacaoService pushService = new PushService();
        pushService.notificar("Device-Token-XYZ987", aviso);
    }
}

/*
 * ========================================================================================
 * EXPLICAÇÃO: COMO ADICIONAR UM NOVO CANAL (EX: WHATSAPP) SEM ALTERAR O FLUXO EXISTENTE
 * ========================================================================================
 * De acordo com o Princípio Aberto/Fechado (OCP), para incluir um novo canal (WhatsApp) 
 * nós não modificamos nenhuma classe existente. Precisamos apenas de dois novos passos:
 * 
 * 1. Criar o Produto Concreto implementando a interface 'Notificador':
 *    public class WhatsappNotificador implements Notificador {
 *        public void enviar(String destinatario, String mensagem) {
 *            System.out.println("[WHATSAPP] Enviando para " + destinatario + " -> " + mensagem);
 *        }
 *    }
 * 
 * 2. Criar o Criador Concreto estendendo 'NotificacaoService':
 *    public class WhatsappService extends NotificacaoService {
 *        protected Notificador criarNotificador() {
 *            return new WhatsappNotificador();
 *        }
 *    }
 * 
 * Com isso, o método 'notificar()' da classe abstrata continua intacto, reutilizando 
 * perfeitamente a lógica de montagem de mensagem e registro de log para o novo canal.
 */