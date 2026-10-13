# Design Pattern: Builder

## 1. O que o Builder representa?
O padrão **Builder** é um padrão de projeto criacional que tem como objetivo **separar a construção de um objeto complexo da sua representação**, permitindo que o mesmo processo de construção crie diferentes tipos e representações de objetos.

### O Problema que ele resolve:
Em muitos sistemas, precisamos criar objetos com muitos atributos (vários deles opcionais ou com combinações diferentes), como um cadastro completo, uma configuração avançada ou um pedido de e-commerce. Sem o Builder, enfrentamos problemas comuns:
- **Construtor telescópico:** Construtores com dezenas de parâmetros que tornam o código difícil de ler e propenso a erros de troca de ordem de parâmetros.
- **Muitos setters soltos:** Deixam o objeto em um estado inconsistente no meio da montagem e dificultam garantir que tudo o que era necessário foi configurado.
- **Variações de construção espalhadas:** A lógica de como montar diferentes variações do objeto se espalha por todo o código cliente.

---

## 2. Em que situações se usa?
Use o Builder quando:
1. **Muitos parâmetros:** Um objeto tiver muitos parâmetros, especialmente opcionais ou com combinações complexas.
2. **Evitar construtores gigantescos:** Quiser deixar a inicialização do código mais limpa, legível e segura (com API fluente).
3. **Múltiplas representações:** Você precisar construir diferentes versões ou variações do mesmo tipo de objeto usando o mesmo processo.
4. **Processo centralizado:** O processo de construção precisar ser reutilizável, isolando a lógica de validação e montagem.

---

## 3. Instruções passo a passo de como implementar (em Java)

1. **Tornar o construtor do produto privado:** Garante que o objeto só possa ser instanciado através do Builder.
2. **Criar uma classe estática interna (Builder):** Com os mesmos atributos do produto para receber os valores passo a passo.
3. **Criar métodos de configuração fluentes:** Métodos que definem cada atributo e retornam `this` (o próprio Builder) para permitir o encadeamento de chamadas.
4. **Criar o método `build()`:** Responsável por validar as regras de negócio e instanciar o produto final repassando o Builder.

### Exemplo básico de implementação:
```java
public class Lanche {
    private final String pao;
    private final String proteina;

    private Lanche(Builder builder) {
        this.pao = builder.pao;
        this.proteina = builder.proteina;
    }

    public static class Builder {
        private String pao;
        private String proteina;

        public Builder setPao(String pao) {
            this.pao = pao;
            return this;
        }

        public Builder setProteina(String proteina) {
            this.proteina = proteina;
            return this;
        }

        public Lanche build() {
            if (pao == null || proteina == null) {
                throw new IllegalStateException("Parâmetros obrigatórios ausentes!");
            }
            return new Lanche(this);
        }
    }
}
```

---

## 4. Explicação detalhada do Exemplo Prático (Montagem de Lanches)

O código fornecido exemplifica o padrão **Builder** aplicado em um contexto de uma lanchonete, combinando o uso de uma **API Fluente** com a figura de um **Director** para orquestrar receitas prontas.

### A Classe Produto: `Lanche` e sua classe interna `Builder`
A classe `Lanche` possui campos obrigatórios e opcionais (como queijo, vegetais, molhos e observações). O construtor é privado, aceitando apenas o `Builder`.
- O `Builder` usa métodos encadeados (`setPao`, `setProteina`, `addVegetal`, etc.) para configurar o lanche passo a passo.
- O método `build()` realiza validações essenciais (como garantir que pão e proteína foram informados) antes de retornar o objeto final construído.

```java
import java.util.ArrayList;
import java.util.Collections;
import java.util.List;

public class Lanche {
    private final String pao;
    private final String proteina;
    private final String queijo;
    private final List<String> vegetais;
    private final String molho;
    private final boolean bemPassado;
    private final String observacoes;

    private Lanche(Builder builder) {
        this.pao = builder.pao;
        this.proteina = builder.proteina;
        this.queijo = builder.queijo;
        this.vegetais = Collections.unmodifiableList(new ArrayList<>(builder.vegetais));
        this.molho = builder.molho;
        this.bemPassado = builder.bemPassado;
        this.observacoes = builder.observacoes;
    }

    // Getters omitidos para brevidade...

    @Override
    public String toString() {
        String vegetaisStr = vegetais.isEmpty() ? "Nenhum" : String.join(", ", vegetais);
        return "Lanche [Pão: " + pao + ", Proteína: " + proteina + ", Queijo: " + (queijo != null ? queijo : "Não") + 
               ", Vegetais: [" + vegetaisStr + "], Molho: " + (molho != null ? molho : "Nenhum") + 
               ", Bem Passado: " + (bemPassado ? "Sim" : "Não") + ", Observações: " + (observacoes != null ? observacoes : "Nenhuma") + "]";
    }

    public static class Builder {
        private String pao;
        private String proteina;
        private String queijo;
        private List<String> vegetais = new ArrayList<>();
        private String molho;
        private boolean bemPassado;
        private String observacoes;

        public Builder setPao(String pao) {
            this.pao = pao;
            return this; 
        }

        public Builder setProteina(String proteina) {
            this.proteina = proteina;
            return this;
        }

        public Builder setQueijo(String queijo) {
            this.queijo = queijo;
            return this;
        }

        public Builder addVegetal(String vegetal) {
            this.vegetais.add(vegetal);
            return this;
        }

        public Builder setMolho(String molho) {
            this.molho = molho;
            return this;
        }

        public Builder setBemPassado(boolean bemPassado) {
            this.bemPassado = bemPassado;
            return this;
        }

        public Builder setObservacoes(String observacoes) {
            this.observacoes = observacoes;
            return this;
        }

        public Lanche build() {
            if (pao == null || pao.trim().isEmpty()) {
                throw new IllegalStateException("Erro de validação: O campo obrigatório 'pao' não foi informado.");
            }
            if (proteina == null || proteina.trim().isEmpty()) {
                throw new IllegalStateException("Erro de validação: O campo obrigatório 'proteina' não foi informado.");
            }
            return new Lanche(this);
        }
    }
}
```

---

### A Classe Opcional: `LancheDirector`
O `Director` funciona como a "cozinha" que conhece receitas pré-definidas. Ele recebe um `Builder` e executa uma sequência padrão de chamadas para entregar sanduíches específicos (como Misto, X-Salada ou Especial da Casa) sem que o cliente precise se preocupar com os detalhes de montagem.

```java
public class LancheDirector {
    private final Lanche.Builder builder;

    public LancheDirector(Lanche.Builder builder) {
        this.builder = builder;
    }

    public Lanche criarMisto() {
        return builder
            .setPao("Pão de Forma")
            .setProteina("Presunto")
            .setQueijo("Mussarela")
            .setBemPassado(false)
            .build();
    }

    public Lanche criarXSalada() {
        return builder
            .setPao("Pão Hambúrguer")
            .setProteina("Hambúrguer Bovino")
            .setQueijo("Prato")
            .addVegetal("Alface")
            .addVegetal("Tomate")
            .setMolho("Maionese da Casa")
            .setBemPassado(true)
            .build();
    }

    public Lanche criarEspecialDaCasa() {
        return builder
            .setPao("Pão Australiano")
            .setProteina("Costela Desfiada")
            .setQueijo("Cheddar")
            .addVegetal("Cebola Caramelizada")
            .addVegetal("Rúcula")
            .setMolho("Molho Barbecue")
            .setBemPassado(true)
            .setObservacoes("Aquecer bem o pão antes de montar.")
            .build();
    }
}
```

---

### A Execução: `Main`
A classe principal demonstra as diferentes formas de uso do padrão:
1. **Montagem via API Fluente (Apenas Obrigatórios):** Criação direta de um lanche simples utilizando apenas os métodos essenciais do builder.
2. **Montagem via API Fluente (Completo):** Encadeamento de múltiplos opcionais (queijos, múltiplos vegetais, molhos e observações).
3. **Geração através do Director:** Utilização de receitas prontas encapsuladas no `LancheDirector`.
4. **Teste de Validação:** Demonstração do tratamento de erro quando um campo obrigatório (como a proteína) é omitido, capturando a exceção lançada pelo método `build()`.

```java
package lanchonete;

public class Main {
    public static void main(String[] args) {
        System.out.println("=== 1. MONTAGEM VIA API FLUENTE (APENAS OBRIGATÓRIOS) ===");
        try {
            Lanche lancheSimples = new Lanche.Builder()
                .setPao("Pão Francês")
                .setProteina("Ovo")
                .build();

            System.out.println(lancheSimples);
        } catch (Exception ex) {
            System.out.println(ex.getMessage());
        }

        System.out.println("\n=== 2. MONTAGEM VIA API FLUENTE (COMPLETO) ===");
        Lanche lanchePersonalizado = new Lanche.Builder()
            .setPao("Pão Integral")
            .setProteina("Frango Grelhado")
            .setQueijo("Minas Frescal")
            .addVegetal("Alface")
            .addVegetal("Tomate")
            .addVegetal("Cenoura Ralada")
            .setMolho("Iogurte com Ervas")
            .setBemPassado(true)
            .setObservacoes("Cortar o lanche ao meio.")
            .build();

        System.out.println(lanchePersonalizado);

        System.out.println("\n=== 3. GERAÇÃO DE LANCHES ATRAVÉS DO DIRECTOR ===");
        LancheDirector director1 = new LancheDirector(new Lanche.Builder());
        System.out.println(director1.criarMisto());

        LancheDirector director2 = new LancheDirector(new Lanche.Builder());
        System.out.println(director2.criarXSalada());

        LancheDirector director3 = new LancheDirector(new Lanche.Builder());
        System.out.println(director3.criarEspecialDaCasa());

        System.out.println("\n=== 4. TESTE DE VALIDAÇÃO (FALHA ESPERADA) ===");
        try {
            System.out.println("Tentando montar um lanche sem informar a proteína...");
            Lanche lancheInvalido = new Lanche.Builder()
                .setPao("Pão Francês")
                .setQueijo("Mussarela")
                .build(); 
        } catch (IllegalStateException ex) {
            System.out.println("[EXCEÇÃO CAPTURADA COM SUCESSO]: " + ex.getMessage());
        }
    }
}
```
