﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.Resources.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using System.Collections.Specialized;
using COPRastreioObjeto.Data;
using COPRastreioObjeto;
using Bing.Maps;
using Entities;
using Entities.Endereco;

// The data model defined by this file serves as a representative example of a strongly-typed
// model that supports notification when members are added, removed, or modified.  The property
// names chosen coincide with data bindings in the standard item templates.
//
// Applications may use this model as a starting point and build on it, or discard it entirely and
// replace it with something appropriate to their needs.

namespace COPRastreioObjeto.Data
{
    /// <summary>
    /// Base class for <see cref="SampleDataItem"/> and <see cref="SampleDataGroup"/> that
    /// defines properties common to both.
    /// </summary>
    [Windows.Foundation.Metadata.WebHostHidden]
    public abstract class SampleDataCommon : COPRastreioObjeto.Common.BindableBase
    {
        private static Uri _baseUri = new Uri("ms-appx:///");

        public SampleDataCommon(String uniqueId, String title, String subtitle, String imagePath, String description)
        {
            this._uniqueId = uniqueId;
            this._title = title;
            this._subtitle = subtitle;
            this._description = description;
            this._imagePath = imagePath;
        }

        public SampleDataCommon(string uniqueId, string title, string subtitle, MyUserControl1 map, string description)
        {
            // TODO: Complete member initialization
            this.UniqueId = uniqueId;
            this.Title = title;
            this.Subtitle = subtitle;
            this.map = map;
            this.Description = description;
        }

        private string _uniqueId = string.Empty;
        public string UniqueId
        {
            get { return this._uniqueId; }
            set { this.SetProperty(ref this._uniqueId, value); }
        }

        private string _title = string.Empty;
        public string Title
        {
            get { return this._title; }
            set { this.SetProperty(ref this._title, value); }
        }

        private string _subtitle = string.Empty;
        public string Subtitle
        {
            get { return this._subtitle; }
            set { this.SetProperty(ref this._subtitle, value); }
        }

        private string _description = string.Empty;
        public string Description
        {
            get { return this._description; }
            set { this.SetProperty(ref this._description, value); }
        }

        private ImageSource _image = null;

        private Map _map = null;

        public Map Map
        {
            get { return _map; }
            set { _map = value; }
        }

        private String _imagePath = null;
        private MyUserControl1 map;
        public ImageSource Image
        {
            get
            {
                if (this._image == null && this._imagePath != null)
                {
                    this._image = new BitmapImage(new Uri(SampleDataCommon._baseUri, this._imagePath));
                }
                return this._image;
            }

            set
            {
                this._imagePath = null;
                this.SetProperty(ref this._image, value);
            }
        }

        public void SetImage(String path)
        {
            this._image = null;
            this._imagePath = path;
            this.OnPropertyChanged("Image");
        }

        public override string ToString()
        {
            return this.Title;
        }
    }

    /// <summary>
    /// Generic item data model.
    /// </summary>
    public class SampleDataItem : SampleDataCommon
    {
        public SampleDataItem(String uniqueId, String title, String subtitle, String imagePath, String description, String content, SampleDataGroup group)
            : base(uniqueId, title, subtitle, imagePath, description)
        {
            this._content = content;
            this._group = group;
        }

        public SampleDataItem(String uniqueId, String title, String subtitle, MyUserControl1 map, String description, String content, SampleDataGroup group)
            : base(uniqueId, title, subtitle, map, description)
        {
            this._content = content;
            this._group = group;
        }

        private string _content = string.Empty;
        public string Content
        {
            get { return this._content; }
            set { this.SetProperty(ref this._content, value); }
        }

        private SampleDataGroup _group;
        public SampleDataGroup Group
        {
            get { return this._group; }
            set { this.SetProperty(ref this._group, value); }
        }
    }

    /// <summary>
    /// Generic group data model.
    /// </summary>
    public class SampleDataGroup : SampleDataCommon
    {
        public SampleDataGroup(String uniqueId, String title, String subtitle, String imagePath, String description)
            : base(uniqueId, title, subtitle, imagePath, description)
        {
            Items.CollectionChanged += ItemsCollectionChanged;
        }

        private void ItemsCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // Provides a subset of the full items collection to bind to from a GroupedItemsPage
            // for two reasons: GridView will not virtualize large items collections, and it
            // improves the user experience when browsing through groups with large numbers of
            // items.
            //
            // A maximum of 12 items are displayed because it results in filled grid columns
            // whether there are 1, 2, 3, 4, or 6 rows displayed

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewStartingIndex < 12)
                    {
                        TopItems.Insert(e.NewStartingIndex, Items[e.NewStartingIndex]);
                        if (TopItems.Count > 12)
                        {
                            TopItems.RemoveAt(12);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Move:
                    if (e.OldStartingIndex < 12 && e.NewStartingIndex < 12)
                    {
                        TopItems.Move(e.OldStartingIndex, e.NewStartingIndex);
                    }
                    else if (e.OldStartingIndex < 12)
                    {
                        TopItems.RemoveAt(e.OldStartingIndex);
                        TopItems.Add(Items[11]);
                    }
                    else if (e.NewStartingIndex < 12)
                    {
                        TopItems.Insert(e.NewStartingIndex, Items[e.NewStartingIndex]);
                        TopItems.RemoveAt(12);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (e.OldStartingIndex < 12)
                    {
                        TopItems.RemoveAt(e.OldStartingIndex);
                        if (Items.Count >= 12)
                        {
                            TopItems.Add(Items[11]);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    if (e.OldStartingIndex < 12)
                    {
                        TopItems[e.OldStartingIndex] = Items[e.OldStartingIndex];
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    TopItems.Clear();
                    while (TopItems.Count < Items.Count && TopItems.Count < 12)
                    {
                        TopItems.Add(Items[TopItems.Count]);
                    }
                    break;
            }
        }

        private ObservableCollection<SampleDataItem> _items = new ObservableCollection<SampleDataItem>();
        public ObservableCollection<SampleDataItem> Items
        {
            get { return this._items; }
        }

        private ObservableCollection<SampleDataItem> _topItem = new ObservableCollection<SampleDataItem>();
        public ObservableCollection<SampleDataItem> TopItems
        {
            get { return this._topItem; }
        }
    }

    /// <summary>
    /// Creates a collection of groups and items with hard-coded content.
    /// 
    /// SampleDataSource initializes with placeholder data rather than live production
    /// data so that sample data is provided at both design-time and run-time.
    /// </summary>
    public sealed class SampleDataSource
    {
        private static SampleDataSource _sampleDataSource = new SampleDataSource();

        private ObservableCollection<SampleDataGroup> _allGroups = new ObservableCollection<SampleDataGroup>();
        public ObservableCollection<SampleDataGroup> AllGroups
        {
            get { return this._allGroups; }
        }

        public static IEnumerable<SampleDataGroup> GetGroups(string uniqueId)
        {
            if (!uniqueId.Equals("AllGroups")) throw new ArgumentException("Only 'AllGroups' is supported as a collection of groups");

            return _sampleDataSource.AllGroups;
        }

        public static SampleDataGroup GetGroup(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.AllGroups.Where((group) => group.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public static SampleDataItem GetItem(string uniqueId)
        {
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.AllGroups.SelectMany(group => group.Items).Where((item) => item.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public SampleDataSource()
        {
            String ITEM_CONTENT = String.Format("Item Content: {0}\n\n{0}\n\n{0}\n\n{0}\n\n{0}\n\n{0}\n\n{0}",
                        "Curabitur class aliquam vestibulum nam curae maecenas sed integer cras phasellus suspendisse quisque donec dis praesent accumsan bibendum pellentesque condimentum adipiscing etiam consequat vivamus dictumst aliquam duis convallis scelerisque est parturient ullamcorper aliquet fusce suspendisse nunc hac eleifend amet blandit facilisi condimentum commodo scelerisque faucibus aenean ullamcorper ante mauris dignissim consectetuer nullam lorem vestibulum habitant conubia elementum pellentesque morbi facilisis arcu sollicitudin diam cubilia aptent vestibulum auctor eget dapibus pellentesque inceptos leo egestas interdum nulla consectetuer suspendisse adipiscing pellentesque proin lobortis sollicitudin augue elit mus congue fermentum parturient fringilla euismod feugiat");

            var group1 = new SampleDataGroup("Group-1",
                    "Meus Objetos",
                    "Cadastrar Objeto",
                    "Assets/DarkGray.png",
                    "Group Description: Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus tempor scelerisque lorem in vehicula. Aliquam tincidunt, lacus ut sagittis tristique, turpis massa volutpat augue, eu rutrum ligula ante a ante");
            group1.Items.Add(new SampleDataItem("Group-1-Item-1",
                    "Item Title: 1",
                    "Item Subtitle: 1",
                    "Assets/LightGray.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    ITEM_CONTENT,
                    group1));
            group1.Items.Add(new SampleDataItem("Group-1-Item-2",
                    "Item Title: 2",
                    "Item Subtitle: 2",
                    "Assets/DarkGray.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    ITEM_CONTENT,
                    group1));
            group1.Items.Add(new SampleDataItem("Group-1-Item-3",
                    "Item Title: 3",
                    "Item Subtitle: 3",
                    "Assets/MediumGray.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    ITEM_CONTENT,
                    group1));
            group1.Items.Add(new SampleDataItem("Group-1-Item-4",
                    "Item Title: 4",
                    "Item Subtitle: 4",
                    "Assets/DarkGray.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    ITEM_CONTENT,
                    group1));
            group1.Items.Add(new SampleDataItem("Group-1-Item-5",
                    "Item Title: 5",
                    "Item Subtitle: 5",
                    "Assets/MediumGray.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    ITEM_CONTENT,
                    group1));
            this.AllGroups.Add(group1);

            var group2 = new SampleDataGroup("Group-2",
                    "Histórico",
                    "Group Subtitle: 2",
                    "Assets/LightGray.png",
                    "Group Description: Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus tempor scelerisque lorem in vehicula. Aliquam tincidunt, lacus ut sagittis tristique, turpis massa volutpat augue, eu rutrum ligula ante a ante");
            group2.Items.Add(new SampleDataItem("Group-2-Item-1",
                    "Item Title: 1",
                    "Item Subtitle: 1",
                    "Assets/DarkGray.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    ITEM_CONTENT,
                    group2));
            group2.Items.Add(new SampleDataItem("Group-2-Item-2",
                    "Item Title: 2",
                    "Item Subtitle: 2",
                    "Assets/MediumGray.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    ITEM_CONTENT,
                    group2));
            group2.Items.Add(new SampleDataItem("Group-2-Item-3",
                    "Item Title: 3",
                    "Item Subtitle: 3",
                    "Assets/LightGray.png",
                    "Item Description: Pellentesque porta, mauris quis interdum vehicula, urna sapien ultrices velit, nec venenatis dui odio in augue. Cras posuere, enim a cursus convallis, neque turpis malesuada erat, ut adipiscing neque tortor ac erat.",
                    ITEM_CONTENT,
                    group2));
            this.AllGroups.Add(group2);
        }
    }
}

public sealed class SampleDataSourceEndereco
{
    private static SampleDataSourceEndereco _sampleDataSource = new SampleDataSourceEndereco();
    private static List<Endereco> _listEndereco;

    private ObservableCollection<SampleDataGroup> _allGroups = new ObservableCollection<SampleDataGroup>();
    public ObservableCollection<SampleDataGroup> AllGroups
    {
        get { return this._allGroups; }
    }

    public static IEnumerable<SampleDataGroup> GetGroups(string uniqueId)
    {
        if (!uniqueId.Equals("AllGroups")) throw new ArgumentException("Only 'AllGroups' is supported as a collection of groups");

        return _sampleDataSource.AllGroups;
    }

    public SampleDataSourceEndereco GetsGroups(string uniqueId, List<Endereco> listEndereco)
    {
        _listEndereco = listEndereco;
        // Simple linear search is acceptable for small data sets


        CarregarItens();
        return _sampleDataSource;
    }

    public SampleDataGroup GetGroup(string uniqueId, List<Endereco> listEndereco)
    {
        _listEndereco = listEndereco;
        // Simple linear search is acceptable for small data sets


        return CarregarItens();
    }

    public SampleDataItem GetItem(string uniqueId)
    {
        // Simple linear search is acceptable for small data sets
        var matches = _sampleDataSource.AllGroups.SelectMany(group => group.Items).Where((item) => item.UniqueId.Equals(uniqueId));
        if (matches.Count() == 1) return matches.First();
        return null;
    }

    /// <summary>
    /// Carregar Enderecos cadastrados.
    /// </summary>
    public SampleDataGroup CarregarItens()
    {
        String ITEM_CONTENT = String.Format("Item Content: {0}\n\n{0}\n\n{0}\n\n{0}\n\n{0}\n\n{0}\n\n{0}",
                    "Endereços");
        SampleDataGroup group1 = new SampleDataGroup("GroupEnderecos",
                "Endereços",
                "Cadastrar Endereço",
                "Assets/Lixeira_Vazia_01_PNG.png",
                "Nosso sistema é um retorno dos dados dos Correios, e para sua segurança os Correios não disponibilizam o endereço final. Neste caso solicitamos seu endereço de entrega para uma melhor precisão na marcação do mapa.");

        if (_listEndereco != null && _listEndereco.Count > 0)
        {
            for (int i = 0; i < _listEndereco.Count; i++)
            {
                group1.Items.Add(new SampleDataItem(_listEndereco[i].Id.ToString(),
                _listEndereco[i].CEP,
                _listEndereco[i].Estado + "\r\n" + _listEndereco[i].Cidade,
                "Assets/endereco.png",
                _listEndereco[i].Rua + " " + _listEndereco[i].Numero + "\r\n" + _listEndereco[i].Bairro,
                ITEM_CONTENT,
                group1));
            }

            //_sampleDataSource._allGroups.Add(group1);
        }

        return group1;
    }

    /// <summary>
    /// Carregar mapa do user control.
    /// </summary>
    /// <returns>Mapa Carregado com as coordenadas do endereço.</returns>
    public MyUserControl1 CarregarMapa()
    {
        MyUserControl1 userMapa = new MyUserControl1();
        userMapa.Height = 5;
        userMapa.Width = 5;

        return userMapa;
    }

    public SampleDataSourceEndereco()
    {

    }
}

//Carregar Dados dos Correios.
public sealed class SampleDataSourceObjetosCorreios
{
    private static SampleDataSourceObjetosCorreios _sampleDataSource = new SampleDataSourceObjetosCorreios();
    private static SROXml _sroXml;
    private static HistoricoSRO _historicoSRO;

    private ObservableCollection<SampleDataGroup> _allGroups = new ObservableCollection<SampleDataGroup>();
    public ObservableCollection<SampleDataGroup> AllGroups
    {
        get { return this._allGroups; }
    }

    public static IEnumerable<SampleDataGroup> GetGroups(string uniqueId)
    {
        if (!uniqueId.Equals("AllGroups")) throw new ArgumentException("Only 'AllGroups' is supported as a collection of groups");

        return _sampleDataSource.AllGroups;
    }

    public IEnumerable<SampleDataGroup> GetsGroups(string uniqueId, SROXml sroXml, HistoricoSRO hitoricoSro)
    {
        _sroXml = sroXml;
        _historicoSRO = hitoricoSro;
        // Simple linear search is acceptable for small data sets


        CarregarAllGroups();
        return _sampleDataSource.AllGroups;
    }

    public SampleDataGroup GetGroup(string uniqueId, SROXml sroXml)
    {
        _sroXml = sroXml;
        // Simple linear search is acceptable for small data sets
        CarregarItens();

        return _allGroups[0];
    }

    public SampleDataGroup GetGroupDefault()
    {

        CarregarGroupDefault();

        return _allGroups[0];
    }

    public SampleDataItem GetItem(string uniqueId)
    {
        // Simple linear search is acceptable for small data sets
        var matches = _sampleDataSource.AllGroups.SelectMany(group => group.Items).Where((item) => item.UniqueId.Equals(uniqueId));
        if (matches.Count() == 1) return matches.First();
        return null;
    }

    public void CarregarGroupDefault()
    {
        var groupSRO = new SampleDataGroup("Correios",
            "Objeto dos Correios",
            "Cadastro de Objetos",
            "",
            "...");

        this.AllGroups.Add(groupSRO);
    }

    /// <summary>
    /// Carregar Todos os Grupos Cadastrados.
    /// </summary>
    public void CarregarAllGroups()
    {
        try
        {
            _sampleDataSource = new SampleDataSourceObjetosCorreios();
            _allGroups.Clear();
            var groupSRO = new SampleDataGroup("Correios",
                Enumeration.Correio.NomeObjetos,
                "Cadastro de Objetos",
                "",
                "...");


            if (_sroXml.Qtd == 0 && (_sroXml.Objetos == null || _sroXml.Objetos.Count == 0))
            {
                groupSRO.Items.Add(new SampleDataItem("",
                        "Nenhum  Objeto Encontrado.",
                        _sroXml.TipoPesquisa,
                        "Assets/InicialEncomenda.png",
                        "Não Encontrado",
                        "\r\nO nosso sistema não possui dados sobre o objeto informado. Se o objeto foi postado recentemente, é natural que seus rastros não tenham ingressado no sistema, nesse caso, por favor, tente novamente mais tarde. Adicionalmente, verifique se o código digitado está correto",
                        groupSRO));
            }
            else
            {
                if (_sroXml.Qtd > 0 && _sroXml.Objetos == null)
                {
                    groupSRO.Items.Add(new SampleDataItem("",
                            "Objeto não Encontrado.",
                            _sroXml.TipoPesquisa,
                            "Assets/correios.png",
                            "Não Encontrado",
                            "\r\nO nosso sistema não possui dados sobre o objeto informado. Se o objeto foi postado recentemente, é natural que seus rastros não tenham ingressado no sistema, nesse caso, por favor, tente novamente mais tarde. Adicionalmente, verifique se o código digitado está correto",
                            groupSRO));
                }
                else
                {
                    if (_sroXml.Objetos != null)
                    {
                        for (int i = 0; i < _sroXml.Objetos.Count; i++)
                        {
                            string imagem = "Assets/correios.png";
                            if (_sroXml.Objetos[i].Eventos != null && _sroXml.Objetos[i].Eventos.Count > 0)
                            {
                                imagem = Status.GetIcoStatus(_sroXml.Objetos[i].Eventos[0].Tipo, Convert.ToInt16(_sroXml.Objetos[i].Eventos[0].Status));
                            }

                            groupSRO.Items.Add(new SampleDataItem(_sroXml.Objetos[i].Numero,
                                _sroXml.Objetos[i].Descricao + "\r\n" + CarregarEventos(_sroXml.Objetos[i].Eventos, false),
                                _sroXml.Objetos[i].Numero,
                                imagem,
                                CarregarEventos(_sroXml.Objetos[i].Eventos, false),
                                CarregarEventos(_sroXml.Objetos[i].Eventos, true),
                                groupSRO));
                        }
                    }
                }
            }
            _sampleDataSource._allGroups.Add(groupSRO);

            var groupPublicidade = new SampleDataGroup("Publicidade",
                 Enumeration.Correio.NomePublicidade,
                 Enumeration.Correio.NomePublicidade,
                 "",
                 "...");

            //Publicidade
            groupPublicidade.Items.Add(new SampleDataItem("Publicidade",
                        "Conheça a Versão PRO deste aplicativo.",
                        "",
                        "Assets/COPPro.png",
                        "",
                        "",
                        groupPublicidade));
            _sampleDataSource._allGroups.Add(groupPublicidade);

            //Qdo implementar essa publicidade comentar linha de cima pra nao repetir
            ////PublicidadePart publicidadePart = new PublicidadePart();
            ////publicidadePart.ObtemPublicidade();
            ////////Publicidade
            ////groupPublicidade.Items.Add(new SampleDataItem("PublicidadePart",
            ////            publicidadePart.Descricao,
            ////            "",
            ////            publicidadePart.Imagem,
            ////            "",
            ////            "",
            ////            groupPublicidade));
            ////_sampleDataSource._allGroups.Add(groupPublicidade);
            //Publicidade

            ///Carregar Historico
            var groupHistorico = new SampleDataGroup("Historico",
               Enumeration.Correio.NomeHistorico,
               Enumeration.Correio.NomeHistorico,
               "",
               "...");


            if (_historicoSRO.ObjetoSRO == null || (_historicoSRO.ObjetoSRO.Objetos == null || _historicoSRO.ObjetoSRO.Objetos.Count == 0))
            {
                groupHistorico.Items.Add(new SampleDataItem("",
                        "O histórico é gerado após 24Hrs que um objeto recebeu o status de Entregue.",
                        "",
                        "Assets/HistoricoAguardando.png",
                        "",
                        "",
                        groupHistorico));
            }
            else
            {
                if (_historicoSRO.ObjetoSRO != null && _historicoSRO.ObjetoSRO.Objetos != null)
                {
                    for (int i = _historicoSRO.ObjetoSRO.Objetos.Count - 1; i >= 0; i--)
                    {
                        string imagem = "Assets/HistoricoCorreios.png";

                        if (_historicoSRO.ObjetoSRO.Objetos[i].Eventos != null && _historicoSRO.ObjetoSRO.Objetos[i].Eventos.Count > 0)
                        {
                            DateTime dataEvento = Convert.ToDateTime(_historicoSRO.ObjetoSRO.Objetos[i].Eventos[0].Data + " " + _historicoSRO.ObjetoSRO.Objetos[i].Eventos[0].Hora);

                            if (dataEvento.AddDays(2) >= DateTime.Now)
                                imagem = "Assets/HistoricoInicial.png";
                        }

                        groupHistorico.Items.Add(new SampleDataItem(_historicoSRO.ObjetoSRO.Objetos[i].Numero,
                            _historicoSRO.ObjetoSRO.Objetos[i].Descricao + "\r\n" + CarregarEventos(_historicoSRO.ObjetoSRO.Objetos[i].Eventos, false),
                            _historicoSRO.ObjetoSRO.Objetos[i].Numero,
                            imagem,
                            CarregarEventos(_historicoSRO.ObjetoSRO.Objetos[i].Eventos, false),
                            CarregarEventos(_historicoSRO.ObjetoSRO.Objetos[i].Eventos, true),
                            groupHistorico));
                    }
                }
            }

            _sampleDataSource._allGroups.Add(groupHistorico);
        }
        catch (Exception ex)
        {
            Entities.LOG.MetroEventSource.Log.Error("SampleDataSource" + "#" + "CarregarAllGroups" + "#" + "ERRO " + ex.Message + ex.Source + " ##FIM");
        }
    }

    /// <summary>
    /// Carregar Objetos cadastrados.
    /// </summary>
    public void CarregarItens()
    {
        //String ITEM_CONTENT = String.Format("Item Content: {0}\n\n{0}\n\n{0}\n\n{0}\n\n{0}\n\n{0}\n\n{0}",
        //                "a");

        try
        {
            var groupSRO = new SampleDataGroup("Correios",
            Enumeration.Correio.NomeObjetos,
            "Cadastro de Objetos",
            "",
            "...");

            if (_sroXml.Objetos == null || (_sroXml.Objetos.Count > 0 && _sroXml.Objetos[0].Eventos != null && _sroXml.Objetos[0].Eventos.Count == 0))
            {
                for (int i = 0; i < _sroXml.Objetos.Count; i++)
                {
                    string content = _sroXml.TipoPesquisa == Enumeration.Correio.Falha ? "\r\n" + _sroXml.TipoPesquisa : "\r\nO nosso sistema não possui dados sobre o objeto informado. Se o objeto foi postado recentemente, é natural que seus rastros não tenham ingressado no sistema, nesse caso, por favor, tente novamente mais tarde. Adicionalmente, verifique se o código digitado está correto";

                    string descricao = string.Empty;
                    string numero = string.Empty;
                    string subHeader = string.Empty;

                    if (content != Enumeration.Correio.Falha)
                    {
                        descricao = _sroXml.Objetos[i].Descricao;
                        numero = _sroXml.Objetos[i].Numero;
                        subHeader = numero;
                    }
                    else
                    {
                        descricao = "Objeto não Encontrado.";
                        numero = "Não Encontrado" + Util.GeraSequenciaNumerica();
                        subHeader = _sroXml.TipoPesquisa;
                    }

                    groupSRO.Items.Add(new SampleDataItem(numero,
                            descricao,
                            subHeader,
                            "Assets/correios.png",
                            numero,
                            content,
                            groupSRO));
                }
            }
            else
            {
                for (int i = 0; i < _sroXml.Objetos.Count; i++)
                {
                    string imagem = "Assets/correios.png";
                    if (_sroXml.Objetos[i].Eventos != null && _sroXml.Objetos[i].Eventos.Count > 0)
                    {
                        imagem = Status.GetIcoStatus(_sroXml.Objetos[i].Eventos[0].Tipo, Convert.ToInt16(_sroXml.Objetos[i].Eventos[0].Status));
                    }

                    SiglasCorreios siglasCorreios = new SiglasCorreios();
                    siglasCorreios.VerificaFormaEnvio(_sroXml.Objetos[i].Numero);

                    groupSRO.Items.Add(new SampleDataItem(_sroXml.Objetos[i].Numero,
                        _sroXml.Objetos[i].Descricao,
                        _sroXml.Objetos[i].Numero + "\r\n" + siglasCorreios.Descricao,
                        imagem,
                        CarregarEventos(_sroXml.Objetos[i].Eventos, false),
                        CarregarEventos(_sroXml.Objetos[i].Eventos, true),
                        groupSRO));
                }
            }

            this.AllGroups.Add(groupSRO);
        }
        catch (Exception ex)
        {
            Entities.LOG.MetroEventSource.Log.Error("SampleDataSource" + "#" + "CarregarItens" + "#" + "ERRO " + ex.Message + ex.Source + " ##FIM");
        }
    }

    /// <summary>
    /// Monta query do Evento e Destino.
    /// </summary>
    /// <param name="evento">Lista de eventos para montar os detalhes do objeto.</param>
    /// <param name="espacoInic">Determina se existe o primeiro espaco. quando a chamada for do grupo.</param>
    /// <returns>String concatenada com os eventos e destino.</returns>
    private string CarregarEventos(List<Evento> evento, bool espacoInic)
    {
        string eventos = string.Empty;
        string destino = string.Empty;
        string cep = string.Empty;

        try
        {
            if (espacoInic)
                eventos = "\r\n";
            foreach (Evento item in evento)
            {
                eventos = eventos + item.Descricao + "\r\nEM " + item.Data + " ÁS " + item.Hora + "\r\n" +
                        item.Local + "-" + item.Cidade + " " + item.UF;

                if (cep == string.Empty)
                {
                    cep = item.Tipo + " CEP: " + item.Codigo;
                    eventos = eventos + "\r\n" + cep;
                }

                //eventos += VerificaDetalhe(item.Tipo, item.Status);

                if (item.Destino != null)
                {
                    destino = item.DestinoEvento();

                    if (destino != string.Empty)
                        destino = "\r\n" + destino + item.Destino.Local + " - " + item.Destino.Bairro + " " + item.Destino.Cidade + " " + item.Destino.Uf;


                    eventos = eventos + destino;
                }

                eventos = eventos + "\r\n\r\n";
            }
        }
        catch (Exception ex)
        {
            Entities.LOG.MetroEventSource.Log.Error("SampleDataSource" + "#" + "CarregarEventos" + "#" + "ERRO " + ex.Message + ex.Source + " ##FIM");
        }

        return eventos;
    }

    /// <summary>
    /// Monta Detalhes.
    /// </summary>
    /// <param name="tipo">Codigo do Correio</param>
    /// <param name="status">Numero de status.</param>
    /// <returns></returns>
    public string VerificaDetalhe(string tipo, string status, ref string imagem)
    {
        string eventos = string.Empty;

        try
        {
            if ((tipo == "BDE" || tipo == "BDR") && status == "46")
            {
                eventos += "\r\n\tA entrega domiciliar não pode ser realizada por motivo de força maior\r\n\tAcompanhar";
            }
            else if ((tipo == "BDE" || tipo == "BDI" || tipo == "BDR") && status == "2")
            {
                eventos += "\r\n\tDestinatário Ausente\r\n\tEncaminhado para entrega interna";
            }
            else if ((tipo == "BDE" || tipo == "BDI" || tipo == "BDR") && status == "4")
            {
                eventos += "\r\n\tEm tratamento, aguarde.\r\n\tAcompanhar";
            }
            else if ((tipo == "BDE" || tipo == "BDI" || tipo == "BDR") && status == "5")
            {
                eventos += "\r\n\tMotivo: falecido\r\n\tAcompanhar o retorno do objeto ao remetente.";
            }
            else if ((tipo == "BDE" || tipo == "BDI" || tipo == "BDR") && status == "6")
            {
                eventos += "\r\n\tEm tratamento, aguarde.\r\n\tAcompanhar";
            }
            else if ((tipo == "BDE" || tipo == "BDI" || tipo == "BDR") && status == "7")
            {
                eventos += "\r\n\tEm tratamento, aguarde.\r\n\tAcompanhar";
            }
            else if ((tipo == "BDE" || tipo == "BDI" || tipo == "BDR") && status == "8")
            {
                eventos += "\r\n\tEm tratamento, aguarde.\r\n\tAcompanhar";
            }
            else if ((tipo == "BDE" || tipo == "BDI" || tipo == "BDR") && status == "9")
            {
                eventos += "\r\n\tConfirmar com a unidade\r\n\tAcionar atendimento dos Correios.";
            }
            else if ((tipo == "BDE" || tipo == "BDI" || tipo == "BDR") && status == "10")
            {
                eventos += "\r\n\tEm tratamento, aguarde. \r\n\tAcompanhar";
            }
            else if ((tipo == "BDE" || tipo == "BDI") && status == "11")
            {
                eventos += "\r\n\tAcompanhar";
            }
            else if ((tipo == "BDE" || tipo == "BDI" || tipo == "BDR") && status == "12")
            {
                eventos += "\r\n\tConsulte a unidade\r\n\tAcionar atendimento dos Correios";
            }
            else if ((tipo == "BDE" || tipo == "BDI" || tipo == "BDR") && status == "19")
            {
                eventos += "\r\n\tPoderá haver atraso ou devolução ao remetente\r\n\tAcompanhar";
            }
            else if ((tipo == "BDE" || tipo == "BDI" || tipo == "BDR") && status == "20")
            {
                eventos += "\r\n\tSerá realizada uma nova tentativa de entrega\r\n\tAcompanhar";
            }
            else if ((tipo == "BDE" || tipo == "BDI" || tipo == "BDR") && status == "21")
            {
                eventos += "\r\n\tO objeto está sendo devolvido ao remetente\r\n\tAcompanhar";
            }
            else if ((tipo == "BDE" || tipo == "BDI" || tipo == "BDR") && (status == "22" || status == "23" || status == "24" || status == "25" || status == "41"))
            {
                eventos += "\r\n\tAcompanhar";
            }
            else if ((tipo == "BDE" || tipo == "BDI" || tipo == "BDR") && status == "26")
            {
                eventos += "\r\n\tDevolvido ao remetente\r\n\tAcompanhar o retorno do objeto ao remetente.";
            }
            else if ((tipo == "BDE" || tipo == "BDI" || tipo == "BDR") && status == "27")
            {
                eventos += "\r\n\tAcompanhar o retorno do objeto ao remetente.";
            }
            else if ((tipo == "BDE" || tipo == "BDI" || tipo == "BDR") && status == "28")
            {
                eventos += "\r\n\tAcionar atendimento dos Correios.";
            }
            else if ((tipo == "BDE" || tipo == "BDI" || tipo == "BDR") && status == "31")
            {
                eventos += "\r\n\tConsultar a unidade\r\n\tAcionar atendimento dos Correios.";
            }
            else if ((tipo == "BDE" || tipo == "BDR") && (status == "32" || status == "33" || status == "34"))
            {
                eventos += "\r\n\tConsultar a unidade\r\n\tAcionar atendimento dos Correios.";
            }
            else if ((tipo == "BDE" || tipo == "BDI" || tipo == "BDR") && status == "35")
            {
                eventos += "\r\n\tNova tentativa\r\n\tAcompanhar";
            }
            else if ((tipo == "BDE" || tipo == "BDI" || tipo == "BDR") && status == "36")
            {
                eventos += "\r\n\tDevolução da Entrega\r\n\tAcompanhar";
            }
            else if (tipo == "BDI" && status == "40")
            {
                eventos += "\r\n\tImportação não autorizada\r\n\tAcompanhar";
            }
            else if ((tipo == "BDE" || tipo == "BDR") && status == "42")
            {
                eventos += "\r\n\tLote incompleto\r\n\tAcompanhar o retorno do objeto ao remetente";
            }
            else if ((tipo == "BDE" || tipo == "BDR") && status == "43")
            {
                eventos += "\r\n\tConsultar a unidade\r\n\tAcionar atendimento dos Correios.";
            }
            else if ((tipo == "BDI" || tipo == "BDR") && status == "44")
            {
                eventos += "\r\n\tConsultar a unidade\r\n\tAcionar atendimento dos Correios.";
            }
            else if ((tipo == "BDE" || tipo == "BDI" || tipo == "BDR") && status == "45")
            {
                eventos += "\r\n\tRecebido na unidade de distribuição\r\n\tAcompanhar";
            }
            else if ((tipo == "BDE" || tipo == "BDR") && status == "47")
            {
                eventos += "\r\n\tFalha no lançamento da encomenda na lista de objetos entregues ao carteiro. A situação será regularizada pelos Correios.\r\n\tAcompanhar";
            }
            else if ((tipo == "BDI" || tipo == "BDR") && status == "48")
            {
                eventos += "\r\n\tEndereço sem distribuição domiciliária e com entrega interna não autorizada pelo remetente.\r\n\tAcompanhar";
            }
            else if ((tipo == "BDE" || tipo == "BDI" || tipo == "BDR") && (status == "50" || status == "51" || status == "52" || status == "69"))
            {
                eventos += "\r\n\tConsultar a unidade\r\n\tAcionar atendimento dos Correios.";
            }
            //else if (tipo == "EST" && status == "1")//////////
            //{
            //    eventos += "\r\n\tEvento errado\r\n\tAcompanhar";
            //}
            //else if (tipo == "FC" && (status == "1" || status == "2" || status == "3"|| status == "4"||status == "5"||status == "6"||status == "7"))
            //{
            //    eventos += "\r\n\tEvento errado\r\n\tAcompanhar";
            //}
        }
        catch (Exception)
        {

        }

        return eventos;
    }

    public SampleDataSourceObjetosCorreios()
    {

    }
}
