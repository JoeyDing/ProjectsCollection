using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Automation.UI.Shell.Wpf.Infrastructure.TestRunner;

namespace Automation.UI.Shell.Core
{
    public interface ITestCaseImageStore
    {
        void AddItem(TestCaseImageStoreItem item);

        List<TestCaseImageStoreItem> GetAllItems();
    }
    public class TestCaseImageStoreService: ITestCaseImageStore
    {
        private readonly List<TestCaseImageStoreItem> store;
        public TestCaseImageStoreService()
        {
            this.store = new List<TestCaseImageStoreItem>();
        }
        public void AddItem(TestCaseImageStoreItem item)
        {
            this.store.Add(item);
        }
        public List<TestCaseImageStoreItem> GetAllItems()
        {
            return this.store;
        }
    }

    public class TestCaseImageStoreItem
    {
        public string Path { get; set; }
        public TestCase TestCase { get; set; }
        public byte[] Image { get; set; }
    }
}
