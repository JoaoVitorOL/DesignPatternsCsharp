# Atividade 15: Observer

## Exercício 1: Aplicações

### Para cada cenário abaixo, indique se o padrão Observer é apropriado ou não e justifique em 2–3 frases.

1. Uma estação meteorológica publica novas medições e vários displays (temperatura, estatísticas, previsão) precisam se atualizar automaticamente, sem que a estação conheça cada display.

- O padrão Observer é adequado. Pois, lida com eventos espcíficos que devem ser notificados a outras partes do sistema e são algo que pode acontecer a qualquer momento. 

2. Uma interface gráfica em que um botão pode ter vários listeners e o modelo precisa avisar as views quando muda, permitindo novos interessados sem alterar o modelo.

- Observers fazem sentido. Já que podem ser usados para realizar uma ação em cadeia para mudar o visual gráfico do site, por meio dos componentes inscritos a um mesmo evento de interface.

3. Uma newsletter em que um número variável de assinantes recebe o conteúdo novo e pode assinar/cancelar a qualquer momento.

- o padrão Observer faz sentido. Pois, é um acontencimento em que o sistema não tem controle ou previsibilidade sobre. Assim, é bem melhor que tenha um evento associado a este acontecimento.


4. Uma classe Configuracao que guarda valores lidos uma única vez por um único consumidor fixo, sempre de forma direta e imediata, sem notificações.

- Observer não faz sentido. Não há um evento e nem algúem para notificar sobre algo..


5. Uma classe Ponto, com apenas x e y, que só guarda coordenadas e nunca avisa ninguém sobre mudanças.
- Observer não faz sentido. Não há um evento e nem algúem para notificar sobre algo.


## Exercício 2: Rastreando o anti-pattern em um projeto real

1. A EstacaoMeteorologica guarda referências a DisplayTemperatura e DisplayEstatisticas. Por que isso é um acoplamento forte entre sujeito e observadores?

Dependência de classes concretas: A EstacaoMeteorologica instancia e conhece diretamente os tipos concretos (DisplayTemperatura e DisplayEstatisticas) por meio de atributos privados e fixos (private final DisplayTemperatura ...).

Violação do princípio de inversão de dependência: A estação depende diretamente de implementações específicas e detalhadas de displays, em vez de depender de uma abstração (como uma interface genérica Observador).

Rigidez na manutenção: Se houver qualquer alteração nos métodos de atualização ou na assinatura dos displays, a classe EstacaoMeteorologica precisará ser modificada e recompilada.

2. O que é preciso alterar na estação para adicionar um DisplayUmidade? Liste os pontos tocados.

Para adicionar um novo display de umidade, você precisaria alterar diretamente a classe EstacaoMeteorologica nos seguintes pontos:

Atributos: Adicionar o campo com a nova instância do display (private final DisplayUmidade displayUmidade = new DisplayUmidade();).

Método de notificação (setMedicoes): Adicionar a chamada explícita para atualizar o novo display dentro da rotina de medições (displayUmidade.atualizar(umidade);).

Criação da classe: Criar a nova classe DisplayUmidade implementando sua própria lógica de exibição.

3. Por que não dá para registrar/remover um display em tempo de execução? O que faltaria para permitir isso?

Por que não dá: Os displays estão fixos em atributos declarados com new diretamente no corpo da classe EstacaoMeteorologica, e suas chamadas estão hardcoded dentro do método setMedicoes. Não há estruturas dinâmicas (como uma List) para gerenciar os observadores em tempo de execução.

O que faltaria para permitir:

Uma interface comum (Observador ou Observer): Todos os displays deveriam implementar uma interface padronizada com um método genérico (ex: atualizar(...)).

Uma lista interna de observadores na estação: A EstacaoMeteorologica precisaria de uma coleção (List<Observador>) em vez de referências fixas.

Métodos de gerenciamento: Adicionar métodos como adicionarObservador(Observador o) e removerObservador(Observador o) para permitir o registro e a remoção dinâmica de qualquer display em tempo de execução.



4. Proponha uma refatoração com o padrão Observer, deixando explícitos:
a interface de observador (Observador, com atualizar(...));
o sujeito (Sujeito), que registra, remove e notifica;
os observadores concretos (DisplayTemperatura, DisplayEstatisticas).