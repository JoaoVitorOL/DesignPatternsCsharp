
import java.util.ArrayList;
import java.util.List;

public class DisplayEstatisticas implements ISubscriber {

    
    private final List<Double> historico = new ArrayList<>();

    @Override 
    public void atualizar(double temperatura, double umidade, double pressao) {
        historico.add(temperatura);
        double soma = historico.stream().mapToDouble(Double::doubleValue).sum();
        System.out.println("Média das temperaturas: " + (soma / historico.size()) + "°C");
    }
}
