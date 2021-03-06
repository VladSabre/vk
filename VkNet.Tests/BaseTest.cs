﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Moq;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using VkNet.Abstractions.Utils;
using VkNet.Enums.Filters;
using VkNet.Model;
using VkNet.Utils;

namespace VkNet.Tests
{
	/// <summary>
	/// Базовый класс для тестирования категорий методов.
	/// </summary>
	public abstract class
		BaseTest //TODO: V3072 http://www.viva64.com/en/w/V3072 The 'BaseTest' class containing IDisposable members does not itself implement IDisposable. Inspect: Api.
	{
		/// <summary>
		/// Экземпляр класса API.
		/// </summary>
		protected VkApi Api;

		/// <summary>
		/// Ответ от сервера.
		/// </summary>
		protected string Json;

		/// <summary>
		/// Url запроса.
		/// </summary>
		protected string Url;

		/// <summary>
		/// Параметры запроса.
		/// </summary>
		protected VkParameters Parameters = new VkParameters();

		/// <summary>
		/// Пред установки выполнения каждого теста.
		/// </summary>
		[SetUp]
		public void Init()
		{
			var browser = new Mock<IBrowser>();

			browser.Setup(m => m.GetJson(It.Is<string>(s => s == Url), It.IsAny<IEnumerable<KeyValuePair<string, string>>>()))
				.Callback(Callback)
				.Returns(() =>
				{
					if (string.IsNullOrWhiteSpace(Json))
					{
						throw new ArgumentNullException(nameof(Json), @"Json не может быть равен null. Обновите значение поля Json");
					}

					return Json;
				});

			browser.Setup(o => o.Authorize(It.IsAny<IApiAuthParams>()))
				.Returns(VkAuthorization.From("https://vk.com/auth?__q_hash=qwerty&access_token=token&expires_in=1000&user_id=1"));

			browser.Setup(m => m.Validate(It.IsAny<string>(), It.IsAny<string>()))
				.Returns(VkAuthorization.From(
					"https://oauth.vk.com/blank.html#success=1&access_token=token&user_id=1"));

			var restClient = new Mock<IRestClient>();

			restClient.Setup(x =>
					x.PostAsync(It.Is<Uri>(s => s == new Uri(Url)), It.IsAny<IEnumerable<KeyValuePair<string, string>>>()))
				.Callback(Callback)
				.Returns(() =>
				{
					if (string.IsNullOrWhiteSpace(Json))
					{
						throw new ArgumentNullException(nameof(Json), @"Json не может быть равен null. Обновите значение поля Json");
					}

					return Task.FromResult(HttpResponse<string>.Success(HttpStatusCode.OK, Json, Url));
				});

			restClient.Setup(x => x.PostAsync(It.Is<Uri>(s => string.IsNullOrWhiteSpace(Url)),
					It.IsAny<IEnumerable<KeyValuePair<string, string>>>()))
				.Throws<ArgumentException>();

			Api = new VkApi
			{
				Browser = browser.Object,
				RestClient = restClient.Object
			};

			Api.Authorize(new ApiAuthParams
			{
				ApplicationId = 1,
				Login = "login",
				Password = "pass",
				Settings = Settings.All
			});

			Api.RequestsPerSecond = 100000; // Чтобы тесты быстрее выполнялись
		}

		/// <summary>
		/// После исполнения каждого теста.
		/// </summary>
		[TearDown]
		public void Cleanup()
		{
			Json = null;
			Parameters = new VkParameters();
			Url = null;
		}

		protected VkResponse GetResponse()
		{
			var response = JToken.Parse(Json);

			return new VkResponse(response) {RawJson = Json};
		}

		private void Callback()
		{
			if (string.IsNullOrWhiteSpace(Url))
			{
				throw new ArgumentNullException(nameof(Json), @"Url не может быть равен null. Обновите значение поля Url");
			}

			Url = Url.Replace("\'", "%27");
		}
	}
}