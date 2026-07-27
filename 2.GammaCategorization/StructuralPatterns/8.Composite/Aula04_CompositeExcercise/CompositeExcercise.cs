// Consider the code presented below. 
// The Sum()  extension method adds up all the values in a list of IValueContainer 
// elements it gets passed. We can have a single value or a set of values.
// Complete the implementation of the interfaces so that Sum()  begins to work correctly.


using System;
using System.Collections;
using System.Collections.Generic;

  namespace Coding.Exercise
  {
    public interface IValueContainer
    {
      
    }

    public class SingleValue : IValueContainer
    {
      public int Value;
    }

    public class ManyValues : List<int>, IValueContainer
    {
      
    }

    public static class ExtensionMethods
    {
      public static int Sum(this List<IValueContainer> containers)
      {
        int result = 0;
        foreach (var c in containers)
        foreach (var i in c)
          result += i;
        return result;
      }
    }
  }
