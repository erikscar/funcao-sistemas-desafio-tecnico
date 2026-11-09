using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FI.AtividadeEntrevista.BLL
{
    public class BoBeneficiario
    {
        /// <summary>
        /// Inclui um novo beneficiário
        /// </summary>
        /// <param name="beneficiario">Objeto de Beneficiário</param>
        public long Incluir(DML.Beneficiario beneficiario)
        {
            DAL.DaoBeneficiario ben = new DAL.DaoBeneficiario();
            return ben.Incluir(beneficiario);
        }

        public List<DML.Beneficiario> Consultar(long id)
        {
            DAL.DaoBeneficiario ben = new DAL.DaoBeneficiario();
            return ben.Consultar(id);
        }

        /// <summary>
        /// Altera um beneficiário
        /// </summary>
        /// <param name="beneficiario">Objeto de beneficiário</param>
        public void Alterar(DML.Beneficiario beneficiario)
        {
            DAL.DaoBeneficiario ben = new DAL.DaoBeneficiario();
            ben.Alterar(beneficiario);
        }

        /// <summary>
        /// Exclui os beneficiários de um cliente
        /// </summary>
        /// <param name="idCliente">ID CLIENTE do beneficiário</param>
        /// <param name="idBeneficiario">ID do beneficiário</param>
        public void Excluir(long idCliente, long idBeneficiario)
        {
            DAL.DaoBeneficiario ben = new DAL.DaoBeneficiario();
            ben.Excluir(idCliente, idBeneficiario);
        }
    }
}
