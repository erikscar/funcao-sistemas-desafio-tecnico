using FI.WebAtividadeEntrevista.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;

namespace FI.WebAtividadeEntrevista.Models
{
    public class BeneficiaryModel
    {
        public long Id { get; set; }

        /// <summary>
        /// Nome
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// CPF
        /// </summary>
        [Required]
        [CpfValidator(ErrorMessage = "Digite um CPF válido")]
        public string CPF { get; set; }
    }
}