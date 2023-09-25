using Api_fsc_Entities.Models;
using System.Security;

namespace Api_fsc_Interfaces
{
    public interface ICypher
    {
        RequisiteInformation Encrypt(
            SecureString user, SecureString pass);
        RequisiteInformation Decrypt(
            SecureString userEncrypted, string uSalt, SecureString passEncrypted, string pSalt);
        string ToInsecureString(SecureString input);
        SecureString ToSecureString(string input);
        SecureString DecryptString(SecureString encryptedData, string salt);
    }
}