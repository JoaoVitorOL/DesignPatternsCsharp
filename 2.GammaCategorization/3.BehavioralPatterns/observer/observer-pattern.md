<a id="inicio"></a>
# Observer — notificações da estação meteorológica

## Sumário
[↑ Voltar ao início](#inicio)

- [O que o padrão faz](#sec-001)
- [Implementação corrigida](#sec-002)
- [Siga uma atualização da estação](#sec-003)
- [Arquivos para acompanhar](#sec-004)
- [Observer em aplicações reais: síncrono, assíncrono e ciclo de vida](#sec-005)
- [Variações de inscrição e notificação](#sec-006)
- [Papéis técnicos no exemplo](#sec-007)


**Família GoF:** comportamental. **Escopo predominante:** objetos publicadores e observadores inscritos.

<a id="sec-001"></a>
## O que o padrão faz

[↑ Voltar ao início](#inicio)

Cria uma relação um-para-muitos: quando o sujeito muda, notifica os observadores inscritos. O sujeito depende de uma abstração, não dos tipos concretos dos assinantes.

<a id="sec-002"></a>
## Implementação corrigida

[↑ Voltar ao início](#inicio)

Em [projeto_observer_corrigido](projeto_observer_corrigido), ISubscriber declara atualizar(temperatura, umidade, pressao). EstacaoMeteorologica guarda as medições e a lista de inscritos. setMedicoes atualiza os dados e chama notificar(); cada assinante recebe-os por callback.

DisplayTemperatura, DisplayPressao e DisplayEstatisticas implementam o contrato. Main registra os três, publica uma medição, remove o display de pressão e publica outra. É uma notificação push: os valores acompanham o evento. Compare com [projeto_observer_antipattern](projeto_observer_antipattern/observer-antipattern).

A lista mantém referências fortes; remova observadores quando deixam de ser necessários. Defina também como tratar erros de callback ou alterações na lista durante a notificação.


<a id="sec-003"></a>
## Siga uma atualização da estação

[↑ Voltar ao início](#inicio)

EstacaoMeteorologica é o sujeito porque possui as medições e controla quando elas mudam. Um display não consulta a estação em intervalos repetidos; ele se inscreve uma vez. Quando setMedicoes recebe novos dados, a estação primeiro atualiza seu próprio estado e depois chama cada ISubscriber.

O contrato comum permite que um novo display seja adicionado sem editar a estação. DisplayEstatisticas pode manter seu histórico para calcular média, enquanto DisplayPressao mostra apenas o valor atual. A estação não precisa saber dessas diferenças; só sabe que cada inscrito aceita atualizar(...).

A lista de inscritos é uma decisão prática importante: desinscrever evita notificações depois que um display deixa de ser usado. Em um app real também se decide se um assinante lento bloqueia o sujeito, se exceções interrompem outros assinantes e se os dados são enviados (push, como aqui) ou consultados (pull). Observer reduz conhecimento direto, mas o fluxo de controle fica menos óbvio porque a reação começa em uma notificação.


<a id="sec-004"></a>
## Arquivos para acompanhar

[↑ Voltar ao início](#inicio)

- [ISubscriber.java](projeto_observer_corrigido/ISubscriber.java) é o contrato de observador.
- [EstacaoMeteorologica.java](projeto_observer_corrigido/EstacaoMeteorologica.java) mantém estado e lista de inscritos.
- [DisplayTemperatura.java](projeto_observer_corrigido/DisplayTemperatura.java), [DisplayPressao.java](projeto_observer_corrigido/DisplayPressao.java) e [DisplayEstatisticas.java](projeto_observer_corrigido/DisplayEstatisticas.java) reagem de formas diferentes.
- [Main.java](projeto_observer_corrigido/Main.java) ilustra inscrição, notificação e remoção.


<a id="sec-005"></a>
## Observer em aplicações reais: síncrono, assíncrono e ciclo de vida

[↑ Voltar ao início](#inicio)

O callback do exemplo é síncrono: setMedicoes só termina depois que todos os displays atualizam. Isso é simples e mantém a ordem previsível, mas um assinante lento bloqueia a publicação. Publicar por fila/event bus assíncrono muda garantias: o estado pode avançar antes de um consumidor processar e falhas precisam ser monitoradas.

A inscrição mantém a referência do observador. Se a estação viver mais que um display e não houver desinscrição, o display pode permanecer na memória e continuar recebendo atualizações. Dispose ou um token de inscrição pode tornar o ciclo explícito. Em .NET, o keyword event controla quem pode invocar a notificação, mas não remove automaticamente o problema de ciclo de vida.

Observer deve comunicar que algo aconteceu; não é sempre adequado para uma solicitação que exige resposta imediata ou coordenação de muitos colegas. Para isso, um mediador ou chamada explícita pode ser mais fácil de entender. Decida também se os dados serão enviados no evento (push) ou consultados pelo assinante (pull).


<a id="sec-006"></a>
## Variações de inscrição e notificação

[↑ Voltar ao início](#inicio)

A estação local usa uma interface especial de assinante e envia todas as medições no callback (push). Um evento C# encapsula delegates e limita quem pode disparar a notificação. Em padrão pull, o observador recebe o aviso e consulta a estação para obter os valores atuais; isso reduz dados no evento, mas pode mostrar estado posterior ao evento original.

Weak Event evita que uma fonte de vida longa mantenha um assinante vivo apenas pela referência do delegate. Coleções observáveis notificam inclusão/remoção; notification de propriedades individuais é outro contrato. Inscrição declarativa pode usar interfaces/metadata para conectar handlers automaticamente, tornando a configuração mais implícita.

Observer bidirecional e dependências de propriedade são casos mais delicados: uma atualização pode provocar outra e formar ciclo. Use uma fonte de verdade, teste igualdade antes de propagar e controle reentrância. Para sujeitos de longa duração, forneça desinscrição/token de assinatura e defina o tratamento de exceções para que um observador com falha não silencie os demais inadvertidamente.

<a id="sec-007"></a>
## Papéis técnicos no exemplo

Os nomes **Context**, **Strategy**, **Concrete Strategy** e semelhantes descrevem responsabilidades no desenho. O código não precisa usar esses nomes literalmente: o mapeamento abaixo identifica quem exerce cada papel no exemplo desta página.

[↑ Voltar ao início](#inicio)

| Papel no padrão | Classe do exemplo | O que representa no código |
|---|---|---|
| Subject/Publisher | **EstacaoMeteorologica** | Mantém medições, registra assinantes e os notifica quando os valores mudam. |
| Observer/Subscriber | **ISubscriber** | Define a operação de atualização solicitada pelo sujeito. |
| Concrete Observers | **DisplayTemperatura**, **DisplayPressao**, **DisplayEstatisticas** | Reagem à atualização apresentando ou calculando uma visão das medições. |
| Cliente/composição | **Main** | Registra os displays, altera medições e demonstra as notificações. |

**Leitura técnica:** a estação depende de ISubscriber, não das classes de display concretas. Observadores podem ser inscritos ou removidos sem reescrever o Subject.

<a id="sec-glossario"></a>
## Glossário
[↑ Voltar ao início](#inicio)

- **Subject:** objeto que mantém estado e notifica observadores sobre mudanças.
- **Observer:** componente inscrito para receber notificações.
- **Inscrição:** vínculo que registra o interesse do observador.
- **Push:** notificação que já carrega os dados da mudança.
- **Pull:** modelo em que o observador consulta o sujeito após a notificação.
