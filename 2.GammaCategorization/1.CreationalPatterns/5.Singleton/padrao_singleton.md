# Design Pattern: Singleton

## 1. O que o Singleton representa?
O padrão **Singleton** é um padrão de projeto criacional que tem como objetivo **garantir que uma classe tenha apenas uma única instância** em toda a aplicação, fornecendo ao mesmo tempo um **ponto global de acesso** a essa instância.

### O Problema que ele resolve:
Em sistemas complexos, existem recursos que devem ser únicos na aplicação inteira, tais como:
- Arquivos de configuração central.
- Gerenciadores de conexão com o banco de dados.
- Serviços de log ou centrais de alerta.

Se cada parte do sistema puder instanciar esses recursos livremente usando `new`, surgem problemas como **inconsistência de dados**, **uso excessivo de recursos** (estouro de conexões) e **dificuldade de coordenação**.

---

## 2. Em que situações se usa?
Use o Singleton quando:
1. **Exigência de unicidade:** Deve existir apenas uma instância de uma classe específica compartilhada por todo o sistema (ex: Gerenciador de Configurações, Pool de Conexões).
2. **Ponto global de acesso:** Quando você precisa acessar esse recurso compartilhado de diferentes lugares da aplicação (ex: preferências do usuário, cache em memória).
3. **Controle de inicialização tardia (Lazy Initialization):** Para criar o objeto apenas quando ele for realmente necessário, economizando recursos na inicialização do sistema.

---

## 3. Instruções passo a passo de como implementar (em Java)

1. **Tornar o construtor privado:** Impede que outras classes criem instâncias diretamente usando `new`.
2. **Criar um atributo estático privado:** Armazena a única instância da classe.
3. **Criar um método estático público (`getInstancia`):** Controla a criação e o retorno do objeto único.

### Exemplo básico de implementação:
```java
public class ConfiguracaoSistema {
    private static ConfiguracaoSistema instancia;

    // 1. Construtor privado
    private ConfiguracaoSistema() {
        // Inicialização de recursos
    }

    // 3. Método estático de acesso
    public static ConfiguracaoSistema getInstancia() {
        if (instancia == null) {
            instancia = new ConfiguracaoSistema();
        }
        return instancia;
    }
}
```

---

## 4. Explicação detalhada do Exemplo Prático (Central de Alertas)

O código fornecido exemplifica o padrão **Singleton** aplicado em um cenário de **Central de Alertas de Emergência** (inspirado na analogia de serviços como Polícia, Bombeiros e SAMU).

### A Classe Singleton: `CentralDeAlertas`
Esta classe centraliza o gerenciamento de alertas utilizando uma abordagem **Thread-Safe** (segura para ambientes concorrentes com múltiplas threads) através de *Double-Checked Locking* e a palavra-chave `volatile`:
- **Atributo `instancia` (`volatile`):** Garante a visibilidade correta da instância única entre diferentes threads.
- **Construtor privado:** Impede que qualquer classe crie uma nova central com `new CentralDeAlertas()`. Utiliza um `CopyOnWriteArrayList` para armazenar as mensagens com segurança em ambientes concorrentes.
- **Método `getInstancia()`:** Verifica se a instância já existe. Caso contrário, sincroniza a criação para evitar que duas threads criem instâncias simultâneas.

```java
import java.util.ArrayList;
import java.util.Collections;
import java.util.List;
import java.util.concurrent.CopyOnWriteArrayList;

public class CentralDeAlertas {
    private final List<String> mensagens;
    private static volatile CentralDeAlertas instancia;

    private CentralDeAlertas() {
        mensagens = new CopyOnWriteArrayList<>();
    }

    public static CentralDeAlertas getInstancia() {
        if (instancia == null) {
            synchronized (CentralDeAlertas.class) {
                if (instancia == null) {
                    instancia = new CentralDeAlertas();
                }
            }
        }
        return instancia;
    }

    public void enviarAlerta(String orgao, String mensagem) {
        String alertaFormatado = "[" + orgao + "] " + mensagem;
        mensagens.add(alertaFormatado);
    }

    public List<String> getAlertas() {
        return Collections.unmodifiableList(new ArrayList<>(mensagens));
    }
}
```

---

### As Classes de Acesso (`POLICIA`, `BOMBEIRO`, `SAMU`)
Estes órgãos funcionam como os "diferentes telefones" da nossa analogia. Cada um possui métodos para enviar alertas e listar o histórico, delegando sempre as operações para a única instância de `CentralDeAlertas` obtida via `CentralDeAlertas.getInstancia()`.

Exemplo (`POLICIA`):
```java
import java.util.List;

public class POLICIA {
    public void enviarAlerta(String mensagem) {
        CentralDeAlertas.getInstancia().enviarAlerta("POLICIA", mensagem);
    }

    public List<String> listarAlertas() {
        return CentralDeAlertas.getInstancia().getAlertas();
    }
}
```
*(O mesmo comportamento ocorre nas classes `BOMBEIRO` e `SAMU`, identificando seus respectivos órgãos).*

---

### A Execução: `Main`
A classe principal demonstra o funcionamento prático e a comprovação da unicidade:
1. **Comprovação de Instância Única (`hashCode()`):** É impresso o hash code obtido a partir de chamadas feitas pela Polícia, Bombeiros e SAMU. Todos exibem exatamente o **mesmo hash code**, provando que se trata do mesmo objeto em memória.
2. **Envio de Alertas:** Cada órgão envia sua respectiva ocorrência. Como compartilham a mesma lista na `CentralDeAlertas`, todas as mensagens são centralizadas.
3. **Compartilhamento do Histórico:** Ao listar os alertas a partir da Polícia ou do SAMU, o histórico completo e unificado é exibido, provando que o estado é compartilhado globalmente.

```java
public class Main {
    public static void main(String[] args) {
        POLICIA policia = new POLICIA();
        BOMBEIRO bombeiros = new BOMBEIRO();
        SAMU samu = new SAMU();

        System.out.println("=== COMPROVAÇÃO DE INSTÂNCIA ÚNICA (HASHCODE) ===");
        System.out.println("Hash via Polícia:   " + CentralDeAlertas.getInstancia().hashCode());
        System.out.println("Hash via Bombeiros: " + CentralDeAlertas.getInstancia().hashCode());
        System.out.println("Hash via SAMU:      " + CentralDeAlertas.getInstancia().hashCode());
        System.out.println("===============================================
");

        policia.enviarAlerta("Ocorrência de roubo na Zona Sul.");
        bombeiros.enviarAlerta("Princípio de incêndio em edificação comercial.");
        samu.enviarAlerta("Acidente de trânsito grave na Rodovia Principal.");

        System.out.println("--- Histórico visualizado pela POLÍCIA ---");
        policia.listarAlertas().forEach(System.out::println);

        System.out.println("\n--- Histórico visualizado pelo SAMU ---");
        samu.listarAlertas().forEach(System.out::println);
    }
}
```
