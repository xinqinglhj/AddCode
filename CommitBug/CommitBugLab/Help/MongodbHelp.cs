using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using CommitBugLab.Model;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;

namespace CommitBugLab.Help
{

    internal class MongodbHelper
    {
        // private const string _conn = "mongodb://e2e100Development:shenmemima..01@192.168.31.129/ErrorDatabase";
        //private const string _conn = "mongodb://e2e100Development:shenmemima..01@123.56.129.104/ErrorDatabase";

        private static readonly string Conn = ConfigurationManager.ConnectionStrings["MangoDbConnection"].ConnectionString;
        private readonly IMongoDatabase _dbDatabase;

        public const string TbName = "ErrorMasterCollection";
        public const string DbName = "ErrorDatabase";

        public MongodbHelper()
        {

            // 创建数据连接
            var client = new MongoClient(Conn);
            // 获取指定数据库
            _dbDatabase = client.GetDatabase(DbName);
        }

        //添加一条数据
        public async Task Add<T>(T t)
        {
            // 获取表
            var collection = _dbDatabase.GetCollection<T>(TbName);

            // 插入
            await collection.InsertOneAsync(t);
        }

        //获取mongodb连接
        public IMongoCollection<T> GetCollection<T>()
        {
            var ts = new List<T>();
            return _dbDatabase.GetCollection<T>(TbName);
        }

        public async Task Delete<T>(int id)
        {
            var collection = _dbDatabase.GetCollection<T>(TbName);
            var filter = Builders<T>.Filter.Eq("id", id);
            await collection.DeleteOneAsync(filter);
        }

        //测试
        public Task<IEnumerable<T>> Ts<T>()
        {
            var client1 = new MongoClient(Conn);
            var database = client1.GetDatabase("ErrorDatabase");
            var collection = database.GetCollection<BsonDocument>("ErrorMasterCollection");
            var awit = collection.InsertOneAsync(new BsonDocument("Name", "Jack"));
            awit.Wait();
            var list = collection.Find(new BsonDocument("Name", "Jack"))
                .ToListAsync();
            list.Wait();

            foreach (var document in list.Result)
            {
                Console.WriteLine(document["Name"]);
            }

            return null;
        }

        ///万能获取全部方法
        public IEnumerable<BsonDocument> SelectBsonDocumentAll()
        {
            var list = GetCollection<BsonDocument>().Find(x => true).ToListAsync();
            list.Wait();
            return list.Result;

        }

        ///万能获取单个方法
        public IEnumerable<BsonDocument> SelectBsonDocumentSingle(string key, string value)
        {
            var list = GetCollection<BsonDocument>().Find(new BsonDocument(key, value)).ToListAsync();
            list.Wait();
            return list.Result;

        }

        /// <summary>
        /// 获取指定对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IEnumerable<T> SelectAll<T>()
        {
            var list = GetCollection<T>().Find(x => true).ToListAsync();
            list.Wait();
            return list.Result;

        }

        /// <summary>
        /// 根据单个属性获取id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BugModel GetSingle(string id)
        {
            var resual = GetCollection<BugModel>().Find(x => x.Id == id).ToListAsync();
            resual.Wait();
            var resualValue = resual.Result;
            return resualValue.Count > 0 ? resualValue[0] : null;
        }
    }
}
