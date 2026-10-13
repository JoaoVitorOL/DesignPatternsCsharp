package clima;

public class AccuWeatherApi {
    public int getTemperature(String cidade) {
        // Retorna a temperatura em Fahrenheit como int
        if (cidade.equalsIgnoreCase("São Paulo")) {
            return 82; // 82°F equivale a aprox. 27.7°C
        }
        return 72; // 72°F equivale a aprox. 22.2°C
    }
}