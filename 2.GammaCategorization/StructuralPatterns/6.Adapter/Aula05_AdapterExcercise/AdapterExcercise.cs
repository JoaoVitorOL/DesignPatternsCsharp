
//  Exercicio:
//  implemente o padrao Adapter para adaptar um Square a interface IRectangle.
//
//  Importante:
//  - o objetivo nao e apenas calcular a area de forma isolada
//  - o objetivo e criar um adaptador estrutural que faca o Square se comportar como um IRectangle
//  - a interface IRectangle exige duas propriedades de leitura: Width e Height
//  - como um quadrado possui todos os lados iguais, ambas as propriedades devem refletir o tamanho de seu Side
//
//  Em outras palavras, o Adapter deve:
//  1. receber um objeto do tipo Square no construtor
//  2. expor as propriedades Width e Height exigidas pela interface IRectangle
//  3. mapear internamente as propriedades Width e Height para ler o valor de Side do Square original
//
//  Observe os detalhes pedidos no enunciado:
//  - o adaptador deve implementar a interface IRectangle
//  - o adaptador deve receber o Square por injecao no construtor
//  - as mudancas no Side do Square original devem ser refletidas dinamicamente pelo adaptador


using System;

namespace Coding.Exercise
{
    // 1º Identifique o Adaptee (a classe existente que possui a interface incompativel).

    // ===== Classe do Adaptee =====
    public class Square
    {
        // ===== Campos =====
        public int Side;
    }

    // 2º Identifique o Target (a interface que o seu cliente espera e sabe consumir).

    // ===== Interface Target =====
    public interface IRectangle
    {
        int Width { get; }
        int Height { get; }
    }
    
    // ===== Classe Auxiliar =====
    public static class ExtensionMethods
    {
        // ===== Metodos =====
        public static int Area(this IRectangle rc)
        {
            return rc.Width * rc.Height;
        }
    }

    // 3º Crie a classe do Adapter que implementa a interface Target esperada.

    // ===== Classe do Adapter =====
    public class SquareToRectangleAdapter : IRectangle
    {
        // 4º O adaptador precisa manter internamente uma referencia ao objeto adaptado (composicao).

        // ===== Campos =====
        private readonly Square _square;

        // ===== Construtores =====
        public SquareToRectangleAdapter(Square square)
        {
            // 5º No construtor, faca a validacao de guarda e guarde a referencia do Adaptee.
            if (square == null)
            {
                throw new ArgumentNullException(nameof(square));
            }
            _square = square;
        }

        // 6º Implemente os membros da interface Target adaptando os acessos para o Adaptee original.

        // ===== Propriedades =====
        
        // Um quadrado tem lados iguais; portanto, a largura mapeia diretamente para o Side do quadrado.
        public int Width => _square.Side;

        // A altura tambem mapeia dinamicamente para o mesmo Side do quadrado.
        public int Height => _square.Side;
    }

    // ===== Classe =====
    public static class Program
    {
        // ===== Metodos =====
        public static void Main()
        {
            // 7º O cliente cria o objeto com a assinatura incompativel (Square).
            var square = new Square { Side = 11 };

            // 8º O cliente envolve o objeto incompativel dentro do adaptador correspondente.
            var adapter = new SquareToRectangleAdapter(square);

            // 9º O adaptador agora permite usar as operacoes da interface esperada sem alterar o Square original.
            Console.WriteLine($"Square Side: {square.Side}");
            Console.WriteLine($"Adapted Width: {adapter.Width}, Height: {adapter.Height}");
            Console.WriteLine($"Calculated Area via Extension Method: {adapter.Area()}");
        }
    }
}

