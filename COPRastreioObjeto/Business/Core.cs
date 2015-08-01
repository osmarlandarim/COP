using Entities;
using Entities.Endereco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    /// <summary>
    /// Kernel.
    /// Metodos estaticos para consultas em todas as partes do sistema
    /// </summary>
    public static class Core
    {
        //Consultas
        /// <summary>
        /// Carrega todos os Estados de um país
        /// </summary>
        /// <returns>Retorna lista de estados</returns>
        public static List<Estado> GetEstados()
        {
            using (Estado objEstado = new Estado())
            {
                return objEstado.GetEstado(1); //Id País 1 = Brasil
            }
        }

        /// <summary>
        /// Carrega todas as cidades de um estado
        /// </summary>
        /// <param name="IdEstado">ID do estado</param>
        /// <returns>Retorna a lista de todas as cidades</returns>
        public static List<Cidade> GetCidades(int IdEstado)
        {
            using (Cidade objCidade = new Cidade())
            {
                return objCidade.GetCidade(IdEstado);
            }
        }

        /// <summary>
        /// Asincrono. Deve analisar retorno!
        /// Retorna todos os endereços de entrega cadastrados pelo usuário
        /// </summary>
        /// <returns>retorna uma lista de endereço</returns>
        public static async Task<List<Endereco>> GetEnderecos()
        {
            using (Endereco objEndereco = new Endereco())
            {
                return await objEndereco.GetEnderecos();
            }
        }

        /// <summary>
        /// Retorna o endereco pelo id.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static async Task<Endereco> GetEnderecos(int Id)
        {
            using (Endereco objEndereco = new Endereco())
            {
                List<Endereco> retorno = await objEndereco.GetEnderecos();

                if (retorno != null)
                {
                    var endereco = retorno.Find((x) => x.Id.Equals(Id));
                    return endereco;
                }
                else
                    return new Endereco();
            }
        }

        /// <summary>
        /// Busca obj por codigo. Clique detalhe.
        /// </summary>
        /// <param name="codigoRastreio"></param>
        /// <returns>Sro com objeto unico.</returns>
        public static async Task<SROXml> GetSROXml(string codigoRastreio)
        {
            using (SROXml sroXml = new SROXml())
            {
                try
                {
                    SROXml sro = await sroXml.GetSROXml();
                    if (sro != null)
                    {
                        var retorno = sro.Objetos.Find((x) => x.Numero.Equals(codigoRastreio));

                        SROXml sroRetorno = new SROXml();
                        sroRetorno.Objetos = new List<Objeto>();
                        sroRetorno.Objetos.Add(retorno);

                        return sroRetorno;
                    }
                    else
                        return new SROXml();
                }
                catch (Exception ex)
                {
                    GravarLog("Core", "GetSROXml", ex);
                }

                return new SROXml();
            }
        }

        /// <summary>
        /// Asincrono. Deve analisar retorno!
        /// Retorna todo SROXml cadastrados pelo usuário.
        /// </summary>
        /// <returns>retorna um objeto SROXml.</returns>
        public static async Task<SROXml> GetSROXml()
        {
            using (SROXml objSROXml = new SROXml())
            {
                return await objSROXml.GetSROXml();
            }
        }

        /// <summary>
        /// Asincrono. Deve analisar retorno!
        /// Retorna todo preferencias cadastrados pelo usuário.
        /// </summary>
        /// <returns>retorna um objeto Preferencias do Usuario.</returns>
        public static async Task<Preferencias> GetSPreferencias()
        {
            using (Preferencias objSPreferencias = new Preferencias())
            {
                Preferencias preferenciaEncontrada = await objSPreferencias.GetPreferencias();

                if (preferenciaEncontrada == null || !preferenciaEncontrada.Criado)
                {
                    var sucesso = await Core.CriarPreferenciasPadrao();
                    if (sucesso)
                    {
                        return await Core.GetSPreferencias();
                    }
                }

                return preferenciaEncontrada;
            }
        }

        /// <summary>
        /// Criar preferencias padrao.
        /// </summary>
        /// <returns>Sucesso.</returns>
        public static async Task<bool> CriarPreferenciasPadrao()
        {
            using (Preferencias preferencias = new Preferencias())
            {
                try
                {
                    preferencias.AtualizacaoAutomatica = true;
                    preferencias.TempoAtualizacao = 2;
                    preferencias.Criado = true;
                    preferencias.PosicaoPublicidade = Windows.UI.Xaml.HorizontalAlignment.Center;

                    return await GravarPreferencias(preferencias);
                }
                catch (Exception ex)
                {
                    Core.GravarLog("Core", "CriarPreferenciasPadrao", ex);

                    return false;
                }
            }
        }

        /// <summary>
        /// Gravar Preferencias do usuario.
        /// </summary>
        /// <param name="preferencias"></param>
        /// <returns>Gravado com sucesso.</returns>
        public static async Task<bool> GravarPreferencias(Preferencias preferencias)
        {
            return await preferencias.GravarPreferencias();
        }

        /// <summary>
        /// Asincrono. Deve analisar retorno!
        /// Retorna todo Historico salvo pelo sistema.
        /// </summary>
        /// <returns>retorna um objeto HistoricoSROXml.</returns>
        public static async Task<HistoricoSRO> GetHistorico()
        {
            using (HistoricoSRO objHistorico = new HistoricoSRO())
            {
                return await objHistorico.GetSHistorico();
            }
        }

        /// <summary>
        /// Retorna o objeto pelo numero.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static async Task<SROXml> GetHistorico(string numero)
        {
            using (HistoricoSRO objHistorico = new HistoricoSRO())
            {
                try
                {
                    SROXml sroXmlRetorno = new SROXml();
                    HistoricoSRO historico = await objHistorico.GetSHistorico();
                    if (historico != null && historico.ObjetoSRO != null)
                    {
                        var objeto = historico.ObjetoSRO.Objetos.Find((x) => x.Numero.Equals(numero));
                        if (objeto != null)
                        {
                            sroXmlRetorno.Objetos = new List<Objeto>();
                            sroXmlRetorno.Objetos.Add(objeto);
                        }
                    }

                    return sroXmlRetorno;
                }
                catch (Exception ex)
                {
                    Core.GravarLog("Core", "GetHistorico", ex);
                    return new SROXml();
                }
            }
        }

        //Persistências
        /// <summary>
        /// Grava o endereço informado pelo usuário. Asincrono.
        /// O retorno deve ser tratado antes de continuar os processos!
        /// </summary>
        /// <param name="estado">objeto completo do estado</param>
        /// <param name="cidade">objeto completo da cidade</param>
        /// <param name="intNumero">número da casa</param>
        /// <param name="strLogradouro">rua da casa</param>
        /// <param name="strBairro">bairro onde está a casa</param>
        /// <param name="strComplemento">complemento (ex: Ap, Num. Casa, etc)</param>
        /// <param name="strCEP">CEP do endereço</param>
        /// <returns>retorna verdadeiro para sucesso, falso em caso de qualquer erro</returns>
        public static async Task<bool> GravarEndereco(string estado, string cidade, int? intNumero, string strLogradouro, string strBairro, string strComplemento, string strCEP)
        {
            //preencher o endereço
            using (Endereco objEndereco = new Endereco())
            {
                try
                {
                    objEndereco.Cidade = cidade;
                    objEndereco.Estado = estado;
                    objEndereco.Numero = intNumero;
                    objEndereco.Rua = strLogradouro;
                    objEndereco.CEP = strCEP;
                    objEndereco.Bairro = strBairro;
                    objEndereco.Complemento = strComplemento;
                    objEndereco.BuscaGeoLocalizacaoEndereco();
                    //asincrono
                    return await objEndereco.GravaEndereco();
                }
                catch (Exception ex)
                {
                    Core.GravarLog("Core", "GravarEndereco", ex);
                    return false;
                }
            }
        }

        /// <summary>
        /// Gravar Historico dos objetos.
        /// </summary>
        /// <returns></returns>
        public static async Task<bool> GravarObjetoHistorico()
        {
            var sucessoOnLine = false;
            SROXml sroObjeto = null;
            List<Objeto> listExcluidos = null;
            HistoricoSRO historico = null;

            try
            {
                sroObjeto = await GetSROXml();

                if (sroObjeto.Objetos == null)
                    return sucessoOnLine;

                foreach (Objeto objetoLocal in sroObjeto.Objetos)
                {
                    if (objetoLocal.Eventos != null && objetoLocal.Eventos.Count > 0)
                    {
                        if (objetoLocal.Eventos[0].Codigo != null)
                        {
                            DateTime dataEvento = new DateTime(Convert.ToInt16(objetoLocal.Eventos[0].Data.Substring(6, 4)), Convert.ToInt16(objetoLocal.Eventos[0].Data.Substring(3, 2)), Convert.ToInt16(objetoLocal.Eventos[0].Data.Substring(0, 2)), Convert.ToInt16(objetoLocal.Eventos[0].Hora.Substring(0, 2)), Convert.ToInt16(objetoLocal.Eventos[0].Hora.Substring(3, 2)), 0);

                            if (Status.VerificarStatus(objetoLocal.Eventos[0].Tipo, Convert.ToInt16(objetoLocal.Eventos[0].Status))//objetoLocal.Eventos[0].Descricao == Entities.Enumeration.DescricaoEvento.Entregue
                                && dataEvento.AddDays(1) < DateTime.Now)
                            {
                                if (listExcluidos == null)
                                {
                                    listExcluidos = new List<Objeto>();
                                }

                                listExcluidos.Add(objetoLocal);
                            }
                        }
                    }
                }

                if (listExcluidos != null && listExcluidos.Count > 0)
                {
                    historico = await GetHistorico();

                    if (historico != null)
                    {
                        foreach (Objeto item in listExcluidos)
                        {
                            sroObjeto.Objetos.Remove(item);
                            sroObjeto.Qtd--;
                            item.DataFim = new DateTime(Convert.ToInt16(item.Eventos[0].Data.Substring(6, 4)), Convert.ToInt16(item.Eventos[0].Data.Substring(3, 2)), Convert.ToInt16(item.Eventos[0].Data.Substring(0, 2)), Convert.ToInt16(item.Eventos[0].Hora.Substring(0, 2)), Convert.ToInt16(item.Eventos[0].Hora.Substring(3, 2)), 0);
                            if (historico.ObjetoSRO == null)
                                historico.ObjetoSRO = new SROXml();
                            if (historico.ObjetoSRO.Objetos == null)
                                historico.ObjetoSRO.Objetos = new List<Objeto>();

                            historico.ObjetoSRO.Objetos.Add(item);
                        }

                        historico.ObjetoSRO.UltimaAtualizacao = DateTime.Now;
                    }

                    var sucesso = await historico.GravarCorreioOnLine();

                    if (sucesso)
                    {
                        sucessoOnLine = await sroObjeto.GravarCorreioOnLine();

                        if (sucessoOnLine)
                        {

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Core.GravarLog("Core", "GravarObjetoHistorico", ex);
            }
            finally
            {
                if (sroObjeto != null)
                {
                    sroObjeto.Dispose();
                    sroObjeto = null;
                }

                if (listExcluidos != null)
                {
                    listExcluidos.Clear();
                    listExcluidos = null;
                }

                if (historico != null)
                {
                    historico.Dispose();
                    historico = null;
                }
            }

            return sucessoOnLine;
        }

        /// <summary>
        /// Gravar os objetos dos correios em disco.
        /// </summary>
        /// <param name="codigoRastreio"></param>
        /// <param name="descricao"></param>
        /// <param name="idEndereco"></param>
        /// <returns></returns>
        public static async Task<bool> GravarObjetoCorreio(string codigoRastreio, string descricao, int idEndereco)
        {
            var sucesso = false;
            try
            {
                using (SROXml sroXml = new SROXml())
                {
                    using (Objeto objeto = new Objeto())
                    {
                        objeto.Numero = codigoRastreio;
                        objeto.Descricao = descricao;
                        objeto.DataInicio = DateTime.Now;
                        objeto.DataFim = DateTime.MinValue;
                        objeto.IdEndereco = idEndereco;

                        sroXml.Objetos = new List<Objeto>();
                        sroXml.Objetos.Add(objeto);

                        sucesso = await sroXml.GravarCorreioOffLine();

                        if (sucesso)
                        {

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Core.GravarLog("Core", "GravarObjetoCorreio", ex);
                return false;
            }

            return sucesso;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="codigosRastreio"></param>
        /// <param name="resultado"></param>
        /// <returns>T-> Serão retornados todos os Eventos, U -> Sera retornado apenas o ultimo Evento.</returns>
        public async static Task<SROXml> GetXmlCorreios(string codigosRastreio, Enumeration.Resultado resultado)
        {
            List<string> listCodigo = new List<string>();
            listCodigo.Add(codigosRastreio);

            return await GetXmlCorreios(listCodigo, resultado);
        }


        /// <summary>
        /// Carregar os dados dos correios e descricao do XML Local.
        /// </summary>
        /// <param name="listCodigosRastreio"></param>
        /// <param name="resultado">T-> Serão retornados todos os Eventos, U -> Sera retornado apenas o ultimo Evento.</param>
        /// <returns></returns>
        public async static Task<SROXml> GetXmlCorreios(List<string> listCodigosRastreio, Enumeration.Resultado resultado)
        {
            try
            {
                Correio correio = new Correio();
                SROXml sroXml = correio.BuscaObjetosOnline(listCodigosRastreio, resultado);

                if (sroXml != null && sroXml.Objetos != null)
                {
                    SROXml sroXmlDescricaoObjeto = await Core.GetSROXml();
                    if (sroXmlDescricaoObjeto != null)
                    {
                        //Alterar o forech para linqs.
                        foreach (Objeto itemOnLine in sroXml.Objetos)
                        {
                            foreach (Objeto itemDisco in sroXmlDescricaoObjeto.Objetos)
                            {
                                if (itemOnLine.Eventos != null && itemOnLine.Eventos.Count > 0)
                                {
                                    foreach (Evento eventoCoord in itemOnLine.Eventos)
                                    {
                                        if (eventoCoord.GeoX == 0 || eventoCoord.GeoY == 0)
                                            eventoCoord.BuscaGeoLocalizacaoEvento();
                                    }
                                }

                                if (itemOnLine.Numero == itemDisco.Numero)
                                {
                                    itemOnLine.Descricao = itemDisco.Descricao;
                                    itemOnLine.IdEndereco = itemDisco.IdEndereco;
                                    break;
                                }
                            }
                        }
                    }
                }

                return sroXml;
            }
            catch (Exception ex)
            {
                Core.GravarLog("Core", "GerXmlCorreios", ex);
                return new SROXml();
            }

        }

        /// <summary>
        /// Atualizar conforme preferencias.
        /// </summary>
        /// <returns></returns>
        public async static Task<SROXml> AtualizacaoAutomatica()
        {
            Preferencias preferecias = await GetSPreferencias();
            if (preferecias == null)
                preferecias = new Preferencias();

            return await AtualizacaoAutomatica(preferecias.TempoAtualizacao);
        }

        /// <summary>
        /// Atualizar automaticamente com valor estipulado.
        /// </summary>
        /// <returns>Objeto atualizado.</returns>
        public async static Task<SROXml> AtualizacaoAutomatica(int tempoAtualizacao)
        {
            try
            {
                SROXml sroDisco = await GetSROXml();
                //if (sroDisco == null)
                //    sroDisco = new SROXml();

                if (tempoAtualizacao == 0)
                    return sroDisco;

                if (DateTime.Now >= sroDisco.UltimaAtualizacao.AddHours(tempoAtualizacao))
                {
                    sroDisco.UltimaAtualizacao = DateTime.Now.AddHours(tempoAtualizacao);

                    var gravouDisco = await sroDisco.GravarCorreioOnLine();

                    if (gravouDisco)
                    {

                    }

                    var atualizouCorreios = await Core.AtualizarObjetosDisco();

                    if (atualizouCorreios)
                    {

                    }
                }

                sroDisco.Dispose();
                sroDisco = null;

                sroDisco = await GetSROXml();

                return sroDisco;
            }
            catch (Exception ex)
            {
                Core.GravarLog("Core", "AtualizacaoAutomatica", ex);
                return new SROXml();
            }
        }

        /// <summary>
        /// Excluir Objeto ou Historico. Obs se remover direto o objeto ele nao encontra. Precisa percorrer e remover dele mesmo.
        /// </summary>
        /// <param name="objetoExcluir"></param>
        /// <returns></returns>
        public async static Task<bool> ExcluirObjeto(Objeto objetoExcluir)
        {
            SROXml sroXml = null;
            SROXml sroExclusao = null;

            try
            {
                var sucesso = false;
                sroXml = await GetSROXml(objetoExcluir.Numero);

                if (sroXml == null)
                    sroXml = new SROXml();

                if (sroXml != null && sroXml.Objetos[0] == null)
                {
                    using (HistoricoSRO historico = await GetHistorico())
                    {
                        foreach (Objeto item in historico.ObjetoSRO.Objetos)
                        {
                            if (item.Numero == objetoExcluir.Numero)
                            {
                                historico.ObjetoSRO.Objetos.Remove(item);
                                break;
                            }
                        }

                        sucesso = await historico.GravarCorreioOnLine();

                        if (sucesso)
                        {

                        }
                    }
                }
                else
                {
                    sroExclusao = await GetSROXml();
                    if (sroExclusao == null)
                        sroExclusao = new SROXml();

                    foreach (Objeto item in sroExclusao.Objetos)
                    {
                        if (item.Numero == objetoExcluir.Numero)
                        {
                            sroExclusao.Objetos.Remove(item);
                            sroExclusao.Qtd--;
                            break;
                        }
                    }

                    sucesso = await sroExclusao.GravarCorreioOnLine();
                    if (sucesso)
                    {

                    }
                }

                return sucesso;
            }
            catch (Exception ex)
            {
                Core.GravarLog("Core", "ExcluirObjeto", ex);
                return false;
            }
            finally
            {
                if (sroXml != null)
                {
                    sroXml.Dispose();
                    sroXml = null;
                }

                if (sroExclusao != null)
                {
                    sroExclusao.Dispose();
                    sroExclusao = null;
                }
            }
        }

        /// <summary>
        /// Atualizar a lista de objetos gravados em disco.
        /// </summary>
        /// <param name="listCodigosRastreio"></param>
        /// <returns></returns>
        public async static Task<bool> AtualizarObjetosDisco()
        {
            //muda a posicao da publicidade.
            Preferencias preferencias = await GetSPreferencias();
            switch (preferencias.PosicaoPublicidade)
            {
                case Windows.UI.Xaml.HorizontalAlignment.Center:
                    preferencias.PosicaoPublicidade = Windows.UI.Xaml.HorizontalAlignment.Right;
                    break;
                case Windows.UI.Xaml.HorizontalAlignment.Left:
                    preferencias.PosicaoPublicidade = Windows.UI.Xaml.HorizontalAlignment.Center;
                    break;
                case Windows.UI.Xaml.HorizontalAlignment.Right:
                    preferencias.PosicaoPublicidade = Windows.UI.Xaml.HorizontalAlignment.Left;
                    break;
                default:
                    break;
            }
            var gravouPref = await GravarPreferencias(preferencias);

            if (gravouPref)
            {

            }

            //Corrigir método, pois tem que ser atualizado do disco a partir do correio, mas nao pode perder os que ainda nao foram mostrados nos correios.
            var sucesso = false;
            SROXml sroXmlLocal = null;
            SROXml sroXml = null;

            try
            {
                sroXmlLocal = await Core.GetSROXml();

                List<string> listCodigoRastreio = new List<string>();

                if (sroXmlLocal != null && sroXmlLocal.Objetos != null)
                {
                    foreach (Objeto item in sroXmlLocal.Objetos)
                    {
                        listCodigoRastreio.Add(item.Numero);
                    }
                }

                if (listCodigoRastreio.Count > 0)
                {
                    Correio correios = new Correio();
                    sroXml = correios.BuscaObjetosOnline(listCodigoRastreio, Enumeration.Resultado.T);

                    List<string> listContain = null;
                    List<string> listNaoContain = null;

                    if (sroXml != null && sroXml.Objetos != null && (listCodigoRastreio.Count != sroXml.Objetos.Count))
                    {
                        listContain = new List<string>();
                        listNaoContain = new List<string>();

                        foreach (var item in sroXml.Objetos)
                        {
                            listContain.Add(item.Numero);
                        }

                        for (int i = 0; i < listCodigoRastreio.Count; i++)
                        {
                            if (!listContain.Contains(listCodigoRastreio[i]))
                                listNaoContain.Add(listCodigoRastreio[i]);
                        }

                    }

                    if (sroXml != null && sroXml.Objetos != null)
                    {
                        //Alterar o forech para linqs.
                        foreach (Objeto itemOnLine in sroXml.Objetos)
                        {
                            foreach (Objeto itemDisco in sroXmlLocal.Objetos)
                            {
                                if (itemOnLine.Numero == itemDisco.Numero)
                                {
                                    if (itemOnLine.Eventos != null && itemOnLine.Eventos.Count > 0)
                                    {
                                        foreach (Evento eventoCoord in itemOnLine.Eventos)
                                        {
                                            if (eventoCoord.GeoX == 0 || eventoCoord.GeoY == 0)
                                                eventoCoord.BuscaGeoLocalizacaoEvento();
                                        }
                                    }

                                    itemOnLine.Descricao = itemDisco.Descricao;
                                    itemOnLine.IdEndereco = itemDisco.IdEndereco;
                                    break;
                                }
                            }
                        }

                        if (listNaoContain != null)
                        {
                            for (int i = 0; i < listNaoContain.Count; i++)
                            {
                                foreach (Objeto item in sroXmlLocal.Objetos)
                                {
                                    if (listNaoContain[i] == item.Numero)
                                    {
                                        Evento ev = new Evento();
                                        ev.Descricao = "O nosso sistema não possui dados sobre o objeto informado. Se o objeto foi postado recentemente, é natural que seus rastros não tenham ingressado no sistema, nesse caso, por favor, tente novamente mais tarde. Adicionalmente, verifique se o código digitado está correto";
                                        item.Eventos = new List<Evento>();
                                        item.Eventos.Add(ev);
                                        sroXml.Objetos.Add(item);
                                        break;
                                    }
                                }
                            }
                        }

                        sroXml.UltimaAtualizacao = DateTime.Now;
                        sucesso = await sroXml.GravarCorreioOnLine();

                        if (sucesso)
                        {

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Core.GravarLog("Core", "AtualizarObjetosDisco", ex);
            }
            finally
            {
                if (sroXmlLocal != null)
                {
                    sroXmlLocal.Dispose();
                    sroXmlLocal = null;
                }

                if (sroXml != null)
                {
                    sroXml.Dispose();
                    sroXml = null;
                }
            }

            return sucesso;
        }

        /// <summary>
        /// Gravar log.
        /// </summary>
        /// <param name="classe">Nome do arquivo cs.</param>
        /// <param name="metodo">Metodo do log.</param>
        /// <param name="ex">Exception gerada.</param>
        public static void GravarLog(string classe, string metodo, Exception ex)
        {
            Entities.LOG.MetroEventSource.Log.Error(classe + "#" + metodo + "#" + "ERRO " + ex.Message + ex.Source + " ##FIM");
        }
    }
}
