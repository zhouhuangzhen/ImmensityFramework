using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Framework.Entity;
using Xavier.Framework.Db.Repository;

namespace Tests.Framework.Db.Consoles
{
    class Program
    {
        static void Main(string[] args)
        {
            TestDefault();
            TestSqlServer();
            TestMySql();
            TestOracle();
            TestSQLite();
            TestPostgre();
            Console.ReadLine();
        }

        private static void TestDefault()
        {
            //  其他
            var dal = new RepositoryBase<CityEntity>();
            //  Oracle的Schema
            //  var dal = new RepositoryBase<CityEntity>("PIZICHONG");
            //  Postgre的Schema
            //  var dal = new RepositoryBase<CityEntity>("test");
            List<CityEntity> list = dal.IQueryable().ToList();
            Console.WriteLine(JsonConvert.SerializeObject(list));
        }

        public static void TestSqlServer()
        {
            var dal = new RepositoryBase<CityEntity>(
                "Server=.;Initial Catalog=XavierTest;User ID=sa;Password=abc,,,123", 
                Xavier.Framework.Db.Models.DatabaseType.SqlServer
                );
            List<CityEntity> list = dal.IQueryable().ToList();
            Console.WriteLine("SqlServer : ");
            Console.WriteLine(JsonConvert.SerializeObject(list));
        }

        public static void TestMySql()
        {
            var dal = new RepositoryBase<CityEntity>(
                "server=localhost;user id=root;password=root;database=ormtest;charset=utf8",
                Xavier.Framework.Db.Models.DatabaseType.MySql
                );
            List<CityEntity> list = dal.IQueryable().ToList();
            Console.WriteLine("MySql : ");
            Console.WriteLine(JsonConvert.SerializeObject(list));
        }

        public static void TestOracle()
        {
            var dal = new RepositoryBase<CityEntity>(
                "Data Source=(DESCRIPTION = (ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = DESKTOP-RR0NQSH)(PORT = 1521)))(CONNECT_DATA =(SERVICE_NAME = XavierFrame)));User ID=pizichong;Password=wtball580166;",
                Xavier.Framework.Db.Models.DatabaseType.Oracle,
                "PIZICHONG"
                );
            List<CityEntity> list = dal.IQueryable().ToList();
            Console.WriteLine("Oracle : ");
            Console.WriteLine(JsonConvert.SerializeObject(list));
        }

        public static void TestSQLite()
        {
            var dal = new RepositoryBase<CityEntity>(
                @"Data Source=D:\Mine\MyCodings\MyCoding\Xavier.Frame\Tests.Framework.Db.Console\test.db;Version=3;",
                Xavier.Framework.Db.Models.DatabaseType.SQLite
                );
            List<CityEntity> list = dal.IQueryable().ToList();
            Console.WriteLine("SQLite : ");
            Console.WriteLine(JsonConvert.SerializeObject(list));
        }

        public static void TestPostgre()
        {
            var dal = new RepositoryBase<CityEntity>(
                "Server=192.168.210.235;Port=54322;UserId=postgres;Password=Data@2020;Database=postgres;",
                Xavier.Framework.Db.Models.DatabaseType.Postgre,
                "test"
                );
            List<CityEntity> list = dal.IQueryable().ToList();
            Console.WriteLine("Postgre : ");
            Console.WriteLine(JsonConvert.SerializeObject(list));
        }
    }
}
