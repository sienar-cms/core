using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Sienar.Email;

/// <summary>
/// A default email sender that doesn't actually send any email
/// </summary>
public class DefaultEmailSender : IEmailSender
{
	private readonly ILogger<DefaultEmailSender> _logger;

	/// <summary>
	/// Creates a new instance of <see cref="DefaultEmailSender"/>
	/// </summary>
	/// <param name="logger">a logger</param>
	public DefaultEmailSender(ILogger<DefaultEmailSender> logger)
	{
		_logger = logger;
	}

	/// <inheritdoc/>
	/// <remarks>
	/// This implementation simply returns <c>true</c>, indicating that an email was sent. However, no email will actually be sent by the default mail sender. This is to avoid requiring developers to adopt a particular mail strategy. Developers who want their app to send email should add their own implementation of <see cref="IEmailSender"/>, or else install one of the existing packages for sending email from Sienar
	/// </remarks>
	public Task<bool> Send(MailMessage message)
	{
		_logger.LogInformation(
			"You are currently using the default Sienar email sender. This email sender does not send any email because it has not been configured to do so. If you want to send email, you need to either a) add your own implementation of {emailSenderType}, or b) install an existing plugin that includes email functionality.",
			typeof(IEmailSender));

		_logger.LogInformation(
			"Sending simulated email. " +
			"Recipient: \"{recipient}\" " +
			"Subject: \"{subject}\" " +
			"Message content: \"{content}\"",
			message.To.First().Address,
			message.Subject,
			message.Body);

		return Task.FromResult(true);
	}
}