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
using EmailService.Models;
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

        public async void SendFive(int from,int count)
        {
            TaskService ts = new TaskService();
            string s = "Tasks form database: \n";
            var t = await ts.GetAll(from * 5, count);
            foreach (var task in t)
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

        protected override async void OnStart(string[] args)
        {
            System.Diagnostics.Debugger.Launch();

            eventLog1.WriteEntry("In OnStart");
            try
            {
                TaskService ts = new TaskService();

                int count = ts.GetCount();
                int operations = (int) (count / 5);
                int last = (int) (count % 5);

                for (int i = 0; i < operations; i++)
                {
                    SendFive(i,5);
                }

                if (last != 0)
                {
                    SendFive(operations,last);
                }
            }
            catch (Exception e)
            {
                eventLog1.WriteEntry(e.Message);
            }
            //var tasks = await ts.GetAll();
    
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
