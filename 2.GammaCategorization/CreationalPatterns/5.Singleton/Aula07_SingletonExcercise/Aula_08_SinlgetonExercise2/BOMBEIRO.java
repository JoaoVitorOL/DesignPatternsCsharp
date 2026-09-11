import java.util.List;

public class BOMBEIRO {
    public void enviarAlerta(String mensagem) {
        CentralDeAlertas.getInstancia().enviarAlerta("BOMBEIRO", mensagem);
    }

    public List<String> listarAlertas() {
        return CentralDeAlertas.getInstancia().getAlertas();
    }
}