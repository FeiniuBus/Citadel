using System;

namespace Citadel.Infrastructure
{
    public interface ISerializer
    {
        byte[] Serialize(Type t, object content);
    }
}
