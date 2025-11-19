using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class GroupedLeopardProfileModel
    {
        public string LeopardTypeName { get; set; }
        public List<ListLeopardProfileModel> Leopards { get; set; } = new List<ListLeopardProfileModel>();
    }
}
