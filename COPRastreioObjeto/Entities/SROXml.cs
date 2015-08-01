using DAO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class HistoricoSRO : XmlHelper, IDisposable
    {
        private bool disposed = false;

        public SROXml ObjetoSRO { get; set; }

        public HistoricoSRO()
            : base("HistoricoSRO.dat")
        {
            //this.ObjetoSRO = objeto;
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
                    this.ObjetoSRO = null;

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
        /// Destrutor da classe SROXml
        /// </summary>
        ~HistoricoSRO()
        {
            //por padrão, chama-se o dispose
            Dispose(false);
        }

        /// <summary>
        /// Carrega o SROXml cadastrados no disco
        /// </summary>
        /// <returns>retorna sroXml cadastrados com seus objetos.</returns>
        public async Task<HistoricoSRO> GetSHistorico()
        {
            //cria um objeto de endereços
            //todo o metodo de IO deve ser asincrono
            SROXml sroXml = await base.Deserialize<SROXml>();
            HistoricoSRO historicoSrroXml = new HistoricoSRO();

            if (sroXml != null)
                historicoSrroXml.ObjetoSRO = sroXml;

            //retorna os endereços
            return historicoSrroXml;
        }

        /// <summary>
        /// Grava o Resultado off line da busca online em disco
        /// </summary>
        /// <returns>retorna verdadeiro par sucesso, falso para falhas</returns>
        public async Task<bool> GravarCorreioOnLine()
        {
            return await base.Serialize<SROXml>(this.ObjetoSRO);
        }

    }
    public class SROXml : XmlHelper, IDisposable
    {
        private bool disposed = false;

        /// <summary>
        /// Construtor da classe SROXml
        /// Informa ao XmlHelper qual é o XML de saída/entrada
        /// </summary>
        public SROXml()
            : base("ObjetoCorreio.dat")
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
                    this.Objetos = null;

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
        /// Destrutor da classe SROXml
        /// </summary>
        ~SROXml()
        {
            //por padrão, chama-se o dispose
            Dispose(false);
        }

        /// <summary>
        /// Dentro de um SRO existem vários Objetos (pacotes)
        /// </summary>
        public List<Objeto> Objetos { get; set; }
        
        /// <summary>
        /// Versão do SRO do Correios
        /// </summary>
        public string Versao { get; set; }
        
        /// <summary>
        /// Quantidade de itens listados
        /// </summary>
        public int Qtd { get; set; }
        
        /// <summary>
        /// Tipo da pesquisa
        /// </summary>
        public string TipoPesquisa { get; set; }
        
        /// <summary>
        /// Tipo do resultado 
        /// </summary>
        public string TipoResultado { get; set; }
        
        /// <summary>
        /// Ultima atualizacao do sistema via correios.
        /// </summary>
        public DateTime UltimaAtualizacao { get; set; }

        /// <summary>
        /// Carrega o SROXml cadastrados no disco
        /// </summary>
        /// <returns>retorna sroXml cadastrados com seus objetos.</returns>
        public async Task<SROXml> GetSROXml()
        {
            //cria um objeto de endereços
            //todo o metodo de IO deve ser asincrono
            SROXml srroXml = await base.Deserialize<SROXml>();

            //validamos o retorno para não apontar à NULL
            //isso termina a Task paralela e retorna à thread principal
            if (srroXml == null)
                srroXml = new SROXml();

            //retorna os endereços
            return srroXml;
        }

        /// <summary>
        /// Grava o Resultado off line da busca online em disco
        /// </summary>
        /// <returns>retorna verdadeiro par sucesso, falso para falhas</returns>
        public async Task<bool> GravarCorreioOffLine()
        {
            SROXml sroXml = await base.Deserialize<SROXml>();

            if (sroXml == null)
                sroXml = new SROXml();

            if (sroXml.Objetos != null && sroXml.Objetos.Count > 0 && sroXml.Objetos.Count <= 50)
                sroXml.Objetos.AddRange(this.Objetos);
            else
            {
                sroXml.Objetos = new List<Objeto>();
                sroXml.Objetos.AddRange(this.Objetos);
            }

            return await base.Serialize<SROXml>(sroXml);
        }

        /// <summary>
        /// Grava o retorno dos correios com coordenadas e descricao.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> GravarCorreioOnLine()
        {
            return await base.Serialize<SROXml>(this);
        }
    }
}
