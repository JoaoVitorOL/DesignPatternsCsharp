Para facilitar o seu download, você pode clicar no botão de copiar no canto superior direito do bloco de código abaixo e salvá-lo em um arquivo com o nome `adapter-pattern.md` no seu computador:

```markdown
# Design Pattern: Adapter

## 1. O que o Adapter representa?
O padrão **Adapter (Adaptador)** é um padrão de projeto estrutural que permite a colaboração entre objetos com interfaces incompatíveis, criando um objeto intermediário (o adaptador) responsável por traduzir uma interface em outra.

### O Problema que ele resolve:
Em sistemas que precisam integrar bibliotecas de terceiros ou serviços externos, a incompatibilidade de interfaces e formatos de dados gera problemas práticos:
- **Alto acoplamento:** O código cliente passa a conhecer detalhes da biblioteca externa.
- **Conversões espalhadas:** Lógicas de tradução de dados (como XML para JSON ou conversões de unidades) ficam espalhadas pelo sistema.
- **Vulnerabilidade a mudanças:** Qualquer mudança no fornecedor impacta vários pontos do código.
- **Impossibilidade de modificação:** Muitas vezes a biblioteca externa não pode ser modificada (por ser de terceiros ou já estar em produção em outros sistemas).

---

## 2. Em que situações se usa?
Use o Adapter quando:
1. **Incompatibilidade de interfaces:** Você deseja usar uma classe existente, mas sua interface não é compatível com o restante da aplicação.
2. **Reutilização de código legado/terceiros:** Precisa reutilizar um componente ou biblioteca externa sem modificar seu código-fonte original.
3. **Isolamento de tradução:** Deseja concentrar a conversão de dados, formatos, parâmetros ou convenções em um único lugar, mantendo o cliente limpo.
4. **Aplicação do OCP:** Quer adicionar novos fornecedores ou serviços no futuro sem alterar o código cliente existente.

---

## 3. Instruções passo a passo de como implementar (em Java)

1. **Identificar a interface esperada:** Reconhecer a interface que o cliente já utiliza e espera.
2. **Identificar o serviço incompatível:** Localizar a classe ou biblioteca externa que precisa ser reutilizada.
3. **Criar a classe adaptadora:** Implementar a interface esperada pelo cliente dentro do adaptador.
4. **Armazenar referência:** Fazer o adaptador armazenar uma referência para o serviço real incompatível.
5. **Traduzir chamadas e dados:** Realizar a tradução de métodos, parâmetros, unidades ou formatos dentro do adaptador e delegar para o serviço real.
6. **Depender da abstração:** Garantir que o cliente dependa estritamente da abstração esperada.

### Exemplo básico de implementação:
```java
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

```

---

## 4. Explicação detalhada do Exemplo Prático (Painel de Clima)

O código fornecido demonstra o padrão **Adapter** aplicado em um painel meteorológico, integrando diferentes APIs de previsão do tempo (AccuWeather e OpenWeather) de forma transparente para a classe cliente.

### A Interface Alvo: `PrevisaoService`

Define o contrato padrão esperado pelo cliente.

```java
package clima;

public interface PrevisaoService {
    int obterTemperatura(String cidade);
}

```

---

### A Classe Cliente: `PainelClima`

Depende estritamente da abstração `PrevisaoService`, desconhecendo os detalhes das APIs de clima subjacentes.

```java
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

```

---

### Os Serviços Incompatíveis (Adaptees) e seus Adaptadores

* **OpenWeather:** Retorna temperatura em `double` e usa o método `temperatura(cidade)`. O `OpenWeatherAdapter` traduz a chamada para o padrão `PrevisaoService`.
* **AccuWeather:** Retorna temperatura em Fahrenheit (`int`) via `getTemperature(cidade)`. O `AccuWeatherAdapter` traduz a chamada e converte a unidade para Celsius.

```java
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

```

```java
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

```

---

### A Execução: `Main`

Demonstra a troca transparente de fornecedores através da injeção dos adaptadores.

```java
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

```

```

---
Tem alguma dúvida sobre este padrão ou gostaria de ver outro Design Pattern estruturado desta mesma forma?

```
