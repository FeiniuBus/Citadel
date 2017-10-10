using System;
using System.Collections.Generic;
using System.Text;

namespace Citadel.Infrastructure
{
    public interface IEntity<TPrimaryKey>
    {
        TPrimaryKey Id { get; set; }
    }
}
