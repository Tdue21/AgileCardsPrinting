// ********************************************************************
// * Copyright © 2014 Scanvaegt Systems A/S
// *
// * This file is the property of Scanvaegt Systems A/S and may not be 
// * distributed without the written consent of the author.    
// ********************************************************************

using System;
using System.IO;
using System.Security.Cryptography;

namespace PrintIssueCards.Common
{
    internal static class EncryptionHelper
    {
        private static readonly byte[] RgbIv =
        {
            0xAA, 0xE7, 0xE4, 0xDD, 0x73, 0x2D, 0x3B, 0x0D, 0xAF, 0x7D, 0xA4, 0x66, 0x99, 0x11, 0xE1, 0x45
        };

        private static readonly byte[] RgbKey =
        {
            0xE6, 0xE7, 0xB7, 0x32, 0xDF, 0x03, 0x45, 0x00, 0x84, 0x51, 0x51, 0x47, 0x4C, 0x84, 0x33, 0x9E,
            0x09, 0x10, 0x49, 0x41, 0xB8, 0x09, 0x13, 0xF4, 0x43, 0x5A, 0x08, 0xF4, 0xBE, 0xAA, 0x08, 0x64
        };


        internal static string Decrypt(string password)
        {
            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }
            var cipherBytes = Convert.FromBase64String(password);
            using (var alg = new RijndaelManaged())
            {
                var decryptor = alg.CreateDecryptor(RgbKey, RgbIv);
                using (var ms = new MemoryStream(cipherBytes))
                {
                    using (var decrypt = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (var reader = new StreamReader(decrypt))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
        }

        internal static string Encrypt(string password)
        {
            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }
            byte[] encrypted;

            using (var alg = new RijndaelManaged())
            {
                var decryptor = alg.CreateEncryptor(RgbKey, RgbIv);
                using (var ms = new MemoryStream())
                {
                    using (var decrypt = new CryptoStream(ms, decryptor, CryptoStreamMode.Write))
                    {
                        using (var writer = new StreamWriter(decrypt))
                        {
                            writer.Write(password);
                        }
                    }
                    encrypted = ms.ToArray();
                }
            }

            return Convert.ToBase64String(encrypted);
        }
    }
}
