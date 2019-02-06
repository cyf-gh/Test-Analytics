using System;
using System.Collections.Generic;
using System.Text;

namespace Test_Analytics.Model {
    public class TestAnalyticsViewModel {
        public List<ProjectModel> Projects { get; set; }
        public ProjectModel Project { get; set; }
        public List<AttributeModel> Attributes { get; set; }
        public List<ComponentModel> Components { get; set; }
        public List<CapabilityModel> Capabilities { get; set; }
    }
}
