using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace JustSay.Common.DotNetEmail
{
    public class SMTPManager
    {
        /// <summary>
        /// ���ʼ�
        /// </summary>
        /// <param name="mailAddress">�ʼ���ַ������á�;������</param>
        /// <param name="mailsubject">�ʼ�����</param>
        /// <param name="mailContent">�ʼ�����</param>
        /// <param name="filePath">������ַ  һ��Ϊ����</param>
        /// <param name="mailNO">��config�ļ������õ��ʼ���</param>
        /// <returns></returns>
        public static string MailSending(string mailAddress, string mailsubject, string mailContent, string filePath,int mailNO)
        {
            string MailUser, MailName, MailHost, MailPwd;
            if (mailNO == 0)
            {
                 MailUser = ConfigurationManager.AppSettings["MailUser"].ToString();
                 MailName = ConfigurationManager.AppSettings["MailName"].ToString();
                 MailHost = ConfigurationManager.AppSettings["MailHost"].ToString();
                 MailPwd = ConfigurationManager.AppSettings["MailPwd"].ToString();
            }
            else
            {
                 MailUser = ConfigurationManager.AppSettings["MailUser"+mailNO.ToString()].ToString();
                 MailName = ConfigurationManager.AppSettings["MailName"+mailNO.ToString()].ToString();
                 MailHost = ConfigurationManager.AppSettings["MailHost" + mailNO.ToString()].ToString();
                 MailPwd = ConfigurationManager.AppSettings["MailPwd"+mailNO.ToString()].ToString();
            }
            MailAddress from = new MailAddress(MailUser, MailName);
            MailMessage mail = new MailMessage();
            mail.Subject = mailsubject;
            mail.From = from;
            string[] mailNames = (mailAddress+ ";").Split(new char[]
			{
				';'
			});
            string[] array = mailNames;
            for (int i = 0; i < array.Length; i++)
            {
                string name = array[i];
                if (name != string.Empty)
                {
                    string displayName;
                    string address;
                    if (name.IndexOf('<') > 0)
                    {
                        displayName = name.Substring(0, name.IndexOf('<'));
                        address = name.Substring(name.IndexOf('<') + 1).Replace('>', ' ');
                    }
                    else
                    {
                        displayName = string.Empty;
                        address = name.Substring(name.IndexOf('<') + 1).Replace('>', ' ');
                    }
                    mail.To.Add(new MailAddress(address, displayName));
                }
            }
            mail.Body = mailContent;
            mail.BodyEncoding = Encoding.UTF8;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.Normal;
            
            if (filePath != "")
            {
                mail.Attachments.Add(new Attachment(filePath));
                mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;
            }

            SmtpClient client = new SmtpClient();
            client.Host = MailHost;
            client.Port = 25;
           // client.Port = 465;  //QQVIP
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(MailUser, MailPwd);
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            try
            {
                client.Send(mail);
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
            
            return "���ͳɹ�";
        }
    }
}