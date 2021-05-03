using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace xLiAdProjectTemplate.Entities
{
    [Table("DCC_JsonCache")]
    public class JsonCache
    {
        [Identity, Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
