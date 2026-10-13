package br.venson.net.designpatterns.mediator;

public class Main {

    public static void main(String[] args) {
        Usuario ana = new Usuario("Ana");
        Usuario bruno = new Usuario("Bruno");
        Usuario carla = new Usuario("Carla");
        Usuario diego = new Usuario("Diego");

        // Cada usuario precisa conhecer todos os outros, um a um.
        ana.adicionarContato(bruno);
        ana.adicionarContato(carla);
        ana.adicionarContato(diego);
        bruno.adicionarContato(ana);
        bruno.adicionarContato(carla);
        bruno.adicionarContato(diego);
        carla.adicionarContato(ana);
        carla.adicionarContato(bruno);
        carla.adicionarContato(diego);
        diego.adicionarContato(ana);
        diego.adicionarContato(bruno);
        diego.adicionarContato(carla);

        ana.enviar("Bom dia, pessoal!");

        // Tente responder: quantas ligacoes novas seriam criadas para incluir o
        // Diego na sala? E para remover a Carla de todas as listas de contatos?
    }
}
