using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LostCities.CardGame.WebApi.Models
{
    public class ExpeditionCandidate
    {
        public IEnumerable<Card> HandCards { get; set; }
        public IEnumerable<Card> ExpeditionCards { get; set; }
        public int Score { get; set; }
    }
}
