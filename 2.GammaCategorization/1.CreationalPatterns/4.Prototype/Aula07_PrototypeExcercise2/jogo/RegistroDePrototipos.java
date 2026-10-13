package jogo;

import java.util.HashMap;
import java.util.Map;

public class RegistroDePrototipos {
    private final Map<String, InimigoPrototype> prototipos = new HashMap<>();

    public RegistroDePrototipos() {
        carregarPrototiposPadrao();
    }

    private void carregarPrototiposPadrao() {
        prototipos.put("guerreiro", new Inimigo("Guerreiro", 100, 20, new Arma("Espada Longa", 10)));
        prototipos.put("mago", new Inimigo("Mago", 60, 35, new Arma("Cajado Mágico", 25)));
        prototipos.put("arqueiro", new Inimigo("Arqueiro", 80, 25, new Arma("Arco Composto", 15)));
        prototipos.put("chefe", new Inimigo("Chefe", 500, 60, new Arma("Machado Gigante", 40)));
    }

    // Retorna sempre um clone do protótipo solicitado
    public Inimigo getPrototipo(String nome) {
        InimigoPrototype prototipo = prototipos.get(nome.toLowerCase());
        if (prototipo == null) {
            throw new IllegalArgumentException("Protótipo não encontrado: " + nome);
        }
        return (Inimigo) prototipo.clonar();
    }
}