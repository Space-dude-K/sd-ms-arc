using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models.Forum
{
    public class ForumAccountType
    {
        public int Id { get; set; }
        public string TypeName { get; set; }

        public virtual ForumAccount? ForumAccount { get; set; }
    }
}
