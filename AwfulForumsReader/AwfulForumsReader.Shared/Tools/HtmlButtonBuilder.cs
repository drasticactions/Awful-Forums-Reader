using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace AwfulForumsReader.Tools
{
    public class HtmlButtonBuilder
    {
        /// <summary>
        /// Create HTML Submit button for Web Views.
        /// </summary>
        /// <param name="buttonName">Name of button.</param>
        /// <param name="buttonClick">Click handler to be applied to button.</param>
        /// <returns>HTML Submit Button String.</returns>
        public static string CreateSubmitButton(string buttonName, string buttonClick, string id)
        {
            return WebUtility.HtmlDecode(
                       string.Format(
                           "<li><button class=\"btn\" id=\"{2}\" type=\"submit\" onclick=\"{1}\";>{0}</button></li>",
                           buttonName, buttonClick, id));
        }
    }
}
