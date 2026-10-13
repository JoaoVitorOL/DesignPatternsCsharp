package restaurante;

import java.util.ArrayList;
import java.util.List;

public class Combo implements ItemMenu {
    private final String nome;
    private final List<ItemMenu> itens = new ArrayList<>();

    public Combo(String nome) {
        this.nome = nome;
    }

    public void adicionar(ItemMenu item) {
        itens.add(item);
    }

    public void remover(ItemMenu item) {
        itens.remove(item);
    }

    @Override
    public String getNome() {
        return nome;
    }

    @Override
    public double getPreco() {
        // Soma recursiva do preço de todos os filhos (seja prato ou outro combo)
        double total = 0.0;
        for (ItemMenu item : itens) {
            total += item.getPreco();
        }
        return total;
    }
}