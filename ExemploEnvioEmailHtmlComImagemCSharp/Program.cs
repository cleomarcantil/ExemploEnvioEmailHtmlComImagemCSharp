using System.Net;
using System.Net.Mail;
using System.Net.Mime;

Console.WriteLine("Exemplo de envio de e-mail com conteúdo html formatado e imagens!");

try
{
	var emailOrigem = InputHelper.GetValue("E-Mail origem: ")
		?? throw new Exception("E-Mail de origem requerido!");

	var password = InputHelper.GetPassword("Senha do servidor smpt: ");

	var emailDestino = InputHelper.GetValue("E-Mail destino: ")
		?? throw new Exception("E-Mail de destino requerido!");


	var content = @"
	<div>
		<p>Mensagem 1</p>
		<p>bla bla bla</p>
	</div>
	";

	var templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EmailTemplate");
	var alternateView = new TemplatedAlternateViewBuider(templatePath)
		.SetContent(content)
		.Build();

	var mail = new MailMessage(emailOrigem, emailDestino);
	mail.Subject = "Teste";
	mail.IsBodyHtml = true;
	mail.AlternateViews.Add(alternateView);


	using var smtpClient = new SmtpClient();
	smtpClient.Host = "smtp-mail.outlook.com";
	smtpClient.Port = 587;
	smtpClient.EnableSsl = true;
	smtpClient.UseDefaultCredentials = false;
	smtpClient.Credentials = new NetworkCredential(emailOrigem, password);

	smtpClient.Send(mail);
}
catch (Exception ex)
{
	Console.WriteLine($"Erro: {ex.Message} {ex.InnerException?.Message}");
}
