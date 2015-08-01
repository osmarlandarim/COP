using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;

namespace Entities
{
    public class Util
    {
        /// <summary>
        /// Gerar id sequencial.
        /// </summary>
        /// <returns></returns>
        public static string GeraSequenciaNumerica()
        {
            int i = 1;
            foreach (int b in Guid.NewGuid().ToByteArray())
            {
                i = ((int)b + 1);
            }
            return string.Format("{0}", i);
        }

        /// <summary>
        /// Conexão com a internet.
        /// </summary>
        /// <returns></returns>
        public static string GetInternetStatus()
        {
            string connectionProfileInfo = string.Empty;
            try
            {
                ConnectionProfile InternetConnectionProfile = NetworkInformation.GetInternetConnectionProfile();

                if (InternetConnectionProfile == null)
                {
                    connectionProfileInfo = "Não conectado a Internet";
                }
                else
                {
                    connectionProfileInfo = "Conectado em : " + InternetConnectionProfile.ProfileName;
                }
            }
            catch (Exception ex)
            {
                //rootPage.NotifyUser("Unexpected exception occured: " + ex.ToString(), NotifyType.ErrorMessage);
            }

            return connectionProfileInfo;
        }
    }
}
