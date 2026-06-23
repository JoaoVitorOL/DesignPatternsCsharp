using System;
using System.Collections.Generic;
using static System.Console;

namespace AulasSOLIDpatterns.Aula03_LiskovSubstituiton
{
    // O principio da Substituicao de Liskov diz que uma classe filha
    // deve poder ser usada no lugar da classe pai sem mudar a expectativa de quem consome o objeto.
    // Em outras palavras: se um metodo funciona com Rectangle,
    // ele deveria continuar funcionando de forma previsivel com Square.
    //
    // A palavra importante aqui e "contrato".
    // Rectangle passa a ideia de que Width e Height sao lados independentes.
    // Se Square herda de Rectangle, ele deveria respeitar essa mesma ideia.

    // ====================================
    //   Classe de Retangulo
    // ====================================
    public class Rectangle
    {
        // O contrato implicito desta classe e:
        // largura e altura podem ser tratadas como valores independentes.
        //
        // Exemplo esperado:
        // rect.Width = 4;
        // rect.Height = 5;
        // Resultado esperado: um retangulo 4 x 5.
        public int Width {get; set;}
        public int Height {get; set;}

        public Rectangle()
        {
             
        }

        public Rectangle(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public override string ToString()
        {
            return $"{nameof(Width)}: {Width}, {nameof(Height)}: {Height}";
        }
    }



    // ==============================================
    //   Classe de Quadrado (herda de Retangulo)
    // ==============================================
    public class Square : Rectangle
    {
        // Aqui esta o problema.
        //
        // "new" aqui NAO significa criar objeto.
        // Nesse caso, "new" significa "ocultar" o membro herdado da classe pai.
        // Ou seja: Square cria uma nova versao de Width e Height,
        // escondendo as propriedades que ja existiam em Rectangle.
        //
        // Isso e diferente de substituir o comportamento da classe pai.
        // Com "new", a escolha de qual propriedade sera usada depende do TIPO DA VARIAVEL:
        //
        // Square sq = new Square();      -> usa Square.Width
        // Rectangle sq2 = new Square();  -> usa Rectangle.Width
        //
        // Entao passamos a ter duas "Width":
        // uma visivel quando o objeto e enxergado como Rectangle
        // e outra visivel quando o objeto e enxergado como Square.
        //
        // Alem disso, a regra muda:
        // ao definir Width, ele tambem muda Height;
        // ao definir Height, ele tambem muda Width.
        // Isso quebra a expectativa de quem usa Rectangle.
        public new int Width
        {
            set { base.Width = base.Height = value;}
        }

        public new int Height
        {
            set { base.Width = base.Height = value;}
        }

    }

  




    public class Demo
        {
            static public int Area(Rectangle r)=> r.Width * r.Height;

            public static void Main(string[] args)
            {
                Rectangle rc = new Rectangle(2,3);
                WriteLine($"{rc} has area {Area(rc)}");
 

                Square sq = new Square();
                sq.Width = 4;
                WriteLine($"{sq} has area {Area(sq)}");

                Rectangle sq2 = new Square();
                sq2.Width = 4;
                WriteLine($"{sq2} has area {Area(sq2)}");

// Violacao do principio de substituicao de Liskov:
//
// A linha abaixo cria um objeto Square, mas a variavel e do tipo Rectangle:
// Rectangle sq2 = new Square();
//
// Quando fazemos sq2.Width = 4, como Width foi ocultado com "new",
// o compilador usa Rectangle.Width, porque ele olha para o tipo da variavel.
// Ele NAO usa Square.Width.
//
// Esse e o efeito da ocultacao com "new":
// o comportamento escolhido depende de como o objeto esta sendo enxergado.
//
// Por que errado.cs e errado?
// Porque Square nao se comporta como um Rectangle comum.
// Quem usa Rectangle espera conseguir trabalhar com largura e altura separadamente.
// Mas Square muda esse combinado e passa a forcar os dois lados a terem o mesmo valor.
// Entao a classe filha altera o contrato esperado da classe pai.
// Quando isso acontece, a substituicao deixa de ser segura.
// Assim, o principio de Liskov e quebrado.





            }
        }





}
