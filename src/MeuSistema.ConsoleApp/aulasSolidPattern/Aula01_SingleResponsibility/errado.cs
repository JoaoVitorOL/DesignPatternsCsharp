using System;
using System.Collections.Generic;
using static System.Console;

namespace AulasSOLIDpatterns.Aula01_SingleResponsibility
{



// !!! Apenas a classe Journal !!!!!!!!
public class JournalErrado
{
    private readonly List<string> entries = new List<string>();

    private static int count = 0;

// =================================
// Escrever no Journal
// =================================
    public int AddEntry(string text)
    {
        entries.Add($"{++count}: {text}");
        return count;
    }

// =================================
// Apagar no Journal
// =================================
    public void RemoveEntry(int index)
    {
        entries.RemoveAt(index);
    }


// =================================
// Imprimir Journal
// =================================
    public override string ToString()
    {
        return string.Join(Environment.NewLine, entries);
    }




// x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-
// VIOLAÇÔES da Single Responsability !!! A partir daqui deixa de fazer sentido !!!
// x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-



// ===================================================================================================
// Salvar Journal ??? (Isso não faz sentido em ser responbalidiade de um Journal)
// ===================================================================================================
    public void Save(string filename)
    {
        File.WriteAllText(filename,ToString());
    }
    

//===================================================================================================================================
// Carregar Journal ??? (Isso não faz sentido em ser responbalidiade de um Journal)
//===================================================================================================

   public static JournalErrado Load(string filename)
    {
        var journal = new JournalErrado();
        journal.entries.AddRange(File.ReadAllLines(filename));
        return journal;
        
    }


    public void Load(Uri uri)
    {
        // Carregar Journal a partir de uma URI ...
    }


// Como pode ver, conforme o desenvolvedor vai tendo ideais, ele pode acabar colocando
// responsabilidades que não fazem sentido para o que a classe foi projetada inicialmente.
// "Mas, ainda tem a ver", "Mas é só um métodozinho"
// E assim, a classe vai ficando cada vez mais inchada e difiícil de manter
// Além disso, se a classe está complicada demais, sua legibilidade é comprometida.
// "Faz o que então?"
// Nesse caso, uma nova classe específica para lidar com a persistência de arquivos


}

public class ErradoMain
{
    public static void RunDemo()
    {
        var j = new JournalErrado();
        j.AddEntry("Querido diário, Ontem foi uma bosta");
        j.AddEntry("Querido diário, Hoje foi uma bosta");
        j.AddEntry("Querido diário, Amanhã será uma bosta");
        WriteLine(j);
    }
}
}
