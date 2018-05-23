using System;

namespace Domain.Helpers
{
    public class EssentialEntityBase : IEssentialEntityBase
    {
        public DateTime AddTime { get; set; } = DateTime.UtcNow;
        public DateTime UpdateTime { get; set; } = DateTime.UtcNow;
        public bool Active { get; set; } = true;
    }
}