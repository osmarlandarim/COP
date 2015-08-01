using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Endereco
{
    public class EnderecoCorreios
    {
        public string Logradouro { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string UF { get; set; }
        public string Cep { get; set; }

        /// <summary>
        /// Busca enderecos dos correios
        /// </summary>
        /// <param name="CEP"></param>
        //private async static BuscarCEP(string CEP)
        //{
        //    HttpWebRequest request = (HttpWebRequest)
        //        HttpWebRequest.Create("http://www.buscacep.correios.com.br/servicos/dnec/consultaEnderecoAction.do");

        //    request.Method = "POST";
        //    string postData = "relaxation=" + CEP +
        //        "&TipoCep=ALL&semelhante=N&cfm=1&Metodo=listaLogradouro&TipoConsulta=relaxation&StartRow=1&EndRow=10";
        //    byte[] byteArray = Encoding.UTF8.GetBytes(postData);

        //    request.ContentType = "application/x-www-form-urlencoded";
        //    //request.ContentLength = byteArray.Length;

        //    Stream dataStream = await request.GetRequestStreamAsync();
        //    dataStream.Write(byteArray, 0, byteArray.Length);
        //    //dataStream.Close();

        //    HttpWebResponse response = await (HttpWebResponse)request.GetResponseAsync();
        //    dataStream = response.GetResponseStream();

        //    Encoding encoder = Encoding.GetEncoding("ISO-8859-1");
        //    StreamReader reader = new StreamReader(dataStream, encoder);

        //    string responseFromServer = reader.ReadToEnd();

        //    reader.Close();
        //    dataStream.Close();
        //    response.Close();

        //    string keyWord = "<?xml version = '1.0' encoding = 'ISO-8859-1'?>";
        //    int pos = responseFromServer.IndexOf(keyWord);

        //    responseFromServer = responseFromServer.Substring(pos, responseFromServer.Length - pos);
        //    responseFromServer = responseFromServer.Replace(keyWord, string.Empty);
        //    responseFromServer = responseFromServer.Replace("<table", "<table id=\"9999\"");
        //    responseFromServer = responseFromServer.Substring(0, responseFromServer.IndexOf("</table>") + "</table>".Length);

        //    XDocument xdoc = XDocument.Parse(responseFromServer.Replace("\r\n", string.Empty));

        //    HtmlDocument doc = new HtmlDocument();
        //    doc.LoadHtml(responseFromServer);

        //    XmlDocument xml = new XmlDocument();
        //    xml.CreateNode(XmlNodeType.Element, "Retorno", "");

        //    this.Logradouro = doc.DocumentNode.ChildNodes[1].ChildNodes[1].ChildNodes[1].InnerHtml;
        //    this.Bairro = doc.DocumentNode.ChildNodes[1].ChildNodes[1].ChildNodes[3].InnerHtml;
        //    this.Cidade = doc.DocumentNode.ChildNodes[1].ChildNodes[1].ChildNodes[5].InnerHtml;
        //    this.UF = doc.DocumentNode.ChildNodes[1].ChildNodes[1].ChildNodes[7].InnerHtml;
        //    this.Cep = doc.DocumentNode.ChildNodes[1].ChildNodes[1].ChildNodes[9].InnerHtml.Replace("-", string.Empty);

        //    string endereco = this.Logradouro + " " + this.Bairro + " " + this.Cidade + " " + this.UF;
        //    DataSet ds = new DataSet();
        //    //ds.ReadXml("http://maps.googleapis.com/maps/api/geocode/xml?address=" + endereco + "&sensor=false");
        //    ds.ReadXml("https://maps.googleapis.com/maps/api/place/textsearch/xml?type=restaurante&query=" + endereco + "&sensor=false&key=AIzaSyBIcY9naBlYWpfaTKD2GQ3vlD9rxuahGVQ");




        //    return doc.GetElementbyId("9999").InnerHtml;
        //}
    }
}
