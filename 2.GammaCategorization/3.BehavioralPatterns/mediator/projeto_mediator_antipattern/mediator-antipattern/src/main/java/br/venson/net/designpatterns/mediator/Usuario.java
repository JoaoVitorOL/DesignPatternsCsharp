package br.venson.net.designpatterns.mediator;

import java.util.ArrayList;
import java.util.List;

public class Usuario {
    private final String nome;
    private final List<Usuario> contatos = new ArrayList<>();

    public Usuario(String nome) {
        this.nome = nome;
    }

    public String getNome() {
        return nome;
    }

    public void adicionarContato(Usuario usuario) {
        if (usuario != this && !contatos.contains(usuario)) {
            contatos.add(usuario);
        }
    }

    public void removerContato(Usuario usuario) {
        contatos.remove(usuario);
    }

    public void enviar(String mensagem) {
        for (Usuario contato : contatos) {
            contato.receber(nome, mensagem);
        }
    }

    public void receber(String remetente, String mensagem) {
        System.out.println(nome + " recebeu de " + remetente + ": " + mensagem);
    }
}
