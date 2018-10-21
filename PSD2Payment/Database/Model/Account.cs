using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PSD2Payment.Database.Model
{
    public class Account
    {
        public Account()
        {
            _Links = new Links();
        }
        [JsonProperty("resourceId")]
        public string ResourceId { get; set; }

        [JsonProperty("iban")]
        [Key]
        public string Iban { get; set; }

        [JsonProperty("bban")]
        public string Bban { get; set; }

        [JsonProperty("msisdn")]
        public string Msisdn { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("product")]
        public string Product { get; set; }

        [JsonProperty("cashAccountType")]
        public string CashAccountType { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("bic")]
        public string Bic { get; set; }

        [JsonProperty("linkedAccounts")]
        public string LinkedAccounts { get; set; }

        [JsonProperty("usage")]
        public string Usage { get; set; }

        [JsonProperty("details")]
        public string Details { get; set; }


        [JsonIgnore]
        [Column("Links")]
        public string Links
        {
            get
            {
                return _Links == null ? null : JsonConvert.SerializeObject(_Links);
            }
            set
            {
                _Links = value == null ? null : JsonConvert.DeserializeObject<Links>(value);
            }
        }

        [JsonIgnore]
        [Column("Balances")]
        public string _Balances
        {
            get
            {
                return Balances == null ? null : JsonConvert.SerializeObject(Balances);
            }
            set
            {
                Balances = value == null ? null : JsonConvert.DeserializeObject<List<Balance>>(value);
            }
        }

        [JsonProperty("balances")]
        [NotMapped]
        public List<Balance> Balances { get; set; }

        [JsonProperty("_links")]
        [NotMapped]
        public Links _Links { get; set; }
    }
    public partial class Balance
    {
        [JsonProperty("balanceAmount")]
        public BalanceAmount BalanceAmount { get; set; }

        [JsonProperty("balanceType")]
        public string BalanceType { get; set; }

        [JsonProperty("lastChangeDateTime")]
        public DateTimeOffset LastChangeDateTime { get; set; }

        [JsonProperty("referenceDate")]
        public string ReferenceDate { get; set; }

        [JsonProperty("lastCommittedTransaction")]
        public string LastCommittedTransaction { get; set; }
    }

    public partial class BalanceAmount
    {
        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }
    }

    public partial class Links
    {
        [JsonProperty("balances")]
        public string Balances { get; set; }

        [JsonProperty("transactions")]
        public string Transactions { get; set; }
    }
}
