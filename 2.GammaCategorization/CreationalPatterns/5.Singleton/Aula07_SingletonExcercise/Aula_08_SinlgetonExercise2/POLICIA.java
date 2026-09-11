import java.util.List;

public class POLICIA {
    public void enviarAlerta(String mensagem) {
        CentralDeAlertas.getInstancia().enviarAlerta("POLICIA", mensagem);
    }

    public List<String> listarAlertas() {
        return CentralDeAlertas.getInstancia().getAlertas();
    }
}