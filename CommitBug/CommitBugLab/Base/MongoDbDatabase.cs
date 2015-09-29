using System;
using MongoDB.Driver;
using Newtonsoft.Json;
using MongoDB.Bson;
using System.Configuration;

namespace CommitBugLab.Base
{
    public class MongoDbDatabase : IDatabase
    {

        public string GetDataAll()
        {
            throw new NotImplementedException();
        }

        public void MongoSetData(BugModel model)
        {
            MongodbHelper h = new MongodbHelper();

            model.BrowserInfo = JsonConvert.SerializeObject(model.BrowserInfo);
            model.exception = JsonConvert.SerializeObject(model.exception);
            var task = h.Add(model);
            //task.Wait();
        }

        public void SetData(string text)
        {


        }
    }
}
