using Business;
using COPRastreioObjeto.Data;
using Entities;
using Entities.Endereco;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Group Detail Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234229

namespace COPRastreioObjeto
{
    /// <summary>
    /// A page that displays an overview of a single group, including a preview of the items
    /// within the group.
    /// </summary>
    public sealed partial class GroupDetailPageEndereco : COPRastreioObjeto.Common.LayoutAwarePage
    {
        public GroupDetailPageEndereco()
        {
            this.InitializeComponent();
        }

        string nomeClasse = "GroupDetailPageEndereco.xaml.cs";

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
            // TODO: Assign a bindable group to this.DefaultViewModel["Group"]
            // TODO: Assign a collection of bindable items to this.DefaultViewModel["Items"]
            progressCep.IsActive = false;

            this.CarregarEnderecos();

            this.lblStatusInternet.Text = "\r\n\r\n\r\n" + Util.GetInternetStatus();

            PublicidadeClass publicidade = new PublicidadeClass();
            Publicidade.Children.Add(await publicidade.MontarPublicidade());
        }

        /// <summary>
        /// Limpar campos apos salvar
        /// </summary>
        private void Limpar()
        {
            this.txtEstado.Text = string.Empty;
            this.txtCidade.Text = string.Empty;
            this.txtCEP.Text = string.Empty;
            this.txtBairro.Text = string.Empty;
            this.txtRua.Text = string.Empty;
            this.txtComplemento.Text = string.Empty;
            this.txtNumero.Text = string.Empty;
        }

        /// <summary>
        /// Carregar enderecos cadastrados.
        /// </summary>
        private async void CarregarEnderecos()
        {
            List<Endereco> listEnderecos = null;
            try
            {
                listEnderecos = await Core.GetEnderecos();
                SampleDataSourceEndereco sample = new SampleDataSourceEndereco();

                var group = sample.GetGroup("GroupEnderecos", listEnderecos);
                this.DefaultViewModel["Group"] = group;
                if (group != null && group.Items != null)
                    this.DefaultViewModel["Items"] = group.Items;
            }
            catch (Exception ex)
            {
                Core.GravarLog(nomeClasse, "CarregarEnderecos", ex);
            }
            finally
            {
                if (listEnderecos != null)
                {
                    listEnderecos.Clear();
                    listEnderecos = null;
                }
            }
        }

        /// <summary>
        /// Invoked when an item is clicked.
        /// </summary>
        /// <param name="sender">The GridView (or ListView when the application is snapped)
        /// displaying the item clicked.</param>
        /// <param name="e">Event data that describes the item clicked.</param>
        private void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                // Navigate to the appropriate destination page, configuring the new page
                // by passing required information as a navigation parameter
                var itemId = ((SampleDataItem)e.ClickedItem).UniqueId;
                this.GoBack(null, null);
            }
            catch (Exception ex)
            {
                Core.GravarLog(nomeClasse, "ItemView_ItemClick", ex);
            }
        }

        /// <summary>
        /// Busca endereco pelo CEP.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void txtCEP_LostFocus(object sender, RoutedEventArgs e)
        {
            progressCep.IsActive = true;
            await Task.Delay(TimeSpan.FromMilliseconds(100));

            try
            {
                using (WebCep webCep = new WebCep())
                {
                    webCep.GetEnderecoCep(this.txtCEP.Text);
                    if (webCep.Resultado != 0)
                    {
                        this.txtEstado.Text = webCep.UF;
                        this.txtCidade.Text = webCep.Cidade;
                        this.txtBairro.Text = webCep.Bairro;
                        this.txtRua.Text = webCep.Logradouro;

                        if (webCep.Bairro != string.Empty && webCep.Logradouro != string.Empty)
                            this.txtComplemento.Focus(Windows.UI.Xaml.FocusState.Keyboard);
                    }
                    else
                    {
                        this.txtEstado.Text = string.Empty;
                        this.txtCidade.Text = string.Empty;
                        this.txtBairro.Text = string.Empty;
                        this.txtRua.Text = string.Empty;
                        this.txtComplemento.Text = string.Empty;
                        this.txtNumero.Text = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                Core.GravarLog(nomeClasse, "txtCEP_LostFocus", ex);
            }
            finally
            {
                progressCep.IsActive = false;
            }
        }

        /// <summary>
        /// Adicionar Enderecos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnAdicionar_Click(object sender, RoutedEventArgs e)
        {
            bool valido = true;
            //validar campos obrigatorios.
            if (this.txtCEP.Text == string.Empty || this.txtCEP.Text.Length < 8)
            {
                MessageDialog msgDialog = new MessageDialog("Campo CEP Obrigatório ou Formato Inválido");

                //OK Button
                UICommand okBtn = new UICommand("OK");
                okBtn.Invoked = OkBtnClick;
                msgDialog.Commands.Add(okBtn);

                //Show message
                IUICommand command = await msgDialog.ShowAsync();

                if (command.Label == "OK")
                    valido = false;
            }

            if (this.txtEstado.Text == string.Empty)
            {
                MessageDialog msgDialog = new MessageDialog("Campo UF Obrigatório");

                //OK Button
                UICommand okBtn = new UICommand("OK");
                okBtn.Invoked = OkBtnClick;
                msgDialog.Commands.Add(okBtn);

                //Show message
                IUICommand command = await msgDialog.ShowAsync();

                if (command.Label == "OK")
                    valido = false;
            }

            if (this.txtCidade.Text == string.Empty)
            {
                MessageDialog msgDialog = new MessageDialog("Campo Cidade Obrigatório");

                //OK Button
                UICommand okBtn = new UICommand("OK");
                okBtn.Invoked = OkBtnClick;
                msgDialog.Commands.Add(okBtn);

                //Show message
                IUICommand command = await msgDialog.ShowAsync();

                if (command.Label == "OK")
                    valido = false;
            }

            if (this.txtRua.Text == string.Empty)
            {
                MessageDialog msgDialog = new MessageDialog("Campo Rua Obrigatório");

                //OK Button
                UICommand okBtn = new UICommand("OK");
                okBtn.Invoked = OkBtnClick;
                msgDialog.Commands.Add(okBtn);

                //Show message
                IUICommand command = await msgDialog.ShowAsync();

                if (command.Label == "OK")
                    valido = false;
            }

            try
            {
                if (valido)
                {
                    Salvando.IsIndeterminate = true;
                    await Task.Delay(TimeSpan.FromMilliseconds(100));

                    int? numero = null;
                    if (this.txtNumero.Text != string.Empty)
                    {
                        numero = Convert.ToInt32(this.txtNumero.Text);
                    }

                    var sucesso = await Core.GravarEndereco(this.txtEstado.Text,
                        this.txtCidade.Text,
                        numero,
                        this.txtRua.Text, this.txtBairro.Text, this.txtComplemento.Text, this.txtCEP.Text);

                    if (sucesso)
                    {
                        this.CarregarEnderecos();
                        this.Limpar();
                    }
                }
            }
            catch (Exception ex)
            {
                Core.GravarLog(nomeClasse, "btnAdicionar_Click", ex);
            }
            finally
            {
                Salvando.IsIndeterminate = false;
            }
        }

        /// <summary>
        /// Add novo endereco.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCadastrarEndereco_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(GroupDetailPageEndereco), null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        private void OkBtnClick(IUICommand command)
        {
            return;
        }
    }
}
