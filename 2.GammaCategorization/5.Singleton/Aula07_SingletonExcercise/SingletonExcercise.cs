// Singleton Coding Exercise
// Since implementing a singleton is easy,
// you have a different challenge: write a method called IsSingleton(). 
// This method takes a factory method that returns an object 
// and it's up to you to determine 
// whether or not that object is a singleton instance.



using System;

namespace Coding.Exercise
{
    public class SingletonTester
    {
        public static bool IsSingleton(Func<object> func)
        {
            // Executa a fábrica pela primeira vez
            object instance1 = func();
            
            // Executa a fábrica pela segunda vez
            object instance2 = func();

            // Se ambas as referências apontarem para o mesmo objeto na memória, 
            // significa que a fábrica está gerenciando uma instância única (Singleton).
            return ReferenceEquals(instance1, instance2);
        }
    }
}

