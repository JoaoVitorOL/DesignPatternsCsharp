package clima;

public class Main {
    public static void main(String[] args) {
        String cidade = "São Paulo";

        System.out.println("=== 1. PAINEL CLIMA USANDO OPENWEATHER API (Via Adaptador) ===");
        PrevisaoService servicoOpenWeather = new OpenWeatherAdapter(new OpenWeatherApi());
        PainelClima painel1 = new PainelClima(servicoOpenWeather);
        painel1.exibir(cidade);

        System.out.println("\n=== 2. PAINEL CLIMA USANDO ACCUWEATHER API (Via Adaptador) ===");
        PrevisaoService servicoAccuWeather = new AccuWeatherAdapter(new AccuWeatherApi());
        
        // Demonstra que o PainelClima permanece exatamente o mesmo, trocando apenas o adaptador injetado
        PainelClima painel2 = new PainelClima(servicoAccuWeather);
        painel2.exibir(cidade);
    }
}

/*
 * ========================================================================================
 * EXPLICAÇÃO: COMO ADICIONAR UM TERCEIRO FORNECEDOR SEM ALTERAR O PAINELCLIMA
 * ========================================================================================
 * Para integrar um novo fornecedor de terceiros (por exemplo, WeatherStackApi com um método 
 * próprio 'buscarClima(String local)'), seguindo o Princípio Aberto/Fechado (OCP), nós não 
 * precisamos tocar em nenhuma linha de código da classe 'PainelClima' nem nas classes anteriores.
 * 
 * O processo exigiria apenas duas etapas isoladas:
 * 1. Manter a classe da API de terceiros intacta (conforme restrição do fornecedor).
 * 2. Criar uma nova classe adaptadora (ex: WeatherStackAdapter implements PrevisaoService) 
 *    que traduz a chamada de 'obterTemperatura(String cidade)' para o método específico da nova API, 
 *    realizando as conversões de dados necessárias.
 * 
 * Como o 'PainelClima' interage estritamente com a interface 'PrevisaoService', ele aceitará 
 * o novo adaptador injetado de forma transparente, sem qualquer impacto ou reescrita no fluxo existente.
 */