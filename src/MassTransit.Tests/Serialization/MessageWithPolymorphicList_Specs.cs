// Copyright 2007-2015 Chris Patterson, Dru Sellers, Travis Smith, et. al.
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace MassTransit.Tests.Serialization
{
    namespace Polymorphic
    {
        using System.Collections.Generic;
        using System.Threading.Tasks;
        using Newtonsoft.Json;
        using NUnit.Framework;
        using TestFramework;

        public interface ITestMessageWithPolymorphicList
        {
            [JsonProperty(ItemTypeNameHandling = TypeNameHandling.All)]
            IList<ITestMessageWithPolymorphicListItem> Items { get; set; }
        }

        public class TestMessageWithPolymorphicList : ITestMessageWithPolymorphicList
        {
            public IList<ITestMessageWithPolymorphicListItem> Items { get; set; }
        }

        public interface ITestMessageWithPolymorphicListItem
        {

        }

        public class TestMessageWithPolymorphicListItem : ITestMessageWithPolymorphicListItem
        {
            public int SomeData { get; set; }
        }

        [TestFixture]
        public class MessageWithPolymorphicList_Specs :
            InMemoryTestFixture
        {
            [Test]
            public async Task Message_with_polymorphic_list_should_be_properly_deserialized()
            {
                TestMessageWithPolymorphicList message = new TestMessageWithPolymorphicList
                {
                    Items = new List<ITestMessageWithPolymorphicListItem>()
                    {
                         new TestMessageWithPolymorphicListItem()
                         {
                             SomeData = 2
                         }
                     }

                };

                await InputQueueSendEndpoint.Send(message);

                ConsumeContext<ITestMessageWithPolymorphicList> context = await _handled;

                Assert.IsInstanceOf<TestMessageWithPolymorphicListItem>(context.Message.Items[0]);
            }

            Task<ConsumeContext<TestMessageWithPolymorphicList>> _handled;

            protected override void ConfigureInMemoryReceiveEndpoint(IInMemoryReceiveEndpointConfigurator configurator)
            {
                _handled = Handled<TestMessageWithPolymorphicList>(configurator);
            }
        }
    }
}