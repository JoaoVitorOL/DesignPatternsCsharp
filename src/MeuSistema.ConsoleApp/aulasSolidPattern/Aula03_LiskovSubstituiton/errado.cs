using System;
using System.Collections.Generic;
using static System.Console;

namespace AulasSOLIDpatterns.Aula03_LiskovSubstituiton
{
    // O principio da Substituicao de Liskov diz que uma classe filha
    // deve poder substituir a classe pai sem mudar o comportamento esperado.
    // Em outras palavras: se o codigo funciona com Rectangle,
    // ele deveria continuar funcionando corretamente com Square.

    // ====================================
    //   Classe de Retangulo
    // ====================================
    public class Rectangle
    {
        // O contrato implicito desta classe e:
        // largura e altura podem ser tratadas como valores independentes.
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
        // Aqui esta o problema:
        // Square esconde Width e Height com "new" e muda a regra original.
        // Ao definir Width, ele tambem muda Height.
        // Ao definir Height, ele tambem muda Width.
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
// Por que errado.cs e errado?
// Porque Square nao se comporta como um Rectangle comum.
// Quem usa Rectangle espera conseguir trabalhar com largura e altura separadamente.
// Mas Square muda esse combinado e passa a forcar os dois lados a terem o mesmo valor.
// Entao a classe filha altera o contrato esperado da classe pai.
// Quando isso acontece, a substituicao deixa de ser segura.
// Assim, quebrando totalmente o princípio de Liskov.





            }
        }





}

