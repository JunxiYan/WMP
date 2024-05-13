using SqlSugar;
namespace SQLModel
{
    public class SQLModel
    { public SQLModel() { 
        SqlSugarClient Db = new SqlSugarClient(new ConnectionConfig()
        {
            ConnectionString = "datasource=demo.db",
            DbType = DbType.Sqlite,
            IsAutoCloseConnection = true
        },
        db =>   {
            db.Aop.OnLogExecuting = (sql, pars) =>
                { Console.WriteLine(UtilMethods.GetNativeSql(sql, pars)); };
                });
        Db.DbMaintenance.CreateDatabase();

            Db.CodeFirst.InitTables();

        }

    }
}
