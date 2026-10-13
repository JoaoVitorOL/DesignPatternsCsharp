import java.util.List;

public class SAMU {
    public void enviarAlerta(String mensagem) {
        CentralDeAlertas.getInstancia().enviarAlerta("SAMU", mensagem);
    }

    public List<String> listarAlertas() {
        return CentralDeAlertas.getInstancia().getAlertas();
    }
}