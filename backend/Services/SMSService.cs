using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Interfaces;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

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

            AccountSID = _config["SMSSettings:AccountSID"];
            AuthToken = _config["SMSSettings:AuthToken"];
            FromNumber = _config["SMSSettings:FromNumber"];
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
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Task.FromResult(false);
            }
        }
    }
}