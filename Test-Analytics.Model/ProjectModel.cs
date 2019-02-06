namespace Test_Analytics.Model {
    public class ProjectModel {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Owners { get; set; }
        public string Editors { get; set; }
        public string Viewers { get; set; }
        public bool IsPublic { get; set; } = false;
    }
}
