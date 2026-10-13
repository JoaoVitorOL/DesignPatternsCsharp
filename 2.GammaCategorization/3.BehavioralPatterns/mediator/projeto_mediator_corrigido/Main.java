package projeto_mediator_corrigido;

public class Main {
    public static void main(String[] args) {
        MediadorChat sala = new SalaChat();

        Usuario alice = new Usuario("Alice", sala);
        Usuario bob = new Usuario("Bob", sala);
        Usuario charlie = new Usuario("Charlie", sala);

        alice.enviar("Vai tomar no cu Bob!", bob, alice);
        bob.enviar("Vai se fuder Alice!", alice, bob);

        bob.sair();
    }
}