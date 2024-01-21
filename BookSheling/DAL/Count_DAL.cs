using System.Data.Common;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace BookSheling.DAL
{
    public class Count_DAL : DAL_Helper
    {
        public DataTable PR_DASHBOARD_COUNTS()
        {
            SqlDatabase sqlDB = new SqlDatabase(ConnString);
            DbCommand dbCMD = sqlDB.GetStoredProcCommand("PR_DASHBOARD_COUNTS");

            DataTable dt = new DataTable();
            using (IDataReader dr = sqlDB.ExecuteReader(dbCMD))
            {
                dt.Load(dr);
            }

            return dt;

        }
    }
}
