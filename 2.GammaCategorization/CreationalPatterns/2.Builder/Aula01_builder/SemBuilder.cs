using System;
using System.Collections.Generic;
using static System.Console;

namespace Aula01_builder
{
    // ===== Builder =====
    public class SemBuilder
    {
        // "SemBuilder" aqui significa: sem um Builder do padrão de projeto.
        // Importante: ainda usamos System.Text.StringBuilder, mas isso é outra coisa.
        // StringBuilder resolve montagem eficiente de texto.
        // O padrão Builder resolve organização da construção de um objeto complexo.

        // ===== Metodos =====
        static void Main(string[] args)
        {
            // Caso 1: montar um parágrafo simples "na mão".
            // Para algo pequeno, isso ainda parece aceitável.
            // O problema aparece quando a estrutura cresce.

            var hello = "hello";
            var sb = new System.Text.StringBuilder();

            // Aqui estamos pensando em "pedaços de texto", não em "estrutura HTML".
            // Ou seja: o código conhece diretamente as tags e a ordem em que elas entram.
            sb.Append("<p>");
            sb.Append(hello);
            sb.Append("</p>");
            WriteLine(sb);

            // Caso 2: agora queremos uma lista HTML.
            // Ainda funciona, mas já dá para perceber que a intenção do código
            // começa a ficar escondida por detalhes operacionais:
            // abrir tag, fechar tag, loop, formatar item, etc.
            var words = new[] { "hello", "world" };
            sb.Clear();
            sb.Append("<ul>");

            foreach (var word in words)
            {
                // Cada item exige lembrar manualmente do formato HTML correto.
                // O código cliente está fazendo "trabalho de montagem".
                sb.AppendFormat("<li>{0}</li>", word);
            }

            sb.Append("</ul>");
            WriteLine(sb);

            // Comparação conceitual com o arquivo Builder.cs:
            // Aqui nós montamos TEXTO.
            // Lá nós montamos OBJETOS (uma árvore de elementos HTML),
            // e só no final essa estrutura vira string.
            //
            // Essa é a principal ideia que o professor está comparando:
            // sem Builder -> o cliente constrói tudo manualmente;
            // com Builder -> o cliente descreve o que quer construir,
            // e o Builder cuida dos detalhes da montagem.

        }
    }
}
