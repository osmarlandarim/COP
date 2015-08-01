using DAO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Entities
{
    public class Correio : XmlHelper, IDisposable
    {
        private bool disposed = false;

        /// <summary>
        /// Construtor do correio
        /// </summary>
        public Correio()
            : base("")
        {
            //faz nada
        }

        /// <summary>
        /// Override da interface Dispose
        /// </summary>
        /// <param name="disposing">identifica se já estamos recolhendo o objeto</param>
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
        /// Implementar o IDispose
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            //deve-se parar o garbage collector quando chegamos neste ponto.
            //caso contrario, loop infinito.
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Destrutor da classe dos Correios
        /// </summary>
        ~Correio()
        {
            //por padrão, chama-se o dispose
            Dispose(false);
        }

        /// <summary>
        /// Busca o Objeto Online.
        /// </summary>
        /// <param name="codigosRastreio">Lista de Códigos de Rastreio.</param>
        /// <param name="resultado">T-> Serão retornados todos os Eventos, U -> Sera retornado apenas o ultimo Evento.</param>
        /// <returns>retorna o objeto SRO do Correio</returns>
        public SROXml BuscaObjetosOnline(List<string> codigosRastreio, Enumeration.Resultado resultado)
        {
            //para desenv ECT - SRO
            string usuario = "INVDIAGECT";
            string senha = "J97ENBI?26";
            string strCodigosRastreio = string.Empty;
            //LINQS!!!
            //toda lista implementa Aggregate (aglutine/some)
            //funciona assim:
            //  para cada x e y na lista, faca (=> - operador lambda)
            //  x + y
            //ou seja, pegue a primeira string (a) e a segunda string (b) e faça (a + b) = ab
            if (codigosRastreio.Count > 0)
                strCodigosRastreio = codigosRastreio.Aggregate((x, y) => x + y);

            //busca o objeto online
            //stream deve ser NULO
            return BuscaObjeto("http://websro.correios.com.br/sro_bin/sroii_xml.eventos?Usuario=" + usuario + "&Senha=" + senha + "&Tipo=&Resultado=" + resultado + "&Objetos=" + strCodigosRastreio,
                                null);
        }

        public SROXml BuscaObjetosOnline(string codigosRastreio, Enumeration.Resultado resultado)
        {
            //para desenv ECT - SRO
            string usuario = "INVDIAGECT";
            string senha = "J97ENBI?26";
            return BuscaObjeto("http://websro.correios.com.br/sro_bin/sroii_xml.eventos?Usuario=" + usuario + "&Senha=" + senha + "&Tipo=&Resultado=" + resultado + "&Objetos=" + codigosRastreio,
                                null);
        }
        /// <summary>
        /// Busca, em disco, o resultado mais recente da ultima busca online pelo codigo informado
        /// </summary>
        /// <param name="codigoCorreio">obrigatório. Código de rastreio do objeto</param>
        /// <returns>retorna o SRO completo do objeto cujo código é igual</returns>
        public async Task<SROXml> BuscaObjetoOffline(List<string> codigoRastreio)
        {
            //como semplre, todo o IO deve ser asincrono
            var stream = await base.GetStream();

            //busca o ojeto
            return BuscaObjeto(string.Empty, stream);
        }

        /// <summary>
        /// Busca o Objeto
        /// Caso ocorra erros, os campos Versão, TipoResultado e TipoPesquisa
        /// listaram os erros
        /// </summary>
        /// <param name="strCaminho">URL local ou Internet</param>
        /// <param name="codigosRastreio">Lista dos Códigos de Rastreio</param>
        /// <returns>retorna o objeto SRO do Correio</returns>
        public SROXml BuscaObjeto(string strCaminho, Stream fileStream)
        {
            try
            {
                //Enumerável dos Elementos de um XML
                IEnumerable<XElement> auxData = null;

                if (fileStream == null)
                    auxData = StreamXML(strCaminho, "sroxml", true); //online ou com URL específica
                else
                    auxData = StreamXML(fileStream, "sroxml", true); //com arquivo específico

                //Linq to XML
                IEnumerable<SROXml> data =
                        (from xel in auxData
                         select new SROXml
                         {
                             Versao = xel.Element("versao").Value,
                             Qtd = Convert.ToInt32(xel.Element("qtd").Value),
                             TipoPesquisa = xel.Element("TipoPesquisa").Value,
                             TipoResultado = xel.Element("TipoResultado").Value,
                             Objetos = (from xelObjeto in xel.Descendants("objeto")
                                        select new Objeto
                                        {
                                            Numero = xelObjeto.Element("numero").Value,
                                            Eventos = (from xelEvento in xelObjeto.Descendants("evento")
                                                       select new Evento
                                                       {
                                                           Tipo = xelEvento.Element("tipo").Value,
                                                           Status = xelEvento.Element("status").Value,
                                                           Data = xelEvento.Element("data").Value,
                                                           Hora = xelEvento.Element("hora").Value,
                                                           Descricao = xelEvento.Element("descricao").Value,
                                                           Local = xelEvento.Element("local").Value,
                                                           Codigo = xelEvento.Element("codigo").Value,
                                                           Cidade = xelEvento.Element("cidade").Value,
                                                           UF = xelEvento.Element("uf").Value,
                                                           Sto = xelEvento.Element("sto").Value,
                                                           Destino = (from xelDestino in xelEvento.Descendants("destino")
                                                                      select new Destino
                                                                      {
                                                                          Local = xelDestino.Element("local").Value,
                                                                          Codigo = xelDestino.Element("codigo").Value,
                                                                          Cidade = xelDestino.Element("cidade").Value,
                                                                          Bairro = xelDestino.Element("bairro").Value,
                                                                          Uf = xelDestino.Element("uf").Value
                                                                      }).FirstOrDefault()
                                                       }).ToList<Evento>()
                                        }).ToList<Objeto>()
                         });

                //objeto SRO completo
                //resultado deve ser somente um objeto.
                return data.FirstOrDefault();
            }
            catch (Exception ex)
            {
                SROXml aux = new SROXml();
                aux.Versao = "0";
                aux.TipoResultado = ex.Message;
                aux.TipoPesquisa = "Falha na comunicação com a Internet";

                return aux;
            }
        }
    }
}
