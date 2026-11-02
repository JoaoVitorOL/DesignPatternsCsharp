
public class ApagarComando implements Comando {
    private final Editor editor;
    private final int quantidade;
    private String textoRemovido; // Estado guardado para viabilizar o desfazer

    public ApagarComando(Editor editor, int quantidade) {
        this.editor = editor;
        this.quantidade = quantidade;
    }

    @Override
    public void executar() {
        this.textoRemovido = editor.apagar(quantidade);
    }

    @Override
    public void desfazer() {
        if (textoRemovido != null) {
            editor.digitar(textoRemovido);
        }
    }
}