using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using Avalonia.Controls.Primitives;
using Avalonia.Media.Imaging;
using Microsoft.CodeAnalysis.Scripting.Hosting;
using Newtonsoft.Json;

namespace ShowcaseFullApp.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class tvShow
{
    [JsonPropertyName("tvShow")]
    public TvShow TvShow { get; set; }
}

public class TvShow
{
    
    
    public string Title { get; set; }
    public List<int> EpisodeCount { get; set; }
    public int SeasonCount { get; set; }
    
    public decimal Rating { get; set; }
    
    public int id { get; set; }

    public int CurEpisode { get; set; }

    public Bitmap? ImageStream { get; set; }

    public override string ToString()
    {
        return Title;
    }
    
}