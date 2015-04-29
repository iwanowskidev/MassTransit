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
namespace Automatonymous
{
    using System;


    public class StateMachineRequestConfigurator<T, TRequest, TResponse> :
        RequestConfigurator<T, TRequest, TResponse>,
        RequestSettings
        where T : class
        where TRequest : class
        where TResponse : class
    {
        Uri _schedulingServiceAddress;
        Uri _serviceAddress;
        TimeSpan _timeout;

        public StateMachineRequestConfigurator()
        {
            _timeout = TimeSpan.FromSeconds(30);
        }

        public RequestSettings Settings
        {
            get
            {
                if (_serviceAddress == null)
                    throw new AutomatonymousException("The ServiceAddress was not specified.");

                if (_schedulingServiceAddress == null)
                    throw new AutomatonymousException("The SchedulingServiceAdress was not specified");

                return this;
            }
        }

        public Uri ServiceAddress
        {
            get { return _serviceAddress; }
            set { _serviceAddress = value; }
        }

        public TimeSpan Timeout
        {
            get { return _timeout; }
            set { _timeout = value; }
        }

        public Uri SchedulingServiceAddress
        {
            get { return _schedulingServiceAddress; }
            set { _schedulingServiceAddress = value; }
        }
    }
}