using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Xml;
using Windows.Storage;

namespace DAO
{
    public abstract class XmlHelper
    {
        private string gstrNomeArquivo = string.Empty;
        private StorageFolder localFolder = ApplicationData.Current.LocalFolder;//KnownFolders.DocumentsLibrary;
        //private const string Diretorio = "COP Rastreio de Objetos\\";

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="nomeArquivo">Ex: ArqConf.xml</param>
        protected XmlHelper(string nomeArquivo)
        {
            //this.CreateFolder();
            //armazena o nome do arquivo do processo
            this.gstrNomeArquivo = nomeArquivo;
        }

        /// <summary>
        /// Serializa o objeto em um XML no disco. Asincrono
        /// </summary>
        /// <typeparam name="T">Tipo do Objeto a ser serializado. Ex: Evento</typeparam>
        /// <param name="obj">Nome do Objeto do Tipo a ser serializdo. Ex: Evento objEvento = new Evento();</param>
        protected async Task<bool> Serialize<T>(T obj)
        {
            try
            {
                //carrega o arquivo
                Stream fileStream = await localFolder.OpenStreamForWriteAsync(gstrNomeArquivo, CreationCollisionOption.OpenIfExists);

                //limpa o arquivo
                fileStream.SetLength(0);
                fileStream.Position = 0;

                //serializa o objeto completo
                new XmlSerializer(typeof(T)).Serialize(fileStream, obj);
                await fileStream.FlushAsync(); //força fechar o arquivo

                //limpar...
                fileStream.Dispose();
            }
            catch (Exception)
            {
                return false;
            }

            //sucesso!
            return true;
        }

        /// <summary>
        /// Publica o Stream utilizado nas operações. Asincrono.
        /// </summary>
        /// <returns>retorna o stream do arquivo utilizado nas operações</returns>
        public async Task<Stream> GetStream()
        {
            Stream fileStream = await localFolder.OpenStreamForReadAsync(gstrNomeArquivo);
            return fileStream;
        }

        /// <summary>
        /// Le qualquer XML em memória ou em disco
        /// </summary>
        /// <param name="reader">leitor de xml</param>
        /// <param name="Elemento">elemento de partida desejado</param>
        /// <param name="raiz">retorna o xml completo (elemento raiz)</param>
        /// <returns>retorna um enumerável do XML para preenchimento de objetos</returns>
        public static IEnumerable<XElement> StreamXML(XmlReader reader, string Elemento, bool raiz)
        {
            //move o ponteiro do arquivo para o incio do conteudo.          
            reader.MoveToContent();

            //quando verdadeiro, retorna o documento completo em memória
            if (raiz)
                yield return XElement.ReadFrom(reader) as XElement;

            // Lê o arquivo e traz os nós de XML
            while (reader.Read())
            {
                //quando o nó é um nó de tipo
                switch (reader.NodeType)
                {
                    //verifica se o nó é do tipo elemento
                    case XmlNodeType.Element:
                        if (reader.Name == Elemento) //é o mesmo do parametro desejado?
                        {
                            XElement el = XElement.ReadFrom(reader) as XElement;
                            if (el != null)
                                yield return el; //retorna o bloco do xml somente
                        }
                        break;
                }
            }

            reader = null; //limpando...
        }

        /// <summary>
        /// Overload. retorna enumerável somente com caminho e elemento
        /// </summary>
        /// <param name="Caminho">caminho do arquivo</param>
        /// <param name="Elemento">elemento desejado</param>
        /// <returns>enumeravel para preenchimento de objetos</returns>
        public static IEnumerable<XElement> StreamXML(string Caminho, string Elemento)
        {
            return StreamXML(XmlReader.Create(Caminho), Elemento, false);
        }

        /// <summary>
        /// Overload. retorna enumerável de um xml completo ou não
        /// </summary>
        /// <param name="Caminho">caminho do xml</param>
        /// <param name="Elemento">elemento desejavel</param>
        /// <param name="raiz">marca o elemento como raiz ou não</param>
        /// <returns>retorna enumerável completo ou somente do elemento</returns>
        public static IEnumerable<XElement> StreamXML(string Caminho, string Elemento, bool raiz)
        {
            return StreamXML(XmlReader.Create(Caminho), Elemento, raiz);
        }

        /// <summary>
        /// Overload. retorna enumerável de um xml em qualquer lugar no disco
        /// </summary>
        /// <param name="fs">stream do arquivo desejado</param>
        /// <param name="Elemento">elemento desejado</param>
        /// <param name="raiz">marca se o elemento é raiz ou não</param>
        /// <returns>retorna enumerável do xml completo ou só do elmento</returns>
        public static IEnumerable<XElement> StreamXML(Stream fs, string Elemento, bool raiz)
        {
            return StreamXML(XmlReader.Create(fs), Elemento, raiz);
        }

        /// <summary>
        /// Deserializa o XML em Objeto. Asincrono
        /// </summary>
        /// <typeparam name="T">Tipo do Objeto que o XML será deserializado</typeparam>
        /// <returns>Retorna o XML lido em Objeto</returns>
        protected async Task<T> Deserialize<T>()
        {
            T obj = default(T);

            try
            {
                //carrega o sream do objeto
                Stream fileStream = await localFolder.OpenStreamForReadAsync(gstrNomeArquivo);

                //existe dados no objeto?
                if (fileStream.Length > 0)
                    obj = (T)new XmlSerializer(typeof(T)).Deserialize(fileStream); //podemos deserializar

                //elimina o stream
                fileStream.Dispose();
            }
            catch (Exception ex)
            {
                //erro de iO
                if (ex.Message.Length > 0)
                    return obj;
            }
            return obj;
        }
    }
}
