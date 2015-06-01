using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace CrescentIsland.Website.Models
{
    public class ChatMessage
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [StringLength(256)]
        public string UserName { get; set; }
        [Required]
        [StringLength(100)]
        public string Message { get; set; }
        public string Role { get; set; }
        [Required]
        public DateTime Timestamp { get; set; }
        public string ConnectionId { get; set; }
    }

    public class SavedChatMessageModel
    {
        public string Username { get; set; }
        public string Message { get; set; }
        public string Role { get; set; }
        public string Timestamp { get; set; }
    }

    public partial class ChatDbContext : DbContext
    {
        public ChatDbContext()
            : base("LocalConnection")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

        }

        public virtual DbSet<ChatMessage> ChatMessages { get; set; }
    }
}