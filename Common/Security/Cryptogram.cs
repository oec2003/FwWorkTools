
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Web;
using System.Collections;

namespace FW.CommonFunction
{
    /// <summary>
    /// 对字符串进行加密和解密
    /// </summary>
	public class Cryptogram
	{
		private static readonly byte[] pKEY={ 218,239,227,22,31,53,120,224,223,223,171,210,140,158,47,86,122,39,238,95,47,138,44,155};
		private static readonly byte[] pIV ={1,2,3,4,5,6,7,8};
		
		private static TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();

		
		static Cryptogram()
		{
			//				des.Mode= System.Security.Cryptography.CipherMode.ECB ;
			//				des.Padding = System.Security.Cryptography.PaddingMode.PKCS7 ;
			des.Mode= System.Security.Cryptography.CipherMode.CBC ;
			des.Padding = System.Security.Cryptography.PaddingMode.PKCS7 ;			
		}

		/// <summary>
		/// 加密方法
		/// </summary>
		/// <param name="toEncryptStr"></param>
		/// <returns></returns>
		public static string CommonEncrypt(string toEncryptStr)
		{
            //int ErrorNum=-1;
            //string ErrorDesc="";

			if( toEncryptStr=="" ) return "";
			try
			{
				byte[] Encrypted;
			
				if( Encrypt(pKEY,pIV,ConvertStringToByteArray( toEncryptStr ),out Encrypted ) )
				{
					return ToBase64String( Encrypted );
				}
				else
					return "";
			}
			catch
			{
                //ErrorNum=SSOErrorDefinition.E_SSO_Cryptogram_OTHER_ERROR_ID;
                //ErrorDesc= e.Message ;	
                //throw new AAChinaException(ErrorNum, "CommonEncrypt（使用固定Key对字符串进行加密）-》"+ ErrorDesc, false );
                return "";
			}
			
		}


		/// <summary>
		/// 解密方法
		/// </summary>
		/// <param name="toDecryptStr"></param>
		/// <returns></returns>
		public static string CommonDecrypt(string toDecryptStr)
		{
            //int ErrorNum=-1;
            //string ErrorDesc="";
			
			if( toDecryptStr=="" ) return "";
			try
			{
				byte[] Decrypted;
			
				if( Decrypt(pKEY,pIV,FromBase64String( toDecryptStr ),out Decrypted ) )
				{
					return ConvertByteArrayToString( Decrypted );
				}
				else
					return "";
			}
			catch(Exception e)
			{
                throw e;
			}
			
		}
		/// <summary>
		/// 使用utf-8编码进行Encode解码
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string UrlEncode( string str)
		{
			return HttpUtility.UrlEncode(str, System.Text.Encoding.GetEncoding("utf-8") );
		}
		/// <summary>
		/// 使用utf-8编码进行Decode解码
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static string UrlDecode( string str)
		{
			return HttpUtility.UrlDecode(str, System.Text.Encoding.GetEncoding("utf-8") );
		}

		public static byte[] GenerateKey()
		{
            //int ErrorNum=-1;
            //string ErrorDesc="";
			
			byte[] buf=null;
			try
			{
				TripleDESCryptoServiceProvider tdes=new TripleDESCryptoServiceProvider();
				tdes.GenerateKey();
				buf = tdes.Key;
			}
			catch(Exception e)
			{
                throw e;
			}
			
			return buf;
		}
		public static byte[] GenerateIV()
		{
            //int ErrorNum=-1;
            //string ErrorDesc="";
			byte[] buf=null;
			try
			{
				TripleDESCryptoServiceProvider tdes=new TripleDESCryptoServiceProvider();
				tdes.GenerateIV();
				buf = tdes.IV;
			}
			catch(Exception e)
			{
                throw e;
			}
			
			return buf;
		}
		public static byte[] GenerateKey(string KEY)
		{
            //int ErrorNum=-1;
            //string ErrorDesc="";

			byte[] buf=null;
			try
			{
				buf = new MD5CryptoServiceProvider().ComputeHash((new UTF8Encoding()).GetBytes(KEY));
			}
			catch(Exception e)
			{
                throw e;
			}
			
			return buf;
		}
        /// <summary>
        /// 密码 加密
        /// </summary>
        /// <param name="Password"></param>
        /// <returns></returns>
		public static string EncryptPassword(string Password)
		{
			return CommonEncrypt(Password);
		}
        /// <summary>
        /// 密码 解密
        /// </summary>
        /// <param name="Password"></param>
        /// <returns></returns>
		public static string DecryptPassword(string Password)
		{
			return CommonDecrypt(Password);
		}

		public static string HashPassword(string Password)
		{
			if(Password==null || Password=="")
				return "";
			return Cryptogram.ToBase64String(GenerateKey(Password));
		}

		public static bool Encrypt(byte[] KEY ,byte[] IV ,byte[] TobeEncrypted, out  byte[] Encrypted )
		{
			//int ErrorNum=-1;
			string ErrorDesc="";
			if(KEY==null || IV==null)
					throw new Exception("Encrypt（加密）-》"+ ErrorDesc);
			Encrypted=null;
			try
			{
				byte[] tmpiv={0,1,2,3,4,5,6,7};
				for(int ii=0;ii<8;ii++)
				{
					tmpiv[ii]=IV[ii];
				}
				byte[] tmpkey={0,1,2,3,4,5,6,7,0,1,2,3,4,5,6,7,0,1,2,3,4,5,6,7};
				for(int ii=0;ii<24;ii++)
				{
					tmpkey[ii]=KEY[ii];
				}
				//tridesencrypt.Dispose();
				ICryptoTransform tridesencrypt = des.CreateEncryptor(tmpkey,tmpiv);
				//tridesencrypt = des.CreateEncryptor(KEY,tmpiv);
				Encrypted = tridesencrypt.TransformFinalBlock( TobeEncrypted,0,TobeEncrypted.Length);
				//tridesencrypt.Dispose();
				des.Clear();
				return true;
			}
			catch(Exception e)
			{
                throw e;
			}
		}
		public static bool Decrypt(byte[] KEY ,byte[] IV,byte[] TobeDecrypted,out  byte[] Decrypted )
		{
            //int ErrorNum=-1;
            //string ErrorDesc="";

			Decrypted=null;
			try
			{
				byte[] tmpiv={0,1,2,3,4,5,6,7};
				for(int ii=0;ii<8;ii++)
				{
					tmpiv[ii]=IV[ii];
				}
				byte[] tmpkey={0,1,2,3,4,5,6,7,0,1,2,3,4,5,6,7,0,1,2,3,4,5,6,7};
				for(int ii=0;ii<24;ii++)
				{
					tmpkey[ii]=KEY[ii];
				}
				ICryptoTransform tridesdecrypt = des.CreateDecryptor(tmpkey,tmpiv);
				Decrypted = tridesdecrypt.TransformFinalBlock(TobeDecrypted,0,TobeDecrypted.Length );
				des.Clear();
			}
			catch(Exception e)
			{
                throw e;
			}
			
			return true;
		}
		public static string ComputeHashString(string s) 
		{
			return ToBase64String(ComputeHash(ConvertStringToByteArray(s)));
		}
		public static byte[] ComputeHash(byte[] buf)
		{
			return ((HashAlgorithm)CryptoConfig.CreateFromName("SHA1")).ComputeHash(buf);
		}
		public static string ToBase64String(byte[] buf)
		{
			return System.Convert.ToBase64String(buf);
		}
		public static byte[] FromBase64String(string s)
		{
			return System.Convert.FromBase64String(s);
		}
		public static byte[] ConvertStringToByteArray(String s)
		{
			return System.Text.Encoding.GetEncoding("utf-8").GetBytes(s);//gb2312
		}
		public static string ConvertByteArrayToString(byte[] buf)
		{
			return System.Text.Encoding.GetEncoding("utf-8").GetString(buf);
		}
		public static string ByteArrayToHexString(byte[] buf)
		{
			StringBuilder sb=new StringBuilder();
			for(int i=0;i<buf.Length;i++)
			{
				sb.Append(buf[i].ToString("X").Length==2 ? buf[i].ToString("X"):"0"+buf[i].ToString("X"));
			}
			return sb.ToString();
		}
		public static byte[] HexStringToByteArray(string s)
		{
			Byte[] buf=new byte[s.Length/2];
			for(int i=0;i<buf.Length;i++)
			{
				buf[i]=(byte)(chr2hex(s.Substring(i*2,1))*0x10+chr2hex(s.Substring(i*2+1,1)));
			}
			return buf;
		}
		private static byte chr2hex(string chr)
		{
			switch(chr)
			{
				case "0":
					return 0x00;
				case "1":
					return 0x01;
				case "2":
					return 0x02;
				case "3":
					return 0x03;
				case "4":
					return 0x04;
				case "5":
					return 0x05;
				case "6":
					return 0x06;
				case "7":
					return 0x07;
				case "8":
					return 0x08;
				case "9":
					return 0x09;
				case "A":
					return 0x0a;
				case "B":
					return 0x0b;
				case "C":
					return 0x0c;
				case "D":
					return 0x0d;
				case "E":
					return 0x0e;
				case "F":
					return 0x0f;
			}
			return 0x00;
		}

        //功能：字符串加密
        //参数：待加密串
        //返回：加密变换后的结果
        public static string EnCode(string inStr)
        {
            string StrBuff = null;
            int IntLen = 0;
            int IntCode = 0;
            int IntCode1 = 0;
            int IntCode2 = 0;
            int IntCode3 = 0;
            int i = 0;

            IntLen = inStr.Trim().Length;

            IntCode1 = IntLen % 3;
            IntCode2 = IntLen % 9;
            IntCode3 = IntLen % 5;
            IntCode = IntCode1 + IntCode3;

            for (i = 1; i <= IntLen; i++)
            {
                StrBuff = StrBuff + Convert.ToChar(Convert.ToInt16(inStr.Substring(IntLen - i, 1).ToCharArray()[0]) - IntCode);
                if (IntCode == IntCode1 + IntCode3)
                {
                    IntCode = IntCode2 + IntCode3;
                }
                else
                {
                    IntCode = IntCode1 + IntCode3;
                }
            }

            return StrBuff + new string(' ', inStr.Length - IntLen);

        }

        //功能：字符串解密
        //参数：待反加密串
        //返回：反加密变换后的结果
        public static string DeCode(string inStr)
        {
            string StrBuff = null;
            int IntLen = 0;
            int IntCode = 0;
            int IntCode1 = 0;
            int IntCode2 = 0;
            int IntCode3 = 0;
            int i = 0;

            StrBuff = "";

            IntLen = inStr.Trim().Length;

            IntCode1 = IntLen % 3;
            IntCode2 = IntLen % 9;
            IntCode3 = IntLen % 5;

            if (IntLen % 2 == 0)
            {
                IntCode = IntCode2 + IntCode3;
            }
            else
            {
                IntCode = IntCode1 + IntCode3;
            }


            for (i = 1; i <= IntLen; i++)
            {
                StrBuff = StrBuff + Convert.ToChar(Convert.ToInt16(inStr.Substring(IntLen - i, 1).ToCharArray()[0]) + IntCode);

                if (IntCode == IntCode1 + IntCode3)
                {
                    IntCode = IntCode2 + IntCode3;
                }
                else
                {
                    IntCode = IntCode1 + IntCode3;
                }
            }

            return StrBuff + new string(' ', inStr.Length - IntLen);
        }

	}

}