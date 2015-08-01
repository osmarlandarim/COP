using Business;
using COPRastreioObjeto.Data;
using Entities;
using NotificationsExtensions.ToastContent;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Grouped Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234231

namespace COPRastreioObjeto
{
    /// <summary>
    /// A page that displays a grouped collection of items.
    /// </summary>
    public sealed partial class GroupedItemsPage : COPRastreioObjeto.Common.LayoutAwarePage
    {
        public GroupedItemsPage()
        {
            this.InitializeComponent();

            PublicidadePart publi = new PublicidadePart();
            //publi.ObtemPublicidade();
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
            // TODO: Create an appropriate data model for your problem domain to replace the sample data

            this.RegisterBackgroundTask();

            this.CarregarItensTela((String)navigationParameter);

            PublicidadeClass publicidadeLeft = new PublicidadeClass();
            Publicidade.Children.Add(await publicidadeLeft.MontarPublicidade(Windows.UI.Xaml.HorizontalAlignment.Left, false));

            PublicidadeClass publicidadeCenter = new PublicidadeClass();
            Publicidade.Children.Add(await publicidadeCenter.MontarPublicidade(Windows.UI.Xaml.HorizontalAlignment.Center, false));

            PublicidadeClass publicidadeRight = new PublicidadeClass();
            Publicidade.Children.Add(await publicidadeRight.MontarPublicidade(Windows.UI.Xaml.HorizontalAlignment.Right, false));


            IToastNotificationContent toastContent = null;
            IToastText03 templateContent = ToastContentFactory.CreateToastText03();
            templateContent.TextHeadingWrap.Text = "Body text that wraps over three lines";
            templateContent.TextBody.Text = "Teste";
            toastContent = templateContent;
            ToastNotification toast = toastContent.CreateNotification();
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

        /// <summary>
        /// Carregar Grupos da Tela.
        /// </summary>
        /// <param name="grupos"></param>
        private async void CarregarItensTela(string grupos)
        {
            Atualizando.IsIndeterminate = true;
            SROXml sroXml = null;
            HistoricoSRO historico = null;
            Preferencias preferencias = null;

            try
            {
                this.lblStatusInternet.Text = Util.GetInternetStatus();
                sroXml = await Core.GetSROXml();

                if (sroXml == null)
                    sroXml = new SROXml();

                if (sroXml.Objetos == null)
                {
                    var atualizou = await this.AtualizarObjetos();

                    if (atualizou)
                    {

                    }
                }

                historico = await Core.GetHistorico();

                if (sroXml.Objetos != null && sroXml.Objetos.Count > 0 && sroXml.UltimaAtualizacao != DateTime.MinValue)
                    this.lblUltimaAtualizacao.Text = "\r\n\r\n\r\n\r\nÚltima Atualização: " + sroXml.UltimaAtualizacao.ToString("dd/MM/yy HH:mm");
                else
                    this.lblUltimaAtualizacao.Text = string.Empty;

                if ((sroXml != null && sroXml.Objetos != null && sroXml.Objetos.Count > 0) || (historico.ObjetoSRO != null && historico.ObjetoSRO.Objetos != null && historico.ObjetoSRO.Objetos.Count > 0))
                {
                    preferencias = await Core.GetSPreferencias();

                    if (preferencias == null)
                        preferencias = new Preferencias();

                    if (!preferencias.AtualizacaoAutomatica)
                    {
                        this.lblPrevisaoProxAtualizacao.Text = "\r\n\r\n\r\nAtualização Automática desabilitada.\r\nPara alterar vá Configurações/Preferências do sistema.";
                    }
                    else
                    {
                        if (sroXml.UltimaAtualizacao != DateTime.MinValue)
                            this.lblPrevisaoProxAtualizacao.Text = "\r\n\r\n\r\nPróxima atualização prevista para: " + sroXml.UltimaAtualizacao.AddHours(preferencias.TempoAtualizacao).ToString("dd/MM/yy HH:mm");
                    }

                    SampleDataSourceObjetosCorreios sample = new SampleDataSourceObjetosCorreios();
                    var sampleDataGroups = sample.GetsGroups(grupos, sroXml, historico);
                    this.DefaultViewModel["Groups"] = sampleDataGroups;
                }
                else
                {
                    SampleDataSourceObjetosCorreios sample = new SampleDataSourceObjetosCorreios();
                    var sampleDataGroups = sample.GetsGroups(grupos, sroXml, historico);
                    this.DefaultViewModel["Groups"] = sampleDataGroups;
                }
            }
            catch (Exception ex)
            {
                Core.GravarLog("GroupedItemPage.xaml.cs", "CarregarItensTela", ex);
            }
            finally
            {
                Atualizando.IsIndeterminate = false;

                if (sroXml != null)
                {
                    sroXml.Dispose();
                    sroXml = null;
                }

                if (historico != null)
                {
                    historico.Dispose();
                    historico = null;
                }

                if (preferencias != null)
                {
                    preferencias.Dispose();
                    preferencias = null;
                }
            }
        }

        /// <summary>
        /// Cria thread para Tiles.
        /// </summary>
        private async void RegisterBackgroundTask()
        {
            try
            {
                var backgroundAccessStatus = await BackgroundExecutionManager.RequestAccessAsync();
                if (backgroundAccessStatus == BackgroundAccessStatus.AllowedMayUseActiveRealTimeConnectivity ||
                    backgroundAccessStatus == BackgroundAccessStatus.AllowedWithAlwaysOnRealTimeConnectivity)
                {
                    foreach (var task in BackgroundTaskRegistration.AllTasks)
                    {
                        if (task.Value.Name == taskName)
                        {
                            task.Value.Unregister(true);
                        }
                    }

                    BackgroundTaskBuilder taskBuilder = new BackgroundTaskBuilder();
                    taskBuilder.Name = taskName;
                    taskBuilder.TaskEntryPoint = taskEntryPoint;
                    taskBuilder.SetTrigger(new TimeTrigger(15, true));
                    var registration = taskBuilder.Register();
                }
            }
            catch (Exception ex)
            {
                Core.GravarLog("GroupedItemPage.xaml.cs", "RegisterBackgroundTask", ex);
            }
        }

        private const string taskName = "CorreiosBackgroundTask";
        private const string taskEntryPoint = "BackgroundTasks.CorreiosBackgroundTask";

        /// <summary>
        /// Invoked when a group header is clicked.
        /// </summary>
        /// <param name="sender">The Button used as a group header for the selected group.</param>
        /// <param name="e">Event data that describes how the click was initiated.</param>
        void Header_Click(object sender, RoutedEventArgs e)
        {
            // Determine what group the Button instance represents
            var group = (sender as FrameworkElement).DataContext;

            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            if (((SampleDataGroup)group).UniqueId == "Publicidade")
            {
                this.AbrirCOPPro();
            }
            else
            {
                this.Frame.Navigate(typeof(GroupDetailPage), ((SampleDataGroup)group).UniqueId);
            }
        }

        /// <summary>
        /// Invoked when an item within a group is clicked.
        /// </summary>
        /// <param name="sender">The GridView (or ListView when the application is snapped)
        /// displaying the item clicked.</param>
        /// <param name="e">Event data that describes the item clicked.</param>
        void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            var itemId = ((SampleDataItem)e.ClickedItem).UniqueId;

            if (itemId == "Publicidade")
            {
                this.AbrirCOPPro();
            }
            else
            {
                if (itemId != string.Empty)
                    this.Frame.Navigate(typeof(ItemDetailPage), itemId);
                else
                {
                    this.Frame.Navigate(typeof(GroupDetailPage), "Correios");
                }
            }
        }

        private async void AbrirCOPPro()
        {
            var storeURI = new Uri("ms-windows-store:PDP?PFN=6293OsmarLandarim.COPRastreiodeObjetoPRO_g2d2vmfjy3ra0&sessionId=933385ace0a84767b1db3be183b3e539&form=WEBPDP");
            await Windows.System.Launcher.LaunchUriAsync(storeURI);
        }

        private void AddObjeto_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(GroupDetailPage), "Correios");
        }

        /// <summary>
        /// Atualizar a tela.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Refresh_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var sucesso = await this.AtualizarObjetos();

                if (sucesso)
                {

                }
            }
            catch (Exception ex)
            {
                Core.GravarLog("GroupedItemPage.xaml.cs", "Refresh_Click", ex);
            }
        }

        /// <summary>
        /// Atualizar objetos.
        /// </summary>
        private async Task<bool> AtualizarObjetos()
        {
            Atualizando.IsIndeterminate = true;
            await Task.Delay(TimeSpan.FromMilliseconds(100));
            var sucesso = false;
            try
            {
                sucesso = await Core.AtualizarObjetosDisco();

                if (sucesso)
                {
                    var sucessoHistorico = await Core.GravarObjetoHistorico();

                    if (sucessoHistorico)
                    {

                    }

                    this.CarregarItensTela("AllGroups");//Atualizar todos os grupos.
                }
            }
            catch (Exception ex)
            {
                Core.GravarLog("GroupedItemPage.xaml.cs", "AtualizarObjetos", ex);
            }
            finally
            {
                Atualizando.IsIndeterminate = false;
            }

            return sucesso;
        }
    }
}