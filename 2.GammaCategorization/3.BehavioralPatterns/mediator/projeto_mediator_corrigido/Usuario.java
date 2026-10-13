
public class Usuario {
    private final String nome;
    private final MediadorChat mediador;

    public Usuario(String nome, MediadorChat mediador) {
        this.nome = nome;
        this.mediador = mediador;
        this.mediador.adicionarUsuario(this);
    }

    public String getNome() {
        return nome;
    }

    public void enviar(String mensagem, Usuario destinatario, Usuario remetente) {
        mediador.enviar(mensagem, destinatario, this);
    }

    public void receber(String remetente, String mensagem) {
        System.out.println(nome + " recebeu de " + remetente + ": " + mensagem);
    }


    public void sair() {
        mediador.removerUsuario(this);
    }
}