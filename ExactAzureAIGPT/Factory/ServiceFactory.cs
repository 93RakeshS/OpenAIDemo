﻿using Exact.Azure.AI.GPT.Services.Class;
using Exact.Azure.AI.GPT.Services.Interface;

namespace Exact.Azure.AI.GPT.Factory
{
    public class ServiceFactory
    {
        private readonly IServiceProvider serviceProvider;

        public ServiceFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IAIService GetService(string userSelection)
        {    
            if(userSelection == "text-davinci-003")
                return (IAIService)serviceProvider.GetService(typeof(DavinciService));
            else
                return (IAIService)serviceProvider.GetService(typeof(Gpt35Service));
        }
    }
}
