package br.venson.net.designpatterns.memento;

import java.util.ArrayList;
import java.util.List;

public class GerenciadorSave {

    // O "snapshot" e um vetor de Strings sem tipo: conhece a ordem e o formato
    // de cada campo do Jogo. Qualquer campo novo quebra este codigo.
    private final List<String[]> checkpoints = new ArrayList<>();

    public void salvar(Jogo jogo) {
        checkpoints.add(new String[] {
                jogo.getFase(),
                String.valueOf(jogo.getVidas()),
                String.valueOf(jogo.getPontuacao()),
                String.valueOf(jogo.getX()),
                String.valueOf(jogo.getY())
        });
    }

    public void restaurar(Jogo jogo) {
        if (checkpoints.isEmpty()) {
            System.out.println("Nenhum checkpoint salvo.");
            return;
        }
        String[] c = checkpoints.remove(checkpoints.size() - 1);
        jogo.setFase(c[0]);
        jogo.setVidas(Integer.parseInt(c[1]));
        jogo.setPontuacao(Integer.parseInt(c[2]));
        jogo.setX(Double.parseDouble(c[3]));
        jogo.setY(Double.parseDouble(c[4]));
    }
}
