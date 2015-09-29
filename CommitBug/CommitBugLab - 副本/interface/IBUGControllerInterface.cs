using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommitBugLab.Base;

namespace CommitBugLab
{
    public interface IBugControllerInterface
    {
        string SaveBug(Exception ex);

        string SaveBug(BugModel model);
    }
}
