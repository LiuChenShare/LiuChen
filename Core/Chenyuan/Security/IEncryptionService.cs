using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan.Security
{
    /// <summary>
    /// 数据解密服务定义
    /// </summary>
    public interface IEncryptionService
    {
        /// <summary>
        /// Create salt key
        /// </summary>
        /// <param name="size">Key size</param>
        /// <param name="refdata">参考数据对象</param>
        /// <returns>Salt key</returns>
        string CreateSaltKey(int size, byte[] refdata = null);

        /// <summary>
        /// Create a password hash
        /// </summary>
        /// <param name="password">{assword</param>
        /// <param name="saltkey">Salk key</param>
        /// <param name="passwordFormat">Password format (hash algorithm)</param>
        /// <returns>Password hash</returns>
        string CreatePasswordHash(string password, string saltkey, string passwordFormat = "SHA1");

        /// <summary>
        /// Encrypt text
        /// </summary>
        /// <param name="plainText">Text to encrypt</param>
        /// <param name="encryptionPrivateKey">Encryption private key</param>
        /// <returns>Encrypted text</returns>
        string EncryptText(string plainText, string encryptionPrivateKey = "");

        /// <summary>
        /// Decrypt text
        /// </summary>
        /// <param name="cipherText">Text to decrypt</param>
        /// <param name="encryptionPrivateKey">Encryption private key</param>
        /// <returns>Decrypted text</returns>
        string DecryptText(string cipherText, string encryptionPrivateKey = "");

        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="encryptString">encryptString</param>
        /// <param name="saltKey">saltKey</param>
        /// <returns></returns>
        string DesEncrypt(string encryptString, string saltKey);

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="decryptString">decryptString</param>
        /// <param name="saltKey">saltKey</param>
        /// <returns></returns>
        string DesDecrypt(string decryptString, string saltKey);
    }
}
