import java.util.ArrayList;
import java.util.Collections;
import java.util.List;
import java.util.concurrent.CopyOnWriteArrayList;

public class CentralDeAlertas {

    private final List<String> mensagens;


    // 1. Instância única garantida com volatile para visibilidade entre threads
    private static volatile CentralDeAlertas instancia;


    // 2. Construtor privado para impedir a criação de instâncias com 'new'
    private CentralDeAlertas() {
        mensagens = new CopyOnWriteArrayList<>();
    }

    // 3. Método global de acesso à instância única (Singleton Thread-Safe)
    public static CentralDeAlertas getInstancia() {
        if (instancia == null) {
            synchronized (CentralDeAlertas.class) {
                if (instancia == null) {
                    instancia = new CentralDeAlertas();
                }
            }
        }
        return instancia;
    }


    

    public void enviarAlerta(String orgao, String mensagem) {
        String alertaFormatado = "[" + orgao + "] " + mensagem;
        mensagens.add(alertaFormatado);
    }

    public List<String> getAlertas() {
        return Collections.unmodifiableList(new ArrayList<>(mensagens));
    }
}