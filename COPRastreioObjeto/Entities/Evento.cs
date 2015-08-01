using DAO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Entities
{
    public class Evento : IDisposable
    {
        private bool disposed = false;

        /// <summary>
        /// Construtor da classe Evento
        /// </summary>
        public Evento()
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
                {
                    this.Destino.Dispose();
                    this.Destino = null;
                }

                disposed = true;
            }
        }

        /// <summary>
        /// implementação do IDispose
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            //deve-se parar o garbage collector quando chegamos neste ponto.
            //caso contrario, loop infinito.
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Destrutor da classe Evento
        /// </summary>
        ~Evento()
        {
            //por padrão, devemos chamar o dispose no destrutor.
            Dispose(false);
        }

        /// <summary>
        /// Objeto Destino. Guarda as propriedades do destino do pacote
        /// </summary>
        public Destino Destino { get; set; }
        
        /// <summary>
        /// Hora do Evento
        /// </summary>
        public string Hora { get; set; }
        
        /// <summary>
        /// Data do Evento
        /// </summary>
        public string Data { get; set; }
        
        /// <summary>
        /// Tipo do Evento
        /// </summary>
        public string Tipo { get; set; }
        
        /// <summary>
        /// Status do Evento
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Descrição do Evento
        /// </summary>
        public string Descricao { get; set; }
        
        /// <summary>
        /// Local do Evento
        /// </summary>
        public string Local { get; set; }
        
        /// <summary>
        /// Código do Evento
        /// </summary>
        public string Codigo { get; set; }
        

        /// <summary>
        /// Cidade do Evento
        /// </summary>
        public string Cidade { get; set; }
        
        /// <summary>
        /// Estado (UF) do Evento
        /// </summary>
        public string UF { get; set; }
        
        /// <summary>
        /// Codigo interno do Evento
        /// </summary>
        public string Sto { get; set; }
        
        /// <summary>
        /// Coordenadas X do Local Endereco objeto.
        /// </summary>
        public double GeoX { get; set; }

        /// <summary>
        /// Coordenadas Y do Local Endereco objeto.
        /// </summary>
        public double GeoY { get; set; }

        /// <summary>
        /// Montar todas as regras que podem vir dos correios.
        /// </summary>
        /// <returns></returns>
        public string DestinoEvento()
        {
            string destino = string.Empty;

            if (this.Tipo == "RO" && (this.Status == "01" || this.Status == "99"))
            {
                destino = "Em trânsito para ";
            }
            else
            {
                if (this.Tipo == "DO" && this.Status == "01")
                {
                    destino = "Encaminhado para ";
                }
                else
                    destino = string.Empty;
            }

            return destino;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool BuscaGeoLocalizacaoEvento()
        {
            string endereco = string.Empty;
            string localCorreios = string.Empty;

            if (this.Local.Contains("AC "))
            {
                localCorreios = this.Local.Replace("AC", "AC CORREIOS");
            }

            if (this.Local.Contains("CDD "))
            {
                localCorreios = this.Local.Replace(this.Cidade, "");
                localCorreios = localCorreios.Replace("()", "");
                localCorreios = localCorreios.Replace("CDD", "CDD CORREIOS");                
            }

            if (localCorreios == string.Empty)
                localCorreios = this.Local;


            if (endereco != string.Empty && localCorreios != string.Empty)
                endereco = endereco + "," + localCorreios;
            else if (localCorreios != string.Empty)
                endereco = endereco + localCorreios;

            if (this.Cidade != string.Empty)
            {
                endereco = endereco + " " + this.Cidade;
            }

            if (endereco != string.Empty && this.UF != string.Empty)
                endereco = endereco + " " + this.UF;
            else if (this.UF != string.Empty)
                endereco = endereco + this.UF;
            
            return BuscaGeoLocalizacaoEvento("http://maps.googleapis.com/maps/api/geocode/xml?address=" + endereco + "&sensor=false",
                                            null);
        }

        /// <summary>
        /// Busca o Objeto
        /// </summary>
        /// <param name="strCaminho">URL local ou Internet</param>
        /// <param name="codigosRastreio">Lista dos Códigos de Rastreio</param>
        /// <returns>retorna o objeto SRO do Correio</returns>
        private bool BuscaGeoLocalizacaoEvento(string strCaminho, Stream fileStream)
        {
            try
            {
                //Enumerável dos Elementos de um XML
                IEnumerable<XElement> auxData = XmlHelper.StreamXML(strCaminho, "location", false); //online ou com URL específica                
                //Linq to XML
                Evento geoLocation =
                        (from xel in auxData
                         select new Evento
                         {
                             GeoX = Convert.ToDouble(xel.Element("lat").Value, new CultureInfo("en-US")),
                             GeoY = Convert.ToDouble(xel.Element("lng").Value, new CultureInfo("en-US")),
                         }).FirstOrDefault<Evento>();

                if (geoLocation != null)
                {
                    this.GeoX = geoLocation.GeoX;
                    this.GeoY = geoLocation.GeoY;
                }
            }
            catch (Exception ex)
            {
                
            }

            return true;
        }
    }
}
