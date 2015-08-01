using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Enumeration
    {
        public enum Resultado
        {
            /// <summary>
            /// Ultimo registro do evento.
            /// </summary>
            U = 'U',
            /// <summary>
            /// Todos os eventos.
            /// </summary>
            T = 'T'
        }

        /// <summary>
        /// 
        /// </summary>
        public class DescricaoEvento
        {
            public const string Entregue = "Entregue";
        }

        /// <summary>
        /// Mensagens
        /// </summary>
        public class Mensagens
        {
            public const string MsgExclusao = "Deseja realmente excluir?";
        }

        /// <summary>
        /// 
        /// </summary>
        public class Correio
        {
            public const string Falha = "Falha na comunicação com a Internet";
            public const string NomeObjetos = "Encomendas dos Correios";
            public const string NomeHistorico = "Histórico";
            public const string NomePublicidade = "Publicidade";
        }

        /// <summary>
        /// 
        /// </summary>
        public class Folders
        {
            public const string SistemaArquivos = "COP";
        }

        public enum Status_BDE_BDI_BDR
        {
            Entregue = 1,
            Destinatario_Ausente = 2,
            Nao_Procurado = 3,
            Recusado = 4,
            Em_Devolucao = 5,
            Destinatario_Desconhecido_Endereco = 6,
            Endereco_Insuficiente_Para_Entrega = 7,
            Nao_Existe_Numero_Indicado = 8,
            Extraviado = 9,
            Destinatario_Mudou_Se = 10,
            Outros = 11,
            Refugado = 12,
            Endereco_Incorreto = 19,
            Destinatario_Ausentes = 20,
            Destinatario_Ausentess = 21,
            Reintegrado = 22,
            Distribuido_Remetente = 23,
            Disponivel_Caixa_Postal = 24,
            Empresa_Sem_Expediente = 25,
            NaoProcurado = 26,
            Pedido_Nao_Solicitado = 27,
            MercadoriaAvariada = 28,
            Extraviados = 31,
            Entrega_Programada = 32,
            Documentacao_Nao_Fornecida_Pelo_Destinatário = 33,
            Logradouro_Com_Numeracao_Irregular_Em_Pesquisa = 34,
            Log_Reversa_Simultânea = 35,
            Log_Reversa_Simultâneas = 36,
            Devolvido_Remetente = 40,
            Aguardando_Parte_Lote = 41,
            Devolvido_Remetentes = 42,
            Objeto_Apreendido_Por_Autoridade_Competente = 43,
            Falta_Documento_Para_Liberacao_Para_Retirada_Interna = 44,
            Residuo_Mesa = 45,
            Entrega_Nao_Efetuada = 46,
            Erro_Lancamento = 47,
            Posta_Restante_Nao_Autorizada = 48,
            Roubo_A_Carteiro = 50,
            Roubo_A_Veículo = 51,
            Roubo_A_Unidade = 52,
            Extraviadoss = 69
        };
        public enum Status_CAR_CD_CMR_CO_CUN
        {
            Conferido = 1
        };

        public enum Status_DO
        {
            Encaminhado = 1
        };

        public enum Status_EST
        {
            Estornado = 1
        };

        public enum Status_FC
        {
            Devolvido_A_Pedido_Do_Cliente = 1,
            Com_Entrega_Agendada = 2,
            Mal_Encaminhado = 3,
            Mal_Endereçado = 4,
            Reintegrado = 5,
            Restricao_Lancamento_Externo = 6,
            Empresa_Sem_Expediente = 7
        };

        public enum Status_IDC
        {
            Indenizado = 1
        };

        public enum Status_IE
        {
            Irregularidade_Na_Expedicao = 1
        };

        public enum Status_IT
        {
            Passagem_Interna = 1
        };

        public enum Status_LDI
        {
            Aguardando_Retirada = 1,
            Caixa_Postal = 2,
            Fiscalização = 8
        };

        public enum Status_OEC
        {
            Saiu_Para_Entrega = 1
        };

        public enum Status_PAR
        {
            Conferido = 15
        };

        public enum Status_PAR_Dois
        {
            Conferido = 16
        };

        public enum Status_PMT
        {
            Partiu_Em_meio_De_Transporte = 1
        };

        public enum Status_PO
        {
            Postado_dh = 9
        };

        public enum Status_RO
        {
            Encaminhado = 1,
            Encaminhado_Estornado = 99
        };

        public enum Status_TR
        {
            Transito = 1
        };
    }
}
