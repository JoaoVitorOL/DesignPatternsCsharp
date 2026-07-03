using System;
using System.Collections.Generic;
using System.Text;
using static System.Console;

namespace Aula05_ObjectTrackingandBulkReplacement
{
    // A aula mostra dois cenarios independentes baseados na mesma ideia:
    // centralizar a criacao em uma factory.
    // Importante: isso NAO significa que toda factory precisa fazer essas coisas.
    // Tracking e bulk replacement sao capacidades opcionais, usadas quando o problema pede.
    // 1. Object Tracking: observar o que ja foi criado.
    // 2. Bulk Replacement: trocar, de uma vez, o tema visto por varios clientes.

    // ===== Interface =====
    public interface ITheme
    {
        // Product Abstraction: contrato que o cliente enxerga.

        // ===== Propriedades =====
        string TextColor { get; }
        string BackgroundColor { get; }
    }

    // ===== Classe =====
    internal sealed class LightTheme : ITheme
    {
        // Concrete Product: tema claro.

        // ===== Propriedades =====
        public string TextColor => "Black";
        public string BackgroundColor => "White";
    }

    // ===== Classe =====
    internal sealed class DarkTheme : ITheme
    {
        // Concrete Product: tema escuro.

        // ===== Propriedades =====
        public string TextColor => "White";
        public string BackgroundColor => "Black";
    }

//xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
    // ===== Factory Object Tracking =====
    public class TrackingThemeFactory
    {
        // Objetivo: observar os temas que ja nasceram.
        // Ela registra e inspeciona; nao altera os temas ja entregues.
        // Este e um estilo possivel de factory, nao uma obrigacao do pattern.

        // ===== Campos =====
        private readonly List<WeakReference<ITheme>> themes = new();

        // A lista guarda rastros dos temas criados para consulta posterior.
        // WeakReference observa o objeto sem impedir sua coleta pelo GC.

        // ===== Metodos =====
        public ITheme CreateTheme(bool dark)
        {
            // A factory escolhe qual produto concreto criar.
            ITheme theme = dark ? new DarkTheme() : new LightTheme();

            // Registra o tema sem virar dona do ciclo de vida dele.
            themes.Add(new WeakReference<ITheme>(theme));
            return theme;
        }

        // ===== Propriedades =====
        public string Info
        {
            // `get` executa este bloco a cada leitura.
            // O relatorio nao fica salvo; ele e montado na hora.
            get
            {
                // StringBuilder monta varias partes de texto com menos ruido.
                var sb = new StringBuilder();

                for (int i = 0; i < themes.Count; i++)
                {
                    var reference = themes[i];

                    // Tenta recuperar o tema real, se ele ainda estiver vivo.
                    if (reference.TryGetTarget(out var theme))
                    {
                        sb.Append("Theme #")
                            .Append(i + 1)
                            .Append(": ")
                            .Append(theme is DarkTheme ? "DarkTheme" : "LightTheme")
                            .Append(" -> TextColor: ")
                            .Append(theme.TextColor)
                            .Append(", BackgroundColor: ")
                            .Append(theme.BackgroundColor)
                            .AppendLine();
                    }
                    else
                    {
                        sb.Append("Theme #")
                            .Append(i + 1)
                            .AppendLine(": collected");
                    }
                }

                return sb.ToString();
            }
        }
    }



//xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
 
    // ===== Factory Bulk Replacement =====
    public class ReplaceableThemeFactory
    {
        // Objetivo: trocar em massa o tema que varios clientes enxergam.
        // Ela entrega handles mutaveis e depois atualiza o `Value` de todos eles.
        // Este e outro estilo possivel de factory, usado quando a troca centralizada faz sentido.

        // ===== Campos =====
        private readonly List<WeakReference<Ref<ITheme>>> themes = new();

        // Aqui o rastreamento existe como meio para a troca em massa.
        // A factory rastreia os handles, nao os temas crus.

        // ===== Metodos =====
        private static ITheme CreateThemeImpl(bool dark)
        {
            // Politica central de criacao do tema.
            return dark ? new DarkTheme() : new LightTheme();
        }

        public Ref<ITheme> CreateTheme(bool dark)
        {
            // O cliente recebe um handle; o tema real fica dentro de `Value`.
            var reference = new Ref<ITheme>(CreateThemeImpl(dark));
            themes.Add(new WeakReference<Ref<ITheme>>(reference));
            return reference;
        }

        public void ReplaceTheme(bool dark)
        {
            // "Troca em massa" = uma unica chamada atualiza varios clientes de uma vez.
            // Ex.: varias telas deixam de ver DarkTheme e passam a ver LightTheme.
            foreach (var weakReference in themes)
            {
                if (weakReference.TryGetTarget(out var reference))
                {
                    // O cliente continua com o mesmo handle; so o Value muda.
                    reference.Value = CreateThemeImpl(dark);
                }
            }
        }
    }

    // ===== Classe Handle Mutavel =====
    public class Ref<T> where T : class
    {
        // Pense em `Ref<T>` como uma caixa.
        // O cliente segura a caixa; a factory pode trocar o objeto dentro dela.
        // A caixa e o handle. O objeto dentro dela e o alvo atual.

        // ===== Propriedades =====
        public T Value { get; set; }

        // ===== Construtores =====
        public Ref(T value)
        {
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }
    }

    // ===== Classe =====
    public class Demo
    {
        // Client do exemplo: conversa com as factories, nao com classes concretas.

        // ===== Metodos =====
        static void Main(string[] args)
        {
            WriteLine("=== Object Tracking ===");

            var trackingFactory = new TrackingThemeFactory();

            // O cliente recebe o contrato `ITheme`.
            var trackedDarkTheme = trackingFactory.CreateTheme(dark: true);
            var trackedLightTheme = trackingFactory.CreateTheme(dark: false);

            WriteLine(
                $"trackedDarkTheme -> TextColor: {trackedDarkTheme.TextColor}, " +
                $"BackgroundColor: {trackedDarkTheme.BackgroundColor}");
            WriteLine(
                $"trackedLightTheme -> TextColor: {trackedLightTheme.TextColor}, " +
                $"BackgroundColor: {trackedLightTheme.BackgroundColor}");
            WriteLine();
            WriteLine("Info da factory sobre os temas criados:");
            WriteLine(trackingFactory.Info);

            WriteLine("=== Bulk Replacement ===");

            var replaceableFactory = new ReplaceableThemeFactory();

            // Dois clientes diferentes recebem handles distintos.
            var headerTheme = replaceableFactory.CreateTheme(dark: true);
            var footerTheme = replaceableFactory.CreateTheme(dark: true);

            WriteLine("Antes da troca em massa:");
            WriteLine(
                $"headerTheme -> {headerTheme.Value.BackgroundColor}");
            WriteLine(
                $"footerTheme -> {footerTheme.Value.BackgroundColor}");

            // Uma unica chamada troca o tema visto por todos os handles vivos.
            replaceableFactory.ReplaceTheme(dark: false);

            WriteLine("Depois da troca em massa:");
            WriteLine(
                $"headerTheme -> {headerTheme.Value.BackgroundColor}");
            WriteLine(
                $"footerTheme -> {footerTheme.Value.BackgroundColor}");
        }
    }
}
