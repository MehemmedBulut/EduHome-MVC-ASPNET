using Eduprob.Models;
using System.Collections.Generic;

namespace Eduprob.ViewModels
{
    public class HomeVM
    {
        public List<Slider> Sliders { get; set; }
        public List<Service> Services { get; set; }
        public List<Course> Courses { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Blog> Blogs { get; set; }
    }
}
