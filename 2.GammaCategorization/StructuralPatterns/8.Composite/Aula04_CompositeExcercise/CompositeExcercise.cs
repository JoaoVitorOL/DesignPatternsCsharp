//  Exercicio:
//  complete a implementacao das interfaces para que o metodo de extensao Sum()
//  consiga somar todos os valores recebidos em uma lista de IValueContainer.
//
//  Importante:
//  - a lista pode conter um valor unico (SingleValue)
//  - a lista tambem pode conter varios valores (ManyValues)
//  - o metodo Sum() nao deve precisar distinguir manualmente esses dois casos
//  - a solucao deve permitir que ambos sejam percorridos com foreach como
//    sequencias de inteiros
//
//  Em outras palavras, o Composite deve:
//  1. criar um contrato comum para "algo que contem valores inteiros"
//  2. fazer SingleValue se comportar como uma colecao de um unico int
//  3. fazer ManyValues se comportar como uma colecao de varios ints
//  4. permitir que Sum() some tudo usando apenas o contrato IValueContainer


using System;
using System.Collections;
using System.Collections.Generic;

namespace Coding.Exercise
  {
    // 1o Identifique o contrato comum que o cliente precisa consumir.
    // Sum() usa foreach em cada IValueContainer, entao a interface precisa
    // herdar de IEnumerable<int>.

    // ===== Interface Component =====
    public interface IValueContainer : IEnumerable<int>
    {
      
    }

    // 2o Modele o valor unico como uma folha do Composite.
    // Mesmo tendo apenas um inteiro, ele precisa ser percorrivel como sequencia.

    // ===== Classe Leaf =====
    public class SingleValue :  IValueContainer
    {
      // ===== Campos =====
      public int Value;

      // ===== Metodos =====
      // Este enumerador faz o SingleValue entregar um unico int ao foreach.
      // Exemplo: SingleValue { Value = 5 } se comporta como [5].
      public IEnumerator<int> GetEnumerator()
      {
        yield return Value;
      }

      // Implementacao exigida pela interface IEnumerable nao generica.
      // Delegamos para o enumerador generico para manter uma unica logica.
      IEnumerator IEnumerable.GetEnumerator()
      {
        return GetEnumerator();
      }
    }

    // 3o Modele o conjunto de valores como o Composite.
    // List<int> ja implementa IEnumerable<int>, entao ManyValues herda esse
    // comportamento e tambem cumpre o contrato IValueContainer.

    // ===== Classe Composite =====
    public class ManyValues : List<int>, IValueContainer
    {
      
    }

    // ===== Classe Auxiliar =====
    public static class ExtensionMethods
    {
      // ===== Metodos =====
      // 4o O cliente trabalha apenas com IValueContainer.
      // Ele nao pergunta se o item atual e SingleValue ou ManyValues.
      public static int Sum(this List<IValueContainer> containers)
      {
        int result = 0;

        // Primeiro foreach:
        // percorre os containers recebidos.
        foreach (var c in containers)
          // Segundo foreach:
          // percorre os inteiros expostos por cada container.
          foreach (var i in c)
            result += i;

        return result;
      }
    }
  }
