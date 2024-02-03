using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Threads.MVVM.Models
{
    public class Thread
    {
        public Guid Id { get; set; }
        public bool IsInternal { get; set; }
        public string Name { get; set; }
        public Guid TypeId { get; set; }
        public decimal? Pitch { get; set; }
        public string Description { get; set; }

        [ForeignKey(nameof(TypeId))]
        public ThreadType Type { get; set; }
    }
}
