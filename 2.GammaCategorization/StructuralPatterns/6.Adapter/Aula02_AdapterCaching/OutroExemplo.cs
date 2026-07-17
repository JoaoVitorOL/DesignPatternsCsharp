using System;
using System.Collections.Generic;

// ============================================================================
// Aula - Adapter com Caching (Padrão Adapter Otimizado)
// ----------------------------------------------------------------------------
// O QUE É: Uma evolução direta do Adapter básico. Aqui, adicionamos um mecanismo
//           de Cache temporário para evitar o reprocessamento redundante de dados.
//
// O PROBLEMA QUE ESTAMOS RESOLVENDO:
// * No fluxo de e-commerce, toda vez que o sistema precisa exibir ou processar
//   os metadados de um pedido (ex: gerar um sumário textual para notas fiscais
//   ou telas de listagem), o adaptador é instanciado e reconcatena strings,
//   faz formatações de moeda ou executa lógicas pesadas do zero.
// * Se o mesmo pedido for renderizado várias vezes na tela (ou atualizado em lote),
//   isso causa desperdício de CPU e alocação desnecessária de memória para strings.
//
// A SOLUÇÃO COM CACHE:
// * Guardamos os sumários já processados em um Dicionário Estático (`_summaryCache`),
//   usando o ID único do pedido (`Order.Id`) como chave de busca.
// * Se o sistema pedir para adaptar um pedido que já foi processado antes, nós
//   simplesmente pulamos a etapa de cálculo e reaproveitamos o resultado em memória.
// ============================================================================
namespace AdapterPatternsDemo.Caching
{
    // O Adaptee (Classe existente com os dados brutos)
    public class Order
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
    }

    // O Target (A interface limpa que o sistema espera)
    public interface IOrderMetadata
    {
        string DisplaySummary { get; }
    }

    // O Adapter Otimizado com Cache
    public class WebOrderMetadataAdapter : IOrderMetadata
    {
        private readonly Order _order;
        
        // Cache estático: Chave (ID do Pedido) -> Valor (Sumário textual já calculado)
        private static readonly Dictionary<int, string> _summaryCache = new();
        private static int _generationCounter = 0;

        public WebOrderMetadataAdapter(Order order)
        {
            _order = order ?? throw new ArgumentNullException(nameof(order));
            
            // [VERIFICAÇÃO DO CACHE]: Se o dicionário já possuir essa chave, abortamos o processamento!
            if (_summaryCache.ContainsKey(_order.Id)) return;

            // Se não estiver no cache, executa a formatação ("processamento pesado") uma única vez
            _generationCounter++;
            string computedSummary = $"[Pedido #{_order.Id}] Cliente: {_order.CustomerName} | Total: R$ {_order.TotalAmount:F2}";
            
            Console.WriteLine($"[Processamento Pesado #{_generationCounter}] Gerando sumário para o Pedido {_order.Id}...");
            
            // [SALVANDO NO CACHE]: Registra para consultas futuras
            _summaryCache.Add(_order.Id, computedSummary);
        }

        // Retorna o valor direto do cache usando o ID como chave
        public string DisplaySummary => _summaryCache[_order.Id];
    }

    public static class Program
    {
        public static void Main()
        {
            Console.Clear();
            Console.WriteLine("--- TESTE: ADAPTER COM CACHE ---");
            var pedidoDoJoao = new Order { Id = 1001, CustomerName = "João Silva", TotalAmount = 250.50m };

            // Primeira chamada: vai disparar a geração (log do processamento pesado aparece)
            var adapter1 = new WebOrderMetadataAdapter(pedidoDoJoao);
            Console.WriteLine($"Resultado 1: {adapter1.DisplaySummary}\n");

            // Segunda chamada: Resolve instantaneamente via cache (não gera novo log)
            Console.WriteLine("--> Solicitando o mesmo sumário novamente...");
            var adapter2 = new WebOrderMetadataAdapter(pedidoDoJoao);
            Console.WriteLine($"Resultado 2 (Via Cache): {adapter2.DisplaySummary}\n");
        }
    }
}