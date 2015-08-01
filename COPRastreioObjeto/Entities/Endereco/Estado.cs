using DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Endereco
{
    public class Estado : XmlHelper, IDisposable
    {
        private bool disposed = false;

        /// <summary>
        /// Id do Estado
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// nome do estado
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// UF do Estado (abreviação do nome)
        /// </summary>
        public string UF { get; set; }

        /// <summary>
        /// ID do País que este estado pertence
        /// </summary>
        public int Pais { get; set; }

        /// <summary>
        /// Construtor da classe
        /// </summary>
        public Estado()
            : base("")
        {
            //faz nada
        }

        /// <summary>
        /// override do dispose
        /// </summary>
        /// <param name="disposing">identifica se devemos recolher os obejtos</param>
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
        /// Implementação do iDispose
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            //loop infito...
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Destrutor da classe
        /// </summary>
        ~Estado()
        {
            //Dispose...
            Dispose(false);
        }

        /// <summary>
        /// Recupera os Estados de um País
        /// de um XML da aplicação. o mesmo deve estar em ../dados/ junto do exe da aplicação
        /// </summary>
        /// <param name="IdPais">id do país que queremos os estados</param>
        /// <returns>retorna uma lista de todos os estados de um páis</returns>
        public List<Estado> GetEstado(int IdPais)
        {
            //linq to xml
            IEnumerable<Estado> data =
                (from el in StreamXML("dados/Estado.xml", "Estado")
                 where (int)el.Element("Pais") == IdPais //onde o iD do país é igual ao id do pais que contem o estado
                 select new Estado
                 {
                     Id = (int)el.Element("Id"),
                     Nome = el.Element("Nome").Value,
                     UF = el.Element("UF").Value,
                     Pais = (int)el.Element("Pais")
                 });

            //retorna a lista dos estados.
            return data.ToList<Estado>();
        }
    }
}
