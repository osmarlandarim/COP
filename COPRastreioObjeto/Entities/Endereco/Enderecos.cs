using DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Devices.Geolocation;
using System.Globalization;

namespace Entities.Endereco
{
    /// <summary>
    /// Classe Endereços.
    /// Existe somente para facilitar a serialização/deserialização
    /// dos endereços cadastrados.
    /// </summary>
    public class Enderecos : XmlHelper, IDisposable
    {
        private bool disposed = false;

        /// <summary>
        /// lista dos endereços cadastrados
        /// </summary>
        public List<Endereco> ListaEnderecos { get; set; }

        /// <summary>
        /// Construtor da classe Endereços
        /// informa ao XmlHelper qual arquivo devemos procurar
        /// </summary>
        public Enderecos()
            : base("ArqEndereco.xml")
        {
            //Faz nada
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
                    this.ListaEnderecos = null; //limpar os endereços da memória

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
        /// Destrutor da classe Endereços
        /// </summary>
        ~Enderecos()
        {
            //por padrão, chama-se o dispose
            Dispose(false);
        }
    }

    /// <summary>
    /// Calsse responsável pelo cadastro do endereço
    /// </summary>
    public class Endereco : XmlHelper, IDisposable
    {
        private bool disposed = false;

        /// <summary>
        /// Construtor do Endereço
        /// informa o arquivo xml a ser lido/escrito
        /// </summary>
        public Endereco()
            : base("ArqEndereco.dat")
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
                    this.Enderecos = null;

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
        /// Destrutor da classe
        /// </summary>
        ~Endereco()
        {
            //limpar memória
            Dispose(false);
        }

        /// <summary>
        /// Identificação unica do endereço
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Estado (UF) do endereço final
        /// </summary>
        public string Estado { get; set; }

        /// <summary>
        /// Cidade do endereço final
        /// </summary>
        public string Cidade { get; set; }

        /// <summary>
        /// Rua do endereço final
        /// </summary>
        public string Rua { get; set; }

        /// <summary>
        /// Número do Endereço final - se nulo, é um endereço único
        /// </summary>
        public int? Numero { get; set; }

        /// <summary>
        /// código postal (CEP)
        /// </summary>
        public string CEP { get; set; }

        /// <summary>
        /// Bairro do endereço
        /// </summary>
        public string Bairro { get; set; }

        /// <summary>
        /// Complemento do Endereço, (ex: Casa Nº 1, Andar 23, etc)
        /// </summary>
        public string Complemento { get; set; }

        /// <summary>
        /// Coordenadas X do Endereco.
        /// </summary>
        public double GeoX { get; set; }

        /// <summary>
        /// Coordenadas Y do Endereco.
        /// </summary>
        public double GeoY { get; set; }

        /// <summary>
        /// Endereço visível, formatado de forma amigavel para seleção em tela
        /// </summary>
        public string EnderecoVisivel
        {
            get
            {
                string strEndereco = /*this.Id.ToString() + " - " +*/ this.Rua + ", " + (this.Numero == null ? "S/N" : this.Numero.ToString());
                return strEndereco;
            }
        }

        /// <summary>
        /// lista dos endereços cadastrados, somente para serializar/deserializar
        /// </summary>
        public List<Enderecos> Enderecos { get; set; }

        /// <summary>
        /// Grava o endereço cadastrado em disco
        /// </summary>
        /// <returns>retorna verdadeiro no caso de sucesso, falso quando ocorreram erros</returns>
        public async Task<bool> GravaEndereco()
        {
            //deve-se ler todos os endereços
            Enderecos ends = await base.Deserialize<Enderecos>();

            //e gerar um ID para o próximo endereço
            //if (ends == null || ends.ListaEnderecos == null)
            //    this.Id = 1;
            //else
            //    this.Id = ends.ListaEnderecos.Count + 1;

            this.Id = Convert.ToInt32(Util.GeraSequenciaNumerica());

            //Validamos o endereço, se esta tudo preenchido corretamente
            if (this.ValidaEndereco())
            {
                //devemos criar uma lista de endereços
                //no caso da primeira serialização.
                if (ends != null && ends.ListaEnderecos == null)
                    ends.ListaEnderecos = new List<Endereco>();

                //e esta lista deve ser refletida no objeto de Endereços
                if (ends == null)
                {
                    ends = new Enderecos();
                    ends.ListaEnderecos = new List<Endereco>();
                }

                //adiciona-se o novo endereco na coleção
                ends.ListaEnderecos.Add(this);

                //serializamos
                //toda a operação de IO em disco deve ser asincrona
                var sucesso = await base.Serialize<Enderecos>(ends);

                //sucesso! ou não...
                if (sucesso)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        /// <summary>
        /// Validação do preenchimento dos parametros que caracterizam um endereço
        /// </summary>
        /// <returns>retorna verdadeiro quando os campos obrigatórios foram devidamente preenchidos</returns>
        public bool ValidaEndereco()
        {
            if (this.Estado == string.Empty)
                throw new Exception("Estado não Preenchido");

            if (this.Cidade == string.Empty)
                throw new Exception("Cidade não Preenchida");

            if (this.Rua == string.Empty)
                throw new Exception("Rua não Preenchida");

            if (this.Numero != null && this.Numero <= 0)
                throw new Exception("Número com formato Inválido");

            return true;
        }

        /// <summary>
        /// Carrega os endereços cadastrados no disco
        /// note que deserializa um objeto Enderecos
        /// </summary>
        /// <returns>retorna a lista de todos os endereços cadastrados</returns>
        public async Task<List<Endereco>> GetEnderecos()
        {
            //cria um objeto de endereços
            //todo o metodo de IO deve ser asincrono
            Enderecos ends = await base.Deserialize<Enderecos>();

            //validamos o retorno para não apontar à NULL
            //isso termina a Task paralela e retorna à thread principal
            if (ends == null)
                ends = new Enderecos();

            //retorna os endereços
            return ends.ListaEnderecos;
        }

        public bool BuscaGeoLocalizacaoEndereco()
        {
            string endereco = string.Empty;

            if (this.CEP != string.Empty)
                endereco = this.CEP;

            if (this.Rua != string.Empty)
                endereco = endereco + "," + this.Rua;

            if (this.Numero != null)
                endereco = endereco + "," + this.Numero.ToString();

            if (this.Cidade != string.Empty)
                endereco = endereco + "," + this.Cidade;

            if (this.Estado != string.Empty)
                endereco = endereco + "," + this.Estado;


            return BuscaGeoLocalizacaoEndereco("http://maps.googleapis.com/maps/api/geocode/xml?address=" + endereco + "&sensor=false",
                                            null);
        }

        /// <summary>
        /// Busca o Objeto
        /// </summary>
        /// <param name="strCaminho">URL local ou Internet</param>
        /// <param name="codigosRastreio">Lista dos Códigos de Rastreio</param>
        /// <returns>retorna o objeto SRO do Correio</returns>
        private bool BuscaGeoLocalizacaoEndereco(string strCaminho, Stream fileStream)
        {
            try
            {
                //Enumerável dos Elementos de um XML
                IEnumerable<XElement> auxData = StreamXML(strCaminho, "location", false); //online ou com URL específica                
                //Linq to XML
                Endereco data =
                        (from xel in auxData
                         select new Endereco
                         {
                             GeoX = Convert.ToDouble(xel.Element("lat").Value, new CultureInfo("en-US")),
                             GeoY = Convert.ToDouble(xel.Element("lng").Value, new CultureInfo("en-US")),
                         }).FirstOrDefault<Endereco>();

                this.GeoX = data.GeoX;
                this.GeoY = data.GeoY;
            }
            catch (Exception ex)
            {
                //SROXml aux = new SROXml();
                //aux.Versao = "0";
                //aux.TipoResultado = ex.Message;
                //aux.TipoPesquisa = "Falha na comunicação com a Internet";

                //return aux;
            }

            return true;
        }
    }
}
