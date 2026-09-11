using System;
using System.Collections.Concurrent;
using System.Collections.Generic;


namespace CentralDeAlertasNamespace
{
    public sealed class CentralDeAlertas
    {
        // 1. Instância única garantida com volatile para visibilidade entre threads
        private static CentralDeAlertas _instancia;

        // 2. Construtor privado para impedir a criação de instâncias com 'new'
        private CentralDeAlertas()
        {
            _mensagens = new ConcurrentBag<string>();
        }


        // 3. Método global de acesso à instância única (Singleton Thread-Safe)
        public static CentralDeAlertas Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    lock (_lock)
                    {
                        if (_instancia == null)
                        {
                            _instancia = new CentralDeAlertas();
                        }
                    }
                }
                return _instancia;
            }
        }


        private static readonly object _lock = new object();
        
        // ConcurrentBag é thread-safe para adições e leituras simultâneas
        private readonly ConcurrentBag<string> _mensagens;

        


        public void EnviarAlerta(string orgao, string mensagem)
        {
            string alertaFormatado = $"[{orgao}] {mensagem}";
            _mensagens.Add(alertaFormatado);
        }

        public IReadOnlyCollection<string> GetAlertas()
        {
            // Retorna uma cópia para leitura segura externamente
            return new List<string>(_mensagens);
        }
    }

}