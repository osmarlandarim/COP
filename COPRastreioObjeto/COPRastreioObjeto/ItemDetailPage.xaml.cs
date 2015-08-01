using Bing.Maps;
using Business;
using COPRastreioObjeto.Data;
using Entities;
using NotificationsExtensions.ToastContent;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Item Detail Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234232

namespace COPRastreioObjeto
{
    /// <summary>
    /// A page that displays details for a single item within a group while allowing gestures to
    /// flip through other items belonging to the same group.
    /// </summary>
    public sealed partial class ItemDetailPage : COPRastreioObjeto.Common.LayoutAwarePage
    {
        string nomeClasse = "ItemDetailPage.xaml.cs";
        SROXml sroCorrent = null;//Objeto para excluir
        bool possoExcluir = false;//Confirmacao de exclusao.


        public ItemDetailPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected async override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            // Allow saved page state to override the initial item to display
            if (pageState != null && pageState.ContainsKey("SelectedItem"))
            {
                navigationParameter = pageState["SelectedItem"];
            }

            this.lblStatusInternet.Text = "\r\n\r\n\r\n\r\n" + Util.GetInternetStatus();

            SROXml sroXml = null;
            Preferencias preferencias = null;
            SROXml sroXmlAtualizado = null;

            try
            {
                sroXml = await Core.GetHistorico(navigationParameter.ToString());

                if (sroXml.Objetos == null || sroXml.Objetos.Count == 0)
                {
                    Correio correio = new Correio();
                    sroXml.Dispose();

                    preferencias = await Core.GetSPreferencias();

                    if (preferencias == null)
                        preferencias = new Preferencias();

                    sroXmlAtualizado = await Core.AtualizacaoAutomatica(preferencias.TempoAtualizacao);

                    if (sroXmlAtualizado == null)
                    {

                    }

                    sroXml = await Core.GetSROXml(navigationParameter.ToString());

                    if (sroXml == null)
                    {

                    }

                    PublicidadeClass publicidade = new PublicidadeClass();
                    Publicidade.Children.Add(await publicidade.MontarPublicidade());
                }


                MyUserControl1.objSROXmlSet(sroXml);

                SampleDataSourceObjetosCorreios sample = new SampleDataSourceObjetosCorreios();

                var group = sample.GetGroup("GroupCorreios", sroXml);
                this.DefaultViewModel["Group"] = group;
                if (group != null && group.Items != null)
                    this.DefaultViewModel["Items"] = group.Items;

                this.flipView.SelectedItem = group;

                sroCorrent = sroXml;
            }
            catch (Exception ex)
            {
                Core.GravarLog(nomeClasse, "LoadState", ex);
            }
            finally
            {
                //if (sroXml != null)
                //{
                //    sroXml.Dispose();
                //    sroXml = null;
                //}

                //if (preferencias != null)
                //{
                //    preferencias.Dispose();
                //    preferencias = null;
                //}
                //if (sroXmlAtualizado != null)
                //{
                //    sroXmlAtualizado.Dispose();
                //    sroXmlAtualizado = null;
                //}
            }
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
            var selectedItem = (SampleDataItem)this.flipView.SelectedItem;
            pageState["SelectedItem"] = selectedItem.UniqueId;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        private void CancelBtnClick(IUICommand command)
        {
            this.possoExcluir = false;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        private void OkBtnClick(IUICommand command)
        {
            this.possoExcluir = true;
        }

        /// <summary>
        /// Delete objeto corrente.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog msgDialog = new MessageDialog(Enumeration.Mensagens.MsgExclusao + " " + this.sroCorrent.Objetos[0].Descricao + " - " + this.sroCorrent.Objetos[0].Numero);

            //OK Button
            UICommand okBtn = new UICommand("OK");
            okBtn.Invoked = OkBtnClick;
            msgDialog.Commands.Add(okBtn);

            //Cancel Button
            UICommand cancelBtn = new UICommand("Cancel");
            cancelBtn.Invoked = CancelBtnClick;
            msgDialog.Commands.Add(cancelBtn);

            //Show message
            IUICommand command = await msgDialog.ShowAsync();

            try
            {
                if (this.possoExcluir)
                {
                    var sucesso = await Core.ExcluirObjeto(sroCorrent.Objetos[0]);

                    if (sucesso)
                    {
                        this.lblExcluido.Text = "\r\n\r\n\r\n\r\nObjeto Excluido Com Sucesso.";
                        this.Delete.IsEnabled = false;
                        this.BottomAppBar1.IsOpen = true;
                    }
                    else
                        this.lblExcluido.Text = "\r\n\r\n\r\n\r\nFalha na Exclusão.";
                }
            }
            catch (Exception ex)
            {
                Core.GravarLog(nomeClasse, "Delete_Click", ex);
            }
        }
    }
}
