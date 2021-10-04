using System.Net;
using System.Net.Mail;
using System.Net.Mime;

Console.WriteLine("Exemplo de envio de e-mail com conteúdo html formatado e imagens!");


var from = "<remetente>@outlook.com";
var to = "<destinatario>@gmail.com";

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

var mail = new MailMessage(from, to);
mail.Subject = "Teste";
mail.IsBodyHtml = true;
mail.AlternateViews.Add(alternateView);


using var smtpClient = new SmtpClient();
smtpClient.Host = "smtp-mail.outlook.com";
smtpClient.Port = 587;
smtpClient.EnableSsl = true;
smtpClient.UseDefaultCredentials = false;
smtpClient.Credentials = new NetworkCredential("<usuario>", "<senha>");

try
{
	smtpClient.Send(mail);
}
catch (Exception ex)
{
	Console.WriteLine($"Erro: {ex.Message} {ex.InnerException?.Message}");
}
