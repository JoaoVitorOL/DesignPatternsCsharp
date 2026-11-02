
public class ValidadorStatus implements ValidadorMagia {
    private ValidadorMagia proximoValidador;

    @Override
    public ValidadorMagia proximo(ValidadorMagia proximo) {
        this.proximoValidador = proximo;
        return proximo;
    }

    @Override
    public boolean validar(ContextoMagia contexto) {
        Jogador j = contexto.getJogador();
        if (!j.isVivo()) {
            System.out.println("❌ Falha: " + j.getNome() + " está morto e não pode lançar magias.");
            return false;
        }
        if (j.isAtordoado()) {
            System.out.println("❌ Falha: " + j.getNome() + " está atordoado (Stun)!");
            return false;
        }
        
        // Passa para o próximo elo da corrente, se existir
        if (proximoValidador != null) {
            return proximoValidador.validar(contexto);
        }
        return true;
    }
}