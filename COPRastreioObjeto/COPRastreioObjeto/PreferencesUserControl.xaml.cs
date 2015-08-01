using Business;
using Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace COPRastreioObjeto
{
    public sealed partial class PreferencesUserControl : UserControl
    {
        string nomeClasse = "PreferencesUserControl.xaml.cs";

        public PreferencesUserControl()
        {
            this.InitializeComponent();
            this.CarregarInicial();
        }

        /// <summary>
        /// Carrega dados da Tela.
        /// </summary>
        private async void CarregarInicial()
        {
            Preferencias preferencias = null;
            try
            {
                this.lblSalvoComSucesso.Text = string.Empty;
                preferencias = await Core.GetSPreferencias();

                if (!preferencias.AtualizacaoAutomatica)
                    this.txtTempoAtualizacao.Text = string.Empty;
                else
                {
                    if (preferencias.TempoAtualizacao == 0)
                        this.txtTempoAtualizacao.Text = string.Empty;
                    else
                        this.txtTempoAtualizacao.Text = preferencias.TempoAtualizacao.ToString();
                }

                this.atualizarAutomatico.IsOn = preferencias.AtualizacaoAutomatica;
                this.txtTempoAtualizacao.IsEnabled = preferencias.AtualizacaoAutomatica;
            }
            catch (Exception ex)
            {
                Core.GravarLog(nomeClasse, "CarregarInicial", ex);
            }
            finally
            {
                if (preferencias != null)
                {
                    preferencias.Dispose();
                    preferencias = null;
                }
            }
        }

        /// <summary>
        /// Evento enable txt valor atualizacao
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ToggleSwitch_PointerCaptureLost_1(object sender, PointerRoutedEventArgs e)
        {
            Preferencias preferencias = null;
            try
            {
                preferencias = await Core.GetSPreferencias();

                if (!atualizarAutomatico.IsOn)
                    this.txtTempoAtualizacao.Text = string.Empty;
                else
                {
                    if (preferencias.TempoAtualizacao == 0)
                        this.txtTempoAtualizacao.Text = string.Empty;
                    else
                        this.txtTempoAtualizacao.Text = preferencias.TempoAtualizacao.ToString();
                }

                this.txtTempoAtualizacao.IsEnabled = atualizarAutomatico.IsOn;
            }
            catch (Exception ex)
            {
                Core.GravarLog(nomeClasse, "ToggleSwitch_PointerCaptureLost_1", ex);
            }
            finally
            {
                if (preferencias != null)
                {
                    preferencias.Dispose();
                    preferencias = null;
                }
            }
        }

        /// <summary>
        /// Salvar Preferencias.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            this.Salvar();
        }

        /// <summary>
        /// Salvar preferencias.
        /// </summary>
        private async void Salvar()
        {
            this.lblSalvoComSucesso.Text = string.Empty;
            Preferencias preferencias = null;

            try
            {
                if (this.atualizarAutomatico.IsOn && this.txtTempoAtualizacao.Text == string.Empty || (this.txtTempoAtualizacao.Text != string.Empty && Convert.ToInt16(this.txtTempoAtualizacao.Text) <= 0))
                {
                    this.lblSalvoComSucesso.Text = "Quando Atualização Automática estiver ativado.\r\nO campo Tempo para atualização é Obrigatório e maior que Zero!";
                }
                else
                {
                    preferencias = await Core.GetSPreferencias();
                    preferencias.AtualizacaoAutomatica = this.atualizarAutomatico.IsOn;
                    if (this.txtTempoAtualizacao.Text != string.Empty)
                        preferencias.TempoAtualizacao = Convert.ToInt16(this.txtTempoAtualizacao.Text);
                    else
                        preferencias.TempoAtualizacao = 0;
                    this.lblSalvoComSucesso.Text = "Alteração Salva com Sucesso.";

                    var sucess = await Core.GravarPreferencias(preferencias);

                    if (sucess)
                    {

                    }
                }
            }
            catch (Exception)
            {
                this.lblSalvoComSucesso.Text = "Valor Inválido";
            }
            finally
            {
                if (preferencias != null)
                {
                    preferencias.Dispose();
                    preferencias = null;
                }
            }
        }

        /// <summary>
        /// Salvar quando sair da tela.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StackPanel_Unloaded_1(object sender, RoutedEventArgs e)
        {
            this.Salvar();
        }

        bool m_InMyTextChanged = false;

        /// <summary>
        /// validar somente numeros.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtTempoAtualizacao_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (m_InMyTextChanged)
                return; // Recursive!  We can bail quickly.  

            try
            {
                m_InMyTextChanged = true; // Prevent recursion when we change it.
                int selectionStart = txtTempoAtualizacao.SelectionStart;
                int selectionLength = txtTempoAtualizacao.SelectionLength;
                string originalText = txtTempoAtualizacao.Text;
                string newText = originalText.ToUpper();
                if (newText != originalText)
                {
                    short retorno = 0;
                    if (!Int16.TryParse(originalText, out retorno))
                    {
                        if (this.txtTempoAtualizacao.Text.Length == 1)
                            this.txtTempoAtualizacao.Text = string.Empty;

                        txtTempoAtualizacao.Text = originalText.Substring(0, originalText.Length - 1);// newText; // Will cause a new TextChanged event.
                        // Set the selection back *after* the assignment, which has reset them.
                        txtTempoAtualizacao.SelectionStart = selectionStart;
                        txtTempoAtualizacao.SelectionLength = selectionLength;
                    }
                }

                m_InMyTextChanged = false; // Allow it for next time.
            }
            catch (Exception)
            {

            }
        }
    }
}
