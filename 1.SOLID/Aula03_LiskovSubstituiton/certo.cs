using System;
using System.Collections.Generic;
using static System.Console;

namespace AulasSOLIDpatterns.Aula03_LiskovSubstituiton
{

    // Neste arquivo, a intencao e explicar melhor a diferenca tecnica
    // entre "ocultar" um membro herdado e "sobrescrever" esse membro.
    //
    // A diferenca central para o errado.cs e esta:
    // - la, Square escondia Width e Height com "new"
    // - aqui, Rectangle2 declara Width e Height como "virtual"
    // - e Square2 participa do mesmo contrato usando "override"
    //
    // Em resumo:
    // "new" cria uma segunda versao do membro na classe filha.
    // "virtual" + "override" mantem o mesmo membro, mas com uma implementacao sobrescrita.

    // ====================================
    //   Classe de Retangulo
    // ====================================

    // ===== Classe =====
    public class Rectangle2
    {
        // "virtual" diz:
        // "esta propriedade faz parte de um contrato que pode ser redefinido pela classe filha".
        //
        // Com isso, se uma variavel do tipo Rectangle2 apontar para um objeto filho,
        // o runtime podera usar a implementacao da classe filha.
        //
        // Diferenca para o arquivo errado:
        // la a decisao dependia do tipo da variavel.
        // aqui a decisao pode considerar o tipo real do objeto.

        // ===== Propriedades =====
        public virtual int Width {get; set;}  // VIRTUAL habilita polimorfismo real para essa propriedade
        public virtual int Height {get; set;}  // VIRTUAL habilita polimorfismo real para essa propriedade

        // ===== Construtores =====
        public Rectangle2()
        {
             
        }

        public Rectangle2(int width, int height)
        {
            Width = width;
            Height = height;
        }

        // ===== Metodos =====
        public override string ToString()
        {
            return $"{nameof(Width)}: {Width}, {nameof(Height)}: {Height}";
        }
    }



    // ==============================================
    //   Classe de Quadrado (herda de Retangulo)
    // ==============================================

    // ===== Classe =====
    public class Square2 : Rectangle2
    {
        // "override" significa:
        // a classe filha NAO esta criando uma segunda Width escondida.
        // Ela esta pegando a mesma propriedade virtual da classe pai
        // e trocando a implementacao dela.
        //
        // Isso quer dizer que, se o objeto real for Square2,
        // mesmo uma variavel do tipo Rectangle2 podera executar este codigo.

        // ===== Campos =====
        public override int Width // OVERRIDE e a palavra chave para substituir a implementacao do membro virtual
        {
            set { base.Width = base.Height = value;}
        }

        public override int Height // OVERRIDE e a palavra chave para substituir a implementacao do membro virtual
        {
            set { base.Width = base.Height = value;}
        }

    }

  




    // ===== Classe =====
    public class Demoliskov
        {
            // ===== Metodos =====
            static public int Area(Rectangle2 r)=> r.Width * r.Height;

            public static void Main(string[] args)
            {
                Rectangle2 rc = new Rectangle2(2,3);
                WriteLine($"{rc} has area {Area(rc)}");
 

                Square2 sq = new Square2();
                sq.Width = 4;
                WriteLine($"{sq} has area {Area(sq)}");

                Rectangle2 sq2 = new Square2();
                sq2.Width = 4;
                WriteLine($"{sq2} has area {Area(sq2)}");

                // Como ler a diferenca na pratica:
                //
                // No arquivo errado:
                // Rectangle sq2 = new Square();
                // sq2.Width = 4;
                // Aqui o compilador usa Rectangle.Width, porque "new" apenas oculta o membro.
                //
                // Neste arquivo:
                // Rectangle2 sq2 = new Square2();
                // sq2.Width = 4;
                // Aqui a chamada pode cair em Square2.Width, porque existe
                // um membro virtual sobrescrito com override.
                //
                // Entao a diferenca tecnica e:
                // - com "new", o membro usado depende do tipo da variavel
                // - com "virtual/override", o membro usado pode depender do tipo real do objeto
                //
                // Essa e a grande mudanca de mecanismo entre um arquivo e outro.
            }
        }





}
