using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projOnTheFly.Services
{
    public class ValidateCPF
    { 
        private readonly string _cpf;
        public ValidateCPF(string cpf)
        {
            _cpf = FormatCPF(cpf);
        }
        public bool IsValid()
        {
            if(string.IsNullOrEmpty(_cpf)) return false;

            if (!IsCpfInFormatCorrect(_cpf))
            {
                var cpfToValidate = _cpf.Remove(9, 2);
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

                if (_cpf == cpfComplete)
                {
                    return true;
                }
            }
            return false;
        }
        private string FormatCPF(string cpfDotAndDash)
        {
            return cpfDotAndDash.Trim().Replace(".", "").Replace("-", "");
        }
        public bool IsCpfInFormatCorrect(string cpf)
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
