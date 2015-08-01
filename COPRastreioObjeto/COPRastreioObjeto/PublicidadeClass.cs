using Business;
using Entities;
using Microsoft.Advertising.WinRT.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COPRastreioObjeto
{
    public class PublicidadeClass
    {
        string nomeClasse = "Publicidade.cs";

        /// <summary>
        /// Monta Publicidade Telas.
        /// </summary>
        /// <returns></returns>
        public async Task<AdControl> MontarPublicidade()
        {
            return await this.MontarPublicidade(Windows.UI.Xaml.HorizontalAlignment.Stretch, true);
        }

        /// <summary>
        /// Monta Publicidade Telas.
        /// </summary>
        /// <returns></returns>
        public async Task<AdControl> MontarPublicidade(Windows.UI.Xaml.HorizontalAlignment posicao, bool preferencia)
        {
            AdControl adControl = new AdControl();

            try
            {
                Preferencias preferencias = await Core.GetSPreferencias();

                adControl.ApplicationId = "1ac1f041-8be6-4aa1-b007-856d2f2589eb";
                adControl.AdUnitId = "132757";
                if(preferencia)
                    adControl.HorizontalAlignment = preferencias.PosicaoPublicidade;
                else
                    adControl.HorizontalAlignment = posicao;

                adControl.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Top;
                adControl.Width = 396;
                adControl.Height = 50;
            }
            catch (Exception ex)
            {
                Core.GravarLog(nomeClasse, "MontaPublicidade", ex);
            }

            return adControl;
        }
    }
}
