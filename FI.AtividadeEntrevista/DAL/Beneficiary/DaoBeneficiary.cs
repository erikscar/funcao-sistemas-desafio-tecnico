using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FI.AtividadeEntrevista.DAL
{
    internal class DaoBeneficiary : AcessoDados
    {
        /// <summary>
        /// Inclui um novo beneficiário
        /// </summary>
        /// <param name="cliente">Objeto de beneficiário</param>
        internal long Register(DML.Beneficiary beneficiary)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("Nome", beneficiary.Nome));
            parametros.Add(new System.Data.SqlClient.SqlParameter("CPF", beneficiary.CPF));
            parametros.Add(new System.Data.SqlClient.SqlParameter("IDCLIENTE", beneficiary.IdCliente));

            DataSet ds = base.Consultar("FI_SP_IncBeneficiary", parametros);
            long ret = 0;
            if (ds.Tables[0].Rows.Count > 0)
                long.TryParse(ds.Tables[0].Rows[0][0].ToString(), out ret);
            return ret;
        }

        /// <summary>
        /// Consulta um novo beneficiário
        /// </summary>
        /// <param name="beneficiary">Objeto de beneficiário</param>
        internal List<DML.Beneficiary> Consultar(long Id)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("IDCLIENTE", Id));

            DataSet ds = base.Consultar("FI_SP_ConsBeneficiary", parametros);
            List<DML.Beneficiary> ben = Converter(ds);

            return ben;
        }

        private List<DML.Beneficiary> Converter(DataSet ds)
        {
            List<DML.Beneficiary> list = new List<DML.Beneficiary>();
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DML.Beneficiary ben = new DML.Beneficiary();
                    ben.Id = row.Field<long>("Id");
                    ben.CPF = row.Field<string>("CPF");
                    ben.Nome = row.Field<string>("Nome");
                    list.Add(ben);
                }
            }

            return list;
        }

        /// <summary>
        /// Altera um beneficiário
        /// </summary>
        /// <param name="beneficiary">Objeto de beneficiário</param>
        internal void Alterar(DML.Beneficiary beneficiary)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("Nome", beneficiary.Nome));
            parametros.Add(new System.Data.SqlClient.SqlParameter("CPF", beneficiary.CPF));
            parametros.Add(new System.Data.SqlClient.SqlParameter("ID", beneficiary.Id));
            parametros.Add(new System.Data.SqlClient.SqlParameter("IDCLIENTE", beneficiary.IdCliente));
            base.Executar("FI_SP_AltBeneficiary", parametros);
        }

        /// <summary>
        /// Excluir os beneficiários de um cliente
        /// </summary>
        /// <param name="idClient">ID Cliente do beneficiário</param>
        /// <param name="beneficiaryId">ID do beneficiário</param>
        internal void Excluir(long idClient, long beneficiaryId)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("IDCLIENTE", idClient));
            parametros.Add(new System.Data.SqlClient.SqlParameter("ID", beneficiaryId));

            base.Executar("FI_SP_DeleteBeneficiary", parametros);
        }
    }
}
    
