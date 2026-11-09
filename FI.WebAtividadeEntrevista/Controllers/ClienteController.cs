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
            BoCliente boCliente = new BoCliente();
            BoBeneficiario boBeneficiario = new BoBeneficiario();

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
                var cpf = Regex.Replace(model.CPF, @"\D", "");

                var usuarioJaExiste = boCliente.VerificarExistencia(cpf, 0);

                if (usuarioJaExiste)
                {
                    Response.StatusCode = 400;
                    return Json("CPF já cadastrado");
                }

                if (model.Beneficiarios != null)
                {
                    var cpfs = new List<string>();

                    foreach (var beneficiario in model.Beneficiarios)
                    {
                        string beneficiarioCpf = Regex.Replace(beneficiario.CPF, @"\D", "");

                        if (cpfs.Contains(beneficiarioCpf))
                        {
                            Response.StatusCode = 400;
                            return Json("Há CPFs repetidos na lista de beneficiários. Cada CPF deve aparecer apenas uma vez.");
                        }

                        cpfs.Add(beneficiarioCpf);
                    }
                }

                model.Id = boCliente.Incluir(new Cliente()
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
                    foreach (var beneficiario in model.Beneficiarios)
                    {
                        boBeneficiario.Incluir(new Beneficiario
                        {
                            Nome = beneficiario.Nome,
                            CPF = Regex.Replace(beneficiario.CPF, @"\D", ""),
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
            BoCliente boCliente = new BoCliente();
            BoBeneficiario boBeneficiario = new BoBeneficiario();

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
                var cpf = Regex.Replace(model.CPF, @"\D", "");

                var usuarioJaExiste = boCliente.VerificarExistencia(cpf, model.Id);

                if (usuarioJaExiste)
                {
                    Response.StatusCode = 400;
                    return Json("CPF já cadastrado");
                }

                if (model.Beneficiarios != null)
                {
                    var cpfs = new List<string>();

                    foreach (var beneficiario in model.Beneficiarios)
                    {
                        string beneficiarioCpf = Regex.Replace(beneficiario.CPF, @"\D", "");

                        if (cpfs.Contains(beneficiarioCpf))
                        {
                            Response.StatusCode = 400;
                            return Json("Há CPFs repetidos na lista de beneficiários. Cada CPF deve aparecer apenas uma vez.");
                        }

                        cpfs.Add(beneficiarioCpf);
                    }
                }

                if (model.Beneficiarios == null) model.Beneficiarios = new List<BeneficiarioModel>();

                var beneficiarios = boBeneficiario.Consultar(model.Id);

                foreach (var beneficiario in beneficiarios)
                {
                    bool aindaNaLista = model.Beneficiarios.Any(b => b.Id == beneficiario.Id);

                    if (!aindaNaLista)
                    {
                        boBeneficiario.Excluir(model.Id, beneficiario.Id);
                    }
                }

                boCliente.Alterar(new Cliente()
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

                    foreach (var beneficiario in model.Beneficiarios)
                    {
                        //Se ID > 0 Significa que é um beneficiário já existente, logo apenas atualizar
                        if (beneficiario.Id > 0)
                        {
                        boBeneficiario.Alterar(new Beneficiario()
                            {
                                Id = beneficiario.Id,
                                Nome = beneficiario.Nome,
                                CPF = Regex.Replace(beneficiario.CPF, @"\D", ""),
                                IdCliente = model.Id
                            });
                        }
                        //Se ID == 0 Significa que é um beneficiário que não existente, logo incluir
                        else
                        {
                        boBeneficiario.Incluir(new Beneficiario
                            {
                                CPF = Regex.Replace(beneficiario.CPF, @"\D", ""),
                                Nome = beneficiario.Nome,
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
            BoCliente boCliente = new BoCliente();
            BoBeneficiario boBeneficiario = new BoBeneficiario();

            Cliente cliente = boCliente.Consultar(id);
            List<Beneficiario> beneficiarios = boBeneficiario.Consultar(id);

            List<BeneficiarioModel> beneficiariosModel = beneficiarios.Select(b => new BeneficiarioModel
            {
                Id = b.Id,
                CPF = b.CPF,
                Nome = b.Nome
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
                    Beneficiarios = beneficiariosModel
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