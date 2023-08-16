// Copyright (c) 2005, Ben Baker
// All rights reserved.
// 
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree. 

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CoinDrop
{
	/// <summary>
	/// Summary description for Crypt.
	/// </summary>
	public class Crypt
	{
		private static Byte[] KEY_64 = {1, 2, 3, 4, 5, 6, 7, 8};
		private static Byte[] IV_64 = {8, 7, 6, 5, 4, 3, 2, 1};

		// returns DES encrypted string
		public static string DESEncrypt(string value)
		{
			DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            string retVal = null;

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, cryptoProvider.CreateEncryptor(KEY_64, IV_64), CryptoStreamMode.Write))
                {
                    using (StreamWriter sw = new StreamWriter(cs))
                    {
                        sw.Write(value);
                        sw.Flush();
                        cs.FlushFinalBlock();
                        ms.Flush();

                        retVal = System.Convert.ToBase64String(ms.GetBuffer(), 0, System.Convert.ToInt32(ms.Length));
                    }
                }
            }

			// convert back to a string
            return retVal;
		}

		// returns DES decrypted string
		public static string DESDecrypt(string value)
		{
			DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            Byte[] buffer = System.Convert.FromBase64String(value);
            string retVal = null;

            using (MemoryStream ms = new MemoryStream(buffer))
            {
                using (CryptoStream cs = new CryptoStream(ms, cryptoProvider.CreateDecryptor(KEY_64, IV_64), CryptoStreamMode.Read))
                {
                    using (StreamReader sr = new StreamReader(cs))
                        retVal = sr.ReadToEnd();
                }
            }

            return retVal;
		}

        public static string MD5Hash(string value)
        {
            byte[] hashedBytes;

            UTF8Encoding encoder = new UTF8Encoding();
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();

            hashedBytes = md5Hasher.ComputeHash(encoder.GetBytes(value));

            return System.Convert.ToBase64String(hashedBytes, 0, System.Convert.ToInt32(hashedBytes.Length));
        }
	}
}
