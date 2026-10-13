# Design Pattern: Prototype

## 1. O que o Prototype representa?
O padrão **Prototype** é um padrão de projeto criacional que permite **copiar objetos existentes** — mesmo aqueles que possuem estruturas complexas ou encapsuladas — sem que o código cliente dependa de suas classes concretas.

### O Problema que ele resolve:
Em sistemas onde precisamos criar muitos objetos parecidos (como inimigos em um jogo, templates ou configurações), a criação direta via `new` gera problemas práticos:
- **Código repetido e acoplado:** Muito código para configurar cada objeto do zero, acoplando o cliente às classes concretas.
- **Dificuldade de clonagem segura:** Copiar dados manualmente campo a campo é trabalhoso, propenso a erros e pode falhar ao lidar com referências internas (*shallow copy* vs *deep copy*).
- **Perda de flexibilidade:** Quando o sistema precisa clonar objetos conhecidos apenas por interfaces genéricas, instanciar diretamente via `new` torna-se inviável.

---

## 2. Em que situações se usa?
Use o Prototype quando:
1. **Objetos semelhantes:** Vários objetos compartilham características base e diferem apenas em pequenos ajustes ou atributos específicos.
2. **Criação custosa:** A instanciação de um objeto envolve etapas pesadas (consultas a banco, leitura de arquivos, processamento complexo) e clonar a partir de um modelo pronto é mais eficiente.
3. **Reduzir acoplamento:** O cliente precisa criar novos objetos sem conhecer a classe exata deles, interagindo apenas com uma interface ou protótipo genérico.
4. **Registro de modelos:** Faz sentido manter modelos pré-configurados em um repositório centralizado para duplicação sob demanda.

---

## 3. Instruções passo a passo de como implementar (em Java)

1. **Definir uma interface comum de clonagem:** Declarar um método (ex: `clonar()`) que será implementado pelas classes clonáveis.
2. **Implementar a cópia na classe concreta:** Criar um construtor de cópia ou método que duplique os atributos do objeto base.
3. **Garantir a Cópia Profunda (Deep Copy):** Decidir se objetos internos (como referências a outras classes) devem ser compartilhados ou clonados separadamente para manter a independência.
4. **Criar um Registro de Protótipos (Opcional):** Um repositório (`Map`) para armazenar modelos padrão prontos para serem buscados e clonados por nome ou categoria.

### Exemplo básico de implementação:
```java
public interface InimigoPrototype {
    InimigoPrototype clonar();
}

public class Inimigo implements InimigoPrototype {
    private String tipo;
    private int vida;

    public Inimigo(String tipo, int vida) {
        this.tipo = tipo;
        this.vida = vida;
    }

    // Construtor de cópia
    public Inimigo(Inimigo base) {
        this.tipo = base.tipo;
        this.vida = base.vida;
    }

    @Override
    public InimigoPrototype clonar() {
        return new Inimigo(this);
    }
}
```

---

## 4. Explicação detalhada do Exemplo Prático (Sistema de Inimigos e Jogo)

O código fornecido demonstra o padrão **Prototype** aplicado em um jogo, onde diferentes tipos de inimigos e suas armas são gerenciados por um registro central e clonados sob demanda, utilizando **Deep Copy**.

### A Classe Componente: `Arma`
Representa o equipamento do inimigo. Possui um método de clonagem manual (`clonar()`) para garantir que, ao clonar o inimigo, a arma também seja duplicada com segurança (evitando que dois inimigos compartilhem exatamente a mesma instância de arma na memória).

```java
package jogo;

public class Arma {
    private String nome;
    private int bonusDano;

    public Arma(String nome, int bonusDano) {
        this.nome = nome;
        this.bonusDano = bonusDano;
    }

    public String getNome() { return nome; }
    public void setNome(String nome) { this.nome = nome; }

    public int getBonusDano() { return bonusDano; }
    public void setBonusDano(int bonusDano) { this.bonusDano = bonusDano; }

    public Arma clonar() {
        return new Arma(this.nome, this.bonusDano);
    }

    @Override
    public String toString() {
        return "Arma [Nome: " + nome + ", Bônus de Dano: " + bonusDano + "]";
    }
}
```

---

### A Interface e a Classe Protótipo: `InimigoPrototype` e `Inimigo`
- `InimigoPrototype` define o contrato de clonagem.
- `Inimigo` implementa o contrato utilizando um **construtor de cópia (Deep Copy)**, que copia os tipos primitivos e chama `arma.clonar()` para isolar o objeto interno.

```java
package jogo;

public interface InimigoPrototype {
    InimigoPrototype clonar();
}
```

```java
package jogo;

public class Inimigo implements InimigoPrototype {
    private String tipo;
    private int vida;
    private int dano;
    private Arma arma;

    public Inimigo(String tipo, int vida, int dano, Arma arma) {
        this.tipo = tipo;
        this.vida = vida;
        this.dano = dano;
        this.arma = arma;
    }

    public Inimigo(Inimigo base) {
        this.tipo = base.tipo;
        this.vida = base.vida;
        this.dano = base.dano;
        this.arma = base.arma != null ? base.arma.clonar() : null;
    }

    @Override
    public InimigoPrototype clonar() {
        return new Inimigo(this);
    }

    public String getTipo() { return tipo; }
    public void setTipo(String tipo) { this.tipo = tipo; }

    public int getVida() { return vida; }
    public void setVida(int vida) { this.vida = vida; }

    public int getDano() { return dano; }
    public void setDano(int dano) { this.dano = dano; }

    public Arma getArma() { return arma; }
    public void setArma(Arma arma) { this.arma = arma; }

    @Override
    public String toString() {
        return "Inimigo [Tipo: " + tipo + ", Vida: " + vida + ", Dano: " + dano + ", " + arma + "]";
    }
}
```

---

### O Registro: `RegistroDePrototipos`
Centraliza e armazena protótipos pré-configurados (guerreiro, mago, arqueiro, chefe) em um mapa (`Map`). O método `getPrototipo()` busca o modelo desejado e retorna diretamente um clone pronto para uso.

```java
package jogo;

import java.util.HashMap;
import java.util.Map;

public class RegistroDePrototipos {
    private final Map<String, InimigoPrototype> prototipos = new HashMap<>();

    public RegistroDePrototipos() {
        carregarPrototiposPadrao();
    }

    private void carregarPrototiposPadrao() {
        prototipos.put("guerreiro", new Inimigo("Guerreiro", 100, 20, new Arma("Espada Longa", 10)));
        prototipos.put("mago", new Inimigo("Mago", 60, 35, new Arma("Cajado Mágico", 25)));
        prototipos.put("arqueiro", new Inimigo("Arqueiro", 80, 25, new Arma("Arco Composto", 15)));
        prototipos.put("chefe", new Inimigo("Chefe", 500, 60, new Arma("Machado Gigante", 40)));
    }

    public Inimigo getPrototipo(String nome) {
        InimigoPrototype prototipo = prototipos.get(nome.toLowerCase());
        if (prototipo == null) {
            throw new IllegalArgumentException("Protótipo não encontrado: " + nome);
        }
        return (Inimigo) prototipo.clonar();
    }
}
```

---

### A Execução: `Main`
A classe principal valida o comportamento do Prototype:
1. **Obtenção via Registro:** Recupera protótipos base diretamente do repositório.
2. **Criação de Variações:** Clona um guerreiro base e altera seus atributos para criar um "Guerreiro Elite" sem afetar o modelo original.
3. **Comprovação de Instâncias Distintas:** Comprova que os clones gerados possuem referências de memória diferentes (`hashCode()` distintos).
4. **Demonstração de Deep Copy:** Altera a arma apenas de um arqueiro clonado e comprova que o outro arqueiro permanece com sua arma intacta.

```java
package jogo;

public class Main {
    public static void main(String[] args) {
        RegistroDePrototipos registro = new RegistroDePrototipos();

        System.out.println("=== 1. OBTENDO INIMIGOS VIA REGISTRO DE PROTÓTIPOS ===");
        Inimigo guerreiroBase = registro.getPrototipo("guerreiro");
        Inimigo magoBase = registro.getPrototipo("mago");

        System.out.println("Original do Registro: " + guerreiroBase);
        System.out.println("Original do Registro: " + magoBase);

        System.out.println("\n=== 2. CRIANDO INIMIGO ELITE A PARTIR DO 'GUERREIRO' ===");
        Inimigo guerreiroElite = registro.getPrototipo("guerreiro");
        guerreiroElite.setTipo("Guerreiro Elite");
        guerreiroElite.setVida(250);
        guerreiroElite.setDano(45);

        System.out.println("Guerreiro Padrão (Não mudou): " + guerreiroBase);
        System.out.println("Guerreiro Elite (Modificado):   " + guerreiroElite);

        System.out.println("\n=== 3. PROVANDO QUE OS CLONES SÃO OBJETOS DISTINTOS ===");
        Inimigo guerreiroClone1 = registro.getPrototipo("guerreiro");
        Inimigo guerreiroClone2 = registro.getPrototipo("guerreiro");

        System.out.println("Hash do Clone 1: " + guerreiroClone1.hashCode());
        System.out.println("Hash do Clone 2: " + guerreiroClone2.hashCode());
        System.out.println("São referências iguais? " + (guerreiroClone1 == guerreiroClone2));

        System.out.println("\n=== 4. DEMONSTRANDO A CÓPIA PROFUNDA (DEEP COPY) ===");
        Inimigo arqueiro1 = registro.getPrototipo("arqueiro");
        Inimigo arqueiro2 = registro.getPrototipo("arqueiro");

        System.out.println("Antes da alteração:");
        System.out.println("Arqueiro 1 Arma: " + arqueiro1.getArma());
        System.out.println("Arqueiro 2 Arma: " + arqueiro2.getArma());

        arqueiro1.getArma().setNome("Arco Élfico Lendário");
        arqueiro1.getArma().setBonusDano(50);

        System.out.println("\nDepois de modificar a arma do Arqueiro 1:");
        System.out.println("Arqueiro 1 Arma (Modificado): " + arqueiro1.getArma());
        System.out.println("Arqueiro 2 Arma (Permaneceu intacta): " + arqueiro2.getArma());
    }
}
```
