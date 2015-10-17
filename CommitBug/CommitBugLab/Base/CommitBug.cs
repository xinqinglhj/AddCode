using System;
using System.Collections.Generic;
using CommitBugLab.Interface;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace CommitBugLab.Base
{
    public class CommitBugBase
    {
        readonly IDatabase _data = new MongoDbDatabase();
        public List<BugModel> GetData(string guid)
        {
            return _data.GetData(guid);
        }

        public void SetData(BugModel model)
        {
            _data.SetData(model);
        }
        /// <summary>
        /// 获取所有的对象，BsonDocument类型
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BsonDocument> GetBugModelsAll()
        {
            return _data.GetDataAll();
        }
        /// <summary>
        /// 获取对象，来自一个集合，自动转换
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public BugModel GetBugModel(string guid)
        {
            return _data.GetBugModel(guid);
        }
    }
}
