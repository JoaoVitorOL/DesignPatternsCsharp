using System;
using System.Runtime.Serialization;
using System.Text;
using static System.Console;

namespace DotNetDesignPatternDemos.Structural.Decorator.CodeBuilder
{
  // ============================================================================
  // Aula01 - Custom String Builder
  // ----------------------------------------------------------------------------
  // Ideia da aula:
  // criar um wrapper em volta de System.Text.StringBuilder para manter quase toda
  // a API original, mas com uma diferenca importante nos metodos que alteram o
  // conteudo: eles retornam CodeBuilder em vez de StringBuilder.
  //
  // Isso permite escrever uma cadeia fluent como:
  //
  //   cb.AppendLine("class Foo")
  //     .AppendLine("{")
  //     .AppendLine("}");
  //
  // Relacao com Decorator:
  // o objeto externo envolve outro objeto e delega trabalho para ele. Ainda assim,
  // este exemplo e mais um wrapper didatico do que um Decorator GoF perfeito,
  // porque StringBuilder nao aparece aqui atras de uma interface propria comum
  // implementada tanto pelo componente real quanto pelo wrapper.
  //
  // As linhas originais abaixo eram lembretes para gerar wrappers mecanicamente:
  // - procurar chamadas no formato: return builder.(.+)$
  // - trocar por: builder.$1; return this;
  // - depois formatar o documento

  // ===== Classe Wrapper =====
  public class CodeBuilder
  {
    // ===== Campos =====
    // Composicao: CodeBuilder NAO reimplementa a montagem de texto.
    // Ele guarda um StringBuilder real e repassa quase todas as operacoes para ele.
    private StringBuilder builder = new StringBuilder();

    // ===== Metodos de leitura / conversao =====
    // Exibe o texto acumulado pelo StringBuilder interno.
    // Para quem usa CodeBuilder, o resultado final ainda e apenas uma string.
    public override string ToString()
    {
      return builder.ToString();
    }

    // Repassa a serializacao para o objeto interno.
    // Este metodo nao altera o conteudo; por isso nao participa da cadeia fluent.
    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      ((ISerializable)builder).GetObjectData(info, context);
    }

    // Mantem o retorno original do StringBuilder.
    // Aqui o valor interessante e a capacidade garantida, nao o proprio wrapper.
    public int EnsureCapacity(int capacity)
    {
      return builder.EnsureCapacity(capacity);
    }

    // Versao de ToString que extrai apenas um trecho do texto acumulado.
    public string ToString(int startIndex, int length)
    {
      return builder.ToString(startIndex, length);
    }

    // ===== Metodos mutaveis com retorno fluent =====
    // A partir daqui, o padrao se repete:
    // 1. chama o mesmo metodo no StringBuilder interno;
    // 2. devolve `this` para permitir a proxima chamada encadeada.
    public CodeBuilder Clear()
    {
      builder.Clear();
      return this;
    }

    // ===== Append =====
    // Os overloads de Append preservam a conveniencia do StringBuilder original,
    // mas trocam o tipo de retorno para CodeBuilder.
    public CodeBuilder Append(char value, int repeatCount)
    {
      builder.Append(value, repeatCount);
      return this;
    }

    public CodeBuilder Append(char[] value, int startIndex, int charCount)
    {
      builder.Append(value, startIndex, charCount);
      return this;
    }

    public CodeBuilder Append(string value)
    {
      builder.Append(value);
      return this;
    }

    public CodeBuilder Append(string value, int startIndex, int count)
    {
      builder.Append(value, startIndex, count);
      return this;
    }

    public CodeBuilder AppendLine()
    {
      builder.AppendLine();
      return this;
    }

    public CodeBuilder AppendLine(string value)
    {
      builder.AppendLine(value);
      return this;
    }

    // CopyTo apenas copia caracteres para um array externo.
    // Como nao muda o builder nem precisa continuar cadeia, permanece void.
    public void CopyTo(int sourceIndex, char[] destination, int destinationIndex, int count)
    {
      builder.CopyTo(sourceIndex, destination, destinationIndex, count);
    }

    // ===== Insert / Remove =====
    // Insert e Remove tambem modificam o conteudo interno, entao seguem o mesmo
    // contrato fluent: executam a operacao real e retornam o proprio wrapper.
    public CodeBuilder Insert(int index, string value, int count)
    {
      builder.Insert(index, value, count);
      return this;
    }

    public CodeBuilder Remove(int startIndex, int length)
    {
      builder.Remove(startIndex, length);
      return this;
    }

    public CodeBuilder Append(bool value)
    {
      builder.Append(value);
      return this;
    }

    public CodeBuilder Append(sbyte value)
    {
      builder.Append(value);
      return this;
    }

    public CodeBuilder Append(byte value)
    {
      builder.Append(value);
      return this;
    }

    public CodeBuilder Append(char value)
    {
      builder.Append(value);
      return this;
    }

    public CodeBuilder Append(short value)
    {
      builder.Append(value);
      return this;
    }

    public CodeBuilder Append(int value)
    {
      builder.Append(value);
      return this;
    }

    public CodeBuilder Append(long value)
    {
      builder.Append(value);
      return this;
    }

    public CodeBuilder Append(float value)
    {
      builder.Append(value);
      return this;
    }

    public CodeBuilder Append(double value)
    {
      builder.Append(value);
      return this;
    }

    public CodeBuilder Append(decimal value)
    {
      builder.Append(value);
      return this;
    }

    public CodeBuilder Append(ushort value)
    {
      builder.Append(value);
      return this;
    }

    public CodeBuilder Append(uint value)
    {
      builder.Append(value);
      return this;
    }

    public CodeBuilder Append(ulong value)
    {
      builder.Append(value);
      return this;
    }

    public CodeBuilder Append(object value)
    {
      builder.Append(value);
      return this;
    }

    public CodeBuilder Append(char[] value)
    {
      builder.Append(value);
      return this;
    }

    // Os varios overloads abaixo existem para o wrapper continuar parecido com
    // a API familiar de StringBuilder. O custo desse estilo e a repeticao:
    // quando o tipo original nao tem uma interface pequena, o wrapper precisa
    // expor manualmente os membros que deseja suportar.
    public CodeBuilder Insert(int index, string value)
    {
      builder.Insert(index, value);
      return this;
    }

    public CodeBuilder Insert(int index, bool value)
    {
      builder.Insert(index, value);
      return this;
    }

    public CodeBuilder Insert(int index, sbyte value)
    {
      builder.Insert(index, value);
      return this;
    }

    public CodeBuilder Insert(int index, byte value)
    {
      builder.Insert(index, value);
      return this;
    }

    public CodeBuilder Insert(int index, short value)
    {
      builder.Insert(index, value);
      return this;
    }

    public CodeBuilder Insert(int index, char value)
    {
      builder.Insert(index, value);
      return this;
    }

    public CodeBuilder Insert(int index, char[] value)
    {
      builder.Insert(index, value);
      return this;
    }

    public CodeBuilder Insert(int index, char[] value, int startIndex, int charCount)
    {
      builder.Insert(index, value, startIndex, charCount);
      return this;
    }

    public CodeBuilder Insert(int index, int value)
    {
      builder.Insert(index, value);
      return this;
    }

    public CodeBuilder Insert(int index, long value)
    {
      builder.Insert(index, value);
      return this;
    }

    public CodeBuilder Insert(int index, float value)
    {
      builder.Insert(index, value);
      return this;
    }

    public CodeBuilder Insert(int index, double value)
    {
      builder.Insert(index, value);
      return this;
    }

    public CodeBuilder Insert(int index, decimal value)
    {
      builder.Insert(index, value);
      return this;
    }

    public CodeBuilder Insert(int index, ushort value)
    {
      builder.Insert(index, value);
      return this;
    }

    public CodeBuilder Insert(int index, uint value)
    {
      builder.Insert(index, value);
      return this;
    }

    public CodeBuilder Insert(int index, ulong value)
    {
      builder.Insert(index, value);
      return this;
    }

    public CodeBuilder Insert(int index, object value)
    {
      builder.Insert(index, value);
      return this;
    }

    // ===== AppendFormat =====
    // AppendFormat mistura formatacao de string com acumulacao de texto.
    // O comportamento continua sendo do StringBuilder interno; CodeBuilder so
    // adapta o retorno para manter o encadeamento.
    public CodeBuilder AppendFormat(string format, object arg0)
    {
      builder.AppendFormat(format, arg0);
      return this;
    }

    public CodeBuilder AppendFormat(string format, object arg0, object arg1)
    {
      builder.AppendFormat(format, arg0, arg1);
      return this;
    }

    public CodeBuilder AppendFormat(string format, object arg0, object arg1, object arg2)
    {
      builder.AppendFormat(format, arg0, arg1, arg2);
      return this;
    }

    public CodeBuilder AppendFormat(string format, params object[] args)
    {
      builder.AppendFormat(format, args);
      return this;
    }

    public CodeBuilder AppendFormat(IFormatProvider provider, string format, params object[] args)
    {
      builder.AppendFormat(provider, format, args);
      return this;
    }

    // ===== Replace / Equals =====
    // Replace altera o texto e por isso retorna CodeBuilder.
    // Equals apenas responde uma pergunta booleana e nao deve entrar na cadeia fluent.
    public CodeBuilder Replace(string oldValue, string newValue)
    {
      builder.Replace(oldValue, newValue);
      return this;
    }

    public bool Equals(CodeBuilder sb)
    {
      return builder.Equals(sb);
    }

    public CodeBuilder Replace(string oldValue, string newValue, int startIndex, int count)
    {
      builder.Replace(oldValue, newValue, startIndex, count);
      return this;
    }

    public CodeBuilder Replace(char oldChar, char newChar)
    {
      builder.Replace(oldChar, newChar);
      return this;
    }

    public CodeBuilder Replace(char oldChar, char newChar, int startIndex, int count)
    {
      builder.Replace(oldChar, newChar, startIndex, count);
      return this;
    }

    // ===== Propriedades delegadas =====
    // Estas propriedades deixam o cliente controlar o mesmo estado operacional
    // que controlaria em um StringBuilder comum.
    public int Capacity
    {
      get => builder.Capacity;
      set => builder.Capacity = value;
    }

    public int MaxCapacity => builder.MaxCapacity;

    public int Length
    {
      get => builder.Length;
      set => builder.Length = value;
    }

    public char this[int index]
    {
      get => builder[index];
      set => builder[index] = value;
    }
  }

  // ===== Client / Demo =====
  // O cliente usa CodeBuilder como um StringBuilder mais conveniente para cadeia.
  // No fim, WriteLine chama ToString() implicitamente e imprime o texto acumulado.
  public class Demo
  {
    static void Main(string[] args)
    {
      var cb = new CodeBuilder();
      cb.AppendLine("class Foo")
        .AppendLine("{")
        .AppendLine("}");
      WriteLine(cb);
    }
  }
}
