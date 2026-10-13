 # Atividade 12: Strategy

 ## Exercício 1: Aplicações

 - Para cada cenário abaixo, indique se o padrão Strategy é apropriado ou não e justifique em 2–3 frases
 
 
1. Uma loja virtual calcula o frete por modalidade (Sedex, PAC, Retirada) e precisa permitir novas modalidades sem alterar o Checkout.

   - O strategy é adequado. Pois, um mesmo algoritmo precisará de mais de um tipo de implemententação (Sedex, PC, Retirada)



2. Um sistema de pagamentos aceita Pix, cartão e boleto, escolhidos pelo cliente no momento da compra, cada um com um fluxo próprio.

   - O strategy é adequado. Pois, há múltiplas opções para uma mesma funcionalidade.


3. Um jogo de batalha permite ao jogador escolher entre fórmulas de dano diferentes (bruto, crítico, à distância), combináveis com a mesma lógica de combate.

   - O Strategy é adequado. Porque pode ser usado para aplicar o dano usando um mesmo método por caminhos diferentes no 
   código.

4. Uma classe Relatorio que sempre gera o mesmo PDF, com um formato fixo que nunca muda e sem variações de comportamento.

   - O padrâo strategy não é necessário nessa ocasião, já que não possui nenhuma mudança.


5. Uma classe Ponto com apenas x e y, que só soma coordenadas e nunca ganha algoritmos alternativos.

   - Strategy não é necessário nesse caso. Pois, o código não recebe nenhuma variação.



## Exercício 2: Rastreando o anti-pattern em um projeto real


1. Localize todas as decisões de comportamento baseadas em TipoCliente. Quantas classes repetem o mesmo switch? O que isso indica sobre o design?

- 3 classes repetem o mesmo switch ( CalculadoraDesconto, CalculadoraFrete e RelatorioPedido).
- Isso indiica que o sistema é frágil a mudanças. Qualquer alteração necessária no código deve tocar em múltiplos trechos de código e em classes diferentes ao mesmo tempo, tornando-o muito mais sucetível a falhas e comportamentos inesperados durante implementações.
 


2. O que é preciso alterar para adicionar um novo tipo de cliente (por exemplo, PARCEIRO)? Liste os arquivos que seriam tocados.

- Seria necessário criar uma interface de strategy que 


3. Por que esse código é difícil de testar isoladamente por regra (desconto, frete, formatação)? Como as condicionais atrapalham?

Sem o padrão Strategy, testar o código por regra é difícil porque:Acoplamento rígido: RelatorioPedido instancia diretamente as calculadoras (new), impedindo testar a formatação isoladamente.   Condicionais (switch) espalhadas: A lógica baseada no tipo de cliente está duplicada em múltiplos arquivos.   Impacto das condicionais: Qualquer alteração exige modificar várias classes ao mesmo tempo, e os testes precisam validar múltiplos caminhos de desvio, tornando-os complexos e frágeis. 

4. Proponha uma refatoração com o padrão Strategy, deixando explícitos:
a interface de estratégia;
as estratégias concretas;
o contexto que delega;
como o RelatorioPedido passa a depender apenas da abstração.

-  A interface de estratégia: A interface IUsuario, que define o contrato unificado para cálculo de desconto, frete e  obtenção de etiqueta.

- As estratégias concretas:
As classes UsuarioComum, UsuarioVIP e UsuarioCorporativo, que implementam IUsuario e encapsulam isoladamente as regras de negócio de cada perfil de cliente.

- O contexto que delega:
A classe RelatorioPedido, que monta a estrutura final do relatório delegando os cálculos diretamente para a estratégia injetada, eliminando os switch condicionais.

- Como o RelatorioPedido passa a depender apenas da abstração:
Por meio de injeção de dependência via construtor, onde o relatório recebe a interface IUsuario em vez de instanciar classes concretas. Assim, ele interage apenas com o contrato abstrato, desconhecendo o tipo específico de cliente.





5. Depois de refatorar, como fica a troca de comportamento em runtime? Dê um exemplo (ex.: aplicar uma regra promocional e depois voltar à padrão).

Se a classe RelatorioPedido possuir um método modificador (setEstrategia), podemos alterar o comportamento do mesmo objeto dinamicamente:

Java
Pedido pedido = new Pedido(200.0, 2.0, "sul");

// 1. Inicia com a regra padrão (comum)
RelatorioPedido relatorio = new RelatorioPedido(new UsuarioComum());
System.out.println(relatorio.formatar(pedido)); 

// 2. Aplica uma regra promocional em runtime (ex: VIP) alterando a estratégia
relatorio.setEstrategia(new UsuarioVIP());
System.out.println(relatorio.formatar(pedido)); 

// 3. Volta à regra padrão
relatorio.setEstrategia(new UsuarioComum());
System.out.println(relatorio.formatar(pedido)); 
Caso o contexto não tenha um setter, a troca é feita instanciando um novo contexto com a estratégia desejada (new RelatorioPedido(new UsuarioVIP())), aproveitando a flexibilidade do polimorfismo.

Entrega: um diagrama de classes (antes/depois), o código refatorado e um parágrafo justificando cada decisão de design.