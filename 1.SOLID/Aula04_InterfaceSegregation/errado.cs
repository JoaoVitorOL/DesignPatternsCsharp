using System;
using System.Collections.Generic;
using static System.Console;

namespace AulasSOLIDpatterns.Aula04_InterfaceSegregation
{
    // O principio de Interface Segregation diz que uma classe nao deve ser
    // obrigada a depender de metodos que ela nao usa.
    //
    // Em termos praticos:
    // e melhor ter interfaces menores e mais especificas
    // do que uma interface grande com responsabilidades demais.
    //
    // Quando a interface fica "gorda", classes simples acabam sendo forcadas
    // a implementar comportamentos que nao fazem sentido para elas.
    //
    // O que e uma interface?
    // Interface e um contrato.
    // Ela diz "quem implementar isso precisa oferecer estes metodos".
    // A interface define O QUE deve existir,
    // mas nao define COMO cada classe vai fazer isso.

    // ================================================
    //  Classe Documento
    // ================================================

    // ===== Classe =====
    public class Document2
    {
        
    }


// ================================================
//  Interface da maquina impressora multifuncional
// ================================================

    // ===== Interface =====
    public interface IMachine2
    {
        // Aqui esta o problema central do exemplo errado:
        // IMachine2 junta imprimir, escanear e enviar fax em uma unica interface.
        //
        // Isso significa que QUALQUER classe que implemente IMachine2
        // sera obrigada a oferecer os 3 comportamentos,
        // mesmo quando ela so deveria imprimir.

        // ===== Metodos =====
        void Print(Document2 d);
        void Scan(Document2 d);
        void Fax(Document2 d);
    }


// ================================================
//  Classe da impressora multifuncional
// ================================================

    // ===== Classe =====
    public class MultiFunctionPrinter2: IMachine2
    {
        // Neste caso, uma multifuncional de verdade ate poderia implementar tudo.
        // Entao aqui o problema quase nao aparece.
        //
        // O problema real aparece quando tentamos reutilizar a MESMA interface
        // em dispositivos mais simples.

        // ===== Metodos =====
        public void Print(Document2 d)
        {
            throw new NotImplementedException();
        }

        public void Scan(Document2 d)
        {
             throw new NotImplementedException();
        }

        public void Fax(Document2 d)
        {
             throw new NotImplementedException();
        }
    }


// ================================================
// Classe da impressora antiga (nao multifuncional)
// ================================================

    // ===== Classe =====
    public class OldFashionedPrinter: IMachine2
    {
        // Por que errado.cs e errado?
        //
        // OldFashionedPrinter representa uma impressora antiga.
        // Ela deveria precisar apenas de Print.
        // Mas, por causa da interface IMachine2, ela tambem e obrigada
        // a implementar Scan e Fax.
        //
        // Isso viola Interface Segregation porque a classe passa a depender
        // de metodos que nao fazem parte da sua responsabilidade real.

        // ===== Metodos =====
        public void Print(Document2 d)
        {
            throw new NotImplementedException();
        }

        public void Scan(Document2 d)
        {
             // Quando uma classe precisa jogar NotImplementedException
             // em metodos da interface, isso costuma ser um forte sinal
             // de que a interface esta grande demais para aquele caso.
             throw new NotImplementedException();
        }

        public void Fax(Document2 d)
        {
             // Aqui acontece a mesma coisa:
             // a classe foi forcada a declarar uma capacidade que ela nao tem.
             throw new NotImplementedException();
        }
    }

    // Resumo do que faz este arquivo ser "errado":
    // 1. Existe uma interface unica com responsabilidades demais.
    // 2. Classes simples ficam acopladas a metodos que nao precisam.
    // 3. O codigo passa a ter implementacoes artificiais ou incompletas.
    // 4. Isso aumenta acoplamento e reduz clareza.

}
