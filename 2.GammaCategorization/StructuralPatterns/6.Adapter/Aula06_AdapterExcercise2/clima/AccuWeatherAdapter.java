package clima;

public class AccuWeatherAdapter implements PrevisaoService {
    private final AccuWeatherApi accuWeatherApi;

    public AccuWeatherAdapter(AccuWeatherApi accuWeatherApi) {
        this.accuWeatherApi = accuWeatherApi;
    }

    @Override
    public int obterTemperatura(String cidade) {
        int tempFahrenheit = accuWeatherApi.getTemperature(cidade);
        // Conversão de Fahrenheit para Celsius
        return (int) ((tempFahrenheit - 32) * 5 / 9);
    }
}