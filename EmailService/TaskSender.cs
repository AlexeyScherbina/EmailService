using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using EmailService.Services;

namespace EmailService
{
    public partial class TaskSender : ServiceBase
    {
        public TaskSender()
        {
            InitializeComponent();

            eventLog1 = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists("MySource"))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    "MySource", "MyNewLog");
            }
            eventLog1.Source = "MySource";
            eventLog1.Log = "MyNewLog";
        }

        protected override async void OnStart(string[] args)
        {
            eventLog1.WriteEntry("In OnStart");
            TaskService ts = new TaskService();
            var tasks = await ts.GetAll();
            string s = "Tasks form database: \n";
            foreach (var task in tasks)
            {
                s += "{ TaskId: " + task.TaskId + ", Name: " + task.Name + ", Day: " + task.Day;
                if (task.User != null)
                {
                    s += ", User: " + task.User.FullName;
                }
                s += " }  " + Environment.NewLine;
            }

            #region Password

            string password = "123456aa!";

            #endregion

            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            mail.To.Add("scherbina.lyosha@gmail.com");
            mail.From = new MailAddress("email.sender.sas@gmail.com", "Tasks from database", System.Text.Encoding.UTF8);
            mail.Subject = "This mail is send from asp.net application";
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            mail.Body = s;
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.High;
            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential("email.sender.sas@gmail.com", password);
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;

            client.Send(mail);
            eventLog1.WriteEntry("Email Sent");
        }

        protected override void OnStop()
        {
            eventLog1.WriteEntry("In OnStop");
        }

        private void eventLog1_EntryWritten(object sender, EntryWrittenEventArgs e)
        {

        }
    }
}
