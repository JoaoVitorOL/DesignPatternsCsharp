using System;
using System.Reflection;
using Microsoft.VisualBasic;

// ============================================================================
// Aula04 - Monostate
// ----------------------------------------------------------------------------
// Monostate é uma variação do Singleton que resolve o mesmo problema de
// negócio - "garantir que todas as instâncias compartilhem o mesmo estado" -
// só que por um caminho estrutural diferente.
//
// No Singleton "clássico" (Aula01), existe UMA instância física na memória,
// e o acesso a ela é feito através de um ponto único e estático
// (`SingletonDatabase.Instance`). Quem quer usar o objeto precisa,
// obrigatoriamente, passar por esse ponto de acesso.
//
// No Monostate, a história é outra: é possível criar QUANTAS instâncias
// quiser com `new CEO()` normalmente - a classe não impede isso, não tem
// construtor privado, não tem `Instance` estático. A "mágica" está em outro
// lugar: os CAMPOS que guardam o estado (`name`, `age`) são `static`. Como
// campos estáticos pertencem à CLASSE, e não a cada instância, todo objeto
// `CEO` criado - não importa quantos - está lendo e escrevendo exatamente
// os mesmos dois campos na memória. O efeito prático de "estado único
// compartilhado" é idêntico ao do Singleton, mas a API pública da classe
// se comporta como se fosse uma classe comum, instanciável livremente.
// ============================================================================
namespace Aula04_Monostate
{
    // ===== Classe =====
    public class CEO
    {
        // ===== Campos =====
        // O `static` aqui é o núcleo inteiro do padrão Monostate.
        // Campos estáticos existem uma única vez por classe, não uma vez
        // por objeto. Ou seja: mesmo que existam 10 instâncias de `CEO`
        // ao mesmo tempo, existe apenas UM `name` e UM `age` na memória,
        // e todas as instâncias enxergam e alteram esses mesmos valores.
        private static string name;
        private static int age;

        // ===== Propriedades =====
        // As propriedades públicas parecem propriedades de instância
        // normais (`ceo.Name`, `ceo2.Name`), mas por baixo dos panos
        // sempre leem e escrevem no campo estático `name` - o mesmo
        // campo, não importa em qual instância a propriedade é acessada.
        public string Name
        {
            get => name;
            set => name = value;
        }

        // Mesma lógica de `Name`: a leitura/escrita aparenta ser "da
        // instância", mas na prática é sempre sobre o único `age`
        // estático compartilhado por toda a classe.
        public int Age
        {
            get => age;
            set => age = value;
        }

        // ===== Métodos =====
        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}, {nameof(Age)}: {Age}";
        }
    }

    // ===== Classe =====
    static class Program
    {
        // ===== Métodos =====
        static void Main(string[] args)
        {
            // Cria a primeira instância e preenche seu estado. Como
            // `Name` e `Age` só existem "por dentro" como campos
            // estáticos, este `set` está, na prática, escrevendo direto
            // nos únicos `name` e `age` que a classe `CEO` possui.
            var ceo = new CEO();
            ceo.Name = "Adam Smith";
            ceo.Age = 55;

            // Cria uma SEGUNDA instância, completamente nova, sem
            // atribuir nada a ela. Num objeto comum, `ceo2.Name` sairia
            // vazio/nulo e `ceo2.Age` sairia 0 - cada instância teria
            // seu próprio estado, independente.
            var ceo2 = new CEO();

            // A saída aqui é "Name: Adam Smith, Age: 55" - os MESMOS
            // valores atribuídos a `ceo`, mesmo `ceo2` nunca tendo
            // recebido nada diretamente. Isso comprova o comportamento
            // do Monostate: não existe UMA instância protegida (como no
            // Singleton), existem VÁRIAS instâncias, todas enxergando o
            // mesmo estado compartilhado por trás das propriedades.
            Console.WriteLine(ceo2);
        }
    }
}