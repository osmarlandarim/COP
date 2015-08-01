using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class PoliticaPrivacidade : UserControl
    {
        public PoliticaPrivacidade()
        {
            this.InitializeComponent();

            this.txtPolitica.Text = "Este aplicativo garante ao usuário a\r\nprivacidade de todos os dados, sendo\r\nassim todos os dados enviados e\r\nrecebidos não são armazenados e estão\r\nunicamente em via de acesso do usuário\r\nque o executa.";
            //this.txtPolitica.Text = "Esta política de privacidade cobre o uso\r\ndeste aplicativo.\r\nO aplicativo\r\n'Rastreio de Objeto dos Correios'\r\nnão recolhe, armazena ou compartilha\r\nqualquer informação pessoal ou\r\nqualquer coisa relacionada ao seu\r\ndispositivo.Nós não coletamos quaisquer estatísticas, tendências e nem armazenamos dados de posição do usuário.";
        }
    }
}
