<<<<<<< HEAD
﻿using ProiectII.Interfaces;
=======
using ProiectII.Interfaces;
>>>>>>> origin/master
using Microsoft.Extensions.Logging;

namespace ProiectII.Services.UtilityServices
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;

        public EmailService(ILogger<EmailService> logger)
        {
            _logger = logger;
        }

        public Task<bool> SendEmailAsync(string toEmail, string subject, string body)
        {
            _logger.LogWarning("================================================================");
            _logger.LogWarning("OUTGOING EMAIL SYSTEM | Status: SIMULATED_SEND");
            _logger.LogWarning("To: {Email}", toEmail);
            _logger.LogWarning("Subject: {Subject}", subject);
            _logger.LogWarning("Content: {Body}", body);
            _logger.LogWarning("================================================================");

<<<<<<< HEAD

            return Task.FromResult(true);
        }
    }
}
=======
            return Task.FromResult(true);
        }
    }
}
>>>>>>> origin/master
