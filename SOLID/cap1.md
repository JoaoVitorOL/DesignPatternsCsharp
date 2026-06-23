# Princípios de Design SOLID
**por Robert C. Martin**

<img width="220" height="278" alt="Robert C. Martin" src="https://github.com/user-attachments/assets/545f9369-f037-4c78-b0ef-81123f4bbb23" />

**Livro:** *Agile Principles, Patterns and Practices in C#*

<img width="1200" height="699" alt="Capa do livro Agile Principles, Patterns and Practices in C#" src="https://github.com/user-attachments/assets/ca8967da-2afa-40ba-983d-76f0120349eb" />

---

## Sumário
1. [Single Responsibility Principle](#single-responsibility-principle)
2. [Open-Closed Principle](#open-closed-principle)
3. [Liskov Substitution Principle](#liskov-substitution-principle)
4. [Interface Segregation Principle](#interface-segregation-principle)
5. [Dependency Inversion Principle](#dependency-inversion-principle)

---

## Single Responsibility Principle

Qualquer classe deve ter apenas uma única razão para existir.

A quebra desse princípio acontece quando o desenvolvedor faz uma classe que, ao evoluir, começa a tomar mais responsabilidades no código do que deveria.

Imagine uma classe chamada `Diario`. É necessário e faz sentido que uma classe assim tenha funções como `AddEntry`, `RemoveEntry`, `ToString`. É exatamente assim que um diário funciona! Agora, se o desenvolvedor começar a incluir funções como `Save`, `LoadFile`, `LoadURI`, veja como isso já não faz mais sentido: a classe que era para ser só um diário também está fazendo salvamento e carregamento de arquivos.

O que deve ser feito? Uma nova classe, `FileManager`. Dessa forma, `Diario` cuida apenas de "coisas de diário", enquanto `FileManager` gerencia carregamento e URI de arquivos.

---
## Open-Closed Principle

Qualquer classe deve estar aberta para extensão, mas fechada para modificação.

Isso quer dizer: deve ser possível adicionar comportamento novo ao sistema sem alterar o código que já existe e já está testado. "Aberto para extensão" é adicionar coisa nova (uma classe nova); "fechado para modificação" é não abrir e remexer numa classe que já estava funcionando.

Imagine uma classe chamada `CalculadoraDeDescontos`. É necessário e faz sentido que uma classe assim aplique regras de negócio de preço, como `CalcularDescontoEstudante`. Agora, se o seu chefe chegar e pedir para o sistema aplicar também desconto de Black Friday, depois desconto de cliente VIP, depois desconto de funcionário, veja o problema: se você começar a incluir funções ou blocos `if/else` para cada nova regra de desconto direto nela, toda vez que um desconto novo aparecer você vai precisar abrir essa classe e modificá-la — e isso é exatamente o que o OCP não quer. Se você errar uma linha de código, pode estragar a regra do estudante que já estava homologada e funcionando!

O que deve ser feito? Em vez de criar uma única classe com métodos amontoados, usamos abstração. Criamos uma interface `IDesconto` com um método `Aplicar(decimal valor)`. Dessa forma, cada tipo de desconto vira uma classe própria (`DescontoEstudante`, `DescontoBlackFriday`) que implementa essa interface. 

Por fim, a `CalculadoraDeDescontos` apenas recebe qualquer classe que respeite o contrato `IDesconto` e executa a operação. Quando o chefe pedir um desconto novo, você só cria uma classe nova — sem tocar em nenhuma das que já existem. Isso é estar aberto para extensão (código novo) e fechado para modificação (código antigo intocado).

### ❌ Abordagem Incorreta (Violação do OCP)

Considere o cenário inicial onde as regras são inseridas por métodos fixos ou condicionais dentro da mesma classe:

```csharp
// Se novos tipos de descontos surgirem, esta classe precisará ser modificada constantemente.
public class CalculadoraDeDescontos
{
    public decimal CalcularEstudante(decimal valor) 
    { 
        return valor * 0.90m; // 10% de desconto
    }

    public decimal CalcularBlackFriday(decimal valor) 
    { 
        // ALERTA DE VIOLAÇÃO: Alterar a classe para adicionar novas regras
        // coloca em risco o método de estudante que já funcionava.
        return valor * 0.50m; // 50% de desconto
    }
}
```

---

###  Abordagem Correta (Aplicação do OCP)

Para aplicar o OCP, criamos uma interface que serve como contrato para qualquer regra de desconto futura:

```csharp
// Abstração que define o comportamento padrão para qualquer tipo de desconto
public interface IDesconto
{
    decimal Aplicar(decimal valor);
}
```

Agora, cada regra de desconto é isolada em sua própria classe independente:

```csharp
public class DescontoEstudante : IDesconto
{
    public decimal Aplicar(decimal valor) => valor * 0.90m;
}

public class DescontoBlackFriday : IDesconto
{
    public decimal Aplicar(decimal valor) => valor * 0.50m;
}

// Quer um desconto novo para Clientes VIP? Basta criar uma nova classe:
public class DescontoVIP : IDesconto
{
    public decimal Aplicar(decimal valor) => valor * 0.80m;
}
```

A nossa calculadora principal fica completamente protegida de alterações futuras, pois ela não conhece classes concretas, apenas a interface:

```csharp
public class CalculadoraDeDescontos
{
    public decimal Processar(decimal valor, IDesconto regraDeDesconto)
    {
        // O polimorfismo resolve qual desconto aplicar em tempo de execução.
        // Nunca mais precisaremos abrir esta classe para adicionar descontos!
        return regraDeDesconto.Aplicar(valor);
    }
}
```

---

* **Aberto para Extensão**: Capacidade de criar novas estratégias de desconto (Ex: `DescontoFuncionario`, `DescontoNatal`) criando novas classes.
* **Fechado para Modificação**: A classe central `CalculadoraDeDescontos` e os descontos antigos não são tocados, evitando falhas colaterais no sistema.

---

###  Abordagem Correta (Aplicação do OCP)

Para aplicar o OCP de forma eficiente, introduzimos uma camada de abstração utilizando um contrato (Interface). Esta interface define o comportamento padrão que qualquer nova fruta deve atender, desvinculando a execução da implementação.

```csharp
// Abstração que define o contrato de comportamento para o domínio de frutas
public interface IFruta
{
    void Preparar();
}
```

A partir desta abstração, novos comportamentos são adicionados criando-se novas classes que herdam e implementam o contrato, garantindo que o ecossistema esteja aberto para extensões:

```csharp
public class BananaAmarela : IFruta
{
    public void Preparar() 
    { 
        // Lógica específica para o processamento de bananas 
    }
}

public class MamaoGrande : IFruta
{
    public void Preparar() 
    { 
        // Lógica específica para o processamento de mamões 
    }
}
```

A classe responsável por gerenciar a execução passa a depender exclusivamente da interface, tornando-se completamente imune a futuras adições de novos tipos de frutas no sistema:

```csharp
public class EngineDeProcessamento
{
    public void Executar(IFruta fruta) 
    {
        // Polimorfismo: O método correto é invocado em tempo de execução 
        // sem que esta classe precise conhecer os detalhes específicos da fruta.
        fruta.Preparar(); 
    }
}
```

---

* **Aberto para Extensão**: Capacidade de acoplar novas variedades de frutas (Ex: Maçã, Morango) criando novas classes.
* **Fechado para Modificação**: O motor de processamento (`EngineDeProcessamento`) e as regras das frutas anteriores permanecem intocados e protegidos contra bugs colaterais.

---

* **Aberto para Extensão**: Criar fruta nova (Mamão, Maçã, Dinossauro).
* **Fechado para Modificação**: Nunca mais mexer no código da Banana que já funcionava.

---

## Liskov Substitution Principle

Qualquer subclasse deve poder substituir sua classe-mãe **sem quebrar o comportamento esperado** pelo código que está usando essa classe.

Em outras palavras: se o seu código funciona recebendo um objeto do tipo `Base`, ele tem que continuar funcionando do mesmo jeito se você trocar esse objeto por qualquer subclasse de `Base` — sem surpresas, sem exceções escondidas, sem comportamento diferente do esperado.

Imagine uma classe `Retangulo`, com `Largura`, `Altura` e um método `CalcularArea`. Faz sentido criar uma classe `Quadrado` que herda de `Retangulo`, já que todo quadrado é um retângulo, certo? O problema aparece na implementação: como num quadrado largura e altura são sempre iguais, `Quadrado` sobrescreve os setters para que, ao mudar a `Largura`, a `Altura` mude junto (e vice-versa).

Agora pensa num código que recebe um `Retangulo` e faz isso:
```
retangulo.Largura = 5;
retangulo.Altura = 10;
// espera Area = 50
```
Se esse `retangulo` na verdade for um `Quadrado`, a `Altura` também vira 5 quando a `Largura` é setada, e a área sai errada. O código que usa `Retangulo` parou de funcionar como esperado só porque recebeu uma subclasse — isso é quebra de LSP.

A solução geralmente passa por repensar a hierarquia: talvez `Quadrado` e `Retangulo` nem devessem ter relação de herança entre si, e sim implementar uma interface comum, como `IFormaGeometrica`, cada um com sua própria regra de cálculo de área. Assim nenhuma subclasse "mente" sobre o comportamento da outra.

---

---

## Interface Segregation Principle

Nenhuma classe deve ser obrigada a implementar métodos que ela não usa.

Isso quebra quando alguém cria uma interface "faz-tudo", pensando em deixar tudo centralizado, e acaba forçando classes completamente diferentes a implementar coisas que não fazem sentido pra elas.

Imagine uma interface `ITrabalhador`, com os métodos `Trabalhar`, `Comer` e `BaterPonto`. Funciona bem para uma classe `Funcionario`. Agora seu chefe pede um sistema que também controle `Robos` que trabalham na linha de produção, e você reaproveita a mesma interface `ITrabalhador` pra eles. Só que `Robo` não come, e não bate ponto — mas como ele implementa `ITrabalhador`, é obrigado a ter os métodos `Comer` e `BaterPonto` mesmo assim (geralmente vazios ou lançando uma exceção `NotImplementedException`). Isso é cheiro de interface mal segregada.

A solução é quebrar a interface grande em interfaces menores e mais específicas: `ITrabalhavel` (com `Trabalhar`), `IComestivel` (com `Comer`), `IPontoEletronico` (com `BaterPonto`). `Funcionario` implementa as três; `Robo` implementa só `ITrabalhavel`. Cada classe implementa só o que realmente faz sentido pra ela, e nenhuma fica carregando método inútil.

---

---

## Dependency Inversion Principle

Módulos de alto nível não devem depender de módulos de baixo nível. Ambos devem depender de **abstrações**.

A quebra mais comum acontece quando uma classe importante do seu sistema cria, na mão, uma instância de outra classe bem específica e fica "amarrada" a ela.

Imagine uma classe `Notificador`, responsável por avisar o usuário sobre alguma coisa. Dentro dela, você instancia diretamente um `EmailSender` e chama `EmailSender.Enviar(mensagem)`. Funciona, até seu chefe pedir: "agora quero que também seja possível notificar por SMS". Como `Notificador` está colado no `EmailSender`, você vai ter que abrir a classe e ficar fazendo `if/else` pra decidir se manda e-mail ou SMS — e cada novo canal de notificação (push, WhatsApp...) vai exigir abrir `Notificador` de novo.

A solução é inverter a dependência: em vez de `Notificador` depender direto de `EmailSender`, ele passa a depender de uma abstração, `ICanalDeEnvio`, com um método `Enviar(mensagem)`. `EmailSender` e `SmsSender` implementam essa interface, e `Notificador` recebe (via construtor, por exemplo) qual canal usar, sem saber nem se importar com os detalhes de implementação. Tanto o módulo de alto nível (`Notificador`) quanto os de baixo nível (`EmailSender`, `SmsSender`) passam a depender da mesma abstração, e não um do outro diretamente.
