package br.venson.net.designpatterns.memento;

public class Main {

    public static void main(String[] args) {
        Jogo jogo = new Jogo();
        GerenciadorSave save = new GerenciadorSave();

        jogo.jogar("Fase 1", 3, 100, 10.0, 20.0);
        save.salvar(jogo);
        System.out.println("Apos salvar:      " + jogo);

        jogo.jogar("Fase 2", 1, 450, 80.0, 15.0);
        System.out.println("Depois de jogar:  " + jogo);

        save.restaurar(jogo);
        System.out.println("Apos restaurar:   " + jogo);

        // Tente responder: por que o estado do Jogo esta todo exposto?
        // Se o Jogo ganhar um campo "inimigos", quantos pontos deste projeto quebram?
        // O snapshot em String[] e seguro (tipo, ordem, conversao)?
    }
}
