
public class Main {

    public static void main(String[] args) {
        Editor editor = new Editor();
        Macro macro = new Macro();

        // Adicionando comandos fortemente tipados
        macro.adicionar(new DigitarComando(editor, "Ola "));
        macro.adicionar(new DigitarComando(editor, "mundo"));
        macro.adicionar(new ApagarComando(editor, 3));

        macro.executar();
        System.out.println("Texto apos executar: \"" + editor.getTexto() + "\"");

        macro.desfazer();
        System.out.println("Texto apos desfazer: \"" + editor.getTexto() + "\"");
    }
}