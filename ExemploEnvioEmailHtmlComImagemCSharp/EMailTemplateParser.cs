using System.Text.RegularExpressions;

public class EMailTemplateParser
{
	private readonly string template;
	private readonly string[] images;

	public EMailTemplateParser(string templateHtml)
	{
		var imgPattern = @"(?<prefix><img.*src=)(?<delimit>[""\'])(?<src>.*)\k<delimit>";
		var regexImages = new Regex(imgPattern, RegexOptions.Multiline);

		var linkedImages = new HashSet<string>();
		template = regexImages.Replace(templateHtml, match =>
		{
			var prefix = match.Groups["prefix"].Value;
			var delimit = match.Groups["delimit"].Value;
			var src = match.Groups["src"].Value;

			if (!linkedImages.Contains(src))
				linkedImages.Add(src);

			src = $"cid:{src}";

			return $"{prefix}{delimit}{src}{delimit}";
		});

		this.images = linkedImages.ToArray();
	}

	public string Template => this.template;
	public string[] Images => this.images;

	public string Render(string content)
	{
		return template.Replace("{{content}}", content);
	}

}