﻿namespace MeetingPlanner.Dto
{
    public class ConnectionMetadata
    {
        public string Sender { get; set; }
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
