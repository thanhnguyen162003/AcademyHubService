﻿using AutoMapper;

namespace AcademyHubService.Api.Impl;

public class CommandsProfile : Profile
{
    public CommandsProfile()
    {
        var srcAssembly = typeof(ServiceModel.Commands.RegisterOrganization).Assembly;
        var svcModelCommands = srcAssembly.GetTypes().Where(x => x.FullName.StartsWith("ServiceModel.Commands"));
        foreach (var c in GetMatchingCommandsByConvention())
            CreateMap(c.Key, c.Value);
    }

    Dictionary<Type, Type> GetMatchingCommandsByConvention()
    {
        var ret = new Dictionary<Type, Type>();
        var srcAssembly = typeof(ServiceModel.Commands.RegisterOrganization).Assembly;
        var dstAssembly = typeof(PL.Commands.RegisterOrganization).Assembly;
        var svcModelCommands = srcAssembly.GetTypes().Where(x => x.FullName.Contains("ServiceModel.Commands"));
        foreach (var t in svcModelCommands)
        {
            var destType = dstAssembly.GetTypes().Where(x => x.Name == t.Name).FirstOrDefault();
            if (destType != null)
            {
                ret.Add(t, destType);
            }
        }

        return ret;
    }
}