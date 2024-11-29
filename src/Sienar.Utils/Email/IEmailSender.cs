using System.Net.Mail;
using System.Threading.Tasks;

namespace Sienar.Email;

/// <summary>
/// Responsible for sending email messages
/// </summary>
public interface IEmailSender
{
	/// <summary>
	/// Sends an email as defined by the provided <see cref="MailMessage"/>
	/// </summary>
	/// <param name="message">The message to send</param>
	/// <returns>whether the message sent successfully or not</returns>
	Task<bool> Send(MailMessage message);
}