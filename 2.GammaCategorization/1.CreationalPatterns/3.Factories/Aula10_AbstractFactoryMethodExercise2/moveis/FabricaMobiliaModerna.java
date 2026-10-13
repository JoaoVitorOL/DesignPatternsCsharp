package moveis;

public class FabricaMobiliaModerna implements FabricaMobilia {
    @Override
    public Cadeira criarCadeira() { return new CadeiraModerna(); }

    @Override
    public Sofa criarSofa() { return new SofaModerno(); }

    @Override
    public MesaDeCentro criarMesaDeCentro() { return new MesaDeCentroModerna(); }
}