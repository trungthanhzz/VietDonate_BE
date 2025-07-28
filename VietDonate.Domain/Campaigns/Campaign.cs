using VietDonate.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietDonate.Domain.Campaigns
{
    public class Campaign : Entity
    {
        public string Code { get; } = null!;
        public string Name { get; } = null!;
        public DateTime CreatedDate { get; }

        public Campaign(Guid id,
                        string name,
                        string code,
                        DateTime createdDate) : base(id)
        {
            Code = code;
            Name = name;
            CreatedDate = DateTime.UtcNow;
        }

        private Campaign()
        {
            
        }
    }
}
