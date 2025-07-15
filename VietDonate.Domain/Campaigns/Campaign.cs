using CleanArchitecture.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietDonate.Domain.Campaigns
{
    public class Campaign : Entity
    {
        public string Code { get; }
        public string Name { get; }

        public Campaign(Guid? id,
                        string name,
                        string code) : base(id ?? Guid.NewGuid())
        {
            Code = code;
            Name = name;
        }
    }
}
