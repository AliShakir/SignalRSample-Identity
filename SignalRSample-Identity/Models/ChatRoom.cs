﻿using System.ComponentModel.DataAnnotations;

namespace SignalRSample_Identity.Models
{
    public class ChatRoom
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
