using Business;
using Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Added during quickstart
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Windows.Web.Syndication;

namespace BackgroundTasks
{
    public sealed class CorreiosBackgroundTask : IBackgroundTask
    {
        /// <summary>
        /// Roda o Tile conforme a sincronizacao.
        /// </summary>
        /// <param name="taskInstance"></param>
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            // Get a deferral, to prevent the task from closing prematurely 
            // while asynchronous code is still running.
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();

            // Download the feed.
            var listObjEventos = await GetObjetosCorreios();

            // Update the live tile with the feed items.
            UpdateTile(listObjEventos);

            // Inform the system that the task is finished.
            deferral.Complete();
        }

        /// <summary>
        /// Busca os Objetos dos Correios.
        /// </summary>
        /// <returns>Dicionario com seus Eventos e destinos.</returns>
        private static async Task<SROXml> GetObjetosCorreios()
        {
            SROXml sroRetorno = new SROXml();
            try
            {
                //Verificar o timer para atualizar a lista do webCorreio.

                using (SROXml sroXmlDisco = await Core.AtualizacaoAutomatica())
                {
                    Objeto objeto = null;
                    Evento evento = null;
                    Destino destino = null;

                    if (sroXmlDisco != null && sroXmlDisco.Objetos != null && sroXmlDisco.Objetos.Count > 0)
                    {
                        foreach (Objeto obj in sroXmlDisco.Objetos)
                        {
                            objeto = new Objeto();
                            evento = new Evento();
                            destino = new Destino();

                            objeto.Numero = obj.Descricao != string.Empty ? obj.Descricao : obj.Numero;

                            foreach (Evento ev in obj.Eventos)
                            {
                                evento.Descricao = ev.Descricao + " - " + ev.Local + "-" + ev.Cidade + " " + ev.UF + ev.Data + " " + ev.Hora;

                                if (ev.Destino != null)
                                {
                                    destino.Cidade = ev.DestinoEvento() + ev.Destino.Bairro + " " + ev.Destino.Cidade + " " + ev.Destino.Uf;
                                    destino.Local = ev.Destino.Local + "\r\n\r\n\r\n\r\n" + Util.GeraSequenciaNumerica();
                                }

                                break;
                            }

                            if (evento.Destino == null)
                                evento.Destino = new Destino();
                            evento.Destino = destino;

                            if (objeto.Eventos == null)
                                objeto.Eventos = new List<Evento>();

                            objeto.Eventos.Add(evento);

                            if (sroRetorno.Objetos == null)
                                sroRetorno.Objetos = new List<Objeto>();

                            sroRetorno.Objetos.Add(objeto);
                        }

                    }
                }                
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }

            return sroRetorno;
        }

        /// <summary>
        /// Atualiza o Tile com informacoes do ObjetoCorreio.
        /// </summary>
        /// <param name="dicObj"></param>
        private static void UpdateTile(SROXml sroTiles)
        {
            // Create a tile update manager for the specified syndication feed.
            var updater = TileUpdateManager.CreateTileUpdaterForApplication();
            updater.EnableNotificationQueue(true);
            updater.Clear();

            // Keep track of the number feed items that get tile notifications. 
            int itemCount = 0;

            // Create a tile notification for each feed item.
            if (sroTiles != null && sroTiles.Objetos != null)
            {
                foreach (var obj in sroTiles.Objetos)
                {
                    string tile = (String.Format("<tile><visual><binding template=\"TileWideText01\">" +
                          "<text id=\"1\">{0}</text>" +
                          "<text id=\"2\">{1}</text>" +
                          "<text id=\"3\">{2}</text>" +
                          "<text id=\"4\">{3}</text>" +
                        "</binding>" +
                        "<binding template=\"TileSquareText03\">" +
                          "<text id=\"1\">{0}</text>" +
                          "<text id=\"2\">{1} </text>" +
                          "<text id=\"3\">{2}</text>" +
                          "<text id=\"4\">{3}</text>" +
                        "</binding>" +
                      "</visual>" +
                    "</tile>", obj.Numero, obj.Eventos[0].Descricao, obj.Eventos[0].Destino.Cidade, obj.Eventos[0].Destino.Local));
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(tile);
                    TileNotification tileNotification = new TileNotification(xmlDoc);
                    TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);

                    // Create a new tile notification. 
                    updater.Update(new TileNotification(xmlDoc));

                    // Don't create more than 5 notifications.
                    if (itemCount++ > 50) break;
                }
            }
        }
    }
}