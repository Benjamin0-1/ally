using System;

namespace Ally.Infrastructure.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
public class RequireRoleAttribute : Attribute
{
    public int RoleId { get; }

    public RequireRoleAttribute(int roleId)
    {
        RoleId = roleId;
    }
}

