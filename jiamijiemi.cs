using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace 自动压缩视频
{

    public static class jiamijiemi
    {

        //设置AES加密解密参数
        private static RijndaelManaged Setting()
        {
            RijndaelManaged rijndaelCipher = new RijndaelManaged
            {
                Key = Encoding.UTF8.GetBytes("TestTestTest0est"), //加密密钥,自己设置，长度必须为16字节的倍数
                IV = Encoding.UTF8.GetBytes("123456781234567o"),  //加密的iv偏移量,长度必须为16字节的倍数
                Mode = CipherMode.CBC,       //加密模式，ECB、CBC、CFB等
                Padding = PaddingMode.PKCS7, //待加密的明文长度不满足条件时使用的填充模式，PKCS7是python中默认的填充模式
                BlockSize = 128              //加密操作的块大小
            };
            return rijndaelCipher;
        }


        //加密字符串，并保存为base64编码格式的字符串
        public static string EncryptStr(string encryptStr)
        {
            string decryptStr = string.Empty;
            try
            {
                //将待加密的明文字符串转为加密所需的字节数组格式
                byte[] plainText = Encoding.UTF8.GetBytes(encryptStr);

                //设定加密参数
                RijndaelManaged rijndaelCipher = Setting();

                //加密字符串
                ICryptoTransform transform = rijndaelCipher.CreateEncryptor();
                byte[] cipherBytes = transform.TransformFinalBlock(plainText, 0, plainText.Length);

                //将加密后的字节数组转为base64格式字符串
                decryptStr = Convert.ToBase64String(cipherBytes);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return decryptStr;
        }

        //解密base64编码格式的字符串
        public static string DecryptStr(string decryptStr)
        {
            string encryptStr = string.Empty;
            try
            {
                //将待解密的base64格式字符串解码为加密所需的字节数组格式
                byte[] plainText = Convert.FromBase64String(decryptStr);

                //设定解密参数
                RijndaelManaged rijndaelCipher = Setting();

                //解密字符串
                ICryptoTransform transform = rijndaelCipher.CreateDecryptor();
                byte[] cipherBytes = transform.TransformFinalBlock(plainText, 0, plainText.Length);

                //将解密后的字节数组转为字符串
                encryptStr = Encoding.UTF8.GetString(cipherBytes);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return encryptStr;
        }


    }
}
