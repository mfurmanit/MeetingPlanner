using System;
using IdentityServer4.Extensions;
using MailKit.Net.Smtp;
using MeetingPlanner.Dto;
using MeetingPlanner.Models;
using MimeKit;

namespace MeetingPlanner.Services
{
    public class EmailService : IEmailService
    {
        private readonly ConnectionMetadata _connectionMetadata;

        public EmailService(ConnectionMetadata connectionMetadata)
        {
            _connectionMetadata = connectionMetadata;
        }

        private String ResolveNotificationContent(Event eventObject)
        {
            var content = " ";

            if (!eventObject.HourFrom.IsNullOrEmpty() && !eventObject.HourTo.IsNullOrEmpty())
            {
                content = $" (w godzinach {eventObject.HourFrom} - {eventObject.HourTo}) ";
            }
            else if (!eventObject.HourFrom.IsNullOrEmpty() && eventObject.HourTo.IsNullOrEmpty())
            {
                content = $" (o godzinie {eventObject.HourFrom}) ";
            }

            return $"W dniu {eventObject.Date.ToShortDateString()}{content}odbędzie się spotkanie zatytuowane '{eventObject.Title}'." +
                   $" Przejdź do kalendarza, aby wyświetlić szczegóły.";
        }

        private EmailMessage PrepareEmailMessage(Event eventObject)
        {
            return new EmailMessage()
            {
                Sender = new MailboxAddress("Meeting Planner", _connectionMetadata.Sender),
                Receiver = new MailboxAddress(eventObject.User.UserName, eventObject.User.Email),
                Subject = "Powiadomienie o nadchodzącym spotkaniu!",
                Content = ResolveNotificationContent(eventObject)
            };
        }

        private MimeMessage CreateMimeMessageFromEmailMessage(EmailMessage message)
        {
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(message.Sender);
            mimeMessage.To.Add(message.Receiver);
            mimeMessage.Subject = message.Subject;
            mimeMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text)
                { Text = message.Content };
            return mimeMessage;
        }

        public void SendNotification(Event eventObject)
        {
            var mimeMessage = CreateMimeMessageFromEmailMessage(PrepareEmailMessage(eventObject));
            using (SmtpClient smtpClient = new SmtpClient())
            {
                smtpClient.Connect(_connectionMetadata.SmtpServer,
                    _connectionMetadata.Port, true);
                smtpClient.Authenticate(_connectionMetadata.Username,
                    _connectionMetadata.Password);
                smtpClient.Send(mimeMessage);
                smtpClient.Disconnect(true);
            }
        }
    }
}
