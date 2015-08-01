using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class SiglasCorreios
    {
        public const string Caminho = "ms-appx:///Assets/Maps/";

        public string Descricao { get; set; }
        public string CaminhoIcon { get; set; }

        public void VerificaFormaEnvio(string codigoRastreio)
        {
            string sigla = codigoRastreio.Substring(0, 2);

            Dictionary<string, Dictionary<string, string>> listSiglas = MontaSiglas(sigla);

            if (listSiglas.Count > 0)
            {
                foreach (KeyValuePair<string, string> item in listSiglas[sigla])
                {
                    Descricao = item.Key;
                    CaminhoIcon = string.Empty;//item.Value;

                    break;
                }
            }
            else
            {
                Descricao = string.Empty;
                CaminhoIcon = string.Empty;
            }
        }

        private static Dictionary<string, Dictionary<string, string>> MontaSiglas(string sigla)
        {
            Dictionary<string, Dictionary<string, string>> listSiglas = new Dictionary<string, Dictionary<string, string>>();
            Dictionary<string, string> siglas = new Dictionary<string, string>();

            switch (sigla.ToUpper())
            {
                case "AL":

                    siglas.Add(sigla.ToUpper() + " - AGENTES DE LEITURA", string.Empty);
                    listSiglas.Add(sigla, siglas);

                    break;

                case "AR":

                    siglas.Add(sigla.ToUpper() + " - AVISO DE RECEBIMENTO", string.Empty);
                    listSiglas.Add(sigla, siglas);

                    break;

                case "AS":

                    siglas.Add(sigla.ToUpper() + " - ENCOMENDA PAC – AÇÃO SOCIAL", Caminho + "PAC.png");
                    listSiglas.Add(sigla, siglas);

                    break;


                case "CC":

                    siglas.Add(sigla.ToUpper() + " - COLIS POSTAUX", Caminho + "Internacional.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "CA":
                case "CB":
                case "CD":
                case "CE":
                case "CF":
                case "CG":
                case "CH":
                case "CI":
                case "CK":
                case "CL":
                case "CM":
                case "CN":
                case "CO":
                case "CQ":
                case "CS":
                case "CT":
                case "CU":
                case "CW":
                case "CX":
                case "CY":
                case "CZ":

                case "EA":
                case "EB":
                case "ED":
                case "EF":
                case "EG":
                case "EI":
                case "EK":
                case "EL":
                case "EM":
                case "EO":
                case "EP":
                case "ET":
                case "EU":
                case "EV":
                case "EW":
                case "EX":
                case "EZ":

                    siglas.Add(sigla.ToUpper() + " - OBJETO INTERNACIONAL", "ms-appx:///Assets/Internacional.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "CJ":
                case "CV":

                    siglas.Add(sigla.ToUpper() + " - REGISTRADO INTERNACIONAL", "ms-appx:///Assets/Internacional.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "CP":

                    siglas.Add(sigla.ToUpper() + " - COLIS POSTAUX", Caminho + "Internacional.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "CR":

                    siglas.Add(sigla.ToUpper() + " - CARTA REGISTRADA SEM VALOR DECLARADO", Caminho + "CartaRegistrada.png");
                    listSiglas.Add(sigla, siglas);

                    break;



                case "DA":

                    siglas.Add(sigla.ToUpper() + " - REM EXPRES COM AR DIGITAL", Caminho + "Default.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "DB":
                case "DC":

                    siglas.Add(sigla.ToUpper() + " - REM EXPRES COM AR DIGITAL BRADESCO", Caminho + "Default.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "DD":

                    siglas.Add(sigla.ToUpper() + " - DEVOLUÇÃO DE DOCUMENTOS", string.Empty);
                    listSiglas.Add(sigla, siglas);

                    break;

                case "DE":

                    siglas.Add(sigla.ToUpper() + " - REMESSA EXPRESSA TALÃO E CARTÃO C/ AR", Caminho + "Default.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "DF":

                    siglas.Add(sigla.ToUpper() + " - E-SEDEX (LÓGICO)", Caminho + "Sedex.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "DI":

                    siglas.Add(sigla.ToUpper() + " - REM EXPRES COM AR DIGITAL ITAU", Caminho + "Default.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "DL":

                    siglas.Add(sigla.ToUpper() + " - ENCOMENDA SEDEX (LÓGICO)", Caminho + "Sedex.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "DP":

                    siglas.Add(sigla.ToUpper() + " - REM EXPRES COM AR DIGITAL PRF", Caminho + "Default.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "DS":

                    siglas.Add(sigla.ToUpper() + " - REM EXPRES COM AR DIGITAL SANTANDER", Caminho + "Default.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "DT":

                    siglas.Add(sigla.ToUpper() + " - REMESSA ECON.SEG.TRANSITO C/AR DIGITAL", Caminho + "Default.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "DX":

                    siglas.Add(sigla.ToUpper() + " - ENCOMENDA SEDEX 10 (LÓGICO)", Caminho + "Sedex.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "EC":

                    siglas.Add(sigla.ToUpper() + " - ENCOMENDA PAC", Caminho + "PAC.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "EE":

                    siglas.Add(sigla.ToUpper() + " - SEDEX INTERNACIONAL", Caminho + "Sedex.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "EH":

                    siglas.Add(sigla.ToUpper() + " - ENCOMENDA NORMAL COM AR DIGITAL", Caminho + "Default.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "EJ":

                    siglas.Add(sigla.ToUpper() + " - ENCOMENDA INTERNACIONAL", Caminho + "Internacional.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "EN":

                    siglas.Add(sigla.ToUpper() + " - ENCOMENDA NORMAL NACIONAL", Caminho + "Default.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "EQ":

                    siglas.Add(sigla.ToUpper() + " - ENCOMENDA SERVIÇO NÃO EXPRESSA ECT", Caminho + "Default.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "ER":

                    siglas.Add(sigla.ToUpper() + " - REGISTRADO", Caminho + "Default.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "ES":

                    siglas.Add(sigla.ToUpper() + " - e-SEDEX", Caminho + "Sedex.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "FA":

                    siglas.Add(sigla.ToUpper() + " - FAC REGISTRATO (LÓGICO)", Caminho + "Default.png");
                    listSiglas.Add(sigla, siglas);

                    break;



                case "FE":

                    siglas.Add(sigla.ToUpper() + " - ENCOMENDA FNDE", Caminho + "Default.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "FF":

                    siglas.Add(sigla.ToUpper() + " - REGISTRADO DETRAN", Caminho + "Detran.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "FH":

                    siglas.Add(sigla.ToUpper() + " - REGISTRADO FAC COM AR DIGITAL", Caminho + "Default.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "FM":

                    siglas.Add(sigla.ToUpper() + " - REGISTRADO - FAC MONITORADO", Caminho + "Default.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "FR":

                    siglas.Add(sigla.ToUpper() + " - REGISTRADO FAC", Caminho + "Default.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "IA":

                    siglas.Add(sigla.ToUpper() + " - INTEGRADA AVULSA", Caminho + "Default.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "IC":

                    siglas.Add(sigla.ToUpper() + " - INTEGRADA A COBRAR", Caminho + "Default.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "ID":

                    siglas.Add(sigla.ToUpper() + " - INTEGRADA DEVOLUCAO DE DOCUMENTO", Caminho + "Default.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "IE":

                    siglas.Add(sigla.ToUpper() + " - INTEGRADA ESPECIAL", Caminho + "Default.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "IF":

                    siglas.Add(sigla.ToUpper() + " - CPF", Caminho + "CPF.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "II":

                    siglas.Add(sigla.ToUpper() + " - INTEGRADA INTERNO", Caminho + "Default.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "IK":

                    siglas.Add(sigla.ToUpper() + " - INTEGRADA COM COLETA SIMULTANEA", Caminho + "Default.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "IM":

                    siglas.Add(sigla.ToUpper() + " - INTEGRADA MEDICAMENTOS", Caminho + "Default.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "IN":

                    siglas.Add(sigla.ToUpper() + " - OBJ DE CORRESP E EMS REC EXTERIOR", Caminho + "EMS.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "IP":

                    siglas.Add(sigla.ToUpper() + " - INTEGRADA PROGRAMADA", Caminho + "Default.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "IR":

                    siglas.Add(sigla.ToUpper() + " - IMPRESSO REGISTRADO", Caminho + "Default.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "IS":

                    siglas.Add(sigla.ToUpper() + " - INTEGRADA STANDARD", Caminho + "Default.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "IT":

                    siglas.Add(sigla.ToUpper() + " - INTEGRADO TERMOLÁBIL", Caminho + "Default.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "IU":

                    siglas.Add(sigla.ToUpper() + " - INTEGRADA URGENTE", Caminho + "Default.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "JA":

                    siglas.Add(sigla.ToUpper() + " - REMESSA ECONOMICA C/AR DIGITAL", Caminho + "Default.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "JB":

                    siglas.Add(sigla.ToUpper() + " - REMESSA ECONOMICA C/AR DIGITAL", Caminho + "Default.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "JC":

                    siglas.Add(sigla.ToUpper() + " - REMESSA ECONOMICA C/AR DIGITAL", Caminho + "Default.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "JD":

                    siglas.Add(sigla.ToUpper() + " - REMESSA ECONÔMICA S/ AR DIGITAL", Caminho + "Default.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "JE":

                    siglas.Add(sigla.ToUpper() + " - REMESSA ECONÔMICA C/ AR DIGITAL", Caminho + "Default.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "JG":

                    siglas.Add(sigla.ToUpper() + " - REGISTRATO AGÊNCIA (FÍSICO)", Caminho + "Default.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "JJ":

                    siglas.Add(sigla.ToUpper() + " - REGISTRADO JUSTIÇA", Caminho + "Default.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "JL":

                    siglas.Add(sigla.ToUpper() + " - OBJETO REGISTRADO (LÓGICO)", Caminho + "Default.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "JM":

                    siglas.Add(sigla.ToUpper() + " - MALA DIRETA POSTAL ESPECIAL (LÓGICO)", Caminho + "Default.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "LA":

                    siglas.Add(sigla.ToUpper() + " - LOGÍSTICA REVERSA SIMULTÂNEA - ENCOMENDA SEDEX (AGÊNCIA)", Caminho + "Sedex.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "LB":

                    siglas.Add(sigla.ToUpper() + " - LOGÍSTICA REVERSA SIMULTÂNEA - ENCOMENDA e-SEDEX (AGÊNCIA)", Caminho + "Sedex.png");
                    listSiglas.Add(sigla, siglas);

                    break;



                case "LC":

                    siglas.Add(sigla.ToUpper() + " - CARTA EXPRESSA", Caminho + "CartaRegistrada.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "LE":

                    siglas.Add(sigla.ToUpper() + " - LOGÍSTICA REVERSA ECONOMICA", Caminho + "Default.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "LP":

                    siglas.Add(sigla.ToUpper() + " - LOGÍSTICA REVERSA SIMULTÂNEA - ENCOMENDA PAC (AGÊNCIA)", Caminho + "PAC.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "LS":

                    siglas.Add(sigla.ToUpper() + " - LOGISTICA REVERSA SEDEX", Caminho + "Sedex.png");
                    listSiglas.Add(sigla, siglas);

                    break;



                case "LV":

                    siglas.Add(sigla.ToUpper() + " - LOGISTICA REVERSA EXPRESSA", Caminho + "Default.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "LX":
                case "LY":
                    siglas.Add(sigla.ToUpper() + " - CARTA EXPRESSA", Caminho + "CartaRegistrada.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "MA":

                    siglas.Add(sigla.ToUpper() + " - SERVIÇOS ADICIONAIS", string.Empty);
                    listSiglas.Add(sigla, siglas);

                    break;

                case "MB":

                    siglas.Add(sigla.ToUpper() + " - TELEGRAMA DE BALCÃO", Caminho + "Telegrama.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "MC":

                    siglas.Add(sigla.ToUpper() + " - MALOTE CORPORATIVO", Caminho + "Default.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "ME":

                    siglas.Add(sigla.ToUpper() + " - TELEGRAMA", Caminho + "Telegrama.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "MF":

                    siglas.Add(sigla.ToUpper() + " - TELEGRAMA FONADO", Caminho + "Telegrama.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "MK":

                    siglas.Add(sigla.ToUpper() + " - TELEGRAMA CORPORATIVO", Caminho + "Telegrama.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "MM":

                    siglas.Add(sigla.ToUpper() + " - TELEGRAMA GRANDES CLIENTES", Caminho + "Telegrama.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "MP":

                    siglas.Add(sigla.ToUpper() + " - TELEGRAMA PRÉ-PAGO", Caminho + "Telegrama.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "MS":

                    siglas.Add(sigla.ToUpper() + " - ENCOMENDA SAUDE", Caminho + "Default.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "MT":

                    siglas.Add(sigla.ToUpper() + " - TELEGRAMA VIA TELEMAIL", Caminho + "Telegrama.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "MY":

                    siglas.Add(sigla.ToUpper() + " - TELEGRAMA INTERNACIONAL ENTRANTE", Caminho + "Internacional.png");
                    listSiglas.Add(sigla, siglas);

                    break;



                case "MZ":

                    siglas.Add(sigla.ToUpper() + " - TELEGRAMA VIA CORREIOS ON LINE", Caminho + "Telegrama.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "NE":

                    siglas.Add(sigla.ToUpper() + " - TELE SENA RESGATADA", Caminho + "Default.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "PA":
                case "PF":

                    siglas.Add(sigla.ToUpper() + " - PASSAPORTE", Caminho + "Passaporte.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "PB":

                    siglas.Add(sigla.ToUpper() + " - ENCOMENDA PAC - NÃO URGENTE", Caminho + "PAC.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "PC":

                    siglas.Add(sigla.ToUpper() + " - ENCOMENDA PAC A COBRAR", Caminho + "PAC.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "PD":

                    siglas.Add(sigla.ToUpper() + " - ENCOMENDA PAC - NÃO URGENTE", Caminho + "PAC.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "PG":

                    siglas.Add(sigla.ToUpper() + " - ENCOMENDA PAC (ETIQUETA FÍSICA)", Caminho + "PAC.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "PH":

                    siglas.Add(sigla.ToUpper() + " - ENCOMENDA PAC (ETIQUETA LÓGICA)", Caminho + "PAC.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "PR":

                    siglas.Add(sigla.ToUpper() + " - REEMBOLSO POSTAL - CLIENTE AVULSO", string.Empty);
                    listSiglas.Add(sigla, siglas);

                    break;

                case "RA":

                    siglas.Add(sigla.ToUpper() + " - REGISTRADO PRIORITÁRIO", string.Empty);
                    listSiglas.Add(sigla, siglas);

                    break;

                case "RB":

                    siglas.Add(sigla.ToUpper() + " - CARTA REGISTRADA", Caminho + "CartaRegistrada.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "RC":

                    siglas.Add(sigla.ToUpper() + " - CARTA REGISTRADA COM VALOR DECLARADO", Caminho + "CartaRegistrada.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "RD":

                    siglas.Add(sigla.ToUpper() + " - REMESSA ECONOMICA DETRAN", Caminho + "Detran.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "RE":

                    siglas.Add(sigla.ToUpper() + " - REGISTRADO ECONÔMICO", Caminho + "CartaRegistrada.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "RF":

                    siglas.Add(sigla.ToUpper() + " - OBJETO DA RECEITA FEDERAL", Caminho + "Default.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "RG":

                    siglas.Add(sigla.ToUpper() + " - REGISTRADO DO SISTEMA SARA", string.Empty);
                    listSiglas.Add(sigla, siglas);

                    break;

                case "RH":

                    siglas.Add(sigla.ToUpper() + " - REGISTRADO COM AR DIGITAL", Caminho + "Default.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "RI":

                    siglas.Add(sigla.ToUpper() + " - REGISTRADO", string.Empty);
                    listSiglas.Add(sigla, siglas);

                    break;

                case "RJ":

                    siglas.Add(sigla.ToUpper() + " - REGISTRADO AGÊNCIA", string.Empty);
                    listSiglas.Add(sigla, siglas);

                    break;

                case "RK":

                    siglas.Add(sigla.ToUpper() + " - REGISTRADO AGÊNCIA", string.Empty);
                    listSiglas.Add(sigla, siglas);

                    break;

                case "RL":

                    siglas.Add(sigla.ToUpper() + " - REGISTRADO LÓGICO", string.Empty);
                    listSiglas.Add(sigla, siglas);

                    break;

                case "RM":

                    siglas.Add(sigla.ToUpper() + " - REGISTRADO AGÊNCIA", string.Empty);
                    listSiglas.Add(sigla, siglas);

                    break;

                case "RN":

                    siglas.Add(sigla.ToUpper() + " - REGISTRADO AGÊNCIA", string.Empty);
                    listSiglas.Add(sigla, siglas);

                    break;

                case "RO":

                    siglas.Add(sigla.ToUpper() + " - REGISTRADO AGÊNCIA", string.Empty);
                    listSiglas.Add(sigla, siglas);

                    break;

                case "RP":

                    siglas.Add(sigla.ToUpper() + " - REEMBOLSO POSTAL - CLIENTE INSCRITO", string.Empty);
                    listSiglas.Add(sigla, siglas);

                    break;

                case "RQ":

                    siglas.Add(sigla.ToUpper() + " - REGISTRADO AGÊNCIA", string.Empty);
                    listSiglas.Add(sigla, siglas);

                    break;



                case "RR":

                    siglas.Add(sigla.ToUpper() + " - CARTA REGISTRADA SEM VALOR DECLARADO", Caminho + "CartaRegistrada.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "RS":

                    siglas.Add(sigla.ToUpper() + " - REGISTRADO LÓGICO", string.Empty);
                    listSiglas.Add(sigla, siglas);

                    break;

                case "RT":

                    siglas.Add(sigla.ToUpper() + " - REM ECON TALAO/CARTAO SEM AR DIGITAL", Caminho + "Default.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "RU":

                    siglas.Add(sigla.ToUpper() + " - REGISTRADO SERVIÇO ECT", Caminho + "Default.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "RV":

                    siglas.Add(sigla.ToUpper() + " - REM ECON CRLV/CRV/CNH COM AR DIGITAL", Caminho + "Detran.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "RY":

                    siglas.Add(sigla.ToUpper() + " - REM ECON TALAO/CARTAO COM AR DIGITAL", Caminho + "Default.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "RZ":

                    siglas.Add(sigla.ToUpper() + " - REGISTRADO", string.Empty);
                    listSiglas.Add(sigla, siglas);

                    break;

                case "SA":

                    siglas.Add(sigla.ToUpper() + " - SEDEX ANOREG", Caminho + "Sedex.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "SB":

                    siglas.Add(sigla.ToUpper() + " - SEDEX 10 AGÊNCIA (FÍSICO)", Caminho + "Sedex.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "SC":

                    siglas.Add(sigla.ToUpper() + " - SEDEX A COBRAR", Caminho + "Sedex.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "SD":

                    siglas.Add(sigla.ToUpper() + " - REMESSA EXPRESSA DETRAN", Caminho + "Detran.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "SE":

                    siglas.Add(sigla.ToUpper() + " - ENCOMENDA SEDEX", Caminho + "Sedex.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "SG":

                    siglas.Add(sigla.ToUpper() + " - SEDEX DO SISTEMA SARA", Caminho + "Sedex.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "SI":

                    siglas.Add(sigla.ToUpper() + " - SEDEX AGÊNCIA", Caminho + "Sedex.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "SJ":

                    siglas.Add(sigla.ToUpper() + " - SEDEX HOJE", Caminho + "Sedex.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "SK":

                    siglas.Add(sigla.ToUpper() + " - SEDEX AGÊNCIA", Caminho + "Sedex.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "SL":

                    siglas.Add(sigla.ToUpper() + " - SEDEX LÓGICO", Caminho + "Sedex.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "SM":

                    siglas.Add(sigla.ToUpper() + " - SEDEX MESMO DIA", Caminho + "Sedex.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "SN":

                    siglas.Add(sigla.ToUpper() + " - SEDEX COM VALOR DECLARADO", Caminho + "Sedex.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "SO":

                    siglas.Add(sigla.ToUpper() + " - SEDEX AGÊNCIA", Caminho + "Sedex.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "SP":

                    siglas.Add(sigla.ToUpper() + " - SEDEX PRÉ-FRANQUEADO", Caminho + "Sedex.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "SQ":
                case "SR":

                    siglas.Add(sigla.ToUpper() + " - SEDEX", Caminho + "Sedex.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "SS":

                    siglas.Add(sigla.ToUpper() + " - SEDEX FÍSICO", Caminho + "Sedex.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "ST":

                    siglas.Add(sigla.ToUpper() + " - REM EXPRES TALAO/CARTAO SEM AR DIGITAL", string.Empty);
                    listSiglas.Add(sigla, siglas);

                    break;

                case "SU":

                    siglas.Add(sigla.ToUpper() + " - ENCOMENDA SERVIÇO EXPRESSA ECT", Caminho + "Default.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "SV":

                    siglas.Add(sigla.ToUpper() + " - REM EXPRES CRLV/CRV/CNH COM AR DIGITAL", Caminho + "CartaRegistrada.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "SW":

                    siglas.Add(sigla.ToUpper() + " - e-SEDEX", Caminho + "Sedex.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "SX":

                    siglas.Add(sigla.ToUpper() + " - SEDEX 10", Caminho + "Sedex.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "SY":

                    siglas.Add(sigla.ToUpper() + " - REM EXPRES TALAO/CARTAO COM AR DIGITAL", string.Empty);
                    listSiglas.Add(sigla, siglas);

                    break;

                case "SZ":

                    siglas.Add(sigla.ToUpper() + " - SEDEX AGÊNCIA", Caminho + "Sedex.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "TE":

                    siglas.Add(sigla.ToUpper() + " - TESTE (OBJETO PARA TREINAMENTO)", Caminho + "Default.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "TS":

                    siglas.Add(sigla.ToUpper() + " - TESTE (OBJETO PARA TREINAMENTO)", Caminho + "Default.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "VA":
                case "VD":
                case "VF":

                    siglas.Add(sigla.ToUpper() + " - ENCOMENDAS COM VALOR DECLARADO", Caminho + "Default.png");
                    listSiglas.Add(sigla, siglas);

                    break;

                case "VC":
                case "VE":

                    siglas.Add(sigla.ToUpper() + " - ENCOMENDAS", Caminho + "Default.png");

                    listSiglas.Add(sigla, siglas);

                    break;

                case "XM":

                    siglas.Add(sigla.ToUpper() + " - SEDEX MUNDI", Caminho + "Sedex.png");

                    listSiglas.Add(sigla, siglas);

                    break;

                case "XR":

                    siglas.Add(sigla.ToUpper() + " - ENCOMENDA SUR POSTAL EXPRESSO", Caminho + "Default.png");

                    listSiglas.Add(sigla, siglas);

                    break;

                case "XX":

                    siglas.Add(sigla.ToUpper() + " - ENCOMENDA SUR POSTAL 24 HORAS", Caminho + "Default.png");

                    listSiglas.Add(sigla, siglas);

                    break;

                default:

                    break;
            }

            return listSiglas;
        }
    }
}
