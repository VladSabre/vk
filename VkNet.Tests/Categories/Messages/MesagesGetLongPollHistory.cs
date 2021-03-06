﻿using System;
using NUnit.Framework;
using VkNet.Model.RequestParams;

namespace VkNet.Tests.Categories.Messages
{
    [TestFixture]
    public class MesagesGetLongPollHistory: BaseTest
    {
        [Test]
        public void GroupsField()
        {
            Url = "https://api.vk.com/method/messages.getLongPollHistory";
            Json = @"
            {
                'response': {
                    'history': [[9, -325170546, 1], [8, -325170546, 1], [9, -61270720, 1], [8, -61270720, 7], [8, -9052214, 7], [9, -16010007, 1], [9, -102554254, 1], [9, -61270720, 1], [7, -103292418, 206948], [4, 210723, 17, -103292418], [80, 1, 0]],
                    'messages': {
                        'count': 1,
                        'items': [{
                            'id': 210723,
                            'date': 1512499304,
                            'out': 0,
                            'user_id': -103292418,
                            'read_state': 0,
                            'title': '',
                            'body': '123'
                        }]
                    },
                    'profiles': [],
                    'groups': [{
                        'id': 103292418,
                        'name': 'Work',
                        'screen_name': 'club103292418',
                        'is_closed': 1,
                        'type': 'group',
                        'is_admin': 1,
                        'admin_level': 3,
                        'is_member': 1,
                        'photo_50': 'https://vk.com/images/community_50.png',
                        'photo_100': 'https://vk.com/images/community_100.png',
                        'photo_200': 'https://vk.com/images/community_200.png'
                    }],
                    'new_pts': 12
                }
            }";
            var result = Api.Messages.GetLongPollHistory(new MessagesGetLongPollHistoryParams
            {
                Ts = 1874397841,
                PreviewLength = 0,
                EventsLimit = 1000,
                MsgsLimit = 200,
                MaxMsgId = 0,
                Onlines = true
            });
            Assert.IsNotEmpty(result.Groups);
        }

        [Test]
        public void GetLongPollHistory_ThrowArgumentException()
        {
            Assert.That(() => Api.Messages.GetLongPollHistory(new MessagesGetLongPollHistoryParams()), Throws.InstanceOf<ArgumentException>());
        }
    }
}