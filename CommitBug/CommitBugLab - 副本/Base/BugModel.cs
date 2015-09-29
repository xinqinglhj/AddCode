using System;
using System.Web;
namespace CommitBugLab.Base
{
    public class BugModel
    {

        public BugModel()
        {
            this.BugCreateTime = DateTime.Now;
            this.Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
        /// <summary>
        /// bug Name or bug Primary
        /// </summary>
        public DateTime BugCreateTime { get; set; }

        public dynamic BrowserInfo { get; set; }

        public string Url { get; set; }

        public object exception { get; set; }

        public string Ip { get; set; }

        public object Session { get; set; }

        public string BrowserName { get; set; }

    }
}
