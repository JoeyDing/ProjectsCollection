namespace VsoApi.Rest
{
    public static class TaskTypes
    {
        public static TaskType EnablingSpecification;
        public static TaskType Epic;
        public static TaskType Bug;
        public static TaskType Task;
        public static TaskType TestCase;
        public static TaskType TestPlan;
        public static TaskType TestSuite;
        public static TaskType Impediment;

        static TaskTypes()
        {
            EnablingSpecification = new TaskType("Enabling Specification");
            Epic = new TaskType("Epic");
            Bug = new TaskType("Bug");
            Task = new TaskType("Task");
            TestCase = new TaskType("Test Case");
            TestPlan = new TaskType("Test Plan");
            TestSuite = new TaskType("Test Suite");
            Impediment = new TaskType("Impediment");
        }
    }
}