using System;
using System.Collections.Generic;
using System.IO;
using static System.Console;

namespace AulasSOLIDpatterns.Aula01_SingleResponsibility
{
    


// !!!!! CLASSE DE JOURNAL !!!!!!!!!!!!!!!!!!!!!!!

// ===== Classe =====
public class JournalCerto
{
    // ===== Campos =====
    private readonly List<string> entries = new List<string>();

    private static int count = 0;

// =================================
// Escrever no Journal
// =================================

    // ===== Metodos =====
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
}




// -----------------------------------------------------------------------------------
// Implementado Corretamente Single Responsability !!!
// ----------------------------------------------------------------------------------



//!!! CLASSE DE PERSISTÊNCIA !!!!!!!!!!!!!!!!!!!!!!!

// ===== Classe =====
public class Persistence
{
    // ===================================================================================================
    // Salvar Journal na classe Persistance
    // ===================================================================================================

    // ===== Metodos =====
    public void SavetoFile(JournalCerto journal, string filename, bool overwrite = false)
    {
        if (overwrite || !File.Exists(filename))
        {
            var directory = Path.GetDirectoryName(filename);
            if (!string.IsNullOrWhiteSpace(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(filename, journal.ToString());
        }
    }
}

// O que é o Single Responsability?
// Principio SOLID que diz que uma classe deve ter apenas uma razão para mudar, ou seja, ela deve ter apenas uma responsabilidade.
// No exemplo acima, a classe Journal é responsável apenas por gerenciar entradas de diário,
// enquanto a classe Persistence é responsável por salvar e carregar essas entradas de um arquivo.
// Isso separa as responsabilidades e torna o código mais modular e fácil de manter.

// ===== Classe =====
public class CertoMain
{
    // ===== Metodos =====
    public static void RunDemo()
    {
        var j = new JournalCerto();
        j.AddEntry("Querido diário, Ontem foi uma bosta");
        j.AddEntry("Querido diário, Hoje foi uma bosta");
        j.AddEntry("Querido diário, Amanhã será uma bosta");
        WriteLine(j);

        var p = new Persistence();
        var aulaPath = GetAulaPath();
        var filename = Path.Combine(aulaPath, "journal.txt");
        p.SavetoFile(j, filename, true);
        WriteLine($"Arquivo salvo pela classe Persistence em: {filename}");
    }

    static string GetAulaPath()
    {
        var current = new DirectoryInfo(Directory.GetCurrentDirectory());

        while (current is not null)
        {
            var fromProject = Path.Combine(current.FullName, "aulasSolidPattern", "Aula01_SingleResponsibility");
            if (Directory.Exists(fromProject))
            {
                return fromProject;
            }

            var fromRoot = Path.Combine(current.FullName, "src", "MeuSistema.ConsoleApp", "aulasSolidPattern", "Aula01_SingleResponsibility");
            if (Directory.Exists(fromRoot))
            {
                return fromRoot;
            }

            current = current.Parent;
        }

        throw new DirectoryNotFoundException("Nao foi possivel localizar a pasta Aula01_SingleResponsibility.");
    }
}
}
