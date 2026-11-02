
public class ValidadorMana implements ValidadorMagia {
    private ValidadorMagia proximoValidador;

    @Override
    public ValidadorMagia proximo(ValidadorMagia proximo) {
        this.proximoValidador = proximo;
        return proximo;
    }

    @Override
    public boolean validar(ContextoMagia contexto) {
        Jogador j = contexto.getJogador();
        if (j.getManaAtual() < contexto.getCustoMana()) {
            System.out.println("❌ Falha: Mana insuficiente! Necessário: " + contexto.getCustoMana() + ", Atual: " + j.getManaAtual());
            return false;
        }

        if (proximoValidador != null) {
            return proximoValidador.validar(contexto);
        }
        return true;
    }
}