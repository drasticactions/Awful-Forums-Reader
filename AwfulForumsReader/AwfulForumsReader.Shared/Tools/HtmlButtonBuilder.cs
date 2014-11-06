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
                           "<li><input id=\"{2}\" type=\"submit\" value=\"{0}\" onclick=\"{1}\";></input></li>",
                           buttonName, buttonClick, id));
        }
    }
}
