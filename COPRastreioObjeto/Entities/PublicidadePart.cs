using DAO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Entities
{
    public class PublicidadePart : XmlHelper, IDisposable
    {
        private bool disposed = false;

        public string Imagem { get; set; }
        public string Link { get; set; }
        public string Descricao { get; set; }

        public PublicidadePart()
            : base("Publicidade")
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
        ~PublicidadePart()
        {
            //por padrão, chama-se o dispose
            Dispose(false);
        }

        public PublicidadePart ObtemPublicidade()
        {
            return BuscaObjeto("http://coprastreio.comule.com/COPPublicidade/Publicidade.xml", null);
        }

        public PublicidadePart BuscaObjeto(string strCaminho, Stream fileStream)
        {
            try
            {
                //Enumerável dos Elementos de um XML
                IEnumerable<XElement> auxData = StreamXML(strCaminho, "Publicidade", true); //online ou com URL específica
                
                PublicidadePart publicidadeParticular =
                    (from xel in auxData
                     select new PublicidadePart
                     {
                         Imagem = xel.Element("Imagem").Value,
                         Link = xel.Element("Link").Value,
                         Descricao = xel.Element("Descricao").Value
                     }).FirstOrDefault();


                if (publicidadeParticular != null)
                {
                    this.Imagem = publicidadeParticular.Imagem;
                    this.Link = publicidadeParticular.Link;
                    this.Descricao = publicidadeParticular.Descricao;
                }

                return publicidadeParticular;
            }
            catch (Exception ex)
            {
                PublicidadePart publicidade = new PublicidadePart();
                publicidade.Link = string.Empty;
                publicidade.Imagem = string.Empty;

                return publicidade;
            }
        }
    }
}
