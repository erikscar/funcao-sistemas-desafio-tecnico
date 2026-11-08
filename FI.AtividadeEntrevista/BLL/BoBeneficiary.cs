using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FI.AtividadeEntrevista.BLL
{
    public class BoBeneficiary
    {
        /// <summary>
        /// Inclui um novo beneficiário
        /// </summary>
        /// <param name="beneficiary">Objeto de Beneficiário</param>
        public long Incluir(DML.Beneficiary beneficiary)
        {
            DAL.DaoBeneficiary ben = new DAL.DaoBeneficiary();
            return ben.Register(beneficiary);
        }

        /// <summary>
        /// VerificaExistencia
        /// </summary>
        /// <param name="CPF"></param>
        /// <returns></returns>
        public bool AlreadyExists(string CPF, long clientId)
        {
            DAL.DaoBeneficiary ben = new DAL.DaoBeneficiary();
            return ben.AlreadyExists(CPF, clientId);
        }

        public List<DML.Beneficiary> Consultar(long id)
        {
            DAL.DaoBeneficiary ben = new DAL.DaoBeneficiary();
            return ben.Consultar(id);
        }

        /// <summary>
        /// Altera um beneficiário
        /// </summary>
        /// <param name="beneficiary">Objeto de beneficiário</param>
        public void Alterar(DML.Beneficiary beneficiary)
        {
            DAL.DaoBeneficiary ben = new DAL.DaoBeneficiary();
            ben.Alterar(beneficiary);
        }

        /// <summary>
        /// Exclui os beneficiários de um cliente
        /// </summary>
        /// <param name="clientId">ID CLIENTE do beneficiário</param>
        /// <param name="beneficiaryId">ID do beneficiário</param>
        public void Excluir(long clientId, long beneficiaryId)
        {
            DAL.DaoBeneficiary ben = new DAL.DaoBeneficiary();
            ben.Excluir(clientId, beneficiaryId);
        }
    }
}
