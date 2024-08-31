using System.ComponentModel.DataAnnotations.Schema;

namespace RestWithASPNET.Model 
{
    [Table("person")]
    public class Person
    {
        [Column("id")] // como está nomeado no banco
        public long Id { get; set; }
        [Column("first_name")]
        public string FirstName { get; set; }
        [Column("last_name")]
        public string LastName { get; set; }
        [Column("address")]
        public string Address { get; set; }
        [Column("gender")]
        public string Gender { get; set; }
    }
}