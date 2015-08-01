using DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class WebCep : XmlHelper, IDisposable
    {
        private bool disposed = false;

        /// <summary>
        /// Abreviação do nome do estado
        /// </summary>
        public string UF { get; set; }

        /// <summary>
        /// cidade do estado
        /// </summary>
        public string Cidade { get; set; }

        /// <summary>
        /// bairro da cidade
        /// </summary>
        public string Bairro { get; set; }

        /// <summary>
        /// tipo do endereço (rua, avenida, etc)
        /// </summary>
        public string TipoLogradouro { get; set; }

        /// <summary>
        /// nome da rua
        /// </summary>
        public string Logradouro { get; set; }

        /// <summary>
        /// resultado 1, 2 ou null 1 - Cep completo, 2 - único ou 3 - não encontrado
        /// </summary>
        public int Resultado { get; set; }

        /// <summary>
        /// explanação dos códigos do resultado
        /// </summary>
        public string ResultadoTxt { get; set; }

        /// <summary>
        /// Construtor da classe
        /// </summary>
        public WebCep()
            : base("")
        {
            //faz nada
        }

        /// <summary>
        /// Virtual do Dispose
        /// </summary>
        /// <param name="disposing">devemos recolher objetos proprios?</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                    //não ha nada para limpar manualente

                    disposed = true;
            }
        }

        /// <summary>
        /// iplementação do dispose
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            //loop...
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// destrutor da classe
        /// </summary>
        ~WebCep()
        {
            //dispose!
            Dispose(false);
        }

        /// <summary>
        /// busca online o cep informado
        /// </summary>
        /// <param name="CEP">CEP para busca</param>
        /// <returns>retorna um objeto WebCep com os parametros preenchidos</returns>
        private WebCep CarregaEnderecoCep(string CEP)
        {
            try
            {
                this.UF = string.Empty;
                this.Cidade = string.Empty;
                this.Bairro = string.Empty;
                this.TipoLogradouro = string.Empty;
                this.Logradouro = string.Empty;
                this.Resultado = 0;
                this.ResultadoTxt = "CEP não encontrado.";

                //linq to XML
                IEnumerable<WebCep> data =
                    (from xel in StreamXML("http://cep.republicavirtual.com.br/web_cep.php?cep=" + CEP.Replace("-", "").Trim() +
                                    "&formato=xml", "webservicecep", true) //retorna o XML completo para serialização via LINQ
                     select new WebCep
                     {
                         UF = xel.Element("uf").Value,
                         Cidade = xel.Element("cidade").Value,
                         Bairro = xel.Element("bairro").Value,
                         TipoLogradouro = xel.Element("tipo_logradouro").Value,
                         Logradouro = xel.Element("logradouro").Value,
                         Resultado = Convert.ToInt32(xel.Element("resultado").Value),
                         ResultadoTxt = "CEP não Encontrado"
                     });

                //retorna somente 1 resultado
                return data.FirstOrDefault<WebCep>();
            }
            catch
            {
                //erro de comunicação 
                throw new System.Exception("Houve um erro de comunicação com o servidor, tente mais tarde.");
            }
        }

        /// <summary>
        /// método de busca do endereço atravéz do cep via internet
        /// </summary>
        /// <param name="CEP">CEP para pesquisa</param>
        public void GetEnderecoCep(string CEP)
        {
            //obj de retorno temporario
            WebCep aux = new WebCep();

            //procura online o cep
            aux = this.CarregaEnderecoCep(CEP);

            //algo deu errado...
            if (aux == null)
                throw new System.Exception("Houve um erro de comunicação com o servidor, tente mais tarde.");

            //verificamos o resultado da pesquisa
            switch (aux.Resultado)
            {
                case 1:
                    aux.ResultadoTxt = "CEP Completo.";
                    break;
                case 2:
                    aux.ResultadoTxt = "CEP único.";
                    break;
                default:
                    aux.ResultadoTxt = "CEP não encontrado.";
                    break;
            }

            //preenche as propriedades e 'retorna' o objeto
            this.UF = aux.UF;
            this.Cidade = aux.Cidade;
            this.Bairro = aux.Bairro;
            this.TipoLogradouro = aux.TipoLogradouro;
            this.Logradouro = aux.Logradouro;
            this.Resultado = aux.Resultado;
            this.ResultadoTxt = aux.ResultadoTxt;

        }
    }
}
