using System;
using System.Collections.Generic;
using System.Text;

namespace Test_Analytics.Model {
    public enum FrequencyOfFailure { NA, Rarely, Seldom, Occasionally, Often }
    public enum Impact { NA, Minimal, Some, Considerable, Maximal }
    
    public class CapabilityModel : _SharedModel {
        public AttributeModel Attribute { get; set; }
        public ComponentModel Component { get; set; }
        public FrequencyOfFailure FrequencyOfFailure { get; set; }
        public Impact Impact { get; set; }
    }
}
