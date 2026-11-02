<a id="inicio"></a>
# Visitor — novas operações sobre tipos estáveis

## Sumário
[↑ Voltar ao início](#inicio)

- [O que o padrão faz](#sec-001)
- [Como implementar](#sec-002)
- [Relação com o projeto](#sec-003)
- [Implementação mínima em C#: área de figuras](#sec-004)
- [Variações e seus custos](#sec-005)


**Família GoF:** comportamental. **Escopo predominante:** objetos de elementos e operações visitor.

<a id="sec-001"></a>
## O que o padrão faz

[↑ Voltar ao início](#inicio)

Separa operações da estrutura de objetos. Cada elemento aceita um visitor, e o visitor concreto implementa uma operação para cada tipo. Adicionar operações fica mais simples sem espalhar lógica nos elementos.

<a id="sec-002"></a>
## Como implementar

[↑ Voltar ao início](#inicio)

Elementos implementam Accept(visitor) e chamam a sobrecarga específica, como visitor.Visit(Circle). A chamada resolve o tipo concreto (double dispatch). CalculateAreaVisitor e ExportXmlVisitor podem percorrer a mesma estrutura para produzir resultados distintos.

A troca é que novo tipo de elemento exige atualizar o contrato e os visitors. Visitor é útil quando tipos são estáveis e operações aumentam com frequência. Variantes reflexivas ou com dynamic reduzem sobrecargas, mas adiam erros de tipo.

<a id="sec-003"></a>
## Relação com o projeto

[↑ Voltar ao início](#inicio)

Não há implementação de Visitor nas pastas de código. Este exemplo descreve o despacho clássico, sem atribuí-lo a um arquivo local.


<a id="sec-004"></a>
## Implementação mínima em C#: área de figuras

[↑ Voltar ao início](#inicio)

O contrato de elemento define Accept. O visitante declara uma sobrecarga para cada tipo conhecido. Cada figura encaminha a visita para a sobrecarga correspondente; esse segundo despacho é o double dispatch que permite ao visitor tratar o tipo concreto sem um switch externo.

    using System;

    public interface IShapeVisitor
    {
        void Visit(Circle circle);
        void Visit(Rectangle rectangle);
    }

    public interface IShape
    {
        void Accept(IShapeVisitor visitor);
    }

    public sealed class Circle : IShape
    {
        public double Radius { get; }
        public Circle(double radius) => Radius = radius;
        public void Accept(IShapeVisitor visitor) => visitor.Visit(this);
    }

    public sealed class Rectangle : IShape
    {
        public double Width { get; }
        public double Height { get; }
        public Rectangle(double width, double height)
        {
            Width = width;
            Height = height;
        }
        public void Accept(IShapeVisitor visitor) => visitor.Visit(this);
    }

    public sealed class AreaVisitor : IShapeVisitor
    {
        public double Area { get; private set; }
        public void Visit(Circle circle) =>
            Area += Math.PI * circle.Radius * circle.Radius;
        public void Visit(Rectangle rectangle) =>
            Area += rectangle.Width * rectangle.Height;
    }

O cliente percorre uma lista de IShape e chama Accept(areaVisitor). Para adicionar outra operação, como exportar JSON, cria-se outro visitor; para adicionar Triangle, é preciso ampliar IShapeVisitor e implementar a nova sobrecarga em todos os visitors concretos. Essa troca de custos é a principal decisão de Visitor.


<a id="sec-005"></a>
## Variações e seus custos

[↑ Voltar ao início](#inicio)

- **Intrusive Visitor:** elementos expõem Accept e conhecem o contrato visitor; é o double dispatch clássico com boa verificação estática.
- **Reflective Visitor:** procura um método pelo tipo em runtime; reduz boilerplate, mas falhas surgem apenas durante a execução e refatorações são menos seguras.
- **Dynamic Visitor (DLR):** C# dynamic escolhe o método em runtime; simplifica sobrecargas em alguns cenários, com o mesmo custo de tipagem tardia.
- **Acyclic Visitor:** separa interfaces por tipo suportado para que um visitor não dependa de todos os elementos; aumenta contratos e verificações de capacidade.
- **Visitor Builder:** registra handlers por tipo e compõe uma tabela de visita; é uma conveniência de configuração, não uma intenção nova.

Reduções agregam resultados da árvore (contagem, área total); transforms constroem outra árvore (simplificação, conversão). Podem ser visitors ou recursões explícitas. Escolha conforme a quantidade de operações externas, a frequência de novos tipos e a clareza da estrutura.


<a id="sec-glossario"></a>
## Glossário
[↑ Voltar ao início](#inicio)

- **Element:** objeto da estrutura que pode receber uma operação de visitante.
- **Visitor:** objeto que implementa uma operação para cada tipo de elemento conhecido.
- **Double dispatch:** seleção da operação pelo tipo do visitante e do elemento.
- **Estrutura de objetos:** elementos percorridos ou processados pelo visitante.
