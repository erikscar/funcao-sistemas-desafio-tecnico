
using FI.AtividadeEntrevista.BLL;
using FI.AtividadeEntrevista.DML;
using FI.WebAtividadeEntrevista.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using WebAtividadeEntrevista.Models;

namespace FI.WebAtividadeEntrevista.Controllers
{
    public class BeneficiaryController : Controller
    {

        [HttpPost]
        public JsonResult Register(long clientId, List<BeneficiaryModel> model)
        {
            var boBeneficiary = new BoBeneficiary();

            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {
                foreach (var beneficiary in model)
                {
                    boBeneficiary.Incluir(new Beneficiary()
                    {
                        Nome = beneficiary.Name,
                        CPF = Regex.Replace(beneficiary.CPF, @"\D", ""),
                        IdCliente = clientId
                    });

                }
                return Json(new { message = "Cadastro efetuado com sucesso" });
            }
        }
    }
}