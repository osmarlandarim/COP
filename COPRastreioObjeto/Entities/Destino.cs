using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Destino : IDisposable
    {
        private bool disposed = false;

        /// <summary>
        /// Construtor do Destino
        /// </summary>
        public Destino()
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
        /// Iplementação do Dispose
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            //deve-se parar o garbage collector quando chegamos neste ponto
            //caso contrario, loop infinito
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Destrutor da classe Destino
        /// </summary>
        ~Destino()
        {
            //por padrão, devemos chamar o dispose no destrutor.
            Dispose(false);
        }

        /// <summary>
        /// Local do Destino da entrega
        /// </summary>
        public string Local { get; set; }

        /// <summary>
        /// Código interno do Correio do DEstino da entrega
        /// </summary>
        public string Codigo { get; set; }

        /// <summary>
        /// Cidade do Destino (nascional)
        /// </summary>
        public string Cidade { get; set; }

        /// <summary>
        /// Bairro do Destino (nascional)
        /// </summary>
        public string Bairro { get; set; }

        /// <summary>
        /// Estado do Destino (nascional)
        /// </summary>
        public string Uf { get; set; }
    }
}
