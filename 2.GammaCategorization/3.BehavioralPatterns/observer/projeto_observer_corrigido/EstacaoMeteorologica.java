
import java.util.ArrayList;
import java.util.List;

public class EstacaoMeteorologica {

    private final List<ISubscriber> inscritos = new ArrayList<>();
    private double temperatura;
    private double umidade;
    private double pressao;

    public void inscrever(ISubscriber inscrito) {
        if (!inscritos.contains(inscrito)) {
            inscritos.add(inscrito);
        }
    }

    public void desinscrever(ISubscriber inscrito) {
        inscritos.remove(inscrito);
    }

    public void notificar() {
        for (ISubscriber inscrito : inscritos) {
            inscrito.atualizar(temperatura, umidade, pressao);
        }
    }

    public void setMedicoes(double temperatura, double umidade, double pressao) {
        this.temperatura = temperatura;
        this.umidade = umidade;
        this.pressao = pressao;
        notificar(); 
    }
}