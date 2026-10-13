package projeto_mediator_corrigido;

import java.util.ArrayList;
import java.util.List;

public class SalaChat implements MediadorChat {
    private final List<Usuario> usuarios = new ArrayList<>();

    @Override
    public void adicionarUsuario(Usuario usuario) {
        if (!usuarios.contains(usuario)) {
            usuarios.add(usuario);
            System.out.println(usuario.getNome() + " ENTROU na sala de chat.");
        }
        
    }

    @Override
    public void enviar(String mensagem, Usuario destinatario, Usuario remetente) {
        for (Usuario usuario : usuarios) {
            if (usuario.equals(destinatario)) {
                usuario.receber(remetente.getNome(), mensagem);
            }
        }
    }

    @Override 
    public void removerUsuario(Usuario usuario){
        System.out.println(usuario.getNome() + " SAIU da sala de chat.");
        usuarios.remove(usuario);
    }
}