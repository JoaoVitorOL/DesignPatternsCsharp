public class DisplayTemperatura implements ISubscriber {

    
    @Override 
    public void atualizar(double temperatura, double umidade, double pressao) {
        System.out.println("Temperatura atual: " + temperatura + "°C");
    }
}
