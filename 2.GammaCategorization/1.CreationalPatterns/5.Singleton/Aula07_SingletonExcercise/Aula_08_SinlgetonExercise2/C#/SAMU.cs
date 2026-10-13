using System.Collections.Generic;

namespace CentralDeAlertasNamespace
{
    public class SAMU
    {
        public void EnviarAlerta(string mensagem)
        {
            CentralDeAlertas.Instancia.EnviarAlerta("SAMU", mensagem);
        }

        public IReadOnlyCollection<string> ListarAlertas()
        {
            return CentralDeAlertas.Instancia.GetAlertas();
        }
    }
}