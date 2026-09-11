package clima;

public class OpenWeatherAdapter implements PrevisaoService {
    private final OpenWeatherApi openWeatherApi;

    public OpenWeatherAdapter(OpenWeatherApi openWeatherApi) {
        this.openWeatherApi = openWeatherApi;
    }

    @Override
    public int obterTemperatura(String cidade) {
        double tempCelsius = openWeatherApi.temperatura(cidade);
        return (int) tempCelsius; // Conversão de double para int
    }
}