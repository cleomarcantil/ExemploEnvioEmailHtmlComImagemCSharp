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


	var imgPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EmailTemplate/mountain.jpg");
	var img1LinkedResource = new LinkedResource(imgPath, MediaTypeNames.Image.Jpeg) { ContentId = "img1" };

	var content = @"
		<div>
			<p>Teste de envio de e-mail com anexo</p>
			<img src='cid:img1' />
			<p>fim</p>
		</div>
		";

	var alternateView = AlternateView.CreateAlternateViewFromString(content, null, MediaTypeNames.Text.Html);
	alternateView.LinkedResources.Add(img1LinkedResource);

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
