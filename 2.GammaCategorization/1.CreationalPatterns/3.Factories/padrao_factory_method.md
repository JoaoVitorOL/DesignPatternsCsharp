# Design Pattern: Factory Method

## 1. O que o Factory Method representa?
O padrão **Factory Method** é um padrão de projeto criacional que fornece uma interface para criar objetos em uma superclasse, mas permite que as subclasses alterem o tipo de objetos que serão criados.

### O Problema que ele resolve:
Em sistemas onde precisamos gerenciar a criação de objetos que podem variar (como diferentes tipos de transporte, notificadores ou parsers), instanciar diretamente usando `new` gera forte acoplamento e código rígido:
- **Uso excessivo de condicionais:** Espalhar blocos `if/else` ou `switch` por todo o código para decidir qual classe instanciar.
- **Alto acoplamento:** O código cliente fica fortemente acoplado às implementações concretas e muda constantemente.
- **Dificuldade de extensão:** Adicionar um novo tipo de objeto exige modificar código já testado e em funcionamento, violando o Princípio Aberto/Fechado (OCP).

---

## 2. Em que situações se usa?
Use o Factory Method quando:
1. **Desconhecimento de tipos concretos:** Uma classe não pode antecipar quais classes concretas de objetos precisará criar.
2. **Extensibilidade sem modificação:** Você quer dar aos usuários de uma biblioteca ou framework uma maneira de estender seus componentes internos.
3. **Centralização e Variação:** A criação de objetos varia de acordo com o contexto (ambiente dev/prod, tipo de entrada, regras de negócio, região/tenant).
4. **Isolamento de lógica:** O fluxo principal da aplicação deve permanecer genérico, delegando a responsabilidade de instanciação para subclasses especializadas.

---

## 3. Instruções passo a passo de como implementar (em Java)

1. **Definir a Interface do Produto:** Criar uma interface comum (ex: `Notificador`) que declare as operações que todos os produtos concretos devem executar.
2. **Criar Produtos Concretos:** Implementar a interface em classes específicas (ex: `EmailNotificador`, `SmsNotificador`).
3. **Criar a Classe Criadora (Creator):** Definir uma classe abstrata (ex: `NotificacaoService`) que contém a lógica de negócio principal e declara o método fábrica abstrato (`criarNotificador()`).
4. **Criar Criadores Concretos:** Estender a classe criadora em subclasses (ex: `EmailService`, `SmsService`) que sobrescrevem o método fábrica para retornar o produto correspondente.

### Exemplo básico de implementação:
```java
public interface Notificador {
    void enviar(String destinatario, String mensagem);
}

public abstract class NotificacaoService {
    protected abstract Notificador criarNotificador();

    public void notificar(String destinatario, String mensagem) {
        Notificador notificador = criarNotificador();
        notificador.enviar(destinatario, mensagem);
    }
}
```

---

## 4. Explicação detalhada do Exemplo Prático (Portal Acadêmico e Notificações)

O código fornecido demonstra o padrão **Factory Method** aplicado em um portal acadêmico, onde diferentes canais de notificação (E-mail, SMS, Push) são instanciados por subclasses criadoras especializadas, mantendo o fluxo principal desacoplado.

### A Interface Produto: `Notificador`
Define o contrato padrão que todas as implementações de canais de comunicação devem seguir.

```java
package notificacoes;

public interface Notificador {
    void enviar(String destinatario, String mensagem);
}
```

---

### Os Produtos Concretos: `EmailNotificador`, `SmsNotificador`, `PushNotificador`
Implementam a interface `Notificador` com mensagens direcionadas para cada canal específico.

```java
package notificacoes;

public class EmailNotificador implements Notificador {
    @Override
    public void enviar(String destinatario, String mensagem) {
        System.out.println("[EMAIL] Enviando para " + destinatario + " -> " + mensagem);
    }
}
```

```java
package notificacoes;

public class SmsNotificador implements Notificador {
    @Override
    public void enviar(String destinatario, String mensagem) {
        System.out.println("[SMS] Enviando para " + destinatario + " -> " + mensagem);
    }
}
```

```java
package notificacoes;

public class PushNotificador implements Notificador {
    @Override
    public void enviar(String destinatario, String mensagem) {
        System.out.println("[PUSH] Enviando para o dispositivo " + destinatario + " -> " + mensagem);
    }
}
```

---

### A Classe Criadora (Creator): `NotificacaoService`
Classe abstrata que centraliza o fluxo principal de notificação (montagem da mensagem padrão, chamada do método fábrica, envio e registro de log), delegando a criação do objeto concreto para as subclasses.

```java
package notificacoes;

public abstract class NotificacaoService {

    // Método fábrica abstrato que as subclasses concretas devem implementar
    protected abstract Notificador criarNotificador();

    // Fluxo comum compartilhado por todos os tipos de notificação
    public void notificar(String destinatario, String mensagem) {
        System.out.println("\n----------------------------------------");
        
        // 1. Montar a mensagem padrão do portal
        String mensagemPronta = montarMensagem(mensagem);
        
        // 2. Chamar o método fábrica para instanciar o canal correto
        Notificador notificador = criarNotificador();
        
        // 3. Enviar a mensagem pelo canal escolhido
        notificador.enviar(destinatario, mensagemPronta);
        
        // 4. Registrar no log do sistema
        registrarLog(destinatario, notificador.getClass().getSimpleName());
    }

    private String montarMensagem(String mensagemOriginal) {
        return "[Portal Acadêmico] " + mensagemOriginal;
    }

    private void registrarLog(String destinatario, String canal) {
        System.out.println("[LOG] Sucesso! Canal utilizado: " + canal + " | Destinatário: " + destinatario);
    }
}
```

---

### Os Criadores Concretos (Concrete Creators): `EmailService`, `SmsService`, `PushService`
Estendem `NotificacaoService` e implementam o método fábrica retornando o produto concreto correspondente.

```java
package notificacoes;

public class EmailService extends NotificacaoService {
    @Override
    protected Notificador criarNotificador() {
        return new EmailNotificador();
    }
}
```

```java
package notificacoes;

public class SmsService extends NotificacaoService {
    @Override
    protected Notificador criarNotificador() {
        return new SmsNotificador();
    }
}
```

```java
package notificacoes;

public class PushService extends NotificacaoService {
    @Override
    protected Notificador criarNotificador() {
        return new PushNotificador();
    }
}
```

---

### A Execução: `Main`
A classe principal demonstra o uso do serviço de notificações através dos diferentes criadores concretos criados para cada canal.

```java
package notificacoes;

public class Main {
    public static void main(String[] args) {
        String destinatario = "aluno.ciencia@universidade.edu";
        String aviso = "Suas notas do semestre já estão disponíveis.";

        System.out.println("=== DEMONSTRAÇÃO DO FACTORY METHOD - PORTAL ACADÊMICO ===");

        // 1. Notificação via Email
        NotificacaoService emailService = new EmailService();
        emailService.notificar(destinatario, aviso);

        // 2. Notificação via SMS
        NotificacaoService smsService = new SmsService();
        smsService.notificar("(11) 98888-7777", aviso);

        // 3. Notificação via Push Notification
        NotificacaoService pushService = new PushService();
        pushService.notificar("Device-Token-XYZ987", aviso);
    }
}

/*
 * ========================================================================================
 * EXPLICAÇÃO: COMO ADICIONAR UM NOVO CANAL (EX: WHATSAPP) SEM ALTERAR O FLUXO EXISTENTE
 * ========================================================================================
 * De acordo com o Princípio Aberto/Fechado (OCP), para incluir um novo canal (WhatsApp) 
 * nós não modificamos nenhuma classe existente. Precisamos apenas de dois novos passos:
 * 
 * 1. Criar o Produto Concreto implementando a interface 'Notificador':
 *    public class WhatsappNotificador implements Notificador {
 *        public void enviar(String destinatario, String mensagem) {
 *            System.out.println("[WHATSAPP] Enviando para " + destinatario + " -> " + mensagem);
 *        }
 *    }
 * 
 * 2. Criar o Criador Concreto estendendo 'NotificacaoService':
 *    public class WhatsappService extends NotificacaoService {
 *        protected Notificador criarNotificador() {
 *            return new WhatsappNotificador();
 *        }
 *    }
 * 
 * Com isso, o método 'notificar()' da classe abstrata continua intacto, reutilizando 
 * perfeitamente a lógica de montagem de mensagem e registro de log para o novo canal.
 */
```
