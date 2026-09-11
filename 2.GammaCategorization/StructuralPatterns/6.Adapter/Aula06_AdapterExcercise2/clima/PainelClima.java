package clima;

public class PainelClima {
    private final PrevisaoService previsaoService;

    // Recebe qualquer implementação de PrevisaoService injetada via construtor
    public PainelClima(PrevisaoService previsaoService) {
        this.previsaoService = previsaoService;
    }

    public void exibir(String cidade) {
        int temperatura = previsaoService.obterTemperatura(cidade);
        System.out.println("[PainelClima] A temperatura atual em " + cidade + " é de " + temperatura + "°C.");
    }
}