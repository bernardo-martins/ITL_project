﻿using System;
using System.Collections.Generic;

namespace Anikatze.Application.Models
{
    public class Video
    {
        public int VideoID { get; set; } // Primary Key
        public string VideoGuid { get; set; } = Guid.NewGuid().ToString(); // New Guid field
        public string Title { get; set; } = string.Empty;
        public int LectionID { get; set; }

        public Lection? Lection { get; set; } // Nullable
    }
}
