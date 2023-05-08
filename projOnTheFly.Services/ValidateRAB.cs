using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projOnTheFly.Services
{
    public class ValidateRAB
    {
        private string _rab;
        List<string> Nation = new () { "PP", "PR", "PS", "PT", "PU"};
        List<string> Registration = new() { "SOS", "XXX", "PAN", "TTT", "IFR", "VMC", "IMC", "TNC", "PQP"};

        public ValidateRAB(string rab)
        {
            _rab = rab.ToUpper();
        }

        public bool IsValid()
        {
            if(string.IsNullOrEmpty(_rab)) return false;

            string[] validation = _rab.Split('-');

            if (!Nation.Contains(validation[0])) return false;
            if (Registration.Contains(validation[1])) return false;
            var RegistrationLetters = validation[1].ToCharArray();
            if (RegistrationLetters[0] == 'Q') return false;
            if (RegistrationLetters[1] == 'W') return false;
            if (_rab.Equals("PU-TAS")) return false;

            return true;
        }

    }
}
