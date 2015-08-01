using DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Preferencias : XmlHelper, IDisposable
    {
        private bool disposed = false;
        
        public Preferencias()
            : base("Preferencias.dat")
        {            
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
                    //this.ObjetoSRO = null;

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
        ~Preferencias()
        {
            //por padrão, chama-se o dispose
            Dispose(false);
        }

        /// <summary>
        /// Marca a posicao da propaganda.
        /// </summary>
        public Windows.UI.Xaml.HorizontalAlignment PosicaoPublicidade { get; set; }
        public bool AtualizacaoAutomatica { get; set; }
        public int TempoAtualizacao { get; set; }
        /// <summary>
        /// Campo para saber se o objeto ja foi criado ou nao, no default fica true. Caso false chamar a criacao padrão. Nao mexer.
        /// </summary>
        public bool Criado { get; set; }

        /// <summary>
        /// Carrega o Preferencias cadastrados no disco
        /// </summary>
        /// <returns>retorna sroXml cadastrados com seus objetos.</returns>
        public async Task<Preferencias> GetPreferencias()
        {
            //cria um objeto de endereços
            //todo o metodo de IO deve ser asincrono
            Preferencias preferencias = await base.Deserialize<Preferencias>();

            //validamos o retorno para não apontar à NULL
            //isso termina a Task paralela e retorna à thread principal
            if (preferencias == null)
                preferencias = new Preferencias();

            //retorna os endereços
            return preferencias;
        }

        /// <summary>
        /// Grava o retorno das prefencias.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> GravarPreferencias()
        {
            return await base.Serialize<Preferencias>(this);
        }
    }
}
