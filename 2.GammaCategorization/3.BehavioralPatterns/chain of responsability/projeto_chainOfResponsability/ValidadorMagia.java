
public interface ValidadorMagia {
    ValidadorMagia proximo(ValidadorMagia proximo);
    boolean validar(ContextoMagia contexto);
}