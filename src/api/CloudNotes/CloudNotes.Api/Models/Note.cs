﻿
using System.ComponentModel.DataAnnotations;

namespace CloudNotes.Api.Models
{
    public class Note : IEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(128)]
        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public DateTime DateCreated { get; set; }

        public DateTime? DateModified { get; set; }

        public override string ToString() => Title;
    }
}
