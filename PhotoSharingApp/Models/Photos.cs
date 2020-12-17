using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoSharingApp.Models
{
    public class Photos
    {
        /// <summary>
        /// Photo id
        /// </summary>
        [Key]
        public int PhotoId { get; set; }

        /// <summary>
        /// Name of photo/post
        /// </summary>
        [Required]
        public string PhotoName { get; set; }

        /// <summary>
        /// optional photo description
        /// </summary>
        public string PhotoDescription { get; set; }


        /// <summary>
        /// is note stored in db
        /// </summary>
        [NotMapped]
        public IFormFile Photo { get; set; }

        /// <summary>
        /// Url where photo is sotred
        /// </summary>
        public string PhotoUrl { get; set; }
    }
}
