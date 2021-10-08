using System.Net.Mail;
using System.Net.Mime;

public class TemplatedAlternateViewBuider
{
	private readonly string templatePath;
	private readonly string templateName;
	private string content = String.Empty;

	public TemplatedAlternateViewBuider(string templatePath, string templateName = "template.html")
	{
		this.templatePath = templatePath;
		this.templateName = templateName;
	}

	public TemplatedAlternateViewBuider SetContent(string content)
	{
		this.content = content;
		return this;
	}

	public AlternateView Build()
	{
		var template = File.ReadAllText(Path.Combine(templatePath, templateName));
		var templateParser = new EMailTemplateParser(template);

		var renderedContent = templateParser.Render(content);

		var alternateView = AlternateView.CreateAlternateViewFromString(renderedContent, null, MediaTypeNames.Text.Html);

		foreach (var img in templateParser.Images)
		{
			var imgPath = Path.Combine(templatePath, img);
			var imgLinkedResource = new LinkedResource(imgPath, MediaTypeNames.Image.Jpeg) { ContentId = img };
			alternateView.LinkedResources.Add(imgLinkedResource);
		}

		return alternateView;
	}
}