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
    // Peca simples do produto final: um campo da classe gerada.
    public class CodeField
    {
        public string Type, Name;

        public override string ToString()
        {
            return $"public {Type} {Name}";
        }
    }

    // Produto em construcao: a "classe C#" que queremos montar.
    public class CodeClass
    {
        public string Name;
        public List<CodeField> Fields = new List<CodeField>();

        public override string ToString()
        {
            // O produto final sabe se renderizar como texto.
            var sb = new StringBuilder();
            sb.AppendLine($"public class {Name}").AppendLine("{");
            foreach (var f in Fields)
                sb.AppendLine($"  {f};");
            sb.AppendLine("}");
            return sb.ToString();
        }
    }

    // Builder: recebe pedidos do cliente e vai preenchendo o produto.
    public class CodeBuilder
    {
        // Todo builder precisa guardar o objeto que esta sendo montado.
        private CodeClass codeClass = new CodeClass();

        public CodeBuilder(string rootName)
        {
            // Primeiro passo da construcao: definir a "raiz" do produto.
            codeClass.Name = rootName;
        }

        public CodeBuilder AddField(string name, string type)
        {
            // Cada metodo de configuracao altera o produto em construcao.
            codeClass.Fields.Add(new CodeField { Name = name, Type = type });

            // `return this` transforma o builder em fluent builder.
            return this;
        }

        public override string ToString()
        {
            // No fim, o builder entrega a representacao final do produto.
            return codeClass.ToString();
        }
    }


    public class Program{
        static void Main(string[] args)
        {
            var cb = new CodeBuilder("Person")
            .AddField("Name", "string")
            .AddField("Age", "int");

        Console.WriteLine(cb);
        }
    }
}
