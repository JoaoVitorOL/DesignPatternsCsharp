# Atividade 14: Mediator

## Para cada cenário abaixo, indique se o padrão Mediator é apropriado ou não e justifique em 2–3 frases.

1. Uma sala de chat em que dezenas de usuários trocam mensagens e as regras de conversa (quem fala com quem, sons, privacidade) mudam com frequência, sem que cada usuário conheça os demais.

 
-  O padrão Mediator faz sentido, já que ajudaria a resolver o problema de cada usuário precisar conhecer seu vizinho ou uma lista inteira de usuários para conversarem entre si.

2. Uma tela de diálogo com muitos widgets que reagem uns aos outros: ao marcar um Checkbox, um Botão é habilitado e um Campo de texto é limpo, formando uma teia de dependências.
 
-  O uso do Mediator é jusitifcável. Pois, tratam-se de objetos diferentes que se comunicam entre si, gerando uma dependência de referência entre os elementos, caso não haja aplicação do padrão.


 3. Uma central de despacho de entregas que precisa coordenar motoristas, pedidos e entregas, decidindo qual pedido vai para qual motorista conforme as regras mudam.

  
-  O uso do Mediator faz sentido. Porque, nesse caso, há um objeto que precisa atuar como uma central de controle principal, como torres de comunicação entre aviãos. Apenas uma autoridade deve comandar os diferentes objetos, e estes, não precisam se conhecer, apenas se comunicam com a central.


 4. Uma biblioteca em que a classe Impressora se comunica com um único Spooler fixo, sempre pelo mesmo método simples, e essa interação nunca cresce nem ganha novos objetos.

 
-   Não é necessário um Mediator, já que a comunicação neste caso é simples e direta, tendo apenas uma instância de cada classe.


5. Uma classe Ponto, com apenas x e y, que só guarda coordenadas e nunca participa de interações com outros objetos.

-   Não é necessário um Mediator, já que há comunição entre os prórpios objetos ou classes externas.





## Exercício 2: Rastreando o anti-pattern em um projeto real


1. Usuario guarda uma lista de contatos (outros Usuario). Para N usuários, quantas ligações podem existir? O que isso indica sobre o acoplamento?

- Do ponto de vista matemática, isto significa que as ligações crescem em um ritmo de N × (N − 1);
  Isso significa que, cada novo usuário deve ser apresentado a todos os usuários já existentes, tanto para adicionar, quanto para remover um usuário da lista de contatos da sala de chat. Uma mesma pessoa deve ser referênciada múltiplas vezes ao longo do código cliente para que todas as outras pessoa passem a reconhecê-la.


2. O que é preciso alterar para incluir um novo usuário (por exemplo, o Diego)? Liste os pontos tocados. E para remover alguém, que problemas podem sobrar?

 - Para adicionar um novo usuário, todas as outras pesssoas existentes devem passar reconhecer esse novo contato em sua lista. O Processo no código se torna cada vez mais longo e forma uma estrutura em malha que fica simplesmente horrorosa de manter nesse caso. Para reomver um contato, o mesmo processo deve ser feito, mas ao invés de adicioanr em cada uma das listas individuais dos usuários, a pessoa deve ser apagada de todas elas. Caso, o desenvolvedor remova alguém de uma das listas, mas se esquecer de tirar de uma outra, isso irá gerar inconsistência de dados e pré-disposição à falhas.



3. As regras de conversa (todos recebem? só o admin fala? mensagem privada?) não estão em nenhum lugar claro. Onde elas deveriam morar? Por quê?

- Essas regras de negócio devem ficar dentro de um objeto central de comunicação entre o usuário individual e resto da sala de chat. Pois, isso centraliza a comunicação e permite que usuários interajam entre si sem precisarem se conhecer por referências gigantes no código de sua classe. O usuário fala com um mediador e este repassa o recado ao destinatário.











