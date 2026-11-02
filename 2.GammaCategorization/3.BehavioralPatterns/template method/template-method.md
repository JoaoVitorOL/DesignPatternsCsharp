<a id="inicio"></a>
# Template Method — esqueleto fixo e etapas variáveis

## Sumário
[↑ Voltar ao início](#inicio)

- [O que o padrão faz](#sec-001)
- [Como implementar](#sec-002)
- [Relação com o projeto](#sec-003)
- [Implementação mínima em C#: importação de arquivos](#sec-004)
- [Variação funcional sem subclasses](#sec-005)
- [Papéis técnicos no exemplo](#sec-006)


**Família GoF:** comportamental. **Escopo predominante:** classe, com etapas redefinidas por subclasses.

<a id="sec-001"></a>
## O que o padrão faz

[↑ Voltar ao início](#inicio)

Define a sequência de um algoritmo numa classe base e permite que subclasses redefinam algumas etapas. O método-template fixa o fluxo; as operações primitivas e hooks são os pontos de variação.

<a id="sec-002"></a>
## Como implementar

[↑ Voltar ao início](#inicio)

Uma classe abstrata ProcessadorRelatorio pode definir Processar() que chama LerDados(), Formatar() e Salvar(). Subclasses fornecem as etapas variáveis sem alterar a ordem. A classe base deve documentar invariantes e pontos que podem ser sobrescritos.

A variação usa herança, por isso muitas combinações podem tornar a hierarquia rígida. Strategy ou delegates são opções quando a composição precisa ser trocada em runtime.

<a id="sec-003"></a>
## Relação com o projeto

[↑ Voltar ao início](#inicio)

Não há implementação de Template Method nas pastas de código do projeto; o exemplo é ilustrativo.


<a id="sec-004"></a>
## Implementação mínima em C#: importação de arquivos

[↑ Voltar ao início](#inicio)

O fluxo de importação é o mesmo para formatos diferentes: ler conteúdo, interpretar os registros e salvar. O que varia é a leitura/parsing do formato. A classe-base mantém a sequência no método não virtual Import; as subclasses implementam as operações primitivas.

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public abstract class DataImporter
    {
        public void Import(string path)
        {
            string content = Read(path);
            var records = Parse(content);
            Save(records);
        }

        protected abstract string Read(string path);
        protected abstract List<string> Parse(string content);

        protected virtual void Save(List<string> records)
        {
            Console.WriteLine($"Salvando {records.Count} registros.");
        }
    }

    public sealed class CsvImporter : DataImporter
    {
        protected override string Read(string path) => File.ReadAllText(path);
        protected override List<string> Parse(string content) =>
            content.Split(Environment.NewLine).ToList();
    }

A classe-base garante a ordem e concentra a política comum de salvar. Uma subclasse pode redefinir Save se o destino também variar; se todas as etapas variarem independentemente, subclasses combinatórias sinalizam que Strategy ou composição por delegates pode ser melhor. O método-template deve continuar controlando invariantes do algoritmo.


<a id="sec-005"></a>
## Variação funcional sem subclasses

[↑ Voltar ao início](#inicio)

Functional Template Method preserva a sequência, mas recebe as etapas variáveis como delegates ou funções. O coordenador chama, por exemplo, read(), parse(content) e save(records), sem exigir uma subclasse por formato.

Essa forma usa composição e pode facilitar testar cada etapa ou configurar pipelines em runtime. Ainda assim, o método coordenador deve definir ordem, tratamento de erro e invariantes. Se qualquer cliente puder fornecer etapas arbitrárias, a sequência talvez deixe de ter significado comum; não transforme um algoritmo fixo num callback framework sem necessidade.

<a id="sec-006"></a>
## Papéis técnicos no exemplo

Os nomes **Context**, **Strategy**, **Concrete Strategy** e semelhantes descrevem responsabilidades no desenho. O código não precisa usar esses nomes literalmente: o mapeamento abaixo identifica quem exerce cada papel no exemplo desta página.

[↑ Voltar ao início](#inicio)

| Papel no padrão | Classe do exemplo | O que representa no código |
|---|---|---|
| Abstract Class | **DataImporter** | Define a sequência fixa de importação e declara etapas variáveis. |
| Concrete Class | **CsvImporter** | Implementa as etapas específicas para ler e interpretar CSV. |
| Template Method | **DataImporter.Import()** | Controla a ordem geral e chama os passos especializados. |
| Cliente | **Código que cria CsvImporter e chama Import** | Inicia o algoritmo sem coordenar cada etapa interna. |

**Leitura técnica:** a classe base controla o fluxo e chama operações implementadas pela subclasse. A alternativa funcional da página fornece etapas como funções, mas não usa a estrutura clássica de Template Method.

<a id="sec-glossario"></a>
## Glossário
[↑ Voltar ao início](#inicio)

- **Template Method:** método que define a ordem fixa das etapas de um algoritmo.
- **Hook:** etapa opcional que pode ser personalizada por uma subclasse.
- **Classe base:** classe que fornece o esqueleto do algoritmo.
- **Inversão de controle:** o esqueleto chama etapas especializadas.
