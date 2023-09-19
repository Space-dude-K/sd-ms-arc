using Entities.DTO.ForumDto.Manipulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO.ForumDto.Create
{
    public class ForumCounterForCreationDto : ForumTopicCounterForManipulationDto
    {
        public int ForumTopicId { get; set; }
    }
}
