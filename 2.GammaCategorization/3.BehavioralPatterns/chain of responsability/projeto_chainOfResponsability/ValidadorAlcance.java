
public class ValidadorAlcance implements ValidadorMagia {
    private ValidadorMagia proximoValidador;
    private final double alcanceMaximo = 15.0; // Exemplo de range da magia

    @Override
    public ValidadorMagia proximo(ValidadorMagia proximo) {
        this.proximoValidador = proximo;
        return proximo;
    }

    @Override
    public boolean validar(ContextoMagia contexto) {
        if (contexto.getDistanciaAlvo() > alcanceMaximo) {
            System.out.println("❌ Falha: O alvo está fora de alcance! Distância: " + contexto.getDistanciaAlvo() + "m (Máx: " + alcanceMaximo + "m)");
            return false;
        }

        if (proximoValidador != null) {
            return proximoValidador.validar(contexto);
        }
        return true;
    }
}