using System;
using System.Web;
namespace CommitBugLab.Base
{
    public class BugModel
    {

        public BugModel()
        {
            this.BugCreateTime = DateTime.Now.ToString("yy-MM-dd HH:mm:ss");
            this.Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
        /// <summary>
        /// bug Name or bug Primary
        /// </summary>
        public string BugCreateTime { get; set; }

        public string BrowserInfo { get; set; }

        public string Url { get; set; }

        public string Exception { get; set; }

        public string Ip { get; set; }

        public string Session { get; set; }

        public string BrowserName { get; set; }

    }
}
