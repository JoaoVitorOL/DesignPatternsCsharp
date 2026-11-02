<a id="inicio"></a>
# Null Object — comportamento neutro explícito

## Sumário
[↑ Voltar ao início](#inicio)

- [O que o padrão faz](#sec-001)
- [Como implementar](#sec-002)
- [Relação com o projeto](#sec-003)
- [Implementação mínima em C](#sec-004)
- [Variações: Singleton Null Object e objeto dinâmico](#sec-005)


**Família:** padrão comportamental relacionado; Null Object não pertence ao catálogo GoF de 23 padrões.

<a id="sec-001"></a>
## O que o padrão faz

[↑ Voltar ao início](#inicio)

Fornece uma implementação neutra de um contrato para situações em que uma dependência é opcional. O cliente continua chamando a interface sem espalhar verificações de null.

<a id="sec-002"></a>
## Como implementar

[↑ Voltar ao início](#inicio)

Considere ILogger com Log(mensagem). ConsoleLogger escreve; NullLogger implementa Log sem produzir saída. O cliente recebe ILogger e não precisa perguntar se existe logger.

O comportamento neutro deve respeitar o contrato. Se uma dependência obrigatória está ausente, responder silenciosamente esconderia um erro. Como normalmente não possui estado, o objeto nulo pode ter instância compartilhada, mas Singleton não é requisito.

<a id="sec-003"></a>
## Relação com o projeto

[↑ Voltar ao início](#inicio)

Não há implementação de Null Object nas pastas de código deste repositório. O logger é um exemplo didático, não uma classe local.


<a id="sec-004"></a>
## Implementação mínima em C#

[↑ Voltar ao início](#inicio)

    using System;

    public interface ILogger
    {
        void Log(string message);
    }

    public sealed class ConsoleLogger : ILogger
    {
        public void Log(string message) => Console.WriteLine(message);
    }

    public sealed class NullLogger : ILogger
    {
        public static readonly NullLogger Instance = new NullLogger();
        private NullLogger() { }
        public void Log(string message) { }
    }

    public sealed class ReportService
    {
        private readonly ILogger logger;
        public ReportService(ILogger logger) => this.logger = logger;
        public void Generate()
        {
            logger.Log("Relatório gerado");
            // restante do caso de uso
        }
    }

Na configuração que não precisa registrar logs, injete NullLogger.Instance. ReportService mantém o mesmo fluxo e não precisa verificar null antes de Log. Em testes, é possível injetar um logger falso que registra mensagens e permite verificar a interação.

A instância única de NullLogger é uma conveniência porque o objeto não tem estado; o mecanismo de Null Object é a implementação neutra de ILogger. Se logger for obrigatório e a configuração esquecer de fornecê-lo, não substitua automaticamente a falha por silêncio: a aplicação pode falhar cedo com uma mensagem clara.


<a id="sec-005"></a>
## Variações: Singleton Null Object e objeto dinâmico

[↑ Voltar ao início](#inicio)

Uma implementação neutra sem estado pode expor uma instância compartilhada, como NullLogger.Instance. Isso evita criar muitos objetos equivalentes, mas Singleton é apenas a forma de compartilhar a instância; Null Object continua sendo a intenção de fornecer uma resposta neutra pelo contrato.

Um proxy dinâmico pode criar implementações vazias em runtime para interfaces maiores. Ele reduz classes repetitivas, mas respostas padrão ficam implícitas e podem mascarar operações que deveriam falhar. Prefira uma implementação explícita quando o contrato for pequeno ou quando o silêncio puder ocultar erro.

Defina a neutralidade método a método. Log pode não fazer nada; uma busca sem resultado pode retornar coleção vazia; uma operação de pagamento não deve retornar sucesso fictício só para evitar null.


<a id="sec-glossario"></a>
## Glossário
[↑ Voltar ao início](#inicio)

- **Null Object:** implementação com comportamento neutro para uma dependência opcional.
- **No-op:** operação válida que intencionalmente não produz efeito.
- **Sentinela:** valor ou objeto especial que representa ausência de opção.
- **Referência nula:** referência sem objeto associado.
