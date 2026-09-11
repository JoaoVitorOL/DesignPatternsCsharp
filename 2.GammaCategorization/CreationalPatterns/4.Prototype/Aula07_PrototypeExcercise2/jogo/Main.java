package jogo;

public class Main {
    public static void main(String[] args) {
        RegistroDePrototipos registro = new RegistroDePrototipos();/

        System.out.println("=== 1. OBTENDO INIMIGOS VIA REGISTRO DE PROTÓTIPOS ===");
        Inimigo guerreiroBase = registro.getPrototipo("guerreiro");
        Inimigo magoBase = registro.getPrototipo("mago");

        System.out.println("Original do Registro: " + guerreiroBase);
        System.out.println("Original do Registro: " + magoBase);

        System.out.println("\n=== 2. CRIANDO INIMIGO ELITE A PARTIR DO 'GUERREIRO' ===");
        Inimigo guerreiroElite = registro.getPrototipo("guerreiro");
        guerreiroElite.setTipo("Guerreiro Elite");
        guerreiroElite.setVida(250);
        guerreiroElite.setDano(45);

        System.out.println("Guerreiro Padrão (Não mudou): " + guerreiroBase);
        System.out.println("Guerreiro Elite (Modificado):   " + guerreiroElite);

        System.out.println("\n=== 3. PROVANDO QUE OS CLONES SÃO OBJETOS DISTINTOS ===");
        Inimigo guerreiroClone1 = registro.getPrototipo("guerreiro");
        Inimigo guerreiroClone2 = registro.getPrototipo("guerreiro");

        System.out.println("Hash do Clone 1: " + guerreiroClone1.hashCode());
        System.out.println("Hash do Clone 2: " + guerreiroClone2.hashCode());
        System.out.println("São referências iguais? " + (guerreiroClone1 == guerreiroClone2));

        System.out.println("\n=== 4. DEMONSTRANDO A CÓPIA PROFUNDA (DEEP COPY) ===");
        Inimigo arqueiro1 = registro.getPrototipo("arqueiro");
        Inimigo arqueiro2 = registro.getPrototipo("arqueiro");

        System.out.println("Antes da alteração:");
        System.out.println("Arqueiro 1 Arma: " + arqueiro1.getArma());
        System.out.println("Arqueiro 2 Arma: " + arqueiro2.getArma());

        // Modificando a arma apenas do Arqueiro 1
        arqueiro1.getArma().setNome("Arco Élfico Lendário");
        arqueiro1.getArma().setBonusDano(50);

        System.out.println("\nDepois de modificar a arma do Arqueiro 1:");
        System.out.println("Arqueiro 1 Arma (Modificado): " + arqueiro1.getArma());
        System.out.println("Arqueiro 2 Arma (Permaneceu intacta): " + arqueiro2.getArma());
    }
}