
public class Jogo {

    private String fase;
    private int vidas;
    private int pontuacao;
    private double x;
    private double y;

    public void jogar(String fase, int vidas, int pontuacao, double x, double y) {
        this.fase = fase;
        this.vidas = vidas;
        this.pontuacao = pontuacao;
        this.x = x;
        this.y = y;
    }

    // Cria o Memento (captura o estado atual de forma imutável)
    public Checkpoint salvar() {
        return new CheckpointImpl(fase, vidas, pontuacao, x, y);
    }

    // Restaura o estado a partir do Memento
    public void restaurar(Checkpoint checkpoint) {
        if (checkpoint instanceof CheckpointImpl) {
            CheckpointImpl c = (CheckpointImpl) checkpoint;
            this.fase = c.fase;
            this.vidas = c.vidas;
            this.pontuacao = c.pontuacao;
            this.x = c.x;
            this.y = c.y;
        }
    }

    @Override
    public String toString() {
        return "Jogo{fase=" + fase + ", vidas=" + vidas + ", pontuacao=" + pontuacao
                + ", x=" + x + ", y=" + y + "}";
    }

    // ==========================================
    // DEFINIÇÃO DO MEMENTO E DAS INTERFACES
    // ==========================================

    /**
     * Interface Estreita (Narrow Interface):
     * Exposta ao Caretaker (GerenciadorSave). O Caretaker vê apenas um tipo opaco, 
     * sem acesso a nenhum campo ou método interno do jogo.
     */
    public interface Checkpoint {
        // Pode ser uma interface de marcação vazia
    }

    /**
     * Interface Ampla (Wide Interface):
     * Visível apenas para o Originator (Jogo), permitindo acessar os dados 
     * encapsulados para realizar a restauração com segurança de tipos.
     */
    private static class CheckpointImpl implements Checkpoint {
        private final String fase;
        private final int vidas;
        private final int pontuacao;
        private final double x;
        private final double y;

        public CheckpointImpl(String fase, int vidas, int pontuacao, double x, double y) {
            this.fase = fase;
            this.vidas = vidas;
            this.pontuacao = pontuacao;
            this.x = x;
            this.y = y;
        }
    }
}