using FI.AtividadeEntrevista.BLL;
using FI.AtividadeEntrevista.DML;
using FI.WebAtividadeEntrevista.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using WebAtividadeEntrevista.Models;

namespace WebAtividadeEntrevista.Controllers
{
    public class ClienteController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Incluir()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Incluir(ClienteModel model)
        {
            BoCliente boClient = new BoCliente();
            BoBeneficiary boBeneficiary = new BoBeneficiary();

            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join("<br/>", erros));
            }
            else
            {
                var cpf = Regex.Replace(model.CPF, @"\D", "");

                var userAlreadyExists = boClient.VerificarExistencia(cpf, 0);

                if (userAlreadyExists)
                {
                    Response.StatusCode = 400;
                    return Json("O CPF informado já está vinculado a outro cliente.");
                }

                if (model.Beneficiarios != null)
                {
                    var cpfs = new List<string>();

                    foreach (var beneficiary in model.Beneficiarios)
                    {
                        string beneficiaryCpf = Regex.Replace(beneficiary.CPF, @"\D", "");

                        if (cpfs.Contains(beneficiaryCpf))
                        {
                            Response.StatusCode = 400;
                            return Json("Informe um CPF único para cada beneficiário.");
                        }

                        cpfs.Add(beneficiaryCpf);
                    }
                }

                model.Id = boClient.Incluir(new Cliente()
                {
                    CEP = model.CEP,
                    Cidade = model.Cidade,
                    CPF = cpf,
                    Email = model.Email,
                    Estado = model.Estado,
                    Logradouro = model.Logradouro,
                    Nacionalidade = model.Nacionalidade,
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    Telefone = model.Telefone
                });

                if (model.Beneficiarios != null)
                {
                    foreach (var beneficiary in model.Beneficiarios)
                    {
                        boBeneficiary.Incluir(new Beneficiary
                        {
                            Nome = beneficiary.Name,
                            CPF = Regex.Replace(beneficiary.CPF, @"\D", ""),
                            IdCliente = model.Id
                        });
                    }
                }

                return Json(new { message = "Cadastro efetuado com sucesso", clientId = model.Id });
            }
        }

        [HttpPost]
        public JsonResult Alterar(ClienteModel model)
        {
            BoCliente boClient = new BoCliente();
            BoBeneficiary boBeneficiary = new BoBeneficiary();

            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join("<br />", erros));
            }
            else
            {
                var cpf = Regex.Replace(model.CPF, @"\D", "");

                var userAlreadyExists = boClient.VerificarExistencia(cpf, model.Id);

                if (userAlreadyExists)
                {
                    Response.StatusCode = 400;
                    return Json("O CPF informado já está vinculado a outro cliente.");
                }

                if (model.Beneficiarios != null)
                {
                    var cpfs = new List<string>();

                    foreach (var beneficiary in model.Beneficiarios)
                    {
                        string beneficiaryCpf = Regex.Replace(beneficiary.CPF, @"\D", "");

                        if (cpfs.Contains(beneficiaryCpf))
                        {
                            Response.StatusCode = 400;
                            return Json("Informe um CPF único para cada beneficiário.");
                        }

                        cpfs.Add(beneficiaryCpf);
                    }
                }

                if (model.Beneficiarios == null) model.Beneficiarios = new List<BeneficiaryModel>();

                var beneficiaries = boBeneficiary.Consultar(model.Id);

                foreach (var beneficiary in beneficiaries)
                {
                    bool stillInList = model.Beneficiarios.Any(b => b.Id == beneficiary.Id);

                    if (!stillInList)
                    {
                        boBeneficiary.Excluir(model.Id, beneficiary.Id);
                    }
                }

                boClient.Alterar(new Cliente()
                {
                    Id = model.Id,
                    CEP = model.CEP,
                    CPF = cpf,
                    Cidade = model.Cidade,
                    Email = model.Email,
                    Estado = model.Estado,
                    Logradouro = model.Logradouro,
                    Nacionalidade = model.Nacionalidade,
                    Nome = model.Nome,
                    Sobrenome = model.Sobrenome,
                    Telefone = model.Telefone
                });

                    foreach (var beneficiary in model.Beneficiarios)
                    {
                        //Se ID > 0 Significa que é um beneficiário já existente, logo apenas atualizar
                        if (beneficiary.Id > 0)
                        {
                            boBeneficiary.Alterar(new Beneficiary()
                            {
                                Id = beneficiary.Id,
                                Nome = beneficiary.Name,
                                CPF = Regex.Replace(beneficiary.CPF, @"\D", ""),
                                IdCliente = model.Id
                            });
                        }
                        //Se ID == 0 Significa que é um beneficiário que não existente, logo incluir
                        else
                        {
                            boBeneficiary.Incluir(new Beneficiary
                            {
                                CPF = Regex.Replace(beneficiary.CPF, @"\D", ""),
                                Nome = beneficiary.Name,
                                IdCliente = model.Id
                            });
                        }
                    }
                return Json("Cadastro alterado com sucesso");
            }
        }

        [HttpGet]
        public ActionResult Alterar(long id)
        {
            BoCliente boClient = new BoCliente();
            BoBeneficiary boBeneficiary = new BoBeneficiary();

            Cliente cliente = boClient.Consultar(id);
            List<Beneficiary> beneficiaries = boBeneficiary.Consultar(id);

            List<BeneficiaryModel> beneficiaryModels = beneficiaries.Select(b => new BeneficiaryModel
            {
                Id = b.Id,
                CPF = b.CPF,
                Name = b.Nome
            }).ToList();

            Models.ClienteModel model = null;

            if (cliente != null)
            {
                model = new ClienteModel()
                {
                    Id = cliente.Id,
                    CEP = cliente.CEP,
                    CPF = cliente.CPF,
                    Cidade = cliente.Cidade,
                    Email = cliente.Email,
                    Estado = cliente.Estado,
                    Logradouro = cliente.Logradouro,
                    Nacionalidade = cliente.Nacionalidade,
                    Nome = cliente.Nome,
                    Sobrenome = cliente.Sobrenome,
                    Telefone = cliente.Telefone,
                    Beneficiarios = beneficiaryModels
                };
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult ClienteList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                int qtd = 0;
                string campo = string.Empty;
                string crescente = string.Empty;
                string[] array = jtSorting.Split(' ');

                if (array.Length > 0)
                    campo = array[0];

                if (array.Length > 1)
                    crescente = array[1];

                List<Cliente> clientes = new BoCliente().Pesquisa(jtStartIndex, jtPageSize, campo, crescente.Equals("ASC", StringComparison.InvariantCultureIgnoreCase), out qtd);

                //Return result to jTable
                return Json(new { Result = "OK", Records = clientes, TotalRecordCount = qtd });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }
    }
}