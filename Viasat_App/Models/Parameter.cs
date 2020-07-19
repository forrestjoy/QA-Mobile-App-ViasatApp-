/*
 * This class defines a the parameter object
 * These objects are used to create a list of parameters in order for the users to be able to do custom searches by many parameters.
 * Each parameter contains a key (i.e Part_type) and a value for that key (i.e WASHER)
 */

namespace Viasat_App
{
    public class Parameter
    {
        public string key { get; set; }
        public string value { get; set; } 
    }
}