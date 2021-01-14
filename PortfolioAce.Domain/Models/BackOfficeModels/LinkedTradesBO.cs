using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PortfolioAce.Domain.Models.BackOfficeModels
{
    [Table("bo_LinkedTrades")]
    public class LinkedTradesBO
    {
        [Key]
        public int LinkedTradeId { get; set; }

        public virtual ICollection<TransactionsBO> Transactions { get; set; }

    }
}
