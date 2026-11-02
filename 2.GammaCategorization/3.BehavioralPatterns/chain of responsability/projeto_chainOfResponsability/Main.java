
public class Main {
    public static void main(String[] args) {
        // 1. Montagem da Corrente (Pipeline de Validação)
        // Status -> Mana -> Cooldown -> Alcance
        ValidadorMagia cadeiaValidacao = new ValidadorStatus();
        cadeiaValidacao
            .proximo(new ValidadorMana())
            .proximo(new ValidadorCooldown())
            .proximo(new ValidadorAlcance());

        // Cenário 1: Jogador apto a lançar a magia
        System.out.println("--- Cenário 1: Jogador com recursos e condições normais ---");
        Jogador mago = new Jogador("Gandalf", true, false, 100);
        ContextoMagia contexto1 = new ContextoMagia(mago, "Bola de Fogo", 40, 0.0, 10.0);
        
        boolean sucesso1 = cadeiaValidacao.validar(contexto1);
        if (sucesso1) {
            mago.gastarMana(contexto1.getCustoMana());
            System.out.println("✨ Sucesso! Magia '" + contexto1.getNomeMagia() + "' conjurada com sucesso! Mana restante: " + mago.getManaAtual());
        }

        System.out.println("\n--- Cenário 2: Jogador sem mana suficiente ---");
        ContextoMagia contexto2 = new ContextoMagia(mago, "Meteorito", 200, 0.0, 5.0);
        cadeiaValidacao.validar(contexto2);

        System.out.println("\n--- Cenário 3: Jogador atordoado (Stun) ---");
        Jogador guerreiroAtordoado = new Jogador("Arthas", true, true, 100);
        ContextoMagia contexto3 = new ContextoMagia(guerreiroAtordoado, "Golpe Furtivo", 10, 0.0, 2.0);
        cadeiaValidacao.validar(contexto3);
    }
}