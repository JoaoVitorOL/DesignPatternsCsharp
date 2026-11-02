
import java.util.ArrayList;
import java.util.List;

public class Macro implements Comando {
    private final List<Comando> comandos = new ArrayList<>();
    private final List<Comando> executados = new ArrayList<>();

    public void adicionar(Comando comando) {
        comandos.add(comando);
    }

    @Override
    public void executar() {
        for (Comando comando : comandos) {
            comando.executar();
            executados.add(comando); // Guarda no histórico de execução
        }
    }

    @Override
    public void desfazer() {
        // Desfaz na ordem inversa (LIFO - Last In, First Out) dos comandos executados
        while (!executados.isEmpty()) {
            Comando comando = executados.remove(executados.size() - 1);
            comando.desfazer();
        }
    }
}