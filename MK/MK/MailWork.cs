using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static MK.DataClass;

namespace MK
{
    class MailWork
    {

        /// 生成邮件消息体字符串
        /// </summary>
        /// <param name="mail"></param>
        /// <returns></returns>
        public static string GenerateMailMsg(MailBody mail)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("<p><span>Project Name:</span>&nbsp;{0}<p>", string.IsNullOrEmpty(mail.ProjectName) ? "" : mail.ProjectName));
            sb.Append(string.Format("<p><span>User Name:</span>&nbsp;{0}<p>", string.IsNullOrEmpty(mail.UserName) ? "" : mail.UserName));
            sb.Append(string.Format("<p><span>Status:</span>&nbsp;{0}<p>", string.IsNullOrEmpty(mail.Status) ? "" : mail.Status));
            if (!string.IsNullOrEmpty(mail.Reason))
            {
                sb.Append(string.Format("<p>{0}<p>", mail.Reason));
            }
            return sb.ToString();
        }
        public static void SendMail(string[] list, string text, string action = "")
        {
            string userEmailAddress = "Lcfc.mbl.system@lcfuturecenter.com";
            string password = "Qq66669999";
            string host = "webmail.lcfuturecenter.com";
            string userName = "lcfc.mbl.system";
            string subject = "MBL System Notification - " + action;
            MailWork.SendMail(userEmailAddress, userName, password, host, 25, list, null, subject, text,
                out string errorMessage);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                MessageBox.Show(errorMessage, "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                MessageBox.Show("通知邮件发送成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public static bool SendMessage(string userEmailAddress, string userName, string password, string host, int port,
            string[] sendToList, string[] sendCCList, string subject, string body, string[] attachmentsPath, out string errorMessage)
        {

            System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
            foreach (string send in sendToList)
            {
                msg.To.Add(send);
            }
            /*   
            * msg.To.Add("b@b.com");   
            * msg.To.Add("b@b.com");   
            * msg.To.Add("b@b.com");可以发送给多人   
            */
            // msg.CC.Add(c@c.com);
            /*   
            * msg.CC.Add("c@c.com");   
            * msg.CC.Add("c@c.com");可以抄送给多人   
            */
            msg.From = new MailAddress(userEmailAddress, userName, System.Text.Encoding.UTF8);
            /* 上面3个参数分别是发件人地址（可以随便写），发件人姓名，编码*/
            msg.Subject = "MBL通知邮件";//邮件标题    
            msg.SubjectEncoding = System.Text.Encoding.UTF8;//邮件标题编码    
            msg.Body = "邮件内容" + body;//邮件内容    
            msg.BodyEncoding = System.Text.Encoding.UTF8;//邮件内容编码    
            msg.IsBodyHtml = false;//是否是HTML邮件    
            msg.Priority = MailPriority.High;//邮件优先级    
            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential(userEmailAddress, password);
            //上述写你的GMail邮箱和密码    
            client.Port = 587;//Gmail使用的端口    
            client.Host = "smtp.qq.com";
            client.EnableSsl = true;//经过ssl加密    
            object userState = msg;
            try
            {
                client.SendAsync(msg, userState);
                //简单一点儿可以client.Send(msg);  
                errorMessage = "发送成功";
                MessageBox.Show("发送成功");
            }
            catch (System.Net.Mail.SmtpException ex)
            {
                errorMessage = "发送邮件出错";
                MessageBox.Show(ex.Message, "发送邮件出错");
            }
            return true;

        }

        internal static void SendRegisterMail(string[] a, object p)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="userEmailAddress">发件人地址</param>
        /// <param name="userName">发件人姓名(可为空)</param>
        /// <param name="password">密码</param>
        /// <param name="host">邮件服务器地址</param>
        /// <param name="port"></param>
        /// <param name="sendToList">收件人(多个电子邮件地址之间必须用逗号字符（“,”）分隔)</param>
        /// <param name="sendCCList">抄送人(多个电子邮件地址之间必须用逗号字符（“,”）分隔)</param>
        /// <param name="subject">主题</param>
        /// <param name="body">内容</param>
        /// <param name="attachmentsPath">附件路径</param>
        /// <param name="errorMessage">错误信息</param>
        public static bool SendMail(string userEmailAddress, string userName, string password, string host, int port,
            string[] sendToList, string[] sendCCList, string subject, string body, out string errorMessage)
        {
            errorMessage = string.Empty;
            SmtpClient client = new SmtpClient(host, port)
            {
                Credentials = new System.Net.NetworkCredential(userEmailAddress, password),//用户名、密码
                DeliveryMethod = SmtpDeliveryMethod.Network,//指定电子邮件发送方式    
                Host = host,//邮件服务器
                Port = port,
                UseDefaultCredentials = false,
                EnableSsl = true//经过ssl加密 
            };

            MailMessage msg = new MailMessage();
            //加发件人
            foreach (string send in sendToList)
            {
                msg.To.Add(send);
            }

            if (sendCCList != null)
            {
                //加抄送
                foreach (string cc in sendCCList)
                {
                    msg.To.Add(cc);
                }
            }

            msg.From = new MailAddress(userEmailAddress, userName);//发件人地址
            msg.Subject = subject;//邮件标题   
            msg.Body = x1 + x2 + body + x3;//邮件内容   
            msg.BodyEncoding = System.Text.Encoding.UTF8;//邮件内容编码   
            msg.IsBodyHtml = true;//是否是HTML邮件   
            msg.Priority = MailPriority.High;//邮件优先级   
            try
            {
                client.Send(msg);
                return true;
            }
            catch (System.Net.Mail.SmtpException ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }
        static string x1 = @"Hi ";
        static string x2 = @" ,
 
            This mail is sent by MBL Management System, please do not reply directly to it.";



        static string x3 = @" 
            Thanks and Regards,
            System Admin
            LCFC Notebook Research & Development Department
            ";

    }
}
