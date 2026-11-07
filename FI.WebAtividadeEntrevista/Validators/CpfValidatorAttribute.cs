using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace FI.WebAtividadeEntrevista.Validators
{
    public class CpfValidatorAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                return true;

            string cpf = value.ToString();

            cpf = Regex.Replace(cpf, @"\D", "");

            if (cpf.Length != 11)
                return false;

            if (cpf == new string(cpf[0], 11))
                return false;


            int sum = 0;
            for (int i = 0; i < 9; i++)
            {
                sum += int.Parse(cpf[i].ToString()) * (10 - i);
            }

            int rest = sum % 11;
            int firstDigit = rest < 2 ? 0 : 11 - rest;

            if (int.Parse(cpf[9].ToString()) != firstDigit)
                return false;

            sum = 0;
            for (int i = 0; i < 10; i++)
            {
                sum += int.Parse(cpf[i].ToString()) * (11 - i);
            }

            rest = sum % 11;
            int secondDigit = rest < 2 ? 0 : 11 - rest;

            if (int.Parse(cpf[10].ToString()) != secondDigit)
                return false;

            return true;
        }
    }
}