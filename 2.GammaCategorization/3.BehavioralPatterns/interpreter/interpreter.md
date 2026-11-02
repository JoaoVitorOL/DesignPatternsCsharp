<a id="inicio"></a>
# Interpreter — avaliação de uma linguagem pequena

## Sumário
[↑ Voltar ao início](#inicio)

- [O que o padrão faz](#sec-001)
- [Como implementar](#sec-002)
- [Relação com o projeto](#sec-003)
- [Implementação mínima em C](#sec-004)
- [Variação por etapas: lexer, parser e avaliador](#sec-005)


**Família GoF:** comportamental. **Escopo predominante:** classes que representam regras e expressões de uma gramática.

<a id="sec-001"></a>
## O que o padrão faz

[↑ Voltar ao início](#inicio)

Interpreter representa as regras de uma linguagem por uma estrutura de expressões que sabe avaliar seu significado. É útil para gramáticas pequenas e estáveis, como fórmulas, filtros e regras configuráveis.

<a id="sec-002"></a>
## Como implementar

[↑ Voltar ao início](#inicio)

Defina tokens, transforme o texto em tokens (lexing), organize-os em uma árvore respeitando precedência (parsing) e avalie os nós. Para 1 + 2 * 3, a árvore deve ser Add(1, Multiply(2, 3)); sua avaliação resulta em 7. Cada nó composto avalia seus filhos.

ANTLR pode gerar lexer/parser a partir de uma gramática, reduzindo o parsing manual ao custo de configurar e integrar uma ferramenta. Entradas malformadas precisam produzir erros claros.

<a id="sec-003"></a>
## Relação com o projeto

[↑ Voltar ao início](#inicio)

Não há implementação de Interpreter entre os arquivos de código nas pastas de padrões. Esta página explica a implementação típica, sem atribuir ao repositório um exemplo inexistente. Para linguagens grandes, prefira parser especializado.


<a id="sec-004"></a>
## Implementação mínima em C#

[↑ Voltar ao início](#inicio)

A ideia pode ser demonstrada sem um parser completo: monte a expressão em objetos e avalie a árvore. NumberExpression é a folha; AddExpression e MultiplyExpression são nós compostos. A mesma estrutura recursiva usada para construir a expressão também define como calcular seu resultado.

    public interface IExpression
    {
        int Evaluate();
    }

    public sealed class NumberExpression : IExpression
    {
        private readonly int value;
        public NumberExpression(int value) => this.value = value;
        public int Evaluate() => value;
    }

    public sealed class AddExpression : IExpression
    {
        private readonly IExpression left, right;
        public AddExpression(IExpression left, IExpression right)
        {
            this.left = left;
            this.right = right;
        }
        public int Evaluate() => left.Evaluate() + right.Evaluate();
    }

    public sealed class MultiplyExpression : IExpression
    {
        private readonly IExpression left, right;
        public MultiplyExpression(IExpression left, IExpression right)
        {
            this.left = left;
            this.right = right;
        }
        public int Evaluate() => left.Evaluate() * right.Evaluate();
    }

    IExpression expression = new AddExpression(
        new NumberExpression(1),
        new MultiplyExpression(
            new NumberExpression(2),
            new NumberExpression(3)));
    int result = expression.Evaluate(); // 7

Este trecho constrói a árvore manualmente; não transforma texto em tokens. Um parser precisaria reconhecer operadores e parênteses e produzir os mesmos objetos, respeitando precedência. Essa diferença evita passar a impressão de que Interpreter, sozinho, já é um parser de texto.


<a id="sec-005"></a>
## Variação por etapas: lexer, parser e avaliador

[↑ Voltar ao início](#inicio)

O exemplo de árvore acima cria expressões manualmente. Para aceitar texto escrito pelo usuário, separe três responsabilidades:

1. Lexer transforma caracteres em tokens e registra posições de erro.
2. Parser transforma tokens numa árvore conforme precedência e parênteses.
3. Avaliador percorre a árvore e calcula ou executa o significado.

Para 1 + 2 * 3, o parser deve construir Add(Number(1), Multiply(Number(2), Number(3))). Não faça a avaliação enquanto ainda tenta adivinhar a sintaxe: separar as etapas permite reportar erro como “parêntese não fechado” sem confundir isso com falha de cálculo.

Uma gramática pequena pode usar parser recursivo manual. ANTLR gera lexer e parser a partir de uma gramática declarativa e é útil quando há muitas regras. O padrão Interpreter não obriga usar ANTLR nem mesmo texto; sua essência é representar regras e significado em objetos. Para linguagem extensa, geração de classes por regra pode tornar o design pesado; use parser apropriado.


<a id="sec-glossario"></a>
## Glossário
[↑ Voltar ao início](#inicio)

- **Gramática:** regras que definem expressões válidas em uma linguagem.
- **Expressão:** representação de uma operação ou valor segundo a gramática.
- **Parser:** transforma texto em uma estrutura de expressões.
- **Avaliação:** cálculo do resultado representado pela expressão.
- **AST:** árvore sintática abstrata que representa a estrutura da expressão.
