package lanchonete;

import java.util.ArrayList;
import java.util.Collections;
import java.util.List;

public class Lanche {
    // Campos obrigatórios (somente leitura - objeto imutável)
    private final String pao;
    private final String proteina;

    // Campos opcionais
    private final String queijo;
    private final List<String> vegetais;
    private final String molho;
    private final boolean bemPassado;
    private final String observacoes;

    // 2. Construtor privado: apenas o Builder consegue instanciar o Lanche
    private Lanche(Builder builder) {
        this.pao = builder.pao;
        this.proteina = builder.proteina;
        this.queijo = builder.queijo;
        this.vegetais = Collections.unmodifiableList(new ArrayList<>(builder.vegetais));
        this.molho = builder.molho;
        this.bemPassado = builder.bemPassado;
        this.observacoes = builder.observacoes;
    }

    // Métodos Get para leitura dos campos
    public String getPao() { return pao; }
    public String getProteina() { return proteina; }
    public String getQueijo() { return queijo; }
    public List<String> getVegetais() { return vegetais; }
    public String getMolho() { return molho; }
    public boolean isBemPassado() { return bemPassado; }
    public String getObservacoes() { return observacoes; }

    @Override
    public String toString() {
        String vegetaisStr = vegetais.isEmpty() ? "Nenhum" : String.join(", ", vegetais);
        return "Lanche [Pão: " + pao + ", Proteína: " + proteina + ", Queijo: " + (queijo != null ? queijo : "Não") + 
               ", Vegetais: [" + vegetaisStr + "], Molho: " + (molho != null ? molho : "Nenhum") + 
               ", Bem Passado: " + (bemPassado ? "Sim" : "Não") + ", Observações: " + (observacoes != null ? observacoes : "Nenhuma") + "]";
    }

    // --- CLASSE BUILDER ---
    public static class Builder {
        // 1. Atributos espelho do Builder para receber os valores passo a passo
        private String pao;
        private String proteina;
        private String queijo;
        private List<String> vegetais = new ArrayList<>();
        private String molho;
        private boolean bemPassado;
        private String observacoes;

        // 3. Métodos de configuração fluentes (retornam 'this' para encadeamento) this -> o próprio builder
        public Builder setPao(String pao) {
            this.pao = pao;
            return this; 
        }

        public Builder setProteina(String proteina) {
            this.proteina = proteina;
            return this;
        }

        public Builder setQueijo(String queijo) {
            this.queijo = queijo;
            return this;
        }

        public Builder addVegetal(String vegetal) {
            this.vegetais.add(vegetal);
            return this;
        }

        public Builder setMolho(String molho) {
            this.molho = molho;
            return this;
        }

        public Builder setBemPassado(boolean bemPassado) {
            this.bemPassado = bemPassado;
            return this;
        }

        public Builder setObservacoes(String observacoes) {
            this.observacoes = observacoes;
            return this;
        }

        // 4. Método build responsável por validar as regras e instanciar o produto final
        public Lanche build() {
            if (pao == null || pao.trim().isEmpty()) {
                throw new IllegalStateException("Erro de validação: O campo obrigatório 'pao' não foi informado.");
            }
            if (proteina == null || proteina.trim().isEmpty()) {
                throw new IllegalStateException("Erro de validação: O campo obrigatório 'proteina' não foi informado.");
            }

            return new Lanche(this);
        }
    }
}