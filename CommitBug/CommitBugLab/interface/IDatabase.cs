using System.Collections.Generic;
using CommitBugLab.Base;
using MongoDB.Bson;

namespace CommitBugLab.Interface
{
    interface IDatabase
    {
        void SetData(BugModel text);

        List<BugModel> GetData(string guid);

        IEnumerable<BsonDocument> GetDataAll();
        BugModel GetBugModel(string guid);
    }
}
