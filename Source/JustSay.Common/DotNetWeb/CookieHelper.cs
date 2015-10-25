using System;
using System.Web;
using System.Diagnostics;

using JustSay.Common.DotNetEncrypt;
using JustSay.Common.DotNetCode;
using JustSay.Common.Extension;

namespace JustSay.Common.DotNetWeb
{
    public class CookieHelper
    {

#warning  �˴���������
        public  static string mainKey = "JustSayInfo";
        static string adminkey = "Duan@JustSay";
        static string subKey = "UserInfo";
        static string uidKey = "Uid";
        static  string UserEncryptKey = HttpContext.Current.Request.UserAgent.Substring(HttpContext.Current.Request.UserAgent.Length - 3);

        /// <summary>
        /// ���Cookie ���ǺϷ��û�
        /// �����㷵��ID
        /// ����-1��Cookie��
        /// ����-2 ����������
        /// ����-3 cookie������
        /// </summary>
        /// <returns>����ID</returns>    
        public static int VerifyUser(out string username,out string email)
        {
            username = null;
            email=null;
            if (IsExistCookie(mainKey))
            {

                string adminEncrypt = GetValue(mainKey,mainKey);

                email = DESEncrypt.Decrypt(adminEncrypt, adminkey);
                if (IsExistCookie(mainKey, subKey))
                {
                    HttpCookie nCookie = HttpContext.Current.Request.Cookies[mainKey];
                    string EncryptData = nCookie.Values[subKey];
                    string uidData=nCookie.Values[uidKey];
                    if (email == DESEncrypt.Decrypt(EncryptData, UserEncryptKey))
                    {
                        username =HttpContext.Current.Server.UrlDecode(GetValue(mainKey, "username"));
                        return DESEncrypt.Decrypt(uidData, email).ToInt();

                    }
                    else
                        return - 1;
                }
                else
                {
                    return -2;
                }
            }
            else
            {
                return  -3;
            }
        }


        public static string GetUserName()
        {
            return HttpContext.Current.Server.UrlDecode(GetValue(mainKey, "username"));
        }

        /// <summary>
        /// ���Ӽ���Cookie��Ϣ
        /// </summary>
        /// <param name="email">��¼����</param>
        /// <param name="days">cookie����Чʱ��</param>
        /// <returns></returns>
        public static bool AddUserInfo(string email,int days,int uid,string username)
        {
            try
            {
                username = HttpContext.Current.Server.UrlEncode(username);
                DateTime cookieExpires = DateTime.Now.AddDays(days);
                string adminEncryptData = DESEncrypt.Encrypt(email, adminkey);
                string expiresEncryptData = DESEncrypt.Encrypt(email, UserEncryptKey);
                string uidData = DESEncrypt.Encrypt(uid.ToString(), email);
                #region дCookie
                HttpCookie nCookie = new HttpCookie(mainKey);
  
                nCookie.Expires = cookieExpires;
                nCookie.Values.Add(mainKey, adminEncryptData);
                nCookie.Values.Add(subKey,expiresEncryptData);
                nCookie.Values.Add(uidKey, uidData);
                nCookie.Values.Add("username", username);
                nCookie.HttpOnly = true;
                HttpContext.Current.Response.AppendCookie(nCookie);
                return true;
            #endregion


                //Add(mainKey, adminEncryptData, cookieExpires);
                //Add(mainKey, subKey, expiresEncryptData);
                //Add(mainKey, uidKey, uidData);
               // return true;
            }
            catch(Exception ex)
            {

                LogHelper.WriteLog(ex, "ǰ���쳣", "CookieHelper");
                return false;

            }

        }
        /// <summary>
        /// �Ƿ���ڴ˼�
        /// </summary>
        /// <param name="key"></param>
        /// <param name="subkey"></param>
        /// <returns></returns>
        public static bool IsExistCookie(string key, string subkey)
        {
            HttpCookie nCookie = HttpContext.Current.Request.Cookies[key];
            if (nCookie == null)
                return false;
            else if (nCookie.Values[subkey] == null)
                return false;
            return true;
        }
        /// <summary>
        ///�Ƿ���ڴ˼�
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool IsExistCookie(string key)
        {
            HttpCookie nCookie = HttpContext.Current.Request.Cookies[key];
            if (nCookie == null)
                return false;
            return true;
        }
        /// <summary>
        /// ���Ӽ�,ָ����һ��Ҫ����
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static bool Add(string key, string value, DateTime time)
        {
            HttpCookie nCookie = new HttpCookie(key);
            nCookie.Value = value;
            nCookie.Expires = time;
            HttpContext.Current.Response.AppendCookie(nCookie);
            return true;
        }
        /// <summary>
        /// ָ����һ��Ҫ���ڣ��������򷵻�false
        /// </summary>
        /// <param name="key"></param>
        /// <param name="subkey"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool Add(string key, string subkey, string value)
        {
            HttpCookie nCookie = HttpContext.Current.Request.Cookies[key];
            if (null == nCookie)
            {
                return false;
            }
            try
            {
                nCookie.Values.Add(subkey, value);
                HttpContext.Current.Response.AppendCookie(nCookie);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// ָ�������ܴ��ڣ������ڴ���
        /// </summary>
        /// <param name="key"></param>
        /// <param name="subkey"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool Add(string key, string subkey, string value, DateTime time)
        {
            HttpCookie nCookie = HttpContext.Current.Request.Cookies[key];
            if (null == nCookie)
            {
                nCookie = new HttpCookie(key);
                nCookie.Expires = time;
            }
            try
            {
                nCookie.Values.Add(subkey, value);
                HttpContext.Current.Response.AppendCookie(nCookie);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// �õ�ֵ
        /// </summary>
        /// <param name="key"></param>
        /// <param name="subkey">�ɲ�д������cookie����ֵ</param>
        /// <returns></returns>
        public static string GetValue(string key, string subkey)
        {
            HttpCookie nCookie = HttpContext.Current.Request.Cookies[key];
            try
            {
                if (string.IsNullOrEmpty(subkey))
                    return nCookie.Value;
                else
                    return nCookie[subkey];
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// ɾ��Cookie
        /// </summary>
        /// <param name="key"></param>
        public static bool Delete(string key)
        {
            HttpCookie nCookie = HttpContext.Current.Request.Cookies[key];
            try
            {
                nCookie.Expires = new DateTime(2000, 1, 1);
                HttpContext.Current.Response.AppendCookie(nCookie);
            }
            catch
            {
                return false;
            }

            return true;
        }


    }
}