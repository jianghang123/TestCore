using MailKit.Net.Smtp;
using MimeKit;
using System;

namespace TestCore.Common.Helper
{
    public class EmailHelper
    {

//        public string content = @"guigudejub 您好,

//此邮件由系统根据您找回密码的申请自动发出，请勿回复。 

//以下为您的"临时密码"： 

//"6222c70a " 

//请您打开官网后或点击这里使用临时密码进行登录，成功登录后请按照提示更换您的新密码。 

//请注意： 新密码须由8-10个字母或数字组成(A-Z, a-z, 0-9)，密码区分字母大小写，不可包含空格。 

//如找回密码非您本人操作，或有其他任何问题，请立即通过以下方式联系我们。 

//全天候客服：在线客服
//微信客服：w88cs1
//客服热线：008602180245688
//电子邮箱：cncs @w88yd.com

//请收藏我们的备用域名w88.net & w88cc.cc，方便您随时访问娱乐。 

//此为隐私文件，受相关法律保护。如果您并非此邮件的预期接收者，请不要阅读、使用或者散播任何和此邮件相关的信息，并立即且彻底地删除此邮件（及任何相关的拷贝）。 
//我们建议您永久自己保存密码。请不要将您的个人密码告诉任何人，包括我们的员工。作为官方，我们也不会通过邮件来询问您的个人密码。 
//仅有在您本人登录我们的网站时才会使用到您的密码。"

        public static void SendMail(int Username,string EmailAddress,string TmpPwd,string smtpSrv,int smtpPort,string FromAddress,string FromPwd)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(FromAddress));
                message.To.Add(new MailboxAddress(EmailAddress));
                message.Subject = "[" + Username + "]-忘记密码";
                //var plain = new MimeKit.TextPart("plain")
                //{
                //    Text = @"不好意思，我在测试程序，Sorry！"
                //};
                string text = String.Format(@"{0} 您好,<br>此邮件由系统根据您找回密码的申请自动发出，请勿回复。<br> 以下为您的临时密码： {1}<br>请您打开官网后或点击这里使用临时密码进行登录，成功登录后请按照提示更换您的新密码。 ", Username, TmpPwd);
                var html = new TextPart("html")
                {
                    Text = text
                };
                // create an image attachment for the file located at path
                //var path = "D:\\雄安.jpg";
                //var fs = File.OpenRead(path);
                //var attachment = new MimeKit.MimePart("image", "jpeg")
                //{

                //    ContentObject = new MimeKit.ContentObject(fs, MimeKit.ContentEncoding.Default),
                //    ContentDisposition = new MimeKit.ContentDisposition(MimeKit.ContentDisposition.Attachment),
                //    ContentTransferEncoding = MimeKit.ContentEncoding.Base64,
                //    FileName = Path.GetFileName(path)
                //};
                var alternative = new Multipart("alternative")
                {
                    //alternative.Add(plain);
                    html
                };
                // now create the multipart/mixed container to hold the message text and the
                // image attachment
                var multipart = new Multipart("mixed")
                {
                    alternative
                };
                //multipart.Add(attachment);
                message.Body = multipart;
                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    // client.QueryCapabilitiesAfterAuthenticating = false;
                    client.Connect(smtpSrv, smtpPort, true);

                    // Note: since we don't have an OAuth2 token, disable
                    // the XOAUTH2 authentication mechanism.
                    client.AuthenticationMechanisms.Remove("XOAUTH2");

                    // Note: only needed if the SMTP server requires authentication
                    //var mailFromAccount = FromAddress;
                    //var mailPassword = FromPwd;
                    client.Authenticate(FromAddress, FromPwd);

                    client.Send(message);
                    client.Disconnect(true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //fs.Dispose();
        }
        public static void SendMailx(string Username, string EmailAddress, string TmpPwd, string smtpSrv, int smtpPort, string FromAddress, string FromPwd)
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress(FromAddress));
            message.To.Add(new MailboxAddress(EmailAddress));
            message.Subject = "[" + Username + "]-忘记密码";

            var builder = new BodyBuilder();
            var url = "http://localhost:5175/Sys/User/UpdatePwd?username=" + Username + "&r=" + DateTime.Now.Millisecond;
            builder.HtmlBody = String.Format(@"{0} 您好,<br>此邮件由系统根据您找回密码的申请自动发出，请勿回复。<br> 以下为您的临时密码： {1}<br>请您打开官网后或点击这里使用临时密码进行登录，成功登录后请按照提示更换您的新密码。 ", Username, TmpPwd);
         
            message.Body = builder.ToMessageBody();
            var client = new SmtpClient();

            client.Connect(smtpSrv, smtpPort, true);
            client.AuthenticationMechanisms.Remove("XOAUTH2");
            client.Authenticate(FromAddress, FromPwd); 
            client.Send(message);
            client.Disconnect(true);
        }
    }
}

