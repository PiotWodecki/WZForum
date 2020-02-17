using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumWZ.Models.Forum
{
    public class ForumIndexModel
    {
        public IEnumerable<ForumListingModel> ForumList { get; set; } //to nam posłuży do zapakowania ienumerable forumlistingmodel
        //żeby przesłać to dalej kontrolerem
    }
}
