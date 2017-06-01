using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sfb.Core
{
    public class Node<T>
    {
        public T Element { get; set; }
        public List<Node<T>> Items { get; set; }
    }
}