using stLib.Mvc.IModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Test_Analytics.Model {
    public class _SharedModel : IModel {
        public ProjectModel Project { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool ValidOperation { get; set; }
        public string Tags { get; set; }
    }
}
