using System;
using System.Collections.Generic;
using MongoDB.Driver;
using Newtonsoft.Json;
using MongoDB.Bson;
using System.Configuration;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using CommitBugLab.Help;
using CommitBugLab.Interface;

namespace CommitBugLab.Base
{
    internal class MongoDbDatabase : IDatabase
    {
        readonly MongodbHelper _help = new MongodbHelper();
        public void SetData(BugModel model)
        {
            model.BrowserInfo = JsonConvert.SerializeObject(model.BrowserInfo);
            model.Exception = JsonConvert.SerializeObject(model.Exception);
            var task = _help.Add(model);
            //task.Wait();
        }

        public List<BugModel> GetData(string guid)
        {
            var items = _help.GetSingle(guid);
            return null;
        }

        public IEnumerable<BsonDocument> GetDataAll()
        {
            var reus = _help.SelectBsonDocumentAll();
            return reus;
        }

        public BugModel GetBugModel(string guid)
        {
            
            return _help.GetSingle(guid);
        }
    }
}
