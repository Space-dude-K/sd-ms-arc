
namespace Api_fsc_Interfaces
{
    interface ISpaceChecker
    {
        Tuple<ulong, ulong> CheckSpace(string ip, string disk, ILogger logger, RequisiteInformation req, ICypher cypher, bool isShare = false);
    }
}