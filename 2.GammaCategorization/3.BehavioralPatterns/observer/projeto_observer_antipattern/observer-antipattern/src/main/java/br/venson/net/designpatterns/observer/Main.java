package br.venson.net.designpatterns.observer;

public class Main {

    public static void main(String[] args) {
        EstacaoMeteorologica estacao = new EstacaoMeteorologica();

        estacao.setMedicoes(25.0, 60.0, 1013.0);
        estacao.setMedicoes(27.0, 55.0, 1010.0);

        // Tente responder: quantas classes precisam ser editadas para adicionar
        // um DisplayUmidade? Como registrar/remover displays em tempo de execucao?
        // A estacao poderia ser reusada em outro painel sem recompilar?
    }
}
