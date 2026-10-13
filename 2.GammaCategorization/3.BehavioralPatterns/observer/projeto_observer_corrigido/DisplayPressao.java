public class DisplayPressao implements ISubscriber {
    @Override 
    public void atualizar(double temperatura, double umidade, double pressao) {
        System.out.println("Pressão atual: " + pressao + " hPa");
    }
}
