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
    }
}
