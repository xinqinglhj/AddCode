using System;
using Newtonsoft.Json;

namespace CommitBugLab.Base
{
    public class CommitBugBase : IBugControllerInterface
    {
        readonly IDatabase _data = new MongoDbDatabase();
        public void SetBug(BugModel model)
        {
            string jsonText = JsonConvert.SerializeObject(model);
            _data.SetData(jsonText);
        }


        public void GetBugAll()
        {
            _data.GetDataAll();
        }


        public string SaveBug(Exception ex)
        {
            var guid = Guid.NewGuid().ToString();
            ex.HelpLink = guid;
            string jsonText = JsonConvert.SerializeObject(ex);
            _data.SetData(jsonText);
            return guid;
        }

        public string SaveBug(BugModel model)
        {
            var guid = Guid.NewGuid().ToString();
            _data.MongoSetData(model);
            return guid;
        }
    }
}
