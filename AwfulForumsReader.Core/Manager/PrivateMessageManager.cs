﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AwfulForumsReader.Core.Entity;
using AwfulForumsReader.Core.Interface;
using AwfulForumsReader.Core.Tools;
using HtmlAgilityPack;

namespace AwfulForumsReader.Core.Manager
{
    public class PrivateMessageManager
    {
        private readonly IWebManager _webManager;

        public PrivateMessageManager(IWebManager webManager)
        {
            _webManager = webManager;
        }

        public PrivateMessageManager()
            : this(new WebManager())
        {
        }

        public async Task<bool> SendPrivateMessage(NewPrivateMessageEntity newPrivateMessageEntity)
        {
            if (newPrivateMessageEntity == null)
                return false;
            MultipartFormDataContent form;
            try
            {
                form = new MultipartFormDataContent
            {
                {new StringContent("dosend"), "action"},
                {new StringContent(newPrivateMessageEntity.Receiver), "touser"},
                {new StringContent(newPrivateMessageEntity.Icon.Id.ToString(CultureInfo.InvariantCulture)), "iconid"},
                {new StringContent(Extensions.HtmlEncode(newPrivateMessageEntity.Title)), "title"},
                {new StringContent(Extensions.HtmlEncode(newPrivateMessageEntity.Body)), "message"},
                {new StringContent("yes"), "parseurl"},
                {new StringContent("yes"), "parseurl"},
                {new StringContent("Send Message"), "submit"}
            };
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to create private message form", ex);
            }
            try
            {
                HttpResponseMessage response = await _webManager.PostFormData(Constants.NewPrivateMessageBase, form);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to send private message.", ex);
            }
        }

        public async Task<List<PrivateMessageEntity>> GetPrivateMessages(int page)
        {
            var privateMessageEntities = new List<PrivateMessageEntity>();
            var url = Constants.PrivateMessages;
            if (page > 0)
            {
                url = Constants.PrivateMessages + string.Format(Constants.PageNumber, page);
            }

            HtmlDocument doc = (await _webManager.GetData(url)).Document;

            HtmlNode forumNode =
                doc.DocumentNode.Descendants("tbody").FirstOrDefault();


            foreach (
                HtmlNode threadNode in
                    forumNode.Descendants("tr"))
            {
                var threadEntity = new PrivateMessageEntity();
                try
                {
                    Parse(threadEntity, threadNode);
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to parse private message list", ex);
                }
                privateMessageEntities.Add(threadEntity);
            }
            return privateMessageEntities;
        }

        public void Parse(PrivateMessageEntity pmEntity, HtmlNode rowNode)
        {
            pmEntity.Status =
                rowNode.Descendants("td")
                    .FirstOrDefault(node => node.GetAttributeValue("class", string.Empty).Equals("status"))
                    .Descendants("img")
                    .FirstOrDefault()
                    .GetAttributeValue("src", string.Empty);

            var icon = rowNode.Descendants("td")
                    .FirstOrDefault(node => node.GetAttributeValue("class", string.Empty).Equals("icon"))
                    .Descendants("img")
                    .FirstOrDefault();

            if (icon != null)
            {
                pmEntity.Icon = string.Format("/Assets/ThreadTags/{0}.png", Path.GetFileNameWithoutExtension(icon.GetAttributeValue("src", string.Empty)));
            }

            var titleNode = rowNode.Descendants("td")
                .FirstOrDefault(node => node.GetAttributeValue("class", string.Empty).Equals("title"));

            pmEntity.Title =
               titleNode
                    .InnerText.Replace("\n", string.Empty);

            string titleHref = titleNode.Descendants("a").FirstOrDefault().GetAttributeValue("href", string.Empty).Replace("&amp;", "&");

            pmEntity.MessageUrl = Constants.BaseUrl + titleHref;

            pmEntity.Sender = rowNode.Descendants("td")
                .FirstOrDefault(node => node.GetAttributeValue("class", string.Empty).Equals("sender"))
                .InnerText;
            pmEntity.Date = rowNode.Descendants("td")
                .FirstOrDefault(node => node.GetAttributeValue("class", string.Empty).Equals("date"))
                .InnerText;
        }
    }
}
