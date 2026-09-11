package clima;

public class OpenWeatherApi {
    public double temperatura(String cidade) {
        // Retorna a temperatura em Celsius como double
        if (cidade.equalsIgnoreCase("São Paulo")) {
            return 28.5;
        }
        return 22.0;
    }
}