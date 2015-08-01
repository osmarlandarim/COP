using DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Endereco
{
    public class Cidade : XmlHelper, IDisposable
    {
        private bool disposed = false;

        /// <summary>
        /// ID da cidade
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nome da cidade
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// ID do Estado em que encontra-se esta Cidade
        /// </summary>
        public int Estado { get; set; }

        /// <summary>
        /// Construtor da Classe de Cidades
        /// </summary>
        public Cidade()
            : base("")
        {
            //faz nada
        }

        /// <summary>
        /// override do IDispose
        /// </summary>
        /// <param name="disposing">identifica se devemos recolher os objetos</param>
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
        /// implementação do IDispose
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            //evitar loop infinto
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Destrutor da classe 
        /// </summary>
        ~Cidade()
        {
            //por padrão, chama-se o dispose
            Dispose(false);
        }

        /// <summary>
        /// Busca as Cidades no XML em Disco
        /// este XML deve estar em ../dados/ e junto do exe da aplicação!
        /// </summary>
        /// <param name="IdEstado">Id do Estado para listar as cidades</param>
        /// <returns>retorna uma lista de cidades pertencentes ao estado</returns>
        public List<Cidade> GetCidade(int IdEstado)
        {
            //linq to XML
            IEnumerable<Cidade> data =
                (from el in StreamXML("dados/Cidade.xml", "Cidade")
                 where (int)el.Element("Estado") == IdEstado //onde o iD do estado seja igual ao id estado da cidade
                 select new Cidade
                 {
                     Id = (int)el.Element("Id"),
                     Nome = el.Element("Nome").Value,
                     Estado = (int)el.Element("Estado")
                 });

            //retorna uma lista de cidades
            return data.ToList<Cidade>();
        }
    }
}
