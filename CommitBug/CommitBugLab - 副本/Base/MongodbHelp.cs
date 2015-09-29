using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace CommitBugLab.Base
{

    public class MongodbHelper
    {
        //  private const string Conn = "mongodb://e2e100Development:shenmemima..01@192.168.31.129/ErrorDatabase";

        private string Conn = ConfigurationManager.ConnectionStrings["MangoDbConnection"].ConnectionString;

        private const string DbName = "ErrorDatabase";

        private const string TbName = "ErrorMasterCollection";

        private readonly MongoClient client;

        private readonly IMongoDatabase dbDatabase;

        public MongodbHelper()
        {
            // 创建数据连接
            client = new MongoClient(Conn);

            // 获取指定数据库
            dbDatabase = client.GetDatabase(DbName);
        }

        public async Task Add<T>(T t)
        {
            // 获取表
            var collection = dbDatabase.GetCollection<T>(TbName);

            // 插入
            await collection.InsertOneAsync(t);
        }

        public async Task Delete<T>(int id)
        {
            var collection = dbDatabase.GetCollection<T>(TbName);
            var filter = Builders<T>.Filter.Eq("id", id);

            await collection.DeleteOneAsync(filter);
        }

        public IEnumerable<T> SelectAll<T>()
        {
            var Ts = new List<T>();

            var collection = dbDatabase.GetCollection<T>(TbName);
            var result = collection.Find(p => true).ToListAsync();
            result.Wait();
            Ts.AddRange(result.Result);

            return Ts;
        }
    }
}
