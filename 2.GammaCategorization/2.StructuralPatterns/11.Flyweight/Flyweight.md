<a id="inicio"></a>
# Flyweight — compartilhar estado repetido

## Sumário
[↑ Voltar ao início](#inicio)

- [O que o padrão faz](#sec-001)
- [Exemplo: nomes repetidos](#sec-002)
- [Como implementar](#sec-003)
- [Relação com este repositório](#sec-004)
- [Implementação mínima em C#: floresta com tipos de árvore compartilhados](#sec-005)
- [Como saber se Flyweight vale o custo](#sec-006)
- [Variações e aprofundamentos](#sec-007)
- [72. Repeating User Names](#sec-008)
- [73. Text Formatting](#sec-009)
- [Coding Exercise 10: Flyweight Coding Exercise](#sec-010)
- [Papéis técnicos no exemplo](#sec-011)


**Família GoF:** estrutural. **Escopo predominante:** objetos com estado intrínseco compartilhado.

<a id="sec-001"></a>
## O que o padrão faz

[↑ Voltar ao início](#inicio)

Flyweight reduz memória quando muitos objetos guardam partes idênticas de estado. Separa o estado intrínseco, compartilhável, do estado extrínseco, específico de cada uso. Uma fábrica mantém/cacheia os flyweights para evitar instâncias duplicadas.

<a id="sec-002"></a>
## Exemplo: nomes repetidos

[↑ Voltar ao início](#inicio)

Em vez de cada usuário guardar duas strings próprias, uma tabela pode guardar tokens únicos e cada usuário armazenar apenas índices. Jane Smith e Jane Doe reutilizam a mesma entrada Jane; John Smith reutiliza Smith.

<a id="sec-003"></a>
## Como implementar

[↑ Voltar ao início](#inicio)

1. Meça onde existe repetição em grande escala.
2. Separe dados compartilháveis dos dados contextuais.
3. Armazene os valores compartilhados em um flyweight/cache.
4. Faça cada contexto referenciar o flyweight e carregue apenas seu estado exclusivo.

Em formatação de texto, em vez de um objeto de estilo por caractere, guarde intervalos (início/fim) associados a estilos compartilhados. Para poucos elementos, a indireção pode custar mais que a memória economizada; meça antes de otimizar.

<a id="sec-004"></a>
## Relação com este repositório

[↑ Voltar ao início](#inicio)

Não há arquivo de código Flyweight nas pastas de padrões. O exemplo da floresta desta página demonstra o estado compartilhado e contextual; os exemplos de nomes e formatação também podem ser lidos sem consultar outro documento.


<a id="sec-005"></a>
## Implementação mínima em C#: floresta com tipos de árvore compartilhados

[↑ Voltar ao início](#inicio)

Uma floresta pode conter milhares de árvores. Espécie, cor e textura tendem a se repetir; coordenadas são próprias de cada árvore. TreeType guarda o estado intrínseco compartilhável e Tree mantém o estado extrínseco (posição).

    using System;
    using System.Collections.Generic;

    public sealed class TreeType
    {
        public string Species { get; }
        public string Color { get; }
        public string Texture { get; }

        public TreeType(string species, string color, string texture)
        {
            Species = species;
            Color = color;
            Texture = texture;
        }

        public void Draw(int x, int y) =>
            Console.WriteLine($"{Species} {Color} em ({x}, {y})");
    }

    public static class TreeTypeFactory
    {
        private static readonly Dictionary<(string, string, string), TreeType> cache = new();

        public static TreeType Get(string species, string color, string texture)
        {
            var key = (species, color, texture);
            if (!cache.TryGetValue(key, out var type))
            {
                type = new TreeType(species, color, texture);
                cache.Add(key, type);
            }
            return type;
        }
    }

    public sealed class Tree
    {
        private readonly int x, y;
        private readonly TreeType type;

        public Tree(int x, int y, TreeType type)
        {
            this.x = x;
            this.y = y;
            this.type = type;
        }

        public void Draw() => type.Draw(x, y);
    }

Ao plantar árvores, a floresta pede TreeTypeFactory.Get e guarda a referência retornada junto com a posição. Muitas árvores da mesma espécie recebem o mesmo TreeType. A fábrica é parte importante: sem cache, cada chamada poderia criar uma cópia e não haveria compartilhamento.

O estado compartilhado deve ser imutável ou tratado como somente leitura; alterar a textura de uma instância afetaria todas as árvores que a reutilizam. Flyweight reduz duplicação, mas adiciona indireção, lookup e gestão de cache. Compare memória e custo de acesso com uma representação direta antes de aplicar.


<a id="sec-006"></a>
## Como saber se Flyweight vale o custo

[↑ Voltar ao início](#inicio)

Estime quantos objetos existem, quanto estado repetido cada um guarda e quanto custa compartilhá-lo. Uma floresta de dez árvores não precisa de cache; milhões de árvores que repetem textura e espécie podem justificar. Flyweight economiza memória ao custo de lookup, indirection e uma política de cache.

A separação intrínseco/extrínseco precisa ser explícita: espécie/cor/textura são comuns e pertencem ao TreeType; posição x/y muda por árvore e permanece no Tree. Se um campo contextual for colocado por engano no objeto compartilhado, uma alteração para uma árvore pode afetar todas as árvores daquele tipo. Estado intrínseco costuma ser imutável.

A fábrica deve definir identidade e igualdade dos flyweights: quais campos compõem a chave? Como limitar o cache se as combinações forem ilimitadas? Meça memória e CPU com dados representativos antes de adotar otimização.

<a id="sec-007"></a>
## Variações e aprofundamentos

[↑ Voltar ao início](#inicio)

As seções abaixo reúnem variações, exemplos e decisões que complementam a explicação principal deste padrão. Elas preservam a linguagem e o contexto do material didático original.

<a id="sec-008"></a>
## 72. Repeating User Names

[↑ Voltar ao início](#inicio)


Repeating User Names mostra um problema simples: várias pessoas podem compartilhar partes do nome. Se cada objeto `User` guarda strings completas, muitos pedaços se repetem.

Exemplo:

```text
Jane Smith
John Smith
Jane Doe
```

Os tokens `Jane` e `Smith` aparecem mais de uma vez. Uma versão flyweight pode guardar uma tabela de strings únicas e fazer cada usuário referenciar índices.

Mapa mental:

```text
tabela compartilhada:
  0 -> Jane
  1 -> Smith
  2 -> John
  3 -> Doe

usuarios:
  [0, 1]
  [2, 1]
  [0, 3]
```

O ganho aparece em escala. Para três usuários, isso é exagero. Para milhões de registros com nomes repetidos, pode fazer sentido.

---

<a id="sec-009"></a>
## 73. Text Formatting

[↑ Voltar ao início](#inicio)


Text Formatting mostra Flyweight em formatação de texto. O erro ingênuo é guardar um objeto de formatação para cada caractere.

```text
caractere 0 -> bold=false, italic=false
caractere 1 -> bold=false, italic=false
caractere 2 -> bold=true, italic=false
```

Uma alternativa mais econômica é guardar intervalos de formatação:

```text
texto inteiro:
  "hello world"

formatacoes:
  intervalo 0..4 -> bold=true
  intervalo 6..10 -> italic=true
```

O caractere não precisa carregar todas as opções. O estado compartilhado ou agrupado descreve regiões.

A frase para guardar:

> Em Flyweight, a economia vem de não repetir estado que pode ser referenciado ou calculado a partir de uma estrutura compartilhada.

---

<a id="sec-010"></a>
## Coding Exercise 10: Flyweight Coding Exercise

[↑ Voltar ao início](#inicio)


O exercício deve treinar a separação entre texto bruto e metadados de formatação. A solução esperada normalmente mantém o conteúdo principal simples e guarda tokens, intervalos ou referências para representar variações.

Checklist mental:

- encontre o dado repetido;
- separe o que é compartilhável do que é contextual;
- evite criar um objeto pesado por item minúsculo;
- preserve uma API confortável para o cliente;
- confirme se a otimização faz sentido para o volume esperado.

---

<a id="sec-011"></a>
## Papéis técnicos no exemplo

Os nomes **Context**, **Strategy**, **Concrete Strategy** e semelhantes descrevem responsabilidades no desenho. O código não precisa usar esses nomes literalmente: o mapeamento abaixo identifica quem exerce cada papel no exemplo desta página.

[↑ Voltar ao início](#inicio)

| Papel no padrão | Classe do exemplo | O que representa no código |
|---|---|---|
| Flyweight | **TreeType** | Guarda características compartilháveis, como espécie, cor e textura. |
| Fábrica/registro | **TreeTypeFactory** | Reutiliza instâncias de TreeType para dados intrínsecos iguais. |
| Contexto/ocorrência | **Tree** | Guarda dados individuais, como posição, e referencia um TreeType compartilhado. |
| Cliente | **Código que cria as árvores** | Pede tipos à fábrica e cria ocorrências com dados extrínsecos. |

**Leitura técnica:** TreeType só economiza memória se não guardar a posição individual da árvore. Dados compartilhados pertencem ao flyweight; dados que variam por ocorrência ficam no contexto.

<a id="sec-glossario"></a>
## Glossário
[↑ Voltar ao início](#inicio)

- **Estado intrínseco:** dado estável que pode ser compartilhado entre usos.
- **Estado extrínseco:** dado variável fornecido pelo cliente a cada uso.
- **Flyweight:** objeto leve que mantém estado intrínseco compartilhado.
- **Fábrica de flyweights:** reutiliza instâncias em vez de criar duplicatas.
