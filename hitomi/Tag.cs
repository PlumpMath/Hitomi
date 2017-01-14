using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hitomi
{
    public struct Tag
    {
        public string Value;
        public Sex Sex;

        public Tag(string tag)
        {
            if (tag.EndsWith("♀"))
                Sex = Sex.FEMALE;
            else if (tag.EndsWith("♂"))
                Sex = Sex.MALE;
            else
                throw new FormatException("Bad sex format");

            Value = tag.TrimEnd(' ', '♀', '♂');
        }
    }

    public enum Sex
    {
        NONE,
        FEMALE,
        MALE
    }
}
