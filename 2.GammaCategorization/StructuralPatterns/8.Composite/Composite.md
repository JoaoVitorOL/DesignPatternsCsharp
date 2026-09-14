
## 1. O que o Composite representa?

O **Composite** é um padrão de projeto estrutural que permite compor objetos em **estruturas de árvore** para representar hierarquias do tipo parte-todo. A ideia central é que o código cliente trate objetos individuais (folhas) e composições de objetos (compostos) de maneira uniforme, utilizando exatamente a mesma interface.

### O Problema que ele resolve:

Em sistemas com estruturas recursivas (como sistemas de arquivos, menus ou cardápios de restaurantes), tratar cada tipo de nó de forma diferente gera graves problemas:

* O código cliente precisa usar checagens de tipos (`instanceof`) para distinguir elementos simples de recipientes.
* A lógica de percorrimento e cálculo recursivo fica espalhada e duplicada pelo cliente.
* Listas genéricas sem tipagem segura (como `List<Object>`) tornam o código vulnerável a erros.
* Adicionar novos tipos de nós exige alterar múltiplos pontos do sistema.

---

## 2. Em que situações se usa?

* Há uma estrutura recursiva do tipo parte-todo (itens que contêm outros itens).
* O cliente precisa tratar itens individuais e grupos de itens exatamente da mesma forma (uniformidade).
* Você quer processar hierarquias de forma genérica e recursiva.
* Deseja adicionar novos tipos de nós sem alterar o código que percorre a estrutura.

---

## 3. Instruções passo a passo de como implementar (em Java)

1. **Identifique a operação comum** que faz sentido para todos os nós da árvore (ex: obter preço, calcular tamanho).
2. **Defina a interface `Component**` contendo essa operação.
3. **Implemente a folha (`Leaf`)**, que representa o nó final e resolve a operação de forma isolada.
4. **Implemente o composto (`Composite`)**, que armazena uma lista de filhos do tipo `Component` e delega ou combina os resultados recursivamente.
5. **Decida onde gerenciar os filhos** (métodos como adicionar/remover): na interface (abordagem transparente) ou apenas no composto (abordagem segura).

---

## 4. Explicação detalhada do Exemplo Prático (Cardápio e Combos de Restaurante)

O código fornecido demonstra o padrão Composite aplicado na gestão de um cardápio, onde pratos individuais (`Prato`) e combinações complexas aninhadas (`Combo`) são tratados de forma totalmente uniforme através da interface `ItemMenu`.

### A Interface Componente: `ItemMenu`

Define o contrato padrão (`getNome()` e `getPreco()`) que tanto os itens individuais quanto os combos devem implementar.

```java
package restaurante;

public interface ItemMenu {
    String getNome();
    double getPreco();
}

```

---

### A Classe Folha (Leaf): `Prato`

Representa os itens simples do cardápio que não possuem filhos. Implementam as operações diretamente.

```java
package restaurante;

public class Prato implements ItemMenu {
    private final String nome;
    private final double preco;

    public Prato(String nome, double preco) {
        this.nome = nome;
        this.preco = preco;
    }

    @Override
    public String getNome() {
        return nome;
    }

    @Override
    public double getPreco() {
        return preco;
    }
}

```

---

### A Classe Composta (Composite): `Combo`

Representa os recipientes que podem conter pratos simples ou outros combos, formando a estrutura de árvore recursiva. O método `getPreco()` realiza a soma recursiva dos preços de todos os seus filhos.

```java
package restaurante;

import java.util.ArrayList;
import java.util.List;

public class Combo implements ItemMenu {
    private final String nome;
    private final List<ItemMenu> itens = new ArrayList<>();

    public Combo(String nome) {
        this.nome = nome;
    }

    public void adicionar(ItemMenu item) {
        itens.add(item);
    }

    public void remover(ItemMenu item) {
        itens.remove(item);
    }

    @Override
    public String getNome() {
        return nome;
    }

    @Override
    public double getPreco() {
        // Soma recursiva do preço de todos os filhos (seja prato ou outro combo)
        double total = 0.0;
        for (ItemMenu item : itens) {
            total += item.getPreco();
        }
        return total;
    }
}

```

---

### A Execução: `Main`

Demonstra a criação de pratos, a composição de combos simples, combos aninhados dentro de outros combos (recursão) e o tratamento perfeitamente uniforme pelo método auxiliar `imprimirDetalhesItem`.

```java
package restaurante;

public class Main {
    public static void main(String[] args) {
        System.out.println("=== 1. MONTANDO PRATOS INDIVIDUAIS (FOLHAS) ===");
        ItemMenu hamburguer = new Prato("Hambúrguer Artesanal", 25.00);
        ItemMenu batataFrita = new Prato("Batata Frita Média", 12.50);
        ItemMenu refrigerante = new Prato("Refrigerante Lata", 7.00);

        System.out.println(hamburguer.getNome() + " - R$ " + hamburguer.getPreco());
        System.out.println(batataFrita.getNome() + " - R$ " + batataFrita.getPreco());
        System.out.println(refrigerante.getNome() + " - R$ " + refrigerante.getPreco());

        System.out.println("\n=== 2. MONTANDO O COMBO DUPLO ===");
        Combo comboDuplo = new Combo("Combo Duplo");
        comboDuplo.adicionar(hamburguer);
        comboDuplo.adicionar(hamburguer);
        comboDuplo.adicionar(refrigerante);

        System.out.println("Preço do " + comboDuplo.getNome() + ": R$ " + comboDuplo.getPreco());

        System.out.println("\n=== 3. MONTANDO O COMBO FAMÍLIA (COM RECURSÃO) ===");
        Combo comboFamilia = new Combo("Combo Família");
        comboFamilia.adicionar(comboDuplo); // Inclui o Combo Duplo dentro dele!
        comboFamilia.adicionar(batataFrita);
        comboFamilia.adicionar(refrigerante);

        System.out.println("Preço do " + comboFamilia.getNome() + ": R$ " + comboFamilia.getPreco());

        System.out.println("\n=== 4. DEMONSTRANDO A UNIFORMIDADE DA INTERFACE ===");
        imprimirDetalhesItem(hamburguer);
        imprimirDetalhesItem(comboDuplo);
        imprimirDetalhesItem(comboFamilia);
    }

    private static void imprimirDetalhesItem(ItemMenu item) {
        System.out.println("[Tratamento Uniforme] Item: " + item.getNome() + " | Preço Total: R$ " + item.getPreco());
    }
}

```

> **Vantagem prática:** O código cliente (`Main` e o método `imprimirDetalhesItem`) não precisa saber se está lidando com um prato isolado ou com um combo complexo contendo subcombos, pois ambos compartilham exatamente a mesma interface `ItemMenu`.

---

Tem alguma dúvida sobre o padrão Composite ou gostaria de explorar outro padrão de projeto?
