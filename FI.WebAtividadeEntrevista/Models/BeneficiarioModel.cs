using FI.WebAtividadeEntrevista.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;

namespace FI.WebAtividadeEntrevista.Models
{
    public class BeneficiarioModel
    {
        public long Id { get; set; }

        /// <summary>
        /// Nome
        /// </summary>
        [Required]
        public string Nome { get; set; }

        /// <summary>
        /// CPF
        /// </summary>
        [Required]
        [CpfValidator(ErrorMessage = "Informe um CPF válido para o beneficiário.")]
        public string CPF { get; set; }
    }
}