﻿namespace CureFusion.Entities;

public class Hospital
{

    public int ID { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; }=string.Empty;
    public string Zone { get; set; } = string.Empty;
    public string PhoneNumber { get; set; }     
    public double? Rating { get; set; }        
    public string Website { get; set; } = string.Empty;       
    public double? Distance { get; set; }      
    public Location Location { get; set; }     

}
