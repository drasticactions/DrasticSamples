using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedPlayground
{
    public static class StringExtensions
    {
        public static string ToHtmlParagraph(this string str)
            => $"<p>{str}</p>";

        public static string GenerateHtmlString(this Faker faker, int paragraphs = 1)
        {
            var str = new StringBuilder();
            paragraphs = paragraphs < 1 ? 1 : paragraphs;

            var p = faker.Make(paragraphs, () => { return faker.Lorem.Paragraph(); });

            var linkText = "<a href=\"" + faker.Internet.Url() + "\">" + faker.Lorem.Word() + "</a>";

            foreach (var item in p)
            {
                var link = item.Insert(faker.Random.Int(0, item.Split(' ').Count() - 1), linkText);
                str.Append(link.ToHtmlParagraph());
            }

            return str.ToString();
        }
    }
}
