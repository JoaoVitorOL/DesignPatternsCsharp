using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.Marshalling;
using static System.Console;

namespace AulasSOLIDpatterns.Aula04_InterfaceSegregation
{
    // O principio de Interface Segregation diz que interfaces devem ser pequenas
    // e focadas no que cada cliente realmente precisa.
    //
    // A ideia nao e criar "uma interface para tudo".
    // A ideia e separar contratos por capacidade.
    //
    // Assim, cada classe implementa apenas o que faz sentido para ela.
    //
    // O que e uma interface?
    // Interface e um contrato.
    // Ela define quais operacoes uma classe precisa oferecer,
    // mas nao obriga todas as classes a terem a mesma implementacao.
    // Cada classe pode cumprir esse contrato do seu proprio jeito.

    // ================================================
    //  Classe Documento
    // ================================================
    public class Document
    {
        
    }


// ================================================
//  Interfaces que maquinas podem implementar isoladamente
// ================================================
    // Aqui esta a correcao conceitual do exemplo:
    // em vez de uma interface unica com tudo,
    // o codigo foi dividido em interfaces menores e especificas.
    //
    // Isso permite montar apenas os contratos necessarios para cada maquina.

    public interface IPrinter
    {
        // Interface pequena: apenas imprimir.
        void Print(Document2 d);
    }

    public interface IScanner
    {
        // Interface pequena: apenas escanear.
        void Scan(Document2 d);
    }

    public interface IFax
    {
        // Interface pequena: apenas enviar fax.
        void Fax(Document2 d);
    }

// ==========================================================================================
//  Interface especifica para maquinas multifuncionais (Usam todas as interfaces acima)
// ==========================================================================================
    public interface IMultiFunctionDevice: IPrinter,IScanner,IFax
    {
        // Esta interface nao cria novos metodos.
        // Ela apenas combina interfaces menores.
        //
        // Ou seja:
        // uma maquina multifuncional continua podendo expor tudo,
        // mas sem obrigar TODAS as outras maquinas a fazerem o mesmo.
    }


// ================================================
//  Classe de PhotoCopier 
// ================================================
    public class PhotoCopier: IPrinter, IScanner // Implementando mais de uma interface
    {
        // Por que certo.cs e certo?
        //
        // Porque PhotoCopier implementa somente o que ela realmente faz:
        // imprimir e escanear.
        //
        // Ela NAO e obrigada a implementar fax,
        // porque o contrato foi segregado em interfaces menores.
        public void Print(Document2 d)
        {
            // implementa do seu jeito
        }

        public void Scan(Document2 d)
        {
            // implementa do seu jeito
        }
    }


// ================================================
// Classe da impressora multifuncional
// ================================================
    public class MultiFunctionPrinter: IMultiFunctionDevice
    {
        // Aqui aparece outro efeito positivo da mudanca:
        // como os contratos ficaram separados,
        // uma multifuncional pode ser montada por composicao,
        // recebendo uma implementacao para imprimir,
        // outra para escanear
        // e outra para fax.
        //
        // Assim, a classe depende de capacidades especificas,
        // nao de uma implementacao gigante e fixa.
        private IPrinter printer;
        private IScanner scanner;
        private IFax fax;


        public MultiFunctionPrinter(IPrinter printer, IScanner scanner, IFax fax)
        {
            if (printer == null)
            {
                throw new ArgumentNullException(paramName: nameof(printer));
            }

            if (scanner == null)
            {
                throw new ArgumentNullException(paramName: nameof(scanner));
            }

            if (fax == null)
            {
                throw new ArgumentNullException(paramName: nameof(fax));
            }

            this.printer = printer;
            this.scanner = scanner;
            this.fax = fax;
        }

        public void Print(Document2 d)
        {
            printer.Print(d);
        }

        public void Scan(Document2 d)
        {
            scanner.Scan(d);
        }

        public void Fax(Document2 d)
        {
            fax.Fax(d);
            // Ideia de Decorator:
            // esta classe envolve outras implementacoes e delega o trabalho para elas.
            // Um decorator classico faria isso e ainda poderia adicionar comportamento
            // antes ou depois da chamada, sem mudar a interface publica.
        }
        
        // Qual efeito a mudanca teve no codigo para torna-lo certo?
        //
        // 1. A interface grande foi quebrada em interfaces menores.
        // 2. Cada classe passou a implementar apenas o que realmente usa.
        // 3. Sumiu a necessidade de metodos "falsos" ou sem sentido.
        // 4. O codigo ficou mais flexivel para composicao e reutilizacao.
        // 5. O acoplamento diminuiu, porque cada classe depende so do contrato necessario.
    }

}
