using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Pulpa.TrainerSafety.Data.Entities
{
    public class FamilyGroup : BaseEntity, IBaseEntity
    {
        public int FamilyGroupId { get; set; }
        [MaxLength(255)]
        public string Name { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual ICollection<Usuario> Members { get; set; }
    }



}
