
## 1. O que o Decorator representa?

O **Decorator** é um padrão de projeto estrutural que permite adicionar novas responsabilidades a um objeto de forma dinâmica, sem modificar sua estrutura original e sem recorrer à criação de subclasses para cada combinação possível.

* **O Problema:** O uso de herança para adicionar recursos opcionais gera uma explosão combinatória de subclasses ($2^N$), tornando o código rígido e estático em tempo de compilação.
* **A Solução:** Envolver o objeto principal em camadas sucessivas de objetos decoradores que compartilham a mesma interface, delegando as chamadas e somando comportamentos em tempo de execução.

## 2. Em que situações se usa?

* Quando funcionalidades opcionais precisam ser combinadas livremente em tempo de execução.
* Quando o uso de herança resultaria em uma hierarquia complexa, duplicada ou inviável de subclasses.
* Quando é necessário estender o comportamento de um objeto de maneira totalmente transparente para o código cliente.

## 3. Como implementar passo a passo

* **Interface Comum:** Defina o contrato que será compartilhado pelo objeto base e pelos decoradores.
* **Componente Concreto:** Implemente a classe base com o comportamento fundamental do negócio.
* **Decorator Abstrato:** Crie uma classe que implementa a interface e armazena internamente uma referência para o componente encapsulado.
* **Decorators Concretos:** Estenda o decorator abstrato para acrescentar novos comportamentos (antes ou depois de delegar a execução para o objeto interno).

## 4. Exemplo Prático (Cafeteria)

Em um sistema de cafeteria, em vez de criar dezenas de classes estáticas para cada mistura de ingredientes, você instancia a bebida base e a envolve dinamicamente com os complementos desejados:
`Bebida meuCafe = new Chantilly(new Leite(new CafeExpresso()));`
Cada camada decoradora intercepta os métodos (como `getPreco()` ou `getDescricao()`), soma o seu próprio valor/texto ao resultado da camada interna e retorna o total de forma flexível.

---

Tem alguma dúvida sobre como o Decorator se diferencia de outros padrões estruturais como o Adapter?
