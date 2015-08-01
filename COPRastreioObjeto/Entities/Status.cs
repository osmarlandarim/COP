using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Status
    {
        public static bool VerificarStatus(string tipo, int status)
        {
            bool entregue = false;

            GetIcoStatus(tipo,
                (Enumeration.Status_BDE_BDI_BDR)status, (Entities.Enumeration.Status_CAR_CD_CMR_CO_CUN)status,
                (Entities.Enumeration.Status_DO)status, (Entities.Enumeration.Status_EST)status,
                (Entities.Enumeration.Status_FC)status, (Entities.Enumeration.Status_IDC)status,
                (Entities.Enumeration.Status_IE)status, (Entities.Enumeration.Status_IT)status,
                (Entities.Enumeration.Status_LDI)status, (Entities.Enumeration.Status_OEC)status,
                (Entities.Enumeration.Status_PAR)status, (Entities.Enumeration.Status_PMT)status,
                (Entities.Enumeration.Status_PO)status, (Entities.Enumeration.Status_RO)status,
                (Entities.Enumeration.Status_TR)status, ref entregue);

            return entregue;
        }


        public static string GetIcoStatus(string tipo, int status)
        {
            bool entregue = false;
            string imagem = GetIcoStatus(tipo,
                (Enumeration.Status_BDE_BDI_BDR)status, (Entities.Enumeration.Status_CAR_CD_CMR_CO_CUN)status,
                (Entities.Enumeration.Status_DO)status, (Entities.Enumeration.Status_EST)status,
                (Entities.Enumeration.Status_FC)status, (Entities.Enumeration.Status_IDC)status,
                (Entities.Enumeration.Status_IE)status, (Entities.Enumeration.Status_IT)status,
                (Entities.Enumeration.Status_LDI)status, (Entities.Enumeration.Status_OEC)status,
                (Entities.Enumeration.Status_PAR)status, (Entities.Enumeration.Status_PMT)status,
                (Entities.Enumeration.Status_PO)status, (Entities.Enumeration.Status_RO)status,
                (Entities.Enumeration.Status_TR)status, ref entregue);

            if (imagem == string.Empty)
                imagem = "Assets/correios.png";

            return imagem;
        }

        private static string GetIcoStatus(string tipo, Enumeration.Status_BDE_BDI_BDR status_BDE_BDI_BDR, Entities.Enumeration.Status_CAR_CD_CMR_CO_CUN status_CAR_CD_CMR_CO_CUN,
            Entities.Enumeration.Status_DO status_DO, Entities.Enumeration.Status_EST status_EST, Entities.Enumeration.Status_FC status_FC,
            Entities.Enumeration.Status_IDC status_IDC, Entities.Enumeration.Status_IE status_IE, Entities.Enumeration.Status_IT status_IT,
            Entities.Enumeration.Status_LDI status_LDI, Entities.Enumeration.Status_OEC status_OEC,
            Entities.Enumeration.Status_PAR status_PAR, Entities.Enumeration.Status_PMT status_PMT,
            Entities.Enumeration.Status_PO status_PO, Entities.Enumeration.Status_RO status_RO, Entities.Enumeration.Status_TR status_TR, ref bool entregue)
        {
            string imagem = string.Empty;

            switch (tipo)
            {
                case "BDE":
                case "BDI":
                case "BDR":
                    imagem = VerificarICO_BDE_BDI_BDR(status_BDE_BDI_BDR, ref entregue);
                    break;

                case "CAR":
                case "CD":
                case "CMR":
                case "CO":
                case "CUN":
                    imagem = VerificarICO_CAR_CD_CMR_CO_CUN(status_CAR_CD_CMR_CO_CUN);
                    break;
                case "DO":
                    imagem = VerificarICO_DO(status_DO);
                    break;
                case "EST":
                    imagem = VerificarICO_EST(status_EST);
                    break;
                case "FC":
                    imagem = VerificarICO_FC(status_FC);
                    break;
                case "IDC":
                    imagem = VerificarICO_IDC(status_IDC);
                    break;
                case "IE":
                    imagem = VerificarICO_IE(status_IE);
                    break;
                case "IT":
                    imagem = VerificarICO_IT(status_IT);
                    break;
                case "LDI":
                    imagem = VerificarICO_LDI(status_LDI);
                    break;
                case "OEC":
                    imagem = VerificarICO_OEC(status_OEC);
                    break;
                case "PAR":
                    imagem = VerificarICO_PAR(status_PAR);
                    break;
                case "PMT":
                    imagem = VerificarICO_PMT(status_PMT);
                    break;
                case "PO":
                    imagem = VerificarICO_PO(status_PO);
                    break;
                case "RO":
                    imagem = VerificarICO_RO(status_RO);
                    break;
                case "TR":
                    imagem = VerificarICO_TR(status_TR);
                    break;
                default:
                    break;
            }

            return imagem;
        }

        private static string VerificarICO_BDE_BDI_BDR(Enumeration.Status_BDE_BDI_BDR status, ref bool entregue)
        {
            string imagem = string.Empty;
            switch (status)
            {
                case Enumeration.Status_BDE_BDI_BDR.Entregue:
                    entregue = true;
                    imagem = "Assets/Entregue.png";
                    break;
                case Enumeration.Status_BDE_BDI_BDR.Destinatario_Ausente:
                    imagem = "Assets/DestinatarioAusente.png";
                    break;
                case Enumeration.Status_BDE_BDI_BDR.Nao_Procurado:
                case Enumeration.Status_BDE_BDI_BDR.Recusado:
                    imagem = "Assets/ProblemaEncomenda.png";
                    break;
                case Enumeration.Status_BDE_BDI_BDR.Em_Devolucao:
                    break;
                case Enumeration.Status_BDE_BDI_BDR.Destinatario_Desconhecido_Endereco:
                case Enumeration.Status_BDE_BDI_BDR.Endereco_Insuficiente_Para_Entrega:
                case Enumeration.Status_BDE_BDI_BDR.Nao_Existe_Numero_Indicado:
                    imagem = "Assets/DestinatarioAusente.png";
                    break;
                case Enumeration.Status_BDE_BDI_BDR.Extraviado:
                    imagem = "Assets/ProblemaEncomenda.png";
                    break;
                case Enumeration.Status_BDE_BDI_BDR.Destinatario_Mudou_Se:
                    imagem = "Assets/DestinatarioAusente.png";
                    break;
                case Enumeration.Status_BDE_BDI_BDR.Outros:
                    break;
                case Enumeration.Status_BDE_BDI_BDR.Refugado:
                    imagem = "Assets/ProblemaEncomenda.png";
                    break;
                case Enumeration.Status_BDE_BDI_BDR.Endereco_Incorreto:
                case Enumeration.Status_BDE_BDI_BDR.Destinatario_Ausentes:
                case Enumeration.Status_BDE_BDI_BDR.Destinatario_Ausentess:
                    imagem = "Assets/DestinatarioAusente.png";
                    break;
                case Enumeration.Status_BDE_BDI_BDR.Reintegrado:
                    break;
                case Enumeration.Status_BDE_BDI_BDR.Distribuido_Remetente:
                    break;
                case Enumeration.Status_BDE_BDI_BDR.Disponivel_Caixa_Postal:
                    imagem = "Assets/DestinatarioAusente.png";
                    break;
                case Enumeration.Status_BDE_BDI_BDR.Empresa_Sem_Expediente:
                    imagem = "Assets/DestinatarioAusente.png";
                    break;
                case Enumeration.Status_BDE_BDI_BDR.NaoProcurado:
                    imagem = "Assets/ProblemaEncomenda.png";
                    break;
                case Enumeration.Status_BDE_BDI_BDR.Pedido_Nao_Solicitado:
                    imagem = "Assets/ProblemaEncomenda.png";
                    break;
                case Enumeration.Status_BDE_BDI_BDR.MercadoriaAvariada:
                    imagem = "Assets/ProblemaEncomenda.png";
                    break;
                case Enumeration.Status_BDE_BDI_BDR.Extraviados:
                    imagem = "Assets/ProblemaEncomenda.png";
                    break;
                case Enumeration.Status_BDE_BDI_BDR.Entrega_Programada:
                    break;
                case Enumeration.Status_BDE_BDI_BDR.Documentacao_Nao_Fornecida_Pelo_Destinatário:
                    imagem = "Assets/ProblemaEncomenda.png";
                    break;
                case Enumeration.Status_BDE_BDI_BDR.Logradouro_Com_Numeracao_Irregular_Em_Pesquisa:
                    imagem = "Assets/ProblemaEncomenda.png";
                    break;
                case Enumeration.Status_BDE_BDI_BDR.Log_Reversa_Simultânea:
                    break;
                case Enumeration.Status_BDE_BDI_BDR.Log_Reversa_Simultâneas:
                    break;
                case Enumeration.Status_BDE_BDI_BDR.Devolvido_Remetente:
                    break;
                case Enumeration.Status_BDE_BDI_BDR.Aguardando_Parte_Lote:
                    break;
                case Enumeration.Status_BDE_BDI_BDR.Devolvido_Remetentes:
                    break;
                case Enumeration.Status_BDE_BDI_BDR.Objeto_Apreendido_Por_Autoridade_Competente:
                case Enumeration.Status_BDE_BDI_BDR.Falta_Documento_Para_Liberacao_Para_Retirada_Interna:
                    imagem = "Assets/ProblemaEncomenda.png";
                    break;
                case Enumeration.Status_BDE_BDI_BDR.Residuo_Mesa:
                    break;
                case Enumeration.Status_BDE_BDI_BDR.Entrega_Nao_Efetuada:
                    imagem = "Assets/ProblemaEncomenda.png";
                    break;
                case Enumeration.Status_BDE_BDI_BDR.Erro_Lancamento:
                    imagem = "Assets/ProblemaEncomenda.png";
                    break;
                case Enumeration.Status_BDE_BDI_BDR.Posta_Restante_Nao_Autorizada:
                case Enumeration.Status_BDE_BDI_BDR.Roubo_A_Carteiro:
                case Enumeration.Status_BDE_BDI_BDR.Roubo_A_Veículo:
                case Enumeration.Status_BDE_BDI_BDR.Roubo_A_Unidade:
                    imagem = "Assets/ProblemaEncomenda.png";
                    break;
                case Enumeration.Status_BDE_BDI_BDR.Extraviadoss:
                    imagem = "Assets/DestinatarioAusente.png";
                    break;
                default:
                    break;
            }

            return imagem;
        }

        private static string VerificarICO_CAR_CD_CMR_CO_CUN(Enumeration.Status_CAR_CD_CMR_CO_CUN status)
        {
            string imagem = string.Empty;
            switch (status)
            {
                case Enumeration.Status_CAR_CD_CMR_CO_CUN.Conferido:
                    imagem = "Assets/Conferido.png";
                    break;
                default:
                    break;
            }
            return imagem;
        }

        private static string VerificarICO_DO(Enumeration.Status_DO status)
        {
            string imagem = string.Empty;
            switch (status)
            {
                case Enumeration.Status_DO.Encaminhado:
                    break;
                default:
                    break;
            }

            return imagem;
        }

        private static string VerificarICO_EST(Enumeration.Status_EST status)
        {
            string imagem = string.Empty;
            switch (status)
            {
                case Enumeration.Status_EST.Estornado:
                    break;
                default:
                    break;
            }

            return imagem;
        }

        private static string VerificarICO_FC(Enumeration.Status_FC status)
        {
            string imagem = string.Empty;
            switch (status)
            {
                case Enumeration.Status_FC.Devolvido_A_Pedido_Do_Cliente:
                    break;
                case Enumeration.Status_FC.Com_Entrega_Agendada:
                    break;
                case Enumeration.Status_FC.Mal_Encaminhado:
                    imagem = "Assets/DestinatarioAusente.png";
                    break;
                case Enumeration.Status_FC.Mal_Endereçado:
                    imagem = "Assets/DestinatarioAusente.png";
                    break;
                case Enumeration.Status_FC.Reintegrado:
                    break;
                case Enumeration.Status_FC.Restricao_Lancamento_Externo:
                    break;
                case Enumeration.Status_FC.Empresa_Sem_Expediente:
                    imagem = "Assets/DestinatarioAusente.png";
                    break;
                default:
                    break;
            }

            return imagem;
        }

        private static string VerificarICO_IDC(Enumeration.Status_IDC status)
        {
            string imagem = string.Empty;
            switch (status)
            {
                case Enumeration.Status_IDC.Indenizado:
                    break;
                default:
                    break;
            }

            return imagem;
        }

        private static string VerificarICO_IE(Enumeration.Status_IE status)
        {
            string imagem = string.Empty;
            switch (status)
            {
                case Enumeration.Status_IE.Irregularidade_Na_Expedicao:
                    break;
                default:
                    break;
            }

            return imagem;
        }

        private static string VerificarICO_IT(Enumeration.Status_IT status)
        {
            string imagem = string.Empty;
            switch (status)
            {
                case Enumeration.Status_IT.Passagem_Interna:
                    break;
                default:
                    break;
            }

            return imagem;
        }

        private static string VerificarICO_LDI(Enumeration.Status_LDI status)
        {
            string imagem = string.Empty;
            switch (status)
            {
                case Enumeration.Status_LDI.Aguardando_Retirada:
                    break;
                case Enumeration.Status_LDI.Caixa_Postal:
                    break;
                case Enumeration.Status_LDI.Fiscalização:
                    break;
                default:
                    break;
            }

            return imagem;
        }

        private static string VerificarICO_OEC(Enumeration.Status_OEC status)
        {
            string imagem = string.Empty;
            switch (status)
            {
                case Enumeration.Status_OEC.Saiu_Para_Entrega:
                    imagem = "Assets/SaiuEntrega.png";
                    break;
                default:
                    break;
            }

            return imagem;
        }

        private static string VerificarICO_PAR(Enumeration.Status_PAR status)
        {
            string imagem = string.Empty;
            switch (status)
            {
                case Enumeration.Status_PAR.Conferido:
                    imagem = "Assets/Conferido.png";
                    break;
                default:
                    imagem = "Assets/CarimboRecebido.png";
                    break;
            }

            return imagem;
        }

        private static string VerificarICO_PMT(Enumeration.Status_PMT status)
        {
            string imagem = string.Empty;
            switch (status)
            {
                case Enumeration.Status_PMT.Partiu_Em_meio_De_Transporte:
                    break;
                default:
                    break;
            }

            return imagem;
        }

        private static string VerificarICO_PO(Enumeration.Status_PO status)
        {
            string imagem = string.Empty;
            switch (status)
            {
                case Enumeration.Status_PO.Postado_dh:
                    break;
                default:
                    break;
            }

            return imagem;
        }

        private static string VerificarICO_RO(Enumeration.Status_RO status)
        {
            string imagem = string.Empty;
            switch (status)
            {
                case Enumeration.Status_RO.Encaminhado:
                    break;
                case Enumeration.Status_RO.Encaminhado_Estornado:
                    break;
                default:
                    break;
            }
            return imagem;
        }

        private static string VerificarICO_TR(Enumeration.Status_TR status)
        {
            string imagem = string.Empty;
            switch (status)
            {
                case Enumeration.Status_TR.Transito:
                    break;
                default:
                    break;
            }

            return imagem;
        }
    }
}
