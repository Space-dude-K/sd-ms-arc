
using Api_fsc_Entities.Models;

namespace Api_fsc_Interfaces
{
    public interface ISpaceChecker
    {
        Tuple<ulong, ulong> CheckSpace(string ip, string disk, RequisiteInformation req, ICypher cypher, bool isShare = false);
    }
}