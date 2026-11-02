
public class DigitarComando implements Comando {
    private final Editor editor;
    private final String texto;

    public DigitarComando(Editor editor, String texto) {
        this.editor = editor;
        this.texto = texto;
    }

    @Override
    public void executar() {
        editor.digitar(texto);
    }

    @Override
    public void desfazer() {
        editor.apagar(texto.length());
    }
}