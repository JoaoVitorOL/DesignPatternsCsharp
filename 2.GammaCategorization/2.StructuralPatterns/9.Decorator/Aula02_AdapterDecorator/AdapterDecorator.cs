using System;
using System.Runtime.Serialization;
using System.Text;
using static System.Console;

namespace DotNetDesignPatternDemos.Structural.AdapterDecorator
{
  // ============================================================================
  // Aula02 - Adapter Decorator
  // ----------------------------------------------------------------------------
  // Ideia da aula:
  // criar um wrapper em volta de System.Text.StringBuilder, mas agora com um
  // tempero extra de "adapter": permitir que uma string comum seja convertida
  // automaticamente para MyStringBuilder.
  //
  // Relacao com Decorator:
  // - MyStringBuilder envolve um StringBuilder real por composicao.
  // - A maior parte dos metodos apenas delega para o objeto interno.
  //
  // Relacao com Adapter:
  // - uma string pura, como "hello ", passa a poder ser usada onde o codigo
  //   espera um MyStringBuilder, por causa do operador de conversao implicita.
  //
  // Em resumo:
  // StringBuilder continua sendo o motor real de montagem de texto.
  // MyStringBuilder e a casca externa que adapta a entrada e expoe uma API parecida.

  // ===== Classe Wrapper / Adapter =====
  public class MyStringBuilder
  {
    // ===== Campos =====
    // Composicao: o wrapper nao armazena texto sozinho.
    // Ele guarda um StringBuilder interno e repassa as operacoes para ele.
    StringBuilder sb = new StringBuilder();

    //=============================================

    // ===== Conversao implicita =====
    // Este operador permite escrever:
    //
    //   MyStringBuilder s = "hello ";
    //
    // Sem ele, uma string nao poderia ser atribuida diretamente a MyStringBuilder.
    // Aqui esta a parte "Adapter" da aula: adaptamos uma string simples para o
    // tipo esperado pelo cliente.
    public static implicit operator MyStringBuilder(string s)
    {
      // Cria um wrapper novo...
      var msb = new MyStringBuilder();

      // ...e coloca a string recebida dentro do StringBuilder interno.
      msb.sb.Append(s);
      return msb;
    }

    // ===== Sobrecarga do operador + =====
    // Permite escrever:
    //
    //   s = s + "world";
    //
    // ou, de forma abreviada:
    //
    //   s += "world";
    //
    // Diferenca importante:
    // este operador altera o mesmo MyStringBuilder recebido e devolve esse
    // proprio objeto. Ou seja, ele funciona como um Append com sintaxe de soma.
    public static MyStringBuilder operator +(MyStringBuilder msb, string s)
    {
      msb.Append(s);
      return msb;
    }

    // ===== Conversao para texto final =====
    // Quando o objeto precisa virar texto, a resposta vem do StringBuilder interno.
    // Isso tambem e usado por Console.WriteLine e por concatenacao de string.
    public override string ToString()
    {
      return sb.ToString();
    }

    //=============================================

    // ===== Metodos delegados =====
    // A partir daqui, a classe tenta parecer bastante com StringBuilder.
    // O padrao dos metodos e simples:
    //
    //   receber a chamada em MyStringBuilder
    //   repassar para sb
    //   devolver o resultado original de StringBuilder
    //
    // Repare na diferenca para a aula anterior:
    // em CodeBuilder, os metodos mutaveis retornavam CodeBuilder para manter a
    // cadeia no wrapper. Aqui eles retornam StringBuilder, entao uma cadeia como
    // s.Append("a").Append("b") continua, mas depois da primeira chamada ela ja
    // esta conversando diretamente com StringBuilder.

    // Repassa a serializacao para o objeto interno.
    // Este metodo existe porque StringBuilder implementa ISerializable.
    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      ((ISerializable) sb).GetObjectData(info, context);
    }

    // Garante capacidade minima no buffer interno e devolve a capacidade efetiva.
    public int EnsureCapacity(int capacity)
    {
      return sb.EnsureCapacity(capacity);
    }

    // Extrai uma parte do texto acumulado, delegando para StringBuilder.
    public string ToString(int startIndex, int length)
    {
      return sb.ToString(startIndex, length);
    }

    // Limpa o conteudo do StringBuilder interno.
    // Como o retorno e StringBuilder, o cliente recebe o objeto interno,
    // nao o MyStringBuilder.
    public StringBuilder Clear()
    {
      return sb.Clear();
    }

    // ===== Append =====
    // Os overloads abaixo existem porque StringBuilder aceita muitos tipos
    // diferentes. Com composicao, esses metodos nao aparecem automaticamente
    // em MyStringBuilder; cada assinatura precisa ser exposta manualmente.
    public StringBuilder Append(char value, int repeatCount)
    {
      return sb.Append(value, repeatCount);
    }

    public StringBuilder Append(char[] value, int startIndex, int charCount)
    {
      return sb.Append(value, startIndex, charCount);
    }

    public StringBuilder Append(string value)
    {
      return sb.Append(value);
    }

    public StringBuilder Append(string value, int startIndex, int count)
    {
      return sb.Append(value, startIndex, count);
    }

    public StringBuilder AppendLine()
    {
      return sb.AppendLine();
    }

    public StringBuilder AppendLine(string value)
    {
      return sb.AppendLine(value);
    }

    public void CopyTo(int sourceIndex, char[] destination, int destinationIndex, int count)
    {
      sb.CopyTo(sourceIndex, destination, destinationIndex, count);
    }

    // ===== Insert / Remove =====
    // Mesmo principio dos Append: MyStringBuilder apenas repassa a chamada.
    // O comportamento real continua sendo do StringBuilder interno.
    public StringBuilder Insert(int index, string value, int count)
    {
      return sb.Insert(index, value, count);
    }

    public StringBuilder Remove(int startIndex, int length)
    {
      return sb.Remove(startIndex, length);
    }

    public StringBuilder Append(bool value)
    {
      return sb.Append(value);
    }

    public StringBuilder Append(sbyte value)
    {
      return sb.Append(value);
    }

    public StringBuilder Append(byte value)
    {
      return sb.Append(value);
    }

    public StringBuilder Append(char value)
    {
      return sb.Append(value);
    }

    public StringBuilder Append(short value)
    {
      return sb.Append(value);
    }

    public StringBuilder Append(int value)
    {
      return sb.Append(value);
    }

    public StringBuilder Append(long value)
    {
      return sb.Append(value);
    }

    public StringBuilder Append(float value)
    {
      return sb.Append(value);
    }

    public StringBuilder Append(double value)
    {
      return sb.Append(value);
    }

    public StringBuilder Append(decimal value)
    {
      return sb.Append(value);
    }

    public StringBuilder Append(ushort value)
    {
      return sb.Append(value);
    }

    public StringBuilder Append(uint value)
    {
      return sb.Append(value);
    }

    public StringBuilder Append(ulong value)
    {
      return sb.Append(value);
    }

    public StringBuilder Append(object value)
    {
      return sb.Append(value);
    }

    public StringBuilder Append(char[] value)
    {
      return sb.Append(value);
    }

    // Esta repeticao grande e o custo de criar um wrapper com superficie
    // parecida com a classe original. StringBuilder possui muitos overloads;
    // se o wrapper quer oferece-los, precisa declarar um por um.
    public StringBuilder Insert(int index, string value)
    {
      return sb.Insert(index, value);
    }

    public StringBuilder Insert(int index, bool value)
    {
      return sb.Insert(index, value);
    }

    public StringBuilder Insert(int index, sbyte value)
    {
      return sb.Insert(index, value);
    }

    public StringBuilder Insert(int index, byte value)
    {
      return sb.Insert(index, value);
    }

    public StringBuilder Insert(int index, short value)
    {
      return sb.Insert(index, value);
    }

    public StringBuilder Insert(int index, char value)
    {
      return sb.Insert(index, value);
    }

    public StringBuilder Insert(int index, char[] value)
    {
      return sb.Insert(index, value);
    }

    public StringBuilder Insert(int index, char[] value, int startIndex, int charCount)
    {
      return sb.Insert(index, value, startIndex, charCount);
    }

    public StringBuilder Insert(int index, int value)
    {
      return sb.Insert(index, value);
    }

    public StringBuilder Insert(int index, long value)
    {
      return sb.Insert(index, value);
    }

    public StringBuilder Insert(int index, float value)
    {
      return sb.Insert(index, value);
    }

    public StringBuilder Insert(int index, double value)
    {
      return sb.Insert(index, value);
    }

    public StringBuilder Insert(int index, decimal value)
    {
      return sb.Insert(index, value);
    }

    public StringBuilder Insert(int index, ushort value)
    {
      return sb.Insert(index, value);
    }

    public StringBuilder Insert(int index, uint value)
    {
      return sb.Insert(index, value);
    }

    public StringBuilder Insert(int index, ulong value)
    {
      return sb.Insert(index, value);
    }

    public StringBuilder Insert(int index, object value)
    {
      return sb.Insert(index, value);
    }

    // ===== AppendFormat =====
    // Repassa formatacao composta para StringBuilder.
    // Ha varias assinaturas para cobrir formatos com diferentes quantidades
    // de argumentos e tambem formatos sensiveis a cultura via IFormatProvider.
    public StringBuilder AppendFormat(string format, object arg0)
    {
      return sb.AppendFormat(format, arg0);
    }

    public StringBuilder AppendFormat(string format, object arg0, object arg1)
    {
      return sb.AppendFormat(format, arg0, arg1);
    }

    public StringBuilder AppendFormat(string format, object arg0, object arg1, object arg2)
    {
      return sb.AppendFormat(format, arg0, arg1, arg2);
    }

    public StringBuilder AppendFormat(string format, params object[] args)
    {
      return sb.AppendFormat(format, args);
    }

    public StringBuilder AppendFormat(IFormatProvider provider, string format, object arg0)
    {
      return sb.AppendFormat(provider, format, arg0);
    }

    public StringBuilder AppendFormat(IFormatProvider provider, string format, object arg0,
      object arg1)
    {
      return sb.AppendFormat(provider, format, arg0, arg1);
    }

    public StringBuilder AppendFormat(IFormatProvider provider, string format, object arg0,
      object arg1, object arg2)
    {
      return sb.AppendFormat(provider, format, arg0, arg1, arg2);
    }

    public StringBuilder AppendFormat(IFormatProvider provider, string format, params object[] args)
    {
      return sb.AppendFormat(provider, format, args);
    }

    // ===== Replace / Equals =====
    // Replace modifica o texto interno e devolve o StringBuilder real.
    public StringBuilder Replace(string oldValue, string newValue)
    {
      return sb.Replace(oldValue, newValue);
    }

    // Compara o StringBuilder interno deste objeto com outro StringBuilder.
    // Diferente da aula anterior, o parametro aqui ja e StringBuilder,
    // entao a comparacao esta alinhada com o objeto realmente armazenado.
    public bool Equals(StringBuilder sb)
    {
      return this.sb.Equals(sb);
    }

    public StringBuilder Replace(string oldValue, string newValue, int startIndex, int count)
    {
      return sb.Replace(oldValue, newValue, startIndex, count);
    }

    public StringBuilder Replace(char oldChar, char newChar)
    {
      return sb.Replace(oldChar, newChar);
    }

    public StringBuilder Replace(char oldChar, char newChar, int startIndex, int count)
    {
      return sb.Replace(oldChar, newChar, startIndex, count);
    }

    // ===== Propriedades delegadas =====
    // Capacity, MaxCapacity, Length e o indexador sao portas diretas para o
    // estado operacional do StringBuilder interno.
    public int Capacity
    {
      get => sb.Capacity;
      set => sb.Capacity = value;
    }

    public int MaxCapacity => sb.MaxCapacity;

    public int Length
    {
      get => sb.Length;
      set => sb.Length = value;
    }

    public char this[int index]
    {
      get => sb[index];
      set => sb[index] = value;
    }
  }

  // ===== Client / Demo =====
  // Demonstra a parte Adapter:
  // uma string literal e aceita diretamente como MyStringBuilder.
  public class Demo
  {
    static void Main(string[] args)
    {
      // O operador implicito converte "hello " em um MyStringBuilder novo.
      MyStringBuilder s = "hello ";

      // Com o operator + definido acima, esta linha chama:
      //
      //   MyStringBuilder.operator +(s, "world")
      //
      // e o proprio objeto interno recebe Append("world").
      s += "world"; // will work even without op+ in MyStringBuilder
                    // why? you figure it out!

      // Observacao sobre o comentario original:
      // mesmo sem operator +, `s += "world"` ainda poderia compilar por outro
      // caminho da linguagem. Como o lado direito e string, o C# pode usar
      // concatenacao de string com object, chamar ToString() em `s`, produzir
      // uma string final e depois usar o operador implicito string -> MyStringBuilder.
      //
      // Com operator +, porem, a intencao fica explicita e evita esse caminho
      // indireto de converter para string e voltar para MyStringBuilder.
      WriteLine(s);
    }
  }
}
