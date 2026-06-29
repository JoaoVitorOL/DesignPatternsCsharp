// Exemplo da aula:
// um builder funcional para montar uma `Person`.
//
// A diferenca principal para a aula anterior (Stepwise Builder) e esta:
// - no Stepwise Builder, o foco era FORCAR A ORDEM dos passos
//   por meio de interfaces diferentes a cada etapa;
// - no Functional Builder, o foco muda para COMPOR OPERACOES
//   que serao aplicadas ao objeto so no final.
//
// Entao a pergunta central deixa de ser:
// "qual e o proximo passo obrigatorio?"
//
// e passa a ser:
// "quais operacoes eu quero acumular para materializar o objeto depois?"
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Console;

namespace Aula04_FunctionalBuilder
{
    public class Person
    {
        public string Name;
        public string Position;

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}, {nameof(Position)}: {Position}";
        }
    }

    // Esta classe base generaliza a ideia do Functional Builder.
    //
    // `TSubject` = qual objeto final esta sendo construido
    // `TSelf`    = qual e o tipo concreto do builder que esta encadeando
    //
    // O mesmo padrao de "SELF" apareceu na aula de recursive generics:
    // ele serve para manter o tipo concreto correto na API fluent.
    public abstract class FunctionalBuilder<TSubject, TSelf>
        where TSubject : new()
        where TSelf : FunctionalBuilder<TSubject, TSelf>
    {
        // Em vez de guardar o objeto pronto e ir mutando ele imediatamente,
        // o builder funcional guarda uma lista de "acoes de construcao".
        //
        // Cada item da lista recebe um `TSubject`, aplica alguma mudanca
        // e devolve o mesmo objeto para a proxima etapa.
        private readonly List<Func<TSubject, TSubject>> actions =
            new List<Func<TSubject, TSubject>>();

        // `Do()` e a porta de entrada publica para registrar uma nova operacao.
        //
        // Repare na diferenca mental para o Stepwise Builder:
        // la, o retorno mudava de interface para limitar o que o cliente podia chamar.
        // aqui, o retorno continua sendo o builder, porque o objetivo nao e restringir ordem;
        // o objetivo e acumular comportamento.
        public TSelf Do(Action<TSubject> action)
        {
            return AddAction(action);
        }

        // O objeto final so nasce de verdade aqui.
        //
        // `new TSubject()` cria a instancia base.
        // `Aggregate()` percorre a lista de funcoes acumuladas
        // e aplica uma por uma sobre o mesmo objeto.
        //
        // Leitura mental:
        // 1. cria uma Person vazia
        // 2. aplica Called(...)
        // 3. aplica WorksAsA(...)
        // 4. devolve a Person final
        public TSubject Build()
        {
            return actions.Aggregate(
                new TSubject(),
                (subject, action) => action(subject));
        }

        // `Action<TSubject>` so executa algo e retorna `void`.
        // Mas a lista interna guarda `Func<TSubject, TSubject>`,
        // porque queremos encadear transformacoes que devolvem o proprio objeto.
        //
        // Entao este metodo faz a adaptacao:
        // ele empacota uma `Action` dentro de uma `Func`
        // que muta o objeto e depois devolve esse mesmo objeto.
        private TSelf AddAction(Action<TSubject> action)
        {
            actions.Add(subject =>
            {
                action(subject);
                return subject;
            });

            return (TSelf)this;
        }
    }

    // Builder concreto da nossa pessoa.
    //
    // Aqui a ideia e simples:
    // cada metodo nao constroi a pessoa imediatamente.
    // Ele apenas registra mais uma operacao na lista interna.
    public sealed class PersonBuilder
        : FunctionalBuilder<Person, PersonBuilder>
    {
        public PersonBuilder Called(string name)
        {
            return Do(person => person.Name = name);
        }
    }

    // Um detalhe legal deste estilo e que novos passos podem nascer
    // fora da classe principal, via extension methods.
    //
    // Isso combina bem com o espirito "compor pequenas operacoes".
    // Em vez de concentrar tudo dentro de `PersonBuilder`,
    // podemos pluggar novos comportamentos por fora.
    public static class PersonBuilderExtensions
    {
        public static PersonBuilder WorksAsA(
            this PersonBuilder builder,
            string position)
        {
            return builder.Do(person => person.Position = position);
        }
    }

    public class Demo
    {
        public static void Main(string[] args)
        {
            // Repare na leitura da cadeia:
            // - `Called()` registra uma operacao
            // - `WorksAsA()` registra outra operacao
            // - `Build()` finalmente materializa a Person
            //
            // Diferenca para o Stepwise Builder:
            // aqui nada esta forcando uma ordem obrigatoria por interfaces.
            // O ganho nao e "seguranca de fluxo";
            // o ganho e "composicao flexivel de passos".
            var person = new PersonBuilder()
                .Called("Sara")
                .WorksAsA("Professor")
                .Build();

            WriteLine(person);
        }
    }
}
