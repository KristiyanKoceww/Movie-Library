namespace MovieLibrary.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using MovieLibrary.Data.Common.Models;

    public class MovieCategory : BaseDeletableModel<int>
    {
        public string Name { get; set; }
    }
}
