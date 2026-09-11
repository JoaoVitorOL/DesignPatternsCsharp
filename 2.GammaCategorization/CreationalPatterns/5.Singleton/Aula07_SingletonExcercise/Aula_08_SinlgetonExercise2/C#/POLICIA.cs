using System.Collections.Generic;

namespace CentralDeAlertasNamespace
{
    public class POLICIA
    {
        public void EnviarAlerta(string mensagem)
        {
            CentralDeAlertas.Instancia.EnviarAlerta("POLÍCIA", mensagem);
        }

        public IReadOnlyCollection<string> ListarAlertas()
        {
            return CentralDeAlertas.Instancia.GetAlertas();
        }
    }
}