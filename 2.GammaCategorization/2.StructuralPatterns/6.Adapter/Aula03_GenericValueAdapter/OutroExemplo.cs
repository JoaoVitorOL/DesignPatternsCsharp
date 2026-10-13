using System;

// ============================================================================
// Aula - Adapter de Valores Literais para Tipos Genéricos
// ----------------------------------------------------------------------------
// O PROBLEMA QUE ESTAMOS RESOLVENDO:
// * O C# nativamente NÃO suporta parâmetros genéricos de valor puro na assinatura 
//   (ex: não podemos fazer `var cupom = new PromoCode<6>()` para exigir tamanho 6).
//   A linguagem exige que todo parâmetro genérico `<T>` seja estritamente um Tipo.
// * Devido a isso, se quisermos regras estruturais fixas acopladas ao tipo (como
//   validadores de cupons com tamanhos estritos em tempo de compilação), teríamos
//   que criar classes separadas redundantes ou injetar valores dinâmicos via construtor,
//   perdendo a validação e segurança do sistema de tipos da linguagem.
//
// A SOLUÇÃO (PADRÃO ADAPTER DE VALOR PARA TIPO):
// * Adaptamos um valor numérico bruto para que ele se comporte como um "Tipo" (Classe).
// * Criamos uma interface comum (`IInteger`) que expõe um valor inteiro e criamos 
//   classes pequenas (`Six`, `Eight`) que a implementam retornando valores fixos.
// * Ao passar essas classes como parâmetro genérico (`TLength`), a classe alvo consegue
//   instanciá-las dinamicamente em tempo de execução e ler seu valor numérico interno.
// * Isso permite embutir constantes numéricas diretamente na assinatura do tipo genérico!
// ============================================================================
namespace AdapterPatternsDemo.GenericValue
{
    // O Contrato do Adapter: Transforma um "Tipo" em um "Valor numérico" para o compilador
    public interface IInteger
    {
        int Value { get; }
    }

    // Os Adaptadores de Valor (Classes estruturais que representam números literais)
    public static class Length
    {
        public class Six : IInteger { public int Value => 6; }
        public class Eight : IInteger { public int Value => 8; }
    }

    // A classe genérica que usa o valor adaptado para aplicar regras de negócios
    public class PromoCode<TLength> where TLength : IInteger, new()
    {
        private readonly int _requiredLength;

        public PromoCode()
        {
            // Instancia a classe que atua como metadado e extrai seu valor inteiro correspondente
            // Ex: se TLength for Six, 'new Six().Value' retorna o número 6.
            _requiredLength = new TLength().Value;
        }

        public bool Validate(string code)
        {
            return !string.IsNullOrWhiteSpace(code) && code.Length == _requiredLength;
        }

        public int AllowedLength => _requiredLength;
    }

    public static class Program
    {
        public static void Main()
        {
            Console.Clear();
            Console.WriteLine("--- TESTE: GENERIC VALUE ADAPTER ---");
            
            // Criamos um validador de cupom que EXIGE estritamente 6 caracteres através do tipo 'Six'
            var validadorBlackFriday = new PromoCode<Length.Six>();
            string cupom1 = "BF2026"; 
            string cupom2 = "SUPERDESCONTO"; 

            Console.WriteLine($"Configurado para cupons de {validadorBlackFriday.AllowedLength} caracteres.");
            Console.WriteLine($"O cupom '{cupom1}' é válido? {validadorBlackFriday.Validate(cupom1)}");
            Console.WriteLine($"O cupom '{cupom2}' é válido? {validadorBlackFriday.Validate(cupom2)}\n");

            // Criamos outro validador que EXIGE estritamente 8 caracteres através do tipo 'Eight'
            var validadorNatal = new PromoCode<Length.Eight>();
            string cupomNatal = "NATAL026"; 

            Console.WriteLine($"Configurado para cupons de {validadorNatal.AllowedLength} caracteres.");
            Console.WriteLine($"O cupom '{cupomNatal}' é válido? {validadorNatal.Validate(cupomNatal)}");
        }
    }
}