using System;
using System.Collections.Generic;
using System.Text;

namespace ERP.Lavanderia.Module.PacoteUtil
{
    public class StringUtils
    {
        /// <summary>
        /// Remove os caracteres especiais de um CPF ou CNPJ
        /// </summary>
        /// <param name="vCpfCnpj"></param>
        /// <returns></returns>
        public static string CleanCpfCnpj(string vCpfCnpj)
        {
            string valor = vCpfCnpj.Replace(".", "");

            valor = valor.Replace("-", "");

            valor = valor.Replace("/", "");

            return valor;
        }
    }
}
