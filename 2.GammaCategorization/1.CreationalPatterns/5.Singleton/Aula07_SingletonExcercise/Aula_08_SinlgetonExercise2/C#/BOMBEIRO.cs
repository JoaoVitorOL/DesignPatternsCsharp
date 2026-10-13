using System.Collections.Generic;

namespace CentralDeAlertasNamespace
{
    public class BOMBEIRO
    {
        public void EnviarAlerta(string mensagem)
        {
            CentralDeAlertas.Instancia.EnviarAlerta("BOMBEIRO", mensagem);
        }

        public IReadOnlyCollection<string> ListarAlertas()
        {
            return CentralDeAlertas.Instancia.GetAlertas();
        }
    }
}