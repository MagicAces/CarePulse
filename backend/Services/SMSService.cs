using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Interfaces;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
// using Vonage.Messaging;
// using Vonage.Request;
// using Microsoft.Extensions.Configuration;
// using Vonage;
namespace backend.Services
{
    public class SMSService : ISMSService
    {
        private readonly IConfiguration _config;
        private readonly string AccountSID;
        private readonly string AuthToken;
        private readonly string FromNumber;

        public SMSService(IConfiguration config)
        {
            _config = config;

            AccountSID = _config["SMSSettings:Twilio_AccountSID"];
            AuthToken = _config["SMSSettings:Twilio_AuthToken"];
            FromNumber = _config["SMSSettings:Twilio_FromNumber"];
        }

        public Task<bool> SendSmsAsync(string to, string message)
        {
            try
            {
                TwilioClient.Init(AccountSID, AuthToken);

                var messageOptions = new CreateMessageOptions(new PhoneNumber(to))
                {
                    From = new PhoneNumber(FromNumber),
                    Body = message
                };

                var msg = MessageResource.Create(messageOptions);
                return Task.FromResult(true);
                // var credentials = Credentials.FromApiKeyAndSecret(ApiKey, ApiSecret);
                // var client = new VonageClient(credentials);

                // var response = await client.SmsClient.SendAnSmsAsync(new SendSmsRequest
                // {
                //     To = to,
                //     From = From,
                //     Text = message
                // });

                // return response.Messages[0].Status == "0";
            }
            catch (Exception e)
            {
                Console.WriteLine($"SMS sending failed: {e.Message}");
                return Task.FromResult(false);
            }
        }
    }
}