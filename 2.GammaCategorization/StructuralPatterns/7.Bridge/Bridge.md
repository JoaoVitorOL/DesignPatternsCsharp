
## 1. O que o Bridge representa?

O **Bridge (Ponte)** é um padrão de projeto estrutural que **separa uma solução em duas partes independentes**: a abstração e a implementação.

### O Problema que ele resolve:

Quando um sistema possui duas variações independentes que precisam crescer juntas (por exemplo: tipos de veículos e tipos de motores), o uso exclusivo da herança gera um cenário conhecido como **explosão combinatória de subclasses**. Os principais problemas disso são:

* O número de classes cresce rapidamente de forma descontrolada.


* Cada nova dimensão multiplica as combinações existentes (ex: $N \times M$ classes).


* A manutenção, os testes e as extensões tornam-se caros e rígidos.



---

## 2. Em que situações se usa?

* Há duas dimensões independentes de variação em um problema.


* A hierarquia de classes está crescendo vertiginosamente por causa de combinações.


* Você deseja trocar implementações ou comportamentos em tempo de execução.


* Precisa reduzir o acoplamento entre a lógica de alto nível e os detalhes específicos de plataforma ou infraestrutura.



---

## 3. Instruções passo a passo de como implementar (em Java)

1. **Identifique as duas dimensões ortogonais** do problema (ex: o que faz vs. como faz).


2. **Defina a interface para as implementações** de baixo nível.


3. **Defina a abstração principal** (classe de alto nível) que o cliente vai utilizar.


4. **Faça a abstração receber a implementação via construtor** (composição/ponte).


5. **Delegue operações** da abstração para o objeto de implementação sempre que fizer sentido.



---

## 4. Explicação detalhada do Exemplo Prático (Veículos e Motores)

O código fornecido demonstra o padrão Bridge aplicado na relação entre **Veículos** (`Sedan`, `Suv`) e **Motores** (`MotorGasolina`, `MotorEletrico`).

### A Interface de Implementação: `Motor`

Define o contrato de baixo nível que os motores devem seguir, desacoplando o comportamento mecânico/elétrico dos veículos.

```java
package veiculos;

public interface Motor {
    void ligar();
    void acelerar();
}

```

---

### As Implementações Concretas: `MotorGasolina` e `MotorEletrico`

Fornecem os detalhes específicos de funcionamento de cada tipo de motor.

```java
package veiculos;

public class MotorGasolina implements Motor {
    @Override
    public void ligar() {
        System.out.println("Vrum! Motor a combustão (Gasolina) ligado.");
    }

    @Override
    public void acelerar() {
        System.out.println("Acelerando com ronco do motor e queima de combustível.");
    }
}

```

```java
package veiculos;

public class MotorEletrico implements Motor {
    @Override
    public void ligar() {
        System.out.println("Silêncio... Sistema elétrico ativado e pronto.");
    }

    @Override
    public void acelerar() {
        System.out.println("Acelerando instantaneamente de forma 100% silenciosa.");
    }
}

```

---

### A Abstração: `Veiculo`

Contém a referência para a interface `Motor` (a "ponte") e delega as chamadas operacionais para ela.

```java
package veiculos;

public abstract class Veiculo {
    // Referência para a implementação (o "Bridge" que conecta as duas pontas)
    protected final Motor motor;

    public Veiculo(Motor motor) {
        this.motor = motor;
    }

    public void dirigir() {
        System.out.print("Preparando veículo -> ");
        motor.ligar();
    }

    public void acelerar() {
        motor.acelerar();
    }
}

```

---

### As Abstrações Refinadas: `Sedan` e `Suv`

Especializam a classe `Veiculo` adicionando características próprias ao comportamento de dirigir e acelerar.

```java
package veiculos;

public class Sedan extends Veiculo {
    public Sedan(Motor motor) {
        super(motor);
    }

    @Override
    public void dirigir() {
        System.out.print("[Sedan Confortável]: ");
        super.dirigir();
    }

    @Override
    public void acelerar() {
        System.out.print("[Sedan Dinâmico]: ");
        super.acelerar();
    }
}

```

```java
package veiculos;

public class Suv extends Veiculo {
    public Suv(Motor motor) {
        super(motor);
    }

    @Override
    public void dirigir() {
        System.out.print("[SUV Robusta]: ");
        super.dirigir();
    }

    @Override
    public void acelerar() {
        System.out.print("[SUV Potente]: ");
        super.acelerar();
    }
}

```

---

### A Execução: `Main`

Demonstra a combinação dinâmica entre diferentes tipos de veículos e motores em tempo de execução sem a necessidade de criar classes combinadas estáticas.

```java
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

```

> **Vantagem prática:** Sem o Bridge, para 3 veículos e 3 motores, precisaríamos de 9 classes fixas. Com o Bridge, criamos apenas 3 + 3 classes independentes e as combinamos livremente via composição.
> 
> 

---

Tem alguma dúvida sobre este padrão ou gostaria de ver como ele se compara com outros padrões estruturais?
