using System;

namespace CentralDeAlertasNamespace
{
    class Program
    {
        static void Main(string[] args)
        {
            // Criando objetos dos órgãos
            POLICIA POLICIA = new POLICIA();
            BOMBEIRO BOMBEIRO = new BOMBEIRO();
            SAMU SAMU = new SAMU();

            // Comprovando que a CentralDeAlertas é a mesma instância através do GetHashCode()
            Console.WriteLine("=== COMPROVAÇÃO DE INSTÂNCIA ÚNICA (HASHCODE) ===");
            Console.WriteLine($"Hash via Polícia:   {CentralDeAlertas.Instancia.GetHashCode()}");
            Console.WriteLine($"Hash via BOMBEIRO: {CentralDeAlertas.Instancia.GetHashCode()}");
            Console.WriteLine($"Hash via SAMU:      {CentralDeAlertas.Instancia.GetHashCode()}");
            Console.WriteLine("===============================================\n");

            // Cada órgão envia um alerta
            POLICIA.EnviarAlerta("Ocorrência de roubo na Zona Sul.");
            BOMBEIRO.EnviarAlerta("Princípio de incêndio em edificação comercial.");
            SAMU.EnviarAlerta("Acidente de trânsito grave na Rodovia Principal.");

            // Listando a partir de órgãos diferentes para provar o compartilhamento do histórico
            Console.WriteLine("--- Histórico visualizado pela POLÍCIA ---");
            foreach (var alerta in POLICIA.ListarAlertas())
            {
                Console.WriteLine(alerta);
            }

            Console.WriteLine("\n--- Histórico visualizado pelo SAMU ---");
            foreach (var alerta in SAMU.ListarAlertas())
            {
                Console.WriteLine(alerta);
            }
            
            Console.ReadKey();
        }
    }
}