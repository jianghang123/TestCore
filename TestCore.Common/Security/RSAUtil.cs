using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace TestCore.Common.Security
{
   public  class RSAUtil
    {

        //openssl genrsa -out rsa_1024_priv.pem 1024
        private static readonly string _privateKey = @"MIIEpAIBAAKCAQEAvvsg3NdmKIXUmCiyZPO46vVMmPW07TQZ2Acs8Eadbw1IM+en5Igb1X8BNVO/9ISFQGsX8NTVWsI9sSbFv/4ipNe6CT/z/bwEVptM42uL1KFF7RvaXLkZQACxoSCiikqU/hRgkD6B7ERS3ITXzzUh5dvPNi/K6jAgb6wSxdTxr13/c91jMLIbmaS6cpX7s2Aa+1cWWBA3xL6fs/hxfkGj3MX4wlJkDx8GnVRi85xcjHDRcx0BMi/u9jiV0L0J6TeMxU5CWWfsu/8XJ9AwgiFF0AfwsskmweYdbc0nRuLkAcv2BezGnWkODknSQIV/NUDHIM5xvQvqWE0ycIn0aJfqTwIDAQABAoIBAQCd14Zon6nyMfHsNC6E2x5pKoW9Ic4AAgCeGAgfKe2yO+MB6yNK8Oc4Q7im72oF2IUdIaXuyKjxUvqT8Hyd2pedcCJ0xKOOgReA0OfySg+OlB/sAQovelgRGsG0bvmHnG6ZtBO/A7pzoGBvm2eV/M7gYBRZL5Tgixgz057MXNvHWFUNg+NS49KOSHapvJS2foqc8KEZQw6zUAg3mBtLMgdqbSgm8ndazDfh7+2xpofLFL547a84Tlyc006tJkae7hF3ni1Ah4QwooHxU9PGSF4MAQcJOQ6l3Ab9QgCAjYhsMqzywCmgfigQFWUTIyfHaWKASJyjWShshiZ9axZhuEZhAoGBAPEOcaTvmuOgsle98NpLghKf9U/NyNVyKVvsgFI3o5YWfcWNBJTeddoCF5FGOODblBQVkv81gzIPzijfhnhlcIsOTKyUYGS9E8p016H0qXjzaOPYbCOa0X6ARbjlihFApID9cdBIHzWjlv5j8OOvczRpqq4sccc4PhpLiHuOkLSRAoGBAMrR/S1JGKrh6SaQAqXMaJ8UwqtSaJTvTb5wF7TJQp7jgIVGcrJZVSk7KFPbEmuZgwXBQ2VqCd/uNbOJbIGLKoHyEVVsJwdTh+ERYAd74Xgi63927SCTAwjzmjSF4/rUf5et+wnXGOgaLKbko0OgAAgPv3GhQi/DzbmMufRksKDfAoGAUnfjj07dyaRPf08fGD+e52pMDZpUbBmbZrR5jic00wMs0ioVeCzHWuc+UtgYW20jst9So/cVxEm9+SPHqGbj4t7ogl5cv/ojzC6/GsxOMm/r4Y9IJ8Iui72snL4CxzY7UVnj1yLlcn9eB/f/EJYUrsi7uMzBvCtUq8BNmB/FB9ECgYBmh2T/QZZwvCdK0T4lZoH0+V+3j79Cmv6oKK4zPBKrk8JinUxaEILWhwtJ6NMVBdOQUzSozcQUKa2IBw6NJjbk3eQZVMUeFQH5qvjqj5tZSf3Wv937u5WrLspjhtPK9yVRHihrvwreOm0cKTTHeNjLOnrDDIwS7V7QT+2pOEV57QKBgQCu6xfb2RKgsEmxXo3nciF0AKY4vxhhpfizo9Wn8lkJUSovyOMg38zKlL6sz20G6ehJYo0FT83oxvpqNrrxRx/UPuxXT/ym5hsnfxS288xvv0lqolF41UYUOCzM+d7mnGGsFNSDIqEhIGpar+1agzcCowl3Q6oOY1HNOyBSCIh2iA==".Replace("\n", "");

        //openssl rsa -pubout -in rsa_1024_priv.pem -out rsa_1024_pub.pem
        private static readonly string _publicKey = @"MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAvvsg3NdmKIXUmCiyZPO46vVMmPW07TQZ2Acs8Eadbw1IM+en5Igb1X8BNVO/9ISFQGsX8NTVWsI9sSbFv/4ipNe6CT/z/bwEVptM42uL1KFF7RvaXLkZQACxoSCiikqU/hRgkD6B7ERS3ITXzzUh5dvPNi/K6jAgb6wSxdTxr13/c91jMLIbmaS6cpX7s2Aa+1cWWBA3xL6fs/hxfkGj3MX4wlJkDx8GnVRi85xcjHDRcx0BMi/u9jiV0L0J6TeMxU5CWWfsu/8XJ9AwgiFF0AfwsskmweYdbc0nRuLkAcv2BezGnWkODknSQIV/NUDHIM5xvQvqWE0ycIn0aJfqTwIDAQAB".Replace("\n", "");
        public static void Main(string[] args)
        {
            var plainText = "cnblogs.com";

            //Encrypt
            RSA rsa = CreateRsaFromPublicKey(_publicKey);
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            var cipherBytes = rsa.Encrypt(plainTextBytes, RSAEncryptionPadding.Pkcs1);
            var cipher = Convert.ToBase64String(cipherBytes);
            
           // Console.WriteLine($"{nameof(cipher)}:{cipher}");

            //Decrypt
            rsa = CreateRsaFromPrivateKey(_privateKey);
            cipherBytes = System.Convert.FromBase64String(cipher);
            plainTextBytes = rsa.Decrypt(cipherBytes, RSAEncryptionPadding.Pkcs1);
            plainText = Encoding.UTF8.GetString(plainTextBytes);
            //Console.WriteLine($"{nameof(plainText)}:{plainText}");
        }

        public static RSA CreateRsaFromPrivateKey(string privateKey)
        {
            var privateKeyBits = System.Convert.FromBase64String(privateKey);
            var rsa = RSA.Create();
            var RSAparams = new RSAParameters();

            using (var binr = new BinaryReader(new MemoryStream(privateKeyBits)))
            {
                byte bt = 0;
                ushort twobytes = 0;
                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130)
                    binr.ReadByte();
                else if (twobytes == 0x8230)
                    binr.ReadInt16();
                else
                    throw new Exception("Unexpected value read binr.ReadUInt16()");

                twobytes = binr.ReadUInt16();
                if (twobytes != 0x0102)
                    throw new Exception("Unexpected version");

                bt = binr.ReadByte();
                if (bt != 0x00)
                    throw new Exception("Unexpected value read binr.ReadByte()");

                RSAparams.Modulus = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.Exponent = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.D = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.P = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.Q = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.DP = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.DQ = binr.ReadBytes(GetIntegerSize(binr));
                RSAparams.InverseQ = binr.ReadBytes(GetIntegerSize(binr));
            }

            rsa.ImportParameters(RSAparams);
            return rsa;
        }

        private static int GetIntegerSize(BinaryReader binr)
        {
            byte bt = 0;
            byte lowbyte = 0x00;
            byte highbyte = 0x00;
            int count = 0;
            bt = binr.ReadByte();
            if (bt != 0x02)
                return 0;
            bt = binr.ReadByte();

            if (bt == 0x81)
                count = binr.ReadByte();
            else
                if (bt == 0x82)
            {
                highbyte = binr.ReadByte();
                lowbyte = binr.ReadByte();
                byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                count = BitConverter.ToInt32(modint, 0);
            }
            else
            {
                count = bt;
            }

            while (binr.ReadByte() == 0x00)
            {
                count -= 1;
            }
            binr.BaseStream.Seek(-1, SeekOrigin.Current);
            return count;
        }

        public static RSA CreateRsaFromPublicKey(string publicKeyString)
        {
            byte[] SeqOID = { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };
            byte[] x509key;
            byte[] seq = new byte[15];
            int x509size;

            x509key = Convert.FromBase64String(publicKeyString);
            x509size = x509key.Length;

            using (var mem = new MemoryStream(x509key))
            {
                using (var binr = new BinaryReader(mem))
                {
                    byte bt = 0;
                    ushort twobytes = 0;

                    twobytes = binr.ReadUInt16();
                    if (twobytes == 0x8130)
                        binr.ReadByte();
                    else if (twobytes == 0x8230)
                        binr.ReadInt16();
                    else
                        return null;

                    seq = binr.ReadBytes(15);
                    if (!CompareBytearrays(seq, SeqOID))
                        return null;

                    twobytes = binr.ReadUInt16();
                    if (twobytes == 0x8103)
                        binr.ReadByte();
                    else if (twobytes == 0x8203)
                        binr.ReadInt16();
                    else
                        return null;

                    bt = binr.ReadByte();
                    if (bt != 0x00)
                        return null;

                    twobytes = binr.ReadUInt16();
                    if (twobytes == 0x8130)
                        binr.ReadByte();
                    else if (twobytes == 0x8230)
                        binr.ReadInt16();
                    else
                        return null;

                    twobytes = binr.ReadUInt16();
                    byte lowbyte = 0x00;
                    byte highbyte = 0x00;

                    if (twobytes == 0x8102)
                        lowbyte = binr.ReadByte();
                    else if (twobytes == 0x8202)
                    {
                        highbyte = binr.ReadByte();
                        lowbyte = binr.ReadByte();
                    }
                    else
                        return null;
                    byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                    int modsize = BitConverter.ToInt32(modint, 0);

                    int firstbyte = binr.PeekChar();
                    if (firstbyte == 0x00)
                    {
                        binr.ReadByte();
                        modsize -= 1;
                    }

                    byte[] modulus = binr.ReadBytes(modsize);

                    if (binr.ReadByte() != 0x02)
                        return null;
                    int expbytes = (int)binr.ReadByte();
                    byte[] exponent = binr.ReadBytes(expbytes);

                    var rsa = RSA.Create();
                    var rsaKeyInfo = new RSAParameters
                    {
                        Modulus = modulus,
                        Exponent = exponent
                    };
                    rsa.ImportParameters(rsaKeyInfo);
                    return rsa;
                }

            }
        }

        private static bool CompareBytearrays(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
                return false;
            int i = 0;
            foreach (byte c in a)
            {
                if (c != b[i])
                    return false;
                i++;
            }
            return true;
        }

    }
}
