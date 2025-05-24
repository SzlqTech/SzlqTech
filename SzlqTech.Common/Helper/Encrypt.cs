using Masuit.Tools.Systems;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;


namespace SzlqTech.Common.Helper
{
    //
    // 摘要:
    //     常用的加密解密算法
    public static class Encrypt
    {
        private static byte[] Keys = new byte[16]
        {
        65, 114, 101, 121, 111, 117, 109, 121, 83, 110,
        111, 119, 109, 97, 110, 63
        };

        //
        // 摘要:
        //     加密字符串 加密密钥必须为8位
        //
        // 参数:
        //   strText:
        //     被加密的字符串
        //
        //   strEncrKey:
        //     8位长度密钥
        //
        // 返回结果:
        //     加密后的数据
        public static string DesEncrypt(this string strText, string strEncrKey)
        {
            if (strEncrKey.Length < 8)
            {
                throw new Exception("密钥长度无效，密钥必须是8位！");
            }

            StringBuilder stringBuilder = new StringBuilder();
            using DES dES = DES.Create();
            byte[] bytes = Encoding.Default.GetBytes(strText);
            dES.Key = Encoding.ASCII.GetBytes(strEncrKey.Substring(0, 8));
            dES.IV = Encoding.ASCII.GetBytes(strEncrKey.Substring(0, 8));
            using PooledMemoryStream pooledMemoryStream = new PooledMemoryStream();
            using CryptoStream cryptoStream = new CryptoStream(pooledMemoryStream, dES.CreateEncryptor(), CryptoStreamMode.Write);
            cryptoStream.Write(bytes, 0, bytes.Length);
            cryptoStream.FlushFinalBlock();
            foreach (byte item in pooledMemoryStream)
            {
                stringBuilder.AppendFormat($"{item:X2}");
            }

            return stringBuilder.ToString();
        }

        //
        // 摘要:
        //     加密字符串 加密密钥必须为8位
        //
        // 参数:
        //   strText:
        //     被加密的字符串
        //
        //   desKey:
        //
        //   desIV:
        //
        // 返回结果:
        //     加密后的数据
        public static string DesEncrypt(this string strText, byte[] desKey, byte[] desIV)
        {
            StringBuilder stringBuilder = new StringBuilder();
            using DES dES = DES.Create();
            byte[] bytes = Encoding.Default.GetBytes(strText);
            dES.Key = desKey;
            dES.IV = desIV;
            using PooledMemoryStream pooledMemoryStream = new PooledMemoryStream();
            using CryptoStream cryptoStream = new CryptoStream(pooledMemoryStream, dES.CreateEncryptor(), CryptoStreamMode.Write);
            cryptoStream.Write(bytes, 0, bytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] array = pooledMemoryStream.ToArray();
            foreach (byte b in array)
            {
                stringBuilder.AppendFormat($"{b:X2}");
            }

            return stringBuilder.ToString();
        }

        //
        // 摘要:
        //     DES加密文件
        //
        // 参数:
        //   fin:
        //     文件输入流
        //
        //   outFilePath:
        //     文件输出路径
        //
        //   strEncrKey:
        //     加密密钥
        public static void DesEncrypt(this FileStream fin, string outFilePath, string strEncrKey)
        {
            byte[] rgbIV = new byte[8] { 18, 52, 86, 120, 144, 171, 205, 239 };
            byte[] bytes = Encoding.UTF8.GetBytes(strEncrKey.Substring(0, 8));
            using FileStream fileStream = new FileStream(outFilePath, FileMode.OpenOrCreate, FileAccess.Write);
            fileStream.SetLength(0L);
            byte[] buffer = new byte[100];
            long num = 0L;
            long length = fin.Length;
            using DES dES = DES.Create();
            CryptoStream cryptoStream = new CryptoStream(fileStream, dES.CreateEncryptor(bytes, rgbIV), CryptoStreamMode.Write);
            int num2;
            for (; num < length; num += num2)
            {
                num2 = fin.Read(buffer, 0, 100);
                cryptoStream.Write(buffer, 0, num2);
            }
        }

        //
        // 摘要:
        //     DES加密文件
        //
        // 参数:
        //   fin:
        //     文件输入流
        //
        //   outFilePath:
        //     文件输出路径
        //
        //   desKey:
        //
        //   desIV:
        public static void DesEncrypt(this FileStream fin, string outFilePath, byte[] desKey, byte[] desIV)
        {
            using FileStream fileStream = new FileStream(outFilePath, FileMode.OpenOrCreate, FileAccess.Write);
            fileStream.SetLength(0L);
            byte[] buffer = new byte[100];
            long num = 0L;
            long length = fin.Length;
            using DES dES = DES.Create();
            CryptoStream cryptoStream = new CryptoStream(fileStream, dES.CreateEncryptor(desKey, desIV), CryptoStreamMode.Write);
            int num2;
            for (; num < length; num += num2)
            {
                num2 = fin.Read(buffer, 0, 100);
                cryptoStream.Write(buffer, 0, num2);
            }
        }

        //
        // 摘要:
        //     DES解密文件
        //
        // 参数:
        //   fin:
        //     输入文件流
        //
        //   outFilePath:
        //     文件输出路径
        //
        //   sDecrKey:
        //     解密密钥
        public static void DesDecrypt(this FileStream fin, string outFilePath, string sDecrKey)
        {
            byte[] rgbIV = new byte[8] { 18, 52, 86, 120, 144, 171, 205, 239 };
            byte[] bytes = Encoding.UTF8.GetBytes(sDecrKey.Substring(0, 8));
            using FileStream fileStream = new FileStream(outFilePath, FileMode.OpenOrCreate, FileAccess.Write);
            fileStream.SetLength(0L);
            byte[] buffer = new byte[100];
            long num = 0L;
            long length = fin.Length;
            using DES dES = DES.Create();
            CryptoStream cryptoStream = new CryptoStream(fileStream, dES.CreateDecryptor(bytes, rgbIV), CryptoStreamMode.Write);
            int num2;
            for (; num < length; num += num2)
            {
                num2 = fin.Read(buffer, 0, 100);
                cryptoStream.Write(buffer, 0, num2);
            }
        }

        //
        // 摘要:
        //     DES解密文件
        //
        // 参数:
        //   fin:
        //     输入文件流
        //
        //   outFilePath:
        //     文件输出路径
        //
        //   desKey:
        //
        //   desIV:
        public static void DesDecrypt(this FileStream fin, string outFilePath, byte[] desKey, byte[] desIV)
        {
            using FileStream fileStream = new FileStream(outFilePath, FileMode.OpenOrCreate, FileAccess.Write);
            fileStream.SetLength(0L);
            byte[] buffer = new byte[100];
            long num = 0L;
            long length = fin.Length;
            using DES dES = DES.Create();
            CryptoStream cryptoStream = new CryptoStream(fileStream, dES.CreateDecryptor(desKey, desIV), CryptoStreamMode.Write);
            int num2;
            for (; num < length; num += num2)
            {
                num2 = fin.Read(buffer, 0, 100);
                cryptoStream.Write(buffer, 0, num2);
            }
        }

        //
        // 摘要:
        //     DES解密算法 密钥为8位
        //
        // 参数:
        //   pToDecrypt:
        //     需要解密的字符串
        //
        //   sKey:
        //     密钥
        //
        // 返回结果:
        //     解密后的数据
        public static string DesDecrypt(this string pToDecrypt, string sKey)
        {
            if (sKey.Length < 8)
            {
                throw new Exception("密钥长度无效，密钥必须是8位！");
            }

            using PooledMemoryStream pooledMemoryStream = new PooledMemoryStream();
            using DES dES = DES.Create();
            byte[] array = new byte[pToDecrypt.Length / 2];
            for (int i = 0; i < pToDecrypt.Length / 2; i++)
            {
                int num = Convert.ToInt32(pToDecrypt.Substring(i * 2, 2), 16);
                array[i] = (byte)num;
            }

            dES.Key = Encoding.ASCII.GetBytes(sKey.Substring(0, 8));
            dES.IV = Encoding.ASCII.GetBytes(sKey.Substring(0, 8));
            using CryptoStream cryptoStream = new CryptoStream(pooledMemoryStream, dES.CreateDecryptor(), CryptoStreamMode.Write);
            cryptoStream.Write(array, 0, array.Length);
            cryptoStream.FlushFinalBlock();
            return Encoding.Default.GetString(pooledMemoryStream.ToArray());
        }

        //
        // 摘要:
        //     DES解密算法 密钥为8位
        //
        // 参数:
        //   pToDecrypt:
        //     需要解密的字符串
        //
        //   desKey:
        //
        //   desIV:
        //
        // 返回结果:
        //     解密后的数据
        public static string DesDecrypt(this string pToDecrypt, byte[] desKey, byte[] desIV)
        {
            using PooledMemoryStream pooledMemoryStream = new PooledMemoryStream();
            using DES dES = DES.Create();
            byte[] array = new byte[pToDecrypt.Length / 2];
            for (int i = 0; i < pToDecrypt.Length / 2; i++)
            {
                int num = Convert.ToInt32(pToDecrypt.Substring(i * 2, 2), 16);
                array[i] = (byte)num;
            }

            dES.Key = desKey;
            dES.IV = desIV;
            using CryptoStream cryptoStream = new CryptoStream(pooledMemoryStream, dES.CreateDecryptor(), CryptoStreamMode.Write);
            cryptoStream.Write(array, 0, array.Length);
            cryptoStream.FlushFinalBlock();
            return Encoding.Default.GetString(pooledMemoryStream.ToArray());
        }

        //
        // 摘要:
        //     生成符合AES加密规则的密钥
        //
        // 参数:
        //   length:
        public static string GenerateAesKey(int length)
        {
            using Aes aes = Aes.Create();
            aes.KeySize = length;
            aes.BlockSize = 128;
            aes.GenerateKey();
            return Convert.ToBase64String(aes.Key);
        }

        //
        // 摘要:
        //     对称加密算法AES(块式加密算法)
        //
        // 参数:
        //   encryptString:
        //     待加密字符串
        //
        //   encryptKey:
        //     加密密钥，须半角字符
        //
        //   mode:
        //     加密模式
        //
        // 返回结果:
        //     加密结果字符串
        public static string AESEncrypt(this string encryptString, string encryptKey, CipherMode mode = CipherMode.CBC)
        {
            encryptKey = encryptKey.GetSubString(32, "");
            encryptKey = encryptKey.PadRight(32, ' ');
            using Aes aes = Aes.Create("AesManaged");
            aes.Key = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 32));
            aes.IV = Keys;
            aes.Mode = mode;
            using ICryptoTransform cryptoTransform = aes.CreateEncryptor();
            byte[] bytes = Encoding.UTF8.GetBytes(encryptString);
            return Convert.ToBase64String(cryptoTransform.TransformFinalBlock(bytes, 0, bytes.Length));
        }

        //
        // 摘要:
        //     对称加密算法AES加密(块式加密算法)
        //
        // 参数:
        //   encryptString:
        //     待加密字符串
        //
        //   options:
        //     加密选项
        //
        // 返回结果:
        //     加密结果字符串
        public static string AESEncrypt(this string encryptString, Aes options)
        {
            using ICryptoTransform cryptoTransform = options.CreateEncryptor();
            byte[] bytes = Encoding.UTF8.GetBytes(encryptString);
            return Convert.ToBase64String(cryptoTransform.TransformFinalBlock(bytes, 0, bytes.Length));
        }

        //
        // 摘要:
        //     对称加密算法AES加密(块式加密算法)
        //
        // 参数:
        //   encryptString:
        //     待加密字符串
        //
        //   encryptKey:
        //     加密密钥，须半角字符
        //
        //   mode:
        //     加密模式
        //
        // 返回结果:
        //     加密结果字符串
        public static string AESEncrypt(this string encryptString, byte[] encryptKey, CipherMode mode = CipherMode.CBC)
        {
            using Aes aes = Aes.Create("AesManaged");
            aes.Key = encryptKey;
            aes.IV = Keys;
            aes.Mode = mode;
            using ICryptoTransform cryptoTransform = aes.CreateEncryptor();
            byte[] bytes = Encoding.UTF8.GetBytes(encryptString);
            return Convert.ToBase64String(cryptoTransform.TransformFinalBlock(bytes, 0, bytes.Length));
        }

        //
        // 摘要:
        //     对称加密算法AES解密字符串
        //
        // 参数:
        //   decryptString:
        //     待解密的字符串
        //
        //   decryptKey:
        //     解密密钥,和加密密钥相同
        //
        //   mode:
        //     加密模式
        //
        // 返回结果:
        //     解密成功返回解密后的字符串,失败返回空
        public static string AESDecrypt(this string decryptString, string decryptKey, CipherMode mode = CipherMode.CBC)
        {
            try
            {
                decryptKey = decryptKey.GetSubString(32, "");
                decryptKey = decryptKey.PadRight(32, ' ');
                using Aes aes = Aes.Create("AesManaged");
                aes.Key = Encoding.UTF8.GetBytes(decryptKey);
                aes.IV = Keys;
                aes.Mode = mode;
                using ICryptoTransform cryptoTransform = aes.CreateDecryptor();
                byte[] array = Convert.FromBase64String(decryptString);
                byte[] bytes = cryptoTransform.TransformFinalBlock(array, 0, array.Length);
                return Encoding.UTF8.GetString(bytes);
            }
            catch
            {
                return string.Empty;
            }
        }

        //
        // 摘要:
        //     对称加密算法AES解密字符串
        //
        // 参数:
        //   decryptString:
        //     待解密的字符串
        //
        //   options:
        //     加密选项
        //
        // 返回结果:
        //     解密成功返回解密后的字符串,失败返回空
        public static string AESDecrypt(this string decryptString, Aes options)
        {
            try
            {
                using ICryptoTransform cryptoTransform = options.CreateDecryptor();
                byte[] array = Convert.FromBase64String(decryptString);
                byte[] bytes = cryptoTransform.TransformFinalBlock(array, 0, array.Length);
                return Encoding.UTF8.GetString(bytes);
            }
            catch
            {
                return string.Empty;
            }
        }

        //
        // 摘要:
        //     对称加密算法AES解密字符串
        //
        // 参数:
        //   decryptString:
        //     待解密的字符串
        //
        //   decryptKey:
        //     解密密钥,和加密密钥相同
        //
        //   mode:
        //     加密模式
        //
        // 返回结果:
        //     解密成功返回解密后的字符串,失败返回空
        public static string AESDecrypt(this string decryptString, byte[] decryptKey, CipherMode mode = CipherMode.CBC)
        {
            try
            {
                using Aes aes = Aes.Create("AesManaged");
                aes.Key = decryptKey;
                aes.IV = Keys;
                aes.Mode = mode;
                using ICryptoTransform cryptoTransform = aes.CreateDecryptor();
                byte[] array = Convert.FromBase64String(decryptString);
                byte[] bytes = cryptoTransform.TransformFinalBlock(array, 0, array.Length);
                return Encoding.UTF8.GetString(bytes);
            }
            catch
            {
                return string.Empty;
            }
        }

        //
        // 摘要:
        //     按字节长度(按字节,一个汉字为2个字节)取得某字符串的一部分
        //
        // 参数:
        //   sourceString:
        //     源字符串
        //
        //   length:
        //     所取字符串字节长度
        //
        //   tailString:
        //     附加字符串(当字符串不够长时，尾部所添加的字符串，一般为"...")
        //
        // 返回结果:
        //     某字符串的一部分
        private static string GetSubString(this string sourceString, int length, string tailString)
        {
            return sourceString.GetSubString(0, length, tailString);
        }

        //
        // 摘要:
        //     按字节长度(按字节,一个汉字为2个字节)取得某字符串的一部分
        //
        // 参数:
        //   sourceString:
        //     源字符串
        //
        //   startIndex:
        //     索引位置，以0开始
        //
        //   length:
        //     所取字符串字节长度
        //
        //   tailString:
        //     附加字符串(当字符串不够长时，尾部所添加的字符串，一般为"...")
        //
        // 返回结果:
        //     某字符串的一部分
        private static string GetSubString(this string sourceString, int startIndex, int length, string tailString)
        {
            if (Regex.IsMatch(sourceString, "[ࠀ-一]+") || Regex.IsMatch(sourceString, "[가-힣]+"))
            {
                if (startIndex >= sourceString.Length)
                {
                    return string.Empty;
                }

                return sourceString.Substring(startIndex, (length + startIndex > sourceString.Length) ? (sourceString.Length - startIndex) : length);
            }

            if (length <= 0)
            {
                return string.Empty;
            }

            byte[] bytes = Encoding.Default.GetBytes(sourceString);
            if (bytes.Length > startIndex)
            {
                int num = bytes.Length;
                if (bytes.Length > startIndex + length)
                {
                    num = length + startIndex;
                }
                else
                {
                    length = bytes.Length - startIndex;
                    tailString = "";
                }

                int[] array = new int[length];
                int num2 = 0;
                for (int i = startIndex; i < num; i++)
                {
                    if (bytes[i] > 127)
                    {
                        num2++;
                        if (num2 == 3)
                        {
                            num2 = 1;
                        }
                    }
                    else
                    {
                        num2 = 0;
                    }

                    array[i] = num2;
                }

                if (bytes[num - 1] > 127 && array[length - 1] == 1)
                {
                    length++;
                }

                byte[] array2 = new byte[length];
                Array.Copy(bytes, startIndex, array2, 0, length);
                return Encoding.Default.GetString(array2) + tailString;
            }

            return string.Empty;
        }

        //
        // 摘要:
        //     加密文件流
        //
        // 参数:
        //   fs:
        //     需要加密的文件流
        //
        //   decryptKey:
        //     加密密钥
        //
        //   mode:
        //     加密模式
        //
        // 返回结果:
        //     加密流
        public static CryptoStream AESEncryptStrream(this FileStream fs, string decryptKey, CipherMode mode = CipherMode.CBC)
        {
            decryptKey = decryptKey.GetSubString(32, "");
            decryptKey = decryptKey.PadRight(32, ' ');
            using Aes aes = Aes.Create("AesManaged");
            aes.Key = Encoding.UTF8.GetBytes(decryptKey);
            aes.IV = Keys;
            aes.Mode = mode;
            using ICryptoTransform transform = aes.CreateEncryptor();
            return new CryptoStream(fs, transform, CryptoStreamMode.Write);
        }

        //
        // 摘要:
        //     加密文件流
        //
        // 参数:
        //   fs:
        //     需要加密的文件流
        //
        //   decryptKey:
        //     加密密钥
        //
        //   mode:
        //     加密模式
        //
        // 返回结果:
        //     加密流
        public static CryptoStream AESEncryptStrream(this FileStream fs, byte[] decryptKey, CipherMode mode = CipherMode.CBC)
        {
            using Aes aes = Aes.Create("AesManaged");
            aes.Key = decryptKey;
            aes.IV = Keys;
            aes.Mode = mode;
            using ICryptoTransform transform = aes.CreateEncryptor();
            return new CryptoStream(fs, transform, CryptoStreamMode.Write);
        }

        //
        // 摘要:
        //     解密文件流
        //
        // 参数:
        //   fs:
        //     需要解密的文件流
        //
        //   decryptKey:
        //     解密密钥
        //
        //   mode:
        //     加密模式
        //
        // 返回结果:
        //     加密流
        public static CryptoStream AESDecryptStream(this FileStream fs, string decryptKey, CipherMode mode = CipherMode.CBC)
        {
            decryptKey = decryptKey.GetSubString(32, "");
            decryptKey = decryptKey.PadRight(32, ' ');
            using Aes aes = Aes.Create("AesManaged");
            aes.Key = Encoding.UTF8.GetBytes(decryptKey);
            aes.IV = Keys;
            aes.Mode = mode;
            using ICryptoTransform transform = aes.CreateDecryptor();
            return new CryptoStream(fs, transform, CryptoStreamMode.Read);
        }

        //
        // 摘要:
        //     解密文件流
        //
        // 参数:
        //   fs:
        //     需要解密的文件流
        //
        //   decryptKey:
        //     解密密钥
        //
        //   mode:
        //     加密模式
        //
        // 返回结果:
        //     加密流
        public static CryptoStream AESDecryptStream(this FileStream fs, byte[] decryptKey, CipherMode mode = CipherMode.CBC)
        {
            using Aes aes = Aes.Create("AesManaged");
            aes.Key = decryptKey;
            aes.IV = Keys;
            aes.Mode = mode;
            using ICryptoTransform transform = aes.CreateDecryptor();
            return new CryptoStream(fs, transform, CryptoStreamMode.Read);
        }

        //
        // 摘要:
        //     对指定文件AES加密
        //
        // 参数:
        //   input:
        //     源文件流
        //
        //   key:
        //     加密密钥
        //
        //   mode:
        //     加密模式
        //
        //   outputPath:
        //     输出文件路径
        public static void AESEncryptFile(this FileStream input, string outputPath, string key, CipherMode mode = CipherMode.CBC)
        {
            using FileStream fs = new FileStream(outputPath, FileMode.Create);
            using CryptoStream cryptoStream = fs.AESEncryptStrream(key, mode);
            byte[] array = new byte[input.Length];
            input.Read(array, 0, array.Length);
            cryptoStream.Write(array, 0, array.Length);
        }

        //
        // 摘要:
        //     对指定的文件AES解密
        //
        // 参数:
        //   input:
        //     源文件流
        //
        //   key:
        //     解密密钥
        //
        //   mode:
        //     加密模式
        //
        //   outputPath:
        //     输出文件路径
        public static void AESDecryptFile(this FileStream input, string outputPath, string key, CipherMode mode = CipherMode.CBC)
        {
            using FileStream fileStream = new FileStream(outputPath, FileMode.Create);
            using CryptoStream cryptoStream = input.AESDecryptStream(key, mode);
            byte[] array = new byte[1024];
            int num;
            do
            {
                num = cryptoStream.Read(array, 0, array.Length);
                fileStream.Write(array, 0, num);
            }
            while (num >= array.Length);
        }

        //
        // 摘要:
        //     Base64加密
        //
        // 参数:
        //   str:
        //     需要加密的字符串
        //
        // 返回结果:
        //     加密后的数据
        public static string Base64Encrypt(this string str)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(str));
        }

        //
        // 摘要:
        //     Base64解密
        //
        // 参数:
        //   str:
        //     需要解密的字符串
        //
        // 返回结果:
        //     解密后的数据
        public static string Base64Decrypt(this string str)
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(str);
                return Encoding.UTF8.GetString(bytes);
            }
            catch
            {
                return str;
            }
        }

        //
        // 摘要:
        //     SHA256函数
        //
        // 参数:
        //   str:
        //     原始字符串
        //
        // 返回结果:
        //     SHA256结果(返回长度为44字节的字符串)
        public static string SHA256(this string str)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            using SHA256 sHA = System.Security.Cryptography.SHA256.Create();
            return Convert.ToBase64String(sHA.ComputeHash(bytes));
        }

        //
        // 摘要:
        //     对字符串进行MD5摘要
        //
        // 参数:
        //   message:
        //     需要摘要的字符串
        //
        // 返回结果:
        //     MD5摘要字符串
        public static string MDString(this string message)
        {
            MD5 mD = MD5.Create();
            byte[] bytes = Encoding.Default.GetBytes(message);
            return GetHexString(mD.ComputeHash(bytes));
        }

        //
        // 摘要:
        //     对字符串进行MD5二次摘要
        //
        // 参数:
        //   message:
        //     需要摘要的字符串
        //
        // 返回结果:
        //     MD5摘要字符串
        public static string MDString2(this string message)
        {
            return message.MDString().MDString();
        }

        //
        // 摘要:
        //     MD5 三次摘要算法
        //
        // 参数:
        //   s:
        //     需要摘要的字符串
        //
        // 返回结果:
        //     MD5摘要字符串
        public static string MDString3(this string s)
        {
            using MD5 mD = MD5.Create();
            byte[] bytes = Encoding.ASCII.GetBytes(s);
            byte[] buffer = mD.ComputeHash(bytes);
            byte[] buffer2 = mD.ComputeHash(buffer);
            return GetHexString(mD.ComputeHash(buffer2));
        }

        //
        // 摘要:
        //     对字符串进行MD5加盐摘要
        //
        // 参数:
        //   message:
        //     需要摘要的字符串
        //
        //   salt:
        //     盐
        //
        // 返回结果:
        //     MD5摘要字符串
        public static string MDString(this string message, string salt)
        {
            return (message + salt).MDString();
        }

        //
        // 摘要:
        //     对字符串进行MD5二次加盐摘要
        //
        // 参数:
        //   message:
        //     需要摘要的字符串
        //
        //   salt:
        //     盐
        //
        // 返回结果:
        //     MD5摘要字符串
        public static string MDString2(this string message, string salt)
        {
            return (message + salt).MDString().MDString(salt);
        }

        //
        // 摘要:
        //     MD5 三次摘要算法
        //
        // 参数:
        //   s:
        //     需要摘要的字符串
        //
        //   salt:
        //     盐
        //
        // 返回结果:
        //     MD5摘要字符串
        public static string MDString3(this string s, string salt)
        {
            using MD5 mD = MD5.Create();
            byte[] bytes = Encoding.ASCII.GetBytes(s + salt);
            byte[] buffer = mD.ComputeHash(bytes);
            byte[] buffer2 = mD.ComputeHash(buffer);
            return GetHexString(mD.ComputeHash(buffer2));
        }

        //
        // 摘要:
        //     获取文件的MD5值
        //
        // 参数:
        //   fileName:
        //     需要求MD5值的文件的文件名及路径
        //
        // 返回结果:
        //     MD5摘要字符串
        public static string MDFile(this string fileName)
        {
            using BufferedStream inputStream = new BufferedStream(File.Open(fileName, FileMode.Open, FileAccess.Read), 1048576);
            using MD5 mD = MD5.Create();
            return GetHexString(mD.ComputeHash(inputStream));
        }

        //
        // 摘要:
        //     计算文件的sha256
        //
        // 参数:
        //   stream:
        public static string SHA256(this Stream stream)
        {
            using BufferedStream inputStream = new BufferedStream(stream, 1048576);
            using SHA256 sHA = System.Security.Cryptography.SHA256.Create();
            return BitConverter.ToString(sHA.ComputeHash(inputStream)).Replace("-", string.Empty);
        }

        //
        // 摘要:
        //     获取数据流的MD5摘要值
        //
        // 参数:
        //   stream:
        //
        // 返回结果:
        //     MD5摘要字符串
        public static string MDString(this Stream stream)
        {
            using BufferedStream inputStream = new BufferedStream(stream, 1048576);
            using MD5 mD = MD5.Create();
            string hexString = GetHexString(mD.ComputeHash(inputStream));
            stream.Position = 0L;
            return hexString;
        }

        public static string GetHexString(byte[] bytes)
        {
            char[] array = new char[bytes.Length << 1];
            for (int j = 0; j < array.Length; j += 2)
            {
                byte b = bytes[j >> 1];
                array[j] = GetHexValue(b >> 4);
                array[j + 1] = GetHexValue(b & 0xF);
            }

            return new string(array, 0, array.Length);
            static char GetHexValue(int i)
            {
                if (i < 10)
                {
                    return (char)(i + 48);
                }

                return (char)(i - 10 + 97);
            }
        }
    }
}
