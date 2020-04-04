using System;
using System.Collections.Generic;
using System.Text;

namespace RE.Client.Core
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        public bool IsDeleted { get; set; }
    }
}
