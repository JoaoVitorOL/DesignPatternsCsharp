package veiculos;

public class Main {
    public static void main(String[] args) {
        System.out.println("=== 1. SEDAN COM MOTOR A GASOLINA ===");
        Veiculo sedanGasolina = new Sedan(new MotorGasolina());
        sedanGasolina.dirigir();
        sedanGasolina.acelerar();

        System.out.println("\n=== 2. SEDAN COM MOTOR ELÉTRICO ===");
        Veiculo sedanEletrico = new Sedan(new MotorEletrico());
        sedanEletrico.dirigir();
        sedanEletrico.acelerar();

        System.out.println("\n=== 3. SUV COM MOTOR A GASOLINA ===");
        Veiculo suvGasolina = new Suv(new MotorGasolina());
        suvGasolina.dirigir();
        suvGasolina.acelerar();

        System.out.println("\n=== 4. SUV COM MOTOR ELÉTRICO ===");
        Veiculo suvEletrico = new Suv(new MotorEletrico());
        suvEletrico.dirigir();
        suvEletrico.acelerar();
    }
}

/*
 * ========================================================================================
 * EXPLICAÇÃO: COMO ADICIONAR UM NOVO MOTOR OU UM NOVO VEÍCULO USANDO O PADRÃO BRIDGE
 * ========================================================================================
 * O principal benefício do padrão Bridge é separar a hierarquia de controle (os tipos de veículos) 
 * da hierarquia de plataforma/comportamento (os tipos de motores). 
 * 
 * 1. Adicionando um novo Motor (ex: MotorHibrido implements Motor):
 *    - Basta criar a nova classe implementando a interface 'Motor'.
 *    - Nenhuma classe de veículo ('Sedan', 'Suv', 'Veiculo') precisa ser modificada ou reescrita. 
 *    - O motor híbrido poderá ser injetado em qualquer veículo existente de forma transparente.
 * 
 * 2. Adicionando um novo Veículo (ex: Picape extends Veiculo):
 *    - Basta criar a nova classe estendendo 'Veiculo'.
 *    - Nenhuma classe de motor existente precisa ser alterada. A picape poderá usar motores 
 *      a gasolina, elétricos ou híbridos sem esforço adicional.
 * 
 * Sem o padrão Bridge, se tivéssemos 3 tipos de veículos e 3 tipos de motores, precisaríamos criar 
 * 9 classes combinadas fixas. Com o Bridge, criamos apenas 3 + 3 classes independentes e as combinamos 
 * dinamicamente via composição em tempo de execução.
 */