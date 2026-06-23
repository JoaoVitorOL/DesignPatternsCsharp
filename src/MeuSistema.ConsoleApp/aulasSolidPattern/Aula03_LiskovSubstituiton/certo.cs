using System;
using System.Collections.Generic;
using static System.Console;

namespace AulasSOLIDpatterns.Aula03_LiskovSubstituiton
{

    // Neste arquivo, a intencao do exemplo "certo" e mostrar uma tentativa
    // de tornar a substituicao mais explicita usando polimorfismo real.
    // A diferenca central para o errado.cs e que aqui a classe base expõe
    // membros virtuais e a classe filha participa desse contrato com override,
    // em vez de esconder membros com "new".

    // ====================================
    //   Classe de Retangulo
    // ====================================
    public class Rectangle2
    {
        // "virtual" permite que a classe filha sobrescreva esse comportamento
        // de forma explicita dentro do mesmo contrato da classe base.
        public virtual int Width {get; set;}  // VIRTUAL e a palavra chave para permitir a substituicao de um metodo
        public virtual int Height {get; set;}  // VIRTUAL e a palavra chave para permitir a substituicao de um metodo

        public Rectangle2()
        {
             
        }

        public Rectangle2(int width, int height)
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
    public class Square2 : Rectangle2
    {
        // "override" significa:
        // a classe filha nao esta escondendo membros da classe pai,
        // ela esta sobrescrevendo o comportamento previsto no contrato virtual.
        public override int Width // OVERRIDE e a palavra chave para realizar a substituicao de um metodo virtual
        {
            set { base.Width = base.Height = value;}
        }

        public override int Height // OVERRIDE e a palavra chave para realizar a substituicao de um metodo virtual
        {
            set { base.Width = base.Height = value;}
        }

    }

  




    public class Demoliskov
        {
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

                // Por que este arquivo foi pensado como o "certo"?
                // Porque a mudanca proposta foi sair de uma ocultacao com "new"
                // para uma sobrescrita com "virtual" e "override".
                // A ideia didatica aqui e mostrar que a classe filha deve participar
                // do mesmo contrato da classe pai, e nao criar um contrato paralelo.

                // Qual efeito essa mudanca teve no codigo?
                // 1. Rectangle2 passou a declarar Width e Height como virtuais.
                // 2. Square2 passou a sobrescrever essas propriedades com override.
                // 3. Com isso, a substituicao deixa de ser uma simples ocultacao de membros
                //    e passa a acontecer pelo mecanismo de polimorfismo da linguagem.
            }
        }





}
