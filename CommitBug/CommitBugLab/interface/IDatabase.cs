using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommitBugLab
{
    interface IDatabase
    {
        void SetData(string text);
        string GetDataAll();

        void MongoSetData(Base.BugModel model);
    }
}
