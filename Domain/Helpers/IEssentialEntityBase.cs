using System;

namespace Domain.Helpers
{
    public interface IEssentialEntityBase
    {
        DateTime AddTime { get; set; }
        DateTime UpdateTime { get; set; }
    }
}