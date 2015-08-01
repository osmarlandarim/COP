using Business;
using COPRastreioObjeto.Data;
using Entities;
using Entities.Endereco;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using Microsoft.Advertising.WinRT.UI;
using System.Threading.Tasks;

// The Group Detail Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234229

namespace COPRastreioObjeto
{
    /// <summary>
    /// A page that displays an overview of a single group, including a preview of the items
    /// within the group.
    /// </summary>
    public sealed partial class GroupDetailPage : COPRastreioObjeto.Common.LayoutAwarePage
    {
        public GroupDetailPage()
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
        protected override async void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            // TODO: Create an appropriate data model for your problem domain to replace the sample data
            this.cbxEndereco.ItemsSource = await Core.GetEnderecos();
            this.lblStatusInternet.Text = "\r\n\r\n\r\n" + Util.GetInternetStatus();
            
            var sucesso = await this.CarregarSROXml();

            if (sucesso)
            {

            }

            PublicidadeClass publicidade = new PublicidadeClass();
            Publicidade.Children.Add(await publicidade.MontarPublicidade());
        }

        /// <summary>
        /// Invoked when an item is clicked.
        /// </summary>
        /// <param name="sender">The GridView (or ListView when the application is snapped)
        /// displaying the item clicked.</param>
        /// <param name="e">Event data that describes the item clicked.</param>
        void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                // Navigate to the appropriate destination page, configuring the new page
                // by passing required information as a navigation parameter
                var itemId = ((SampleDataItem)e.ClickedItem).UniqueId;
                this.Frame.Navigate(typeof(ItemDetailPage), itemId);
            }
            catch (Exception ex)
            {
                Core.GravarLog("GroupDetailPage.xaml.cs", "ItemView_ItemClick", ex);
            }
        }

        private void btnCadastrarEndereco_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(GroupDetailPageEndereco), null);
        }

        /// <summary>
        /// Botao de mensagem.
        /// </summary>
        /// <param name="command"></param>
        private void OkBtnClick(IUICommand command)
        {
            return;
        }

        /// <summary>
        /// Cadastrar codigo de rastreio.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            bool valido = true;
            ////validar campos obrigatorios.
            //if (this.cbxEndereco.SelectedIndex == -1)
            //{
            //    MessageDialog msgDialog = new MessageDialog("Campo Endereço Obrigatório.");

            //    //OK Button
            //    UICommand okBtn = new UICommand("OK");
            //    okBtn.Invoked = OkBtnClick;
            //    msgDialog.Commands.Add(okBtn);

            //    //Show message
            //    IUICommand command = await msgDialog.ShowAsync();

            //    if (command.Label == "OK")
            //        valido = false;

            //}

            //if (this.txtCodigoRastreio.Text == string.Empty)
            //{
            //    MessageDialog msgDialog = new MessageDialog("Campo Código de Rastreio Obrigatório");

            //    //OK Button
            //    UICommand okBtn = new UICommand("OK");
            //    okBtn.Invoked = OkBtnClick;
            //    msgDialog.Commands.Add(okBtn);

            //    //Show message
            //    IUICommand command = await msgDialog.ShowAsync();

            //    if (command.Label == "OK")
            //        valido = false;
            //}
            //else if (this.txtCodigoRastreio.Text.Length < 13)
            //{
            //    MessageDialog msgDialog = new MessageDialog("Formato inválido: Código de Rastreio deve Conter 13 Dígitos");

            //    //OK Button
            //    UICommand okBtn = new UICommand("OK");
            //    okBtn.Invoked = OkBtnClick;
            //    msgDialog.Commands.Add(okBtn);

            //    //Show message
            //    IUICommand command = await msgDialog.ShowAsync();

            //    if (command.Label == "OK")
            //        valido = false;
            //}

            try
            {
                valido = true;
                if (valido)
                {
                    Salvando.IsIndeterminate = true;

                    int idEndereco = this.cbxEndereco.SelectedIndex != -1 ? (this.cbxEndereco.SelectedValue as Endereco).Id : -1;
                    var sucesso = await Core.GravarObjetoCorreio(this.txtCodigoRastreio.Text, this.txtDescricao.Text, idEndereco);

                    if (sucesso)
                    {
                        this.LimparDados();
                        var recebido = await Core.AtualizarObjetosDisco();

                        if (recebido)
                        {

                        }

                        var carregou = await this.CarregarSROXml();

                        if (carregou)
                        {

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Core.GravarLog("GroupDetailPage.xaml.cs", "btnSalvar_Click", ex);
            }
            finally
            {
                Salvando.IsIndeterminate = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void LimparDados()
        {
            this.txtCodigoRastreio.Text = string.Empty;
            this.txtDescricao.Text = string.Empty;
        }

        /// <summary>
        /// Desabilitar campos caso chego no limite de 50.
        /// </summary>
        /// <param name="enable"></param>
        private void EnableCampos(bool enable)
        {
            this.cbxEndereco.IsEnabled = enable;
            this.txtCodigoRastreio.IsEnabled = enable;
            this.txtDescricao.IsEnabled = enable;
            this.btnSalvar.IsEnabled = enable;
        }

        /// <summary>
        /// Carregar Itens dos correios.
        /// </summary>
        private async Task<bool> CarregarSROXml()
        {
            SROXml sroXml = null;
            
            try
            {
                sroXml = await Core.GetSROXml();

                if (sroXml != null && sroXml.Objetos.Count >= 50)
                {
                    this.lblErro.Text = "Atingiu Limite Máximo de Encomendas: 50.";
                    this.EnableCampos(false);
                }
                else
                {
                    this.EnableCampos(true);
                }

                if (sroXml != null && sroXml.Objetos != null && sroXml.Objetos.Count > 0)
                {
                    SampleDataSourceObjetosCorreios sample = new SampleDataSourceObjetosCorreios();

                    var group = sample.GetGroup("GroupCorreios", sroXml);
                    this.DefaultViewModel["Group"] = group;
                    if (group != null && group.Items != null)
                        this.DefaultViewModel["Items"] = group.Items;
                }
                else
                {
                    SampleDataSourceObjetosCorreios sample = new SampleDataSourceObjetosCorreios();
                    var group = sample.GetGroupDefault();
                    this.DefaultViewModel["Group"] = group;
                }

                return true;
            }
            catch (Exception ex)
            {
                Core.GravarLog("GroupDetailPage.xaml.cs", "CarregarSROXml", ex);
                return false;
            }
            finally
            {
                if (sroXml != null)
                {
                    sroXml.Dispose();
                    sroXml = null;
                }
            }
        }

        bool m_InMyTextChanged = false;

        /// <summary>
        /// Deixar o codigo de rastreio com letras maiusculas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCodigoRastreio_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (m_InMyTextChanged)
                return; // Recursive!  We can bail quickly.  

            try
            {
                m_InMyTextChanged = true; // Prevent recursion when we change it.
                int selectionStart = txtCodigoRastreio.SelectionStart;
                int selectionLength = txtCodigoRastreio.SelectionLength;
                string originalText = txtCodigoRastreio.Text;
                string newText = originalText.ToUpper();

                if (newText != originalText)
                {
                    txtCodigoRastreio.Text = newText; // Will cause a new TextChanged event.
                    // Set the selection back *after* the assignment, which has reset them.
                    txtCodigoRastreio.SelectionStart = selectionStart;
                    txtCodigoRastreio.SelectionLength = selectionLength;
                }
                m_InMyTextChanged = false; // Allow it for next time.
            }
            catch (Exception)
            {

            }
        }
    }
}
