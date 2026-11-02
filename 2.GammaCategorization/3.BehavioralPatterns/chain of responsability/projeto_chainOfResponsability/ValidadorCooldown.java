
public class ValidadorCooldown implements ValidadorMagia {
    private ValidadorMagia proximoValidador;

    @Override
    public ValidadorMagia proximo(ValidadorMagia proximo) {
        this.proximoValidador = proximo;
        return proximo;
    }

    @Override
    public boolean validar(ContextoMagia contexto) {
        if (contexto.getCooldownRestante() > 0) {
            System.out.println("❌ Falha: A magia '" + contexto.getNomeMagia() + "' está em cooldown (" + contexto.getCooldownRestante() + "s restantes).");
            return false;
        }

        if (proximoValidador != null) {
            return proximoValidador.validar(contexto);
        }
        return true;
    }
}