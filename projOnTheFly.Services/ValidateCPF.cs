using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projOnTheFly.Services
{
    public class ValidateCPF
    {
        public bool cpfIsValid(string cpf, string cpfToValidate)
        {
            if (!isCpfInFormatCorrect(cpf))
            {
                cpfToValidate = cpfToValidate.Remove(9, 2);
                int aux = 0;

                for (int i = 0, j = 10; i <= 8; i++, j--)
                    aux += (cpfToValidate[i] - '0') * (j);

                var firstDigit = (aux * 10) % 11;

                if (firstDigit == 10)
                    firstDigit = 0;

                string cpfFirstDigit = cpfToValidate + firstDigit;

                aux = 0;
                for (int i = 0, j = 11; i <= 9; i++, j--)
                    aux += (cpfFirstDigit[i] - '0') * (j);

                var secondDigit = (aux * 10) % 11;

                if (secondDigit == 10)
                    secondDigit = 0;

                string cpfComplete = cpfFirstDigit + secondDigit;

                if (cpf == cpfComplete)
                {
                    return true;
                }
            }
            return false;
        }
        public string formatCPF(string cpfDotAndDash)
        {
            string cpf = cpfDotAndDash.Trim().Replace(".", "").Replace("-", "");
            return cpf;
        }
        public bool isCpfInFormatCorrect(string cpf)
        {
            string aux = "11111111111";

            for (int i = 0; i < 10; i++)
            {
                if (float.Parse(aux) * i == float.Parse(cpf))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
