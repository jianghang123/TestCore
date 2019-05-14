using System;
using System.Collections.Generic;
using System.Text;

namespace TestCore.Common.Security
{
    public class RSATest
    {
        //openssl genrsa -out rsa_1024_priv.pem 1024
        private static readonly string _privateKey = @"MIIEpAIBAAKCAQEAvvsg3NdmKIXUmCiyZPO46vVMmPW07TQZ2Acs8Eadbw1IM+en5Igb1X8BNVO/9ISFQGsX8NTVWsI9sSbFv/4ipNe6CT/z/bwEVptM42uL1KFF7RvaXLkZQACxoSCiikqU/hRgkD6B7ERS3ITXzzUh5dvPNi/K6jAgb6wSxdTxr13/c91jMLIbmaS6cpX7s2Aa+1cWWBA3xL6fs/hxfkGj3MX4wlJkDx8GnVRi85xcjHDRcx0BMi/u9jiV0L0J6TeMxU5CWWfsu/8XJ9AwgiFF0AfwsskmweYdbc0nRuLkAcv2BezGnWkODknSQIV/NUDHIM5xvQvqWE0ycIn0aJfqTwIDAQABAoIBAQCd14Zon6nyMfHsNC6E2x5pKoW9Ic4AAgCeGAgfKe2yO+MB6yNK8Oc4Q7im72oF2IUdIaXuyKjxUvqT8Hyd2pedcCJ0xKOOgReA0OfySg+OlB/sAQovelgRGsG0bvmHnG6ZtBO/A7pzoGBvm2eV/M7gYBRZL5Tgixgz057MXNvHWFUNg+NS49KOSHapvJS2foqc8KEZQw6zUAg3mBtLMgdqbSgm8ndazDfh7+2xpofLFL547a84Tlyc006tJkae7hF3ni1Ah4QwooHxU9PGSF4MAQcJOQ6l3Ab9QgCAjYhsMqzywCmgfigQFWUTIyfHaWKASJyjWShshiZ9axZhuEZhAoGBAPEOcaTvmuOgsle98NpLghKf9U/NyNVyKVvsgFI3o5YWfcWNBJTeddoCF5FGOODblBQVkv81gzIPzijfhnhlcIsOTKyUYGS9E8p016H0qXjzaOPYbCOa0X6ARbjlihFApID9cdBIHzWjlv5j8OOvczRpqq4sccc4PhpLiHuOkLSRAoGBAMrR/S1JGKrh6SaQAqXMaJ8UwqtSaJTvTb5wF7TJQp7jgIVGcrJZVSk7KFPbEmuZgwXBQ2VqCd/uNbOJbIGLKoHyEVVsJwdTh+ERYAd74Xgi63927SCTAwjzmjSF4/rUf5et+wnXGOgaLKbko0OgAAgPv3GhQi/DzbmMufRksKDfAoGAUnfjj07dyaRPf08fGD+e52pMDZpUbBmbZrR5jic00wMs0ioVeCzHWuc+UtgYW20jst9So/cVxEm9+SPHqGbj4t7ogl5cv/ojzC6/GsxOMm/r4Y9IJ8Iui72snL4CxzY7UVnj1yLlcn9eB/f/EJYUrsi7uMzBvCtUq8BNmB/FB9ECgYBmh2T/QZZwvCdK0T4lZoH0+V+3j79Cmv6oKK4zPBKrk8JinUxaEILWhwtJ6NMVBdOQUzSozcQUKa2IBw6NJjbk3eQZVMUeFQH5qvjqj5tZSf3Wv937u5WrLspjhtPK9yVRHihrvwreOm0cKTTHeNjLOnrDDIwS7V7QT+2pOEV57QKBgQCu6xfb2RKgsEmxXo3nciF0AKY4vxhhpfizo9Wn8lkJUSovyOMg38zKlL6sz20G6ehJYo0FT83oxvpqNrrxRx/UPuxXT/ym5hsnfxS288xvv0lqolF41UYUOCzM+d7mnGGsFNSDIqEhIGpar+1agzcCowl3Q6oOY1HNOyBSCIh2iA==".Replace("\n", "");

        //openssl rsa -pubout -in rsa_1024_priv.pem -out rsa_1024_pub.pem
        private static readonly string _publicKey = @"MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAvvsg3NdmKIXUmCiyZPO46vVMmPW07TQZ2Acs8Eadbw1IM+en5Igb1X8BNVO/9ISFQGsX8NTVWsI9sSbFv/4ipNe6CT/z/bwEVptM42uL1KFF7RvaXLkZQACxoSCiikqU/hRgkD6B7ERS3ITXzzUh5dvPNi/K6jAgb6wSxdTxr13/c91jMLIbmaS6cpX7s2Aa+1cWWBA3xL6fs/hxfkGj3MX4wlJkDx8GnVRi85xcjHDRcx0BMi/u9jiV0L0J6TeMxU5CWWfsu/8XJ9AwgiFF0AfwsskmweYdbc0nRuLkAcv2BezGnWkODknSQIV/NUDHIM5xvQvqWE0ycIn0aJfqTwIDAQAB".Replace("\n", "");

        public static void Test()
        {
            IDictionary<string, string> paramsMap = new Dictionary<string, string>();
            paramsMap.Add("appId", "2013092500031084");
            var sign =  Signature.RSASign(paramsMap, _privateKey, "", "RSA2");
            paramsMap.Add("sign", sign);
           
            bool checkSign = Signature.RSACheckV2(paramsMap, _publicKey, null, "RSA2", false);

            //RSA rsa = CreateRsaFromPublicKey(_publicKey);
            //var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            //var cipherBytes = rsa.Encrypt(plainTextBytes, RSAEncryptionPadding.Pkcs1);
            //var cipher = Convert.ToBase64String(cipherBytes);
            //Console.WriteLine($"{nameof(cipher)}:{cipher}");

            //Decrypt
            //rsa = CreateRsaFromPrivateKey(_privateKey);
            //cipherBytes = System.Convert.FromBase64String(cipher);
            //plainTextBytes = rsa.Decrypt(cipherBytes, RSAEncryptionPadding.Pkcs1);
            //plainText = Encoding.UTF8.GetString(plainTextBytes);

        }
    }
}
