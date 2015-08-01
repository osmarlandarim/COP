using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class Objeto : IDisposable
    {
        private bool disposed = false;

        /// <summary>
        /// Construtor da classe Objeto
        /// </summary>
        public Objeto()
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
                    this.Eventos = null;

                disposed = true;
            }
        }

        /// <summary>
        /// iplementação do IDispose
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            //deve-se parar o garbage collector quando chegamos neste ponto.
            //caso contrario, loop infinito.
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Destrutor da classe Objeto
        /// </summary>
        ~Objeto()
        {
            //por padrão, devemos chamar o dispose no destrutor
            Dispose(false);
        }

        /// <summary>
        /// Lista os Eventos ocorridos no Objeto (pacote)
        /// </summary>
        public List<Evento> Eventos { get; set; }
        
        /// <summary>
        /// Numero do objeto (código rastreador)
        /// </summary>
        public string Numero { get; set; }
        
        /// <summary>
        /// Data do cadastro do objeto.
        /// </summary>
        public DateTime DataInicio { get; set; }
        
        /// <summary>
        /// Data que o objeto foi entregue. Apos 1 ou dois dias enviar o objeto para lista de Historico.
        /// </summary>
        public DateTime DataFim { get; set; }

        //Amarração com Endereco e Descricao
        //do cadastro do aplicativo
        /// <summary>
        /// Id do Endereco cadastrado para entrega
        /// </summary>
        public int IdEndereco { get; set; }

        /// <summary>
        /// Descrição do pacote (ex: luvas, jogos)
        /// </summary>
        public string Descricao { get; set; }
    }
}
