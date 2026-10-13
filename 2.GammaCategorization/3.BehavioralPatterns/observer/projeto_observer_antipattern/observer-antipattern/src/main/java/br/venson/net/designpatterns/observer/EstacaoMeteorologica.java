package br.venson.net.designpatterns.observer;

public class EstacaoMeteorologica {

    private double temperatura;
    private double umidade;
    private double pressao;

    private final DisplayTemperatura displayTemperatura = new DisplayTemperatura();
    private final DisplayEstatisticas displayEstatisticas = new DisplayEstatisticas();

    public void setMedicoes(double temperatura, double umidade, double pressao) {
        this.temperatura = temperatura;
        this.umidade = umidade;
        this.pressao = pressao;

        // A estacao conhece cada display e o atualiza diretamente.
        displayTemperatura.atualizar(temperatura);
        displayEstatisticas.atualizar(temperatura);
    }

    public double getTemperatura() {
        return temperatura;
    }

    public double getUmidade() {
        return umidade;
    }

    public double getPressao() {
        return pressao;
    }
}
