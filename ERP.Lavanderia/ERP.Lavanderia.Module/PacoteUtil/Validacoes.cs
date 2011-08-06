using System;
using System.ComponentModel;

using DevExpress.Xpo;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace ERP.Lavanderia.Module.PacoteUtil
{
    /// <summary>
    /// Classe com funções de validação.
    /// </summary>
    public class Validacoes
    {
        /// <summary>
        /// Efetua a validação de um CPF.
        /// </summary>
        /// <param name="vrCPF"></param>
        /// <returns>Caso o CPF seja válido o método retornará true, caso não seja retornará false.</returns>
        public static bool ValidateCpf(string vrCPF)
        {

            string valor = vrCPF.Replace(".", "");

            valor = valor.Replace("-", "");



            if (valor.Length != 11)

                return false;



            bool igual = true;

            for (int i = 1; i < 11 && igual; i++)

                if (valor[i] != valor[0])

                    igual = false;



            if (igual || valor == "12345678909")

                return false;



            int[] numeros = new int[11];



            for (int i = 0; i < 11; i++)

                numeros[i] = int.Parse(

                  valor[i].ToString());



            int soma = 0;

            for (int i = 0; i < 9; i++)

                soma += (10 - i) * numeros[i];



            int resultado = soma % 11;



            if (resultado == 1 || resultado == 0)
            {

                if (numeros[9] != 0)

                    return false;

            }

            else if (numeros[9] != 11 - resultado)

                return false;



            soma = 0;

            for (int i = 0; i < 10; i++)

                soma += (11 - i) * numeros[i];



            resultado = soma % 11;



            if (resultado == 1 || resultado == 0)
            {

                if (numeros[10] != 0)

                    return false;

            }

            else

                if (numeros[10] != 11 - resultado)

                    return false;



            return true;

        }

        /// <summary>
        /// Efetua a validação de um CNPJ.
        /// </summary>
        /// <param name="vrCNPJ"></param>
        /// <returns>Caso o CNPJ seja válido o método retornará true, caso não seja retornará false.</returns>
        public static bool ValidateCnpj(string vrCNPJ)
        {

            string CNPJ = vrCNPJ.Replace(".", "");

            CNPJ = CNPJ.Replace("/", "");

            CNPJ = CNPJ.Replace("-", "");



            int[] digitos, soma, resultado;

            int nrDig;

            string ftmt;

            bool[] CNPJOk;



            ftmt = "6543298765432";

            digitos = new int[14];

            soma = new int[2];

            soma[0] = 0;

            soma[1] = 0;

            resultado = new int[2];

            resultado[0] = 0;

            resultado[1] = 0;

            CNPJOk = new bool[2];

            CNPJOk[0] = false;

            CNPJOk[1] = false;



            try
            {

                for (nrDig = 0; nrDig < 14; nrDig++)
                {

                    digitos[nrDig] = int.Parse(

                        CNPJ.Substring(nrDig, 1));

                    if (nrDig <= 11)

                        soma[0] += (digitos[nrDig] *

                          int.Parse(ftmt.Substring(

                          nrDig + 1, 1)));

                    if (nrDig <= 12)

                        soma[1] += (digitos[nrDig] *

                          int.Parse(ftmt.Substring(

                          nrDig, 1)));

                }



                for (nrDig = 0; nrDig < 2; nrDig++)
                {

                    resultado[nrDig] = (soma[nrDig] % 11);

                    if ((resultado[nrDig] == 0) || (

                         resultado[nrDig] == 1))

                        CNPJOk[nrDig] = (

                        digitos[12 + nrDig] == 0);

                    else

                        CNPJOk[nrDig] = (

                        digitos[12 + nrDig] == (

                        11 - resultado[nrDig]));

                }

                return (CNPJOk[0] && CNPJOk[1]);

            }

            catch
            {

                return false;

            }

        }

        /// <summary>
        /// Efetua a validação de um número do PIS/PASEP.
        /// </summary>
        /// <param name="pis">Número do pis que será validado</param>
        /// <returns>Caso o número do PIS seja válido o método retornará true, caso não seja retornará false.</returns>
        public static bool ValidatePis(string pis)
        {
            int[] multiplicador = new int[10] { 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma;
            int resto;

            if (pis.Trim().Length == 0)
                return false;

            pis = pis.Trim();
            pis = pis.Replace("-", "").Replace(".", "").PadLeft(11, '0');

            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(pis[i].ToString()) * multiplicador[i];

            resto = soma % 11;

            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            return pis.EndsWith(resto.ToString());
        }

        /// <summary>
        /// Efetua a validação de um número do CEI.
        /// </summary>
        /// <param name="cei">Número do cei que será validado</param>
        /// <returns>Caso o número do CEI seja válido o método retornará true, caso não seja retornará false.</returns>
        public static bool ValidateCei(string cei)
        {

            cei = cei.Trim();
            cei = cei.Replace(".", "");
            cei = cei.Replace("/", "");


            if (cei.Length == 0)
                return false;

            if (cei.Length != 12)
            {
                return false;
            }

            int[] multiplicador = new int[11] { 7, 4, 1, 8, 5, 2, 1, 6, 3, 7, 4 };
            int soma;
            int resultado;
            //Digito Verificador
            int dv = Convert.ToInt32(cei.Substring(11, 1));

            //45.218.77551/44
            //123123123123
            //654654654654

            soma = 0;
            for (int i = 0; i < 11; i++)
                soma += int.Parse(cei[i].ToString()) * multiplicador[i];



            resultado = 0;
            resultado = (soma % 10) + (((soma % 100) - (soma % 10)) / 10);

            resultado = 10 - resultado % 10;

            if (dv == resultado) { return true; }
            else
                return false;

        }
    }

}
