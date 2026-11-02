
import java.util.ArrayList;
import java.util.List;

public class GerenciadorSave {

    // Guarda referências à interface estreita (Checkpoint), mantendo a opacidade
    private final List<Jogo.Checkpoint> checkpoints = new ArrayList<>();

    public void salvar(Jogo jogo) {
        checkpoints.add(jogo.salvar());
    }

    public void restaurar(Jogo jogo) {
        if (checkpoints.isEmpty()) {
            System.out.println("Nenhum checkpoint salvo.");
            return;
        }
        Jogo.Checkpoint checkpoint = checkpoints.remove(checkpoints.size() - 1);
        jogo.restaurar(checkpoint);
    }
}