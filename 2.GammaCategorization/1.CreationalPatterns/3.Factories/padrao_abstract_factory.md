# Design Pattern: Abstract Factory

## 1. O que o Abstract Factory representa?
O padrão **Abstract Factory** é um padrão de projeto criacional que permite a criação de **famílias de objetos relacionados ou dependentes** sem que suas classes concretas sejam especificadas diretamente pelo código cliente.

### O Problema que ele resolve:
Em sistemas que precisam suportar múltiplos temas, ambientes ou plataformas (como interfaces visuais Windows vs. macOS ou diferentes drivers de banco de dados), a criação direta de objetos acopla o código e gera problemas críticos:
- **Alto acoplamento:** O código fica cheio de chamadas `new` para classes concretas específicas de um ambiente.
- **Mistura indevida de famílias:** Falta de garantia estrutural, permitindo acidentalmente combinar componentes incompatíveis no mesmo contexto (ex: um botão do Windows com um checkbox do macOS).
- **Baixa extensibilidade:** Adicionar uma nova família ou plataforma exige modificar múltiplos pontos do sistema.

---

## 2. Em que situações se usa?
Use o Abstract Factory quando:
1. **Famílias de objetos compatíveis:** O sistema precisa criar conjuntos de objetos que devem trabalhar juntos de forma harmoniosa (ex: móveis de um mesmo estilo ou componentes de interface de um mesmo tema).
2. **Independência de plataforma/ambiente:** Você deseja isolar a criação de objetos para permitir a troca fácil de temas, drivers, provedores de nuvem ou motores de renderização sem alterar o código cliente.
3. **Restrição de consistência:** É necessário garantir que o cliente utilize apenas produtos pertencentes à mesma família selecionada.

---

## 3. Instruções passo a passo de como implementar (em Java)

1. **Definir as Interfaces de Produto:** Criar uma interface para cada tipo de produto distinto que compõe a família (ex: `Cadeira`, `Sofa`, `MesaDeCentro`).
2. **Criar Produtos Concretos:** Implementar as interfaces para cada família específica (ex: `CadeiraModerna`, `CadeiraVitoriana`).
3. **Definir a Interface da Fábrica Abstrata:** Criar uma interface que declare um método de criação para cada produto da família (ex: `criarCadeira()`, `criarSofa()`).
4. **Criar Fábricas Concretas:** Implementar a fábrica abstrata para cada família/estilo, retornando os produtos concretos correspondentes e compatíveis.

### Exemplo básico de implementação:
```java
public interface FabricaMobilia {
    Cadeira criarCadeira();
    Sofa criarSofa();
}

public class FabricaMobiliaModerna implements FabricaMobilia {
    @Override
    public Cadeira criarCadeira() { return new CadeiraModerna(); }
    @Override
    public Sofa criarSofa() { return new SofaModerno(); }
}
```

---

## 4. Explicação detalhada do Exemplo Prático (Configuração de Mobília de Sala)

O código fornecido demonstra o padrão **Abstract Factory** aplicado na montagem de conjuntos de móveis para sala de estar, garantindo que estilos diferentes (Moderno e Vitoriano) não sejam misturados de forma incorreta.

### As Interfaces e Classes de Produtos: `Cadeira`, `Sofa`, `MesaDeCentro`
Definem os contratos e as implementações concretas para cada estilo de mobília.

```java
package moveis;

public interface Cadeira {
    void assentar();
}

public class CadeiraModerna implements Cadeira {
    @Override
    public void assentar() {
        System.out.println("[Moderna] Sentando em uma cadeira Moderna (design minimalista e limpo).");
    }
}

public class CadeiraVitoriana implements Cadeira {
    @Override
    public void assentar() {
        System.out.println("[Vitoriana] Sentando em uma cadeira Vitoriana (entalhes clássicos em madeira nobre).");
    }
}
```

```java
package moveis;

public interface Sofa {
    void deitar();
}

public class SofaModerno implements Sofa {
    @Override
    public void deitar() {
        System.out.println("[Moderna] Deitando em um sofá Moderno (linhas retas e tecido confortável).");
    }
}

public class SofaVitoriano implements Sofa {
    @Override
    public void deitar() {
        System.out.println("[Vitoriana] Deitando em um sofá Vitoriano (estofado capitonê luxuoso).");
    }
}
```

```java
package moveis;

public interface MesaDeCentro {
    void apoiar();
}

public class MesaDeCentroModerna implements MesaDeCentro {
    @Override
    public void apoiar() {
        System.out.println("[Moderna] Apoiando objetos em uma mesa de centro Moderna (vidro e metal).");
    }
}

public class MesaDeCentroVitoriana implements MesaDeCentro {
    @Override
    public void apoiar() {
        System.out.println("[Vitoriana] Apoiando objetos em uma mesa de centro Vitoriana (acabamento clássico detalhado).");
    }
}
```

---

### A Interface e as Fábricas Concretas: `FabricaMobilia`
A interface `FabricaMobilia` define os métodos de criação da família, enquanto as classes concretas encapsulam a instanciação de cada conjunto estilístico.

```java
package moveis;

public interface FabricaMobilia {
    Cadeira criarCadeira();
    Sofa criarSofa();
    MesaDeCentro criarMesaDeCentro();
}
```

```java
package moveis;

public class FabricaMobiliaModerna implements FabricaMobilia {
    @Override
    public Cadeira criarCadeira() { return new CadeiraModerna(); }

    @Override
    public Sofa criarSofa() { return new SofaModerno(); }

    @Override
    public MesaDeCentro criarMesaDeCentro() { return new MesaDeCentroModerna(); }
}
```

```java
package moveis;

public class FabricaMobiliaVitoriana implements FabricaMobilia {
    @Override
    public Cadeira criarCadeira() { return new CadeiraVitoriana(); }

    @Override
    public Sofa criarSofa() { return new SofaVitoriano(); }

    @Override
    public MesaDeCentro criarMesaDeCentro() { return new MesaDeCentroVitoriana(); }
}
```

---

### A Classe Cliente: `ConfiguradorDeSala`
Recebe a fábrica abstrata em seu construtor e inicializa os móveis necessários de forma totalmente desacoplada das classes concretas.

```java
package moveis;

public class ConfiguradorDeSala {
    private final Cadeira cadeira;
    private final Sofa sofa;
    private final MesaDeCentro mesaDeCentro;

    // Recebe a fábrica abstrata no construtor
    public ConfiguradorDeSala(FabricaMobilia fabrica) {
        this.cadeira = fabrica.criarCadeira();
        this.sofa = fabrica.criarSofa();
        this.mesaDeCentro = fabrica.criarMesaDeCentro();
    }

    public void exibir() {
        System.out.println("--- Configuração da Sala de Estar ---");
        cadeira.assentar();
        sofa.deitar();
        mesaDeCentro.apoiar();
    }
}
```

---

### A Execução: `Main`
Demonstra a troca de famílias de objetos alterando apenas a implementação da fábrica injetada no cliente.

```java
package moveis;

public class Main {
    public static void main(String[] args) {
        System.out.println("=== 1. MONTANDO SALA COM O ESTILO MODERNO ===");
        FabricaMobilia fabricaModerna = new FabricaMobiliaModerna();
        ConfiguradorDeSala salaModerna = new ConfiguradorDeSala(fabricaModerna);
        salaModerna.exibir();

        System.out.println("\n=== 2. TROCANDO PARA O ESTILO VITORIANO (Com uma única mudança na inicialização) ===");
        FabricaMobilia fabricaVitoriana = new FabricaMobiliaVitoriana();
        ConfiguradorDeSala salaVitoriana = new ConfiguradorDeSala(fabricaVitoriana);
        salaVitoriana.exibir();
    }
}

/*
 * ========================================================================================
 * EXPLICAÇÃO: O QUE ACONTECERIA SE ALGUÉM TENTASSE MISTURAR MÓVEIS E POR QUE O ABSTRACT FACTORY EVITA ISSO
 * ========================================================================================
 * Se a criação de móveis fosse feita espalhada pelo código usando o operador 'new' diretamente 
 * (ex: new CadeiraModerna(), new SofaVitoriano()), o sistema estaria vulnerável a inconsistências de design, 
 * permitindo que peças de estilos completamente diferentes (como uma cadeira moderna minimalista combinada 
 * com um sofá vitoriano cheio de entalhes clássicos) fossem agrupadas em um mesmo kit de sala de estar.
 * 
 * O padrão Abstract Factory blinda a aplicação contra esse problema ao isolar a criação dos objetos por famílias. 
 * Como a classe cliente ('ConfiguradorDeSala') interage exclusivamente com a interface 'FabricaMobilia' e nunca 
 * instancia classes concretas diretamente, torna-se estruturalmente impossível misturar produtos de famílias diferentes. 
 * A própria fábrica garante de forma consistente que todos os móveis gerados pertençam estritamente ao mesmo estilo selecionado.
 */
```
