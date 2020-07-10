using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prank.Model
{
    public class PrankCallCostModel
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int PrankCallCostId { get; set; }
        public string CountryPrefix { get; set; }
        public int CostPoint { get; set; }
    }
}
