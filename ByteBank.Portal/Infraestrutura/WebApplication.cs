using ByteBank.Portal.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ByteBank.Portal.Infraestrutura
{
    public class WebApplication
    {
        private readonly string[] _prefixos;

        public WebApplication(string[] prefixos)
        {
            _prefixos = prefixos ?? throw new ArgumentNullException(nameof(prefixos));
        }

        public void Iniciar()
        {
            Console.Write("Aplicação Web Rodando com Reflection");

            while (true)
                ManipularRequisicao();
        }

        private void ManipularRequisicao()
        {
            var httpListener = new HttpListener();

            foreach (var prefixo in _prefixos)
            {
                httpListener.Prefixes.Add(prefixo);
            }

            httpListener.Start();

            var contexto = httpListener.GetContext();
            var requisicao = contexto.Request;
            var resposta = contexto.Response;


            var path = requisicao.Url.AbsolutePath;

            if (Utilidades.EhArquivo(path))
            {
                var Manipulador = new ManipuladorRequisicaoArquivo();
                Manipulador.Manipular(resposta, path);
            }
            else
            {
                var manipulador = new ManipuladorRequisicaoController();
                manipulador.Manipular(resposta, path);
            }

            httpListener.Stop();
        }
    }
}
