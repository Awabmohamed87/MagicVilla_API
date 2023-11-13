using static MagicVilla_Utility.SD;

namespace MagicVilla_web.Models
{
    public class APIRequest
    {
        public ApiType ApiType { get; set; } = ApiType.Get;
        public string Url { get; set; }
        public object Data { get; set; }
    }
}
