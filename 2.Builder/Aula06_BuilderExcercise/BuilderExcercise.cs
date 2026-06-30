//  Exercicio:
//  implemente o padrao Builder para montar o TEXTO de uma classe C# simples.
//
//  Importante:
//  - o objetivo nao e renderizar "qualquer trecho de codigo"
//  - o objetivo e montar a representacao textual de UMA classe
//  - essa classe tera um nome e uma lista de campos publicos
//
//  Em outras palavras, o builder deve:
//  1. receber o nome da classe
//  2. permitir adicionar campos, um por vez
//  3. no final, gerar uma string com o codigo dessa classe formatado corretamente
//
//  A API esperada para o uso do builder e esta:
//
//  var cb = new CodeBuilder("Person")
//    .AddField("Name", "string")
//    .AddField("Age", "int");
//
//  Console.WriteLine(cb);
//
//  A saida esperada e exatamente esta:
//
//  public class Person
//  {
//    public string Name;
//    public int Age;
//  }
//
//  Observe os detalhes pedidos no enunciado:
//  - as chaves devem ficar exatamente nessa posicao
//  - a indentacao interna deve usar 2 espacos
//  - cada campo deve virar uma linha no formato:
//      public <tipo> <nome>;
//


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static System.Console;

namespace Aula06_BuilderExcercise
{
    // 1º Defina as pecas simples que o produto final vai conter.
    public class CodeField
    {
        public string Type, Name;

        public override string ToString()
        {
            return $"public {Type} {Name}";
        }
    }

    // 2º Defina o produto completo que sera montado pelo builder.
    public class CodeClass
    {
        public string Name;
        public List<CodeField> Fields = new List<CodeField>();

        public override string ToString()
        {
            // 8º Internamente, alguem precisa saber renderizar o resultado final.
            var sb = new StringBuilder();//classe utilitária do .NET.Ele só ajuda a montar texto de forma eficiente.
            sb.AppendLine($"public class {Name}").AppendLine("{");
            foreach (var f in Fields)
                sb.AppendLine($"  {f};");
            sb.AppendLine("}");
            return sb.ToString();
        }
    }

    // 3º Crie o builder que vai guardar e preencher esse produto.
    public class CodeBuilder
    {
        // 4º Todo builder precisa manter internamente o objeto em construcao.
        private CodeClass codeClass = new CodeClass(); //Instância vazia do objeto que será construído

        public CodeBuilder(string rootName)
        {
            // 5º No construtor, receba os dados iniciais obrigatorios.
            codeClass.Name = rootName;
        }

        public CodeBuilder AddField(string name, string type)
        {
            // 6º Cada metodo de configuracao altera o produto em construcao.
            codeClass.Fields.Add(new CodeField { Name = name, Type = type });

            // 7º `return this` permite continuar a montagem na mesma linha. ( É o que o torna um fluent builder)
            return this;
        }

        public override string ToString()
        {
            // 9º No fim, o builder expoe essa representacao final ao cliente.
            return codeClass.ToString();
        }
    }


    public class Program{
        static void Main(string[] args)
        {
            // 10º O cliente usa o builder descrevendo o que quer montar.
            var cb = new CodeBuilder("Person")
            .AddField("Name", "string")
            .AddField("Age", "int")
            .AddField("Job", "string");

         Console.WriteLine(cb);
        }
    }
}
