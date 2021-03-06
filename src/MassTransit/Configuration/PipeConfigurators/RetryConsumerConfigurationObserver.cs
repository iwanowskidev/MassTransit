﻿// Copyright 2007-2018 Chris Patterson, Dru Sellers, Travis Smith, et. al.
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
namespace MassTransit.PipeConfigurators
{
    using System;
    using System.Collections.Generic;
    using ConsumeConfigurators;
    using Context;
    using GreenPipes.Configurators;


    public class RetryConsumerConfigurationObserver :
        IConsumerConfigurationObserver
    {
        readonly IReceiveEndpointConfigurator _configurator;
        readonly Action<IRetryConfigurator> _configure;
        readonly HashSet<Type> _consumerTypes;
        readonly HashSet<Tuple<Type, Type>> _messageTypes;

        public RetryConsumerConfigurationObserver(IReceiveEndpointConfigurator configurator, Action<IRetryConfigurator> configure)
        {
            _configurator = configurator;
            _configure = configure;
            _consumerTypes = new HashSet<Type>();
            _messageTypes = new HashSet<Tuple<Type, Type>>();
        }

        void IConsumerConfigurationObserver.ConsumerConfigured<TConsumer>(IConsumerConfigurator<TConsumer> configurator)
        {
            _consumerTypes.Add(typeof(TConsumer));
        }

        void IConsumerConfigurationObserver.ConsumerMessageConfigured<TConsumer, TMessage>(IConsumerMessageConfigurator<TConsumer, TMessage> configurator)
        {
            Tuple<Type, Type> key = Tuple.Create(typeof(TConsumer), typeof(TMessage));
            if (_messageTypes.Contains(key))
                return;

            _messageTypes.Add(key);

            var specification = new ConsumeContextRetryPipeSpecification<ConsumeContext<TMessage>,
                RetryConsumeContext<TMessage>>(x => new RetryConsumeContext<TMessage>(x));

            _configure?.Invoke(specification);

            _configurator.AddPipeSpecification(specification);
        }
    }
}