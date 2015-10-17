using System;
using System.Collections.Generic;
using System.IO;
using CommitBugLab.Interface;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace CommitBugLab.Base
{
    internal class StreamDatabase : IDatabase
    {
        private const string ConnectionPath = "d:\\bug.bug";


        public List<BugModel> GetData(string guids)
        {
            var read = new StreamReader(ConnectionPath);
            return JsonConvert.DeserializeObject<List<BugModel>>(read.ReadToEnd());
        }

        public IEnumerable<BsonDocument> GetDataAll()
        {
            throw new NotImplementedException();
        }

        public BugModel GetBugModel(string guid)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BsonDocument> GetBugModelsAll()
        {
            throw new NotImplementedException();
        }


        public void SetData(BugModel model)
        {
            var write = new StreamWriter(ConnectionPath, true);
            write.WriteLine(model);
            write.Close();
        }
    }
}
