﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using VkNet.Model.Attachments;
using VkNet.Utils;

namespace VkNet.Model.RequestParams
{
	/// <summary>
	/// Параметры метода wall.addComment
	/// </summary>
	[Serializable]
	public class BoardAddTopicParams
	{
        /// <summary>
        /// идентификатор сообщества, в котором находится обсуждение.положительное число, обязательный параметр
        /// </summary>
        [JsonProperty("group_id")]
        public long GroupId { get; set; }

        /// <summary>
        /// название обсуждения. Обязательный параметр. 
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// текст первого сообщения в обсуждении. 
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; set; }

        /// <summary>
        /// 1 — сообщение будет опубликовано от имени группы, 0 — сообщение будет опубликовано от имени пользователя (по умолчанию).
        /// </summary>
        [JsonProperty("from_group")]
        public bool? FromGroup { get; set; }

        /// <summary>
        /// Список объектов, приложенных к комментарию и разделённых символом ",". Поле attachments представляется в формате:
        /// &lt;type&gt;&lt;owner_id&gt;_&lt;media_id&gt;,&lt;type&gt;&lt;owner_id&gt;_&lt;media_id&gt;
        /// &lt;type&gt; — тип медиа-вложения:
        /// photo — фотография 
        /// video — видеозапись 
        /// audio — аудиозапись 
        /// doc — документ
        /// &lt;owner_id&gt; — идентификатор владельца медиа-вложения 
        /// &lt;media_id&gt; — идентификатор медиа-вложения. 
        /// 
        /// Например:
        /// photo100172_166443618,photo66748_265827614
        /// Параметр является обязательным, если не задан параметр text. список строк, разделенных через запятую.
        /// </summary>
        [JsonProperty("attachments")]
        public IEnumerable<MediaAttachment> Attachments { get; set; }

        /// <summary>
        /// Идентификатор капчи
        /// </summary>
        [JsonProperty("captcha_sid")]
        public long? CaptchaSid { get; set; }

        /// <summary>
        /// текст, который ввел пользователь
        /// </summary>
        [JsonProperty("captcha_key")]
        public string CaptchaKey { get; set; }

		/// <summary>
		/// Привести к типу VkParameters.
		/// </summary>
		/// <param name="p">Параметры.</param>
		/// <returns></returns>
		public static VkParameters ToVkParameters(BoardAddTopicParams p)
		{
			var parameters = new VkParameters
			{
				{ "group_id", p.GroupId },
				{ "title", p.Title },
                { "text", p.Text },
                { "from_group", p.FromGroup },
				{ "attachments", p.Attachments },
                { "captcha_sid", p.CaptchaSid},
                { "captcha_key", p.CaptchaKey}
			};

			return parameters;
		}
	}
}