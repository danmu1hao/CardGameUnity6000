using System;

namespace UIFrame
{
    public class UIType
    {
        private string _path;
        public String Path
        {
            get => _path;
        }
        private string _name;
        public String Name
        {
            get => _name;
        }
        public UIType(string path,string name)
        {
            _path = path;
            _name = name;
        }
    
    }
}
