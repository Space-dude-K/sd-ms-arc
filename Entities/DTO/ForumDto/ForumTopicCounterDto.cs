using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO.ForumDto
{
    public class ForumTopicCounterDto
    {
        public int Id { get; set; } 
        public int ForumTopicId { get; set; }
        public int PostCounter { get; set; } = 0;
    }
}
