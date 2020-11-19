using System;
using System.Collections.Generic;
using System.Linq;
using sibintek.sibmobile.core;

namespace sibintek.http
{
    public class DefaultCommandHandlerFactory : ICommandHandlerFactory
    {
        private readonly List<CommandHandlerMap> commandHandlerMaps;

        public DefaultCommandHandlerFactory()
        {    
            var type = typeof(ICommandHandler<>);

            commandHandlerMaps = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(x => Attribute.GetCustomAttribute(x, typeof (CommandHandlerAttribute)) != null)
                .Select(p => new CommandHandlerMap { 
                    CommandHandlerType = p,
                    CommandType = p.GetGenericArguments().FirstOrDefault(),
                    CommandName = ((CommandHandlerAttribute)Attribute.GetCustomAttribute(p, typeof (CommandHandlerAttribute)))?.CommandTypeName
                })
                .ToList();
                
        }

        public IEnumerable<Type> GetHandlers(string name)
        {
            return commandHandlerMaps.Where(r => r.CommandName == name)
                .Select(r => r.CommandHandlerType)
                .ToList();
        }
        
        public class CommandHandlerMap
        {
            public string CommandName { get; set; }
            public Type CommandHandlerType { get; set;}
            public Type CommandType { get; set; }
        }
    }
}
