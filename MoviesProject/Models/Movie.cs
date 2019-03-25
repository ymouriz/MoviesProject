using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace MoviesProject
{

    class Movie
    {
        [JsonProperty(PropertyName = "code")]
        public string Code { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "year")]
        public int Year { get; set; }

        [JsonProperty(PropertyName = "cast")]
        public List<string> Cast { get; set; }

        [JsonProperty(PropertyName = "genres")]
        public List<string> Genres { get; set; }

        public override string ToString()
        {
            string movie = "CODE: \t" + Code;
            movie += "\nTITLE: \t" + Title;
            movie += "\nYEAR: \t" + Year;
            movie += "\nGENRE: ";
            if (Genres != null)
            {
                for (int i = 0; i < Genres.Count(); i++)
                {                    
                    movie += "\t" + Genres[i];
                    if (i < Genres.Count() - 1)
                    {
                        movie += "\n";
                    }
                }
            }
            movie += "\nCAST: ";
            if (Cast != null)
            {
                for (int i = 0; i < Cast.Count(); i++)
                {                    
                    movie += "\t" + Cast[i] + "\n";
                }
            }

            return movie;
        }
    }
}
