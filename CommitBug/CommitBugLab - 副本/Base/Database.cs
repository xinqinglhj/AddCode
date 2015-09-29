using System;
using System.IO;

namespace CommitBugLab.Base
{
    internal class StreamDatabase : IDatabase
    {
        private const string ConnectionPath = "d:\\bug.bug";

        public string GetDataAll()
        {
            StreamReader read = new StreamReader(ConnectionPath);
            return read.ReadToEnd();
        }

        public string MongoSetData(BugModel model)
        {
            throw new NotImplementedException();
        }

        public void SetData(string text)
        {
            StreamWriter write = new StreamWriter(ConnectionPath, true);
            write.WriteLine(text);
            write.Close();
        }

        void IDatabase.MongoSetData(BugModel model)
        {
            throw new NotImplementedException();
        }


    }
}
