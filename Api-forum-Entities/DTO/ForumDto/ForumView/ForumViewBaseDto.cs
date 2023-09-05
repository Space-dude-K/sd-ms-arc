using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO.ForumDto.ForumView
{
    public class ForumViewBaseDto : ForumBaseDto
    {
        public int TotalPosts { get; set; }
        public int TotalViews { get; set; }
        public int TopicsCount { get; set; }
    }
}
